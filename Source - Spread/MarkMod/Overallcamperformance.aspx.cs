using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;


public partial class Overallcamperformance : System.Web.UI.Page
{
    string group_code = "", columnfield = "", singleuser = "", usercode = "", collegecode = "", group_user = "";
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DataSet ds2 = new DataSet();
    DataTable dtable = new DataTable();
    DataRow drow = null;
    int count = 0;
    string course_id = "", branch = "", batch = "";
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
        errmsg.Visible = false;
        lblerrexcel.Visible = false;
        Session["QueryString"] = "";
        group_code = Session["group_code"].ToString();
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblerrexcel.Visible = false;
        if (!IsPostBack)
        {
            // ddlTest.Attributes.Add("Size", "5");
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
            ds2 = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds2;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                BindBatch();
                BindDegree();
                BindBranch();
                GetTest();
            }
            else
            {
                errmsg.Text = "Set college rights to the staff";
                errmsg.Visible = true;
                errmsg.Visible = false;
                gridview1.Visible = false;
                return;
            }

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " group_code='" + group_user.ToString().Trim() + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            if (Session["usercode"] != "")
            {
                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                ds2.Reset();
                ds2.Dispose();
                ds2 = d2.select_method(Master1, hat, "Text");
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int set = 0; set < ds2.Tables[0].Rows.Count; set++)
                    {
                        string strdayflag = ds2.Tables[0].Rows[set]["settings"].ToString();
                        string value = ds2.Tables[0].Rows[set]["value"].ToString();

                        if (strdayflag.Trim() == "Roll No" && value.Trim() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (strdayflag.Trim() == "Register No" && value.Trim() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (strdayflag.Trim() == "Student_Type" && value.Trim() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
            clear();
        }
    }
    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            chkbatch.Checked = false;
            txtbatch.Text = "---Select---";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                txtbatch.Text = "Batch (1)";
            }
        }
        catch
        {
        }
    }
    public void BindDegree()
    {
        try
        {
            txtdegree.Text = "---Select---";
            chkdegree.Checked = false;
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
            chklsdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsdegree.DataSource = ds2;
                chklsdegree.DataTextField = "course_name";
                chklsdegree.DataValueField = "course_id";
                chklsdegree.DataBind();
                chklsdegree.Items[0].Selected = true;
                txtdegree.Enabled = true;
                txtdegree.Text = "Degree (1)";
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch
        {

        }
    }
    public void BindBranch()
    {
        try
        {
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
            chklsbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklsdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklsbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbranch.DataSource = ds2;
                chklsbranch.DataTextField = "dept_name";
                chklsbranch.DataValueField = "degree_code";
                chklsbranch.DataBind();
                chklsbranch.Items[0].Selected = true;
                txtbranch.Text = "Branch (1)";
            }
        }
        catch
        {

        }

    }
    public void GetTest()
    {
        try
        {
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = "" + chklsbatch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        batch = batch + "," + "" + chklsbatch.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (batch.Trim() != "" && batch != null)
            {
                batch = " and r.batch_year in(" + batch + ")";
            }
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + chklsbranch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "," + "" + chklsbranch.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (branch.Trim() != "" && branch != null)
            {
                branch = " and r.degree_code in(" + branch + ")";
            }
            ddlTest.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            string Sqlstr = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s,deptprivilages d where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'  and d.Degree_code=r.degree_code and d.Degree_code=s.degree_code and r.college_code='" + collegecode + "' and user_code='" + usercode + "' " + branch + " " + batch + "";
            ds2 = d2.select_method_wo_parameter(Sqlstr, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlTest.Items.Clear();
                ddlTest.DataSource = ds2;
                //ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
                ddlTest.Items.Add("--Select--");
                ddlTest.SelectedIndex = ddlTest.Items.Count - 1;
            }
        }
        catch
        {

        }

    }
    public void clear()
    {
        gridview1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        errmsg.Visible = false;
        // txtexcelname.Text = "";
        Printcontrol.Visible = false;
        //txtexcelname.Text = "";
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        BindBranch();
        GetTest();
        clear();
    }
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
            BindDegree();
            BindBranch();
            GetTest();
        }
        catch
        {
        }
    }
    protected void chklsbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtbatch.Text = "---Select---";
            chkbatch.Checked = false;
            count = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count == chklsbatch.Items.Count)
            {
                txtbatch.Text = "Batch (" + count + ")";
                chkbatch.Checked = true;
            }
            else if (count > 0)
            {
                txtbatch.Text = "Batch (" + count + ")";
            }
            BindDegree();
            BindBranch();
            GetTest();
        }
        catch
        {
        }
    }
    protected void chkdegree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklsdegree.Items.Count; i++)
                {
                    chklsdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklsdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsdegree.Items.Count; i++)
                {
                    chklsdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
            }
            BindBranch();
            GetTest();
        }
        catch
        {
        }
    }
    protected void chklsdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtdegree.Text = "---Select---";
            chkdegree.Checked = false;
            count = 0;
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count == chklsdegree.Items.Count)
            {
                txtdegree.Text = "Degree (" + count + ")";
                chkdegree.Checked = true;
            }
            else if (count > 0)
            {
                txtdegree.Text = "Degree (" + count + ")";
            }
            BindBranch();
            GetTest();
        }
        catch
        {
        }
    }
    protected void chkbranch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklsbranch.Items.Count; i++)
                {
                    chklsbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklsbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbranch.Items.Count; i++)
                {
                    chklsbranch.Items[i].Selected = false;
                }
                txtbranch.Text = "---Select---";
            }
            GetTest();
        }
        catch
        {
        }
    }
    protected void chklsbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            count = 0;
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count == chklsbranch.Items.Count)
            {
                txtbranch.Text = "Degree (" + count + ")";
                chkbranch.Checked = true;
            }
            else if (count > 0)
            {
                txtbranch.Text = "Degree (" + count + ")";
            }
            GetTest();
        }
        catch
        {
        }
    }
    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        clear();
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gridview1.PageIndex = e.NewPageIndex;
            btnGo_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (ddlTest.Items.Count == 0)
            {
                errmsg.Text = "No Test Conducted";
                errmsg.Visible = true;
                return;
            }
            if (ddlTest.SelectedItem.ToString() == "--Select--")
            {
                errmsg.Text = "Please Select Test and then Proceed";
                errmsg.Visible = true;
                return;
            }
            string toprank = txttop.Text.ToString();

            if (toprank.Trim() == "" || toprank == null)
            {
                toprank = "10";
            }
            else
            {
                if (Convert.ToInt32(toprank.Trim()) == 0)
                {
                    errmsg.Text = "Please Enter Value Greater than Zero";
                    errmsg.Visible = true;
                    return;
                }
            }
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = "" + chklsbatch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        batch = batch + "," + "" + chklsbatch.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (batch.Trim() != "" && batch != null)
            {
                batch = " and r.batch_year in(" + batch + ")";
            }
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                if (chklsbranch.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + chklsbranch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "," + "" + chklsbranch.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (branch.Trim() != "" && branch != null)
            {
                branch = " and r.degree_code in(" + branch + ")";
            }
            string insquery = "if exists(select * from sysobjects where name='tbl_mark_calcu' and Type='U') drop table tbl_mark_calcu ;create table tbl_mark_calcu (roll_no nvarchar(25),totalmarks float,percentage float)";
            int a = d2.update_method_wo_parameter(insquery, "Text");
            string test = ddlTest.SelectedItem.ToString();
            string strquery = "select distinct c.criteria,c.criteria_no,r.batch_year,r.degree_code,r.current_semester,r.sections from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0";
            strquery = strquery + "and r.exam_flag<>'debar' AND C.criteria='" + test + "' " + batch + " " + branch + "";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.select_method_wo_parameter(strquery, "Text");
            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
            {
                string criteriano = ds2.Tables[0].Rows[i]["criteria_no"].ToString();
                string strtestmarkquery = "select c.criteria_no,e.subject_no,e.exam_code,r.roll_no,r.marks_obtained,e.min_mark,e.max_mark from exam_type e,result r,criteriaforinternal c where e.exam_code=r.exam_code and c.criteria_no=e.criteria_no and e.criteria_no='" + criteriano + "' order by r.roll_no,subject_no";
                DataSet ds = d2.select_method_wo_parameter(strtestmarkquery, "Text");
                string rollno = "";
                Double totalmarks = 0;
                Double percentage = 0;
                int subcount = 0;
                Boolean failflag = false;
                Double examtotal = 0;
                for (int t = 0; t < ds.Tables[0].Rows.Count; t++)
                {
                    string temproll = ds.Tables[0].Rows[t]["roll_no"].ToString();
                    if (rollno != temproll)
                    {
                        if (rollno != "")
                        {
                            if (failflag == false)
                            {
                                percentage = totalmarks / examtotal * 100;
                                if (percentage > 0)
                                {
                                    percentage = Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
                                    insquery = "if not exists(select * from tbl_mark_calcu where roll_no='" + rollno + "') insert into tbl_mark_calcu (roll_no,totalmarks,percentage) values('" + rollno + "','" + totalmarks + "','" + percentage + "') else update tbl_mark_calcu set totalmarks='" + totalmarks + "',percentage='" + percentage + "' where roll_no='" + rollno + "'";
                                    a = d2.update_method_wo_parameter(insquery, "Text");
                                }
                            }
                        }
                        rollno = temproll;
                        subcount = 0;
                        totalmarks = 0;
                        percentage = 0;
                        examtotal = 0;
                        failflag = false;
                    }
                    subcount++;
                    string marks = ds.Tables[0].Rows[t]["marks_obtained"].ToString();
                    string minmarks = ds.Tables[0].Rows[t]["min_mark"].ToString();
                    if (minmarks.Trim() != "" && minmarks != null)
                    {
                        Double minmark = Convert.ToDouble(ds.Tables[0].Rows[t]["min_mark"].ToString());
                        if (marks != "-2")
                        {
                            Double maxmark = Convert.ToDouble(ds.Tables[0].Rows[t]["max_mark"].ToString());
                            examtotal = examtotal + maxmark;
                        }
                        if (marks.Trim() != "" && marks != null && marks != "-2")
                        {
                            if (minmark > Convert.ToDouble(marks))
                            {
                                failflag = true;
                            }
                            totalmarks = totalmarks + Convert.ToDouble(marks);
                        }
                    }
                    if (t == ds.Tables[0].Rows.Count - 1)
                    {
                        if (failflag == false)
                        {
                            percentage = totalmarks / examtotal * 100;
                            if (percentage == 0)
                            {
                                percentage = Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
                                insquery = "if not exists(select * from tbl_mark_calcu where roll_no='" + rollno + "') insert into tbl_mark_calcu (roll_no,totalmarks,percentage) values('" + rollno + "','" + totalmarks + "','" + percentage + "') else update tbl_mark_calcu set totalmarks='" + totalmarks + "',percentage='" + percentage + "' where roll_no='" + rollno + "'";
                                a = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }

                }
            }
            insquery = "select r.roll_no,r.reg_no,r.stud_name,r.batch_year,case when r.stud_type='Hostler' then 'H' else 'D' end studtpe,r.current_semester,r.sections,totalmarks,percentage,c.Course_Name,de.Dept_Name,de.dept_acronym,r.app_no,d.acronym  from tbl_mark_calcu t,registration r,Degree d,course  c,Department de where r.roll_no=t.roll_no and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and r.roll_no in(SELECT roll_no FROM (SELECT roll_no, totalmarks, dense_Rank() over (ORDER BY percentage DESC ) AS toprank FROM tbl_mark_calcu) rs WHERE toprank <= " + toprank + ") order by t.percentage desc";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.select_method_wo_parameter(insquery, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {


                gridview1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;

                dtable.Columns.Add("sno");
                dtable.Columns.Add("rollno");
                dtable.Columns.Add("regno");
                dtable.Columns.Add("stud_name");
                dtable.Columns.Add("stype");
                dtable.Columns.Add("total");
                dtable.Columns.Add("percentage");
                dtable.Columns.Add("cutof");
                dtable.Columns.Add("branch");

                drow = dtable.NewRow();
                drow["sno"] = "S.No";
                drow["rollno"] = "Roll No";
                drow["regno"] = "Reg No";
                drow["stud_name"] = "Student Name";
                drow["stype"] = "Student Type";
                drow["total"] = "Total Marks";
                drow["percentage"] = "Percentage";
                drow["cutof"] = "Cut Off Mark";
                drow["branch"] = "Branch";

                dtable.Rows.Add(drow);

              
                //svsort = FpSpread1.ActiveSheetView;
                //svsort.AllowSort = true;
                string cutofquery = "select sd.app_no,pm.max_marks as maxma,pm.acual_marks subma,r.roll_no,textval as sub from Stud_prev_details sd,perv_marks_history pm,Registration r,textvaltable t where sd.course_entno=pm.course_entno and r.App_No=sd.app_no and pm.psubjectno=t.TextCode " + batch + " " + branch + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and sd.course_code in(select textcode from textvaltable where textcriteria='cours' and (textval like 'Hsc' or textval like'+%' or textval like'plus%' or textval like'%two%')) and (textval  like '%phy%' or textval  like '%che%' or textval  like '%ma%' )";
                // string cutofquery = "select sd.app_no,SUM(pm.max_marks) as maxma,SUM(pm.acual_marks) as subma from Stud_prev_details sd,perv_marks_history pm,Registration r,textvaltable t where sd.course_entno=pm.course_entno and r.App_No=sd.app_no and pm.psubjectno=t.TextCode and textval not like '%phy%' and textval not like '%che%' and textval not like '%math%' "+batch+" "+branch+" and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar'  group by sd.app_no";
                DataSet dscutoff = d2.select_method_wo_parameter(cutofquery, "Text");
                DataTable dtcut = dscutoff.Tables[0];
                DataView dvcutof = new DataView();
                int sno = 0;
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    string degree = ds2.Tables[0].Rows[i]["Batch_year"].ToString() + '-' + ds2.Tables[0].Rows[i]["Course_Name"].ToString() + '-' + ds2.Tables[0].Rows[i]["acronym"].ToString() + '-' + ds2.Tables[0].Rows[i]["current_semester"].ToString();
                    string sec = ds2.Tables[0].Rows[i]["sections"].ToString();
                    if (sec != null && sec.Trim() != "" && sec.Trim() != "-1")
                    {
                        degree = degree + '-' + sec;
                    }
                    string appno = ds2.Tables[0].Rows[i]["app_no"].ToString();
                    //dtcut.DefaultView.RowFilter = " app_no='"+appno+"'";
                    dvcutof = dtcut.DefaultView;
                    string cutvalue = "-";
                    dtcut.DefaultView.RowFilter = " app_no='" + appno + "' and sub like 'Ma%'";
                    dvcutof = dtcut.DefaultView;
                    Double mathcut = 0;
                    if (dvcutof.Count > 0)
                    {
                        Double maxmark = Convert.ToDouble(dvcutof[0]["maxma"].ToString());
                        Double submark = Convert.ToDouble(dvcutof[0]["subma"].ToString());
                        submark = Math.Round(submark, 0, MidpointRounding.AwayFromZero);
                        maxmark = Math.Round(maxmark, 0, MidpointRounding.AwayFromZero);
                        if (maxmark > 0 && submark > 0)
                        {
                            Double totalmar = submark / maxmark * 100;
                            totalmar = Math.Round(totalmar, 2, MidpointRounding.AwayFromZero);
                            mathcut = totalmar;
                        }
                    }
                    dtcut.DefaultView.RowFilter = " app_no='" + appno + "' and (sub  like '%phy%' or sub  like '%che%')";
                    dvcutof = dtcut.DefaultView;
                    Double othercut = 0;
                    if (dvcutof.Count > 0)
                    {
                        double getma = 0;
                        Double getmax = 0;
                        for (int cus = 0; cus < dvcutof.Count; cus++)
                        {
                            Double maxmark = Convert.ToDouble(dvcutof[cus]["maxma"].ToString());
                            Double submark = Convert.ToDouble(dvcutof[cus]["subma"].ToString());
                            submark = Math.Round(submark, 0, MidpointRounding.AwayFromZero);
                            maxmark = Math.Round(maxmark, 0, MidpointRounding.AwayFromZero);
                            if (maxmark > 0 && submark > 0)
                            {
                                getma = getma + submark;
                                getmax = getmax + maxmark;
                            }
                        }
                        othercut = getma / getmax * 100;
                        othercut = Math.Round(othercut, 2, MidpointRounding.AwayFromZero);
                    }
                    mathcut = mathcut + othercut;
                    if (mathcut > 0)
                    {
                        cutvalue = mathcut.ToString();
                    }

                    drow = dtable.NewRow();
                    drow["sno"] = Convert.ToString(sno);
                    drow["rollno"] = ds2.Tables[0].Rows[i]["Roll_no"].ToString();
                    drow["regno"] = ds2.Tables[0].Rows[i]["reg_no"].ToString();
                    drow["stud_name"] = ds2.Tables[0].Rows[i]["stud_name"].ToString();
                    drow["stype"] = ds2.Tables[0].Rows[i]["studtpe"].ToString();
                    drow["total"] = ds2.Tables[0].Rows[i]["totalmarks"].ToString();
                    drow["percentage"] = ds2.Tables[0].Rows[i]["percentage"].ToString();
                    drow["cutof"] = cutvalue;
                    drow["branch"] = degree;
                    dtable.Rows.Add(drow);
                }
                if (Session["Rollflag"].ToString() == "0")
                {
                    dtable.Columns.Remove("rollno");
                }
                if (Session["Regflag"].ToString() == "0")
                {
                    dtable.Columns.Remove("regno");
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    dtable.Columns.Remove("stype");
                }

                gridview1.DataSource = dtable;
                gridview1.DataBind();
                gridview1.Visible = true;
              
               RowHead(gridview1);
            }
            else
            {
                gridview1.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }

        }
        catch
        {
        }
    }

    protected void RowHead(GridView gridview1)
    {
        for (int head = 0; head < 1; head++)
        {
            gridview1.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gridview1.Rows[head].Font.Bold = true;
            gridview1.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = Convert.ToString(FpEntry.ColumnHeader.RowCount);//
        Session["column_header_row_count"] = Convert.ToString(gridview1.Columns.Count);
     
        // string periods = FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), 7].Tag.ToString();
        // periods = "@Conducted Periods : " + periods + "";
        //string degreedetails = "Overall Attendance Details -Splitup Report" + '@' + "Degree: " + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString() + '-' + "Sem-" + ddlSemYr.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString()+" @Conducted Period (a):"+periods+"";
        string degreedetails =  "Over All Best Performance Report @ Date :" + DateTime.Now.ToString("dd/MM/yyyy") + "";
        string pagename = "Overallcamperformance.aspx";
        //Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);//
        //Printcontrol.Visible = true;
      
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails,0,ss);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {

        try
        {


            Printcontrol.Visible = false;
            string reportname = txtexcelname.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(gridview1, reportname);
                lblrptname.Visible = false;
            }
            else
            {
                lblerrexcel.Text = "Please Enter Your Report Name";
                lblerrexcel.Visible = true;
                txtexcelname.Focus();
            }

        }
        catch (Exception ex)
        {

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

}