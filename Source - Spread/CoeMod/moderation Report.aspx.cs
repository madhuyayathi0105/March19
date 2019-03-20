using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class moderation_Report : System.Web.UI.Page
{
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string group_code = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataRow dr = null;
    Hashtable hast = new Hashtable();
    DataTable dt = new DataTable();

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
            lblmsg.Visible = false;
            if (!IsPostBack)
            {
                lblPopAlertMsg.Text = string.Empty;
                divPopupAlert.Visible = false;
                chkShowNoteDescription.Checked = true;
                chkSubjectNameWithSubjectCode.Checked = false;
                chkReportWithStream.Checked = false;
                txtReportName.Text = string.Empty;
                txtCollegeHeader.Text = string.Empty;
                lblrptname.Visible = false;
                checkregular.Checked = true;
                FpSpread2.Visible = false;
                Printcontrol.Visible = false;
                bindcollege();
                bindbatch();
                binddegree();
                binddept();
                bindsem();
                bindMonthandYear();
                bindSubject();
            }
        }
        catch(Exception ex){
        }
    }

    protected void log_OnClick(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if (Session["single_user"] != null && (group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hast.Clear();
            hast.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hast, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = false;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            int count = 0;
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "batch_year";
                    ddlbatch.DataValueField = "batch_year";
                    ddlbatch.DataBind();

                }
            }
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                int count1 = ds.Tables[1].Rows.Count;
                if (count > 0)
                {
                    int max_bat = 0;
                    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                    ddlbatch.SelectedValue = max_bat.ToString();

                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = false;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void binddegree()
    {
        try
        {
            ds.Clear();
            ds = da.BindDegree(Session["single_user"].ToString(), Session["group_code"].ToString(), ddlcollege.SelectedValue.ToString(), Session["usercode"].ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = false;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void binddept()
    {
        try
        {
            ddldept.Items.Clear();
            ds.Clear();
            hast.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hast.Add("single_user", singleuser);
            hast.Add("group_code", group_user);
            hast.Add("course_id", ddldegree.SelectedValue);
            hast.Add("college_code", collegecode);
            hast.Add("user_code", usercode);
            ds = da.select_method("bind_branch", hast, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "degree_code";
                    ddldept.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = false;
            lblmsg.Text = ex.ToString();
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
            string strsemquery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddldept.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
            DataSet dssem = da.select_method_wo_parameter(strsemquery, "Text");
            if (dssem.Tables.Count > 0 && dssem.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
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
            else
            {
                dssem.Clear();
                strsemquery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddldept.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "";
                dssem = da.select_method_wo_parameter(strsemquery, "Text");
                if (dssem.Tables.Count > 0 && dssem.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
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
        }
        catch (Exception ex)
        {
            lblmsg.Visible = false;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindSubject()
    {
        //string notInUse = "SELECT distinct ed.current_semester,s.subject_code,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sm  where sm.Degree_code=ed.degree_code and ed.batch_year=sm.Batch_Year and sm.syll_code=s.syll_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='48' and ed.batch_year = '2015' and ed.current_semester='3' and  ed.Exam_Month='11' and ed.Exam_year='2016' -- and ead.attempts=0 and sm.semester=3 order by ed.current_semester,ead.subject_no";

        //notInUse = "SELECT distinct ed.current_semester,s.subject_code,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sm  where sm.Degree_code=ed.degree_code and ed.batch_year=sm.Batch_Year and sm.syll_code=s.syll_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='48' and ed.batch_year = '2015' and ed.current_semester='3' and  ed.Exam_Month='11' and ed.Exam_year='2016' -- and ead.attempts=0 and sm.semester<>3 order by ed.current_semester,ead.subject_no"; 
        chklssubject.Items.Clear();
        chksubject.Checked = false;
        txtsubject.Text = "---Select---";
        string sql = string.Empty;
        string sem = ddlsem.SelectedItem.ToString();
        string degcode = ddldept.SelectedItem.Value;

        string arrer_regular = string.Empty;
        string semesterCurrentArrear = string.Empty;
        if (checkregular.Checked == true && Checkarrear.Checked == true)
        {
            arrer_regular = string.Empty;
            semesterCurrentArrear = string.Empty;
        }
        else if (Checkarrear.Checked == true)
        {
            arrer_regular = " and ead.attempts>0";
            semesterCurrentArrear = " and sm.semester<>'" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
        }
        else if (checkregular.Checked == true)
        {
            arrer_regular = " and ead.attempts=0";
            semesterCurrentArrear = " and sm.semester='" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
        }
        ds.Clear();
        if (ddlmonth.Items.Count > 0 && ddlyear.Items.Count > 0)
        {
            //sql = "SELECT Subject_No,Subject_Code,Subject_Name FROM Subject S,Syllabus_Master Y,Exam_Details D where s.syll_code = y.syll_code and y.degree_code = d.degree_code and y.Batch_Year = d.batch_year and y.semester = d.current_semester and d.degree_code ='" + ddldept.SelectedValue + "' and d.batch_year = '" + ddlbatch.SelectedValue + "' and d.current_semester = '" + ddlsem.SelectedValue + "' and d.exam_code = (select exam_code from Exam_Details where degree_code = '" + ddldept.SelectedValue + "' and batch_year ='" + ddlbatch.SelectedValue + "'  and current_semester = '" + ddlsem.SelectedValue + "')";
            //sql = "SELECT distinct ed.current_semester,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='" + ddldept.SelectedValue + "'  and ed.batch_year = '" + ddlbatch.SelectedValue + "' and ed.current_semester='" + ddlsem.SelectedValue + "' and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' " + arrer_regular + " order by ed.current_semester,ead.subject_no ";
            //sql = "SELECT distinct ed.current_semester,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='" + ddldept.SelectedValue + "'  and ed.batch_year = '" + ddlbatch.SelectedValue + "' and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' " + semesterCurrentArrear + " order by ed.current_semester,ead.subject_no ";

            sql = "SELECT distinct ed.current_semester,s.subject_code,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s,syllabus_master sm  where sm.Degree_code=ed.degree_code and ed.batch_year=sm.Batch_Year and sm.syll_code=s.syll_code and ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='" + ddldept.SelectedValue + "' and ed.batch_year = '" + ddlbatch.SelectedValue + "' and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' " + semesterCurrentArrear + " order by ed.current_semester,ead.subject_no";
            ds = da.select_method_wo_parameter(sql, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklssubject.DataSource = ds;
                    chklssubject.DataTextField = "subject_name";
                    chklssubject.DataValueField = "subject_no";
                    chklssubject.DataBind();
                }
            }
        }
    }

    public void bindMonthandYear()
    {
        try
        {
            ddlmonth.Items.Clear();
            ddlyear.Items.Clear();

            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            for (int l = 0; l <= 7; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year - l));
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
        bindMonthandYear();
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
        bindsem();
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlsem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread2.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblmsg.Visible = false;
        lblrptname.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void checkregular_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            bindSubject();
            FpSpread2.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblmsg.Visible = false;
            lblrptname.Visible = false;
            Printcontrol.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void Checkarrear_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            bindSubject();
            FpSpread2.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblmsg.Visible = false;
            lblrptname.Visible = false;
            Printcontrol.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            divPopupAlert.Visible = false;
            lblPopAlertMsg.Text = string.Empty;

            if (ddlcollege.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No College Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlbatch.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Batch Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddldegree.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Degree Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddldept.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Branch Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlsem.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Semester Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (chklssubject.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Subjects Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlmonth.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Exam Month Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlyear.Items.Count == 0)
            {
                lblPopAlertMsg.Text = "No Exam Year Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            DataSet ds1 = new DataSet();
            string batchyr = ddlbatch.SelectedItem.ToString();
            string sem = ddlsem.SelectedItem.ToString();
            string degcode = ddldept.SelectedItem.Value;
            string extcode = string.Empty;
            double ext2 = 0;
            double val1 = 0;
            string mexval = string.Empty;
            double firstnumber = 0;
            double secondnumber = 0;
            double mexval1 = 0;
            double mintotal = 0;
            Boolean rowflag = false;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            string strsubqury = string.Empty;
            string subva = string.Empty;
            int seletedCount = 0;
            if (chklssubject.Items.Count == 0)
            {
                FpSpread2.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                lblmsg.Text = "No Subjects Were Found";
                lblmsg.Visible = true;
                return;
            }
            for (int i = 0; i < chklssubject.Items.Count; i++)
            {
                if (chklssubject.Items[i].Selected == true)
                {
                    seletedCount++;
                    if (subva == "")
                    {
                        subva = "'" + Convert.ToString(chklssubject.Items[i].Value).Trim() + "'";
                    }
                    else
                    {
                        subva = subva + ",'" + Convert.ToString(chklssubject.Items[i].Value).Trim() + "'";
                    }
                }
            }
            if (seletedCount == 0)
            {
                FpSpread2.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                lblmsg.Text = "Please Select Atleast One Subject And Then Proceed";
                lblmsg.Visible = true;
                return;
            }
            if (subva != "")
            {
                strsubqury = " and s.subject_no in(" + subva + ")";
            }
            else
            {
                FpSpread2.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                lblmsg.Text = "Please Select The Subject And Then Proceed";
                lblmsg.Visible = true;
                return;
            }
            val1 = 0;
            string diff = da.GetFunction("select value from COE_Master_Settings where settings='Mark Moderation'");
            if (diff.Trim() != "" && diff.Trim() != "Null" && diff.Trim() != "0")
            {
                val1 = 0;
                double.TryParse(diff.Trim(), out val1);
                Session["moder"] = Convert.ToString(val1).Trim();
            }
            if (val1 == 0)
            {
                FpSpread2.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                lblmsg.Text = "Please Allocate Mark Difference";
                lblmsg.Visible = true;
                return;
            }

            string strmarkqury = "select distinct r.roll_no,Reg_No,Stud_Name,internal_mark,external_mark,s.subject_no,total,s.subject_name,s.min_ext_marks,s.min_int_marks,s.mintotal,s.max_int_marks,s.max_ext_marks,s.maxtotal from mark_entry m,Registration r,Exam_Details e,subject s where m.roll_no = r.Roll_No and m.exam_code = e.exam_code and s.subject_no=m.subject_no and e.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlyear.SelectedValue).Trim() + "' " + strsubqury + " order by s.subject_no,Reg_No, Stud_Name";
            DataSet dsmark = da.select_method_wo_parameter(strmarkqury, "Text");

            if (dsmark.Tables.Count > 0 && dsmark.Tables[0].Rows.Count > 0)
            {
                string course = da.GetFunction("Select edu_level from course where course_id='" + Convert.ToString(ddldegree.SelectedValue).Trim() + "'");
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#219DA5");
                darkstyle.ForeColor = System.Drawing.Color.White;
                FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.Visible = true;
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].Columns.Count = 0;
                FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread2.Sheets[0].Columns.Count = 7;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg. No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "CIA Marks";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "External Marks";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Deficit (Marks)";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Moderated Ext Marks";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;

                string strmoderation = "select distinct r.roll_no,Reg_No,Stud_Name,internal_mark,external_mark,s.subject_no,total,s.subject_name,me.bf_moderation_extmrk,me.af_moderation_extmrk,me.passmark from mark_entry m,Registration r,Exam_Details e,subject s,moderation me where m.roll_no = r.Roll_No and me.subject_no=m.subject_no and me.exam_code=m.exam_code and m.roll_no=me.roll_no and me.exam_code=e.exam_code and s.subject_no=me.subject_no and r.Roll_No=me.roll_no and m.exam_code = e.exam_code and s.subject_no=m.subject_no and e.Exam_Month='" + Convert.ToString(ddlmonth.SelectedValue).Trim() + "' and e.Exam_year='" + Convert.ToString(ddlyear.SelectedValue).Trim() + "' " + strsubqury + " order by s.subject_no,Reg_No, Stud_Name";
                DataSet dsmodera = da.select_method_wo_parameter(strmoderation, "Text");

                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    if (chklssubject.Items[i].Selected == true)
                    {
                        string subjectno = Convert.ToString(chklssubject.Items[i].Value).Trim();
                        string subjectCode = da.GetFunctionv("select subject_code from subject where subject_no='" + subjectno + "'");
                        string mex = "select min_ext_marks,mintotal from subject s,syllabus_master y where s.syll_code = y.syll_code and degree_code = '" + degcode + "' and Batch_Year = '" + batchyr + "' and s.subject_no='" + subjectno + "'";
                        DataSet ds2 = da.select_method_wo_parameter(mex, "Text");
                        if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            mexval = Convert.ToString(ds2.Tables[0].Rows[0]["min_ext_marks"]).Trim();
                            if (mexval.Trim() != "" && mexval.Trim() != "Null")
                            {
                                mexval1 = 0;
                                double.TryParse(Convert.ToString(mexval).Trim(), out mexval1);
                                mintotal = 0;
                                double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["mintotal"]).Trim(), out mintotal);
                            }
                            else
                            {
                                mexval1 = 0;
                            }
                        }
                        if (mexval1 != 0)
                        {
                            firstnumber = mexval1 - val1;
                            secondnumber = mexval1 - 1;
                            DataView dvmodera = new DataView();
                            if (dsmodera.Tables.Count > 0 && dsmodera.Tables[0].Rows.Count > 0)
                            {
                                dsmodera.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                                dvmodera = dsmodera.Tables[0].DefaultView;
                            }
                            int ssno = 0;
                            if (dvmodera.Count > 0)
                            {
                                rowflag = true;
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dvmodera[0]["subject_name"]).Trim() + ((chkSubjectNameWithSubjectCode.Checked) ? " [ " + subjectCode + " ]" : "");
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.AliceBlue;
                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 7);

                                for (int mo = 0; mo < dvmodera.Count; mo++)
                                {
                                    ssno++;
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ssno).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[0].Width = 40;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvmodera[mo]["Reg_No"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[1].Width = 50;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvmodera[mo]["Stud_Name"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread2.Sheets[0].Columns[2].Width = 150;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvmodera[mo]["internal_mark"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[3].Width = 70;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvmodera[mo]["bf_moderation_extmrk"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[4].Width = 80;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvmodera[mo]["passmark"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[5].Width = 50;

                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvmodera[mo]["af_moderation_extmrk"]).Trim();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Columns[6].Width = 70;
                                }
                            }
                            //else
                            //{
                            //    DataView dvmark = new DataView();
                            //    if (dsmark.Tables.Count > 0 && dsmark.Tables[0].Rows.Count > 0)
                            //    {
                            //        dsmark.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "'";
                            //        dvmark = dsmark.Tables[0].DefaultView;
                            //    }
                            //    for (int m = 0; m < dvmark.Count; m++)
                            //    {
                            //        double stumexternal = 0;
                            //        double stuminternal = 0;
                            //        double stumintotal = 0;

                            //        double minInternal=0;
                            //        double.TryParse(Convert.ToString(dvmark[m]["min_int_marks"]).Trim(), out minInternal);
                            //        //,s.min_ext_marks,s.min_int_marks,s.mintotal,s.max_int_marks,s.max_ext_marks,s.maxtotal
                                    
                            //        double.TryParse(Convert.ToString(dvmark[m]["external_mark"]).Trim(), out stumexternal);
                            //        double.TryParse(Convert.ToString(dvmark[m]["internal_mark"]).Trim(), out stuminternal);
                            //        double.TryParse(Convert.ToString(dvmark[m]["total"]).Trim(), out stumintotal);
                            //        if ((stumexternal < mexval1 || stumintotal < mintotal) && (stumexternal > 0 && stuminternal > 0 && stuminternal >= minInternal))
                            //        {
                            //            double diffectt = 0;
                            //            double diftotal = 0;
                            //            double diffectt1 = 0;
                            //            double diftotal1 = 0;
                            //            if (stumexternal < mexval1)
                            //            {
                            //                diffectt1 = mexval1 - stumexternal;
                            //                diftotal1 = stumexternal + diffectt1 + stuminternal;
                            //                if (diftotal1 < mintotal)
                            //                {
                            //                    diffectt = diffectt1 + mintotal - diftotal1;
                            //                    diftotal = (mintotal - diftotal1) + diftotal1;
                            //                }
                            //            }
                            //            else
                            //            {
                            //                diffectt = mintotal - stumintotal;
                            //                diftotal = stumintotal + diffectt;
                            //            }

                            //            diffectt = mintotal - stumintotal;
                            //            if (diffectt < diffectt1)
                            //            {
                            //                diffectt = diffectt1;
                            //            }
                            //            diftotal = stumintotal + diffectt;

                            //            if (diftotal >= mintotal && diffectt <= val1)
                            //            {
                            //                rowflag = true;
                            //                ssno++;
                            //                if (ssno == 1)
                            //                {
                            //                    FpSpread2.Sheets[0].RowCount++;
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dvmark[0]["subject_name"]).Trim() + ((chkSubjectNameWithSubjectCode.Checked) ? " [ " + subjectCode + " ]" : "");
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                            //                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.AliceBlue;
                            //                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 7);
                            //                }

                            //                FpSpread2.Sheets[0].RowCount++;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ssno).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvmark[m]["Reg_No"]).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvmark[m]["Stud_Name"]).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvmark[m]["internal_mark"]).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvmark[m]["external_mark"]).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(diffectt).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            //                double valset = 0;
                            //                double extMarks = 0;
                            //                double.TryParse(Convert.ToString(dvmark[m]["external_mark"]).Trim(), out extMarks);
                            //                valset = extMarks + diffectt;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(valset).Trim();
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            //                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                if (rowflag == true)
                {
                    if (chkShowNoteDescription.Checked)
                    {
                        string strnote = strnote = "NOTE : A maximum of " + val1 + " Marks can be added to the external marks to get a total of " + mexval1 + " for " + course + " for a passing minimum of " + mintotal + "";
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 7);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = strnote;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    }
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    lblmsg.Visible = false;
                    lblrptname.Visible = true;
                }
                else
                {
                    FpSpread2.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    lblmsg.Text = "No Records Found";
                    lblmsg.Visible = true;
                    lblrptname.Visible = false;
                }
            }
            else
            {
                FpSpread2.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                lblmsg.Text = "No Records Found";
                lblmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string strexcelname = txtexcelname.Text.Trim().Replace(" ", "_");
            if (!string.IsNullOrEmpty(strexcelname.Trim()))
            {
                da.printexcelreport(FpSpread2, strexcelname);
            }
            else
            {
                txtexcelname.Focus();
                lbl_norec1.Text = "Please enter the Report Name in the below TextBox";
                lbl_norec1.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnprintmaster_Clcik(object sender, EventArgs e)
    {
        try
        {
            string collegeHeaderName = txtCollegeHeader.Text.Trim();
            int selectedSubjectsCount = 0;
            string subjectCode = string.Empty;
            string selSubjectCode = string.Empty;

            if (chklssubject.Items.Count > 0)
            {
                foreach (ListItem liSubjects in chklssubject.Items)
                {
                    if (liSubjects.Selected)
                    {
                        selectedSubjectsCount++;
                        selSubjectCode = liSubjects.Value;
                    }
                }
            }
            string reportName = string.Empty;
            string aided = string.Empty;
            //aided = da.GetFunctionv("select type from course c,degree dg,Department dt where c.college_code=dt.college_code and dt.college_code=dg.college_code and dg.college_code=c.college_code and c.Course_id=dg.course_id and dt.Dept_code=dg.dept_code and  c.college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "' and dg.degree_code='" + Convert.ToString(ddldept.SelectedValue).Trim() + "'");
            aided = da.GetFunctionv("select distinct type from Course c where c.college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "'");
            if (!string.IsNullOrEmpty(txtReportName.Text.Trim()))
            {
                reportName = "$" + txtReportName.Text.Trim() + ((chkReportWithStream.Checked) ? " ( " + aided + " )" : "");
            }
            else
            {
                reportName = "$List of Candidates Eligible For Moderation (Before Moderation)";
            }

            if (selectedSubjectsCount == 1 && !string.IsNullOrEmpty(selSubjectCode.Trim()))
            {
                string subject = da.GetFunctionv("select subject_code from subject where subject_no='" + selSubjectCode + "'");
                subjectCode = "@Subject Code\t\t:\t\t" + subject;
            }
            string degreedetails = ((string.IsNullOrEmpty(collegeHeaderName)) ? "" : collegeHeaderName + "$") + "Office of the Controller of Examinations " + reportName + '@' + "Degree / Branch : " + ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text + "@Exam Month / Year : " + ddlmonth.SelectedItem.Text + " - " + ddlyear.SelectedItem.Text + "" + subjectCode + "@Semester :" + ddlsem.SelectedItem.Text + "" + "@Batch: " + ddlbatch.SelectedItem.Text + "";
            string pagename = "moderation Report.aspx";
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblmsg.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Text = ex.ToString();
            lblmsg.Visible = true;
        }
    }

    protected void chksubject_ChekedChange(object sender, EventArgs e)
    {
        txtsubject.Text = "---Select---";
        if (chklssubject.Items.Count > 0)
        {
            if (chksubject.Checked == true)
            {
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = true;
                }
                txtsubject.Text = "Subject (" + chklssubject.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklssubject.Items.Count; i++)
                {
                    chklssubject.Items[i].Selected = false;
                }
                txtsubject.Text = "---Select---";
            }
        }
        else
        {
            txtsubject.Text = "---Select---";
            chksubject.Checked = false;
        }
    }

    protected void chklssubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsubject.Text = "---Select---";
        chksubject.Checked = false;
        int cou = 0;
        for (int i = 0; i < chklssubject.Items.Count; i++)
        {
            if (chklssubject.Items[i].Selected == true)
            {
                cou++;
            }
        }
        if (cou > 0)
        {
            txtsubject.Text = "Subject (" + cou + ")";
            if (cou == chklssubject.Items.Count)
            {
                chksubject.Checked = true;
            }
        }
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblPopAlertMsg.Text = string.Empty;
        divPopupAlert.Visible = false;
    }

}