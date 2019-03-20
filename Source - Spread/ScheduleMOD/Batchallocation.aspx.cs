using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using InsproDataAccess;
using System.Collections.Generic;
using System.Text;


public partial class Batchallocation : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dir = new InsproDirectAccess();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";

    enum UserAct { FView, Fsave, Fupdate, FDelete, FReport, FChange };

    enum InsModules { MStudent, MStaff, MOffice, MAcademic, MFinance, MLibrary, MHostel, MHr, MInventory, MWizard, MAdmin };

    DataSet dsoldbacth = new DataSet();
    DataTable dtCommon = new DataTable();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    string group_user1 = string.Empty;
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    DataTable dtable = new DataTable();
    DataRow dtrow;
    DataTable dtable1 = new DataTable();
    DataRow dtrow1;
    Hashtable subno1 = new Hashtable();
    DataTable subno = new DataTable();
    ArrayList addnewlis = new ArrayList();
    DataTable dtddl = new DataTable();
    DataRow drddl = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            //if (Session["collegecode"] == null) //Aruna For Back Button
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if (!Page.IsPostBack)
            {

                btnsave.Visible = false;
                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");
                selbtn.Visible = false;

                bcntlbl.Visible = false;
                btctxt.Visible = false;
                bcntddl.Visible = false;
                //bcntddllbl.Visible = false;
                btnsave.Enabled = false;
                Button1.Enabled = false;
                delbtn.Enabled = false;
                errlbl.Visible = false;

                batchpanel.Visible = false;
                deglbl.Visible = false;
                branlbl.Visible = false;
                seclbl.Visible = false;
                semlbl.Visible = false;
                fmlbl.Visible = false;

                sfrlbl.Enabled = false;
                sfmtxt.Enabled = false;
                stolbl.Enabled = false;
                stotxt.Enabled = false;
                Button1.Enabled = false;

                panel_sp1.Visible = false;
                Panel_sp2.Visible = false;


                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                Session["curr_year"] = dsplit[2].ToString();
                txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
                if (Session["batch"] == null && Session["Branch"] == null && Session["degcode"] == null && Session["sem"] == null && Session["fDate"] == null)
                {
                    Bindcollege();
                    bindbatch();
                    binddegree();
                    bindBranch();
                    bindsem();
                    bindsec();
                }

                //ddlduration.Items.Insert(0, new ListItem("--Select--", "-1"));
                //ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
                //ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));

                //Rajkumar
                if (Session["batch"] != null && Session["Branch"] != null && Session["degcode"] != null && Session["sem"] != null && Session["fDate"] != null)
                {
                    Bindcollege();
                    bindbatch();
                    binddegree();
                    ddlbatch.SelectedValue = Convert.ToString(Session["batch"]);
                    ddldegree.SelectedValue = Convert.ToString(Session["Branch"]);
                    bindBranch();
                    ddlbranch.SelectedValue = Convert.ToString(Session["degcode"]);
                    bindsem();
                    ddlduration.SelectedValue = Convert.ToString(Session["sem"]);
                    if (string.IsNullOrEmpty(Convert.ToString(Session["sec"])))
                        ddlsec.Enabled = false;
                    else
                    {
                        //ddlduration_SelectedIndexChanged(sender,e);
                        bindsec();
                        ddlsec.SelectedValue = Convert.ToString(Session["sec"]);
                        //ddlsec.SelectedItem.Text = Convert.ToString(Session["sec"]);
                        ddlsec.Enabled = true;
                    }
                    BatchAlloc();
                    string date = Session["fDate"].ToString();
                    string[] dsplit1 = date.Split(new Char[] { '/' });
                    txtFromDate.Text = dsplit1[0].ToString().PadLeft(2, '0') + "/" + dsplit1[1].ToString().PadLeft(2, '0') + "/" + dsplit1[2].ToString();

                }
                //
                string strdayflag = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dsmasetr = d2.select_method_wo_parameter(Master1, "Text");
                if (dsmasetr.Tables[0].Rows.Count > 0)
                {
                    for (int m = 0; m < dsmasetr.Tables[0].Rows.Count; m++)
                    {
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Roll No" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Register No" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Student_Type" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Days Scholor" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Hostel" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (Stud_Type='Hostler'";
                            }
                        }
                    }
                }
                if (strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;
            }
        }
        catch
        {
        }

    }

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            group_user1 = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user1.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbatch()
    {
        try
        {
            string sqlstr = " select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year";
            DataSet ds1 = d2.select_method_wo_parameter(sqlstr, "Text");
            ddlbatch.DataSource = ds1;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();

            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            int max_bat = Convert.ToInt32(d2.GetFunction(sqlstr));
            ddlbatch.SelectedValue = max_bat.ToString();
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    public void BatchAlloc()
    {
        try
        {
            dsoldbacth.Clear();
            string BatchYear = Convert.ToString(ddlbatch.Text);
            string degCode = Convert.ToString(ddlbranch.SelectedValue);
            string sem = Convert.ToString(ddlduration.SelectedValue);
            string strsec = "";
            if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + ddlsec.Text.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(BatchYear) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(sem))
            {
                string Alloc = "select distinct  sc.Batch from Registration r,subjectChooser sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and ss.lab=1  and  r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + "'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "'  " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>'' order by sc.Batch";
                dsoldbacth = d2.select_method_wo_parameter(Alloc, "text");
                if (dsoldbacth.Tables.Count > 0 && dsoldbacth.Tables[0].Rows.Count > 0)
                {
                    btctxt.Text = dsoldbacth.Tables[0].Rows.Count.ToString();
                    btctxt.Visible = true;
                    bcntlbl.Visible = true;
                    //bcntddllbl.Visible = true;
                    bcntddl.Visible = true;
                    bcntddl.Items.Clear();
                    string numbatch = "";
                    int b_val = 0;
                    numbatch = btctxt.Text.ToString();
                    if (numbatch != "" && numbatch != "0")
                    {
                        bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                        for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                        {
                            bcntddl.Items.Add("B" + b_val.ToString());

                        }
                        btnsave.Enabled = true;
                        Button1.Enabled = true;
                        delbtn.Enabled = true;
                        btn2sv.Enabled = true;
                    }
                    else
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Select Number of batch";
                        btnsave.Enabled = false;
                        Button1.Enabled = false;
                        delbtn.Enabled = false;
                        btn2sv.Enabled = false;
                    }
                }
            }
        }
        catch
        {

        }
    }
    public void binddegree()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
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
            DataSet ds = d2.select_method("bind_degree", hat, "sp");
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "course_id";
            ddldegree.DataTextField = "course_name";
            ddldegree.DataBind();

            //ddldegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    public void bindBranch()
    {
        try
        {
            string course_id = ddldegree.SelectedValue.ToString();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            DataSet ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
            //ddlbranch.SelectedItem.Value = Convert.ToString(Session["degcode"]);
        }
        catch
        {

        }
    }
    public void bindsem()
    {
        try
        {
            ddlduration.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            string strsemquery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + collegecode + "";
            DataSet dssem = d2.select_method_wo_parameter(strsemquery, "Text");
            if (dssem.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());

                string current_semqry = "select distinct Current_Semester from Registration where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and college_code='" + collegecode + "' and Batch_Year='" + ddlbatch.Text.ToString() + "'";   //modified by prabha on feb 28 2018
                int cur_sem = 0;
                Int32.TryParse(d2.GetFunction(current_semqry), out cur_sem);

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }

                    if (cur_sem == i)
                    {
                        ddlduration.SelectedIndex = i - 1;
                    }
                }
            }
            else
            {
                strsemquery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + collegecode + "";
                dssem.Dispose();
                dssem.Reset();
                dssem = d2.select_method_wo_parameter(strsemquery, "Text");
                if (dssem.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
                    string current_semqry = "select distinct Current_Semester from Registration where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and college_code='" + collegecode + "' and Batch_Year='" + ddlbatch.Text.ToString() + "'";       //modified by prabha on feb 28 2018
                    int cur_sem = 0;
                    Int32.TryParse(d2.GetFunction(current_semqry), out cur_sem);
                    for (i = 1; i <= duration; i++)
                    {

                        if (first_year == false)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        if (cur_sem == i)
                        {
                            ddlduration.SelectedIndex = i - 1;
                        }
                    }
                }
            }
            //ddlduration.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsec();
    }
    public void bindsec()
    {
        try
        {
            //ddlsec.Items.Clear();
            ddlsec.Enabled = false;
            string strsecquery = " select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and delflag=0 and exam_flag<>'Debar' and sections<>'-1' and isnull(sections,'')<>''";
            DataSet ds = d2.select_method_wo_parameter(strsecquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                ddlsec.Enabled = true;
            }
            //ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
            semlbl.Visible = false;
            seclbl.Visible = false;
            fmlbl.Visible = false;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbatch();
            binddegree();
            bindBranch();
            bindsem();
            bindsec();
        }
        catch
        {
        }
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //ddlbranch.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string course_id = ddldegree.SelectedValue.ToString();
            DataSet ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
            //ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
            deglbl.Visible = false;
            branlbl.Visible = false;
            semlbl.Visible = false;
            seclbl.Visible = false;
            fmlbl.Visible = false;
            BatchAlloc();

        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        branlbl.Visible = false;
        semlbl.Visible = false;
        seclbl.Visible = false;
        fmlbl.Visible = false;
        bindsem();
        bindsec();
        BatchAlloc();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindsem();
        //ddlsec.Items.Clear();
        //ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
        ddlsec.Enabled = true;
        BatchAlloc();
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        seclbl.Visible = false;
        BatchAlloc();
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btctxt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable gviewdt = (DataTable)Session["subno"];
            bcntddl.Items.Clear();
            string numbatch = "";
            int b_val = 0;
            numbatch = btctxt.Text.ToString();
            if (numbatch != "" && numbatch != "0")
            {
                string fmdate = "";
                DataTable dat = new DataTable(); dat.Columns.Add("batch");
                DataRow drow = null;
                bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                drow = dat.NewRow();
                drow["batch"] = string.Empty;
                dat.Rows.Add(drow);
                for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                {
                    bcntddl.Items.Add("B" + b_val.ToString());
                    drow = dat.NewRow();
                    drow["batch"] = "B" + b_val.ToString();
                    dat.Rows.Add(drow);
                }
                btnsave.Enabled = true;
                Button1.Enabled = true;
                delbtn.Enabled = true;
                btn2sv.Enabled = true;

                if (gview.Rows.Count > 0)
                {
                    DataSet dsstu = new DataSet();
                    DataSet dsstubatch = new DataSet();
                    DataSet dtoldBacth = new DataSet();
                    string strsec = "";
                    string date1 = "";
                    string dateval = "";
                    string strDay = "";
                    string strsql = "";
                    Boolean noflag = false;
                    Dictionary<int, string> chkrow = new Dictionary<int, string>();

                    collegecode = Convert.ToString(ddlCollege.SelectedValue);
                    if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
                    {
                        strsec = "";
                    }
                    else
                    {
                        strsec = " and sections='" + ddlsec.Text.ToString() + "'";
                    }
                    date1 = txtFromDate.Text.ToString();
                    string[] date_fm = date1.Split(new Char[] { '/' });
                    if (date_fm.GetUpperBound(0) == 2)
                    {
                        if (Convert.ToInt16(date_fm[0].ToString()) <= 31 && Convert.ToInt16(date_fm[1].ToString()) <= 12 && Convert.ToInt16(date_fm[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                        {
                            fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
                            dateval = date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString();
                            DateTime head_date = Convert.ToDateTime(dateval.ToString());
                            DateTime dt1 = Convert.ToDateTime(fmdate.ToString());

                            strDay = head_date.ToString("ddd");
                            if (strDay != "Sun")
                            {
                                string strorder = "ORDER BY Roll_No";
                                string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + collegecode + " and linkname='Student Attendance'");
                                if (serialno == "1")
                                {
                                    strorder = "order by serialno";
                                }
                                else
                                {
                                    string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
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
                                }
                                string oldbachAlloc = "select distinct  sc.roll_no,sc.semester,sc.Batch from Registration r,subjectChooser sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and (ss.lab=1 or ss.projThe=1)  and  r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + "'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "'  " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>'' order by sc.roll_no";
                                dtoldBacth = d2.select_method_wo_parameter(oldbachAlloc, "text");


                                string strstubatchquery = "select sc.roll_no,sc.semester,sc.Batch,sc.fromdate,sc.todate from Registration r,subjectChooser_New sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and (ss.lab=1 or ss.projThe=1) and sc.fromdate='" + fmdate.ToString() + "' and sc.todate='" + fmdate.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + " 'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>''";
                                dsstubatch = d2.select_method_wo_parameter(strstubatchquery, "Text");

                                strsql = "select distinct roll_no,reg_no,stud_name,stud_type,app_no,serialno from registration where cc=0 and delflag=0 and exam_flag<>'DEBAR' and batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and roll_no<>'' " + strorder + "";
                                dsstu = d2.select_method_wo_parameter(strsql, "Text");
                            }
                        }


                        for (int row = 0; row < gview.Rows.Count; row++)
                        {
                            DropDownList ddlst = (gview.Rows[row].FindControl("ddlbatch") as DropDownList);
                            ddlst.DataSource = dat;
                            ddlst.DataTextField = "batch";
                            ddlst.DataValueField = "batch";
                            ddlst.DataBind();
                            
                            bool isNew = false;
                            if (dsstu.Tables[0].Rows.Count > 0)
                            {
                                //for (int r = 0; r < dsstu.Tables[0].Rows.Count; r++)
                                //{
                                string bat = string.Empty;
                                noflag = true;
                                DropDownList ddl = (gview.Rows[row].FindControl("ddlbatch") as DropDownList);
                                string roll = (gview.Rows[row].FindControl("lblroll") as Label).Text;
                                dsstubatch.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "' and semester = " + ddlduration.SelectedValue.ToString() + "  and fromdate='" + fmdate.ToString() + "' and todate='" + fmdate.ToString() + "'";
                                DataView dvstubatch = dsstubatch.Tables[0].DefaultView;

                                DataView dvstuoldbatch = new DataView();
                                if (dtoldBacth.Tables.Count > 0 && dtoldBacth.Tables[0].Rows.Count > 0)
                                {
                                    dtoldBacth.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "'";
                                    dvstuoldbatch = dtoldBacth.Tables[0].DefaultView;
                                }

                                if (dvstubatch.Count > 0)
                                {
                                    bat = Convert.ToString(dvstubatch[0]["Batch"]);
                                    (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                                }
                                else if (dvstuoldbatch.Count > 0)
                                {
                                    isNew = true;
                                    bat = Convert.ToString(dvstuoldbatch[0]["Batch"]);
                                    (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                                }
                                else
                                {
                                    bat = string.Empty;
                                    (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                                }
                                if (ddl.SelectedItem != null)
                                {
                                    if (ddl.SelectedItem.Text != bat)
                                    {
                                        ddl.Items.Insert(ddl.Items.Count, bat);
                                        (gview.Rows[row].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[row].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                                    }
                                }
                                //if (isNew)
                                //    chkrow.Add(dtable.Rows.Count - 1, "Row");
                                //}
                            }
                        }
                        //}
                        //}
                    }
                }
                if (gview1.Rows.Count > 0)
                {
                    DataTable data = new DataTable(); data.Columns.Add("batch");
                    DataRow droww = null;
                    if (bcntddl.Items.Count > 1)
                    {
                        for (int i = 1; i < bcntddl.Items.Count; i++)
                        {
                            droww = data.NewRow();
                            droww["batch"] = bcntddl.Items[i].Text;
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

                        int IntRowCtr = 0;
                        string Day_Value = "";
                        string Hour_Value = "";
                        int intColCtr = 0;
                        string SnoStr = "";
                        string Setbatch1 = "";
                        string strsecvall = "";

                        if (ddlsec.SelectedValue.ToString() == "-1")
                        {
                            strsecvall = "";
                        }
                        else
                        {
                            strsecvall = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                        }

                        for (IntRowCtr = 0; IntRowCtr < gview1.Rows.Count; IntRowCtr++)
                        {
                            Day_Value = (gview1.Rows[IntRowCtr].FindControl("lbldate") as Label).Text;
                            Hour_Value = (gview1.Rows[IntRowCtr].FindControl("lblhour") as Label).Text;
                            if (Hour_Value != "" && Day_Value != "")
                            {
                                for (intColCtr = 2; intColCtr < gviewdt.Columns.Count; intColCtr++)
                                {
                                    Setbatch1 = "";
                                    //SnoStr = subno[intColCtr].ToString();
                                    SnoStr = Convert.ToString(gviewdt.Columns[intColCtr].ColumnName);
                                    if (SnoStr != "")
                                    {
                                        string stubatchquery = "Select distinct stu_batch from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and Day_Value ='" + Day_Value + "' and Hour_Value = " + Hour_Value + "" + strsecvall + "  and fdate='" + fmdate.ToString() + "' and tdate='" + fmdate.ToString() + "' and  Subject_No = " + SnoStr.ToString() + "";
                                        DataSet dsstubatch = d2.select_method_wo_parameter(stubatchquery, "Text");

                                        string batchYear = "select  distinct Stu_Batch from LabAlloc  where Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and Degree_Code=" + ddlbranch.SelectedValue.ToString() + " and Semester=" + ddlduration.SelectedValue.ToString() + "" + strsecvall + " and Day_Value='" + ddlDayOrder.Text + "' and Hour_Value=" + Hour_Value + "  and  Subject_No = " + SnoStr.ToString() + "";
                                        DataSet dtlab = d2.select_method_wo_parameter(batchYear, "Text");

                                        if (dsstubatch.Tables[0].Rows.Count > 0)
                                        {
                                            for (int s = 0; s < dsstubatch.Tables[0].Rows.Count; s++)
                                            {
                                                if (Setbatch1 == "")
                                                {
                                                    Setbatch1 = dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                                }
                                                else
                                                {
                                                    Setbatch1 = Setbatch1 + "," + dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                                }
                                                if (gview1.Rows[IntRowCtr].Cells[intColCtr + 1].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
                                                {
                                                    string[] spiltbatch = Setbatch1.Split(',');
                                                    if (spiltbatch.GetUpperBound(0) > 0)
                                                    {
                                                        int count = 0;
                                                        int r = intColCtr - 1;
                                                        string name = "";
                                                        TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                                        CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                                        for (int sel = 0; sel < chk.Items.Count; sel++)
                                                        {

                                                            name = Convert.ToString(chk.Items[sel].Text);
                                                            for (int y = 0; y < spiltbatch.Length; y++)
                                                            {
                                                                string name1 = spiltbatch[y].ToString();
                                                                if (name == name1)
                                                                {
                                                                    chk.Items[sel].Selected = true;
                                                                    count++;
                                                                }

                                                            }
                                                        }
                                                        txt.Text = Setbatch1;
                                                    }
                                                    else
                                                    {
                                                        int count = 0;
                                                        int r = intColCtr - 1;
                                                        string name = "";
                                                        string name1 = "";
                                                        TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                                        CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                                        for (int sel = 0; sel < chk.Items.Count; sel++)
                                                        {
                                                            name = Convert.ToString(chk.Items[sel].Text);
                                                            for (int y = 0; y < spiltbatch.Length; y++)
                                                            {
                                                                name1 = spiltbatch[y].ToString();
                                                                if (name == name1)
                                                                {
                                                                    chk.Items[sel].Selected = true;
                                                                    count++;
                                                                }
                                                            }
                                                        }
                                                        txt.Text = name1;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (dtlab.Tables.Count > 0 && dtlab.Tables[0].Rows.Count > 0)
                                            {
                                                for (int s = 0; s < dtlab.Tables[0].Rows.Count; s++)
                                                {
                                                    if (Setbatch1 == "")
                                                    {
                                                        Setbatch1 = dtlab.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                                    }
                                                    else
                                                    {
                                                        Setbatch1 = Setbatch1 + "," + dtlab.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                                    }
                                                    if (gview1.Rows[IntRowCtr].Cells[intColCtr + 1].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
                                                    {
                                                        string[] spiltbatch = Setbatch1.Split(',');
                                                        if (spiltbatch.GetUpperBound(0) > 0)
                                                        {
                                                            int count = 0;
                                                            int r = intColCtr - 1;
                                                            string name = "";
                                                            CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                                            TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                                            for (int sel = 0; sel < chk.Items.Count; sel++)
                                                            {

                                                                name = Convert.ToString(chk.Items[sel].Text);
                                                                for (int y = 0; y < spiltbatch.Length; y++)
                                                                {
                                                                    string name1 = spiltbatch[y].ToString();
                                                                    if (name == name1)
                                                                        chk.Items[sel].Selected = true;
                                                                }
                                                            }
                                                            if (count > 0)
                                                            {
                                                                if (count == 1)
                                                                {
                                                                    txt.Text = "" + name + "";
                                                                }
                                                                else
                                                                {
                                                                    txt.Text = "Batch" + "(" + count + ")";
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
                    }
                }
            }
            else
            {
                errlbl.Visible = true;
                errlbl.Text = "Select Number of batch";
                btnsave.Enabled = false;
                Button1.Enabled = false;
                delbtn.Enabled = false;
                btn2sv.Enabled = false;
            }
        }
        catch { }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //btctxt.Text = "";

        if (ddldegree.Text == "-1" || ddldegree.Text == "")
        {
            errlbl.Text = "Select Any degree";
            errlbl.Visible = true;
            return;
        }
        else if (ddlbranch.Text == "-1" || ddlbranch.Text == "")
        {
            errlbl.Text = "Select Any branch";
            errlbl.Visible = true;
            return;
        }
        else if (ddlduration.Text == "" || ddlduration.Text == "-1")
        {
            errlbl.Text = "Select Any  semester";
            errlbl.Visible = true;
            return;
        }
        else if (ddlsec.Enabled == true && ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text == "")
        {
            errlbl.Text = "Select Any section";
            errlbl.Visible = true;
        }
        else if (ddlsec.Enabled == true && ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text == "-1")//&& 
        {
            errlbl.Text = "Select Any section";
            errlbl.Visible = true;
        }
        else if (txtFromDate.Text == "")
        {
            errlbl.Text = "Select Any Date";
            errlbl.Visible = true;
        }

        else if (txtFromDate.Text != "" && ddldegree.SelectedValue != "-1" && ddlbranch.SelectedValue != "-1" && ddlduration.SelectedValue != "-1")//Modified by Manikandan from above Line on 20/08/2013
        {
            string strsec = string.Empty;
            string labSec = string.Empty;
            if (string.IsNullOrEmpty(Convert.ToString(ddlsec.SelectedValue)) && !Convert.ToString(ddlsec.SelectedValue).Contains("Select"))//ddlsec.SelectedValue.ToString() == "-1"//--Select--
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
                labSec = " and Sections='" + ddlsec.SelectedValue.ToString() + "'";
            }
            string stroldBatch = "select distinct batch from subjectchooser,registration,sub_sem,subject Where subjectchooser.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
            DataSet dsOldBatch = d2.select_method_wo_parameter(stroldBatch, "text");

            FarPoint.Web.Spread.ComboBoxCellType batch_no = new FarPoint.Web.Spread.ComboBoxCellType();
            ddlDayOrder.Visible = false;
            lblDayOrder.Visible = false;

            if (dsOldBatch.Tables.Count > 0 && dsOldBatch.Tables[0].Rows.Count > 0)
            {
                batch_no.DataSource = dsOldBatch;
                batch_no.DataTextField = "batch";
                batch_no.DataValueField = "batch";
                btn2sv.Enabled = true;

                string LabdayOrder = "select distinct Day_Value,Hour_Value,Stu_Batch from LabAlloc  where Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and Degree_Code=" + ddlbranch.SelectedValue.ToString() + " and Semester=" + ddlduration.SelectedValue.ToString() + "" + labSec;
                DataTable dtdayOrder = dir.selectDataTable(LabdayOrder);
                if (dtdayOrder.Rows.Count > 0)
                {
                    DataTable dtDay = dtdayOrder.DefaultView.ToTable(true, "Day_Value");
                    DataTable dtdicbatch = new DataTable();

                    if (dtDay.Rows.Count > 0)
                    {
                        ddlDayOrder.DataSource = dtDay;
                        ddlDayOrder.DataTextField = "Day_Value";
                        ddlDayOrder.DataValueField = "Day_Value";
                        ddlDayOrder.DataBind();
                        ddlDayOrder.Visible = true;
                        lblDayOrder.Visible = true;
                    }
                }
            }
            loadbatch();

            DataTable dtt = new DataTable(); dtt.Columns.Add("batch");
            DataRow drr = null;
            if (bcntddl.Items.Count > 1)
            {
                for (int dd = 1; dd < bcntddl.Items.Count; dd++)
                {
                    drr = dtt.NewRow();
                    drr["batch"] = bcntddl.Items[dd].Text;
                    dtt.Rows.Add(drr);
                }
            }
            if (bcntddl.Items.Count > 1)
            {
                for (int bat = 0; bat < gview.Rows.Count; bat++)
                {
                    DropDownList ddl = (gview.Rows[bat].FindControl("ddlbatch") as DropDownList);
                    ddl.DataSource = dtt;
                    ddl.DataTextField = "batch";
                    ddl.DataValueField = "batch";
                    ddl.DataBind();
                    ddl.Items.Insert(0, "");
                }
            }
            else
            {
                for (int bat = 0; bat < gview.Rows.Count; bat++)
                {
                    DropDownList ddl = (gview.Rows[bat].FindControl("ddlbatch") as DropDownList);
                    ddl.DataSource = dsOldBatch;
                    ddl.DataTextField = "batch";
                    ddl.DataValueField = "batch";
                    ddl.DataBind();
                    ddl.Items.Insert(0, "");
                }
            }

            for (int bach = 0; bach < dtable.Rows.Count; bach++)
            {
                DropDownList dd = (gview.Rows[bach].FindControl("ddlbatch") as DropDownList);
                string bat = Convert.ToString(dtable.Rows[bach]["Batch"]);
                (gview.Rows[bach].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[bach].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[bach].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bat));
                if (dd.SelectedItem != null)
                {
                    if (dd.SelectedItem.Text != bat)
                    {
                        (gview.Rows[bach].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[bach].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[bach].FindControl("ddlbatch") as DropDownList).Items.FindByValue(string.Empty));
                    }
                }
            }
        }
    }

    public void loadbatch()
    {
        try
        {
            bcntlbl.Visible = true;
            btctxt.Visible = true;
            bcntddl.Visible = true;
            //bcntddllbl.Visible = true;

            dtable.Columns.Add("Roll No");
            dtable.Columns.Add("Reg No");
            dtable.Columns.Add("Student Name");
            dtable.Columns.Add("Batch");

            //bcntddl.Items.Clear();

            Fieldset5.Visible = false;


            int col_val = 0;
            int row_val = 0;
            int maxrow = 0;
            string strsql = "";
            string date1 = "";
            string fmdate = "";
            Boolean noflag = false;
            string dateval = "";
            string strDay = "";
            string strsec = "";
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + ddlsec.Text.ToString() + "'";
            }
            Dictionary<int, string> chkrow = new Dictionary<int, string>();
            date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            if (date_fm.GetUpperBound(0) == 2)
            {
                if (Convert.ToInt16(date_fm[0].ToString()) <= 31 && Convert.ToInt16(date_fm[1].ToString()) <= 12 && Convert.ToInt16(date_fm[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {

                    fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
                    dateval = date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());
                    DateTime dt1 = Convert.ToDateTime(fmdate.ToString());

                    strDay = head_date.ToString("ddd");
                    if (strDay != "Sun")
                    {
                        string strorder = "ORDER BY Roll_No";
                        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + collegecode + " and linkname='Student Attendance'");
                        if (serialno == "1")
                        {
                            strorder = "order by serialno";
                        }
                        else
                        {
                            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
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
                        }

                        string oldbachAlloc = "select distinct  sc.roll_no,sc.semester,sc.Batch from Registration r,subjectChooser sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and ss.lab=1  and  r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + "'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "'  " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>'' order by sc.roll_no";
                        DataSet dtoldBacth = d2.select_method_wo_parameter(oldbachAlloc, "text");


                        string strstubatchquery = "select sc.roll_no,sc.semester,sc.Batch,sc.fromdate,sc.todate from Registration r,subjectChooser_New sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and ss.lab=1 and sc.fromdate='" + fmdate.ToString() + "' and sc.todate='" + fmdate.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + " 'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>''";
                        DataSet dsstubatch = d2.select_method_wo_parameter(strstubatchquery, "Text");

                        strsql = "select distinct roll_no,reg_no,stud_name,stud_type,app_no,serialno from registration where cc=0 and delflag=0 and exam_flag<>'DEBAR' and batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and roll_no<>'' " + strorder + "";
                        DataSet dsstu = d2.select_method_wo_parameter(strsql, "Text");
                        bool isNew = false;
                        if (dsstu.Tables[0].Rows.Count > 0)
                        {
                            for (int r = 0; r < dsstu.Tables[0].Rows.Count; r++)
                            {
                                noflag = true;
                                dtrow = dtable.NewRow();

                                dtrow["Roll No"] = Convert.ToString(dsstu.Tables[0].Rows[r]["roll_no"]);
                                dtrow["Reg No"] = Convert.ToString(dsstu.Tables[0].Rows[r]["reg_no"]);
                                dtrow["Student Name"] = Convert.ToString(dsstu.Tables[0].Rows[r]["stud_name"]);


                                dsstubatch.Tables[0].DefaultView.RowFilter = "roll_no='" + dsstu.Tables[0].Rows[r]["roll_no"].ToString() + "' and semester = " + ddlduration.SelectedValue.ToString() + "  and fromdate='" + fmdate.ToString() + "' and todate='" + fmdate.ToString() + "'";
                                DataView dvstubatch = dsstubatch.Tables[0].DefaultView;

                                DataView dvstuoldbatch = new DataView();
                                if (dtoldBacth.Tables.Count > 0 && dtoldBacth.Tables[0].Rows.Count > 0)
                                {
                                    dtoldBacth.Tables[0].DefaultView.RowFilter = "roll_no='" + dsstu.Tables[0].Rows[r]["roll_no"].ToString() + "'";
                                    dvstuoldbatch = dtoldBacth.Tables[0].DefaultView;
                                }

                                if (dvstubatch.Count > 0)
                                {
                                    dtrow["Batch"] = dvstubatch[0]["Batch"].ToString();

                                }
                                else if (dvstuoldbatch.Count > 0)
                                {
                                    isNew = true;
                                    dtrow["Batch"] = dvstuoldbatch[0]["Batch"].ToString();
                                }
                                else
                                {
                                    dtrow["Batch"] = "";
                                }
                                dtable.Rows.Add(dtrow);
                                if (isNew)
                                    chkrow.Add(dtable.Rows.Count - 1, "Row");
                            }
                        }

                        strsql = "select distinct s.Batch from subjectChooser_New s,Registration r where  r.Roll_No=s.roll_no and batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + "" + strsec.ToString() + " and s.Batch<>'';";
                        string strsqlold = "select distinct s.Batch from subjectChooser s,Registration r where  r.Roll_No=s.roll_no and batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + "" + strsec.ToString() + " and s.Batch<>''";

                        DataSet ds = d2.select_method(strsql, hat, "Text");
                        DataSet ds1 = d2.select_method_wo_parameter(strsqlold, "text");
                        Checkboxlistbatch.Items.Clear();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Checkboxlistbatch.DataSource = ds;
                            Checkboxlistbatch.DataTextField = "Batch";
                            Checkboxlistbatch.DataValueField = "Batch";
                            Checkboxlistbatch.DataBind();
                        }
                        else if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            Checkboxlistbatch.DataSource = ds1;
                            Checkboxlistbatch.DataTextField = "Batch";
                            Checkboxlistbatch.DataValueField = "Batch";
                            Checkboxlistbatch.DataBind();
                        }

                        if (noflag == false)
                        {
                            Panel_sp2.Visible = false;
                            errlbl.Visible = true;
                            errlbl.Text = "No data found on that day";
                            bcntlbl.Visible = false;
                            btctxt.Visible = false;
                            bcntddl.Visible = false;
                            //bcntddllbl.Visible = false;
                        }
                        else
                        {
                            batchpanel.Visible = true;
                            bcntlbl.Visible = true;
                            btctxt.Visible = true;
                            bcntddl.Visible = true;
                            //bcntddllbl.Visible = true;

                            Panel_sp2.Visible = true;
                            errlbl.Visible = false;
                            if (dtable.Columns.Count > 0 && dtable.Rows.Count > 0)
                            {
                                gview.DataSource = dtable;
                                gview.DataBind();
                                gview.Visible = true;

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



                                foreach (KeyValuePair<int, string> dr in chkrow)
                                {
                                    int r = dr.Key;
                                    System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[r].FindControl("chck");
                                    val.Checked = true;
                                }
                            }
                            enablesave();

                            loaddays();
                        }
                        if (isNew == true)
                        {
                            CheckBox1.Checked = true;
                            sfmtxt.Enabled = true;
                            stotxt.Enabled = true;
                            btnsave.Enabled = true;
                            Button1.Enabled = true;
                            delbtn.Enabled = true;
                        }

                    }
                    else
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Sunday can not be accepted";
                        bcntlbl.Visible = false;
                        btctxt.Visible = false;
                        bcntddl.Visible = false;
                        //bcntddllbl.Visible = false;
                        return;
                    }
                    if (Convert.ToInt32(gview.Rows.Count) > 0)
                    {
                        panel_sp1.Visible = true;
                        Double totalRows = 0;
                        totalRows = Convert.ToInt32(gview.Rows.Count);
                        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                    }
                }
                else
                {
                    fmlbl.Visible = true;
                    fmlbl.Text = "Enter Valid date";
                }
            }
            else
            {
                fmlbl.Visible = true;
                fmlbl.Text = "Enter Valid date";
            }

        }
        catch (Exception ex)
        {
            errlbl.Text = ex.ToString();
            errlbl.Visible = true;
        }
    }
    public void enablesave()
    {
        if (CheckBox1.Checked == false)
        {
            btnsave.Enabled = false;
            Button1.Enabled = false;
            btn2sv.Enabled = false;
            delbtn.Enabled = false;
        }
        else
        {
            btnsave.Enabled = true;
            Button1.Enabled = true;
            btn2sv.Enabled = true;
            delbtn.Enabled = true;
        }
    }
    public void loaddays()
    {

        try
        {
            subno.Clear();
            string scode = "";
            int l = 0;
            int intNHrs = 0;
            string[] WkArr = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            string strsec = "";
            string strsecval = "";
            int SchOrder = 0;
            int nodays = 0;
            string ini_day = "";
            string stt = "";
            string date1 = "";
            string fmdate = "";
            string sql = "";
            int IntRCtr = 0;
            int row = 0;
            string tagsub = "";
            Boolean flag = false;
            int col = 1;
            string todate = string.Empty;
            string startdate = string.Empty;
            string start_dayorder = string.Empty;
            string labSec = string.Empty;
            if (string.IsNullOrEmpty(ddlsec.SelectedValue.ToString()) || ddlsec.SelectedValue.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
                labSec = " and Sections='" + ddlsec.SelectedValue.ToString() + "'";
            }


            string strsyllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and Batch_Year = " + ddlbatch.SelectedValue.ToString() + "");
            if (strsyllcode.Trim() != "" && strsyllcode.Trim() != "0")
            {
                scode = strsyllcode;
                gview1.Visible = true;

                dtable1.Columns.Add("Day");
                dtable1.Columns.Add("Hour");

                subno.Columns.Add("Day");
                subno.Columns.Add("Hour");


                if (ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsecval = "";
                }
                else
                {
                    strsecval = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                }


                string strseminf = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "";
                DataSet dsseminf = d2.select_method_wo_parameter(strseminf, "text");
                if (dsseminf.Tables[0].Rows.Count > 0)
                {
                    if ((dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString()) != "")
                    {
                        intNHrs = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                        SchOrder = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["schorder"]);
                        nodays = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["nodays"]);
                    }
                }

                date1 = txtFromDate.Text.ToString();
                string[] date_fm = date1.Split(new Char[] { '/' });
                fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
                Session["todate"] = fmdate.ToString();
                DateTime dt1 = Convert.ToDateTime(fmdate.ToString());

                string strstadquery = "select start_date,end_date,starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
                DataSet dsstar = d2.select_method_wo_parameter(strstadquery, "Text");

                if (dsstar.Tables[0].Rows.Count > 0)
                {
                    if ((dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "" && (dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
                    {
                        start_dayorder = dsstar.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        string[] tmpdate = dsstar.Tables[0].Rows[0]["start_date"].ToString().Split(new char[] { ' ' });
                        startdate = tmpdate[0].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Update semester Information')", true);
                        return;
                    }
                }

                if (intNHrs > 0)
                {
                    if (SchOrder != 0)
                    {
                        ini_day = dt1.ToString("ddd");
                    }
                    else
                    {
                        todate = txtFromDate.Text.Trim().ToString();
                        string[] spd = todate.Split('/');
                        string curdate = spd[1] + '/' + spd[0] + '/' + spd[2];
                        ini_day = d2.findday(curdate, ddlbranch.SelectedItem.Value.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.SelectedItem.ToString(), startdate.ToString(), Convert.ToString(nodays), Convert.ToString(start_dayorder));//Added by Manikandan 25/07/2013
                    }
                }


                string getlabsub = " Select subjecT_no,subjecT_code from subject,sub_sem where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code=" + scode.ToString() + "";
                DataSet dslasu = d2.select_method_wo_parameter(getlabsub, "Text");


                sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";
                DataSet dsalter = d2.select_method_wo_parameter(sql, "Text");
                DataTable dtv = dslasu.Tables[0];
                Hashtable hatsubject = new Hashtable();
                string validsunno = "";

                if (dsalter.Tables[0].Rows.Count <= 0)
                {
                    errlbl.Text = "No Records Found";
                    errlbl.Visible = true;
                    batchpanel.Visible = false;
                    return;
                }
                errlbl.Visible = false;
                if (intNHrs > 0 && nodays > 0 && nodays <= 7)
                {
                    for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
                    {
                        stt = ini_day + IntRCtr;

                        string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
                        string[] sp = schdeva.Split(';');
                        Boolean getflag = false;
                        string othsub = "";
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
                        if (getflag == true)
                        {
                            string[] val = othsub.Split(',');
                            for (int k = 0; k <= val.GetUpperBound(0); k++)
                            {
                                string gva = val[k];
                                if (!hatsubject.Contains(gva))
                                {
                                    hatsubject.Add(gva, stt);
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
                                    gphr = gphr + ',' + stt;
                                    hatsubject[gva] = gphr;
                                }
                            }
                        }
                    }
                }
                DataSet ds_subjectnum = new DataSet();
                string subjectnumber = "";
                if (validsunno != "")
                {
                    subjectnumber = "select subjecT_no,subjecT_code from subject where subject_no in(" + validsunno + ")";

                }
                else
                {
                    subjectnumber = "select subjecT_no,subjecT_code from subject ";

                }
                ds_subjectnum = d2.select_method(subjectnumber, hat, "Text");
                for (int suc = 0; suc < ds_subjectnum.Tables[0].Rows.Count; suc++)
                {
                    col++;
                    //dtable1.Columns.Add(ds_subjectnum.Tables[0].Rows[suc]["Subject_Code"].ToString());
                    System.Text.StringBuilder dayordate = new System.Text.StringBuilder(ds_subjectnum.Tables[0].Rows[suc]["Subject_Code"].ToString());

                    AddTableColumn(dtable1, dayordate);
                    //subno.Add(col, Convert.ToString(ds_subjectnum.Tables[0].Rows[suc]["subjecT_no"]));
                    subno.Columns.Add(Convert.ToString(ds_subjectnum.Tables[0].Rows[suc]["subjecT_no"]));
                }
                Session["subno"] = subno;

                sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";//this query modified by Manikandan from above commented query on 27/08/2013
                for (int i = 0; i < dsalter.Tables[0].Rows.Count; i++)
                {
                    if (intNHrs > 0 && nodays > 0 && nodays <= 7)
                    {
                        for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
                        {
                            stt = ini_day + IntRCtr;
                            string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
                            string[] sp = schdeva.Split(';');
                            Boolean getflag = false;
                            string othsub = "";
                            for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
                            {

                                string val = sp[hr].ToString();
                                if (val.Trim() != "" && val != null)
                                {
                                    string[] spsub = val.Split('-');
                                    if (spsub.GetUpperBound(0) > 1)
                                    {
                                        string subjectnu = spsub[0].ToString();
                                        Boolean valiflag = false;
                                        if (hatsubject.Contains(subjectnu))
                                        {
                                            string gethr = hatsubject[subjectnu].ToString();
                                            string[] spi = gethr.Split(',');
                                            for (int lo = 0; lo <= spi.GetUpperBound(0); lo++)
                                            {
                                                string valhr = spi[lo].ToString();
                                                if (valhr.Trim().ToLower() == stt.Trim().ToLower())
                                                {
                                                    valiflag = true;
                                                }
                                            }
                                        }
                                        int col_cntt = 0;

                                        if (col >= 2)
                                        {

                                            for (col_cntt = 2; col_cntt <= col; col_cntt++)
                                            {
                                                if (valiflag == true)
                                                {
                                                    if (col_cntt == 2 && hr == 0)
                                                    {
                                                        dtrow1 = dtable1.NewRow();
                                                        dtable1.Rows.Add(dtrow1);

                                                    }
                                                    //tagsub = subno[col_cntt].ToString();
                                                    tagsub = Convert.ToString(subno.Columns[col_cntt].ColumnName);
                                                    if (subjectnu == tagsub)
                                                    {
                                                        Panel_sp2.Visible = true;
                                                        row = dtable1.Rows.Count;
                                                        dtable1.Rows[row - 1][0] = ini_day;
                                                        dtable1.Rows[row - 1][1] = IntRCtr.ToString();




                                                        //string strstubatchquery = "select distinct batch from subjectchooser_New,registration,sub_sem,subject Where subjectchooser_New.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser_New.subtype_no=sub_sem.subtype_no and subjectchooser_New.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and subjectchooser_New.fromdate='" + fmdate.ToString() + "' and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
                                                        //DataSet bat_set = d2.select_method_wo_parameter(strstubatchquery, "Text");

                                                        //string stroldBatch = "select distinct batch from subjectchooser,registration,sub_sem,subject Where subjectchooser.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
                                                        //DataSet dsOldBatch = d2.select_method_wo_parameter(stroldBatch, "text");


                                                        ////ddlDayOrder.Visible = false;
                                                        ////lblDayOrder.Visible = false;
                                                        //if (bat_set.Tables.Count > 0 && bat_set.Tables[0].Rows.Count > 0)
                                                        //{
                                                        //    batch_no.DataSource = bat_set;
                                                        //    batch_no.DataTextField = "batch";
                                                        //    batch_no.DataValueField = "batch";

                                                        //}
                                                        //else if (dsOldBatch.Tables.Count > 0 && dsOldBatch.Tables[0].Rows.Count > 0)
                                                        //{
                                                        //    batch_no.DataSource = dsOldBatch;
                                                        //    batch_no.DataTextField = "batch";
                                                        //    batch_no.DataValueField = "batch";
                                                        //    btn2sv.Enabled = true;

                                                        //    //string LabdayOrder = "select distinct Day_Value,Hour_Value,Stu_Batch from LabAlloc  where Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and Degree_Code=" + ddlbranch.SelectedValue.ToString() + " and Semester=" + ddlduration.SelectedValue.ToString() + "" + labSec;
                                                        //    //DataTable dtdayOrder = dir.selectDataTable(LabdayOrder);
                                                        //    //if (dtdayOrder.Rows.Count > 0)
                                                        //    //{
                                                        //    //    DataTable dtDay = dtdayOrder.DefaultView.ToTable(true, "Day_Value");
                                                        //    //    DataTable dtdicbatch = new DataTable();

                                                        //    //    if (dtDay.Rows.Count > 0)
                                                        //    //    {
                                                        //    //        ddlDayOrder.DataSource = dtDay;
                                                        //    //        ddlDayOrder.DataTextField = "Day_Value";
                                                        //    //        ddlDayOrder.DataValueField = "Day_Value";
                                                        //    //        ddlDayOrder.DataBind();
                                                        //    //        ddlDayOrder.Visible = true;
                                                        //    //        lblDayOrder.Visible = true;

                                                        //    //    }
                                                        //    //}
                                                        //}


                                                        flag = true;
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

                if (dtable1.Columns.Count > 0 && dtable1.Rows.Count > 0)
                {
                    Session["dtable1"] = dtable1;
                    Session["subno"] = subno;
                    gview1.DataSource = dtable1;
                    gview1.DataBind();
                    gview1.Visible = true;

                    btn2sv.Enabled = true;

                    string strstubatchquery = "select distinct batch from subjectchooser_New,registration,sub_sem,subject Where subjectchooser_New.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser_New.subtype_no=sub_sem.subtype_no and subjectchooser_New.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and subjectchooser_New.fromdate='" + fmdate.ToString() + "' and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
                    DataSet bat_set = d2.select_method_wo_parameter(strstubatchquery, "Text");

                    string stroldBatch = "select distinct batch from subjectchooser,registration,sub_sem,subject Where subjectchooser.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
                    DataSet dsOldBatch = d2.select_method_wo_parameter(stroldBatch, "text");



                    //ddlDayOrder.Visible = false;
                    //lblDayOrder.Visible = false;
                    if (bat_set.Tables.Count > 0 && bat_set.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < gview1.Rows.Count; i++)
                        {
                            for (int j = 1; j < gview1.Rows[i].Cells.Count - 2; j++)
                            {

                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataSource = bat_set;
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataTextField = "Batch";
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataValueField = "Batch";
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataBind();

                                gview1.Rows[i].Cells[j + 2].BackColor = Color.CornflowerBlue;
                            }
                        }
                    }
                    else if (dsOldBatch.Tables.Count > 0 && dsOldBatch.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < gview1.Rows.Count; i++)
                        {
                            for (int j = 1; j < gview1.Rows[i].Cells.Count - 2; j++)
                            {

                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataSource = dsOldBatch;
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataTextField = "Batch";
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataValueField = "Batch";
                                (gview1.Rows[i].FindControl("Chklst_lab" + j) as CheckBoxList).DataBind();

                                gview1.Rows[i].Cells[j + 2].BackColor = Color.CornflowerBlue;
                            }
                        }
                        btn2sv.Enabled = true;
                    }
                }
                for (int vis = dtable1.Columns.Count + 1; vis < gview1.HeaderRow.Cells.Count; vis++)
                {
                    gview1.Columns[vis].Visible = false;
                }



                for (int i = 0; i < gview1.Rows.Count; i++)
                {
                    for (int j = 2; j < gview1.Rows[i].Cells.Count; j++)
                    {
                        if (gview1.Rows[i].Cells[j].BackColor == Color.CornflowerBlue)
                        {
                            gview1.Rows[i].Cells[j].Enabled = true;
                        }
                        else
                        {
                            gview1.Rows[i].Cells[j].Enabled = false;
                        }
                    }
                }
            }
            if (flag == false)
            {
                Panel_sp2.Visible = false;
            }



            int IntRowCtr = 0;
            string Day_Value = "";
            string Hour_Value = "";
            int intColCtr = 0;
            string SnoStr = "";
            string Setbatch1 = "";
            string strsecvall = "";

            if (ddlsec.SelectedValue.ToString() == "-1")
            {
                strsecvall = "";
            }
            else
            {
                strsecvall = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
            }

            for (IntRowCtr = 0; IntRowCtr < gview1.Rows.Count; IntRowCtr++)
            {
                Label lbday = (Label)gview1.Rows[IntRowCtr].FindControl("lbldate") as Label;
                Label lbhr = (Label)gview1.Rows[IntRowCtr].FindControl("lblhour") as Label;
                Day_Value = lbday.Text.ToString();
                Hour_Value = lbhr.Text.ToString();
                if (Hour_Value != "" && Day_Value != "")
                {
                    for (intColCtr = 2; intColCtr < dtable1.Columns.Count; intColCtr++)
                    {
                        Setbatch1 = "";
                        //SnoStr = subno[intColCtr].ToString();
                        SnoStr = Convert.ToString(subno.Columns[intColCtr].ColumnName);
                        if (SnoStr != "")
                        {
                            string stubatchquery = "Select distinct stu_batch from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and Day_Value ='" + Day_Value + "' and Hour_Value = " + Hour_Value + "" + strsecvall + "  and fdate='" + fmdate.ToString() + "' and tdate='" + fmdate.ToString() + "' and  Subject_No = " + SnoStr.ToString() + "";
                            DataSet dsstubatch = d2.select_method_wo_parameter(stubatchquery, "Text");

                            string batchYear = "select  distinct Stu_Batch from LabAlloc  where Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and Degree_Code=" + ddlbranch.SelectedValue.ToString() + " and Semester=" + ddlduration.SelectedValue.ToString() + "" + strsecvall + " and Day_Value='" + ddlDayOrder.Text + "' and Hour_Value=" + Hour_Value + "  and  Subject_No = " + SnoStr.ToString() + "";
                            DataSet dtlab = d2.select_method_wo_parameter(batchYear, "Text");

                            if (dsstubatch.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < dsstubatch.Tables[0].Rows.Count; s++)
                                {
                                    if (Setbatch1 == "")
                                    {
                                        Setbatch1 = dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                    }
                                    else
                                    {
                                        Setbatch1 = Setbatch1 + "," + dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                    }
                                    if (gview1.Rows[IntRowCtr].Cells[intColCtr + 1].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
                                    {
                                        string[] spiltbatch = Setbatch1.Split(',');
                                        if (spiltbatch.GetUpperBound(0) > 0)
                                        {
                                            int count = 0;
                                            int r = intColCtr - 1;
                                            string name = "";
                                            TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                            CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                            for (int sel = 0; sel < chk.Items.Count; sel++)
                                            {

                                                name = Convert.ToString(chk.Items[sel].Text);
                                                for (int y = 0; y < spiltbatch.Length; y++)
                                                {
                                                    string name1 = spiltbatch[y].ToString();
                                                    if (name == name1)
                                                    {
                                                        chk.Items[sel].Selected = true;
                                                        count++;
                                                    }

                                                }
                                            }
                                            txt.Text = Setbatch1;
                                            //if (count > 0)
                                            //{
                                            //    if (count == 1)
                                            //    {
                                            //        txt.Text = "" + name + "";
                                            //    }
                                            //    else
                                            //    {
                                            //        txt.Text = "Batch" + "(" + count + ")";
                                            //    }

                                            //}
                                        }
                                        else
                                        {
                                            int count = 0;
                                            int r = intColCtr - 1;
                                            string name = "";
                                            string name1 = "";
                                            TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                            CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                            for (int sel = 0; sel < chk.Items.Count; sel++)
                                            {
                                                name = Convert.ToString(chk.Items[sel].Text);
                                                for (int y = 0; y < spiltbatch.Length; y++)
                                                {
                                                    name1 = spiltbatch[y].ToString();
                                                    if (name == name1)
                                                    {
                                                        chk.Items[sel].Selected = true;
                                                        count++;
                                                    }
                                                }
                                            }
                                            txt.Text = name1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dtlab.Tables.Count > 0 && dtlab.Tables[0].Rows.Count > 0)
                                {
                                    for (int s = 0; s < dtlab.Tables[0].Rows.Count; s++)
                                    {
                                        if (Setbatch1 == "")
                                        {
                                            Setbatch1 = dtlab.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                        }
                                        else
                                        {
                                            Setbatch1 = Setbatch1 + "," + dtlab.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                        }
                                    }
                                        if (gview1.Rows[IntRowCtr].Cells[intColCtr + 1].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
                                        {
                                            string[] spiltbatch = Setbatch1.Split(',');
                                            if (spiltbatch.GetUpperBound(0) > 0)
                                            {
                                                int count = 0;
                                                int r = intColCtr - 1;
                                                string name = ""; string name1 = string.Empty;
                                                CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                                TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                                for (int sel = 0; sel < chk.Items.Count; sel++)
                                                {
                                                    name = Convert.ToString(chk.Items[sel].Text);
                                                    for (int y = 0; y < spiltbatch.Length; y++)
                                                    {
                                                        name1 = spiltbatch[y].ToString();
                                                        if (name == name1)
                                                            chk.Items[sel].Selected = true;
                                                    }
                                                }
                                                txt.Text = Setbatch1;
                                                //if (count > 0)
                                                //{
                                                //    if (count == 1)
                                                //    {
                                                //        txt.Text = "" + name + "";
                                                //    }
                                                //    else
                                                //    {
                                                //        txt.Text = "Batch" + "(" + count + ")";
                                                //    }

                                                //}
                                            }
                                            else
                                            {
                                                int r = intColCtr - 1;
                                                string name = ""; string name1 = string.Empty;
                                                CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                                                TextBox txt = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);
                                                for (int sel = 0; sel < chk.Items.Count; sel++)
                                                {
                                                    name = Convert.ToString(chk.Items[sel].Text);
                                                    for (int y = 0; y < spiltbatch.Length; y++)
                                                    {
                                                        name1 = spiltbatch[y].ToString();
                                                        if(name==name1){
                                                            chk.Items[sel].Selected = true;
                                                        }
                                                    }
                                                }
                                                txt.Text = name1;
                                            }
                                        }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void ddlDayOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loaddays();
        }
        catch
        {

        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        int maxrow = 0;
        maxrow = gview.Rows.Count;
        if (maxrow != 0 && CheckBox1.Checked == true)
        {
            sfrlbl.Enabled = true;
            sfmtxt.Enabled = true;
            stolbl.Enabled = true;
            stotxt.Enabled = true;
            sfmtxt.Text = "";
            stotxt.Text = "";
            maxrow = gview.Rows.Count;
            Button1.Enabled = true;
            btnsave.Enabled = true;

            delbtn.Enabled = true;
            btn2sv.Enabled = true;
        }
        else
        {
            sfrlbl.Enabled = false;
            sfmtxt.Enabled = false;
            stolbl.Enabled = false;
            stotxt.Enabled = false;
            Button1.Enabled = false;
            sfmtxt.Text = "";
            stotxt.Text = "";

            btnsave.Enabled = false;
            Button1.Enabled = false;
            delbtn.Enabled = false;
            btn2sv.Enabled = false;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            Boolean selectflag = false;
            int isval;
            string col_val = "";
            Boolean savflag = false;
            errlbl.Visible = false;

            bcntlbl.Visible = true;
            btctxt.Visible = true;
            bcntddl.Visible = true;
            //bcntddllbl.Visible = true;

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());

            //if (btctxt.Text != "")
            //{
            //    string x = "";
            //    x = bcntddl.SelectedIndex.ToString();
            //    if (bcntddl.SelectedIndex.ToString() != "0" && bcntddl.SelectedIndex.ToString() != "" && bcntddl.SelectedIndex.ToString() != "-1" && bcntddl.SelectedIndex.ToString() != "--Select--")
            //    {

            //if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
            //{
            //    int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
            //    int totxt = Convert.ToInt32(stotxt.Text.ToString());

            //    for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
            //    {
            //        if (readspread <= gview.Rows.Count - 1)
            //        {
            //            System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[readspread].FindControl("chck");
            //            //val.Checked = true;

            //        }
            //    }
            //}
            //if (bcntddl.Items.Count == 0 || bcntddl.Enabled == false)
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "Please select the batch";
            //    return;
            //}

            for (int i = 0; i < gview.Rows.Count; i++)
            {
                System.Web.UI.WebControls.CheckBox val1 = (System.Web.UI.WebControls.CheckBox)gview.Rows[i].FindControl("chck");

                //if (val1.Checked)
                //{
                btnsave.Enabled = true;
                Button1.Enabled = true;
                btn2sv.Enabled = true;
                delbtn.Enabled = true;
                selectflag = true;

                string name = (gview.Rows[i].FindControl("lblroll") as Label).Text;

                col_val = name;
                string strdelsubnew = "delete subjectchooser_New  where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int insupaddelquery = d2.update_method_wo_parameter(strdelsubnew, "Text");

                //Label batchname = (Label)gview.Rows[i].FindControl("lblbatch");
                //string batchnam = batchname.Text;
                DropDownList ddl = (gview.Rows[i].FindControl("ddlbatch") as DropDownList);
                string BatchName = ddl.SelectedItem.Text;
                //if (string.IsNullOrEmpty(batchnam))
                //{

                //    //gview.Rows[i].Cells[5].Text = bcntddl.SelectedValue.ToString();
                //    //BatchName = gview.Rows[i].Cells[5].Text;
                //}
                //else
                //BatchName = batchnam;
                selectflag = true;
                savflag = true;



                DataTable dtable1 = (DataTable)Session["dtable1"];
                DataTable dttag = (DataTable)Session[""];
                for (int k = 2; k < dtable1.Columns.Count; k++)
                {
                    string subcode = dtable1.Columns[k].ColumnName.Trim();
                    string Subno = d2.GetFunction("select subjecT_no from subject where subjecT_code in('" + subcode + "')");

                    string subtypeno = d2.GetFunction("select subtype_no from subject where subject_no='" + Subno + "'");
                    Session["subject_no"] = Convert.ToString(Subno);//delsi2703
                    Session["SubjectTypeNo"] = Convert.ToString(subtypeno);
                    Session["Sem"] = Convert.ToString(ddlduration.SelectedValue);
                    Session["Batch"] = Convert.ToString(BatchName);
                    Session["fromdateVal"] = dtf;
                    Session["todateVal"] = dtf;


                    string insupdaquery = "if not exists(Select * from subjectchooser_New where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and subject_no='" + Subno + "')";
                    insupdaquery = insupdaquery + " insert into subjectchooser_New(semester,roll_no,subject_no,subtype_no,batch,fromdate,todate)values('" + ddlduration.SelectedValue.ToString() + "','" + col_val.ToString() + "','" + Subno.ToString() + "','" + subtypeno.ToString() + "','" + BatchName + "','" + dtf.ToString("MM/dd/yyyy") + "','" + dtf.ToString("MM/dd/yyyy") + "')";
                    insupdaquery = insupdaquery + " else update subjectchooser_New set batch='" + BatchName + "'  where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and subject_no='" + Subno + "'";

                    insupaddelquery = d2.update_method_wo_parameter(insupdaquery, "Text");
                }
                //}
            }
            if (selectflag == false)
            {
                errlbl.Visible = true;
                errlbl.Text = "Please select atleast one student";
            }

            if (savflag == true)
            {
                errlbl.Visible = true;
                sfmtxt.Text = "";
                stotxt.Text = "";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Batch allocated successfully')", true);
                //loadbatch();


                bcntddl.Items.Clear();
                string numbatch = "";
                int b_val = 0;
                numbatch = btctxt.Text.ToString();
                if (numbatch != "" && numbatch != "0")
                {
                    bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                    for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                    {
                        bcntddl.Items.Add("B" + b_val.ToString());
                    }
                }
            }
            //    }
            //    else
            //    {
            //        errlbl.Visible = true;
            //        errlbl.Text = "Please select the batch";
            //    }
            //}
            //else
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "Please select the no of batches";
            //}
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void btn2sv_Click(object sender, EventArgs e)
    {
        try
        {

            btnsave_Click(sender, e);
            DataTable dttag = (DataTable)Session["subno"];
            int intColCtr = 0;
            int IntRowCtr = 0;
            string Day_Value = "";
            int intSi = 0;
            string Hour_Value = "";
            string SnoStr = "";
            string strSchText = "";
            string strsec = "";
            string Stu_B = "";
            string sql = "";
            int i = 0;
            Boolean savflag = false;

            string sec = "";
            if (ddlsec.Enabled == false)
            {
                strsec = "";
            }

            else if (ddlsec.SelectedValue.ToString() != "-1" && ddlsec.SelectedValue.ToString().Trim() != "")
            {
                strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                sec = ddlsec.SelectedValue.ToString();
            }
            else
            {
                strsec = "";
            }

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());



            DataTable dtable1 = (DataTable)Session["dtable1"];

            if (dtable1.Rows.Count > 0 && dtable1.Columns.Count > 2)
            {
                sql = "Delete from LabAlloc_New where degree_code = '" + ddlbranch.SelectedValue.ToString() + "' and semester = '" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and Batch_Year ='" + ddlbatch.SelectedValue.ToString() + "' and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int dellabnew = d2.update_method_wo_parameter(sql, "Text");
            }


            for (IntRowCtr = 0; IntRowCtr < gview1.Rows.Count; IntRowCtr++)//--------------------row increment
            {
                Label day = (Label)gview1.Rows[i].FindControl("lbldate");
                Day_Value = day.Text;
                Label hour = (Label)gview1.Rows[i].FindControl("lblhour");
                Hour_Value = hour.Text;
                //Day_Value = gview1.Rows[IntRowCtr].Cells[1].Text;
                //Hour_Value = gview1.Rows[IntRowCtr].Cells[2].Text;
                if (Day_Value != "" && Hour_Value != "")
                {
                    for (intColCtr = 2; intColCtr < dtable1.Columns.Count; intColCtr++)//--col increment
                    {
                        int r = intColCtr - 1;
                        TextBox txtvalue = (gview1.Rows[IntRowCtr].FindControl("txt_lab" + r) as TextBox);

                        if (txtvalue.Text != "" && txtvalue.Text != null)
                        {
                            //string subcode = dttag.Columns[intColCtr].ColumnName.Trim();
                            //SnoStr = d2.GetFunction("select subjecT_no from subject where subjecT_code in('" + subcode + "')");
                            SnoStr = dttag.Columns[intColCtr].ColumnName.Trim();


                            CheckBoxList chk = (gview1.Rows[IntRowCtr].FindControl("Chklst_lab" + r) as CheckBoxList);
                            for (int sel = 0; sel < chk.Items.Count; sel++)
                            {
                                if (chk.Items[sel].Selected)
                                {
                                    string name = Convert.ToString(chk.Items[sel].Text);
                                    if (Stu_B == "")
                                    {
                                        Stu_B = name;
                                    }
                                    else
                                    {
                                        Stu_B = Stu_B + "," + name;
                                    }
                                }
                            }
                            //Stu_B = gview1.Rows[IntRowCtr].Cells[intColCtr].Text;
                            if (Stu_B.Trim() != "" && Stu_B != null)
                            {
                                string[] Stu_Batch = Stu_B.Split(new Char[] { ',' });

                                if (Stu_Batch.GetUpperBound(0) >= 0)
                                {
                                    for (i = 0; i <= Stu_Batch.GetUpperBound(0); i++)
                                    {
                                        if (SnoStr.ToString().Trim() != "" && Stu_Batch[i].ToString().Trim() != "" && SnoStr != null && Stu_Batch[i] != null)
                                        {
                                            sql = "select " + Day_Value + "" + Hour_Value + " from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + "" + strsec + "  and  " + Day_Value + "" + Hour_Value + " is not null";
                                            DataSet dsalsem = d2.select_method_wo_parameter(sql, "Text");
                                            if (dsalsem.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsalsem.Tables[0].Rows[0][0].ToString().Trim() != "" && dsalsem.Tables[0].Rows[0][0] != null)
                                                {
                                                    strSchText = dsalsem.Tables[0].Rows[0][0].ToString();
                                                    string[] ArSchText1 = strSchText.Split(new Char[] { ';' });
                                                    if (ArSchText1.GetUpperBound(0) >= 0)
                                                    {
                                                        for (intSi = 0; intSi <= ArSchText1.GetUpperBound(0); intSi++)
                                                        {
                                                            if (ArSchText1[intSi].ToString().Trim() != "" && ArSchText1[intSi] != null)
                                                            {
                                                                string[] CntSchText1 = ArSchText1[intSi].Split(new Char[] { '-' });
                                                                if (CntSchText1.GetUpperBound(0) >= 0)
                                                                {
                                                                    if (CntSchText1[0].ToString() == SnoStr)
                                                                    {
                                                                        Session["section"] = Convert.ToString(sec);//delsi2703
                                                                        Session["dayvalue"] = Convert.ToString(Day_Value);
                                                                        Session["hourVal"] = Convert.ToString(Hour_Value);


                                                                        string strquer = "if not exists (Select * from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and staff_code='" + CntSchText1[1].ToString() + "' and Day_Value = '" + Day_Value.ToString() + "' and Hour_Value = " + Hour_Value.ToString() + " and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "' and SubjecT_no =" + SnoStr.ToString() + " " + strsec + " and Stu_Batch='" + Stu_Batch[i].ToString() + "')";
                                                                        strquer = strquer + " insert into LabAlloc_New(Degree_Code,Semester,Batch_Year,Sections,Subject_No,Day_Value,Hour_Value,Stu_Batch,Staff_Code,fdate,tdate)values('" + ddlbranch.SelectedValue.ToString() + "','" + ddlduration.SelectedValue.ToString() + "','" + ddlbatch.SelectedValue.ToString() + "','" + sec + "','" + SnoStr.ToString() + "','" + Day_Value.ToString() + "','" + Hour_Value.ToString() + "','" + Stu_Batch[i].ToString() + "','" + CntSchText1[1].ToString() + "','" + dtf.ToString("MM/dd/yyyy") + "','" + dtf.ToString("MM/dd/yyyy") + "')";
                                                                        int updsin = d2.update_method_wo_parameter(strquer, "Text");
                                                                        savflag = true;
                                                                        if (updsin == 1)//delsi 05/04/2018
                                                                        {
                                                                            Session["toDate"] = Convert.ToString(txtFromDate.Text.ToString());

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
                            }
                        }
                    }
                }
            }
            if (savflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Batch allocated Successfully')", true);
                btnGo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void selbtn_Click(object sender, EventArgs e)
    {
        try
        {
            string from = sfmtxt.Text;
            string to = stotxt.Text;
            errlbl.Visible = false;
            string batch = string.Empty;
            //int maxrow = 0;
            //int i = 0;

            //maxrow = gview.Rows.Count;
            //if (string.IsNullOrEmpty(sfmtxt.Text))
            //    frm_date = "0";
            //if (string.IsNullOrEmpty(sfmtxt.Text))
            //    to_date = "0";


            //if (sfmtxt.Text == "")
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "Please enter from value";
            //    sfmtxt.Focus();
            //}

            //if (stotxt.Text == "")
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "Please enter to value";
            //    stotxt.Focus();
            //}

            //if (Convert.ToInt16(frm_date.ToString()) <= 0)
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "From value cannot be less than 1";
            //    sfmtxt.Text = maxrow.ToString();
            //}

            //if (Convert.ToInt16(frm_date.ToString()) > maxrow)
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "From value cannot be greater than total no of students";
            //    sfmtxt.Text = maxrow.ToString();
            //}

            //if (Convert.ToInt16(to_date.ToString()) <= 0)
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "To value cannot be less than 1";
            //    stotxt.Text = maxrow.ToString();
            //}

            //if (Convert.ToInt16(to_date.ToString()) > maxrow)
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "To value cannot be greater than total no of students";
            //    sfmtxt.Text = maxrow.ToString();
            //}

            //if (Convert.ToInt16(frm_date.ToString()) > Convert.ToInt16(to_date.ToString()))
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "From value cannot be greater than To value";
            //    sfmtxt.Focus();
            //}

            ////bcntddl.Items.Clear();
            ////string numbatch = "";
            ////int b_val = 0;
            ////numbatch = btctxt.Text.ToString();
            ////if (numbatch != "" && numbatch != "0")
            ////{
            ////    bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
            ////    for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
            ////    {
            ////        bcntddl.Items.Add("B" + b_val.ToString());
            ////    }
            ////}
            //string batch = bcntddl.SelectedItem.Text;
            //if (frm_date != "0" && to_date != "0" && !string.IsNullOrEmpty(frm_date) && !string.IsNullOrEmpty(to_date))
            //{
            //    if (!string.IsNullOrEmpty(batch) && batch != "--Select--")
            //    {
            //        for (i = Convert.ToInt16(frm_date); i <= Convert.ToInt16(to_date); i++)
            //        {
            //            System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[i - 1].FindControl("chck");
            //            val.Checked = true;
            //            Label name1 = (Label)gview.Rows[i - 1].FindControl("lblroll");
            //            string name = name1.Text;

            //            // Label batchname = (Label)gview.Rows[i - 1].FindControl("lblbatch");
            //            gview.Rows[i - 1].Cells[5].Text = batch;
            //        }
            //    }
            //}



            if (bcntddl.Text != "Select" && bcntddl.Text != "-1" && bcntddl.Text != "")
            {
                if (from != null && from != "" && to != null && to != "")
                {
                    int m = Convert.ToInt32(sfmtxt.Text);
                    int n = Convert.ToInt32(stotxt.Text);
                    if (m != 0 && n != 0)
                    {
                        if (gview.Rows.Count >= n)
                        {
                            batch = bcntddl.Text;
                            for (int rowcount = m; rowcount <= n; rowcount++)
                            {
                                if (btctxt.Text != "" && btctxt.Text != "0" && btctxt.Text != null && bcntddl.SelectedItem.ToString() != null && bcntddl.SelectedItem.ToString() != "" && bcntddl.SelectedItem.ToString() != "--Select--")
                                {
                                    DropDownList ddl = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList);
                                    (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).SelectedIndex = (gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.IndexOf((gview.Rows[rowcount - 1].FindControl("ddlbatch") as DropDownList).Items.FindByValue(bcntddl.Text));

                                    //added by srinath 31/8/2013
                                    btnsave.Enabled = true;
                                    delbtn.Enabled = true;
                                }
                                else
                                {
                                    errlbl.Visible = true;
                                    errlbl.Text = "Please Add No of Batch";
                                }
                            }
                        }
                        else
                        {
                            errlbl.Visible = true;
                            errlbl.Text = "Please Enter Available Student Count";
                        }
                    }
                    else
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Please Enter Greater than Zero";
                    }
                }
                else
                {
                    errlbl.Visible = true;
                    errlbl.Text = "Please Enter Values";
                }
            }
            else
            {
                errlbl.Visible = true;
                errlbl.Text = "Please Select Batch";
            }
            sfmtxt.Text = string.Empty;
            stotxt.Text = string.Empty;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void delbtn_Click(object sender, EventArgs e)
    {
        try
        {
            string SqlStr = "";
            int isval = 0;
            string roll_no = "";

            string strsec = "";
            string sql = "";
            Boolean delsml = false;
            Boolean blnDelete = false;

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());


            if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
            {
                int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
                int totxt = Convert.ToInt32(stotxt.Text.ToString());

                for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
                {
                    if (readspread <= gview.Rows.Count - 1)
                    {
                        System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[readspread].FindControl("chck");
                        val.Checked = true;

                    }
                }
            }

            for (int i = 0; i < gview.Rows.Count; i++)
            {
                System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[i].FindControl("chck");

                //if (val.Checked)
                //{
                //if (gview.Rows[i].Cells[6].Text != "")
                //{
                blnDelete = true;
                roll_no = (gview.Rows[i].FindControl("lblroll") as Label).Text;
                SqlStr = "delete subjectchooser_New  where roll_no='" + roll_no + "' and semester='" + ddlduration.SelectedItem.ToString() + "' and fromdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int delequery = d2.update_method_wo_parameter(SqlStr, "Text");
                //}
                //}
            }

            if (ddlsec.Enabled == false)
            {
                strsec = "";
            }

            else if (ddlsec.SelectedValue.ToString() != "-1" && ddlsec.SelectedValue.ToString().Trim() != "")
            {
                strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
            }
            else
            {
                strsec = "";
            }

            if (gview1.Rows.Count > 0 && gview1.HeaderRow.Cells.Count > 2)
            {
                sql = "Delete from LabAlloc_New where degree_code = '" + ddlbranch.SelectedValue.ToString() + "' and semester = '" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and Batch_Year ='" + ddlbatch.SelectedValue.ToString() + "' and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int delquery = d2.update_method_wo_parameter(sql, "Text");
                delsml = true;
            }
            if (blnDelete == true || delsml == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            }
            bcntddl.Items.Clear();
            string numbatch = "";
            int b_val = 0;
            numbatch = btctxt.Text.ToString();
            if (numbatch != "" && numbatch != "0")
            {
                bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                {
                    bcntddl.Items.Add("B" + b_val.ToString());
                }
            }
            loadbatch();
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void bcntddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void stotxt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
            {
                if (Convert.ToInt32(sfmtxt.Text) <= Convert.ToInt32(stotxt.Text))
                {
                    if (Convert.ToInt32(stotxt.Text) <= gview.Rows.Count)
                    {
                        errlbl.Visible = false;
                        int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
                        int totxt = Convert.ToInt32(stotxt.Text.ToString());

                        for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
                        {
                            if (readspread <= gview.Rows.Count - 1)
                            {
                                System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[readspread].FindControl("chck");
                                val.Checked = true;

                            }
                        }
                    }
                    else
                    {
                        errlbl.Text = "Only " + gview.Rows.Count + " are available";
                        errlbl.Visible = true;
                        stotxt.Text = "";
                    }
                }
            }
            btnsave.Enabled = true;
            Button1.Enabled = true;
            delbtn.Enabled = true;
            btn2sv.Enabled = true;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            //int ar = 0;
            //int ac = 0;
            //string value = "";
            //ar = sml_spread.ActiveSheetView.ActiveRow;
            //ac = sml_spread.ActiveSheetView.ActiveColumn;
            //if (ac > 1)
            //{
            //    Checkboxlistbatch.Visible = true;
            //    Button3.Visible = true;
            //    Fieldset5.Visible = true;
            //    string batchbb = sml_spread.Sheets[0].Cells[ar, ac].Text;


            //    string[] batc = batchbb.Split(',');
            //    if (batc.GetUpperBound(0) > 0)
            //    {
            //        for (int uu = 0; uu <= batc.GetUpperBound(0); uu++)
            //        {
            //            string bvv = batc[uu].ToString();
            //            for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
            //            {
            //                value = Checkboxlistbatch.Items[i].Text;

            //                if (bvv == value)
            //                {
            //                    Checkboxlistbatch.Items[i].Selected = true;
            //                }

            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
            //        {
            //            value = Checkboxlistbatch.Items[i].Text;

            //            if (batchbb == value)
            //            {
            //                Checkboxlistbatch.Items[i].Selected = true;
            //            }
            //            else
            //            {
            //                Checkboxlistbatch.Items[i].Selected = false;
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void Checkboxlistbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            string code = "";

            for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
            {
                if (Checkboxlistbatch.Items[i].Selected == true)
                {
                    value = Checkboxlistbatch.Items[i].Text;
                    code = Checkboxlistbatch.Items[i].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            string code = "";
            string batchva = "";

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
            int ar = 0;
            int ac = 0;
            //ar = sml_spread.ActiveSheetView.ActiveRow;
            //ac = sml_spread.ActiveSheetView.ActiveColumn;

            if (ac > 1)
            {
                //if (sml_spread.Sheets[0].Cells[ar, ac].BackColor == Color.CornflowerBlue)
                //{

                //    FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
                //    sml_spread.Sheets[0].Cells[ar, ac].CellType = btva;
                //    sml_spread.Sheets[0].Cells[ar, ac].Text = batchva;
                //    sml_spread.Sheets[0].Cells[ar, ac].Locked = true;
                //    Checkboxlistbatch.Visible = false;
                //}
            }

            Button3.Visible = false;
            Fieldset5.Visible = false;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }


    protected void gview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DataTable dtable1 = (DataTable)Session["dtable1"];
            for (int cell = 2; cell < dtable1.Columns.Count; cell++)
            {
                if (gview1.Columns[cell + 1].Visible == true)
                {
                    e.Row.Cells[cell + 1].Text = Convert.ToString(dtable1.Columns[cell].ColumnName);

                }
            }
        }

    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
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
        catch (Exception ex)
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
        catch (Exception ex)
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
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
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
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    //protected void batch_spread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        Boolean actflag = false;

    //        string actrow = e.CommandArgument.ToString();
    //        if (bcntddl.SelectedIndex.ToString() != "0" && bcntddl.SelectedIndex.ToString() != "" && bcntddl.SelectedIndex.ToString() != "-1" && bcntddl.SelectedIndex.ToString() != "--Select--")
    //        {
    //            for (int i = 0; i < gview.Rows.Count; i++)
    //            {
    //                int isval = 0;
    //                System.Web.UI.WebControls.CheckBox val = (System.Web.UI.WebControls.CheckBox)gview.Rows[i].FindControl("chck");

    //                if (val.Checked)
    //                {
    //                    actflag = true;
    //                    i = gview.Rows.Count;
    //                }
    //            }
    //            string val = e.EditValues[0].ToString();
    //            if (val.Trim().ToLower() == "true")
    //            {
    //                actflag = true;
    //            }
    //            if (actflag == true)
    //            {
    //                btnsave.Enabled = true;
    //                Button1.Enabled = true;
    //                delbtn.Enabled = true;
    //            }

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errlbl.Visible = true;
    //        errlbl.Text = ex.ToString();
    //    }
    //}

    //public void loaddaysNew()
    //{

    //    string scode = "";
    //    int l = 0;
    //    int intNHrs = 0;
    //    string[] WkArr = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    //    string strsec = "";
    //    string strsecval = "";
    //    int SchOrder = 0;
    //    int nodays = 0;
    //    string ini_day = "";
    //    string stt = "";
    //    string date1 = "";
    //    string fmdate = "";
    //    string sql = "";
    //    int IntRCtr = 0;
    //    int row = 0;
    //    string tagsub = "";
    //    Boolean flag = false;

    //    string todate = string.Empty;
    //    string startdate = string.Empty;
    //    string start_dayorder = string.Empty;

    //    if (ddlsec.SelectedValue.ToString() == "-1")
    //    {
    //        strsec = "";
    //    }
    //    else
    //    {
    //        strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
    //    }

    //    sml_spread.Sheets[0].RowCount = 0;
    //    sml_spread.Sheets[0].ColumnCount = 0;

    //    string strsyllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and Batch_Year = " + ddlbatch.SelectedValue.ToString() + "");
    //    if (strsyllcode.Trim() != "" && strsyllcode.Trim() != "0")
    //    {
    //        scode = strsyllcode;
    //        sml_spread.Visible = true;
    //        sml_spread.Sheets[0].ColumnCount += 1;
    //        sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //        sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

    //        sml_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day";
    //        sml_spread.Sheets[0].ColumnCount += 1;
    //        sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //        sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

    //        sml_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hour";

    //        sml_spread.Sheets[0].Columns[0].Locked = true;
    //        sml_spread.Sheets[0].Columns[1].Locked = true;

    //        if (ddlsec.SelectedValue.ToString() == "-1")
    //        {
    //            strsecval = "";
    //        }
    //        else
    //        {
    //            strsecval = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
    //        }


    //        string strseminf = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "";
    //        DataSet dsseminf = d2.select_method_wo_parameter(strseminf, "text");
    //        if (dsseminf.Tables[0].Rows.Count > 0)
    //        {
    //            if ((dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString()) != "")
    //            {
    //                intNHrs = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"]);
    //                SchOrder = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["schorder"]);
    //                nodays = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["nodays"]);
    //            }
    //        }

    //        date1 = txtFromDate.Text.ToString();
    //        string[] date_fm = date1.Split(new Char[] { '/' });
    //        fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
    //        Session["todate"] = fmdate.ToString();
    //        DateTime dt1 = Convert.ToDateTime(fmdate.ToString());

    //        string strstadquery = "select start_date,end_date,starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
    //        DataSet dsstar = d2.select_method_wo_parameter(strstadquery, "Text");

    //        if (dsstar.Tables[0].Rows.Count > 0)
    //        {
    //            if ((dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "" && (dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
    //            {
    //                start_dayorder = dsstar.Tables[0].Rows[0]["starting_dayorder"].ToString();
    //                string[] tmpdate = dsstar.Tables[0].Rows[0]["start_date"].ToString().Split(new char[] { ' ' });
    //                startdate = tmpdate[0].ToString();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Update semester Information')", true);
    //                return;
    //            }
    //        }

    //        if (intNHrs > 0)
    //        {
    //            if (SchOrder != 0)
    //            {
    //                ini_day = dt1.ToString("ddd");
    //            }
    //            else
    //            {
    //                todate = txtFromDate.Text.Trim().ToString();
    //                string[] spd = todate.Split('/');
    //                string curdate = spd[1] + '/' + spd[0] + '/' + spd[2];
    //                ini_day = d2.findday(curdate, ddlbranch.SelectedItem.Value.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.SelectedItem.ToString(), startdate.ToString(), Convert.ToString(nodays), Convert.ToString(start_dayorder));//Added by Manikandan 25/07/2013
    //            }
    //        }


    //        string getlabsub = " Select subjecT_no,subjecT_code from subject,sub_sem where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code=" + scode.ToString() + "";
    //        DataSet dslasu = d2.select_method_wo_parameter(getlabsub, "Text");


    //        sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";
    //        DataSet dsalter = d2.select_method_wo_parameter(sql, "Text");
    //        DataTable dtv = dslasu.Tables[0];
    //        Hashtable hatsubject = new Hashtable();
    //        string validsunno = "";

    //        if (dsalter.Tables[0].Rows.Count <= 0)
    //        {
    //            errlbl.Text = "No Records Found";
    //            errlbl.Visible = true;
    //            batchpanel.Visible = false;
    //            return;
    //        }
    //        errlbl.Visible = false;
    //        if (intNHrs > 0 && nodays > 0 && nodays <= 7)
    //        {
    //            for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
    //            {
    //                stt = ini_day + IntRCtr;

    //                string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
    //                string[] sp = schdeva.Split(';');
    //                Boolean getflag = false;
    //                string othsub = "";
    //                for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
    //                {
    //                    string val = sp[hr].ToString();
    //                    if (val.Trim() != "" && val != null)
    //                    {
    //                        string[] spsub = val.Split('-');
    //                        if (spsub.GetUpperBound(0) > 1)
    //                        {
    //                            dtv.DefaultView.RowFilter = " subject_no='" + spsub[0] + "'";
    //                            DataView dt = dtv.DefaultView;
    //                            if (dt.Count > 0)
    //                            {
    //                                getflag = true;
    //                            }
    //                            if (othsub == "")
    //                            {
    //                                othsub = spsub[0];
    //                            }
    //                            else
    //                            {
    //                                othsub = othsub + ',' + spsub[0];
    //                            }
    //                        }
    //                    }
    //                }
    //                if (getflag == true)
    //                {
    //                    string[] val = othsub.Split(',');
    //                    for (int k = 0; k <= val.GetUpperBound(0); k++)
    //                    {
    //                        string gva = val[k];
    //                        if (!hatsubject.Contains(gva))
    //                        {
    //                            hatsubject.Add(gva, stt);
    //                            if (validsunno == "")
    //                            {
    //                                validsunno = gva;
    //                            }
    //                            else
    //                            {
    //                                validsunno = validsunno + ',' + gva;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string gphr = hatsubject[gva].ToString();
    //                            gphr = gphr + ',' + stt;
    //                            hatsubject[gva] = gphr;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        DataSet ds_subjectnum = new DataSet();
    //        string subjectnumber = "";
    //        if (validsunno != "")
    //        {
    //            subjectnumber = "select subjecT_no,subjecT_code from subject where subject_no in(" + validsunno + ")";

    //        }
    //        else
    //        {
    //            subjectnumber = "select subjecT_no,subjecT_code from subject ";

    //        }
    //        ds_subjectnum = d2.select_method(subjectnumber, hat, "Text");
    //        for (int suc = 0; suc < ds_subjectnum.Tables[0].Rows.Count; suc++)
    //        {
    //            sml_spread.Sheets[0].ColumnCount += 1;
    //            sml_spread.Sheets[0].ColumnHeader.Cells[0, (sml_spread.Sheets[0].ColumnCount) - 1].Text = ds_subjectnum.Tables[0].Rows[suc]["Subject_Code"].ToString();
    //            sml_spread.Sheets[0].ColumnHeader.Cells[0, (sml_spread.Sheets[0].ColumnCount) - 1].Tag = ds_subjectnum.Tables[0].Rows[suc]["subjecT_no"].ToString();
    //        }


    //        sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";//this query modified by Manikandan from above commented query on 27/08/2013
    //        for (int i = 0; i < dsalter.Tables[0].Rows.Count; i++)
    //        {
    //            if (intNHrs > 0 && nodays > 0 && nodays <= 7)
    //            {
    //                for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
    //                {
    //                    stt = ini_day + IntRCtr;
    //                    string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
    //                    string[] sp = schdeva.Split(';');
    //                    Boolean getflag = false;
    //                    string othsub = "";
    //                    for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
    //                    {

    //                        string val = sp[hr].ToString();
    //                        if (val.Trim() != "" && val != null)
    //                        {
    //                            string[] spsub = val.Split('-');
    //                            if (spsub.GetUpperBound(0) > 1)
    //                            {
    //                                string subjectnu = spsub[0].ToString();
    //                                Boolean valiflag = false;
    //                                if (hatsubject.Contains(subjectnu))
    //                                {
    //                                    string gethr = hatsubject[subjectnu].ToString();
    //                                    string[] spi = gethr.Split(',');
    //                                    for (int lo = 0; lo <= spi.GetUpperBound(0); lo++)
    //                                    {
    //                                        string valhr = spi[lo].ToString();
    //                                        if (valhr.Trim().ToLower() == stt.Trim().ToLower())
    //                                        {
    //                                            valiflag = true;
    //                                        }
    //                                    }
    //                                }
    //                                int col_cntt = 0;

    //                                if (sml_spread.Sheets[0].ColumnCount > 2)
    //                                {

    //                                    for (col_cntt = 2; col_cntt < sml_spread.Sheets[0].ColumnCount; col_cntt++)
    //                                    {
    //                                        if (valiflag == true)
    //                                        {
    //                                            if (col_cntt == 2 && hr == 0)
    //                                            {
    //                                                sml_spread.Sheets[0].RowCount = sml_spread.Sheets[0].RowCount + 1;
    //                                            }
    //                                            tagsub = sml_spread.Sheets[0].ColumnHeader.Cells[0, col_cntt].Tag.ToString();

    //                                            if (subjectnu == tagsub)
    //                                            {
    //                                                Panel_sp2.Visible = true;
    //                                                row = sml_spread.Sheets[0].RowCount;
    //                                                sml_spread.Sheets[0].Cells[row - 1, 0].Text = ini_day;
    //                                                sml_spread.Sheets[0].Cells[row - 1, 1].Text = IntRCtr.ToString();
    //                                                sml_spread.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
    //                                                sml_spread.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;

    //                                                sml_spread.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
    //                                                sml_spread.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
    //                                                FarPoint.Web.Spread.ComboBoxCellType chkcell = new FarPoint.Web.Spread.ComboBoxCellType();
    //                                                sml_spread.Sheets[0].Columns[col_cntt].CellType = chkcell;

    //                                                string strstubatchquery = "select distinct batch from subjectchooser_New,registration,sub_sem,subject Where subjectchooser_New.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser_New.subtype_no=sub_sem.subtype_no and subjectchooser_New.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
    //                                                DataSet bat_set = d2.select_method_wo_parameter(strstubatchquery, "Text");

    //                                                string stroldBatch = "select distinct batch from subjectchooser,registration,sub_sem,subject Where subjectchooser.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and Batch<>' ' and Batch<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
    //                                                DataSet dsOldBatch = d2.select_method_wo_parameter(stroldBatch, "text");

    //                                                FarPoint.Web.Spread.ComboBoxCellType batch_no = new FarPoint.Web.Spread.ComboBoxCellType();

    //                                                if (bat_set.Tables.Count > 0 && bat_set.Tables[0].Rows.Count > 0)
    //                                                {

    //                                                    batch_no.DataSource = bat_set;
    //                                                    batch_no.DataTextField = "batch";
    //                                                    batch_no.DataValueField = "batch";
    //                                                }
    //                                                else if (dsOldBatch.Tables.Count > 0 && dsOldBatch.Tables[0].Rows.Count > 0)
    //                                                {
    //                                                    batch_no.DataSource = dsOldBatch;
    //                                                    batch_no.DataTextField = "batch";
    //                                                    batch_no.DataValueField = "batch";
    //                                                    btn2sv.Enabled = true;
    //                                                }
    //                                                sml_spread.ActiveSheetView.Cells[row - 1, col_cntt].CellType = batch_no;
    //                                                sml_spread.Sheets[0].Cells[row - 1, col_cntt].CellType = batch_no;
    //                                                sml_spread.Sheets[0].Cells[row - 1, col_cntt].BackColor = Color.CornflowerBlue;
    //                                                sml_spread.SaveChanges();
    //                                                flag = true;
    //                                            }

    //                                        }
    //                                    }
    //                                }

    //                            }
    //                        }


    //                    }
    //                }
    //            }
    //        }

    //        for (int i = 0; i < sml_spread.Sheets[0].RowCount; i++)
    //        {
    //            for (int j = 0; j < sml_spread.Sheets[0].ColumnCount; j++)
    //            {
    //                if (sml_spread.ActiveSheetView.Cells[i, j].BackColor == Color.CornflowerBlue)
    //                {
    //                    sml_spread.ActiveSheetView.Cells[i, j].Locked = false;
    //                }
    //                else
    //                {
    //                    sml_spread.ActiveSheetView.Cells[i, j].Locked = true;
    //                }
    //            }
    //        }
    //    }
    //    if (flag == false)
    //    {
    //        Panel_sp2.Visible = false;
    //    }

    //    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    //    int IntRowCtr = 0;
    //    string Day_Value = "";
    //    string Hour_Value = "";
    //    int intColCtr = 0;
    //    string SnoStr = "";
    //    string Setbatch1 = "";
    //    string strsecvall = "";

    //    if (ddlsec.SelectedValue.ToString() == "-1")
    //    {
    //        strsecvall = "";
    //    }
    //    else
    //    {
    //        strsecvall = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
    //    }

    //    for (IntRowCtr = 0; IntRowCtr < sml_spread.Sheets[0].RowCount; IntRowCtr++)
    //    {
    //        Day_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 0].Text;
    //        Hour_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 1].Text;
    //        if (Hour_Value != "" && Day_Value != "")
    //        {
    //            for (intColCtr = 2; intColCtr < sml_spread.Sheets[0].ColumnCount; intColCtr++)
    //            {
    //                Setbatch1 = "";
    //                SnoStr = sml_spread.Sheets[0].ColumnHeader.Cells[0, intColCtr].Tag.ToString();
    //                if (SnoStr != "")
    //                {
    //                    string stubatchquery = "Select distinct stu_batch from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and Day_Value ='" + Day_Value + "' and Hour_Value = " + Hour_Value + "" + strsecvall + "  and fdate='" + fmdate.ToString() + "' and tdate='" + fmdate.ToString() + "' and  Subject_No = " + SnoStr.ToString() + "";
    //                    DataSet dsstubatch = d2.select_method_wo_parameter(stubatchquery, "Text");
    //                    if (dsstubatch.Tables[0].Rows.Count > 0)
    //                    {
    //                        for (int s = 0; s < dsstubatch.Tables[0].Rows.Count; s++)
    //                        {
    //                            if (Setbatch1 == "")
    //                            {
    //                                Setbatch1 = dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
    //                            }
    //                            else
    //                            {
    //                                Setbatch1 = Setbatch1 + "," + dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
    //                            }
    //                            if (sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
    //                            {
    //                                string[] spiltbatch = Setbatch1.Split(',');
    //                                if (spiltbatch.GetUpperBound(0) > 0)
    //                                {
    //                                    sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].CellType = txt;
    //                                    sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text = Setbatch1;
    //                                }
    //                                else
    //                                {
    //                                    sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text = Setbatch1;
    //                                }
    //                            }
    //                        }

    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
















