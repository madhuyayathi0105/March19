using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Batchschedular : System.Web.UI.Page
{
    DAccess2 obi_access = new DAccess2();
    DAccess2 d2 = new DAccess2();
    bool flagbatch = false;
    bool flag_true = false;
    Hashtable htsubjectno = new Hashtable();
    string ddlcollegevalue = string.Empty;
    string ddlbatchvalue = string.Empty;
    string ddlsemvalue = string.Empty;
    string ddlsecvalue = string.Empty;
    string ddldegreevalue = string.Empty;
    string ddlbranchvalue = string.Empty;
    string selectedbatch = string.Empty;
    string studentname = string.Empty;
    string rollno = string.Empty;
    string regno = string.Empty;
    string userCollegeCode = string.Empty;

    string batchva = string.Empty;
    bool chk = false;
    string droptime = string.Empty;

    DropDownList ddlsection;
    bool serialflag = false;
    static string dayorder = string.Empty;
    Hashtable hat = new Hashtable();

    DataSet ds = new DataSet();
    DataSet dsprint = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;

    #region
    DataTable gviewdtable = new DataTable();
    DataRow gviewdrow = null;
    DataTable gview1dtable = new DataTable();
    DataRow gview1row = null;
    DataTable gview1tag = new DataTable();
    DataRow gview1tagrow = null;
    #endregion

    //protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lblerror.Visible = false;//added by Srinath 17/8/2013
    //    DropDownList ddlbatchparent = (DropDownList)this.user_control.FindControl("ddlBatch");
    //    DropDownList ddlbranchparent = (DropDownList)this.user_control.FindControl("ddlBranch");
    //    DropDownList ddlsemparent = (DropDownList)this.user_control.FindControl("ddlSemYr");
    //    DropDownList ddlsection = (DropDownList)this.user_control.FindControl("ddlSec");
    //    ddlsection.Items.Remove("All");
    //    ddltimetable.Items.Clear();
    //    DataSet ds_cssem = new DataSet();
    //    string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchparent.SelectedValue.ToString() + "' and batch_year='" + ddlbatchparent.SelectedValue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
    //    ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
    //    string currentsem = string.Empty;
    //    if (ds_cssem.Tables[0].Rows.Count > 0)
    //    {
    //        currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
    //        if (Convert.ToString(currentsem) == Convert.ToString(ddlsemparent.SelectedValue.ToString()))
    //        {
    //            if (ddltimetable != null)
    //            {
    //                string sections = string.Empty;
    //                string strsec = string.Empty;
    //                sections = ddlsection.SelectedValue.ToString();
    //                if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
    //                {
    //                    strsec = string.Empty;
    //                }
    //                else
    //                {
    //                    strsec = " and sections='" + sections.ToString().Trim() + "'";
    //                }
    //                DataSet ds_batchs = new DataSet();
    //                string batchquery = " select TTName, convert(varchar(15),FromDate,103) as FromDate from Semester_Schedule where degree_code='" + ddlbranchparent.SelectedValue.ToString() + "' and batch_year='" + ddlbatchparent.SelectedValue.ToString() + "' " + strsec + " and semester='" + ddlsemparent.SelectedValue.ToString() + "'";
    //                ds_batchs = obi_access.select_method_wo_parameter(batchquery, "text");
    //                if (ds_batchs.Tables[0].Rows.Count > 0)
    //                {
    //                    ddltimetable.Items.Clear();
    //                    DataTable tbl = ds_batchs.Tables[0];
    //                    foreach (DataRow row in tbl.Rows)
    //                    {
    //                        object value = row["TTname"];
    //                        object value1 = row["FromDate"];
    //                        string total = value.ToString() + "@" + value1.ToString();
    //                        ddltimetable.Items.Add(total);
    //                    }
    //                }
    //                else
    //                {
    //                    ddltimetable.Items.Clear();
    //                    ddltimetable.Visible = true;
    //                    lblerror.Text = "Please Add Timetable Name Before Allot the Batch";
    //                    lblerror.Visible = true;
    //                }
    //            }
    //        }
    //    }
    //}

    //protected void Removesecall(object sender, EventArgs e)
    //{
    //    DropDownList ddlsection = (DropDownList)this.user_control.FindControl("ddlSec");
    //    ddltimetable.Items.Clear();
    //    ddlsection.Items.Remove("All");
    //    //ddlsection_SelectedIndexChanged(sender, e);
    //}

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
            userCollegeCode = Convert.ToString(Session["collegecode"]);
            lblerror.Visible = false;
            //End============
            if (!IsPostBack)
            {
                Fieldset1.Visible = false;
                Fieldset3.Visible = false;
                lblerror.Visible = false;
                gview.Visible = false;
                gridTimeTable.Visible = false;
                lblselect.Visible = false;
                Btnsave.Visible = false;
                Button1.Visible = false;
                Btndelete.Visible = false;
                fromno.Visible = false;
                tono.Visible = false;
                Button2.Visible = false;
                CheckBox1.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                Fieldset4.Visible = false;
                Fieldset2.Visible = false;
                // Panel3.Visible = false;
                Checkboxlistbatch.Visible = false;
                LinkButton1.Visible = false;
                Button3.Visible = false;
                Fieldset5.Visible = false;
                Btnsave.Enabled = false;//added by srinath 31/8/2013
                Btndelete.Enabled = false;
                Fieldset6.Visible = false;
                Fieldset7.Visible = false;
                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                DataSet ds = d2.select_method_wo_parameter(Master1, "text");
                //bindTTName();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                BindCollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                bindTTName();
            }
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    public void BindCollege()
    {
        try
        {
            ddlcollege.Items.Clear();

            collegecode = ddlcollege.SelectedValue.ToString();// Session["Collegecode"].ToString();
            usercode = Session["UserCode"].ToString();

            Session["QueryString"] = "";
            group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
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
            dsprint = obi_access.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                //ddlcollege_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds = obi_access.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlBatch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    public void BindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
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
            ds = obi_access.select_method("bind_degree", hat, "sp");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    public void bindbranch()
    {

        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = ddlcollege.SelectedValue.ToString();// Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds = obi_access.select_method("bind_branch", hat, "sp");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }

    }

    public void bindsem()
    {
        ddlSemYr.Items.Clear();
        Boolean first_year;
        DataSet ds = new DataSet();
        first_year = false;
        int duration = 0;
        int i = 0;
        string qry = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
        ds = d2.select_method_wo_parameter(qry, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0]["ndurations"].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {

            string qrye = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
            ds = d2.select_method_wo_parameter(qrye, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0]["first_year_nonsemester"].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0]["duration"].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
        }
    }

    public void BindSectionDetail()
    {
        ddlSec.Items.Clear();
        string qry = "select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
        DataSet ds = d2.select_method_wo_parameter(qry, "Type");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataBind();
            ddlSec.Items.Insert(0, "All");
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Text = "";
            ddlSec.Enabled = false;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        bindTTName();
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        bindTTName();
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        bindbranch();
        bindsem();
        BindSectionDetail();
        bindTTName();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        bindsem();
        BindSectionDetail();
        bindTTName();
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        BindSectionDetail();
        bindTTName();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fieldset1.Visible = false;
        Fieldset3.Visible = false;
        string sections = "";
        string strsec = "";
        if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
            sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and sections='" + sections.ToString() + "'";
        }
        string SyllabusQry;
        SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(SyllabusQry, "Text");
        string str = ds.Tables[0].Rows[0]["syllabus_year"].ToString();
        string Sqlstr;
        Sqlstr = "";

        Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + str.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
        DataSet titles = new DataSet();
        titles = d2.select_method_wo_parameter(Sqlstr, "Type");
        bindTTName();
        if (titles.Tables[0].Rows.Count > 0)
        {
            //ddlparent1.DataSource = titles;
            //ddlparent1.DataValueField = "Criteria_No";
            //ddlparent1.DataTextField = "Criteria";
            //ddlparent1.DataBind();
            //ddlparent1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
        }
    }

    protected void bindTTName()
    {
        ddlSec.Items.Remove("All");
        lblerror.Visible = false;//added by Srinath 17/8/2013
        ddltimetable.Items.Clear();

        DataSet ds_cssem = new DataSet();
        string cseme = "select distinct current_semester from registration where degree_code ='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
        ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
        string currentsem = string.Empty;
        if (ds_cssem.Tables[0].Rows.Count > 0)
        {
            currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
            if (Convert.ToString(currentsem) == Convert.ToString(ddlSemYr.SelectedValue.ToString()))
            {
                if (ddltimetable != null)
                {
                    string sections = string.Empty;
                    string strsec = string.Empty;
                    if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                        sections = ddlSec.SelectedValue.ToString();
                    if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + sections.ToString().Trim() + "'";
                    }
                    DataSet ds_batchs = new DataSet();
                    string batchquery = " select TTName, convert(varchar(15),FromDate,103) as FromDate from Semester_Schedule where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + strsec + " and semester='" + ddlSemYr.SelectedValue.ToString() + "'";
                    ds_batchs = obi_access.select_method_wo_parameter(batchquery, "text");
                    if (ds_batchs.Tables[0].Rows.Count > 0)
                    {
                        ddltimetable.Items.Clear();
                        DataTable tbl = ds_batchs.Tables[0];
                        foreach (DataRow row in tbl.Rows)
                        {
                            object value = row["TTname"];
                            object value1 = row["FromDate"];
                            string total = value.ToString() + "@" + value1.ToString();
                            ddltimetable.Items.Add(total);
                        }
                    }
                    else
                    {
                        ddltimetable.Items.Clear();
                        ddltimetable.Visible = true;
                        lblerror.Text = "Please Add Timetable Name Before Allot the Batch";
                        lblerror.Visible = true;
                    }
                }
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["gviewdtable"] != null)
            {
                Session.Remove("gviewdtable");
            }
        }
        callGridBind();
    }

    public void callGridBind()
    {
        //if (Session["gviewdtable"] != null)
        //{
        //    DataTable dtGrid = (DataTable)Session["gviewdtable"];
        //    gview.DataSource = dtGrid;
        //    gview.DataBind();
        //    gview.HeaderRow.Visible = false;
        //}
        //else
        //{
        //    gview.DataSource = null;
        //    gview.DataBind();
        //}
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gview, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Batch Allocation Report";
            string pagename = "BatchSchedular.aspx";
            string ss = null;
            Printcontrol1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            Printcontrol1.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        droptime = ddltimetable.SelectedItem.ToString();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            userCollegeCode = ddlcollege.SelectedItem.Value;
            Fieldset1.Visible = true;
            Fieldset3.Visible = true;
            lblerror.Visible = false;
            rptprint.Visible = true;
            //Printcontrol.Visible = true;
            Fieldset6.Visible = false;
            Fieldset7.Visible = false;
            if (ddltimetable.Items.Count >= 1)
            {
                if (ddltimetable.SelectedItem.Text.Trim().ToString() == "")
                {
                    lblerror.Text = "Select Time Table Name";
                    lblerror.Visible = true;
                    gview.Visible = false;
                    gridTimeTable.Visible = false;
                    Fieldset1.Visible = false;
                    Fieldset2.Visible = false;
                    Fieldset3.Visible = false;
                    Fieldset4.Visible = false;
                    LinkButton1.Visible = false;
                    Fieldset5.Visible = false;
                    Button3.Visible = false;
                    Checkboxlistbatch.Visible = false;
                    return;
                }
            }
            else
            {
                lblerror.Text = "Select Time Table Name";
                lblerror.Visible = true;
                gview.Visible = false;
                gridTimeTable.Visible = false;
                Fieldset1.Visible = false;
                Fieldset2.Visible = false;
                Fieldset3.Visible = false;
                Fieldset4.Visible = false;
                LinkButton1.Visible = false;
                Fieldset5.Visible = false;
                Button3.Visible = false;
                Checkboxlistbatch.Visible = false;
                return;
            }
            int icount = 0;
            string subjectnu = string.Empty;

            ddlcollegevalue = ddlcollege.SelectedValue.ToString();
            ddlbatchvalue = ddlBatch.SelectedValue.ToString();
            ddldegreevalue = ddlDegree.SelectedValue.ToString();
            if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                ddlsecvalue = ddlSec.SelectedValue.ToString();
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();
            ddlbranchvalue = ddlBranch.SelectedValue.ToString();

            string sections = string.Empty;
            string strsec = string.Empty;
            if (ddlsecvalue.ToString().Trim().ToLower() == "all")
            {
                lblerror.Visible = true;
                gview.Visible = false;
                gridTimeTable.Visible = false;
                Fieldset2.Visible = false;
                Fieldset4.Visible = false;
                LinkButton1.Visible = false;
                Fieldset5.Visible = false;
                Button3.Visible = false;
                Checkboxlistbatch.Visible = false;
                lblerror.Text = "Please Select Any One Section";
            }
            else
            {
                sections = ddlsecvalue.ToString().Trim();
                if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + sections.ToString().Trim() + "'";
                }
                string cmd = "select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'";

                DataSet dnew = new DataSet();
                dnew = obi_access.select_method_wo_parameter(cmd, "Text");
                if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
                {
                    if (dnew.Tables[0].Rows[0]["LinkValue"].ToString() == "1")
                    {
                        serialflag = true;
                    }
                    else
                    {
                        serialflag = false;
                    }
                }
                DataSet ds_stu_names = new DataSet();
                string stu_namequery = "select roll_no as rollno,Reg_No as regno,stud_name as studentname  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by roll_no";
                if (serialflag == true)
                {
                    stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no as regno  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by serialno";  //modified by prabha  on jan 
                }
                else
                {
                    string orderby_Setting = obi_access.GetFunction("select value from master_Settings where settings='order_by'");
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
                    stu_namequery = "select roll_no as rollno,Reg_No as regno, stud_name as studentname  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                }
                ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");
                //saved batch details 
                DataSet ds_totalsubjects = new DataSet();
                string total_subjects = "select batch from subjectchooser,subject,registration where subject.subject_no=Subjectchooser.subject_no and Subjectchooser.semester = '" + ddlsemvalue.ToString() + "'and Subjectchooser.roll_no=registration.roll_no and  registration.degree_code='" + ddlbranchvalue.ToString() + "' and registration.batch_year='" + ddlbatchvalue.ToString() + "' and registration.current_Semester='" + ddlsemvalue.ToString() + "'";
                ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");

                DataSet ds_syllcode = new DataSet();
                string syllcode = "select syll_code from syllabus_master where degree_code = '" + ddlbranchvalue.ToString() + "' and semester = '" + ddlsemvalue.ToString() + "' and Batch_Year = '" + ddlbatchvalue.ToString() + "'";
                ds_syllcode = obi_access.select_method_wo_parameter(syllcode, "Text");//ds_syllcode.Tables[0].Rows[0]["syll_code"].ToString();
                string syllCode = string.Empty;
                if (ds_syllcode.Tables.Count > 0 && ds_syllcode.Tables[0].Rows.Count > 0)
                    syllCode = Convert.ToString(ds_syllcode.Tables[0].Rows[0]["syll_code"]);

                DataSet ds_subjectnum = new DataSet();
                string subjectnumber = "Select subjecT_no,subjecT_code from subject,sub_sem where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code='" + syllCode + "'";
                ds_subjectnum = obi_access.select_method_wo_parameter(subjectnumber, "Text");
                string SubNo = string.Empty;
                if (ds_subjectnum.Tables.Count > 0 && ds_subjectnum.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_subjectnum.Tables[0].Rows)
                    {
                        if (string.IsNullOrEmpty(SubNo))
                            SubNo = Convert.ToString(dr["subjecT_no"]);
                        else
                            SubNo = SubNo + "," + Convert.ToString(dr["subjecT_no"]);
                    }
                }

                if (ds_stu_names.Tables.Count > 0 && ds_stu_names.Tables[0].Rows.Count > 0)
                {
                    gview.Visible = true;
                    lblerror.Visible = false;
                    lblselect.Visible = false;
                    Fieldset4.Visible = true;
                    Btnsave.Visible = false;
                    Button1.Visible = true;
                    Btndelete.Visible = true;
                    CheckBox1.Visible = true;
                    Fieldset2.Visible = true;
                    //sasi
                    LinkButton1.Visible = false;
                    Fieldset5.Visible = false;
                    Button3.Visible = false;
                    Checkboxlistbatch.Visible = false;
                    //                    
                    //start===Added by Manikandan 28/07/2013                    
                    //=====end

                    gviewdtable.Columns.Add("SNo");
                    gviewdtable.Columns.Add("Roll No");
                    gviewdtable.Columns.Add("Reg No");
                    gviewdtable.Columns.Add("Student Name");
                    gviewdtable.Columns.Add("Batch");

                    int sno = 0;
                    for (int i = 0; i < ds_stu_names.Tables[0].Rows.Count; i++)
                    {
                        sno++;
                        studentname = ds_stu_names.Tables[0].Rows[i]["studentname"].ToString();
                        regno = ds_stu_names.Tables[0].Rows[i]["regno"].ToString();
                        rollno = ds_stu_names.Tables[0].Rows[i]["rollno"].ToString();

                        string selectedbatch = "select distinct batch from subjectchooser where roll_no='" + rollno + "' and semester='" + ddlsemvalue.ToString() + "' and subject_no in(" + SubNo + ") and batch is not null and batch<>''";
                        DataSet ds_selebatch = new DataSet();
                        ds_selebatch = obi_access.select_method_wo_parameter(selectedbatch, "text");
                        string bat = string.Empty;
                        if (ds_selebatch.Tables[0].Rows.Count > 0)
                        {
                            bat = ds_selebatch.Tables[0].Rows[0]["batch"].ToString();
                        }

                        gviewdrow = gviewdtable.NewRow();
                        gviewdrow["SNo"] = Convert.ToString(sno);
                        gviewdrow["Roll No"] = rollno;
                        gviewdrow["Reg No"] = regno;
                        gviewdrow["Student Name"] = studentname;

                        if (bat == "")
                        {
                            gviewdrow["Batch"] = string.Empty;
                        }
                        else
                        {
                            gviewdrow["Batch"] = bat;
                        }
                        gviewdtable.Rows.Add(gviewdrow);
                    }

                    Session["gviewdtable"] = gviewdtable;
                    gview.DataSource = gviewdtable;
                    gview.DataBind();
                    if (gview.Rows.Count > 1)
                    {
                        gview.Visible = true;
                        Btnsave.Enabled = false;
                        Btndelete.Enabled = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
                        btnprintmaster.Visible = false;//true
                    }
                    DataSet ds_batchbox = new DataSet();
                    string batchcombox = "select distinct subjectchooser.batch as batch from subjectchooser,Registration where subjectchooser.roll_no= registration.roll_no and semester ='" + ddlsemvalue.ToString() + "' and  registration.degree_Code = '" + ddlbranchvalue.ToString() + "' and registration.batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and subjectchooser.subject_no in(" + SubNo + ") and batch<>''";
                    ds_batchbox = obi_access.select_method_wo_parameter(batchcombox, "text");
                    DataTable dtt = new DataTable(); dtt.Columns.Add("batch");
                    DataRow drr = null;
                    if (ddlnobatches.Items.Count > 1)
                    {
                        for (int dd = 1; dd < ddlnobatches.Items.Count; dd++)
                        {
                            drr = dtt.NewRow();
                            drr["batch"] = ddlnobatches.Items[dd].Text;
                            dtt.Rows.Add(drr);
                        }
                    }
                    for (int ji = 0; ji < gview.Rows.Count; ji++)
                    {
                        if (ddlnobatches.Items.Count > 1)
                        {
                            DropDownList ddlbatch = (gview.Rows[ji].FindControl("ddlbatch") as DropDownList);
                            ddlbatch.DataSource = dtt;
                            ddlbatch.DataTextField = "batch";
                            ddlbatch.DataValueField = "batch";
                            ddlbatch.DataBind();
                            ddlbatch.Items.Insert(0, "");
                        }
                        else
                        {
                            if (ds_batchbox.Tables[0].Rows.Count > 0)
                            {
                                DropDownList ddlbatch = (gview.Rows[ji].FindControl("ddlbatch") as DropDownList);
                                ddlbatch.DataSource = ds_batchbox;
                                ddlbatch.DataTextField = "batch";
                                ddlbatch.DataValueField = "batch";
                                ddlbatch.DataBind();
                                ddlbatch.Items.Insert(0, "");
                            }
                        }
                    }
                    string selectedbat = "select * from subjectchooser where semester='" + ddlsemvalue.ToString() + "' and subject_no in(" + SubNo + ") and batch is not null and batch<>''";
                    DataSet ds_selebat = new DataSet();
                    ds_selebat = obi_access.select_method_wo_parameter(selectedbat, "text");

                    for (int jk = 0; jk < gview.Rows.Count; jk++)
                    {
                        string roll = (gview.Rows[jk].FindControl("lblroll") as Label).Text;
                        DataView dvOpen = new DataView();
                        ds_selebat.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "'";
                        dvOpen = ds_selebat.Tables[0].DefaultView;
                        if (dvOpen.Count > 0)
                        {
                            string bat = Convert.ToString(dvOpen[0]["Batch"]);

                            DropDownList ddl = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList);
                            (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                            if (ddl.SelectedItem != null)
                            {
                                if (ddl.SelectedItem.Text != bat)
                                {
                                    (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[jk].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                                }
                            }
                        }
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


                    ///////======================LabAlloc Start

                    gridTimeTable.Visible = true;
                    DataSet ds_periodallot = new DataSet();
                    int numbhrs = 0;
                    int numbdays = 0;
                    ArrayList daylist = new ArrayList();
                    daylist.Add("Mon");
                    daylist.Add("Tue");
                    daylist.Add("Wed");
                    daylist.Add("Thu");
                    daylist.Add("Fri");
                    daylist.Add("Sat");
                    daylist.Add("Sun");

                    string getsyllcode = string.Empty;
                    if (ds_syllcode.Tables.Count > 0 && ds_syllcode.Tables[0].Rows.Count > 0)
                    {
                        getsyllcode = ds_syllcode.Tables[0].Rows[0]["syll_code"].ToString();
                        string date1 = ddltimetable.SelectedItem.ToString();
                        string[] date_fm = date1.Split(new Char[] { '@' });
                        string[] date_fm1 = date_fm[date_fm.GetUpperBound(0)].Split(new Char[] { '/' });
                        string fmdate = date_fm1[2].ToString() + "/" + date_fm1[1].ToString() + "/" + date_fm1[0].ToString();
                        string period = "select * from semester_schedule where degree_Code = '" + ddlbranchvalue.ToString() + "' and semester = '" + ddlsemvalue.ToString() + "' and batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and fromdate = '" + fmdate + "'";
                        ds_periodallot = obi_access.select_method_wo_parameter(period, "text");
                        string numberofhrs = "select no_of_hrs_per_day,nodays from periodattndschedule where degree_Code = '" + ddlbranchvalue.ToString() + "'and semester ='" + ddlsemvalue.ToString() + "'";
                        DataSet ds_noofdays = obi_access.select_method_wo_parameter(numberofhrs, "text");
                        numbhrs = Convert.ToInt32(ds_noofdays.Tables[0].Rows[0]["no_of_hrs_per_day"]);
                        numbdays = Convert.ToInt32(ds_noofdays.Tables[0].Rows[0]["nodays"]);
                        DataTable dtv = ds_subjectnum.Tables[0];
                        Hashtable hatsubject = new Hashtable();
                        string validsunno = string.Empty;
                        if (ds_periodallot.Tables.Count > 0 && ds_periodallot.Tables[0].Rows.Count > 0)
                        {
                            for (int days = 0; days < numbdays; days++)
                            {
                                string dayvalue = Convert.ToString(daylist[days]);
                                string temphr = string.Empty;
                                for (int hrs = 1; hrs <= numbhrs; hrs++)
                                {
                                    int hrvalue = Convert.ToInt32(hrs);
                                    string dayhrvalue = dayvalue.ToString() + hrvalue.ToString();
                                    string schdeva = ds_periodallot.Tables[0].Rows[0][dayhrvalue].ToString();
                                    string[] sp = schdeva.Split(';');
                                    bool getflag = false;
                                    string othsub = string.Empty;
                                    for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
                                    {
                                        string val = sp[hr].ToString();
                                        if (val.Trim() != "" && val != null)
                                        {
                                            string[] spsub = val.Split('-');
                                            if (spsub.GetUpperBound(0) > 1)
                                            {
                                                dtv.DefaultView.RowFilter = " subject_no='" + spsub[0] + "'";
                                                DataView dt = dtv.DefaultView;
                                                if (dt.Count > 0)
                                                {
                                                    getflag = true;
                                                }
                                                if (dt.Count > 0)//magesh 4.9.18
                                                {
                                                    if (othsub == "")
                                                    {
                                                        othsub = spsub[0];
                                                    }
                                                    else
                                                    {
                                                        othsub = othsub + ',' + spsub[0];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (getflag == true)
                                    {
                                        string[] val = othsub.Split(',');
                                        for (int k = 0; k <= val.GetUpperBound(0); k++)
                                        {
                                            string gva = val[k];
                                            if (!hatsubject.Contains(gva))
                                            {
                                                hatsubject.Add(gva, dayhrvalue);
                                                if (validsunno == "")
                                                {
                                                    validsunno = gva;
                                                }
                                                else
                                                {
                                                    validsunno = validsunno + ',' + gva;
                                                }
                                            }
                                            else
                                            {
                                                string gphr = hatsubject[gva].ToString();
                                                gphr = gphr + ',' + dayhrvalue;
                                                hatsubject[gva] = gphr;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (validsunno.Trim() != "" && validsunno != null)
                        {
                            subjectnumber = "select subjecT_no,subjecT_code from subject where subject_no in(" + validsunno + ")";
                            ds_subjectnum = obi_access.select_method_wo_parameter(subjectnumber, "Text");
                            if (ds_subjectnum.Tables.Count > 0 && ds_subjectnum.Tables[0].Rows.Count > 0)
                            {
                                lblerror.Visible = false;
                                //start===Added by Manikandan 28/07/2013
                                //=====end

                                gview1dtable.Columns.Add("day");
                                gview1dtable.Columns.Add("hour");

                                gview1tag.Columns.Add("sno");
                                gview1tag.Columns.Add("day");
                                gview1tag.Columns.Add("hour");

                                for (int ssno = 0; ssno < ds_subjectnum.Tables[0].Rows.Count; ssno++)
                                {
                                    string selectedsubjectno = ds_subjectnum.Tables[0].Rows[ssno]["subjecT_no"].ToString();
                                    string selectedsubjectcode = ds_subjectnum.Tables[0].Rows[ssno]["subjecT_code"].ToString();

                                    gview1dtable.Columns.Add(selectedsubjectcode.ToString());
                                    gview1tag.Columns.Add(selectedsubjectno.ToString());
                                }
                                DataSet ds_batch = new DataSet();
                                string batchcomboxquery = "select distinct subjectchooser.batch as batch from subjectchooser,Registration where subjectchooser.roll_no= registration.roll_no and semester ='" + ddlsemvalue.ToString() + "' and subjectchooser.subject_no in(" + SubNo + ") and registration.degree_Code = '" + ddlbranchvalue.ToString() + "' and registration.batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and batch<>''";
                                ds_batch = obi_access.select_method_wo_parameter(batchcomboxquery, "text");
                                DataTable dt_batch = ds_batch.Tables[0];
                                string[] sublist1 = new string[dt_batch.Rows.Count + 1];
                                if (ds_batch.Tables.Count > 0 && ds_batch.Tables[0].Rows.Count > 0)
                                {
                                    for (icount = 0; icount < dt_batch.Rows.Count; icount++)
                                    {
                                        sublist1[icount] = dt_batch.Rows[icount]["batch"].ToString();
                                    }
                                    if (sublist1.GetUpperBound(0) > 0)
                                    {
                                        sublist1[icount] = " ";
                                    }
                                }
                                //added by sasi  on 
                                if (ds_batch.Tables.Count > 0 && ds_batch.Tables[0].Rows.Count > 0)
                                {
                                    Checkboxlistbatch.DataSource = ds_batch;
                                    Checkboxlistbatch.DataValueField = "batch";
                                    Checkboxlistbatch.DataTextField = "batch";
                                    Checkboxlistbatch.DataBind();
                                }
                                //-------end--------
                                if (ds_periodallot.Tables.Count > 0 && ds_periodallot.Tables[0].Rows.Count > 0)
                                {
                                    for (int days = 0; days < numbdays; days++)
                                    {
                                        string dayvalue = Convert.ToString(daylist[days]);
                                        string temphr = string.Empty;
                                        for (int hrs = 1; hrs <= numbhrs; hrs++)
                                        {
                                            int hrvalue = Convert.ToInt32(hrs);
                                            string dayhrvalue = dayvalue.ToString() + hrvalue.ToString();
                                            string sub = ds_periodallot.Tables[0].Rows[0][dayhrvalue].ToString();
                                            string[] sp_rd_split = sub.Split(';');
                                            for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                            {
                                                string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                if (sp2.GetUpperBound(0) >= 1)
                                                {
                                                    int upperbound = sp2.GetUpperBound(0);
                                                    subjectnu = sp2[0].ToString();
                                                    bool valiflag = false;
                                                    if (hatsubject.Contains(subjectnu))
                                                    {
                                                        string gethr = hatsubject[subjectnu].ToString();
                                                        string[] spi = gethr.Split(',');
                                                        for (int lo = 0; lo <= spi.GetUpperBound(0); lo++)
                                                        {
                                                            string valhr = spi[lo].ToString();
                                                            if (valhr.Trim().ToLower() == dayhrvalue.Trim().ToLower())
                                                            {
                                                                valiflag = true;
                                                            }
                                                        }
                                                    }
                                                    for (int subcol = 3; subcol < gview1tag.Columns.Count; subcol++)
                                                    {
                                                        if (subjectnu == gview1tag.Columns[subcol].ColumnName)
                                                        {
                                                            if (valiflag == true)
                                                            {
                                                                if (temphr.ToString() != hrvalue.ToString())
                                                                {
                                                                    temphr = hrvalue.ToString();
                                                                    gview1row = gview1dtable.NewRow();
                                                                    gview1row["day"] = dayvalue.ToString();
                                                                    gview1row["hour"] = hrvalue.ToString();
                                                                    gview1dtable.Rows.Add(gview1row);
                                                                }
                                                                string timetablename = string.Empty;
                                                                if (ddltimetable.Text.ToString().Trim() != "")
                                                                {
                                                                    string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                                                                    timetablename = " and timetablename='" + ttname[0] + "'";
                                                                }
                                                                string selecttedbatch = "select distinct stu_batch from laballoc where batch_year='" + ddlbatchvalue.ToString() + "' and Degree_code='" + ddlbranchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and  day_value='" + dayvalue.ToString() + "' and Hour_value='" + hrvalue.ToString() + "' and subject_no='" + subjectnu.ToString() + "' " + strsec + " " + timetablename + " ";
                                                                DataSet ds_setbatch = new DataSet();
                                                                ds_setbatch = obi_access.select_method_wo_parameter(selecttedbatch, "text");
                                                                if (ds_setbatch.Tables.Count > 0 && ds_setbatch.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string gkj = string.Empty;
                                                                    for (int bb = 0; bb < ds_setbatch.Tables[0].Rows.Count; bb++)
                                                                    {
                                                                        string shg = ds_setbatch.Tables[0].Rows[bb]["stu_batch"].ToString();
                                                                        if (gkj == "")
                                                                        {
                                                                            gkj = shg;
                                                                        }
                                                                        else
                                                                        {
                                                                            gkj = gkj + ',' + shg;
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
                                    Fieldset6.Visible = true;
                                    chkautoswitch.Checked = false;
                                    string timetablename1 = string.Empty;
                                    if (ddltimetable.Text.ToString().Trim() != "")
                                    {
                                        string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                                        timetablename1 = " and timetablename='" + ttname[0] + "'";
                                    }
                                    sections = ddlsecvalue.ToString().Trim();
                                    if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                                    {
                                        sections = string.Empty;
                                    }
                                    else
                                    {
                                        sections = " and sections='" + sections.ToString().Trim() + "'";
                                    }
                                    txtautoswitch.Text = "---Select---";
                                    chkswitch.Checked = false;
                                    string getautoswitchsub = "select distinct Day_Value,Hour_Value from LabAlloc where Batch_Year='" + ddlbatchvalue + "' and Degree_Code='" + ddlbranchvalue + "' and Semester='" + ddlsemvalue + "' " + sections + " and ISNULL(auto_switch,'0')<>'0'  " + timetablename1 + "";
                                    DataSet dsautoswitch = obi_access.select_method_wo_parameter(getautoswitchsub, "Text");
                                    if (dsautoswitch.Tables.Count > 0 && dsautoswitch.Tables[0].Rows.Count > 0)
                                    {
                                        chkautoswitch.Checked = true;
                                        loadautoswich();
                                    }
                                }

                                Session["tabletag"] = gview1tag;
                                Session["dtable1"] = gview1dtable;
                                gridTimeTable.DataSource = gview1dtable;
                                gridTimeTable.DataBind();
                                gridTimeTable.Visible = true;
                                DataTable data = new DataTable(); data.Columns.Add("batch");
                                DataRow droww = null;
                                if (gridTimeTable.Rows.Count > 0)
                                {
                                    if (ds_batch.Tables.Count > 0 && ds_batch.Tables[0].Rows.Count > 0)
                                    {
                                        if (ddlnobatches.Items.Count > 1)
                                        {
                                            for (int i = 1; i < ddlnobatches.Items.Count; i++)
                                            {
                                                droww = data.NewRow();
                                                droww["batch"] = ddlnobatches.Items[i].Text;
                                                data.Rows.Add(droww);
                                            }
                                        }
                                        for (int row = 0; row < gridTimeTable.Rows.Count; row++)
                                        {
                                            for (int cell = 1; cell < gridTimeTable.Rows[row].Cells.Count - 2; cell++)
                                            {
                                                (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataSource = ds_batch;// data;
                                                (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataTextField = "batch";
                                                (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataValueField = "batch";
                                                (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataBind();
                                            }
                                        }
                                    }
                                }
                                //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
                                //for (int row = 0; row < gridTimeTable.Rows.Count; row++)
                                //{

                                ddlcollegevalue = ddlcollege.SelectedValue.ToString();
                                string ddlbatchval = ddlBatch.SelectedValue.ToString();
                                string ddldegreeval = ddlDegree.SelectedValue.ToString();
                                string ddlsecval = string.Empty;
                                if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                                    ddlsecval = ddlSec.SelectedValue.ToString();
                                string ddlsemval = ddlSemYr.SelectedValue.ToString();
                                string ddlbranchval = ddlBranch.SelectedValue.ToString();

                                string tname = string.Empty;
                                if (ddltimetable.Text.ToString().Trim() != "")
                                {
                                    string ttnamee = ddltimetable.SelectedItem.Text;
                                    string[] spl = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                                    tname = spl[0];
                                }

                                string sections1 = string.Empty;
                                string strsec1 = string.Empty;

                                sections1 = ddlsecval.ToString().Trim();
                                if (sections1.ToString().Trim().ToLower() == "all" || sections1.ToString().Trim().ToLower() == string.Empty || sections1.ToString().Trim().ToLower() == "-1")
                                {
                                    strsec1 = string.Empty;
                                }
                                else
                                {
                                    strsec1 = " and sections='" + sections1.ToString().Trim() + "'";
                                }

                                string quer = "select * from Semester_Schedule where batch_year='" + ddlbatchval + "' and degree_code='" + ddlbranchval + "' and semester='" + ddlsemval + "' and TTName='" + tname + "' " + strsec1 + "";

                                DataSet dsenblebox = obi_access.select_method_wo_parameter(quer, "Text");
                                if (dsenblebox.Tables.Count > 0 && dsenblebox.Tables[0].Rows.Count > 0)
                                {
                                    for (int row = 0; row < gridTimeTable.Rows.Count; row++)
                                    {
                                        for (int col = 3; col < gview1tag.Columns.Count; col++)
                                        {
                                            string day = (gridTimeTable.Rows[row].FindControl("lblday") as Label).Text;
                                            string hor = (gridTimeTable.Rows[row].FindControl("lblhour") as Label).Text;
                                            string subNO = gview1tag.Columns[col].ColumnName;
                                            string colVal = day + hor;
                                            string cellVal = Convert.ToString(dsenblebox.Tables[0].Rows[0][colVal]);

                                            if (cellVal.Contains(subNO))
                                            {
                                                string txtbatch = string.Empty;
                                                string seldbatch = "select distinct stu_batch from laballoc where batch_year='" + ddlbatchvalue.ToString() + "' and Degree_code='" + ddlbranchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and  day_value='" + day.ToString() + "' and Hour_value='" + hor.ToString() + "' and subject_no='" + subNO.ToString() + "' and Timetablename='" + tname + "' " + strsec1 + "";
                                                DataSet dsbatch = obi_access.select_method_wo_parameter(seldbatch, "Text");
                                                TextBox txtBox = (gridTimeTable.Rows[row].FindControl("txtPeriod" + (col - 2)) as TextBox);
                                                txtBox.Enabled = true;
                                                CheckBoxList chklst = (gridTimeTable.Rows[row].FindControl("cblPeriod" + (col - 2)) as CheckBoxList);
                                                if (dsbatch.Tables.Count > 0 && dsbatch.Tables[0].Rows.Count > 0 && chklst.Items.Count > 0)
                                                {
                                                    int cun = 0;
                                                    for (int c = 0; c < dsbatch.Tables[0].Rows.Count; c++)
                                                    {
                                                        for (int cbl = 0; cbl < chklst.Items.Count; cbl++)
                                                        {
                                                            string bath = Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                                            if (chklst.Items[cbl].Text.Trim() == bath)
                                                            {
                                                                chklst.Items[cbl].Selected = true;
                                                                txtBox.Enabled = true;
                                                                cun++;
                                                                if (txtbatch == string.Empty)
                                                                {
                                                                    txtbatch = bath; //Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                                                }
                                                                else
                                                                {
                                                                    txtbatch = txtbatch + "," + bath; //txtbatch + "," + Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                                                }
                                                            }
                                                        }
                                                    }
                                                    txtBox.Text = txtbatch;
                                                }
                                            }
                                            else
                                            {
                                                TextBox txtBox = (gridTimeTable.Rows[row].FindControl("txtPeriod" + (col - 2)) as TextBox);
                                                txtBox.Enabled = false;
                                            }
                                        }
                                    }
                                }
                                //}
                                for (int ddl = 3; ddl < 13; ddl++)
                                {
                                    gridTimeTable.Columns[ddl].Visible = false;
                                }
                                for (int ddl = 0; ddl <= gview1dtable.Columns.Count; ddl++)
                                {
                                    string str = gridTimeTable.Columns[ddl].ToString();
                                    gridTimeTable.Columns[ddl].Visible = true;
                                }
                            }
                            else
                            {
                                lblerror.Visible = true;
                                gview.Visible = false;
                                gridTimeTable.Visible = false;
                                Fieldset2.Visible = false;
                                Fieldset4.Visible = false;
                                Button3.Visible = false;
                                LinkButton1.Visible = false;
                                Button3.Visible = false;
                                Fieldset5.Visible = false;

                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                btnExcel.Visible = false;
                                btnprintmaster.Visible = false;
                                Fieldset1.Visible = false;
                                Fieldset3.Visible = false;
                                lblerror.Text = "There Is No Lab Subject For This TimeTable";
                                lblerror.Visible = true;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            gview.Visible = false;

                            gridTimeTable.Visible = false;
                            Fieldset2.Visible = false;
                            Fieldset4.Visible = false;
                            Button3.Visible = false;
                            LinkButton1.Visible = false;
                            Button3.Visible = false;
                            Fieldset5.Visible = false;

                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnExcel.Visible = false;
                            btnprintmaster.Visible = false;
                            Fieldset1.Visible = false;
                            Fieldset3.Visible = false;
                            lblerror.Text = "There Is No Lab Subject For This TimeTable";
                            lblerror.Visible = true;
                        }
                    }
                }
                else
                {
                    lblerror.Visible = true;

                    gview.Visible = false;

                    gridTimeTable.Visible = false;
                    Fieldset2.Visible = false;
                    Fieldset4.Visible = false;
                    Button3.Visible = false;
                    LinkButton1.Visible = false;
                    Button3.Visible = false;
                    Fieldset5.Visible = false;
                    lblerror.Text = "Students Not Available In This Semester";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void gridTimeTable_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DataTable dtable1 = (DataTable)Session["dtable1"];
            for (int cell = 3; cell <= dtable1.Columns.Count; cell++)
            {
                e.Row.Cells[cell].Text = Convert.ToString(dtable1.Columns[cell - 1].ColumnName);
            }
        }
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void txtbatch_TextChanged(object sender, EventArgs e)
    {
        DataTable gviewdt = (DataTable)Session["tabletag"];
        string ddlcollegevalu = ddlcollege.SelectedValue.ToString();
        string ddlbatchvalu = ddlBatch.SelectedValue.ToString();
        string ddldegreevalu = ddlDegree.SelectedValue.ToString();
        string ddlsecvalu = string.Empty;
        if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
            ddlsecvalu = ddlSec.SelectedValue.ToString();
        string ddlsemvalu = ddlSemYr.SelectedValue.ToString();
        string ddlbranchvalu = ddlBranch.SelectedValue.ToString();

        lblerror.Visible = false;//Added By Srinath 17/8/2013
        ddlnobatches.Items.Clear();
        string numbatch = string.Empty;
        int b_val = 0;
        numbatch = txtbatch.Text.ToString();
        if (numbatch != "" && numbatch != "0")
        {
            string selectedbat = "select * from subjectchooser where semester='" + ddlsemvalu + "' and subject_no in(Select subject.subject_no from subject,sub_sem,syllabus_master sy where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code=sy.syll_code and sy.Batch_Year='" + ddlbatchvalu + "' and sy.degree_code='" + ddlbranchvalu + "' and sy.semester='" + ddlsemvalu + "') and batch is not null and batch<>''";
            DataSet ds_selebat = new DataSet();
            ds_selebat = obi_access.select_method_wo_parameter(selectedbat, "text");

            DataTable dat = new DataTable(); dat.Columns.Add("batch");
            DataRow drow = null;
            ddlnobatches.Items.Insert(0, new ListItem("--Select--", "-1"));
            drow = dat.NewRow();
            drow["batch"] = string.Empty;
            dat.Rows.Add(drow);
            for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
            {
                ddlnobatches.Items.Add("B" + b_val.ToString());
                drow = dat.NewRow();
                drow["batch"] = "B" + b_val.ToString();
                dat.Rows.Add(drow);

            }
            if (gview.Rows.Count > 0)
            {
                for (int row = 0; row < gview.Rows.Count; row++)
                {
                    DropDownList ddlst = (gview.Rows[row].FindControl("ddlbatch") as DropDownList);
                    ddlst.DataSource = dat;
                    ddlst.DataTextField = "batch";
                    ddlst.DataValueField = "batch";
                    ddlst.DataBind();

                    string roll = (gview.Rows[row].FindControl("lblroll") as Label).Text;
                    DataView dvOpen = new DataView();
                    ds_selebat.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "'";
                    dvOpen = ds_selebat.Tables[0].DefaultView;

                    if (dvOpen.Count > 0)
                    {
                        string bat = Convert.ToString(dvOpen[0]["Batch"]);
                        DropDownList ddl = (gview.Rows[row].FindControl("ddlbatch") as DropDownList);
                        (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                        gview.Rows[row].Enabled = true;
                        if (ddl.SelectedItem != null)
                        {
                            if (ddl.SelectedItem.Text != bat)
                            {
                                ddl.Items.Insert(ddl.Items.Count, bat);
                                (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                            }
                        }
                    }
                }
            }
            if (gridTimeTable.Rows.Count > 0)
            {
                DataTable data = new DataTable(); data.Columns.Add("batch");
                DataRow droww = null;
                if (ddlnobatches.Items.Count > 1)
                {
                    for (int i = 1; i < ddlnobatches.Items.Count; i++)
                    {
                        droww = data.NewRow();
                        droww["batch"] = ddlnobatches.Items[i].Text;
                        data.Rows.Add(droww);
                    }
                    for (int row = 0; row < gridTimeTable.Rows.Count; row++)
                    {
                        for (int cell = 1; cell < gridTimeTable.Rows[row].Cells.Count - 2; cell++)
                        {
                            (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataSource = data;
                            (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataTextField = "batch";
                            (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataValueField = "batch";
                            (gridTimeTable.Rows[row].FindControl("cblPeriod" + cell) as CheckBoxList).DataBind();
                        }
                    }
                }
            }
            string ddlbatch = ddlBatch.SelectedValue.ToString();
            string ddlsec = string.Empty;
            if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                ddlsec = ddlSec.SelectedValue.ToString();
            string ddlsem = ddlSemYr.SelectedValue.ToString();
            string ddlbranch = ddlBranch.SelectedValue.ToString();
            string tname = string.Empty;
            if (ddltimetable.Text.ToString().Trim() != "")
            {
                string ttnamee = ddltimetable.SelectedItem.Text;
                string[] spl = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                tname = spl[0];
            }
            string sections1 = string.Empty;
            string strsec1 = string.Empty;

            sections1 = ddlsec;
            if (sections1.ToString().Trim().ToLower() == "all" || sections1.ToString().Trim().ToLower() == string.Empty || sections1.ToString().Trim().ToLower() == "-1")
            {
                strsec1 = string.Empty;
            }
            else
            {
                strsec1 = " and sections='" + sections1.ToString().Trim() + "'";
            }

            string quer = "select * from Semester_Schedule where batch_year='" + ddlbatch + "' and degree_code='" + ddlbranch + "' and semester='" + ddlsem + "' and TTName='" + tname + "' " + strsec1 + "";

            DataSet dsenblebox = obi_access.select_method_wo_parameter(quer, "Text");
            if (dsenblebox.Tables.Count > 0 && dsenblebox.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < gridTimeTable.Rows.Count; row++)
                {
                    for (int col = 3; col < gviewdt.Columns.Count; col++)
                    {
                        string day = (gridTimeTable.Rows[row].FindControl("lblday") as Label).Text;
                        string hor = (gridTimeTable.Rows[row].FindControl("lblhour") as Label).Text;
                        string subNO = gviewdt.Columns[col].ColumnName;
                        string colVal = day + hor;
                        string cellVal = Convert.ToString(dsenblebox.Tables[0].Rows[0][colVal]);

                        if (cellVal.Contains(subNO))
                        {
                            string txtbatch1 = string.Empty;
                            string seldbatch = "select distinct stu_batch from laballoc where batch_year='" + ddlbatch.ToString() + "' and Degree_code='" + ddlbranch.ToString() + "' and semester='" + ddlsem.ToString() + "' and  day_value='" + day.ToString() + "' and Hour_value='" + hor.ToString() + "' and subject_no='" + subNO.ToString() + "' and Timetablename='" + tname + "' " + strsec1 + "";
                            DataSet dsbatch = obi_access.select_method_wo_parameter(seldbatch, "Text");
                            TextBox txtBox = (gridTimeTable.Rows[row].FindControl("txtPeriod" + (col - 2)) as TextBox);
                            txtBox.Enabled = true;
                            CheckBoxList chklst = (gridTimeTable.Rows[row].FindControl("cblPeriod" + (col - 2)) as CheckBoxList);
                            if (dsbatch.Tables.Count > 0 && dsbatch.Tables[0].Rows.Count > 0 && chklst.Items.Count > 0)
                            {
                                int cun = 0;
                                for (int c = 0; c < dsbatch.Tables[0].Rows.Count; c++)
                                {
                                    for (int cbl = 0; cbl < chklst.Items.Count; cbl++)
                                    {
                                        string bath = Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                        if (chklst.Items[cbl].Text.Trim() == bath)
                                        {
                                            chklst.Items[cbl].Selected = true;
                                            txtBox.Enabled = true;
                                            cun++;
                                            if (txtbatch1 == string.Empty)
                                            {
                                                txtbatch1 = bath;// Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                            }
                                            else
                                            {
                                                txtbatch1 = txtbatch1 + "," + bath;// Convert.ToString(dsbatch.Tables[0].Rows[c]["stu_batch"]).Trim();
                                            }
                                        }
                                    }
                                }
                                txtBox.Text = txtbatch1;
                            }
                        }
                        else
                        {
                            TextBox txtBox = (gridTimeTable.Rows[row].FindControl("txtPeriod" + (col - 2)) as TextBox);
                            txtBox.Enabled = false;
                        }
                    }
                }
            }

        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Select Number of Batch";
        }
    }

    protected void ddlnobatches_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        selectedbatch = ddlnobatches.SelectedItem.ToString();
        //Btnsave.Enabled = false;
        //Btndelete.Enabled = false;
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dttable = (DataTable)Session["tabletag"];
            flagbatch = false;
            lblerror.Visible = false;
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();
            string allsubject = string.Empty;
            for (int l = 3; l < dttable.Columns.Count; l++)
            {
                if (allsubject == "")
                {
                    allsubject = dttable.Columns[l].ColumnName;
                }
                else
                {
                    allsubject = allsubject + ',' + dttable.Columns[l].ColumnName;
                }
            }
            for (int stdcount = 0; stdcount < gview.Rows.Count; stdcount++)
            {
                DropDownList ddll = (gview.Rows[stdcount].FindControl("ddlbatch") as DropDownList);
                selectedbatch = ddll.SelectedItem.Text;
                rollno = (gview.Rows[stdcount].FindControl("lblroll") as Label).Text;
                DataSet ds_subjectno = new DataSet();
                string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and semester = '" + ddlsemvalue.ToString() + "' and subjectchooser.subject_no in(" + allsubject + ")  and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                ds_subjectno = obi_access.select_method_wo_parameter(batchsql, "Text");
                if (ds_subjectno.Tables[0].Rows.Count > 0)
                {
                    for (int subno = 0; subno < ds_subjectno.Tables[0].Rows.Count; subno++)
                    {
                        string ssub_no = ds_subjectno.Tables[0].Rows[subno]["subject_no"].ToString();
                        string paper_order = ds_subjectno.Tables[0].Rows[subno]["paper_order"].ToString();
                        string subtype = ds_subjectno.Tables[0].Rows[subno]["subtype_no"].ToString();
                        string updatquery = "update subjectchooser set batch ='" + selectedbatch + "' where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "'";
                        int u = obi_access.update_method_wo_parameter(updatquery, "Text");
                        flagbatch = true;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void Btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            int cnt = 0;
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();

            string from = fromno.Text;
            string to = tono.Text;
            lblerror.Visible = false;
            string batch = string.Empty;

            if (gview.Rows.Count > 0)
            {
                for (int rowcount = 0; rowcount < gview.Rows.Count; rowcount++)
                {
                    cnt++;
                    DropDownList ddl = (gview.Rows[rowcount].FindControl("ddlbatch") as DropDownList);
                    rollno = (gview.Rows[rowcount].FindControl("lblroll") as Label).Text;
                    string deletbatch = "update subjectchooser set batch ='' where roll_no='" + rollno + "' and semester='" + ddlsemvalue.ToString() + "' ";
                    int d = obi_access.update_method_wo_parameter(deletbatch, "Text");
                    (gview.Rows[rowcount].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[rowcount].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[rowcount].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "There is no student to delete batch";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void Batchallotsave_Click(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        flagbatch = false;//added By Srinath 17/8/2013
        Btnsave_Click(sender, e);
        int insert = 0;
        try
        {
            ddlcollegevalue = ddlcollege.SelectedValue.ToString();
            ddlbatchvalue = ddlBatch.SelectedValue.ToString();
            ddldegreevalue = ddlDegree.SelectedValue.ToString();
            if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                ddlsecvalue = ddlSec.SelectedValue.ToString();
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();
            ddlbranchvalue = ddlBranch.SelectedValue.ToString();
            string sections = string.Empty;
            string strsec = string.Empty;

            sections = ddlsecvalue;
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            DataTable dtabletag = (DataTable)Session["tabletag"];
            //Modified by rajkumar 11-12-2018
            string date1 = ddltimetable.SelectedItem.ToString();
            string[] date_fm = date1.Split(new Char[] { '@' });
            string selecteddate = date_fm[0];
            string deletequery = "delete from laballoc where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "'  and sections='" + ddlsecvalue.ToString() + "' and Timetablename='" + selecteddate.ToString() + "'";

            insert = obi_access.update_method_wo_parameter(deletequery, "Text");

            for (int batchrowcount = 0; batchrowcount < gridTimeTable.Rows.Count; batchrowcount++)
            {
                string fpday = (gridTimeTable.Rows[batchrowcount].FindControl("lblday") as Label).Text;
                string fphour = (gridTimeTable.Rows[batchrowcount].FindControl("lblhour") as Label).Text;

                string[] date_fm1 = date_fm[1].Split(new Char[] { '/' });

                string fmdate = date_fm1[2].ToString() + "/" + date_fm1[1].ToString() + "/" + date_fm1[0].ToString();


                for (int batchcolcount = 3; batchcolcount < dtabletag.Columns.Count; batchcolcount++)// gridTimeTable.Columns.Count
                {
                    if (gridTimeTable.Columns[batchcolcount].Visible == true)
                    {
                        TextBox textbx = (gridTimeTable.Rows[batchrowcount].FindControl("txtPeriod" + (batchcolcount - 2)) as TextBox);
                        if (textbx.Text != "--Select--" && textbx.Enabled == true)
                        {
                            string fpsubno = string.Empty;
                            string batchname = string.Empty;
                            string batchname1 = string.Empty;
                            //added by sasi 
                            CheckBoxList chkllist = (gridTimeTable.Rows[batchrowcount].FindControl("cblPeriod" + (batchcolcount - 2)) as CheckBoxList);

                            for (int val = 0; val < chkllist.Items.Count; val++)
                            {
                                if (chkllist.Items[val].Selected)
                                {
                                    if (batchname == "")
                                    {
                                        batchname = chkllist.Items[val].Text.Trim();
                                    }
                                    else
                                    {
                                        batchname = batchname + "," + chkllist.Items[val].Text.Trim();
                                    }
                                }
                            }
                            if (batchname != "")
                            {
                                string[] setbatch = batchname.Split(',');
                                for (int index = 0; index <= setbatch.GetUpperBound(0); index++)
                                {
                                    string setbatchname = setbatch[index].ToString();
                                    //------end---
                                    fpsubno = dtabletag.Columns[batchcolcount].ColumnName;
                                    for (int batchsubcolcount = batchcolcount; batchsubcolcount < dtabletag.Columns.Count; batchsubcolcount++)
                                    {
                                        if (batchsubcolcount != batchcolcount)
                                        {
                                            string selectedbatcname1 = string.Empty;
                                            CheckBoxList chkllist1 = (gridTimeTable.Rows[batchrowcount].FindControl("cblPeriod" + (batchsubcolcount - 2)) as CheckBoxList);
                                            for (int val = 0; val < chkllist1.Items.Count; val++)
                                            {
                                                if (chkllist1.Items[val].Selected)
                                                {
                                                    if (selectedbatcname1 == "")
                                                    {
                                                        selectedbatcname1 = chkllist1.Items[val].Text.Trim();
                                                    }
                                                    else
                                                    {
                                                        selectedbatcname1 = selectedbatcname1 + "," + chkllist1.Items[val].Text.Trim();
                                                    }
                                                }
                                            }
                                            if (batchname == selectedbatcname1)
                                            {
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student cannot select the same subjects more than once')", true);
                                                continue;
                                                //return;
                                            }
                                            else
                                            {
                                                goto l1;
                                            }
                                        }
                                    }
                                l1: string insertcmd = "insert into laballoc(Degree_code,batch_year,semester,day_value,Hour_value,sections,stu_batch,subject_no,Timetablename,Fromdate)values('" + ddlbranchvalue.ToString() + "','" + ddlbatchvalue.ToString() + "','" + ddlsemvalue + "','" + fpday + "','" + fphour + "','" + ddlsecvalue.ToString() + "','" + setbatchname.ToString() + "','" + fpsubno.ToString() + "','" + selecteddate + "','" + fmdate.ToString() + "')";
                                    insert = obi_access.update_method_wo_parameter(insertcmd, "Text");
                                    flagbatch = true;
                                }
                            }
                            //----end-------
                        }
                    }
                }
            }
            if (flagbatch == true)
            {
                btnGo_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('successfully saved')", true);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    private object GetCorrespondingKey(string p, Hashtable htsubjectno)
    {
        throw new NotImplementedException();
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
        if (CheckBox1.Checked)
        {
            this.fromno.Visible = true;
            this.tono.Visible = true;
            this.lblfrom.Visible = true;
            this.lblto.Visible = true;
            this.Button2.Visible = true;
        }
        else
        {
            this.fromno.Visible = false;
            this.tono.Visible = false;
            this.Button2.Visible = false;
            this.lblfrom.Visible = false;
            this.lblto.Visible = false;
        }
    }

    protected void selectgo_Click(object sender, EventArgs e)
    {
        string from = fromno.Text;
        string to = tono.Text;
        lblerror.Visible = false;
        string batch = string.Empty;

        if (ddlnobatches.Text != "Select" && ddlnobatches.Text != "-1" && ddlnobatches.Text != "")
        {
            if (from != null && from != "" && to != null && to != "")
            {
                int m = Convert.ToInt32(fromno.Text);
                int n = Convert.ToInt32(tono.Text);
                if (m != 0 && n != 0)
                {
                    if (gview.Rows.Count >= n)
                    {
                        batch = ddlnobatches.Text;
                        for (int rowcount = m; rowcount <= n; rowcount++)
                        {
                            if (txtbatch.Text != "" && txtbatch.Text != "0" && txtbatch.Text != null && ddlnobatches.SelectedItem.ToString() != null && ddlnobatches.SelectedItem.ToString() != "" && ddlnobatches.SelectedItem.ToString() != "--Select--")
                            {
                                DropDownList ddl = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList);
                                (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.FindByValue(ddlnobatches.Text));

                                //added by srinath 31/8/2013
                                Btnsave.Enabled = false;
                                Btndelete.Enabled = true;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "Please Add No of Batch";
                            }
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter Available Student Count";
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter Greater than Zero";
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter Values";
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Select Batch";
        }
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
    }

    protected void Checkboxlistbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
        {
            if (Checkboxlistbatch.Items[i].Selected == true)
            {
                value = Checkboxlistbatch.Items[i].Text;
                code = Checkboxlistbatch.Items[i].Value.ToString();
            }
        }
    }

    protected void Batchallot_spread_SelectedIndexChanged(Object sender, EventArgs e)
    {
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string value = string.Empty;
        string code = string.Empty;
        //string[] strcomo = new string[20];
        //int j = 0;

        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
        {
            if (Checkboxlistbatch.Items[i].Selected == true)
            {
                value = Checkboxlistbatch.Items[i].Text;
                code = Checkboxlistbatch.Items[i].Value.ToString();
                if (batchva == "")
                {
                    batchva = value;
                }
                else
                {
                    batchva = batchva + ',' + value;
                }
            }
            //strcomo[j++] = Checkboxlistbatch.Items[i].Text;
        }
        //strcomo[j++] = string.Empty;
        //int ar = 0;
        //int ac = 0;
        //ar = Batchallot_spread.ActiveSheetView.ActiveRow;
        //ac = Batchallot_spread.ActiveSheetView.ActiveColumn;
        //if (ac > 1)
        //{
        //    if (Batchallot_spread.Sheets[0].Cells[ar, ac].BackColor == Color.CornflowerBlue)
        //    {
        //        FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
        //        Batchallot_spread.Sheets[0].Cells[ar, ac].CellType = btva;
        //        Batchallot_spread.Sheets[0].Cells[ar, ac].Text = batchva;
        //        Batchallot_spread.Sheets[0].Cells[ar, ac].Locked = true;
        //        Checkboxlistbatch.Visible = false;
        //    }
        //}
        Button3.Visible = false;
        Fieldset5.Visible = false;
        //  Batchallot_spread.SaveChanges();
        //   Batchallot_spread.Sheets[0].AutoPostBack = true;
    }

    public string Getdayorder(string strday_value)
    {
        dayorder = string.Empty;
        if (strday_value == "Mon")
        {
            dayorder = "Day 1";
        }
        else if (strday_value == "Tue")
        {
            dayorder = "Day 2";
        }
        else if (strday_value == "Wed")
        {
            dayorder = "Day 3";
        }
        else if (strday_value == "Thu")
        {
            dayorder = "Day 4";
        }
        else if (strday_value == "Fri")
        {
            dayorder = "Day 5";
        }
        else if (strday_value == "Sat")
        {
            dayorder = "Day 6";
        }
        else if (strday_value == "Sun")
        {
            dayorder = "Day 7";
        }
        return dayorder;
    }

    public void loadautoswich()
    {
        try
        {
            ddlcollegevalue = ddlcollege.SelectedValue.ToString();
            ddlbatchvalue = ddlBatch.SelectedValue.ToString();
            ddldegreevalue = ddlDegree.SelectedValue.ToString();
            if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                ddlsecvalue = ddlSec.SelectedValue.ToString();
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();
            ddlbranchvalue = ddlBranch.SelectedValue.ToString();
            string timetablename = string.Empty;
            string sections = string.Empty;
            if (ddltimetable.Text.ToString().Trim() != "")
            {
                string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                timetablename = " and timetablename='" + ttname[0] + "'";
            }
            sections = ddlsecvalue;
            if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = " and sections='" + sections.ToString().Trim() + "'";
            }
            Hashtable hatautoswitch = new Hashtable();
            string getautoswitchsub = "select distinct Day_Value,Hour_Value from LabAlloc where Batch_Year='" + ddlbatchvalue + "' and Degree_Code='" + ddlbranchvalue + "' and Semester='" + ddlsemvalue + "' " + sections + " and ISNULL(auto_switch,'')<>''  " + timetablename + "";
            DataSet dsautoswitch = obi_access.select_method_wo_parameter(getautoswitchsub, "Text");
            if (dsautoswitch.Tables.Count > 0 && dsautoswitch.Tables[0].Rows.Count > 0)
            {
                for (int af = 0; af < dsautoswitch.Tables[0].Rows.Count; af++)
                {
                    string setval = dsautoswitch.Tables[0].Rows[af]["Day_Value"].ToString() + '/' + dsautoswitch.Tables[0].Rows[af]["Hour_Value"].ToString();
                    if (!hatautoswitch.Contains(setval))
                    {
                        hatautoswitch.Add(setval, setval);
                    }
                }
            }
            Fieldset7.Visible = true;
            chklsautoswitch.Items.Clear();
            txtautoswitch.Text = "---Select---";
            chkswitch.Checked = false;
            bool getswitchlab = false;
            int icou = 0;
            bool automatcilab = false;
            DataTable dttable = (DataTable)Session["tabletag"];
            for (int i = 0; i < gridTimeTable.Rows.Count; i++)
            {
                string value = (gridTimeTable.Rows[i].FindControl("lblday") as Label).Text + '/' + (gridTimeTable.Rows[i].FindControl("lblhour") as Label).Text;
                string subno = string.Empty;
                getswitchlab = false;
                for (int su = 3; su < dttable.Columns.Count; su++)
                {
                    if (subno == "")
                    {
                        subno = dttable.Columns[su].ColumnName;
                    }
                    else
                    {
                        getswitchlab = true;
                        subno = subno + ',' + dttable.Columns[su].ColumnName;
                    }
                }
                if (getswitchlab == true)
                {
                    automatcilab = true;
                    chklsautoswitch.Items.Insert(icou, new System.Web.UI.WebControls.ListItem(value, subno));
                    if (hatautoswitch.Contains(value))
                    {
                        chklsautoswitch.Items[chklsautoswitch.Items.Count - 1].Selected = true;
                    }
                    else
                    {
                        chklsautoswitch.Items[chklsautoswitch.Items.Count - 1].Selected = false;
                    }
                    icou++;
                }
            }
            if (automatcilab == true)
            {
                Fieldset7.Visible = true;
                int t = 0;
                for (int ch = 0; ch < chklsautoswitch.Items.Count; ch++)
                {
                    if (chklsautoswitch.Items[ch].Selected == true)
                    {
                        t++;
                    }
                }
                if (t > 0)
                {
                    txtautoswitch.Text = "Items (" + t + ")";
                    if (t == chklsautoswitch.Items.Count)
                    {
                        chkswitch.Checked = true;
                    }
                }
            }
            else
            {
                Fieldset7.Visible = false;
                lblerror.Text = "No Items to Automatic Switch Lab";
                lblerror.Visible = true;
                chkautoswitch.Checked = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void chkautoswitch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkautoswitch.Checked == true)
        {
            Fieldset7.Visible = true;
            loadautoswich();
        }
        else
        {
            Fieldset7.Visible = false;
        }
    }

    protected void chkswitch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkswitch.Checked == true)
        {
            for (int i = 0; i < chklsautoswitch.Items.Count; i++)
            {
                chklsautoswitch.Items[i].Selected = true;
            }
            txtautoswitch.Text = "Items (" + chklsautoswitch.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsautoswitch.Items.Count; i++)
            {
                chklsautoswitch.Items[i].Selected = false;
            }
            txtautoswitch.Text = "---Select---";
        }
    }

    protected void chklsautoswitch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int coun = 0;
        for (int i = 0; i < chklsautoswitch.Items.Count; i++)
        {
            if (chklsautoswitch.Items[i].Selected == true)
            {
                coun++;
            }
        }
        chkswitch.Checked = false;
        if (coun > 0)
        {
            if (coun == chklsautoswitch.Items.Count)
            {
                chkswitch.Checked = true;
            }
            txtautoswitch.Text = "Items (" + coun + ")";
        }
        else
        {
            txtautoswitch.Text = "---Select---";
        }
    }

    protected void btnautoswitch_Click(object sender, EventArgs e)
    {
        try
        {
            ddlcollegevalue = ddlcollege.SelectedValue.ToString();
            ddlbatchvalue = ddlBatch.SelectedValue.ToString();
            ddldegreevalue = ddlDegree.SelectedValue.ToString();
            if (ddlSec.SelectedValue != null && ddlSec.Enabled == true)
                ddlsecvalue = ddlSec.SelectedValue.ToString();
            ddlsemvalue = ddlSemYr.SelectedValue.ToString();
            ddlbranchvalue = ddlBranch.SelectedValue.ToString();
            string date1 = ddltimetable.SelectedItem.ToString();
            string[] date_fm = date1.Split(new Char[] { '@' });
            string selecteddate = date_fm[0];
            string sections = ddlsecvalue;
            if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = " and sections='" + sections.ToString().Trim() + "'";
            }
            bool saveflag = false;
            int set = obi_access.update_method_wo_parameter("update LabAlloc set Auto_Switch='' where Degree_Code='" + ddlbranchvalue.ToString() + "' and Batch_Year='" + ddlbatchvalue.ToString() + "' and Semester='" + ddlsemvalue.ToString() + "' " + sections + " and Timetablename='" + selecteddate + "' ", "Text");
            if (set > 0)
            {
                for (int i = 0; i < chklsautoswitch.Items.Count; i++)
                {
                    string strdayhour = chklsautoswitch.Items[i].Text.ToString();
                    string subno = chklsautoswitch.Items[i].Value.ToString();
                    string[] stg = strdayhour.Split('/');
                    if (stg.GetUpperBound(0) == 1)
                    {
                        if (chklsautoswitch.Items[i].Selected == true)
                        {
                            string[] spsu = subno.Split(',');
                            if (spsu.GetUpperBound(0) > 0)
                            {
                                set = obi_access.update_method_wo_parameter("update LabAlloc set Auto_Switch='" + subno + "' where Degree_Code='" + ddlbranchvalue.ToString() + "' and Batch_Year='" + ddlbatchvalue.ToString() + "' and Semester='" + ddlsemvalue.ToString() + "' " + sections + " and Timetablename='" + selecteddate + "' and Day_Value='" + stg[0].ToString() + "' and Hour_Value='" + stg[1].ToString() + "'", "Text");
                                saveflag = true;
                            }
                        }
                    }
                }
            }
            else
            {
                lblerror.Text = "Please Save Batch Allocation Before Automatic Batch Switch";
                lblerror.Visible = true;
            }
            if (saveflag == false)
            {
                lblerror.Text = "Please Select The Items And Then Proceed.";
                lblerror.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Automatic Batch Switch is Saved successfully')", true);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void chkPeriod_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ddlLabTest = (CheckBox)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("chkPeriod", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chkPeriod" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("cblPeriod" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txtPeriod" + colIndx);
            CallCheckboxChange(ddlAddLabTestShortName, cbl, txtB, "Batch", "--Select--");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBoxList ddlLabTest = (CheckBoxList)sender;
            var row = ddlLabTest.NamingContainer;
            string rowIndxS = ddlLabTest.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = ddlLabTest.UniqueID.ToString().Split('$')[4].Replace("cblPeriod", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);
            CheckBox ddlAddLabTestShortName = (CheckBox)row.FindControl("chkPeriod" + colIndx);
            CheckBoxList cbl = (CheckBoxList)row.FindControl("cblPeriod" + colIndx);
            TextBox txtB = (TextBox)row.FindControl("txtPeriod" + colIndx);
            CallCheckboxListChange(ddlAddLabTestShortName, cbl, txtB, "Batch", "--Select--");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Batch Allocation"); }
    }

    protected void gview_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                e.Row.Cells[0].Text = "Select";
            }
        }
    }

}