using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;
using System.Reflection;
using System.Text;
//using Insproplus.BL;
//using Insproplus.AttendanceAccess;
//using Insproplus.Connection;


public partial class Usercontrols_Input_Events : System.Web.UI.UserControl
{
    //static Boolean forschoolsetting = false;
    DAccess2 obi_access = new DAccess2();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;

    Hashtable hat = new Hashtable();

    DataSet ds = new DataSet();
    DataSet dsprint = new DataSet();

    static Boolean splhr_flag = false;
    static int prevs_endrow = 0;

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string columnfield = string.Empty;



    public delegate void DropDownSelectionChanged(object sender, EventArgs e);
    public DropDownSelectionChanged FirstDropDownChanged;



    //public event EventHandler FirstDropDownChanged;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string grouporusercode = "";

            if (!IsPostBack)
            {
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                collegecode = ddlcollege.SelectedValue.ToString();// Session["Collegecode"].ToString();
                usercode = Session["UserCode"].ToString();
                prevs_endrow = 0;
                Session["QueryString"] = "";
                group_code = Session["group_code"].ToString();
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
                dsprint = obi_access.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = dsprint;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    ddlcollege_SelectedIndexChanged(sender, e);
                }


                //Pageload(sender, e);

                splhr_flag = false;
                con.Close();
                con.Open();
                string RIGHTQUERY = "select rights from  special_hr_rights where usercode=" + usercode + "";
                SqlCommand cmd1 = new SqlCommand("" + RIGHTQUERY + "", con);

