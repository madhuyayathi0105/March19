using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Configuration;

public partial class Arrear_Report : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    ArrayList addr = new ArrayList();

    string sql = "", bindbatch1 = "", Chklstbatchvalue = "", bindbranch1 = "", Chklstdegreevalue = "";
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string bran = "", strquery = "", buildvalue = "", build = "";
    string getrollno = "", regularflag = "", genderflag = "", strdayflag = "";
    string onarrear_value = "", totonearrer = "", tottwoarrer = "", totthreearrer = "", totallpass = "", twoarrear_value = "", threearrear_value = "";
    string batchyeartbl = "", cursem = "", degreecodetbl = "", coursename = "", acronym = "", post = "";
    int semarrearcount = 0, first = 0, sl_no = 1;
    string rollno = "", deg = "";
    string stu_name = "", regno = "";
    FarPoint.Web.Spread.StyleInfo style4 = new FarPoint.Web.Spread.StyleInfo();

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
            if (!IsPostBack)
            {
                FpSpread2.Visible = false;
                FpSpread2.Sheets[0].ColumnCount = 7;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                FpSpread2.Sheets[0].AutoPostBack = true;
                FpSpread2.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread2.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread2.CommandBar.Visible = false;

                bindclg();
                bindbatch();
                binddegree();
                bindbranch(bran);
                bindsemester();
                bindsec();
                radiooverall.Checked = true;
            }
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            ds = da.select_method_wo_parameter(Master1, "text");
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
        catch (Exception ex)
        {
        }
    }
    public void bindclg()
    {
        try
        {
            string columnfield = "";
            usercode = Session["UserCode"].ToString();
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
            has.Clear();
            has.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", has, "sp");
            dropcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                dropcollege.DataSource = ds;
                dropcollege.DataTextField = "collname";
                dropcollege.DataValueField = "college_code";
                dropcollege.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsec()
    {
        try
        {
            dropsec.Enabled = false;
            dropsec.Items.Clear();
            has.Clear();
            string bindvalue = "";
            for (int b = 0; b < chklst_branch.Items.Count; b++)
            {
                if (chklst_branch.Items[b].Selected == true)
                {
                    if (bindvalue.Trim() == "")
                    {
                        bindvalue = chklst_branch.Items[b].Value;
                    }
                    else
                    {
                        bindvalue = bindvalue + ',' + chklst_branch.Items[b].Value;
                    }
                }
            }
            string bindbatch = "";
            for (int b = 0; b < Chklst_batch.Items.Count; b++)
            {
                if (Chklst_batch.Items[b].Selected == true)
                {
                    if (bindbatch.Trim() == "")
                    {
                        bindbatch = Chklst_batch.Items[b].Value;
                    }
                    else
                    {
                        bindbatch = bindbatch + ',' + Chklst_batch.Items[b].Value;
                    }
                }
            }

            if (bindbatch != "" && bindvalue != "")
            {
                string jk = "select distinct sections from registration where batch_year in(" + bindbatch + ") and degree_code in(" + bindvalue + ") and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
                ds = da.select_method_wo_parameter("select distinct sections from registration where batch_year in(" + bindbatch + ") and degree_code in(" + bindvalue + ") and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'", "text");
                int count5 = ds.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    dropsec.DataSource = ds;
                    dropsec.DataTextField = "sections";
                    dropsec.DataValueField = "sections";
                    dropsec.DataBind();
                    dropsec.Enabled = true;
                    dropsec.Items.Insert(0, "All");
                }
            }
            else
            {
                dropsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindsemester()
    {
        try
        {
            dropsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strbranch = "";
            for (int b = 0; b < chklst_branch.Items.Count; b++)
            {
                if (chklst_branch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = chklst_branch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + chklst_branch.Items[b].Value;
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + dropcollege.SelectedValue.ToString() + " " + strbranch + " order by NDurations desc";

            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i < duration; i++)
                {
                    if (first_year == false)
                    {
                        dropsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        dropsem.Items.Add(i.ToString());
                    }

                }
                dropsec.Enabled = true;
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + dropcollege.SelectedValue.ToString() + " " + strbranch + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i < duration; i++)
                    {
                        if (first_year == false)
                        {
                            dropsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            dropsem.Items.Add(i.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void dropsem_selected(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            Label2.Visible = false;
            gridviewreport.Visible = false;
            Excel.Visible = false;
            print.Visible = false;
            Label1.Visible = false;
            FpSpread1.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            FpSpread2.Visible = false;
            excelspread.Visible = false;
            pdf.Visible = false;
        }
        catch (Exception ex)
        { }
    }

    protected void dropsec_selected(object sender, EventArgs e)
    {
        gridviewreport.Visible = false;
        Excel.Visible = false;
        Label2.Visible = false;
        print.Visible = false;
        Label1.Visible = false;
        FpSpread1.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        FpSpread2.Visible = false;
        excelspread.Visible = false;
        pdf.Visible = false;
    }
    protected void dropReport_selected(object sender, EventArgs e)
    {
        gridviewreport.Visible = false;
        Excel.Visible = false;
        print.Visible = false;
        Label1.Visible = false;
        FpSpread1.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
        FpSpread2.Visible = false;
        excelspread.Visible = false;
        pdf.Visible = false;
        Label2.Visible = false;
    }

    public void bindbatch()
    {
        try
        {
            txt_batch.Text = "Batch(" + (1) + ")";
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                Chklst_batch.DataSource = ds;
                Chklst_batch.DataTextField = "batch_year";
                Chklst_batch.DataValueField = "batch_year";
                Chklst_batch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                Chklst_batch.SelectedValue = max_bat.ToString();
            }
        }
        catch (Exception ex)
        { }
    }

    public void binddegree()
    {
        try
        {
            txt_degree.Text = "Degree(" + (5) + ")";
            usercode = Session["usercode"].ToString();
            collegecode = dropcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {

                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();
            }
            int v = 0;
            if (count1 > 0)
            {
                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    v++;
                    Chklst_degree.Items[h].Selected = true;
                    txt_degree.Text = "Degree " + "(" + v + ")";
                }
            }
        }
        catch (Exception ex)
        { }
    }

    public void bindbranch(string mainvalue)
    {
        try
        {
            int count = 0;
            chklst_branch.Items.Clear();
            if (mainvalue.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), mainvalue, dropcollege.SelectedItem.Value, Session["usercode"].ToString());
            }
            else
            {
                ds = da.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code='" + dropcollege.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code", "text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_branch.DataSource = ds;
                chklst_branch.DataTextField = "dept_name";
                chklst_branch.DataValueField = "degree_code";
                chklst_branch.DataBind();
            }
            if (chklst_branch.Items.Count > 0)
            {

                for (int h = 0; h < chklst_branch.Items.Count; h++)
                {
                    count++;
                    chklst_branch.Items[h].Selected = true;
                }
                txt_branch.Text = "Dept(" + (count) + ")";
                bindsemester();
                bindsec();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            chk_degree.Checked = false;

            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_degree.Text = "--Select--";
                    build = Chklst_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + build;

                    }
                }
            }
            bindbranch(buildvalue);
            //bindsemester();
            bindsec();

            if (seatcount == Chklst_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                chk_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }

        }

        catch (Exception ex)
        { }
    }

    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_branch.Checked == true)
            {

                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                    txt_branch.Text = "Dept(" + (chklst_branch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                    txt_branch.Text = "--Select--";
                }
            }
            bindsemester();
            bindsec();
        }
        catch (Exception ex)
        {
        }

    }


    protected void Chlk_batchchanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_batch.Checked == true)
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {

                    Chklst_batch.Items[i].Selected = true;
                    txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
                }
            }

            else
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = false;

                    txt_batch.Text = "--Select--";
                }
            }
            //bindsemester();
            bindsec();
        }
        catch (Exception ex)
        { }

    }

    protected void Chlk_batchselected(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            Chk_batch.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = Chklst_batch.Items[i].Value.ToString();
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

            //bindsemester();
            bindsec();
            if (seatcount == Chklst_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                Chk_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        { }
    }


    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            chk_branch.Checked = false;
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == chklst_branch.Items.Count)
            {
                txt_branch.Text = "Dept(" + seatcount.ToString() + ")";
                chk_branch.Checked = true;
            }

            else if (seatcount == 0)
            {
                txt_branch.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Dept(" + seatcount.ToString() + ")";
            }
            bindsemester();
            bindsec();
        }
        catch (Exception ex)
        {
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string Chklstbatchvalue = "";
            string bind1 = "";

            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    if (chk_degree.Checked == true)
                    {
                        Chklst_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
                        bind1 = Chklst_degree.Items[i].Value.ToString();
                        if (Chklstbatchvalue == "")
                        {
                            Chklstbatchvalue = bind1;
                        }
                        else
                        {
                            Chklstbatchvalue = Chklstbatchvalue + "," + bind1;
                        }
                    }
                    else if (chk_degree.Checked == false)
                    {
                    }
                }
                bindbranch(buildvalue);
                //bindsemester();
                bindsec();
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    Chklst_degree.ClearSelection();
                    chk_branch.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void radio2_Checked(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Label2.Visible = false;
            gridviewreport.Visible = false;
            Excel.Visible = false;
            print.Visible = false;
            Label1.Visible = false;
            FpSpread1.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            FpSpread2.Visible = false;
            excelspread.Visible = false;
            pdf.Visible = false;
        }
        catch (Exception ex)
        { }
    }


    protected void radio1_checked(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            Label2.Visible = false;
            gridviewreport.Visible = false;
            Excel.Visible = false;
            print.Visible = false;
            Label1.Visible = false;
            FpSpread1.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            FpSpread2.Visible = false;
            excelspread.Visible = false;
            pdf.Visible = false;
        }
        catch (Exception ex)
        { }
    }


    protected void buttongo(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (radiooverall.Checked == true)
            {
                Label2.Visible = false;
                FpSpread2.Visible = false;
                excelspread.Visible = false;
                pdf.Visible = false;
                FpSpread1.Visible = false;
                Printcontrol.Visible = false;
                go();
            }
            else  ///added by jeyagandhi ////
            {
                Boolean present = false;
                Label2.Visible = false;
                Label1.Visible = false;
                FpSpread2.Visible = false;
                excelspread.Visible = false;
                pdf.Visible = false;
                string sem = "", subcode = "", sname = "", attempts = "";
                gridviewreport.Visible = false;
                btn();
                string getrollno = " select distinct r.roll_no,r.Reg_No,r.stud_name, r.degree_code,r.Batch_Year,c.course_name, e.current_semester,g.acronym, textval,TextCode from mark_entry m,Exam_Details e,course c, Registration r,applyn a, textvaltable t,Degree g where e.exam_code=m.exam_code  and m.roll_no=r.Roll_No and  c.course_id=g.course_id  and e.degree_code=r.degree_code and  r.App_No = a.app_no   and a.seattype = t.TextCode and r.degree_code = g.degree_code and r.cc=0 and    r.exam_flag <>'DEBAR' and  r.delflag=0 and m.attempts=1 and r.college_code='" + dropcollege.SelectedValue.ToString() + "'  and r.Batch_Year in ('" + Chklstbatchvalue + "') AND g.degree_code in ('" + Chklstdegreevalue + "') AND e.Current_Semester in ('" + dropsem.SelectedItem.Text + "') ";
                if (dropsec.Enabled == true)
                {
                    if (dropsec.SelectedItem.Text != "All")
                    {
                        getrollno = getrollno + "and Sections='" + dropsec.SelectedItem.Text + "'";

                        if (dropReport.SelectedItem.Text == "Lateral Entry")
                        {
                            getrollno = getrollno + " and r.mode=2";
                        }
                        else if (dropReport.SelectedItem.Text == "Hostel Students")
                        {
                            getrollno = getrollno + "and r.Stud_Type='Hostler' ";
                        }
                    }
                    else
                    {
                        if (dropReport.SelectedItem.Text == "Lateral Entry")
                        {
                            getrollno = getrollno + " and r.mode=2";
                        }
                        else if (dropReport.SelectedItem.Text == "Hostel Students")
                        {
                            getrollno = getrollno + "and r.Stud_Type='Hostler' ";
                        }
                    }
                }
                else
                {
                    if (dropReport.SelectedItem.Text == "Lateral Entry")
                    {
                        getrollno = getrollno + " and r.mode=2";
                    }
                    else if (dropReport.SelectedItem.Text == "Hostel Students")
                    {
                        getrollno = getrollno + "and r.Stud_Type='Hostler' ";
                    }
                }
                getrollno = getrollno + "  and delflag =0 and exam_flag <>'Debar'  order by g.acronym ";
                DataSet dsgetrollno = new DataSet();
                dsgetrollno = da.select_method_wo_parameter(getrollno, "text");
                Hashtable htb = new Hashtable();
                FpSpread2.Sheets[0].RowCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 0;
                FpSpread2.Sheets[0].ColumnCount = 7;
                FpSpread2.Sheets[0].AllowTableCorner = true;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.White;
                style2.BackColor = Color.Teal;
                FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FarPoint.Web.Spread.TextCellType integercell = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType integercell1 = new FarPoint.Web.Spread.TextCellType();
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].Columns[0].Width = 50;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread2.Sheets[0].Columns[1].Width = 150;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread2.Sheets[0].Columns[2].Width = 150;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread2.Sheets[0].Columns[3].Width = 320;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subjects";
                FpSpread2.Sheets[0].Columns[4].Width = 480;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Attempts";
                FpSpread2.Sheets[0].Columns[5].Width = 75;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
                FpSpread2.Sheets[0].Columns[6].Width = 75;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

                if (dsgetrollno.Tables[0].Rows.Count > 0)
                {
                    for (int loop = 0; loop < dsgetrollno.Tables[0].Rows.Count; loop++)
                    {
                        batchyeartbl = dsgetrollno.Tables[0].Rows[loop]["batch_year"].ToString();
                        degreecodetbl = dsgetrollno.Tables[0].Rows[loop]["degree_code"].ToString();
                        rollno = dsgetrollno.Tables[0].Rows[loop]["roll_no"].ToString();
                        stu_name = dsgetrollno.Tables[0].Rows[loop]["stud_name"].ToString();
                        regno = dsgetrollno.Tables[0].Rows[loop]["Reg_No"].ToString();
                        acronym = dsgetrollno.Tables[0].Rows[loop]["acronym"].ToString();
                        coursename = dsgetrollno.Tables[0].Rows[loop]["course_name"].ToString();
                        string defg = degreecodetbl;
                        if (!htb.ContainsKey(dsgetrollno.Tables[0].Rows[loop]["degree_code"].ToString()))
                        {
                            htb.Add(dsgetrollno.Tables[0].Rows[loop]["degree_code"].ToString(), dsgetrollno.Tables[0].Rows[loop]["degree_code"].ToString());
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 7);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = batchyeartbl + "-" + coursename + "-" + acronym;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.LightYellow;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        }
                        if (Session["Rollflag"].ToString() == "1")
                        {
                            FpSpread2.Sheets[0].Columns[1].Visible = true;
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Columns[1].Visible = false;
                        }
                        if (Session["Regflag"].ToString() == "1")
                        {
                            FpSpread2.Sheets[0].Columns[2].Visible = true;
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Columns[2].Visible = false;
                        }
                        FpSpread2.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

                        FpSpread2.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

                        string studcal = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name, s.subject_code, e.Current_Semester,m.attempts,r.Batch_Year,r.degree_code from mark_entry m,Exam_Details e,Registration r , subject s,subjectChooser c  where e.exam_code=m.exam_code and   m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year     and s.subject_no =c.subject_no and  c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no    and r.college_code='" + Session["collegecode"].ToString() + "' and r.Batch_Year in('" + Chklstbatchvalue + "') and e.current_semester<='" + dropsem.SelectedItem.Text + "'       and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in     ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + Chklstdegreevalue + "')      and r.Roll_No in('" + rollno + "') ORDER BY  e.current_semester desc  ";
                        DataSet dsarrsub = da.select_method_wo_parameter(studcal, "text");
                        if (dsarrsub.Tables[0].Rows.Count > 0)
                        {
                            for (int arrsubcount = 0; arrsubcount < dsarrsub.Tables[0].Rows.Count; arrsubcount++)
                            {
                                present = true;
                                semarrearcount = dsarrsub.Tables[0].Rows.Count;
                                if (post == "")
                                {
                                    sname = dsarrsub.Tables[0].Rows[arrsubcount]["subject_name"].ToString();
                                    subcode = dsarrsub.Tables[0].Rows[arrsubcount]["subject_code"].ToString();
                                    sem = dsarrsub.Tables[0].Rows[arrsubcount]["Current_Semester"].ToString();
                                    attempts = dsarrsub.Tables[0].Rows[arrsubcount]["attempts"].ToString();
                                }
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = integercell;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = rollno.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = integercell;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                if (regno.Trim() == "")
                                {
                                    regno = "-";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                }

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = integercell1;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = regno.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = integercell1;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = stu_name.ToString();

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = sname.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = attempts.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = sem.ToString();
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            }

                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 4);
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 5, 1, 3);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = "    No.of Papers : " + semarrearcount;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.Tomato;
                            sl_no++;
                        }
                        else
                        {
                            semarrearcount = 0;
                            lblError.Visible = true;
                            lblError.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            excelspread.Visible = false;
                            pdf.Visible = false;
                        }

                        FpSpread2.Visible = true;
                        pdf.Visible = true;
                        excelspread.Visible = true;
                        btnprint.Visible = false;
                        btnexcel.Visible = false;
                        FpSpread1.Visible = false;
                        Label2.Visible = false;
                        Label1.Visible = false;
                        Excel.Visible = false;
                        print.Visible = false;
                        lblError.Visible = false;
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    }
                }
                else
                {
                    Excel.Visible = false;
                    print.Visible = false;
                    lblError.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    Label1.Visible = false;
                    Label2.Visible = false;
                    FpSpread2.Visible = false;
                    lblError.Visible = true;
                    excelspread.Visible = false;
                    pdf.Visible = false;
                }

                if (present == false)
                {
                    Excel.Visible = false;
                    print.Visible = false;
                    lblError.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    Label1.Visible = false;
                    Label2.Visible = false;
                    FpSpread2.Visible = false;
                    lblError.Visible = true;
                    excelspread.Visible = false;
                    pdf.Visible = false;
                }
            }
        }
        catch (Exception ex)    ///added by jeyagandhi ////
        {
            lblError.Text = ex.ToString();
            lblError.Visible = true;
        }
    }

    public void go()   ///added by jeyagandhi ////
    {
        try
        {
            int gh = 0;
            int mn = 0;
            Label1.Visible = false;
            FpSpread2.Visible = false;
            FpSpread1.Visible = false;
            btnprint.Visible = false;
            btnexcel.Visible = false;
            print.Visible = false;
            Excel.Visible = false;
            Label2.Visible = false;
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            DataView dv2 = new DataView();
            DataTable dt = new DataTable();
            ArrayList add = new ArrayList();
            ArrayList add1 = new ArrayList();
            int allsub = 0;
            int onesub = 0;
            int twosub = 0;
            int abovethreesub = 0;
            double passper = 0;
            string finalpercent = "0";
            int total = 0;
            int deg = 0;
            string rollc = "";
            DataSet dn = new DataSet();
            sql = "select distinct r.degree_code,r.Batch_Year,c.course_name,e.current_semester,g.acronym,textval,TextCode from mark_entry m,course c,Exam_Details e,Registration r,applyn a,textvaltable t,Degree g where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and c.course_id=g.course_id and e.degree_code=r.degree_code and  r.App_No = a.app_no and a.seattype = t.TextCode  and r.degree_code = g.degree_code and r.cc=0 and  r.exam_flag <>'DEBAR' and  r.delflag=0 and m.attempts=1 and r.college_code=" + dropcollege.SelectedValue + "  ";
            btn();
            if (dropsec.Enabled == true)
            {
                sql = sql + " select distinct r.Roll_No,r.degree_code,e.Current_Semester,r.Batch_Year,t.TextCode from mark_entry m,Exam_Details e,Registration r,textvaltable t,applyn a where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No  and r.App_No=a.app_no and a.seattype=t.TextCode and r.college_code=" + dropcollege.SelectedValue + "  and r.Batch_Year in ('" + Chklstbatchvalue + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "')   ";
                if (dropsec.SelectedItem.Text != "All")
                {
                    sql = sql + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                }
                if (dropReport.SelectedItem.Text == "Lateral Entry")
                {
                    sql = sql + "and r.mode=2 ";
                }

                else if (dropReport.SelectedItem.Text == "Hostel Students")
                {
                    sql = sql + "and r.Stud_Type='Hostler' ";
                }
                sql = sql + " select  r.Roll_No,e.Current_Semester,result from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + Chklstbatchvalue + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + Chklstdegreevalue + "') ";

                if (dropsec.SelectedItem.Text != "All")
                {
                    sql = sql + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                }

                string arrer = "select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where  m.roll_no=r.Roll_No and e.exam_code=m.exam_code and  e.batch_year=r.Batch_Year and r.Batch_Year in ('" + Chklstbatchvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR' and  r.delflag=0 and m.attempts=1 and result in ('fail','AAA','WHD') and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "')";
                if (dropsec.SelectedItem.Text != "All")
                {
                    arrer = arrer + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                }

                dn = da.select_method_wo_parameter(arrer, "text");
                if (dn.Tables[0].Rows.Count > 0)
                {
                    for (int g = 0; g < dn.Tables[0].Rows.Count; g++)
                    {
                        if (rollc == "")
                        {
                            rollc = dn.Tables[0].Rows[g]["roll_no"].ToString();
                        }
                        else
                        {
                            rollc = rollc + "'" + "," + "'" + dn.Tables[0].Rows[g]["roll_no"].ToString();
                        }
                    }
                }

                string rollnm = "";
                if (rollc != "")
                {
                    rollnm = " and m.roll_no not in ( '" + rollc + "')";
                }
                sql = sql + " select  r.Roll_No,e.Current_Semester,result from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year in ('" + Chklstbatchvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR' and e.exam_code=m.exam_code  and r.delflag=0 and result='pass' and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "') " + rollnm;

                if (dropsec.SelectedItem.Text != "All")
                {
                    sql = sql + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                }
            }
            else
            {
                sql = sql + " select distinct r.Roll_No,r.degree_code,e.Current_Semester,r.Batch_Year,t.TextCode from mark_entry m,Exam_Details e,Registration r,textvaltable t,applyn a where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.App_No=a.app_no and a.seattype=t.TextCode and r.college_code=" + dropcollege.SelectedValue + "  and r.Batch_Year in ('" + Chklstbatchvalue + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "')  ";
                if (dropReport.SelectedItem.Text == "Lateral Entry")
                {
                    sql = sql + "and r.mode=2 ";
                }

                else if (dropReport.SelectedItem.Text == "Hostel Students")
                {
                    sql = sql + "and r.Stud_Type='Hostler' ";
                }
                sql = sql + " select  r.Roll_No,e.Current_Semester,result from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + Chklstbatchvalue + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + Chklstdegreevalue + "') ";
                string arrer = " select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where m.roll_no=r.Roll_No and e.exam_code=m.exam_code and  e.batch_year=r.Batch_Year and r.Batch_Year in ('" + Chklstbatchvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "')";
                dn = da.select_method_wo_parameter(arrer, "text");
                if (dn.Tables[0].Rows.Count > 0)
                {
                    for (int g = 0; g < dn.Tables[0].Rows.Count; g++)
                    {
                        if (rollc == "")
                        {
                            rollc = dn.Tables[0].Rows[g]["roll_no"].ToString();
                        }
                        else
                        {
                            rollc = rollc + "'" + "," + "'" + dn.Tables[0].Rows[g]["roll_no"].ToString();
                        }
                    }
                }

                string rollnm = "";
                if (rollc != "")
                {
                    rollnm = " and m.roll_no not in ( '" + rollc + "')";
                }
                sql = sql + " select  r.Roll_No,e.Current_Semester,result from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year in ('" + Chklstbatchvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR' and e.exam_code=m.exam_code  and r.delflag=0 and result='pass' and m.attempts=1  AND r.degree_code in ('" + Chklstdegreevalue + "')  " + rollnm;
            }
            ds = da.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                dt.Columns.Add("sno", typeof(string));
                dt.Columns.Add("batch_Year", typeof(string));
                dt.Columns.Add("Department", typeof(string));
                dt.Columns.Add("Quota", typeof(string));
                dt.Columns.Add("All Clear", typeof(string));
                dt.Columns.Add("One Arrear", typeof(string));
                dt.Columns.Add("Two Arrear", typeof(string));
                dt.Columns.Add("3 & above Arrear", typeof(string));
                dt.Columns.Add("Pass Percentage", typeof(string));
                dt.Columns.Add("degreecode", typeof(string));
                dt.Columns.Add("textcode", typeof(string));
                dt.Columns.Add("roll_no", typeof(string));
                dt.Columns.Add("roll_no1", typeof(string));
                dt.Columns.Add("roll_no2", typeof(string));
                dt.Columns.Add("rollpass", typeof(string));
                dt.Columns.Add("Sub_code1", typeof(string));
                dt.Columns.Add("Sub_code2", typeof(string));
                dt.Columns.Add("Sub_code_above", typeof(string));

                int count = 0;
                Hashtable htb = new Hashtable();
                for (deg = 0; deg <= ds.Tables[0].Rows.Count; deg++)
                {
                    DataRow dr1 = null;
                    bool test = false;
                    if (deg != 0)
                    {
                        if (deg == ds.Tables[0].Rows.Count)
                        {
                            {
                                if (!htb.ContainsKey(ds.Tables[0].Rows[deg - 1]["degree_code"].ToString()))
                                {
                                    dr1 = dt.NewRow();
                                    test = true;
                                    htb.Add(ds.Tables[0].Rows[deg - 1]["degree_code"].ToString(), ds.Tables[0].Rows[deg - 1]["degree_code"].ToString());
                                    dr1[0] = "Total";

                                    if (test == true)
                                    {
                                        gh = 1;
                                        dt.Rows.Add(dr1);
                                        add1.Add(dt.Rows.Count + "-" + "Total");

                                        if (allsub != Convert.ToInt32("0"))
                                        {
                                            dr1[4] = Convert.ToString(allsub);
                                            dr1[14] = Convert.ToString(totallpass);
                                            totallpass = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[4] = "0";
                                        }
                                        if (onesub != Convert.ToInt32("0"))
                                        {
                                            dr1[5] = Convert.ToString(onesub);
                                            dr1[11] = Convert.ToString(totonearrer);
                                            totonearrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[5] = "0";
                                        }
                                        if (twosub != Convert.ToInt32("0"))
                                        {
                                            dr1[6] = Convert.ToString(twosub);
                                            dr1[12] = Convert.ToString(tottwoarrer);
                                            tottwoarrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[6] = "0";
                                        }
                                        if (abovethreesub != Convert.ToInt32("0"))
                                        {
                                            dr1[7] = Convert.ToString(abovethreesub);
                                            dr1[13] = Convert.ToString(totthreearrer);
                                            totthreearrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[7] = "0";
                                        }
                                        if (finalpercent != ("0"))
                                        {
                                            dr1[8] = "-";

                                            dr1[8] = Convert.ToString(finalpercent);
                                        }

                                        else if (finalpercent == "0")
                                        {
                                            dr1[8] = "0";
                                        }
                                    }
                                    allsub = 0;
                                    onesub = 0;
                                    twosub = 0;
                                    abovethreesub = 0;
                                    passper = 0;
                                }
                            }
                        }
                        else
                        {
                            if ((ds.Tables[0].Rows[deg]["degree_code"].ToString()) != (ds.Tables[0].Rows[deg - 1]["degree_code"].ToString()))
                            {
                                if (!htb.ContainsKey(ds.Tables[0].Rows[deg]["degree_code"].ToString()))
                                {
                                    dr1 = dt.NewRow();
                                    test = true;
                                    htb.Add(ds.Tables[0].Rows[deg - 1]["degree_code"].ToString(), ds.Tables[0].Rows[deg - 1]["degree_code"].ToString());
                                    dr1[0] = "Total";

                                    if (test == true)
                                    {
                                        dt.Rows.Add(dr1);
                                        add1.Add(dt.Rows.Count + "-" + "Total");

                                        if (allsub != Convert.ToInt32("0"))
                                        {
                                            dr1[4] = Convert.ToString(allsub);
                                            dr1[14] = Convert.ToString(totallpass);
                                            totallpass = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[4] = "0";
                                        }
                                        if (onesub != Convert.ToInt32("0"))
                                        {
                                            dr1[5] = Convert.ToString(onesub);
                                            dr1[11] = Convert.ToString(totonearrer);
                                            totonearrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[5] = "0";
                                        }
                                        if (twosub != Convert.ToInt32("0"))
                                        {
                                            dr1[6] = Convert.ToString(twosub);
                                            dr1[12] = Convert.ToString(tottwoarrer);
                                            tottwoarrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[6] = "0";
                                        }
                                        if (abovethreesub != Convert.ToInt32("0"))
                                        {
                                            dr1[7] = Convert.ToString(abovethreesub);
                                            dr1[13] = Convert.ToString(totthreearrer);
                                            totthreearrer = "";
                                            mn = 1;
                                        }
                                        else
                                        {
                                            dr1[7] = "0";
                                        }
                                        if (finalpercent != ("0"))
                                        {
                                            dr1[8] = "-";

                                            dr1[8] = Convert.ToString(finalpercent);
                                        }

                                        else if (finalpercent == "0")
                                        {
                                            dr1[8] = "0";
                                        }
                                    }
                                    allsub = 0;
                                    onesub = 0;
                                    twosub = 0;
                                    abovethreesub = 0;
                                    passper = 0;
                                }
                            }
                        }
                    }
                    if (gh == 0)
                    {
                        DataRow dr = null;
                        int pass = 0;
                        int j = 0;
                        int k = 0;
                        int m = 0;
                        string scode = "";
                        string scode1 = "";
                        string scode2 = "";
                        string sem = ds.Tables[0].Rows[deg]["current_semester"].ToString();
                        string batch_year = ds.Tables[0].Rows[deg]["Batch_Year"].ToString();
                        string degree_code = ds.Tables[0].Rows[deg]["degree_code"].ToString();
                        string dept = ds.Tables[0].Rows[deg]["acronym"].ToString();
                        string str1 = dept;

                        string allpass = "";
                        ArrayList arrsub1 = new ArrayList();
                        ArrayList arrsub2 = new ArrayList();
                        ArrayList arrsub3 = new ArrayList();
                        dr = dt.NewRow();
                        if (!addr.Contains(str1))
                        {
                            count++;
                            addr.Add(str1);
                        }
                        dr[0] = Convert.ToString(count);
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "degree_code=" + degree_code + " and Batch_Year=" + batch_year + " and Current_Semester=" + sem + " and TextCode=" + ds.Tables[0].Rows[deg]["TextCode"].ToString();
                            dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int roll = 0; roll < dv.Count; roll++)
                                {
                                    string roll_number = Convert.ToString(dv[roll]["roll_no"]);

                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        ds.Tables[2].DefaultView.RowFilter = "roll_no='" + roll_number + "'";
                                        dv1 = ds.Tables[2].DefaultView;

                                        if (dv1.Count > 0)
                                        {
                                            if (dv1.Count == 1)
                                            {
                                                if (onarrear_value.Trim() == "")
                                                {
                                                    onarrear_value = roll_number;
                                                    if (totonearrer.Trim() == "")
                                                    {
                                                        totonearrer = roll_number;
                                                    }
                                                    else
                                                    {
                                                        totonearrer = totonearrer + "'" + "," + "'" + roll_number;
                                                    }
                                                }
                                                else
                                                {
                                                    onarrear_value = onarrear_value + "'" + "," + "'" + roll_number;
                                                    totonearrer = totonearrer + "'" + "," + "'" + roll_number;
                                                }
                                                j = j + 1;
                                            }

                                            if (dv1.Count == 2)
                                            {
                                                if (twoarrear_value.Trim() == "")
                                                {
                                                    twoarrear_value = roll_number;
                                                    if (tottwoarrer.Trim() == "")
                                                    {
                                                        tottwoarrer = roll_number;
                                                    }
                                                    else
                                                    {
                                                        tottwoarrer = tottwoarrer + "'" + "," + "'" + roll_number;
                                                    }
                                                }
                                                else
                                                {
                                                    twoarrear_value = twoarrear_value + "'" + "," + "'" + roll_number;
                                                    tottwoarrer = tottwoarrer + "'" + "," + "'" + roll_number;
                                                }

                                                k = k + 1;
                                            }

                                            if (dv1.Count >= 3)
                                            {

                                                if (threearrear_value.Trim() == "")
                                                {
                                                    threearrear_value = roll_number;
                                                    if (totthreearrer.Trim() == "")
                                                    {
                                                        totthreearrer = roll_number;
                                                    }
                                                    else
                                                    {
                                                        totthreearrer = totthreearrer + "'" + "," + "'" + roll_number;
                                                    }
                                                }
                                                else
                                                {
                                                    threearrear_value = threearrear_value + "'" + "," + "'" + roll_number;
                                                    totthreearrer = totthreearrer + "'" + "," + "'" + roll_number;
                                                }
                                                m = m + 1;
                                            }
                                        }
                                    }

                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        ds.Tables[3].DefaultView.RowFilter = "roll_no='" + roll_number + "' and Current_Semester ='" + sem + "'";
                                        dv2 = ds.Tables[3].DefaultView;
                                        if (dv2.Count > 0)
                                        {
                                            if (allpass.Trim() == "")
                                            {
                                                allpass = roll_number;
                                                if (totallpass.Trim() == "")
                                                {
                                                    totallpass = roll_number;
                                                }
                                                else
                                                {
                                                    totallpass = totallpass + "'" + "," + "'" + roll_number;
                                                }
                                            }
                                            else
                                            {
                                                allpass = allpass + "'" + "," + "'" + roll_number;
                                                totallpass = totallpass + "'" + "," + "'" + roll_number;
                                            }

                                            pass = pass + 1;
                                        }
                                    }
                                    lblError.Visible = true;
                                    FpSpread1.Visible = false;
                                    gridviewreport.Visible = false;
                                }
                            }
                        }
                        dr[1] = Convert.ToString(batch_year);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[deg]["course_name"].ToString()) + "-" + Convert.ToString(ds.Tables[0].Rows[deg]["acronym"].ToString());
                        dr[3] = Convert.ToString(ds.Tables[0].Rows[deg]["textval"].ToString());
                        dr[4] = Convert.ToString(pass);
                        dr[5] = Convert.ToString(j);
                        dr[6] = Convert.ToString(k);
                        dr[7] = Convert.ToString(m);
                        dr[11] = Convert.ToString(onarrear_value);
                        onarrear_value = "";
                        dr[12] = Convert.ToString(twoarrear_value);
                        twoarrear_value = "";
                        dr[13] = Convert.ToString(threearrear_value);
                        threearrear_value = "";
                        dr[14] = Convert.ToString(allpass);
                        allpass = "";
                        dr[15] = Convert.ToString(scode);
                        dr[16] = Convert.ToString(scode1);
                        dr[17] = Convert.ToString(scode2);

                        string allclear1 = pass.ToString();
                        int arrear1 = j + k + m;
                        int total1 = (Convert.ToInt32(allclear1) + (Convert.ToInt32(arrear1)));
                        double passpercentage1 = 0;
                        passpercentage1 = (((Convert.ToDouble(allclear1)) / (Convert.ToDouble(total1)) * 100));
                        double passper1 = Math.Round(passpercentage1, 2);
                        string finalpercent1 = Convert.ToString(passper1);
                        dr[8] = Convert.ToString(passper1);
                        dr[9] = Convert.ToString(degree_code);
                        string textcode1 = Convert.ToString(ds.Tables[0].Rows[deg]["TextCode"]);
                        dr[10] = textcode1;

                        dt.Rows.Add(dr);

                        if (pass == 0)
                        {
                            dr[8] = "0   ";
                            dr[4] = " 0  ";

                        }

                        if (j == 0)
                        {
                            dr[5] = "0  ";
                        }

                        if (k == 0)
                        {
                            dr[6] = " 0 ";
                        }

                        if (m == 0)
                        {
                            dr[7] = "0";
                        }

                        if (allsub == 0)
                        {
                            allsub = pass;
                        }
                        else if (allsub != 0)
                        {
                            allsub = allsub + pass;
                        }

                        if (onesub == 0)
                        {
                            onesub = j;
                        }
                        else if (onesub != 0)
                        {
                            onesub = onesub + j;
                        }

                        if (twosub == 0)
                        {
                            twosub = k;
                        }
                        else if (twosub != 0)
                        {
                            twosub = twosub + k;
                        }

                        if (abovethreesub == 0)
                        {
                            abovethreesub = m;
                        }
                        else if (abovethreesub != 0)
                        {
                            abovethreesub = abovethreesub + m;

                        }
                        int clear = allsub;
                        string allclear = allsub.ToString();
                        int arrear = onesub + twosub + abovethreesub;
                        total = (Convert.ToInt32(allclear) + (Convert.ToInt32(arrear)));
                        if (total != 0)
                        {
                            double passpercentage = (((Convert.ToDouble(allclear)) / (Convert.ToDouble(total)) * 100));
                            passper = Math.Round(passpercentage, 2);
                            finalpercent = Convert.ToString(passper);
                        }
                        else
                        {
                            finalpercent = "-";
                        }
                    }
                    gridviewreport.DataSource = dt;
                    gridviewreport.DataBind();
                    excelspread.Visible = false;
                    pdf.Visible = false;
                    lblError.Visible = false;
                    gridviewreport.Visible = true;
                    FpSpread1.Visible = false;
                    Excel.Visible = true;
                    print.Visible = true;

                    if (add1.Count > 0)
                    {
                        for (int a = 0; a < add1.Count; a++)
                        {
                            string countrow = Convert.ToString(add1[a]);
                            int rowspanning = 0;
                            if (countrow.Contains("-") == true)
                            {
                                string[] split = countrow.Split('-');
                                if (split.Length > 0)
                                {
                                    rowspanning = Convert.ToInt32(split[0]);
                                    rowspanning = rowspanning - 1;
                                    gridviewreport.Rows[rowspanning].Cells[0].ColumnSpan = 4;
                                    gridviewreport.Rows[rowspanning].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    gridviewreport.Rows[rowspanning].Cells[0].ForeColor = System.Drawing.Color.Navy;
                                    gridviewreport.Rows[rowspanning].Cells[1].Visible = false;
                                    gridviewreport.Rows[rowspanning].Cells[2].Visible = false;
                                    gridviewreport.Rows[rowspanning].Cells[3].Visible = false;
                                }

                                gridviewreport.Rows[rowspanning].Cells[4].ForeColor = System.Drawing.Color.Navy;
                                gridviewreport.Rows[rowspanning].Cells[5].ForeColor = System.Drawing.Color.Navy;
                                gridviewreport.Rows[rowspanning].Cells[6].ForeColor = System.Drawing.Color.Navy;
                                gridviewreport.Rows[rowspanning].Cells[7].ForeColor = System.Drawing.Color.Navy;
                                gridviewreport.Rows[rowspanning].Cells[8].ForeColor = System.Drawing.Color.Navy;
                            }
                        }
                    }
                }

            }
            else
            {
                lblError.Text = "No Records Found";
                lblError.Visible = true;
                gridviewreport.Visible = false;
                FpSpread1.Visible = false;
                print.Visible = false;
                Excel.Visible = false;
                FpSpread2.Visible = false;
            }
            if (mn == 0)
            {
                lblError.Text = "No Records Found";
                lblError.Visible = true;
                gridviewreport.Visible = false;
                FpSpread1.Visible = false;
                print.Visible = false;
                Excel.Visible = false;
                FpSpread2.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btn()
    {
        try
        {
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    bindbatch1 = Chklst_batch.Items[i].Value.ToString();
                    if (Chklstbatchvalue == "")
                    {
                        Chklstbatchvalue = bindbatch1;
                    }
                    else
                    {
                        Chklstbatchvalue = Chklstbatchvalue + "'" + "," + "'" + bindbatch1;

                    }
                }
            }

            if (Chklstbatchvalue != "")
            {
                sql = sql + "and r.Batch_Year in ('" + Chklstbatchvalue + "')";
            }
            else
            {
                sql = sql + "";
            }

            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    bindbranch1 = chklst_branch.Items[i].Value.ToString();
                    if (Chklstdegreevalue == "")
                    {
                        Chklstdegreevalue = bindbranch1;
                    }
                    else
                    {
                        Chklstdegreevalue = Chklstdegreevalue + "'" + "," + "'" + bindbranch1;
                    }
                }
            }

            if (Chklstdegreevalue != "")
            {
                sql = sql + " AND g.degree_code in ('" + Chklstdegreevalue + "')";
            }
            else
            {
                sql = sql + "";
            }

            if (dropsem.SelectedItem.Text != "")
            {
                sql = sql + " AND e.Current_Semester in ('" + dropsem.SelectedItem.Text + "')";
            }

            if (dropReport.SelectedItem.Text == "Lateral Entry")
            {
                sql = sql + "and r.mode=2 ";
            }

            else if (dropReport.SelectedItem.Text == "Hostel Students")
            {
                sql = sql + "and r.Stud_Type='Hostler' ";
            }

            if (sql != "")
            {
                sql = sql + "order by r.degree_code,r.batch_year" + "";
            }
        }

        catch (Exception ex)
        {

        }

    }

    protected void bindbound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gridviewreport.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gridviewreport.Rows[i];
                GridViewRow previousRow = gridviewreport.Rows[i - 1];

                for (int j = 0; j <= 2; j++)
                {
                    if (j == 0)
                    {
                        Label lnlname = (Label)row.FindControl("Iblserial");
                        Label lnlname1 = (Label)previousRow.FindControl("Iblserial");

                        if (lnlname.Text == lnlname1.Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }

                    }

                    if (j == 1)
                    {
                        Label lnlname = (Label)row.FindControl("lblbatch");
                        Label lnlname1 = (Label)previousRow.FindControl("lblbatch");

                        if (lnlname.Text == lnlname1.Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }

                    if (j == 2)
                    {
                        Label lnlname = (Label)row.FindControl("lblacronym");
                        Label lnlname1 = (Label)previousRow.FindControl("lblacronym");

                        if (lnlname.Text == lnlname1.Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        { }
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string sno = "";
            string batch_year = "";
            string branchname = "";
            string quotaname = "";
            string allclearvalue = "";
            string onearrearvalue = "";
            string twoarrearvalue = "";
            string threearrearvalue = "";
            string passvalue = "";
            string acdmic_date = "";

            DataSet dspdf = new DataSet();
            System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
            System.Drawing.Font Fontmedium = new System.Drawing.Font("Book Antiqua", 14, FontStyle.Regular);
            System.Drawing.Font fonttabhead = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
            System.Drawing.Font fbody = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            mypdfpage = mydoc.NewPage();

            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
            dspdf = da.select_method_wo_parameter(college, "text");
            string collname = dspdf.Tables[0].Rows[0]["collname"].ToString();
            string address = dspdf.Tables[0].Rows[0]["address1"].ToString();
            string address1 = dspdf.Tables[0].Rows[0]["address2"].ToString();
            string address2 = dspdf.Tables[0].Rows[0]["address3"].ToString();
            string pincode = dspdf.Tables[0].Rows[0]["pincode"].ToString();
            string logo = dspdf.Tables[0].Rows[0]["logo"].ToString();

            PdfTextArea pdf101 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 580, 550), System.Drawing.ContentAlignment.TopCenter, collname);
            mypdfpage.Add(pdf101);

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 20, 20, 500);
            }
            PdfTextArea pdf106 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 40, 580, 550), System.Drawing.ContentAlignment.TopCenter, address + "  " + address1 + "  " + address2);
            mypdfpage.Add(pdf106);
            PdfTextArea pdf109 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 60, 580, 550), System.Drawing.ContentAlignment.TopCenter, address2 + "  " + pincode + "  ");
            mypdfpage.Add(pdf109);
            PdfTextArea pdf107 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 20, 580, 550), System.Drawing.ContentAlignment.TopCenter, collname);
            mypdfpage.Add(pdf107);
            PdfTextArea pdf103 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 80, 580, 550), System.Drawing.ContentAlignment.TopCenter, "Departmentwise Arrear Statement");
            mypdfpage.Add(pdf103);

            string sqlschool = "select value from Master_Settings where settings='Academic year'";
            DataSet dschool = new DataSet();
            string fvalue = "";
            string lvalue = "";

            dschool = da.select_method_wo_parameter(sqlschool, "Text");
            string splitvalue = dschool.Tables[0].Rows[0]["value"].ToString();
            string[] dsplit = splitvalue.Split(',');

            fvalue = dsplit[0].ToString();
            lvalue = dsplit[1].ToString();
            acdmic_date = fvalue + "-" + lvalue;

            PdfTextArea pdf1078 = new PdfTextArea(fbody, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 100, 580, 550), System.Drawing.ContentAlignment.TopCenter, "ACADEMIC YEAR" + " " + acdmic_date);
            mypdfpage.Add(pdf1078);
            PdfTextArea pdf1073 = new PdfTextArea(fbody, System.Drawing.Color.Black, new PdfArea(mydoc, 0, 120, 580, 550), System.Drawing.ContentAlignment.TopCenter, dropReport.SelectedItem.Text + " " + "PERFORMANCE");
            mypdfpage.Add(pdf1073);
            PdfArea prheader = new PdfArea(mydoc, 14, 12, 564, 800);
            PdfRectangle prheadertop = new PdfRectangle(mydoc, prheader, Color.Black);
            mypdfpage.Add(prheadertop);
            Gios.Pdf.PdfTable tablepagelast;
            string val = gridviewreport.HeaderRow.Cells[0].Text;
            int count = 0;
            int addcount = 0;
            int spanvalue = 0;
            int j = 0;
            count = gridviewreport.Rows.Count;
            double pagecount = 0;
            if (count > 50)
            {
                tablepagelast = mydoc.NewTable(Fontmedium, 50 + 1, gridviewreport.Columns.Count, 1);
                tablepagelast.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                tablepagelast.SetColumnsWidth(new int[] { 5, 5, 5 });
                tablepagelast.VisibleHeaders = false;

                while (count > 50)
                {
                    if (pagecount != 0)
                    {
                        Gios.Pdf.PdfTablePage newtablepage5last = tablepagelast.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 140, 550, 800));
                        mypdfpage.Add(newtablepage5last);
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        tablepagelast = mydoc.NewTable(Fontmedium, 50 + 1, gridviewreport.Columns.Count, 1);
                        tablepagelast.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                        tablepagelast.SetColumnsWidth(new int[] { 5, 5, 5 });
                        tablepagelast.VisibleHeaders = false;
                    }
                    count = count - 50;

                    for (int mn = 0; mn < gridviewreport.Columns.Count; mn++)
                    {
                        string header = gridviewreport.HeaderRow.Cells[mn].Text;
                        string pdfvalue = gridviewreport.Rows[0].Cells[mn].Text.ToString();
                        tablepagelast.Cell(0, mn).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(0, mn).SetContent(header);
                        tablepagelast.Cell(0, mn).SetFont(fonttabhead);
                    }

                    for (j = addcount; j < 50; j++)
                    {

                        sno = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblserial") as Label).Text);
                        batch_year = Convert.ToString((gridviewreport.Rows[j].FindControl("lblbatch") as Label).Text);
                        branchname = Convert.ToString((gridviewreport.Rows[j].FindControl("lblacronym") as Label).Text);
                        quotaname = Convert.ToString((gridviewreport.Rows[j].FindControl("lbltextval") as Label).Text);
                        allclearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblallclear") as Label).Text);
                        onearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblonearrear") as Label).Text);
                        twoarrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Ibltwoarrear") as Label).Text);
                        threearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblthreearrear") as Label).Text);
                        passvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("percentage") as Label).Text);

                        spanvalue++;
                        tablepagelast.Cell(j + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 0).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 0).SetContent(sno.ToString());
                        tablepagelast.Cell(j + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 1).SetContent(batch_year.ToString());
                        tablepagelast.Cell(j + 1, 1).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 2).SetContent(branchname.ToString());
                        tablepagelast.Cell(j + 1, 2).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 3).SetContent(quotaname.ToString());
                        tablepagelast.Cell(j + 1, 3).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 4).SetContent(allclearvalue.ToString());
                        tablepagelast.Cell(j + 1, 4).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 5).SetContent(onearrearvalue.ToString());
                        tablepagelast.Cell(j + 1, 5).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 6).SetContent(twoarrearvalue.ToString());
                        tablepagelast.Cell(j + 1, 6).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 7).SetContent(threearrearvalue.ToString());
                        tablepagelast.Cell(j + 1, 7).SetFont(fbody);
                        tablepagelast.Cell(j + 1, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(j + 1, 8).SetContent(passvalue.ToString());
                        tablepagelast.Cell(j + 1, 8).SetFont(fbody);

                        if (sno == "Total")
                        {
                            foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 0, j - (spanvalue - 2), 0).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);
                            }

                            foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 1, j - (spanvalue - 2), 1).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);
                            }

                            foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 2, j - (spanvalue - 2), 2).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);

                            }

                            spanvalue = 0;
                            foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 1), 0, j - (spanvalue - 1), 0).Cells)
                            {
                                pr.ColSpan = 4;
                            }
                        }
                    }
                    addcount = j;
                    pagecount++;
                }
                Gios.Pdf.PdfTablePage newtablepage5last1 = tablepagelast.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 140, 550, 800));
                mypdfpage.Add(newtablepage5last1);
                mypdfpage.SaveToDocument();
                mypdfpage = mydoc.NewPage();

                if (count < 50)
                {
                    spanvalue = 0;
                    tablepagelast = mydoc.NewTable(Fontmedium, count + 1, gridviewreport.Columns.Count, 1);
                    tablepagelast.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                    tablepagelast.SetColumnsWidth(new int[] { 5, 5, 5 });
                    tablepagelast.VisibleHeaders = false;

                    int col = 0;

                    for (int mn = 0; mn < gridviewreport.Columns.Count; mn++)
                    {
                        string header = gridviewreport.HeaderRow.Cells[mn].Text;
                        string pdfvalue = gridviewreport.Rows[0].Cells[mn].Text.ToString();
                        tablepagelast.Cell(0, mn).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(0, mn).SetContent(header);
                        tablepagelast.Cell(0, mn).SetFont(fonttabhead);
                    }

                    for (j = 50; j < gridviewreport.Rows.Count - 1; j++)
                    {
                        col++;
                        sno = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblserial") as Label).Text);
                        batch_year = Convert.ToString((gridviewreport.Rows[j].FindControl("lblbatch") as Label).Text);
                        branchname = Convert.ToString((gridviewreport.Rows[j].FindControl("lblacronym") as Label).Text);
                        quotaname = Convert.ToString((gridviewreport.Rows[j].FindControl("lbltextval") as Label).Text);
                        allclearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblallclear") as Label).Text);
                        onearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblonearrear") as Label).Text);
                        twoarrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Ibltwoarrear") as Label).Text);
                        threearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblthreearrear") as Label).Text);
                        passvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("percentage") as Label).Text);
                        spanvalue++;
                        tablepagelast.Cell(col + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 0).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 0).SetContent(sno.ToString());
                        tablepagelast.Cell(col + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 1).SetContent(batch_year.ToString());
                        tablepagelast.Cell(col + 1, 1).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 2).SetContent(branchname.ToString());
                        tablepagelast.Cell(col + 1, 2).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 3).SetContent(quotaname.ToString());
                        tablepagelast.Cell(col + 1, 3).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 4).SetContent(allclearvalue.ToString());
                        tablepagelast.Cell(col + 1, 4).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 5).SetContent(onearrearvalue.ToString());
                        tablepagelast.Cell(col + 1, 5).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 6).SetContent(twoarrearvalue.ToString());
                        tablepagelast.Cell(col + 1, 6).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 7).SetContent(threearrearvalue.ToString());
                        tablepagelast.Cell(col + 1, 7).SetFont(fbody);
                        tablepagelast.Cell(col + 1, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tablepagelast.Cell(col + 1, 8).SetContent(passvalue.ToString());
                        tablepagelast.Cell(col + 1, 8).SetFont(fbody);

                        if (sno == "Total")
                        {
                            foreach (PdfCell pr in tablepagelast.CellRange(col - (spanvalue - 2), 0, col - (spanvalue - 2), 0).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);
                            }

                            foreach (PdfCell pr in tablepagelast.CellRange(col - (spanvalue - 2), 1, col - (spanvalue - 2), 1).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);
                            }

                            foreach (PdfCell pr in tablepagelast.CellRange(col - (spanvalue - 2), 2, col - (spanvalue - 2), 2).Cells)
                            {
                                pr.RowSpan = (spanvalue - 1);
                            }

                            spanvalue = 0;
                            foreach (PdfCell pr in tablepagelast.CellRange(col - (spanvalue - 1), 0, col - (spanvalue - 1), 0).Cells)
                            {
                                pr.ColSpan = 4;
                            }
                        }
                    }

                    Gios.Pdf.PdfTablePage newtablepage5last = tablepagelast.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 140, 550, 800));
                    mypdfpage.Add(newtablepage5last);
                    mypdfpage.SaveToDocument();
                }
            }
            else
            {
                tablepagelast = mydoc.NewTable(Fontmedium, count + 1, gridviewreport.Columns.Count, 1);
                tablepagelast.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                tablepagelast.SetColumnsWidth(new int[] { 5, 5, 5 });
                tablepagelast.VisibleHeaders = false;

                for (int mn = 0; mn < gridviewreport.Columns.Count; mn++)
                {
                    string header = gridviewreport.HeaderRow.Cells[mn].Text;
                    string pdfvalue = gridviewreport.Rows[0].Cells[mn].Text.ToString();
                    tablepagelast.Cell(0, mn).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(0, mn).SetContent(header);
                    tablepagelast.Cell(0, mn).SetFont(fonttabhead);
                }

                for (j = 0; j < gridviewreport.Rows.Count; j++)
                {

                    sno = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblserial") as Label).Text);
                    batch_year = Convert.ToString((gridviewreport.Rows[j].FindControl("lblbatch") as Label).Text);
                    branchname = Convert.ToString((gridviewreport.Rows[j].FindControl("lblacronym") as Label).Text);
                    quotaname = Convert.ToString((gridviewreport.Rows[j].FindControl("lbltextval") as Label).Text);
                    allclearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblallclear") as Label).Text);
                    onearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblonearrear") as Label).Text);
                    twoarrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Ibltwoarrear") as Label).Text);
                    threearrearvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("Iblthreearrear") as Label).Text);
                    passvalue = Convert.ToString((gridviewreport.Rows[j].FindControl("percentage") as Label).Text);

                    spanvalue++;

                    tablepagelast.Cell(j + 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 0).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 0).SetContent(sno.ToString());
                    tablepagelast.Cell(j + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 1).SetContent(batch_year.ToString());
                    tablepagelast.Cell(j + 1, 1).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 2).SetContent(branchname.ToString());
                    tablepagelast.Cell(j + 1, 2).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 3).SetContent(quotaname.ToString());
                    tablepagelast.Cell(j + 1, 3).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 4).SetContent(allclearvalue.ToString());
                    tablepagelast.Cell(j + 1, 4).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 5).SetContent(onearrearvalue.ToString());
                    tablepagelast.Cell(j + 1, 5).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 6).SetContent(twoarrearvalue.ToString());
                    tablepagelast.Cell(j + 1, 6).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 7).SetContent(threearrearvalue.ToString());
                    tablepagelast.Cell(j + 1, 7).SetFont(fbody);
                    tablepagelast.Cell(j + 1, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tablepagelast.Cell(j + 1, 8).SetContent(passvalue.ToString());
                    tablepagelast.Cell(j + 1, 8).SetFont(fbody);

                    if (sno == "Total")
                    {
                        foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 0, j - (spanvalue - 2), 0).Cells)
                        {
                            pr.RowSpan = (spanvalue - 1);
                        }

                        foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 1, j - (spanvalue - 2), 1).Cells)
                        {
                            pr.RowSpan = (spanvalue - 1);
                        }

                        foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 2), 2, j - (spanvalue - 2), 2).Cells)
                        {
                            pr.RowSpan = (spanvalue - 1);
                        }

                        spanvalue = 0;

                        foreach (PdfCell pr in tablepagelast.CellRange(j - (spanvalue - 1), 0, j - (spanvalue - 1), 0).Cells)
                        {
                            pr.ColSpan = 4;
                        }
                    }
                }
                PdfArea prheader1 = new PdfArea(mydoc, 14, 12, 564, 800);
                PdfRectangle prheadertop1 = new PdfRectangle(mydoc, prheader1, Color.Black);
                mypdfpage.Add(prheadertop1);
                Gios.Pdf.PdfTablePage newtablepage5last = tablepagelast.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, 140, 550, 800));
                mypdfpage.Add(newtablepage5last);
                mypdfpage.SaveToDocument();
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "DepartmentwiseArrearStatement" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
            else
            {
            }

        }
        catch (Exception ex)
        {
            lblError.Text = ex.ToString();
            lblError.Visible = true;
        }
    }
    protected void gridview_onselectedchanged(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Excel.Visible = true;
            print.Visible = true;

            DataView dvone = new DataView();
            string report = "";
            int row = Convert.ToInt32(e.CommandArgument);
            string buildvalue = "";
            string build = "";
            string branvalue = "";
            string bran = "";
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    build = Chklst_batch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + build;
                    }
                }
            }
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    bran = chklst_branch.Items[i].Value.ToString();
                    if (branvalue == "")
                    {
                        branvalue = bran;
                    }
                    else
                    {
                        branvalue = branvalue + "'" + "," + "'" + bran;
                    }
                }

            }
            if (e.CommandName == "All clear")
            {
                string degree_name = Convert.ToString((gridviewreport.Rows[row].FindControl("lblacronym") as Label).Text);
                string degree_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid") as Label).Text);
                string batch_year = Convert.ToString((gridviewreport.Rows[row].FindControl("lblbatch") as Label).Text);
                string text_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid12") as Label).Text);
                string rollno = Convert.ToString((gridviewreport.Rows[row].FindControl("rollpass") as Label).Text);
                if (dropsec.Enabled == true)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "All clear";
                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c where e.exam_code=m.exam_code and m.roll_no=r.Roll_No  and e.batch_year=r.Batch_Year and s.subject_no =c.subject_no and m.subject_no=s.subject_no  and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Roll_No in('" + rollno + "') and r.Batch_Year='" + batch_year + "' AND r.degree_code in ('" + degree_code + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result not in ('fail','AAA','WHD') and m.attempts=1   ";
                    }
                    else
                    {
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = batch_year + " -" + degree_name + "-" + "All clear";
                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c where e.exam_code=m.exam_code and m.roll_no=r.Roll_No  and e.batch_year=r.Batch_Year and s.subject_no =c.subject_no and m.subject_no=s.subject_no  and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Roll_No in('" + rollno + "') and r.Batch_Year in(" + buildvalue + ") AND r.degree_code in ('" + branvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result not in ('fail','AAA','WHD') and m.attempts=1   ";
                    }
                    if (dropsec.SelectedItem.Text != "All")
                    {
                        report = report + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                    }
                }
                else if (dropsec.Enabled == false)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "All clear";
                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c where e.exam_code=m.exam_code and m.roll_no=r.Roll_No  and e.batch_year=r.Batch_Year and s.subject_no =c.subject_no and m.subject_no=s.subject_no  and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Roll_No in('" + rollno + "') and r.Batch_Year='" + batch_year + "' AND r.degree_code in ('" + degree_code + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result not in ('fail','AAA','WHD') and m.attempts=1   ";
                    }
                    else
                    {
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = batch_year + " -" + "-" + degree_name + "-" + "All clear";

                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c where e.exam_code=m.exam_code and m.roll_no=r.Roll_No  and e.batch_year=r.Batch_Year and s.subject_no =c.subject_no and m.subject_no=s.subject_no  and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Roll_No in('" + rollno + "') and r.Batch_Year in(" + buildvalue + ") AND r.degree_code in ('" + branvalue + "') and r.college_code=" + dropcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result not in ('fail','AAA','WHD') and m.attempts=1   ";
                    }
                }

                ds = da.select_method_wo_parameter(report, "text");

            }
            if (e.CommandName == "One Arrear")
            {
                string degree_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid") as Label).Text);
                string batch_year = Convert.ToString((gridviewreport.Rows[row].FindControl("lblbatch") as Label).Text);
                string text_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid12") as Label).Text);
                string rollno1 = Convert.ToString((gridviewreport.Rows[row].FindControl("name") as Label).Text);
                string degree_name = Convert.ToString((gridviewreport.Rows[row].FindControl("lblacronym") as Label).Text);

                if (dropsec.Enabled == true)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "One Arrear";
                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno1 + "') ";
                    }
                    else
                    {
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = "Total Students" + "-" + degree_name + "-" + "One Arrear";

                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno1 + "') ";
                    }
                    if (dropsec.SelectedItem.Text != "All")
                    {
                        report = report + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                    }
                }
                if (dropsec.Enabled == false)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "One Arrear";
                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno1 + "') ";
                    }
                    else
                    {
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + "One Arrear";

                        report = " select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno1 + "') ";
                    }
                }

                ds = da.select_method_wo_parameter(report, "text");
            }
            if (e.CommandName == "Two Arrear")
            {
                string degree_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid") as Label).Text);
                string batch_year = Convert.ToString((gridviewreport.Rows[row].FindControl("lblbatch") as Label).Text);
                string text_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid12") as Label).Text);
                string rollno2 = Convert.ToString((gridviewreport.Rows[row].FindControl("name1") as Label).Text);
                string degree_name = Convert.ToString((gridviewreport.Rows[row].FindControl("lblacronym") as Label).Text);
                if (dropsec.Enabled == true)
                {
                    if (degree_code != "")
                    {
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "Two Arrears";
                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno2 + "') ";
                    }
                    else
                    {
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = "Total Students" + " -" + degree_name + "-" + "Two Arrears";

                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno2 + "') ";
                    }
                    if (dropsec.SelectedItem.Text != "All")
                    {
                        report = report + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                    }
                }
                else if (dropsec.Enabled == false)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "Two Arrears";
                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno2 + "') ";
                    }
                    else
                    {
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = "Total Students" + "-" + degree_name + "-" + "Two Arrears";

                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno2 + "') ";
                    }
                }

                ds = da.select_method_wo_parameter(report, "text");
            }
            else if (e.CommandName == "3 & above Arrear")
            {
                string degree_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid") as Label).Text);
                string batch_year = Convert.ToString((gridviewreport.Rows[row].FindControl("lblbatch") as Label).Text);
                string text_code = Convert.ToString((gridviewreport.Rows[row].FindControl("quotaid12") as Label).Text);
                string rollno3 = Convert.ToString((gridviewreport.Rows[row].FindControl("name2") as Label).Text);
                string degree_name = Convert.ToString((gridviewreport.Rows[row].FindControl("lblacronym") as Label).Text);
                if (dropsec.Enabled == true)
                {
                    if (degree_code != "")
                    {

                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "3 & above Arrears";
                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno3 + "') ";
                    }
                    else
                    {
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = "Total Students" + "-" + degree_name + "-" + "3 & above Arrears";

                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno3 + "') ";
                    }
                    if (dropsec.Enabled == true && dropsec.SelectedItem.Text != "All")
                    {
                        report = report + " and r.Sections='" + dropsec.SelectedItem.Text + "'";
                    }
                }
                if (dropsec.Enabled == false)
                {
                    if (degree_code != "")
                    {
                        Label2.Visible = false;
                        Label1.Visible = true;
                        Label1.Text = batch_year + "-" + degree_name + "-" + Convert.ToString((gridviewreport.Rows[row].FindControl("lbltextval") as Label).Text) + "-" + "3 & above Arrears";
                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in ('" + batch_year + "') and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + degree_code + "')  and r.Roll_No in('" + rollno3 + "') ";
                    }
                    else
                    {
                        batch_year = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblbatch") as Label).Text);
                        degree_code = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("quotaid") as Label).Text);
                        degree_name = Convert.ToString((gridviewreport.Rows[row - 1].FindControl("lblacronym") as Label).Text);
                        Label1.Visible = true;
                        Label1.Text = batch_year + " -" + "-" + degree_name + "-" + "3 & above Arrears";

                        report = "  select distinct r.Roll_No,r.Reg_No,r.stud_name,s.subject_name,e.Current_Semester from mark_entry m,Exam_Details e,Registration r ,subject s,subjectChooser c  where e.exam_code=m.exam_code and m.subject_no=s.subject_no  and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no =c.subject_no and c.roll_no =r.Roll_No  and  m.roll_no =c.roll_no and r.college_code=" + dropcollege.SelectedValue + " and r.Batch_Year in(" + buildvalue + ") and e.current_semester='" + dropsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA','WHD') and m.attempts=1 AND r.degree_code in ('" + branvalue + "')  and r.Roll_No in('" + rollno3 + "') ";
                    }
                }

                ds = da.select_method_wo_parameter(report, "text");
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
                Label1.Visible = false;
                Label2.Visible = false;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 5;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.White;
                style2.BackColor = Color.Teal;

                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].AllowTableCorner = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name ";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subjects";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].Width = 50;
                FpSpread1.Sheets[0].Columns[1].Width = 200;
                FpSpread1.Sheets[0].Columns[2].Width = 200;
                FpSpread1.Sheets[0].Columns[3].Width = 350;
                FpSpread1.Sheets[0].Columns[4].Width = 450;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FarPoint.Web.Spread.TextCellType intgrcell = new FarPoint.Web.Spread.TextCellType();
                int sno = 0;
                DataView dv = new DataView();
                Hashtable hn = new Hashtable();
                for (int temp = 0; temp < ds.Tables[0].Rows.Count; temp++)
                {
                    if (!hn.ContainsKey(ds.Tables[0].Rows[temp]["roll_no"].ToString()))
                    {
                        Label2.Visible = false;
                        FpSpread1.Visible = true;
                        btnexcel.Visible = true;
                        btnprint.Visible = true;
                        Label1.Visible = true;
                        sno++;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Azure;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.Color.Azure;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.Color.Azure;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.Color.Azure;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[temp]["roll_no"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = intgrcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[temp]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = intgrcell;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[temp]["stud_name"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        ds.Tables[0].DefaultView.RowFilter = "roll_no='" + ds.Tables[0].Rows[temp]["roll_no"].ToString() + "'";
                        dv = ds.Tables[0].DefaultView;
                        hn.Add(ds.Tables[0].Rows[temp]["roll_no"].ToString(), ds.Tables[0].Rows[temp]["roll_no"].ToString());
                        int rcnt = FpSpread1.Sheets[0].RowCount - 1;
                        for (int m = 0; m < dv.Count; m++)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Azure;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.Color.Azure;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.Color.Azure;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.Color.Azure;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[temp]["roll_no"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = intgrcell;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[temp]["Reg_No"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = intgrcell;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[temp]["stud_name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dv[m]["subject_name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            if (FpSpread1.Sheets[0].RowCount % 2 == 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.Color.PowderBlue;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.Color.Azure;
                            }
                            FpSpread1.Sheets[0].RowCount++;
                        }
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SpanModel.Add(rcnt, 0, dv.Count, 1);
                        FpSpread1.Sheets[0].SpanModel.Add(rcnt, 1, dv.Count, 1);
                        FpSpread1.Sheets[0].SpanModel.Add(rcnt, 2, dv.Count, 1);
                        FpSpread1.Sheets[0].SpanModel.Add(rcnt, 3, dv.Count, 1);
                        FpSpread1.Sheets[0].RowCount--;

                        btnexcel.Visible = true;
                        btnprint.Visible = true;
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    if (Session["Regflag"].ToString() == "0")
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                    }
                    if (Session["Rollflag"].ToString() == "0")
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                    }
                }
            }
            else
            {
                FpSpread1.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
                Label1.Visible = false;
                Label2.Visible = true;
                Label2.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        { }

    }
    protected void databoud(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].BackColor = System.Drawing.Color.Azure;
            e.Row.Cells[1].BackColor = System.Drawing.Color.Azure;
            e.Row.Cells[2].BackColor = System.Drawing.Color.Azure;
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridviewreport, "All clear$" + e.Row.RowIndex);
            e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridviewreport, "One Arrear$" + e.Row.RowIndex);
            e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridviewreport, "Two Arrear$" + e.Row.RowIndex);
            e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridviewreport, "3 & above Arrear$" + e.Row.RowIndex);
            e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gridviewreport, "Pass Percentage$" + e.Row.RowIndex);
        }

    }

    protected void dropcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch(bran);
        bindsemester();
        bindsec();
        gridviewreport.Visible = false;
        Excel.Visible = false;
        print.Visible = false;
        Label1.Visible = false;
        FpSpread1.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
    }

    protected void Buttonprint(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = true;
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy") + "@" + "Batch" + " : " + Label1.Text;
            string pagename = "Arrear_Report.aspx";
            string degreedetails = "DepartmentwiseArrearStatement" + date;
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        { }
    }

    protected void Buttonprint1(object sender, EventArgs e)
    {
        try
        {
            Label1.Visible = false;
            FpSpread2.Visible = true;
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy") + "@";
            string pagename = "Arrear_Report.aspx";
            string degreedetails = "StudentWiseArrearStatement" + date;
            Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        { }
    }

    protected void buttonexcel(object sender, EventArgs e)
    {
        try
        {
            {
                da.printexcelreport(FpSpread1, "DepartmentwiseArrearStatement");
            }
        }
        catch (Exception ex)
        { }
    }



    protected void spread_excel(object sender, EventArgs e)
    {
        try
        {
            {

                da.printexcelreport(FpSpread2, "DepartmentwiseArrearStatement");
            }
        }
        catch (Exception ex)
        { }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /*Verifies that the control is rendered */
    }
    protected void Logout_btn_Click(object sender, EventArgs e)
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
        { }
    }

    protected void Exportexcel_click(object sender, EventArgs e)
    {
        try
        {
            this.gridviewreport.AllowPaging = false;
            this.gridviewreport.AllowSorting = false;
            this.gridviewreport.EditIndex = -1;
            Response.Clear();
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition",
                    "attachment;filename=DepartmentwiseArrearStatement.xls");
            Response.Charset = "";
            StringWriter swriter = new StringWriter();
            HtmlTextWriter hwriter = new HtmlTextWriter(swriter);
            gridviewreport.RenderControl(hwriter);
            Response.Write(swriter.ToString());
            Response.End();
        }
        catch (Exception ex)
        { }
    }
}