using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;

public partial class Common_Subjectwise_Result : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hast = new Hashtable();

    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    static string subcode = "";
    static string degree = "";
    static string sem = "";
    static string status = "";
    static string result = "";
    static string subra1 = "";
    static string degdep = "";
    static string apraj = "";
    static string subra = "";
    static string subcod1 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Visible = false;
            lblmsg2.Visible = false;
            Label3.Visible = false;
            lblerrmsgxl.Visible = false;
            Label2.Visible = false;

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
                bindMonthandYear();
                bindSubjectName();
                binddegree();
                binddepartment();
                bindtestname();
                Rbtn.SelectedIndex = 1;
                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                ds = da.select_method_wo_parameter(Master1, "text");

                Session["strvar"] = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Daywise"] = "0";
                Session["Hourwise"] = "0";
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
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Days Scholor" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Hostel" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
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
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (ds.Tables[0].Rows[k]["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                        }
                        //if (ds.Tables[0].Rows[k]["settings"].ToString() == "Transfer")
                        //{
                        //    if (regularflag != "")
                        //    {
                        //        regularflag = regularflag + " or (registration.mode=2)";
                        //    }
                        //    else
                        //    {
                        //        regularflag = regularflag + " and ((registration.mode=2)";
                        //    }
                        //    //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                        //}

                        //if (ds.Tables[0].Rows[k]["settings"].ToString() == "Male" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        //{
                        //    genderflag = " and (sex='0'";
                        //}
                        //if (ds.Tables[0].Rows[k]["settings"].ToString() == "Female" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        //{
                        //    if (genderflag != "" && genderflag != "\0")
                        //    {
                        //        genderflag = genderflag + " or sex='1'";
                        //    }
                        //    else
                        //    {
                        //        genderflag = " and (sex='1'";
                        //    }
                        //}
                        //if (ds.Tables[0].Rows[k]["settings"].ToString() == "Day Wise" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        //{
                        //    Session["Daywise"] = "1";
                        //}
                        //if (ds.Tables[0].Rows[k]["settings"].ToString() == "Hour Wise" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                        //{
                        //    Session["Hourwise"] = "1";
                        //}
                    }
                }
            }
        }
        catch (Exception ex)
        {
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

    public void bindMonthandYear()
    {
        try
        {
            ddlexm.Items.Clear();
            ddlexm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlexm.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlexm.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlexm.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlexm.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlexm.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlexm.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlexm.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlexm.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlexm.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlexm.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlexm.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                ddlyear.Items.Add(Convert.ToString(year - l));
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void binddegree()
    {
        try
        {
            ds.Clear();
            ds = da.BindDegree(Session["single_user"].ToString(), Session["group_code"].ToString(), Session["collegecode"].ToString(), Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.Items.Clear();
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void binddepartment()
    {
        try
        {
            hast.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
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
            ds.Clear();
            ds = da.select_method("bind_branch", hast, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddldept.Items.Clear();
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "degree_code";
                ddldept.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void bindSubjectName()
    {
        try
        {
            cblsubject.Items.Clear();
            string sql = "";
            Hashtable hstbl = new Hashtable();

            string buildvalue = "";

            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    hstbl.Add(cbltest.Items[i].Text, cbltest.Items[i].Text);
                    string build = cbltest.Items[i].Value.ToString();
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

            //if (ddlexm.SelectedValue != "" && ddlyear.SelectedValue != "")
            //{
            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                if (Rbtn.Items[1].Selected == true)
                {
                    sql = "select distinct s.subject_name,s.subject_code,lab from Mark_Entry m,Exam_Details e,subject s,sub_sem u where s.subType_no = u.subType_no and m.subject_no=s.subject_no and m.exam_code=e.exam_code and e.exam_Month='" + ddlexm.SelectedValue + "' and e.Exam_year='" + ddlyear.SelectedValue + "' and s.CommonSub=1 order by lab,s.subject_code";
                }
                else
                {
                    sql = "select distinct s.subject_code,s.subject_name from subject s,syllabus_master sy,Registration r,CriteriaForInternal c,Exam_type e where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and s.CommonSub=1 and s.syll_code=sy.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and CC=0 and DelFlag=0 and Exam_Flag<>'debar' and c.criteria in ('" + buildvalue + "') and r.degree_code= '" + ddldept.SelectedValue + "'";
                }
            }
            else
            {
                sql = "select distinct s.subject_name,m.subject_no,lab from Mark_Entry m,Exam_Details e,subject s,sub_sem u where s.subType_no = u.subType_no and m.subject_no=s.subject_no and m.exam_code=e.exam_code and  e.exam_Month='" + ddlexm.SelectedValue + "' and e.Exam_year='" + ddlyear.SelectedValue + "' and e.degree_code='" + ddldept.SelectedValue.ToString() + "' order by lab,m.subject_no";
            }
            //}
            //else
            //{
            //    if (ddlsubtype.SelectedItem.Text == "Common")
            //    {
            //        if (Rbtn.Items[1].Selected == true)
            //        {
            //            sql = "select distinct s.subject_name,s.subject_code from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code and s.CommonSub=1 order by s.subject_code";
            //        }
            //        else
            //        {
            //            sql = "select distinct s.subject_code,s.subject_name from subject s,syllabus_master sy,Registration r,CriteriaForInternal c,Exam_type e where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and s.CommonSub=1 and s.syll_code=sy.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and CC=0 and DelFlag=0 and Exam_Flag<>'debar'";
            //        }
            //    }
            //    else
            //    {
            //        sql = "select distinct s.subject_name,m.subject_no from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code order by m.subject_no";
            //    }
            //}

            if (ddldept.SelectedValue.ToString() != "")
            {
                ds = da.select_method_wo_parameter(sql, "text");

                if (ddlsubtype.SelectedItem.Text == "Common")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cblsubject.Items.Clear();
                        cblsubject.DataSource = ds;
                        cblsubject.DataTextField = "subject_name";
                        cblsubject.DataValueField = "subject_code";
                        cblsubject.DataBind();
                    }
                    else
                    {
                        txtsubject.Text = "---Select---";
                        cbsubject.Checked = false;
                        //cblsubject.Items.Clear();
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cblsubject.DataSource = ds;
                        cblsubject.DataTextField = "subject_name";
                        cblsubject.DataValueField = "subject_no";
                        cblsubject.DataBind();
                    }
                }
                if (cblsubject.Items.Count > 0)
                {
                    int cout = 0;
                    for (int i = 0; i < cblsubject.Items.Count; i++)
                    {
                        cout++;
                        cblsubject.Items[i].Selected = true;
                        cbsubject.Checked = true;
                        txtsubject.Text = "Subject Name(" + cout + ")";
                    }
                }
                else
                {
                    txtsubject.Text = "---Select---";
                    cbsubject.Checked = false;
                }
            }
            //bindtestname();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }


    protected void bindtestname()
    {
        try
        {
            cbltest.Items.Clear();
            if (ddldept.SelectedValue.ToString() != "")
            {
                //string qreryraj = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddldept.SelectedValue.ToString() + "'   order by criteria";

                string qreryraj = "select distinct criteria from criteriaforinternal,syllabus_master sy,Registration r where criteriaforinternal.syll_code=sy.syll_code and r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and sy.degree_code='" + ddldept.SelectedValue.ToString() + "' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' order by criteria";
                ds.Clear();
                ds = da.select_method_wo_parameter(qreryraj, "Text");
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    cbltest.DataSource = ds;
                    cbltest.DataTextField = "criteria";
                    cbltest.DataValueField = "criteria";
                    cbltest.DataBind();
                }

                if (cbltest.Items.Count > 0)
                {
                    int cout = 0;
                    for (int i = 0; i < cbltest.Items.Count; i++)
                    {
                        cout++;
                        cbltest.Items[i].Selected = true;
                    }
                    cbtest.Checked = true;
                    txttest.Text = "Test(" + cout + ")";
                }
                else
                {
                    cbtest.Checked = false;
                    txttest.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddlsubtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                if (Rbtn.SelectedItem.Text != "CAM wise")
                {
                    lblmonth.Visible = true;
                    ddlexm.Visible = true;
                    lblyear.Visible = true;
                    ddlyear.Visible = true;
                    lblTestname.Visible = false;
                    txttest.Visible = false;
                    lblsubject.Visible = true;
                    txtsubject.Visible = true;
                    pnlsubject.Visible = true;
                    cbsubject.Visible = true;
                    cblsubject.Visible = true;
                    lbldegree.Visible = false;
                    ddldegree.Visible = false;
                    lbldept.Visible = false;
                    ddldept.Visible = false;
                    Label1.Visible = false;
                    CommonClick.Visible = false;
                    Rbtn.SelectedIndex = 1;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }
                else
                {
                    bindSubjectName();
                    lblmonth.Visible = false;
                    ddlexm.Visible = false;
                    lblyear.Visible = false;
                    ddlyear.Visible = false;
                    lblTestname.Visible = true;
                    txttest.Visible = true;
                    lblsubject.Visible = true;
                    txtsubject.Visible = true;
                    pnlsubject.Visible = true;
                    cbsubject.Visible = true;
                    cblsubject.Visible = true;
                    lbldegree.Visible = true;
                    ddldegree.Visible = true;
                    lbldept.Visible = true;
                    ddldept.Visible = true;
                    Label1.Visible = false;
                    CommonClick.Visible = false;
                    Rbtn.SelectedIndex = 0;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }
            }
            else if (ddlsubtype.SelectedItem.Text == "General")
            {
                //bindSubjectName();
                lblsubject.Visible = false;
                txtsubject.Visible = false;
                pnlsubject.Visible = false;
                cbsubject.Visible = false;
                cblsubject.Visible = false;
                lbldegree.Visible = true;
                ddldegree.Visible = true;
                Label1.Visible = false;
                CommonClick.Visible = false;
                lbldept.Visible = true;
                ddldept.Visible = true;
                Genaralchart.Visible = false;
                GenaralGrid.Visible = false;
                Generalreportgrid.Visible = false;
                Fpspread.Visible = false;
                lblexportxl.Visible = false;
                txtexcelname.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
                Commongrid.Visible = false;
                BtnReport.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddlexm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindSubjectName();
            Genaralchart.Visible = false;
            GenaralGrid.Visible = false;
            CommonClick.Visible = false;
            Generalreportgrid.Visible = false;
            Fpspread.Visible = false;
            lblexportxl.Visible = false;
            txtexcelname.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            Commongrid.Visible = false;
            BtnReport.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            Label1.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddlyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindSubjectName();
            Genaralchart.Visible = false;
            GenaralGrid.Visible = false;
            CommonClick.Visible = false;
            Generalreportgrid.Visible = false;
            Fpspread.Visible = false;
            lblexportxl.Visible = false;
            txtexcelname.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            Commongrid.Visible = false;
            BtnReport.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            Label1.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddepartment();
            bindtestname();
            Genaralchart.Visible = false;
            GenaralGrid.Visible = false;
            Generalreportgrid.Visible = false;
            Fpspread.Visible = false;
            lblexportxl.Visible = false;
            txtexcelname.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            Commongrid.Visible = false;
            BtnReport.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            Label1.Visible = false;
            CommonClick.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void ddldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindtestname();
            Genaralchart.Visible = false;
            GenaralGrid.Visible = false;
            Generalreportgrid.Visible = false;
            Fpspread.Visible = false;
            lblexportxl.Visible = false;
            txtexcelname.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            Commongrid.Visible = false;
            BtnReport.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            lblmsg2.Visible = false;
            Label1.Visible = false;
            CommonClick.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void cbsubject_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsubject.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblsubject.Items.Count; i++)
                {
                    cout++;
                    cblsubject.Items[i].Selected = true;
                    cbsubject.Checked = true;
                    txtsubject.Text = "Subject Name(" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblsubject.Items.Count; i++)
                {
                    cout++;
                    cblsubject.Items[i].Selected = false;
                    txtsubject.Text = "---Select---";
                    cbsubject.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void cblsubject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbsubject.Checked = false;
            txtsubject.Text = "---Select---";
            for (int i = 0; i < cblsubject.Items.Count; i++)
            {
                if (cblsubject.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtsubject.Text = "Subject Name(" + cout + ")";
                if (cout == cblsubject.Items.Count)
                {
                    cbsubject.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void cbtest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbtest.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = true;

                }
                cbtest.Checked = true;
                txttest.Text = "Test(" + cout + ")";
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = false;

                }
                cbtest.Checked = false;
                txttest.Text = "---Select---";
            }
            bindSubjectName();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void cbltest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbtest.Checked = false;
            txttest.Text = "---Select---";
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txttest.Text = "Test(" + cout + ")";
                if (cout == cbltest.Items.Count)
                {
                    cbtest.Checked = true;
                }
            }
            bindSubjectName();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void Rbtn_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                if (Rbtn.SelectedItem.Text == "CAM wise")
                {
                    //bindtestname();
                    bindSubjectName();
                    pnltest.Visible = true;
                    Subjtype.Visible = true;
                    ddlsubtype.Visible = true;
                    lblsubject.Visible = true;
                    txtsubject.Visible = true;
                    lblmonth.Visible = false;
                    ddlexm.Visible = false;
                    lblyear.Visible = false;
                    ddlyear.Visible = false;
                    Commongrid.Visible = false;
                    CommonClick.Visible = false;
                    Print.Visible = false;
                    Excel.Visible = false;

                    pnlsubject.Visible = true;
                    //cbsubject.Visible = false;
                    //cblsubject.Visible = false;
                    Label1.Visible = false;
                    lbldegree.Visible = true;
                    lbldegree.Visible = true;
                    ddldegree.Visible = true;
                    lbldept.Visible = true;
                    ddldept.Visible = true;
                    lblTestname.Visible = true;
                    txttest.Visible = true;

                    cbtest.Visible = true;
                    cbltest.Visible = true;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }
                else if (Rbtn.SelectedItem.Text == "University wise")
                {
                    bindSubjectName();
                    ddlsubtype.ClearSelection();
                    lbldegree.Visible = false;
                    ddldegree.Visible = false;
                    lbldept.Visible = false;
                    ddldept.Visible = false;
                    lblTestname.Visible = false;
                    txttest.Visible = false;
                    pnltest.Visible = false;
                    cbtest.Visible = false;
                    cbltest.Visible = false;
                    Print.Visible = false;
                    Excel.Visible = false;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    BtnReport.Visible = false;
                    Label1.Visible = false;
                    lblmonth.Visible = true;
                    ddlexm.Visible = true;
                    lblyear.Visible = true;
                    ddlyear.Visible = true;
                    lblsubject.Visible = true;
                    txtsubject.Visible = true;
                    pnlsubject.Visible = true;
                    cbsubject.Visible = true;
                    cblsubject.Visible = true;
                    Subjtype.Visible = true;
                    ddlsubtype.Visible = true;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                    CommonClick.Visible = false;
                }
            }
            else if (ddlsubtype.SelectedItem.Text == "General")
            {
                if (Rbtn.SelectedItem.Text == "CAM wise")
                {
                    lblmonth.Visible = false;
                    ddlexm.Visible = false;
                    lblyear.Visible = false;
                    ddlyear.Visible = false;
                    Commongrid.Visible = false;
                    CommonClick.Visible = false;
                    Print.Visible = false;
                    Excel.Visible = false;
                    lblsubject.Visible = false;
                    txtsubject.Visible = false;
                    pnlsubject.Visible = false;
                    cbsubject.Visible = false;
                    cblsubject.Visible = false;
                    Subjtype.Visible = true;
                    ddlsubtype.Visible = true;
                    Label1.Visible = false;
                    lbldegree.Visible = true;
                    lbldegree.Visible = true;
                    ddldegree.Visible = true;
                    lbldept.Visible = true;
                    ddldept.Visible = true;
                    lblTestname.Visible = true;
                    txttest.Visible = true;
                    pnltest.Visible = true;
                    cbtest.Visible = true;
                    cbltest.Visible = true;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }
                else if (Rbtn.SelectedItem.Text == "University wise")
                {
                    ddlsubtype.ClearSelection();
                    //ddlexm.ClearSelection();
                    //ddlyear.ClearSelection();

                    lbldegree.Visible = false;
                    ddldegree.Visible = false;
                    lbldept.Visible = false;
                    ddldept.Visible = false;
                    lblTestname.Visible = false;
                    txttest.Visible = false;
                    pnltest.Visible = false;
                    cbtest.Visible = false;
                    cbltest.Visible = false;
                    Print.Visible = false;
                    Excel.Visible = false;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    BtnReport.Visible = false;
                    Label1.Visible = false;
                    lblmonth.Visible = true;
                    ddlexm.Visible = true;
                    lblyear.Visible = true;
                    ddlyear.Visible = true;
                    lblsubject.Visible = true;
                    txtsubject.Visible = true;
                    pnlsubject.Visible = true;
                    cbsubject.Visible = true;
                    cblsubject.Visible = true;
                    Subjtype.Visible = true;
                    ddlsubtype.Visible = true;
                    Genaralchart.Visible = false;
                    GenaralGrid.Visible = false;
                    Generalreportgrid.Visible = false;
                    Fpspread.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcelname.Visible = false;
                    g1btnexcel.Visible = false;
                    g1btnprint.Visible = false;
                    Commongrid.Visible = false;
                    BtnReport.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                    CommonClick.Visible = false;

                    bindSubjectName();
                }
            }

            //if (Rbtn.SelectedItem.Text == "CAM wise")
            //{
            //    if (ddlsubtype.SelectedItem.Text == "Common")
            //    {
            //        Subjtype.Visible = true;
            //        ddlsubtype.Visible = true;
            //        lblsubject.Visible = false;
            //        txtsubject.Visible = false;

            //        lblmonth.Visible = false;
            //        ddlexm.Visible = false;
            //        lblyear.Visible = false;
            //        ddlyear.Visible = false;
            //        Commongrid.Visible = false;
            //        CommonClick.Visible = false;
            //        Print.Visible = false;
            //        Excel.Visible = false;
            //        pnlsubject.Visible = false;
            //        cbsubject.Visible = false;
            //        cblsubject.Visible = false;
            //        Label1.Visible = false;
            //        lbldegree.Visible = true;
            //        lbldegree.Visible = true;
            //        ddldegree.Visible = true;
            //        lbldept.Visible = true;
            //        ddldept.Visible = true;
            //        lblTestname.Visible = true;
            //        txttest.Visible = true;
            //        pnltest.Visible = true;
            //        cbtest.Visible = true;
            //        cbltest.Visible = true;
            //        Genaralchart.Visible = false;
            //        GenaralGrid.Visible = false;
            //        Generalreportgrid.Visible = false;
            //        Fpspread.Visible = false;
            //        lblexportxl.Visible = false;
            //        txtexcelname.Visible = false;
            //        g1btnexcel.Visible = false;
            //        g1btnprint.Visible = false;
            //        Commongrid.Visible = false;
            //        BtnReport.Visible = false;
            //        Excel.Visible = false;
            //        Print.Visible = false;
            //    }
            //    else if (ddlsubtype.SelectedItem.Text == "General")
            //    {
            //        lblmonth.Visible = false;
            //        ddlexm.Visible = false;
            //        lblyear.Visible = false;
            //        ddlyear.Visible = false;
            //        Commongrid.Visible = false;
            //        CommonClick.Visible = false;
            //        Print.Visible = false;
            //        Excel.Visible = false;
            //        lblsubject.Visible = false;
            //        txtsubject.Visible = false;
            //        pnlsubject.Visible = false;
            //        cbsubject.Visible = false;
            //        cblsubject.Visible = false;
            //        Subjtype.Visible = true;
            //        ddlsubtype.Visible = true;
            //        Label1.Visible = false;
            //        lbldegree.Visible = true;
            //        lbldegree.Visible = true;
            //        ddldegree.Visible = true;
            //        lbldept.Visible = true;
            //        ddldept.Visible = true;
            //        lblTestname.Visible = true;
            //        txttest.Visible = true;
            //        pnltest.Visible = true;
            //        cbtest.Visible = true;
            //        cbltest.Visible = true;
            //        Genaralchart.Visible = false;
            //        GenaralGrid.Visible = false;
            //        Generalreportgrid.Visible = false;
            //        Fpspread.Visible = false;
            //        lblexportxl.Visible = false;
            //        txtexcelname.Visible = false;
            //        g1btnexcel.Visible = false;
            //        g1btnprint.Visible = false;
            //        Commongrid.Visible = false;
            //        BtnReport.Visible = false;
            //        Excel.Visible = false;
            //        Print.Visible = false;
            //    }

            //}
            //else if (Rbtn.SelectedItem.Text == "University wise")
            //{
            //    ddlsubtype.ClearSelection();
            //    lbldegree.Visible = false;
            //    ddldegree.Visible = false;
            //    lbldept.Visible = false;
            //    ddldept.Visible = false;
            //    lblTestname.Visible = false;
            //    txttest.Visible = false;
            //    pnltest.Visible = false;
            //    cbtest.Visible = false;
            //    cbltest.Visible = false;
            //    Print.Visible = false;
            //    Excel.Visible = false;
            //    Genaralchart.Visible = false;
            //    GenaralGrid.Visible = false;
            //    BtnReport.Visible = false;
            //    Label1.Visible = false;
            //    lblmonth.Visible = true;
            //    ddlexm.Visible = true;
            //    lblyear.Visible = true;
            //    ddlyear.Visible = true;
            //    lblsubject.Visible = true;
            //    txtsubject.Visible = true;
            //    pnlsubject.Visible = true;
            //    cbsubject.Visible = true;
            //    cblsubject.Visible = true;
            //    Subjtype.Visible = true;
            //    ddlsubtype.Visible = true;
            //    Genaralchart.Visible = false;
            //    GenaralGrid.Visible = false;
            //    Generalreportgrid.Visible = false;
            //    Fpspread.Visible = false;
            //    lblexportxl.Visible = false;
            //    txtexcelname.Visible = false;
            //    g1btnexcel.Visible = false;
            //    g1btnprint.Visible = false;
            //    Commongrid.Visible = false;
            //    BtnReport.Visible = false;
            //    Excel.Visible = false;
            //    Print.Visible = false;
            //    CommonClick.Visible = false;
            //}
            //pnltest.Visible = false;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            BtnReport.Visible = false;
            string sub = "";
            string sub1 = "";
            ArrayList addarray = new ArrayList();
            ArrayList al = new ArrayList();
            DataView dv = new DataView();
            DataView dv1 = new DataView();

            //for (int i = 0; i < cblsubject.Items.Count; i++)
            //{
            //    if (cblsubject.Items[i].Selected == true)
            //    {
            //        sub = cblsubject.Items[i].Text.ToString();
            //        if (sub1 == "")
            //        {
            //            sub1 = sub;
            //        }
            //        else
            //        {
            //            sub1 = sub1 + "'" + "," + "'" + sub;
            //        }
            //    }
            //}
            ArrayList ar = new ArrayList();
            DataTable dt = new DataTable();
            DataSet ds1 = new DataSet();
            DataRow row = null;
            DataView dvw1 = new DataView();
            Boolean flagvar = false;
            Boolean flagcommon = false;

            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                if (Rbtn.Items[1].Selected == true)
                {
                    dt.Columns.Add("S.No", typeof(string));
                    dt.Columns.Add("Subcode", typeof(string));
                    dt.Columns.Add("Subject No", typeof(string));
                    dt.Columns.Add("SubName", typeof(string));
                    dt.Columns.Add("Dept", typeof(string));
                    dt.Columns.Add("Appeared", typeof(string));
                    dt.Columns.Add("Passed", typeof(string));
                    dt.Columns.Add("Failed", typeof(string));
                    dt.Columns.Add("Absentees", typeof(string));
                    dt.Columns.Add("Pass%", typeof(string));

                    if (cblsubject.Items.Count > 0)
                    {
                        for (int i2 = 0; i2 < cblsubject.Items.Count; i2++)
                        {
                            if (cblsubject.Items[i2].Selected == true)
                            {
                                sub1 = cblsubject.Items[i2].Value;
                                string sub2 = "select subject_no from subject where subject_code = '" + sub1 + "'";
                                DataSet dsub = da.select_method_wo_parameter(sub2, "text");

                                if (dsub.Tables[0].Rows.Count > 0)
                                {
                                    for (int su = 0; su < dsub.Tables[0].Rows.Count; su++)
                                    {
                                        string subra = dsub.Tables[0].Rows[su]["subject_no"].ToString();
                                        if (Rbtn.Items[1].Selected == true)
                                        {
                                            if (txtsubject.Text != "---Select---")
                                            {
                                                string SQL = "select COUNT(distinct roll_no) as appeared,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and result !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_no in  ('" + subra + "')  group by s.subject_code,s.subject_name,s.subject_no,d.Acronym   select COUNT(distinct roll_no) as pass,s.subject_no,s.subject_code,s.subject_name  from subject s,syllabus_master y,mark_entry m, Degree d,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('Pass') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no in  ('" + subra + "') and roll_no not in (select distinct roll_no from subject s,syllabus_master y, mark_entry m, Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('fail') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no in ('" + subra + "')) group by s.subject_code,s.subject_name,s.subject_no,d.Acronym  select COUNT(distinct roll_no) as fail,s.subject_no,s.subject_code,s.subject_name from subject s,syllabus_master y, mark_entry m, Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('fail','AAA') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no in  ('" + subra + "') group by s.subject_code,s.subject_name,s.subject_no,d.Acronym  select COUNT(distinct roll_no) as abscent,s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master y, mark_entry m, Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result  in ('AAA','WHD','W') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no in ('" + subra + "') group by s.subject_code,s.subject_name,s.subject_no,d.Acronym  ";
                                                ds.Clear();
                                                ds = da.select_method_wo_parameter(SQL, "Text");
                                            }
                                            else
                                            {
                                                lblmsg.Visible = true;
                                                lblmsg.Text = "Please Select Any One Subject";
                                                Commongrid.Visible = false;
                                                GenaralGrid.Visible = false;
                                                Genaralchart.Visible = false;
                                                CommonClick.Visible = false;
                                                Generalreportgrid.Visible = false;
                                                Fpspread.Visible = false;
                                                lblexportxl.Visible = false;
                                                txtexcelname.Visible = false;
                                                g1btnexcel.Visible = false;
                                                g1btnprint.Visible = false;
                                                Excel.Visible = false;
                                                Print.Visible = false;
                                                Label1.Visible = false;
                                                return;
                                            }

                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                flagcommon = true;
                                                if (cblsubject.Items[i2].Selected == true)
                                                {
                                                    ds.Tables[0].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i2].Value + "' ";
                                                    DataView dview = ds.Tables[0].DefaultView;
                                                    if (dview.Count > 0)
                                                    {
                                                        row = dt.NewRow();
                                                        row[3] = Convert.ToString(dview[0]["subject_code"]) + "-" + Convert.ToString(dview[0]["subject_name"]);
                                                        dt.Rows.Add(row);
                                                        al.Add(dview[0]["subject_code"]);
                                                        addarray.Add(dt.Rows.Count);

                                                        row = dt.NewRow();
                                                        row[1] = Convert.ToString(dview[0]["subject_code"]);
                                                        row[2] = Convert.ToString(dview[0]["subject_no"]);
                                                        row[3] = Convert.ToString(dview[0]["subject_name"]);
                                                        row[4] = Convert.ToString(dview[0]["Acronym"]);
                                                        row[5] = Convert.ToString(dview[0]["appeared"]);

                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i2].Value + "' ";
                                                            dvw1 = ds.Tables[1].DefaultView;
                                                            if (dvw1.Count > 0)
                                                            {
                                                                row[6] = Convert.ToString(dvw1[0]["pass"]);
                                                                double appeared = Convert.ToDouble(dview[0]["appeared"]);
                                                                double pass = Convert.ToDouble(dvw1[0]["pass"]);
                                                                double percentage = pass / appeared * 100;
                                                                percentage = Math.Round(percentage, 2);
                                                                row[9] = Convert.ToString(percentage);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            row[6] = 0;
                                                            row[9] = 0;
                                                        }

                                                        if (ds.Tables[2].Rows.Count > 0)
                                                        {
                                                            dvw1 = null;
                                                            ds.Tables[2].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i2].Value + "' ";
                                                            dvw1 = ds.Tables[2].DefaultView;
                                                            if (dvw1.Count > 0)
                                                            {
                                                                row[7] = Convert.ToString(dvw1[0]["fail"]);
                                                            }
                                                            else
                                                            {
                                                                row[7] = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            row[7] = 0;
                                                        }

                                                        if (ds.Tables[3].Rows.Count > 0)
                                                        {
                                                            dvw1 = null;
                                                            ds.Tables[3].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i2].Value + "' ";
                                                            dvw1 = ds.Tables[3].DefaultView;
                                                            if (dvw1.Count > 0)
                                                            {
                                                                row[8] = Convert.ToString(dvw1[0]["abscent"]);
                                                            }
                                                            else
                                                            {
                                                                row[8] = 0;
                                                            }
                                                        }

                                                        else
                                                        {
                                                            row[8] = 0;
                                                        }
                                                        dt.Rows.Add(row);
                                                    }
                                                }

                                                if (dt.Rows.Count > 0)
                                                {
                                                    Commongrid.DataSource = dt;
                                                    Commongrid.DataBind();
                                                    //Commongrid.Visible = true;
                                                    //CommonClick.Visible = false;
                                                    //GenaralGrid.Visible = false;
                                                    //Generalreportgrid.Visible = false;
                                                    //Fpspread.Visible = false;
                                                    //lblexportxl.Visible = false;
                                                    //txtexcelname.Visible = false;
                                                    //g1btnexcel.Visible = false;
                                                    //g1btnprint.Visible = false;
                                                    //BtnReport.Visible = false;
                                                    //Excel.Visible = true;
                                                    //Print.Visible = true;
                                                    //Label1.Visible = false;
                                                    //Label3.Visible = false;
                                                    //lblmsg.Visible = false;

                                                    if (addarray.Count > 0)
                                                    {
                                                        for (int add = 0; add < addarray.Count; add++)
                                                        {
                                                            int row_value = Convert.ToInt32(addarray[add]);
                                                            row_value = row_value - 1;
                                                            int chell_kutty = row_value + 1;
                                                            string getvalue = Convert.ToString(Commongrid.Rows[row_value].Cells[3].Text);
                                                            Commongrid.Rows[row_value].Cells[0].ColumnSpan = 10;
                                                            Commongrid.Rows[row_value].Cells[1].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[2].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[3].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[4].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[5].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[6].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[7].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[8].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[9].Visible = false;
                                                            Commongrid.Rows[row_value].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                                            Commongrid.Rows[row_value].Cells[0].Text = Convert.ToString(getvalue);
                                                            Commongrid.Rows[chell_kutty].Cells[0].Text = Convert.ToString(add + 1);
                                                            Commongrid.Rows[chell_kutty].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                }

                                                for (int j = 0; j < Commongrid.Rows.Count; j++)
                                                {
                                                    Commongrid.Rows[j].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                                                    Commongrid.Rows[j].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                                    Commongrid.Rows[j].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                                    Commongrid.Rows[j].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                                    Commongrid.Rows[j].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                                    Commongrid.Rows[j].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                                                    Commongrid.Rows[j].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                            else
                                            {
                                                lblmsg.Visible = true;
                                                lblmsg.Text = "No Records Found";
                                                Commongrid.Visible = false;
                                                GenaralGrid.Visible = false;
                                                Genaralchart.Visible = false;
                                                CommonClick.Visible = false;
                                                Label1.Visible = false;
                                                Generalreportgrid.Visible = false;
                                                Fpspread.Visible = false;
                                                lblexportxl.Visible = false;
                                                txtexcelname.Visible = false;
                                                g1btnexcel.Visible = false;
                                                g1btnprint.Visible = false;
                                                Excel.Visible = false;
                                                Print.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Please Select Any One Subject";
                                Commongrid.Visible = false;
                                GenaralGrid.Visible = false;
                                Genaralchart.Visible = false;
                                CommonClick.Visible = false;
                                Label1.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Excel.Visible = false;
                                Print.Visible = false;
                            }
                        }
                        if (flagcommon == true)
                        {
                            Commongrid.Visible = true;
                            CommonClick.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            BtnReport.Visible = false;
                            Excel.Visible = true;
                            Print.Visible = true;
                            Label1.Visible = false;
                            Label3.Visible = false;
                            lblmsg.Visible = false;
                            Label2.Visible = false;
                        }
                        //else
                        //{
                        //    lblmsg.Visible = true;
                        //    lblmsg.Text = "No Records Found";
                        //    Commongrid.Visible = false;
                        //    GenaralGrid.Visible = false;
                        //    Genaralchart.Visible = false;
                        //    CommonClick.Visible = false;
                        //    Label1.Visible = false;
                        //    Generalreportgrid.Visible = false;
                        //    Fpspread.Visible = false;
                        //    lblexportxl.Visible = false;
                        //    txtexcelname.Visible = false;
                        //    g1btnexcel.Visible = false;
                        //    g1btnprint.Visible = false;
                        //    Excel.Visible = false;
                        //    Print.Visible = false;
                        //}
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Any One Subject";
                        Commongrid.Visible = false;
                        GenaralGrid.Visible = false;
                        Genaralchart.Visible = false;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Generalreportgrid.Visible = false;
                        Fpspread.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcelname.Visible = false;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        Excel.Visible = false;
                        Print.Visible = false;
                        Label2.Visible = false;
                    }
                }
                else
                {
                    string build = "";
                    string buildval = "";
                    int sno = 1;
                    Boolean flagv = false;
                    string strraj = "";
                    int sno1 = 1;

                    dt.Columns.Add("S.No", typeof(string));
                    dt.Columns.Add("Subcode", typeof(string));
                    dt.Columns.Add("SubName", typeof(string));
                    dt.Columns.Add("Test Name", typeof(string));
                    dt.Columns.Add("Appeared", typeof(string));
                    dt.Columns.Add("Passed", typeof(string));
                    dt.Columns.Add("Failed", typeof(string));
                    dt.Columns.Add("Absentees", typeof(string));
                    dt.Columns.Add("Pass%", typeof(string));

                    if (cbltest.Items.Count > 0)
                    {
                        for (int i = 0; i < cbltest.Items.Count; i++)
                        {
                            sno = 0;


                            if (cbltest.Items[i].Selected == true)
                            {
                                buildval = cbltest.Items[i].Value;

                                string sub2 = "select distinct criteria_no from criteriaforinternal c,Registration r,syllabus_master sy where sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester=r.Current_Semester and sy.syll_code=c.syll_code and c.criteria= '" + buildval + "' and r.cc=0 and r.DelFlag=0 and sy.degree_code='" + ddldept.SelectedValue.ToString() + "' and r.Exam_Flag<>'debar'";
                                DataSet dsub = new DataSet();
                                dsub.Clear();
                                dsub = da.select_method_wo_parameter(sub2, "text");

                                if (dsub.Tables[0].Rows.Count > 0)
                                {
                                    for (int su = 0; su < dsub.Tables[0].Rows.Count; su++)
                                    {
                                        string subra = dsub.Tables[0].Rows[su]["criteria_no"].ToString();

                                        string buildvalue1 = "";
                                        for (int im = 0; im < cblsubject.Items.Count; im++)
                                        {
                                            if (cblsubject.Items[im].Selected == true)
                                            {
                                                string build1 = cblsubject.Items[im].Value;
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

                                        string SQL1 = "select count(distinct rt.roll_no) as Attended, c.criteria,rt.degree_code,s.subject_code,s.subject_name from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c,subject s where marks_obtained>=0 and r.roll_no=rt.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria_no = '" + subra + "'  and s.CommonSub=1 and rt.Roll_No not in(select distinct r1.Roll_No from result r1, exam_type ex1 where  r1.marks_obtained<0  and r1.roll_no=su.roll_no and su.subject_no=ex1.subject_no and  r1.exam_code=ex1.exam_code and ex1.subject_no=s.subject_no and r1.roll_no=rt.roll_no and ex1.criteria_no = ex.criteria_no and s.CommonSub=1) and s.subject_code in ('" + buildvalue1 + "') group by rt.degree_code,s.subject_code,s.subject_name,c.criteria select count(distinct r.roll_no) as 'pass',c.criteria,rt.degree_code,s.subject_code,s.subject_name from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and c.criteria_no = '" + subra + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "'  and s.CommonSub=1 and rt.Roll_No not in ( select distinct rt.roll_no from  result r,exam_type ex,subjectchooser su, registration rt,criteriaforinternal c where  s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and   rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and   rt.Current_Semester=su.semester and  su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag  <> 'DEBAR' and rt.delflag=0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria_no = '" + subra + "' and s.CommonSub=1)and rt.Roll_No not in (select distinct rt.roll_no from result r,registration rt,exam_type ex,subjectchooser su , criteriaforinternal c where s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and  r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and  rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria_no = '" + subra + "'  and s.CommonSub=1 ) and s.subject_code in ('" + buildvalue1 + "') group by rt.degree_code,s.subject_code,s.subject_name,c.criteria select count(distinct rt.roll_no) as fail,c.criteria, rt.degree_code,s.subject_code,s.subject_name from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c,subject s where  r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and r.marks_obtained<ex.min_mark  and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria_no = '" + subra + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1 and s.subject_code in ('" + buildvalue1 + "') group by rt.degree_code,s.subject_code,s.subject_name,c.criteria select count(distinct rt.roll_no) as absent,c.criteria, rt.degree_code,s.subject_code,s.subject_name from result r,registration rt,exam_type ex, subjectchooser su ,criteriaforinternal c,subject s where r.marks_obtained='-1' and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria_no = '" + subra + "' and rt.degree_code= '" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1 and s.subject_code in ('" + buildvalue1 + "') group by rt.degree_code,s.subject_code,s.subject_name,c.criteria";
                                        ds.Clear();
                                        ds = da.select_method_wo_parameter(SQL1, "Text");

                                        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0)
                                        {
                                            flagv = true;

                                            string strkan = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text; ;
                                            if (strraj != strkan)
                                            {
                                                strraj = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text;
                                                row = dt.NewRow();
                                                row[3] = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text;
                                                dt.Rows.Add(row);
                                                //al.Add(dview[0]["subject_code"]);
                                                addarray.Add(dt.Rows.Count);
                                            }

                                            ds.Tables[0].DefaultView.RowFilter = "criteria='" + buildval + "' ";
                                            DataView dview = ds.Tables[0].DefaultView;
                                            if (dview.Count > 0)
                                            {
                                                for (int sk = 0; sk < dview.Count; sk++)
                                                {
                                                    row = dt.NewRow();
                                                    row[0] = sno1;
                                                    sno1++;

                                                    row[1] = Convert.ToString(dview[sk]["subject_code"]);
                                                    row[2] = Convert.ToString(dview[sk]["subject_name"]);
                                                    row[3] = buildval;
                                                    row[4] = Convert.ToString(dview[sk]["Attended"]);

                                                    if (ds.Tables[1].Rows.Count > 0 && sk < ds.Tables[1].Rows.Count)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "criteria='" + buildval + "' ";
                                                        dvw1 = ds.Tables[1].DefaultView;
                                                        if (dvw1.Count > 0)
                                                        {
                                                            row[5] = Convert.ToString(dvw1[sk]["pass"]);
                                                            double appeared = Convert.ToDouble(dview[sk]["Attended"]);
                                                            double pass = Convert.ToDouble(dvw1[sk]["pass"]);
                                                            double percentage = pass / appeared * 100;
                                                            percentage = Math.Round(percentage, 2);
                                                            row[8] = Convert.ToString(percentage);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row[5] = 0;
                                                        row[8] = 0;
                                                    }

                                                    if (ds.Tables[2].Rows.Count > 0 && sk < ds.Tables[2].Rows.Count)
                                                    {
                                                        dvw1 = null;
                                                        ds.Tables[2].DefaultView.RowFilter = "criteria='" + buildval + "' ";
                                                        dvw1 = ds.Tables[2].DefaultView;
                                                        if (dvw1.Count > 0)
                                                        {
                                                            row[6] = Convert.ToString(dvw1[sk]["fail"]);
                                                        }
                                                        else
                                                        {
                                                            row[6] = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row[6] = 0;
                                                    }

                                                    if (ds.Tables[3].Rows.Count > 0 && sk < ds.Tables[3].Rows.Count)
                                                    {
                                                        dvw1 = null;
                                                        ds.Tables[3].DefaultView.RowFilter = "criteria='" + buildval + "' ";
                                                        dvw1 = ds.Tables[3].DefaultView;
                                                        if (dvw1.Count > 0)
                                                        {
                                                            row[7] = Convert.ToString(dvw1[sk]["absent"]);
                                                        }
                                                        else
                                                        {
                                                            row[7] = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row[7] = 0;
                                                    }

                                                    dt.Rows.Add(row);
                                                }
                                            }
                                            else
                                            {
                                                row = dt.NewRow();
                                                row[1] = Convert.ToString(ds.Tables[2].Rows[i]["subject_code"]);
                                                row[2] = Convert.ToString(ds.Tables[2].Rows[i]["subject_name"]);
                                                row[3] = buildval;

                                                row[4] = 0;
                                            }

                                        }
                                        else
                                        {
                                            lblmsg.Visible = true;
                                            lblmsg.Text = "No Records Found";
                                            Commongrid.Visible = false;
                                            GenaralGrid.Visible = false;
                                            Genaralchart.Visible = false;
                                            CommonClick.Visible = false;
                                            Label1.Visible = false;
                                            Generalreportgrid.Visible = false;
                                            Fpspread.Visible = false;
                                            lblexportxl.Visible = false;
                                            txtexcelname.Visible = false;
                                            g1btnexcel.Visible = false;
                                            g1btnprint.Visible = false;
                                            Excel.Visible = false;
                                            Print.Visible = false;
                                            Label2.Visible = false;
                                        }

                                    }
                                }
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            Commongrid.DataSource = dt;
                            Commongrid.DataBind();

                            if (addarray.Count > 0)
                            {
                                for (int add = 0; add < addarray.Count; add++)
                                {
                                    int row_value = Convert.ToInt32(addarray[add]);
                                    row_value = row_value - 1;
                                    int chell_kutty = row_value + 1;
                                    string getvalue = Convert.ToString(Commongrid.Rows[row_value].Cells[3].Text);
                                    Commongrid.Rows[row_value].Cells[0].ColumnSpan = 9;
                                    Commongrid.Rows[row_value].Cells[1].Visible = false;
                                    Commongrid.Rows[row_value].Cells[2].Visible = false;
                                    Commongrid.Rows[row_value].Cells[3].Visible = false;
                                    Commongrid.Rows[row_value].Cells[4].Visible = false;
                                    Commongrid.Rows[row_value].Cells[5].Visible = false;
                                    Commongrid.Rows[row_value].Cells[6].Visible = false;
                                    Commongrid.Rows[row_value].Cells[7].Visible = false;
                                    Commongrid.Rows[row_value].Cells[8].Visible = false;
                                    Commongrid.Rows[row_value].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Commongrid.Rows[row_value].Cells[0].Text = Convert.ToString(getvalue);
                                    Commongrid.Rows[chell_kutty].Cells[0].Text = Convert.ToString(add + 1);
                                    Commongrid.Rows[chell_kutty].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Commongrid.Rows[row_value].Cells[0].ForeColor = System.Drawing.Color.Brown;
                                    Commongrid.Rows[row_value].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                                }
                            }
                        }

                        for (int j = 0; j < Commongrid.Rows.Count; j++)
                        {
                            Commongrid.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Commongrid.Rows[j].Cells[3].HorizontalAlign = HorizontalAlign.Left;
                            Commongrid.Rows[j].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            Commongrid.Rows[j].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                            Commongrid.Rows[j].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                            Commongrid.Rows[j].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                            Commongrid.Rows[j].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                        }
                        if (flagv == true)
                        {
                            Commongrid.Visible = true;
                            CommonClick.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            BtnReport.Visible = false;
                            Excel.Visible = true;
                            Print.Visible = true;
                            Label1.Visible = false;
                            Label3.Visible = false;
                            lblmsg.Visible = false;
                            Printcontrol.Visible = false;
                            Label2.Visible = false;
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Records Found";
                            Commongrid.Visible = false;
                            GenaralGrid.Visible = false;
                            Genaralchart.Visible = false;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label3.Visible = false;
                            Label2.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Atleast One Test";
                        Commongrid.Visible = false;
                        GenaralGrid.Visible = false;
                        Genaralchart.Visible = false;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Generalreportgrid.Visible = false;
                        Fpspread.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcelname.Visible = false;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        Excel.Visible = false;
                        Print.Visible = false;
                        Label3.Visible = false;
                        Label2.Visible = false;
                    }
                }
                //else
                //{
                //    lblmsg.Visible = true;
                //    lblmsg.Text = "Please Select Atleast One Subject";
                //    Commongrid.Visible = false;
                //    GenaralGrid.Visible = false;
                //    Genaralchart.Visible = false;
                //    CommonClick.Visible = false;
                //    Label1.Visible = false;
                //    Generalreportgrid.Visible = false;
                //    Excel.Visible = false;
                //    Print.Visible = false;
                //}

            }

            else if (ddlsubtype.SelectedItem.Text == "General")
            {
                string build = "";
                string tstname = "";
                string tstname1 = "";

                DataSet dc = new DataSet();
                ArrayList add = new ArrayList();
                Generalreportgrid.Visible = false;
                Fpspread.Visible = false;
                lblexportxl.Visible = false;
                txtexcelname.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
                int sno = 1;
                dt.Columns.Add("S.No", typeof(string));
                dt.Columns.Add("Subcode", typeof(string));
                dt.Columns.Add("SubName", typeof(string));

                if (Rbtn.Items[0].Selected == true)
                {
                    dt.Columns.Add("Test Name", typeof(string));
                    dt.Columns.Add("Appeared", typeof(string));
                    dt.Columns.Add("Passed", typeof(string));
                    dt.Columns.Add("Failed", typeof(string));
                    dt.Columns.Add("Absentees", typeof(string));
                    dt.Columns.Add("Pass%", typeof(string));

                    if (cbltest.Items.Count > 0)
                    {
                        //DataRow dr112 = null;
                        //dr112 = dt.NewRow();
                        //dr112[0] = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text;
                        //dt.Rows.Add(dr112);
                        //add.Add(dt.Rows.Count);

                        for (int i1 = 0; i1 < cbltest.Items.Count; i1++)
                        {
                            if (cbltest.Items[i1].Selected == true)
                            {
                                build = cbltest.Items[i1].Value.ToString();

                                // string mn = "select distinct s.subject_code,c.criteria from CriteriaForInternal  c,subject s,exam_type ex where s.syll_code=c.syll_code and ex.criteria_no=c.Criteria_no and ex.subject_no=s.subject_no and c.criteria='" + build + "'  ";
                                string mn = "select distinct s.subject_code,c.criteria from CriteriaForInternal  c,subject s,exam_type ex,syllabus_master sy  where s.syll_code=c.syll_code and ex.criteria_no=c.Criteria_no and ex.subject_no=s.subject_no  and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and sy.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria='" + build + "'";
                                dc = da.select_method_wo_parameter(mn, "text");
                                if (dc.Tables[0].Rows.Count > 0)
                                {
                                    // ------------- rajesh -- adding sub-heading
                                    tstname1 = cbltest.Items[i1].Text;
                                    for (int ik = 0; ik < cbltest.Items.Count; ik++)
                                    {
                                        tstname = cbltest.Items[ik].Text;
                                        dc.Tables[0].DefaultView.RowFilter = "criteria='" + tstname + "'";
                                        DataView dview1 = dc.Tables[0].DefaultView;
                                        if (dview1.Count > 0)
                                        {
                                            DataRow dr11 = null;
                                            dr11 = dt.NewRow();
                                            dr11[0] = tstname;
                                            dt.Rows.Add(dr11);
                                            add.Add(dt.Rows.Count);
                                        }
                                    }
                                    // ------------- rajehs -- adding sub-heading
                                    //string SQL1 = "select count(distinct rt.roll_no) as Attended, rt.degree_code,s.subject_code,s.subject_name from result r, registration rt,subjectchooser su,exam_type ex, criteriaforinternal c,subject s where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + build + "' and rt.Roll_No not in(select distinct rt.Roll_No from result r, registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and  r.marks_obtained<>'-3') and marks_obtained<>'-1' and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and rt.RollNo_Flag<>0 and c.criteria = '" + build + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "') group by rt.degree_code,s.subject_code,s.subject_name order by s.subject_name    select count(distinct r.roll_no) as 'pass',rt.degree_code,s.subject_code,s.subject_name from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and c.criteria = '" + build + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' group by rt.degree_code,s.subject_code,s.subject_name order by s.subject_name select count(distinct rt.roll_no) as fail, rt.degree_code,s.subject_code,s.subject_name from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c,subject s where  r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or  r.marks_obtained='-3' or r.marks_obtained='-2') and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria ='" + build + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "'  group by rt.degree_code,s.subject_code,s.subject_name order by s.subject_name select count(distinct rt.roll_no) as absent, rt.degree_code,s.subject_code,s.subject_name from result r,registration rt,exam_type ex, subjectchooser su ,criteriaforinternal c,subject s where r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' )and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria = '" + build + "' and rt.degree_code= '" + ddldept.SelectedValue.ToString() + "'  group by rt.degree_code,s.subject_code,s.subject_name  order by s.subject_name ";

                                    //string SQL1 = "select COUNT(r.roll_no) as Attended,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as pass,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and (r.marks_obtained>=e.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1' and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as fail,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and (r.marks_obtained<e.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1' and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as absent,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name";

                                    string SQL1 = "select COUNT(r.roll_no) as Attended,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and r.marks_obtained>=0 and s.subject_no = c.subject_no and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as pass,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and (r.marks_obtained>=e.min_mark or r.marks_obtained='-3') and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as fail,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and r.marks_obtained<e.min_mark and r.marks_obtained<>'-3' and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name select COUNT(r.roll_no) as absent,s.subject_code,s.subject_name from Result r,Exam_type e,CriteriaForInternal i,subject s,Registration g,subjectchooser C where r.exam_code = e.exam_code and e.criteria_no = i.Criteria_no and e.subject_no = s.subject_no and r.roll_no = g.Roll_No and c.roll_no = r.roll_no and r.roll_no = c.roll_no and e.subject_no = c.subject_no and s.subject_no = c.subject_no and r.marks_obtained='-1' and criteria = '" + build + "' AND G.degree_code='" + ddldept.SelectedValue.ToString() + "' and g.CC = 0 and g.Exam_Flag <> 'DEBAR' group by subject_code,subject_name order by s.subject_name"; ds.Clear();
                                    ds = da.select_method_wo_parameter(SQL1, "Text");
                                    string s = "";
                                    for (int kl = 0; kl < dc.Tables[0].Rows.Count; kl++)
                                    {
                                        if (Rbtn.Items[0].Selected == true)
                                        {
                                            if (dc.Tables[0].Rows.Count > 0)
                                            {
                                                //if (ds.Tables[0].Rows.Count > 0)
                                                //{
                                                ds.Tables[0].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                DataView dvapperar = ds.Tables[0].DefaultView;
                                                if (dvapperar.Count > 0)
                                                {
                                                    flagvar = true;

                                                    int y = 0;
                                                    if (dvapperar.Count > 0)
                                                    {
                                                        dv = ds.Tables[0].DefaultView;
                                                        row = dt.NewRow();
                                                        row[0] = sno;
                                                        row[1] = Convert.ToString(dvapperar[0]["subject_code"]);
                                                        row[2] = Convert.ToString(dvapperar[0]["subject_name"]);
                                                        row[3] = tstname1;
                                                        row[4] = Convert.ToString(dvapperar[0]["Attended"]);

                                                        ds.Tables[1].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                        DataView dvpass = ds.Tables[1].DefaultView;

                                                        if (dvpass.Count > 0)
                                                        {
                                                            row[5] = Convert.ToString(dvpass[0]["pass"]);
                                                        }
                                                        else
                                                        {
                                                            row[5] = 0;
                                                        }

                                                        ds.Tables[2].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                        DataView dvfail = ds.Tables[2].DefaultView;
                                                        if (dvfail.Count > 0)
                                                        {
                                                            row[6] = Convert.ToString(dvfail[0]["fail"]);
                                                        }

                                                        else
                                                        {
                                                            row[6] = 0;
                                                        }

                                                        ds.Tables[3].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                        DataView dvabsebt = ds.Tables[3].DefaultView;
                                                        if (dvabsebt.Count > 0)
                                                        {
                                                            row[7] = Convert.ToString(dvabsebt[0]["absent"]);

                                                        }
                                                        else
                                                        {
                                                            row[7] = 0;
                                                        }

                                                        double pass = 0;
                                                        ds.Tables[0].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                        DataView dvattend = ds.Tables[0].DefaultView;
                                                        double appeared = Convert.ToDouble(dvattend[0]["Attended"]);

                                                        ds.Tables[1].DefaultView.RowFilter = "subject_code='" + dc.Tables[0].Rows[kl]["subject_code"].ToString() + "'";
                                                        DataView dvpassd = ds.Tables[1].DefaultView;
                                                        if (dvpassd.Count > 0)
                                                        {

                                                            pass = Convert.ToDouble(dvpassd[0]["pass"]);

                                                        }
                                                        else
                                                        {
                                                            pass = 0;
                                                            y--;
                                                        }
                                                        double percentage = pass / appeared * 100;
                                                        percentage = Math.Round(percentage, 2);
                                                        row[8] = Convert.ToString(percentage);

                                                        dt.Rows.Add(row);
                                                        GenaralGrid.DataSource = dt;
                                                        GenaralGrid.DataBind();
                                                        sno++;

                                                        //------------- rajehs -- sub-heading spanning start
                                                        if (add.Count > 0)
                                                        {
                                                            for (int a = 0; a < add.Count; a++)
                                                            {
                                                                string roww = Convert.ToString(add[a]);
                                                                int rowat = 0;
                                                                rowat = Convert.ToInt32(roww) - 1;
                                                                GenaralGrid.Rows[rowat].Cells[1].ColumnSpan = 10;
                                                                GenaralGrid.Rows[rowat].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                                                GenaralGrid.Rows[rowat].Cells[1].ForeColor = System.Drawing.Color.Black;
                                                                GenaralGrid.Rows[rowat].Cells[1].BackColor = System.Drawing.Color.Gainsboro;
                                                                GenaralGrid.Rows[rowat].Cells[0].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[2].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[3].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[4].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[5].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[6].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[7].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[8].Visible = false;
                                                                GenaralGrid.Rows[rowat].Cells[9].Visible = false;
                                                            }
                                                        }
                                                        //------------- rajesh -- sub-heading spanning end

                                                        GenaralGrid.Visible = true;
                                                        Genaralchart.DataSource = dt;
                                                        Genaralchart.DataBind();
                                                        Genaralchart.Visible = true;
                                                        Genaralchart.ChartAreas[0].AxisX.RoundAxisValues();
                                                        Genaralchart.ChartAreas[0].AxisX.Minimum = 0;
                                                        Genaralchart.ChartAreas[0].AxisX.Interval = 1;
                                                        Genaralchart.ChartAreas[0].AxisY.Maximum = 100;
                                                        Genaralchart.Series["Series1"].IsValueShownAsLabel = true;
                                                        Genaralchart.Series[0].ChartType = SeriesChartType.Column;
                                                        Commongrid.Visible = false;
                                                        CommonClick.Visible = false;
                                                        BtnReport.Visible = true;
                                                        Excel.Visible = false;
                                                        Print.Visible = false;
                                                        Label1.Visible = false;
                                                        lblmsg.Visible = false;
                                                        Printcontrol.Visible = false;
                                                        Label2.Visible = false;

                                                        for (int j = 0; j < GenaralGrid.Rows.Count; j++)
                                                        {
                                                            GenaralGrid.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                                                            GenaralGrid.Rows[j].Cells[9].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        y++;
                                                    }
                                                }
                                                else
                                                {
                                                    lblmsg.Visible = true;
                                                    lblmsg.Text = "No Records Found";
                                                    Commongrid.Visible = false;
                                                    GenaralGrid.Visible = false;
                                                    Genaralchart.Visible = false;
                                                    CommonClick.Visible = false;
                                                    Generalreportgrid.Visible = false;
                                                    Fpspread.Visible = false;
                                                    lblexportxl.Visible = false;
                                                    txtexcelname.Visible = false;
                                                    g1btnexcel.Visible = false;
                                                    g1btnprint.Visible = false;
                                                    Excel.Visible = false;
                                                    Print.Visible = false;
                                                    Label1.Visible = false;
                                                    BtnReport.Visible = false;
                                                    Label2.Visible = false;
                                                }
                                                //}
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lblmsg.Visible = true;
                                    lblmsg.Text = "No Records Found";
                                    Commongrid.Visible = false;
                                    GenaralGrid.Visible = false;
                                    Genaralchart.Visible = false;
                                    CommonClick.Visible = false;
                                    Generalreportgrid.Visible = false;
                                    Fpspread.Visible = false;
                                    lblexportxl.Visible = false;
                                    txtexcelname.Visible = false;
                                    g1btnexcel.Visible = false;
                                    g1btnprint.Visible = false;
                                    Excel.Visible = false;
                                    Print.Visible = false;
                                    Label1.Visible = false;
                                    BtnReport.Visible = false;
                                    Label2.Visible = false;
                                }
                            }
                            else
                            {
                                lblmsg.Visible = true;
                                lblmsg.Text = "Please Select Test";
                                Commongrid.Visible = false;
                                GenaralGrid.Visible = false;
                                Genaralchart.Visible = false;
                                CommonClick.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Excel.Visible = false;
                                Print.Visible = false;
                                Label1.Visible = false;
                                BtnReport.Visible = false;
                                Label2.Visible = false;
                            }
                            sno = 1;
                        }
                        if (GenaralGrid.Rows.Count > 0)
                        {
                            GenaralGrid.Visible = true;
                            Genaralchart.Visible = true;
                            Commongrid.Visible = false;
                            CommonClick.Visible = false;
                            BtnReport.Visible = true;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label1.Visible = false;
                            lblmsg.Visible = false;
                            Printcontrol.Visible = false;
                            Label2.Visible = false;
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Records Found";
                            Commongrid.Visible = false;
                            GenaralGrid.Visible = false;
                            Genaralchart.Visible = false;
                            CommonClick.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label1.Visible = false;
                            BtnReport.Visible = false;
                            Label2.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Test";
                        Commongrid.Visible = false;
                        GenaralGrid.Visible = false;
                        Genaralchart.Visible = false;
                        CommonClick.Visible = false;
                        Generalreportgrid.Visible = false;
                        Fpspread.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcelname.Visible = false;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        Excel.Visible = false;
                        Print.Visible = false;
                        Label1.Visible = false;
                        BtnReport.Visible = false;
                        Label2.Visible = false;
                    }
                }

                //// ------------- rajehs
                //if (add.Count > 0)
                //{
                //    for (int a = 0; a < add.Count; a++)
                //    {
                //        string roww = Convert.ToString(add[a]);
                //        int rowat = 0;
                //        rowat = Convert.ToInt32(roww) - 1;
                //        GenaralGrid.Rows[rowat].Cells[2].ColumnSpan = 9;
                //        GenaralGrid.Rows[rowat].Cells[2].HorizontalAlign = HorizontalAlign.Center;
                //        GenaralGrid.Rows[rowat].Cells[2].ForeColor = System.Drawing.Color.Black;
                //        GenaralGrid.Rows[rowat].Cells[2].BackColor = System.Drawing.Color.Gainsboro;
                //        GenaralGrid.Rows[rowat].Cells[1].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[2].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[3].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[4].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[5].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[6].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[7].Visible = false;
                //        GenaralGrid.Rows[rowat].Cells[8].Visible = false;
                //    }
                //}
                //else
                //{
                //    lblmsg.Visible = true;
                //    lblmsg.Text = "No Records Found";
                //    Commongrid.Visible = false;
                //    GenaralGrid.Visible = false;
                //    Genaralchart.Visible = false;
                //    CommonClick.Visible = false;
                //    Generalreportgrid.Visible = false;
                //    Excel.Visible = false;
                //    Print.Visible = false;
                //    Label1.Visible = false;
                //}
                //// ------------- rajesh
            }
            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                //if (Rbtn.Items[1].Selected == true)
                //{
                //    //dt.Columns.Add("Appeared", typeof(string));
                //    //dt.Columns.Add("Passed", typeof(string));
                //    //dt.Columns.Add("Failed", typeof(string));
                //    //dt.Columns.Add("Absentees", typeof(string));
                //    //dt.Columns.Add("Pass%", typeof(string));

                //    if (cblsubject.Items.Count > 0)
                //    {
                //        int k = 0;
                //        int kfail = 0;
                //        int kpas = 0;
                //        int katt = 0;
                //        int snoo = 1;

                //        if (ddlexm.SelectedItem.Text != "Select" && ddlyear.SelectedItem.Text != "Select")
                //        {
                //            string SQLQuery = "select COUNT(distinct roll_no) as Attended,s.subject_code,s.subject_name,sb.subject_type  from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,sub_sem sb  where  sb.subType_no=s.subType_no and sb.syll_code=s.syll_code and s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and d.Degree_Code=y.degree_code  and  e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and result  !='AAA'    and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,sb.subject_type,lab order by lab,s.subject_code   select COUNT(distinct roll_no) as pass,s.subject_code,s.subject_name  from subject s,syllabus_master y,mark_entry m, Degree d,Exam_Details e,sub_sem sb    where  sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no    and d.Degree_Code=y.degree_code and   e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester  and result  in  ('Pass') and passorfail  in(1)  and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "           group by s.subject_code,s.subject_name,lab order by lab,s.subject_code     select COUNT(distinct roll_no) as fail,s.subject_code,s.subject_name   from subject s,syllabus_master y,   mark_entry m, Degree d ,Exam_Details e,sub_sem sb  where sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and    d.Degree_Code=y.degree_code and    e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and    result  in('fail','AAA','WHD') and passorfail  in(0)       and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,lab order by lab,s.subject_code    select COUNT(distinct roll_no) as absent,s.subject_code,s.subject_name  from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,sub_sem sb where sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and d.Degree_Code=y.degree_code  and  e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and result  ='AAA'    and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,lab order by lab,s.subject_code  ";
                //            DataSet dset = da.select_method_wo_parameter(SQLQuery, "Text");

                //            if (dset.Tables[0].Rows.Count > 0)
                //            {
                //                dt.Columns.Add("Appeared", typeof(string));
                //                dt.Columns.Add("Passed", typeof(string));
                //                dt.Columns.Add("Failed", typeof(string));
                //                dt.Columns.Add("Absentees", typeof(string));
                //                dt.Columns.Add("Pass%", typeof(string));
                //                flagvar = true;

                //                int y = 0;
                //                for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                //                {
                //                    DataView dview = dset.Tables[0].DefaultView;
                //                    row = dt.NewRow();
                //                    row[0] = snoo;
                //                    snoo++;
                //                    row[1] = Convert.ToString(dset.Tables[0].Rows[i]["subject_code"]);
                //                    row[2] = Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]);
                //                    row[3] = Convert.ToString(dset.Tables[0].Rows[i]["Attended"]);

                //                    if (dset.Tables[0].Rows.Count > 0)
                //                    {
                //                        if (dset.Tables[1].Rows.Count > i)
                //                        {
                //                            if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[1].Rows[kpas]["subject_name"]))
                //                            {
                //                                row[4] = Convert.ToString(dset.Tables[1].Rows[kpas]["pass"]);
                //                                kpas++;
                //                            }
                //                            else
                //                            {
                //                                row[4] = 0;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dset.Tables[1].Rows.Count > kpas)
                //                            {
                //                                row[4] = Convert.ToString(dset.Tables[1].Rows[kpas]["pass"]);
                //                                kpas++;
                //                            }
                //                            else
                //                            {
                //                                row[4] = 0;
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        row[4] = 0;
                //                    }

                //                    if (dset.Tables[2].Rows.Count > 0)
                //                    {
                //                        if (dset.Tables[2].Rows.Count > i)
                //                        {
                //                            if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[2].Rows[kfail]["subject_name"]))
                //                            {
                //                                row[5] = Convert.ToString(dset.Tables[2].Rows[kfail]["fail"]);
                //                                kfail++;
                //                            }
                //                            else
                //                            {
                //                                row[5] = 0;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dset.Tables[2].Rows.Count > kfail)
                //                            {
                //                                row[5] = Convert.ToString(dset.Tables[2].Rows[kfail]["fail"]);
                //                                kfail++;
                //                            }
                //                            else
                //                            {
                //                                row[5] = 0;
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        row[5] = 0;
                //                    }

                //                    if (dset.Tables[3].Rows.Count > 0)
                //                    {
                //                        if (dset.Tables[3].Rows.Count > i)
                //                        {
                //                            if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[3].Rows[k]["subject_name"]))
                //                            {
                //                                row[6] = Convert.ToString(dset.Tables[3].Rows[k]["absent"]);
                //                                k++;
                //                            }
                //                            else
                //                            {
                //                                row[6] = 0;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dset.Tables[3].Rows.Count > k)
                //                            {
                //                                row[6] = Convert.ToString(dset.Tables[3].Rows[k]["absent"]);
                //                                k++;
                //                            }
                //                            else
                //                            {
                //                                row[6] = 0;
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        row[6] = 0;
                //                    }

                //                    double pass = 0;
                //                    double appeared = Convert.ToDouble(dset.Tables[0].Rows[i]["Attended"]);

                //                    if (dset.Tables[1].Rows.Count > 0)
                //                    {
                //                        if (dset.Tables[1].Rows.Count > i)
                //                        {

                //                            if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[1].Rows[katt]["subject_name"]))
                //                            {
                //                                pass = Convert.ToDouble(dset.Tables[1].Rows[katt]["pass"]);
                //                                katt++;
                //                            }
                //                            else
                //                            {
                //                                pass = 0;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            if (dset.Tables[1].Rows.Count > katt)
                //                            {
                //                                pass = Convert.ToDouble(dset.Tables[1].Rows[katt]["pass"]);
                //                                katt++;
                //                            }
                //                            else
                //                            {
                //                                pass = 0;
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        pass = 0;
                //                    }

                //                    double passpercentage = 0;
                //                    passpercentage = (((Convert.ToDouble(pass)) / (Convert.ToDouble(appeared)) * 100));
                //                    double percentage = Math.Round(passpercentage, 2);
                //                    row[7] = Convert.ToString(percentage);

                //                    dt.Rows.Add(row);
                //                    GenaralGrid.DataSource = dt;
                //                    GenaralGrid.DataBind();
                //                    Genaralchart.DataSource = dt;
                //                    Genaralchart.DataBind();
                //                    Genaralchart.ChartAreas[0].AxisX.RoundAxisValues();
                //                    Genaralchart.ChartAreas[0].AxisX.Minimum = 0;
                //                    Genaralchart.ChartAreas[0].AxisX.Interval = 1;
                //                    Genaralchart.ChartAreas[0].AxisY.Maximum = 100;
                //                    Genaralchart.Series["Series1"].IsValueShownAsLabel = true;
                //                    Genaralchart.Series[0].ChartType = SeriesChartType.Column;
                //                    Commongrid.Visible = false;
                //                    CommonClick.Visible = false;
                //                    Excel.Visible = false;
                //                    Print.Visible = false;

                //                    for (int j = 0; j < GenaralGrid.Rows.Count; j++)
                //                    {
                //                        GenaralGrid.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                //                        GenaralGrid.Rows[j].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                //                    }
                //                    y++;
                //                }

                //            }//
                //            else
                //            {
                //                lblmsg.Visible = true;
                //                lblmsg.Text = "No Records Found";
                //                Commongrid.Visible = false;
                //                GenaralGrid.Visible = false;
                //                Genaralchart.Visible = false;
                //                CommonClick.Visible = false;
                //                Generalreportgrid.Visible = false;
                //                Fpspread.Visible = false;
                //                lblexportxl.Visible = false;
                //                txtexcelname.Visible = false;
                //                g1btnexcel.Visible = false;
                //                g1btnprint.Visible = false;
                //                Excel.Visible = false;
                //                Print.Visible = false;
                //                Label1.Visible = false;
                //            }
                //            if (flagvar == true)
                //            {
                //                lblmsg.Visible = false;
                //                Commongrid.Visible = false;
                //                GenaralGrid.Visible = true;
                //                Genaralchart.Visible = true;
                //                CommonClick.Visible = false;
                //                Generalreportgrid.Visible = false;
                //                Fpspread.Visible = false;
                //                lblexportxl.Visible = false;
                //                txtexcelname.Visible = false;
                //                g1btnexcel.Visible = false;
                //                g1btnprint.Visible = false;
                //                Excel.Visible = false;
                //                Print.Visible = false;
                //                Label1.Visible = false;
                //                BtnReport.Visible = true;
                //                lblmsg.Visible = false;
                //            }
                //            else
                //            {
                //                lblmsg.Visible = true;
                //                lblmsg.Text = "No Records Found";
                //                Commongrid.Visible = false;
                //                GenaralGrid.Visible = false;
                //                Genaralchart.Visible = false;
                //                CommonClick.Visible = false;
                //                Generalreportgrid.Visible = false;
                //                Fpspread.Visible = false;
                //                lblexportxl.Visible = false;
                //                txtexcelname.Visible = false;
                //                g1btnexcel.Visible = false;
                //                g1btnprint.Visible = false;
                //                Excel.Visible = false;
                //                Print.Visible = false;
                //                Label1.Visible = false;
                //            }
                //        }
                //        else
                //        {
                //            lblmsg.Visible = true;
                //            lblmsg.Text = "Please Select Month and Year";
                //            Commongrid.Visible = false;
                //            GenaralGrid.Visible = false;
                //            Genaralchart.Visible = false;
                //            CommonClick.Visible = false;
                //            Generalreportgrid.Visible = false;
                //            Fpspread.Visible = false;
                //            lblexportxl.Visible = false;
                //            txtexcelname.Visible = false;
                //            g1btnexcel.Visible = false;
                //            g1btnprint.Visible = false;
                //            Excel.Visible = false;
                //            Print.Visible = false;
                //            Label1.Visible = false;
                //        }
                //    }
                //    else
                //    {
                //        lblmsg.Visible = true;
                //        lblmsg.Text = "Please Select Subject";
                //        Commongrid.Visible = false;
                //        GenaralGrid.Visible = false;
                //        Genaralchart.Visible = false;
                //        CommonClick.Visible = false;
                //        Label1.Visible = false;
                //        Generalreportgrid.Visible = false;
                //        Fpspread.Visible = false;
                //        lblexportxl.Visible = false;
                //        txtexcelname.Visible = false;
                //        g1btnexcel.Visible = false;
                //        g1btnprint.Visible = false;
                //        Excel.Visible = false;
                //        Print.Visible = false;
                //    }
                //}
            }
            else if (ddlsubtype.SelectedItem.Text == "General")
            {
                if (Rbtn.Items[1].Selected == true)
                {
                    //dt.Columns.Add("Appeared", typeof(string));
                    //dt.Columns.Add("Passed", typeof(string));
                    //dt.Columns.Add("Failed", typeof(string));
                    //dt.Columns.Add("Absentees", typeof(string));
                    //dt.Columns.Add("Pass%", typeof(string));

                    //if (cblsubject.Items.Count > 0)
                    //{
                    int k = 0;
                    int kfail = 0;
                    int kpas = 0;
                    int katt = 0;
                    int snoo = 1;

                    if (ddlexm.SelectedItem.Text != "Select" && ddlyear.SelectedItem.Text != "Select")
                    {
                        string SQLQuery = "select COUNT(distinct roll_no) as Attended,s.subject_code,s.subject_name,sb.subject_type  from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,sub_sem sb  where  sb.subType_no=s.subType_no and sb.syll_code=s.syll_code and s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and d.Degree_Code=y.degree_code  and  e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and result  !='AAA'    and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,sb.subject_type,lab order by lab,s.subject_code   select COUNT(distinct roll_no) as pass,s.subject_code,s.subject_name  from subject s,syllabus_master y,mark_entry m, Degree d,Exam_Details e,sub_sem sb    where  sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no    and d.Degree_Code=y.degree_code and   e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester  and result  in  ('Pass') and passorfail  in(1)  and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "           group by s.subject_code,s.subject_name,lab order by lab,s.subject_code     select COUNT(distinct roll_no) as fail,s.subject_code,s.subject_name   from subject s,syllabus_master y,   mark_entry m, Degree d ,Exam_Details e,sub_sem sb  where sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and    d.Degree_Code=y.degree_code and    e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and    result  in('fail','AAA','WHD','W') and passorfail  in(0)       and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,lab order by lab,s.subject_code    select COUNT(distinct roll_no) as absent,s.subject_code,s.subject_name  from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,sub_sem sb where sb.subType_no=s.subType_no and sb.syll_code=s.syll_code AND s.syll_code = y.syll_code and s.subject_no  = m.subject_no   and d.Degree_Code=y.degree_code  and  e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code  and y.semester=e.current_semester    and result   in ('AAA','WHD','W')   and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and e.degree_code=" + ddldept.SelectedValue + "    group by s.subject_code,s.subject_name,lab order by lab,s.subject_code  ";
                        DataSet dset = da.select_method_wo_parameter(SQLQuery, "Text");

                        if (dset.Tables[0].Rows.Count > 0)
                        {
                            dt.Columns.Add("Appeared", typeof(string));
                            dt.Columns.Add("Passed", typeof(string));
                            dt.Columns.Add("Failed", typeof(string));
                            dt.Columns.Add("Absentees", typeof(string));
                            dt.Columns.Add("Pass%", typeof(string));
                            flagvar = true;

                            for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                            {
                                //}
                                dset.Tables[0].DefaultView.RowFilter = "subject_code='" + dset.Tables[0].Rows[i]["subject_code"].ToString() + "' ";
                                DataView dview = dset.Tables[0].DefaultView;
                                if (dview.Count > 0)
                                {
                                    //row = dt.NewRow();
                                    //row[3] = Convert.ToString(dview[0]["subject_code"]) + "-" + Convert.ToString(dview[0]["subject_name"]);
                                    //dt.Rows.Add(row);
                                    //al.Add(dview[0]["subject_code"]);
                                    //addarray.Add(dt.Rows.Count);

                                    row = dt.NewRow();
                                    row[0] = snoo;
                                    snoo++;
                                    row[1] = Convert.ToString(dview[0]["subject_code"]);
                                    row[2] = Convert.ToString(dview[0]["subject_name"]);
                                    row[3] = Convert.ToString(dview[0]["Attended"]);
                                    //row[4] = Convert.ToString(dview[0]["appeared"]);

                                    if (dset.Tables[1].Rows.Count > 0)
                                    {
                                        dset.Tables[1].DefaultView.RowFilter = "subject_code='" + dset.Tables[0].Rows[i]["subject_code"].ToString() + "' ";
                                        dvw1 = dset.Tables[1].DefaultView;
                                        if (dvw1.Count > 0)
                                        {
                                            row[4] = Convert.ToString(dvw1[0]["pass"]);
                                            double appeared = Convert.ToDouble(dview[0]["Attended"]);
                                            double pass = Convert.ToDouble(dvw1[0]["pass"]);
                                            double percentage = pass / appeared * 100;
                                            percentage = Math.Round(percentage, 2);
                                            row[7] = Convert.ToString(percentage);
                                        }
                                        else
                                        {
                                            row[4] = 0;
                                            row[7] = 0;
                                        }
                                    }
                                    else
                                    {
                                        row[4] = 0;
                                        row[7] = 0;
                                    }

                                    if (dset.Tables[2].Rows.Count > 0)
                                    {
                                        dvw1 = null;
                                        dset.Tables[2].DefaultView.RowFilter = "subject_code='" + dset.Tables[0].Rows[i]["subject_code"].ToString() + "' ";
                                        dvw1 = dset.Tables[2].DefaultView;
                                        if (dvw1.Count > 0)
                                        {
                                            row[5] = Convert.ToString(dvw1[0]["fail"]);
                                        }
                                        else
                                        {
                                            row[5] = 0;
                                        }
                                    }
                                    else
                                    {
                                        row[5] = 0;
                                    }

                                    if (dset.Tables[3].Rows.Count > 0)
                                    {
                                        dvw1 = null;
                                        dset.Tables[3].DefaultView.RowFilter = "subject_code='" + dset.Tables[0].Rows[i]["subject_code"].ToString() + "' ";
                                        dvw1 = dset.Tables[3].DefaultView;
                                        if (dvw1.Count > 0)
                                        {
                                            row[6] = Convert.ToString(dvw1[0]["absent"]);
                                        }
                                        else
                                        {
                                            row[6] = 0;
                                        }
                                    }

                                    else
                                    {
                                        row[6] = 0;
                                    }
                                    dt.Rows.Add(row);
                                }
                            }

                            //int y = 0;
                            //for (int i = 0; i < dset.Tables[0].Rows.Count; i++)
                            //{
                            //    DataView dview = dset.Tables[0].DefaultView;
                            //    row = dt.NewRow();
                            //    row[0] = snoo;
                            //    snoo++;
                            //    row[1] = Convert.ToString(dset.Tables[0].Rows[i]["subject_code"]);
                            //    row[2] = Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]);
                            //    row[3] = Convert.ToString(dset.Tables[0].Rows[i]["Attended"]);

                            //    if (dset.Tables[0].Rows.Count > 0)
                            //    {
                            //        dset.Tables[1].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i].Value + "' ";
                            //        DataView dv1 = dset.Tables[1].DefaultView;

                            //        if (dv1.Count > 0)
                            //        {
                            //            //if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[1].Rows[kpas]["subject_name"]))
                            //            //{
                            //                row[4] = Convert.ToString(dv1[0]["pass"]);
                            //                kpas++;
                            //            //}
                            //            //else
                            //            //{
                            //            //    row[4] = 0;
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            //if (dset.Tables[1].Rows.Count > kpas)
                            //            //{
                            //            //    row[4] = Convert.ToString(dset.Tables[1].Rows[kpas]["pass"]);
                            //            //    kpas++;
                            //            //}
                            //            //else
                            //            //{
                            //                row[4] = 0;
                            //            //}
                            //        }
                            //    }
                            //    else
                            //    {
                            //        row[4] = 0;
                            //    }

                            //    if (dset.Tables[2].Rows.Count > 0)
                            //    {

                            //        dset.Tables[1].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i].Value + "' ";
                            //        DataView dv2 = dset.Tables[1].DefaultView;

                            //        if (dv2.Count > 0)
                            //        {
                            //            //if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[2].Rows[kfail]["subject_name"]))
                            //            //{
                            //                row[5] = Convert.ToString(dv2[0]["fail"]);
                            //                kfail++;
                            //            //}
                            //            //else
                            //            //{
                            //            //    row[5] = 0;
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            //if (dset.Tables[2].Rows.Count > kfail)
                            //            //{
                            //            //    row[5] = Convert.ToString(dset.Tables[2].Rows[kfail]["fail"]);
                            //            //    kfail++;
                            //            //}
                            //            //else
                            //            //{
                            //                row[5] = 0;
                            //            //}
                            //        }
                            //    }
                            //    else
                            //    {
                            //        row[5] = 0;
                            //    }

                            //    if (dset.Tables[3].Rows.Count > 0)
                            //    {
                            //        dset.Tables[1].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i].Value + "' ";
                            //        DataView dv3 = dset.Tables[1].DefaultView;

                            //        if (dv3.Count > 0)
                            //        {
                            //            //if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[3].Rows[k]["subject_name"]))
                            //            //{
                            //                row[6] = Convert.ToString(dv3[0]["absent"]);
                            //                k++;
                            //            //}
                            //            //else
                            //            //{
                            //            //    row[6] = 0;
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            //if (dset.Tables[3].Rows.Count > k)
                            //            //{
                            //            //    row[6] = Convert.ToString(dset.Tables[3].Rows[k]["absent"]);
                            //            //    k++;
                            //            //}
                            //            //else
                            //            //{
                            //                row[6] = 0;
                            //            //}
                            //        }
                            //    }
                            //    else
                            //    {
                            //        row[6] = 0;
                            //    }

                            //    double pass = 0;
                            //    double appeared = Convert.ToDouble(dset.Tables[0].Rows[i]["Attended"]);

                            //    if (dset.Tables[1].Rows.Count > 0)
                            //    {
                            //        dset.Tables[1].DefaultView.RowFilter = "subject_code='" + cblsubject.Items[i].Value + "' ";
                            //        DataView dv4 = dset.Tables[1].DefaultView;

                            //        if (dset.Tables[1].Rows.Count > i)
                            //        {

                            //            if (Convert.ToString(dset.Tables[0].Rows[i]["subject_name"]) == Convert.ToString(dset.Tables[1].Rows[katt]["subject_name"]))
                            //            {
                            //                pass = Convert.ToDouble(dset.Tables[1].Rows[katt]["pass"]);
                            //                katt++;
                            //            }
                            //            else
                            //            {
                            //                pass = 0;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (dset.Tables[1].Rows.Count > katt)
                            //            {
                            //                pass = Convert.ToDouble(dset.Tables[1].Rows[katt]["pass"]);
                            //                katt++;
                            //            }
                            //            else
                            //            {
                            //                pass = 0;
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        pass = 0;
                            //    }

                            //    double passpercentage = 0;
                            //    passpercentage = (((Convert.ToDouble(pass)) / (Convert.ToDouble(appeared)) * 100));
                            //    double percentage = Math.Round(passpercentage, 2);
                            //    row[7] = Convert.ToString(percentage);

                            //dt.Rows.Add(row);
                            GenaralGrid.DataSource = dt;
                            GenaralGrid.DataBind();
                            Genaralchart.DataSource = dt;
                            Genaralchart.DataBind();
                            Genaralchart.ChartAreas[0].AxisX.RoundAxisValues();
                            Genaralchart.ChartAreas[0].AxisX.Minimum = 0;
                            Genaralchart.ChartAreas[0].AxisX.Interval = 1;
                            Genaralchart.ChartAreas[0].AxisY.Maximum = 100;
                            Genaralchart.Series["Series1"].IsValueShownAsLabel = true;
                            Genaralchart.Series[0].ChartType = SeriesChartType.Column;
                            Commongrid.Visible = false;
                            CommonClick.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Printcontrol.Visible = false;
                            Label2.Visible = false;

                            for (int j = 0; j < GenaralGrid.Rows.Count; j++)
                            {
                                GenaralGrid.Rows[j].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[5].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[7].HorizontalAlign = HorizontalAlign.Center;
                                GenaralGrid.Rows[j].Cells[8].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //y++;
                            //}

                        }//
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Records Found";
                            Commongrid.Visible = false;
                            GenaralGrid.Visible = false;
                            Genaralchart.Visible = false;
                            CommonClick.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                        if (flagvar == true)
                        {
                            lblmsg.Visible = false;
                            Commongrid.Visible = false;
                            GenaralGrid.Visible = true;
                            Genaralchart.Visible = true;
                            CommonClick.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label1.Visible = false;
                            BtnReport.Visible = true;
                            Printcontrol.Visible = false;
                            Label2.Visible = false;
                        }
                        else
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Records Found";
                            Commongrid.Visible = false;
                            GenaralGrid.Visible = false;
                            Genaralchart.Visible = false;
                            CommonClick.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Month and Year";
                        Commongrid.Visible = false;
                        GenaralGrid.Visible = false;
                        Genaralchart.Visible = false;
                        CommonClick.Visible = false;
                        Generalreportgrid.Visible = false;
                        Fpspread.Visible = false;
                        lblexportxl.Visible = false;
                        txtexcelname.Visible = false;
                        g1btnexcel.Visible = false;
                        g1btnprint.Visible = false;
                        Excel.Visible = false;
                        Print.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                    }
                    //}
                    //else
                    //{
                    //    lblmsg.Visible = true;
                    //    lblmsg.Text = "Please Select Subject";
                    //    Commongrid.Visible = false;
                    //    GenaralGrid.Visible = false;
                    //    Genaralchart.Visible = false;
                    //    CommonClick.Visible = false;
                    //    Label1.Visible = false;
                    //    Generalreportgrid.Visible = false;
                    //    Fpspread.Visible = false;
                    //    lblexportxl.Visible = false;
                    //    txtexcelname.Visible = false;
                    //    g1btnexcel.Visible = false;
                    //    g1btnprint.Visible = false;
                    //    Excel.Visible = false;
                    //    Print.Visible = false;
                    //}
                }
            }

            //}
            //else
            //{
            //    lblmsg.Visible = true;
            //    lblmsg.Text = "No Records Found";
            //    Commongrid.Visible = false;
            //    GenaralGrid.Visible = false;
            //    Genaralchart.Visible = false;
            //    CommonClick.Visible = false;
            //    Generalreportgrid.Visible = false;
            //    Fpspread.Visible = false;
            //    lblexportxl.Visible = false;
            //    txtexcelname.Visible = false;
            //    g1btnexcel.Visible = false;
            //    g1btnprint.Visible = false;
            //    Excel.Visible = false;
            //    Print.Visible = false;
            //    Label1.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    //----------------------------->>GeraralReportGrid

    public string loadmark(string mr)
    {
        string strgetval = "";
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "NE";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        else
        {
            strgetval = mr;
        }
        return strgetval;
    }

    protected void BtnReport_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataView dvw = new DataView();
            ArrayList addarray = new ArrayList();
            ArrayList ar = new ArrayList();
            ArrayList add = new ArrayList();
            DataRow drow = null;
            DataSet dsettst = new DataSet();
            string subcode3 = "";
            string subcode1 = "";
            string testname = "";
            int cnt = 0;
            int kk = 0;
            int colm = 0;
            int snmbr = 1;
            int km = 1;
            Boolean flag1 = false;
            ArrayList arrfpcol = new ArrayList();
            if (Rbtn.Items[1].Selected == true)
            {
                dt2.Columns.Add("S.No", typeof(string));
                dt2.Columns.Add("Roll No", typeof(string));
                dt2.Columns.Add("Reg No", typeof(string));
                dt2.Columns.Add("Student Name", typeof(string));
                if (GenaralGrid.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    for (int rows = 0; rows < GenaralGrid.Rows.Count; rows++)
                    {
                        if ((GenaralGrid.Rows[rows].FindControl("cbSelect") as CheckBox).Checked == true)
                        {
                            flag1 = true;
                            cnt++;
                            subcode3 = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                            if (km == 1)
                            {
                                subcode1 = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                subcode = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                testname = Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text);
                                km++;
                            }
                            else
                            {
                                subcode1 = subcode1 + "'" + "," + "'" + Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                subcode = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                testname = testname + "'" + "," + "'" + Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text);
                            }

                            if (subcode3 != "" && subcode != "&nbsp;")
                            {
                                if (!ht.ContainsKey(GenaralGrid.Rows[rows].Cells[2].Text))
                                {

                                    dt2.Columns.Add(subcode);
                                    ht.Add(GenaralGrid.Rows[rows].Cells[2].Text, cnt);
                                    kk++;
                                }
                                else
                                {
                                    kk = 0;
                                }
                            }
                            //}
                            //else
                            //{
                            //    Generalreportgrid.Visible = false;
                            //    Excel.Visible = false;
                            //    Print.Visible = false;
                            //    GenaralGrid.Visible = true;
                            //    lblmsg.Text = "Please Select Atleast One Subject";
                            //    lblmsg.Visible = true;
                        }
                    }
                    string SQL2 = "";
                    Boolean flag = false;

                    if (subcode != "&nbsp;")
                    {
                        //string sub2 = da.GetFunction("select subject_no from subject where subject_code in ('" + subcode + "')");

                        if (Rbtn.Items[1].Selected == true)
                        {
                            //SQL2 = "select distinct  rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_name,s.subject_code,s.subject_no from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration rt where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and y.semester=e.current_semester and rt.Roll_No=m.roll_no and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and e.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.subject_code in ('" + subcode1 + "') order by rt.Stud_Name,s.subject_name";

                            SQL2 = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_name,s.subject_code,s.subject_no,m.result from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration rt where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and y.semester=e.current_semester and rt.Roll_No=m.roll_no and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and e.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.subject_code in ('" + subcode1 + "') order by rt.Stud_Name,s.subject_name";
                            ds.Clear();
                            ds = da.select_method_wo_parameter(SQL2, "Text");
                        }
                        else
                        {
                            SQL2 = "select distinct rt.Roll_No,rt.Reg_No,rt.Stud_Name,r.marks_obtained,s.subject_code,s.subject_name,c.criteria,s.subject_no from result r,exam_type ex, subjectchooser su,registration rt,criteriaforinternal c,subject s where r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria in ('" + testname + "') and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.subject_code in ('" + subcode1 + "') order by rt.Stud_Name,s.subject_name";
                            ds.Clear();
                            ds = da.select_method_wo_parameter(SQL2, "Text");
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // --------- heading
                            //if (!ht.ContainsKey(GenaralGrid.Rows[rows].Cells[2].Text))
                            //{
                            //    if (testname != "")
                            //    {
                            //        //DataRow dr11 = null;
                            //        drow = dt2.NewRow();
                            //        drow[0] = testname;
                            //        dt2.Rows.Add(drow);
                            //        add.Add(dt2.Rows.Count);
                            //    }
                            //}
                            // --------- heading

                            DataView dv1 = new DataView();
                            Hashtable hashdv = new Hashtable();
                            for (int ij = 0; ij < ds.Tables[0].Rows.Count; ij++)
                            {
                                if (!hashdv.ContainsKey(ds.Tables[0].Rows[ij]["Roll_No"].ToString()))
                                {
                                    string rollno = ds.Tables[0].Rows[ij]["Roll_No"].ToString();
                                    ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
                                    dv1 = ds.Tables[0].DefaultView;
                                    //
                                    hashdv.Add(ds.Tables[0].Rows[ij]["Roll_No"].ToString(), colm);
                                    if (dv1.Count > 0)
                                    {
                                        flag = true;
                                        drow = dt2.NewRow();
                                        drow[0] = snmbr;
                                        snmbr++;
                                        drow[1] = dv1[0]["Roll_No"].ToString();
                                        drow[2] = dv1[0]["Reg_No"].ToString();
                                        drow[3] = dv1[0]["Stud_Name"].ToString();

                                        for (int jk = 0; jk < dv1.Count; jk++)
                                        {
                                            colm = Convert.ToInt32(ht[dv1[jk]["subject_code"].ToString()]);

                                            if (Rbtn.Items[1].Selected == true)
                                            {
                                                if (dv1[jk]["grade"].ToString() != "")
                                                {
                                                    if (dv1[jk]["grade"].ToString() != "")
                                                    {
                                                        drow[colm + 3] = Convert.ToString(dv1[jk]["grade"].ToString());
                                                    }
                                                    else
                                                    {
                                                        drow[colm + 3] = Convert.ToString(dv1[jk]["result"]).ToString();
                                                    }
                                                }
                                                else if (dv1[jk]["total"].ToString() != "")
                                                {
                                                    drow[colm + 3] = Convert.ToString(dv1[jk]["total"].ToString());
                                                }
                                                else
                                                {
                                                    drow[colm + 3] = Convert.ToString(dv1[jk]["result"]).ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (kk == 1)
                                                {
                                                    if (Convert.ToInt32(dv1[jk]["marks_obtained"]) > 0)
                                                    {
                                                        drow[(colm) + 3] = Convert.ToString(dv1[jk]["marks_obtained"]);
                                                        colm++;
                                                    }
                                                    else
                                                    {
                                                        drow[(colm) + 3] = loadmark(Convert.ToString(dv1[jk]["marks_obtained"]));
                                                        colm++;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(dv1[jk]["marks_obtained"]) > 0)
                                                    {
                                                        drow[(colm) + 3] = Convert.ToString(dv1[jk]["marks_obtained"]);
                                                        colm++;
                                                    }
                                                    else
                                                    {
                                                        drow[(colm) + 3] = loadmark(Convert.ToString(dv1[jk]["marks_obtained"]));
                                                        colm++;
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                        dt2.Rows.Add(drow);
                                    }
                                }
                            }

                        }
                        else
                        {
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            GenaralGrid.Visible = false;
                            lblmsg.Text = "No Records Found";
                            lblmsg.Visible = true;
                        }
                        if (flag == true)
                        {
                            Generalreportgrid.DataSource = dt2;
                            Generalreportgrid.DataBind();
                            Generalreportgrid.Visible = true;
                            Excel.Visible = true;
                            Print.Visible = true;
                            GenaralGrid.Visible = true;
                            lblmsg.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;


                        }
                        else
                        {
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            GenaralGrid.Visible = true;
                            lblmsg.Text = "Please Select Atleast One Subject";
                            lblmsg.Visible = true;
                        }
                    }
                }
            }
            else if (Rbtn.Items[0].Selected == true)
            {
                int ks = 4;
                int ksss = 4;
                Boolean flagraj = false;

                Fpspread.Sheets[0].RowCount = 0;
                Fpspread.Sheets[0].ColumnCount = 0;
                Fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread.Sheets[0].ColumnCount = 4;
                Fpspread.CommandBar.Visible = false;
                Fpspread.Sheets[0].SheetCorner.ColumnCount = 0;

                Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";

                Fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = System.Drawing.Color.White;
                style2.BackColor = System.Drawing.Color.Teal;
                Fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

                Fpspread.Sheets[0].Columns[0].Width = 50;
                Fpspread.Sheets[0].Columns[1].Width = 100;
                Fpspread.Sheets[0].Columns[2].Width = 150;
                Fpspread.Sheets[0].Columns[3].Width = 200;

                FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txt2 = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txt3 = new FarPoint.Web.Spread.TextCellType();

                Hashtable hattest = new Hashtable();
                if (GenaralGrid.Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    for (int rows = 0; rows < GenaralGrid.Rows.Count; rows++)
                    {
                        if ((GenaralGrid.Rows[rows].FindControl("cbSelect") as CheckBox).Checked == true)
                        {
                            //flag1 = true;
                            cnt++;
                            subcode3 = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                            if (km == 1)
                            {
                                subcode1 = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                subcode = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                if (!hattest.Contains(Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)))
                                {
                                    hattest.Add(Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text), subcode);
                                    testname = "'" + Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text) + "'";
                                }
                                else
                                {
                                    string getsub = hattest[Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)].ToString();
                                    getsub = getsub + ',' + subcode;
                                    hattest[Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)] = getsub;
                                }
                                km++;
                            }
                            else
                            {
                                subcode1 = subcode1 + "'" + "," + "'" + Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                subcode = Convert.ToString(GenaralGrid.Rows[rows].Cells[2].Text);
                                if (!hattest.Contains(Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)))
                                {
                                    hattest.Add(Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text), subcode);
                                    testname = testname + ",'" + Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text) + "'";
                                }
                                else
                                {
                                    string getsub = hattest[Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)].ToString();
                                    getsub = getsub + ',' + subcode;
                                    hattest[Convert.ToString(GenaralGrid.Rows[rows].Cells[4].Text)] = getsub;
                                }
                            }

                            //---------------------- Adding Dynamic Column -- Subject Code start
                            if (subcode3 != "" && subcode != "&nbsp;")
                            {
                                if (!ht.ContainsKey(GenaralGrid.Rows[rows].Cells[2].Text))
                                {
                                    Fpspread.Sheets[0].ColumnCount++;
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, Fpspread.Sheets[0].ColumnCount - 1].Text = subcode;
                                    Fpspread.Sheets[0].ColumnHeader.Cells[0, Fpspread.Sheets[0].ColumnCount - 1].Tag = subcode;
                                    ht.Add(GenaralGrid.Rows[rows].Cells[2].Text, cnt);
                                    ks++;
                                    kk++;
                                }
                                else
                                {
                                    kk = 0;
                                }
                            }
                            //---------------------- Adding Dynamic Column -- Subject Code end
                        }
                        else
                        {
                            Generalreportgrid.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            GenaralGrid.Visible = true;
                            lblmsg.Text = "Please Select Atleast One Subject";
                            lblmsg.Visible = true;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                        }
                    }
                    string SQL2 = "";
                    //Boolean flag = false;

                    if (subcode != "&nbsp;")
                    {
                        if (subcode1 != "" && subcode != "")
                        {
                            if (Rbtn.Items[1].Selected == true)
                            {
                                SQL2 = "select distinct  rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_name,s.subject_code,s.subject_no from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration rt where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and y.semester=e.current_semester and rt.Roll_No=m.roll_no and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and e.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.subject_code in ('" + subcode1 + "') order by rt.Stud_Name,s.subject_name";
                                ds = da.select_method_wo_parameter(SQL2, "Text");
                            }
                            else
                            {
                                SQL2 = "select rt.Roll_No,rt.Reg_No,rt.Stud_Name,r.marks_obtained,s.subject_code,s.subject_name,c.criteria,s.subject_no from result r,exam_type ex, subjectchooser su,registration rt,criteriaforinternal c,subject s where r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria in (" + testname + ") and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.subject_code in ('" + subcode1 + "') order by c.criteria,rt.Stud_Name,s.subject_name";
                                ds = da.select_method_wo_parameter(SQL2, "Text");
                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Hashtable hat = new Hashtable();
                                Fpspread.Sheets[0].AutoPostBack = true;
                                string[] sptes = testname.Split(',');
                                for (int t = 0; t <= sptes.GetUpperBound(0); t++)
                                {
                                    string[] sptg = sptes[t].Split('\'');
                                    ds.Tables[0].DefaultView.RowFilter = "criteria='" + sptg[1].ToString() + "'";
                                    DataView dvtestu = ds.Tables[0].DefaultView;
                                    Hashtable hatroll = new Hashtable();
                                    if (dvtestu.Count > 0)
                                    {
                                        //---------------------- Adding test name sub topics -- start
                                        Fpspread.Sheets[0].RowCount++;
                                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = sptg[1].ToString();
                                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Tag = sptg[1].ToString();
                                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Gainsboro;
                                        Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, ks);
                                        //---------------------- Adding test name sub topics -- end

                                        int sno = 1;
                                        int startrow = Fpspread.Sheets[0].RowCount;
                                        for (int s = 0; s < dvtestu.Count; s++)
                                        {
                                            string rollno = dvtestu[s]["Roll_No"].ToString();
                                            if (!hatroll.Contains(rollno.Trim().ToLower()))
                                            {
                                                flagraj = true;
                                                Fpspread.Sheets[0].RowCount++;
                                                hatroll.Add(rollno.Trim().ToLower(), rollno.Trim().ToLower());
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                sno++;
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].Text = dvtestu[s]["Roll_No"].ToString();
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].CellType = txt1;
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].Text = dvtestu[s]["Reg_No"].ToString();
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].CellType = txt2;
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = dvtestu[s]["Stud_Name"].ToString();
                                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].CellType = txt3;
                                            }
                                        }

                                        string getsubname = hattest[sptg[1].ToString()].ToString();
                                        for (int c = 4; c < Fpspread.Sheets[0].ColumnCount; c++)
                                        {
                                            string getsubnumber = Fpspread.Sheets[0].ColumnHeader.Cells[0, c].Tag.ToString();
                                            if (getsubname.Contains(getsubnumber))
                                            {
                                                for (int r = startrow; r < Fpspread.Sheets[0].RowCount; r++)
                                                {
                                                    string roll = Fpspread.Sheets[0].Cells[r, 1].Text.ToString();

                                                    ds.Tables[0].DefaultView.RowFilter = "criteria=" + sptes[t].ToString() + " and subject_code='" + getsubnumber + "' and roll_no='" + roll + "'";
                                                    DataView dvstu = ds.Tables[0].DefaultView;
                                                    if (dvstu.Count > 0)
                                                    {
                                                        if (Convert.ToInt32(dvstu[0]["marks_obtained"]) > 0)
                                                        {
                                                            Fpspread.Sheets[0].Cells[r, c].Text = dvstu[0]["marks_obtained"].ToString();
                                                            Fpspread.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            Fpspread.Sheets[0].Cells[r, c].Text = loadmark(dvstu[0]["marks_obtained"].ToString());
                                                            Fpspread.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                                //DataView dv1 = new DataView();
                                //Hashtable hashdv = new Hashtable();

                                // for (int ij = 0; ij < ds.Tables[0].Rows.Count; ij++)
                                //{
                                //    // --------- adding heading start
                                //    if (!hashdv.ContainsKey(ds.Tables[0].Rows[ij]["criteria"].ToString()))
                                //    {
                                //        string tstname = ds.Tables[0].Rows[ij]["criteria"].ToString();
                                //        ds.Tables[0].DefaultView.RowFilter = "criteria='" + tstname + "'";
                                //        dv1 = ds.Tables[0].DefaultView;

                                //        if (tstname != "")
                                //        {
                                //            Fpspread.Width = 630;

                                //            Fpspread.Sheets[0].RowCount++;
                                //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = dv1[0]["criteria"].ToString();
                                //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Tag = dv1[0]["criteria"].ToString();
                                //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                //            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Gainsboro;
                                //            Fpspread.Sheets[0].SpanModel.Add(Fpspread.Sheets[0].RowCount - 1, 0, 1, ks);
                                //        }
                                //    }
                                //    // --------- adding heading end
                                //    ArrayList subcodefpoint = new ArrayList();
                                //    if (!hashdv.ContainsKey(ds.Tables[0].Rows[ij]["criteria"].ToString()))
                                //    {
                                //        string tstname = ds.Tables[0].Rows[ij]["criteria"].ToString();
                                //        ds.Tables[0].DefaultView.RowFilter = "criteria='" + tstname + "'";
                                //        dv1 = ds.Tables[0].DefaultView;

                                //        hashdv.Add(ds.Tables[0].Rows[ij]["criteria"].ToString(), colm);
                                //        int lastrow = 0;
                                //        if (dv1.Count > 0)
                                //        {
                                //            int snm = 1;

                                //            for (int jk = 0; jk < dv1.Count; jk++)
                                //            {
                                //                flag = true;
                                //                Fpspread.Sheets[0].AutoPostBack = true;
                                //                if (!subcodefpoint.Contains(dv1[jk]["Roll_No"].ToString()))
                                //                {
                                //                    subcodefpoint.Add(dv1[jk]["Roll_No"].ToString());
                                //                    Fpspread.Sheets[0].RowCount++;
                                //                    Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = snm.ToString();
                                //                    Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //                    snm++;
                                //                }

                                //                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].Text = dv1[jk]["Roll_No"].ToString();
                                //                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].Text = dv1[jk]["Reg_No"].ToString();
                                //                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = dv1[jk]["Stud_Name"].ToString();

                                //                //colm = Convert.ToInt32(ht[dv1[jk]["subject_code"].ToString()]);

                                //                if (Rbtn.Items[1].Selected == true)
                                //                {
                                //                    if (dv1[jk]["grade"].ToString() != "")
                                //                    {
                                //                        if (dv1[jk]["grade"].ToString() != "")
                                //                        {
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[jk]["grade"].ToString());
                                //                        }
                                //                        else
                                //                        {
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = "AAA";
                                //                        }
                                //                    }
                                //                    else if (dv1[jk]["total"].ToString() != "")
                                //                    {
                                //                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv1[jk]["total"].ToString());
                                //                    }
                                //                    else
                                //                    {
                                //                        Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = "AAA";
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    if (kk == 1)
                                //                    {
                                //                        if (Convert.ToInt32(dv1[jk]["marks_obtained"]) > 0)
                                //                        {
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv1[jk]["marks_obtained"]);
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                //                        }
                                //                        else
                                //                        {
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].Text = loadmark(Convert.ToString(dv1[jk]["marks_obtained"]));
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                //                        }
                                //                    }
                                //                    else
                                //                    {
                                //                        if (Convert.ToInt32(dv1[jk]["marks_obtained"]) > 0)
                                //                        {
                                //                            if (lastrow == Fpspread.Sheets[0].RowCount - 1)
                                //                            {
                                //                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss + 1].Text = Convert.ToString(dv1[jk]["marks_obtained"]);
                                //                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss + 1].HorizontalAlign = HorizontalAlign.Center;
                                //                            }
                                //                            else
                                //                            {
                                //                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss].Text = Convert.ToString(dv1[jk]["marks_obtained"]);
                                //                                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss].HorizontalAlign = HorizontalAlign.Center;
                                //                            }
                                //                            lastrow = Fpspread.Sheets[0].RowCount - 1;
                                //                        }
                                //                        else
                                //                        {
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss].Text = loadmark(Convert.ToString(dv1[jk]["marks_obtained"]));
                                //                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, ksss].HorizontalAlign = HorizontalAlign.Center;
                                //                        }
                                //                    }
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                            }
                            else
                            {
                                Generalreportgrid.Visible = false;
                                Excel.Visible = false;
                                Print.Visible = false;
                                GenaralGrid.Visible = false;
                                lblmsg.Text = "No Records Found";
                                lblmsg.Visible = true;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                            }
                            if (Fpspread.Sheets[0].RowCount > 0)
                            {
                                //Generalreportgrid.Visible = true;
                                Excel.Visible = true;
                                Print.Visible = true;
                                Fpspread.Visible = true;
                                lblmsg.Visible = false;
                                lblexportxl.Visible = true;
                                txtexcelname.Visible = true;
                                g1btnexcel.Visible = true;
                                g1btnprint.Visible = true;
                            }
                            else
                            {
                                Generalreportgrid.Visible = false;
                                Excel.Visible = false;
                                Print.Visible = false;
                                GenaralGrid.Visible = true;
                                lblmsg.Text = "No Records Found";
                                lblmsg.Visible = true;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                            }
                        }
                        else
                        {
                            Generalreportgrid.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                            GenaralGrid.Visible = true;
                            lblmsg.Text = "Please Select Atleast One Subject";
                            lblmsg.Visible = true;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                        }
                    }

                }
                Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
                Fpspread.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string strexcelname = txtexcelname.Text;
            if (strexcelname != "")
            {
                lblerrmsgxl.Text = " ";
                lblerrmsgxl.Visible = false;
                da.printexcelreport(Fpspread, strexcelname);
            }
            else
            {
                txtexcelname.Focus();
                lblerrmsgxl.Text = "Please Enter the Report Name";
                lblerrmsgxl.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Common Subjectwise Result Analysys" + '@' + "                                                                                                                               " + Rbtn.SelectedItem.Text + '@' + "Subject Type: " + ddlsubtype.SelectedItem.Text + '@' + "Degree & Department: " + ddldegree.SelectedItem.Text + " - " + ddldept.SelectedItem.Text;
            string pagename = "Common_Subjectwise_Result.aspx";
            Printcontrol.loadspreaddetails(Fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void Generalreportgrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int colm = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (Session["Regflag"].ToString() == "0")
                {
                    e.Row.Cells[2].Visible = false;
                }

                if (Session["Rollflag"].ToString() == "0")
                {
                    e.Row.Cells[1].Visible = false;
                }

                for (int j = 4; j > colm; j++)
                {
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["Regflag"].ToString() == "0")
                {
                    e.Row.Cells[2].Visible = false;
                }
                if (Session["Rollflag"].ToString() == "0")
                {
                    e.Row.Cells[1].Visible = false;
                }

            }
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            for (int j = 2; j <= Generalreportgrid.Rows.Count; j++)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            //}

        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
    //----------------------------->>CommonclickGrid


    protected void GenaralGrid_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            //GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            //TableHeaderCell cell = new TableHeaderCell();
            //if (ddldegree.SelectedValue != "")
            //{
            //    cell.Text = "Common Subjectwise Result Analysis";
            //}

            //cell.ColumnSpan = column + 9;
            //row.Controls.Add(cell);
            //GenaralGrid.HeaderRow.Parent.Controls.AddAt(0, row);

            //GridViewRow row1 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Normal);
            //TableHeaderCell cell1 = new TableHeaderCell();
            //if (ddldegree.SelectedValue != "")
            //{
            //    cell1.Text = ddldegree.SelectedItem.Text + " - " + ddldept.SelectedItem.Text;
            //}

            //cell1.ColumnSpan = column + 9;
            //row1.Controls.Add(cell1);
            //GenaralGrid.HeaderRow.Parent.Controls.AddAt(1, row1);
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void GenaralGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int colm = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (GenaralGrid.Columns.Count > 2)
                {
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void Commongrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (ddlsubtype.SelectedItem.Text == "Common")
            {
                if (Rbtn.Items[0].Selected == true)
                {
                    string buildvalue1 = "";
                    for (int i = 0; i < cbltest.Items.Count; i++)
                    {
                        if (cbltest.Items[i].Selected == true)
                        {
                            string build1 = cbltest.Items[i].Value.ToString();
                            string subqury = da.GetFunction("select distinct subject_code from subject where subject_name='" + build1 + "'");

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

                    int row = Convert.ToInt32(e.CommandArgument);
                    int snoraj = 1;
                    degdep = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text;

                    if (e.CommandName == "Appeared")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        //sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        //status = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                        subra1 = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);


                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        //string SQL3 = "select distinct rt.roll_no,Stud_Name,rt.Reg_No, c.criteria,rt.degree_code,s.subject_code,s.subject_name,marks_obtained from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c,subject s where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "' and s.CommonSub=1 and rt.Roll_No not in(   select distinct rt.Roll_No from result r, registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and  r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and rt.RollNo_Flag<>0 and c.criteria = '" + subra1 + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1)";

                        string SQL3 = "select distinct rt.roll_no,Stud_Name,rt.Reg_No, c.criteria,rt.degree_code,s.subject_code,s.subject_name,marks_obtained from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c,subject s where marks_obtained>=0 and r.roll_no=rt.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "' and s.CommonSub=1 and rt.Roll_No not in(select distinct r1.Roll_No from result r1, exam_type ex1 where  r1.marks_obtained<0  and r1.roll_no=su.roll_no and su.subject_no=ex1.subject_no and  r1.exam_code=ex1.exam_code and ex1.subject_no=s.subject_no and r1.roll_no=rt.roll_no and ex1.criteria_no = ex.criteria_no and s.CommonSub=1) and s.subject_code in ('" + subcode + "')";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Appeared";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Appeared";
                            apraj = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Appeared";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["marks_obtained"].ToString() != "")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["marks_obtained"]) > 0)
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]);
                                    }
                                    else
                                    {
                                        drow1[4] = loadmark(Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]));
                                    }
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Pass")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        //sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra1 = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        string SQL3 = "select distinct r.roll_no,Stud_Name,rt.Reg_No,c.criteria,rt.degree_code,s.subject_code,s.subject_name,marks_obtained from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and c.criteria = '" + subra1 + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "'  and s.CommonSub=1 and rt.Roll_No not in ( select distinct rt.roll_no from  result r,exam_type ex,subjectchooser su, registration rt,criteriaforinternal c where s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and   rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and   rt.Current_Semester=su.semester and  su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag  <> 'DEBAR' and rt.delflag=0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "' and s.CommonSub=1) and rt.Roll_No not in (select distinct rt.roll_no from result r,registration rt,exam_type ex,subjectchooser su , criteriaforinternal c where s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and  r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and  rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "'  and s.CommonSub=1) and s.subject_code in ('" + subcode + "')";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Passed";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Passed";
                            apraj = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Passed";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["marks_obtained"].ToString() != "")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["marks_obtained"]) > 0)
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]);
                                    }
                                    else
                                    {
                                        drow1[4] = loadmark(Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]));
                                    }
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Fail")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        //sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra1 = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        //string SQL3 = "select distinct rt.roll_no,Stud_Name,rt.Reg_No,c.criteria, rt.degree_code,s.subject_code,s.subject_name,r.marks_obtained from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c,subject s where  r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or  r.marks_obtained='-3' or r.marks_obtained='-2')and r.marks_obtained='-1' and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria = '" + subra1 + "'  and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1 and s.subject_code in ('" + subcode + "')";

                        string SQL3 = "select distinct rt.roll_no,Stud_Name,rt.Reg_No,c.criteria, rt.degree_code,s.subject_code,s.subject_name,r.marks_obtained from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c,subject s where  r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and r.marks_obtained<ex.min_mark and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria = '" + subra1 + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "'  and s.CommonSub=1 and s.subject_code in ('" + subcode + "')";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Failed";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Failed";
                            apraj = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Failed";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["marks_obtained"].ToString() != "")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["marks_obtained"]) > 0)
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]);
                                    }
                                    else
                                    {
                                        drow1[4] = loadmark(Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]));
                                    }
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Absentees")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        //sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra1 = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        string SQL3 = "select distinct rt.roll_no,Stud_Name,rt.Reg_No,c.criteria, rt.degree_code,s.subject_code,s.subject_name,r.marks_obtained from result r,registration rt,exam_type ex, subjectchooser su ,criteriaforinternal c,subject s where r.marks_obtained='-1' and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + subra1 + "' and rt.degree_code= '" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1 and s.subject_code in ('" + subcode + "')";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Absentees";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Absentees";
                            apraj = degdep + "-" + subra1 + "-" + subcode + "-" + degree + "-" + "Absentees";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["marks_obtained"].ToString() != "")
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["marks_obtained"]) > 0)
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]);
                                    }
                                    else
                                    {
                                        drow1[4] = loadmark(Convert.ToString(ds.Tables[0].Rows[i]["marks_obtained"]));
                                    }
                                }
                                else
                                {
                                    drow1[4] = "AAA";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }
                }
                // --------------  Common Universitywise Selection By Rajesh
                else if (Rbtn.Items[1].Selected == true)
                {
                    int row = Convert.ToInt32(e.CommandArgument);
                    int snoraj = 1;
                    degdep = ddldegree.SelectedItem.Text + "-" + ddldept.SelectedItem.Text;

                    if (e.CommandName == "Appeared")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        subcod1 = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        // old query
                        //string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and result !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subra + "'  ";

                        string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and result !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'   and s.subject_no = '" + subra + "'";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Appeared";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = subcode + "-" + subcod1 + "-" + degree + "-" + "Appeared";
                            apraj = subcode + "-" + subcod1 + "-" + degree + "-" + "Appeared";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                {
                                    if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                        {

                                            drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                        }
                                        else
                                        {
                                            drow1[4] = "-";
                                        }
                                    }
                                }
                                else if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                {
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                    }
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {

                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Passed")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        subcod1 = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('Pass') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no = '" + subra + "' and r.roll_no not in (select distinct roll_no from subject s,syllabus_master y, mark_entry m, Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('fail') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no= '" + subra + "')";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Passed";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = subcode + "-" + subcod1 + "-" + degree + "-" + "Passed";
                            apraj = subcode + "-" + subcod1 + "-" + degree + "-" + "Passed";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                {
                                    if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                        {
                                            drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                        }
                                        else
                                        {
                                            drow1[4] = "-";
                                        }
                                    }
                                }
                                else if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                {
                                    drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Failed")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        subcod1 = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        //string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in('fail') and passorfail in(0) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "'";

                        string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('fail','AAA') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no  = '" + subra + "'";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Failed";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = subcode + "-" + subcod1 + "-" + degree + "-" + "Failed";
                            apraj = subcode + "-" + subcod1 + "-" + degree + "-" + "Failed";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                {
                                    if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                        {
                                            drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                        }
                                        else
                                        {
                                            drow1[4] = "-";
                                        }
                                    }
                                }
                                else if (ds.Tables[0].Rows[i]["total"].ToString() != "")
                                {
                                    drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["total"]);
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }

                    else if (e.CommandName == "Absentees")
                    {
                        subcode = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                        degree = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                        subra = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                        subcod1 = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);

                        DataTable dt3 = new DataTable();
                        DataView dvw1 = new DataView();
                        DataRow drow1 = null;

                        //string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym,result from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result  ='AAA' and passorfail  in(0) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "'";

                        string SQL3 = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym,result from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r  where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('AAA','WHD','W') and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_no = '" + subra + "'";
                        ds = da.select_method_wo_parameter(SQL3, "Text");
                        dt3.Columns.Add("S.No", typeof(string));
                        dt3.Columns.Add("Roll No", typeof(string));
                        dt3.Columns.Add("Reg No", typeof(string));
                        dt3.Columns.Add("Student Name", typeof(string));
                        dt3.Columns.Add("Mark / Grade", typeof(string));
                        result = "Absentees";

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Label1.Text = subcode + "-" + subcod1 + "-" + degree + "-" + "Absentees";
                            apraj = subcode + "-" + subcod1 + "-" + degree + "-" + "Absentees";
                            Label1.Visible = true;

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dvw1 = ds.Tables[0].DefaultView;
                                drow1 = dt3.NewRow();
                                drow1[0] = snoraj;
                                snoraj++;
                                drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                                drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                                drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                                if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                {
                                    if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                                    {
                                        drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                                    }
                                    else
                                    {
                                        if (ds.Tables[0].Rows[i]["result"].ToString() != "")
                                        {
                                            drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["result"]);
                                        }
                                        else
                                        {
                                            drow1[4] = "-";
                                        }
                                    }
                                }
                                else if (ds.Tables[0].Rows[i]["result"].ToString() != "")
                                {
                                    drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["result"]);
                                }
                                else
                                {
                                    drow1[4] = "-";
                                }

                                dt3.Rows.Add(drow1);
                                CommonClick.DataSource = dt3;
                                CommonClick.DataBind();
                                CommonClick.Visible = true;
                                BtnReport.Visible = false;
                                GenaralGrid.Visible = false;
                                Generalreportgrid.Visible = false;
                                Fpspread.Visible = false;
                                lblexportxl.Visible = false;
                                txtexcelname.Visible = false;
                                g1btnexcel.Visible = false;
                                g1btnprint.Visible = false;
                                Genaralchart.Visible = false;
                                Label3.Visible = false;
                                Label2.Visible = false;

                                for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                                {
                                    CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                        }
                        else
                        {
                            Label3.Text = "No Records Found";
                            Label3.Visible = true;
                            CommonClick.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                        }
                    }
                }
            }

            else
            {
                int row = Convert.ToInt32(e.CommandArgument);
                int snoraj = 1;
                if (e.CommandName == "Appeared")
                {
                    subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                    degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                    sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                    status = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);
                    //result = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);

                    DataTable dt3 = new DataTable();
                    DataView dvw1 = new DataView();
                    DataRow drow1 = null;

                    string SQL3 = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                    ds = da.select_method_wo_parameter(SQL3, "Text");
                    dt3.Columns.Add("S.No", typeof(string));
                    dt3.Columns.Add("Roll No", typeof(string));
                    dt3.Columns.Add("Reg No", typeof(string));
                    dt3.Columns.Add("Student Name", typeof(string));
                    dt3.Columns.Add("Mark / Grade", typeof(string));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Label1.Text = sem + "-" + subcode + "-" + degree + "-" + "Appeared";
                        Label1.Visible = true;
                        result = "Appeared";

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dvw1 = ds.Tables[0].DefaultView;
                            drow1 = dt3.NewRow();
                            drow1[0] = snoraj;
                            snoraj++;
                            drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                            if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                            {
                                drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                            }
                            else
                            {
                                drow1[4] = "-";
                            }

                            dt3.Rows.Add(drow1);
                            CommonClick.DataSource = dt3;
                            CommonClick.DataBind();
                            CommonClick.Visible = true;
                            BtnReport.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Genaralchart.Visible = false;
                            Label3.Visible = false;
                            Label2.Visible = false;

                            for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                            {

                                CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Label3.Text = "No Records Found";
                        Label3.Visible = true;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                    }
                }

                else if (e.CommandName == "Pass")
                {
                    subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                    degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                    sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                    status = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);

                    DataTable dt3 = new DataTable();
                    DataView dvw1 = new DataView();
                    DataRow drow1 = null;

                    string SQL3 = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='Pass' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                    ds = da.select_method_wo_parameter(SQL3, "Text");
                    dt3.Columns.Add("S.No", typeof(string));
                    dt3.Columns.Add("Roll No", typeof(string));
                    dt3.Columns.Add("Reg No", typeof(string));
                    dt3.Columns.Add("Student Name", typeof(string));
                    dt3.Columns.Add("Mark / Grade", typeof(string));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Label1.Text = sem + "-" + subcode + "-" + degree + "-" + "Passed";
                        Label1.Visible = true;
                        result = "Passed";

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dvw1 = ds.Tables[0].DefaultView;
                            drow1 = dt3.NewRow();
                            drow1[0] = snoraj;
                            snoraj++;
                            drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                            if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                            {
                                drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                            }
                            else
                            {
                                drow1[4] = "-";
                            }

                            dt3.Rows.Add(drow1);
                            CommonClick.DataSource = dt3;
                            CommonClick.DataBind();
                            CommonClick.Visible = true;
                            BtnReport.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Genaralchart.Visible = false;
                            Label3.Visible = false;
                            Label2.Visible = false;

                            for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                            {
                                CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Label3.Text = "No Records Found";
                        Label3.Visible = true;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                    }
                }

                else if (e.CommandName == "Fail")
                {
                    subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                    degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                    sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                    status = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);

                    DataTable dt3 = new DataTable();
                    DataView dvw1 = new DataView();
                    DataRow drow1 = null;

                    string SQL3 = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='Fail' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                    ds = da.select_method_wo_parameter(SQL3, "Text");
                    dt3.Columns.Add("S.No", typeof(string));
                    dt3.Columns.Add("Roll No", typeof(string));
                    dt3.Columns.Add("Reg No", typeof(string));
                    dt3.Columns.Add("Student Name", typeof(string));
                    dt3.Columns.Add("Mark / Grade", typeof(string));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Label1.Text = sem + "-" + subcode + "-" + degree + "-" + "Failed";
                        Label1.Visible = true;
                        result = "Failed";

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dvw1 = ds.Tables[0].DefaultView;
                            drow1 = dt3.NewRow();
                            drow1[0] = snoraj;
                            snoraj++;
                            drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                            if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                            {
                                drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                            }
                            else
                            {
                                drow1[4] = "-";
                            }

                            dt3.Rows.Add(drow1);
                            CommonClick.DataSource = dt3;
                            CommonClick.DataBind();
                            CommonClick.Visible = true;
                            BtnReport.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Genaralchart.Visible = false;
                            Label3.Visible = false;
                            Label2.Visible = false;

                            for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                            {
                                CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Label3.Text = "No Records Found";
                        Label3.Visible = true;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                    }
                }

                else if (e.CommandName == "Absentees")
                {
                    subcode = Convert.ToString(Commongrid.Rows[row].Cells[1].Text);
                    degree = Convert.ToString(Commongrid.Rows[row].Cells[2].Text);
                    sem = Convert.ToString(Commongrid.Rows[row].Cells[3].Text);
                    status = Convert.ToString(Commongrid.Rows[row].Cells[4].Text);

                    DataTable dt3 = new DataTable();
                    DataView dvw1 = new DataView();
                    DataRow drow1 = null;

                    string SQL3 = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                    ds = da.select_method_wo_parameter(SQL3, "Text");
                    dt3.Columns.Add("S.No", typeof(string));
                    dt3.Columns.Add("Roll No", typeof(string));
                    dt3.Columns.Add("Reg No", typeof(string));
                    dt3.Columns.Add("Student Name", typeof(string));
                    dt3.Columns.Add("Mark / Grade", typeof(string));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Label1.Text = sem + "-" + subcode + "-" + degree + "-" + "Absentees";
                        Label1.Visible = true;
                        result = "Absentees";

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dvw1 = ds.Tables[0].DefaultView;
                            drow1 = dt3.NewRow();
                            drow1[0] = snoraj;
                            snoraj++;
                            drow1[1] = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            drow1[2] = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            drow1[3] = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);

                            if (ds.Tables[0].Rows[i]["grade"].ToString() != "")
                            {
                                drow1[4] = Convert.ToString(ds.Tables[0].Rows[i]["grade"]);
                            }
                            else
                            {
                                drow1[4] = "AAA";
                            }

                            dt3.Rows.Add(drow1);
                            CommonClick.DataSource = dt3;
                            CommonClick.DataBind();
                            CommonClick.Visible = true;
                            BtnReport.Visible = false;
                            GenaralGrid.Visible = false;
                            Generalreportgrid.Visible = false;
                            Fpspread.Visible = false;
                            lblexportxl.Visible = false;
                            txtexcelname.Visible = false;
                            g1btnexcel.Visible = false;
                            g1btnprint.Visible = false;
                            Genaralchart.Visible = false;
                            Label3.Visible = false;
                            Label2.Visible = false;

                            for (int ik = 0; ik < CommonClick.Rows.Count; ik++)
                            {
                                CommonClick.Rows[ik].Cells[4].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        Label3.Text = "No Records Found";
                        Label3.Visible = true;
                        CommonClick.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Label2.Text = ex.ToString();
            Label2.Visible = true;
        }
    }

    protected void CommonClick_OnDataBound(object sender, EventArgs e)
    {
        try
        {

            //for (int i = CommonClick.Rows.Count - 1; i > 0; i--)
            //{
            //    GridViewRow row = CommonClick.Rows[i];
            //    GridViewRow previousRow = CommonClick.Rows[i - 1];

            //    for (int j = 0; j <= 2; j++)
            //    {
            //        if (j == 0)
            //        {
            //            Label lnlname = (Label)row.FindControl("Iblserial");
            //            Label lnlname1 = (Label)previousRow.FindControl("Iblserial");

            //            if (lnlname.Text == lnlname1.Text)
            //            {
            //                if (previousRow.Cells[j].RowSpan == 0)
            //                {
            //                    if (row.Cells[j].RowSpan == 0)
            //                    {
            //                        previousRow.Cells[j].RowSpan += 2;
            //                    }
            //                    else
            //                    {
            //                        previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
            //                    }
            //                    row.Cells[j].Visible = false;
            //                }
            //            }

            //        }
            //        if (j == 1)
            //        {
            //            Label lnlname = (Label)row.FindControl("lblbatch");
            //            Label lnlname1 = (Label)previousRow.FindControl("lblbatch");

            //            if (lnlname.Text == lnlname1.Text)
            //            {
            //                if (previousRow.Cells[j].RowSpan == 0)
            //                {
            //                    if (row.Cells[j].RowSpan == 0)
            //                    {
            //                        previousRow.Cells[j].RowSpan += 2;
            //                    }
            //                    else
            //                    {
            //                        previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
            //                    }
            //                    row.Cells[j].Visible = false;
            //                }
            //            }
            //        }

            //        if (j == 2)
            //        {
            //            Label lnlname = (Label)row.FindControl("Acronym");
            //            Label lnlname1 = (Label)previousRow.FindControl("Acronym");

            //            if (lnlname.Text == lnlname1.Text)
            //            {
            //                if (previousRow.Cells[j].RowSpan == 0)
            //                {
            //                    if (row.Cells[j].RowSpan == 0)
            //                    {
            //                        previousRow.Cells[j].RowSpan += 2;
            //                    }
            //                    else
            //                    {
            //                        previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
            //                    }
            //                    row.Cells[j].Visible = false;
            //                }
            //            }
            //        }

            //    }
            //}
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    protected void Commongrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlsubtype.SelectedItem.Text == "Common")
                {
                    if (Rbtn.Items[1].Selected == true)
                    {
                        e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Appeared$" + e.Row.RowIndex);
                        e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Passed$" + e.Row.RowIndex);
                        e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Failed$" + e.Row.RowIndex);
                        e.Row.Cells[8].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Absentees$" + e.Row.RowIndex);

                        e.Row.Cells[5].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[6].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[7].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[8].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                    }
                    else
                    {
                        e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Appeared$" + e.Row.RowIndex);
                        e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Pass$" + e.Row.RowIndex);
                        e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Fail$" + e.Row.RowIndex);
                        e.Row.Cells[7].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Commongrid, "Absentees$" + e.Row.RowIndex);

                        e.Row.Cells[4].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[5].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[6].Attributes.Add("onmouseover", "setMouseOverColor(this);");
                        e.Row.Cells[7].Attributes.Add("onmouseover", "setMouseOverColor(this);");

                    }
                }
            }

            e.Row.Cells[1].Visible = true;
            e.Row.Cells[2].Visible = true;
            CommonClick.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }

    }

    protected void chkboxSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Excel.Visible = false;
            Print.Visible = false;
            Generalreportgrid.Visible = false;
            Fpspread.Visible = false;
            lblexportxl.Visible = false;
            txtexcelname.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;

            CheckBox ChkBoxHeader = (CheckBox)GenaralGrid.HeaderRow.FindControl("chkboxSelectAll");
            foreach (GridViewRow row in GenaralGrid.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("cbSelect");
                if (ChkBoxHeader.Checked == true)
                {
                    ChkBoxRows.Checked = true;
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }
            }
            GenaralGrid.Visible = true;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }


    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition",
            "attachment;filename=CommonSubjectwiseResultAnalysis.xls");
            Response.ContentType = "applicatio/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            Commongrid.RenderControl(htm);
            GenaralGrid.RenderControl(htm);
            //Label lb = new Label();
            //lb.Text = "Rajesh                       ";
            //lb.ForeColor = System.Drawing.Color.Black;
            //lb.RenderControl(htm);
            Label lb = new Label();
            lb.Text = "<br>";
            lb.ForeColor = System.Drawing.Color.Black;
            lb.RenderControl(htm);
            Generalreportgrid.RenderControl(htm);
            CommonClick.RenderControl(htm);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */

    }

    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {
            //int row = Convert.ToInt32(e.CommandArgument);
            if (ddlsubtype.SelectedItem.Text == "General")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CommonSubjectwiseResultAnalysis.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                if (GenaralGrid.Rows.Count > 0)
                {
                    Label lb = new Label();
                    lb.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Common Subjectwise Result Analysis";
                    lb.Style.Add("height", "200px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb.Style.Add("font-size", "14px");
                    lb.Style.Add("text-align", "left");
                    lb.RenderControl(hw);

                    Label lb3 = new Label();
                    lb3.Text = "<br>";
                    lb3.Style.Add("height", "200px");
                    lb3.Style.Add("text-decoration", "none");
                    lb3.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb3.Style.Add("font-size", "14px");
                    lb3.Style.Add("text-align", "left");
                    lb3.RenderControl(hw);

                    Label lb2 = new Label();
                    lb2.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Rbtn.SelectedItem.Text;
                    lb2.Style.Add("height", "200px");
                    lb2.Style.Add("text-decoration", "none");
                    lb2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb2.Style.Add("font-size", "14px");
                    lb2.Style.Add("text-align", "left");
                    lb2.RenderControl(hw);

                    if (Rbtn.SelectedItem.Text != "CAM wise")
                    {
                        Label lb4 = new Label();
                        lb4.Text = "<br>";
                        lb4.Style.Add("height", "200px");
                        lb4.Style.Add("text-decoration", "none");
                        lb4.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb4.Style.Add("font-size", "14px");
                        lb4.Style.Add("text-align", "left");
                        lb4.RenderControl(hw);

                        Label lb5 = new Label();
                        lb5.Text = "Exam Month & Year: " + ddlexm.SelectedItem.Text + "  " + ddlyear.SelectedItem.Text;
                        lb5.Style.Add("height", "200px");
                        lb5.Style.Add("text-decoration", "none");
                        lb5.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb5.Style.Add("font-size", "14px");
                        lb5.Style.Add("text-align", "left");
                        lb5.RenderControl(hw);
                    }
                    Label lb6 = new Label();
                    lb6.Text = "<br>";
                    lb6.Style.Add("height", "200px");
                    lb6.Style.Add("text-decoration", "none");
                    lb6.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb6.Style.Add("font-size", "14px");
                    lb6.Style.Add("text-align", "left");
                    lb6.RenderControl(hw);

                    Label lb7 = new Label();
                    lb7.Text = "Subject Type: " + ddlsubtype.SelectedItem.Text;
                    lb7.Style.Add("height", "200px");
                    lb7.Style.Add("text-decoration", "none");
                    lb7.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb7.Style.Add("font-size", "14px");
                    lb7.Style.Add("text-align", "left");
                    lb7.RenderControl(hw);

                    Label lb8 = new Label();
                    lb8.Text = "<br>";
                    lb8.Style.Add("height", "200px");
                    lb8.Style.Add("text-decoration", "none");
                    lb8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb8.Style.Add("font-size", "14px");
                    lb8.Style.Add("text-align", "left");
                    lb8.RenderControl(hw);

                    Label lb9 = new Label();
                    lb9.Text = "Degree & Department: " + ddldegree.SelectedItem.Text + " - " + ddldept.SelectedItem.Text;
                    lb9.Style.Add("height", "200px");
                    lb9.Style.Add("text-decoration", "none");
                    lb9.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb9.Style.Add("font-size", "14px");
                    lb9.Style.Add("text-align", "left");
                    lb9.RenderControl(hw);

                    Label lb11 = new Label();
                    lb11.Text = "<br>";
                    lb11.Style.Add("height", "200px");
                    lb11.Style.Add("text-decoration", "none");
                    lb11.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb11.Style.Add("font-size", "14px");
                    lb11.Style.Add("text-align", "left");
                    lb11.RenderControl(hw);

                    if (Rbtn.SelectedItem.Text == "CAM wise")
                    {
                        string build1 = "";
                        string buildvalue = "";
                        for (int i1 = 0; i1 < cbltest.Items.Count; i1++)
                        {
                            if (cbltest.Items[i1].Selected == true)
                            {

                                build1 = cbltest.Items[i1].Value.ToString();

                                if (buildvalue == "")
                                {
                                    buildvalue = build1;
                                }
                                else
                                {
                                    buildvalue = buildvalue + ", " + build1;
                                }
                            }
                        }
                        Label lb1A = new Label();
                        lb1A.Text = "Test Name: " + buildvalue;
                        lb1A.Style.Add("height", "200px");
                        lb1A.Style.Add("text-decoration", "none");
                        lb1A.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb1A.Style.Add("font-size", "14px");
                        lb1A.Style.Add("text-align", "left");
                        lb1A.RenderControl(hw);

                        Label lb1B = new Label();
                        lb1B.Text = "<br>";
                        lb1B.Style.Add("height", "200px");
                        lb1B.Style.Add("text-decoration", "none");
                        lb1B.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb1B.Style.Add("font-size", "14px");
                        lb1B.Style.Add("text-align", "left");
                        lb1B.RenderControl(hw);

                        Label lb10 = new Label();
                        lb10.Text = " ";
                        lb10.Style.Add("height", "200px");
                        lb10.Style.Add("text-decoration", "none");
                        lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb10.Style.Add("font-size", "14px");
                        lb10.Style.Add("text-align", "left");
                        lb10.RenderControl(hw);

                        GenaralGrid.AllowPaging = false;
                        GenaralGrid.HeaderRow.Style.Add("width", "15%");
                        GenaralGrid.HeaderRow.Style.Add("font-size", "10px");
                        GenaralGrid.HeaderRow.Style.Add("text-align", "center");
                        GenaralGrid.Style.Add("font-family", "Bood Antiqua;");
                        GenaralGrid.Style.Add("font-size", "10px");
                        GenaralGrid.RenderControl(hw);
                        GenaralGrid.DataBind();

                        //}
                    }
                    else
                    {
                        Label lb10 = new Label();
                        lb10.Text = " ";
                        lb10.Style.Add("height", "200px");
                        lb10.Style.Add("text-decoration", "none");
                        lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb10.Style.Add("font-size", "14px");
                        lb10.Style.Add("text-align", "left");
                        lb10.RenderControl(hw);

                        GenaralGrid.AllowPaging = false;
                        GenaralGrid.HeaderRow.Style.Add("width", "15%");
                        GenaralGrid.HeaderRow.Style.Add("font-size", "10px");
                        GenaralGrid.HeaderRow.Style.Add("text-align", "center");
                        GenaralGrid.Style.Add("font-family", "Bood Antiqua;");
                        GenaralGrid.Style.Add("font-size", "10px");
                        GenaralGrid.RenderControl(hw);
                        GenaralGrid.DataBind();
                    }
                }

                BtnReport_OnClick(sender, e);
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
                if (Generalreportgrid.Rows.Count > 0)
                {
                    Generalreportgrid.AllowPaging = false;
                    Generalreportgrid.HeaderRow.Style.Add("width", "15%");
                    Generalreportgrid.HeaderRow.Style.Add("font-size", "10px");
                    Generalreportgrid.HeaderRow.Style.Add("text-align", "center");
                    Generalreportgrid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    Generalreportgrid.Style.Add("font-size", "10px");
                    Generalreportgrid.RenderControl(hw1);
                    Generalreportgrid.DataBind();
                }
                Label lb1 = new Label();
                lb1.Text = "<br/><br/>";
                lb1.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString() + sw1.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();
            }
            else
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CommonSubjectwiseResultAnalysis.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                if (Commongrid.Rows.Count > 0)
                {
                    Label lb = new Label();
                    lb.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Common Subjectwise Result Analysis";
                    lb.Style.Add("height", "200px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb.Style.Add("font-size", "14px");
                    lb.Style.Add("text-align", "left");
                    lb.RenderControl(hw);

                    Label lb3 = new Label();
                    lb3.Text = "<br>";
                    lb3.Style.Add("height", "200px");
                    lb3.Style.Add("text-decoration", "none");
                    lb3.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb3.Style.Add("font-size", "14px");
                    lb3.Style.Add("text-align", "left");
                    lb3.RenderControl(hw);

                    Label lb2 = new Label();
                    lb2.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Rbtn.SelectedItem.Text;
                    lb2.Style.Add("height", "200px");
                    lb2.Style.Add("text-decoration", "none");
                    lb2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb2.Style.Add("font-size", "14px");
                    lb2.Style.Add("text-align", "left");
                    lb2.RenderControl(hw);

                    if (Rbtn.SelectedItem.Text != "CAM wise")
                    {
                        Label lb4 = new Label();
                        lb4.Text = "<br>";
                        lb4.Style.Add("height", "200px");
                        lb4.Style.Add("text-decoration", "none");
                        lb4.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb4.Style.Add("font-size", "14px");
                        lb4.Style.Add("text-align", "left");
                        lb4.RenderControl(hw);

                        Label lb5 = new Label();
                        lb5.Text = "Exam Month & Year: " + ddlexm.SelectedItem.Text + "  " + ddlyear.SelectedItem.Text;
                        lb5.Style.Add("height", "200px");
                        lb5.Style.Add("text-decoration", "none");
                        lb5.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb5.Style.Add("font-size", "14px");
                        lb5.Style.Add("text-align", "left");
                        lb5.RenderControl(hw);
                    }

                    Label lb6 = new Label();
                    lb6.Text = "<br>";
                    lb6.Style.Add("height", "200px");
                    lb6.Style.Add("text-decoration", "none");
                    lb6.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb6.Style.Add("font-size", "14px");
                    lb6.Style.Add("text-align", "left");
                    lb6.RenderControl(hw);

                    Label lb7 = new Label();
                    lb7.Text = "Subject Type: " + ddlsubtype.SelectedItem.Text;
                    lb7.Style.Add("height", "200px");
                    lb7.Style.Add("text-decoration", "none");
                    lb7.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb7.Style.Add("font-size", "14px");
                    lb7.Style.Add("text-align", "left");
                    lb7.RenderControl(hw);

                    if (Rbtn.SelectedItem.Text != "CAM wise" && ddlsubtype.SelectedItem.Text != "Common")
                    {
                        Label lb8 = new Label();
                        lb8.Text = "<br>";
                        lb8.Style.Add("height", "200px");
                        lb8.Style.Add("text-decoration", "none");
                        lb8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb8.Style.Add("font-size", "14px");
                        lb8.Style.Add("text-align", "left");
                        lb8.RenderControl(hw);

                        Label lb71 = new Label();
                        lb71.Text = "Degree & Dept: " + ddldegree.SelectedItem.Text + " - " + ddldept.SelectedItem.Text;
                        lb71.Style.Add("height", "200px");
                        lb71.Style.Add("text-decoration", "none");
                        lb71.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                        lb71.Style.Add("font-size", "14px");
                        lb71.Style.Add("text-align", "left");
                        lb71.RenderControl(hw);
                    }

                    Label lb81 = new Label();
                    lb81.Text = "<br>";
                    lb81.Style.Add("height", "200px");
                    lb81.Style.Add("text-decoration", "none");
                    lb81.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb81.Style.Add("font-size", "14px");
                    lb81.Style.Add("text-align", "left");
                    lb81.RenderControl(hw);

                    Label lb10 = new Label();
                    lb10.Text = " ";
                    lb10.Style.Add("height", "200px");
                    lb10.Style.Add("text-decoration", "none");
                    lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    lb10.Style.Add("font-size", "14px");
                    lb10.Style.Add("text-align", "left");
                    lb10.RenderControl(hw);

                    Commongrid.AllowPaging = false;
                    Commongrid.HeaderRow.Style.Add("width", "15%");
                    Commongrid.HeaderRow.Style.Add("font-size", "10px");
                    Commongrid.HeaderRow.Style.Add("text-align", "center");
                    Commongrid.Style.Add("font-family", "Bood Antiqua;");
                    Commongrid.Style.Add("font-size", "10px");
                    Commongrid.RenderControl(hw);
                    Commongrid.DataBind();
                }
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
                if (CommonClick.Rows.Count > 0)
                {
                    string subnme = "";
                    DataSet dset1 = new DataSet();
                    string SQL3query = "";
                    Boolean printval = false;

                    if (ddlsubtype.SelectedItem.Text == "Common")
                    {
                        if (Rbtn.Items[0].Selected == true)
                        {
                            if (result == "Appeared")
                            {
                                SQL3query = "select distinct rt.roll_no,Stud_Name,rt.Reg_No, c.criteria,rt.degree_code,s.subject_code,s.subject_name,marks_obtained from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c,subject s where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "' and s.CommonSub=1 and rt.Roll_No not in(   select distinct rt.Roll_No from result r, registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and  r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and rt.RollNo_Flag<>0 and c.criteria = '" + subra1 + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1)";
                                printval = true;
                                dset1 = da.select_method_wo_parameter(SQL3query, "Text");

                            }
                            else if (result == "Passed")
                            {
                                SQL3query = "select distinct r.roll_no,Stud_Name,rt.Reg_No,c.criteria,rt.degree_code,s.subject_code,s.subject_name,marks_obtained from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no  and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and  r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and c.criteria = '" + subra1 + "' and rt.degree_code='" + ddldept.SelectedValue.ToString() + "'  and s.CommonSub=1 and rt.Roll_No not in ( select distinct rt.roll_no from  result r,exam_type ex,subjectchooser su, registration rt,criteriaforinternal c where s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and   rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and   rt.Current_Semester=su.semester and  su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag  <> 'DEBAR' and rt.delflag=0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "' and s.CommonSub=1) and rt.Roll_No not in (select distinct rt.roll_no from result r,registration rt,exam_type ex,subjectchooser su , criteriaforinternal c where s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and  r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and  rt.delflag=0 and rt.RollNo_Flag<>0 and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and c.criteria = '" + subra1 + "'  and s.CommonSub=1)";
                                printval = true;
                                dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                            }
                            else if (result == "Failed")
                            {
                                SQL3query = "select distinct rt.roll_no,Stud_Name,rt.Reg_No,c.criteria, rt.degree_code,s.subject_code,s.subject_name,r.marks_obtained from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c,subject s where  r.roll_no=rt.roll_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or  r.marks_obtained='-3' or r.marks_obtained='-2')and r.marks_obtained='-1' and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and c.criteria = '" + subra1 + "'  and rt.degree_code='" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1";
                                printval = true;
                                dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                            }
                            else if (result == "Absentees")
                            {
                                SQL3query = "select distinct rt.roll_no,Stud_Name,rt.Reg_No,c.criteria, rt.degree_code,s.subject_code,s.subject_name,r.marks_obtained from result r,registration rt,exam_type ex, subjectchooser su ,criteriaforinternal c,subject s where r.marks_obtained='-1' and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and r.roll_no=su.roll_no and rt.Current_Semester=su.semester and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and c.criteria = '" + subra1 + "' and rt.degree_code= '" + ddldept.SelectedValue.ToString() + "' and s.CommonSub=1";
                                printval = true;
                                dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                            }

                            if (printval == true)
                            {
                                if (dset1.Tables[0].Rows.Count > 0)
                                {
                                    if (CommonClick.Visible == true)
                                    {
                                        Label lbl1a = new Label();
                                        lbl1a.Text = Label1.Text;
                                        lbl1a.ForeColor = System.Drawing.Color.Brown;
                                        lbl1a.Style.Add("height", "200px");
                                        lbl1a.Style.Add("text-decoration", "none");
                                        lbl1a.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                        lbl1a.Style.Add("font-size", "14px");
                                        lbl1a.Style.Add("text-align", "left");
                                        lbl1a.RenderControl(hw1);

                                        Label lba8 = new Label();
                                        lba8.Text = "<br>";
                                        lba8.Style.Add("height", "200px");
                                        lba8.Style.Add("text-decoration", "none");
                                        lba8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                        lba8.Style.Add("font-size", "14px");
                                        lba8.Style.Add("text-align", "left");
                                        lba8.RenderControl(hw1);

                                        Label lb10 = new Label();
                                        lb10.Text = " ";
                                        lb10.Style.Add("height", "200px");
                                        lb10.Style.Add("text-decoration", "none");
                                        lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                        lb10.Style.Add("font-size", "14px");
                                        lb10.Style.Add("text-align", "left");
                                        lb10.RenderControl(hw1);
                                    }
                                }
                            }
                        }
                        else if (Rbtn.Items[1].Selected == true)
                        {
                            if (result == "Appeared")
                            {
                                SQL3query = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and result !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subra + "'  ";
                            }
                            else if (result == "Passed")
                            {
                                SQL3query = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('Pass') and passorfail in(1) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "' and r.roll_no not in (select distinct roll_no from subject s,syllabus_master y, mark_entry m, Degree d ,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in ('fail') and passorfail in(0) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "') ";
                            }
                            else if (result == "Failed")
                            {
                                SQL3query = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result in('fail') and passorfail in(0) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "'";
                            }
                            else if (result == "Absentees")
                            {
                                SQL3query = "select distinct r.roll_no,r.Reg_No,r.Stud_Name,r.Batch_Year,r.degree_code,m.total,m.Actual_Grade,m.grade,s.subject_no,s.subject_code,s.subject_name,d.Acronym from subject s,syllabus_master y,mark_entry m,Degree d,Exam_Details e,Registration r where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and r.degree_code=e.degree_code and r.Batch_Year=e.batch_year and r.degree_code=y.degree_code and r.Batch_Year=y.Batch_Year and r.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and result  ='AAA' and passorfail  in(0) and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "' and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "' and s.subject_code = '" + subra + "'";
                            }

                            dset1 = da.select_method_wo_parameter(SQL3query, "Text");

                            if (dset1.Tables[0].Rows.Count > 0)
                            {
                                if (CommonClick.Visible == true)
                                {
                                    Label lbl1a = new Label();
                                    lbl1a.Text = Label1.Text;
                                    lbl1a.ForeColor = System.Drawing.Color.Brown;
                                    lbl1a.Style.Add("height", "200px");
                                    lbl1a.Style.Add("text-decoration", "none");
                                    lbl1a.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                    lbl1a.Style.Add("font-size", "14px");
                                    lbl1a.Style.Add("text-align", "left");
                                    lbl1a.RenderControl(hw1);

                                    Label lba8 = new Label();
                                    lba8.Text = "<br>";
                                    lba8.Style.Add("height", "200px");
                                    lba8.Style.Add("text-decoration", "none");
                                    lba8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                    lba8.Style.Add("font-size", "14px");
                                    lba8.Style.Add("text-align", "left");
                                    lba8.RenderControl(hw1);

                                    Label lb10 = new Label();
                                    lb10.Text = " ";
                                    lb10.Style.Add("height", "200px");
                                    lb10.Style.Add("text-decoration", "none");
                                    lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                                    lb10.Style.Add("font-size", "14px");
                                    lb10.Style.Add("text-align", "left");
                                    lb10.RenderControl(hw1);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (result == "Appeared")
                        {
                            SQL3query = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  !='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                            dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                        }
                        else if (result == "Pass")
                        {
                            SQL3query = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='Pass' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                            dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                        }
                        else if (result == "Fail")
                        {
                            SQL3query = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='Fail' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                            dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                        }
                        else if (result == "Absentees")
                        {
                            SQL3query = "select distinct rt.Reg_No,rt.Roll_No,rt.Stud_Name,m.total,m.grade,s.subject_code,s.subject_name from subject s,syllabus_master y,Registration rt,  mark_entry m,Degree d ,Exam_Details e  where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and  d.Degree_Code=y.degree_code   and e.exam_code=m.exam_code    and rt.Roll_No=m.roll_no        and result  ='AAA' and e.Exam_year='" + ddlyear.SelectedValue.ToString() + "'  and e.Exam_Month='" + ddlexm.SelectedValue.ToString() + "'  and s.subject_code = '" + subcode + "'  and m.attempts=1";
                            dset1 = da.select_method_wo_parameter(SQL3query, "Text");
                        }

                        if (dset1.Tables[0].Rows.Count > 0)
                        {
                            subnme = dset1.Tables[0].Rows[0]["subject_name"].ToString();
                            subcode = dset1.Tables[0].Rows[0]["subject_code"].ToString();

                            Label lbl1 = new Label();
                            lbl1.Text = "Department: " + sem;
                            lbl1.Style.Add("height", "200px");
                            lbl1.Style.Add("text-decoration", "none");
                            lbl1.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            lbl1.Style.Add("font-size", "14px");
                            lbl1.Style.Add("text-align", "left");
                            lbl1.RenderControl(hw1);

                            Label lb8 = new Label();
                            lb8.Text = "<br>";
                            lb8.Style.Add("height", "200px");
                            lb8.Style.Add("text-decoration", "none");
                            lb8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            lb8.Style.Add("font-size", "14px");
                            lb8.Style.Add("text-align", "left");
                            lb8.RenderControl(hw1);

                            Label lbl1a = new Label();
                            lbl1a.Text = "Subject Code & Subject Name: " + subcode + " - " + subnme + " - " + result;
                            lbl1a.Style.Add("height", "200px");
                            lbl1a.Style.Add("text-decoration", "none");
                            lbl1a.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            lbl1a.Style.Add("font-size", "14px");
                            lbl1a.Style.Add("text-align", "left");
                            lbl1a.RenderControl(hw1);

                            Label lba8 = new Label();
                            lba8.Text = "<br>";
                            lba8.Style.Add("height", "200px");
                            lba8.Style.Add("text-decoration", "none");
                            lba8.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            lba8.Style.Add("font-size", "14px");
                            lba8.Style.Add("text-align", "left");
                            lba8.RenderControl(hw1);

                            Label lb10 = new Label();
                            lb10.Text = " ";
                            lb10.Style.Add("height", "200px");
                            lb10.Style.Add("text-decoration", "none");
                            lb10.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            lb10.Style.Add("font-size", "14px");
                            lb10.Style.Add("text-align", "left");
                            lb10.RenderControl(hw1);

                            //CommonClick.AllowPaging = false;
                            //CommonClick.HeaderRow.Style.Add("width", "15%");
                            //CommonClick.HeaderRow.Style.Add("font-size", "10px");
                            //CommonClick.HeaderRow.Style.Add("text-align", "center");
                            //CommonClick.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                            //CommonClick.Style.Add("font-size", "10px");
                            //CommonClick.RenderControl(hw1);
                            //CommonClick.DataBind();
                        }
                    }

                    CommonClick.AllowPaging = false;
                    CommonClick.HeaderRow.Style.Add("width", "15%");
                    CommonClick.HeaderRow.Style.Add("font-size", "10px");
                    CommonClick.HeaderRow.Style.Add("text-align", "center");
                    CommonClick.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    CommonClick.Style.Add("font-size", "10px");
                    CommonClick.RenderControl(hw1);
                    CommonClick.DataBind();
                }
                Label lb1 = new Label();
                lb1.Text = "<br/><br/>";
                lb1.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString() + sw1.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void CommonClick_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["Regflag"].ToString() == "0")
                {
                    e.Row.Cells[2].Visible = false;
                }

                if (Session["Rollflag"].ToString() == "0")
                {
                    e.Row.Cells[1].Visible = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (Session["Regflag"].ToString() == "0")
                {
                    e.Row.Cells[2].Visible = false;
                }
                if (Session["Rollflag"].ToString() == "0")
                {
                    e.Row.Cells[1].Visible = false;
                }

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }
}



