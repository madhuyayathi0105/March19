using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using AjaxControlToolkit;
using System.Configuration;
using System.Drawing;
using wc = System.Web.UI.WebControls;
using System.Text;
using InsproDataAccess;
using System.Globalization;

public partial class Cam_Internal_Mark_Calculation : System.Web.UI.Page
{
    #region "Variables"

    ArrayList forschoolcheck = new ArrayList();

    bool ShowGradeDetails = false;
    bool chk_flag = false;
    bool splhr_flag = false;
    bool dateflag = false;
    bool savefalg = false;
    static bool forschoolsetting = false;
    string collegeCode = string.Empty;
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    InsproDirectAccess dir = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    ReuasableMethods rs = new ReuasableMethods();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    DataTable dtCommon = new DataTable();

    static string subjectno = string.Empty;
    static string grouporusercode = string.Empty;
    static string syllcode = string.Empty;

    string Covertvalue = string.Empty;
    string criname = string.Empty;
    string Testname = string.Empty;
    string controlatt = string.Empty;
    string strorder = string.Empty;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string order = string.Empty;
    string sem_start_date = string.Empty;
    string strDay = string.Empty;
    string dummy_date = string.Empty;
    string temp_hr_field = string.Empty;
    string full_hour = string.Empty;
    string single_hour = string.Empty;
    string date_temp_field = string.Empty;
    string month_year = string.Empty;
    string groupusercodevalue = string.Empty;
    string tempvalue = "-1";

    string[] spitcamcount56;
    string[] spiltcamvalue;
    string[] spiltcalcount;
    string[] spiltcalvalue;

    int subcount = 0;
    int top = 10;
    int subtop = 10;
    int subid = 0;
    int id = 0;
    int calid = 0;
    int calid1 = 0;
    int roundid = 0;
    int objvalue = 0;
    int count_master = 0;
    int split_holiday_status_1 = 0;
    int split_holiday_status_2 = 0;
    int mng_hrs = 0;
    int evng_hrs = 0;
    int span_count = 0;
    int no_of_hrs = 0;
    int count = 0;

    //Criteria Controls
    RadioButton rbvtotal;
    RadioButton rbvbest;
    RadioButton rbvAveragebest;
    RadioButton rbvindividual;
    RadioButton rbvsetting;

    TextBox txtsettings;
    TextBox txtcalcu;
    TextBox txtaveragebest;
    TextBox txtconvert;
    TextBox txtcriterianame;
    TextBox txtcalconvert;

    Label lbl;
    Label lblcalname;

    Panel pan;

    CheckBox chkSubSubject;
    Panel panSubSubject;
    CheckBoxList cblSubSubject;

    //Calculate Controls
    CheckBox chkcalcriteria;
    CheckBox chkcalculation;
    CheckBox chkcalother;
    CheckBox chkinmarkcalset;
    CheckBox chksub;

    FilteredTextBoxExtender ftbeavg;
    RequiredFieldValidator rfvcam;

    DataSet ds_sphr = new DataSet();
    DataSet dssubject = new DataSet();
    DataSet dsgetdetails = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    DataSet ds1 = new DataSet();

    static Hashtable has_subtype = new Hashtable();
    static Hashtable ht_sphr = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable stdpresnt = new Hashtable();
    Hashtable stdconducted = new Hashtable();
    Hashtable has_hs = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable htCamCalculationInsert = new Hashtable();
    Hashtable htCamFinalInternalInsert = new Hashtable();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;

    //DataSet ds_alter = new DataSet();

