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
using Gios.Pdf;
using System.IO;

public partial class HostelMod_GymAllotment : System.Web.UI.Page
{
    #region initialization
    string user_code;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Boolean Cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string Rollflag1 = string.Empty;
    string Regflag1 = string.Empty;
    string Studflag1 = string.Empty;
    string college_code = "";
    string college = "";
    string course_id = string.Empty;
    static string Hostelcode = "";
    static string hosname = "";
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string query = "";
    int count = 0;
    string sqladd = "";
    static string mm = "";
    static string cln = "";
    string build = "";
    string floor = "";
    string room = "";
    string[] fr;
    string[] address;
    string buildvalue1 = "";
    string build1 = "";
    string buildvalue2 = "";
    string build2 = "";
    string buildvalue3 = "";
    string build3 = "";
    string buildvalue4 = "";
    string build4 = "";
    string buildvalue6 = "";
    string build6 = "";
    string buildvalue7 = "";
    string build7 = "";
    string builldvalue5 = "";
    string builld5 = "";
    string buildvalue8 = "";
    string build8 = "";
    string batch = "";
    string batchval = "";
    string grouporusercode = "";
    string[] datesp;
    int i = 0;
    static string statichostelfk = "";
    string stugymname = string.Empty;
    string stugymcost = string.Empty;
    string stugymjoindate = string.Empty;
    string stugymcode = string.Empty;
    string hostelname = string.Empty;
    string buildname = string.Empty;
    string floorname = string.Empty;
    string roomname = string.Empty;
    string community = string.Empty;
    string gender = string.Empty;
    string gymnamestudent = string.Empty;
    string degree = string.Empty;
    string branch = string.Empty;
    string collcode = string.Empty;
    string department = string.Empty;
    string designation = string.Empty;
    string stafftype = string.Empty;
    string sql = string.Empty;
    string pattern = "";
    string header_id = string.Empty;
    string ledgPK = string.Empty;
    string exincludemessbill = string.Empty;
    DataSet dsgymfees = new DataSet();

