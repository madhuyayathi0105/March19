using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Drawing;
using System.Configuration;

public partial class Facultywiseperformance : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string collegecode = string.Empty;
    int commcount = 0;
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataTable data = new DataTable();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Request.FilePath.Contains("CAMHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/MarkMod/CAMHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtfromdate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
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
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            BindDepartment();
            BindDesignation();
            BindStaff();
            //loadacademic();
            for (int c = 0; c < chklscolumn.Items.Count; c++)
            {
                chklscolumn.Items[c].Selected = true;
            }
        }
    }

    //Load Designation
    public void BindDesignation()
    {
        txtdesign.Text = "---Select---";
        chkdesign.Checked = false;
        collegecode = ddlcollege.SelectedValue.ToString();
        chklsdesign.Items.Clear();
        ds.Dispose();
        ds.Reset();
        ds = d2.binddesi(collegecode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            chklsdesign.DataSource = ds;
            chklsdesign.DataValueField = "desig_code";
            chklsdesign.DataTextField = "desig_name";
            chklsdesign.DataBind();
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = true;
            }
            chkdesign.Checked = true;
            txtdesign.Text = "Design (" + chklsdesign.Items.Count + ")";
        }
    }

    //Load Department
    public void BindDepartment()
    {
        txtdept.Text = "---Select---";
        chkdept.Checked = false;
        chklsdept.Items.Clear();
        ds.Dispose();
        ds.Reset();
        collegecode = ddlcollege.SelectedValue.ToString();
        ds = d2.loaddepartment(collegecode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            chklsdept.DataSource = ds;
            chklsdept.DataTextField = "dept_name";
            chklsdept.DataValueField = "Dept_Code";
            chklsdept.DataBind();
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = true;
            }
            chkdept.Checked = true;
            txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
        }
    }

    //Load Staff
    public void BindStaff()
    {
        string degsign = string.Empty;
        string department = string.Empty;
        chklsstaff.Items.Clear();
        txtstaff.Text = "---Select---";
        chkstaff.Checked = false;
        collegecode = ddlcollege.SelectedValue.ToString();
        for (int i = 0; i < chklsdesign.Items.Count; i++)
        {
            if (chklsdesign.Items[i].Selected == true)
            {
                if (degsign == "")
                {
                    degsign = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                }
                else
                {
                    degsign = "" + degsign + "," + "'" + chklsdesign.Items[i].Value.ToString() + "'";
                }
            }
        }
        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                if (department == "")
                {
                    department = "'" + chklsdept.Items[i].Value.ToString() + "'";
                }
                else
                {
                    department = department + "," + "'" + chklsdept.Items[i].Value.ToString() + "'";
                }
            }
        }
        if (degsign.Trim() != "" && department.Trim() != "")
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.bindstaffnme(collegecode, degsign, department);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsstaff.Enabled = true;
                chklsstaff.DataSource = ds;
                chklsstaff.DataTextField = "staffnamecode";
                chklsstaff.DataValueField = "staff_code";
                chklsstaff.DataBind();
                for (int i = 0; i < chklsstaff.Items.Count; i++)
                {
                    chklsstaff.Items[i].Selected = true;
                }
                txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
                chkstaff.Checked = true;
            }
        }
    }

    public void clear()
    {
        Showgrid.Visible = false;
        Printcontrol.Visible = false;
        btnprint.Visible = false;
        lblexcel.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnDirtprint.Visible = false;
        txtexcelname.Text = string.Empty;

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDepartment();
        BindDesignation();
        BindStaff();
    }

    protected void chkdept_ChekedChange(object sender, EventArgs e)
    {
        if (chkdept.Checked == true)
        {
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = true;
            }
            txtdept.Text = "Dept(" + chklsdept.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                chklsdept.Items[i].Selected = false;
            }
            txtdept.Text = "--Select--";
        }
        BindStaff();
    }

    protected void chkdesign_ChekedChange(object sender, EventArgs e)
    {
        if (chkdesign.Checked == true)
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = true;
            }
            txtdesign.Text = "Design (" + chklsdesign.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                chklsdesign.Items[i].Selected = false;
            }
            txtdesign.Text = "--Select--";
        }
        BindStaff();
    }

    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtdesign.Text = "--Select--";
        chkdesign.Checked = false;
        commcount = 0;
        for (int i = 0; i < chklsdesign.Items.Count; i++)
        {
            if (chklsdesign.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdesign.Text = "Design (" + commcount.ToString() + ")";
            if (chklsdesign.Items.Count == commcount)
            {
                chkdesign.Checked = true;
            }
        }
        BindStaff();
    }

    protected void chkstaff_ChekedChange(object sender, EventArgs e)
    {
        if (chkstaff.Checked == true)
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = true;
            }
            txtstaff.Text = "Staff(" + chklsstaff.Items.Count.ToString() + ")";
        }
        else
        {
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                chklsstaff.Items[i].Selected = false;
            }
            txtstaff.Text = "--Select--";
        }
    }

    protected void chklsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtstaff.Text = "--Select--";
        chkstaff.Checked = false;
        for (int i = 0; i < chklsstaff.Items.Count; i++)
        {
            if (chklsstaff.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtstaff.Text = "Staff(" + commcount.ToString() + ")";
            if (chklsstaff.Items.Count == commcount)
            {
                chkstaff.Checked = true;
            }
        }
    }

    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        commcount = 0;
        txtdept.Text = "---Select---";
        chkdept.Checked = false;
        for (int i = 0; i < chklsdept.Items.Count; i++)
        {
            if (chklsdept.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txtdept.Text = "Dept(" + commcount.ToString() + ")";
            if (chklsdept.Items.Count == commcount)
            {
                chkdept.Checked = true;
            }
        }
        BindStaff();
    }

    //protected void chklsacedemic_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    txtacedemic.Text = "---Select---";
    //    chkacedmic.Checked = false;
    //    for (int i = 0; i < chklsacedemic.Items.Count; i++)
    //    {
    //        if (chklsacedemic.Items[i].Selected == true)
    //        {
    //            commcount = commcount + 1;
    //        }
    //    }
    //    if (commcount > 0)
    //    {
    //        txtacedemic.Text = "Academic (" + commcount.ToString() + ")";
    //        if (chklsacedemic.Items.Count == commcount)
    //        {
    //            chkacedmic.Checked = true;
    //        }
    //    }
    //}
    //protected void chkacedmic_ChekedChange(object sender, EventArgs e)
    //{
    //    if (chkacedmic.Checked == true)
    //    {
    //        for (int i = 0; i < chklsacedemic.Items.Count; i++)
    //        {
    //            chklsacedemic.Items[i].Selected = true;
    //        }
    //        txtacedemic.Text = "Academic (" + chklsstaff.Items.Count.ToString() + ")";
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklsacedemic.Items.Count; i++)
    //        {
    //            chklsacedemic.Items[i].Selected = false;
    //        }
    //        txtacedemic.Text = "--Select--";
    //    }
    //}
    //public void loadacademic()
    //{
    //    try
    //    {
    //        //ddlacademic.Items.Clear();
    //        //int acyera = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 10;
    //        //int acyert = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
    //        //for (int i = acyert; i >= acyera; i--)
    //        //{
    //        //    ddlacademic.Items.Add(i.ToString());
    //        //}
    //        chklsacedemic.Items.Clear();
    //        txtacedemic.Text = "---Select---";
    //        chkacedmic.Checked = false;
    //        int acyera = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 10;
    //        int acyert = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
    //        for (int i = acyert; i >= acyera; i--)
    //        {
    //            string stracdeic=i+" EVEN";
    //            chklsacedemic.Items.Add(stracdeic); 
    //            stracdeic = i + " ODD";
    //            chklsacedemic.Items.Add(stracdeic);
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            DataRow drow;
            clear();
            ds.Dispose();
            ds = d2.select_method("select * from sysobjects where name='tbl_staff_topper' and Type='U'", hat, "text ");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int p = d2.insert_method("IF not EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbl_staff_topper' AND COLUMN_NAME = 'user_code') alter table tbl_staff_topper add user_code nvarchar(25)", hat, "text");
            }
            else
            {
                int p = d2.insert_method("create  table tbl_staff_topper (staff_code nvarchar(25),Staff_name nvarchar(50),degree nvarchar(50),subject nvarchar(100),internal_exam_type nvarchar(50),in_total float (8),in_appear float (8),in_pass float (8),in_fail float (8),external_exam_type nvarchar(50),ext_total float(8),ext_appear float(8),ext_pass float(8),ext_fail float(8),isExternal int,user_code nvarchar(25))", hat, "text");
            }
            int strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
            string fadte = txtfromdate.Text.ToString();
            string[] spf = fadte.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Less Than Or Equal To Date";
                return;
            }
            Boolean visfalg = false;
            for (int c = 0; c < chklscolumn.Items.Count; c++)
            {
                if (chklscolumn.Items[c].Selected == true)
                {
                    visfalg = true;
                }
            }
            if (visfalg == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Column Order And Then Proceed";
                return;
            }
            Boolean setflag = false;
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Department And Then Proceed";
                return;
            }
            setflag = false;
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    setflag = true;
                }
            }
            if (setflag == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Designation And Then Proceed";
                return;
            }
            setflag = false;
            string staffc = string.Empty;
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                if (chklsstaff.Items[i].Selected == true)
                {
                    if (staffc == "")
                    {
                        staffc = "'" + chklsstaff.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        staffc = staffc + ",'" + chklsstaff.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (staffc.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Staff And Then Proceed";
                return;
            }
            else
            {
                staffc = " and st.staff_code in(" + staffc + ")";
            }
            //string academicyear = ddlacademic.SelectedValue.ToString();
            //string strsemtype = " and si.semester%2=0 ";
            //if (ddlsemtype.SelectedItem.ToString() == "ODD")
            //{
            //    strsemtype = " and si.semester%2=1 ";
            //}
            //else
            //{
            //    int acyearval = Convert.ToInt32(academicyear);
            //    acyearval++;
            //    academicyear = acyearval.ToString();
            //}
            ArrayList arrColHdrNames1 = new ArrayList();
            Dictionary<int, int> dicrowspansubpass = new Dictionary<int, int>();
            data.Columns.Add("S.No", typeof(string));
            data.Columns.Add("Department", typeof(string));
            data.Columns.Add("Designation", typeof(string));
            data.Columns.Add("Staff Name", typeof(string));
            data.Columns.Add("Staff Code", typeof(string));
            data.Columns.Add("Degree Details", typeof(string));
            data.Columns.Add("Subject Code", typeof(string));
            data.Columns.Add("Subject Name", typeof(string));
            data.Columns.Add("Exam", typeof(string));
            data.Columns.Add("Total No.of Students", typeof(string));
            data.Columns.Add("Appear", typeof(string));
            data.Columns.Add("Passed", typeof(string));
            data.Columns.Add("Absent", typeof(string));
            data.Columns.Add("Fail", typeof(string));
            data.Columns.Add("Pass %", typeof(string));
            data.Columns.Add("Over All Pass %", typeof(string));


            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Department");
            arrColHdrNames1.Add("Designation");
            arrColHdrNames1.Add("Staff Name");
            arrColHdrNames1.Add("Staff Code");
            arrColHdrNames1.Add("Degree Details");
            arrColHdrNames1.Add("Subject Code");
            arrColHdrNames1.Add("Subject Name");
            arrColHdrNames1.Add("Exam");
            arrColHdrNames1.Add("Total No.of Students");
            arrColHdrNames1.Add("Appear");
            arrColHdrNames1.Add("Passed");
            arrColHdrNames1.Add("Absent");
            arrColHdrNames1.Add("Fail");
            arrColHdrNames1.Add("Pass %");
            arrColHdrNames1.Add("Over All Pass %");

            DataRow drHdr1 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames1[grCol];

            data.Rows.Add(drHdr1);

            string strgetexam = "select c.criteria,c.Criteria_no,e.exam_code,e.batch_year,e.sections,e.subject_no,c.syll_code,e.min_mark from CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no ";
            strgetexam = strgetexam + " select distinct ed.batch_year,ed.degree_code,ed.current_semester,ed.Exam_Month,ed.Exam_year,ed.exam_code,m.subject_no from Exam_Details ed,mark_entry m where ed.exam_code=m.exam_code";
            DataSet dsexam = d2.select_method_wo_parameter(strgetexam, "Text");
            //for (int a = 0; a < chklsacedemic.Items.Count; a++)
            //{
            //    if (chklsacedemic.Items[a].Selected == true)
            //    {
            //        string acval = chklsacedemic.Items[a].Text.ToString();
            //string[] stra = acval.Split(' ');
            //string academicyear = stra[0].ToString();
            //string strsemtype = " and si.semester%2=0 ";
            //if (stra[1].ToString().Trim() == "ODD")
            //{
            //    strsemtype = " and si.semester%2=1 ";
            //}
            //else
            //{
            //    int year = Convert.ToInt32(academicyear);
            //    year++;
            //    academicyear = year.ToString();
            //}
            strdelexistval = d2.update_method_wo_parameter("delete from tbl_staff_topper where user_code='" + usercode + "'", "Text");
            //string strqureystaff = "select distinct sy.Batch_Year,sy.degree_code,sy.semester,st.Sections,sy.syll_code,st.staff_code,s.subject_name,s.subject_no from seminfo si,syllabus_master sy,sub_sem ss,subject s,staff_selector st where si.batch_year=sy.Batch_Year and si.degree_code=sy.degree_code and si.semester=sy.semester and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code  and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and st.subject_no=s.subject_no and st.batch_year=si.batch_year  and st.batch_year=sy.Batch_Year and ss.promote_count=1 and year(si.start_date)='" + academicyear + "' " + strsemtype + " " + staffc + "";
            string strqureystaff = "select distinct sy.Batch_Year,sy.degree_code,sy.semester,st.Sections,sy.syll_code,st.staff_code,s.subject_name,s.subject_no from seminfo si,syllabus_master sy,sub_sem ss,subject s,staff_selector st where si.batch_year=sy.Batch_Year and si.degree_code=sy.degree_code and si.semester=sy.semester and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code  and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and st.subject_no=s.subject_no and st.batch_year=si.batch_year  and st.batch_year=sy.Batch_Year and ss.promote_count=1 and si.start_date between '" + dtf.ToString("MM/dd/yyyy") + "' and '" + dtt.ToString("MM/dd/yyyy") + "' " + staffc + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strqureystaff, "Text");
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                if (chklsstaff.Items[i].Selected == true)
                {
                    string staffcode = chklsstaff.Items[i].Value.ToString();
                    DataView dvstaff = new DataView();
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "Staff_code='" + staffcode + "'";
                        dvstaff = ds.Tables[0].DefaultView;
                    }
                    for (int s = 0; s < dvstaff.Count; s++)
                    {
                        string staffname = string.Empty;
                        string staff = dvstaff[s]["staff_code"].ToString();
                        string subject = dvstaff[s]["subject_name"].ToString();
                        string degreecode = dvstaff[s]["degree_code"].ToString();
                        string subjectno = dvstaff[s]["subject_no"].ToString();
                        string batch = dvstaff[s]["batch_year"].ToString();
                        string syllcode = dvstaff[s]["syll_code"].ToString();
                        string sections = dvstaff[s]["sections"].ToString();
                        string semester = dvstaff[s]["semester"].ToString();
                        string departmentvalue = string.Empty;
                        string sp_section = string.Empty;
                        if (sections.ToString().Trim() != "-1" && sections.ToString().Trim() != "" && sections != null)
                        {
                            sp_section = sections;
                            sections = "and r.sections='" + sections + "'";
                        }
                        else
                        {
                            sections = string.Empty;
                        }
                        //internal Exam 
                        if (ddlexam.SelectedItem.ToString() != "External")
                        {
                            DataView dvint = new DataView();
                            if (dsexam.Tables.Count > 0 && dsexam.Tables[0].Rows.Count > 0)
                            {
                                dsexam.Tables[0].DefaultView.RowFilter = "syll_code='" + syllcode + "' and subject_no='" + subjectno + "' and batch_year='" + batch + "' and sections='" + sp_section + "'";
                                dvint = dsexam.Tables[0].DefaultView;
                            }
                            for (int ine = 0; ine < dvint.Count; ine++)
                            {
                                string examname = dvint[ine]["criteria"].ToString();
                                string examcode = dvint[ine]["exam_code"].ToString();
                                string minmarks = dvint[ine]["min_mark"].ToString();
                                string totalstudent = string.Empty;
                                string staffvaluequery = "select distinct count(s.roll_no) as total from subjectchooser s,registration r where r.roll_no=s.roll_no and r.cc=0 and r.exam_flag<>'debar' and r.delflag=0 and subject_no='" + subjectno + "' and r.batch_year=" + batch + " and s.semester=" + semester + " and r.degree_code=" + degreecode + " " + sections + "";
                                hat.Clear();
                                DataSet dsstaff = d2.select_method(staffvaluequery, hat, "Text");
                                if (dsstaff.Tables[0].Rows.Count > 0)
                                {
                                    totalstudent = dsstaff.Tables[0].Rows[0]["total"].ToString();
                                }
                                hat.Clear();
                                hat.Add("exam_code", examcode);
                                hat.Add("min_marks", minmarks);
                                hat.Add("section", sp_section);
                                DataSet dsexamdetails = d2.select_method("Proc_All_Subject_Details", hat, "sp");
                                if (dsexamdetails.Tables.Count > 0 && dsexamdetails.Tables[0].Rows.Count > 0)
                                {
                                    string appear = dsexamdetails.Tables[8].Rows[0]["PRESENT_COUNT"].ToString();
                                    string passcount = dsexamdetails.Tables[1].Rows[0]["PASS_COUNT"].ToString();
                                    string failcount = dsexamdetails.Tables[11].Rows[0]["FAIL_COUNT_WITHOUT_AB"].ToString();
                                    string absent = dsexamdetails.Tables[9].Rows[0]["Absent_count"].ToString();
                                    int totalcount = Convert.ToInt32(appear) + Convert.ToInt32(passcount) + Convert.ToInt32(failcount);
                                    if (totalcount != 0)
                                    {
                                        string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail,ext_appear,isExternal,user_code) values ";
                                        insertallexam = "" + insertallexam + " ('" + staff + "','" + staffname + "','" + sp_section + "','" + subjectno + "','" + examname + "'," + totalstudent + "," + appear + "," + passcount + "," + failcount + ",'" + absent + "',0,'" + usercode + "')";
                                        int value = d2.insert_method(insertallexam, hat, "Text");
                                    }
                                }
                            }
                        }
                        //internal External  
                        if (ddlexam.SelectedItem.ToString() != "Internal")
                        {
                            string strgetexamquery = "select ed.Exam_Month,ed.Exam_year,ed.exam_code from Exam_Details ed,mark_entry m where m.exam_code=ed.exam_code and ed.batch_year='" + batch + "' and ed.degree_code='" + degreecode + "' and ed.current_semester='" + semester + "' and m.subject_no='" + subjectno + "'";
                            DataSet dsexamquery = d2.select_method_wo_parameter(strgetexamquery, "Text");
                            DataView dvext = new DataView();
                            if (dsexam.Tables.Count > 1 && dsexam.Tables[1].Rows.Count > 0)
                            {
                                dsexam.Tables[1].DefaultView.RowFilter = "batch_year='" + batch + "' and degree_code='" + degreecode + "' and current_semester='" + semester + "' and subject_no='" + subjectno + "'";
                                dvext = dsexam.Tables[1].DefaultView;
                            }
                            if (dvext.Count > 0)
                            {
                                string examcode = dvext[0]["exam_code"].ToString();
                                string exammonth = dvext[0]["Exam_month"].ToString();
                                string examyear = dvext[0]["Exam_Year"].ToString();
                                if (exammonth == "1")
                                    exammonth = "Jan";
                                else if (exammonth == "2")
                                    exammonth = "Feb";
                                else if (exammonth == "3")
                                    exammonth = "Mar";
                                else if (exammonth == "4")
                                    exammonth = "Apr";
                                else if (exammonth == "5")
                                    exammonth = "May";
                                else if (exammonth == "6")
                                    exammonth = "Jun";
                                else if (exammonth == "7")
                                    exammonth = "Jul";
                                else if (exammonth == "8")
                                    exammonth = "Aug";
                                else if (exammonth == "9")
                                    exammonth = "Sep";
                                else if (exammonth == "10")
                                    exammonth = "Oct";
                                else if (exammonth == "11")
                                    exammonth = "Nov";
                                else if (exammonth == "12")
                                    exammonth = "Dec";
                                string examname = examyear + " / " + exammonth;
                                hat.Clear();
                                hat.Add("Exam_code", examcode);
                                hat.Add("Subject_no", subjectno);
                                DataSet dsexterdetail = d2.select_method("Sp_External_Student_Details", hat, "sp");
                                if (dsexterdetail.Tables.Count > 0 && dsexterdetail.Tables[0].Rows.Count > 0)
                                {
                                    string Total = dsexterdetail.Tables[5].Rows[0]["Total"].ToString();
                                    string Pass = dsexterdetail.Tables[2].Rows[0]["Pass_Count"].ToString();
                                    string Fail = dsexterdetail.Tables[3].Rows[0]["Fail_Count_With_AB"].ToString();
                                    string Appear = dsexterdetail.Tables[0].Rows[0]["Present_count"].ToString();
                                    int totalcount = Convert.ToInt32(Total) + Convert.ToInt32(Pass) + Convert.ToInt32(Fail) + Convert.ToInt32(Appear);
                                    if (totalcount != 0)
                                    {
                                        string insertallexam = "insert into tbl_staff_topper (staff_code,staff_name,degree,subject,internal_exam_type,in_total,in_appear,in_pass,in_fail,isExternal,user_code) values ";
                                        insertallexam = "" + insertallexam + " ('" + staff + "','" + staffname + "','" + departmentvalue + "','" + subjectno + "','" + examname + "'," + Total + "," + Appear + "," + Pass + "," + Fail + ",1,'" + usercode + "')";
                                        int value = d2.insert_method(insertallexam, hat, "Text");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Hashtable hatstaff = new Hashtable();
            string type = string.Empty;
            if (ddlexam.SelectedItem.ToString() == "Internal")
            {
                type = " and isExternal='0'";
            }
            else if (ddlexam.SelectedItem.ToString() == "External")
            {
                type = " and isExternal='1'";
            }
            ds.Dispose();
            ds.Reset();
            string strstaffperformancequery = "select distinct sm.staff_code,sm.staff_name,h.dept_name,d.desig_name,s.subject_code,s.subject_name,s.subject_no,sy.Batch_Year,c.Course_Name,dep.Dept_Name as department,sy.degree_code,sy.semester,ss.Sections,ts.in_total,ts.in_appear,ts.in_pass,ts.in_fail,ts.ext_appear,ts.internal_exam_type from tbl_staff_topper ts,staffmaster sm,stafftrans st,hrdept_master h,desig_master d,subject s,syllabus_master sy,staff_selector ss,Degree de,Course c,Department dep where st.staff_code=sm.staff_code and sm.staff_code=ts.staff_code and st.staff_code=st.staff_code and sm.staff_code=ss.staff_code and st.staff_code=ss.staff_code and ss.staff_code=ts.staff_code and s.subject_no=ss.subject_no and sm.college_code=h.college_code and sm.college_code=d.collegeCode and de.Degree_Code=sy.degree_code and de.Dept_Code=dep.Dept_Code and c.Course_Id=de.Course_Id and st.dept_code=h.dept_code and st.desig_code=d.desig_code and ts.subject=s.subject_no and s.syll_code=sy.syll_code and ss.Sections=ts.degree and st.latestrec='1' and ts.user_code='" + usercode + "' " + type + " order by sm.staff_code,sy.Batch_Year,c.Course_Name,department";
            ds = d2.select_method_wo_parameter(strstaffperformancequery, "text");
            string getpervcstaff = "select distinct round(sum(round(isnull(in_pass,0)/isnull(in_appear,0)*100,2))/(count(staff_code)*100)*100,2) as passpercentage,count(staff_code),staff_code,subject,degree from tbl_staff_topper where in_appear is not null " + type + " and isnull(in_pass,'0')<>'0' and isnull(in_appear,0)<>'0' and user_code='" + usercode + "' group by staff_code,subject,degree order by staff_code,degree,subject";
            DataSet dsstafffper = d2.select_method_wo_parameter(getpervcstaff, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                btnprint.Visible = true;
                btnexcel.Visible = true;
                lblexcel.Visible = true;
                txtexcelname.Visible = true;
                btnDirtprint.Visible = true;

                Double filva = 0;
                if (txtrange.Text.ToString() != "")
                {
                    filva = Convert.ToDouble(txtrange.Text.ToString());
                }
                int sno = 0;
                int row = 1;
                if (dsstafffper.Tables.Count > 0 && dsstafffper.Tables[0].Rows.Count > 0)
                {
                    dicrowspansubpass.Clear();
                    row++;
                    for (int st = 0; st < dsstafffper.Tables[0].Rows.Count; st++)
                    {
                        string subjectno = dsstafffper.Tables[0].Rows[st]["subject"].ToString();
                        string staffcode = dsstafffper.Tables[0].Rows[st]["staff_code"].ToString();
                        string overperc = dsstafffper.Tables[0].Rows[st]["passpercentage"].ToString();
                        string secval = dsstafffper.Tables[0].Rows[st]["degree"].ToString();
                        if (overperc.Trim() != "")
                        {
                            Boolean disflag = false;
                            if (filva == 0)
                            {
                                disflag = true;
                            }
                            else
                            {
                                Double getval = Convert.ToDouble(overperc);
                                if (ddlran.SelectedItem.ToString() == "Above")
                                {
                                    if (filva < getval)
                                    {
                                        disflag = true;
                                    }
                                }
                                else
                                {
                                    if (filva > getval)
                                    {
                                        disflag = true;
                                    }
                                }
                            }
                            if (disflag == true)
                            {
                                if (secval.Trim() != "" && secval.Trim() != "0" && secval.Trim() != "-1")
                                {
                                    secval = " and sections='" + secval + "'";
                                }
                                DataView dvstaffco = new DataView();
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "staff_code='" + staffcode + "' and subject_no='" + subjectno + "' " + secval + "";
                                    dvstaffco = ds.Tables[0].DefaultView;
                                }
                                Double paper = 0;
                                if (dvstaffco.Count > 0)
                                {
                                    setflag = true;
                                    for (int spe = 0; spe < dvstaffco.Count; spe++)
                                    {
                                        sno++;
                                        string degeredeatisl = dvstaffco[spe]["Batch_Year"].ToString() + " - " + dvstaffco[spe]["Course_Name"].ToString() + " - " + dvstaffco[spe]["department"].ToString() + " - " + dvstaffco[spe]["semester"].ToString();
                                        if (dvstaffco[spe]["Sections"].ToString().Trim() != "-1" && dvstaffco[spe]["Sections"].ToString().Trim() != "")
                                        {
                                            degeredeatisl = degeredeatisl + " - " + dvstaffco[spe]["Sections"].ToString();
                                        }
                                        Double total = 0;
                                        Double apperar = 0;
                                        Double pass = 0;
                                        Double fail = 0;
                                        Double absent = 0;
                                        Double appear = 0;
                                        string exam = dvstaffco[spe]["internal_exam_type"].ToString();

                                        drow = data.NewRow();
                                        drow["S.No"] = sno.ToString();
                                        drow["Department"] = dvstaffco[spe]["dept_name"].ToString();
                                        drow["Designation"] = dvstaffco[spe]["desig_name"].ToString();
                                        drow["Staff Name"] = dvstaffco[spe]["staff_name"].ToString();
                                        drow["Staff Code"] = staffcode;
                                        drow["Degree Details"] = degeredeatisl;
                                        drow["Subject Code"] = dvstaffco[spe]["subject_code"].ToString();
                                        drow["Subject Name"] = dvstaffco[spe]["subject_name"].ToString();
                                        drow["Exam"] = exam.ToString();





                                        if (dvstaffco[spe]["ext_appear"].ToString().Trim() != "")
                                        {
                                            absent = Convert.ToDouble(dvstaffco[spe]["ext_appear"].ToString());
                                        }
                                        if (dvstaffco[spe]["in_appear"].ToString().Trim() != "")
                                        {
                                            appear = Convert.ToDouble(dvstaffco[spe]["in_appear"].ToString());
                                        }
                                        if (dvstaffco[spe]["in_total"].ToString().Trim() != "")
                                        {
                                            total = Convert.ToDouble(dvstaffco[spe]["in_total"].ToString());
                                        }
                                        if (dvstaffco[spe]["in_appear"].ToString().Trim() != "")
                                        {
                                            apperar = Convert.ToDouble(dvstaffco[spe]["in_appear"].ToString());
                                        }
                                        if (dvstaffco[spe]["in_pass"].ToString().Trim() != "")
                                        {
                                            pass = Convert.ToDouble(dvstaffco[spe]["in_pass"].ToString());
                                        }
                                        if (dvstaffco[spe]["in_fail"].ToString().Trim() != "")
                                        {
                                            fail = Convert.ToDouble(dvstaffco[spe]["in_fail"].ToString());
                                        }
                                        Double passper = pass / apperar * 100;
                                        if (passper > 100)
                                        {
                                            passper = 100;
                                        }
                                        passper = Math.Round(passper, 2, MidpointRounding.AwayFromZero);
                                        paper = paper + passper;

                                        drow["Total No.of Students"] = total.ToString();
                                        drow["Appear"] = appear.ToString();
                                        drow["Passed"] = pass.ToString();
                                        drow["Absent"] = absent.ToString();
                                        drow["Fail"] = fail.ToString();
                                        drow["Pass %"] = passper.ToString();
                                        data.Rows.Add(drow);


                                    }
                                    paper = paper / (Convert.ToDouble(dvstaffco.Count) * 100) * 100;
                                    paper = Math.Round(paper, 2, MidpointRounding.AwayFromZero);

                                    data.Rows[data.Rows.Count - 1]["Over All Pass %"] = paper.ToString();
                                    dicrowspansubpass.Add(row, dvstaffco.Count);
                                    row = row + dvstaffco.Count;
                                }
                            }
                        }
                    }
                }
                //Double nofoexam = 0, paper = 0;
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    sno++;
                //    string degeredeatisl = ds.Tables[0].Rows[i]["Batch_Year"].ToString() + " - " + ds.Tables[0].Rows[i]["Course_Name"].ToString() + " - " + ds.Tables[0].Rows[i]["department"].ToString() + " - " + ds.Tables[0].Rows[i]["semester"].ToString();
                //    if (ds.Tables[0].Rows[i]["Sections"].ToString().Trim() != "-1" && ds.Tables[0].Rows[i]["Sections"].ToString().Trim() != "")
                //    {
                //        degeredeatisl = degeredeatisl + " - " + ds.Tables[0].Rows[i]["Sections"].ToString();
                //    }
                //    Double total = 0;
                //    Double apperar = 0;
                //    Double pass = 0;
                //    Double fail = 0;
                //    Double absent = 0;
                //    string exam = ds.Tables[0].Rows[i]["internal_exam_type"].ToString();
                //    if (ds.Tables[0].Rows[i]["in_total"].ToString().Trim() != "")
                //    {
                //        total = Convert.ToDouble(ds.Tables[0].Rows[i]["in_total"].ToString());
                //    }
                //    if (ds.Tables[0].Rows[i]["in_appear"].ToString().Trim() != "")
                //    {
                //        apperar = Convert.ToDouble(ds.Tables[0].Rows[i]["in_appear"].ToString());
                //    }
                //    if (ds.Tables[0].Rows[i]["in_pass"].ToString().Trim() != "")
                //    {
                //        pass = Convert.ToDouble(ds.Tables[0].Rows[i]["in_pass"].ToString());
                //    }
                //    if (ds.Tables[0].Rows[i]["in_fail"].ToString().Trim() != "")
                //    {
                //        fail = Convert.ToDouble(ds.Tables[0].Rows[i]["in_fail"].ToString());
                //    }
                //    string subjectno = ds.Tables[0].Rows[i]["subject_no"].ToString();
                //    string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                //    Boolean rowf = false;
                //    if (!hatstaff.Contains(staffcode + subjectno) || ds.Tables[0].Rows.Count - 1 == i)
                //    {
                //        if (hatstaff.Count > 0)
                //        {
                //            if (ds.Tables[0].Rows.Count - 1 == i)
                //            {
                //                if (hatstaff.Contains(staffcode + subjectno))
                //                {
                //                    Double passper1 = pass / apperar * 100;
                //                    if (passper1 > 100)
                //                    {
                //                        passper1 = 100;
                //                    }
                //                    passper1 = Math.Round(passper1, 2, MidpointRounding.AwayFromZero);
                //                    paper = paper + passper1;
                //                    nofoexam = nofoexam + 1;
                //                    FpSpread1.Sheets[0].RowCount++;
                //                    rowf = true;
                //                }
                //            }
                //            Double getper = paper / (nofoexam * 100) * 100;
                //            getper = Math.Round(getper, 2, MidpointRounding.AwayFromZero);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 14, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 1, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 2, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 3, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 4, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 5, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 6, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 7, Convert.ToInt32(nofoexam), 1);
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 14].Text = getper.ToString();
                //            nofoexam = 0;
                //            paper = 0;
                //        }
                //        if (!hatstaff.Contains(staffcode + subjectno))
                //        {
                //            hatstaff.Add(staffcode + subjectno, staffcode + subjectno);
                //        }
                //    }
                //    if (rowf==false)
                //    {
                //        FpSpread1.Sheets[0].RowCount++;
                //    }
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = staffcode;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = degeredeatisl;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = exam;
                //    absent = total - apperar;
                //    Double passper = pass / apperar * 100;
                //    if (passper > 100)
                //    {
                //        passper = 100;
                //    }
                //    passper = Math.Round(passper, 2, MidpointRounding.AwayFromZero);
                //    paper = paper + passper;
                //    nofoexam = nofoexam + 1;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = total.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = pass.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = absent.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = fail.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].Text = passper.ToString();
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //    if (rowf == false)
                //    {
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - Convert.ToInt32(nofoexam), 14].Text = paper.ToString();
                //    }
                //}
            }
            if (setflag == false)
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            else
            {

                Showgrid.DataSource = data;
                Showgrid.DataBind();
                Showgrid.Visible = true;
                divMainContents.Visible = true;

                for (int c = 0; c < chklscolumn.Items.Count; c++)
                {
                    if (chklscolumn.Items[c].Selected == false)
                    {
                        Showgrid.HeaderRow.Cells[c].Visible = false;
                        for (int r = 0; r < data.Rows.Count; r++)
                            Showgrid.Rows[r].Cells[c].Visible = false;
                    }
                }

                //Rowspan
                for (int t = Showgrid.Rows.Count - 1; t > 0; t--)
                {
                    GridViewRow row = Showgrid.Rows[t];
                    GridViewRow previousRow = Showgrid.Rows[t - 1];
                    for (int g = 1; g < data.Columns.Count - 7; g++)
                    {
                        if (row.Cells[g].Text == previousRow.Cells[g].Text)
                        {
                            if (previousRow.Cells[g].RowSpan == 0)
                            {
                                if (row.Cells[g].RowSpan == 0)
                                {
                                    previousRow.Cells[g].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[g].RowSpan = row.Cells[g].RowSpan + 1;
                                }
                                row.Cells[g].Visible = false;
                            }
                        }
                    }
                }

                int col = data.Columns.Count;
                foreach (KeyValuePair<int, int> dr in dicrowspansubpass)
                {
                    int rowstno = dr.Key;
                    int rowspn = dr.Value;
                    int span = rowstno + rowspn;

                    string value = data.Rows[span - 2][col - 1].ToString();
                    Showgrid.Rows[rowstno - 1].Cells[col - 1].Text = value;
                    Showgrid.Rows[rowstno - 1].Cells[col - 1].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[rowstno - 1].Cells[col - 1].RowSpan = rowspn;
                    for (int a = rowstno; a < span - 1; a++)
                    {
                        Showgrid.Rows[a].Cells[col - 1].Visible = false;
                    }

                }
                for (int j = 0; j < Showgrid.Rows.Count; j++)
                    Showgrid.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                int col1 = data.Columns.Count - 7;
                for (int i = col1; i < data.Columns.Count; i++)
                {
                    for (int j = 0; j < Showgrid.Rows.Count; j++)
                        Showgrid.Rows[j].Cells[i].HorizontalAlign = HorizontalAlign.Center;

                }
                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[0].Font.Bold = true;
                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;

            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(Showgrid, "FacultyPerformance.aspx", "Faculty Performance", 0, ss);
        Printcontrol.Visible = true;
    }

    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
            txtexcelname.Text = string.Empty;
            reportname = string.Empty;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Faculty Wise Performance";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}
