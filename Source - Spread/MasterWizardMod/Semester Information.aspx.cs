using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
public partial class Semester_Information : System.Web.UI.Page
{
    static Boolean forschoolsetting = false;
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // Session["Semester"] =Convert.ToString(ddlSem.SelectedValue);
            if (!IsPostBack)
            {
                //'--------------------------------------
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                txt_startdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                txt_enddate.Text = DateTime.Now.ToString("dd/MM/yyy");
                Lblreport.Visible = false;
                txtexcl.Visible = false;
                btnprnt.Visible = false;
                btnxcl.Visible = false;
                lblexer.Visible = false;
                colg();
                bindbatch();
                binddegree();
                bindbranch();

                // bindsem();
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                // Added By Sridharan 12 Mar 2015
                //{
                DataSet schoolds = new DataSet();
                string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
                schoolds.Clear();
                schoolds.Dispose();
                schoolds = daccess.select_method_wo_parameter(sqlschool, "Text");
                if (schoolds.Tables[0].Rows.Count > 0)
                {
                    string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                    if (schoolvalue.Trim() == "0")
                    {
                        forschoolsetting = true;
                        Lblclg.Text = "School";
                        lblBatch.Text = "Year";
                        lblDegree.Text = "School Type";
                        lblBranch.Text = "Standard";
                        //lblsem.Text = "Term";
                        //lblDegree.Attributes.Add("style", " width: 95px;");
                        //lblBranch.Attributes.Add("style", " width: 67px;");
                        //ddlBranch.Attributes.Add("style", " width: 241px;");
                    }
                    else
                    {
                        forschoolsetting = false;
                    }
                }
                //} Sridharan
            }
            lblexer.Visible = false;
        }
        catch (Exception ex) { }
    }


    public void colg()
    {

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
        string clgname = "select college_code,collname from collinfo ";
        if (clgname != "")
        {
            ds_load = daccess.select_method(clgname, hat, "Text");
            ddlclg.DataSource = ds_load;
            ddlclg.DataTextField = "collname";
            ddlclg.DataValueField = "college_code";
            ddlclg.DataBind();
        }

    }
    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
        }
    }
    public void bindbranch()
    {
        cbl_branch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        string typ = string.Empty;
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbl_degree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "" + "," + "" + cbl_degree.Items[i].Value + "";
                    }
                }
            }
        }
        if (typ != "")
        {
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds_load = daccess.BindBranchMultiple(singleuser, group_user, typ, collegecode, usercode);
          //  ds_load = daccess.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                cbl_branch.DataSource = ds_load;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();

                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            bindsem();
        }
    }
    public void binddegree()
    {
        cbl_degree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlclg.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            cbl_degree.DataSource = ds_load;
            cbl_degree.DataTextField = "course_name";
            cbl_degree.DataValueField = "course_id";
            cbl_degree.DataBind();

            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                cbl_degree.Items[i].Selected = true;
            }
            txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        string duration = string.Empty;
        Boolean first_year = false;

        has.Clear();
        string typ = string.Empty;
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbl_degree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "'" + "," + "'" + cbl_degree.Items[i].Value + "";
                    }
                }

            }
        }
        string typ1 = string.Empty;
        if (cbl_branch.Items.Count > 0)
        {
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (typ1 == "")
                    {
                        typ1 = "" + cbl_branch.Items[i].Value + "";
                    }
                    else
                    {
                        typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                    }
                }

            }
        }

        if (typ != "" && typ1 != "")
        {
            collegecode = Convert.ToString(ddlclg.SelectedValue);
            has.Add("degree_code", typ1);
            has.Add("batch_year", typ);
            has.Add("college_code", collegecode);
            string sems = " select distinct ndurations,first_year_nonsemester from ndegree where degree_code in('" + typ1 + "')  and college_code=" + collegecode + " and batch_year='" + ddlBatch.SelectedValue + "' select distinct duration,first_year_nonsemester from degree where degree_code in('" + typ1 + "') and college_code=" + collegecode + "";
           // DataSet ds = daccess.select_method("bind_sem", has, "sp");
            DataSet ds = daccess.select_method_wo_parameter(sems, "text");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                txt_sem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        cbl_sem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        cbl_sem.Items.Add(loop_val.ToString());
                    }
                }
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    txt_sem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            cbl_sem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            cbl_sem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    txt_sem.Enabled = false;
                }
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
        }
        else
            txt_sem.Enabled = false;
    }


    public void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_degree.Text = "--Select--";
            cb_degree.Checked = false;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_degree.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
            }
            bindbranch();
            bindsem();
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
            bindsem();
        }
        catch (Exception ex)
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
            if (commcount > 0)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
            }
            bindsem();
        }
        catch (Exception ex)
        {
        }
    }

    public void ch_sem_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (ch_sem.Checked == true)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
                txt_sem.Text = "--Select--";
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
            int commcount = 0;
            txt_sem.Text = "--Select--";
            ch_sem.Checked = false;
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
                if (commcount == cbl_sem.Items.Count)
                {
                    ch_sem.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btnprnt_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = Session["usercode"].ToString();
            string degreedetails = "Semester Information";
            string pagename = "Semester Information.aspx";
            NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            NEWPrintMater1.Visible = true;
          
        }
        catch
        {
        }
    }
    public void btnxcl_click(object sender, EventArgs e)
    {
        try
        {
            string report = txtexcl.Text;

            if (report.ToString().Trim() != "")
            {

                daccess.printexcelreportgrid(gview, report);
                lblerr.Visible = false;
                
            }
            else
            {
                lblerr.Text = "Please Enter Your Report Name";
                lblerr.Visible = true;
            }
            btnxcl.Focus();

        }
        catch (Exception ex)
        {
            // lbl_norec.Text = ex.ToString();
        }
    }

    public void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string typ = string.Empty;
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbl_sem.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbl_sem.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ1 = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cbl_branch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cbl_branch.Items[i].Value + "";
                        }
                    }

                }
            }
            DataTable dtsem = new DataTable();
            DataRow dr;
            int days = 0;
            dtsem.Columns.Add("S.No");
            dtsem.Columns.Add("Batch Year");
            dtsem.Columns.Add("Department");
            dtsem.Columns.Add("Degreecode");
            dtsem.Columns.Add("Semester");
            dtsem.Columns.Add("Semester Start Date");
            dtsem.Columns.Add("Semester End Date");
            dtsem.Columns.Add("Remaining No of Working Days");
            dtsem.Columns.Add("Total No of Working Days");
            dtsem.Columns.Add("Schedule Order");
            dr = dtsem.NewRow();
            dr["S.No"] = "S.No";
            dr["Batch Year"] = "Batch Year";
            dr["Degreecode"] = "Degreecode";
            dr["Department"] = "Department";
            dr["Semester"] = "Semester";
            dr["Semester Start Date"] = "Semester Start Date";
            dr["Semester End Date"] = "Semester End Date";
            dr["Remaining No of Working Days"] = "Remaining No of Working Days";
            dr["Total No of Working Days"] = "Total No of Working Days";
            dr["Schedule Order"] = "Schedule Order";
            dtsem.Rows.Add(dr);
            string sql = "  select Dept_Name,batch_year,d.degree_code,CONVERT(varchar,start_date,103) as start_date,CONVERT(datetime,start_date,103) as st_date,CONVERT(datetime,end_date,103) as en_date,CONVERT(varchar,end_date,103) as end_date,s.semester,no_of_working_Days,no_of_working_hrs,schOrder  from Degree d,Department de,course c,seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code=de.college_code and s.degree_code in('" + typ1 + "')  and batch_year='" + Convert.ToString(ddlBatch.SelectedValue) + "'  and s.semester in('" + typ + "') order by c.college_code,c.Priority,d.Degree_Code";

            //  select * from seminfo as s , PeriodAttndSchedule as p where s.degree_code in('" + typ1 + "') and s.semester in('" + typ + "') and s.batch_year=" + Convert.ToString(ddlBatch.SelectedValue) + " And S.degree_code = p.degree_code And S.semester = p.semester";
            DataSet dsattabsent = daccess.select_method_wo_parameter(sql, "text");
            if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsattabsent.Tables[0].Rows.Count; i++)
                {
                    dr = dtsem.NewRow();
                    dr["S.No"] = i + 1;
                    dr["Batch Year"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["batch_year"]);
                    dr["Department"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["Dept_Name"]);
                    dr["Degreecode"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["degree_code"]);
                    dr["Semester"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["Semester"]);
                    dr["Semester Start Date"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["start_date"]);
                    dr["Semester End Date"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["end_date"]);
                    dr["Remaining No of Working Days"] = Convert.ToString(dsattabsent.Tables[0].Rows[i]["no_of_working_Days"]);

                    string sch = Convert.ToString(dsattabsent.Tables[0].Rows[i]["schOrder"]);
                    dt = Convert.ToDateTime(dsattabsent.Tables[0].Rows[i]["st_date"]);
                    dt1 = Convert.ToDateTime(dsattabsent.Tables[0].Rows[i]["en_date"]);
                    string nod = (dt1 - dt).TotalDays.ToString();
                    int.TryParse(nod, out days);
                    days += 1;
                    dr["Total No of Working Days"] = days;
                    if (sch == "0")
                        dr["Schedule Order"] = "Day Order";
                    else
                        dr["Schedule Order"] = "Week Days";
                    dtsem.Rows.Add(dr);

                }
            }
            if (dtsem.Rows.Count > 1)
            {

                gview.DataSource = dtsem;
                gview.DataBind();
                gview.Visible = true;
                RowHead(gview);

                Lblreport.Visible = true;
                txtexcl.Visible = true;
                btnprnt.Visible = true;
                btnxcl.Visible = true;
                lblexer.Visible = true;
                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    gview.Rows[i].Cells[3].Visible = false;
                    

                                  
                                    gview.Rows[i].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                    gview.Rows[i].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            else
            {
                gview.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record";
            }

        }
        catch
        {
        }
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    public void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            collg();
            bindbatchs();
            bindbranchs();
            binddegrees();
            bindsems();
            poperrjs.Visible = true;
            gdayset.Visible = true;
            btn_save.Visible = true;
            DataTable dt = new DataTable();
            DataView dv = new DataView();
            DataRow dr;

            dt.Columns.Add("Header");
            dt.Columns.Add("Total No.of Hours");
            dt.Columns.Add("Min Hours To be Present");
            dr = dt.NewRow();
            dr["Header"] = "Full Day";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Header"] = "I-st Half  Day";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Header"] = "II-nd Half  Day";
            dt.Rows.Add(dr);
            gdayset.DataSource = dt;
            gdayset.DataBind();
            gdayset.Visible = true;

            DataTable dts = new DataTable();
            DataRow drs;

            dts.Columns.Add("frange");
            dts.Columns.Add("trange");
            dts.Columns.Add("attnd_mark");
            drs = dts.NewRow();
            drs["frange"] = "";
            dts.Rows.Add(drs);
            gattn.DataSource = dts;
            gattn.DataBind();
            gattn.Visible = true;


            DataTable dtm = new DataTable();
            DataRow drm;





            dtm.Columns.Add("Header");
            dtm.Columns.Add("Min");
            drm = dtm.NewRow();
            drm["Header"] = "To Write Exam";
            dtm.Rows.Add(drm);
            drm = dtm.NewRow();
            drm["Header"] = "To Write Exam Next Year/Sem";
            dtm.Rows.Add(drm);
            drm = dt.NewRow();

            genligi.DataSource = dtm;
            genligi.DataBind();
            genligi.Visible = true;


            DataTable dtn = new DataTable();
            DataRow drn;

            dtn.Columns.Add("From");
            dtn.Columns.Add("To");
            dtn.Columns.Add("Mark");
            dtn.Columns.Add("Point");
            drn = dtn.NewRow();
            dtn.Rows.Add(drn);
            Gvgrade.DataSource = dtn;
            Gvgrade.DataBind();
            Gvgrade.Visible = true;
            btn_update.Visible = false;
        }
        catch
        {
        }
    }

    public void cbdegree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cbdegree.Checked == true)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = true;
                }
                txtdeg.Text = "Degree(" + (cbldegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = false;
                }
                txtdeg.Text = "--Select--";
            }
            bindbranchs();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtdeg.Text = "--Select--";
            cbdegree.Checked = false;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdeg.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbldegree.Items.Count)
                {
                    cbdegree.Checked = true;
                }
            }
            bindbranchs();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbbranch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cbbranch.Checked == true)
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cblbranch.Items[i].Selected = true;
                }
                txtbran.Text = "Branch(" + (cblbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cblbranch.Items[i].Selected = false;
                }
                txtbran.Text = "--Select--";
            }
            bindsems();
        }
        catch (Exception ex)
        {
        }
    }
    public void cblbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtbran.Text = "--Select--";
            cbbranch.Checked = false;
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbran.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cblbranch.Items.Count)
                {
                    cbbranch.Checked = true;
                }
            }
            bindsems();
        }
        catch (Exception ex)
        {
        }
    }

    //public void chsem_checkedchange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (chsem.Checked == true)
    //        {
    //            for (int i = 0; i < cblsem.Items.Count; i++)
    //            {
    //                cblsem.Items[i].Selected = true;
    //            }
    //            txtsem.Text = "Semester(" + (cblsem.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cblsem.Items.Count; i++)
    //            {
    //                cblsem.Items[i].Selected = false;
    //            }
    //            txtsem.Text = "--Select--";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void cblsem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int commcount = 0;
    //        txtsem.Text = "--Select--";
    //        chsem.Checked = false;
    //        for (int i = 0; i < cblsem.Items.Count; i++)
    //        {
    //            if (cblsem.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            txtsem.Text = "Semester(" + commcount.ToString() + ")";
    //            if (commcount == cblsem.Items.Count)
    //            {
    //                chsem.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;

    }

    public void cblholiday_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtholiday.Text = "--Select--";

            for (int i = 0; i < cblholiday.Items.Count; i++)
            {
                if (cblholiday.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtholiday.Text = "Semester(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btnaddrow_Click(object sender, EventArgs e)
    {
        DataTable dtCurrentTable = new DataTable();
        DataRow drCurrentRow;
        dtCurrentTable.Columns.Add("frange");
        dtCurrentTable.Columns.Add("trange");
        dtCurrentTable.Columns.Add("attnd_mark");
        for (int i = 0; i < gattn.Rows.Count; i++)
        {
            TextBox box1 = (TextBox)gattn.Rows[i].Cells[0].FindControl("txt_gviewfrom");

            TextBox box2 = (TextBox)gattn.Rows[i].Cells[1].FindControl("txt_gviewto");

            TextBox box3 = (TextBox)gattn.Rows[i].Cells[2].FindControl("txt_gviewmark");
            drCurrentRow = dtCurrentTable.NewRow();
            drCurrentRow["frange"] = box1.Text;
            

            drCurrentRow["trange"] = box2.Text;

            drCurrentRow["attnd_mark"] = box3.Text;
            dtCurrentTable.Rows.Add(drCurrentRow);
        }
        if (dtCurrentTable.Rows.Count > 0)
        {
            drCurrentRow = dtCurrentTable.NewRow();
            dtCurrentTable.Rows.Add(drCurrentRow);
            gattn.DataSource = dtCurrentTable;

            gattn.DataBind();
        }
        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
        {
            int m = dtCurrentTable.Rows.Count - 1;
            TextBox box1 = (TextBox)gattn.Rows[i].Cells[0].FindControl("txt_gviewfrom");

            TextBox box2 = (TextBox)gattn.Rows[i].Cells[1].FindControl("txt_gviewto");

            TextBox box3 = (TextBox)gattn.Rows[i].Cells[2].FindControl("txt_gviewmark");
            drCurrentRow = dtCurrentTable.NewRow();
            box1.Text = Convert.ToString(dtCurrentTable.Rows[i]["frange"]);

            box2.Text = Convert.ToString(dtCurrentTable.Rows[i]["trange"]);

            box3.Text = Convert.ToString(dtCurrentTable.Rows[i]["attnd_mark"]);
            if (i == m)
            {
                string sr = Convert.ToString(dtCurrentTable.Rows[i - 1]["trange"]);
                int.TryParse(Convert.ToString(dtCurrentTable.Rows[i - 1]["trange"]), out m);
                box1 = (TextBox)gattn.Rows[i].Cells[0].FindControl("txt_gviewfrom");
                box1.Text = Convert.ToString(m + 1);
            }
        }
    }

    public void btnrow_Click(object sender, EventArgs e)
    {
        DataTable dtCurrentTable = new DataTable();
        DataRow drCurrentRow;
        dtCurrentTable.Columns.Add("From");
        dtCurrentTable.Columns.Add("To");
        dtCurrentTable.Columns.Add("Mark");
        dtCurrentTable.Columns.Add("Point");
        for (int i = 0; i < Gvgrade.Rows.Count; i++)
        {
            TextBox box1 = (TextBox)Gvgrade.Rows[i].FindControl("txt_gviewfrom");

            TextBox box2 = (TextBox)Gvgrade.Rows[i].FindControl("txt_gviewto");

            TextBox box3 = (TextBox)Gvgrade.Rows[i].FindControl("txt_gviewmark");
            TextBox box4 = (TextBox)Gvgrade.Rows[i].FindControl("txt_gviewPoint");
            drCurrentRow = dtCurrentTable.NewRow();
            drCurrentRow["From"] = box1.Text;

            drCurrentRow["To"] = box2.Text;

            drCurrentRow["Mark"] = box3.Text;
            drCurrentRow["Point"] = box4.Text;
            dtCurrentTable.Rows.Add(drCurrentRow);
        }
        if (dtCurrentTable.Rows.Count > 0)
        {
            drCurrentRow = dtCurrentTable.NewRow();
            dtCurrentTable.Rows.Add(drCurrentRow);
            Gvgrade.DataSource = dtCurrentTable;

            Gvgrade.DataBind();
        }
        for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
        {
            int m = dtCurrentTable.Rows.Count - 1;
            TextBox box1 = (TextBox)Gvgrade.Rows[i].Cells[0].FindControl("txt_gviewfrom");

            TextBox box2 = (TextBox)Gvgrade.Rows[i].Cells[1].FindControl("txt_gviewto");

            TextBox box3 = (TextBox)Gvgrade.Rows[i].Cells[2].FindControl("txt_gviewmark");
            TextBox box4 = (TextBox)Gvgrade.Rows[i].FindControl("txt_gviewPoint");
            box1.Text = Convert.ToString(dtCurrentTable.Rows[i]["from"]);

            box2.Text = Convert.ToString(dtCurrentTable.Rows[i]["To"]);

            box3.Text = Convert.ToString(dtCurrentTable.Rows[i]["Mark"]);
            box4.Text = Convert.ToString(dtCurrentTable.Rows[i]["Point"]);
            if (i == m)
            {
                string sr = Convert.ToString(dtCurrentTable.Rows[i - 1]["To"]);
                int.TryParse(Convert.ToString(dtCurrentTable.Rows[i - 1]["To"]), out m);
                box1 = (TextBox)Gvgrade.Rows[i].Cells[0].FindControl("txt_gviewfrom");
                box1.Text = Convert.ToString(m + 1);
            }
        }
    }
    public void btn_save_Click(object sender, EventArgs e)
    {
        funsave();
        poperrjs.Visible = false;
        btnGo_Click(sender, e);
    }
    public void funsave()
    {
        try
        {

            int semin = 0;
            txt_hour.Text = hid.Value;
            txt_work.Text = hidwork.Value;
            string fromdate = txt_startdate.Text;
            string enddate = txt_enddate.Text;
            string[] spl = fromdate.Split('/');
            string frm_date = spl[1] + '/' + spl[0] + '/' + spl[2];
            string[] spl1 = enddate.Split('/');
            string end_date = spl1[1] + '/' + spl1[0] + '/' + spl1[2];
            TextBox perday = (TextBox)gdayset.Rows[0].FindControl("txt_gviewfrom");
            TextBox minday = (TextBox)gdayset.Rows[0].FindControl("txt_gviewto");
            TextBox Ihalf = (TextBox)gdayset.Rows[1].FindControl("txt_gviewfrom");
            TextBox minIhalf = (TextBox)gdayset.Rows[1].FindControl("txt_gviewto");
            TextBox IIhalf = (TextBox)gdayset.Rows[2].FindControl("txt_gviewfrom");
            TextBox minIIhalf = (TextBox)gdayset.Rows[2].FindControl("txt_gviewto");

            string noof_hour = perday.Text;
            string min_hour = minday.Text;
            string Ihalf_hour = Ihalf.Text;
            string min_Ihalf = minIhalf.Text;
            string noof_IIhalf = IIhalf.Text;
            string min_IIhalf = minIIhalf.Text;
            string noofday = txtdays.Text;
            int sch = 0;
            string start = string.Empty;
            if (ddlschedule.SelectedItem.Text == "Day Order")
            {
               
                sch = 0;
            }
            else
            {
                sch = 1;
            }

            if (ddlschedule.SelectedItem.Text == "Day Order")
            {
                string semester = ddlStartday.SelectedItem.Text.Remove(0, 9);
                //if (semester.Length > 1)
                //    start = semester[1];
                start = semester;
            
            }
            else
            {
                start = "0";
            }
            string typ = string.Empty;
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    if (cbldegree.Items[i].Selected == true)
                    {
                        if (typ == "")
                        {
                            typ = "" + cbldegree.Items[i].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + cbldegree.Items[i].Value + "";
                        }
                    }

                }
            }
            string typ1 = string.Empty;
            if (cblbranch.Items.Count > 0)
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cblbranch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cblbranch.Items[i].Value + "";
                        }
                    }

                }
            }
            TextBox subelg = (TextBox)genligi.Rows[1].FindControl("txt_gviewMin");
            TextBox percentelg = (TextBox)genligi.Rows[0].FindControl("txt_gviewMin");
            string eligiblesub = subelg.Text;
            string eligiblexam = percentelg.Text;

            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    string upperiod = " if exists(select * from PeriodAttndSchedule where degree_code='" + cblbranch.Items[i].Value + "' and semester='" + Convert.ToString(ddlsem.SelectedValue) + "')  Update PeriodAttndSchedule set No_of_hrs_per_day='" + noof_hour + "', min_hrs_per_day='" + min_hour + "',no_of_hrs_I_half_day='" + Ihalf_hour + "',min_pres_I_half_day='" + min_Ihalf + "',no_of_hrs_II_half_day='" + noof_IIhalf + "',min_pres_II_half_day='" + min_IIhalf + "',percent_eligible_for_Subject='" + eligiblesub + "',percent_eligible_for_exam='" + eligiblexam + "',nodays='" + txtdays.Text + "',schOrder='" + sch + "',atnd_mark_total='" + TextBox1.Text + "' where degree_code  in ('" + cblbranch.Items[i].Value + "') and semester = " + Convert.ToString(ddlsem.SelectedValue) + " else insert into PeriodAttndSchedule (degree_code,nodays,No_of_hrs_per_day,min_hrs_per_day,no_of_hrs_I_half_day,min_pres_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day,percent_eligible_for_Subject,percent_eligible_for_exam,schOrder,atnd_mark_total,semester) values ('" + cblbranch.Items[i].Value + "','" + txtdays.Text + "','" + noof_hour + "','" + min_hour + "','" + Ihalf_hour + "','" + min_Ihalf + "','" + noof_IIhalf + "','" + min_IIhalf + "','" + eligiblesub + "','" + eligiblexam + "','" + sch + "','" + TextBox1.Text + "'," + Convert.ToString(ddlsem.SelectedValue) + ")";

                    int up_ins = daccess.update_method_wo_parameter(upperiod, "text");
                }
            }

            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    string paracode = daccess.GetFunction("select para_code from PeriodAttndSchedule where degree_code  in ('" + cblbranch.Items[i].Value + "') and semester = " + Convert.ToString(ddlsem.SelectedValue) + "");
                
                    for (int j = 0; j < gattn.Rows.Count; j++)
                    {
                        TextBox frang = (TextBox)gattn.Rows[j].FindControl("txt_gviewfrom");
                        TextBox trang = (TextBox)gattn.Rows[j].FindControl("txt_gviewto");
                        TextBox attnd_mark = (TextBox)gattn.Rows[j].FindControl("txt_gviewmark");
                        string fran = frang.Text;
                        string tran = trang.Text;
                        string Attnmark = attnd_mark.Text;
                        if (!string.IsNullOrEmpty(fran) && !string.IsNullOrEmpty(tran) && !string.IsNullOrEmpty(Attnmark))
                        {
                            string upattn = " if exists( select * from attnd_para where para_code=" + paracode + " and frange='" + fran + "' and trange='" + tran + "') Update attnd_para set frange='" + fran + "', trange='" + tran + "',attnd_mark='" + Attnmark + "' where para_code = " + paracode + "  and frange='" + fran + "' and trange='" + tran + "' else insert into attnd_para (frange,trange,attnd_mark,para_code) values ('" + fran + "','" + tran + "','" + Attnmark + "'," + paracode + ")";
                            int upins = daccess.update_method_wo_parameter(upattn, "text");
                        }
                    }
                }
            }
            string seminfor = "select LinkValue from inssettings where linkname='Semester Duration Settings' and college_code='" + ddlcol.SelectedValue + "'";
            DataSet dsattabsent = daccess.select_method_wo_parameter(seminfor, "text");
            if (dsattabsent.Tables.Count > 0 && dsattabsent.Tables[0].Rows.Count > 0)
            {
                string linkval = Convert.ToString(dsattabsent.Tables[0].Rows[0]["LinkValue"]);
                if (linkval == "0")
                    FindHolidays();
                else if (txt_work.Text == "")
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter the Number of working days";
                }

            }
            for (int m = 0; m < cblbranch.Items.Count; m++)
            {
                if (cblbranch.Items[m].Selected == true)
                {
                    string seminfo = "if exists (select * from seminfo where degree_code ='" + cblbranch.Items[m].Value + "' and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + ")  update seminfo set start_date ='" + frm_date + "',end_date ='" + end_date + "', no_of_working_days='" + txt_work.Text + "', no_of_working_hrs='" + txt_hour.Text + "' ,starting_dayorder=" + start + " where degree_code  in ('" + cblbranch.Items[m].Value + "') and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + " else insert into seminfo (degree_code, start_date, end_date, no_of_working_Days, semester, batch_year,no_of_working_hrs,starting_dayorder) values (" + cblbranch.Items[m].Value + ",'" + frm_date + "','" + end_date + "','" + txt_work.Text + "'," + Convert.ToString(ddlsem.SelectedValue) + "," + Convert.ToString(ddlbatc.SelectedValue) + ",'" + txt_hour.Text + "'," + start + ")";
                    semin = daccess.update_method_wo_parameter(seminfo, "text");
                }
            }
            for (int m = 0; m < cblbranch.Items.Count; m++)
            {
                if (cblbranch.Items[m].Selected == true)
                {
                    for (int j = 0; j < Gvgrade.Rows.Count; j++)
                    {
                        TextBox frmmark = (TextBox)Gvgrade.Rows[j].FindControl("txt_gviewfrom");
                        TextBox tomark = (TextBox)Gvgrade.Rows[j].FindControl("txt_gviewto");
                        TextBox grade = (TextBox)Gvgrade.Rows[j].FindControl("txt_gviewmark");
                        TextBox point = (TextBox)Gvgrade.Rows[j].FindControl("txt_gviewPoint");

                        string frm_mark = frmmark.Text;
                        string tm_mark = tomark.Text;
                        string garde_mark = grade.Text;
                        string Points = point.Text;
                        if (!string.IsNullOrEmpty(frm_mark) && !string.IsNullOrEmpty(tm_mark) && !string.IsNullOrEmpty(garde_mark) && !string.IsNullOrEmpty(Points))
                        {
                            string gradeset = "if exists (select * from grade_master where degree_code = '" + cblbranch.Items[m].Value + "' and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + " and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and frange='" + frm_mark + "' and trange='" + tm_mark + "') update grade_master set frange='" + frm_mark + "',trange='" + tm_mark + "',mark_grade='" + garde_mark + "',credit_points='" + Points + "' where degree_code = '" + cblbranch.Items[m].Value + "' and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + " and semester=" + Convert.ToString(ddlsem.SelectedValue) + " else insert into grade_master (frange,trange,mark_grade,degree_code,college_code,credit_points,batch_year,semester) values('" + frm_mark + "','" + tm_mark + "','" + garde_mark + "','" + cblbranch.Items[m].Value + "'," + ddlcol.SelectedValue + ",'" + Points + "'," + Convert.ToString(ddlbatc.SelectedValue) + "," + Convert.ToString(ddlsem.SelectedValue) + ")";
                            int upgrade = daccess.update_method_wo_parameter(gradeset, "text");
                        }
                    }
                }
                
            }
            funholi();
            if (semin == 1)
            {
                poperrjs.Visible = false;
              
                duni.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Record Saved Sucessfully";
              
            }
            else
            {
                duni.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Not Saved";
                //poperrjs.Visible = false;
            }
            
         
        }
        catch
        {
        }
    }
    public void funholi()
    {
        try
        {
            txt_hour.Text = hid.Value;
            txt_work.Text = hidwork.Value;
            string fromdate = txt_startdate.Text;
            string enddate = txt_enddate.Text;
            string[] spl = fromdate.Split('/');
            string frm_date = spl[1] + '/' + spl[0] + '/' + spl[2];
            string[] spl1 = enddate.Split('/');
            string end_date = spl1[1] + '/' + spl1[0] + '/' + spl1[2];
            string day=string.Empty;
              DateTime dbconvto = Convert.ToDateTime(end_date);
             DateTime dbconvfrm = Convert.ToDateTime(frm_date);
             string typ1 = string.Empty;
             if (cblbranch.Items.Count > 0)
             {
                 for (int i = 0; i < cblbranch.Items.Count; i++)
                 {
                     if (cblbranch.Items[i].Selected == true)
                     {
                         if (typ1 == "")
                         {
                             typ1 = "" + cblbranch.Items[i].Value + "";
                         }
                         else
                         {
                             typ1 = typ1 + "'" + "," + "'" + cblbranch.Items[i].Value + "";
                         }
                     }

                 }
             }
             string holi = "delete from holidaystudents where degree_code in('" + typ1 + "')   and semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
        int upin = daccess.update_method_wo_parameter(holi, "text");
             while (dbconvfrm < dbconvto)
             {
                 day = dbconvfrm.ToString("dddd");
                 for (int i = 0; i < cblholiday.Items.Count; i++)
                 {
                     if (cblholiday.Items[i].Text.ToUpper().Trim() == day.ToUpper().Trim())
                     {
                         if (cblholiday.Items[i].Selected == true)
                         {
                             for (int m = 0; m < cblbranch.Items.Count; m++)
                             {

                                 if (cblbranch.Items[m].Selected == true)
                                 {
                                     string holiday = " insert into holidaystudents(degree_code,holiday_date,holiday_desc,semester,halforfull,morning,evening) values(" + cblbranch.Items[m].Value + ",'" + dbconvfrm + "','" + day + "'," + Convert.ToString(ddlsem.SelectedValue) + ",0,0,0)";
                                     int upholi = daccess.update_method_wo_parameter(holiday, "text");
                                 }
                             }
                         }
                     }
                 }

                 dbconvfrm = dbconvfrm.AddDays(1);

             } 
        }
        catch
        {
        }
    }
    public void btn_update_Click(object sender, EventArgs e)
    {
        funsave();
       
    
    }
    public void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            string typ1 = string.Empty;
            if (cblbranch.Items.Count > 0)
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cblbranch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cblbranch.Items[i].Value + "";
                        }
                    }

                }
            }
            string seminfo = "delete from seminfo where degree_code in('" + typ1 + "')  and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + "";
            int upins = daccess.update_method_wo_parameter(seminfo, "text");

            string holi = "delete from holidaystudents where degree_code in('" + typ1 + "')   and semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
            int upin = daccess.update_method_wo_parameter(holi, "text");
            string period = "update periodattndschedule set holiday='' where degree_code in('" + typ1 + "') and semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
            int perio = daccess.update_method_wo_parameter(period, "text");
            string detper = "delete from periodattndschedule where degree_code in('" + typ1 + "')  and semester=" + Convert.ToString(ddlsem.SelectedValue) + "";
            int delperio = daccess.update_method_wo_parameter(detper, "text");
            if (upins == 1)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Record Deleted Sucessfully";
            }
            poperrjs.Visible = false;
        }
        catch
        {
        }
    }
    public void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void collg()
    {

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
        string clgname = "select college_code,collname from collinfo ";
        if (clgname != "")
        {
            ds_load = daccess.select_method(clgname, hat, "Text");
            ddlcol.DataSource = ds_load;
            ddlcol.DataTextField = "collname";
            ddlcol.DataValueField = "college_code";
            ddlcol.DataBind();
        }

    }
    public void bindbatchs()
    {
        ddlbatc.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatc.DataSource = ds_load;
            ddlbatc.DataTextField = "batch_year";
            ddlbatc.DataValueField = "batch_year";
            ddlbatc.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlbatc.SelectedValue = max_bat.ToString();
        }
    }
    public void bindbranchs()
    {
        cblbranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlcol.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }

        string typ = string.Empty;
        if (cbldegree.Items.Count > 0)
        {
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "'" + cbldegree.Items[i].Value + "'";
                    }
                    else
                    {
                        typ = typ  + ",'" + cbldegree.Items[i].Value + "'";
                    }
                }

            }
        }
        if (typ != "")
        {
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", typ);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds_load = daccess.BindBranchMultiple(singleuser, group_user, typ, collegecode, usercode);
          //  ds_load = daccess.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                cblbranch.DataSource = ds_load;
                cblbranch.DataTextField = "dept_name";
                cblbranch.DataValueField = "degree_code";
                cblbranch.DataBind();
            }
            bindsems();
        }
    }
    public void binddegrees()
    {
        cbldegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlcol.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            cbldegree.DataSource = ds_load;
            cbldegree.DataTextField = "course_name";
            cbldegree.DataValueField = "course_id";
            cbldegree.DataBind();
        }
    }
    public void bindsems()
    {
        ddlsem.Items.Clear();
        string duration = string.Empty;
        Boolean first_year = false;

        has.Clear();
        string typ = string.Empty;
        if (cbl_degree.Items.Count > 0)
        {
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    if (typ == "")
                    {
                        typ = "" + cbldegree.Items[i].Value + "";
                    }
                    else
                    {
                        typ = typ + "'" + "," + "'" + cbldegree.Items[i].Value + "";
                    }
                }

            }
        }
        string typ1 = string.Empty;
        if (cblbranch.Items.Count > 0)
        {
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    if (typ1 == "")
                    {
                        typ1 = "" + cblbranch.Items[i].Value + "";
                    }
                    else
                    {
                        typ1 = typ1 + "'" + "," + "'" + cblbranch.Items[i].Value + "";
                    }
                }

            }
        }

        if (typ != "" && typ1 != "")
        {
            collegecode = Convert.ToString(ddlcol.SelectedValue);
            has.Add("degree_code", typ1);
            has.Add("batch_year", typ);
            has.Add("college_code", collegecode);

            string sems = " select distinct ndurations,first_year_nonsemester from ndegree where degree_code in('" + typ1 + "')  and college_code=" + collegecode + " and batch_year='" + ddlbatc.SelectedValue + "' select distinct duration,first_year_nonsemester from degree where degree_code in('" + typ1 + "') and college_code=" + collegecode + "";
            DataSet ds = daccess.select_method_wo_parameter(sems, "text");
         //   DataSet ds = daccess.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                txt_sem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    txt_sem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    txt_sem.Enabled = false;
                }
            }
        }
        else
            txt_sem.Enabled = false;
    }
    public void ddlcoll_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegrees();
    }
    public void btnexit_Click(object sender, EventArgs e)
    {
        duni.Visible = false;
    }
    public void btnsave_Click(object sender, EventArgs e)
    {
        int upins = 0;
        if (cblbranch.Items.Count > 0)
        {
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    string unihrs = "if exists(select * from univ_college_hrs where batch_year=" + ddlbatc.SelectedValue + " and semester=" + ddlsem.SelectedValue + " and degree_code=" + cblbranch.Items[i].Value + ") update univ_college_hrs set univ_hrs= " + txtunihrs.Text + ",college_hrs=" + txtclghrs.Text + " where batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + " and semester=" + ddlsem.SelectedValue + " and degree_code=" + cblbranch.Items[i].Value + " else insert into univ_college_hrs (univ_hrs,college_hrs,batch_year,degree_code,semester) values (" + txtunihrs.Text + "," + txtclghrs.Text + "," + Convert.ToString(ddlbatc.SelectedValue) + "," + cblbranch.Items[i].Value + "," + ddlsem.SelectedValue + ")";

                    upins = daccess.update_method_wo_parameter(unihrs, "text");
                }
            }
        }
        if (upins == 1)
        {
            duni.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Record Saved Sucessfully";
        }
    }

    public void FindHolidays()
    {
         string typ1 = string.Empty;
         int work = 0;
         int holiday = 0;
            if (cblbranch.Items.Count > 0)
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected == true)
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + cblbranch.Items[i].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + cblbranch.Items[i].Value + "";
                        }
                    }

                }
            }
           string fromdate = txt_startdate.Text;
            string enddate = txt_enddate.Text;
            string[] spl = fromdate.Split('/');
            string frm_date = spl[1] + '/' + spl[0] + '/' + spl[2];
            string[] spl1 = enddate.Split('/');
            string end_date = spl1[1] + '/' + spl1[0] + '/' + spl1[2];
            string holicun = daccess.GetFunction("select COUNT(holiday_date) holiday_date from holidayStudents where degree_code in('" + typ1 + "') and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and holiday_date between '" + frm_date + "' and '" + end_date + "'");
       
        int.TryParse(txt_work.Text,out work);
        int.TryParse(holicun, out holiday);
        txt_work.Text =Convert.ToString(work - holiday);
    }
    public void LinkButton_Click(object sender, EventArgs e)
    {
        duni.Visible = true;
    }
    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void gview_onselectedindexchanged(Object sender, EventArgs e)
    {
        try
        {
            collg();
            bindbatchs();

            DataTable dt = new DataTable();
            DataView dv = new DataView();
            DataRow dr;

            dt.Columns.Add("Header");
            dt.Columns.Add("Total No.of Hours");
            dt.Columns.Add("Min Hours To be Present");
            dr = dt.NewRow();
            dr["Header"] = "Full Day";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Header"] = "I-st Half  Day";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Header"] = "II-nd Half  Day";
            dt.Rows.Add(dr);
            gdayset.DataSource = dt;
            gdayset.DataBind();
            gdayset.Visible = true;

            //DataTable dts = new DataTable();
            //DataRow drs;

            //dts.Columns.Add("Header");
            //dts.Columns.Add("Total No.of Hours");
            //dts.Columns.Add("Min Hours To be Present");
            //drs = dts.NewRow();
            //drs["Header"] = "Full Day";
            //dts.Rows.Add(drs);
            //gattn.DataSource = dts;
            //gattn.DataBind();
            //gattn.Visible = true;


            DataTable dtm = new DataTable();
            DataRow drm;

            dtm.Columns.Add("Header");
            dtm.Columns.Add("Min");
            drm = dtm.NewRow();
            drm["Header"] = "To Write Exam";
            dtm.Rows.Add(drm);
            drm = dtm.NewRow();
            drm["Header"] = "To Write Exam Next Year/Sem";
            dtm.Rows.Add(drm);
            drm = dt.NewRow();

            genligi.DataSource = dtm;
            genligi.DataBind();
            genligi.Visible = true;

            poperrjs.Visible = true;
            btn_save.Visible = false;
            btn_update.Visible = true;
            btn_delete.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string activerow = Convert.ToString(rowIndex);
            string activecol = Convert.ToString(selectedCellIndex);
            if (activerow.Trim() != "")
            {
                int row = Convert.ToInt32(activerow);
                int col = Convert.ToInt32(activecol);
                string deg = Convert.ToString(gview.Rows[row].Cells[3].Text);
                string cour = daccess.GetFunction("select course_id from Degree where Degree_Code='" + deg + "'");
                binddegrees();
                int cun = 0;
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {

                    if (cour == cbldegree.Items[i].Value)
                    {
                        cun++;
                        cbldegree.Items[i].Enabled = true;
                        cbldegree.Items[i].Selected = true;
                    }
                    else
                    {
                        if (cbldegree.Items[i].Selected != true)
                            cbldegree.Items[i].Enabled = false;
                    }
                }
                txtdeg.Text = "Degree(" + cun + ")";
                bindbranchs();
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {

                    if (deg == cblbranch.Items[i].Value)
                    {
                        cun++;
                        cblbranch.Items[i].Enabled = true;
                        cblbranch.Items[i].Selected = true;
                    }
                    else
                    {
                        if (cblbranch.Items[i].Selected != true)
                            cblbranch.Items[i].Enabled = false;
                    }
                }
                bindsems();
                string sem = Convert.ToString(gview.Rows[row].Cells[4].Text);
                ddlsem.SelectedIndex = ddlsem.Items.IndexOf(ddlsem.Items.FindByText(sem));

                string upperiod = "select * from PeriodAttndSchedule where degree_code='" + deg + "' and semester='" + Convert.ToString(ddlsem.SelectedValue) + "'";
                DataSet up_ins = daccess.select_method_wo_parameter(upperiod, "text");


                string sch = Convert.ToString(up_ins.Tables[0].Rows[0]["schOrder"]);
                if (sch == "0")
                {
                    ddlStartday.Enabled = true;
                    ddlschedule.SelectedIndex = ddlschedule.Items.IndexOf(ddlschedule.Items.FindByText("Day Order"));
                }
                else
                {
                    ddlschedule.SelectedIndex = ddlschedule.Items.IndexOf(ddlschedule.Items.FindByText("Week Days"));
                    ddlStartday.Enabled = false;
                }

                TextBox perday = (TextBox)gdayset.Rows[0].FindControl("txt_gviewfrom");
                TextBox minday = (TextBox)gdayset.Rows[0].FindControl("txt_gviewto");
                TextBox Ihalf = (TextBox)gdayset.Rows[1].FindControl("txt_gviewfrom");
                TextBox minIhalf = (TextBox)gdayset.Rows[1].FindControl("txt_gviewto");
                TextBox IIhalf = (TextBox)gdayset.Rows[2].FindControl("txt_gviewfrom");
                TextBox minIIhalf = (TextBox)gdayset.Rows[2].FindControl("txt_gviewto");

                perday.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                minday.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["min_hrs_per_day"]);
                Ihalf.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
                minIhalf.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["min_pres_I_half_day"]);
                IIhalf.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);
                minIIhalf.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["min_pres_II_half_day"]);
                txtdays.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["nodays"]);
                TextBox subelg = (TextBox)genligi.Rows[1].FindControl("txt_gviewMin");
                TextBox percentelg = (TextBox)genligi.Rows[0].FindControl("txt_gviewMin");
                subelg.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["percent_eligible_for_Subject"]);
                percentelg.Text = Convert.ToString(up_ins.Tables[0].Rows[0]["percent_eligible_for_exam"]);
                 txtdays.Text=Convert.ToString(up_ins.Tables[0].Rows[0]["nodays"]); 
                string gradeset = "select * from grade_master where degree_code = '" + deg + "' and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + " and semester=" + Convert.ToString(ddlsem.SelectedValue) + " ";
                DataSet up_insgrade = daccess.select_method_wo_parameter(gradeset, "text");

                DataTable dtCurrentTable = new DataTable();
                DataRow drCurrentRow;
                dtCurrentTable.Columns.Add("From");
                dtCurrentTable.Columns.Add("To");
                dtCurrentTable.Columns.Add("Mark");
                dtCurrentTable.Columns.Add("Point");
                if (up_insgrade.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < up_insgrade.Tables[0].Rows.Count; i++)
                    {

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["From"] = Convert.ToString(up_insgrade.Tables[0].Rows[i]["frange"]);

                        drCurrentRow["To"] = Convert.ToString(up_insgrade.Tables[0].Rows[i]["trange"]);

                        drCurrentRow["Mark"] = Convert.ToString(up_insgrade.Tables[0].Rows[i]["mark_grade"]);
                        drCurrentRow["Point"] = Convert.ToString(up_insgrade.Tables[0].Rows[i]["credit_points"]);
                        dtCurrentTable.Rows.Add(drCurrentRow);
                    }
                }
                else
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);                  
                }
                if (dtCurrentTable.Rows.Count > 0)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    Gvgrade.DataSource = dtCurrentTable;

                    Gvgrade.DataBind();
                }
                string paracode = daccess.GetFunction("select para_code from PeriodAttndSchedule where degree_code  in ('" + deg + "') and semester = " + Convert.ToString(ddlsem.SelectedValue) + "");
                string upattn = " select * from attnd_para where para_code=" + paracode + "";
                DataSet up_insattn = daccess.select_method_wo_parameter(upattn, "text");
                DataTable dtCurrentTables = new DataTable();
                DataRow drCurrentRows;
                dtCurrentTables.Columns.Add("frange");
                dtCurrentTables.Columns.Add("trange");
                dtCurrentTables.Columns.Add("attnd_mark");
                if (up_insattn.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < up_insattn.Tables[0].Rows.Count; i++)
                    {

                        drCurrentRows = dtCurrentTables.NewRow();
                        drCurrentRows["frange"] = Convert.ToString(up_insattn.Tables[0].Rows[i]["frange"]);

                        drCurrentRows["trange"] = Convert.ToString(up_insattn.Tables[0].Rows[i]["trange"]);

                        drCurrentRows["attnd_mark"] = Convert.ToString(up_insattn.Tables[0].Rows[i]["attnd_mark"]);
                        dtCurrentTables.Rows.Add(drCurrentRows);
                    }
                }
                else
                {
                    drCurrentRows = dtCurrentTables.NewRow();
                    dtCurrentTables.Rows.Add(drCurrentRows);  
                }
                if (dtCurrentTables.Rows.Count > 0)
                {
                    gattn.DataSource = dtCurrentTables;
                    gattn.Visible = true;
                    gattn.DataBind();
                }
                string seminfo = "select CONVERT (varchar,start_date,103) as startdate,CONVERT (varchar,end_date,103)  as enddate,* from seminfo where degree_code ='" + deg + "' and semester=" + Convert.ToString(ddlsem.SelectedValue) + " and batch_year=" + Convert.ToString(ddlbatc.SelectedValue) + "";
                DataSet up_inssem = daccess.select_method_wo_parameter(seminfo, "text");
                //if (up_inssem.Tables[0].Rows.Count > 0)
                //{
                    for (int i = 0; i < up_inssem.Tables[0].Rows.Count; i++)
                    {
                        txt_work.Text = Convert.ToString(up_inssem.Tables[0].Rows[i]["no_of_working_days"]);
                        txt_hour.Text = Convert.ToString(up_inssem.Tables[0].Rows[i]["no_of_working_hrs"]);
                        txt_startdate.Text = Convert.ToString(up_inssem.Tables[0].Rows[i]["startdate"]);
                        txt_enddate.Text = Convert.ToString(up_inssem.Tables[0].Rows[i]["enddate"]);
                        string start = Convert.ToString(up_inssem.Tables[0].Rows[i]["starting_dayorder"]);
                        if (start == "0")
                        {
                            ddlschedule.SelectedIndex = ddlschedule.Items.IndexOf(ddlschedule.Items.FindByText("Week Days"));
                            ddlStartday.Enabled = false;

                        }
                        else
                        {
                            string dat = "Day Order" + start + "";
                            ddlStartday.Enabled = true;
                            ddlStartday.SelectedIndex = ddlStartday.Items.IndexOf(ddlStartday.Items.FindByText(dat));
                        }
                    }
               // }
                //else
                //{
                //    drCurrentRows = dtCurrentTables.NewRow();
                //    dtCurrentTables.Rows.Add(drCurrentRows);  
                //}
            }
          
            
        }
        catch
        {
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
}