    string semval = "";
    string sqlcmd = string.Empty;
    string tcode = string.Empty;
    #endregion


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
        //31.10.15 barath
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        //31.10.15 barath
        string Master = "select * from Master_Settings where " + grouporusercode + "";//31.10.15 barath
        DataSet ds = d2.select_method(Master, hat, "Text");
        if (ds.Tables.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Rollflag"] = "1";
                    Rollflag1 = Session["Rollflag"].ToString();
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Regflag"] = "1";
                    Regflag1 = Session["Regflag"].ToString();
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Studflag"] = "1";
                    Studflag1 = Session["Studflag"].ToString();
                }
            }
        }
        //caladmin.EndDate = DateTime.Now;
        //caldisdate.EndDate = DateTime.Now;

        if (statichostelfk.Trim() == "")
        {
            try
            {
                ds3.Clear();
                string q1 = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
                ds3 = d2.select_method_wo_parameter(" select HostelMasterPK from HM_HostelMaster where MessMasterFK in(" + q1 + ")", "Text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        if (statichostelfk == "")
                        {
                            statichostelfk = Convert.ToString(ds3.Tables[0].Rows[i][0]);
                        }
                        else
                        {
                            statichostelfk = statichostelfk + "','" + Convert.ToString(ds3.Tables[0].Rows[i][0]);
                        }
                    }
                }
            }
            catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        }
        if (!IsPostBack)
        {
            bindcollege();
            bindhostel();
            txt_vacate.Enabled = false;
            txt_batch.Enabled = false;
            txt_branch.Enabled = false;
            txt_degree.Enabled = false;
            loaddegree();
            cb_hostelname.Checked = true;
            cb_hostelname_checkedchange(sender, e);
            cb_hostelname_checkedchange(sender, e);
            bindbatch();
            bindbranch(college);
            bindcommunity();
            Hostelcode = "";
            txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfrmdate.Attributes.Add("ReadOnly", "readonly");
            txttodate.Attributes.Add("ReadOnly", "readonly");
            txt_vacate.Attributes.Add("readonly", "readonly");
            txt_vacate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ViewState["buil"] = null;
            ViewState["fl"] = null;
            ViewState["ro"] = null;
            popwindowaddnew.Visible = false;
            popwindowstudent.Visible = false;
            loadgymname();
            binddepartment();
            binddesignation();
            bindstafftype();
            LoadGymName();
            div2.Visible = false;
            Fpspread1.Visible = false;
            div1.Visible = false;
            fpsturoll.Visible = false;
            Fpstaff.Visible = false;
            lblerr.Visible = false;
            lblerr.Text = "";
            discontinue.Visible = false;
            printdiv1.Visible = false;
            rptprint1.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
            bindDiscontinuereason();
            #region popupStudent
            bindpop2hostel();
            bindpop2collegename();
            bindpop2degree();
            loadbranch();
            bindpop2batchyear();
            #endregion


            #region popupStaff
            loadcollegestaffpopup();
            bindstaffdepartmentpopup();
            #endregion
        }


        lbl_errmsg.Visible = false;
    }

    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #region College
    public void bindcollege()
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
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        {
        }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddepartment();
        bindhostel();
        div2.Visible = false;
        Fpspread1.Visible = false;

    }
    #endregion

    #region Hostel
    protected void bindhostel()
    {
        try
        {
            cbl_hostelname.Items.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            string MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");

            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                // cbl_hostelname.Items.Insert(0, "--Select--");
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_buildingname.Text = "--Select--";
            txt_floorname.Text = "--Select--";
            txt_roomname.Text = "--Select--";

            div2.Visible = false;
            Fpspread1.Visible = false;

            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cb_hostelname.Checked == true)
                        {
                            cbl_hostelname.Items[i].Selected = true;
                            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                            build1 = cbl_hostelname.Items[i].Value.ToString();
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
                    Hostelcode = buildvalue1;
                    clgbuild(buildvalue1);
                }
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_buildname.ClearSelection();
                    cbl_floorname.ClearSelection();
                    cbl_roomname.ClearSelection();
                    cb_buildname.Checked = false;
                    cb_floorname.Checked = false;
                    cb_roomname.Checked = false;
                }
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_hostelname.Text = "--Select--";

        div2.Visible = false;
        Fpspread1.Visible = false;

        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname.Checked = false;
                ///new 22/08/15
                build = cbl_hostelname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
                clgbuild(buildvalue);
                Hostelcode = buildvalue;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }

    }
    #endregion

    #region StaffloadDepartment
    public void binddepartment()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
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
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                cbl_department.Items[i].Selected = true;
                txt_department.Text = "Department(" + (cbl_department.Items.Count) + ")";
                cb_department.Checked = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }
    protected void cb_department_CheckedChanged(object sender, EventArgs e)
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cbl_department_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_department.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                if (cbl_department.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_department.Text = "--Select--";
                    cb_department.Checked = false;
                    build = cbl_department.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_department.Items.Count)
            {
                txt_department.Text = "Department(" + seatcount + ")";
                cb_department.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_department.Text = "--Select--";
            }
            else
            {
                txt_department.Text = "Department(" + seatcount + ")";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    #endregion

    #region StaffloadDesignation
    public void binddesignation()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loaddesignation(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_designation.DataSource = ds;
                cbl_designation.DataTextField = "desig_name";
                cbl_designation.DataValueField = "desig_code";
                cbl_designation.DataBind();
                if (cbl_designation.Items.Count > 0)
                {
                    for (int ro = 0; ro < cbl_designation.Items.Count; ro++)
                    {
                        cbl_designation.Items[ro].Selected = true;
                        cb_designation.Checked = true;
                    }
                    txt_designation.Text = "Designation(" + cbl_designation.Items.Count + ")";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cb_designation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_designation.Checked == true)
            {
                for (int i = 0; i < cbl_designation.Items.Count; i++)
                {
                    cbl_designation.Items[i].Selected = true;
                }
                txt_designation.Text = "Department(" + (cbl_designation.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_designation.Items.Count; i++)
                {
                    cbl_designation.Items[i].Selected = false;
                }
                txt_designation.Text = "--Select--";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cbl_designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_designation.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_designation.Items.Count; i++)
            {
                if (cbl_designation.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_designation.Text = "--Select--";
                    cb_designation.Checked = false;
                    build = cbl_designation.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_designation.Items.Count)
            {
                txt_designation.Text = "Department(" + seatcount + ")";
                cb_designation.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_designation.Text = "--Select--";
            }
            else
            {
                txt_designation.Text = "Department(" + seatcount + ")";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    #endregion

    #region SearchName_For_Student
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        //query = "select  r.Stud_Name from Registration r, HT_HostelRegistration h where r.App_No=h.App_No and r.Stud_Name like '" + prefixText + "%'  order by r.Stud_Name";
        query = " select r.Stud_Name from Registration r, HT_HostelRegistration h where r.App_No=h.App_No and  r.Stud_Name like '" + prefixText + "%' and HostelMasterFK in('" + statichostelfk + "') order by Stud_Name";
        //AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0'
        name = ws.Getname(query);
        return name;
    }
    #endregion

    #region SearchRollno_For_Student
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        query = " select distinct top 10 r.Roll_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_No like '" + prefixText + "%' and HostelMasterFK in('" + statichostelfk + "') order by Roll_No ";
        //AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0'
        roll = ws.Getname(query);
        return roll;
    }
    #endregion

    #region Studentd_batch_degree_branch
    #region check
    public void cb1_CheckedChanged(object sender, EventArgs e)
    {
        if (cb1.Checked == true)
        {
            txt_batch.Enabled = true;
            txt_branch.Enabled = true;
            txt_degree.Enabled = true;
        }
        else if (cb1.Checked == false)
        {
            txt_batch.Enabled = false;
            txt_branch.Enabled = false;
            txt_degree.Enabled = false;
        }
    }
    #endregion

    #region Batch
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
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
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
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

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
            }
            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region Degree
    public void loaddegree()
    {
        try
        {
            user_code = Session["usercode"].ToString();
            //college_code = Session["collegecode"].ToString();
            college_code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }

            string q1 = "";
            if (!string.IsNullOrEmpty(group_user) && group_user != "0")
            {
                q1 = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in('" + college_code + "') and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
            }
            else
            {
                q1 = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in('" + college_code + "') and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "' ";
            }
            ds = d2.select_method_wo_parameter(q1, "text");
            int count1 = ds.Tables[0].Rows.Count;
            //ddl_pop2degre.Items.Clear();
            cbl_degree.Items.Clear();
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region Branch
    public void bindbranch(string branch)
    {
        try
        {
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

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
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;
            lblerr.Visible = false;
            lblerr.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

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
            div2.Visible = false;
            Fpspread1.Visible = false;
            lblerr.Visible = false;
            lblerr.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion
    #endregion

    #region Building Name
    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildname.DataSource = ds;
                cbl_buildname.DataTextField = "Building_Name";
                cbl_buildname.DataValueField = "code";
                cbl_buildname.DataBind();
            }
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                cbl_buildname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                cb_buildname.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    string builname = cbl_buildname.Items[i].Text;
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cb_buildname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    if (cb_buildname.Checked == true)
                    {
                        cbl_buildname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildname.Items[i].Text.ToString();
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
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    cbl_buildname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cbl_buildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildname.Items[i].Text.ToString();
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
            if (seatcount == cbl_buildname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region Floor
    public void clgfloor(string buildname)
    {
        try
        {
            cbl_floorname.Items.Clear();
            //ds = d2.BindFloor_new(buildname);
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cb_floorname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_buildname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildname.Items.Count; i++)
                    {
                        build1 = cbl_buildname.Items[i].Text.ToString();
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    build1 = cbl_buildname.Items[i].Text.ToString();
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region Room
    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            string itemname = "select Room_Name,Roompk from Room_Detail where Building_Name in('" + buildname + "') and floor_name in('" + floorname + "') order by (len(Room_Name)) asc,Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
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
            string room = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string flrname = cbl_roomname.Items[i].Text;
                    if (room == "")
                    {
                        room = flrname;
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + flrname;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cb_roomname_checkedchange(object sender, EventArgs e)
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion


    #region Stafftype
    public void bindstafftype()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            if (ddl_collegename.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            ds = d2.loadstafftype(clgcode);
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
                        cb_stafftype.Checked = true;
                    }
                    txt_stafftype.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    protected void cb_stafftype_CheckedChanged(object sender, EventArgs e)
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cbl_stafftype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_stafftype.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_stafftype.Text = "--Select--";
                    cb_stafftype.Checked = false;
                    build = cbl_stafftype.Items[i].Text.ToString();
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
            //clgfloorpop(buildvalue);
            if (seatcount == cbl_stafftype.Items.Count)
            {
                txt_stafftype.Text = "Staff Type(" + seatcount + ")";
                cb_stafftype.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_stafftype.Text = "--Select--";
            }
            else
            {
                txt_stafftype.Text = "Staff Type(" + seatcount + ")";
            }

            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    #endregion


    #region Gender
    public void cb_sex_checkedchange(object sender, EventArgs e)
    {
        try
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
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cbl_sex_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
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
            }
            div2.Visible = false;
            Fpspread1.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region Community
    public void bindcommunity()
    {
        try
        {
            string college = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string commname = "select distinct textcode,textval from applyn a,textvaltable t  where textval <> '' and a.community = t.TextCode and t.college_code in('" + college + "')";
            {
                ds = d2.select_method_wo_parameter(commname, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_community.DataSource = ds;
                    cbl_community.DataTextField = "textval";
                    cbl_community.DataValueField = "textcode";
                    cbl_community.DataBind();

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cb_community_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_community.Checked == true)
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = true;
                }
                txt_community.Text = "Community(" + (cbl_community.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = false;
                }
                txt_community.Text = "--Select--";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;
            printdiv1.Visible = false;
            rptprint1.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }
    public void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_community.Text = "--Select--";
            cb_community.Checked = false;
            for (int i = 0; i < cbl_community.Items.Count; i++)
            {
                if (cbl_community.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_community.Text = "Community(" + commcount.ToString() + ")";
                if (commcount == cbl_community.Items.Count)
                {
                    cb_community.Checked = true;
                }
            }
            div2.Visible = false;
            Fpspread1.Visible = false;
            printdiv1.Visible = false;
            rptprint1.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region SearchStaffnameGo
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffNamego(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select staff_name from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0 and s.appl_no = a.appl_no and a.appl_id in (select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0) and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    #endregion

    #region SearchStaffCodego
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    #endregion

    #region GymName

    public void LoadGymName()
    {
        try
        {

            cbl_gymname.Items.Clear();
            cb_gymname.Checked = false;
            txt_gymname.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            string Query = "select distinct GymName,GymPK from HM_GymMaster";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_gymname.DataSource = ds;
                cbl_gymname.DataTextField = "GymName";
                cbl_gymname.DataValueField = "GymPK";
                cbl_gymname.DataBind();
                if (cbl_gymname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_gymname.Items.Count; i++)
                    {
                        cbl_gymname.Items[i].Selected = true;
                    }
                    txt_gymname.Text = "GymName(" + cbl_gymname.Items.Count + ")";
                    cb_gymname.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cb_gymname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_gymname.Checked == true)
            {
                for (int i = 0; i < cbl_gymname.Items.Count; i++)
                {
                    cbl_gymname.Items[i].Selected = true;
                }
                txt_gymname.Text = "GymName(" + (cbl_gymname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_gymname.Items.Count; i++)
                {
                    cbl_gymname.Items[i].Selected = false;
                }
                txt_gymname.Text = "--Select--";
            }
            div2.Visible = false;
            Fpspread1.Visible = false;
            div4.Visible = false;
            Fpspread2.Visible = false;
            printdiv1.Visible = false;
            rptprint1.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void cbl_gymname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_gymname.Text = "--Select--";
            cb_gymname.Checked = false;
            for (int i = 0; i < cbl_gymname.Items.Count; i++)
            {
                if (cbl_gymname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_gymname.Text = "GymName(" + commcount.ToString() + ")";
                if (commcount == cbl_gymname.Items.Count)
                {
                    cb_gymname.Checked = true;
                }
            }
            div2.Visible = false;
            Fpspread1.Visible = false;
            div4.Visible = false;
            Fpspread2.Visible = false;
            printdiv1.Visible = false;
            rptprint1.Visible = false;
            printdiv.Visible = false;
            rptprint.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion


    #region Chkdate
    protected void cb_vacatedatebetween_onclick(object sender, EventArgs e)
    {
        if (cb_vacatedatebetween.Checked)
        {
            txtfrmdate.Enabled = true;
            txttodate.Enabled = true;
        }
        else
        {
            txtfrmdate.Enabled = false;
            txttodate.Enabled = false;
        }
    }

    protected void cb_vacate_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_vacate.Checked == true)
        {
            txt_vacate.Enabled = true;
            vacateformdate_div.Visible = true;
        }
        else
        {
            cb_vacatedatebetween.Checked = false;
            txt_vacate.Enabled = false;
            vacateformdate_div.Visible = false;
        }
    }
    #endregion

    #region RblStudentStaff
    protected void rblstudentstaff_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rblstudentstaff.SelectedIndex == 0)
            {
                lbl_roll.Visible = true;
                lbl_satff_dept.Visible = false;
                lbl_name.Visible = true;
                lbl_designation.Visible = false;
                txt_roll.Visible = true;
                upp_department.Visible = false;
                txt_name.Visible = true;
                upp_designation.Visible = false;
                studdetails.Visible = true;
                cb_vacate.Visible = true;
                txt_vacate.Visible = true;
                lbl_stafftype.Visible = false;
                upp_stafftype.Visible = false;
                lbl_Gender.Visible = true;
                upp_Gender.Visible = true;
                lbl_staffname.Visible = false;
                txt_staffname.Visible = false;
                lbl_community.Visible = true;
                Upp_community.Visible = true;
                lblstaffcode.Visible = false;
                txtstafcode.Visible = false;
                Label2.Visible = false;
                txtid.Visible = true;
                Label1.Visible = true;
                txtid1.Visible = false;
                if (cb_vacate.Checked == true)
                {
                    txt_vacate.Enabled = true;
                    vacateformdate_div.Visible = true;
                }
                else
                {
                    cb_vacatedatebetween.Checked = false;
                    txt_vacate.Enabled = false;
                    vacateformdate_div.Visible = false;
                }
                StudentColumnOrder.Visible = true;
                StaffColumnOrder.Visible = false;

                div2.Visible = false;
                Fpspread1.Visible = false;
                div4.Visible = false;
                Fpspread2.Visible = false;
                btn_delete.Visible = false;
                btn_DisContinue.Visible = false;
                discontinue.Visible = false;
                printdiv1.Visible = false;
                rptprint1.Visible = false;
                printdiv.Visible = false;
                rptprint.Visible = false;

            }
            else
            {
                lbl_roll.Visible = false;
                lbl_satff_dept.Visible = true;
                lbl_name.Visible = false;
                lbl_designation.Visible = true;
                txt_roll.Visible = false;
                upp_department.Visible = true;
                txt_name.Visible = false;
                upp_designation.Visible = true;
                studdetails.Visible = false;
                cb_vacate.Visible = false;
                txt_vacate.Visible = false;
                lbl_stafftype.Visible = true;
                upp_stafftype.Visible = true;
                vacateformdate_div.Visible = false;
                lbl_Gender.Visible = false;
                upp_Gender.Visible = false;
                lbl_staffname.Visible = true;
                txt_staffname.Visible = true;
                lbl_community.Visible = false;
                Upp_community.Visible = false;
                lblstaffcode.Visible = true;
                txtstafcode.Visible = true;
                StudentColumnOrder.Visible = false;
                StaffColumnOrder.Visible = true;
                div2.Visible = false;
                Label1.Visible = false;
                Label2.Visible = true;
                txtid.Visible = false;
                Label1.Visible = false;
                txtid1.Visible = true;
                Fpspread1.Visible = false;
                btn_delete.Visible = false;
                btn_DisContinue.Visible = false;
                discontinue.Visible = false;
                div4.Visible = false;
                Fpspread2.Visible = false;
                printdiv1.Visible = false;
                rptprint1.Visible = false;
                printdiv.Visible = false;
                rptprint.Visible = false;


            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion


    #region ColumnOrderforStudent

    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
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
                //tborder.Text = tborder.Text + ItemList[i].ToString();
                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
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
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();

                }
                tborder.Text = "";
                tborder.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region ColumnOrderForStaff
    protected void CheckBox_column1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column1.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder1.Items[i].Selected = true;
                    lnk_columnorder1.Visible = true;
                    ItemList.Add(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder1.Visible = true;
                tborder1.Visible = true;
                tborder1.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder1.Text = tborder1.Text + ItemList[i].ToString();

                    tborder1.Text = tborder1.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = false;
                    lnk_columnorder1.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder1.Items[0].Enabled = false;
                }

                tborder1.Text = "";
                tborder1.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    protected void LinkButtonsremove1_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            CheckBox_column1.Checked = false;
            lnk_columnorder1.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder1.Text = "";
            tborder1.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    protected void cblcolumnorder1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column1.Checked = false;
            string value = "";
            int index;
            cblcolumnorder1.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder1.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {

                    ItemList.Add(cblcolumnorder1.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder1.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
            {

                if (cblcolumnorder1.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder1.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }

            lnk_columnorder1.Visible = true;
            tborder1.Visible = true;
            tborder1.Text = "";
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
            tborder1.Text = colname12;
            if (ItemList.Count == 11)
            {
                CheckBox_column1.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder1.Visible = false;
                lnk_columnorder1.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    #endregion

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            #region StudentDetails

            if (rblstudentstaff.SelectedIndex == 0)
            {

                DataView dv1 = new DataView();
                //Printcontrol.Visible = false;
                string StuAppNo = string.Empty;
                string studisqry = string.Empty;
                DataSet dsdiscont = new DataSet();
                if (cbl_hostelname.Items.Count > 0)
                    hostelname = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                if (cbl_buildname.Items.Count > 0)
                    buildname = rs.GetSelectedItemsValueAsString(cbl_buildname);
                if (cbl_floorname.Items.Count > 0)
                    floorname = rs.GetSelectedItemsValueAsString(cbl_floorname);
                if (cbl_roomname.Items.Count > 0)
                    roomname = rs.GetSelectedItemsValueAsString(cbl_roomname);
                if (cbl_community.Items.Count > 0)
                    community = rs.GetSelectedItemsValueAsString(cbl_community);
                if (cbl_sex.Items.Count > 0)
                    gender = rs.GetSelectedItemsValueAsString(cbl_sex);
                if (cbl_gymname.Items.Count > 0)
                    gymnamestudent = Convert.ToString(rs.GetSelectedItemsValueAsString(cbl_gymname));
                if (ddl_collegename.Items.Count > 0)
                    collcode = Convert.ToString(ddl_collegename.SelectedValue);


                if (cb1.Checked == true)
                {
                    batch = rs.GetSelectedItemsValueAsString(cbl_batch);
                    degree = rs.GetSelectedItemsValueAsString(cbl_degree);
                    branch = rs.GetSelectedItemsValueAsString(cbl_branch);

                }
                if (ItemList.Count == 0)
                {
                    ItemList.Add("Roll_No");
                    ItemList.Add("id");
                    ItemList.Add("Roll_Admit");
                    ItemList.Add("Stud_Name");
                    ItemList.Add("Degree");
                }
                Hashtable columnhash = new Hashtable();
                columnhash.Clear();
                columnhash.Add("Roll_No", "Roll No");
                columnhash.Add("Reg_No", "Reg No");
                columnhash.Add("id", "Student Id");
                columnhash.Add("Roll_Admit", "Admission No");
                columnhash.Add("Stud_Name", "Name");
                columnhash.Add("Degree", "Course");
                //columnhash.Add("DOB", "DOB");
                //columnhash.Add("Parent_AddressP", "Address");
                //columnhash.Add("PAddress", "Street");
                //columnhash.Add("CityP", "City");
                //columnhash.Add("Ppincode", "Pincode");
                //columnhash.Add("districtp", "District");
                //columnhash.Add("StateP", "State");
                columnhash.Add("community", "Community");
                columnhash.Add("region", "Religion");
                columnhash.Add("Admin_Date", "Admit Date");
                columnhash.Add("BuildingFK", "Building");
                columnhash.Add("FloorFK", "Floor");
                columnhash.Add("RoomFK", "Room");
                columnhash.Add("Room_type", "Room Type");
                //columnhash.Add("DiscontinueDate", "Discontinue");
                columnhash.Add("HostelName", "Hostel Name");
                //columnhash.Add("Student_Mobile", "Student Mobile");
                columnhash.Add("StudMessType", "Student Mess Type");
                columnhash.Add("GymName", "Gym Name");
                string messtype = "";
                if (ddl_basemesstype.SelectedItem.Value == "1" || ddl_basemesstype.SelectedItem.Value == "0")
                {
                    messtype = " and StudMessType in('" + ddl_basemesstype.SelectedItem.Value + "')";
                }
                if (ddl_basemesstype.SelectedItem.Value == "2")
                {
                    messtype = "";
                }
                string rollno = "";
                string stuname = "";
                if (txt_roll.Text.Trim() != "")
                {
                    rollno = " and R.Roll_No ='" + txt_roll.Text + "'";
                }
                if (txt_name.Text.Trim() != "")
                {
                    stuname = "  and R.Stud_Name ='" + txt_name.Text + "'";
                }

                if (!string.IsNullOrEmpty(hostelname) && !string.IsNullOrEmpty(collcode))
                {
                    sql = "select hr.APP_No,hr.id,R.Roll_No,R.Reg_No,r.Roll_Admit,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType,gm.GymName from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a,HM_GymMaster gm  where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and gm.GymPK=hr.GymCode  and d.college_code in ('" + collcode + "')  and h.HostelMasterPK in ('" + hostelname + "') " + messtype + rollno + stuname + "";
                    if (txt_buildingname.Text.Trim() != "--Select--")
                    {
                        sql = sql + " and hr.BuildingFK in ('" + buildname + "')";
                    }
                    if (txt_floorname.Text.Trim() != "--Select--")
                    {
                        sql = sql + "  and hr.FloorFK in ('" + floorname + "') ";
                    }
                    if (txt_roomname.Text.Trim() != "--Select--")
                    {
                        sql = sql + "  and hr.RoomFK in ('" + roomname + "')";
                    }
                    if (branch.Trim() != "")
                    {
                        sql = sql + " and d.Degree_Code in ('" + branch + "') ";
                    }
                    if (batch.Trim() != "")
                    {
                        sql = sql + " and r.Batch_Year in ('" + batch + "') ";
                    }
                    if (gender.Trim() != "")
                    {
                        sql = sql + " and a.sex in ('" + gender + "') ";
                    }
                    if (community.Trim() != "")
                    {
                        sql = sql + " and a.community in ('" + community + "') ";
                    }
                    if (gymnamestudent.Trim() != "")
                    {
                        sql = sql + " and  hr.GymCode in ('" + gymnamestudent + "') ";
                    }
                    sql = sql + "and  r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";



                    string[] ay = txtfrmdate.Text.Split('/');
                    string[] ay1 = txttodate.Text.Split('/');
                    string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime fromdate = new DateTime();
                    DateTime todate = new DateTime();
                    fromdate = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                    todate = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
                    if (cb_vacatedatebetween.Checked == true)
                        sql += " and VacatedDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + todate.ToString("MM/dd/yyyy") + "'";
                }
                sql = sql + " order by r.Roll_No,hr.BuildingFK,hr.FloorFK,hr.RoomFK ";
                sql = sql + " select Building_Name,Code  from Building_Master";
                sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                sql = sql + " SELECT Room_type,Floor_Name,Room_Name,Roompk,Building_Name FROM Room_Detail";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 2;
                    Fpspread1.Sheets[0].RowCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    // FpSpread1.Sheets[0].Columns[1].Width = 20;
                    //////true for select all//////// 
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = true;
                    FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    check1.AutoPostBack = false;
                    if (cb_vacate.Checked == true)
                    {
                        Fpspread1.Sheets[0].Columns[1].Visible = true;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Width = 923;
                        Fpspread1.Height = 300;
                        //btn_vacate.Visible = true;
                        //txt_vatreason.Visible = true;
                        //lnk_vacated.Visible = true;
                        //lbl_vatreason.Visible = true;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Columns[1].Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        //btn_vacate.Visible = false;
                        //lnk_vacated.Visible = false; txt_vatreason.Visible = false;
                        //lbl_vatreason.Visible = false;
                    }
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            int insdex = ItemList.IndexOf(Convert.ToString(colno));
                            //FpSpread1.Columns[insdex].Locked = true;
                            Fpspread1.Columns[insdex + 2].Width = 150;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash[colno]);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;
                            if (colno == "Stud_Name")
                            {
                                Fpspread1.Columns[insdex + 2].Width = 200;
                            }
                        }
                    }
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    int indRelDate = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        count++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[i]["HostelMasterFK"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Note = ds.Tables[0].Rows[i]["RoomFK"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        ViewState["Floor_Name"] = null;
                        ViewState["Building_Name"] = null;
                        ViewState["Room_Name"] = null;

                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                int insdex = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].CellType = txt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Locked = true;
                                //Fpspread1.Columns[insdex].Width = 150;
                                string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "DiscontinueDate")
                                {
                                    indRelDate = insdex + 2;
                                }
                                if (colno.Trim() == "BuildingFK")
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
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                ViewState["Building_Name"] = buildvalue;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() == "FloorFK")
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
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                ViewState["Floor_Name"] = buildvalue;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() == "RoomFK")
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
                                                ViewState["Room_Name"] = buildvalue;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }

                                if (colno.Trim() == "Room_type")
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        ds.Tables[2].DefaultView.RowFilter = "FloorPK in (" + Convert.ToString(ds.Tables[0].Rows[i]["FloorFK"]) + ")";
                                        DataView dv3 = ds.Tables[2].DefaultView;
                                        if (dv3.Count > 0)
                                        {
                                            string floor_name = Convert.ToString(dv3[0]["Floor_Name"]);
                                            ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i]["BuildingFK"]) + ")";
                                            DataView dv2 = ds.Tables[1].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string bulid = Convert.ToString(dv2[0]["Building_Name"]);
                                                ds.Tables[3].DefaultView.RowFilter = "Roompk =" + Convert.ToString(ds.Tables[0].Rows[i]["ROOMfK"]) + " and Floor_Name='" + floor_name + "' and Building_Name='" + bulid + "'";
                                                //Convert.ToString(ds.Tables[0].Rows[i][j])
                                                dv1 = ds.Tables[3].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    buildvalue = Convert.ToString(dv1[0]["Room_type"]);
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                            }
                                        }
                                    }
                                }
                                StuAppNo = Convert.ToString(ds.Tables[0].Rows[i]["APP_No"]);
                                if (StuAppNo != "")
                                {
                                    studisqry = "select App_No,IsDiscontinued,MemType,GymCode from Gym_Discontinue where App_No='" + StuAppNo + "'";
                                    dsdiscont.Clear();
                                    dsdiscont = d2.select_method_wo_parameter(studisqry, "Text");
                                    if (dsdiscont.Tables.Count > 0 && dsdiscont.Tables[0].Rows.Count > 0)
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].BackColor = ColorTranslator.FromHtml("violet");
                                        //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 0, 5);
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].BackColor = ColorTranslator.FromHtml("White");
                                    }
                                }

                            }
                        }

                    }
                    div2.Visible = true;
                    Fpspread1.Visible = true;
                    div4.Visible = false;
                    Fpspread2.Visible = false;
                    //Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.SaveChanges();
                    Fpspread1.Width = 1000;
                    Fpspread1.Height = 1000;
                    //div_report.Visible = true;
                    lblerr.Visible = false;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    printdiv1.Visible = true;
                    rptprint1.Visible = true;
                    //lbl_stucnt.Visible = true;
                    //lbl_stucnt.Text = "No of Students:" + count.ToString();


                    if (ItemList.Contains("DiscontinueDate"))
                    {
                        try
                        {
                            string reliveddate1 = "";
                            reliveddate1 = Convert.ToString(ds.Tables[0].Columns["DiscontinueDate"]);
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                reliveddate1 = Convert.ToString(ds.Tables[0].Rows[k]["DiscontinueDate"]);
                                string reliveddate = "01/01/1900";
                                if (reliveddate1 == reliveddate)
                                {
                                    Fpspread1.Sheets[0].Cells[k, indRelDate].Text = "";
                                }
                            }
                        }
                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
                    }
                }
                else
                {
                    div1.Visible = false;
                    div4.Visible = false;
                    Fpspread2.Visible = false;
                    Fpspread1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No Records Found";
                    lbl_stucnt.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    printdiv1.Visible = false;
                    rptprint1.Visible = false;
                    //div_report.Visible = false;
                    //btn_vacate.Visible = false;
                    //lnk_vacated.Visible = false;
                    //txt_vatreason.Visible = false;
                    //lbl_vatreason.Visible = false;
                }
            }

            #endregion


            #region Staff

            if (rblstudentstaff.SelectedIndex == 1)
            {

                int index;
                string colno = "";
                int j = 0;
                DataView dv2 = new DataView();
                string StaffAppNo = string.Empty;
                string staffdisqry = string.Empty;
                DataSet dsdiscontstaff = new DataSet();
                if (cbl_hostelname.Items.Count > 0)
                    hostelname = rs.GetSelectedItemsValueAsString(cbl_hostelname);
                if (cbl_buildname.Items.Count > 0)
                    buildname = rs.GetSelectedItemsValueAsString(cbl_buildname);
                if (cbl_floorname.Items.Count > 0)
                    floorname = rs.GetSelectedItemsValueAsString(cbl_floorname);
                if (cbl_roomname.Items.Count > 0)
                    roomname = rs.GetSelectedItemsValueAsString(cbl_roomname);
                if (cbl_department.Items.Count > 0)
                    department = rs.GetSelectedItemsValueAsString(cbl_department);
                if (cbl_designation.Items.Count > 0)
                    designation = rs.GetSelectedItemsValueAsString(cbl_designation);
                if (cbl_stafftype.Items.Count > 0)
                    stafftype = rs.GetSelectedItemsValueAsString(cbl_stafftype);
                if (cbl_gymname.Items.Count > 0)
                    gymnamestudent = Convert.ToString(rs.GetSelectedItemsValueAsString(cbl_gymname));
                if (ddl_collegename.Items.Count > 0)
                    collcode = Convert.ToString(ddl_collegename.SelectedValue);


                Hashtable columnhash = new Hashtable();
                columnhash.Clear();

                int colinc = 0;
                columnhash.Add("staff_code", "Staff Code");
                columnhash.Add("id", "Staff Id");
                columnhash.Add("staff_name", "Name");
                columnhash.Add("desig_name", "Designation");
                columnhash.Add("dept_name", "Department");
                columnhash.Add("staffcategory", "Staff Type");
                columnhash.Add("Admin_Date", "Admit Date");
                columnhash.Add("HostelName", "Hostel Name");
                columnhash.Add("BuildingFK", "Building");
                columnhash.Add("FloorFK", "Floor");
                columnhash.Add("RoomFK", "Room");
                //  columnhash.Add("Room_Type", "Room Type");
                columnhash.Add("DiscontinueDate", "Discontinue");
                columnhash.Add("VacatedDate", "Vacated");
                columnhash.Add("Reason", "Reason");
                columnhash.Add("StudMessType", "StudMessType");
                columnhash.Add("GymName", "Gym Name");


                if (ItemList.Count == 0)
                {
                    ItemList.Add("staff_code");
                    ItemList.Add("id");
                    ItemList.Add("staff_name");
                    ItemList.Add("desig_name");
                    ItemList.Add("dept_name");
                    Fpspread1.Width = 880;

                }

                string staffname = "";
                string staffcode = "";
                if (txtstafcode.Text != "")
                {
                    staffcode = " and sm.staff_code='" + txt_staffcode.Text + "' ";
                }
                if (txt_staffname.Text != "")
                {
                    staffname = " and sm.staff_name='" + txt_staffname.Text + "' ";
                }

                if (!string.IsNullOrEmpty(hostelname) && !string.IsNullOrEmpty(collcode))
                {
                    sql = "select  hsd.APP_No,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.id,hsd.FloorFK,hsd.RoomFK,CONVERT(varchar(10), VacatedDate,103) as VacatedDate , CONVERT(varchar(10), DiscontinueDate,103) as DiscontinueDate,hsd.Reason,case when StudMessType='0' then 'Veg' when StudMessType='1' then 'Non Veg' end StudMessType,gm.GymName from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,stafftrans st,HM_GymMaster gm where st.staff_code=sm.staff_code and st.staff_code =sm.staff_code  and hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =st.dept_code and dm.desig_code =st.desig_code and settled=0 and resign =0 and hsd.MemType=2 and dm.collegeCode=sm.college_code and hsd.GymCode=gm.GymPK and hsd.BuildingFK in('" + buildname + "') and hsd.FloorFK in('" + floorname + "') and RoomFK in('" + roomname + "')  and hsd.HostelMasterFK in('" + hostelname + "') and h.dept_code in('" + department + "') and dm.desig_code in('" + designation + "') and dm.staffcategory in ('" + stafftype + "') and hsd.GymCode in('" + gymnamestudent + "')  " + staffcode + staffname + "";
                }
                sql = sql + " select Building_Name,Code  from Building_Master";
                sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                sql = sql + " select Room_Name,Roompk from Room_Detail";
                sql = sql + " select distinct co.MasterValue,co.MasterCode from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFVAC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No";
                sql = sql + " select distinct co.MasterValue,co.MasterCode from CO_MasterValues co,HT_HostelRegistration hr,staffmaster r,staff_appl_master a where MasterCriteria='HSFDSC' and co.MasterCode=hr.Reason and a.appl_id =hr.APP_No";

                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;
                    // Divspread.Visible = true;
                    Fpspread2.Visible = true;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].ColumnCount = 11;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = ItemList.Count + 1;
                    Fpspread2.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                    Fpspread2.Sheets[0].AutoPostBack = true;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            index = ItemList.IndexOf(Convert.ToString(colno));
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    int i;
                    int indRelDate = 0;
                    int indvacDate = 0;
                    int reason = 0;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread2.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[i, 0].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();

                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread2.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread2.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread2.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread2.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;


                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "DiscontinueDate")
                                {
                                    indRelDate = index + 1;
                                }
                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "VacatedDate")
                                {
                                    indvacDate = index + 1;
                                }
                                //if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "Reason")
                                //{
                                //    reason = index + 1;
                                //}
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
                                            dv2 = ds.Tables[1].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv2.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv2[r]["Building_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv2[r]["Building_Name"]);
                                                    }
                                                }
                                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;
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
                                            dv2 = ds.Tables[2].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv2.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv2[r]["Floor_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv2[r]["Floor_Name"]);
                                                    }
                                                }
                                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

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
                                            dv2 = ds.Tables[3].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv2.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv2[r]["Room_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv2[r]["Room_Name"]);
                                                    }
                                                }
                                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }

                                    }

                                }
                                if (colno.Trim() != "Reason")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "MasterCode in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv2 = ds.Tables[4].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv2.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv2[r]["MasterValue"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv2[r]["MasterValue"]);
                                                    }
                                                }
                                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }

                                    }

                                }
                                if (colno.Trim() != "Reason")
                                {
                                }
                                else
                                {
                                    if (ds.Tables[5].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "MasterCode in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv2 = ds.Tables[5].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv2.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv2[r]["MasterValue"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv2[r]["MasterValue"]);
                                                    }
                                                }
                                                Fpspread2.Sheets[0].Cells[i, index + 1].Text = Convert.ToString(buildvalue);
                                                Fpspread2.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Left;

                                            }
                                        }
                                    }
                                }

                                StaffAppNo = Convert.ToString(ds.Tables[0].Rows[i]["APP_No"]);
                                if (StaffAppNo != "")
                                {
                                    staffdisqry = "select App_No,IsDiscontinued,MemType,GymCode from Gym_Discontinue where App_No='" + StaffAppNo + "'";
                                    dsdiscontstaff.Clear();
                                    dsdiscontstaff = d2.select_method_wo_parameter(staffdisqry, "Text");
                                    if (dsdiscontstaff.Tables.Count > 0 && dsdiscontstaff.Tables[0].Rows.Count > 0)
                                    {
                                        Fpspread2.Sheets[0].Cells[i, index + 1].BackColor = ColorTranslator.FromHtml("violet");

                                    }
                                    else
                                    {
                                        Fpspread2.Sheets[0].Cells[i, index + 1].BackColor = ColorTranslator.FromHtml("White");
                                    }
                                }
                            }
                        }
                    }
                    rptprint.Visible = true;
                    div2.Visible = false;
                    Fpspread2.Visible = false;
                    div4.Visible = true;
                    Fpspread2.Visible = true;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    lblerr.Visible = false;
                    Fpspread2.SaveChanges();
                    //lbl_error.Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    //22.12.15 add

                    if (ItemList.Contains("DiscontinueDate"))
                    {
                        try
                        {
                            string reliveddate1 = "";
                            reliveddate1 = Convert.ToString(ds.Tables[0].Columns["DiscontinueDate"]);

                            for (int k = 0; k < Fpspread2.Rows.Count; k++)
                            {
                                reliveddate1 = Convert.ToString(ds.Tables[0].Rows[k]["DiscontinueDate"]);
                                string reliveddate = "01/01/1900";
                                if (reliveddate1 == reliveddate)
                                {
                                    Fpspread2.Sheets[0].Cells[k, indRelDate].Text = "";
                                    // Fpspread2.Sheets[0].Cells[k, indRelDate].Text = "";
                                }

                            }
                        }
                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
                    }
                    if (ItemList.Contains("VacatedDate"))
                    {
                        try
                        {
                            string vacatedate1 = "";
                            vacatedate1 = Convert.ToString(ds.Tables[0].Columns["VacatedDate"]);

                            for (int k = 0; k < Fpspread2.Rows.Count; k++)
                            {
                                vacatedate1 = Convert.ToString(ds.Tables[0].Rows[k]["VacatedDate"]);
                                string vacateddate = "01/01/1900";
                                if (vacatedate1 == vacateddate)
                                {
                                    Fpspread2.Sheets[0].Cells[k, indvacDate].Text = "";
                                }
                            }
                        }
                        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
                    }
                }
                else
                {
                    rptprint.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No Records Found";
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    Fpspread2.Visible = false;
                    // Divspread.Visible = false;
                }
            }

            #endregion

        }

        catch (Exception ex) {// d2.sendErrorMail(ex, collegecode, "GymAllotment"); 
        }

    }

    #endregion

    #region StudentFpspread

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {

            Cellclick = true;
            popwindowaddnew.Visible = true;
            btn_Save_student.Visible = true;
            btn_Save_student.Text = "Update";
            btn_delete.Visible = true;
            btn_DisContinue.Visible = true;
            discontinue.Visible = true;
            lbl_pop1rollno.Visible = true;
            lbl_pop1staffname.Visible = false;
            txt_pop1rollno.Visible = true;
            btn1sturoll.Visible = true;
            txt_pop1staffname.Visible = false;
            btnstaffname.Visible = false;
            lbl_studentname.Visible = true;
            lbl_staffcode.Visible = false;
            txt_studentname.Visible = true;
            txt_staffcode.Visible = false;
            lbl_degre.Visible = true;
            lbl_dept.Visible = false;
            Label2.Visible = false;
            Label1.Visible = true;
            txtid.Visible = true;
            txtid1.Visible = false;
            txt_degre.Visible = true;
            txt_dept.Visible = false;
            staffdesign.Visible = false;
            btn_Save_Staff.Visible = false;
            rblstustaff.Items[1].Selected = false;
            rblstustaff.Items[1].Enabled = false;
            rblstustaff.Items[0].Selected = true;
            rblstustaff.Items[0].Enabled = true;
            txt_pop1rollno.Text = "";
            txt_studentname.Text = "";
            txt_degre.Text = "";
            txt_cost.Text = "";
            txt_date.Text = "";
            txt_pop1staffname.Text = "";
            txt_staffcode.Text = "";
            txt_dept.Text = "";
            txt_design.Text = "";



        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }


    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {

                Fpspread1.SaveChanges();
                txt_pop1rollno.Enabled = false;
                txt_studentname.Enabled = false;
                txtid.Enabled = false;
                txt_degre.Enabled = false;
                bindDiscontinuereason();
                string activerow = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                if (activerow.Trim() != "" && activerow != "0")
                {

                    string apno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                    string collegecode = d2.GetFunction("select CollegeCode from HT_HostelRegistration  where APP_No='" + apno + "'");
                    bindpop2collegename();
                    ddl_pop2collgname.SelectedIndex = ddl_pop2collgname.Items.IndexOf(ddl_pop2collgname.Items.FindByValue(collegecode));

                    #region Student
                    if (rblstudentstaff.SelectedIndex == 0)
                    {
                        string purpose = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                        txt_pop1rollno.Text = purpose;

                        string text = "select StudMessType,r.app_no,R.Roll_No,r.Roll_Admit,R.Stud_Name,hr.id,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,CONVERT(VARCHAR(11),VacatedDate,103) as VacatedDate,(select mastervalue from CO_MasterValues where convert(varchar,mastercode)=convert(varchar,reason))as Reason,IsVacated,IsDiscontinued,IsSuspend,gm.GymName,hr.GymJoindate,gfa.Cost,gfa.Pattern  from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a,HM_GymMaster gm,Hm_GymFeeAllot gfa   where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0) and hr.GymCode=gm.GymPK and gm.GymPK=gfa.GymCode and hr.APP_No=gfa.App_No and R.Roll_No='" + txt_pop1rollno.Text + "'";
                        text = text + " select Building_Name,Code  from Building_Master";
                        text = text + " select Floor_Name,Floorpk  from Floor_Master";
                        text = text + " select Room_Name,Roompk from Room_Detail";
                        // text = text + " select MasterValue from CO_MasterValues where MasterCriteria='HSVAC'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(text, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddl_pop1hostelname.SelectedIndex = ddl_pop1hostelname.Items.IndexOf(ddl_pop1hostelname.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"])));
                            txt_studentname.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                            txt_degre.Text = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                            ddl_gymname.SelectedIndex = ddl_gymname.Items.IndexOf(ddl_gymname.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["GymName"])));
                            txt_cost.Text = Convert.ToString(ds.Tables[0].Rows[0]["Cost"]);
                            txt_date.Text = Convert.ToString(ds.Tables[0].Rows[0]["GymJoindate"]);
                            ViewState["app_no"] = Convert.ToString(ds.Tables[0].Rows[0]["App_no"]);
                            txtid.Text = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                            pattern = Convert.ToString(ds.Tables[0].Rows[0]["Pattern"]);

                            if (pattern == "Semester")
                            {
                                rblCost_Wise.Items[0].Selected = true;
                            }
                            else if (pattern == "Year")
                            {
                                rblCost_Wise.Items[1].Selected = true;
                            }
                            else if (pattern == "Month")
                            {
                                rblCost_Wise.Items[2].Selected = true;
                            }
                            else
                            {
                                rblCost_Wise.Items[3].Selected = true;
                            }

                        }
                    }
                    #endregion

                }
            }
        }

        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        //{
        //}
        catch
        {
        }
    }
    #endregion

    #region Delete
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = true;
            lbl_sure.Text = "Do you want to Delete this Record?";
        }
        catch
        {
        }

    }

    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            string hosstudstaffrollno = string.Empty;
            string stuapp_no = string.Empty;
            string staffapp_no = string.Empty;
            string qryhostudgymdelete = string.Empty;
            string HeaderFK = string.Empty;
            string LedgerFK = string.Empty;
            string paid = string.Empty;
            double amt = 0;
            string qry = string.Empty;
            DataSet dsgyfees = new DataSet();
            DataSet dsfeegym = new DataSet();
            int query = 0;
            //header and ledger
            string Gymfee = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode + "'";
            dsgyfees.Clear();
            dsgyfees = d2.select_method_wo_parameter(Gymfee, "Text");
            if (dsgyfees.Tables.Count > 0 && dsgyfees.Tables[0].Rows.Count > 0)
            {
                HeaderFK = Convert.ToString(dsgyfees.Tables[0].Rows[0]["header"]);
                LedgerFK = Convert.ToString(dsgyfees.Tables[0].Rows[0]["ledger"]);
                //exincludemessbill = Convert.ToString(dsgyfees.Tables[0].Rows[0]["Text_value"]);
            }
            if (rblstustaff.SelectedIndex == 0)
            {
                if (txt_pop1rollno.Text != "")
                {
                    hosstudstaffrollno = txt_pop1rollno.Text;
                    stuapp_no = d2.GetFunction("select app_no from Registration where Roll_No='" + hosstudstaffrollno + "'");
                    if (stuapp_no.Trim() != "0" && stuapp_no.Trim() != "")
                    {
                        qryhostudgymdelete = "update HT_HostelRegistration set  GymCode='',GymJoindate='' where App_No='" + stuapp_no + "' and MemType='1'";
                        qryhostudgymdelete += "  delete Hm_GymFeeAllot where APP_No='" + stuapp_no + "' and MemType='1'";
                        query = d2.update_method_wo_parameter(qryhostudgymdelete, "Text");
                    }
                    paid = "select paidamount from ft_feeallot where app_no='" + stuapp_no + "'";
                    dsfeegym.Clear();
                    dsfeegym = d2.select_method_wo_parameter(paid, "text");
                    if (dsfeegym.Tables.Count > 0)
                    {
                        if (dsfeegym.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsfeegym.Tables[0].Rows.Count; i++)
                            {
                                amt = Convert.ToDouble(dsfeegym.Tables[0].Rows[i]["paidamount"]);
                                if (amt == 0.00)
                                {
                                    qry = "delete from ft_Feeallot where app_no='" + stuapp_no + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "') and MemType='2'";
                                    query = d2.update_method_wo_parameter(qry, "Text");
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                if (txt_staffcode.Text != "")
                {
                    hosstudstaffrollno = txt_staffcode.Text;
                    staffapp_no = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + hosstudstaffrollno + "' and sam.appl_no = sm.appl_no");

                    if (staffapp_no.Trim() != "0" && staffapp_no.Trim() != "")
                    {
                        qryhostudgymdelete = "update HT_HostelRegistration set  GymCode=NULL,GymJoindate=NULL where App_No='" + staffapp_no + "' and MemType='2'";
                        qryhostudgymdelete += "  delete Hm_GymFeeAllot where APP_No='" + staffapp_no + "' and MemType='2'";
                        query = d2.update_method_wo_parameter(qryhostudgymdelete, "Text");
                    }
                    paid = "select paidamount from ft_feeallot where app_no='" + staffapp_no + "'";
                    dsfeegym.Clear();
                    dsfeegym = d2.select_method_wo_parameter(paid, "text");
                    if (dsfeegym.Tables.Count > 0)
                    {
                        if (dsfeegym.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsfeegym.Tables[0].Rows.Count; j++)
                            {
                                amt = Convert.ToDouble(dsfeegym.Tables[0].Rows[j]["paidamount"]);
                                if (amt == 0.00)
                                {
                                    qry = "delete from ft_Feeallot where app_no='" + staffapp_no + "' and paidamount='0' and headerfk in('" + HeaderFK + "') and ledgerfk in('" + LedgerFK + "') and MemType='2'";
                                    query = d2.update_method_wo_parameter(qry, "Text");
                                }
                            }
                        }
                    }

                }

            }
            if (query != 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Deleted Successfully";
                loadgymname();
                surediv.Visible = false;
                popwindowaddnew.Visible = true;
                txt_pop1rollno.Text = "";
                txt_studentname.Text = "";
                txt_staffcode.Text = "";
                txt_staffname.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";

                txt_degre.Text = "";
                txt_cost.Text = "";
                txt_date.Text = "";

            }

        }
        catch
        {
        }

    }


    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        try
        {

            surediv.Visible = false;
            imgdiv2.Visible = false;
            popwindowaddnew.Visible = true;
        }
        catch
        {
        }

    }
    #endregion

    #region DiscontinueReason
    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "DisContinue Reason";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_discontinue.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_discontinue.SelectedItem.Value.ToString() + "' and MasterCriteria='Gym_Discontinue' and collegecode='" + ddl_collegename.SelectedValue + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }
            bindDiscontinuereason();
        }
        catch { }
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            int insert = 0;
            string group = Convert.ToString(txt_addgroup.Text);
            string collcode = string.Empty;
            collcode = Convert.ToString(ddl_collegename.SelectedValue);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (txt_addgroup.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='Gym_Discontinue' and CollegeCode='" + collcode + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='Gym_Discontinue' and CollegeCode='" + collcode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','Gym_Discontinue','" + collcode + "')";
                insert = d2.update_method_wo_parameter(sql, "Text");


                if (insert != 0)
                {

                    bindDiscontinuereason();
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                    txt_addgroup.Text = "";
                    plusdiv.Visible = false;
                    panel_addgroup.Visible = false;
                }
                txt_addgroup.Text = "";
            }
            else
            {
                plusdiv.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Enter the Reason";
            }
        }
        catch
        {
        }
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }


    protected void bindDiscontinuereason()
    {
        try
        {
            ddl_discontinue.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='Gym_Discontinue' and CollegeCode ='" + ddl_collegename.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_discontinue.DataSource = ds;
                ddl_discontinue.DataTextField = "MasterValue";
                ddl_discontinue.DataValueField = "MasterCode";
                ddl_discontinue.DataBind();
            }
            ddl_discontinue.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch { }
    }
    #endregion


    #region DisContinue
    protected void btn_DisContinue_Click(object sender, EventArgs e)
    {
        try
        {
            string hosstudrollno = string.Empty;
            string studapp_no = string.Empty;
            string qryhostudgymdis = string.Empty;
            string stuPattern = string.Empty;
            string discont = string.Empty;
            string hosdisstaffcode = string.Empty;
            string disstaffapp_no = string.Empty;
            string gymdiscontdate = string.Empty;
            int Disquery = 0;
            string gymdate1 = string.Empty;

            int Discontinued = 0;

            if (ddl_gymname.Items.Count > 0)
            {
                stugymcode = Convert.ToString(ddl_gymname.SelectedValue);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
                return;
            }
            if (ddl_discontinue.SelectedIndex != 0)
            {
                if (ddl_discontinue.Items.Count > 0)
                {
                    discont = Convert.ToString(ddl_discontinue.SelectedItem.Text).Trim();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
                return;
            }

            if (rblCost_Wise.SelectedIndex == 0)
            {
                stuPattern = "Semester";
            }
            else if (rblCost_Wise.SelectedIndex == 1)
            {
                stuPattern = "Year";
            }
            else if (rblCost_Wise.SelectedIndex == 2)
            {
                stuPattern = "Month";
            }
            else
            {
                stuPattern = "Term";
            }
            if (rblstustaff.SelectedIndex == 0)
            {
                if (txt_pop1rollno.Text != "" && txt_cost.Text != "" && txt_date.Text != "")
                {
                    hosstudrollno = txt_pop1rollno.Text;
                    stugymcost = txt_cost.Text;
                    //gymdiscontdate = txt_date.Text;
                    gymdate1 = txt_date.Text;
                    string[] frdate2 = gymdate1.Split('/');
                    if (frdate2.Length == 3)
                        gymdiscontdate = frdate2[0].ToString() + "/" + frdate2[1].ToString() + "/" + frdate2[2].ToString();

                    studapp_no = d2.GetFunction("select app_no from Registration where Roll_No='" + hosstudrollno + "'");
                    if (studapp_no.Trim() != "0" && studapp_no.Trim() != "")
                    {
                        Discontinued = 1;
                        qryhostudgymdis = "if exists (select * from Gym_Discontinue where App_No='" + studapp_no + "' and MemType='1') update Gym_Discontinue set   GymCode='" + stugymcode + "',Pattern='" + stuPattern + "',GymDiscontinueDate='" + gymdiscontdate + "',Cost='" + stugymcost + "',IsDiscontinued='" + Discontinued + "',DisReason='" + discont + "' where App_No='" + studapp_no + "' and MemType='1' and  GymCode='" + stugymcode + "' else insert into Gym_Discontinue(MemType,App_No,GymCode,Pattern,Cost,GymDiscontinueDate,IsDiscontinued,DisReason) values('1','" + studapp_no + "','" + stugymcode + "','" + stuPattern + "','" + stugymcost + "','" + gymdiscontdate + "','" + Discontinued + "','" + discont + "')";
                        Disquery = d2.update_method_wo_parameter(qryhostudgymdis, "Text");
                    }

                }
            }
            else
            {

                if (txt_staffcode.Text != "" && txt_cost.Text != "" && txt_date.Text != "")
                {
                    stugymcost = txt_cost.Text;
                    //gymdiscontdate = txt_date.Text;
                     gymdate1 = txt_date.Text;
                    string[] frdate1 = gymdate1.Split('/');
                    if (frdate1.Length == 3)
                        gymdiscontdate = frdate1[0].ToString() + "/" + frdate1[1].ToString() + "/" + frdate1[2].ToString();

                    hosdisstaffcode = txt_staffcode.Text;
                    disstaffapp_no = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + hosdisstaffcode + "' and sam.appl_no = sm.appl_no");
                    ds.Clear();
                    if (disstaffapp_no.Trim() != "0" && disstaffapp_no.Trim() != "")
                    {
                        Discontinued = 1;
                        qryhostudgymdis = "if exists (select * from Gym_Discontinue where App_No='" + disstaffapp_no + "' and MemType='2') update Gym_Discontinue set   GymCode='" + stugymcode + "',Pattern='" + stuPattern + "',GymDiscontinueDate='" + gymdiscontdate + "',Cost='" + stugymcost + "',IsDiscontinued='" + Discontinued + "',DisReason='" + discont + "' where App_No='" + disstaffapp_no + "' and MemType='2' and  GymCode='" + stugymcode + "' else insert into Gym_Discontinue(MemType,App_No,GymCode,Pattern,Cost,GymDiscontinueDate,IsDiscontinued,DisReason) values('2','" + disstaffapp_no + "','" + stugymcode + "','" + stuPattern + "','" + stugymcost + "','" + gymdiscontdate + "','" + Discontinued + "','" + discont + "')";
                        Disquery = d2.update_method_wo_parameter(qryhostudgymdis, "Text");
                    }

                }
            }
            if (Disquery != 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "DisContinue Successfully";
                loadgymname();
                bindDiscontinuereason();
                txt_pop1rollno.Text = "";
                txt_studentname.Text = "";
                txt_degre.Text = "";
                txt_cost.Text = "";
                txt_date.Text = "";
                txt_pop1staffname.Text = "";
                txt_staffcode.Text = "";
                txt_dept.Text = "";
                txt_design.Text = "";

            }

        }

        catch
        {


        }

    }
    #endregion

    #region StaffFpspread

    public void FpSpread2_CellClick(object sender, EventArgs e)
    {
        try
        {

            Cellclick = true;
            popwindowaddnew.Visible = true;
            btn_Save_Staff.Visible = true;
            btn_Save_Staff.Text = "Update";
            btn_delete.Visible = true;
            btn_DisContinue.Visible = true;
            discontinue.Visible = true;
            lbl_pop1rollno.Visible = false;
            lbl_pop1staffname.Visible = true;
            txt_pop1rollno.Visible = false;
            btn1sturoll.Visible = false;
            txt_pop1staffname.Visible = true;
            btnstaffname.Visible = true;
            lbl_studentname.Visible = false;
            Label2.Visible = true;
            Label1.Visible = false;
            txtid.Visible = false;
            txtid1.Visible = true;
            lbl_staffcode.Visible = true;
            txt_studentname.Visible = false;
            txt_staffcode.Visible = true;
            lbl_degre.Visible = false;
            lbl_dept.Visible = true;
            txt_degre.Visible = false;
            txt_dept.Visible = true;
            staffdesign.Visible = true;
            btn_Save_student.Visible = false;
            txt_cost.Text = "";
            txt_date.Text = "";
            txt_pop1staffname.Text = "";
            txt_staffcode.Text = "";
            txt_dept.Text = "";
            txt_design.Text = "";

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }


    public void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {

                Fpspread2.SaveChanges();
                txt_pop1staffname.Enabled = false;
                txt_staffcode.Enabled = false;
                txt_dept.Enabled = false;
                txtid1.Enabled = false;
                txt_design.Enabled = false;
                bindDiscontinuereason();
                string activerow1 = "";
                activerow1 = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                if (activerow1.Trim() != "")
                {

                    string clgcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow1), 2].Tag);
                    string collegecode = d2.GetFunction("select CollegeCode from HT_HostelRegistration  where APP_No='" + clgcode + "'");
                    bindpop2collegename();
                    ddl_pop2collgname.SelectedIndex = ddl_pop2collgname.Items.IndexOf(ddl_pop2collgname.Items.FindByValue(collegecode));

                    string staffcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow1), 0].Tag);
                    sql = "select IsVacated,IsDiscontinued,convert(varchar,convert(datetime,a.date_of_birth,103),103) as 'date_of_birth',hsd.StudMessType,hsd.APP_No,hsd.id,sm.staff_code,sm.staff_name,dm.desig_name,h.dept_name,dm.staffcategory,convert(varchar,convert(datetime,hsd.HostelAdmDate,103),103) as 'Admin_Date',hd.HostelName,hsd.BuildingFK,hsd.FloorFK,hsd.RoomFK,convert(varchar,convert(datetime,hsd.DiscontinueDate,103),103) as 'DiscontinueDate',convert(varchar,convert(datetime,hsd.VacatedDate,103),103) as 'VacatedDate',hsd.Reason,gm.GymName,hsd.GymJoindate,gfa.Cost,gfa.Pattern from HT_HostelRegistration hsd,staffmaster sm,HM_HostelMaster hd,desig_master dm,hrdept_master h,staff_appl_master a,HM_GymMaster gm,Hm_GymFeeAllot gfa where hsd.APP_No=a.appl_id and hsd.HostelMasterFK=hd.HostelMasterPK and a.appl_no =sm.appl_no and h.dept_code =a.dept_code and dm.desig_code =a.desig_code and settled=0 and resign =0 and hsd.MemType=2 and hsd.GymCode=gm.GymPK and gm.GymPK=gfa.GymCode and hsd.APP_No=gfa.App_No and hsd.APP_No='" + staffcode + "'";
                    sql = sql + " select Building_Name,Code  from Building_Master";
                    sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                    sql = sql + " select Room_Name,Roompk from Room_Detail";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sql, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rblstustaff.Items[1].Selected = true;
                        rblstustaff.Items[0].Selected = false;
                        rblstustaff.Items[0].Enabled = false;
                        txt_pop1staffname.Text = Convert.ToString(ds.Tables[0].Rows[0]["Staff_Name"]);
                        txt_staffcode.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_code"]);
                        txt_dept.Text = Convert.ToString(ds.Tables[0].Rows[0]["dept_name"]);
                        txt_design.Text = Convert.ToString(ds.Tables[0].Rows[0]["staffcategory"]);
                        ddl_gymname.SelectedIndex = ddl_gymname.Items.IndexOf(ddl_gymname.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["GymName"])));
                        txt_cost.Text = Convert.ToString(ds.Tables[0].Rows[0]["Cost"]);
                        txtid1.Text = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                        txt_date.Text = Convert.ToString(ds.Tables[0].Rows[0]["GymJoindate"]);
                        if (pattern == "Semester")
                        {
                            rblCost_Wise.Items[0].Selected = true;
                        }
                        else if (pattern == "Year")
                        {
                            rblCost_Wise.Items[1].Selected = true;
                        }
                        else if (pattern == "Month")
                        {
                            rblCost_Wise.Items[2].Selected = true;
                        }
                        else
                        {
                            rblCost_Wise.Items[3].Selected = true;
                        }


                    }


                }
            }
        }
        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        //{
        //}
        catch
        {
        }
    }
    #endregion

    #region studentPrint
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname1 = txtexcelname1.Text;
            if (reportname1.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname1);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {

            string degreedetails1 = "Gym Allotment";
            string pagename1 = "GymAllotment.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename1, degreedetails1);
            printdiv1.Visible = true;
            Printcontrol1.Visible = true;
            // 
        }
        catch
        {
        }
    }

    #endregion

    #region Staffprint
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, reportname);
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

            string degreedetails = "Gym Allotment";
            string pagename = "GymAllotment.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            printdiv.Visible = true;
            Printcontrol.Visible = true;
            // 
        }
        catch
        {
        }
    }
    #endregion

    #region AddNew
    public void btn_add_Click(object sender, EventArgs e)
    {
        try
        {
            popwindowaddnew.Visible = true;
            popwindowstudent.Visible = false;
            popupwindowstaff.Visible = false;
            txt_pop1rollno.Text = "";
            txt_pop1staffname.Text = "";
            txt_studentname.Text = "";
            txt_staffcode.Text = "";
            txt_degre.Text = "";
            txt_dept.Text = "";
            txt_design.Text = "";
            txt_cost.Text = "";
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            loadgymname();
            rblstustaff.Items[0].Selected = true;
            rblstustaff.Items[1].Selected = false;
            rblstustaff.Items[0].Enabled = true;
            rblstustaff.Items[1].Enabled = true;
            rblCost_Wise.Items[0].Selected = false;
            rblCost_Wise.Items[1].Selected = false;
            rblCost_Wise.Items[2].Selected = true;
            rblCost_Wise.Items[3].Selected = false;
            discontinue.Visible = false;


            lbl_pop1rollno.Visible = true;
            lbl_pop1staffname.Visible = false;
            txt_pop1rollno.Visible = true;
            btn1sturoll.Visible = true;
            txt_pop1staffname.Visible = false;
            btnstaffname.Visible = false;
            lbl_studentname.Visible = true;
            lbl_staffcode.Visible = false;
            txt_studentname.Visible = true;
            txt_staffcode.Visible = false;
            lbl_degre.Visible = true;
            lbl_dept.Visible = false;
            txt_degre.Visible = true;
            txt_dept.Visible = false;
            staffdesign.Visible = false;
            btn_Save_student.Visible = true;
            btn_Save_Staff.Visible = false;
            btn_Save_student.Text = "Save";
            btn_delete.Visible = false;
            btn_DisContinue.Visible = false;
            discontinue.Visible = false;
            Label2.Visible = false;
            txtid.Visible = true;
            Label1.Visible = true;
            txtid1.Visible = false;
            txtid.Text = "";
            if (rblstustaff.SelectedIndex == 1)
            {
                lbl_pop1rollno.Visible = false;
                lbl_pop1staffname.Visible = true;
                txt_pop1rollno.Visible = false;
                btn1sturoll.Visible = false;
                txt_pop1staffname.Visible = true;
                btnstaffname.Visible = true;
                lbl_studentname.Visible = false;
                lbl_staffcode.Visible = true;
                txt_studentname.Visible = false;
                txt_staffcode.Visible = true;
                lbl_degre.Visible = false;
                lbl_dept.Visible = true;
                txt_degre.Visible = false;
                txt_dept.Visible = true;
                staffdesign.Visible = true;
                btn_Save_student.Visible = false;
                btn_Save_Staff.Text = "Save";
                btn_Save_Staff.Visible = true;
                btn_delete.Visible = false;
                btn_DisContinue.Visible = false;
                discontinue.Visible = false;
                Label2.Visible = true;
                txtid.Visible = false;
                Label1.Visible = false;
                txtid1.Visible = true;


            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    #endregion


    #region AddNew_PopupWindow

    #region radiostuorstaff
    protected void rblstustaff_Selected(object sender, EventArgs e)
    {
        if (rblstustaff.SelectedIndex == 0)
        {
            lbl_pop1rollno.Visible = true;
            lbl_pop1staffname.Visible = false;
            txt_pop1rollno.Visible = true;
            btn1sturoll.Visible = true;
            txt_pop1staffname.Visible = false;
            btnstaffname.Visible = false;
            lbl_studentname.Visible = true;
            lbl_staffcode.Visible = false;
            txt_studentname.Visible = true;
            txt_staffcode.Visible = false;
            lbl_degre.Visible = true;
            lbl_dept.Visible = false;
            txt_degre.Visible = true;
            txt_dept.Visible = false;
            staffdesign.Visible = false;
            btn_Save_student.Visible = true;
            btn_Save_student.Text = "Save";
            btn_Save_Staff.Visible = false;
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            LoadGymName();
            Label2.Visible = false;
            txtid.Visible = true;
            Label1.Visible = true;
            txtid1.Visible = false;


        }
        else
        {
            lbl_pop1rollno.Visible = false;
            lbl_pop1staffname.Visible = true;
            txt_pop1rollno.Visible = false;
            btn1sturoll.Visible = false;
            txt_pop1staffname.Visible = true;
            btnstaffname.Visible = true;
            lbl_studentname.Visible = false;
            lbl_staffcode.Visible = true;
            txt_studentname.Visible = false;
            txt_staffcode.Visible = true;
            lbl_degre.Visible = false;
            lbl_dept.Visible = true;
            txt_degre.Visible = false;
            txt_dept.Visible = true;
            staffdesign.Visible = true;
            btn_Save_student.Visible = false;
            btn_Save_Staff.Visible = true;
            btn_Save_Staff.Text = "Save";
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_date.Attributes.Add("readonly", "readonly");
            LoadGymName();
            Label2.Visible = true;
            txtid.Visible = false;
            Label1.Visible = false;
            txtid1.Visible = true;

        }
    }
    #endregion

    #region pop1bindHostal
    protected void bindpop2hostel()
    {
        try
        {
            ds.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            string MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");

            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop1hostelname.DataSource = ds;
                ddl_pop1hostelname.DataTextField = "HostelName";
                ddl_pop1hostelname.DataValueField = "HostelMasterPK";
                ddl_pop1hostelname.DataBind();
            }
            else
            {
                ddl_pop1hostelname.Items.Clear();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }
    #endregion

    #region GymName
    protected void loadgymname()
    {
        try
        {

            ddl_gymname.Items.Clear();
            string qrygym = "select distinct GymName,GymPK from HM_GymMaster ";
            ds = d2.select_method_wo_parameter(qrygym, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {

                ddl_gymname.DataSource = ds;
                ddl_gymname.DataTextField = "GymName";
                ddl_gymname.DataValueField = "GymPK";
                ddl_gymname.DataBind();
                ddl_gymname.Items.Insert(0, "--select--");
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void ddl_gymname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string stustaffgymname = Convert.ToString(ddl_gymname.SelectedItem.Text).ToLower();
            string qrystustaff = string.Empty;
            string sem = string.Empty;
            string year = string.Empty;
            string month = string.Empty;
            string term = string.Empty;
            DataSet dscost = new DataSet();
            if (!string.IsNullOrEmpty(stustaffgymname))
            {
                qrystustaff = "select distinct gcm.Year, gcm.Month, gcm.Semester, gcm.Term from HM_GymMaster gm,HM_GymCostMaster gcm where gm.GymPK=gcm.GymFK and gm.GymName='" + stustaffgymname + "'";
                dscost.Clear();
                dscost = d2.select_method_wo_parameter(qrystustaff, "Text");
                if (dscost.Tables.Count > 0 && dscost.Tables[0].Rows.Count > 0)
                {
                    if (rblCost_Wise.SelectedIndex == 0)
                    {
                        sem = Convert.ToString(dscost.Tables[0].Rows[0]["Semester"]);
                        txt_cost.Text = sem;
                    }
                    if (rblCost_Wise.SelectedIndex == 1)
                    {
                        year = Convert.ToString(dscost.Tables[0].Rows[0]["Year"]);
                        txt_cost.Text = year;
                    }
                    if (rblCost_Wise.SelectedIndex == 2)
                    {
                        month = Convert.ToString(dscost.Tables[0].Rows[0]["Month"]);
                        txt_cost.Text = month;
                    }
                    if (rblCost_Wise.SelectedIndex == 3)
                    {
                        term = Convert.ToString(dscost.Tables[0].Rows[0]["Term"]);
                        txt_cost.Text = term;
                    }
                }
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }
    #endregion

    #region Studentdetails

    #region SearchStuRollNo
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> roll = new List<string>();
        query = "select distinct top 10 Roll_No from Registration r,applyn a, Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag!='debar'and r.degree_code = g.Degree_Code  and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and  g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'') and  r.roll_no like '" + prefixText + "%' order by roll_no";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                roll.Add(dw.Tables[0].Rows[i]["roll_no"].ToString());
            }
        }
        return roll;
    }
    #endregion

    protected void roll_txtchange(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string sex = d2.GetFunction(" select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            string q1 = "select Textval,Student_Mobile from applyn a, Registration b,textvaltable c where a.app_no =b.App_No  and c.TextCode=a.community and b.Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + " select parent_addressP,Streetp,Cityp+'-'+Districtp city,parent_pincodep,b.Stud_Name from applyn a, Registration b where a.app_no =b.App_No and  Roll_No ='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "  select dob,b.college_code from applyn a,Registration b where a.app_no=b.App_No and b.Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "  select textval from applyn a,textvaltable t,Registration b where a.bldgrp = t.TextCode and a.app_no =b.App_No and  Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "    select distinct c.Course_Name+' - '+de.dept_name branch from degree d,department de,course c,Registration r,applyn a where r.App_No=a.app_no and r.degree_code=d.Degree_Code  and c.course_id=d.course_id  and de.dept_code=d.dept_code and c.college_code = d.college_code and de.college_code = d.college_code and r.App_No not in (select APP_No from HT_HostelRegistration where (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) and a.sex in(" + sex + ") and r.Roll_No='" + txt_pop1rollno.Text + "'";
            string appno = " ";
            appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[4].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string add6 = ds.Tables[1].Rows[0]["Stud_Name"].ToString();
                    txt_studentname.Text = add6;
                }
                else
                {
                    txt_pop1rollno.Text = "";
                    txt_studentname.Text = "";
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    degree = ds.Tables[4].Rows[0]["branch"].ToString();
                    txt_degre.Text = degree;
                }
                else
                {
                    txt_degre.Text = "";
                }
            }
            else
            {
                txt_pop1rollno.Text = "";
                txt_studentname.Text = "";
                txt_degre.Text = "";


            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }


    protected void btnsturollno_Click(object sender, EventArgs e)
    {
        try
        {

            popwindowstudent.Visible = true;
            fpsturoll.Visible = false;
            btn_pop2ok.Visible = false;
            btn_pop2exit.Visible = false;
            bindpop2collegename();
            bindpop2degree();
            loadbranch();
            bindpop2batchyear();
            ddl_pop2sex.SelectedIndex = 0;
            int activerow = 0;
            activerow = Convert.ToInt32(fpsturoll.ActiveSheetView.ActiveRow.ToString());
            for (int i = 0; i < fpsturoll.Sheets[0].RowCount; i++)
            {
                if (i == Convert.ToInt32(activerow))
                {
                    fpsturoll.Sheets[0].Rows[i].BackColor = Color.LightBlue;

                }
                else
                {
                    fpsturoll.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                }
            }
            if (rblstustaff.SelectedIndex == 0)
            {
                popwindowstudent.Visible = true;
                btn_pop2ok.Visible = false;
                btn_pop2exit.Visible = false;

                fpsturoll.CommandBar.Visible = false;
                fpsturoll.SheetCorner.ColumnCount = 0;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fpsturoll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fpsturoll.Sheets[0].ColumnCount =5;
                fpsturoll.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fpsturoll.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
                fpsturoll.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
                fpsturoll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fpsturoll.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
                fpsturoll.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
                fpsturoll.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
                fpsturoll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fpsturoll.Sheets[0].ColumnHeader.Columns[2].Label = "Hostel Id";
                fpsturoll.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";
                fpsturoll.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
                fpsturoll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
               
                fpsturoll.Sheets[0].ColumnHeader.Columns[3].Label = "Admission No";
                fpsturoll.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";
                fpsturoll.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
                fpsturoll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fpsturoll.Sheets[0].Columns[3].Visible = false;
                fpsturoll.Sheets[0].ColumnHeader.Columns[4].Label = "Name";
                fpsturoll.Sheets[0].ColumnHeader.Columns[4].Font.Name = "Book Antiqua";
                fpsturoll.Sheets[0].ColumnHeader.Columns[4].Font.Size = FontUnit.Medium;
                fpsturoll.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                fpsturoll.Sheets[0].Columns[0].Width = 50;
                fpsturoll.Sheets[0].Columns[1].Width = 120;
                fpsturoll.Sheets[0].Columns[2].Width = 100;
                fpsturoll.Sheets[0].Columns[3].Width = 240;
                fpsturoll.Sheets[0].Columns[4].Width = 280;
                fpsturoll.Width = 426;
                fpsturoll.Columns[0].Locked = true;
                fpsturoll.Columns[1].Locked = true;
                fpsturoll.Columns[2].Locked = true;
                fpsturoll.Columns[3].Locked = true;
                fpsturoll.Columns[4].Locked = true;
            }
            else
            {
                lblpop2error.Visible = true;
                lblpop2error.Text = "Please Select Hostel";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }
    #endregion


    #region StaffDetails

    #region staffnamesearch
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select staff_name  from staffmaster where resign =0 and settled =0  and staff_code not in (select Roll_No from Hostel_StudentDetails )  and staff_name like  '" + prefixText + "%' ";
        string query = "select staff_name  from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0 and s.appl_no = a.appl_no  and a.appl_id not in (select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )   and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    protected void Staffname_txtchange(object sender, EventArgs e)
    {


    }
    #endregion


    #region Staffcodesearch
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCodepopup(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and a.appl_id in(select app_no from HT_HostelRegistration where MemType=2 and ISNULL(app_no,0)<>0 )  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    #endregion

    protected void btnstaffname_Click(object sender, EventArgs e)
    {
        popupwindowstaff.Visible = true;
        Fpstaff.Visible = false;
        div1.Visible = false;
        btn_staffok.Visible = false;
        btn_staffexit.Visible = false;
        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        bindstaffdepartmentpopup();
        lbl_errorsearch.Visible = false;
    }
    #endregion


    protected void rblCost_Wise_Selected(object sender, EventArgs e)
    {
        try
        {
            ddl_gymname_OnSelectedIndexChanged(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    #endregion


    #region Save_For_Student
    protected void btn_Save_student_Click(object sender, EventArgs e)
    {
        try
        {
            string hosstudroll = string.Empty;
            string app_no = string.Empty;
            string qryhostudgymsave = string.Empty;
            int query = 0;
            int semandyear = 0;
            string mnthamt = string.Empty;
            string mnthcol = "";
            string mnthvalue1 = string.Empty;
            string month = string.Empty;
            string year = string.Empty;
            Hashtable AddMonthyear = new Hashtable();
            string collegecode = Convert.ToString(ddl_collegename.SelectedValue);
            if (!string.IsNullOrEmpty(collegecode))
            {
                if (rblstustaff.SelectedIndex == 0)
                {
                    if (ddl_gymname.Items.Count > 0)
                    {
                        stugymcode = Convert.ToString(ddl_gymname.SelectedValue);
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Selected";
                    }
                    if (txt_pop1rollno.Text != "" && txt_cost.Text != "" && txt_date.Text != "")
                    {

                        if (rblCost_Wise.SelectedIndex == 0)
                        {
                            pattern = "Semester";
                        }
                        else if (rblCost_Wise.SelectedIndex == 1)
                        {
                            pattern = "Year";
                        }
                        else if (rblCost_Wise.SelectedIndex == 2)
                        {
                            pattern = "Month";
                        }
                        else
                        {
                            pattern = "Term";
                        }
                        hosstudroll = txt_pop1rollno.Text;
                        stugymcost = txt_cost.Text;
                        string gymdate = txt_date.Text;
                        string[] frdate = gymdate.Split('/');
                        if (frdate.Length == 3)
                            stugymjoindate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

                        app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + hosstudroll + "'");


                        //header and ledger
                        string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + collegecode + "'";
                        dsgymfees.Clear();
                        dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                        if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                        {
                            header_id = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                            ledgPK = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                            exincludemessbill = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Please set Fees setting";
                            return;
                        }

                        if (exincludemessbill == "1")
                        {
                            //Hm_GymFeeAllot
                            if (app_no.Trim() != "0" && app_no.Trim() != "")
                            {
                                qryhostudgymsave = "if exists (select * from HT_HostelRegistration where App_No='" + app_no + "' and MemType='1') update HT_HostelRegistration set  GymCode='" + stugymcode + "',GymJoindate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' else insert into HT_HostelRegistration(MemType,App_No,GymCode,GymJoindate) values('1','" + app_no + "','" + stugymcode + "','" + stugymjoindate + "')";

                                qryhostudgymsave += "if exists (select * from Hm_GymFeeAllot where App_No='" + app_no + "' and MemType='1') update Hm_GymFeeAllot set   GymCode='" + stugymcode + "',Pattern='" + pattern + "',Cost='" + stugymcost + "',GymJoinDate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' and  GymCode='" + stugymcode + "' else insert into Hm_GymFeeAllot(MemType,App_No,GymCode,Pattern,Cost,GymJoinDate) values('1','" + app_no + "','" + stugymcode + "','" + pattern + "','" + stugymcost + "','" + stugymjoindate + "')";
                                query = d2.update_method_wo_parameter(qryhostudgymsave, "Text");
                            }
                        }
                        else
                        {
                            //FT_FeeAllot
                            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);
                            if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                            {
                                //fee Category
                                string selsctfeecate = d2.GetFunction("select distinct current_semester from registration r where r.Roll_No='" + hosstudroll + "' and r.college_code in('" + collegecode + "')");
                                //semesterwise
                                if (rblCost_Wise.SelectedIndex == 0)
                                {
                                    semandyear = 1;
                                    if (selsctfeecate == "1")
                                        semval = "1 Semester";
                                    if (selsctfeecate == "2")
                                        semval = "2 Semester";
                                    if (selsctfeecate == "3")
                                        semval = "3 Semester";
                                    if (selsctfeecate == "4")
                                        semval = "4 Semester";
                                    if (selsctfeecate == "5")
                                        semval = "5 Semester";
                                    if (selsctfeecate == "6")
                                        semval = "6 Semester";
                                    if (selsctfeecate == "7")
                                        semval = "7 Semester";
                                    if (selsctfeecate == "8")
                                        semval = "8 Semester";
                                    if (selsctfeecate == "9")
                                        semval = "9 Semester";
                                }
                                //yearwise
                                else if (rblCost_Wise.SelectedIndex == 1)
                                {
                                    semandyear = 1;
                                    if (selsctfeecate == "1" || selsctfeecate == "2")
                                        semval = "1 Year";
                                    else if (selsctfeecate == "3" || selsctfeecate == "4")
                                        semval = "2 Year";
                                    else if (selsctfeecate == "5" || selsctfeecate == "6")
                                        semval = "3 Year";
                                    else if (selsctfeecate == "7" || selsctfeecate == "8")
                                        semval = "4 Year";
                                }

                                else if (rblCost_Wise.SelectedIndex == 2)
                                {
                                    semandyear = 2;
                                    string date = "";
                                    string monthwise = stugymjoindate;
                                    string mon = string.Empty;
                                    string yer = string.Empty;
                                    string dat = string.Empty;

                                    string[] spl_date = monthwise.Split(' ');

                                    if (spl_date.Length > 0)
                                    {
                                        date = spl_date[0];
                                        string[] spl_month = date.Split('/');
                                        if (spl_month.Length > 0)
                                        {
                                            yer = spl_month[2];
                                            mon = spl_month[0];
                                            dat = spl_month[1];
                                            mnthamt = "," + month + ":" + year + ":" + stugymcost;
                                            mnthcol = mon + ":" + yer + ":" + stugymcost;
                                            //magesh 24/4/18
                                            if (selsctfeecate == "1")
                                                semval = "1 Semester";
                                            if (selsctfeecate == "2")
                                                semval = "2 Semester";
                                            if (selsctfeecate == "3")
                                                semval = "3 Semester";
                                            if (selsctfeecate == "4")
                                                semval = "4 Semester";
                                            if (selsctfeecate == "5")
                                                semval = "5 Semester";
                                            if (selsctfeecate == "6")
                                                semval = "6 Semester";
                                            if (selsctfeecate == "7")
                                                semval = "7 Semester";
                                            if (selsctfeecate == "8")
                                                semval = "8 Semester";
                                            if (selsctfeecate == "9")
                                                semval = "9 Semester";
                                        }
                                    }

                                }
                                else
                                {
                                    semandyear = 1;
                                    if (selsctfeecate == "1")
                                        semval = "Term 1";
                                    else if (selsctfeecate == "2")
                                        semval = "Term 2";
                                    else if (selsctfeecate == "3")
                                        semval = "Term 3";
                                    else if (selsctfeecate == "4")
                                        semval = "Term 4";
                                }
                                sqlcmd = d2.GetFunction("select distinct textcode from textvaltable where textcriteria ='FEECA' and textval='" + semval + "' and college_code='" + collegecode + "'");
                                if (sqlcmd != "0")
                                    tcode = Convert.ToString(sqlcmd);

                                if (app_no != "" && ledgPK != "" && stugymcost != "" && stugymcost != "0" && header_id != "" && tcode != "")
                                {
                                    if (semandyear == 1)
                                    {
                                        string paidamt = d2.GetFunction("");
                                        string querystu1 = "if exists (select * from HT_HostelRegistration where App_No='" + app_no + "' and MemType='1') update HT_HostelRegistration set  GymCode='" + stugymcode + "',GymJoindate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' else insert into HT_HostelRegistration(MemType,App_No,GymCode,GymJoindate) values('1','" + app_no + "','" + stugymcode + "','" + stugymjoindate + "')";
                                        querystu1 += " if exists (select * from FT_FeeAllot where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinyearFk='" + finYeaid + "') update FT_FeeAllot set AllotDate='" + stugymjoindate + "',MemType='1',FeeAmount='" + stugymcost + "',TotalAmount ='" + stugymcost + "' ,BalAmount ='" + stugymcost + "'-isnull(PaidAmount,'0')   where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinyearFk='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + app_no + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + stugymjoindate + "','" + stugymcost + "','" + tcode + "','',0,0,'" + stugymcost + "','" + stugymcost + "','1','1',0,0)";

                                        querystu1 += "if exists (select * from Hm_GymFeeAllot where App_No='" + app_no + "' and MemType='1') update Hm_GymFeeAllot set   GymCode='" + stugymcode + "',Pattern='" + pattern + "',Cost='" + stugymcost + "',GymJoinDate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' and  GymCode='" + stugymcode + "' else insert into Hm_GymFeeAllot(MemType,App_No,GymCode,Pattern,Cost,GymJoinDate) values('1','" + app_no + "','" + stugymcode + "','" + pattern + "','" + stugymcost + "','" + stugymjoindate + "')";
                                        query = d2.update_method_wo_parameter(querystu1, "Text");
                                    }
                                    else
                                    {
                                        #region month
                                        string fnlmnth = "";
                                        int balamt = 0;
                                        string Feemnth = d2.GetFunction("select FeeAmountMonthly from FT_FeeAllot where App_No='" + app_no + "' and FeeCategory ='" + tcode + "' and LedgerFK = '" + ledgPK + "'  and HeaderFK in('" + header_id + "') and FinyearFk='" + finYeaid + "'");
                                        if (Feemnth != "" && Feemnth != "0")
                                        {
                                            fnlmnth = mnthcol;
                                            string[] value = Feemnth.Split(',');
                                            for (int i = 0; i < value.Length; i++)
                                            {
                                                string[] mnthval = value[i].Split(':');
                                                {
                                                    if (mnthval.Length > 0)
                                                    {

                                                        if (AddMonthyear.ContainsKey(Convert.ToString(mnthval[0] + ":" + mnthval[1])))
                                                        {
                                                            mnthamt = "";
                                                            stugymcost = Convert.ToString(AddMonthyear[Convert.ToString(mnthval[0] + ":" + mnthval[1])]);
                                                            if (stugymcost == mnthval[2])
                                                            {
                                                                mnthval[2] = stugymcost;
                                                                stugymcost = "0";
                                                            }
                                                            else if (Convert.ToInt32(stugymcost) > Convert.ToInt32(mnthval[2]))
                                                            {
                                                                balamt = Convert.ToInt32(stugymcost) - Convert.ToInt32(mnthval[2]);
                                                                stugymcost = Convert.ToString(balamt);
                                                                mnthval[2] = stugymcost;
                                                            }
                                                            else if (Convert.ToInt32(stugymcost) < Convert.ToInt32(mnthval[2]))
                                                            {

                                                                int val = Convert.ToInt32(stugymcost);
                                                                balamt = Convert.ToInt32(stugymcost) - Convert.ToInt32(mnthval[2]);
                                                                stugymcost = Convert.ToString(balamt);
                                                                mnthval[2] = Convert.ToString(val);
                                                            }
                                                            if (fnlmnth == "")
                                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            else
                                                                fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            AddMonthyear.Remove(Convert.ToString(mnthval[0] + ":" + mnthval[1]));
                                                        }
                                                        else
                                                            if (fnlmnth == "")
                                                            {
                                                                string valuev = mnthval[0].ToString();
                                                                string valuev1 = mnthval[1].ToString();
                                                                string valuev3 = mnthval[2].ToString();
                                                                fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                            }
                                                            else
                                                                //fnlmnth = fnlmnth + "," + mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2];
                                                        fnlmnth = mnthval[0] + ":" + mnthval[1] + ":" + mnthval[2] + "," + fnlmnth;
                                                    }
                                                }
                                            }
                                            if (fnlmnth != "")
                                            {
                                                if (AddMonthyear.Count > 0)
                                                {
                                                    string ConCat = string.Empty;
                                                    foreach (DictionaryEntry Di in AddMonthyear)
                                                    {
                                                        string keyvalue = Convert.ToString(Di.Key);
                                                        string Value = Convert.ToString(Di.Value);
                                                        if (ConCat.Trim() != "")
                                                        {
                                                            ConCat += "," + keyvalue + ":" + Value;
                                                        }
                                                        else
                                                        {
                                                            ConCat = keyvalue + ":" + Value;
                                                        }
                                                    }
                                                    if (ConCat.Trim() != "")
                                                    {
                                                        fnlmnth += "," + ConCat;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (fnlmnth.Trim() == "")
                                                fnlmnth = mnthcol;
                                            else
                                                fnlmnth = mnthamt;
                                        }
                                        string querystu1 = "if exists (select * from HT_HostelRegistration where App_No='" + app_no + "' and MemType='1') update HT_HostelRegistration set  GymCode='" + stugymcode + "',GymJoindate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' else insert into HT_HostelRegistration(MemType,App_No,GymCode,GymJoindate) values('1','" + app_no + "','" + stugymcode + "','" + stugymjoindate + "')";
                                        querystu1 += " if exists (select * from FT_FeeAllot where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinyearFk='" + finYeaid + "') update FT_FeeAllot set FeeAmount='" + stugymcost + "',TotalAmount ='" + stugymcost + "' ,BalAmount ='" + stugymcost + "'-isnull(PaidAmount,'0'), FeeAmountMonthly='" + fnlmnth + "'  where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinyearFk='" + finYeaid + "'  else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt,FeeAmountMonthly)  values ('" + app_no + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + stugymjoindate + "','" + stugymcost + "','" + tcode + "','',0,0,'" + stugymcost + "','" + stugymcost + "','1','1',0,0" + mnthvalue1 + ",'" + fnlmnth + "')";
                                        querystu1 += "if exists (select * from Hm_GymFeeAllot where App_No='" + app_no + "' and MemType='1') update Hm_GymFeeAllot set   GymCode='" + stugymcode + "',Pattern='" + pattern + "',Cost='" + stugymcost + "',GymJoinDate='" + stugymjoindate + "' where App_No='" + app_no + "' and MemType='1' and  GymCode='" + stugymcode + "' else insert into Hm_GymFeeAllot(MemType,App_No,GymCode,Pattern,Cost,GymJoinDate) values('1','" + app_no + "','" + stugymcode + "','" + pattern + "','" + stugymcost + "','" + stugymjoindate + "')";
                                        query = d2.update_method_wo_parameter(querystu1, "Text");

                                        string allotpk = d2.GetFunction(" select FeeAllotPK from FT_FeeAllot  where App_No ='" + app_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinyearFk='" + finYeaid + "'");
                                        if (allotpk != "" && finYeaid != "" && fnlmnth.Trim() != "")
                                        {
                                            string[] FistSplit = fnlmnth.Split(',');
                                            if (FistSplit.Length > 0)
                                            {
                                                for (int itFisrt = 0; itFisrt < FistSplit.Length; itFisrt++)
                                                {
                                                    if (FistSplit[itFisrt].Trim() != "")
                                                    {
                                                        string[] SecondSplit = FistSplit[itFisrt].Split(':');
                                                        if (SecondSplit.Length > 1)
                                                        {
                                                            month = Convert.ToString(SecondSplit[0]);
                                                            year = Convert.ToString(SecondSplit[1]);
                                                            stugymcost = Convert.ToString(SecondSplit[2]);
                                                            if (month.Trim() != "" && year.Trim() != "" && stugymcost.Trim() != "")
                                                            {
                                                                string InsertQ = "if exists(select * from FT_FeeallotMonthly where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + finYeaid + "')update FT_FeeallotMonthly set AllotAmount='" + stugymcost + "',BalAmount='" + stugymcost + "'-isnull(PaidAmount,'0') where FeeAllotPK='" + allotpk + "' and AllotMonth='" + month + "' and AllotYear='" + year + "' and FinYearFK='" + finYeaid + "' else insert into FT_FeeallotMonthly (FeeAllotPK,AllotMonth,AllotYear,AllotAmount,FinYearFK,BalAmount) values('" + allotpk + "','" + month + "','" + year + "','" + stugymcost + "','" + finYeaid + "','" + stugymcost + "')";
                                                                query = d2.update_method_wo_parameter(InsertQ, "Text");
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        #endregion
                                    }
                                }

                            }

                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_erroralert.Text = "Please Select Finance Year";
                                return;
                            }
                        }
                    }
                }

                if (query != 0 && btn_Save_student.Text == "Save")
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Successfully";
                    loadgymname();
                    txt_pop1rollno.Text = "";
                    txt_studentname.Text = "";
                    txt_degre.Text = "";
                    txt_cost.Text = "";
                    txt_date.Text = "";

                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                    loadgymname();
                    txt_pop1rollno.Text = "";
                    txt_studentname.Text = "";
                    txt_degre.Text = "";
                    txt_cost.Text = "";
                    txt_date.Text = "";
                }
            }
        }
        catch (Exception ex) { }

        //d2.sendErrorMail(ex, collegecode, "GymAllotment");

    }
    #endregion

    #region Save_For_Staff
    protected void btn_Save_Staff_Click(object sender, EventArgs e)
    {
        try
        {
            string hosstaffcode = string.Empty;
            string staffapp_no = string.Empty;
            string qryhostaffgymsave = string.Empty;
            string staffPattern = string.Empty;
            int staffquery = 0;
            string staffcollegecode = Convert.ToString(ddl_collegename.SelectedValue);
            if (!string.IsNullOrEmpty(staffcollegecode))
            {
                if (rblstustaff.SelectedIndex == 1)
                {
                    if (ddl_gymname.Items.Count > 0)
                    {
                        stugymcode = Convert.ToString(ddl_gymname.SelectedValue);
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "No Record Selected";
                    }
                    if (txt_staffcode.Text != "" && txt_cost.Text != "" && txt_date.Text != "")
                    {

                        if (rblCost_Wise.SelectedIndex == 0)
                        {
                            staffPattern = "Semester";
                        }
                        else if (rblCost_Wise.SelectedIndex == 1)
                        {
                            staffPattern = "Year";
                        }
                        else if (rblCost_Wise.SelectedIndex == 2)
                        {
                            staffPattern = "Month";
                        }
                        else
                        {
                            staffPattern = "Term";
                        }
                        stugymcost = txt_cost.Text;
                        //stugymjoindate = txt_date.Text;
                        string gydate = txt_date.Text;
                        string[] frdat = gydate.Split('/');
                        if (frdat.Length == 3)
                            stugymjoindate = frdat[1].ToString() + "/" + frdat[0].ToString() + "/" + frdat[2].ToString();
                        hosstaffcode = txt_staffcode.Text;
                        staffapp_no = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + hosstaffcode + "' and sam.appl_no = sm.appl_no");
                        //header and ledger
                        string Gymfeeset = "select header,ledger,Text_value from HM_Feessetting where Type='Gym' and collegecode='" + staffcollegecode + "'";
                        dsgymfees.Clear();
                        dsgymfees = d2.select_method_wo_parameter(Gymfeeset, "Text");
                        if (dsgymfees.Tables.Count > 0 && dsgymfees.Tables[0].Rows.Count > 0)
                        {
                            header_id = Convert.ToString(dsgymfees.Tables[0].Rows[0]["header"]);
                            ledgPK = Convert.ToString(dsgymfees.Tables[0].Rows[0]["ledger"]);
                            exincludemessbill = Convert.ToString(dsgymfees.Tables[0].Rows[0]["Text_value"]);
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Please set Fees setting";
                            return;
                        }

                        if (exincludemessbill == "1")
                        {
                            if (staffapp_no.Trim() != "0" && staffapp_no.Trim() != "")
                            {
                                qryhostaffgymsave = "if exists (select * from HT_HostelRegistration where App_No='" + staffapp_no + "' and MemType='2') update HT_HostelRegistration set  GymCode='" + stugymcode + "',GymJoindate='" + stugymjoindate + "' where App_No='" + staffapp_no + "' and MemType='2' else insert into HT_HostelRegistration(MemType,App_No,GymCode,GymJoindate) values('2','" + staffapp_no + "','" + stugymcode + "','" + stugymjoindate + "')";

                                qryhostaffgymsave += "if exists (select * from Hm_GymFeeAllot where App_No='" + staffapp_no + "' and MemType='2') update Hm_GymFeeAllot set   GymCode='" + stugymcode + "',Pattern='" + staffPattern + "',Cost='" + stugymcost + "',GymJoinDate='" + stugymjoindate + "' where App_No='" + staffapp_no + "' and MemType='2' and  GymCode='" + stugymcode + "' else insert into Hm_GymFeeAllot(MemType,App_No,GymCode,Pattern,Cost,GymJoinDate) values('2','" + staffapp_no + "','" + stugymcode + "','" + staffPattern + "','" + stugymcost + "','" + stugymjoindate + "')";

                                staffquery = d2.update_method_wo_parameter(qryhostaffgymsave, "Text");
                            }
                        }
                        else
                        {
                            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);
                            if (finYeaid.Trim() != "" && finYeaid.Trim() != "0")
                            {
                                tcode = "0";
                                if (staffapp_no != "" && ledgPK != "" && stugymcost != "" && stugymcost != "0" && header_id != "" && tcode != "")
                                {
                                    string paidamt = d2.GetFunction("");

                                    string querystaff = "if exists (select * from HT_HostelRegistration where App_No='" + staffapp_no + "' and MemType='2') update HT_HostelRegistration set  GymCode='" + stugymcode + "',GymJoindate='" + stugymjoindate + "' where App_No='" + staffapp_no + "' and MemType='2' else insert into HT_HostelRegistration(MemType,App_No,GymCode,GymJoindate) values('2','" + staffapp_no + "','" + stugymcode + "','" + stugymjoindate + "')";
                                    querystaff += " if exists (select * from FT_FeeAllot where App_No ='" + staffapp_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' ) update FT_FeeAllot set AllotDate='" + stugymjoindate + "',MemType='2',FeeAmount='" + stugymcost + "',TotalAmount ='" + stugymcost + "' ,BalAmount ='" + stugymcost + "'-isnull(PaidAmount,'0')   where App_No ='" + staffapp_no + "' and LedgerFK='" + ledgPK + "' and HeaderFK='" + header_id + "' and FeeCategory ='" + tcode + "' and FinYearFK='" + finYeaid + "' else insert into FT_FeeAllot (App_No,LedgerFK,HeaderFK,FinYearFK,AllotDate,FeeAmount,FeeCategory,PayStartDate,FineAmount,DeductAmount,TotalAmount,BalAmount,MemType,PayMode,DeductReason,FromGovtAmt)  values ('" + staffapp_no + "','" + ledgPK + "','" + header_id + "','" + finYeaid + "','" + stugymjoindate + "','" + stugymcost + "','" + tcode + "','',0,0,'" + stugymcost + "','" + stugymcost + "','2','1',0,0)";

                                    querystaff += "if exists (select * from Hm_GymFeeAllot where App_No='" + staffapp_no + "' and MemType='2') update Hm_GymFeeAllot set   GymCode='" + stugymcode + "',Pattern='" + staffPattern + "',Cost='" + stugymcost + "',GymJoinDate='" + stugymjoindate + "' where App_No='" + staffapp_no + "' and MemType='2' and  GymCode='" + stugymcode + "' else insert into Hm_GymFeeAllot(MemType,App_No,GymCode,Pattern,Cost,GymJoinDate) values('2','" + staffapp_no + "','" + stugymcode + "','" + staffPattern + "','" + stugymcost + "','" + stugymjoindate + "')";
                                    staffquery = d2.update_method_wo_parameter(querystaff, "Text");

                                }

                                //string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + memtype + ",1," + appno + "," + lid + "," + hid + "," + amt + "," + amt + "," + text_circode + "," + amt + "," + finYeaid + ",0,0) ";

                                //selectQuery = " select * from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "') and  FinYearFK='" + finYeaid + "' and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";

                                //updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=" + memtype + ",FeeAmount=isnull(FeeAmount,0)+" + amt + ",BalAmount=isnull(BalAmount,0)+" + amt + ",TotalAmount=isnull(TotalAmount,0)+" + amt + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + text_circode + "') and  FinYearFK='" + finYeaid + "' and App_No in('" + appno + "') and isnull(istransfer,'0')='0'";


                                //string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";

                            }

                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Finance Year')", true);

                            }


                        }
                    }
                }

                if (staffquery != 0 && btn_Save_Staff.Text == "Save")
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Successfully";
                    loadgymname();
                    txt_pop1staffname.Text = "";
                    txt_staffcode.Text = "";
                    txt_dept.Text = "";
                    txt_design.Text = "";
                    txt_cost.Text = "";
                    txt_date.Text = "";

                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                    loadgymname();
                    txt_pop1staffname.Text = "";
                    txt_staffcode.Text = "";
                    txt_dept.Text = "";
                    txt_design.Text = "";
                    txt_cost.Text = "";
                    txt_date.Text = "";

                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    #endregion


    #region qusPopupWindow_for_Student


    protected void bindpop2collegename()
    {
        try
        {
            string clgname = "select college_code,collname from collinfo ";
            if (clgname != "")
            {
                ds = d2.select_method(clgname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl_pop2collgname.DataSource = ds;
                    ddl_pop2collgname.DataTextField = "collname";
                    ddl_pop2collgname.DataValueField = "college_code";
                    ddl_pop2collgname.DataBind();

                }
            }

            bindpop2hostel();
            bindpop2degree();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void bindpop2degree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_pop2degre.DataSource = ds;
            ddl_pop2degre.DataTextField = "course_name";
            ddl_pop2degre.DataValueField = "course_id";
            ddl_pop2degre.DataBind();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    public void loadbranch()
    {
        try
        {
            string query1 = "";
            string buildvalue1 = "";
            string build1 = "";
            ddl_pop2branch.Items.Clear();
            if (ddl_pop2degre.Items.Count > 0)
            {
                for (int i = 0; i < ddl_pop2degre.Items.Count; i++)
                {
                    build1 = ddl_pop2degre.SelectedValue;
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
                query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_pop2collgname.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code";
                ds = d2.select_method(query1, hat, "Text");
                ddl_pop2branch.DataSource = ds;
                ddl_pop2branch.DataTextField = "dept_name";
                ddl_pop2branch.DataValueField = "degree_code";
                ddl_pop2branch.DataBind();
                //  ddl_pop2branch.Items.Insert(0, "All");
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }

    protected void bindpop2batchyear()
    {
        try
        {

            hat.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop2batchyear.DataSource = ds;
                ddl_pop2batchyear.DataTextField = "batch_year";
                ddl_pop2batchyear.DataValueField = "batch_year";
                ddl_pop2batchyear.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void ddl_pop2collgname_selectedindexchange(object sender, EventArgs e)
    {
        try
        {

            bindpop2batchyear();
            bindpop2degree();
            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void ddl_pop2batchyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindpop2degree();
            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void ddl_pop2degre_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    public void loadStudentdetails()
    {
        try
        {
            if (ddl_pop2branch.Items.Count > 0)
            {
                string buildvalue1 = "";
                string build1 = "";
                build1 = ddl_pop2branch.SelectedValue;
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
                if (buildvalue1 != "" && buildvalue1 != "All")
                {
                    sqladd = sqladd + " AND g.degree_code in ('" + buildvalue1 + "')";
                }
                else
                {
                    sqladd = sqladd + "";
                }
            }

            if (Rollflag1 == "1")
            {
                fpsturoll.Columns[1].Visible = true;
                fpsturoll.Width = 426;
            }
            else
            {
                fpsturoll.Columns[1].Visible = false;
                fpsturoll.Width = 326;
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
            fpsturoll.Sheets[0].RowCount = 0;
            fpsturoll.Sheets[0].RowHeader.Visible = false;
            fpsturoll.SaveChanges();
            fpsturoll.Sheets[0].AutoPostBack = false;
            ds.Clear();
            string q = sqladd + strorderby;
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count <= 0)
            {

                fpsturoll.Visible = false;
                lblpop2error.Visible = true;
                lblpop2error.Text = "No Students Found Or Roll numbers might not be generated";
                btn_pop2ok.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fpsturoll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Columns[0].Locked = true;
                    fpsturoll.Columns[1].Locked = true;
                    fpsturoll.Columns[2].Locked = true;
                    fpsturoll.Columns[3].Locked = true;
                    int sno = 0;
                    lblpop2error.Visible = false;
                    fpsturoll.Visible = true;
                    fpsturoll.CommandBar.Visible = false;
                    btn_pop2ok.Visible = true;
                    btn_pop2exit.Visible = true;
                    sno = 0;
                    int studcount = 0;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (int row1 = 0; row1 < ddl_pop2branch.Items.Count; row1++)
                    {
                        if (ddl_pop2branch.Items[row1].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(ddl_pop2branch.Items[row1].Value) + "'";
                            DataView dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                fpsturoll.Sheets[0].RowCount = fpsturoll.Sheets[0].RowCount + 1;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["branch"]);
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpsturoll.Sheets[0].AddSpanCell(fpsturoll.Sheets[0].RowCount - 1, 0, 1, 4);
                                sno++;
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    studcount++;
                                    fpsturoll.Sheets[0].RowCount++;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["App_No"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["roll_no"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["id"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[0]["branch"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 4].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[row]["stud_name"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                    }
                    int rowcount = fpsturoll.Sheets[0].RowCount;
                    fpsturoll.Height = 270;
                    fpsturoll.Sheets[0].PageSize = 15 + (rowcount * 5);
                    fpsturoll.SaveChanges();
                }
                else
                {
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void btn_pop2go_Click(object sender, EventArgs e)
    {
        try
        {
            string hostudcollcode = string.Empty;
            string hostuddegree = string.Empty;
            string hostudbranch = string.Empty;
            string hostudbatch = string.Empty;

            string hostelgender = d2.GetFunction("select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            hostelgender = " and a.sex in(" + hostelgender + ")";

            if (ddl_pop2collgname.Items.Count > 0)
                hostudcollcode = Convert.ToString(ddl_pop2collgname.SelectedValue);
            if (ddl_pop2degre.Items.Count > 0)
                hostuddegree = Convert.ToString(ddl_pop2degre.SelectedValue);
            if (ddl_pop2batchyear.Items.Count > 0)
                hostudbatch = Convert.ToString(ddl_pop2batchyear.SelectedValue);
            if (ddl_pop2branch.Items.Count > 0)
                hostudbranch = Convert.ToString(ddl_pop2branch.SelectedValue);
            if (!string.IsNullOrEmpty(hostudcollcode) && !string.IsNullOrEmpty(hostuddegree) && !string.IsNullOrEmpty(hostudbatch) && !string.IsNullOrEmpty(hostudbranch))
            {
                if (ddl_pop2sex.SelectedItem.Text == "All")
                {
                    sqladd = "select distinct r.App_No,hr.id, hr.id,roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1' and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and  G.Course_ID ='" + hostuddegree + "' " + hostelgender + " and hr.HostelMasterFK in ('" + Convert.ToString(ddl_pop1hostelname.SelectedValue) + "') ";
                    loadStudentdetails();
                }
                else if (ddl_pop2sex.SelectedItem.Text == "Male")
                {
                    sqladd = "select r.App_No,hr.id, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='0' and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + " and hr.HostelMasterFK in ('" + Convert.ToString(ddl_pop1hostelname.SelectedValue) + "') ";
                    loadStudentdetails();
                }
                else if (ddl_pop2sex.SelectedItem.Text == "Female")
                {
                    sqladd = "select r.App_No,hr.id, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='1' and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + " and hr.HostelMasterFK in ('" + Convert.ToString(ddl_pop1hostelname.SelectedValue) + "') ";
                    loadStudentdetails();
                }
                else
                {
                    sqladd = "select r.App_No,hr.id,roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='2' and g.college_code = d.college_codeand r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + " and hr.HostelMasterFK in ('" + Convert.ToString(ddl_pop1hostelname.SelectedValue) + "')  ";
                    loadStudentdetails();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void btn_pop2ok_Click(object sender, EventArgs e)
    {
        try
        {
            popwindowstudent.Visible = false;
            string activerow = "";
            string activecol = "";
            string appno = " ";
            activerow = fpsturoll.ActiveSheetView.ActiveRow.ToString();
            activecol = fpsturoll.ActiveSheetView.ActiveColumn.ToString();
            string purpose = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string retroll = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            string stuid = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow),2].Text;
            appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + purpose + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            branch = Convert.ToString(fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            string name = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            txt_pop1rollno.Text = purpose;
            txt_degre.Text = branch;
            txt_studentname.Text = name;
            txtid.Text = stuid;

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }

    protected void btn_pop2exit_Click(object sender, EventArgs e)
    {
        popwindowstudent.Visible = false;
    }

    #endregion


    #region quspopupWindow_for_Staff
    public void loadcollegestaffpopup()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegestaff.DataSource = ds;
                ddl_collegestaff.DataTextField = "collname";
                ddl_collegestaff.DataValueField = "college_code";
                ddl_collegestaff.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
        {
        }
    }

    public void bindstaffdepartmentpopup()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            string clgcode = "";
            if (ddl_collegestaff.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_collegestaff.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_staffdepartment.DataSource = ds;
                ddl_staffdepartment.DataTextField = "dept_name";
                ddl_staffdepartment.DataValueField = "dept_code";
                ddl_staffdepartment.DataBind();

                ddl_staffdepartment.Items.Insert(0, "All");
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }
    }


    protected void ddl_collegestaff_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bindstaffdepartmentpopup();
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void ddl_staffdepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = " ";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void ddl_searchbystaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_searchbystaff.SelectedItem.Text == "Staff Name")
            {
                txt_staffnamesearch.Visible = true;
                txt_staffcodesearch.Visible = false;
                txt_staffnamesearch.Text = "";

            }
            else if (ddl_searchbystaff.SelectedItem.Text == "Staff Code")
            {
                txt_staffcodesearch.Visible = true;
                txt_staffnamesearch.Visible = false;
                txt_staffnamesearch.Text = "";
            }
            div1.Visible = false;
            Fpstaff.Visible = false;
            btn_staffok.Visible = false;
            btn_staffexit.Visible = false;
            lbl_errorsearch.Visible = false;
            lbl_errorsearch.Text = " ";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void btn_staffselectgo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            string sql = "";
            int rowcount;
            string hostaffcollcode = string.Empty;
            string hostaffdeptcode = string.Empty;
            string qrystaffdept = string.Empty;
            string qrystaffnamesearch = string.Empty;
            string qrystaffcodesearch = string.Empty;
            //Fpstaff.Visible = true;
            if (ddl_collegestaff.Items.Count > 0)
                hostaffcollcode = Convert.ToString(ddl_collegestaff.SelectedItem.Value);

            if (ddl_staffdepartment.SelectedItem.Text != "All")
            {
                if (ddl_staffdepartment.Items.Count > 0)
                    hostaffdeptcode = Convert.ToString(ddl_staffdepartment.SelectedValue);
                qrystaffdept = "and h.dept_code='" + hostaffdeptcode + "' ";

            }
            if (txt_staffnamesearch.Text != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 0)
                {
                    qrystaffnamesearch = "and s.Staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "'";
                }
            }
            if (txt_staffcodesearch.Text.Trim() != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 1)
                {
                    qrystaffcodesearch = "and s.staff_code ='" + Convert.ToString(txt_staffcodesearch.Text) + "'";
                }
            }


            if (!string.IsNullOrEmpty(hostaffcollcode))
            {
                sql = "select distinct a.appl_id,s.staff_code,s.staff_name ,h.dept_code,hr.id,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a,HT_HostelRegistration hr where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and hr.MemType='2'  and hr.APP_No=a.appl_id and s.college_Code='" + hostaffcollcode + "' " + qrystaffnamesearch + qrystaffcodesearch + qrystaffdept + " and hr.HostelMasterFK in('" + Convert.ToString(ddl_pop1hostelname.SelectedValue) + "') ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
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

            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 6;
            if (ds.Tables[0].Rows.Count > 0)
            {
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

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Id";
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
                Fpstaff.Columns[5].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[5].Locked = true;
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

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["id"]);
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
                // lbl_errorsearch.Visible = true;
                // lbl_errorsearch.Text = "No Records Found";
                //lbl_errorsearch1.Visible = true;
                //lbl_errorsearch1.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 345;
                Fpstaff.Width = 846;
                btn_staffok.Visible = true;
                btn_staffexit.Visible = true;
                Fpstaff.Visible = true;
                div1.Visible = true;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                Fpstaff.Visible = false;
                btn_staffok.Visible = false;
                btn_staffexit.Visible = false;
                div1.Visible = false;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch1.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }


    }

    protected void btn_staffok_Click(object sender, EventArgs e)
    {
        try
        {

            if (txt_staffcodesearch.Text != "" || txt_staffnamesearch.Text != "" || ddl_searchbystaff.SelectedIndex != -1)
            {
                if (Fpstaff.Visible == true)
                {

                    popupwindowstaff.Visible = false;
                    string activerow = "";
                    string activecol = "";
                    string sql = "";
                    activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                    string applid = "";
                    string StaffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string Staffid = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    for (int i = 0; i < Fpstaff.Sheets[0].RowCount; i++)
                    {
                        if (i == Convert.ToInt32(activerow))
                        {
                            Fpstaff.Sheets[0].Rows[i].BackColor = Color.LightBlue;

                        }
                        else
                        {
                            Fpstaff.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        }
                    }
                    if (activerow != Convert.ToString(-1))
                    {
                        string applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + StaffCode + "'");
                        applid = d2.GetFunction("select appl_id  from staff_appl_master sam,staffmaster sm where  sm.staff_code='" + StaffCode + "' and sam.appl_no = sm.appl_no");
                        ViewState["appl_id"] = Convert.ToString(applid);
                        sql = "select convert(varchar,convert(datetime,date_of_birth,103),103) from staff_appl_master where appl_no='" + applno + "'";
                        string StaffDob = d2.GetFunction(sql);
                        string StaffName = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        string StaffDepartment = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                        string StaffDesignation = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                        txt_pop1staffname.Text = StaffName;
                        txt_staffcode.Text = StaffCode;
                        txt_dept.Text = StaffDepartment;
                        txt_design.Text = StaffDesignation;
                        txtid1.Text = Staffid;

                    }
                    else
                    {
                        lbl_errorsearch.Visible = true;
                        lbl_errorsearch.Text = "Please Select Any Staff Name";
                        popupwindowstaff.Visible = true;
                    }
                    txt_staffcodesearch.Text = "";
                    txt_staffnamesearch.Text = "";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "No records found";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "GymAllotment"); }

    }

    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        popupwindowstaff.Visible = false;

    }

    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {

        popupwindowstaff.Visible = false;
    }

    #endregion


    #region close
    protected void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        popwindowaddnew.Visible = false;
    }

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindowstudent.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btnerrclose1_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    #endregion

}