                SqlDataReader dr_rights_spl_hr = cmd1.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = "";
                        Hashtable od_has = new Hashtable();

                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;

                        }
                    }
                }
                con.Close();
                BindDegree();
                BindBatch();
                bindbranch();
                bindsem();
                BindSectionDetail();
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = obi_access.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        //forschoolsetting = true;
                        Label4.Text = "School";
                        lblYear.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        lblDuration.Text = "Term";
                        lblDegree.Attributes.Add("style", " width: 95px;");
                        lblBranch.Attributes.Add("style", " width: 67px;");
                        ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                       // forschoolsetting = false;
                    }
                }
                //} Sridharan
            }


        }
        catch (Exception ex)
        {
        }

    }

    public void bindbranch()
    {

        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = ddlcollege.SelectedValue.ToString();// Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
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

    public string YourValue
    {
        get
        {
            return ddlBatch.SelectedItem.Text;
        }

    }
    public void BindDegree()
    {
        {
            ddlDegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();// Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
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


    }

    public void BindBatch()
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
            con.Close();
        }
    }

    public void bindsem()
    {
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
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
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + ddlcollege.SelectedValue.ToString() + "", con);
            ddlSemYr.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

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

            dr1.Close();
        }
        con.Close();
    }

    public void BindSectionDetail()
    {

        ddlSec.Items.Clear();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
       
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataBind();
            ddlSec.Items.Insert(0, "All");
            SqlDataReader dr_sec;
            dr_sec = cmd.ExecuteReader();
            dr_sec.Read();
            if (dr_sec.HasRows == true)
            {
                if (dr_sec["sections"].ToString() == string.Empty)
                {
                    ddlSec.Enabled = false;
                }
                else
                {
                    ddlSec.Enabled = true;
                }
            }
            else
            {

                ddlSec.Text = "";
                ddlSec.Enabled = false;
            }
        }
        else
        {
            ddlSec.Text = "";
            ddlSec.Enabled = false;

        }
        con.Close();
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            isreadonly.SetValue(this.Request.QueryString, false, null);
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        //start=====Added by Manikandan 29/07/2013
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
        //End============
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Open();
        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        GetSubject();
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();

        string collegecode = Session["InternalCollegeCode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddlDegree.SelectedValue.ToString();

        bindbranch();
        bindsem();
        GetSubject();
        BindSectionDetail();

    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        BindSectionDetail();

        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            bindsem();
            GetSubject();
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlParent = (DropDownList)this.Parent.FindControl("ddltimetable");
        Label lblparent = (Label)this.Parent.FindControl("lblerror");



        string currentsem = "";
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        BindSectionDetail();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataSet ds_cssem = new DataSet();
        string cseme = "select distinct current_semester from registration where degree_code ='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
        ds_cssem = obi_access.select_method(cseme, hat, "text");

        if (ds_cssem.Tables[0].Rows.Count > 0)
        {
            currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
        }

        if (ds.Tables[0].Rows.Count < 0)
        {

            if (ds.Tables[0].Rows.Count < 0)
            {
                ddlParent.Items.Clear();
                lblparent.Visible = true;
                lblparent.Text = "Please Add Timetable Name before Allot the Batch";
            }

        }
        // dropdownlist1 ID of parent dropdownlist
        if ( Convert.ToString (currentsem) == Convert.ToString (ddlSemYr.SelectedValue.ToString()))
        {
            //if (ds.Tables[0].Rows.Count < 0)
            //{
                if (ddlParent != null)
                {
                    
                    string sections = "";
                    string strsec = "";
                    if (ddlSec.Items.Count >= 1)
                    {
                        sections = ddlSec.SelectedValue.ToString();
                    }
                    if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and sections='" + sections.ToString() + "'";
                    }
                    DataSet ds_batchs = new DataSet();
                    string batchquery = " select TTName, convert(varchar(15),FromDate,103) as FromDate from Semester_Schedule where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + strsec + " and semester='" + ddlSemYr.SelectedValue.ToString() + "'";
                    ds_batchs = obi_access.select_method(batchquery, hat, "text");
                    if (ds_batchs.Tables[0].Rows.Count > 0)
                    {
                        ddlParent.Items.Clear();
                        DataTable tbl = ds_batchs.Tables[0];
                        foreach (DataRow row in tbl.Rows)
                        {
                            object value = row["TTname"];
                            object value1 = row["FromDate"];
                            string total = value.ToString() + "@" + value1.ToString();
                            ddlParent.Items.Add(total);
                        }
                    }
                    else
                    {
                        ddlParent.Items.Clear();
                        lblparent.Visible = true;
                        lblparent.Text = "Please Add Timetable Name before Allot the Batch";
                    }
                }
            //}
        }
        BindSectionDetail();
        GetSubject();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddlParent = (DropDownList)this.Parent.FindControl("ddltimetable");
        //Label lblparent = (Label)this.Parent.FindControl("lblerror");

        //// dropdownlist1 ID of parent dropdownlist
        //DataSet ds_cssem = new DataSet();
        //string cseme = "select distinct current_semester from registration where degree_code ='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
        //ds_cssem = obi_access.select_method(cseme, hat, "text");
        //string currentsem = "";
        //if (ds_cssem.Tables[0].Rows.Count > 0)
        //{
        //    currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
        //    if (Convert.ToString (currentsem) == Convert.ToString (ddlSemYr.SelectedValue.ToString()))
        //    {
        //        if (ddlParent != null)
        //        {
        //            string sections = "";
        //            string strsec = "";
        //            sections = ddlSec.SelectedValue.ToString();
        //            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        //            {
        //                strsec = "";
        //            }
        //            else
        //            {
        //                strsec = " and sections='" + sections.ToString() + "'";
        //            }
        //            DataSet ds_batchs = new DataSet();
        //            string batchquery = " select TTName, convert(varchar(15),FromDate,103) as FromDate from Semester_Schedule where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' " + strsec + " and semester='" + ddlSemYr.SelectedValue.ToString() + "'";
        //            ds_batchs = obi_access.select_method(batchquery, hat, "text");
        //            if (ds_batchs.Tables[0].Rows.Count > 0)
        //            {
        //                ddlParent.Items.Clear();
        //                DataTable tbl = ds_batchs.Tables[0];
        //                foreach (DataRow row in tbl.Rows)
        //                {
        //                    object value = row["TTname"];
        //                    object value1 = row["FromDate"];
        //                    string total = value.ToString() + "@" + value1.ToString();
        //                    ddlParent.Items.Add(total);
        //                }
        //            }
        //            else
        //            {
        //                ddlParent.Items.Clear();
        //                lblparent.Visible = true;
        //                lblparent.Text = "Please Add Timetable Name before Allot the Batch";
        //            }
        //        }
        //    }
        //}
        DropDownList ddlparent1 = (DropDownList)this.Parent.FindControl("ddlTest");
        if (ddlparent1 != null)
        {
            string sections = "";
            string strsec = "";
            sections = ddlSec.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            con.Close();
            con.Open();
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(SyllabusQry, con);
            DataSet ds = new DataSet();
            sqlAdapter1.Fill(ds);
            string str = ds.Tables[0].Rows[0]["syllabus_year"].ToString();
            string Sqlstr;
            Sqlstr = "";

            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + str.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";

            SqlDataAdapter sqlAdapter11 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter11.Fill(titles);
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddlparent1.DataSource = titles;
                ddlparent1.DataValueField = "Criteria_No";
                ddlparent1.DataTextField = "Criteria";
                ddlparent1.DataBind();
                ddlparent1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            }
        }
        GetSubject();
    }

    public void GetSubject()
    {

        Panel psubject = (Panel)this.Parent.FindControl("psubject");
        Label lblerror = (Label)this.Parent.FindControl("lblerror");
        TextBox txtsub = (TextBox)this.Parent.FindControl("txtsubject");
        CheckBoxList ddlsubParent = (CheckBoxList)this.Parent.FindControl("chklstsubject");
        CheckBox chksub = (CheckBox)this.Parent.FindControl("chksubject");

        TextBox txtfrom = (TextBox)this.Parent.FindControl("txtfrom") ;
        TextBox txtto = (TextBox)this.Parent.FindControl("txtto");

       

        //txtsub.Text = "";
       // ddlsubParent.Items.Clear();
        if (ddlsubParent != null)
        {
            ddlsubParent.Visible = true;
            psubject.Visible = true;
            ddlsubParent.Enabled = true;
            try
            {
                txtsub.Text = "";
                ddlsubParent.Items.Clear();

                string subjectquery = string.Empty;
                //chklstsubject.Items.Clear();
                string sections = ddlSec.SelectedValue.ToString();
                string strsec = "";
                if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and exam_type.Sections='" + sections.ToString() + "'";
                }

                string sems = "";
                if (ddlSemYr.SelectedValue != "")
                {
                    if (ddlSemYr.SelectedValue == "")
                    {
                        sems = "";
                    }
                    else
                    {
                        sems = "and SM.semester=" + ddlSemYr.SelectedValue.ToString() + "";
                    }

                    if (Session["Staff_Code"].ToString() == "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' order by S.subject_no ";
                    }
                    else if (Session["Staff_Code"].ToString() != "")
                    {
                        subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "'  and staff_code='" + Session["Staff_Code"].ToString() + "'  order by S.subject_no ";
                    }

                    if (subjectquery != "")
                    {
                        ds.Dispose();
                        ds.Reset();
                        ds = obi_access.select_method(subjectquery, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ddlsubParent.Visible = true;
                            psubject.Visible = true;
                            ddlsubParent.Enabled = true;
                            ddlsubParent.DataSource = ds;
                            ddlsubParent.DataValueField = "Subject_No";
                            ddlsubParent.DataTextField = "Subject_Name";
                            ddlsubParent.DataBind();
                            txtsub.Enabled = true;
                        }
                        else
                        {
                            //ddlsubParent.Enabled = false;
                            //psubject.Visible = false;
                            //txtsub.Enabled = false;
                        }
                    }
                }
                else
                {
                    ddlsubParent.SelectedIndex = 0;
                }
            }

            catch (Exception ex)
            {
                throw ex;
                //errmsg.Text = ex.ToString();
            }
        }
        else
        {
             //txtsub.Enabled = false;
            //  psubject.Enabled = false;

        }
        try
        {

            if (chksub.Checked == true)
            {
                chksub.Checked = true;
                for (int i = 0; i < ddlsubParent.Items.Count; i++)
                {
                    ddlsubParent.Items[i].Selected = true;
                    txtsub.Text = "Subject(" + (ddlsubParent.Items.Count) + ")";
                    if (i == 0)
                    {
                        //txtsub.Enabled = false;
                        psubject.Visible = false;
                    }
                }

            }
            else
            {
                for (int i = 0; i < ddlsubParent.Items.Count; i++)
                {
                    ddlsubParent.Items[i].Selected = false;
                   chksub.Checked = false;
                    txtsub.Text = "---Select---";
                    chksub.Checked = false;

                }
            }
        }
        catch (Exception ex)
        {
           // throw ex;
        }
    }

}