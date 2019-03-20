using System;//================modified on 27/6/12 (Kongu modification),28/6/12(change prgm->dept,course->subj),02/07/12(kongu issue, col cnt)
//------------------------------3/7/12(add one more col, header caption "Arrear"),6/7/12(visible sec, change query)]
//----------------------------------20/7/12(multi iso, logo rights)
//========added printmaster setting condition based on mastersetting in pageload on 21.07.12 by mythili
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
public partial class University_mark : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //''----------strudent photo
            System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img1.Width = Unit.Percentage(75);
            img1.Height = Unit.Percentage(70);
            return img1;

            //''------------clg logo
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(105);
            img.Height = Unit.Percentage(70);
            return img;

            //'-------------coe sign
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(75);
            img2.Height = Unit.Percentage(70);
            return img2;

        }
    }
    SqlCommand cmd;
    SqlDataReader dr_exam;
    SqlDataReader dr_mnthyr;
    SqlDataReader dr_convert;
    string grade_setting = "";
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Photo = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Load = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Inssetting = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Examcode = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_loadSubject = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Stud = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_mrkentry = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_currsem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_getdetail = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_daters = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_course = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_exam = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_secrs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_new = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_grademas = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_credit = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_option = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_convertgrade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_rs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade_flag = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_fun = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable has = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds_has = new DataSet();
    Hashtable has_ra = new Hashtable();

    string collnamenew1 = "", address1 = "", address2 = "", address3 = "", pincode = "", categery = "", Affliated = "";
    //'---------------------------new
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int serialno = 0;
    int exam_code_new = 0;
    int examcode_fun = 0;
    //'------------------------------
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string strsec = "";
    int semdec = 0;
    string sections = "";
    string funcgrade = "";
    string mark = "";
    Boolean markflag = false;
    string rol_no = "";
    string courseid = "";
    string atten = "";
    string Master = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string fromdate = "";
    Boolean InsFlag;
    Boolean flag;
    int IntExamCode = 0;
    int column_count = 0;
    string degree_code = "";
    string current_sem = "";
    string batch_year = "";
    string getgradeflag = "";
    string exam_month = "";
    string exam_year = "";
    string getsubno = "";
    string getsubtype = "";
    int rcnt;
    int ExamCode = 0;
    string strmnthyear = "";
    string strexam = "";
    int overallcredit = 0;
    string grade = "";
    string funcsubno = "";
    string funcsubname = "";
    string funcsubcode = "";
    string funcresult = "";
    string funcsemester = "";
    string funccredit = "";
    string EarnedVal = "";
    double cgpa2 = 0;
    int cou = 0;
    Hashtable hat = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    int sl_no1 = 1;
    int allpass_tot_cnt = 0;
    string reg_strsec = "", r_strsec = "";
    string[] string_session_values;


    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

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
                btnxl.Visible = false;
                FpExternal.Visible = false;
                ddlpage.Visible = false;
                lblpages.Visible = false;
                //if (Convert.ToString(Session["value"]) == "1")
                //{
                //    LinkButtonb1.Visible = false;
                //    LinkButtonb2.Visible = true;
                //}
                //else
                //{
                //    LinkButtonb1.Visible = true;
                //    LinkButtonb2.Visible = false;
                //}


                //'--------------------------------------
                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                //=======================on 11/4/12
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));



                int year;
                year = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 20; l++)
                {

                    ddlYear.Items.Add(Convert.ToString(year - l));

                }

                if (Request.QueryString["val"] == null)
                {
                    bindbatch();//-----------------call bind functions
                    binddegree();
                    bindbranch();
                    bindsem();
                    bindsec();

                }
                else
                {
                    //=======================page redirect from master print setting
                    //    try
                    {
                        string_session_values = Request.QueryString["val"].Split(',');
                        if (string_session_values.GetUpperBound(0) == 6)
                        {
                            bindbatch();
                            ddlBatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                            binddegree();
                            ddlDegree.SelectedIndex = Convert.ToInt16(string_session_values[1]);
                            bindbranch();
                            if (ddlBranch.Enabled == true)
                            {
                                ddlBranch.SelectedIndex = Convert.ToInt16(string_session_values[2].ToString());
                            }
                            bindsem();
                            if (ddlSemYr.Enabled == true)
                            {
                                ddlSemYr.SelectedIndex = Convert.ToInt16(string_session_values[3].ToString());
                            }
                            bindsec();
                            if (ddlSec.Enabled == true)
                            {
                                ddlSec.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                            }
                            if (ddlMonth.Enabled == true)
                            {
                                ddlMonth.SelectedIndex = Convert.ToInt16(string_session_values[5].ToString());
                            }
                            if (ddlYear.Enabled == true)
                            {
                                ddlYear.SelectedIndex = Convert.ToInt16(string_session_values[6].ToString());
                            }

                            btnGo_Click(sender, e);

                            //if (final_print_col_cnt > 0)
                            //{
                            //    setheader_print();
                            //    view_header_setting();
                            //    FpExternal.Width = final_print_col_cnt * 100;
                            //}
                        }
                    }
                    //   catch
                    {
                    }
                    //===================================

                }

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].AllowTableCorner = true;
                FpExternal.Sheets[0].SheetCorner.Cells[0, 0].Text = " S.No ";
                FpExternal.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                //FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Border.BorderColor = Color.Black;


                //'--------------------------------------------------- to bind the sem
                string getbranch = ddlBranch.Text.ToString();




                if (Session["usercode"] != "")
                {
                    Master = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                    setcon.Close();
                    setcon.Open();
                    SqlDataReader mtrdr;

                    SqlCommand mtcmd = new SqlCommand(Master, setcon);
                    mtrdr = mtcmd.ExecuteReader();

                    Session["strvar"] = "";
                    Session["Rollflag"] = "0";
                    Session["Regflag"] = "0";
                    Session["Studflag"] = "0";
                    if (mtrdr.HasRows)
                    {
                        while (mtrdr.Read())
                        {
                            if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Regflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Studflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar'";
                            }
                            if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                            {
                                if (strdayflag != "" && strdayflag != "\0")
                                {
                                    strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                                }
                                else
                                {
                                    strdayflag = " and (registration.Stud_Type='Hostler'";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Regular")
                            {
                                regularflag = "and ((registration.mode=1)";

                                // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                            }
                            if (mtrdr["settings"].ToString() == "Lateral")
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
                            if (mtrdr["settings"].ToString() == "Transfer")
                            {
                                if (regularflag != "")
                                {
                                    regularflag = regularflag + " or (registration.mode=2)";
                                }
                                else
                                {
                                    regularflag = regularflag + " and ((registration.mode=2)";
                                }
                                //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                            }

                            if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                            {
                                genderflag = " and (sex='0'";
                            }
                            if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                            {
                                if (genderflag != "" && genderflag != "\0")
                                {
                                    genderflag = genderflag + " or sex='1'";
                                }
                                else
                                {
                                    genderflag = " and (sex='1'";
                                }
                            }
                            //=========== hide the printmaster setting button based on print master setting mythili on 21.07.12
                            if (mtrdr["settings"].ToString() == "print_master_setting" && mtrdr["value"].ToString() == "1")
                            {
                                btnPrint.Visible = false;// true;
                            }
                            else
                            {
                                btnPrint.Visible = false;
                            }
                            //============================================
                        }
                    }
                    if (strdayflag != "")
                    {
                        strdayflag = strdayflag + ")";
                    }
                    Session["strvar"] = strdayflag;
                    if (regularflag != "")
                    {
                        regularflag = regularflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag;
                    if (genderflag != "")
                    {
                        genderflag = genderflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag + genderflag;
                }

                //  ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

                //  ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

            }

        }
        catch(Exception ex)
        {
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
            con.Close();
        }
    }
    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
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
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }
    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load = daccess.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds_load;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }

    public void BindBatch()
    {
        ddlBatch.Items.Clear();
        string sqlstr = "";
        int max_bat = 0;


        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstr));
            ddlBatch.SelectedValue = max_bat.ToString();

            // ddlBatch.Items.Insert(0, new ListItem("- -Select- -", "-1"));

        }
    }
    public void BindDegree()
    {


        ddlDegree.Items.Clear();
        collegecode = Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            //ddlDegree.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }
    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con_Load.Close();
        con_Load.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con_Load);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //  ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //  RequiredFieldValidator5.Visible = false;
            }
            else
            {
                ddlSec.Enabled = true;
                //   RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;
            //   RequiredFieldValidator5.Visible = false;
        }

    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

            for (int i = 1; i <= duration; i++)
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
            //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con_Getfunc.Close();
        con_Getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_Getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_Getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "0";
        }
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();
        }

        ddlSec.SelectedIndex = -1;
    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();
        string course_id = ddlDegree.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();


        }

    }
    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();


        if (!Page.IsPostBack == false)
        {

        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                //Get_Semester();
                bindsem();
                //    BindSectionDetail();
                bindsec();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    public void bindsem()
    {


        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            //     ddlSemYr.Items.Clear();
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
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }

        bindsec();
    }
    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        sections = ddlSec.SelectedValue.ToString();

        int sem_new = 0;
        string sem_fun = "";
        string exam_month_fun = "", exam_year_fun = "";
        string subjects_fun = "";

        if (ddlSec.Enabled == true)
        {
            if (ddlSec.SelectedItem.ToString() != "" && ddlSec.SelectedItem.ToString() != null && ddlSec.Enabled == true)
            {
                reg_strsec = " and registration.sections='" + ddlSec.SelectedValue.ToString() + "'";
                r_strsec = " and r.sections='" + ddlSec.SelectedValue.ToString() + "'";
            }
            else
            {
                reg_strsec = "";
                r_strsec = "";
            }
        }
        else
        {
            reg_strsec = "";
            r_strsec = "";
        }

        con_sem2.Close();
        con_sem2.Open();
        string q = "select distinct current_semester from registration  where batch_year=" + ddlBatch.SelectedItem + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and delflag=0";
        SqlCommand com_sem2 = new SqlCommand(q, con_sem2);
        SqlDataReader sdr_sem2 = com_sem2.ExecuteReader();
        sdr_sem2.Read();
        if (sdr_sem2.HasRows == true)
        {
            Session["sem2"] = sdr_sem2["current_semester"];
        }



        FpExternal.Visible = true;
        string strStudents = "";
        degree_code = ddlBranch.SelectedValue.ToString();
        current_sem = ddlSemYr.SelectedValue.ToString();
        FpExternal.Sheets[0].ColumnCount = 0;
        FpExternal.Sheets[0].RowCount = 0;
        batch_year = ddlBatch.SelectedValue.ToString();
        exam_month_fun = ddlMonth.SelectedValue.ToString();
        exam_year_fun = ddlYear.SelectedValue.ToString();
        sem_new = Convert.ToInt32(ddlSemYr.SelectedValue.ToString());

        sem_fun = GetSemester_AsNumber(Convert.ToInt32(sem_new)).ToString();
        examcode_fun = int.Parse(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month_fun + " and exam_year=" + exam_year_fun + ""));
        Session["examcode"] = examcode_fun;
        has.Clear();

        has.Add("sem_fun", sem_fun);
        has.Add("degree_code", degree_code);
        has.Add("batch_year", batch_year);
        has.Add("examcode_fun", examcode_fun);
        ds_has = d2.select_method("get_subject", has, "sp");

        if (0 < ds_has.Tables[0].Rows.Count)
        {
            FpExternal.Sheets[0].SheetName = " ";
            FpExternal.Sheets[0].ColumnCount = 13 + ds_has.Tables[0].Rows.Count;
            //FpExternal.Sheets[0].ColumnHeader.RowCount = 5;//on 17/7/12
            FpExternal.Sheets[0].ColumnHeader.RowCount = 6;
            FpExternal.Sheets[0].Columns[0].Width = 50;
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            FpExternal.Sheets[0].RowHeader.Visible = false;


            FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpExternal.Sheets[0].DefaultStyle.Font.Bold = false;
            FpExternal.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].RowHeader.Visible = false;

            FpExternal.Sheets[0].RowCount = 4;
            FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 4].Height = 40;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, 0, 1, FpExternal.Sheets[0].ColumnCount);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Text = "END SEMESTER EXAMINATION";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Border.BorderColorTop = Color.White;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Size = 14;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 2);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 2, 1, 7);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "DEGREE : ";// +GetFunction("select dept_acronym from department where dept_name='" + ddlBranch.SelectedItem.ToString() + "'");//ddlBranch.SelectedItem.Text;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Text = ddlDegree.SelectedItem.Text + " " + ddlBranch.SelectedItem.ToString();//ddlBranch.SelectedItem.Text;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].VerticalAlign = VerticalAlign.Middle;

            //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 5, 1, 1);
            //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 6, 1, 3);
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Text = "DEGREE : ";// +ddlDegree.SelectedItem.Text;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Font.Bold = true;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].HorizontalAlign = HorizontalAlign.Left;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].VerticalAlign = VerticalAlign.Middle;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Text = ddlDegree.SelectedItem.Text;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].HorizontalAlign = HorizontalAlign.Center;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].VerticalAlign = VerticalAlign.Middle;
            //   int gg = int.Parse(ddlBatch.SelectedValue.ToString()) + 1;

            int tot_sem = 0;//int.Parse(ddlBatch.SelectedValue.ToString()) + 1;//-23/6/12 PRABHA
            int yr = 0;
            cmd = new SqlCommand("select ndurations from ndegree where batch_year=" + ddlBatch.SelectedValue + "  and degree_code=" + ddlBranch.SelectedValue + "", con);

            SqlDataReader no_on_sem_dr;
            con.Close();
            con.Open();
            no_on_sem_dr = cmd.ExecuteReader();
            if (no_on_sem_dr.HasRows)
            {
                while (no_on_sem_dr.Read())
                {
                    tot_sem = Convert.ToInt32(no_on_sem_dr[0].ToString());
                    yr = Convert.ToInt32(ddlBatch.SelectedValue.ToString()) + (tot_sem / 2);
                }
            }
            else
            {
                cmd = new SqlCommand("select ndurations from degree where degree_code=" + ddlBranch.SelectedValue + "", con);
                con.Close();
                con.Open();
                no_on_sem_dr = cmd.ExecuteReader();
                if (no_on_sem_dr.HasRows)
                {
                    while (no_on_sem_dr.Read())
                    {
                        tot_sem = Convert.ToInt32(no_on_sem_dr[0].ToString());
                        yr = Convert.ToInt32(ddlBatch.SelectedValue.ToString()) + (tot_sem / 2);
                    }
                }
            }
            //-----------------------------------------------------------
            //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 7, 1, 4);
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 7].Text = "ACADAMIC YEAR : " + ddlBatch.SelectedValue.ToString() + " - " +yr;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 7].Font.Bold = true;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 7].HorizontalAlign = HorizontalAlign.Left;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 7].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 9, 1, 3);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 12, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 9].Text = "ACADAMIC YEAR : ";// +ddlBatch.SelectedValue.ToString() + " - " + yr;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].Font.Bold = true;
            DateTime date_today = DateTime.Now;
            int yr_now = Convert.ToInt32(date_today.ToString("yyyy"));
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].Text = (yr_now.ToString() + "-" + (yr_now + 1).ToString());
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 9].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 9].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].VerticalAlign = VerticalAlign.Middle;

            //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 11, 1, FpExternal.Sheets[0].ColumnCount);
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 11].Text = "BATCH PASSING OUT YEAR : "+yr .ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 11].Font.Bold = true;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 11].HorizontalAlign = HorizontalAlign.Left;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 11].VerticalAlign = VerticalAlign.Middle;

            // FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 14, 1, 7);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 14, 1, (FpExternal.Sheets[0].ColumnCount - 14));
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 14].Text = "BATCH PASSING OUT YEAR" + yr.ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 14].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 14].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 14].VerticalAlign = VerticalAlign.Middle;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = yr.ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 21].Text = yr.ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 21].HorizontalAlign = HorizontalAlign.Right;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3,21].VerticalAlign = VerticalAlign.Middle;
            //  FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, 5);
            //   FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "SEMESTER NO. " + ddlSemYr.SelectedItem.Text;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "SEMESTER NO. ";/// +ddlSemYr.SelectedItem.Text;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 2, 1, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 2].Text = findroman(ddlSemYr.SelectedItem.Text);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 2].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 2].VerticalAlign = VerticalAlign.Middle;

            if (sections != "")
            {
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 5, 1, 4);
                //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 6, 1, 3);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Text = "SECTION: " + ddlSec.SelectedItem.ToString();

                // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 6].Text = ddlSec.SelectedItem.ToString();
                //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Font.Size = FontUnit.Medium;

                // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 6].Font.Bold = true;
            }
            else
            {
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 5, 1, 3);
            }

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 6].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 6].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].VerticalAlign = VerticalAlign.Middle;


            //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 11, 1, FpExternal.Sheets[0].ColumnCount);
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 11].Text = "MONTH & YEAR YEAR OF EXAM : " + ddlMonth.SelectedItem.Text + " & " + ddlYear.SelectedItem.Text;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 11].Font.Bold = true;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 11].HorizontalAlign = HorizontalAlign.Left;
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 11].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 9, 1, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 9].Text = "MONTH & YEAR YEAR OF EXAM : ";// +ddlMonth.SelectedItem.Text + " & " + ddlYear.SelectedItem.Text;
            // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 9].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 9].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 9].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 12, 1, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 12].Text = ddlMonth.SelectedItem.Text;// +" & " + ddlYear.SelectedItem.Text;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 12].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 12].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 12].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 12].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 13, 1, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 13].Text = ddlYear.SelectedItem.Text;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 13].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 13].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 13].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 13].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 14, 1, 8);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 14].Text = "AFTER REVALUATION";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 14].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 14].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 14].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 14].VerticalAlign = VerticalAlign.Bottom;
            // }
            //else
            //{
            //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 5, 1, FpExternal.Sheets[0].ColumnCount);
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Text = "MONTH & YEAR YEAR OF EXAM : " + ddlMonth.SelectedItem.Text + " & " + ddlYear.SelectedItem.Text;
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Font.Bold = true;
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Left;
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].VerticalAlign = VerticalAlign.Middle;
            //}



            FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Height = 40;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "S.NO";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = "SUBJECT CODE";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 5);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "SUBJECT NAME";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
            FpExternal.Sheets[0].Columns[3].Width = 300;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;


            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Text = "CREDITS";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 9, 1, FpExternal.Sheets[0].ColumnCount - 12);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = "NAME(S) OF THE SUBJECT TEACHER";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;

            int sl_no = 1;
            if (0 < ds_has.Tables[0].Rows.Count)
            {
                for (int i = 0; i < ds_has.Tables[0].Rows.Count; i++)
                {
                    FpExternal.Sheets[0].RowCount++;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 2);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = ds_has.Tables[0].Rows[i]["subject_code"].ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 5);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = ds_has.Tables[0].Rows[i]["subject_name"].ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].Text = ds_has.Tables[0].Rows[i]["credit_points"].ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    string io = ds_has.Tables[0].Rows[i]["subject_no"].ToString();
                    string staff_code = GetFunction("select distinct staff_code from exam_type where subject_no=" + io + " and batch_year=" + batch_year + "");
                    string staff_name = "";
                    staff_name = GetFunction("select staff_name from staffmaster where staff_code = '" + staff_code + "'");
                    if (staff_name == "0")
                    {
                        staff_name = "";
                    }
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 9, 1, FpExternal.Sheets[0].ColumnCount - 12);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].Text = staff_name;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    sl_no++;
                    if (!has_ra.ContainsKey(i + 1))
                    {
                        has_ra.Add(i + 1, 0);
                    }



                }
            }



            FpExternal.Sheets[0].RowCount += 4;
            Session["rowcount"] = FpExternal.Sheets[0].RowCount;
            FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 4].Height = 40;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, 0, 1, FpExternal.Sheets[0].ColumnCount);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Text = "CONSOLIDATED GRADE SHEET";

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "S.No";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;

            //     FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 1, 3, 1);
            //     FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Text = "Reg.No";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Font.Size = FontUnit.Medium;

            //       FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 2, 3, 1);
            //      FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Text = "Roll.No";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;

            //----------------------------------------------------------
            if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Text = "Reg.No";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Text = "Roll.No";


                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 1, 3, 1);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 2, 3, 1);
            }
            else if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Text = "Roll.No";
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 1, 3, 2);
                FpExternal.Sheets[0].Columns[1].Width = 50;
            }
            else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "1")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Text = "Reg.No";
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 1, 3, 2);
                FpExternal.Sheets[0].Columns[1].Width = 50;
            }
            else
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 1].Text = "Reg.No";
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 1, 3, 2);
            }
            //-------------------------------------------------------------------
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 3, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Text = "Name of The Student";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 4, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = "Stud.Type";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;


            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 5, 1, ds_has.Tables[0].Rows.Count);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Text = "Grade Obtained";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Font.Size = FontUnit.Medium;


            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 5, 1, ds_has.Tables[0].Rows.Count);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Text = "Subject";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 5].Font.Size = FontUnit.Medium;



            int temp = 5;
            for (int j = 0; j < ds_has.Tables[0].Rows.Count; j++)
            {
                FpExternal.Sheets[0].Columns[temp].Width = 20;
                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].Text = ds_has.Tables[0].Rows[j]["subject_code"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].Text = (j + 1).ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].Note = ds_has.Tables[0].Rows[j]["subject_code"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].VerticalAlign = VerticalAlign.Middle;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, temp].Font.Bold = true;
                temp++;
            }

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8].Text = "GPA";
            FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 8].Width = 30;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 8].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7].Text = "No of Subjects AB";//"No of Subjects Absent(AB)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 7].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6].Text = "No.of Subject to RA";// "No.of Subject to Reappear(RA)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 6].Font.Size = FontUnit.Medium;


            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5].Text = "No.of Subject W";// "No.of Subject Withdrawn(W)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 5].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Text = "No.of Subject WH";// "No.of Subject Withheld(WH)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3, 2, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3].Text = "Arrears";//--3/7/12 Prabha
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = "Previous";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = "Cleared Now";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;


            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1, 3, 1);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Text = "Total Subject to be passed in the next exam";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

            //  External_Students();
            main_function();
            course();
            logoset();
            view_header_setting();

        }



        if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) == 0)
        {
            lblnorec.Visible = true;
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            FpExternal.Visible = false;
            btnxl.Visible = false;
            pnlrecordcount.Visible = false;


        }
        else
        {

            Buttontotal.Visible = true;
            lblrecord.Visible = true;
            DropDownListpage.Visible = true;
            TextBoxother.Visible = false;
            lblpage.Visible = true;
            TextBoxpage.Visible = true;
            FpExternal.Visible = true;
            lblnorec.Visible = false;
            btnxl.Visible = true;
            pnlrecordcount.Visible = true;

            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;

                FpExternal.Height = 300;
            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpExternal.Height = 100;
            }
            else
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                CalculateTotalPages();
            }
            FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            FpExternal.Height = 1500;
        }
        //}
        //catch
        //{
        //}
    }

    public void view_header_setting()
    {
        int row_cnt = 0;
        DataSet dsprint = new DataSet();
        string view_footer = "", view_header = "", view_footer_text = "";
        has.Clear();
        has.Add("college_code", Session["collegecode"].ToString());
        has.Add("form_name", "university_mark.aspx");
        dsprint = daccess.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");

        if (dsprint.Tables[0].Rows.Count > 0)
        {

            ddlpage.Visible = true;
            lblpages.Visible = true;

            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            if (view_header == "0" || view_header == "1")
            {
                lblError.Visible = false;

                for (row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {

                    if (FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1" || FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == null)
                    {
                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                    }
                }

                for (row_cnt = 0; row_cnt < FpExternal.Sheets[0].RowCount; row_cnt++)
                {

                    if (FpExternal.Sheets[0].Cells[row_cnt, 0].Text == "CONSOLIDATED GRADE SHEET")
                    {
                        break;
                    }
                }
                row_cnt += 4;

                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = FpExternal.Sheets[0].RowCount;
                int pages = (totrowcount - row_cnt - 31) / 25;
                int intialrow = 1;
                int remainrows = (totrowcount - row_cnt - 31) % 25;
                if (FpExternal.Sheets[0].RowCount > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));

                    }
                    {
                        intialrow = FpExternal.Sheets[0].RowCount - 31;
                        i = i5 + 2;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    for (i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                    {
                        FpExternal.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpExternal.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        FpExternal.Height = 100;
                    }
                    else
                    {
                        FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                        FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }


                    pnlrecordcount.Visible = true;


                }
                else
                {
                    lblError.Visible = false;
                    pnlrecordcount.Visible = false;
                }
            }
            else if (view_header == "2")
            {

                for (row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    if (FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1" || FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == null)
                    {
                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                    }
                }

                lblError.Visible = false;
                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = FpExternal.Sheets[0].RowCount;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                if (FpExternal.Sheets[0].RowCount > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    for (i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                    {
                        FpExternal.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpExternal.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        FpExternal.Height = 100;
                    }
                    else
                    {
                        FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                        FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //  FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }
                    pnlrecordcount.Visible = true;
                }
                else
                {
                    pnlrecordcount.Visible = false;
                }
            }
            else
            {

            }
            lblpages.Visible = true;
            ddlpage.Visible = true;
        }
        else
        {
            lblpages.Visible = false;
            ddlpage.Visible = false;
        }
    }
    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch)
    {

        string GetUnivExamCode = "";
        string degree_code = "";
        string current_sem = "";
        string batch_year = "";
        //  degree_code = ddlBranch.SelectedValue.ToString();
        //   current_sem = ddlSemYr.SelectedValue.ToString();
        //    batch_year = ddlBatch.SelectedValue.ToString();

        string strExam_code = "";
        strExam_code = "Select Exam_Code from Exam_Details where Degree_Code = " + DegreeCode.ToString() + " and Current_Semester = " + Semester.ToString() + " and Batch_Year = " + Batch.ToString() + "";
        con_Examcode.Close();
        con_Examcode.Open();

        SqlDataReader dr_examcode;
        SqlCommand cmd_examcode = new SqlCommand(strExam_code, con_Examcode);
        dr_examcode = cmd_examcode.ExecuteReader();
        while (dr_examcode.Read())
        {
            if (dr_examcode.HasRows == true)
            {
                if (dr_examcode["Exam_Code"].ToString() != "")
                {
                    GetUnivExamCode = dr_examcode["Exam_Code"].ToString();
                }
            }
        }
        if (GetUnivExamCode != "")
        {
            return Convert.ToInt32(GetUnivExamCode);
        }
        else
        {
            return 0;
        }


    }
    //public int LoadSubject(int intExamCode)
    //{
    //    int i = 0;
    //    int IntSCount = 0;
    //    int Stno = 0;
    //    string Stype = "";
    //    string strsubject = "";
    //    string grade = "";
    //    string degree_code = "";
    //    string current_sem = "";
    //    string batch_year = "";

    //    degree_code = ddlBranch.SelectedValue.ToString();
    //    current_sem = ddlSemYr.SelectedValue.ToString();
    //    batch_year = ddlBatch.SelectedValue.ToString();

    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";

    //    //FpExternal.Sheets[0].ColumnHeader.Rows[9].BackColor = Color.AliceBlue;
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, 0].Text = "S.No";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, 2].Text = "RollNo";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, 3].Text = "RegNo";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, 4].Text = "Student Name";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, 5].Text = "Student Type";
    //    //FpExternal.Sheets[0].Columns[4].Width = 150;
    //    //FpExternal.Sheets[0].Columns[2].Width = 150;

    //    strsubject = "Select distinct Subject.Subject_No, isnull(Subject_Code,'') as Subject_Code,credit_points, (select distinct subject_type from subject s,sub_sem ss where s.subtype_no=ss.subtype_no and subject_no=subject.subject_no) subtype  from Mark_Entry,Subject,Syllabus_Master where Syllabus_Master.Syll_Code = Subject.Syll_Code and SubjecT_Code is not null and Syllabus_Master.Semester = " + semdec + " and Degree_Code = " + degree_code + " and Batch_Year = " + batch_year + " and Mark_Entry.Subject_No =  Subject.Subject_No and  Exam_Code = " + intExamCode + " and Type='' and attempts=1 Order by subtype desc,subject.subject_no ";


    //    con_loadSubject.Close();
    //    con_loadSubject.Open();
    //    SqlCommand cmd_loadSub = new SqlCommand(strsubject, con_loadSubject);
    //    SqlDataReader dr_loadSub;
    //    dr_loadSub = cmd_loadSub.ExecuteReader();

    //    while (dr_loadSub.Read())
    //    {
    //        if (dr_loadSub["Subject_Code"].ToString() != "")
    //        {
    //            grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
    //            cmd = new SqlCommand(grade, con_Grade);
    //            con_Grade.Close();
    //            con_Grade.Open();
    //            SqlDataReader dr_grade;
    //            dr_grade = cmd.ExecuteReader();


    //            while (dr_grade.Read())
    //            {
    //                if (dr_grade.HasRows == true)
    //                {

    //                    getgradeflag = Convert.ToString(dr_grade["grade_flag"]);
    //                    getsubno = Convert.ToString(dr_loadSub["Subject_No"]);

    //                    //'---------------------------- setting the chkbox cell type
    //                    FpExternal.Sheets[0].ColumnHeader.Cells[9, 1].Text = "Select";
    //                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    //                    FpExternal.Sheets[0].Columns[1].CellType = chkcell;
    //                    //'----------------------------------------------------



    //                    //FpExternal.Sheets[0].ColumnCount += 2;

    //                    //FpExternal.Sheets[0].ColumnHeader.Cells[5, FpExternal.Sheets[0].ColumnCount - 2].Note = getsubno;
    //                    ////'--------------new
    //                    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, FpExternal.Sheets[0].ColumnCount - 2, 1, 2);
    //                    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(6, FpExternal.Sheets[0].ColumnCount - 2, 1, 2);

    //                    ////'--------------------------
    //                    //FpExternal.Sheets[0].ColumnHeader.Cells[5, FpExternal.Sheets[0].ColumnCount - 2].Text = dr_loadSub["Subject_Code"].ToString();
    //                    //FpExternal.Sheets[0].ColumnHeader.Cells[6, FpExternal.Sheets[0].ColumnCount - 2].Text = dr_loadSub["credit_points"].ToString();
    //                    //FpExternal.Sheets[0].ColumnHeader.Cells[9, FpExternal.Sheets[0].ColumnCount - 2].Text = "Grade";
    //                    //FpExternal.Sheets[0].ColumnHeader.Cells[9, FpExternal.Sheets[0].ColumnCount - 1].Text = "Result";

    //                    i = i + 1;
    //                    if (Stype != dr_loadSub["Subtype"].ToString())
    //                    {
    //                        flag = false;

    //                        if (i > 1)
    //                        {
    //                            Stno = 4;

    //                        }
    //                        Stype = dr_loadSub["Subtype"].ToString();
    //                        i = 1;
    //                    }

    //                    IntSCount = IntSCount + 1;
    //                }
    //            }
    //        }

    //    }
    //    //Session["colcount"] = FpExternal.Sheets[0].ColumnCount;
    //    //FpExternal.Sheets[0].ColumnCount++;
    //    //rcnt = FpExternal.Sheets[0].ColumnCount - 1;
    //    ////FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, rcnt, 4, 1);
    //    ////FpExternal.Sheets[0].ColumnHeader.Cells[5, rcnt].Text = "GPA";
    //    ////FpExternal.Sheets[0].ColumnHeader.Cells[9, rcnt].Text = " ";

    //    //FpExternal.Sheets[0].ColumnCount++;
    //    //rcnt = FpExternal.Sheets[0].ColumnCount - 1;
    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, rcnt, 4, 1);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[5, rcnt].Text = "CGPA";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, rcnt].Text = " ";

    //    //FpExternal.Sheets[0].ColumnCount++;
    //    //rcnt = FpExternal.Sheets[0].ColumnCount - 1;
    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, rcnt, 4, 1);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[5, rcnt].Text = "Result";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[9, rcnt].Text = " ";
    //    if (flag == false)
    //    {
    //    }
    //    else
    //    {
    //    }


    //    column_count = FpExternal.Sheets[0].ColumnCount;


    //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //    {
    //        string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";



    //        SqlCommand collegecmd = new SqlCommand(str, con);
    //        SqlDataReader collegename;
    //        con.Close();
    //        con.Open();
    //        collegename = collegecmd.ExecuteReader();
    //        if (collegename.HasRows)
    //        {

    //            while (collegename.Read())
    //            {

    //                collnamenew1 = collegename["collname"].ToString();
    //                address1 = collegename["address1"].ToString();
    //                address2 = collegename["address2"].ToString();
    //                district = collegename["district"].ToString();
    //                address = address1 + "-" + address2 + "-" + district;

    //                categery = collegename["category"].ToString();
    //                Affliated = collegename["affliated"].ToString();
    //                Phoneno = collegename["phoneno"].ToString();
    //                Faxno = collegename["faxno"].ToString();
    //                phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
    //                email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();

    //            }
    //        }
    //        con.Close();
    //    }

    //    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //    style.Font.Size = 10;
    //    style.Font.Bold = true;
    //    FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //    FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //    FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;

    //    //'---------------------------------------new
    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, column_count - 2);
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorRight = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorLeft = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorLeft = Color.Black;
    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, column_count - 2);
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Text = categery + ", Affiliated to " + Affliated + ".";
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColor = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
    //    //'----------------------------------------------------new----------------------------
    //    FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Text = address;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

    //    FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Text = phnfax;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

    //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Text = email;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, column_count - 2);
    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, column_count - 2);
    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, column_count - 2);


    //    //'-----------------------------------------------------------------------------------
    //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);//'----------spaning for logo


    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].CellType = mi2;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].HorizontalAlign = HorizontalAlign.Center;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorLeft = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.White;


    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
    //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.Black;

    //    FpExternal.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[2].Border.BorderColor = Color.White;
    //    FpExternal.Sheets[0].ColumnHeader.Rows[3].Border.BorderColor = Color.White;
    //    //===================================

    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 0, 1, 6);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Text = "Course Code";
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorLeft = Color.Black;
    //    //FpExternal.Sheets[0].SheetCorner.Cells[6, 0].Border.BorderColorRight = Color.Black;
    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(6, 0, 1, 6);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Credits";
    //    //FpExternal.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
    //    //FpExternal.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.Black;

    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(7, 0, 1, 6);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[7, 0].Text = "MinMarks";
    //    //FpExternal.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
    //    //FpExternal.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.Black;

    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(8, 0, 1, 6);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[8, 0].Text = "MaxMarks";
    //    //FpExternal.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
    //    //FpExternal.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.Black;

    //    //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, column_count - 1, 5, 1);
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorBottom = Color.Black;
    //    //FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorLeft = Color.White;
    //    return IntSCount;
    //}
    //public void Load_Students(int ExamCode)
    //{
    //    string grade_get = "";
    //    string gpa = "";
    //    string strStudents = "";
    //    string grade = "";
    //    Boolean chkflag = false;
    //    //'--------new
    //    string mintotal = "";
    //    string maxtotal = "";
    //    string display_cgpa = "";
    //    int yp = 5;
    //    //----------------------------------------

    //    //-----------------------------------------------
    //    //if (Session["Rollflag"].ToString() == "0")
    //    //{
    //    //    FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
    //    //}
    //    //if (Session["Regflag"].ToString() == "0")
    //    //{
    //    //    FpExternal.Sheets[0].ColumnHeader.Columns[3].Visible = false;
    //    //}
    //    //if (Session["Studflag"].ToString() == "0")
    //    //{
    //    //    FpExternal.Sheets[0].ColumnHeader.Columns[5].Visible = false;
    //    //}



    //    SqlDataReader dr_grade_val;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "", con);
    //    dr_grade_val = cmd.ExecuteReader();
    //    while (dr_grade_val.Read())
    //    {
    //        if (dr_grade_val.HasRows == true)
    //        {
    //            grade_setting = dr_grade_val[0].ToString();
    //        }
    //    }


    //    strStudents = "Select isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.app_no=applyn.app_no and cc=0 and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' order by RlNo ";

    //    con_Stud.Close();
    //    con_Stud.Open();
    //    SqlCommand cmd_Subject = new SqlCommand(strStudents, con_Stud);
    //    SqlDataReader dr_Students;
    //    dr_Students = cmd_Subject.ExecuteReader();



    //    while (dr_Students.Read())
    //    {
    //        chkflag = false;
    //        string stud = dr_Students["RlNo"].ToString();

    //        FpExternal.Sheets[0].RowCount += 1;
    //        serialno++;
    //        FpExternal.Sheets[0].Rows[0].Border.BorderColor = Color.Black;
    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dr_Students["RlNo"].ToString();
    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Tag = dr_Students["RlNo"].ToString();

    //        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 3, dr_Students["RgNo"].ToString());
    //        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 4, dr_Students["SName"].ToString());
    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
    //        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 5, dr_Students["type"].ToString());
    //        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 0, serialno.ToString());

    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Value = 0;
    //        gpa = Calulat_GPA(stud, ddlSemYr.SelectedValue.ToString());
    //        display_cgpa = Calculete_CGPA(stud, ddlSemYr.SelectedValue.ToString());

    //        string result = "";


    //        //for (int col = 5; col <= FpExternal.Sheets[0].ColumnCount - 4; col += 2)
    //        //{
    //        for (int col = 0; col <ds_has.Tables[0].Rows.Count;col++)

    //        {
    //            grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
    //            con_Grade_flag.Close();
    //            con_Grade_flag.Open();
    //            SqlDataReader drgrade;
    //            SqlCommand cmd_grade = new SqlCommand(grade, con_Grade_flag);
    //            drgrade = cmd_grade.ExecuteReader();

    //            while (drgrade.Read())
    //            {

    //                getgradeflag = drgrade["grade_flag"].ToString();
    //                //getsubno = FpExternal.Sheets[0].ColumnHeader.Cells[5, col].Note.ToString();
    //                                  getsubno=ds_has.Tables[0].Rows[col]["subject_no"].ToString();
    //                if (getsubno != "")
    //                {
    //                    string getminmaxmark = "select mintotal,maxtotal from subject where subject_no='" + getsubno.ToString() + "'";
    //                    DataSet ds_getmrk = new DataSet();
    //                    SqlDataAdapter da_getmrk = new SqlDataAdapter(getminmaxmark, con);
    //                    con.Close();
    //                    con.Open();
    //                    da_getmrk.Fill(ds_getmrk);
    //                    mintotal = ds_getmrk.Tables[0].Rows[0]["mintotal"].ToString();
    //                    maxtotal = ds_getmrk.Tables[0].Rows[0]["maxtotal"].ToString();
    //                }
    //                //'---------------------------------------------------
    //                Session["e_code"] = ExamCode;
    //                string sql = "";

    //                sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " and Attempts =1 and roll_no='" + dr_Students["RlNo"].ToString() + "' and  Mark_Entry.subject_no='" + getsubno + "' order by subject_type desc,mark_entry.subject_no";
    //                con_mrkentry.Close();
    //                con_mrkentry.Open();
    //                SqlDataReader drmrkentry;
    //                SqlCommand cmd_mrkentry = new SqlCommand(sql, con_mrkentry);
    //                drmrkentry = cmd_mrkentry.ExecuteReader();
    //                while (drmrkentry.Read())
    //                {
    //                    if (drmrkentry.HasRows == true)
    //                    {
    //                        result = drmrkentry["result"].ToString();

    //                        if (drmrkentry["grade"] != "")
    //                        {

    //                            if (Convert.ToInt32(getgradeflag) == 1)
    //                            {
    //                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(7, col, 1, 2);
    //                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(8, col, 1, 2);
    //                                FpExternal.Sheets[0].ColumnHeader.Cells[7, col].Text = mintotal.ToString();
    //                                FpExternal.Sheets[0].ColumnHeader.Cells[8, col].Text = maxtotal.ToString();

    //                                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, drmrkentry["grade"].ToString());   //old

    //                                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col + 1, result.ToString());

    //                                if ((drmrkentry["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
    //                                }
    //                                else if ((result == "AAA") || (result == "-1"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
    //                                }
    //                                else
    //                                {
    //                                    if (grade_setting == "0")
    //                                    {
    //                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, drmrkentry["grade"].ToString());

    //                                    }

    //                                    else
    //                                    {

    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["total"].ToString();
    //                                        if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
    //                                        {
    //                                            convertgrade(stud, getsubno);
    //                                            result = "Pass";
    //                                        }
    //                                        else
    //                                        {
    //                                            funcgrade = "RA";
    //                                            result = "Fail";
    //                                        }
    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();
    //                                    }
    //                                }


    //                                if (chkflag == false)
    //                                {
    //                                    if (result == "Pass")
    //                                    {
    //                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
    //                                    }
    //                                    else
    //                                    {
    //                                        if ((drmrkentry["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
    //                                        {
    //                                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
    //                                            chkflag = true;
    //                                        }
    //                                        else
    //                                        {
    //                                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
    //                                            chkflag = true;
    //                                        }
    //                                    }
    //                                }

    //                            }


    //                            if (Convert.ToInt32(getgradeflag) == 2)
    //                            {

    //                                //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(7, col, 1, 2);
    //                                //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(8, col, 1, 2);

    //                                //FpExternal.Sheets[0].ColumnHeader.Cells[7, col].Text = mintotal.ToString();
    //                                //FpExternal.Sheets[0].ColumnHeader.Cells[8, col].Text = maxtotal.ToString();

    //                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["grade"].ToString();
    //                               // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();

    //                                if ((drmrkentry["grade"].ToString() == "") && (dr_Students["mode"].ToString() == "3"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
    //                                }
    //                                else if ((result == "AAA") || (result == "-1"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
    //                                }
    //                                else
    //                                {

    //                                    if (grade_setting == "0")
    //                                    {
    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["grade"].ToString();

    //                                    }

    //                                    else
    //                                    {

    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["total"].ToString();


    //                                        if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
    //                                        {
    //                                            convertgrade(stud, getsubno);
    //                                            result = "Pass";
    //                                        }
    //                                        else
    //                                        {
    //                                            funcgrade = "RA";
    //                                            result = "Fail";
    //                                        }
    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();

    //                                    }
    //                                }
    //                                if (chkflag == false)
    //                                {
    //                                    if (result == "Pass")
    //                                    {
    //                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
    //                                    }
    //                                    else
    //                                    {
    //                                        if ((drmrkentry["grade"].ToString() == "") && (dr_Students["mode"].ToString() == "3"))
    //                                        {
    //                                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
    //                                            chkflag = true;
    //                                        }
    //                                        else
    //                                        {
    //                                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
    //                                            chkflag = true;
    //                                        }
    //                                    }
    //                                }


    //                            }
    //                            if (Convert.ToInt32(getgradeflag) == 3)
    //                            {
    //                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(7, col, 1, 2);
    //                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(8, col, 1, 2);

    //                                FpExternal.Sheets[0].ColumnHeader.Cells[7, col].Text = mintotal.ToString();
    //                                FpExternal.Sheets[0].ColumnHeader.Cells[8, col].Text = maxtotal.ToString();

    //                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["grade"].ToString();
    //                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();

    //                                if ((drmrkentry["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
    //                                    if (FpExternal.Sheets[0].GetText(FpExternal.Sheets[0].RowCount - 1, col) == "LE")
    //                                    {
    //                                        result = "LE";
    //                                    }
    //                                }
    //                                else if ((result == "AAA") || (result == "-1"))
    //                                {
    //                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
    //                                }
    //                                else
    //                                {

    //                                    if (grade_setting == "0")
    //                                    {
    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["grade"].ToString();


    //                                    }
    //                                    else
    //                                    {



    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = drmrkentry["total"].ToString();
    //                                        if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
    //                                        {
    //                                            convertgrade(stud, getsubno);
    //                                            result = "Pass";
    //                                        }
    //                                        else
    //                                        {
    //                                            funcgrade = "RA";
    //                                            result = "Fail";
    //                                        }
    //                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();

    //                                    }

    //                                }

    //                            }

    //                        }


    //                    }
    //                }


    //            }


    //        }

    //        if (display_cgpa == "NaN")
    //        {
    //            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 2), "0");
    //        }
    //        else
    //        {
    //            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 2), display_cgpa);
    //        }

    //        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 3), gpa);
    //        if (chkflag == false)
    //        {
    //            if (result == "Pass")
    //            {
    //                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, result);
    //            }
    //            else
    //            {

    //                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, result);
    //                chkflag = true;
    //            }
    //        }

    //    }


    //}
    public void main_function()
    {
        exam_month = ddlMonth.SelectedValue.ToString();
        exam_year = ddlYear.SelectedValue.ToString();
        int absent_cnt = 0;
        int RA_cnt = 0;
        int WH_cnt = 0;
        string grade_get = "";
        string gpa = "";
        string strStudents = "";
        string grade = "";
        Boolean chkflag = false;
        string mintotal = "";
        string maxtotal = "";
        string display_cgpa = "";
        int yp = 5;
        int allpass_cnt = 0;



        SqlDataReader dr_grade_val;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "", con);
        dr_grade_val = cmd.ExecuteReader();
        while (dr_grade_val.Read())
        {
            if (dr_grade_val.HasRows == true)
            {
                grade_setting = dr_grade_val[0].ToString();
            }
        }
        int RA = 0;

        strStudents = "Select isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.app_no=applyn.app_no and cc=0 and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' " + reg_strsec + " order by RlNo ";

        con_Stud.Close();
        con_Stud.Open();
        SqlCommand cmd_Subject = new SqlCommand(strStudents, con_Stud);
        SqlDataReader dr_Students;
        dr_Students = cmd_Subject.ExecuteReader();



        while (dr_Students.Read())
        {

            RA = 0;
            yp = 5;
            chkflag = false;
            string stud = dr_Students["RlNo"].ToString();

            FpExternal.Sheets[0].RowCount += 1;
            serialno++;
            FpExternal.Sheets[0].Rows[0].Border.BorderColor = Color.Black;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = sl_no1.ToString();
            if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dr_Students["RgNo"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dr_Students["RlNo"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Tag = dr_Students["RlNo"].ToString();

                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 1);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 2, 1, 1);
            }
            else if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dr_Students["RlNo"].ToString();
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 2);
            }
            else if (Session["Rollflag"].ToString() == "0" && Session["Regflag"].ToString() == "1")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dr_Students["RgNo"].ToString();
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 2);
            }
            else
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dr_Students["RgNo"].ToString();
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 1, 1, 2);
            }


            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = dr_Students["SName"].ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 4, dr_Students["type"].ToString());



            gpa = Calulat_GPA(stud, ddlSemYr.SelectedValue.ToString());
            display_cgpa = Calculete_CGPA(stud, ddlSemYr.SelectedValue.ToString());

            string result = "";



            for (int col = 0; col < ds_has.Tables[0].Rows.Count; col++)
            {
                grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
                con_Grade_flag.Close();
                con_Grade_flag.Open();
                SqlDataReader drgrade;
                SqlCommand cmd_grade = new SqlCommand(grade, con_Grade_flag);
                drgrade = cmd_grade.ExecuteReader();

                while (drgrade.Read())
                {

                    getgradeflag = drgrade["grade_flag"].ToString();
                    getsubno = ds_has.Tables[0].Rows[col]["subject_no"].ToString();
                    if (getsubno != "")
                    {
                        string getminmaxmark = "select mintotal,maxtotal from subject where subject_no='" + getsubno.ToString() + "'";
                        DataSet ds_getmrk = new DataSet();
                        SqlDataAdapter da_getmrk = new SqlDataAdapter(getminmaxmark, con);
                        con.Close();
                        con.Open();
                        da_getmrk.Fill(ds_getmrk);
                        mintotal = ds_getmrk.Tables[0].Rows[0]["mintotal"].ToString();
                        maxtotal = ds_getmrk.Tables[0].Rows[0]["maxtotal"].ToString();
                    }

                    exam_code_new = int.Parse(Session["examcode"].ToString());
                    string sql = "";

                    sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + exam_code_new + " and Attempts =1 and roll_no='" + dr_Students["RlNo"].ToString() + "' and  Mark_Entry.subject_no='" + getsubno + "' order by subject_type desc,mark_entry.subject_no";
                    con_mrkentry.Close();
                    con_mrkentry.Open();
                    SqlDataReader drmrkentry;
                    SqlCommand cmd_mrkentry = new SqlCommand(sql, con_mrkentry);
                    drmrkentry = cmd_mrkentry.ExecuteReader();

                    if (drmrkentry.HasRows == true)
                    {
                        while (drmrkentry.Read())
                        {
                            result = drmrkentry["result"].ToString();

                            if (drmrkentry["grade"] != "")
                            {
                                if (Convert.ToInt32(getgradeflag) == 1)
                                {

                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, drmrkentry["grade"].ToString());   //old

                                    if ((drmrkentry["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
                                    {
                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "LE");
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "AAA");
                                    }
                                    else
                                    {
                                        if (grade_setting == "0")
                                        {
                                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, drmrkentry["grade"].ToString());
                                            if (drmrkentry["grade"].ToString() == "RA")
                                            {
                                                RA++;
                                            }
                                        }

                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["total"].ToString();
                                            if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(stud, getsubno);
                                                result = "Pass";
                                            }
                                            else
                                            {
                                                funcgrade = "RA";
                                                result = "Fail";
                                                RA++;
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = funcgrade.ToString();
                                        }
                                    }
                                }


                                if (Convert.ToInt32(getgradeflag) == 2)
                                {

                                    //++++++++++++++++++++
                                    string pl = drmrkentry["grade"].ToString();
                                    if (pl == "S" || pl == "A" || pl == "B" || pl == "C" || pl == "D" || pl == "E")
                                    {
                                        allpass_cnt = allpass_cnt + 1;
                                    }
                                    if (drmrkentry["grade"].ToString() == "AB" || drmrkentry["grade"].ToString() == "AAA")
                                    {
                                        absent_cnt = absent_cnt + 1;
                                    }
                                    if (drmrkentry["grade"].ToString() == "RA")
                                    {
                                        RA_cnt = RA_cnt + 1;
                                    }

                                    if (drmrkentry["grade"].ToString() == "WH")
                                    {
                                        WH_cnt = WH_cnt + 1;
                                    }

                                    //++++++++++++++++++++


                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["grade"].ToString();


                                    if ((drmrkentry["grade"].ToString() == "") && (dr_Students["mode"].ToString() == "3"))
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = "LE";
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "AAA");
                                    }
                                    else
                                    {

                                        if (grade_setting == "0")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["grade"].ToString();
                                            if (drmrkentry["grade"].ToString() == "RA")
                                            {
                                                RA++;
                                            }
                                        }

                                        else
                                        {

                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["total"].ToString();


                                            if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(stud, getsubno);
                                                result = "Pass";
                                            }
                                            else
                                            {
                                                funcgrade = "RA";
                                                result = "Fail";
                                                RA++;
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = funcgrade.ToString();

                                        }
                                    }




                                }
                                if (Convert.ToInt32(getgradeflag) == 3)
                                {


                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["grade"].ToString();


                                    if ((drmrkentry["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
                                    {
                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "LE");
                                        if (FpExternal.Sheets[0].GetText(FpExternal.Sheets[0].RowCount - 1, col) == "LE")
                                        {
                                            result = "LE";
                                        }
                                    }
                                    else if ((result == "AAA") || (result == "-1"))
                                    {
                                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "AAA");
                                    }
                                    else
                                    {

                                        if (grade_setting == "0")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["grade"].ToString();
                                            if (drmrkentry["grade"].ToString() == "RA")
                                            {
                                                RA++;
                                            }

                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = drmrkentry["total"].ToString();
                                            if (Convert.ToDouble(drmrkentry["internal_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_int_marks"].ToString()) && Convert.ToDouble(drmrkentry["External_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(stud, getsubno);
                                                result = "Pass";
                                            }
                                            else
                                            {
                                                funcgrade = "RA";
                                                result = "Fail";
                                                RA++;
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, yp].Text = funcgrade.ToString();

                                        }

                                    }

                                }


                            }
                            else
                            {
                                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "0");
                            }
                        }
                    }
                    else
                    {
                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, yp, "-");
                    }

                }

                yp++;
            }

            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 8].Text = gpa;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 7].Text = absent_cnt.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 6].Text = RA_cnt.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 5].Text = "0";////////3/7/12 Prabha
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 4].Text = WH_cnt.ToString();
            WH_cnt = 0;

            absent_cnt = 0;
            RA_cnt = 0;




            sl_no1++;
            if (allpass_cnt == ds_has.Tables[0].Rows.Count)
            {
                allpass_tot_cnt = allpass_tot_cnt + 1;
            }
            allpass_cnt = 0;

            int ra_cnt = 0;
            ra_cnt = Convert.ToInt32(GetCorrespondingKey(RA, has_ra));
            has_ra[RA] = ra_cnt + 1;
        }


    }
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
    public void course()
    {
        string stud_apply = "";
        Hashtable has_count = new Hashtable();
        string subj_no = "", no_of_stud = "";
        int l = 5, row_suj_no = 0;
        FpExternal.Sheets[0].RowCount += 3;
        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 3].Height = 40;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, FpExternal.Sheets[0].ColumnCount);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "SUBJECTWISE PERFOMANCE";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, 5);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "Subject Code";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
        for (int u = 0; u < ds_has.Tables[0].Rows.Count; u++)
        {
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].Text = ds_has.Tables[0].Rows[u]["subject_code"].ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].Text = (u + 1).ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].Tag = ds_has.Tables[0].Rows[u]["subject_no"].ToString();
            if (u == 0)
            {
                row_suj_no = FpExternal.Sheets[0].RowCount - 2;
            }
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, l].HorizontalAlign = HorizontalAlign.Center;
            // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, l].Text = Convert.ToString(sl_no1-1);//------student apply for this subject
            //-------------------------------17/7/12
            string secval = "";
            if (ddlSec.Enabled == true)
            {
                if (ddlSec.SelectedItem.ToString() != "" && ddlSec.SelectedItem.ToString() != null && ddlSec.Enabled == true)
                {
                    secval = " and sections='" + ddlSec.SelectedItem.ToString() + "'";
                }
            }
            stud_apply = GetFunction("select count(ea.roll_no) from exam_application ea,exam_appl_details ead,registration r where ea.appl_no=ead.appl_no and subject_no= " + ds_has.Tables[0].Rows[u]["subject_no"].ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + "  and batch_year=" + ddlBatch.SelectedValue.ToString() + " " + secval + " and ea.roll_no=r.roll_no and exam_code=" + exam_code_new + "");
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, l].Text = stud_apply;
            if (!has_count.ContainsKey("total"))
            {
                has_count.Add("total", Convert.ToInt32(stud_apply));
            }
            else
            {
                int count_temp = 0;
                count_temp = Convert.ToInt32(GetCorrespondingKey("total", has_count));
                has_count["total"] = count_temp + Convert.ToInt32(stud_apply);

            }


            //=======================
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, l].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, l].Font.Bold = true;
            l++;

        }
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7, 2, 1);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7].Text = "Gradewise %";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 7].HorizontalAlign = HorizontalAlign.Center;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 5);
        //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Total No of Students Registered for each Subject";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "No. of students appeared for the exam";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;

        con.Close();
        con.Open();
        string grade_master = " select  distinct(mark_grade),frange,trange,credit_points from grade_master where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and batch_year='" + ddlBatch.SelectedValue.ToString() + "' order by frange desc";
        SqlDataAdapter da_grad_master = new SqlDataAdapter(grade_master, con);
        DataSet ds_grad_master = new DataSet();
        da_grad_master.Fill(ds_grad_master);
        if (ds_grad_master.Tables[0].Rows.Count > 0)
        {
            for (int grade = 0; grade < ds_grad_master.Tables[0].Rows.Count; grade++)
            {


                if (ds_grad_master.Tables[0].Rows[grade]["frange"].ToString() == "0")
                {
                    FpExternal.Sheets[0].RowCount += 2;
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 2, 3);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "PASS IN EACH SUBJECT(" + ds_grad_master.Tables[0].Rows[0]["mark_grade"].ToString() + " to " + ds_grad_master.Tables[0].Rows[ds_grad_master.Tables[0].Rows.Count - 1]["mark_grade"].ToString() + " Grade put together";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;

                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 3, 1, 2);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Text = "No.of Students";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Right;
                    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 2);
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Percentage";
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;

                    for (int u = 5; u < ds_has.Tables[0].Rows.Count + 5; u++)
                    {
                        subj_no = FpExternal.Sheets[0].Cells[row_suj_no, u].Tag.ToString();
                        no_of_stud = GetFunction("select count(internal_mark+external_mark) from mark_entry where  Exam_Code = '" + exam_code_new + "' and subject_no='" + subj_no + "'   and (internal_mark+external_mark)>(select top 1 trange From grade_master where degree_code=" + ddlBranch.SelectedValue.ToString() + "  and batch_year=" + ddlBatch.SelectedValue.ToString() + " and frange=0)");
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, u].Text = no_of_stud;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, u].Text = Math.Round(((Convert.ToDouble(no_of_stud) / Convert.ToDouble((FpExternal.Sheets[0].Cells[(row_suj_no + 1), u].Text))) * 100), 2).ToString();

                        if (!has_count.ContainsKey("allpass"))
                        {
                            has_count.Add("allpass", Convert.ToInt32(no_of_stud));
                        }
                        else
                        {
                            int temp_cnt = 0;
                            temp_cnt = Convert.ToInt32(GetCorrespondingKey("allpass", has_count));
                            has_count["allpass"] = temp_cnt + Convert.ToInt32(no_of_stud);
                        }
                    }
                }

                FpExternal.Sheets[0].RowCount += 2;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 2, 3);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = ds_grad_master.Tables[0].Rows[grade]["frange"].ToString() + " - " + ds_grad_master.Tables[0].Rows[grade]["trange"].ToString() + " Marks(" + ds_grad_master.Tables[0].Rows[grade]["mark_grade"].ToString() + " Grade " + ds_grad_master.Tables[0].Rows[grade]["credit_points"].ToString() + " Points)";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;


                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 3, 1, 2);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Text = "No.of Students";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Right;
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 2);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Percentage";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;

                string frange = ds_grad_master.Tables[0].Rows[grade]["frange"].ToString();
                for (int u = 5; u < ds_has.Tables[0].Rows.Count + 5; u++)
                {
                    subj_no = FpExternal.Sheets[0].Cells[row_suj_no, u].Tag.ToString();
                    no_of_stud = GetFunction("select count(internal_mark+external_mark) from mark_entry where  Exam_Code = '" + exam_code_new + "' and subject_no='" + subj_no + "' and (internal_mark+external_mark) between " + ds_grad_master.Tables[0].Rows[grade]["frange"].ToString() + " and " + ds_grad_master.Tables[0].Rows[grade]["trange"].ToString() + "");
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, u].Text = no_of_stud;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, u].Text = Math.Round(((Convert.ToDouble(no_of_stud) / Convert.ToDouble((FpExternal.Sheets[0].Cells[(row_suj_no + 1), u].Text))) * 100), 2).ToString();
                    if (!has_count.ContainsKey(Convert.ToInt32(ds_grad_master.Tables[0].Rows[grade]["frange"].ToString())))
                    {
                        has_count.Add(Convert.ToInt32(ds_grad_master.Tables[0].Rows[grade]["frange"].ToString()), Convert.ToInt32(no_of_stud));
                    }
                    else
                    {
                        int temp_cnt = 0;
                        temp_cnt = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(frange), has_count));
                        has_count[Convert.ToInt32(ds_grad_master.Tables[0].Rows[grade]["frange"].ToString())] = temp_cnt + Convert.ToInt32(no_of_stud);
                    }

                }
            }

            //--------------------------absent
            FpExternal.Sheets[0].RowCount += 2;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 2, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "Absent(AB)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 3, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Text = "No.of Students";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Right;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Percentage";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;

            for (int u = 5; u < ds_has.Tables[0].Rows.Count + 5; u++)
            {
                subj_no = FpExternal.Sheets[0].Cells[row_suj_no, u].Tag.ToString();
                no_of_stud = GetFunction("select count(*) from mark_entry where result like '%A%' and result<>'pass' and result<>'fail' and Exam_Code = " + exam_code_new + " and subject_no='" + subj_no + "' ");
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, u].Text = no_of_stud;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, u].Text = Math.Round(((Convert.ToDouble(no_of_stud) / Convert.ToDouble((FpExternal.Sheets[0].Cells[(row_suj_no + 1), u].Text))) * 100), 2).ToString();

                if (!has_count.ContainsKey("onefail"))
                {
                    has_count.Add("onefail", Convert.ToInt64(no_of_stud));
                }
                else
                {
                    int temp_cnt = 0;
                    temp_cnt = Convert.ToInt32(GetCorrespondingKey("onefail", has_count));
                    has_count["onefail"] = temp_cnt + Convert.ToInt32(no_of_stud);
                }
            }

            //--------------------------WH
            FpExternal.Sheets[0].RowCount += 2;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 2, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "Withheld(WH)";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;

            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 3, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Text = "No.of Students";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Right;
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Percentage";
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;

            for (int u = 5; u < ds_has.Tables[0].Rows.Count + 5; u++)
            {
                subj_no = FpExternal.Sheets[0].Cells[row_suj_no, u].Tag.ToString();
                no_of_stud = GetFunction("select *from mark_entry where result like '%W%' and Exam_Code = " + exam_code_new + " and subject_no='" + subj_no + "' ");
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, u].Text = no_of_stud;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, u].Text = Math.Round(((Convert.ToDouble(no_of_stud) / Convert.ToDouble((FpExternal.Sheets[0].Cells[(row_suj_no + 1), u].Text))) * 100), 2).ToString();
                if (!has_count.ContainsKey("wh"))
                {
                    has_count.Add("wh", Convert.ToInt32(no_of_stud));
                }
                else
                {
                    int temp_cnt = 0;
                    temp_cnt = Convert.ToInt32(GetCorrespondingKey("wh", has_count));
                    has_count["wh"] = temp_cnt + Convert.ToInt32(no_of_stud);
                }
            }


            //-------------------gradewise 

            for (int grade = 0; grade < ds_grad_master.Tables[0].Rows.Count; grade++)
            {
                string frange = ds_grad_master.Tables[0].Rows[grade]["frange"].ToString();
                if (frange == "0")
                {
                    FpExternal.Sheets[0].Cells[row_suj_no + 2, FpExternal.Sheets[0].ColumnCount - 7].Text = GetCorrespondingKey("allpass", has_count).ToString();
                    FpExternal.Sheets[0].Cells[row_suj_no + 3, FpExternal.Sheets[0].ColumnCount - 7].Text = Math.Round(((Convert.ToDouble(GetCorrespondingKey("allpass", has_count).ToString()) * 100) / Convert.ToDouble(GetCorrespondingKey("total", has_count).ToString())), 2).ToString();
                    row_suj_no += 2;
                }
                FpExternal.Sheets[0].Cells[row_suj_no + 2, FpExternal.Sheets[0].ColumnCount - 7].Text = GetCorrespondingKey(Convert.ToInt32(frange), has_count).ToString();
                FpExternal.Sheets[0].Cells[row_suj_no + 3, FpExternal.Sheets[0].ColumnCount - 7].Text = Math.Round(((Convert.ToDouble(GetCorrespondingKey(Convert.ToInt32(frange), has_count).ToString()) * 100) / Convert.ToDouble(GetCorrespondingKey("total", has_count).ToString())), 2).ToString();
                row_suj_no += 2;
            }
            if (ds_grad_master.Tables[0].Rows.Count > 0)
            {
                FpExternal.Sheets[0].Cells[row_suj_no + 2, FpExternal.Sheets[0].ColumnCount - 7].Text = GetCorrespondingKey("onefail", has_count).ToString();
                FpExternal.Sheets[0].Cells[row_suj_no + 3, FpExternal.Sheets[0].ColumnCount - 7].Text = Math.Round(((Convert.ToDouble(GetCorrespondingKey("onefail", has_count).ToString()) * 100) / Convert.ToDouble(GetCorrespondingKey("total", has_count).ToString())), 2).ToString();
                row_suj_no += 2;

                FpExternal.Sheets[0].Cells[row_suj_no + 2, FpExternal.Sheets[0].ColumnCount - 7].Text = GetCorrespondingKey("wh", has_count).ToString();
                FpExternal.Sheets[0].Cells[row_suj_no + 3, FpExternal.Sheets[0].ColumnCount - 7].Text = Math.Round(((Convert.ToDouble(GetCorrespondingKey("wh", has_count).ToString()) * 100) / Convert.ToDouble(GetCorrespondingKey("total", has_count).ToString())), 2).ToString();
                row_suj_no += 2;
            }

        }






        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6, 3, 5);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6].Text = "NO OF STUDENTS CLEARED IN ALL SUBJECT";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Left;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 6].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1, 3, 1);

        int stud_fail_atleast_one = Convert.ToInt32(GetFunction("select count(distinct(m.roll_no)) from mark_entry m,registration r where m.roll_no=r.roll_no  and r.delflag<>1 and m.attempts = 1 and ltrim(rtrim(type))='' and m.exam_code in (select exam_code from exam_details where degree_code=" + ddlBranch.SelectedValue.ToString() + "  and current_semester=" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + ") and m.result = 'Fail' " + r_strsec + " "));
        int noof_stud_appearexam = Convert.ToInt32(GetFunction("select count(distinct m.roll_no) from mark_entry m,registration r where m.roll_no=r.roll_no and r.delflag<>1 and m.attempts = 1 and ltrim(rtrim(type))='' and m.exam_code = (select exam_code from exam_details where degree_code=" + ddlBranch.SelectedValue.ToString() + " and current_semester=" + ddlSemYr.SelectedItem.ToString() + " and batch_year=" + ddlBatch.SelectedItem.ToString() + ") " + r_strsec + ""));

        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1].Text = (noof_stud_appearexam - stud_fail_atleast_one).ToString();
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 10, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = 13;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6, 3, 5);
        //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].Text ="TOTAL NO. OF STUDENTS REGISTERED";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].Text = "NO. OF STUDENTS APPEARED FOR THE EXAMS";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].Font.Size = 13;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Left;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1, 3, 1);
        //   double g1 = sl_no1 - 1;

        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1].Text = noof_stud_appearexam.ToString();
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 7, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = 13;
        double allpass_prec = 0;
        double allp_prec = Convert.ToDouble((noof_stud_appearexam - stud_fail_atleast_one)) / Convert.ToDouble(noof_stud_appearexam);
        if (allp_prec.ToString() == "NaN")
        {
            allpass_prec = 0;
        }
        else
        {
            allpass_prec = Math.Round(allp_prec, 2);
        }
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6, 4, 5);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].Text = "% OF STUDENTS CLEARED ALL SUBJECT IN THIS SEMESTER";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Left;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].Font.Size = 14;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1, 4, 1); FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1].Text = allpass_prec.ToString();
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = 14;

        FpExternal.Sheets[0].RowCount += 4;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 3, 3);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Border.BorderColorTop = Color.White;
        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 4].Height = 40;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, 0, 1, FpExternal.Sheets[0].ColumnCount - 6);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Text = "FAILURE ANALYSIS (Excluding AB & WH)";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].VerticalAlign = VerticalAlign.Middle;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Font.Size = FontUnit.Medium;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 3, 1, 2);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Text = "No.of Subjects To Reappear:";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Left;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 3, 1, 2);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Text = "No.of Students";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Left;

        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 3, 1, 2);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Percentage (%)";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

        string set_val = "";
        //-------------------------Reappear %
        for (int u = 0; u < ds_has.Tables[0].Rows.Count; u++)
        {
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, u + 5].Text = (u + 1).ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, u + 5].Text = GetCorrespondingKey((u + 1), has_ra).ToString();
            set_val = Math.Round(((Convert.ToDouble(GetCorrespondingKey((u + 1), has_ra)) * 100) / Convert.ToDouble(stud_apply))).ToString();
            if (set_val == "NaN" || set_val == "Infinity")
            {
                set_val = "0";
            }
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, u + 5].Text = set_val;
            //stud_apply
        }

        //----------------------------------




        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6, 2, 6);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, FpExternal.Sheets[0].ColumnCount - 6].Text = "ABSENTEESIM IN EXAM";
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 6, 2, 5);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 6].Text = "Students Absent for atleast one exam";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Left;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1, 2, 1);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, FpExternal.Sheets[0].ColumnCount - 1].Text = stud_fail_atleast_one.ToString();
    }
    public void logoset()
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++ Start logoset +++++++++++++++++++++++++++++++++++++//
        SqlConnection con_header = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        string query_header = "";
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 12;
        style.Font.Bold = true;
        FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternal.Sheets[0].AllowTableCorner = true;

        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        //  FpExternal.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;


        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler/Handler5.ashx?";


        string coll_name = "", phoneno = "", faxno = "", footer = "", new_header = "", new_header_index = "", form_name = "", degree_deatil = "", header_alignment = "", view_header = "", state = "", pincode = "", district = "";
        Boolean check_print_row = false;
        int footer_count = 0, col_count = 0, temp_count = 0;
        string footer_text = "", leftlogo = "", rightlogo = "", multi_iso = "";
        int split_col_for_footer = 0, footer_balanc_col = 0, visi_row_cnt = 0;

        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        // cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(affliated,'') as affliated,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header,isnull(state,'')  as state,isnull(pincode,'') as pincode,isnull(district,'') as district,isnull(new_header_name,'') as new_header_name,isnull(header_align_index,'') as header_align_index,isnull(footer_name,'') as footer_name  from print_master_setting  where form_name='university_mark.aspx'", con);
        cmd = new SqlCommand("select *  from print_master_setting  where form_name='university_mark.aspx'", con);
        dr_collinfo = cmd.ExecuteReader();
        while (dr_collinfo.Read())
        {
            if (dr_collinfo.HasRows == true)
            {
                check_print_row = true;
                coll_name = dr_collinfo["college_name"].ToString();
                address1 = dr_collinfo["address1"].ToString();
                address2 = dr_collinfo["address2"].ToString();
                address3 = dr_collinfo["address3"].ToString();
                phoneno = dr_collinfo["phoneno"].ToString();
                faxno = dr_collinfo["faxno"].ToString();
                email = dr_collinfo["email"].ToString();
                website = dr_collinfo["website"].ToString();
                form_name = dr_collinfo["form_name"].ToString();
                degree_deatil = dr_collinfo["affliated"].ToString();
                header_alignment = dr_collinfo["header_align"].ToString();
                view_header = dr_collinfo["header_flag_value"].ToString();
                district = dr_collinfo["district"].ToString();
                state = dr_collinfo["state"].ToString();
                pincode = dr_collinfo["pincode"].ToString();
                new_header = dr_collinfo["new_header_name"].ToString();
                new_header_index = dr_collinfo["header_align_index"].ToString();
                footer = dr_collinfo["footer_name"].ToString();
                leftlogo = dr_collinfo["leftlogo"].ToString();
                rightlogo = dr_collinfo["rightlogo"].ToString();
                multi_iso = dr_collinfo["MultiISOCode"].ToString();
            }

        }
        if (check_print_row == false)
        {

            con.Close();
            con.Open();
            //  cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(state,'') as state,isnull(pincode,'') as pincode   from collinfo  where college_code=" + Session["collegecode"] + "", con);
            cmd = new SqlCommand("select * from collinfo  where college_code=" + Session["collegecode"] + "", con);
            dr_collinfo = cmd.ExecuteReader();
            while (dr_collinfo.Read())
            {
                if (dr_collinfo.HasRows == true)
                {

                    string sec_val = "";

                    if (ddlSec.SelectedValue.ToString() != string.Empty && ddlSec.SelectedValue.ToString() != null)
                    {
                        sec_val = "Section: " + ddlSec.SelectedItem.ToString();
                    }
                    else
                    {
                        sec_val = "";
                    }


                    check_print_row = true;
                    coll_name = dr_collinfo["collname"].ToString();
                    address1 = dr_collinfo["address1"].ToString();
                    address2 = dr_collinfo["address2"].ToString();
                    address3 = dr_collinfo["address3"].ToString();
                    phoneno = dr_collinfo["phoneno"].ToString();
                    faxno = dr_collinfo["faxno"].ToString();
                    email = dr_collinfo["email"].ToString();
                    website = dr_collinfo["website"].ToString();
                    form_name = "Consolidated Grade Sheet";
                    district = dr_collinfo["district"].ToString();
                    state = dr_collinfo["state"].ToString();
                    pincode = dr_collinfo["pincode"].ToString();
                    degree_deatil = dr_collinfo["category"].ToString() + "," + dr_collinfo["affliatedby"].ToString();// ddlDegree .SelectedItem.ToString() + "-" + ddlBranch .SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr .SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    new_header = "";
                    new_header_index = "";
                    footer = "";
                    leftlogo = "0";
                    rightlogo = "0";
                    multi_iso = "";
                }

            }
        }



        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);

        if (coll_name.Trim() != "")
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = coll_name.ToString();
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Height = 5;
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Tag = 1;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Visible = false;
        }

        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = 14;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;

        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);

        if (degree_deatil.Trim() != "")
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Text = degree_deatil;// sdr_header["category"].ToString() + ", Affliated to" + sdr_header["affliatedby"].ToString();
            FpExternal.Sheets[0].ColumnHeader.Rows[1].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[1].Tag = 1;
            FpExternal.Sheets[0].ColumnHeader.Rows[1].Height = 5;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[1].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[1].Visible = false;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;

        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);
        if (address1.Trim() != "" && address2.Trim() != "")
        {
            address1 = address1 + "-";
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Text = address1 + address2;// +"-" + sdr_header["address1"].ToString();
            FpExternal.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[2].Height = 5;
            FpExternal.Sheets[0].ColumnHeader.Rows[2].Tag = 1;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[2].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[2].Visible = false;
        }

        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;

        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);
        if (address3.Trim() != "" && district.Trim() != "" && pincode.Trim() != "")
        {
            address3 = address3 + "-";
            district = district + ",";

            FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Text = address3 + district + pincode.ToString();
            FpExternal.Sheets[0].ColumnHeader.Rows[3].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[3].Height = 5;
            FpExternal.Sheets[0].ColumnHeader.Rows[3].Tag = 1;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[3].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[3].Visible = false;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;


        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);
        if (phoneno.Trim() != "" && faxno.Trim() != "")
        {
            phoneno = "Phone:" + phoneno;
            faxno = "Fax :" + faxno;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Text = phoneno + faxno;
            FpExternal.Sheets[0].ColumnHeader.Rows[4].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[4].Height = 5;
            FpExternal.Sheets[0].ColumnHeader.Rows[4].Tag = 1;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[4].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[4].Visible = false;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;

        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, FpExternal.Sheets[0].ColumnCount - 4);//5th row span
        if (email.Trim() != "" && website.Trim() != "")
        {
            email = "E-Mail:" + email;
            website = "Web Site :" + website;
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Text = email + website;
            FpExternal.Sheets[0].ColumnHeader.Rows[5].Visible = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[5].Height = 5;
            FpExternal.Sheets[0].ColumnHeader.Rows[5].Tag = 1;
            visi_row_cnt++;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[5].Tag = 0;
            FpExternal.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Font.Name = "Book Antiqua";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;


        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 1);

        if (leftlogo == "1")
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
        }

        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;


        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 6, 1);
        if (rightlogo == "1")
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].CellType = mi2;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;

        //++++++++++++++++++++++++++++++++++++++++++++++++++ End logoset +++++++++++++++++++++++++++++++++++++++//

        //--------------------------------ISO Set
        int row_val = 0;
        if (multi_iso.Trim() != "")
        {
            string[] multi_iso_spt = multi_iso.Split(',');

            for (int iso = 0; iso <= multi_iso_spt.GetUpperBound(0); iso++)
            {
                if (row_val > 5)
                {
                    FpExternal.Sheets[0].ColumnHeader.RowCount++;
                    FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = " ";
                    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add((FpExternal.Sheets[0].ColumnHeader.RowCount - 1), 0, 1, FpExternal.Sheets[0].ColumnCount - 3);
                    FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorRight = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorTop = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorBottom = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Rows[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1)].Tag = "1";
                    if (rightlogo == "1")
                    {
                        FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), (FpExternal.Sheets[0].ColumnCount - 1)].Text = " ";
                        FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[(FpExternal.Sheets[0].ColumnHeader.RowCount - 1), (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
                    }
                }
                if (FpExternal.Sheets[0].ColumnHeader.Rows[row_val].Tag.ToString() == "1")
                {
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_val, (FpExternal.Sheets[0].ColumnCount - 3)].Text = multi_iso_spt[iso];
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_val, (FpExternal.Sheets[0].ColumnCount - 3)].Border.BorderColorLeft = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_val, (FpExternal.Sheets[0].ColumnCount - 3)].Border.BorderColorTop = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_val, (FpExternal.Sheets[0].ColumnCount - 3)].Border.BorderColorBottom = Color.White;

                    if (rightlogo == "1")
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(row_val, (FpExternal.Sheets[0].ColumnCount - 3), 1, 2);
                        FpExternal.Sheets[0].ColumnHeader.Cells[row_val, (FpExternal.Sheets[0].ColumnCount - 3)].Border.BorderColorRight = Color.White;
                    }
                    else
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(row_val, (FpExternal.Sheets[0].ColumnCount - 3), 1, 3);
                    }
                }
                else
                {
                    iso--;
                }
                row_val++;
            }

        }




        //--------------------------set header
        int temp_count_temp = 0;
        string[] header_align_index;
        string[] header_align;

        if (new_header.Trim() != "")
        {

            if (new_header.Trim() != null && new_header.Trim() != "")
            {
                header_align = new_header.ToString().Split(',');
                header_align_index = new_header_index.ToString().Split(',');
                FpExternal.Sheets[0].ColumnHeader.Rows.Count += header_align_index.GetUpperBound(0) + 1;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
                for (int row_head_count = 6; row_head_count < (6 + header_align.GetUpperBound(0) + 1); row_head_count++)
                {
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Text = header_align[temp_count_temp].ToString();
                    FpExternal.Sheets[0].ColumnHeader.Rows[row_head_count].Tag = 1;
                    //if (final_print_col_cnt > 3)
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, 0, 1, (FpExternal.Sheets[0].ColumnCount));
                    }
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Border.BorderColorTop = Color.White;
                    FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].Border.BorderColorBottom = Color.White;


                    if (temp_count_temp <= header_align_index.GetUpperBound(0))
                    {
                        if (header_align_index[temp_count_temp].ToString() != string.Empty)
                        {
                            header_alignment = header_align_index[temp_count_temp].ToString();
                            if (header_alignment == "2")
                            {
                                FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (header_alignment == "1")
                            {
                                FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                FpExternal.Sheets[0].ColumnHeader.Cells[row_head_count, 0].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }

                    temp_count_temp++;
                }
            }
        }
        //2.Footer setting


        if (footer.Trim() != "")
        {
            if (footer != null && footer != "")
            {

                string[] footer_text_split = footer.Split(',');

                footer_count = Convert.ToInt16((footer_text_split.GetUpperBound(0) + 1).ToString());
                FpExternal.Sheets[0].RowCount = FpExternal.Sheets[0].RowCount + 3;

                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 3), 0].ColumnSpan = FpExternal.Sheets[0].ColumnCount;
                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 2), 0].ColumnSpan = FpExternal.Sheets[0].ColumnCount;

                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;




                footer_text = "";




                if (FpExternal.Sheets[0].ColumnCount < footer_count)
                {
                    for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
                    {
                        if (footer_text == "")
                        {
                            footer_text = footer_text_split[concod_footer].ToString();
                        }
                        else
                        {
                            footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
                        }
                    }

                    for (col_count = 0; col_count < FpExternal.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpExternal.Sheets[0].Columns[col_count].Visible == true)
                        {
                            FpExternal.Sheets[0].SpanModel.Add((FpExternal.Sheets[0].RowCount - 1), col_count, 1, FpExternal.Sheets[0].ColumnCount);
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Text = footer_text;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                            break;
                        }
                    }

                }

                else if (FpExternal.Sheets[0].ColumnCount == footer_count)
                {
                    temp_count = 0;
                    for (col_count = 0; col_count < FpExternal.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpExternal.Sheets[0].Columns[col_count].Visible == true)
                        {
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                            temp_count++;
                            if (temp_count == footer_count)
                            {
                                break;
                            }
                        }
                    }

                }

                else
                {

                    temp_count = 0;
                    split_col_for_footer = FpExternal.Sheets[0].ColumnCount / footer_count;
                    footer_balanc_col = FpExternal.Sheets[0].ColumnCount % footer_count;

                    for (col_count = 0; col_count < FpExternal.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpExternal.Sheets[0].Columns[col_count].Visible == true)
                        {
                            if (temp_count == 0)
                            {
                                FpExternal.Sheets[0].SpanModel.Add((FpExternal.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
                            }
                            else
                            {

                                FpExternal.Sheets[0].SpanModel.Add((FpExternal.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

                            }
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Size = FontUnit.Medium;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
                            if (col_count - 1 >= 0)
                            {
                                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
                                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
                            }
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                            FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
                            if (col_count + 1 < FpExternal.Sheets[0].ColumnCount)
                            {
                                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
                                FpExternal.Sheets[0].Cells[(FpExternal.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
                            }


                            temp_count++;
                            if (temp_count == 0)
                            {
                                col_count = col_count + split_col_for_footer + footer_balanc_col;
                            }
                            else
                            {
                                col_count = col_count + split_col_for_footer;
                            }
                            if (temp_count == footer_count)
                            {
                                break;
                            }
                        }
                    }
                }



            }
        }

        //2 end.Footer setting
    }

    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
        SqlDataReader rsChkSet;
        con1.Close();
        con1.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con1);
        rsChkSet = cmd1.ExecuteReader();
        rsChkSet.Read();
        if (rsChkSet.HasRows == true)
        {
            if (rsChkSet["linkvalue"].ToString() == "1")
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "1";
                        break;
                    case 2:
                        sem_roman = "1-II";
                        break;
                    case 3:
                        sem_roman = "2-I";
                        break;
                    case 4:
                        sem_roman = "2-II";
                        break;
                    case 5:
                        sem_roman = "3-I";
                        break;
                    case 6:
                        sem_roman = "3-II";
                        break;
                    case 7:
                        sem_roman = "4-I";
                        break;
                    case 8:
                        sem_roman = "4-II";
                        break;
                    default:
                        sem_roman = " ";
                        break;
                }
            }
            else
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "I";
                        break;
                    case 2:
                        sem_roman = "II";
                        break;
                    case 3:
                        sem_roman = "III";
                        break;
                    case 4:
                        sem_roman = "IV";
                        break;
                    case 5:
                        sem_roman = "V";
                        break;
                    case 6:
                        sem_roman = "VI";
                        break;
                    case 7:
                        sem_roman = "VII";
                        break;
                    case 8:
                        sem_roman = "VIII";
                        break;
                    case 9:
                        sem_roman = "IX";
                        break;
                    case 10:
                        sem_roman = "X";
                        break;
                    default:
                        sem_roman = " ";
                        break;

                }
            }
        }
        return sem_roman;
    }
    public int GetSemester_AsNumber(int IpValue)
    {
        InsFlag = false;
        string strinssetting = "";
        string VarProcessValue = "";
        int GetSemesterAsNumber = 0;
        strinssetting = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName='Semester Display'";
        con_Inssetting.Close();
        con_Inssetting.Open();
        SqlCommand cmd_ins = new SqlCommand(strinssetting, con_Inssetting);
        SqlDataReader dr_ins;
        dr_ins = cmd_ins.ExecuteReader();
        while (dr_ins.Read())
        {
            if (dr_ins.HasRows == true)
            {
                if (dr_ins["LinkName"].ToString() == "Semester Display")
                {
                    InsFlag = true;
                }
                if (Convert.ToInt32(dr_ins["LinkValue"]) == 0)
                {
                    GetSemesterAsNumber = IpValue;
                }
                else if (Convert.ToInt32(dr_ins["LinkValue"]) == 1)
                {
                    VarProcessValue = Convert.ToString(IpValue).Trim();
                }

            }
        }

        return IpValue;
    }
    protected void FpExternal_SelectedIndexChanged(Object sender, EventArgs e)
    {


    }
    protected void FpExternal_CellClick(Object sender, EventArgs e)
    {
        int isval;
        isval = Convert.ToInt32(FpExternal.Sheets[0].Cells[0, 0].Value.ToString());

    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {

        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            LabelE.Visible = false;
            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            LabelE.Visible = false;
            TextBoxother.Visible = false;
            FpExternal.Visible = true;
            FpExternal.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
        FpExternal.CurrentPage = 0;
    }
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {

        try
        {
            if (TextBoxother.Text != "")
            {

                FpExternal.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();

            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    FpExternal.Visible = true;
                    TextBoxpage.Text = "";
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    FpExternal.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpExternal.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }
    public void convertgrade(string roll, string subj)
    {
        strexam = "Select subject_name,subject_code,total,result,cp,mark_entry.subject_no from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + examcode_fun + "  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";

        SqlCommand cmd_exam1 = new SqlCommand(strexam, con_convertgrade);
        con_convertgrade.Close();
        con_convertgrade.Open();

        dr_convert = cmd_exam1.ExecuteReader();
        while (dr_convert.Read())
        {

            funcsubname = dr_convert["subject_name"].ToString();
            funcsubno = dr_convert["subject_no"].ToString();
            funcsubcode = dr_convert["subject_code"].ToString();
            funcresult = dr_convert["result"].ToString();
            funccredit = dr_convert["cp"].ToString();
            mark = dr_convert["total"].ToString();

            string strgrade = "";
            if (dr_convert["total"].ToString() != string.Empty)
            {
                strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_convert["total"] + " between frange and trange";
            }
            else
            {
                strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
            }
            SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
            con_Grade.Close();
            con_Grade.Open();
            SqlDataReader dr_grade;
            dr_grade = cmd_grade.ExecuteReader();
            while (dr_grade.Read())
            {
                funcgrade = dr_grade["mark_grade"].ToString();

            }
        }
    }
    private string Calulat_GPA(string RollNo, string sem)
    {
        int Subno = 0;
        int jvalue = 0;
        string examcodeval = "";
        string gradestr = "";
        string ccva = "";
        string strgrade = "";
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = "";
        string graders = "";
        examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
        }
        else if (ccva == "True")
        {
            if (ChkOutgone.Checked == true)
            {
                strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            }
            else
            {
                strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
            }

        }
        SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
        con_subcrd.Close();
        con_subcrd.Open();
        SqlDataReader dr_subcrd;
        dr_subcrd = cmd_subcrd.ExecuteReader();
        while (dr_subcrd.Read())
        {
            if (dr_subcrd.HasRows)
            {

                if ((dr_subcrd["total"].ToString() != string.Empty))
                {
                    graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_subcrd["total"].ToString() + " between frange and trange";
                }
                else
                {
                    graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                }
                cmd = new SqlCommand(graders, con_Grade);
                con_Grade.Close();
                con_Grade.Open();
                SqlDataReader dr_grades;
                dr_grades = cmd.ExecuteReader();
                while (dr_grades.Read())
                {

                    if (dr_grades.HasRows)
                    {
                        strgrade = dr_grades["credit_points"].ToString();
                    }
                    creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    if (creditsum1 == 0)
                    {
                        creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    }
                    else
                    {
                        creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    }

                    if (gpacal1 == 0)
                    {
                        gpacal1 = Convert.ToDouble(strgrade) * creditval;
                    }
                    else
                    {
                        gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2);
        }


        return finalgpa1.ToString();
    }
    private string Calculete_CGPA(string RollNo, string semval)
    {
        int Subno = 0;
        int jvalue = 0;
        string examcodeval = "";
        string gradestr = "";
        string ccva = "";
        string strgrade = "";
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        int se = 0;
        string latsem = "";
        string syll_code = "";
        string strsubcrd = "";
        string latmode = "";
        for (jvalue = 1; jvalue <= Convert.ToInt32(semval); jvalue++)
        {
            syll_code = GetFunction("select distinct syll_code from syllabus_master where degree_code=" + degree_code + " and semester =" + jvalue + " and batch_year=" + batch_year + "");
            examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
            if (syll_code != "")
            {
                if (jvalue == Convert.ToInt32(semval))
                {
                    ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
                    if (ccva == "False")
                    {
                        strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                    }
                    else if (ccva == "True")
                    {
                        if (ChkOutgone.Checked == true)
                        {

                            strsubcrd = "Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
                        }
                        else
                        {
                            strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                        }

                    }

                }//'''''''
                else
                {
                    strsubcrd = "Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass')and exam_code in (select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and current_semester<=" + semdec + ")";
                }

            }
            SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
            con_subcrd.Close();
            con_subcrd.Open();
            SqlDataReader dr_subcrd;
            dr_subcrd = cmd_subcrd.ExecuteReader();
            while (dr_subcrd.Read())
            {
                if (dr_subcrd.HasRows)
                {
                    if ((dr_subcrd["total"].ToString() != "NULL") && (dr_subcrd["total"].ToString() != string.Empty))
                    {
                        string graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_subcrd["total"].ToString() + " between frange and trange";
                        cmd = new SqlCommand(graders, con_Grade);
                        con_Grade.Close();
                        con_Grade.Open();
                        SqlDataReader dr_grades;
                        dr_grades = cmd.ExecuteReader();
                        dr_grades.Read();

                        if (dr_grades.HasRows)
                        {
                            strgrade = dr_grades["credit_points"].ToString();
                        }
                    }
                    creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    if (creditsum1 == 0)
                    {
                        creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    }
                    else
                    {
                        creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                    }

                    if (gpacal1 == 0)
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                    }
                    else
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }
            if (creditsum1 != 0)
            {
                if (finalgpa1 == 0)
                {
                    finalgpa1 = Math.Round((gpacal1 / creditsum1), 2);
                }
                else
                {
                    finalgpa1 = finalgpa1 + Math.Round((gpacal1 / creditsum1), 2);
                }

            }
            creditsum1 = 0;
            gpacal1 = 0;
            creditval = 0;
            strgrade = "";
        }
        latmode = GetFunction("select mode from registration where roll_no='" + RollNo + "'");
        latsem = GetFunction("select min(semester) from subjectchooser where roll_no='" + RollNo + "'");
        int latsemes = 0;
        string calculate = "";
        if (latsem.Trim() != "" && latsem.Trim() != null)
        {
            if (Convert.ToInt32(semval) >= Convert.ToInt32(latsem))
            {
                for (se = Convert.ToInt32(latsem); se <= Convert.ToInt32(semval); se++)
                {
                    latsemes = latsemes + 1;
                }
            }
            if (Convert.ToInt32(latmode) == 1)
            {
                calculate = Math.Round((finalgpa1 / Convert.ToInt32(semval)), 2).ToString();
            }
            else
            {
                calculate = Math.Round((finalgpa1 / Convert.ToInt32(latsemes)), 2).ToString();
            }
        }
        return calculate;
    }
    private double cgpa(string RollNo, int semval)
    {
        string strgrade = "";
        string strcredit = "";
        string sem = "";
        int i = 0;
        int grcredit1 = 0;
        int gpa1 = 0;
        overallcredit = 0;
        sem = semval.ToString();
        int gpacal2 = 0;
        int gpacal = 0;
        string strsem = "";
        string mgrade = "";
        int gpa = 0;
        int grpoints = 0;
        int grcredit = 0;
        strsem = "select exam_system,first_year_nonsemester from ndegree where degree_code=" + degree_code + " and batch_year=" + batch_year + "";
        SqlCommand cmd_sem = new SqlCommand(strsem, con_sem);
        con_sem.Close();
        con_sem.Open();
        SqlDataReader dr_sem;
        dr_sem = cmd_sem.ExecuteReader();
        dr_sem.Read();
        string examsys = dr_sem["first_year_nonsemester"].ToString();
        if (examsys == "False")
        {
            for (int j = 0; j <= Convert.ToInt32(sem); j++)
            {

                IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + ""));

                string strresult = "";
                strresult = "Select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" + RollNo + "'";
                SqlCommand cmd_result = new SqlCommand(strresult, con_result);
                con_result.Close();
                con_result.Open();
                SqlDataReader dr_result;
                dr_result = cmd_result.ExecuteReader();
                dr_result.Read();
                if (dr_result.HasRows)
                {
                    if (dr_result["grade"].ToString() == "")
                    {
                        mgrade = "";
                    }
                    else
                    {
                        mgrade = dr_result["grade"].ToString();
                    }
                    if (mgrade == "")
                    {
                        mgrade = "-";
                    }
                    if (mgrade != "-")
                    {
                        //'--------------------------wuery for gradepoint
                        strgrade = "select credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + degree_code + " and batch_year='" + batch_year + "'";
                        SqlCommand cmd_grad = new SqlCommand(strgrade, con_Grade1);
                        con_Grade1.Close();
                        con_Grade1.Open();
                        SqlDataReader dr_grad;
                        dr_grad = cmd_grad.ExecuteReader();
                        dr_grad.Read();
                        if (dr_grad.HasRows)
                        {
                            if (dr_grad["credit_points"].ToString() != "")
                            {
                                grpoints = Convert.ToInt32(dr_grad["credit_points"].ToString());
                            }
                            else
                            {
                                grpoints = 0;
                            }
                        }

                    }
                    else //'------else of mgrade
                    {
                        grpoints = 0;
                    }
                    //'------------query for creditpoint
                    strcredit = "select credit_points from subject where subject_no= " + dr_result["subject_no"] + " ";
                    SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                    con_credit.Close();
                    con_credit.Open();
                    SqlDataReader dr_credit;
                    dr_credit = cmd_credit.ExecuteReader();
                    dr_credit.Read();
                    if (dr_credit.HasRows)
                    {
                        if (dr_credit["credit_points"].ToString() != "")
                        {
                            grcredit = Convert.ToInt32(dr_credit["credit_points"].ToString());
                            grcredit1 = grcredit1 + grcredit;
                        }
                    }
                    else
                    {
                        grcredit = 0;
                    }
                    gpa = grpoints * grcredit;
                    gpa1 = gpa1 + gpa;

                }
            }

            if (grcredit1 != 0)
            {
                gpacal = gpa1 / grcredit1;
            }
            else
            {
                gpacal = 0;
            }
            cgpa2 = Math.Round(Convert.ToDouble(gpacal), 2);
        }
        else//'-----------------------------else of examsys condn-----------------------
        {
            for (int j = 1; j <= Convert.ToInt32(sem); j++)
            {
                if (j == 2)
                {
                    gpa = 0;
                    grpoints = 0;
                    grcredit = 0;
                    gpa1 = 0;
                    grcredit1 = 0;
                    IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), j, Convert.ToInt32(batch_year));
                    string strresult = "";
                    strresult = " Select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" + RollNo + "'";
                    SqlCommand cmd_result = new SqlCommand(strresult, con_result);
                    con_result.Close();
                    con_result.Open();
                    SqlDataReader dr_result;
                    dr_result = cmd_result.ExecuteReader();
                    dr_result.Read();
                    if (dr_result.HasRows)
                    {
                        if (dr_result["grade"].ToString() == "")
                        {
                            mgrade = "";
                        }
                        else
                        {
                            mgrade = dr_result["grade"].ToString();
                        }
                        if (mgrade == "")
                        {
                            mgrade = "-";
                        }
                        if (mgrade != "-")
                        {
                            //'--------------------------query for gradepoint
                            strgrade = "select credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + degree_code + " and batch_year='" + batch_year + "'";
                            SqlCommand cmd_grad = new SqlCommand(strgrade, con_Grade1);
                            con_Grade1.Close();
                            con_Grade1.Open();
                            SqlDataReader dr_grad;
                            dr_grad = cmd_grad.ExecuteReader();
                            dr_grad.Read();
                            if (dr_grad.HasRows)
                            {
                                if (dr_grad["credit_points"].ToString() != "")
                                {
                                    grpoints = Convert.ToInt32(dr_grad["credit_points"].ToString());
                                }
                                else
                                {
                                    grpoints = 0;
                                }
                            }

                        }
                        else //'------else of mgrade
                        {
                            grpoints = 0;
                        }
                        //'------------query for creditpoint
                        strcredit = "select credit_points from subject where subject_no= " + dr_result["subject_no"] + " ";
                        SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                        con_credit.Close();
                        con_credit.Open();
                        SqlDataReader dr_credit;
                        dr_credit = cmd_credit.ExecuteReader();
                        dr_credit.Read();
                        if (dr_credit.HasRows)
                        {
                            if (dr_credit["credit_points"].ToString() != "")
                            {
                                grcredit = Convert.ToInt32(dr_credit["credit_points"].ToString());
                                grcredit1 = grcredit1 + grcredit;
                            }
                        }
                        else
                        {
                            grcredit = 0;
                        }
                        gpa = grpoints * grcredit;
                        gpa1 = gpa1 + gpa;

                    }
                }

                gpacal = gpa1 / grcredit1;
                gpacal2 = gpacal2 + gpacal;
            }//'-------------end loop
            int cgpa1 = 0;
            cgpa1 = gpacal2 / (Convert.ToInt32(sem) - 1);
            double cgpa2 = Math.Round(Convert.ToDouble(cgpa1), 2);
        }
        overallcredit = grcredit1;
        return overallcredit;
    }
    public string GetEarnedCreditoutgone(string RollNumber)
    {
        int EarnedCredit = 0;
        string syll_code = "";
        string new_rs = "";
        int ivalue = 0;
        string examcodeval = "";
        EarnedCredit = 0;

        for (ivalue = 1; ivalue <= Convert.ToInt32(ddlSemYr.SelectedValue.ToString()); ivalue++)
        {
            syll_code = GetFunction("select distinct syll_code from syllabus_master where degree_code=" + degree_code + " and semester =" + ivalue + " and batch_year=" + batch_year + "");
            new_rs = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNumber + "' and (result='Pass' or result='pass')and exam_code in (select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and current_semester<=" + ddlSemYr.SelectedValue.ToString() + ")";//'current_semester<=" + semdec +"
            SqlCommand cmd_rs = new SqlCommand(new_rs, con_rs);
            con_rs.Close();
            con_rs.Open();
            SqlDataReader dr_rs;
            dr_rs = cmd_rs.ExecuteReader();
            while (dr_rs.Read())
            {
                if (dr_rs.HasRows)
                {
                    if (dr_rs["credit_points"].ToString() != "")
                    {
                        EarnedCredit = EarnedCredit + Convert.ToInt32(dr_rs["credit_points"].ToString());
                    }
                }
            }
        }

        return EarnedCredit.ToString();
    }
    public string GetEarnedCredit(string RollNumber)
    {
        int EarnedCredit = 0;
        string syll_code = "";
        string new_rs = "";
        int ivalue = 0;
        string examcodeval = "";
        EarnedCredit = 0;
        if (semdec > 0)
        {
            for (ivalue = 1; ivalue <= semdec; ivalue++)
            {
                examcodeval = Get_UnivExamCode(Convert.ToInt32(degree_code), ivalue, Convert.ToInt32(batch_year)).ToString();

                new_rs = "Select Subject.credit_points from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcode_fun + "  and roll_no='" + RollNumber + "' and (result='Pass' or result='pass')";
                SqlCommand cdm_rs = new SqlCommand(new_rs, con_rs);
                con_rs.Close();
                con_rs.Open();

                SqlDataReader dr_rss;
                dr_rss = cdm_rs.ExecuteReader();
                while (dr_rss.Read())
                {
                    if (dr_rss.HasRows)
                    {
                        if (dr_rss["credit_points"].ToString() != "")
                        {
                            EarnedCredit = EarnedCredit + Convert.ToInt32(dr_rss["credit_points"].ToString());
                        }
                    }
                }
            }

        }
        return EarnedCredit.ToString();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void rdMark_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void rdGrade_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void tamilbutton_Click(object sender, EventArgs e)
    {

    }
    public string findroman(string sem)
    {
        string sem3 = "";
        if (sem == "1")
            sem3 = "I";
        else if (sem == "2")
            sem3 = "II";
        else if (sem == "3")
            sem3 = "III";
        else if (sem == "4")
            sem3 = "IV";
        else if (sem == "5")
            sem3 = "V";
        else if (sem == "6")
            sem3 = "VI";
        else if (sem == "7")
            sem3 = "VII";
        else if (sem == "8")
            sem3 = "VIII";
        else if (sem == "9")
            sem3 = "IX";
        else if (sem == "10")
            sem3 = "X";
        return sem3;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
        Boolean child_flag = false;
        int sec_index = 0, sem_index = 0;
        batch = ddlBatch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();
        semester = ddlSemYr.SelectedValue.ToString();
        degreecode = ddlBranch.SelectedValue.ToString();


        if (ddlSec.Text == "")
        {
            strsec = "";
        }
        else
        {
            if (ddlSec.SelectedItem.ToString() == "")
            {
                strsec = "";
            }
            else
            {
                strsec = " - " + ddlSec.SelectedItem.ToString();
            }
        }


        if (ddlSec.Enabled == false)
        {
            sec_index = -1;
        }
        else
        {
            sec_index = ddlSec.SelectedIndex;
        }

        if (ddlSemYr.Enabled == false)
        {
            sem_index = -1;
        }
        else
        {
            sem_index = ddlSemYr.SelectedIndex;
        }

        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + ddlMonth.SelectedIndex + "," + ddlYear.SelectedIndex;

        // first_btngo();
        btnGo_Click(sender, e);


        lblpages.Visible = true;
        ddlpage.Visible = true;
        string clmnheadrname = "";
        int total_clmn_count = FpExternal.Sheets[0].ColumnCount;
        Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "university_mark.aspx" + ":" + ddlBatch.SelectedItem.ToString() + " Batch - " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Consolidated Grade Sheet");


    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Boolean check_flag = false;
        int start_row = 0, temp = 0;
        DataSet dsprint = new DataSet();
        string view_footer = "", view_header = "", view_footer_text = "";

        has.Clear();
        has.Add("college_code", Session["collegecode"].ToString());
        has.Add("form_name", "university_mark.aspx");
        dsprint = daccess.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();

            lblnorec.Visible = false;
            if (view_header == "0")
            {

                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = false;
                }


                int start = 0, end = 0;
                if (ddlpage.SelectedIndex != (ddlpage.Items.Count - 1))
                {

                    for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                    {
                        if (check_flag == false && temp <= 4)
                        {
                            if (FpExternal.Sheets[0].Cells[i, 0].Text == "CONSOLIDATED GRADE SHEET")
                            {
                                temp++;
                            }
                            if (temp != 0)
                            {
                                temp++;
                            }
                            FpExternal.Sheets[0].Rows[i].Visible = true;

                        }
                        else
                        {
                            if (check_flag == false)
                            {
                                check_flag = true;
                                start_row = i;
                            }
                            FpExternal.Sheets[0].Rows[i].Visible = false;
                        }
                    }

                    start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + start_row;
                    end = start + 24;
                    if (end >= FpExternal.Sheets[0].RowCount - 32)
                    {
                        end = FpExternal.Sheets[0].RowCount - 32;
                    }
                }
                else
                {

                    start = FpExternal.Sheets[0].RowCount - 30;
                    end = FpExternal.Sheets[0].RowCount - 1;

                }
                int rowstart = FpExternal.Sheets[0].RowCount - Convert.ToInt32(start);
                int rowend = FpExternal.Sheets[0].RowCount - Convert.ToInt32(end);

                FpExternal.Sheets[0].Rows[start - 1].Border.BorderColorTop = Color.Black;
                for (int i = start - 1; i < end; i++)
                {

                    FpExternal.Sheets[0].Rows[i].Visible = true;
                }
                for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {

                    if (FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1")
                    {
                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                    }
                }


            }
            else if (view_header == "1")
            {
                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = false;
                }

                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    if (FpExternal.Sheets[0].Cells[i, 0].Text != "CONSOLIDATED GRADE SHEET")
                    {
                        FpExternal.Sheets[0].Rows[i].Visible = false;
                        if (check_flag == false)
                        {
                            check_flag = true;
                            start_row = i;
                        }
                    }
                    else
                    {
                        FpExternal.Sheets[0].Rows[i].Visible = true;
                    }
                }



                int start = Convert.ToInt32(ddlpage.SelectedValue.ToString()) + start_row;
                int end = start + 24;
                if (end >= FpExternal.Sheets[0].RowCount - 29)
                {
                    end = FpExternal.Sheets[0].RowCount - 29;
                }
                int rowstart = FpExternal.Sheets[0].RowCount - Convert.ToInt32(start);
                int rowend = FpExternal.Sheets[0].RowCount - Convert.ToInt32(end);
                for (int i = start - 1; i < end; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = true;
                }
                if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
                {
                    for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {

                        if (FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1")
                        {
                            FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                        }
                    }
                }
                else
                {
                    for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {

                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = false;
                }
                int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
                int end = start + 24;
                if (end >= FpExternal.Sheets[0].RowCount)
                {
                    end = FpExternal.Sheets[0].RowCount;
                }
                int rowstart = FpExternal.Sheets[0].RowCount - Convert.ToInt32(start);
                int rowend = FpExternal.Sheets[0].RowCount - Convert.ToInt32(end);
                for (int i = start - 1; i < end; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = true;
                }

                {
                    for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {
                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                    }
                }
            }
            if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
            {

                if (view_header == "1" || view_header == "0")
                {
                    for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {

                        if (FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Tag.ToString() == "1")
                        {
                            FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                        }
                    }
                }
                else
                {
                    for (int row_cnt = 0; row_cnt < FpExternal.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                    {
                        FpExternal.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                    }
                }

                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = true;
                }
                Double totalRows = 0;
                totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpExternal.Height = 335;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    FpExternal.Height = 100;
                }
                else
                {
                    FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                    FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
                if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    //  FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    CalculateTotalPages();
                }

                pnlrecordcount.Visible = true;
            }
            else
            {
                pnlrecordcount.Visible = false;
            }

            if (view_footer_text != "")
            {
                if (view_footer == "0")
                {
                    FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 1)].Visible = true;
                    FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 2)].Visible = true;
                    FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 3)].Visible = true;
                }
                else
                {
                    if (ddlpage.Text != "")
                    {
                        if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
                        {
                            FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 1)].Visible = false;
                            FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 2)].Visible = false;
                            FpExternal.Sheets[0].Rows[(FpExternal.Sheets[0].RowCount - 3)].Visible = false;
                        }
                    }
                }
            }

            FpExternal.Visible = true;
            btnxl.Visible = true;
            pnlrecordcount.Visible = true;
        }
        else
        {
            FpExternal.Visible = false;
            btnxl.Visible = false;
            pnlrecordcount.Visible = false;
            lblnorec.Visible = false;
            lblnorec.Text = "No Header and Footer setting Assigned";
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = "";
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "Consolidated Grade Sheet" + i;
                //FpExternal.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                FpExternal.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================
            }
            catch
            {
                i++;
                goto e;

            }
        }
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

    }
}