    double spl_per_per_hrs = 0;
    double spl_tol_per_hrs = 0;
    double tot_hr = 0;

    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];

    DateTime dt2 = new DateTime();
    DateTime temp_date = new DateTime();


    Dictionary<int, string> dicval = new Dictionary<int, string>();
    static Dictionary<int, string> dicheader = new Dictionary<int, string>();
    DataTable dtsub = new DataTable();
    DataRow dr;
    DataTable dtview = new DataTable();
    DataRow drview;
    DataTable dtstnview = new DataTable();
    DataRow drstngview;
    static int gridview2colcount = 0;
    Dictionary<string, string> dicbtnval = new Dictionary<string, string>();
    bool enableflag;

    static int colvalchk = 0;
    System.Text.StringBuilder internalmarkname = new System.Text.StringBuilder();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            collegecode = Session["collegecode"].ToString();
            string grouporusercodeNew = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                grouporusercode = " group_code='" + group_user.ToString().Trim() + "'";
                groupusercodevalue = " group_code='" + group_user.ToString().Trim() + "'";
                grouporusercodeNew = " and group_code='" + group_user.ToString().Trim() + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
                groupusercodevalue = " user_code='" + Session["usercode"].ToString().Trim() + "'";
                grouporusercodeNew = " and usercode='" + Session["usercode"].ToString().Trim() + "'";
            }

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' " + grouporusercodeNew + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = da.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblsem.Text = "Term";
                    Label5.Text = "Test Mark Calculation      ";
                    lblcriteria.Text = "No of Test Criteria";
                    ckhdegreewise.Text = "Standard Wise";
                    if (ckhdegreewise.Checked == true)
                    {
                        setwidth.Attributes.Add("style", "width:97px;");

                    }
                    else
                    {
                        setwidth.Attributes.Add("style", "width:5px;");
                    }
                    lbledulevel.Visible = false;
                    ddledulevel.Visible = false;

                }
                else
                {
                    forschoolsetting = false;
                }
            }
            else
            {
                forschoolsetting = false;
            }

            if (IsPostBack)
            {
                //
                savefalg = false;
                string buttonok = String.Empty;
                string spread = "";
                Control control = null;
                string ctrlname = Page.Request.Params["__EVENTTARGET"];
                if (ctrlname != null && ctrlname != String.Empty)
                {
                    control = Page.FindControl(ctrlname);
                    spread = ctrlname.ToString();
                }
                else
                {
                    string ctrlStr = String.Empty;
                    Control c = null;
                    foreach (string ctl in Page.Request.Form)
                    {
                        if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                        {
                            ctrlStr = ctl.Substring(0, ctl.Length - 2);
                            c = Page.FindControl(ctrlStr);
                        }
                        else
                        {
                            c = Page.FindControl(ctl);
                            buttonok = ctl;
                        }
                        if (c is System.Web.UI.WebControls.Button ||
                                 c is System.Web.UI.WebControls.ImageButton)
                        {
                            control = c;
                            break;
                        }
                    }
                }

                string buttonstaus = "";
                if (buttonok != "")
                {
                    string[] button = buttonok.Split('$');
                    if (button.Length > 1)
                        buttonstaus = button[2].ToString().Trim();
                }
                string spreadname = "";
                if (spread != "")
                {
                    string[] spiltspreadname = spread.Split('$');
                    if (spiltspreadname.GetUpperBound(0) > 1)
                    {
                        spreadname = spiltspreadname[2].ToString().Trim();
                        controlatt = spreadname;
                        buttonok = "1";
                        if (spreadname.ToString().Trim() != "txtfromdate" || spreadname.ToString().Trim() != "txttomdate")
                        {
                            dateflag = true;
                        }
                        if (spreadname.ToString().Trim() != "GridView1" && spreadname.ToString().Trim() != "Printcontrol" && spreadname.ToString().Trim() != "GridView2")
                        {
                            loadcontrols();
                            chk_flag = true;
                        }
                        if (spreadname.ToString().Trim() == "GridView1")
                        {
                            dateflag = false;
                        }
                    }
                }
                if (buttonstaus == "btnsave" || buttonok.ToString().Trim() == "")
                {
                    controlatt = "btnsave";
                    dateflag = true;
                    savefalg = true;
                    loadcontrols();
                    chk_flag = true;
                }

            }
            if (!IsPostBack)
            {
                chkRound100.Visible = false;
                Txtround.Visible = false;
                Panel2.Visible = false;
                chkRound100.Checked = true;



                lblcriteria.Visible = false;
                txtcriteria.Visible = false;
                lblcalulate.Visible = false;
                txtcalculate.Visible = false;
                chkattendance.Visible = false;
                chkSubSub.Visible = false;
                btncriteria.Visible = false;
                btnAttenSetting.Visible = false;
                chkbasedSettings.Visible = false;
                rbvoverall.Checked = true;
                rbvattpercentage.Checked = true;
                errmsg.Visible = false;
                GridView1.Visible = false;
                chkattsem.Visible = false;
                lblfromdate.Visible = false;
                txtfromdate.Visible = false;
                lbltodate.Visible = false;
                txttodate.Visible = false;
                rbvoverall.Visible = false;
                rbvsubjectwise.Visible = false;
                rbvattmaxmark.Visible = false;
                rbvattpercentage.Visible = false;
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                loadedulevel();
                examyear();
                btncolor1.Visible = false;
                Button2.Visible = false;
                Button1.Visible = false;
                Label2.Visible = false;
                Label4.Visible = false;
                Label6.Visible = false;

                btnxl.Visible = false;
                divPrint.Visible = false;
                txtreport.Visible = false;
                lblreportname.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";

                if (Session["usercode"] != "")
                {
                    string Master1 = "";
                    Master1 = "select * from Master_Settings where " + grouporusercode + "";
                    ds.Reset();
                    ds.Dispose();
                    ds = da.select_method(Master1, hat, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int set = 0; set < ds.Tables[0].Rows.Count; set++)
                        {
                            string strdayflag = ds.Tables[0].Rows[set]["settings"].ToString();
                            string value = ds.Tables[0].Rows[set]["value"].ToString();

                            if (strdayflag.Trim() == "Roll No" && value.Trim() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (strdayflag.Trim() == "Register No" && value.Trim() == "1")
                            {
                                Session["Regflag"] = "1";
                            }

                        }
                    }
                }
                if (Session["Staff_Code"] != null && Session["Staff_Code"] != "")
                {
                    ckhdegreewise.Checked = true;
                    ckhdegreewise.Visible = false;
                }
                else
                {
                    ckhdegreewise.Checked = false;
                    ckhdegreewise.Visible = true;
                }
                loadsettings();
                // Added By Sridharan 12 Mar 2015
                //{


                //} Sridharan
            }
        }
        catch (Exception ex) { }
    }

    public void loadsettings()
    {
        if (ckhdegreewise.Checked == true)
        {
            lblbatch.Visible = true;
            lblbranch.Visible = true;
            lbldegree.Visible = true;
            lblsec.Visible = true;
            lblsem.Visible = true;
            ddlbatch.Visible = true;
            ddldegree.Visible = true;
            ddlbranch.Visible = true;
            ddlsem.Visible = true;
            ddlsec.Visible = true;
            lbledulevel.Visible = false;
            ddledulevel.Visible = false;

            bindbatch();
            binddegree();
            if (ddldegree.Items.Count > 0)
            {
                bindbranch();
                bindsem();
                bindsec();
                BindSubType();
                bindSubject();
                ddlbatch.Enabled = true;
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsem.Enabled = true;
                ddlsec.Enabled = true;
            }
            else
            {
                ddlbatch.Enabled = false;
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsem.Enabled = false;
                ddlsec.Enabled = false;
            }
        }
        else
        {
            BindSubType();
            bindSubject();
            lblbatch.Visible = false;
            lblbranch.Visible = false;
            lblsec.Visible = false;
            lblsem.Visible = false;
            lbldegree.Visible = false;
            ddlbatch.Visible = false;
            ddldegree.Visible = false;
            ddlbranch.Visible = false;
            ddlsem.Visible = false;
            ddlsec.Visible = false;
            lbledulevel.Visible = true;
            ddledulevel.Visible = true;
        }
        GridView1.Visible = false;
        GridView2.Visible = false;
        panel4.Visible = false;
        lblcriteria.Visible = false;
        txtcriteria.Visible = false;
        lblcalulate.Visible = false;
        txtcalculate.Visible = false;
        chkattendance.Visible = false;
        chkSubSub.Visible = false;
        btncriteria.Visible = false;
        btnAttenSetting.Visible = false;
        chkbasedSettings.Visible = false;
        errmsg.Visible = false;

    }

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = da.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
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
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("course_id", ddldegree.SelectedValue);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);

        ds = da.select_method("bind_branch", has, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    public void bindsem()
    {
        errmsg.Visible = false;
        ddlsem.Items.Clear();
        string duration = "";
        Boolean first_year = false;
        has.Clear();
        collegecode = Session["collegecode"].ToString();
        has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("college_code", collegecode);
        ds = da.select_method("bind_sem", has, "sp");
        int count3 = ds.Tables[0].Rows.Count;
        if (count3 > 0)
        {
            ddlsem.Enabled = true;
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
                ddlsem.Enabled = true;
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
                ddlsem.Enabled = false;
            }
        }

    }

    public void bindsec()
    {
        GridView1.Visible = false;
        GridView2.Visible = false;
        panel4.Visible = false;
        lblcriteria.Visible = false;
        txtcriteria.Visible = false;
        lblcalulate.Visible = false;
        txtcalculate.Visible = false;
        chkattendance.Visible = false;
        chkSubSub.Visible = false;
        btncriteria.Visible = false;
        btnAttenSetting.Visible = false;
        chkbasedSettings.Visible = false;
        errmsg.Visible = false;
        btncolor1.Visible = false;
        Button2.Visible = false;
        Button1.Visible = false;
        Label2.Visible = false;
        Label4.Visible = false;
        Label6.Visible = false;
        ddlsec.Items.Clear();
        has.Clear();
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("degree_code", ddlbranch.SelectedValue);
        ds = da.select_method("bind_sec", has, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Enabled = true;
            ddlsec.Items.Add("All");
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    public void examyear()
    {
        string strexamyear = "select distinct Exam_year from exam_details order by Exam_year";
        ds.Reset();
        ds.Dispose();
        ds = da.select_method(strexamyear, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlexamyear.DataSource = ds;
            ddlexamyear.DataTextField = "Exam_year";
            ddlexamyear.DataValueField = "Exam_year";
            ddlexamyear.DataBind();
        }
    }

    public void loadedulevel()
    {
        try
        {
            string streduleve = "SELECT DISTINCT Edu_Level from course where college_code=" + collegecode + "";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method(streduleve, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddledulevel.DataSource = ds;
                ddledulevel.DataTextField = "Edu_Level";
                ddledulevel.DataValueField = "Edu_Level";
                ddledulevel.DataBind();
                ddledulevel.Items.Insert(0, "All");
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindSubType()
    {
        try
        {
            txtSubtype.Text = "--Select--";
            cblSubtype.Items.Clear();
            chkSubtype.Checked = false;
            string eduLevel = Convert.ToString(ddledulevel.SelectedItem.Text);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string degCode = Convert.ToString(ddlbranch.SelectedValue);
            string sem = Convert.ToString(ddlsem.SelectedValue);
            string colCode = Convert.ToString(Session["collegecode"]);
            DataTable dtSubType = new DataTable();
            string SelectQ = string.Empty;
            string eduLeve = string.Empty;
            if (eduLevel.Trim().ToLower() == "all")
                eduLeve = "";
            else
                eduLeve = "   and c.edu_level='" + eduLevel + "'";
            if (!ckhdegreewise.Checked)
            {
                SelectQ = "select distinct subject_type from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "  " + eduLeve + "  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by subject_type";
            }
            else
            {
                SelectQ = "select distinct subject_type from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "  and r.degree_code='" + degCode + "' and r.Batch_Year='" + batchYear + "' and r.Current_Semester='" + sem + "'   and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by subject_type";
            }
            dtSubType = dir.selectDataTable(SelectQ);
            if (dtSubType.Rows.Count > 0)
            {
                cblSubtype.DataSource = dtSubType;
                cblSubtype.DataTextField = "subject_type";
                cblSubtype.DataValueField = "subject_type";
                cblSubtype.DataBind();
                checkBoxListselectOrDeselect(cblSubtype, true);
                CallCheckboxListChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
            }

        }
        catch
        {
        }
    }

    public void bindSubject()
    {
        txtSubject.Text = "--Select--";
        cblSubject.Items.Clear();
        cbSubjet.Checked = false;
        string eduLevel = Convert.ToString(ddledulevel.SelectedItem.Text);
        string batchYear = Convert.ToString(ddlbatch.SelectedValue);
        string degCode = Convert.ToString(ddlbranch.SelectedValue);
        string sem = Convert.ToString(ddlsem.SelectedValue);
        string colCode = Convert.ToString(Session["collegecode"]);
        string subtype = string.Empty;
        if (cblSubtype.Items.Count > 0)
            subtype = rs.getCblSelectedText(cblSubtype);

        string selectQ = string.Empty;
        string eduLeve = string.Empty;
        if (eduLevel.Trim().ToLower() == "all")
            eduLeve = "";
        else
            eduLeve = "   and c.edu_level='" + eduLevel + "'";
        if (!ckhdegreewise.Checked)
        {
            selectQ = "select distinct subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "   " + eduLeve + " and sem.subject_type in('" + subtype + "')  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by subject_name,s.subject_code";
        }
        else
        {
            selectQ = "select distinct subject_name,s.subject_code,CONVERT(nvarchar(max),subject_code)+'-'+CONVERT(nvarchar(max),subject_name) as subName from registration r,subject s,syllabus_master sm,Sub_sem Sem,degree d,course c  where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + colCode + "  and r.degree_code='" + degCode + "' and r.Batch_Year='" + batchYear + "' and r.Current_Semester='" + sem + "'   and  sem.subject_type in('" + subtype + "') and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  order by subject_name";
        }
        DataTable dtSubType = dir.selectDataTable(selectQ);
        if (dtSubType.Rows.Count > 0)
        {
            cblSubject.DataSource = dtSubType;
            cblSubject.DataTextField = "subName";
            cblSubject.DataValueField = "subject_code";
            cblSubject.DataBind();
            checkBoxListselectOrDeselect(cblSubject, true);
            CallCheckboxListChange(cbSubjet, cblSubject, txtSubject, lblSubject.Text, "--Select--");
        }

    }

    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        BindSubType();
        bindSubject();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        binddegree();
        bindbranch();
        bindsem();
        bindsec();
        BindSubType();
        bindSubject();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        BindSubType();
        bindSubject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        bindsem();
        bindsec();
        BindSubType();
        bindSubject();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        bindsec();
        BindSubType();
        bindSubject();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        panel4.Visible = false;
        lblcriteria.Visible = false;
        txtcriteria.Visible = false;
        lblcalulate.Visible = false;
        txtcalculate.Visible = false;
        chkattendance.Visible = false;
        chkSubSub.Visible = false;
        btncriteria.Visible = false;
        btnAttenSetting.Visible = false;
        chkbasedSettings.Visible = false;
        errmsg.Visible = false;
        btncolor1.Visible = false;
        Button2.Visible = false;
        Button1.Visible = false;
        Label2.Visible = false;
        Label4.Visible = false;
        Label6.Visible = false;
    }

    protected void chkdegreewise_Checkedchange(object sender, EventArgs e)
    {
        loadsettings();
        //Added by Sridharan R 13 March 2015
        //{
        if (forschoolsetting == true)
        {
            if (ckhdegreewise.Checked == true)
            {
                setwidth.Attributes.Add("style", "width:93px;");

            }
            else
            {
                setwidth.Attributes.Add("style", "width:5px;");
            }
            lbledulevel.Visible = false;
            ddledulevel.Visible = false;
        }
        //} Sridharan end
    }

    protected void chkattsem_CheckedChanged(object sender, EventArgs e)
    {
        if (chkattsem.Checked == true)
        {
            txtfromdate.Enabled = false;
            txttodate.Enabled = false;
            chkbasedSettings.Checked = false;
        }
        else
        {
            txtfromdate.Enabled = true;
            txttodate.Enabled = true;
            //chkbasedSettings.Checked = true;
        }
    }

    protected void chkbasedSettings_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbasedSettings.Checked == true)
        {

            txtfromdate.Enabled = false;
            txttodate.Enabled = false;
            chkattsem.Checked = false;
            chkattsem.Enabled = false;
        }
        else
        {
            txtfromdate.Enabled = true;
            txttodate.Enabled = true;
            chkattsem.Checked = true;
            chkattsem.Enabled = true;
        }
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {

        string[] spitdate = txtfromdate.Text.ToString().Split('/');
        DateTime from = Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]);
        string[] splitto = txttodate.Text.ToString().Split('/');
        DateTime to = Convert.ToDateTime(splitto[1] + '/' + splitto[0] + '/' + splitto[2]);
        string now = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime nowdate = Convert.ToDateTime(now);
        if (from > nowdate)
        {
            txtfromdate.Text = nowdate.ToString();
            errmsg.Visible = true;
            errmsg.Text = "Please Less Than Current Date";
        }
        if (from > to)
        {
            txtfromdate.Text = txttodate.Text.ToString();
            errmsg.Visible = true;
            errmsg.Text = "Please Enter From Date Lesser Than To Date";
        }
        else
        {
            errmsg.Visible = false;
            errmsg.Text = "";
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        string[] spitdate = txtfromdate.Text.ToString().Split('/');
        DateTime from = Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]);
        string[] splitto = txttodate.Text.ToString().Split('/');
        DateTime to = Convert.ToDateTime(splitto[1] + '/' + splitto[0] + '/' + splitto[2]);
        string now = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime nowdate = Convert.ToDateTime(now);
        if (to > nowdate)
        {
            txttodate.Text = nowdate.ToString();
            errmsg.Visible = true;
            errmsg.Text = "Please Less Than Current Date";
        }
        if (from > to)
        {
            txtfromdate.Text = txttodate.Text.ToString();
            errmsg.Visible = true;
            errmsg.Text = "Please Enter To Date Greater Than From Date";
        }
        else
        {
            errmsg.Visible = false;
            errmsg.Text = "";
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtToDate1.Text))
        {
            string[] spitdate = txtFromDate1.Text.ToString().Split('/');
            DateTime from = Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]);
            string[] splitto = txtToDate1.Text.ToString().Split('/');
            DateTime to = Convert.ToDateTime(splitto[1] + '/' + splitto[0] + '/' + splitto[2]);
            string now = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime nowdate = Convert.ToDateTime(now);
            if (from > nowdate)
            {
                txtfromdate.Text = nowdate.ToString();
                lblError.Visible = true;
                lblError.Text = "Please Less Than Current Date";
            }
            if (from > to)
            {
                txtfromdate.Text = txttodate.Text.ToString();
                lblError.Visible = true;
                lblError.Text = "Please Enter From Date Lesser Than To Date";
            }
            else
            {
                lblError.Visible = false;
                lblError.Text = "";
            }
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        string[] spitdate = txtFromDate1.Text.ToString().Split('/');
        DateTime from = Convert.ToDateTime(spitdate[1] + '/' + spitdate[0] + '/' + spitdate[2]);
        string[] splitto = txtToDate1.Text.ToString().Split('/');
        DateTime to = Convert.ToDateTime(splitto[1] + '/' + splitto[0] + '/' + splitto[2]);
        string now = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime nowdate = Convert.ToDateTime(now);
        if (to > nowdate)
        {
            txttodate.Text = nowdate.ToString();
            lblError.Visible = true;
            lblError.Text = "Please Less Than Current Date";
        }
        if (from > to)
        {
            txtfromdate.Text = txttodate.Text.ToString();
            lblError.Visible = true;
            lblError.Text = "Please Enter To Date Greater Than From Date";
        }
        else
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            dicbtnval.Clear();

            dtsub.Clear();
            dtsub.Columns.Add("degree");
            dtsub.Columns.Add("subject_name");
            dtsub.Columns.Add("subject_no");
            dtsub.Columns.Add("syll_code");
            dtsub.Columns.Add("Sections");
            ds.Dispose();
            ds = da.select_method("select * from sysobjects where name='tbl_Cam_Calculation' and Type='U'", hat, "text ");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int q = da.insert_method("drop table tbl_Cam_Calculation", hat, "text");
                //int p = da.insert_method("create table tbl_Cam_Calculation (subject_no int,syll_code int,Istype nvarchar(25),Cam_option int,roll_no nvarchar(50),Exammark nvarchar(50),conversion int)", hat, "text");
            }
            else
            {
                int p = da.insert_method("create table tbl_Cam_Calculation (subject_no int,syll_code int,Istype nvarchar(25),Cam_option int,roll_no nvarchar(50),Exammark nvarchar(50),conversion int)", hat, "text");
            }
            btnxl.Visible = false;
            divPrint.Visible = false;
            txtreport.Visible = false;
            lblreportname.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            errmsg.Visible = false;
            GridView2.Visible = false;
            //Fpinternal.Sheets[0].AutoPostBack = false;
            //Fpinternaldetails.Visible = false;
            panel4.Visible = false;

            string stredulevelvalue = ddledulevel.SelectedValue.ToString();
            if (stredulevelvalue == "All")
            {
                stredulevelvalue = "";
            }
            else
            {
                stredulevelvalue = "and c.edu_level in ('" + stredulevelvalue + "')";
            }
            if (groupusercodevalue != "")
            {
                groupusercodevalue = "and dep." + groupusercodevalue + "";
            }
            string columnvalue = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string groupuser = Session["group_code"].ToString();
                if (groupuser.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    groupuser = group_semi[0].ToString();
                }
                columnvalue = " group_code ='" + groupuser + "'";
            }
            else
            {
                columnvalue = " user_code='" + Session["usercode"].ToString().Trim() + "'";
            }
            Hashtable hatuser = new Hashtable();
            string gropuquery = " select * from deptprivilages where " + columnvalue + "";
            DataSet dsus = da.select_method_wo_parameter(gropuquery, "Text");
            for (int uc = 0; uc < dsus.Tables[0].Rows.Count; uc++)
            {
                if (!hatuser.Contains(dsus.Tables[0].Rows[uc]["degree_code"].ToString()))
                {
                    hatuser.Add(dsus.Tables[0].Rows[uc]["degree_code"].ToString(), dsus.Tables[0].Rows[uc]["degree_code"].ToString());
                }
            }
            string strvaluecheckquery = "select Istype,s.subject_no,sy.syll_code, isnull(t.sections,'') as sections from internal_cam_calculation_master_setting t,subject s,syllabus_master sy,Degree d,Course c where s.subject_no=t.subject_no and s.syll_code=sy.syll_code and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + stredulevelvalue + "";
            strvaluecheckquery = strvaluecheckquery + " select distinct Istype,s.subject_no,sy.syll_code,isnull(t.sections,'') as sections from tbl_cam_calculation t,subject s,syllabus_master sy,Degree d,Course c where s.subject_no=t.subject_no and s.syll_code=sy.syll_code and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + stredulevelvalue + "";
            //string strsubjectquery = "select distinct r.batch_year,r.degree_code,c.course_Name,d.acronym,r.current_semester ,subject_name,s.subject_no,";
            //strsubjectquery = strsubjectquery + " s.syll_code from registration r,subject s,subjectchooser sc,syllabus_master sm,Sub_sem Sem,degree d,course c,deptprivilages dep  ";
            //strsubjectquery = strsubjectquery + " where r.current_semester=sc.semester and r.Roll_No=sc.roll_no and r.degree_code=sm.degree_code and r.Batch_Year=sm.Batch_Year";
            //strsubjectquery = strsubjectquery + " and r.Current_Semester=sm.semester and s.subject_no=sc.subject_no and s.subType_no=sc.subtype_no and s.syll_code=sm.syll_code";
            //strsubjectquery = strsubjectquery + " and s.subType_no=sem.subType_no and s.syll_code=sem.syll_code	and sc.subtype_no=sem.subType_no and sem.syll_code=sm.syll_code ";
            //strsubjectquery = strsubjectquery + " and d.degree_code=r.degree_code and d.course_id=c.course_id and promote_count=1 and c.college_code=" + collegecode + " and dep.degree_code=r.degree_code ";
            //strsubjectquery = strsubjectquery + " " + stredulevelvalue + " " + groupusercodevalue + " and dep.degree_code=d.Degree_Code and sm.degree_code=dep.degree_code and r.college_code=c.college_code	and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by r.batch_year,r.degree_code";
            string strsubjectquery = "select distinct (str(r.batch_year)+'-'+c.course_Name+'['+d.acronym+']'+'-'+str(r.current_semester)) as degree,c.Course_Name,r.Current_Semester,r.degree_code,d.Acronym ,subject_name,s.subject_no,s.syll_code,r.batch_year,isnull(r.Sections,'') as Sections,subject_code from registration r,subject s,subjectchooser sc,syllabus_master sm,Sub_sem Sem,degree d,course c ";
            strsubjectquery = strsubjectquery + " where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no";
            strsubjectquery = strsubjectquery + " and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + collegecode + " " + stredulevelvalue + "  and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by r.batch_year desc,r.degree_code,r.Current_Semester asc, Sections";

            if (ckhdegreewise.Checked == true)
            {
                string section = "";
                if (ddlsec.SelectedValue.ToString().Trim() != "" && ddlsec.SelectedValue.ToString().Trim().ToLower() != "all" && ddlsec.SelectedValue.ToString().Trim() != "-1" && ddlsec.Enabled == true)
                {
                    section = " and st.sections='" + ddlsec.SelectedItem.ToString() + "'";
                    if (Session["Staff_Code"] == null || Session["Staff_Code"] == "")
                    {
                        section = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
                    }
                }
                else
                {
                    section = "";
                }
                string batch = ddlbatch.SelectedItem.ToString();
                string degree = ddlbranch.SelectedValue.ToString();
                string sem = ddlsem.SelectedItem.ToString();


                if (Session["Staff_Code"] == null || Session["Staff_Code"] == "")
                {
                    strsubjectquery = "select distinct S.subject_no,subject_name,s.syll_code,sm.batch_year,sm.degree_code,r.current_semester,de.acronym,c.course_name,ltrim(rtrim(isnull(r.sections,''))) as Sections,subject_code from subject as S,syllabus_master  as SM,";
                    strsubjectquery = strsubjectquery + " subjectchooser as SC,Sub_sem as Sem,staff_selector st,course c,department d,degree de,registration r where S.subject_no=SC.Subject_no and  ";
                    strsubjectquery = strsubjectquery + " s.syll_code=SM.syll_code and r.degree_code=de.degree_code and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year";
                    strsubjectquery = strsubjectquery + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and d.dept_code=de.dept_code and de.course_id=c.course_id and r.current_semester=sm.semester ";
                    strsubjectquery = strsubjectquery + "and SM.degree_code=" + degree + " and SM.Semester=" + sem + " and  sc.roll_no =r.Roll_No and st.subject_no=s.subject_no  and  SM.batch_year=" + batch + " " + section + " ";
                    strsubjectquery = strsubjectquery + " and S.subtype_no = Sem.subtype_no and promote_count=1 order by sm.batch_year,sm.degree_code,r.current_semester,de.acronym,c.course_name,Sections, subject_name ";
                }
                else
                {
                    string staffcode = "'" + Session["Staff_Code"].ToString() + "'";
                    strsubjectquery = "select distinct S.subject_no,subject_name,s.syll_code,sm.batch_year,sm.degree_code,r.current_semester ,de.acronym,c.course_name,ltrim(rtrim(isnull(r.sections,''))) as Sections,subject_code from subject as S,syllabus_master  as SM,";
                    strsubjectquery = strsubjectquery + " Sub_sem as Sem,staff_selector st,course c,department d,degree de,registration r where ";
                    strsubjectquery = strsubjectquery + " s.syll_code=SM.syll_code and r.degree_code=de.degree_code and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year";
                    strsubjectquery = strsubjectquery + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and d.dept_code=de.dept_code and de.course_id=c.course_id and r.current_semester=sm.semester ";
                    strsubjectquery = strsubjectquery + "and SM.degree_code=" + degree + " and SM.Semester=" + sem + " and st.subject_no=s.subject_no  and  SM.batch_year=" + batch + " and ";
                    strsubjectquery = strsubjectquery + " S.subtype_no = Sem.subtype_no and promote_count=1 and staff_code=" + staffcode + " and isnull(st.sections,'')=isnull(r.sections,'') " + section + " order by  sm.batch_year,sm.degree_code,r.current_semester,de.acronym,c.course_name,Sections,s.subject_name";  //modified by Mullai(isnull)  and  sc.roll_no =r.Roll_No
                }

                strvaluecheckquery = "select Istype,s.subject_no,sy.syll_code,isnull(t.sections,'') as sections from internal_cam_calculation_master_setting t,subject s,syllabus_master sy,Degree d,Course c where s.subject_no=t.subject_no and s.syll_code=sy.syll_code and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.degree_code=" + degree + " and sy.Semester=" + sem + "  and sy.batch_year=" + batch + "";
                strvaluecheckquery = strvaluecheckquery + " select distinct Istype,s.subject_no,sy.syll_code, isnull(t.sections,'') as sections from tbl_cam_calculation t,subject s,syllabus_master sy,Degree d,Course c where s.subject_no=t.subject_no and s.syll_code=sy.syll_code and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.degree_code=" + degree + " and sy.Semester=" + sem + "  and sy.batch_year=" + batch + "";
            }
            //string strsubjectquery = "select distinct r.batch_year,r.degree_code,c.course_Name,d.acronym,r.current_semester ,subject_name,s.subject_no,s.syll_code from registration r,subject s,subjectchooser sc,syllabus_master sm,Sub_sem Sem,degree d,course c ";
            //strsubjectquery = strsubjectquery + " where d.degree_code=r.degree_code and d.course_id=c.course_id and r.degree_code=sm.degree_code and r.batch_year=sm.batch_year and r.current_semester=sm.semester and sm.syll_code=s.syll_code and s.subject_no=sc.subject_no and r.roll_no=sc.roll_no";
            //strsubjectquery = strsubjectquery + "  and  s.syll_code=SM.syll_code  and  S.subtype_no = Sem.subtype_no and promote_count=1 and c.college_code=" + collegecode + " " + stredulevelvalue + "   and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' order by r.batch_year,r.degree_code ";

            //ds.Reset();
            //ds.Dispose();

            string subjectCode = string.Empty;
            if (cblSubject.Items.Count > 0)
                subjectCode = rs.getCblSelectedValue(cblSubject);

            DataSet dssetval = da.select_method_wo_parameter(strvaluecheckquery, "Text");
            string val = string.Empty;
            ds.Clear();
            ds = da.select_method(strsubjectquery, hat, "Text");
            if (ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int sr = 0;
                    int countcheck = 0;
                    int dicct = 0;
                    for (int i = 0; ds.Tables[0].Rows.Count > i; i++)
                    {
                        sr++;
                        string degree = ds.Tables[0].Rows[i]["batch_year"].ToString() + '-' + ds.Tables[0].Rows[i]["course_Name"].ToString() + '[' + ds.Tables[0].Rows[i]["acronym"].ToString() + ']' + '-' + ds.Tables[0].Rows[i]["current_semester"].ToString();
                        string subject = ds.Tables[0].Rows[i]["subject_name"].ToString();
                        string subject_no = ds.Tables[0].Rows[i]["subject_no"].ToString();
                        string syll_code = ds.Tables[0].Rows[i]["syll_code"].ToString();
                        string suCode = Convert.ToString(ds.Tables[0].Rows[i]["subject_code"]);//
                        if (subjectCode.Contains(suCode))
                        {
                            string strsecval = "";
                            string setction = ds.Tables[0].Rows[i]["Sections"].ToString();
                            if (setction.Trim() != "" && setction.Trim() != "-1" && setction.Trim() != "0")
                            {
                                degree = degree + " - " + setction;
                                strsecval = " and Sections='" + setction + "'";
                            }
                            else
                            {
                                setction = "";
                            }
                            bool setflage = false; // added by jairam 01-07-2015 
                            if (Session["UserName"].ToString().Trim() != "admin")
                            {
                                string getdate = da.GetFunction("select  LockDate from InsLockSettings where Batch_Year ='" + Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]) + "' and Degree_Code='" + Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]) + "' and Semester='" + Convert.ToString(ds.Tables[0].Rows[i]["current_semester"]) + "'  and SettingType=1 ");
                                if (getdate.Trim() != "" && getdate.Trim() != "0")
                                {
                                    if (Convert.ToDateTime(getdate) < DateTime.Today)
                                    {
                                        setflage = true;
                                    }
                                }
                            }
                            Boolean allow = true;
                            if (ckhdegreewise.Checked == false)
                            {
                                if (!hatuser.Contains(ds.Tables[0].Rows[i]["degree_code"].ToString()))
                                {
                                    allow = false;
                                }
                            }


                            if (allow == true)
                            {
                                dicct++;
                                dr = dtsub.NewRow();

                                dr["degree"] = degree;
                                dr["Sections"] = setction;
                                dssetval.Tables[0].DefaultView.RowFilter = "subject_no=" + subject_no + " and syll_code=" + syll_code + " " + strsecval + "";
                                DataView dvsetva = dssetval.Tables[0].DefaultView;
                                if (dvsetva.Count > 0)
                                {

                                    dssetval.Tables[1].DefaultView.RowFilter = "subject_no=" + subject_no + " and syll_code=" + syll_code + " " + strsecval + "";
                                    DataView dvsetva1 = dssetval.Tables[1].DefaultView;
                                    if (dvsetva1.Count > 0)
                                    {
                                        val = "3";
                                        dicbtnval.Add(Convert.ToString(dicct), val);
                                        //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.Color.AntiqueWhite;
                                        //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 6].CellType = btnview;
                                    }
                                    else
                                    {
                                        val = "2";
                                        dicbtnval.Add(Convert.ToString(dicct), val);
                                    }
                                    // }
                                }
                                else
                                {
                                    val = "1";
                                    dicbtnval.Add(Convert.ToString(dicct), val);
                                }
                                dr["subject_name"] = subject;
                                dr["subject_no"] = subject_no;
                                dr["syll_code"] = syll_code;

                                if (setflage == true) // added by jairam 01-07-2015
                                {
                                    //countcheck++;
                                    //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 3].Locked = true;
                                    //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 4].Locked = true;
                                    //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 5].Locked = true;
                                    //Fpinternal.Sheets[0].Cells[Fpinternal.Sheets[0].RowCount - 1, 7].Locked = true;
                                }


                            }
                            dtsub.Rows.Add(dr);
                        }
                    }
                    colvalchk = 0;
                    GridView1.DataSource = dtsub;
                    GridView1.DataBind();

                    for (int i = GridView1.Rows.Count - 1; i > 0; i--)
                    {
                        GridViewRow row = GridView1.Rows[i];
                        GridViewRow previousRow = GridView1.Rows[i - 1];
                        for (int j = 1; j < row.Cells.Count; j++)
                        {
                            if (j == 1)
                            {
                                Label ct2 = GridView1.Rows[i].Cells[j].FindControl("lbldegree") as Label;
                                string ct3 = ct2.Text;
                                Label ct21 = GridView1.Rows[i - 1].Cells[j].FindControl("lbldegree") as Label;
                                string ct31 = ct21.Text;
                                if (ct3 == ct31)
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
                    if (dicbtnval.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> dicval1 in dicbtnval)
                        {
                            string rowct2 = dicval1.Value;
                            string vsblval = dicval1.Key;
                            if (vsblval == "1")
                            {

                            }
                        }
                    }

                    btncolor1.Visible = true;
                    Button2.Visible = true;
                    Button1.Visible = true;
                    Label2.Visible = true;
                    Label4.Visible = true;
                    Label6.Visible = true;
                    GridView1.Visible = true;
                    lblcriteria.Visible = true;
                    txtcriteria.Visible = true;
                    lblcalulate.Visible = true;
                    txtcalculate.Visible = true;
                    chkattendance.Visible = true;
                    chkSubSub.Visible = true;
                    btncriteria.Visible = true;
                    btnAttenSetting.Visible = true;
                    chkbasedSettings.Visible = true;
                    btnAttenSetting.Visible = true;
                    chkRound100.Visible = true;
                    chkRound100.Checked = true;
                    Txtround.Visible = true;
                    Panel2.Visible = true;

                }
                else
                {
                    GridView1.Visible = false;
                    errmsg.Visible = true;
                    btncolor1.Visible = false;
                    Button2.Visible = false;
                    Button1.Visible = false;
                    Label2.Visible = false;
                    Label4.Visible = false;
                    Label6.Visible = false;
                    chkRound100.Visible = false;
                    Txtround.Visible = false;
                    Panel2.Visible = false;
                    chkRound100.Checked = true;
                    // Fpinternal.Visible = false;
                    lblcriteria.Visible = false;
                    txtcriteria.Visible = false;
                    lblcalulate.Visible = false;
                    txtcalculate.Visible = false;
                    chkattendance.Visible = false;
                    chkSubSub.Visible = false;
                    btncriteria.Visible = false;
                    btnAttenSetting.Visible = false;
                    chkbasedSettings.Visible = false;
                    btnAttenSetting.Visible = false;
                    errmsg.Text = "No Records Found";
                }
            }
            else
            {
                GridView1.Visible = false;
                errmsg.Visible = true;
                btncolor1.Visible = false;
                Button2.Visible = false;
                Button1.Visible = false;
                Label2.Visible = false;
                Label4.Visible = false;
                Label6.Visible = false;
                // Fpinternal.Visible = false;
                lblcriteria.Visible = false;
                txtcriteria.Visible = false;
                lblcalulate.Visible = false;
                txtcalculate.Visible = false;
                chkattendance.Visible = false;
                chkSubSub.Visible = false;
                btncriteria.Visible = false;
                btnAttenSetting.Visible = false;
                btnAttenSetting.Visible = false;
                chkbasedSettings.Visible = false;
                errmsg.Text = "No Records Found";
            }




            if (forschoolsetting == true)
            {
                if (ckhdegreewise.Checked == true)
                {
                    setwidth.Attributes.Add("style", "width:93px;");

                }
                else
                {
                    setwidth.Attributes.Add("style", "width:93px;");
                }
                lbledulevel.Visible = false;
                ddledulevel.Visible = false;
            }



        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
        //Table    internal_cam_calculation_master_setting Structure
        //Create Table internal_cam_calculation_master_setting (Istype nvarchar(50),subject_no int,syll_code int,
        //Calculate_Cam_Criteria nvarchar(100),Criteria_no nvarchar(100),Cam_Option nvarchar(500),Cam_Ave_best int,Conversion_value int,
        //Attendance nvarchar(100),Att_Cal nvarchar(100),Att_Mark_Per nvarchar(50),Calculation_Criteria nvarchar(100),Calculation_Option nvarchar(100),
        //Include_Final_Calculation int,Round_of int,Round_Value int,int_Mark_settings int)

        //alter table  camarks add Exam_Year int
        //alter table  camarks add Exam_Month int

    }

    protected void btncalculate_OnClick(object sender, EventArgs e)
    {
        string actcol = "5";
        GridView2.Visible = false;
        Button calc = (Button)sender;
        string rowind = calc.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowindex = Convert.ToInt32(rowind) - 2;
        Session["rowIndex"] = Convert.ToString(rowindex);
        txtcriteria.Text = "";
        txtcalculate.Text = "";
        chkattendance.Checked = false;
        chkSubSub.Checked = false;
        int row = rowindex;
        Label sylcod = (Label)GridView1.Rows[rowindex].FindControl("lblsyllcode");
        syllcode = sylcod.Text;
        Label subno = (Label)GridView1.Rows[rowindex].FindControl("lblsubno");
        subjectno = subno.Text;
        panel4.Visible = false;
        //string act = "5";
        // actcol = "5";
        Calculate_Button_function(Convert.ToString(rowindex), actcol);
        if (enableflag == true)
        {
            btngo_Click(sender, e);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Student Cam Internal Marks Calculated Successfully!')", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Marks Not Entered')", true);
        }

    }

    public void Calculate_Button_function(string actrow, string actcol)
    {
        try
        {
            htCamFinalInternalInsert.Clear();
            htCamCalculationInsert.Clear();
            errmsg.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            txtreport.Visible = false;
            divPrint.Visible = false;
            lblreportname.Visible = false;
            ds.Dispose();
            ds = da.select_method("select * from sysobjects where name='tbl_Cam_Calculation' and Type='U'", hat, "text ");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
            {
                int p = da.insert_method("create table tbl_Cam_Calculation (subject_no int,syll_code int,Istype nvarchar(25),Cam_option int,roll_no nvarchar(50),Exammark nvarchar(50),conversion int)", hat, "text");
            }

            errmsg.Visible = false;
            int row = Convert.ToInt32(actrow);
            Label sylcd = (Label)GridView1.Rows[row].FindControl("lblsyllcode");
            syllcode = sylcd.Text;
            Label subno = (Label)GridView1.Rows[row].FindControl("lblsubno");
            subjectno = subno.Text;
            Label sec = (Label)GridView1.Rows[row].FindControl("lblsection");
            string sections = sec.Text;

            string sectionval = "";
            string strsecval = "";
            if (sections.Trim() != "" && sections != "0" && sections != "-1" && sections.Trim().ToLower() != "all")
            {
                strsecval = "  and sections='" + sections + "'";
                sectionval = "  and reg.sections='" + sections + "'";
            }
            string deletequery = "Delete from tbl_cam_calculation where subject_no='" + subjectno + "' " + strsecval + "";
            int del = da.insert_method(deletequery, hat, "Text");

            //=====================Cam and Attendance Calculation=============================

            string strgetmarksquery = "";
            DataSet dsgetmarks = new DataSet();
            string strquery = "Select * from internal_cam_calculation_master_setting where subject_no=" + subjectno + " and syll_code=" + syllcode + " and (Calculation_Option='' or Calculation_Option is null) and Istype<>'Settings' " + strsecval + "  order by idno,subject_no,syll_code,Istype desc";

            DataSet dscalculate = da.select_method(strquery, hat, "Text");
            if (dscalculate.Tables.Count > 0 && dscalculate.Tables[0].Rows.Count > 0)
            {
                for (int calc = 0; dscalculate.Tables[0].Rows.Count > calc; calc++)
                {
                    string Istype = dscalculate.Tables[0].Rows[calc]["Istype"].ToString();
                    string Subject = dscalculate.Tables[0].Rows[calc]["Subject_no"].ToString();
                    subjectno = Subject;
                    string Syllabus = dscalculate.Tables[0].Rows[calc]["Syll_code"].ToString();
                    syllcode = Syllabus;
                    string criteriano = dscalculate.Tables[0].Rows[calc]["criteria_no"].ToString();
                    string convertion = dscalculate.Tables[0].Rows[calc]["Conversion_Value"].ToString();
                    string round = dscalculate.Tables[0].Rows[calc]["Round_value"].ToString();

                    string SubjectId = Convert.ToString(dscalculate.Tables[0].Rows[calc]["subjectid"]);

                    string[] camspit = Istype.Split(' ');

                    if (Istype != "Attendance")
                    {
                        string Camoption = dscalculate.Tables[0].Rows[calc]["Cam_option"].ToString();
                        if (criteriano != "")
                        {
                            Double convertvalue = 100;
                            if (convertion != "" && convertion != "0")
                            {
                                convertvalue = Convert.ToDouble(convertion);
                            }
                            //============Total Avereage=====================
                            double examtotalmark1 = 0;
                            double exammark1 = 0;
                            double exammaxmark1 = 0;
                            string temproll = "";

                            #region Sona CAM Calculation
                            //-------------------Rajkumar for Sona subSubject Cam Calculations
                            if (!string.IsNullOrEmpty(SubjectId))
                            {
                                string SelectQ = "Select  sm.appno,testmark,e.exam_code,reg.roll_no,s.subjectid from subsubjectTestDetails s,exam_type e,subSubjectWiseMarkEntry sm,registration reg  where reg.app_no=sm.appno and sm.subjectid=s.subjectid and  e.exam_code=s.examCode and e.subject_no='" + subjectno + "'  and  criteria_no in(" + criteriano + ") " + sectionval + " order by sm.appno,testmark,e.exam_code,reg.roll_no,s.subjectid";
                                DataTable dtStudentMark = dir.selectDataTable(SelectQ);

                                DataTable dicExamCode = dtStudentMark.DefaultView.ToTable(true, "exam_code");
                                string examCode = string.Empty;
                                if (dicExamCode.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dicExamCode.Rows)
                                    {
                                        if (string.IsNullOrEmpty(examCode))
                                            examCode = Convert.ToString(dr["exam_code"]);
                                        else
                                            examCode = examCode + "," + Convert.ToString(dr["exam_code"]);
                                    }
                                }
                                DataTable dtSubSubject = dir.selectDataTable("select * from subsubjectTestDetails where examCode in(" + examCode + ")");

                                string SelectAllQ = "select r.app_no,r.reg_no,r.roll_no from registration r,subjectchooser s where r.roll_no=s.roll_no and s.subject_no='" + subjectno + "' " + strsecval + "";
                                DataTable dtAllStud = dir.selectDataTable(SelectAllQ);


                                string resultMark = " select distinct reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name,reg.app_no,et.criteria_no from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and et.subject_no=sc.subject_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no=" + Subject + " and et.criteria_no in (" + criteriano + ") and ISNULL(reg.Sections,'')=ISNULL(et.sections,'') " + sectionval + " " + strorder + "";

                                DataTable dtResult = dir.selectDataTable(resultMark);

                                if (dtAllStud.Rows.Count > 0)
                                {
                                    for (int mark = 0; dtAllStud.Rows.Count > mark; mark++)
                                    {
                                        string appNo = dtAllStud.Rows[mark]["app_no"].ToString();
                                        //string examCode = dtAllStud.Rows[mark]["exam_code"].ToString();
                                        string rollNo = dtAllStud.Rows[mark]["roll_no"].ToString();
                                        double subMaxMark = 0;
                                        double studentMark = 0;

                                        if (dtSubSubject.Rows.Count > 0)
                                        {
                                            string[] sub1 = SubjectId.Split(';');
                                            if (sub1.Length > 0)
                                            {
                                                for (int s1 = 0; s1 < sub1.Length; s1++)
                                                {
                                                    string[] sd = Convert.ToString(sub1[s1]).Split('-');
                                                    string Cno = string.Empty;
                                                    if (sd.Length > 1)
                                                    {
                                                        Cno = Convert.ToString(sd[0]);
                                                        string sId = Convert.ToString(sd[1]);
                                                        if (!string.IsNullOrEmpty(sId))
                                                        {
                                                            object sum = dtSubSubject.Compute("Sum(maxmark)", "subjectid in(" + sId + ")");
                                                            double totalMaxMark = 0;
                                                            double.TryParse(Convert.ToString(sum).Trim(), out totalMaxMark);
                                                            object summark = dtStudentMark.Compute("Sum(testmark)", "subjectid in(" + sId + ") and appNo='" + appNo + "'");
                                                            double stuMark = 0;
                                                            double.TryParse(Convert.ToString(summark).Trim(), out stuMark);

                                                            if (stuMark > 0)
                                                                studentMark = studentMark + stuMark;
                                                            if (totalMaxMark != 0)
                                                                subMaxMark = subMaxMark + totalMaxMark;
                                                        }
                                                        else
                                                        {

                                                            object sum = dtResult.Compute("Sum(max_mark)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                                            double totalMaxMark = 0;
                                                            double.TryParse(Convert.ToString(sum).Trim(), out totalMaxMark);
                                                            object summark = dtResult.Compute("Sum(marks_obtained)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                                            double stuMark = 0;
                                                            double.TryParse(Convert.ToString(summark).Trim(), out stuMark);

                                                            if (stuMark > 0)
                                                                studentMark = studentMark + stuMark;
                                                            if (totalMaxMark != 0)
                                                                subMaxMark = subMaxMark + totalMaxMark;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        object sum = dtResult.Compute("Sum(max_mark)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                                        double totalMaxMark = 0;
                                                        double.TryParse(Convert.ToString(sum).Trim(), out totalMaxMark);
                                                        object summark = dtResult.Compute("Sum(marks_obtained)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                                        double stuMark = 0;
                                                        double.TryParse(Convert.ToString(summark).Trim(), out stuMark);

                                                        if (stuMark > 0)
                                                            studentMark = studentMark + stuMark;
                                                        if (totalMaxMark != 0)
                                                            subMaxMark = subMaxMark + totalMaxMark;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string[] sub1 = SubjectId.Split('-');
                                            string Cno = Convert.ToString(sub1[0]);
                                            object sum = dtResult.Compute("Sum(max_mark)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                            double totalMaxMark = 0;
                                            double.TryParse(Convert.ToString(sum).Trim(), out totalMaxMark);
                                            object summark = dtResult.Compute("Sum(marks_obtained)", "criteria_no in(" + Cno + ") and roll_no='" + rollNo + "'");
                                            double stuMark = 0;
                                            double.TryParse(Convert.ToString(summark).Trim(), out stuMark);

                                            if (stuMark > 0)
                                                studentMark = studentMark + stuMark;
                                            if (totalMaxMark != 0)
                                                subMaxMark = subMaxMark + totalMaxMark;
                                        }

                                        examtotalmark1 = Convert.ToDouble(studentMark) / Convert.ToDouble(subMaxMark) * convertvalue;
                                        examtotalmark1 = Math.Round(examtotalmark1, Convert.ToInt32(round), MidpointRounding.AwayFromZero);

                                        string insertqurey1 = "insert into tbl_Cam_Calculation (Subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + rollNo + "','" + examtotalmark1 + "','" + convertvalue + "','" + sections + "')";

                                        if (examtotalmark1 >= 0)
                                        {
                                            htCamCalculationInsert.Clear();
                                            htCamCalculationInsert.Add("@subjectNo", Subject);
                                            htCamCalculationInsert.Add("@syllCode", syllcode);
                                            htCamCalculationInsert.Add("@isType", Istype);
                                            htCamCalculationInsert.Add("@camOptions", Camoption);
                                            htCamCalculationInsert.Add("@rollNo", rollNo);
                                            htCamCalculationInsert.Add("@marks", examtotalmark1);
                                            htCamCalculationInsert.Add("@convertion", convertvalue);
                                            htCamCalculationInsert.Add("@Sections", sections);
                                            int p1 = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                        }

                                        exammaxmark1 = 0;
                                        exammark1 = 0;
                                        examtotalmark1 = 0;

                                    }
                                }
                            }
                            #endregion
                            //-----------------------------------------------
                            else if (Camoption == "1")
                            {



                                strgetmarksquery = "select distinct reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name,reg.app_no from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and et.subject_no=sc.subject_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no=" + Subject + " and et.criteria_no in (" + criteriano + ") and ISNULL(reg.Sections,'')=ISNULL(et.sections,'') " + sectionval + " " + strorder + "";


                                dsgetmarks = da.select_method(strgetmarksquery, hat, "text");

                                if (dsgetmarks.Tables.Count > 0 && dsgetmarks.Tables[0].Rows.Count > 0)
                                {
                                    for (int mark = 0; dsgetmarks.Tables[0].Rows.Count > mark; mark++)
                                    {
                                        string rollno = dsgetmarks.Tables[0].Rows[mark]["roll_no"].ToString();
                                        string exammark = dsgetmarks.Tables[0].Rows[mark]["marks_obtained"].ToString();
                                        string maxmark = dsgetmarks.Tables[0].Rows[mark]["max_mark"].ToString();
                                        if (mark == 0)
                                        {
                                            temproll = rollno;
                                        }
                                        if (temproll != rollno)
                                        {
                                            if (exammark1 > 0)
                                            {
                                                examtotalmark1 = Convert.ToDouble(exammark1) / Convert.ToDouble(exammaxmark1) * convertvalue;
                                                examtotalmark1 = Math.Round(examtotalmark1, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                examtotalmark1 = 0;
                                            }
                                            //string insertqurey = "insert into tbl_Cam_Calculation (Subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + temproll + "','" + examtotalmark1 + "','" + convertvalue + "','" + sections + "')";
                                            //int p = da.insert_method(insertqurey, hat, "Text");

                                            htCamCalculationInsert.Clear();
                                            htCamCalculationInsert.Add("@subjectNo", Subject);
                                            htCamCalculationInsert.Add("@syllCode", syllcode);
                                            htCamCalculationInsert.Add("@isType", Istype);
                                            htCamCalculationInsert.Add("@camOptions", Camoption);
                                            htCamCalculationInsert.Add("@rollNo", temproll);
                                            htCamCalculationInsert.Add("@marks", examtotalmark1);
                                            htCamCalculationInsert.Add("@convertion", convertvalue);
                                            htCamCalculationInsert.Add("@Sections", sections);
                                            int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");


                                            exammaxmark1 = 0;
                                            exammark1 = 0;
                                            examtotalmark1 = 0;
                                            temproll = rollno;
                                        }
                                        if (exammark.Trim() != "")
                                        {
                                            if (Convert.ToDouble(exammark) > 0)
                                            {
                                                exammark1 = exammark1 + Convert.ToDouble(exammark);
                                            }
                                        }
                                        if (maxmark != "" && maxmark != "0")
                                        {
                                            exammaxmark1 = exammaxmark1 + Convert.ToInt32(maxmark);
                                        }
                                    }

                                    examtotalmark1 = Convert.ToDouble(exammark1) / Convert.ToDouble(exammaxmark1) * convertvalue;
                                    examtotalmark1 = Math.Round(examtotalmark1, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                    //string insertqurey1 = "insert into tbl_Cam_Calculation (Subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + temproll + "','" + examtotalmark1 + "','" + convertvalue + "','" + sections + "')";
                                    //int p1 = da.insert_method(insertqurey1, hat, "Text");
                                    string insertqurey1 = "insert into tbl_Cam_Calculation (Subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + temproll + "','" + examtotalmark1 + "','" + convertvalue + "','" + sections + "')";

                                    htCamCalculationInsert.Clear();
                                    htCamCalculationInsert.Add("@subjectNo", Subject);
                                    htCamCalculationInsert.Add("@syllCode", syllcode);
                                    htCamCalculationInsert.Add("@isType", Istype);
                                    htCamCalculationInsert.Add("@camOptions", Camoption);
                                    htCamCalculationInsert.Add("@rollNo", temproll);
                                    htCamCalculationInsert.Add("@marks", examtotalmark1);
                                    htCamCalculationInsert.Add("@convertion", convertvalue);
                                    htCamCalculationInsert.Add("@Sections", sections);
                                    int p1 = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");

                                }

                            }
                            //================Best of============================
                            else if (Camoption == "2")
                            {
                                string strrollquery = "select reg.roll_no,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from registration reg,subjectchooser s where reg.roll_no=s.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar' and s.subject_no=" + Subject + " " + sectionval + " " + strorder + "";
                                DataSet dsgetroll = da.select_method(strrollquery, hat, "Text");
                                if (dsgetroll.Tables[0].Rows.Count > 0)
                                {
                                    for (int roll = 0; dsgetroll.Tables[0].Rows.Count > roll; roll++)
                                    {
                                        string stdroll = dsgetroll.Tables[0].Rows[roll]["roll_no"].ToString();
                                        Double tempmark = 0;
                                        strgetmarksquery = "select distinct  reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no=" + Subject + " and et.subject_no=sc.subject_no and et.criteria_no in (" + criteriano + ")  and ISNULL(reg.Sections,'')=ISNULL(et.sections,'') and r.roll_no='" + stdroll + "'  " + strorder + "";
                                        dsgetmarks = da.select_method(strgetmarksquery, hat, "text");
                                        if (dsgetmarks.Tables[0].Rows.Count > 0)
                                        {
                                            for (int mark = 0; dsgetmarks.Tables[0].Rows.Count > mark; mark++)
                                            {
                                                string rollno = dsgetmarks.Tables[0].Rows[mark]["roll_no"].ToString();
                                                string exammark = dsgetmarks.Tables[0].Rows[mark]["marks_obtained"].ToString();
                                                string maxmark = dsgetmarks.Tables[0].Rows[mark]["max_mark"].ToString();
                                                Double exammaxmark = 0;
                                                Double examgetmark = 0;
                                                Double examtotalmark = 0;
                                                if (exammark.Trim() != "")
                                                {
                                                    if (Convert.ToDouble(exammark) > 0)
                                                    {
                                                        exammaxmark = Convert.ToDouble(maxmark);
                                                        examgetmark = Convert.ToDouble(exammark);
                                                        examtotalmark = examgetmark / exammaxmark * convertvalue;
                                                        examtotalmark = Math.Round(examtotalmark, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                                    }
                                                }
                                                else
                                                {
                                                    examtotalmark = 0;
                                                }
                                                if (tempmark < examtotalmark)
                                                {
                                                    tempmark = examtotalmark;
                                                }
                                            }
                                            string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + stdroll + "','" + tempmark + "','" + convertvalue + "','" + sections + "')";
                                            //int p = da.insert_method(insertqurey, hat, "Text");
                                            htCamCalculationInsert.Clear();
                                            htCamCalculationInsert.Add("@subjectNo", Subject);
                                            htCamCalculationInsert.Add("@syllCode", syllcode);
                                            htCamCalculationInsert.Add("@isType", Istype);
                                            htCamCalculationInsert.Add("@camOptions", Camoption);
                                            htCamCalculationInsert.Add("@rollNo", stdroll);
                                            htCamCalculationInsert.Add("@marks", tempmark);
                                            htCamCalculationInsert.Add("@convertion", convertvalue);
                                            htCamCalculationInsert.Add("@Sections", sections);
                                            int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                        }
                                    }
                                }
                            }
                            //============================Average of Best=================================
                            else if (Camoption == "3")
                            {
                                string bestof = dscalculate.Tables[0].Rows[calc]["Cam_Ave_Best"].ToString();
                                int best = 1;
                                if (bestof != "0" || bestof != "")
                                {
                                    best = Convert.ToInt32(bestof);
                                }

                                string strrollquery = "select reg.roll_no,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from registration reg,subjectchooser s where reg.roll_no=s.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar' and s.subject_no=" + Subject + " " + sectionval + " " + strorder + "";
                                DataSet dsgetroll = da.select_method(strrollquery, hat, "Text");
                                if (dsgetroll.Tables[0].Rows.Count > 0)
                                {
                                    for (int roll = 0; dsgetroll.Tables[0].Rows.Count > roll; roll++)
                                    {
                                        string stdroll = dsgetroll.Tables[0].Rows[roll]["roll_no"].ToString();
                                        strgetmarksquery = "select top " + best + " (r.marks_obtained/et.max_mark * 100), reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no=" + Subject + " and et.subject_no=sc.subject_no and et.criteria_no in (" + criteriano + ")  and ISNULL(reg.Sections,'')=ISNULL(et.sections,'') and r.roll_no='" + stdroll + "' order by r.marks_obtained desc";
                                        dsgetmarks = da.select_method(strgetmarksquery, hat, "text");
                                        if (dsgetmarks.Tables[0].Rows.Count > 0)
                                        {
                                            Double examtotalmark = 0;
                                            Double exammaxmark = 0;
                                            double examinmark = 0;
                                            for (int mark = 0; dsgetmarks.Tables[0].Rows.Count > mark; mark++)
                                            {
                                                string rollno = dsgetmarks.Tables[0].Rows[mark]["roll_no"].ToString();
                                                string exammark = dsgetmarks.Tables[0].Rows[mark]["marks_obtained"].ToString();
                                                string maxmark = dsgetmarks.Tables[0].Rows[mark]["max_mark"].ToString();
                                                if (exammark.Trim() != "")
                                                {
                                                    if (Convert.ToDouble(exammark) >= 0)
                                                    {
                                                        examinmark = examinmark + Convert.ToDouble(exammark);
                                                    }
                                                }
                                                if (maxmark != "")
                                                {
                                                    exammaxmark = exammaxmark + Convert.ToDouble(maxmark);
                                                }
                                            }
                                            if (examinmark != 0)
                                            {
                                                examtotalmark = Convert.ToDouble(examinmark) / Convert.ToDouble(exammaxmark) * convertvalue;
                                                examtotalmark = Math.Round(examtotalmark, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                            }
                                            else
                                            {
                                                examtotalmark = 0;
                                            }
                                            string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + stdroll + "','" + examtotalmark + "','" + convertvalue + "','" + sections + "')";
                                            //int p = da.insert_method(insertqurey, hat, "Text");
                                            htCamCalculationInsert.Clear();
                                            htCamCalculationInsert.Add("@subjectNo", Subject);
                                            htCamCalculationInsert.Add("@syllCode", syllcode);
                                            htCamCalculationInsert.Add("@isType", Istype);
                                            htCamCalculationInsert.Add("@camOptions", Camoption);
                                            htCamCalculationInsert.Add("@rollNo", stdroll);
                                            htCamCalculationInsert.Add("@marks", examtotalmark);
                                            htCamCalculationInsert.Add("@convertion", convertvalue);
                                            htCamCalculationInsert.Add("@Sections", sections);
                                            int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                        }

                                    }
                                }
                            }
                            //==========================Individual Test=======================================
                            else if (Camoption == "4")
                            {
                                strgetmarksquery = "select distinct reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no=" + Subject + " and et.subject_no=sc.subject_no  and ISNULL(reg.Sections,'')=ISNULL(et.sections,'') and et.criteria_no in (" + criteriano + ") " + sectionval + " " + strorder + "";
                                dsgetmarks = da.select_method(strgetmarksquery, hat, "text");
                                if (dsgetmarks.Tables[0].Rows.Count > 0)
                                {
                                    for (int mark = 0; dsgetmarks.Tables[0].Rows.Count > mark; mark++)
                                    {
                                        string rollno = dsgetmarks.Tables[0].Rows[mark]["roll_no"].ToString();
                                        string exammark = dsgetmarks.Tables[0].Rows[mark]["marks_obtained"].ToString();
                                        string maxmark = dsgetmarks.Tables[0].Rows[mark]["max_mark"].ToString();
                                        Double examtotalmark = 0;
                                        Double exammrktotal = 0;
                                        Double exammaxmrk = 0;
                                        if (maxmark != "" && maxmark != "0")
                                        {
                                            exammaxmrk = Convert.ToInt32(maxmark);
                                        }
                                        if (exammark.Trim() != "")
                                        {
                                            if (Convert.ToDouble(exammark) >= 0)
                                            {
                                                exammrktotal = Convert.ToDouble(exammark);
                                            }
                                        }
                                        if (exammrktotal == 0)
                                        {
                                            examtotalmark = 0;
                                        }
                                        else
                                        {
                                            examtotalmark = exammrktotal / exammaxmrk * convertvalue;
                                            examtotalmark = Math.Round(examtotalmark, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                        }
                                        string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,Cam_option,roll_no,Exammark,conversion,sections) values ('" + Subject + "','" + syllcode + "','" + Istype + "','" + Camoption + "','" + rollno + "','" + examtotalmark + "','" + convertvalue + "','" + sections + "')";
                                        //int p = da.insert_method(insertqurey, hat, "Text");
                                        htCamCalculationInsert.Clear();
                                        htCamCalculationInsert.Add("@subjectNo", Subject);
                                        htCamCalculationInsert.Add("@syllCode", syllcode);
                                        htCamCalculationInsert.Add("@isType", Istype);
                                        htCamCalculationInsert.Add("@camOptions", Camoption);
                                        htCamCalculationInsert.Add("@rollNo", rollno);
                                        htCamCalculationInsert.Add("@marks", examtotalmark);
                                        htCamCalculationInsert.Add("@convertion", convertvalue);
                                        htCamCalculationInsert.Add("@Sections", sections);
                                        int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                    }
                                }
                            }
                        }
                    }
                    //============================Attendadance=========================
                    else if (Istype == "Attendance")//Rajkumar 
                    {
                        string batch = "";
                        string degree = "";
                        string sem = "";
                        string fromdatevalue = string.Empty;
                        string todatevalue = string.Empty;
                        string fromdate = dscalculate.Tables[0].Rows[calc]["Attendance"].ToString();//get based  on date
                        string attcall = dscalculate.Tables[0].Rows[calc]["Att_cal"].ToString();
                        string att_mark_pre = dscalculate.Tables[0].Rows[calc]["att_mark_per"].ToString();

                        string getbatch = " select batch_year,degree_code,semester from syllabus_master where syll_code=" + syllcode + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = da.select_method(getbatch, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            batch = ds.Tables[0].Rows[0]["Batch_year"].ToString();
                            degree = ds.Tables[0].Rows[0]["degree_code"].ToString();
                            sem = ds.Tables[0].Rows[0]["semester"].ToString();
                        }
                        string strAttendance = "select distinct CONVERT(varchar(20),fromDate,103) as fdate ,CONVERT(varchar(20),todate,103) as tdate from AttendanceMarkEntry ae,AttendanceMarkValue av where ae.AttndId=av.AttndId and ae.BathYear='" + batch + "' and DegreeCode='" + degree + "' and semester='" + sem + "'";
                        DataTable dtAttnd = dir.selectDataTable(strAttendance);

                        ///------------New Calculation
                        if (dtAttnd.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtAttnd.Rows)
                            {
                                string fdate = Convert.ToString(dr["fdate"]);
                                string todate = Convert.ToString(dr["tdate"]);

                                if (fromdate == "Sem Date")//based on seminfo
                                {
                                    string strseminfo = "Select Convert(varchar(15),start_date,103) as fromdate,Convert(varchar(15),end_date,103) as todate from seminfo where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                    ds.Reset();
                                    ds.Dispose();
                                    ds = da.select_method(strseminfo, hat, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string semfromdate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                                        string[] spiltsemfrom = semfromdate.Split(' ');
                                        string[] spiltsemdatefrom = spiltsemfrom[0].Split('/');
                                        fromdatevalue = spiltsemdatefrom[0] + '/' + spiltsemdatefrom[1] + '/' + spiltsemdatefrom[2];
                                        string semtodate = ds.Tables[0].Rows[0]["todate"].ToString();
                                        string[] spiltsemto = semtodate.Split(' ');
                                        string[] spiltsemdateto = spiltsemto[0].Split('/');
                                        todatevalue = spiltsemdateto[0] + '/' + spiltsemdateto[1] + '/' + spiltsemdateto[2];

                                    }
                                }
                                else if (fromdate.ToLower() == "monthwise")
                                {
                                    fromdatevalue = fdate;
                                    todatevalue = todate;
                                }
                                else
                                {
                                    string[] date = fromdate.Split(';');
                                    string[] spiltfrom1 = date[0].Split('/');
                                    string[] spilto1 = date[1].Split('/');
                                    fromdatevalue = spiltfrom1[0] + '/' + spiltfrom1[1] + '/' + spiltfrom1[2];
                                    todatevalue = spilto1[0] + '/' + spilto1[1] + '/' + spilto1[2];
                                }

                                if (attcall == "1")
                                {
                                    persentmonthcal(fromdatevalue, todatevalue, batch, degree, sem, sections);
                                }
                                else if (attcall == "2")
                                {
                                    load_attendance(fromdatevalue, todatevalue, batch, degree, sem, sections);
                                }

                                if (att_mark_pre == "1")
                                {
                                    if (fromdate.ToLower() != "monthwise")
                                    {
                                        string para_code = "0";
                                        string attndtotal = "";
                                        string strattnd_paraquery = "Select  para_code,atnd_mark_total from PeriodAttndSchedule where degree_code='" + degree + "' and semester='" + sem + "'";
                                        DataSet dspara = da.select_method(strattnd_paraquery, hat, "Text");
                                        if (dspara.Tables[0].Rows.Count > 0)
                                        {
                                            para_code = dspara.Tables[0].Rows[0]["para_code"].ToString();
                                            attndtotal = dspara.Tables[0].Rows[0]["atnd_mark_total"].ToString();
                                        }
                                        if (para_code.Trim() != "" && para_code.Trim() != "0" && attndtotal.Trim() != "" && attndtotal.Trim() != "0")
                                        {
                                            string attnd = string.Empty;
                                            if (chkbasedSettings.Checked)
                                                attnd = "Attendance" + fromdatevalue + "-" + todatevalue;
                                            else
                                                attnd = "attendance";

                                            string strcalculate = "select distinct * from tbl_cam_calculation where istype='" + attnd + "' and subject_no=" + Subject + " " + strsecval + " order by roll_no";
                                            ds.Reset();
                                            ds.Dispose();
                                            ds = da.select_method(strcalculate, hat, "Text");
                                            for (int att = 0; att < ds.Tables[0].Rows.Count; att++)
                                            {
                                                string attroll = ds.Tables[0].Rows[att]["Roll_no"].ToString();
                                                string attpresent = ds.Tables[0].Rows[att]["Exammark"].ToString();
                                                string atttotal = ds.Tables[0].Rows[att]["Conversion"].ToString();
                                                double presenthr = 0;
                                                double totalhr = 0;
                                                double persentpercentage = 0;
                                                if (attpresent != "")
                                                {
                                                    presenthr = Convert.ToDouble(attpresent);
                                                }
                                                if (atttotal != "")
                                                {
                                                    totalhr = Convert.ToDouble(atttotal);
                                                }
                                                if (presenthr != 0)
                                                {
                                                    persentpercentage = presenthr / totalhr * 100;
                                                    persentpercentage = Math.Round(persentpercentage, 2, MidpointRounding.AwayFromZero);
                                                }
                                                string SQlSEL = "select attnd_mark from attnd_para where para_code=" + para_code + " and  " + persentpercentage + " between frange and trange";
                                                string straddmark = da.GetFunction(SQlSEL);
                                                if (straddmark != "")
                                                {
                                                    Double attmarkvalue = Convert.ToDouble(straddmark);
                                                    attmarkvalue = Math.Round(attmarkvalue, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                                    straddmark = attmarkvalue.ToString();
                                                }
                                                string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','AttendanceValue','" + attroll + "','" + straddmark + "','" + attndtotal + "','" + sections + "')";


                                                //int p = da.insert_method(insertqurey, hat, "Text");

                                                htCamCalculationInsert.Clear();
                                                htCamCalculationInsert.Add("@subjectNo", subjectno);
                                                htCamCalculationInsert.Add("@syllCode", syllcode);
                                                htCamCalculationInsert.Add("@isType", "AttendanceValue");
                                                //htCamCalculationInsert.Add("@camOptions", null);
                                                htCamCalculationInsert.Add("@rollNo", attroll);
                                                htCamCalculationInsert.Add("@marks", straddmark);
                                                htCamCalculationInsert.Add("@convertion", attndtotal);
                                                htCamCalculationInsert.Add("@Sections", sections);
                                                int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                            }
                                        }
                                        else
                                        {
                                            deletequery = "Delete from tbl_cam_calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + "";
                                            del = da.insert_method(deletequery, hat, "Text");
                                            errmsg.Visible = true;
                                            errmsg.Text = "Please Add Sem Attendance Max Mark";
                                            return;
                                        }
                                    }

                                    else
                                    {
                                        string[] spiltfrom1 = fromdatevalue.Split('/');
                                        string[] spilto11 = todatevalue.Split('/');
                                        string fromdatevalue1 = spiltfrom1[1] + '/' + spiltfrom1[0] + '/' + spiltfrom1[2];
                                        string todatevalue1 = spilto11[1] + '/' + spilto11[0] + '/' + spilto11[2];

                                        string setting = "select * from AttendanceMarkEntry where BathYear='" + batch + "' and DegreeCode='" + degree + "' and semester='" + sem + "' and fromDate='" + fromdatevalue1 + "' and toDate='" + todatevalue1 + "'";

                                        DataTable dtSettings = dir.selectDataTable(setting);

                                        if (dtSettings.Rows.Count > 0)
                                        {
                                            string attnd = string.Empty;
                                            if (chkbasedSettings.Checked)
                                                attnd = "Attendance" + fromdatevalue + "-" + todatevalue;
                                            else
                                                attnd = "attendance";

                                            string perCode = Convert.ToString(dtSettings.Rows[0]["AttndId"]);
                                            string maxAttendVal = Convert.ToString(dtSettings.Rows[0]["maxAttndValue"]);
                                            string strcalculate = "select distinct * from tbl_cam_calculation where istype='" + attnd + "' and subject_no=" + Subject + " " + strsecval + " order by roll_no";
                                            ds.Reset();
                                            ds.Dispose();
                                            ds = da.select_method(strcalculate, hat, "Text");
                                            for (int att = 0; att < ds.Tables[0].Rows.Count; att++)
                                            {
                                                string attroll = ds.Tables[0].Rows[att]["Roll_no"].ToString();
                                                string attpresent = ds.Tables[0].Rows[att]["Exammark"].ToString();
                                                string atttotal = ds.Tables[0].Rows[att]["Conversion"].ToString();
                                                double presenthr = 0;
                                                double totalhr = 0;
                                                double persentpercentage = 0;
                                                if (attpresent != "")
                                                {
                                                    presenthr = Convert.ToDouble(attpresent);
                                                }
                                                if (atttotal != "")
                                                {
                                                    totalhr = Convert.ToDouble(atttotal);
                                                }
                                                if (presenthr != 0)
                                                {
                                                    persentpercentage = presenthr / totalhr * 100;
                                                    persentpercentage = Math.Round(persentpercentage, 2, MidpointRounding.AwayFromZero);
                                                }
                                                //string SQlSEL = "select attnd_mark from attnd_para where para_code=" + para_code + " and  " + persentpercentage + " between frange and trange";
                                                string SQlSEL = "select AttndValue from AttendanceMarkValue where AttndId='" + perCode + "' and " + persentpercentage + " between frange and torange";

                                                string straddmark = da.GetFunction(SQlSEL);
                                                if (straddmark != "")
                                                {
                                                    Double attmarkvalue = Convert.ToDouble(straddmark);
                                                    attmarkvalue = Math.Round(attmarkvalue, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                                    straddmark = attmarkvalue.ToString();
                                                }



                                                string Attentxt = "Attendance" + fromdatevalue + "-" + todatevalue;
                                                string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','" + Attentxt + "','" + attroll + "','" + straddmark + "','" + maxAttendVal + "','" + sections + "')";
                                                //int p = da.insert_method(insertqurey, hat, "Text");
                                                string deletequery1 = "delete from tbl_Cam_Calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and  istype='" + Attentxt + "' and roll_no='" + attroll + "' " + strsecval;
                                                int p1 = da.update_method_wo_parameter(deletequery1, "text");

                                                htCamCalculationInsert.Clear();
                                                htCamCalculationInsert.Add("@subjectNo", subjectno);
                                                htCamCalculationInsert.Add("@syllCode", syllcode);
                                                htCamCalculationInsert.Add("@isType", Attentxt);
                                                //htCamCalculationInsert.Add("@camOptions", null);
                                                htCamCalculationInsert.Add("@rollNo", attroll);
                                                htCamCalculationInsert.Add("@marks", straddmark);
                                                htCamCalculationInsert.Add("@convertion", maxAttendVal);
                                                htCamCalculationInsert.Add("@Sections", sections);
                                                int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                            }
                                        }
                                        else
                                        {
                                            deletequery = "Delete from tbl_cam_calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + "";
                                            del = da.insert_method(deletequery, hat, "Text");
                                            errmsg.Visible = true;
                                            errmsg.Text = "Please Add Sem Attendance Max Mark";
                                            return;
                                        }
                                    }
                                }
                                else if (att_mark_pre == "2")
                                {
                                    //added by Mullai
                                    string attnd = string.Empty;
                                    if (chkbasedSettings.Checked)
                                        attnd = "Attendance" + fromdatevalue + "-" + todatevalue;
                                    else
                                        attnd = "attendance";

                                    ///
                                    string strcalculate = "select distinct * from tbl_cam_calculation where istype='" + attnd + "' and subject_no=" + Subject + " and syll_code='" + syllcode + "' " + strsecval + " order by roll_no";
                                    ds.Reset();
                                    ds.Dispose();
                                    ds = da.select_method(strcalculate, hat, "Text");
                                    for (int att = 0; att < ds.Tables[0].Rows.Count; att++)
                                    {
                                        string attroll = ds.Tables[0].Rows[att]["Roll_no"].ToString();
                                        string attpresent = ds.Tables[0].Rows[att]["Exammark"].ToString();
                                        string atttotal = ds.Tables[0].Rows[att]["Conversion"].ToString();
                                        double presenthr = 0;
                                        double totalhr = 0;
                                        double persent = 0;
                                        if (attpresent != "")
                                        {
                                            presenthr = Convert.ToDouble(attpresent);
                                        }
                                        if (atttotal != "")
                                        {
                                            totalhr = Convert.ToDouble(atttotal);
                                        }
                                        if (presenthr != 0)
                                        {
                                            persent = presenthr / totalhr * 100;
                                            persent = Math.Round(persent, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            persent = 0;
                                        }

                                        string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','AttendanceValue','" + attroll + "','" + persent + "','100','" + sections + "')";
                                        //int p = da.insert_method(insertqurey, hat, "Text");

                                        htCamCalculationInsert.Clear();
                                        htCamCalculationInsert.Add("@subjectNo", subjectno);
                                        htCamCalculationInsert.Add("@syllCode", syllcode);
                                        htCamCalculationInsert.Add("@isType", "AttendanceValue");
                                        //htCamCalculationInsert.Add("@camOptions", null);
                                        htCamCalculationInsert.Add("@rollNo", attroll);
                                        htCamCalculationInsert.Add("@marks", persent);
                                        htCamCalculationInsert.Add("@convertion", "100");
                                        htCamCalculationInsert.Add("@Sections", sections);
                                        int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                    }
                                }
                            }

                        }

//--------------------old Calculation
                        else
                        {

                            if (fromdate == "Sem Date")//based on seminfo
                            {
                                string strseminfo = "Select Convert(varchar(15),start_date,103) as fromdate,Convert(varchar(15),end_date,103) as todate from seminfo where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "'";
                                ds.Reset();
                                ds.Dispose();
                                ds = da.select_method(strseminfo, hat, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string semfromdate = ds.Tables[0].Rows[0]["fromdate"].ToString();
                                    string[] spiltsemfrom = semfromdate.Split(' ');
                                    string[] spiltsemdatefrom = spiltsemfrom[0].Split('/');
                                    fromdatevalue = spiltsemdatefrom[0] + '/' + spiltsemdatefrom[1] + '/' + spiltsemdatefrom[2];
                                    string semtodate = ds.Tables[0].Rows[0]["todate"].ToString();
                                    string[] spiltsemto = semtodate.Split(' ');
                                    string[] spiltsemdateto = spiltsemto[0].Split('/');
                                    todatevalue = spiltsemdateto[0] + '/' + spiltsemdateto[1] + '/' + spiltsemdateto[2];

                                }
                            }
                            else
                            {
                                string[] date = fromdate.Split(';');
                                string[] spiltfrom1 = date[0].Split('/');
                                string[] spilto1 = date[1].Split('/');
                                fromdatevalue = spiltfrom1[0] + '/' + spiltfrom1[1] + '/' + spiltfrom1[2];
                                todatevalue = spilto1[0] + '/' + spilto1[1] + '/' + spilto1[2];
                            }
                            if (attcall == "1")
                            {
                                persentmonthcal(fromdatevalue, todatevalue, batch, degree, sem, sections);
                            }
                            else if (attcall == "2")
                            {
                                load_attendance(fromdatevalue, todatevalue, batch, degree, sem, sections);
                            }

                            if (att_mark_pre == "1")
                            {
                                if (fromdate.ToLower() != "monthwise")
                                {
                                    string para_code = "0";
                                    string attndtotal = "";
                                    string strattnd_paraquery = "Select  para_code,atnd_mark_total from PeriodAttndSchedule where degree_code='" + degree + "' and semester='" + sem + "'";
                                    DataSet dspara = da.select_method(strattnd_paraquery, hat, "Text");
                                    if (dspara.Tables[0].Rows.Count > 0)
                                    {
                                        para_code = dspara.Tables[0].Rows[0]["para_code"].ToString();
                                        attndtotal = dspara.Tables[0].Rows[0]["atnd_mark_total"].ToString();
                                    }
                                    if (para_code.Trim() != "" && para_code.Trim() != "0" && attndtotal.Trim() != "" && attndtotal.Trim() != "0")
                                    {
                                        string attnd = string.Empty;
                                        attnd = "attendance";

                                        string strcalculate = "select distinct * from tbl_cam_calculation where istype='" + attnd + "' and subject_no=" + Subject + " " + strsecval + " order by roll_no";
                                        ds.Reset();
                                        ds.Dispose();
                                        ds = da.select_method(strcalculate, hat, "Text");
                                        for (int att = 0; att < ds.Tables[0].Rows.Count; att++)
                                        {
                                            string attroll = ds.Tables[0].Rows[att]["Roll_no"].ToString();
                                            string attpresent = ds.Tables[0].Rows[att]["Exammark"].ToString();
                                            string atttotal = ds.Tables[0].Rows[att]["Conversion"].ToString();
                                            double presenthr = 0;
                                            double totalhr = 0;
                                            double persentpercentage = 0;
                                            if (attpresent != "")
                                            {
                                                presenthr = Convert.ToDouble(attpresent);
                                            }
                                            if (atttotal != "")
                                            {
                                                totalhr = Convert.ToDouble(atttotal);
                                            }
                                            if (presenthr != 0)
                                            {
                                                persentpercentage = presenthr / totalhr * 100;
                                                persentpercentage = Math.Round(persentpercentage, 2, MidpointRounding.AwayFromZero);
                                            }
                                            string SQlSEL = "select attnd_mark from attnd_para where para_code=" + para_code + " and  " + persentpercentage + " between frange and trange";
                                            string straddmark = da.GetFunction(SQlSEL);
                                            if (straddmark != "")
                                            {
                                                Double attmarkvalue = Convert.ToDouble(straddmark);
                                                attmarkvalue = Math.Round(attmarkvalue, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                                straddmark = attmarkvalue.ToString();
                                            }

                                            int del11 = da.update_method_wo_parameter("delete tbl_Cam_Calculation where  subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and roll_no='" + attroll + "' and istype='Attendance'", "text");

                                            string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Attendance','" + attroll + "','" + straddmark + "','" + attndtotal + "','" + sections + "')";


                                            //int p = da.insert_method(insertqurey, hat, "Text");

                                            htCamCalculationInsert.Clear();
                                            htCamCalculationInsert.Add("@subjectNo", subjectno);
                                            htCamCalculationInsert.Add("@syllCode", syllcode);
                                            htCamCalculationInsert.Add("@isType", "Attendance");
                                            //htCamCalculationInsert.Add("@camOptions", null);
                                            htCamCalculationInsert.Add("@rollNo", attroll);
                                            htCamCalculationInsert.Add("@marks", straddmark);
                                            htCamCalculationInsert.Add("@convertion", attndtotal);
                                            htCamCalculationInsert.Add("@Sections", sections);
                                            int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                        }
                                    }
                                    else
                                    {
                                        deletequery = "Delete from tbl_cam_calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + "";
                                        del = da.insert_method(deletequery, hat, "Text");
                                        errmsg.Visible = true;
                                        errmsg.Text = "Please Add Sem Attendance Max Mark";
                                        return;
                                    }
                                }

                            }
                            else if (att_mark_pre == "2")
                            {
                                //added by Mullai
                                string attnd = string.Empty;
                                attnd = "attendance";
                                ///
                                string strcalculate = "select distinct * from tbl_cam_calculation where istype='" + attnd + "' and subject_no=" + Subject + " and syll_code='" + syllcode + "' " + strsecval + " order by roll_no";
                                ds.Reset();
                                ds.Dispose();
                                ds = da.select_method(strcalculate, hat, "Text");
                                for (int att = 0; att < ds.Tables[0].Rows.Count; att++)
                                {
                                    string attroll = ds.Tables[0].Rows[att]["Roll_no"].ToString();
                                    string attpresent = ds.Tables[0].Rows[att]["Exammark"].ToString();
                                    string atttotal = ds.Tables[0].Rows[att]["Conversion"].ToString();
                                    double presenthr = 0;
                                    double totalhr = 0;
                                    double persent = 0;
                                    if (attpresent != "")
                                    {
                                        presenthr = Convert.ToDouble(attpresent);
                                    }
                                    if (atttotal != "")
                                    {
                                        totalhr = Convert.ToDouble(atttotal);
                                    }
                                    if (presenthr != 0)
                                    {
                                        persent = presenthr / totalhr * 100;
                                        persent = Math.Round(persent, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        persent = 0;
                                    }

                                    int del11 = da.update_method_wo_parameter("delete tbl_Cam_Calculation where  subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and roll_no='" + attroll + "' and istype='Attendance'", "text");
                                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Attendance','" + attroll + "','" + persent + "','100','" + sections + "')";
                                    //int p = da.insert_method(insertqurey, hat, "Text");

                                    htCamCalculationInsert.Clear();
                                    htCamCalculationInsert.Add("@subjectNo", subjectno);
                                    htCamCalculationInsert.Add("@syllCode", syllcode);
                                    htCamCalculationInsert.Add("@isType", "Attendance");
                                    //htCamCalculationInsert.Add("@camOptions", null);
                                    htCamCalculationInsert.Add("@rollNo", attroll);
                                    htCamCalculationInsert.Add("@marks", persent);
                                    htCamCalculationInsert.Add("@convertion", "100");
                                    htCamCalculationInsert.Add("@Sections", sections);
                                    int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                }
                            }


                        }

                    }
                }
            }

            //====================Calculation ==============================
            string intmrksetting = da.GetFunction("Select int_mark_settings from internal_cam_calculation_master_setting where istype='Settings' and syll_code='" + syllcode + "' and subject_no='" + subjectno + "' " + strsecval + "");
            string getcalcustudent = "select Distinct reg.roll_no,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from tbl_cam_calculation c,registration reg where c.roll_no=reg.roll_no and subject_no=" + subjectno + " and syll_code='" + syllcode + "' and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar' " + sectionval + " " + strorder + "";
            DataSet dscalugetdetails = da.select_method(getcalcustudent, hat, "Text");
            if (dscalugetdetails.Tables.Count > 0 && dscalugetdetails.Tables[0].Rows.Count > 0)
            {
                string getroll = "";
                string exam_code = da.GetFunction("select exam_code from exam_details e,syllabus_master s where s.batch_year=e.batch_year and s.degree_code=e.degree_code and s.semester=e.current_semester and syll_code=" + syllcode + "");
                Double subjectmaxinternalmark = Convert.ToDouble(da.GetFunction("select max_int_marks from subject where subject_no=" + subjectno + ""));

                for (int calroll = 0; dscalugetdetails.Tables[0].Rows.Count > calroll; calroll++)
                {
                    getroll = dscalugetdetails.Tables[0].Rows[calroll]["Roll_no"].ToString();
                    sections = da.GetFunction("select distinct sections from registration where roll_no='" + getroll + "'");
                    Double CalConvert = 0;
                    Double CalMark = 0;
                    Double Calmaxmark = 0;
                    Double Calgetmark = 0;
                    int calcountvalue = 0;
                    strquery = "Select * from internal_cam_calculation_master_setting where (Calculation_Option<>'' or Calculation_Option is not null) and subject_no=" + subjectno + " and syll_code=" + syllcode + " " + strsecval + " order by idno,subject_no,syll_code,Istype asc";

                    dscalculate = da.select_method(strquery, hat, "Text");
                    string calcam = "";
                    if (dscalculate.Tables.Count > 0 && dscalculate.Tables[0].Rows.Count > 0)
                    {
                        CalMark = 0;
                        Calmaxmark = 0;
                        Calgetmark = 0;
                        for (int calcount = 0; dscalculate.Tables[0].Rows.Count > calcount; calcount++)
                        {
                            calcountvalue++;
                            string calculation_option = dscalculate.Tables[0].Rows[calcount]["calculation_option"].ToString();
                            string includefinal = dscalculate.Tables[0].Rows[calcount]["Include_Final_Calculation"].ToString();
                            string Calconvertvalue = dscalculate.Tables[0].Rows[calcount]["Conversion_Value"].ToString();
                            string calculation_criteria = dscalculate.Tables[0].Rows[calcount]["calculation_criteria"].ToString();
                            string round = dscalculate.Tables[0].Rows[calcount]["Round_Value"].ToString();
                            string sumcriteria = dscalculate.Tables[0].Rows[calcount]["sum_select_criteria"].ToString().Trim();

                            if (Calconvertvalue != "" && Calconvertvalue != "0")
                            {
                                CalConvert = Convert.ToDouble(Calconvertvalue);
                            }

                            string[] spiltcam = calculation_option.Split(',');
                            for (int spit = 0; spit <= spiltcam.GetUpperBound(0); spit++)
                            {
                                DataSet dscalucamgetdetails = new DataSet();
                                calcam = spiltcam[spit].ToString();

                                if (sumcriteria.Trim() == "0")
                                {
                                    if (calcam == "Attendance")
                                    {
                                        //added by Mullai
                                        string batch = string.Empty;
                                        string degree = string.Empty;
                                        string sem = string.Empty;
                                        string fromdatevalue = string.Empty;
                                        string todatevalue = string.Empty;

                                        string getbatch = " select batch_year,degree_code,semester from syllabus_master where syll_code=" + syllcode + "";
                                        ds.Reset();
                                        ds.Dispose();
                                        ds = da.select_method(getbatch, hat, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            batch = ds.Tables[0].Rows[0]["Batch_year"].ToString();
                                            degree = ds.Tables[0].Rows[0]["degree_code"].ToString();
                                            sem = ds.Tables[0].Rows[0]["semester"].ToString();
                                        }
                                        string strAttendance = "select distinct CONVERT(varchar(20),fromDate,103) as fdate ,CONVERT(varchar(20),todate,103) as tdate from AttendanceMarkEntry ae,AttendanceMarkValue av where ae.AttndId=av.AttndId and ae.BathYear='" + batch + "' and DegreeCode='" + degree + "' and semester='" + sem + "'";
                                        DataTable dtAttnd = dir.selectDataTable(strAttendance);
                                        if (dtAttnd.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtAttnd.Rows)
                                            {
                                                string fdate = Convert.ToString(dr["fdate"]);
                                                string todate = Convert.ToString(dr["tdate"]);
                                                fromdatevalue = fdate;
                                                todatevalue = todate;

                                                string attnd = string.Empty;
                                                if (chkbasedSettings.Checked)
                                                    attnd = "Attendance" + fromdatevalue + "-" + todatevalue;
                                                else
                                                    attnd = "attendance";

                                                ///
                                                string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + attnd + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                                dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                                if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                                    {
                                                        //Calmaxmark = 0;
                                                        string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                        string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                        string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                        if (Mark.Trim() != "0" && Mark != "-1")
                                                        {
                                                            CalMark = CalMark + Convert.ToDouble(Mark);
                                                        }
                                                        Calmaxmark = Calmaxmark + Convert.ToDouble(conversionvalue);

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string attnd = string.Empty;
                                            attnd = "attendance";
                                            ///
                                            string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + attnd + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                            dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                            if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                                {
                                                    //Calmaxmark = 0;
                                                    string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                    string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                    string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                    if (Mark.Trim() != "0" && Mark != "-1")
                                                    {
                                                        CalMark = CalMark + Convert.ToDouble(Mark);
                                                    }
                                                    Calmaxmark = Calmaxmark + Convert.ToDouble(conversionvalue);

                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + calcam + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                        dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                        if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                            {
                                                string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                if (Mark.Trim() != "0" && Mark != "-1")
                                                {
                                                    CalMark = CalMark + Convert.ToDouble(Mark);
                                                }
                                                Calmaxmark = Calmaxmark + Convert.ToDouble(conversionvalue);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (calcam == "Attendance")
                                    {
                                        //added by Mullai
                                        string batch = string.Empty;
                                        string degree = string.Empty;
                                        string sem = string.Empty;
                                        string fromdatevalue = string.Empty;
                                        string todatevalue = string.Empty;

                                        string getbatch = " select batch_year,degree_code,semester from syllabus_master where syll_code=" + syllcode + "";
                                        ds.Reset();
                                        ds.Dispose();
                                        ds = da.select_method(getbatch, hat, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            batch = ds.Tables[0].Rows[0]["Batch_year"].ToString();
                                            degree = ds.Tables[0].Rows[0]["degree_code"].ToString();
                                            sem = ds.Tables[0].Rows[0]["semester"].ToString();
                                        }
                                        string strAttendance = "select distinct CONVERT(varchar(20),fromDate,103) as fdate ,CONVERT(varchar(20),todate,103) as tdate from AttendanceMarkEntry ae,AttendanceMarkValue av where ae.AttndId=av.AttndId and ae.BathYear='" + batch + "' and DegreeCode='" + degree + "' and semester='" + sem + "'";
                                        DataTable dtAttnd = dir.selectDataTable(strAttendance);
                                        if (dtAttnd.Rows.Count > 0)
                                        {
                                            foreach (DataRow dr in dtAttnd.Rows)
                                            {
                                                string fdate = Convert.ToString(dr["fdate"]);
                                                string todate = Convert.ToString(dr["tdate"]);
                                                fromdatevalue = fdate;
                                                todatevalue = todate;

                                                string attnd = string.Empty;
                                                if (chkbasedSettings.Checked)
                                                    attnd = "Attendance" + fromdatevalue + "-" + todatevalue;
                                                else
                                                    attnd = "attendance";

                                                string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + attnd + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                                dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                                if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                                    {
                                                        string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                        string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                        string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                        Calgetmark = Calgetmark + Convert.ToDouble(Mark);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string attnd = string.Empty;
                                            attnd = "attendance";
                                            string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + attnd + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                            dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                            if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                            {
                                                for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                                {
                                                    string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                    string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                    string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                    Calgetmark = Calgetmark + Convert.ToDouble(Mark);
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        string strcalcgetdetaisl = "select * from tbl_Cam_Calculation where istype='" + calcam + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no";
                                        dscalucamgetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
                                        if (dscalucamgetdetails.Tables.Count > 0 && dscalucamgetdetails.Tables[0].Rows.Count > 0)
                                        {
                                            for (int cal = 0; cal < dscalucamgetdetails.Tables[0].Rows.Count; cal++)
                                            {
                                                string camroll = dscalucamgetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
                                                string Mark = dscalucamgetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
                                                string conversionvalue = dscalucamgetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
                                                Calgetmark = Calgetmark + Convert.ToDouble(Mark);
                                            }
                                        }
                                    }

                                }
                            }


                            if (sumcriteria.Trim() == "0")
                            {
                                if (CalMark != 0)
                                { //added by Mullai
                                    //    if (calcam == "Attendance")
                                    //    {
                                    //        Calgetmark = CalMark;
                                    //    }   ////
                                    //    else
                                    //    {
                                    Calgetmark = CalMark / Calmaxmark * CalConvert;
                                    //}
                                    if (round == "0" || round.Trim() == "" || round.Trim() == null)
                                    {
                                        Calgetmark = Math.Round(Calgetmark, 0, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        Calgetmark = Math.Round(Calgetmark, Convert.ToInt32(round), MidpointRounding.AwayFromZero);
                                    }
                                }
                                else
                                {
                                    Calgetmark = 0;
                                }
                            }
                            string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','" + dscalculate.Tables[0].Rows[calcount]["Istype"].ToString() + "','" + getroll + "','" + Calgetmark + "','" + CalConvert + "','" + sections + "')";
                            //int p = da.insert_method(insertqurey, hat, "Text");

                            htCamCalculationInsert.Clear();
                            htCamCalculationInsert.Add("@subjectNo", subjectno);
                            htCamCalculationInsert.Add("@syllCode", syllcode);
                            htCamCalculationInsert.Add("@isType", Convert.ToString(dscalculate.Tables[0].Rows[calcount]["Istype"]).Trim());
                            //htCamCalculationInsert.Add("@camOptions", null);
                            htCamCalculationInsert.Add("@rollNo", getroll);
                            htCamCalculationInsert.Add("@marks", Calgetmark);
                            htCamCalculationInsert.Add("@convertion", CalConvert);
                            htCamCalculationInsert.Add("@Sections", sections);
                            int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                            if (dscalculate.Tables[0].Rows.Count - 1 == calcount)//Rajkumar
                            {
                                //===============Final InternalMark=============================
                                Double outof100 = 0;
                                Double finalinternalmark = 0;
                                if (Calgetmark != 0)
                                {
                                    if (sumcriteria.Trim() == "0")
                                    {
                                        finalinternalmark = Calgetmark / CalConvert * subjectmaxinternalmark;
                                        if (int.TryParse(subjectmaxinternalmark.ToString(), out p))
                                        {

                                            //added by madhumathi=========
                                            if (chkRound100.Checked)
                                            {
                                                if (chkroundoff.Items[0].Selected == true)//magesh 8.9.18
                                                {
                                                    finalinternalmark = Math.Round(finalinternalmark, 0, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                    finalinternalmark = Math.Round(finalinternalmark, 1, MidpointRounding.AwayFromZero);
                                            }
                                            else
                                                finalinternalmark = Math.Round(finalinternalmark, 1, MidpointRounding.AwayFromZero);
                                            //=============================//
                                        }
                                        else
                                        {
                                            finalinternalmark = Math.Round(finalinternalmark, 1, MidpointRounding.AwayFromZero);
                                        }
                                        if (intmrksetting == "1")
                                        {
                                            outof100 = Calgetmark / CalConvert * 100;
                                        }
                                        else if (intmrksetting == "2")
                                        {
                                            outof100 = finalinternalmark / CalConvert * 100;

                                        }
                                        //if (true)   
                                        //{
                                        //    outof100 = finalinternalmark / subjectmaxinternalmark * 100;
                                        //}
                                        if (chkRound100.Checked)//Rajkumar
                                        {
                                            if (chkroundoff.Items[1].Selected == true)//magesh 8.9.18
                                            {
                                                outof100 = Math.Round(outof100, 0, MidpointRounding.AwayFromZero);
                                            }

                                            else
                                                outof100 = Math.Round(outof100, 1, MidpointRounding.AwayFromZero);
                                        }
                                        else if (CalConvert == 100)
                                            outof100 = Math.Round(outof100, 0, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        string value = da.GetFunction("select Exammark from tbl_cam_calculation where istype='" + dscalculate.Tables[0].Rows[calcount]["Istype"].ToString() + "' and subject_no='" + subjectno + "' and roll_no='" + getroll + "' order by Roll_no");
                                        finalinternalmark = Calgetmark + Convert.ToDouble(value);
                                        outof100 = finalinternalmark * 2;
                                    }
                                }

                                string insertqurey50 = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Internal Marks Out of " + subjectmaxinternalmark + "','" + getroll + "','" + finalinternalmark + "','" + subjectmaxinternalmark + "','" + sections + "')";

                                htCamCalculationInsert.Clear();
                                htCamCalculationInsert.Add("@subjectNo", subjectno);
                                htCamCalculationInsert.Add("@syllCode", syllcode);
                                htCamCalculationInsert.Add("@isType", "Internal Marks Out of " + subjectmaxinternalmark);
                                //htCamCalculationInsert.Add("@camOptions", null);
                                htCamCalculationInsert.Add("@rollNo", getroll);
                                htCamCalculationInsert.Add("@marks", finalinternalmark);
                                htCamCalculationInsert.Add("@convertion", subjectmaxinternalmark);
                                htCamCalculationInsert.Add("@Sections", sections);
                                //int p = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");

                                if (!int.TryParse(subjectmaxinternalmark.ToString(), out p))
                                {
                                    string setval = finalinternalmark.ToString();
                                    if (!setval.Contains("."))
                                    {
                                        setval = setval + ".0";
                                    }
                                    insertqurey50 = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Internal Marks Out of " + subjectmaxinternalmark + "','" + getroll + "','" + setval + "','" + subjectmaxinternalmark + "','" + sections + "')";
                                    htCamCalculationInsert.Clear();
                                    htCamCalculationInsert.Add("@subjectNo", subjectno);
                                    htCamCalculationInsert.Add("@syllCode", syllcode);
                                    htCamCalculationInsert.Add("@isType", "Internal Marks Out of " + subjectmaxinternalmark);
                                    //htCamCalculationInsert.Add("@camOptions", null);
                                    htCamCalculationInsert.Add("@rollNo", getroll);
                                    htCamCalculationInsert.Add("@marks", setval);
                                    htCamCalculationInsert.Add("@convertion", subjectmaxinternalmark);
                                    htCamCalculationInsert.Add("@Sections", sections);

                                }
                                //int d50 = da.insert_method(insertqurey50, hat, "Text");
                                int d50 = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");

                                string insertqurey100 = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Internal Marks Out of 100','" + getroll + "','" + outof100 + "','" + 100 + "','" + sections + "')";
                                htCamCalculationInsert.Clear();
                                htCamCalculationInsert.Add("@subjectNo", subjectno);
                                htCamCalculationInsert.Add("@syllCode", syllcode);
                                htCamCalculationInsert.Add("@isType", "Internal Marks Out of 100");
                                //htCamCalculationInsert.Add("@camOptions", null);
                                htCamCalculationInsert.Add("@rollNo", getroll);
                                htCamCalculationInsert.Add("@marks", outof100);
                                htCamCalculationInsert.Add("@convertion", 100);
                                htCamCalculationInsert.Add("@Sections", sections);
                                //int e1 = da.insert_method(insertqurey100, hat, "Text");
                                int e1 = da.insert_method("usp_InsertCAMCalculationMark", htCamCalculationInsert, "sp");
                                string Finalinsertmethod = "";
                                string deletemrks = "delete camarks where subject_no='" + subjectno + "' and roll_no='" + getroll + "'";
                                int a = da.update_method_wo_parameter(deletemrks, "text");
                                if (exam_code.ToString().Trim() != "" && exam_code.ToString().Trim() != "0" && exam_code.ToString().Trim() != "-1")
                                {
                                    // Finalinsertmethod = "insert into camarks (subject_no,roll_no,total,exam_code,sections) values ('" + subjectno + "','" + getroll + "','" + finalinternalmark + "','" + exam_code + "','" + sections + "')"; ///commanded by madhumathi

                                    Finalinsertmethod = "insert into camarks (subject_no,roll_no,total,exam_code,sections) values ('" + subjectno + "','" + getroll + "','" + Calgetmark + "','" + exam_code + "','" + sections + "')";  // altered by madhumathi 23/04/2018
                                    htCamFinalInternalInsert.Clear();
                                    htCamFinalInternalInsert.Add("@subjectNo", subjectno);
                                    htCamFinalInternalInsert.Add("@rollNo", getroll);
                                    htCamFinalInternalInsert.Add("@marks", Calgetmark);// finalinternalmark altered by madhumathi 23/04/2018
                                    //htCamFinalInternalInsert.Add("@examMonth", null);
                                    //htCamFinalInternalInsert.Add("@examYear", null);
                                    htCamFinalInternalInsert.Add("@Sections", sections);
                                    htCamFinalInternalInsert.Add("@examCode", exam_code);
                                }
                                else
                                {
                                    // Finalinsertmethod = "insert into camarks (subject_no,roll_no,total,sections,Exam_Month,Exam_year) values ('" + subjectno + "','" + getroll + "','" + finalinternalmark + "','" + sections + "','" + ddlexammonth.SelectedValue.ToString() + "','" + ddlexamyear.SelectedValue.ToString() + "')"; ///commanded by madhumathi

                                    Finalinsertmethod = "insert into camarks (subject_no,roll_no,total,sections,Exam_Month,Exam_year) values ('" + subjectno + "','" + getroll + "','" + Calgetmark + "','" + sections + "','" + ddlexammonth.SelectedValue.ToString() + "','" + ddlexamyear.SelectedValue.ToString() + "')";   // altered by madhumathi 23/04/2018 
                                    htCamFinalInternalInsert.Clear();
                                    htCamFinalInternalInsert.Add("@subjectNo", subjectno);
                                    htCamFinalInternalInsert.Add("@rollNo", getroll);
                                    htCamFinalInternalInsert.Add("@marks", Calgetmark);  // finalinternalmark altered by madhumathi 23/04/2018 
                                    htCamFinalInternalInsert.Add("@examMonth", Convert.ToString(ddlexammonth.SelectedValue).Trim());
                                    htCamFinalInternalInsert.Add("@examYear", Convert.ToString(ddlexamyear.SelectedValue).Trim());
                                    htCamFinalInternalInsert.Add("@Sections", sections);
                                    //htCamFinalInternalInsert.Add("@examCode", null);
                                }
                                if (htCamFinalInternalInsert.Count > 0)
                                {
                                    int q = da.insert_method("usp_InsertCAMFinalInternal", htCamFinalInternalInsert, "sp");
                                }
                            }
                        }
                    }
                }
                enableflag = true;


            }

            #region PreviousMethod

            //Double CalConvert = 0;
            //DataSet dscalugetdetails;
            //string strcalcgetdetaisl = "";
            //strquery = "Select distinct * from internal_cam_calculation_master_setting where Istype='Calculation' and subject_no=" + subjectno + " and syll_code=" + syllcode + " order by subject_no,syll_code,Istype asc";
            //dscalculate = da.select_method(strquery, hat, "Text");
            //if (dscalculate.Tables[0].Rows.Count > 0)
            //{
            //    for (int calcount = 0; calcount < dscalculate.Tables[0].Rows.Count; calcount++)
            //    {
            //        string calculation_option = dscalculate.Tables[0].Rows[calcount]["calculation_option"].ToString();
            //        string includefinal = dscalculate.Tables[0].Rows[calcount]["Include_Final_Calculation"].ToString();
            //        string Calconvertvalue = dscalculate.Tables[0].Rows[calcount]["Conversion_Value"].ToString();
            //        string calculation_criteria = dscalculate.Tables[0].Rows[calcount]["calculation_criteria"].ToString();
            //        if (Calconvertvalue != "" && Calconvertvalue != "0")
            //        {
            //            CalConvert = Convert.ToDouble(Calconvertvalue);
            //        }
            //        string[] spiltcam = calculation_option.Split(',');
            //        for (int spit = 0; spit <= spiltcam.GetUpperBound(0); spit++)
            //        {
            //            string calcam = spiltcam[spit].ToString();
            //            if (calcam == "1")
            //            {
            //                strcalcgetdetaisl = "select distinct * from tbl_Cam_Calculation where istype='cam' and cam_option=1 and subject_no='" + subjectno + "' order by Roll_no";
            //                dscalugetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
            //                for (int cal = 0; cal < dscalugetdetails.Tables[0].Rows.Count; cal++)
            //                {
            //                    string calroll = dscalugetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
            //                    string Mark = dscalugetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
            //                    string conversionvalue = dscalugetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
            //                    Double CalMark = 0;
            //                    Double Exma = 0;
            //                    Double Getmark = 0;
            //                    if (Mark != "" && Mark != "0")
            //                    {
            //                        Exma = Convert.ToDouble(Mark);
            //                    }
            //                    if (conversionvalue != "" && conversionvalue != "0")
            //                    {
            //                        CalMark = Convert.ToDouble(conversionvalue);
            //                    }
            //                    Getmark = Exma / CalMark * CalConvert;
            //                    Getmark = Math.Round(Getmark, 0);
            //                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,istype,Cam_option,roll_no,Exammark,conversion) values (" + subjectno + ",'Cal "+calculation_criteria+"'," + calcam + ",'" + calroll + "','" + Getmark + "'," + CalConvert + ")";
            //                    int p = da.insert_method(insertqurey, hat, "Text");
            //                }
            //            }
            //            else if (calcam == "2")
            //            {
            //                strcalcgetdetaisl = "select distinct * from tbl_Cam_Calculation where istype='cam' and cam_option=2 and subject_no='" + subjectno + "' order by Roll_no";
            //                dscalugetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
            //                for (int cal = 0; cal < dscalugetdetails.Tables[0].Rows.Count; cal++)
            //                {
            //                    string calroll = dscalugetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
            //                    string Mark = dscalugetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
            //                    string conversionvalue = dscalugetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
            //                    Double CalMark = 0;
            //                    Double Exma = 0;
            //                    Double Getmark = 0;
            //                    if (Mark != "" && Mark != "0")
            //                    {
            //                        Exma = Convert.ToDouble(Mark);
            //                    }
            //                    if (conversionvalue != "" && conversionvalue != "0")
            //                    {
            //                        CalMark = Convert.ToDouble(conversionvalue);
            //                    }
            //                    Getmark = Exma / CalMark * CalConvert;
            //                    Getmark = Math.Round(Getmark, 0);
            //                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,istype,Cam_option,roll_no,Exammark,conversion) values (" + subjectno + ",'Cal " + calculation_criteria + "'," + calcam + ",'" + calroll + "','" + Getmark + "'," + CalConvert + ")";
            //                    int p = da.insert_method(insertqurey, hat, "Text");
            //                }
            //            }
            //            else if (calcam == "3")
            //            {
            //                strcalcgetdetaisl = "select distinct * from tbl_Cam_Calculation where istype='cam' and cam_option=3 and subject_no='" + subjectno + "' order by Roll_no";
            //                dscalugetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
            //                for (int cal = 0; cal < dscalugetdetails.Tables[0].Rows.Count; cal++)
            //                {
            //                    string calroll = dscalugetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
            //                    string Mark = dscalugetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
            //                    string conversionvalue = dscalugetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
            //                    Double CalMark = 0;
            //                    Double Exma = 0;
            //                    Double Getmark = 0;
            //                    if (Mark != "" && Mark != "0")
            //                    {
            //                        Exma = Convert.ToDouble(Mark);
            //                    }
            //                    if (conversionvalue != "" && conversionvalue != "0")
            //                    {
            //                        CalMark = Convert.ToDouble(conversionvalue);
            //                    }
            //                    Getmark = Exma / CalMark * CalConvert;
            //                    Getmark = Math.Round(Getmark, 0);
            //                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,istype,Cam_option,roll_no,Exammark,conversion) values (" + subjectno + ",'Cal " + calculation_criteria + "'," + calcam + ",'" + calroll + "','" + Getmark + "'," + CalConvert + ")";
            //                    int p = da.insert_method(insertqurey, hat, "Text");
            //                }
            //            }
            //            else if (calcam == "4")
            //            {
            //                strcalcgetdetaisl = "select distinct * from tbl_Cam_Calculation where istype='cam' and cam_option=4 and subject_no='" + subjectno + "' order by Roll_no";
            //                dscalugetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
            //                for (int cal = 0; cal < dscalugetdetails.Tables[0].Rows.Count; cal++)
            //                {
            //                    string calroll = dscalugetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
            //                    string Mark = dscalugetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
            //                    string conversionvalue = dscalugetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
            //                    Double CalMark = 0;
            //                    Double Exma = 0;
            //                    Double Getmark = 0;
            //                    if (Mark != "" && Mark != "0")
            //                    {
            //                        Exma = Convert.ToDouble(Mark);
            //                    }
            //                    if (conversionvalue != "" && conversionvalue != "0")
            //                    {
            //                        CalMark = Convert.ToDouble(conversionvalue);
            //                    }
            //                    Getmark = Exma / CalMark * CalConvert;
            //                    Getmark = Math.Round(Getmark, 0);
            //                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,istype,Cam_option,roll_no,Exammark,conversion) values (" + subjectno + ",'Cal " + calculation_criteria + "'," + calcam + ",'" + calroll + "','" + Getmark + "'," + CalConvert + ")";
            //                    int p = da.insert_method(insertqurey, hat, "Text");
            //                }
            //            }
            //            else if (calcam == "Attendance")
            //            {

            //                string getcalc = da.GetFunction("select Att_mark_per from internal_cam_calculation_master_setting where subject_no="+subjectno+" and istype='attendance'");
            //                strcalcgetdetaisl = "select distinct * from tbl_Cam_Calculation where Istype='Attendancevalue' and subject_no='" + subjectno + "' order by Roll_no";
            //                dscalugetdetails = da.select_method(strcalcgetdetaisl, hat, "Text");
            //                for (int cal = 0; cal < dscalugetdetails.Tables[0].Rows.Count; cal++)
            //                {
            //                    calcam = "10";
            //                    string calroll = dscalugetdetails.Tables[0].Rows[cal]["Roll_no"].ToString();
            //                    string Mark = dscalugetdetails.Tables[0].Rows[cal]["Exammark"].ToString();
            //                    string conversionvalue = dscalugetdetails.Tables[0].Rows[cal]["Conversion"].ToString();
            //                    Double CalMark = 0;
            //                    Double Exma = 0;
            //                    Double Getmark = 0;
            //                        if (Mark != "" && Mark != "0")
            //                        {
            //                            Exma = Convert.ToDouble(Mark);
            //                        }
            //                        if (conversionvalue != "" && conversionvalue != "0")
            //                        {
            //                            CalMark = Convert.ToDouble(conversionvalue);
            //                        }
            //                    Getmark = Exma / CalMark * CalConvert;
            //                    Getmark = Math.Round(Getmark, 0);
            //                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,istype,Cam_option,roll_no,Exammark,conversion) values (" + subjectno + ",'Cal " + calculation_criteria + "'," + calcam + ",'" + calroll + "','" + Getmark + "'," + CalConvert + ")";
            //                    int p = da.insert_method(insertqurey, hat, "Text");
            //                }
            //            }

            //        }
            //    }
            //}
            //============================Get Internal Marks===================================
            //ds.Reset();
            //ds.Dispose();
            //string finalcalculationquery = "select distinct roll_no from tbl_Cam_Calculation where subject_no=" + subjectno + " order by roll_no";
            //ds = da.select_method(finalcalculationquery, hat, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int str = 0; str < ds.Tables[0].Rows.Count; str++)
            //    {
            //        string roll = ds.Tables[0].Rows[str]["roll_no"].ToString();
            //        string sections = da.GetFunction("select distinct sections from registration where roll_no='" + roll + "'");
            //        string exam_code = da.GetFunction("select exam_code from exam_details e,syllabus_master s where s.batch_year=e.batch_year and s.degree_code=e.degree_code and s.semester=e.current_semester and syll_code=" + syllcode + "");
            //        Double finalmark = 0;
            //        Double getmarkvalue = 0;
            //        Double getconversvalu = 0;
            //        string getmark = "select  distinct roll_no,conversion,exammark,subject_no,cam_option from tbl_Cam_Calculation where  subject_no=" + subjectno + " and istype like'cal%' order by roll_no";
            //        DataSet dsgetcalculation = da.select_method(getmark, hat, "Text");
            //        for (int i = 0; i < dsgetcalculation.Tables[0].Rows.Count; i++)
            //        {
            //            string convers = dsgetcalculation.Tables[0].Rows[i]["conversion"].ToString();
            //            string mark = dsgetcalculation.Tables[0].Rows[i]["exammark"].ToString();
            //            Double getmarkvalue1 = 0;
            //            Double getconversvalu1 = 0;
            //            if (convers != "")
            //            {
            //                getconversvalu1 = Convert.ToDouble(convers);
            //                getconversvalu = getconversvalu + getconversvalu1;
            //            }
            //            if (mark != "")
            //            {
            //                getmarkvalue1 = Convert.ToDouble(mark);
            //                getmarkvalue = getmarkvalue + getmarkvalue1;
            //            }
            //        }
            //        finalmark = getmarkvalue / getconversvalu;
            //        finalmark = Math.Round(finalmark, 0);
            //        string insertmethod = "";
            //        if (exam_code.ToString().Trim() != "" && exam_code.ToString().Trim() != "0" && exam_code.ToString().Trim() != "-1")
            //        {
            //            insertmethod = "insert into camrarks (subject_no,roll_no,total,exam_code,sections) values (" + subjectno + ",'" + roll + "'," + finalmark + "," + exam_code + ",'" + sections + "')";
            //        }
            //        else
            //        {
            //            insertmethod = "insert into camrarks (subject_no,roll_no,total,sections,Exam_Month,Exam_year) values (" + subjectno + ",'" + roll + "'," + finalmark + "," + exam_code + ",'" + sections + "','" + ddlexammonth.SelectedValue.ToString() + "','" + ddlexamyear.SelectedValue.ToString() + "')";
            //        }
            //    }
            //}
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student Cam Internal Marks Added Successfully')", true);
            #endregion

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void btnview_OnClick(object sender, EventArgs e)
    {
        Button selview = (Button)sender;
        string rowindx = selview.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowindex = Convert.ToInt32(rowindx) - 2;
        Session["rowIndex"] = rowindex.ToString();
        Label sylcod = (Label)GridView1.Rows[rowindex].FindControl("lblsyllcode");
        syllcode = sylcod.Text;
        Label subno = (Label)GridView1.Rows[rowindex].FindControl("lblsubno");
        subjectno = subno.Text;
        panel4.Visible = false;
        Label degdet = (Label)GridView1.Rows[rowindex].FindControl("lbldegree");
        string degreedetails = degdet.Text;
        chkattendance.Checked = false;
        chkSubSub.Checked = false;
        txtcriteria.Text = "";
        txtcalculate.Text = "";
        loadcalculationdetais(degreedetails);
    }
    public void loadcalculationdetais(string degreedetails)
    {
        try
        {
            //ShowGradeDetails = false;
            #region Added By Malang Raja T on Dec 2 2016 for SV

            ShowGradeDetails = CheckCAMCalculationGradeSettings();
            int gradeColumn = 0;

            #endregion Added By Malang Raja T on Dec 2 2016 for SV

            errmsg.Visible = false;

            panel4.Visible = false;
            chkattendance.Checked = false;
            chkSubSub.Checked = false;

            //manikandan 03Aug2013======================================================
            string rowindexval = Convert.ToString(Session["rowIndex"]);
            int rowval = Convert.ToInt32(rowindexval);

            Dictionary<int, string> dichd = new Dictionary<int, string>();
            int dcval = 0;
            dtview.Clear();
            dtview.Columns.Add("S.No");
            dcval++;
            dichd.Add(dcval, "S.No");
            dtview.Columns.Add("Roll No");
            dcval++;
            dichd.Add(dcval, "Roll No");
            dtview.Columns.Add("Reg No");
            dcval++;
            dichd.Add(dcval, "Reg No");
            dtview.Columns.Add("Student Name");
            dcval++;
            dichd.Add(dcval, "Student Name");

            Label syl = (Label)GridView1.Rows[rowval].FindControl("lblsyllcode");
            string syllabus = syl.Text;

            con.Close();
            con.Open();

            //added By Srinath 15/8/2013
            strorder = "ORDER BY len(reg.roll_no),reg.roll_no";
            string strserial = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (strserial != "" && strserial != "0" && strserial != null)
            {
                strorder = "ORDER BY reg.serialno";
            }
            else
            {
                string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");

                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY len(reg.roll_no),reg.roll_no";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY reg.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY reg.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY len(reg.roll_no),reg.roll_no,reg.Reg_No,reg.stud_name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY len(reg.roll_no),reg.roll_no,reg.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY reg.Reg_No,reg.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY len(reg.roll_no),reg.roll_no,reg.Stud_Name";
                }
            }

            Label sec = (Label)GridView1.Rows[rowval].FindControl("lblsection");
            string sections = sec.Text;
            string sectionval = string.Empty;
            string strsecval = string.Empty;
            if (sections.Trim() != "" && sections != "0" && sections != "-1" && sections.Trim().ToLower() != "all")
            {
                strsecval = " and sections='" + sections + "'";
                sectionval = " and reg.sections='" + sections + "'";
            }

            SqlDataAdapter dagetdegree = new SqlDataAdapter("select batch_year,degree_code,semester from syllabus_master where syll_code=" + syllabus + "", con);
            DataTable dtgetdegree = new DataTable();
            dagetdegree.Fill(dtgetdegree);


            SqlDataAdapter daattend = new SqlDataAdapter("select * from periodattndschedule where degree_code=" + dtgetdegree.Rows[0]["degree_code"].ToString() + " and semester=" + dtgetdegree.Rows[0]["semester"].ToString() + "", con);
            DataTable dtattend = new DataTable();
            daattend.Fill(dtattend);

            DataSet dsGradeDetails = new DataSet();

            #region Added By Malang Raja on Dec 2 2016 for SV

            if (dtgetdegree.Rows.Count > 0)
            {
                string batchYear = Convert.ToString(dtgetdegree.Rows[0]["batch_year"]).Trim();
                string degreeCode = Convert.ToString(dtgetdegree.Rows[0]["degree_code"]).Trim();
                string semester = Convert.ToString(dtgetdegree.Rows[0]["semester"]).Trim();
                string qryGrade = "select Frange,Trange,Mark_Grade,Credit_Points from Grade_Master where Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and Semester='" + semester + "' and Criteria='General' order by Frange asc; select Frange,Trange,Mark_Grade,Credit_Points from Grade_Master where Degree_Code='" + degreeCode + "' and batch_year='" + batchYear + "' and College_Code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and Semester='0' and Criteria='General' order by Frange asc;";
                dsGradeDetails = da.select_method_wo_parameter(qryGrade, "Text");
            }

            #endregion Added By Malang Raja on Dec 2 2016 for SV

            //==========================================================================
            //string issettings = "";
            if (chkbasedSettings.Checked)//Rajkumar 23/3/2018
            {
                string strheadquery = "select istype,conversion_value from internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and istype<>'Settings' " + strsecval + " order by idno";
                ds.Reset();
                ds.Dispose();
                ds = da.select_method(strheadquery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int head = 0; head < ds.Tables[0].Rows.Count; head++)
                    {
                        string cam = ds.Tables[0].Rows[head]["istype"].ToString();
                        string value = string.Empty;
                        if (cam.Trim() == "Attendance")//manikandan 03Aug2013
                        {
                            value = dtattend.Rows[0]["atnd_mark_total"].ToString();
                            string isstring = "select distinct  Istype,conversion  from tbl_cam_calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and Istype like '%Attendance%'";
                            DataTable dtattnds = dir.selectDataTable(isstring);
                            if (dtattnds.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtattnds.Rows)
                                {
                                    string attendancestr = Convert.ToString(dr["Istype"]);
                                    string convert = Convert.ToString(dr["conversion"]);
                                    string colnam = "" + attendancestr + "(" + convert + ")";
                                    dtview.Columns.Add(colnam);
                                    dcval++;
                                    dichd.Add(dcval, colnam);
                                }
                            }
                        }
                        else
                        {
                            value = ds.Tables[0].Rows[head]["conversion_value"].ToString();
                            string colmnam = "" + cam + "(" + value + ")";
                            dtview.Columns.Add(colmnam);
                            dcval++;
                            dichd.Add(dcval, colmnam);

                        }
                    }
                }
            }
            else
            {
                string strheadquery = "select istype,conversion_value from internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and istype<>'Settings' " + strsecval + " order by idno";
                ds.Reset();
                ds.Dispose();
                ds = da.select_method(strheadquery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int head = 0; head < ds.Tables[0].Rows.Count; head++)
                    {
                        string cam = ds.Tables[0].Rows[head]["istype"].ToString();
                        string value = string.Empty;
                        if (cam.Trim() == "Attendance")//manikandan 03Aug2013
                        {
                            value = dtattend.Rows[0]["atnd_mark_total"].ToString();//manikandan 03Aug2013//get atten tot val
                        }
                        else
                        {
                            value = ds.Tables[0].Rows[head]["conversion_value"].ToString();
                        }
                        string colname = "" + cam + "(" + value + ")";
                        dtview.Columns.Add(colname);
                        dcval++;
                        dichd.Add(dcval, colname);

                    }
                }
            }


            Double subjectmaxinternalmark = Convert.ToDouble(da.GetFunction("select max_int_marks from subject where subject_no=" + subjectno + ""));
            string selval = da.GetFunction("select sum_select_criteria from internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and Istype='settings'  " + strsecval + "");
            //int forschoolcheck = Fpinternaldetails.Sheets[0].ColumnCount - 1;
            // forschoolcheck.Add(Fpinternaldetails.Sheets[0].ColumnCount - 1);
            StringBuilder columnname = new StringBuilder();
            if (selval == "2")
            {
                dtview.Columns.Add("Internal Marks Out of " + subjectmaxinternalmark + "");
                dcval++;
                dichd.Add(dcval, "Internal Marks Out of " + subjectmaxinternalmark + "");            
                internalmarkname = new System.Text.StringBuilder("Internal Marks Out of 100");
                AddTableColumn(dtview, internalmarkname);
               // AddTableColumn(dichd, internalmarkname);
               // dtview.Columns.Add("Internal Marks Out of 100");
                dcval++;

                dichd.Add(dcval, "Internal Marks Out of 100");

            }
            else
            {
                dtview.Columns.Add("Internal Marks Out of " + subjectmaxinternalmark + "");
                dcval++;
                dichd.Add(dcval, "Internal Marks Out of " + subjectmaxinternalmark + "");
                internalmarkname = new System.Text.StringBuilder("Internal Marks Out of 100");
                AddTableColumn(dtview, internalmarkname);
               // dtview.Columns.Add("Internal Marks Out of 100");
                dcval++;
                dichd.Add(dcval, "Internal Marks Out of 100");
            }

            string[] splitdetails = degreedetails.Split('-');
            string Batch = splitdetails[0].ToString().Trim();
            string degree = splitdetails[1].ToString().Trim();
            string sem = splitdetails[2].ToString().Trim();
            string subject = "", Subcode = "";
            DataSet dssubject = da.select_method_wo_parameter("select subject_name,subject_code from subject where subject_no=" + subjectno + "", "Text");
            if (dssubject.Tables[0].Rows.Count > 0)
            {
                subject = dssubject.Tables[0].Rows[0]["subject_name"].ToString();
                Subcode = dssubject.Tables[0].Rows[0]["subject_code"].ToString();
            }
            dicheader.Clear();
            int ky = 0;
            if (ckhdegreewise.Checked == true)
            {
                drview = dtview.NewRow();
                if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString().ToLower() != "all" && ddlsec.SelectedValue.ToString() != "-1" && ddlsec.Enabled == true)
                {

                    dicheader.Add(ky, "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "  Sec : " + ddlsec.SelectedValue.ToString() + "");
                    drview["S.No"] = "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "  Sec : " + ddlsec.SelectedValue.ToString() + "";

                }
                else
                {
                    dicheader.Add(ky, "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "");
                    drview["S.No"] = "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "";
                }
                dtview.Rows.Add(drview);
            }
            else if (ckhdegreewise.Checked == false)
            {
                drview = dtview.NewRow();
                ky++;
                dicheader.Add(ky, "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "");
                drview["S.No"] = "Batch : " + Batch + "  Degree : " + degree + "  Sem : " + sem + "  Subject Code - Name  : " + Subcode + "-" + subject + "";
                dtview.Rows.Add(drview);
            }
            drview = dtview.NewRow();
            foreach (KeyValuePair<int, string> valhr in dichd)
            {
                string hdnam = valhr.Value;
                int colnum=valhr.Key;
                drview[colnum-1] = hdnam;
            }
            dtview.Rows.Add(drview);





            DataSet dsmark = da.select_method_wo_parameter("select * from tbl_Cam_Calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + " order by Istype", "Text");
            DataTable dtmar = dsmark.Tables[0];
            int sno = 0;
            for (int val = 2; val < dtview.Columns.Count; val++)
            {
                string column2 = dtview.Columns[val].ColumnName.ToString();
                string[] column5 = column2.Split('(');
                string column = column5[0];
                if (!chkbasedSettings.Checked)
                {
                    if (column == "Attendance")
                    {
                        column = "Attendance";
                    }
                }
                else
                {
                    if (column == "Attendance")
                    {
                        column = "AttendanceValue";
                    }
                }

                if (val == 2)
                {
                    string strheadquery = "select distinct reg.roll_no,reg.reg_no,reg.stud_name,reg.serialno,len(reg.roll_no) from tbl_cam_calculation c,registration reg where reg.roll_no=c.roll_no and subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and reg.cc=0 and reg.delflag=0 and reg.exam_flag<>'Debar' " + sectionval + " " + strorder + "";
                    ds.Reset();
                    ds.Dispose();
                    ds = da.select_method(strheadquery, hat, "Text");
                }
                else
                {

                }
                int sno1 = 0;
                for (int rol = 0; rol < ds.Tables[0].Rows.Count; rol++)
                {
                    string roll = ds.Tables[0].Rows[rol]["roll_no"].ToString();
                    if (val == 2)
                    {
                        sno1++;
                        drview = dtview.NewRow();
                        string regno = ds.Tables[0].Rows[rol]["reg_no"].ToString();
                        string name = ds.Tables[0].Rows[rol]["stud_name"].ToString();
                        drview["S.No"] = sno1.ToString();
                        drview["Roll No"] = roll;
                        drview["Reg No"] = regno;
                        drview["Student Name"] = name;
                        dtview.Rows.Add(drview);
                    }
                    else
                    {
                        rol = rol + 2;
                        roll = Convert.ToString(dtview.Rows[rol]["Roll No"]);
                        dtmar.DefaultView.RowFilter = " roll_no='" + roll + "' and istype='" + column + "'";

                        DataView dv_selectedsub = new DataView();
                        dv_selectedsub = dtmar.DefaultView;
                        if (dv_selectedsub.Count > 0)
                        {
                            string mark = dv_selectedsub[0]["exammark"].ToString();
                            dtview.Rows[rol][val] = mark;
                        }
                        rol = rol - 2;

                    }
                }
            }



            #region Added By Malang Raja on Dec 2 2016 for SV

            if (ShowGradeDetails)
            {
                dtview.Columns.Add("Grade");
                dtview.Rows[1]["Grade"] = "Grade";
                gradeColumn = dtview.Columns.Count;
            }

            #endregion Added By Malang Raja on Dec 2 2016 for SV

            if (forschoolsetting == true)
            {
                dtview.Columns.Add("Test Marks In Words");
                dtview.Rows[1]["Test Marks In Words"] = "Test Marks In Words";
            }
            else
            {
                dtview.Columns.Add("Internal Marks In Words");
                dtview.Rows[1]["Internal Marks In Words"] = "Internal Marks In Words";
            }
            for (int i = 2; i < dtview.Rows.Count; i++)
            {
                string mark = dtview.Rows[i][dtview.Columns.Count - ((ShowGradeDetails) ? 3 : 2)].ToString();

                #region Added By Malang Raja on Dec 2 2016 for SV

                if (ShowGradeDetails)
                {
                    string gradeMark = string.Empty;
                    bool hasGrade = false;
                    if (dsGradeDetails.Tables.Count > 0 && dsGradeDetails.Tables[0].Rows.Count > 0)
                    {
                        hasGrade = findgrade(dsGradeDetails.Tables[0], mark, ref gradeMark);
                    }
                    else if (dsGradeDetails.Tables.Count > 1 && dsGradeDetails.Tables[1].Rows.Count > 0)
                    {
                        hasGrade = findgrade(dsGradeDetails.Tables[1], mark, ref gradeMark);
                    }
                    if (hasGrade)
                    {
                        dtview.Rows[i][gradeColumn] = gradeMark;
                    }
                }

                #endregion Added By Malang Raja on Dec 2 2016 for SV

                //if (getword == "2")
                //{
                //    mark = Fpinternaldetails.Sheets[0].Cells[i, Fpinternaldetails.Sheets[0].ColumnCount - 3].Text.ToString();
                //}

                string setmark = getmarksword(mark);
                dtview.Rows[i][dtview.Columns.Count - 1] = setmark;
            }

            if (dtview.Rows.Count > 0)
            {
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                divPrint.Visible = true;
                txtreport.Visible = true;
                lblreportname.Visible = true;
                GridView2.Visible = true;
            }
            else
            {
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                divPrint.Visible = false;
                txtreport.Visible = false;
                lblreportname.Visible = false;
                GridView2.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            lblcriteria.Visible = true;
            txtcriteria.Visible = true;
            int rowcount = dtview.Rows.Count;

            if (dtview.Rows.Count > 0)
            {
                if (forschoolsetting == true)
                {
                    if (forschoolcheck.Count > 0)
                    {
                        int aschool = Convert.ToInt32(forschoolcheck[0].ToString());
                        dtview.Columns.Add("Test Marks Out of " + subjectmaxinternalmark + "");
                        dtview.Columns.Add("Test Marks Out of 100");

                        #region Added By Malang Raja on Nov 21 2016 for Stanes

                        if (selval == "2")
                        {
                            for (int row = 0; row < dtview.Rows.Count; row++)
                            {
                                string outOf100 = Convert.ToString(dtview.Rows[row][aschool]).Trim();
                                string outOfNot100 = Convert.ToString(dtview.Rows[row][aschool - 1]).Trim();
                                dtview.Rows[row][aschool - 1] = outOf100;
                                dtview.Rows[row][aschool] = outOfNot100;
                            }
                        }

                        #endregion Added By Malang Raja on Nov 21 2016 for Stanes

                    }
                }
            }
            GridView2.DataSource = dtview;
            gridview2colcount = dtview.Columns.Count;
            GridView2.DataBind();

            for (int k = 1; k < dtview.Columns.Count; k++)
            {
                GridView2.Rows[0].Cells[k].Visible = false;
            }
            GridView2.Rows[0].Cells[0].ColumnSpan = dtview.Columns.Count;
            GridView2.Rows[1].Cells[dtview.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
            GridView2.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            GridView2.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GridView2.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GridView2.Rows[1].Cells[3].HorizontalAlign = HorizontalAlign.Center;
            if (Session["Rollflag"].ToString() == "0")
            {
                // GridView2.Columns[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                // GridView2.Columns[2].Visible = false;
            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public string getmarksword(string valueget)
    {
        try
        {
            string strNUmber=string.Empty;
            if (valueget != null && valueget.Trim() != "")
            {
                string setmarkword = "";
                string getmarkval = valueget;
                string[] val1 = null;
                string val2 = string.Empty;
                string val3 = string.Empty;
                if (getmarkval.Contains('.'))
                {
                    val1 = getmarkval.Split('.');
                    val2 = Convert.ToString(val1[0]);
                    val3 = Convert.ToString(val1[1]);
                    int mark = 0;
                    int.TryParse(val2, out mark);
                    strNUmber = ConvertNumbertoWords(mark);

                    strNUmber = strNUmber + " " + " Point";

                    if (!string.IsNullOrEmpty(val3))
                    {
                        for (int spilt = 0; spilt < val3.Length; spilt++)
                        {
                            string val = val3[spilt].ToString().Trim().ToLower();
                            if (val == "1")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "One";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " one";
                                }
                            }
                            else if (val == "2")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Two";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " two";
                                }
                            }
                            else if (val == "3")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Three";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " three";
                                }
                            }
                            else if (val == "4")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Four";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " four";
                                }
                            }
                            else if (val == "5")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Five";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " five";
                                }
                            }
                            else if (val == "6")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Six";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " six";
                                }
                            }
                            else if (val == "7")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Seven";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " seven";
                                }
                            }
                            else if (val == "8")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Eight";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " eight";
                                }
                            }
                            else if (val == "9")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Nine";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " nine";
                                }
                            }
                            else if (val == "0")
                            {
                                if (setmarkword == "")
                                {
                                    setmarkword = "Zero";
                                }
                                else
                                {
                                    setmarkword = setmarkword + " zero";
                                }

                            }

                        }
                        strNUmber = strNUmber + " " + setmarkword;
                    }
                    //return strNUmber;
                }
                else
                {
                    int mark = 0;
                    int.TryParse(getmarkval, out mark);
                    strNUmber = ConvertNumbertoWords(mark);
                }
            }
            else
            {
                return "";
            }
            return strNUmber;
        }
        catch
        {
            return "";
        }
    }

    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs ";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred And ";
            number %= 100;
        }
        if (number > 0)
        {
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }


    #region "Attendance Function"

    public void load_attendance(string fromdatevalue, string todatevalue, string batch, string degree, string sem, string section)
    {
        try
        {

            string strhrs = " select no_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,schorder from PeriodAttndSchedule where degree_code=" + degree + " and semester=" + sem + "";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method(strhrs, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                no_of_hrs = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_per_day"]);
                mng_hrs = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
                evng_hrs = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);
                order = ds.Tables[0].Rows[0]["schorder"].ToString();
            }

            has.Clear();
            has.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", has, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);

            string[] fromdatespit = fromdatevalue.Split('/');
            string[] todatespit = todatevalue.Split('/');
            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
            temp_date = spfromdate;
            dt2 = sptodate;

            has.Clear();
            has.Add("from_date", temp_date);
            has.Add("to_date", dt2);
            has.Add("degree_code", degree);
            has.Add("sem", sem);
            has.Add("coll_code", Session["collegecode"].ToString());

            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + temp_date.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + degree + " and semester=" + sem + "";
            DataSet dsholiday = da.select_method(sqlstr_holiday, hat, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            has.Add("iscount", iscount);
            DataSet ds_holi = da.select_method("HOLIDATE_DETAILS_FINE", has, "sp");

            string halforfull = "";
            string mng = "";
            string evng = "";
            string holiday_sched_details = "";
            if (ds_holi.Tables[0].Rows.Count > 0)
            {
                for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                {

                    if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                    {
                        halforfull = "0";
                    }
                    else
                    {
                        halforfull = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                    {
                        mng = "0";
                    }
                    else
                    {
                        mng = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
                    {
                        evng = "0";
                    }
                    else
                    {
                        evng = "1";
                    }

                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                    if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                    {
                        hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                    }
                }
            }

            string hrdetno = "";
            string getsphr = "select distinct date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + degree + " and batch_year=" + batch + " and semester=" + sem + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
            ds_sphr = da.select_method(getsphr, hat, "Text");
            if (ds_sphr.Tables[0].Rows.Count > 0)
            {
                for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                {
                    if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                    {
                        hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                        hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                        ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                    }
                    else
                    {
                        ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                    }
                }
            }

            string rights = da.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
            if (rights == "True" || rights == "true")
            {
                splhr_flag = true;
            }
            //string sectionvalsubatt = "";
            //if (ckhdegreewise.Checked == true)
            //{
            //    if (ddlsec.SelectedValue.ToString().Trim() != "" && ddlsec.SelectedValue.ToString() != "-1" && ddlsec.Enabled == true)
            //    {
            //        sectionvalsubatt = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
            //    }
            //}

            string sections = section;
            string secvalue = "";
            if (sections.Trim() == "" || sections.Trim() == "-1" || sections.ToString().Trim().ToLower() != "all")
            {
                secvalue = "";
            }
            else
            {
                secvalue = " and Sections='" + sections + "'";
            }
            string strrollquer = "select distinct sections from registration where batch_year=" + batch + " and degree_code=" + degree + " and current_semester=" + sem + " " + secvalue + "";
            strrollquer = strrollquer + " select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + sem + "' and s.batch_year='" + batch + "'  and s.degree_code='" + degree + "'";
            DataSet dsroll = da.select_method(strrollquer, hat, "Text");

            string nodays = "";
            string starting_dayorder = "";
            if (dsroll.Tables[1].Rows.Count > 0)
            {
                sem_start_date = dsroll.Tables[1].Rows[0]["start_date"].ToString();
                nodays = dsroll.Tables[1].Rows[0]["nodays"].ToString();
                starting_dayorder = dsroll.Tables[1].Rows[0]["starting_dayorder"].ToString();
            }
            tot_hr = 0;


            temp_date = spfromdate;
            dt2 = sptodate;


            string strsec = "";
            string rstrsec = "";
            string splhrsec = "";
            if (sections.Trim() != "" && sections.Trim() != "-1" && sections.ToString().Trim().ToLower() != "all")
            {
                strsec = " and sections='" + sections + "'";
                rstrsec = " and r.sections='" + sections + "'";
                splhrsec = "and sections='" + sections + "'";
            }


            string stralldetaisquery = "select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.subject_no='" + subjectno + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select r.roll_no,s.subject_no,s.batch,r.adm_date,s.fromdate from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + batch + "' and r.degree_code='" + degree + "' and s.subject_no='" + subjectno + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + batch + "' and degree_code='" + degree + "' and subject_no='" + subjectno + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + batch + "' and degree_code='" + degree + "' and subject_no='" + subjectno + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + batch + "' and r.degree_code='" + degree + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + batch + "' and r.degree_code='" + degree + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select * from Semester_Schedule where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "'  " + strsec + " order by FromDate desc";
            stralldetaisquery = stralldetaisquery + " ;select * from Alternate_Schedule where batch_year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "'  " + strsec + "  order by FromDate desc";
            DataSet dsalldetails = da.select_method_wo_parameter(stralldetaisquery, "Text");

            string subj_type = da.GetFunction("select subject_type From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subjectno + "')");
            string practicle = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subjectno + "'");
            while (temp_date <= dt2)
            {
                Boolean chk_alter = false;
                if (splhr_flag == true)
                {
                    if (ht_sphr.Contains(Convert.ToString(temp_date)))
                    {
                        string attenmdance = "1";
                        string roll = "";
                        string datevale = temp_date.ToString();
                        getspecial_hr(batch, degree, sem, sections, subjectno, attenmdance, roll, datevale);
                    }
                }
                span_count = 0;
                if (!hat_holy.ContainsKey(temp_date))
                {
                    if (!hat_holy.ContainsKey(temp_date))
                    {
                        hat_holy.Add(temp_date, "3*0*0");
                    }
                }

                value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
                split_holiday_status = value_holi_status.Split('*');

                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                {
                    split_holiday_status_1 = 1;
                    split_holiday_status_2 = no_of_hrs;
                }
                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                {

                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                    {
                        split_holiday_status_1 = mng_hrs + 1;
                        split_holiday_status_2 = no_of_hrs;
                    }

                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = mng_hrs;
                    }
                }
                else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                {
                    split_holiday_status_1 = 0;
                    split_holiday_status_2 = 0;
                }


                if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                {
                    //temp_date = temp_date.AddDays(1); aruna 30oct2012
                }
                else
                {

                    //---------------alternate schedule
                    //ds_alter.Clear();
                    //string stralterschedule = "select  * from alternate_schedule where degree_code = " + degree + " and semester = " + sem + " and batch_year = " + batch + " " + secvalue + " and FromDate ='" + temp_date + "' order by FromDate Desc";
                    //ds_alter = da.select_method(stralterschedule, hat, "Text");

                    ////----------------Semester Schedule-----------------------------
                    //ds.Clear();
                    //string strsemesterschedule = "select top 1 * from semester_schedule where degree_code = " + degree + " and semester = " + sem + " and batch_year = " + batch + "  " + secvalue + " and FromDate <='" + temp_date + "'  order by FromDate Desc";
                    //ds = da.select_method(strsemesterschedule, hat, "Text");

                    dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + degree + " and semester = " + sem + " and batch_year = " + batch + " and FromDate ='" + temp_date + "' " + secvalue + "";
                    DataView dvaltersech = dsalldetails.Tables[7].DefaultView;
                    //---------------------------------------------

                    //ds.Clear();
                    //con.Close();
                    //con.Open();
                    //cmd_sem_shed = new SqlCommand("select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd_sem_shed);
                    //da.Fill(ds);
                    dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + degree + " and semester = " + sem + " and batch_year = " + batch + " and FromDate <='" + temp_date + "' " + secvalue + "";
                    DataView dvsemsech = dsalldetails.Tables[6].DefaultView;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (no_of_hrs > 0)
                        {

                            dummy_date = temp_date.ToString();
                            string[] dummy_date_split = dummy_date.Split(' ');
                            string[] final_date_string = dummy_date_split[0].Split('/');
                            dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                            month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                            if (order != "0")
                            {
                                strDay = temp_date.ToString("ddd");
                            }
                            else
                            {
                                //strDay = findday(no_of_hrs, sem_start_date.ToString(), dummy_date, batch, degree, sem);
                                strDay = da.findday(temp_date.ToString(), degree, sem, batch, sem_start_date, nodays, starting_dayorder);
                            }

                            for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                            {
                                temp_hr_field = strDay + temp_hr;
                                date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;

                                if (dvaltersech.Count > 0)
                                {
                                    for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                    {
                                        full_hour = dvaltersech[hasrow][temp_hr_field].ToString();

                                        if (full_hour.Trim() != "")
                                        {
                                            chk_alter = true;
                                            string[] split_full_hour = full_hour.Split(';');
                                            for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                            {
                                                single_hour = split_full_hour[semi_colon].ToString();
                                                string[] split_single_hour = single_hour.Split('-');
                                                if (split_single_hour.GetUpperBound(0) > 1)
                                                {

                                                    if (split_single_hour[0].ToString() == subjectno)
                                                    {
                                                        if (practicle != "1" && practicle.ToLower() != "true")
                                                        {
                                                            dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subjectno + "'";
                                                            DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                            for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                            {
                                                                string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                if (dvattva.Count > 0)
                                                                {
                                                                    //DataSet dsattendance = da.select_method("select r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from registration r,attendance a   where r.degree_code=" + degree + " and batch_year=" + batch + " " + secvalue + "  and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=a.roll_no and  month_year=" + month_year + " order by a.roll_no", hat, "Text");
                                                                    //if (dsattendance.Tables[0].Rows.Count > 0)
                                                                    //{
                                                                    //    for (int attr = 0; attr < dsattendance.Tables[0].Rows.Count; attr++)
                                                                    //    {
                                                                    //string attroll = dsattendance.Tables[0].Rows[attr]["Roll_no"].ToString();
                                                                    //string preset = dsattendance.Tables[0].Rows[attr]["" + date_temp_field + ""].ToString();
                                                                    string attroll = dvattva[0]["Roll_no"].ToString();
                                                                    string preset = dvattva[0][date_temp_field].ToString();
                                                                    if (preset != null && preset != "0" && preset != "7" && preset != "" && preset != "8" && preset != "12")
                                                                    {
                                                                        if (sstu == 0)
                                                                        {
                                                                            tot_hr++;
                                                                        }
                                                                        if (!stdconducted.Contains(attroll.ToString()))
                                                                        {
                                                                            stdconducted.Add(attroll.ToString(), 1);
                                                                        }
                                                                        else
                                                                        {
                                                                            int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdconducted));
                                                                            pre++;
                                                                            stdconducted[attroll.ToString()] = pre;
                                                                        }
                                                                        for (int j = 0; j < count_master; j++)
                                                                        {
                                                                            if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == preset.ToString())
                                                                            {
                                                                                objvalue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                                j = count_master;
                                                                            }
                                                                        }
                                                                        if (objvalue == 0)
                                                                        {
                                                                            if (!stdpresnt.Contains(attroll.ToString()))
                                                                            {
                                                                                stdpresnt.Add(attroll.ToString(), 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdpresnt));
                                                                                pre++;
                                                                                stdpresnt[attroll.ToString()] = pre;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {

                                                            dsalldetails.Tables[3].DefaultView.RowFilter = "hour_value=" + temp_hr + "  and day_value='" + strDay + "' and subject_no='" + subjectno + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                            DataView dvlabbatch = dsalldetails.Tables[3].DefaultView;
                                                            for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                            {
                                                                string batchvalt = dvlabbatch[lb]["stu_batch"].ToString();
                                                                if (batchvalt != null && batchvalt.Trim() != "")
                                                                {
                                                                    dsalldetails.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subjectno + "' and batch='" + batchvalt + "' and fromdate='" + temp_date.ToString("MM/dd/yyyy") + "'";
                                                                    DataView dvlabhr = dsalldetails.Tables[1].DefaultView;
                                                                    for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                    {
                                                                        string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                        DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                        if (dvattva.Count > 0)
                                                                        {
                                                                            string attroll = dvattva[0]["Roll_no"].ToString();
                                                                            string preset = dvattva[0][date_temp_field].ToString();
                                                                            //string labstudent = "select  r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from  registration r,subjectchooser_New s,laballoc_new l,attendance  a  where a.roll_no=r.roll_no and r.batch_year=l.batch_year and r.roll_no=s.roll_no and s.subject_no =l.subject_no and r.degree_code=l.degree_code  and s.subject_no =l.subject_no and l.sections=r.sections and r.degree_code=" + degree + " and r.batch_year=" + batch + "  and cc=0 and delflag=0 and exam_flag<>'debar' and r.sections='" + sections + "'  and hour_value=" + temp_hr + "  and day_value='" + strDay + "'  and l.fdate=s.fromdate and l.Stu_Batch=s.Batch  and l.subject_no=" + subjectno + " and s.batch=l.stu_batch  and FromDate ='" + temp_date + "' and  a.month_year=" + month_year + " and r.Adm_Date<='" + temp_date + "' order by r.roll_no";
                                                                            //DataSet dsattendance = da.select_method(labstudent, hat, "Text");
                                                                            //if (dsattendance.Tables[0].Rows.Count > 0)
                                                                            //{
                                                                            //    for (int attr = 0; attr < dsattendance.Tables[0].Rows.Count; attr++)
                                                                            //    {
                                                                            //string attroll = dsattendance.Tables[0].Rows[attr]["Roll_no"].ToString();
                                                                            //string preset = dsattendance.Tables[0].Rows[attr]["" + date_temp_field + ""].ToString();

                                                                            if (preset != null && preset != "0" && preset != "7" && preset != "" && preset != "8" && preset != "12")
                                                                            {
                                                                                if (sstu == 0)
                                                                                    tot_hr++;

                                                                                if (!stdconducted.Contains(attroll.ToString()))
                                                                                {
                                                                                    stdconducted.Add(attroll.ToString(), 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdconducted));
                                                                                    pre++;
                                                                                    stdconducted[attroll.ToString()] = pre;
                                                                                }

                                                                                for (int j = 0; j < count_master; j++)
                                                                                {

                                                                                    if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == preset.ToString())
                                                                                    {
                                                                                        objvalue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                                        j = count_master;
                                                                                    }
                                                                                }
                                                                                if (objvalue == 0)
                                                                                {
                                                                                    if (!stdpresnt.Contains(attroll.ToString()))
                                                                                    {
                                                                                        stdpresnt.Add(attroll.ToString(), 1);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdpresnt));
                                                                                        pre++;
                                                                                        stdpresnt[attroll.ToString()] = pre;
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
                                            chk_alter = false;
                                        }
                                    }
                                }
                                if (chk_alter == false)
                                {
                                    full_hour = dvsemsech[0][temp_hr_field].ToString();
                                    if (full_hour.Trim() != "")
                                    {
                                        string[] split_full_hour_sem = full_hour.Split(';');
                                        for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                        {
                                            single_hour = split_full_hour_sem[semi_colon].ToString();
                                            string[] split_single_hour = single_hour.Split('-');
                                            if (split_single_hour.GetUpperBound(0) > 1)
                                            {
                                                if (split_single_hour[0].ToString() == subjectno)
                                                {
                                                    if (practicle != "1" && practicle.Trim().ToLower() != "true")
                                                    {
                                                        dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subjectno + "'";
                                                        DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                        for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                        {
                                                            string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                            dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                            DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                            if (dvattva.Count > 0)
                                                            {
                                                                string attval = dvattva[0][date_temp_field].ToString();

                                                                //DataSet dsattendance = da.select_method("select r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from registration r,attendance a   where r.degree_code=" + degree + " and batch_year=" + batch + " " + secvalue + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=a.roll_no and  month_year=" + month_year + " order by a.roll_no", hat, "Text");
                                                                //if (dsattendance.Tables[0].Rows.Count > 0)
                                                                //{
                                                                //    for (int attr = 0; attr < dsattendance.Tables[0].Rows.Count; attr++)
                                                                //    {
                                                                string attroll = dvattva[0]["Roll_no"].ToString();
                                                                string preset = dvattva[0]["" + date_temp_field + ""].ToString();
                                                                if (preset != null && preset != "0" && preset != "7" && preset != "" && preset != "8" && preset != "12")
                                                                {
                                                                    if (sstu == 0)
                                                                        tot_hr++;

                                                                    if (!stdconducted.Contains(attroll.ToString()))
                                                                    {
                                                                        stdconducted.Add(attroll.ToString(), 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdconducted));
                                                                        pre++;
                                                                        stdconducted[attroll.ToString()] = pre;
                                                                    }
                                                                    for (int j = 0; j < count_master; j++)
                                                                    {
                                                                        if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == preset.ToString())
                                                                        {
                                                                            objvalue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                            j = count_master;
                                                                        }
                                                                    }
                                                                    if (objvalue == 0)
                                                                    {
                                                                        if (!stdpresnt.Contains(attroll.ToString()))
                                                                        {
                                                                            stdpresnt.Add(attroll.ToString(), 1);
                                                                        }
                                                                        else
                                                                        {
                                                                            int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdpresnt));
                                                                            pre++;
                                                                            stdpresnt[attroll.ToString()] = pre;
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {

                                                        dsalldetails.Tables[2].DefaultView.RowFilter = "hour_value=" + temp_hr + " and subject_no='" + subjectno + "'  and day_value='" + strDay + "' and timetablename='" + dvsemsech[0]["ttname"].ToString() + "'";
                                                        DataView dvlabbatch = dsalldetails.Tables[2].DefaultView;
                                                        for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                        {
                                                            string batchbal = dvlabbatch[lb]["stu_batch"].ToString();
                                                            if (batch != null && batch.Trim() != "")
                                                            {
                                                                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subjectno + "' and batch='" + batchbal + "' ";
                                                                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                {
                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                    dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                    DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                    if (dvattva.Count > 0)
                                                                    {

                                                                        //string labstudent = "select  r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from  registration r,subjectchooser s,laballoc l,attendance  a  where a.roll_no=r.roll_no and r.batch_year=l.batch_year and r.roll_no=s.roll_no and s.subject_no =l.subject_no and r.degree_code=l.degree_code  and s.subject_no =l.subject_no and l.sections=r.sections and r.degree_code=" + degree + " and r.batch_year=" + batch + "  and cc=0 and delflag=0 and exam_flag<>'debar' and r.sections='" + sections + "'  and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subjectno + " and s.batch=l.stu_batch  and FromDate ='" + temp_date + "' and  a.month_year=" + month_year + " order by r.roll_no";
                                                                        //string labstudent = "select  r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from  registration r,subjectchooser s,laballoc l,attendance  a  where a.roll_no=r.roll_no and r.batch_year=l.batch_year and r.roll_no=s.roll_no and s.subject_no =l.subject_no and r.degree_code=l.degree_code  and s.subject_no =l.subject_no and l.sections=r.sections and r.degree_code=" + degree + " and r.batch_year=" + batch + "  and cc=0 and delflag=0 and exam_flag<>'debar' and r.sections='" + sections + "'  and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subjectno + " and s.batch=l.stu_batch  and l.Timetablename='" + dvsemsech[0]["ttname"].ToString() + "' and  a.month_year=" + month_year + " and r.Adm_Date<='" + temp_date.ToString("MM/dd/yyyy") + "' order by r.roll_no";
                                                                        //DataSet dsattendance = da.select_method(labstudent, hat, "Text");
                                                                        //// DataSet dsattendance = da.select_method("select r.roll_no," + date_temp_field + ", convert(varchar(15),adm_date,103) as adm_date from registration r,attendance a   where r.degree_code=" + degree + " and batch_year=" + batch + " " + secvalue + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=a.roll_no and  month_year=" + month_year + " order by a.roll_no", hat, "Text");
                                                                        //if (dsattendance.Tables[0].Rows.Count > 0)
                                                                        //{

                                                                        //    for (int attr = 0; attr < dsattendance.Tables[0].Rows.Count; attr++)
                                                                        //    {
                                                                        string attroll = dvattva[0]["Roll_no"].ToString();
                                                                        string preset = dvattva[0]["" + date_temp_field + ""].ToString();
                                                                        if (preset != null && preset != "0" && preset != "7" && preset != "" && preset != "8" && preset != "12")
                                                                        {
                                                                            if (sstu == 0)
                                                                                tot_hr++;

                                                                            if (!stdconducted.Contains(attroll.ToString()))
                                                                            {
                                                                                stdconducted.Add(attroll.ToString(), 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdconducted));
                                                                                pre++;
                                                                                stdconducted[attroll.ToString()] = pre;
                                                                            }

                                                                            for (int j = 0; j < count_master; j++)
                                                                            {
                                                                                if (ds_attndmaster.Tables[0].Rows[j]["LeaveCode"].ToString() == preset.ToString())
                                                                                {
                                                                                    objvalue = int.Parse(ds_attndmaster.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                                    j = count_master;
                                                                                }
                                                                            }
                                                                            if (objvalue == 0)
                                                                            {
                                                                                if (!stdpresnt.Contains(attroll.ToString()))
                                                                                {
                                                                                    stdpresnt.Add(attroll.ToString(), 1);
                                                                                }
                                                                                else
                                                                                {
                                                                                    int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdpresnt));
                                                                                    pre++;
                                                                                    stdpresnt[attroll.ToString()] = pre;
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
                }
                temp_date = temp_date.AddDays(1);
            }
            ds = da.select_method("select r.roll_no from registration r,subjectchooser s where s.roll_no=r.roll_no and s.subject_no='" + subjectno + "' and degree_code=" + degree + " " + secvalue + " and batch_year=" + batch + " and cc=0 and delflag=0 and exam_flag<>'debar' order by r.roll_no", hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    string roll = ds.Tables[0].Rows[r]["Roll_no"].ToString();
                    int pre = 0, tot_hr1 = 0;
                    if (stdpresnt.Contains(roll.ToString()))
                    {
                        tot_hr1 = Convert.ToInt32(GetCorrespondingKey(roll.ToString(), stdconducted));
                    }
                    if (tot_hr1 > 0)
                    {
                        if (stdpresnt.Contains(roll.ToString()))
                        {
                            pre = Convert.ToInt32(GetCorrespondingKey(roll.ToString(), stdpresnt));
                        }
                    }

                    //string deletequery = "delete from tbl_Cam_Calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and  istype='Attendance' and roll_no='" + roll + "'";
                    //int p = da.insert_method(deletequery, hat, "Text");

                    //string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values (" + subjectno + ",'" + syllcode + "','Attendance','" + roll + "','" + pre + "'," + tot_hr1 + ",'"+section+"')";
                    //p = da.insert_method(insertqurey, hat, "Text");

                    string insertqurey = "if not exists(select * from tbl_Cam_Calculation where roll_no='" + roll + "' and subject_no='" + subjectno + "' and istype='Attendance')";
                    insertqurey = insertqurey + " insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','Attendance','" + roll + "','" + pre + "','" + tot_hr1 + "','" + section + "')";
                    insertqurey = insertqurey + " else update tbl_Cam_Calculation set syll_code='" + syllcode + "',Exammark='" + pre + "',conversion='" + tot_hr1 + "',sections='" + section + "' where roll_no='" + roll + "' and subject_no='" + subjectno + "' and istype='Attendance'";
                    int p = da.insert_method(insertqurey, hat, "Text");
                }
            }
        }
        catch
        {
        }
    }

    public void getspecial_hr(string batch, string degree, string sem, string sections, string subjectno, string Attendance, string roll, string date)
    {

        try
        {
            //added By Srinath =========Start
            string hrdetno = "";
            int ObtValue = 0;
            string set = "";
            if (sections.Trim() != "" && sections.Trim() != "-1")
            {
                set = "And Sections=" + sections + "";
            }
            else
            {
                set = "";
            }
            if (ht_sphr.Contains(Convert.ToString(date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(date), ht_sphr));

            }
            if (Attendance == "1")
            {
                if (hrdetno != "")
                {
                    DataSet subjectstudent = da.select_method("Select Distinct roll_no from registration where batch_year=" + batch + " and degree_code=" + degree + " and current_semester=" + sem + " " + set + "", hat, "Text");
                    if (subjectstudent.Tables[0].Rows.Count > 0)
                    {
                        for (int rollco = 0; rollco < subjectstudent.Tables[0].Rows.Count; rollco++)
                        {
                            string attroll = subjectstudent.Tables[0].Rows[rollco]["roll_no"].ToString();
                            string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subjectno + "' and spa.roll_no='" + attroll.Trim() + "' and spd.hrdet_no in(" + hrdetno + ") order by spa.hrdet_no";
                            DataSet dssplhr = da.select_method(splhr_query_master, hat, "Text");
                            if (dssplhr.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dssplhr.Tables[0].Rows.Count; i++)
                                {
                                    string attendance = dssplhr.Tables[0].Rows[i]["attendance"].ToString();
                                    int preset = 0;
                                    spl_tol_per_hrs = spl_tol_per_hrs + 1;
                                    if (attendance != null && attendance != "0" && attendance != "7" && attendance != "")
                                    {
                                        if (tempvalue != attendance)
                                        {
                                            tempvalue = attendance;
                                            for (int j = 0; j < count; j++)
                                            {
                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == attendance.ToString())
                                                {
                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                    j = count;
                                                }
                                            }
                                        }
                                        if (ObtValue == 0)
                                        {
                                            preset = 1;
                                        }
                                        if (!stdpresnt.Contains(attroll.ToString()))
                                        {
                                            stdpresnt.Add(attroll.ToString(), preset);
                                        }
                                        else
                                        {
                                            int pre = Convert.ToInt32(GetCorrespondingKey(attroll.ToString(), stdpresnt));
                                            pre++;
                                            stdpresnt[attroll.ToString()] = pre;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (Attendance == "2")
            {
                if (hrdetno != "")
                {
                    spl_tol_per_hrs = 0;
                    spl_per_per_hrs = 0;
                    DataSet ds_splhr_query_master = new DataSet();

                    string splhr_query_master = "select attendance from specialhr_attendance where roll_no=" + roll + " and  hrdet_no in(" + hrdetno + ")";

                    ds_splhr_query_master = da.select_method(splhr_query_master, hat, "Text");
                    if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
                    {
                        for (int l = 0; l < ds_splhr_query_master.Tables[0].Rows.Count; l++)
                        {
                            string value = ds_splhr_query_master.Tables[0].Rows[l]["attendance"].ToString();

                            if (value != null && value != "0" && value != "7" && value != "")
                            {
                                spl_tol_per_hrs = spl_tol_per_hrs + 1;
                                if (tempvalue != value)
                                {
                                    tempvalue = value;
                                    for (int j = 0; j < count; j++)
                                    {

                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                        {
                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                            j = count;
                                        }
                                    }
                                }
                                if (ObtValue == 1)
                                {
                                    spl_per_per_hrs = spl_per_per_hrs + 1;
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

    public void persentmonthcal(string fromdatevalue, string todatevalue, string batch, string degree, string sem, string sections)
    {
        try
        {

            int mmyycount = 0, moncount = 0;
            int notconsider_value = 0, conduct_hour_new = 0;
            Hashtable holiday_table11 = new Hashtable();
            Hashtable holiday_table21 = new Hashtable();
            Hashtable holiday_table31 = new Hashtable();
            int cal_from_date = 0;
            int cal_to_date, cal_to_date_tmp;
            string frdate, todate;
            string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
            DateTime per_from_date;
            DateTime per_to_date;
            DateTime per_from_gendate;
            DateTime per_to_gendate;
            DateTime dumm_from_date;
            DateTime Admission_date;
            TimeSpan ts;
            string diff_date;
            int NoHrs = 0, fnhrs = 0, anhrs = 0, minpresI = 0, col_count = 0, next = 0, minpresII = 0;
            double dif_date = 0, dif_date1 = 0;
            double leave_pointer, absent_pointer;
            double leave_point = 0;
            double absent_point = 0;
            double per_perhrs = 0;
            double per_abshrs = 0;
            double Present = 0;
            double Absent = 0;
            double Onduty = 0;
            double Leave = 0;
            double halfday = 0;
            string date;
            int per_dum_unmark = 0;
            int dum_unmark = 0; ;
            int tot_per_hrs = 0;
            int per_tot_per_hrs = 0;
            int tot_wok_hrs = 0;
            double per_con_hrs, cum_con_hrs;
            double njhr = 0;
            double njdate = 0;
            double per_njdate = 0;
            double per_per_hrs = 0;
            double tot_ondu = 0;
            double per_tot_ondu = 0;
            double cum_tot_ondu, cum_tot_ml;
            double tot_ml = 0;
            double per_tot_ml = 0;
            double workingdays = 0;
            double per_workingdays = 0;
            double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
            int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
            int ObtValue = -1;
            int unmark = 0; ;
            double per_ondu = 0;
            double per_leave = 0;
            double per_hhday = 0;
            double cum_ondu = 0;
            double cum_leave = 0;
            double cum_hhday = 0;
            double per_holidate = 0;
            int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_hrs = 0;
            evng_hrs = 0;

            notconsider_value = 0;
            conduct_hour_new = 0;

            //Opt--------
            frdate = fromdatevalue;
            todate = todatevalue;
            string dt = frdate;
            string[] dsplit = dt.Split(new Char[] { '/' });
            frdate = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            int demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            int cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

            string monthcal = cal_from_date.ToString();
            dt = todate;
            dsplit = dt.Split(new Char[] { '/' });
            todate = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            int demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
            cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

            per_from_gendate = Convert.ToDateTime(frdate);
            per_to_gendate = Convert.ToDateTime(todate);
            splhr_flag = false;
            string rights = da.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
            if (rights.ToLower().Trim() == "true")
            {
                splhr_flag = true;
            }
            if (splhr_flag == true)
            {
                ht_sphr.Clear();
                string hrdetno = "";
                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + degree + " and batch_year=" + batch + " and semester=" + sem + " and date between '" + frdate.ToString() + "' and '" + todate.ToString() + "'";
                ds_sphr = da.select_method(getsphr, hat, "Text");
                if (ds_sphr.Tables[0].Rows.Count > 0)
                {
                    for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                    {
                        if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                        {
                            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                            hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                            ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                        }
                        else
                        {
                            ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                        }
                    }
                }

            }
            string sectionvalpreatt = "";
            //if (ckhdegreewise.Checked == true)
            //{
            //    if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "-1" && ddlsec.Enabled == true)
            //    {
            //        sectionvalpreatt = " and reg.sections='" + ddlsec.SelectedValue.ToString() + "'";
            //    }
            //}

            if (sections.Trim() != "" && sections.Trim() != "-1" && sections.Trim() != "0")
            {
                sectionvalpreatt = " and reg.sections='" + sections + "'";
            }
            string dd, value;
            DataSet ds2 = new DataSet();
            string strroll = "select reg.roll_no,reg.adm_date,reg.reg_no,reg.stud_name,reg.serialno,len(reg.roll_no) from registration reg,subjectchooser s where reg.roll_no=s.roll_no and s.subject_no=" + subjectno + " and reg.cc=0 and reg.delflag=0 and reg.exam_flag<>'Debar' " + sectionvalpreatt + " " + strorder + "";
            DataSet ds4 = da.select_method(strroll, hat, "Text");
            if (ds4.Tables[0].Rows.Count > 0)
            {
                for (int rows_count = 0; rows_count < ds4.Tables[0].Rows.Count; rows_count++)
                {
                    Boolean isadm = false;
                    spl_per_per_hrs = 0;
                    spl_tol_per_hrs = 0;
                    cal_from_date = cal_from_date_tmp;
                    cal_to_date = cal_to_date_tmp;
                    per_from_date = per_from_gendate;
                    per_to_date = per_to_gendate;
                    dumm_from_date = Convert.ToDateTime(frdate);

                    string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
                    string[] spiltup = admdate.Split(' ');
                    string[] admdatesp = spiltup[0].Split(new Char[] { '/' });
                    admdate = admdatesp[0].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[2].ToString();
                    Admission_date = Convert.ToDateTime(admdate);

                    dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
                    hat.Clear();
                    hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
                    hat.Add("from_month", cal_from_date);
                    hat.Add("to_month", cal_to_date);
                    ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
                    if (ds2.Tables.Count > 0)
                    {
                        mmyycount = ds2.Tables[0].Rows.Count;
                        moncount = mmyycount - 1;
                    }
                    if (rows_count == 0)
                    {
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                        count = ds1.Tables[0].Rows.Count;

                        hat.Clear();
                        hat.Add("degree_code", degree);
                        hat.Add("sem_ester", sem);
                        ds = da.select_method("period_attnd_schedule", hat, "sp");
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                            fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                            anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                            minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                            minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                        }

                        hat.Clear();
                        hat.Add("degree_code", int.Parse(degree));
                        hat.Add("sem", int.Parse(sem));
                        hat.Add("from_date", frdate);
                        hat.Add("to_date", todate);
                        hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                        //------------------------------------------------------------------
                        int iscount = 0;
                        string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate + "' and '" + todate + "' and degree_code=" + degree + " and semester=" + sem + "";
                        DataSet dsholiday = da.select_method(sqlstr_holiday, hat, "Text");
                        if (dsholiday.Tables[0].Rows.Count > 0)
                        {
                            iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                        }
                        hat.Add("iscount", iscount);
                        ds.Dispose();
                        ds.Reset();
                        ds = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                        Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                        Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                        Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                        holiday_table11.Clear();
                        holiday_table21.Clear();
                        holiday_table31.Clear();
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                if (ds.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                                {
                                    halforfull = "0";
                                }
                                else
                                {
                                    halforfull = "1";
                                }
                                if (ds.Tables[0].Rows[0]["morning"].ToString() == "False")
                                {
                                    mng = "0";
                                }
                                else
                                {
                                    mng = "1";
                                }
                                if (ds.Tables[0].Rows[0]["evening"].ToString() == "False")
                                {
                                    evng = "0";
                                }
                                else
                                {
                                    evng = "1";
                                }

                                holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                string[] split_date_time1 = ds.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');
                                holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                            }
                        }

                        if (ds.Tables[1].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                            {
                                string[] split_date_time1 = ds.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');
                                holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                                if (ds.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                                {
                                    halforfull = "0";
                                }
                                else
                                {
                                    halforfull = "1";
                                }
                                if (ds.Tables[1].Rows[k]["morning"].ToString() == "False")
                                {
                                    mng = "0";
                                }
                                else
                                {
                                    mng = "1";
                                }
                                if (ds.Tables[1].Rows[k]["evening"].ToString() == "False")
                                {
                                    evng = "0";
                                }
                                else
                                {
                                    evng = "1";
                                }

                                holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                {
                                    holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                                }
                                holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                            }
                        }

                        if (ds.Tables[2].Rows.Count != 0)
                        {
                            for (int k = 0; k < ds.Tables[2].Rows.Count; k++)
                            {
                                string[] split_date_time1 = ds.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');
                                holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                                if (ds.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                                {
                                    halforfull = "0";
                                }
                                else
                                {
                                    halforfull = "1";
                                }
                                if (ds.Tables[2].Rows[k]["morning"].ToString() == "False")
                                {
                                    mng = "0";
                                }
                                else
                                {
                                    mng = "1";
                                }
                                if (ds.Tables[2].Rows[k]["evening"].ToString() == "False")
                                {
                                    evng = "0";
                                }
                                else
                                {
                                    evng = "1";
                                }
                                holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                                {
                                    holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                                }

                                holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                            }
                        }
                    }

                    //------------------------------------------------------------------
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
                    {
                        ts = DateTime.Parse(ds.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                        diff_date = Convert.ToString(ts.Days);
                        dif_date1 = double.Parse(diff_date.ToString());
                    }
                    next = 0;

                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count != 0)
                    {
                        int rowcount = 0;
                        int ccount;
                        ccount = ds.Tables[1].Rows.Count;
                        ccount = ccount - 1;
                        //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                        while (dumm_from_date <= (per_to_date))
                        {
                            isadm = false;
                            if (dumm_from_date >= Admission_date)
                            {
                                isadm = true;
                                int temp_unmark = 0;
                                if (splhr_flag == true)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                                    {
                                        string attenmdance = "2";
                                        string section = "";
                                        string roll = dd;
                                        string datevale = dumm_from_date.ToString();
                                        getspecial_hr(batch, degree, sem, section, subjectno, attenmdance, roll, datevale); // getspecial_hr();
                                    }
                                }
                                for (int i = 1; i <= mmyycount; i++)
                                {
                                    if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                                    {
                                        string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                        string[] dummy_split = split_date_time1[0].Split('/');

                                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                        {
                                            holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                                        }

                                        if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                        {
                                            value_holi_status = GetCorrespondingKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                            split_holiday_status = value_holi_status.Split('*');

                                            if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                            {
                                                split_holiday_status_1 = 1;
                                                split_holiday_status_2 = 1;
                                            }
                                            else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                            {
                                                if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                                {
                                                    split_holiday_status_1 = 0;
                                                    split_holiday_status_2 = 1;
                                                }

                                                if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                                {
                                                    split_holiday_status_1 = 1;
                                                    split_holiday_status_2 = 0;
                                                }
                                            }
                                            else if (split_holiday_status[0].ToString() == "0")
                                            {
                                                dumm_from_date = dumm_from_date.AddDays(1);
                                                if (dumm_from_date.Day == 1)
                                                {
                                                    cal_from_date++;
                                                    if (moncount > next)
                                                    {
                                                        next++;
                                                    }
                                                }
                                                break;
                                            }

                                            if (ds.Tables[1].Rows.Count != 0)
                                            {
                                                ts = DateTime.Parse(ds.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                                diff_date = Convert.ToString(ts.Days);
                                                dif_date = double.Parse(diff_date.ToString());
                                            }
                                            else
                                            {
                                                dif_date = 0;
                                            }
                                            if (dif_date == 1)
                                            {
                                                leave_pointer = holi_leav;
                                                absent_pointer = holi_absent;
                                            }
                                            else if (dif_date == -1)
                                            {
                                                leave_pointer = holi_leav;
                                                absent_pointer = holi_absent;
                                                if (ccount > rowcount)
                                                {
                                                    rowcount += 1;
                                                }
                                            }
                                            else
                                            {
                                                leave_pointer = leav_pt;
                                                absent_pointer = absent_pt;
                                            }

                                            if (ds.Tables[2].Rows.Count != 0)
                                            {
                                                ts = DateTime.Parse(ds.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                                diff_date = Convert.ToString(ts.Days);
                                                dif_date = double.Parse(diff_date.ToString());
                                                if (dif_date == 1)
                                                {
                                                    leave_pointer = holi_leav;
                                                    absent_pointer = holi_absent;
                                                }
                                            }
                                            if (dif_date1 == -1)
                                            {
                                                leave_pointer = holi_leav;
                                                absent_pointer = holi_absent;
                                            }
                                            dif_date1 = 0;
                                            if (split_holiday_status_1 == 1)
                                            {
                                                for (i = 1; i <= fnhrs; i++)
                                                {
                                                    date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                    value = ds2.Tables[0].Rows[next][date].ToString();
                                                    if (value != null && value != "0" && value != "7" && value != "")
                                                    {
                                                        if (tempvalue != value)
                                                        {
                                                            tempvalue = value;
                                                            for (int j = 0; j < count; j++)
                                                            {
                                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                                {
                                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                    j = count;
                                                                }
                                                            }
                                                        }
                                                        if (ObtValue == 1)
                                                        {
                                                            per_abshrs += 1;
                                                        }
                                                        else if (ObtValue == 2)
                                                        {
                                                            notconsider_value += 1;
                                                            njhr += 1;
                                                        }
                                                        else if (ObtValue == 0)
                                                        {
                                                            per_perhrs += 1;
                                                            tot_per_hrs += 1;
                                                        }
                                                        if (value == "3")
                                                        {
                                                            per_ondu += 1;
                                                            tot_ondu += 1;
                                                        }
                                                        else if (value == "10")
                                                        {
                                                            per_leave += 1;
                                                        }
                                                        else if (value == "4")
                                                        {
                                                            tot_ml += 1;
                                                        }
                                                    }
                                                    else if (value == "7")
                                                    {
                                                        per_hhday += 1;

                                                    }
                                                    else
                                                    {
                                                        unmark += 1;
                                                        temp_unmark++;

                                                        my_un_mark++;
                                                    }
                                                }
                                                if (per_perhrs + njhr >= minpresI)
                                                {
                                                    Present += 0.5;
                                                }
                                                else if (per_leave >= 1)
                                                {
                                                    leave_point += leave_pointer / 2;
                                                    Leave += 0.5;
                                                }
                                                else if (per_abshrs >= 1)
                                                {
                                                    Absent += 0.5;
                                                    absent_point += absent_pointer / 2;
                                                }
                                                if (njhr >= minpresI)
                                                {
                                                    njdate += 0.5;
                                                    njdate_mng += 1;
                                                }
                                                if (per_ondu >= 1)
                                                {
                                                    Onduty += 0.5;
                                                }
                                                if (temp_unmark == fnhrs)
                                                {
                                                    per_holidate_mng += 1;
                                                    per_holidate += 0.5;
                                                    unmark = 0;
                                                }
                                                else
                                                {
                                                    dum_unmark = temp_unmark;
                                                }
                                                if (fnhrs - temp_unmark >= minpresI)
                                                {
                                                    workingdays += 0.5;
                                                }
                                                mng_conducted_half_days += 1;
                                            }
                                            per_perhrs = 0;
                                            per_ondu = 0;
                                            per_leave = 0;
                                            per_abshrs = 0;
                                            //   unmark = 0;
                                            temp_unmark = 0;
                                            njhr = 0;

                                            int k = fnhrs + 1;

                                            if (split_holiday_status_2 == 1)
                                            {
                                                for (i = k; i <= NoHrs; i++)
                                                {
                                                    date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                    value = ds2.Tables[0].Rows[next][date].ToString();

                                                    if (value != null && value != "0" && value != "7" && value != "")
                                                    {
                                                        if (tempvalue != value)
                                                        {
                                                            tempvalue = value;
                                                            for (int j = 0; j < count; j++)
                                                            {

                                                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                                {
                                                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                                    j = count;
                                                                }
                                                            }
                                                        }
                                                        if (ObtValue == 1)
                                                        {
                                                            per_abshrs += 1;
                                                        }
                                                        else if (ObtValue == 2)
                                                        {
                                                            notconsider_value += 1;
                                                            njhr += 1;
                                                        }
                                                        else if (ObtValue == 0)
                                                        {
                                                            per_perhrs += 1;
                                                            tot_per_hrs += 1;
                                                        }
                                                        if (value == "3")
                                                        {
                                                            per_ondu += 1;
                                                            tot_ondu += 1;
                                                        }
                                                        else if (value == "10")
                                                        {
                                                            per_leave += 1;
                                                        }
                                                        if (value == "4")
                                                        {
                                                            tot_ml += 1;
                                                        }
                                                    }
                                                    else if (value == "7")
                                                    {
                                                        per_hhday += 1;
                                                    }
                                                    else
                                                    {
                                                        unmark += 1;
                                                        temp_unmark++;

                                                        my_un_mark++; //added 080812
                                                    }
                                                }
                                                //   if (per_perhrs >= minpresII)
                                                if (per_perhrs + njhr >= minpresII)
                                                {
                                                    Present += 0.5;
                                                }

                                                else if (per_leave >= 1)
                                                {
                                                    leave_point += leave_pointer / 2;
                                                    Leave += 0.5;
                                                }
                                                else if (per_abshrs >= 1)
                                                {
                                                    Absent += 0.5;
                                                    absent_point += absent_pointer / 2;
                                                }
                                                if (njhr >= minpresII)
                                                {
                                                    njdate_evng += 1;
                                                    njdate += 0.5;
                                                }
                                                if (per_ondu >= 1)
                                                {
                                                    Onduty += 0.5;
                                                }
                                                if (temp_unmark == NoHrs - fnhrs)
                                                {
                                                    per_holidate_evng += 1;
                                                    per_holidate += 0.5;
                                                    unmark = 0;
                                                }
                                                else
                                                {
                                                    dum_unmark += unmark;
                                                }
                                                if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                                {
                                                    workingdays += 0.5;
                                                }
                                                evng_hrs += 1;
                                            }
                                            per_perhrs = 0;
                                            per_ondu = 0;
                                            per_leave = 0;
                                            per_abshrs = 0;
                                            unmark = 0; //hided
                                            njhr = 0;
                                            dumm_from_date = dumm_from_date.AddDays(1);
                                            if (dumm_from_date.Day == 1)
                                            {
                                                cal_from_date++;
                                                if (moncount > next)
                                                {
                                                    next++;
                                                }
                                            }
                                            per_perhrs = 0;
                                        }

                                    }
                                    else
                                    {
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                        if (dumm_from_date.Day == 1)
                                        {
                                            cal_from_date++;
                                            if (moncount > next)
                                            {
                                                next++;
                                            }
                                        }
                                    }
                                }
                            }
                            if (isadm == false)
                            {
                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {
                                    cal_from_date++;
                                    if (moncount > next)
                                    {
                                        next++;

                                    }
                                }
                            }
                        }
                        int diff_Date = per_from_date.Day - dumm_from_date.Day;
                    }
                    per_tot_ondu = tot_ondu;
                    per_tot_ml = tot_ml;
                    per_njdate = njdate;
                    pre_present_date = Present - njdate;
                    per_per_hrs = tot_per_hrs;
                    per_absent_date = Absent;
                    pre_ondu_date = Onduty;
                    pre_leave_date = Leave;
                    per_workingdays = workingdays - per_njdate;
                    per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_hrs * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));
                    per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_hrs * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
                    per_dum_unmark = dum_unmark;
                    per_per_hrs = per_per_hrs + spl_per_per_hrs;
                    per_workingdays1 = per_workingdays1 + Convert.ToInt32(spl_tol_per_hrs);
                    if (per_per_hrs.ToString().Trim() == "NaN")
                    {
                        per_per_hrs = 0;
                    }
                    string strSecValue = string.Empty;

                    string attndtxt = string.Empty;
                    string monthwise = string.Empty;
                    if (chkbasedSettings.Checked)
                        attndtxt = "Attendance" + fromdatevalue + "-" + todatevalue;
                    else
                        attndtxt = "Attendance";
                    //monthwise = "and monthWise='" + attndtxt + "'";
                    if (!string.IsNullOrEmpty(sections) && sections != "-1" && sections.Trim().ToLower() != "all")
                    {
                        strSecValue = " and sections='" + sections + "'";
                    }

                    string deletequery = "delete from tbl_Cam_Calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' and  istype='" + attndtxt + "' and roll_no='" + dd + "' " + strSecValue;
                    int p = da.insert_method(deletequery, hat, "Text");

                    string insertqurey = "insert into tbl_Cam_Calculation (subject_no,syll_code,istype,roll_no,Exammark,conversion,sections) values ('" + subjectno + "','" + syllcode + "','" + attndtxt + "','" + dd + "','" + per_per_hrs + "','" + per_workingdays1 + "','" + sections + "')";
                    p = da.insert_method(insertqurey, hat, "Text");
                    mng_conducted_half_days = 0;
                    evng_hrs = 0;
                    my_un_mark = 0;
                    notconsider_value = 0;
                    per_workingdays1 = 0;
                    per_workingdays = 0;
                    Present = 0;
                    tot_per_hrs = 0;
                    Absent = 0;
                    Onduty = 0;
                    Leave = 0;
                    workingdays = 0;
                    per_holidate = 0;
                    dum_unmark = 0;
                    absent_point = 0;
                    leave_point = 0;
                    njdate = 0;
                    tot_ondu = 0;
                    tot_ml = 0;
                    spl_tol_per_hrs = 0;
                    spl_per_per_hrs = 0;
                }
            }
        }
        catch
        {
        }

    }

    private string findday(int no, string sdate, string todate, string batch, string degree, string sem)//------------------find day order 
    {
        int order, holino;
        holino = 0;
        string day_order = "";
        string from_date = "", tmpdate = "";
        string fdate = "", smdate = "";
        int diff_work_day = 0;

        tmpdate = sdate.ToString();
        string[] semstart_date = tmpdate.Split(new Char[] { ' ' });
        string[] sm_date = semstart_date[0].Split(new Char[] { '/' });
        smdate = sm_date[0].ToString() + "/" + sm_date[1].ToString() + "/" + sm_date[2].ToString();


        from_date = todate.ToString();
        string[] fm_date = from_date.Split(new Char[] { '/' });
        fdate = fm_date[1].ToString() + "/" + fm_date[0].ToString() + "/" + fm_date[2].ToString();


        DataSet dsholiday = da.select_method("select count(*) as count from holidaystudents where degree_code=" + degree + " and semester=" + sem + " and holiday_date between '" + sdate.ToString() + "' and  '" + fdate.ToString() + "' and halforfull='0' and isnull(Not_include_dayorder,0)<>'1'", hat, "Text");//01.03.17 barath";

        if (dsholiday.Tables[0].Rows.Count > 0)
        {
            holino = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["Count"]);
        }

        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + degree + " and semester=" + sem;
        string nodays = da.GetFunction(quer);
        int no_days = Convert.ToInt32(nodays);
        DateTime dt1 = Convert.ToDateTime(smdate);
        DateTime dt2 = Convert.ToDateTime(fdate);
        TimeSpan t = dt2 - dt1;

        int days = t.Days;

        diff_work_day = days - holino;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        //-----------------------------------------------------------
        order = order + 1;
        string stastdayorder = "";

        stastdayorder = da.GetFunction("select starting_dayorder from seminfo where degree_code=" + batch + " and semester=" + sem + " and batch_year=" + batch + "");
        if (stastdayorder.ToString().Trim() != "")
        {
            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
            {
                order = order + (Convert.ToInt16(stastdayorder) - 1);
                if (order == (no_days + 1))
                    order = 1;
                else if (order > no_days)
                    order = order % no_days;
            }
        }
        //-----------------------------------------------------------


        if (order.ToString() == "0")
        {
            order = no_days;
        }
        if (order.ToString() == "1")
        {
            day_order = "mon";
        }
        else if (order.ToString() == "2")
        {
            day_order = "tue";
        }
        else if (order.ToString() == "3")
        {
            day_order = "wed";
        }
        else if (order.ToString() == "4")
        {
            day_order = "thu";
        }
        else if (order.ToString() == "5")
        {
            day_order = "fri";
        }
        else if (order.ToString() == "6")
        {
            day_order = "sat";
        }
        else if (order.ToString() == "7")
        {
            day_order = "sun";
        }
        return (day_order);
    }

    #endregion
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }
        return null;
    }
    protected void btnupdate_OnClick(object sender, EventArgs e)
    {
        Button btupd = (Button)sender;
        string rowindx = btupd.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowindex = Convert.ToInt32(rowindx) - 2;
        Session["rowIndex"] = rowindex.ToString();
        Label sylcod = (Label)GridView1.Rows[rowindex].FindControl("lblsyllcode");
        syllcode = sylcod.Text;
        Label subno = (Label)GridView1.Rows[rowindex].FindControl("lblsubno");
        subjectno = subno.Text;
        chk_flag = false;
        loadcontrols();

    }
    void loadcontrols()
    {
        try
        {
            errmsg.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            divPrint.Visible = false;
            txtreport.Visible = false;
            lblreportname.Visible = false;
            string camoption = "";
            string istypevalue = "";
            string camconvert = "";
            string[] camcriteriano;
            string camavgbest = "";
            int loadvalue = 0;
            string roundofcheck = "0";
            string roundvalues = "";
            string txtcriname = "";
            string calcutexts = "";
            txtfromdate.AutoPostBack = true;
            txttodate.AutoPostBack = true;
            ArrayList arrSections = new ArrayList();
            subjectno = string.Empty;
            syllcode = string.Empty;
            if (chk_flag == false)
            {
                string strsecval = "";
                int noofsect = 0;

                foreach (GridViewRow s in GridView1.Rows)
                {
                    int isval = 0;
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)s.FindControl("cbselect");
                    if (chk.Checked)
                    {
                        noofsect++;
                        if (subjectno == "")
                        {
                            Label subn = (Label)s.FindControl("lblsubno");
                            subjectno = subn.Text;
                            Label syl = (Label)s.FindControl("lblsyllcode");
                            syllcode = syl.Text;
                        }
                        else
                        {
                            Label subn = (Label)s.FindControl("lblsubno");
                            subjectno = subjectno + ',' + (subn.Text);
                            Label syl = (Label)s.FindControl("lblsyllcode");
                            syllcode = syllcode + ',' + (syl.Text);
                        }

                        Label sec = (Label)s.FindControl("lblsection");
                        string sections = sec.Text;

                        strsecval = "";
                        if (sections.Trim() != "" && sections != "0" && sections != "-1")
                        {
                            strsecval = " and sections='" + sections + "'";
                            if (!arrSections.Contains(sections.Trim()))
                            {
                                arrSections.Add(sections.Trim());
                            }
                        }
                    }
                }

                // if (noofsect > 1)
                if (arrSections.Count > 1)
                {
                    strsecval = "";
                }

                if (syllcode != "" && subjectno != "")
                {
                    string ss = string.Empty;
                    if (ddlsec.Enabled == true && ckhdegreewise.Checked)
                    {
                        ss = Convert.ToString(ddlsec.SelectedItem.Text);
                    }
                    string secval = string.Empty;
                    if (ss.Trim() != "" && ss != "0" && ss != "-1" && !string.IsNullOrEmpty(ss))
                        secval = "  and sections='" + ss + "'";

                    DataTable dtExamDetails = dir.selectDataTable("select s.subsubjectname,s.subjectid,criteria  from criteriaforinternal c,Exam_type e,subsubjectTestDetails s where e.exam_code=s.examcode and e.criteria_no=c.criteria_no and e.subject_no in(" + subjectno + ") and c.syll_code in (" + syllcode + ") " + secval + " ");
                    //string Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + Degreecode + "' and semester=" + sem + " and syllabus_year=" + Syllabusyr + " and batch_year=" + batchyear + " order by criteria";
                    string Sqlstr = "select distinct criteria from criteriaforinternal c,Exam_type e where e.criteria_no=c.criteria_no and e.subject_no in(" + subjectno + ") and c.syll_code in (" + syllcode + ") " + strsecval + "";
                    dssubject.Reset();
                    dssubject.Dispose();
                    dssubject = da.select_method(Sqlstr, hat, "Text");
                    int count = 0;
                    int calculatecout = 0;

                    string sumcrit = "0";
                    int camcount = 0;
                    int calcount = 0;
                    int attadd = 0;
                    string criteriavalue = "";
                    string calcriteriavalue = "";
                    string attendancevalue = "";
                    string settingvalue = "";
                    string wordssettings = "";
                    Hashtable hatsub = new Hashtable();
                    string strgetdetails = "Select * from internal_cam_calculation_master_setting where subject_no in (" + subjectno + ") and syll_code in (" + syllcode + ") " + strsecval + " order by idno,subject_no,syll_code,Istype";
                    dsgetdetails = da.select_method(strgetdetails, hat, "Text");

                    if (txtcriteria.Text != "" && txtcalculate.Text != "" && txtcriteria.Text != "0" && txtcalculate.Text != "0")
                    {
                        count = Convert.ToInt32(txtcriteria.Text);
                        calculatecout = Convert.ToInt32(txtcalculate.Text);
                    }
                    //else
                    //{
                    if (dsgetdetails.Tables[0].Rows.Count > 0)
                    {
                        //DataTable dtNew =dsgetdetails.Tables[0].DefaultView.ToTable(true, "Istype","syll_code","Criteria_no","sections");
                        for (int a = 0; a < dsgetdetails.Tables[0].Rows.Count; a++)
                        {
                            if (a == 6)
                                break;
                            istypevalue = dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();
                            string[] camspiltv = istypevalue.Split(' ');
                            string calcultionoption = dsgetdetails.Tables[0].Rows[a]["Calculation_option"].ToString().Trim();
                            string subjectId = Convert.ToString(dsgetdetails.Tables[0].Rows[a]["Calculation_option"]);
                            if (!string.IsNullOrEmpty(subjectId))
                                chkSubSub.Checked = true;

                            if (!hatsub.Contains(calcultionoption))
                                hatsub.Add(calcultionoption, subjectId);
                            if (calcultionoption.Trim() != "")
                            {
                                istypevalue = dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();
                                calcount++;
                                if (calcriteriavalue == "")
                                    calcriteriavalue = dsgetdetails.Tables[0].Rows[a]["Calculation_Criteria"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Calculation_Option"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Include_Final_Calculation"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Conversion_Value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_of"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["sum_select_criteria"].ToString();
                                else
                                    calcriteriavalue = calcriteriavalue + '&' + dsgetdetails.Tables[0].Rows[a]["Calculation_Criteria"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Calculation_Option"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Include_Final_Calculation"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Conversion_Value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_of"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["sum_select_criteria"].ToString();

                                if (calcutexts == "")
                                    calcutexts = dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();
                                else
                                    calcutexts = calcutexts + ";" + dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();

                            }
                            else if ("Attendance" == istypevalue)
                            {
                                attadd++;
                                attendancevalue = dsgetdetails.Tables[0].Rows[a]["Attendance"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Att_cal"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Att_Mark_Per"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_of"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_value"].ToString().Trim();
                            }
                            else if ("Settings" == istypevalue)
                            {
                                settingvalue = dsgetdetails.Tables[0].Rows[a]["int_Mark_settings"].ToString();
                                wordssettings = dsgetdetails.Tables[0].Rows[a]["sum_select_criteria"].ToString();
                            }
                            else if (calcultionoption.Trim() == "")
                            {
                                camcount++;
                                if (criteriavalue == "")
                                    criteriavalue = dsgetdetails.Tables[0].Rows[a]["Criteria_no"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Cam_Option"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Cam_Ave_best"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Conversion_Value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_of"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();
                                else
                                    criteriavalue = criteriavalue + '&' + dsgetdetails.Tables[0].Rows[a]["Criteria_no"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Cam_Option"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Cam_Ave_best"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Conversion_Value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_of"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Round_value"].ToString().Trim() + '@' + dsgetdetails.Tables[0].Rows[a]["Istype"].ToString().Trim();
                            }
                        }

                        if (camcount.ToString().Trim() != "" && camcount.ToString().Trim() != "0")
                        {
                            txtcriteria.Text = camcount.ToString();
                        }
                        if (calcount == 0)
                        {
                            calcount = 1;
                        }
                        if (calcount.ToString().Trim() != "")
                        {
                            txtcalculate.Text = calcount.ToString();
                        }
                        if (attadd.ToString().Trim() != "" && attadd.ToString().Trim() != "0")//Change For subSub
                        {
                            chkattendance.Checked = true;
                        }
                        else
                        {
                            chkattendance.Checked = false;
                        }
                        spitcamcount56 = criteriavalue.Split('&');
                        spiltcalcount = calcriteriavalue.Split('&');
                        count = camcount;
                        calculatecout = calcount;
                    }
                    //}
                    if (dssubject.Tables[0].Rows.Count > 0)
                    {
                        if (count != 0 && calculatecout != 0)
                        {
                            panel4.Visible = true;

                            // panSubSubject = new Panel();
                            // panSubSubject.ID = "panSubSubject";
                            // panSubSubject.Visible = true;
                            // panel4.Controls.Add(panSubSubject);

                            // chkSubSubject = new CheckBox();
                            // chkSubSubject.ID = "chkSubSubject";
                            // chkSubSubject.Text = "Include Sub Subjects";
                            // chkSubSubject.Font.Bold = true;
                            //// chkSubSubject.CheckedChanged += this.chkSubSubject_CheckedChanged;
                            // chkSubSubject.Font.Size = FontUnit.Medium;

                            // chkSubSubject.Style.Value = "left: 450px; top: 10px; position: absolute;";
                            // panSubSubject.Controls.Add(chkSubSubject);



                            //===================Start Criteria Controls==========================
                            for (int i = 0; i < count; i++)
                            {
                                GridView2.Visible = false;
                                camoption = "";
                                camconvert = "";
                                camavgbest = "";
                                string camcrititrian = "";
                                roundid++;
                                id++;
                                loadvalue++;
                                pan = new Panel();
                                pan.ID = "pan" + id;
                                pan.Visible = true;
                                panel4.Controls.Add(pan);


                                lbl = new Label();
                                lbl.ID = "lblcriteria" + id;
                                lbl.Text = "Criteria" + loadvalue;
                                lbl.Font.Bold = true;
                                lbl.Font.Size = FontUnit.Medium;
                                lbl.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(lbl);

                                if (criteriavalue == "")
                                {
                                    txtcriterianame = new TextBox();
                                    txtcriterianame.ID = "txtcriname" + id;
                                    txtcriterianame.Text = "Criteria" + loadvalue;
                                    txtcriterianame.Font.Bold = true;
                                    txtcriterianame.Font.Size = FontUnit.Medium;
                                    txtcriterianame.Width = 100;
                                    txtcriterianame.Height = 15;
                                    txtcriterianame.Style.Value = "left: 325px; top: " + top + "px; position: absolute;";
                                    pan.Controls.Add(txtcriterianame);
                                }


                                roundofcheck = "0";
                                roundvalues = "";
                                if (criteriavalue != "")
                                {
                                    spiltcamvalue = spitcamcount56[i].Split('@');
                                    camcriteriano = spiltcamvalue[0].Split(',');
                                    camoption = spiltcamvalue[1].ToString();
                                    camconvert = spiltcamvalue[3].ToString();
                                    camavgbest = spiltcamvalue[2].ToString();
                                    roundofcheck = spiltcamvalue[4].ToString();
                                    roundvalues = spiltcamvalue[5].ToString();
                                    txtcriname = spiltcamvalue[6].ToString();

                                    txtcriterianame = new TextBox();
                                    txtcriterianame.ID = "txtcriname" + id;
                                    if (txtcriname == "")
                                    {
                                        txtcriterianame.Text = "Criteria" + loadvalue;
                                    }
                                    else
                                    {
                                        txtcriterianame.Text = txtcriname;
                                    }
                                    txtcriterianame.Font.Bold = true;
                                    txtcriterianame.Font.Size = FontUnit.Medium;
                                    txtcriterianame.Width = 100;
                                    txtcriterianame.Height = 15;
                                    txtcriterianame.Style.Value = "left: 325px; top: " + top + "px; position: absolute;";
                                    pan.Controls.Add(txtcriterianame);


                                    for (int crino = 0; camcriteriano.GetUpperBound(0) >= crino; crino++)
                                    {
                                        string chkcrina = camcriteriano[crino].ToString();
                                        if (chkcrina != "")
                                        {
                                            if (camcrititrian == "")
                                                camcrititrian = da.GetFunction("select distinct criteria from criteriaforinternal where criteria_no=" + chkcrina + "");
                                            else
                                                camcrititrian = camcrititrian + '/' + da.GetFunction("select distinct criteria from criteriaforinternal where criteria_no=" + chkcrina + "");
                                        }
                                        else
                                        {
                                            if (camcrititrian == "")
                                                camcrititrian = "";
                                            else
                                                camcrititrian = camcrititrian + '/' + camcrititrian;
                                        }
                                    }
                                }

                                top = top + 25;
                                subtop = top - 25;
                                rbvtotal = new RadioButton();
                                rbvtotal.ID = "rbvtotal" + id;
                                rbvtotal.Text = "Total Average";
                                rbvtotal.GroupName = "Cam" + id;
                                if (camoption == "1" || camoption == "")
                                {
                                    rbvtotal.Checked = true;
                                }
                                rbvtotal.AutoPostBack = true;
                                rbvtotal.CheckedChanged += this.rbvtotal_Checked;
                                rbvtotal.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rbvtotal);


                                top = top + 25;
                                rbvbest = new RadioButton();
                                rbvbest.ID = "rbvbest" + id;
                                rbvbest.Text = "Best of";
                                rbvbest.GroupName = "Cam" + id;
                                if (camoption == "2")
                                {
                                    rbvbest.Checked = true;
                                }
                                else
                                {
                                    rbvbest.Checked = false;
                                }
                                rbvbest.AutoPostBack = true;
                                rbvbest.CheckedChanged += this.rbvbest_Checked;
                                rbvbest.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rbvbest);


                                top = top + 25;
                                rbvAveragebest = new RadioButton();
                                rbvAveragebest.ID = "rbvAveragebest" + id;
                                rbvAveragebest.Text = "Average of Best";
                                rbvAveragebest.GroupName = "Cam" + id;
                                rbvAveragebest.AutoPostBack = true;
                                if (camoption == "3")
                                {
                                    rbvAveragebest.Checked = true;
                                }
                                else
                                {
                                    rbvAveragebest.Checked = false;
                                }
                                rbvAveragebest.CheckedChanged += this.CheckBoxChanged;
                                rbvAveragebest.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rbvAveragebest);

                                txtaveragebest = new TextBox();
                                txtaveragebest.ID = "txtaveragebest" + id;
                                txtaveragebest.MaxLength = 3;
                                txtaveragebest.Width = 30;
                                if (camoption == "3")
                                {
                                    txtaveragebest.Visible = true;
                                    if (camavgbest != "")
                                    {
                                        txtaveragebest.Text = camavgbest;
                                    }
                                }
                                else
                                {
                                    txtaveragebest.Visible = false;
                                }
                                txtaveragebest.Attributes.Add("runat", "Server");
                                txtaveragebest.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtaveragebest);

                                ftbeavg = new FilteredTextBoxExtender();
                                ftbeavg.ID = "ftbecamavg" + id;
                                ftbeavg.TargetControlID = "txtaveragebest" + id;
                                ftbeavg.FilterType = FilterTypes.Numbers;
                                pan.Controls.Add(ftbeavg);

                                top = top + 25;
                                rbvindividual = new RadioButton();
                                rbvindividual.ID = "rbvindividual" + id;
                                rbvindividual.Text = "Individual Test";
                                rbvindividual.GroupName = "Cam" + id;
                                if (camoption == "4")
                                {
                                    rbvindividual.Checked = true;
                                }
                                else
                                {
                                    rbvindividual.Checked = false;
                                }
                                rbvindividual.AutoPostBack = true;
                                rbvindividual.CheckedChanged += this.rbvindividual_Checked;
                                rbvindividual.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rbvindividual);

                                top = top + 25;
                                lbl = new Label();
                                lbl.ID = "lblconvert" + id;
                                lbl.Text = "Convert To";
                                lbl.Style.Value = "left: 28px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(lbl);

                                txtconvert = new TextBox();
                                txtconvert.ID = "txtconvertcam" + id;
                                txtconvert.Width = 30;
                                txtconvert.MaxLength = 5;
                                if (camconvert != "")
                                {
                                    txtconvert.Text = camconvert;
                                }
                                txtconvert.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtconvert);

                                //ftbeavg = new FilteredTextBoxExtender();
                                //ftbeavg.ID = "ftbecamconvert" + id;
                                //ftbeavg.TargetControlID = "txtconvertcam" + id;
                                //ftbeavg.FilterType = FilterTypes.Numbers;
                                //pan.Controls.Add(ftbeavg);


                                rfvcam = new RequiredFieldValidator();
                                rfvcam.ID = "rfvcamconvert" + id;
                                rfvcam.ControlToValidate = "txtconvertcam" + id;
                                rfvcam.ForeColor = System.Drawing.Color.Red;
                                rfvcam.ErrorMessage = "Enter Value";
                                rfvcam.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rfvcam);

                                for (int sub = 0; sub < dssubject.Tables[0].Rows.Count; sub++)
                                {
                                    subid++;
                                    subtop = subtop + 25;
                                    chksub = new CheckBox();
                                    chksub.ID = "subchk" + subid;
                                    chksub.Text = "" + dssubject.Tables[0].Rows[sub]["criteria"] + "";

                                    cblSubSubject = new CheckBoxList();
                                    cblSubSubject.ID = "cblSubSubject" + subid;

                                    if (chkSubSub.Checked)
                                    {
                                        if (dtExamDetails.Rows.Count > 0)
                                        {

                                            dtExamDetails.DefaultView.RowFilter = "criteria='" + Convert.ToString(dssubject.Tables[0].Rows[sub]["criteria"]) + "'";
                                            DataTable dtSubss = dtExamDetails.DefaultView.ToTable();
                                            if (dtSubss.Rows.Count > 0)
                                            {
                                                cblSubSubject.DataSource = dtSubss;
                                                cblSubSubject.DataTextField = "subsubjectname";
                                                cblSubSubject.DataValueField = "subjectid";
                                                cblSubSubject.DataBind();
                                                cblSubSubject.Style.Value = "left: 500px; top: " + subtop + "px; position: absolute;";
                                                cblSubSubject.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                                                cblSubSubject.ForeColor = Color.Blue;
                                                //cblSubSubject.ite
                                                //cblSubSubject.Visible = false;
                                                //cblSubSubject.Attributes.Add(RepeatDirection="Horizontal");
                                                for (int cbl = 0; cbl < cblSubSubject.Items.Count; cbl++)
                                                {
                                                    string subs = Convert.ToString(hatsub[txtcriterianame.Text.Trim()]);
                                                    string cblVal = Convert.ToString(cblSubSubject.Items[cbl].Value);
                                                    if (!string.IsNullOrEmpty(subs) && subs.Contains(subs))
                                                    {

                                                    }
                                                }
                                                pan.Controls.Add(cblSubSubject);
                                            }
                                        }
                                    }

                                    //cblSubSubject.Text = "" + dssubject.Tables[0].Rows[sub]["criteria"] + "";


                                    chksub.Checked = false;
                                    //===========Start=============
                                    if (camcrititrian != "") //Modify By SakthiPriya
                                    {
                                        string[] tempcri = camcrititrian.Split('/');

                                        if (tempcri.GetUpperBound(0) >= 0)
                                        {
                                            for (int temcrin = 0; tempcri.GetUpperBound(0) >= temcrin; temcrin++)
                                            {
                                                string chk = tempcri[temcrin].ToString();
                                                if (chk == chksub.Text)
                                                {
                                                    chksub.Checked = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int l = 0; l < 1; l++)
                                            {
                                                chksub.Checked = true;
                                            }
                                        }
                                    }
                                    //==========End================
                                    chksub.Style.Value = "left: 400px; top: " + subtop + "px; position: absolute;";
                                    pan.Controls.Add(chksub);
                                }
                                if (camcrititrian == "") //Modify By SakthiPriya
                                {
                                    for (int l = 0; l < 1; l++)
                                    {
                                        chksub.Checked = true;
                                    }
                                }
                                top = top + 30;
                                chkinmarkcalset = new CheckBox();
                                chkinmarkcalset.ID = "chkround" + roundid;
                                chkinmarkcalset.Text = "Round of";
                                if (roundofcheck == "1")
                                {
                                    chkinmarkcalset.Checked = true;
                                }
                                else
                                {
                                    chkinmarkcalset.Checked = false;
                                }
                                chkinmarkcalset.AutoPostBack = true;
                                chkinmarkcalset.CheckedChanged += this.rbvround_Checked;
                                chkinmarkcalset.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(chkinmarkcalset);

                                txtsettings = new TextBox();
                                txtsettings.ID = "txtround" + roundid;
                                if (roundvalues.Trim() != "0" && roundvalues.Trim() != "")
                                {
                                    txtsettings.Text = roundvalues;
                                    txtsettings.Visible = true;
                                }
                                else
                                {
                                    txtsettings.Text = "";
                                    txtsettings.Visible = false;
                                }
                                txtsettings.Width = 40;
                                txtsettings.MaxLength = 1;
                                txtsettings.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtsettings);

                                ftbeavg = new FilteredTextBoxExtender();
                                ftbeavg.ID = "ftbesetting" + roundid;
                                ftbeavg.TargetControlID = "txtround" + roundid;
                                ftbeavg.FilterType = FilterTypes.Numbers;
                                pan.Controls.Add(ftbeavg);


                                rfvcam = new RequiredFieldValidator();
                                rfvcam.ID = "rfvsetting" + roundid;
                                rfvcam.ControlToValidate = "txtround" + roundid;
                                rfvcam.ErrorMessage = "Enter Value";
                                rfvcam.ForeColor = System.Drawing.Color.Red;
                                rfvcam.Style.Value = "left: 550px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rfvcam);

                                if (top < subtop)
                                {
                                    top = subtop;
                                    top = top + 50;
                                }
                                else
                                {
                                    top = top + 50;
                                }
                            }


                            //===================Start Attendance Controls=============================
                            if (top < subtop)
                            {
                                top = subtop;
                            }

                            if (chkattendance.Checked == true || attadd != 0)
                            {
                                chkattsem.Visible = true;
                                lblfromdate.Visible = true;
                                txtfromdate.Visible = true;
                                lbltodate.Visible = true;
                                txttodate.Visible = true;
                                rbvoverall.Visible = true;
                                rbvsubjectwise.Visible = true;
                                rbvattpercentage.Visible = true;
                                rbvattmaxmark.Visible = true;

                                string attendancevalue1 = "";
                                string att_calvalue = "";
                                string att_markvalue = "";
                                roundofcheck = "0";
                                roundvalues = "";
                                if (attendancevalue != "")
                                {
                                    string[] attendacespiltvalue = attendancevalue.Split('@');
                                    attendancevalue1 = attendacespiltvalue[0].ToString();
                                    att_calvalue = attendacespiltvalue[1].ToString();
                                    att_markvalue = attendacespiltvalue[2].ToString();
                                    roundofcheck = attendacespiltvalue[3].ToString();
                                    roundvalues = attendacespiltvalue[4].ToString();
                                }

                                top = top + 10;
                                lbl = new Label();
                                lbl.ID = "lblattendance" + id;
                                lbl.Text = "Attendance";
                                lbl.Font.Bold = true;
                                lbl.Font.Size = FontUnit.Medium;
                                lbl.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(lbl);

                                top = top + 25;
                                if (controlatt.Trim() != "chkattsem" && controlatt.Trim() != "txtfromdate" && controlatt.Trim() != "txttodate" && controlatt.Trim() != "btnsave")
                                {
                                    if (attendancevalue1 == "Sem Date" || chkattsem.Checked == true)
                                    {
                                        chkattsem.Checked = true;
                                        txtfromdate.Enabled = false;
                                        txttodate.Enabled = false;
                                    }
                                }
                                chkattsem.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";

                                top = top + 25;
                                if (attendancevalue1 != "" && attendancevalue1 != "Sem Date" && dateflag == false && attendancevalue1.ToLower() != "monthwise")
                                {

                                    string[] attendancload = attendancevalue1.Split(';');
                                    //manikandan 03Aug2013===============================================
                                    //txtfromdate.Text = attendancload[0].ToString();
                                    //txttodate.Text = attendancload[1].ToString();  
                                    string[] spitfrom = attendancload[0].Split('/');
                                    string[] spitto = attendancload[1].Split('/');
                                    DateTime dtfrom = Convert.ToDateTime(spitfrom[1].ToString() + '/' + spitfrom[0] + '/' + spitfrom[2]);
                                    DateTime dtto = Convert.ToDateTime(spitto[1].ToString() + '/' + spitto[0] + '/' + spitto[2]);
                                    //====================================================================
                                    txtfromdate.Text = dtfrom.ToString("dd/MM/yyyy");
                                    txttodate.Text = dtto.ToString("dd/MM/yyyy");
                                    txtfromdate.Enabled = true;
                                    txttodate.Enabled = true;
                                }
                                if (dateflag == true)
                                    dateflag = false;
                                lblfromdate.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                txtfromdate.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";

                                top = top + 35;
                                lbltodate.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                txttodate.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";

                                top = top + 35;
                                if (savefalg == false)
                                {
                                    if (att_calvalue == "1")
                                    {
                                        rbvoverall.Checked = true;
                                        rbvsubjectwise.Checked = false;
                                    }
                                }
                                rbvoverall.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                if (savefalg == false)
                                {
                                    if (att_calvalue == "2")
                                    {
                                        rbvsubjectwise.Checked = true;
                                        rbvoverall.Checked = false;
                                    }
                                }
                                rbvsubjectwise.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";

                                top = top + 25;
                                if (savefalg == false)
                                {
                                    if (att_markvalue == "1")
                                    {
                                        rbvattmaxmark.Checked = true;
                                        rbvattpercentage.Checked = false;
                                    }
                                }
                                rbvattmaxmark.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                if (savefalg == false)
                                {
                                    if (att_markvalue == "2")
                                    {
                                        rbvattpercentage.Checked = true;
                                        rbvattmaxmark.Checked = false;
                                    }
                                }
                                rbvattpercentage.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";

                                top = top + 30;
                                roundid++;
                                chkinmarkcalset = new CheckBox();
                                chkinmarkcalset.ID = "chkround" + roundid;
                                chkinmarkcalset.Text = "Round of";
                                if (roundofcheck == "1")
                                {
                                    chkinmarkcalset.Checked = true;
                                }
                                else
                                {
                                    chkinmarkcalset.Checked = false;
                                }
                                chkinmarkcalset.AutoPostBack = true;
                                chkinmarkcalset.CheckedChanged += this.rbvround_Checked;
                                chkinmarkcalset.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(chkinmarkcalset);

                                txtsettings = new TextBox();
                                txtsettings.ID = "txtround" + roundid;
                                if (roundvalues.Trim() != "0" && roundvalues.Trim() != "")
                                {
                                    txtsettings.Text = roundvalues;
                                    txtsettings.Visible = true;
                                }
                                else
                                {
                                    txtsettings.Text = "";
                                    txtsettings.Visible = false;
                                }
                                txtsettings.Width = 40;
                                txtsettings.MaxLength = 1;
                                txtsettings.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtsettings);

                                ftbeavg = new FilteredTextBoxExtender();
                                ftbeavg.ID = "ftbesetting" + roundid;
                                ftbeavg.TargetControlID = "txtround" + roundid;
                                ftbeavg.FilterType = FilterTypes.Numbers;
                                pan.Controls.Add(ftbeavg);


                                rfvcam = new RequiredFieldValidator();
                                rfvcam.ID = "rfvsetting" + roundid;
                                rfvcam.ControlToValidate = "txtround" + roundid;
                                rfvcam.ErrorMessage = "Enter Value";
                                rfvcam.ForeColor = System.Drawing.Color.Red;
                                rfvcam.Style.Value = "left: 550px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rfvcam);

                            }
                            else
                            {
                                chkattsem.Visible = false;
                                lblfromdate.Visible = false;
                                txtfromdate.Visible = false;
                                lbltodate.Visible = false;
                                txttodate.Visible = false;
                                rbvoverall.Visible = false;
                                rbvsubjectwise.Visible = false;
                                rbvattpercentage.Visible = false;
                                rbvattmaxmark.Visible = false;
                            }

                            //=====================Load Calculte Controls===================================
                            //calid = 0;
                            loadvalue = 0;
                            //int caltextcount = 0;
                            //DataView dv_demand_data = new DataView();
                            //dsgetdetails.Tables[0].DefaultView.RowFilter = "calculation_option<>''";
                            //dv_demand_data = dsgetdetails.Tables[0].DefaultView;
                            //int count4 = 0;
                            //count4 = dv_demand_data.Count;
                            string[] splitcalcutexts = calcutexts.Split(';');
                            for (int i = 0; i < calculatecout; i++)
                            {

                                string newcaltext = "";
                                //if (count4 > 0 && caltextcount < count4)
                                //{
                                //    newcaltext = dv_demand_data[caltextcount]["istype"].ToString();
                                //    caltextcount++;
                                //}
                                loadvalue++;
                                calid++;
                                top = top + 50;
                                lbl = new Label();
                                txtcalcu = new TextBox();


                                txtcalcu.ID = "txtcalculatlkmnsdflv" + calid;
                                int ssch = splitcalcutexts.GetUpperBound(0);
                                if (ssch == calculatecout)
                                {
                                    txtcalcu.Text = splitcalcutexts[i].ToString();
                                }
                                else
                                {
                                    txtcalcu.Text = "Calculate" + calid + "";
                                }

                                txtcalcu.Font.Bold = true;
                                txtcalcu.Font.Size = FontUnit.Medium;
                                txtcalcu.Style.Value = "left: 336px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtcalcu);

                                lbl.ID = "lblcalculatlkmnsdflv" + calid;
                                lbl.Text = "Calculate " + loadvalue;
                                lbl.Font.Bold = true;
                                lbl.Font.Size = FontUnit.Medium;
                                lbl.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(lbl);
                                string inclfinal = "";
                                string calcoption = "";
                                string calconversionvalue = "";
                                roundofcheck = "0";
                                roundvalues = "";
                                if (calcriteriavalue != "")
                                {
                                    spiltcalvalue = spiltcalcount[i].Split('@');
                                    calcoption = spiltcalvalue[1].ToString();
                                    inclfinal = spiltcalvalue[2].ToString();
                                    calconversionvalue = spiltcalvalue[3].ToString();
                                    roundofcheck = spiltcalvalue[4].ToString();
                                    roundvalues = spiltcalvalue[5].ToString();
                                    sumcrit = spiltcalvalue[6].ToString();
                                }

                                subtop = top + 25;
                                calid1 = 0;
                                attadd = 0;
                                for (int j = 0; j < count; j++)
                                {
                                    calid1++;
                                    top = top + 25;
                                    string nameid = "lblcriteria" + calid1;
                                    Label lblcal = new Label();
                                    lblcal = (Label)pan.FindControl(nameid);
                                    string cal = lblcal.Text;

                                    nameid = "txtcriname" + calid1;
                                    TextBox txtcrime = new TextBox();
                                    txtcrime = (TextBox)pan.FindControl(nameid);
                                    cal = txtcrime.Text;

                                    chkcalcriteria = new CheckBox();
                                    chkcalcriteria.ID = "chkcalcriteria" + calid1 + "" + calid;
                                    chkcalcriteria.Text = "" + cal + "";
                                    if (calcoption != "")
                                    {
                                        string[] chkcal = calcoption.Split(',');
                                        for (int k = 0; k <= chkcal.GetUpperBound(0); k++)
                                        {
                                            string chkcalval = chkcal[k].ToString();
                                            // if (chkcalval.ToString() == "Criteria " + calid1.ToString())
                                            if (chkcalval.ToString() == cal)
                                                chkcalcriteria.Checked = true;
                                            if (chkcalval.ToString() == "Attendance")
                                                attadd++;
                                        }
                                    }
                                    chkcalcriteria.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                    pan.Controls.Add(chkcalcriteria);
                                }
                                if (calcoption == "") //Modify By SakthiPriya
                                {
                                    chkcalcriteria.Checked = true;
                                }
                                int newsubtop = subtop + 10;
                                lblcalname = new Label();
                                lblcalname.ID = "lblcalname" + calid;
                                lblcalname.Text = "Convert To";
                                lblcalname.Style.Value = "left: 400px; top: " + newsubtop + "px; position: absolute;";
                                pan.Controls.Add(lblcalname);

                                txtcalconvert = new TextBox();
                                txtcalconvert.ID = "txtcalconvert" + calid;
                                txtcalconvert.Width = 40;
                                txtcalconvert.MaxLength = 5;
                                if (calconversionvalue.Trim() != "")
                                {
                                    txtcalconvert.Text = calconversionvalue;
                                }

                                txtcalconvert.Style.Value = "left: 470px; top: " + newsubtop + "px; position: absolute;";
                                pan.Controls.Add(txtcalconvert);

                                //ftbeavg = new FilteredTextBoxExtender();
                                //ftbeavg.ID = "ftbecalconvert" + calid;
                                //ftbeavg.TargetControlID = "txtcalconvert" + calid;
                                //ftbeavg.FilterType = FilterTypes.Numbers;
                                //pan.Controls.Add(ftbeavg);


                                rfvcam = new RequiredFieldValidator();
                                rfvcam.ID = "rfvcalconvert" + calid;
                                rfvcam.ControlToValidate = "txtcalconvert" + calid;
                                rfvcam.ErrorMessage = "Enter Value";
                                rfvcam.ForeColor = System.Drawing.Color.Red;
                                rfvcam.Style.Value = "left: 550px; top: " + subtop + "px; position: absolute;";
                                pan.Controls.Add(rfvcam);


                                if (chkattendance.Checked == true || attadd != 0)
                                {
                                    top = top + 25;
                                    chkcalother = new CheckBox();
                                    chkcalother.ID = "chkcalattendance" + calid;
                                    chkcalother.Text = "Attendance";
                                    if (attadd != 0)
                                    {
                                        chkcalother.Checked = true;
                                    }
                                    chkcalother.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                    pan.Controls.Add(chkcalother);
                                }


                                int countcalcontrol = i;
                                calid1 = 0;
                                if (countcalcontrol != 0)
                                {
                                    for (int cl = 0; cl < countcalcontrol; cl++)
                                    {
                                        top = top + 25;
                                        calid1++;
                                        chkcalculation = new CheckBox();
                                        chkcalculation.ID = "chkcalculatonvalue" + calid + "" + calid1;
                                        chkcalculation.Text = "Calculate " + calid1;
                                        string chkcalval = "Calculate " + calid1;
                                        if (calcoption != "")
                                        {
                                            string[] chkcal = calcoption.Split(',');
                                            for (int k = 0; k <= chkcal.GetUpperBound(0); k++)
                                            {
                                                string chkcheck = chkcal[k].ToString();
                                                if (chkcalval == chkcheck)
                                                    chkcalculation.Checked = true;
                                            }
                                        }

                                        chkcalculation.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                        pan.Controls.Add(chkcalculation);
                                    }
                                }


                                top = top + 30;
                                roundid++;
                                chkinmarkcalset = new CheckBox();
                                chkinmarkcalset.ID = "chkround" + roundid;
                                chkinmarkcalset.Text = "Round of";
                                if (roundofcheck == "1")
                                {
                                    chkinmarkcalset.Checked = true;
                                }
                                else
                                {
                                    chkinmarkcalset.Checked = false;
                                }
                                chkinmarkcalset.AutoPostBack = true;
                                chkinmarkcalset.CheckedChanged += this.rbvround_Checked;
                                chkinmarkcalset.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(chkinmarkcalset);

                                txtsettings = new TextBox();
                                txtsettings.ID = "txtround" + roundid;
                                if (roundvalues.Trim() != "0" && roundvalues.Trim() != "")
                                {
                                    txtsettings.Text = roundvalues;
                                    txtsettings.Visible = true;
                                }
                                else
                                {
                                    txtsettings.Text = "";
                                    txtsettings.Visible = false;
                                }
                                txtsettings.Width = 40;
                                txtsettings.MaxLength = 1;
                                txtsettings.Style.Value = "left: 180px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(txtsettings);

                                ftbeavg = new FilteredTextBoxExtender();
                                ftbeavg.ID = "ftbesetting" + roundid;
                                ftbeavg.TargetControlID = "txtround" + roundid;
                                ftbeavg.FilterType = FilterTypes.Numbers;
                                pan.Controls.Add(ftbeavg);


                                rfvcam = new RequiredFieldValidator();
                                rfvcam.ID = "rfvsetting" + roundid;
                                rfvcam.ControlToValidate = "txtround" + roundid;
                                rfvcam.ErrorMessage = "Enter Value";
                                rfvcam.ForeColor = System.Drawing.Color.Red;
                                rfvcam.Style.Value = "left: 550px; top: " + top + "px; position: absolute;";
                                pan.Controls.Add(rfvcam);

                                if (i == calculatecout - 1)
                                {
                                    top = top + 5;
                                    chkcalother = new CheckBox();
                                    chkcalother.ID = "chkcalincludefinal" + calid;
                                    chkcalother.Text = "Include In Final Calc";
                                    chkcalother.Checked = true;
                                    chkcalother.Enabled = false;
                                    if (inclfinal == "1")
                                    {
                                        chkcalother.Checked = true;
                                    }
                                    chkcalother.Visible = false;
                                    chkcalother.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                                    pan.Controls.Add(chkcalother);
                                }


                            }
                            //=============Settings==============
                            calid = 1;
                            top = top + 30;
                            lbl = new Label();
                            lbl.ID = "lblsettings" + calid;
                            lbl.Text = "Settings";
                            lbl.Font.Bold = true;
                            lbl.Font.Size = FontUnit.Medium;
                            lbl.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(lbl);
                            string settings = "1";

                            if (settingvalue != "")
                            {
                                settings = settingvalue.ToString().Trim();
                            }

                            top = top + 40;
                            chkcalother = new CheckBox();
                            chkcalother.ID = "chksumselectedcriteria" + calid;
                            chkcalother.Text = "Sum selected Criteria for Final Calculation";
                            if (sumcrit == "1")
                            {
                                chkcalother.Checked = true;
                            }
                            chkcalother.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(chkcalother);

                            top = top + 40;
                            lbl = new Label();
                            lbl.ID = "lblintmarkcalcula" + calid;
                            lbl.Text = "Internal Mark Calculation";
                            lbl.Font.Bold = true;
                            lbl.Font.Size = FontUnit.Medium;
                            lbl.Style.Value = "left: 50px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(lbl);

                            top = top + 30;
                            rbvsetting = new RadioButton();
                            rbvsetting.ID = "rbvtotalmark" + calid;
                            rbvsetting.GroupName = "Settings";
                            rbvsetting.Checked = true;
                            if (settings == "1")
                            {
                                rbvsetting.Checked = true;
                            }
                            else
                            {

                                rbvsetting.Checked = false;
                            }
                            rbvsetting.Text = "Total Marks";
                            rbvsetting.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(rbvsetting);

                            rbvsetting = new RadioButton();
                            rbvsetting.ID = "rbvincludeinter" + calid;
                            rbvsetting.Text = "Min Internal Marks";
                            rbvsetting.GroupName = "Settings";
                            if (settings == "2")
                            {
                                rbvsetting.Checked = true;

                            }
                            else
                            {
                                rbvsetting.Checked = false;
                            }
                            rbvsetting.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(rbvsetting);

                            top = top + 40;
                            lbl = new Label();
                            lbl.ID = "lbltotwords" + calid;
                            lbl.Text = "Total in Words For";
                            lbl.Font.Bold = true;
                            lbl.Font.Size = FontUnit.Medium;
                            lbl.Style.Value = "left: 50px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(lbl);

                            string setword = "2";
                            if (wordssettings.Trim() != "" && wordssettings != null)
                            {
                                setword = wordssettings.ToString();
                            }
                            top = top + 30;
                            rbvsetting = new RadioButton();
                            rbvsetting.ID = "rbwordmin" + calid;
                            rbvsetting.GroupName = "words";
                            rbvsetting.Checked = true;
                            if (setword == "1")
                            {
                                rbvsetting.Checked = true;
                            }
                            else
                            {
                                rbvsetting.Checked = false;
                            }
                            rbvsetting.Text = "Min Internal Marks";
                            rbvsetting.Style.Value = "left: 25px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(rbvsetting);

                            rbvsetting = new RadioButton();
                            rbvsetting.ID = "rbwordmax" + calid;
                            rbvsetting.Text = "Max Internal Marks";
                            rbvsetting.GroupName = "words";
                            if (setword == "2")
                            {
                                rbvsetting.Checked = true;
                            }
                            else
                            {
                                rbvsetting.Checked = false;
                            }
                            rbvsetting.Style.Value = "left: 250px; top: " + top + "px; position: absolute;";
                            pan.Controls.Add(rbvsetting);

                            top = top + 40;
                            btnsave.Style.Value = "left: 280px; top: " + top + "px; position: absolute;";
                            btnclose.Style.Value = "left: 350px; top: " + top + "px; position: absolute;";
                            panel4.Height = top + 50;
                            panel4.Visible = true;
                        }
                        else
                        {
                            panel4.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "Please Enter Greater Than 0";
                        }

                    }
                    else
                    {
                        panel4.Visible = false;
                        errmsg.Visible = true;
                        errmsg.Text = "No Test Conducted";
                    }
                }
                else
                {
                    panel4.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Exam Conducted";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
            panel4.Visible = false;
        }
    }
    #region "Dynamic Checked Change"

    private FilterTypes AjaxControlToolkit(string p)
    {
        throw new NotImplementedException();
    }

    protected void rbvround_Checked(object sender, EventArgs e)
    {
        int roundcount = Convert.ToInt32(txtcriteria.Text);
        roundcount = roundcount + Convert.ToInt32(txtcalculate.Text);
        if (chkattendance.Checked == true)
        {
            roundcount++;
        }
        int roundcheckid = 0;
        for (int i = 0; i < roundcount; i++)
        {
            roundcheckid++;
            CheckBox rbvround = (CheckBox)panel4.FindControl("chkround" + roundcheckid);
            TextBox txtavgvalue = (TextBox)panel4.FindControl("txtround" + roundcheckid);
            if (rbvround.Checked == true)
            {
                txtavgvalue.Visible = true;
            }
            else
            {
                txtavgvalue.Visible = false;
            }
        }
    }

    protected void CheckBoxChanged(object sender, EventArgs e)
    {
        int a = Convert.ToInt32(txtcriteria.Text);
        int tol = 0;
        for (int s = 0; s < a; s++)
        {
            tol++;
            string rbtotalid = "rbvAveragebest" + tol;
            RadioButton rbveragebest = (RadioButton)panel4.FindControl(rbtotalid);
            rbtotalid = "txtaveragebest" + tol;
            TextBox txtavgvalue = (TextBox)panel4.FindControl(rbtotalid);
            if (rbveragebest.Checked == true)
            {
                txtavgvalue.Visible = true;
            }
            else
            {
                txtavgvalue.Visible = false;
            }
        }
    }


    protected void rbvtotal_Checked(object sender, EventArgs e)
    {
        int a = Convert.ToInt32(txtcriteria.Text);
        int tol = 0;
        for (int s = 0; s < a; s++)
        {
            tol++;
            string rbtotalid = "rbvtotal" + tol;
            RadioButton rbvtotal = (RadioButton)panel4.FindControl(rbtotalid);
            rbtotalid = "txtaveragebest" + tol;
            TextBox txtavgvalue = (TextBox)panel4.FindControl(rbtotalid);
            if (rbvtotal.Checked == true)
            {
                txtavgvalue.Visible = false;
            }
            else
            {
                txtavgvalue.Visible = true;
            }
        }
    }

    protected void rbvindividual_Checked(object sender, EventArgs e)
    {
        int a = Convert.ToInt32(txtcriteria.Text);
        int tol = 0;
        for (int s = 0; s < a; s++)
        {
            tol++;
            string rbtotalid = "rbvindividual" + tol;
            RadioButton rbvindividual = (RadioButton)panel4.FindControl(rbtotalid);
            rbtotalid = "txtaveragebest" + tol;
            TextBox txtavgvalue = (TextBox)panel4.FindControl(rbtotalid);
            if (rbvindividual.Checked == true)
            {
                txtavgvalue.Visible = false;
            }
            else
            {
                rbvbest.Visible = true;
            }
        }
    }

    protected void rbvdirect_Checked(object sender, EventArgs e)
    {
        int a = Convert.ToInt32(txtcriteria.Text);
        int tol = 0;
        for (int s = 0; s < a; s++)
        {
            tol++;
            string rbtotalid = "rbvdirect" + tol;
            RadioButton rbvdirect = (RadioButton)panel4.FindControl(rbtotalid);
            rbtotalid = "txtaveragebest" + tol;
            TextBox txtavgvalue = (TextBox)panel4.FindControl(rbtotalid);
            if (rbvdirect.Checked == true)
            {
                txtavgvalue.Visible = false;
            }
            else
            {
                rbvbest.Visible = true;
            }
        }
    }

    protected void rbvbest_Checked(object sender, EventArgs e)
    {
        int a = Convert.ToInt32(txtcriteria.Text);
        int tol = 0;
        for (int s = 0; s < a; s++)
        {
            tol++;
            string rbtotalid = "rbvbest" + tol;
            RadioButton rbvbest = (RadioButton)panel4.FindControl(rbtotalid);
            rbtotalid = "txtaveragebest" + tol;
            TextBox txtavgvalue = (TextBox)panel4.FindControl(rbtotalid);
            if (rbvbest.Checked == true)
            {
                txtavgvalue.Visible = false;
            }
            else
            {
                rbvbest.Visible = true;
            }
        }
    }

    #endregion
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string strsecval = "";
        try
        {
            //if (chksub.Checked == true)   //Modify By SakthiPriya
            //{
            errror.Visible = false;
            //loadcontrols();
            errmsg.Visible = false;
            int calct = 0;
            string Criteria_Cam = "";
            int subchec = 0;
            foreach (GridViewRow s in GridView1.Rows)
            {
                calct++;
                int isval = 0;
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)s.FindControl("cbselect");
                if (chk.Checked == true)
                {
                    Label sylcod = (Label)s.FindControl("lblsyllcode");
                    Label subno = (Label)s.FindControl("lblsubno");
                    Label sec = (Label)s.FindControl("lblsection");
                    subjectno = subno.Text;
                    syllcode = sylcod.Text;
                    string sections = sec.Text;
                    strsecval = "";
                    if (sections.Trim() != "" && sections != "0" && sections != "-1" && sections.Trim().ToLower() != "all")
                    {
                        strsecval = " and sections='" + sections + "'";
                    }

                    //string strchequery = "Select * From internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "'";
                    //ds = da.select_method(strchequery, hat, "Text");
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{

                    int del = da.insert_method("Delete internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + "", hat, "Text");
                    //}

                    //========Save Criteria Calculation==================================
                    int count = Convert.ToInt32(txtcriteria.Text);
                    int id = 0;
                    int subjid = 0;
                    int roundid = 0;
                    for (int i = 0; i < count; i++)
                    {
                        id++;
                        string convertid = "txtconvertcam" + id;
                        TextBox tbconvert = (TextBox)panel4.FindControl(convertid);
                        Covertvalue = tbconvert.Text.ToString();


                        string crimenameid = "txtcriname" + id;
                        TextBox tbcrimename = (TextBox)panel4.FindControl(crimenameid);
                        criname = tbcrimename.Text.ToString();

                        string rbtotalid = "rbvtotal" + id;
                        RadioButton rbtotal = (RadioButton)panel4.FindControl(rbtotalid);
                        if (rbtotal.Checked == true)
                        {
                            Criteria_Cam = "1";
                        }

                        rbtotalid = "rbvbest" + id;
                        RadioButton rbbest = (RadioButton)panel4.FindControl(rbtotalid);
                        if (rbbest.Checked == true)
                        {
                            Criteria_Cam = "2";
                        }
                        string avgbestval = " 0";
                        rbtotalid = "rbvAveragebest" + id;
                        RadioButton rbveragebest = (RadioButton)panel4.FindControl(rbtotalid);
                        if (rbveragebest.Checked == true)
                        {
                            Criteria_Cam = "3";
                            convertid = "txtaveragebest" + id;
                            TextBox txtavgvalue = (TextBox)panel4.FindControl(convertid);
                            avgbestval = txtavgvalue.Text.ToString();
                        }

                        rbtotalid = "rbvindividual" + id;
                        RadioButton rbindividual = (RadioButton)panel4.FindControl(rbtotalid);

                        if (rbindividual.Checked == true)
                        {
                            Criteria_Cam = "4";
                        }

                        // ========== Add Round Value=========
                        roundid++;
                        string Roundchks = "0";
                        string roundvalueget = "0";
                        CheckBox chkroundvalue = (CheckBox)panel4.FindControl("chkround" + roundid);
                        if (chkroundvalue.Checked == true)
                        {
                            Roundchks = "1";
                            TextBox txtroundvalue = (TextBox)panel4.FindControl("txtround" + roundid);
                            roundvalueget = txtroundvalue.Text;
                        }
                        else
                        {
                            Roundchks = "0";
                            roundvalueget = "";
                        }

                        Testname = "";
                        string subjectDet = string.Empty;
                        for (int sub = 0; sub < dssubject.Tables[0].Rows.Count; sub++)
                        {
                            subjid++;
                            string subjectid = "subchk" + subjid;
                            string cNo = string.Empty;
                            // string subSubjectDet=string.Empty;
                            CheckBox chksubjcet = (CheckBox)panel4.FindControl(subjectid);

                            if (chksubjcet.Checked == true)
                            {
                                string SubSubj = string.Empty;

                                string Subsubjectid = "cblSubSubject" + subjid;
                                CheckBoxList cblSubSub = (CheckBoxList)panel4.FindControl(Subsubjectid);
                                if (cblSubSub != null)
                                {
                                    if (cblSubSub.Items.Count > 0)
                                        SubSubj = getCblSelectedValue123(cblSubSub);
                                }

                                if (Testname == "")
                                {

                                    string strcriteriano = da.GetFunction("select criteria_no from criteriaforinternal c,subject s where c.syll_code=s.syll_code and s.subject_no='" + subjectno + "' and c.syll_code='" + syllcode + "' and c.criteria='" + chksubjcet.Text + "'");

                                    Testname = "" + strcriteriano + "";
                                    cNo = strcriteriano;
                                }
                                else
                                {
                                    string strcriteriano = da.GetFunction("select criteria_no from criteriaforinternal c,subject s where c.syll_code=s.syll_code and s.subject_no='" + subjectno + "' and c.syll_code='" + syllcode + "' and c.criteria='" + chksubjcet.Text + "'");
                                    Testname = "" + Testname + "," + strcriteriano + "";
                                    cNo = strcriteriano;
                                }
                                if (Criteria_Cam == "4")
                                {
                                    subjid = subjid + dssubject.Tables[0].Rows.Count - (sub + 1);
                                    sub = dssubject.Tables[0].Rows.Count;

                                }

                                if (string.IsNullOrEmpty(subjectDet))
                                    subjectDet = cNo + "-" + SubSubj;
                                else
                                    subjectDet = subjectDet + ";" + cNo + "-" + SubSubj;
                            }
                        }
                        if (chkSubSub.Checked == false)
                            subjectDet = string.Empty;

                        string strsave = "insert into internal_cam_calculation_master_setting (Istype,subject_no,syll_code,Calculate_Cam_Criteria,Criteria_no,Cam_Option,Cam_Ave_best,Conversion_value,Round_of,Round_Value,sections,subjectid) values";
                        // strsave = strsave + "('Criteria " + id + "','" + subjectno + "','" + syllcode + "','" + id + "','" + Testname + "','" + Criteria_Cam + "','" + avgbestval + "','" + Covertvalue + "','" + Roundchks + "','" + roundvalueget + "')";
                        strsave = strsave + "('" + criname + "','" + subjectno + "','" + syllcode + "','" + id + "','" + Testname + "','" + Criteria_Cam + "','" + avgbestval + "','" + Covertvalue + "','" + Roundchks + "','" + roundvalueget + "','" + sections + "','" + subjectDet + "')";

                        int save = da.insert_method(strsave, hat, "Text");



                    }

                    //=========Save To Attendance========================
                    if (chkattendance.Checked == true)
                    {
                        string batch = string.Empty;
                        string degree = string.Empty;
                        string sem = string.Empty;
                        string getbatch = " select batch_year,degree_code,semester from syllabus_master where syll_code=" + syllcode + "";
                        ds.Reset();
                        ds.Dispose();
                        ds = da.select_method(getbatch, hat, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            batch = ds.Tables[0].Rows[0]["Batch_year"].ToString();
                            degree = ds.Tables[0].Rows[0]["degree_code"].ToString();
                            sem = ds.Tables[0].Rows[0]["semester"].ToString();
                        }

                        int attval = 0;
                        if (chkbasedSettings.Checked == true)
                        {
                            string dicDate = "select fromDate,toDate from AttendanceMarkEntry ae,AttendanceMarkValue av where ae.AttndId=av.AttndId and ae.BathYear='" + batch + "' and DegreeCode='" + degree + "' and semester='" + sem + "'";

                        }
                        string Att_Call = "";
                        string Attendance_value = "";
                        string Attendance = "";
                        string fromSpilt = txtfromdate.Text;

                        string tosplit = txttodate.Text;
                        Attendance = fromSpilt + ';' + tosplit;

                        if (chkattsem.Checked == true)
                        {
                            Attendance = "Sem Date";
                        }
                        if (chkbasedSettings.Checked == true)
                        {
                            Attendance = "MonthWise";
                        }

                        if (rbvoverall.Checked == true)
                        {
                            Att_Call = "1";
                        }
                        if (rbvsubjectwise.Checked == true)
                        {
                            Att_Call = "2";
                        }
                        if (rbvattmaxmark.Checked == true)
                        {
                            Attendance_value = "1";
                        }
                        if (rbvattpercentage.Checked == true)
                        {
                            Attendance_value = "2";
                        }

                        roundid++;
                        string Roundchks = "0";
                        string roundvalueget = "0";
                        CheckBox chkroundvalue = (CheckBox)panel4.FindControl("chkround" + roundid);
                        if (chkroundvalue.Checked == true)
                        {
                            Roundchks = "1";
                            TextBox txtroundvalue = (TextBox)panel4.FindControl("txtround" + roundid);
                            roundvalueget = txtroundvalue.Text;
                        }
                        else
                        {
                            Roundchks = "0";
                            roundvalueget = "";
                        }

                        string strinsertquery = "insert into internal_cam_calculation_master_setting (istype,subject_no,syll_code,Attendance,Att_Cal,Att_Mark_Per,Round_of,Round_Value,sections) values";
                        strinsertquery = strinsertquery + "('Attendance'," + subjectno + "," + syllcode + ",'" + Attendance + "','" + Att_Call + "','" + Attendance_value + "','" + Roundchks + "','" + roundvalueget + "','" + sections + "')";
                        int o = da.insert_method(strinsertquery, hat, "Text");
                    }

                    //============Save To Calculate ========================
                    int calculatecout = Convert.ToInt32(txtcalculate.Text);
                    id = 0;
                    string calconvert = "";
                    string CalculationOption = "";
                    string IncludeFinalCalculation = "0";
                    string criteria = "Cam";
                    for (int i = 0; i < calculatecout; i++)
                    {
                        id++;
                        CalculationOption = "";
                        string convertid = "";

                        subjid = 0;
                        for (int calc = 0; calc < count; calc++)
                        {
                            subjid++;


                            string subjectid = "chkcalcriteria" + subjid + "" + id;
                            CheckBox chksubjcet = (CheckBox)panel4.FindControl(subjectid);
                            if (chksubjcet.Checked == true)
                            {
                                string criname = "txtcriname" + subjid.ToString();
                                TextBox txtcriname = (TextBox)panel4.FindControl(criname);
                                if (CalculationOption == "")
                                {
                                    CalculationOption = txtcriname.Text;
                                }
                                else
                                {
                                    //CalculationOption = CalculationOption + ',' + "Criteria " + subjid.ToString();
                                    CalculationOption = CalculationOption + ',' + txtcriname.Text;
                                }
                            }
                        }
                        if (chkattendance.Checked == true)
                        {
                            convertid = "chkcalattendance" + id;
                            CheckBox chkcalattendance1 = (CheckBox)panel4.FindControl(convertid);
                            if (chkcalattendance1.Checked == true)
                            {
                                if (CalculationOption == "")
                                    CalculationOption = chkcalattendance1.Text.ToString();
                                else
                                    CalculationOption = CalculationOption + ',' + chkcalattendance1.Text.ToString();
                            }
                        }

                        if (i != 0)
                        {
                            subjid = 0;
                            for (int calc = 0; calc < i; calc++)
                            {
                                subjid++;
                                string subjectid = "chkcalculatonvalue" + id + "" + subjid;
                                CheckBox chksubjcet = (CheckBox)panel4.FindControl(subjectid);
                                if (chksubjcet.Checked == true)
                                {
                                    if (CalculationOption == "")
                                        CalculationOption = "Calculate " + subjid.ToString();
                                    else
                                        CalculationOption = CalculationOption + ',' + "Calculate " + subjid.ToString();
                                }
                            }
                        }

                        if (i == calculatecout - 1)
                        {
                            convertid = "chkcalincludefinal" + id;
                            CheckBox chkcalincludefinal = (CheckBox)panel4.FindControl(convertid);
                            if (chkcalincludefinal.Checked == true)
                            {
                                IncludeFinalCalculation = "1";
                            }
                        }
                        convertid = "txtcalconvert" + id;
                        TextBox txtconvert = (TextBox)panel4.FindControl(convertid);
                        calconvert = txtconvert.Text.ToString();

                        roundid++;
                        string Roundchks = "0";
                        string roundvalueget = "0";
                        CheckBox chkroundvalue = (CheckBox)panel4.FindControl("chkround" + roundid);
                        if (chkroundvalue.Checked == true)
                        {
                            Roundchks = "1";
                            TextBox txtroundvalue = (TextBox)panel4.FindControl("txtround" + roundid);
                            roundvalueget = txtroundvalue.Text;
                        }
                        else
                        {
                            Roundchks = "0";
                            roundvalueget = "";
                        }

                        string sumcriteria = "0";
                        CheckBox sumcriteriasel = (CheckBox)panel4.FindControl("chksumselectedcriteria1");
                        if (sumcriteriasel.Checked == true)
                        {
                            sumcriteria = "1";
                        }

                        string crimenameid = "txtcalculatlkmnsdflv" + id;
                        TextBox tbcrimename = (TextBox)panel4.FindControl(crimenameid);
                        string calcriname = tbcrimename.Text.ToString();

                        string strcalinsertquery = "insert into internal_cam_calculation_master_setting (istype,subject_no,syll_code,Calculation_Criteria,Calculation_Option,Include_Final_Calculation,Conversion_value,Round_of,Round_Value,sum_select_criteria,sections) values";
                        strcalinsertquery = strcalinsertquery + "('" + calcriname + "'," + subjectno + "," + syllcode + ",'" + id + "','" + CalculationOption + "','" + IncludeFinalCalculation + "'," + calconvert + ",'" + Roundchks + "','" + roundvalueget + "','" + sumcriteria + "','" + sections + "')";

                        int p = da.insert_method(strcalinsertquery, hat, "Text");
                    }

                    //==========Settings==============

                    string intermarkset = "1";
                    RadioButton rvvsett = (RadioButton)panel4.FindControl("rbvtotalmark1");
                    if (rvvsett.Checked == true)
                    {
                        intermarkset = "1";
                    }
                    rvvsett = (RadioButton)panel4.FindControl("rbvincludeinter1");
                    if (rvvsett.Checked == true)
                    {
                        intermarkset = "2";
                    }
                    string wordsetting = "1";
                    rvvsett = (RadioButton)panel4.FindControl("rbwordmin1");
                    if (rvvsett.Checked == true)
                    {
                        wordsetting = "1";
                    }
                    rvvsett = (RadioButton)panel4.FindControl("rbwordmax1");
                    if (rvvsett.Checked == true)
                    {
                        wordsetting = "2";
                    }


                    string strsettingquery = "insert into internal_cam_calculation_master_setting (istype,subject_no,syll_code,int_Mark_settings,sum_select_criteria,sections) values";
                    strsettingquery = strsettingquery + "('Settings'," + subjectno + "," + syllcode + ",'" + intermarkset + "','" + wordsetting + "','" + sections + "')";
                    int set = da.insert_method(strsettingquery, hat, "Text");


                    subchec++;

                }
            }
            //txtcriteria.Text = "";
            //txtcalculate.Text = "";
            //chkattendance.Checked = false;
            if (subchec != 0)
            {
                btngo_Click(sender, e);
                panel4.Visible = false;
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Record Save  Successfully')", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Record Saved  Successfully!')", true);
            }
            //  }
            //else
            //{
            //    errror.Visible = true;
            //    errror.Text = "please select any one test";

            //}
        }
        catch (Exception ex)
        {
            int del = da.insert_method("Delete internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "' " + strsecval + "", hat, "Text");
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
            panel4.Visible = false;
        }
    }
    public string getCblSelectedValue123(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    protected void btncriteria_Click(object sender, EventArgs e)
    {
        try
        {
            // Fpinternal.SaveChanges();
            //Criteria Controls
            rbvtotal = null;
            rbvbest = null;
            rbvAveragebest = null;
            rbvindividual = null;
            rbvsetting = null;
            chkinmarkcalset = null;
            lbl = null;
            txtaveragebest = null;
            txtconvert = null;
            pan = null;
            chksub = null;
            txtsettings = null;
            txtcriterianame = null;

            //Calculate Controls
            chkcalcriteria = null;
            chkcalculation = null;
            chkcalother = null;
            lblcalname = null;
            txtcalconvert = null;
            ftbeavg = null;
            rfvcam = null;

            top = 10;
            subtop = 10;
            subid = 0;
            id = 0;
            calid = 0;
            calid1 = 0;
            roundid = 0;


            chk_flag = false;
            subjectno = "";
            syllcode = "";
            GridView1.Visible = true;
            subcount = 0;
            chkattsem.Checked = false;
            txtfromdate.Enabled = true;
            txttodate.Enabled = true;
            GridView2.Visible = false;
            txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            foreach (GridViewRow s in GridView1.Rows)
            // for (int s = 1; Fpinternal.Sheets[0].RowCount > s; s++)
            {
                int isval = 0;
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)s.FindControl("cbselect");
                if (chk.Checked == true)
                {
                    Label sylcod = (Label)s.FindControl("lblsyllcode");
                    Label subno = (Label)s.FindControl("lblsubno");
                    subcount++;
                    if (subjectno == "")
                    {
                        subjectno = subno.Text;
                        syllcode = sylcod.Text;
                    }
                    else
                    {
                        subjectno = subjectno + ',' + (subno.Text);
                        syllcode = syllcode + ',' + (sylcod.Text);
                    }
                }
            }
            if (subcount != 0)
            {
                errmsg.Visible = false;
                panel4.Visible = true;
                loadcontrols();
            }
            else
            {
                panel4.Visible = false;
                errmsg.Visible = true;
                txtcriteria.Text = "";
                txtcalculate.Text = "";
                errmsg.Text = "Please Select Subject And Proceed";
            }
            if (chkbasedSettings.Checked == true)
            {

                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
                chkattsem.Checked = false;
                chkattsem.Enabled = false;
            }
            else
            {
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
                chkattsem.Checked = true;
                chkattsem.Enabled = true;
            }
            chkbasedSettings.Visible = true;
        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }
    }
    protected void btnclose_Click(object sender, EventArgs e)
    {
        panel4.Visible = false;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        lblreportname.Visible = false;
        string reportname = txtreport.Text;

        if (reportname != "")
        {
            da.printexcelreportgrid(GridView2, reportname);
        }
        else
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Report Name";
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        string degreedetails = "CA Mark Details-Course wise" + "@Exam Month & Year: " + ddlexammonth.SelectedItem.ToString() + " " + ddlexamyear.SelectedItem.ToString();
        string pagename = "Cam Internal mark Calculation.aspx";
        string ss = Session["usercode"].ToString();
        GridView2.Visible = true;
        Printcontrol.loadspreaddetails(GridView2, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;

    }
    protected void btndelete_OnClick(object sender, EventArgs e)
    {
        GridView2.Visible = false;
        Button del = (Button)sender;
        string rowind = del.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int row = Convert.ToInt32(rowind) - 2;
        Session["rowIndex"] = Convert.ToString(row);
        Label sylcod = (Label)GridView1.Rows[row].FindControl("lblsyllcode");
        syllcode = sylcod.Text;
        Label subno = (Label)GridView1.Rows[row].FindControl("lblsubno");
        subjectno = subno.Text;
        panel4.Visible = false;
        chk_flag = false;
        Label degdet = (Label)GridView1.Rows[row].FindControl("lbldegree");
        string degreedetails = degdet.Text;
        chkattendance.Checked = false;
        chkSubSub.Checked = false;
        txtcriteria.Text = "";
        txtcalculate.Text = "";
        deletecamcalculation(degreedetails);
        //FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        //Fpinternal.Sheets[0].Cells[row, 4].CellType = txt;
        //Fpinternal.Sheets[0].Cells[row, 5].CellType = txt;
        //Fpinternal.Sheets[0].Cells[row, 6].CellType = txt;
        //Fpinternal.Sheets[0].Cells[row, 7].CellType = txt;
        //Fpinternal.Sheets[0].Cells[row, 2].BackColor = Color.White;
        divPrint.Visible = false;
        txtreport.Visible = false;
        lblreportname.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btngo_Click(sender, e);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Student Cam Internal Marks Calculation Deleted Successfully!')", true);

    }
    public void deletecamcalculation(string degreedetails)
    {
        try
        {
            string sec = "";
            string acrw = Convert.ToString(Session["rowIndex"]);
            int activerow = Convert.ToInt32(acrw);
            Label secst = (Label)GridView1.Rows[activerow].FindControl("lblsection");
            string sections = secst.Text;
            if (ckhdegreewise.Checked == true)
            {
                if (sections != "" && sections.Trim().ToLower() != "all" && sections.ToString() != "-1")
                {
                    sec = " and sections='" + sections + "'";
                }
            }
            int del = da.update_method_wo_parameter("Delete internal_cam_calculation_master_setting where subject_no='" + subjectno + "' and syll_code='" + syllcode + "'" + sec, "Text");
            del = da.update_method_wo_parameter("Delete from tbl_Cam_Calculation where subject_no='" + subjectno + "' and syll_code='" + syllcode + "'" + sec, "Text");
            del = da.update_method_wo_parameter("Delete from camarks where subject_no='" + subjectno + "' " + sec + "", "Text");


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
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
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
    public void BindRightsBaseBatch()
    {
        try
        {
            DataSet dsBatch = new DataSet();
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            chkBatch.Checked = false;
            cblBatch.Items.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollege = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollege = " and r.college_code in(" + collegeCode + ")";
            }

            dsBatch.Clear();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = da.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            string batchquery = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollege))
            {
                batchquery = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 and r.cc='0' and delflag='0' and exam_flag<>'debar' " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                //ds.Clear();
                ds = da.select_method_wo_parameter(batchquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBatch.DataSource = ds;
                    cblBatch.DataTextField = "Batch_Year";
                    cblBatch.DataValueField = "Batch_Year";
                    cblBatch.DataBind();

                    checkBoxListselectOrDeselect(cblBatch, true);
                    CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch1.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void binddegree1()
    {
        try
        {
            ds.Clear();
            txtDegree.Text = "---Select---";
            string batchCode = string.Empty;
            chkDegree.Checked = false;
            cblDegree.Items.Clear();
            //userCode = Session["usercode"].ToString();
            //singleUser = Session["single_user"].ToString();
            //groupUserCode = Session["group_code"].ToString();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                checkBoxListselectOrDeselect(cblDegree, true);
                CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree1.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch1()
    {
        try
        {
            string degreecode = string.Empty;
            //collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            txtBranch.Text = "---Select---";
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            //string valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            //string valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            string valBatch = string.Empty;// rs.GetSelectedItemsValueAsString(cblBatch);
            string valDegree = string.Empty;//rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblDegree.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblDegree);

            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBranch.DataSource = ds;
                cblBranch.DataTextField = "dept_name";
                cblBranch.DataValueField = "degree_code";
                cblBranch.DataBind();
                checkBoxListselectOrDeselect(cblBranch, true);
                CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch1.Text, "--Select--");
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void bindsem1()
    {
        try
        {
            dtCommon.Clear();
            txtSem.Text = "---Select---";
            chksem.Checked = false;
            cblSem.Items.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblBranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                string sem = "select distinct Current_Semester from Registration where college_code='" + collegeCode + "' and Batch_Year in('" + valBatch + "') and degree_code in('" + valDegree + "')  and  CC=0 and DelFlag=0 order by Current_Semester asc";
                dtCommon = dir.selectDataTable(sem);
                if (dtCommon.Rows.Count > 0)
                {
                    cblSem.DataSource = dtCommon;
                    cblSem.DataTextField = "Current_Semester";
                    cblSem.DataValueField = "Current_Semester";
                    cblSem.DataBind();
                    checkBoxListselectOrDeselect(cblSem, true);
                    CallCheckboxListChange(chksem, cblSem, txtSem, lblSem1.Text, "--Select--");
                }
            }
        }
        catch
        {
        }


    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            BindRightsBaseBatch();
            binddegree1();
            bindbranch1();
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch1.Text, "--Select--");
            binddegree1();
            bindbranch1();
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch1.Text, "--Select--");
            binddegree1();
            bindbranch1();
            bindsem1();


        }
        catch (Exception ex)
        {
        }
    }
    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree1.Text, "--Select--");
            bindbranch1();
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree1.Text, "--Select--");
            bindbranch1();
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch1.Text, "--Select--");
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch1.Text, "--Select--");
            bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void chksem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chksem, cblSem, txtSem, lblSem1.Text, "--Select--");
            //bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void cblSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chksem, cblSem, txtSem, lblSem1.Text, "--Select--");
            //bindsem1();

        }
        catch (Exception ex)
        {
        }
    }
    protected void btnAttenSetting_Click(object sender, EventArgs e)
    {
        popaddnewF2.Visible = true;
        Bindcollege();
        BindRightsBaseBatch();
        binddegree1();
        bindbranch1();
        bindsem1();
        txtFromDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtToDate1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtNoRows.Text = "1";
        txtMaxAttndValue.Text = "1";
        btnsaveSettings.Visible = false;
    }
    protected void btn_popupclose2_Click(object sender, EventArgs e)
    {
        popaddnewF2.Visible = false;
    }
    protected void btnSettingGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtMaxAttndValue.Text))
            {
                lblSave.Text = "";
                lblSave.Visible = false;
                int sno = 0;
                int rows = 0;
                int.TryParse(txtNoRows.Text, out rows);
                GridView3.Visible = true;
                DataTable dtsetng = new DataTable();
                DataRow drsetng;
                dtsetng.Columns.Add("frmrange");
                dtsetng.Columns.Add("torng");
                dtsetng.Columns.Add("atnfval");

                for (int i = 0; i < rows; i++)
                {
                    drsetng = dtsetng.NewRow();
                    dtsetng.Rows.Add(drsetng);
                }
                GridView3.DataSource = dtsetng;
                GridView3.DataBind();
                btnsaveSettings.Visible = true;
                GridView4.Visible = false;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Enter Max Mark";
                txtMaxAttndValue.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnsaveSettings_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtMaxAttndValue.Text))
            {

                int Count = 0;
                string valBatch = string.Empty;
                string valDegree = string.Empty;
                string sem = string.Empty;
                if (cblBatch.Items.Count > 0)
                    valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
                if (cblBranch.Items.Count > 0)
                    valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
                if (cblSem.Items.Count > 0)
                    sem = rs.GetSelectedItemsValueAsString(cblSem);
                //string frange=string.Empty;
                //string trange=string.Empty;
                //string attndmark=string.Empty;
                string SelectQ = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester from Registration r,Degree d where r.degree_code=d.Degree_Code and r.degree_code in('" + valDegree + "') and r.Batch_Year in('" + valBatch + "') and r.Current_Semester in('" + sem + "')";
                DataTable dtBatchDeg = dir.selectDataTable(SelectQ);
                int ins = 0;
                if (dtBatchDeg.Rows.Count > 0)
                {
                    string date1 = string.Empty;
                    string date2 = string.Empty;
                    date1 = txtFromDate1.Text.ToString();
                    date2 = txtToDate1.Text.ToString();
                    DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
                    DateTime dt2 = new DateTime();
                    DateTime.TryParseExact(date1, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1);
                    DateTime.TryParseExact(date2, "dd/MM/yyyy", null, DateTimeStyles.None, out dt2);

                    foreach (DataRow dr in dtBatchDeg.Rows)
                    {

                        string batchYear = Convert.ToString(dr["Batch_Year"]);
                        string DegCode = Convert.ToString(dr["degree_code"]);
                        string sems = Convert.ToString(dr["Current_Semester"]);
                        string UptIns = "if exists(select * from AttendanceMarkEntry where fromDate between '" + dt1 + "' and '" + dt2 + "' and toDate between '" + dt1 + "' and '" + dt2 + "' and BathYear='" + batchYear + "' and degreeCode='" + DegCode + "'  and semester='" + sems + "' ) update AttendanceMarkEntry SET fromDate='" + dt1 + "', todate='" + dt2 + "',bathyear='" + batchYear + "',degreeCode='" + DegCode + "' where fromDate between '" + dt1 + "' and '" + dt2 + "' and toDate between '" + dt1 + "' and '" + dt2 + "' and bathyear='" + batchYear + "' and degreeCode='" + DegCode + "' and semester='" + sems + "' and maxAttndValue='" + txtMaxAttndValue.Text + "' else insert into AttendanceMarkEntry(fromDate,toDate,bathyear,degreeCode,semester,maxAttndValue)  values ('" + dt1 + "','" + dt2 + "','" + batchYear + "','" + DegCode + "','" + sems + "','" + txtMaxAttndValue.Text + "')";

                        int val = da.update_method_wo_parameter(UptIns, "text");

                        string getId = da.GetFunction("select AttndId from AttendanceMarkEntry  where fromDate between '" + dt1 + "' and '" + dt2 + "' and toDate between '" + dt1 + "' and '" + dt2 + "' and BathYear='" + batchYear + "' and degreeCode='" + DegCode + "'  and semester='" + sems + "'");

                        int del = 0;

                        if (!string.IsNullOrEmpty(getId))
                        {
                            del = da.update_method_wo_parameter("delete from AttendanceMarkValue where AttndId='" + getId + "'", "text");
                            foreach (GridViewRow i in GridView3.Rows)
                            {
                                TextBox txtfrng = (TextBox)i.FindControl("txtfrmrange");
                                string frange = txtfrng.Text;
                                TextBox txttorng = (TextBox)i.FindControl("txttorange");
                                string trange = txttorng.Text;
                                TextBox atndmrk = (TextBox)i.FindControl("txtatndrpt");
                                string attndmark = atndmrk.Text;
                                ins = da.update_method_wo_parameter("insert into AttendanceMarkValue(frange,torange,attndvalue,AttndId) values('" + frange + "','" + trange + "','" + attndmark + "','" + getId + "')", "text");
                            }
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Student Not Found";
                }
                if (ins != 0)
                {
                    lblSave.Visible = true;
                    lblSave.Text = "Saved Sucessfully";
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Enter Max Mark";
                txtMaxAttndValue.Focus();
            }

        }
        catch
        {
        }
    }
    protected void btnSettingView1_Click(object sender, EventArgs e)
    {
        try
        {

            btnsaveSettings.Visible = false;
            int sno = 0;
            //int rows=0;
            lblSave.Visible = false;
            string valBatch = string.Empty;
            string valDegree = string.Empty;
            string sem = string.Empty;
            if (cblBatch.Items.Count > 0)
                valBatch = rs.GetSelectedItemsValueAsString(cblBatch);
            if (cblBranch.Items.Count > 0)
                valDegree = rs.GetSelectedItemsValueAsString(cblBranch);
            if (cblSem.Items.Count > 0)
                sem = rs.GetSelectedItemsValueAsString(cblSem);


            GridView3.Visible = false;
            dtstnview.Clear();
            dtstnview.Columns.Add("Sno4");
            dtstnview.Columns.Add("fromdate");
            dtstnview.Columns.Add("todate");
            dtstnview.Columns.Add("Frange");
            dtstnview.Columns.Add("Trange");
            dtstnview.Columns.Add("atndmark");
            Boolean reportfalg = false;



            string query = "select distinct r.Batch_Year,de.dept_acronym,convert(nvarchar(15),am.fromDate,101) as fromDate,convert(nvarchar(15),am.toDate,101) as toDate,ae.frange,ae.torange,ae.AttndValue,am.semester,r.Current_Semester from Registration r,Degree d,Department de,AttendanceMarkEntry am,AttendanceMarkValue ae where am.semester=r.Current_Semester and  r.Batch_Year=am.BathYear and d.Degree_Code=am.DegreeCode and r.degree_code=d.Degree_Code and de.Dept_Code=d.Dept_Code  and am.AttndId=ae.AttndId and r.Batch_Year in('" + valBatch + "') and r.degree_code in ('" + valDegree + "') and r.Current_Semester in ('" + sem + "')";

            ds1 = da.select_method_wo_parameter(query, "text");
            int sno4 = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                dicval.Clear();
                int ky = 0;
                int rwcount = 0;
                DataTable data = new DataTable();
                data = ds1.Tables[0].DefaultView.ToTable(true, "Batch_Year", "dept_acronym", "Current_Semester");
                if (data.Rows.Count > 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        drstngview = dtstnview.NewRow();
                        ky++;

                        string batch = Convert.ToString(data.Rows[i]["Batch_Year"]);
                        string depAcronym = Convert.ToString(data.Rows[i]["dept_acronym"]);
                        string curr_sem = Convert.ToString(data.Rows[i]["Current_Semester"]);
                        string arrangerow = batch + '/' + depAcronym + '/' + curr_sem;
                        dicval.Add(rwcount, arrangerow);
                        rwcount++;
                        drstngview["fromdate"] = arrangerow;
                        dtstnview.Rows.Add(drstngview);
                        ds1.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batch + "' and dept_acronym='" + depAcronym + "' and Current_Semester='" + curr_sem + "'";
                        DataTable dsvalue = ds1.Tables[0].DefaultView.ToTable();

                        foreach (DataRow dr in dsvalue.Rows)
                        {
                            rwcount++;
                            drstngview = dtstnview.NewRow();
                            string fdate = Convert.ToString(dr["fromDate"]).Trim();
                            string tdate = Convert.ToString(dr["toDate"]).Trim();
                            string f_range = Convert.ToString(dr["frange"]).Trim();
                            string t_range = Convert.ToString(dr["torange"]).Trim();
                            string att_mark = Convert.ToString(dr["AttndValue"]).Trim();

                            reportfalg = true;
                            sno4++;
                            drstngview["Sno4"] = sno4.ToString();
                            drstngview["fromdate"] = Convert.ToString(dr["fromDate"]).Trim();
                            drstngview["todate"] = Convert.ToString(dr["toDate"]).Trim();
                            drstngview["Frange"] = Convert.ToString(dr["frange"]).Trim();
                            drstngview["Trange"] = Convert.ToString(dr["torange"]).Trim();
                            drstngview["atndmark"] = Convert.ToString(dr["AttndValue"]).Trim();
                            dtstnview.Rows.Add(drstngview);
                        }
                    }
                    GridView4.DataSource = dtstnview;
                    GridView4.DataBind();
                    int colCount = GridView4.Columns.Count;
                    foreach (KeyValuePair<int, string> dr in dicval)
                    {
                        int g = dr.Key;
                        string DicValue = dr.Value;
                        GridView4.Rows[g].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        GridView4.Rows[g].Cells[1].ColumnSpan = 6;
                        GridView4.Rows[g].Cells[0].Visible = false;
                        for (int a = 2; a < 6; a++)
                            GridView4.Rows[g].Cells[a].Visible = false;
                    }


                }
                if (reportfalg == true)
                {
                    GridView4.Visible = true;
                    Printcontrol.Visible = false;


                }


            }


            else
            {
                GridView4.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            colvalchk++;

            if (dicbtnval.Count > 0)
            {
                foreach (KeyValuePair<string, string> dicval1 in dicbtnval)
                {
                    string rowct2 = dicval1.Value;
                    string vsblval = dicval1.Key;
                    if (colvalchk == Convert.ToInt32(vsblval))
                    {
                        if (rowct2 == "1")
                        {
                            e.Row.Cells[4].Enabled = false;
                            e.Row.Cells[5].Enabled = false;
                            e.Row.Cells[6].Enabled = false;
                            e.Row.Cells[7].Enabled = false;
                            e.Row.Cells[2].BackColor = Color.White;
                        }
                        else if (rowct2 == "2")
                        {
                            e.Row.Cells[6].Enabled = false;
                            e.Row.Cells[2].BackColor = Color.Aquamarine;
                        }
                        else if (rowct2 == "3")
                        {
                            e.Row.Cells[2].BackColor = Color.AntiqueWhite;
                        }

                    }

                }
            }

        }
    }
  
    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        int colct = gridview2colcount;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // colvalchk++;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            for (int i = 4; i < colct; i++)
            {
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                if (i == (colct - 1))
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
            }



        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int grCol = 0; grCol < dtview.Columns.Count; grCol++)
            {
                e.Row.Cells[grCol].Visible = false;
            }
            e.Row.HorizontalAlign = HorizontalAlign.Center;
            //  e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[1].Visible = false;

        }

    }

    //#region grid
    //protected void addnewrow(object sender, EventArgs e)
    //{
    //    AddNewRowToGrid();
    //}
    //private void AddNewRowToGrid()
    //{
    //    try
    //    {
    //        int rowIndex = 0;
    //        if (ViewState["CurrentTable"] != null)
    //        {
    //            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
    //            DataRow drCurrentRow = null;
    //            TextBox box1 = new TextBox();
    //            TextBox box2 = new TextBox();
    //            TextBox box3 = new TextBox();


    //            if (dtCurrentTable.Rows.Count > 0)
    //            {
    //                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
    //                {

    //                    box1 = (TextBox)GridView1.Rows[i].Cells[3].FindControl("TextBox1");
    //                    box2 = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBox2");
    //                    box3 = (TextBox)GridView1.Rows[i].Cells[5].FindControl("TextBox3");
    //                    drCurrentRow = dtCurrentTable.NewRow();

    //                    //dtCurrentTable.Rows[i]["Sno"] = Convert.ToString((i + 1)).Trim();
    //                    //dtCurrentTable.Rows[i]["hrdet_no"] = Label1.Text;
    //                    dtCurrentTable.Rows[i]["frange"] = box1.Text;
    //                    dtCurrentTable.Rows[i]["trange"] = box2.Text;
    //                    dtCurrentTable.Rows[i]["AttndValue"] = box3.Text;
    //                    rowIndex++;
    //                }

    //                dtCurrentTable.Rows.Add(drCurrentRow);
    //                ViewState["CurrentTable"] = dtCurrentTable;
    //                GridView1.DataSource = dtCurrentTable;
    //                GridView1.DataBind();
    //            }

    //        }
    //        else
    //        {
    //            GridView1.DataSource = bindSettingGrid();
    //            GridView1.DataBind();
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Unable to add row ')", true);
    //    }
    //}
    //protected DataTable bindSettingGrid()
    //{
    //    DataTable dtSetting = new DataTable();
    //    //dtSetting.Columns.Add("Sno");
    //    dtSetting.Columns.Add("frange");
    //    dtSetting.Columns.Add("trange");
    //    dtSetting.Columns.Add("AttndValue");
    //    try
    //    {
    //        ArrayList addnew = new ArrayList();
    //        addnew.Add("1");
    //        DataRow dr;
    //        for (int row = 0; row < addnew.Count; row++)
    //        {
    //            dr = dtSetting.NewRow();
    //            //dr["start_time"] = "HH:MM";
    //            //dr["end_time"] = "HH:MM";
    //            dtSetting.Rows.Add(dr);
    //        }
    //    }
    //    catch { }
    //    return dtSetting;
    //}
    //protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
    //protected void GridView1_RowCommand(object sender, EventArgs e)
    //{
    //}
    //protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //}
    //#endregion

    #region Added By Malang Raja T on Dec 2 2016

    private bool CheckCAMCalculationGradeSettings()
    {
        try
        {
            bool isResult = false;
            string grouporusercode1 = string.Empty;
            if ((Session["group_code"] != null && Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string checkValue = da.GetFunctionv("select value from Master_Settings where settings='ShowGradeInCamCalulationDetails' " + grouporusercode1 + "");
            if (string.IsNullOrEmpty(checkValue.Trim()) || checkValue.Trim() == "0")
            {
                isResult = false;
            }
            else if (checkValue.Trim() == "1")
            {
                isResult = true;
            }
            else
            {
                isResult = false;
            }
            return isResult;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool findgrade(DataTable dt, string obtainedmarks, ref string Grade)
    {
        bool result = false;
        if (dt.Rows.Count > 0)
        {
            double marks = 0;
            double.TryParse(obtainedmarks, out marks);
            marks = Math.Round(marks, 0);
            //"Between Frange and Trange";
            dt.DefaultView.RowFilter = "Frange<='" + marks + "' and Trange>='" + marks + "'";
            DataView dv = new DataView();
            dv = dt.DefaultView;
            if (dv.Count > 0)
            {
                if (dv[0]["Mark_Grade"].ToString() != "" && dv[0]["Mark_Grade"].ToString() != null)
                {
                    Grade = dv[0]["Mark_Grade"].ToString();
                    result = true;
                }
                else
                {
                    Grade = obtainedmarks;
                    Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));
                    return false;
                }
            }
            else
            {
                Grade = obtainedmarks;
                Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));
                result = false;
                return false;
            }
        }
        else
        {
            Grade = obtainedmarks;
            Grade = (Convert.ToString(Math.Round(Convert.ToDouble(Grade), 2, MidpointRounding.AwayFromZero)));
            result = false;
        }
        return result;
    }

    #endregion Added By Malang Raja T on Dec 2 2016

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    public void cb_round_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_round.Checked == true)
            {
                for (int j = 0; j < chkroundoff.Items.Count; j++)
                {
                    chkroundoff.Items[j].Selected = true;
                    errmsg.Text = "";
                }
            }

            else
            {
                for (int j = 0; j < chkroundoff.Items.Count; j++)
                {
                    chkroundoff.Items[j].Selected = false;
                    errmsg.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void chkRound100_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chkRound100.Checked == true)
            {
                Txtround.Visible = true;
                Panel2.Visible = true;
            }

            else
            {
                Txtround.Visible = false;
                Panel2.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
        bindSubject();
    }
    public void CheckBox1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(chkSubtype, cblSubtype, txtSubtype, lblSuType.Text, "--Select--");
        bindSubject();
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int i = 0;
            cbSubjet.Checked = false;
            int commcount = 0;
            txtSubject.Text = "--Select--";
            for (i = 0; i < cblSubject.Items.Count; i++)
            {
                if (cblSubject.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSubject.Items.Count)
                {
                    cbSubjet.Checked = true;
                }
                txtSubject.Text = "Subjects(" + commcount.ToString() + ")";
            }
        }
        catch { }
    }
    public void cbSubjet_checkedchange(object sender, EventArgs e)
    {
        try
        {

            txtSubject.Text = "--Select--";
            if (cbSubjet.Checked == true)
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = true;
                }
                txtSubject.Text = "Subjects(" + (cblSubject.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = false;
                }
            }



        }
        catch { }
    }
    public void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {

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
}
