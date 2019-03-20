using System;//-----------------modified on 24/2/12 and 07.05.12 for printmastersetting 11.
using System.Collections; //------------------ modified on 07.06.12 for mark conversion value by mythili
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;

public partial class CAM : System.Web.UI.Page
{
    string strsec = "";
    string code = "";
    string text = "";
    string exampresent = "";
    int flag = 0;
    string subjectscode;
    string atten;
    string strdayflag;
    string regularflag = "";
    string genderflag = "";
    string strorder = "";
    //string sections = "";
    //saravana start
    DAccess2 d2 = new DAccess2();

    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();

    #region
    DataTable dtable1 = new DataTable();
    DataTable dtable2 = new DataTable();
    DataTable fnaltab = new DataTable();
    DataRow dtrow1 = null;
    DataRow dtrow2 = null;
    #endregion

    int i = 0;
    int pass_count, fail_count;
    int tot_cal_stu;
    double avg_pass, class_avg, no_of_absent;
    string st_avg_pass;
    int pass_fail_tot_count;
    string sections = "";
    string batchyear = "";
    string semester = "";
    string DegCode = "";
    string bindstud = "";
    int in_of;

    //string strsec = "";
    string mrkcriteria = "";
    int sub_count;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds7 = new DataSet();

    string marks_per;

    //saravana end
    //'--------------------new my
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    DataSet ds_load = new DataSet();
    //'-----------new start for print master on 10.04.12
    DataSet dsprint = new DataSet();
    Boolean PrintMaster = false;

    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;

    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string address3 = "";
    string pincode = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string form_heading_name = "";
    string batch_degree_branch = "";
    int chk_secnd_clmn = 0;
    int right_logo_clmn = 0;
    //'--------------------------
    string txt_mrk_cnv_value = ""; // on 07.06.12
    //----------------------------------
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    //[Serializable()]
    //public class MyImg1 : ImageCellType
    //{
    //    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    //    {
    //        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
    //        img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img.Width = Unit.Percentage(60);
    //        img.Height = Unit.Percentage(70);
    //        return img;

    //    }
    //}
    //public class MyImg2 : ImageCellType
    //{

    //    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    //    {
    //        //''------------clg left logo
    //        System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
    //        img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img1.Width = Unit.Percentage(60);
    //        img1.Height = Unit.Percentage(70);
    //        return img1;

    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        // FpSpread1.Width = 600;


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

            if (!Page.IsPostBack)
            {
                //FpSpread1.Sheets[0].AutoPostBack = true;//
                //FpSpread1.Sheets[0].RowHeader.Visible = false;//

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                CheckBox1.Visible = false;
                CheckBox1.Checked = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;

                btnExcel.Visible = false;

                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();




                //'------------------------------------load the clg information



                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
                    SqlCommand collegecmd = new SqlCommand(college, con);
                    SqlDataReader collegename;
                    con.Close();
                    con.Open();
                    collegename = collegecmd.ExecuteReader();
                    if (collegename.HasRows)
                    {
                        while (collegename.Read())
                        {
                            collnamenew1 = collegename["collname"].ToString();
                            address1 = collegename["address1"].ToString();
                            address2 = collegename["address2"].ToString();
                            district = collegename["district"].ToString();
                            address = address1 + "- " + address2 + "- " + district;
                            Phoneno = collegename["phoneno"].ToString();
                            Faxno = collegename["faxno"].ToString();
                            phnfax = "Phone: " + " " + Phoneno + " " + "Fax: " + " " + Faxno;
                            email = "E-Mail: " + collegename["email"].ToString() + " " + "Web Site: " + collegename["website"].ToString();
                        }
                    }
                    con.Close();
                }

                //'---------------------------------------------load theclg logo photo-------------------------------------


                gview.Visible = false;
                Button1.Visible = false;
                if (Session["usercode"] != "")
                {
                    string Master1 = "";
                    Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";

                    mycon3.Open();
                    SqlDataReader mtrdr;

                    SqlCommand mtcmd = new SqlCommand(Master1, mycon3);
                    mtrdr = mtcmd.ExecuteReader();
                    strdayflag = "";
                    while (mtrdr.Read())
                    {
                        if (mtrdr.HasRows == true)
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
                                strdayflag = " and (Stud_Type='Day Scholar'";
                            }
                            if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
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
                    mycon.Close();
                }

                txt_mrk_cnv_value = txtConvert_Value.Text.ToString();
                if (Request.QueryString["val"] != null)
                {
                    string get_pageload_value = Request.QueryString["val"];
                    if (get_pageload_value.ToString() != null)
                    {
                        string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                        string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val

                        PrintMaster = true;//set the boolean true for printmaster
                        bindbatch();
                        ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());
                        binddegree();
                        ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());

                        if (ddlDegree.Text != "")
                        {
                            bindbranch();
                            ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                        }
                        else
                        {
                            lblnorec.Text = "Give degree rights to the staff";
                            lblnorec.Visible = true;
                        }

                        bindsem();
                        ddlSem.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                        bindsec();
                        ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                        GetSubject();
                        ddlSubject.SelectedIndex = Convert.ToInt32(spl_pageload_val[6].ToString());

                        con.Close();
                        con.Open();
                        string SyllabusYr;
                        string SyllabusQry;
                        SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
                        SyllabusYr = GetFunction(SyllabusQry.ToString());

                        string Sqlstr;
                        Sqlstr = "";
                        if (SyllabusQry != "" && SyllabusQry != null)
                        {

                            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
                            DataSet titles = new DataSet();
                            con.Close();
                            con.Open();
                            sqlAdapter1.Fill(titles);

                            ddltest.DataSource = titles;
                            ddltest.DataValueField = "Criteria_No";
                            ddltest.DataTextField = "Criteria";
                            ddltest.DataBind();
                            ddltest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "-1"));

                        }
                        ////@@@@@@@@
                        ddltest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                        if (spl_load_val.GetUpperBound(0) > 0)
                        {
                            CheckBox1.Checked = true;
                            fromtext.Text = spl_load_val[1].ToString();
                            Totext.Text = spl_load_val[2].ToString();
                            CheckBox1.Visible = true;
                            fromtext.Visible = true;
                            Totext.Visible = true;
                            Panel4.Visible = true;
                        }

                        ddltest_SelectedIndexChanged(sender, e);
                        func_Print_Master_Setting();
                        func_header();
                        function_footer();


                    }
                }
                else
                {
                    bindbatch();
                    binddegree();

                    if (ddlDegree.Text != "")
                    {
                        bindbranch();
                    }
                    else
                    {
                        lblnorec.Text = "Give degree rights to the staff";
                        lblnorec.Visible = true;
                    }

                    bindsem();
                    bindsec();
                    GetSubject();
                }
            }//end brace for ispostback
        }//end for try
        catch
        {

        }
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = d2.select_method_wo_parameter("bind_batch", "sp");
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

        ds_load = d2.select_method("bind_branch", hat, "sp");
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
        ds_load = d2.select_method("bind_degree", hat, "sp");
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
        ds_load = d2.select_method("bind_sec", hat, "sp");
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

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        //Control cntUpdateBtn = FpSpread1.FindControl("Update");
        //Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        //Control cntCopyBtn = FpSpread1.FindControl("Copy");
        //Control cntCutBtn = FpSpread1.FindControl("Clear");
        //Control cntPasteBtn = FpSpread1.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");//
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");//
        // Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        Control cntPageNextBtn = gview.FindControl("Next");
        Control cntPagePreviousBtn = gview.FindControl("Prev");

        if ((cntPageNextBtn != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);


            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            ////tc = (TableCell)cntPagePrintBtn.Parent;
            ////tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    public void GetTest()
    {
        dtable1.Clear();
        dtable1.Columns.Add("Sl.No");
        dtable1.Columns.Add("Roll No");
        dtable1.Columns.Add("Reg No");
        dtable1.Columns.Add("Student Name");
        string colName = "";

        dtrow1 = dtable1.NewRow();
        dtrow1["Sl.No"] = "Sl.No";
        dtrow1["Roll No"] = "Roll No";
        dtrow1["Reg No"] = "Reg No";
        dtrow1["Student Name"] = "Student Name";
        dtable1.Rows.Add(dtrow1);

        DataSet dsss = new DataSet();
        ArrayList alpasscount = new ArrayList();
        DataView dvbindmark = new DataView();
        txt_mrk_cnv_value = txtConvert_Value.Text.ToString();
        try
        {

            if ((ddlDegree.Text != "") && (ddlBranch.Text != ""))
            {
                //FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Batch Year:  " + ddlBatch.SelectedValue.ToString() + " " + "Course: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
            }
            //FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Semester: " + ddlSem.SelectedValue.ToString() + " " + "Section: " + ddlSec.SelectedValue.ToString();
            if (ddltest.Text != "")
            {
                //FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Text = "Subject: "+ ddlSubject.SelectedItem.Text.ToString() + "  ,     Test: "+  ddltest.SelectedItem.ToString();

            }

            //FpSpread1.Sheets[0].ColumnCount = 5;//
            string strsec3 = "";
            sections = ddlSec.SelectedValue.ToString();
            if (!Page.IsPostBack == false)
            {
                strsec = "";
                if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = "";
                    strsec3 = "";
                }
                else
                {
                    strsec = " and exam_type.sections='" + sections.ToString() + "'";
                    strsec3 = " and sections='" + sections.ToString() + "'";
                }
            }

            con.Close();
            if (CheckBox1.Checked != true)
            {
                con.Open();
                string SyllabusYr;
                string SyllabusQry;
                if (ddltest.SelectedItem.ToString() == "ALL")
                {
                    //FpSpread1.SaveChanges();//
                    SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
                    SyllabusYr = GetFunction(SyllabusQry);
                    string Sqlstr;
                    Sqlstr = "";
                    if (SyllabusQry != null && SyllabusQry != "")
                    {
                        Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                        SqlCommand cmd = new SqlCommand(Sqlstr, mycon);
                        mycon.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        filteration();
                        string filterwithsection = "and EXAM_TYPE.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and EXAM_TYPE.sections='" + sections.ToString() + "' and EXAM_TYPE.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + strorder + " ,EXAM_TYPE.exam_code ";
                        string filterwithoutsection = "and EXAM_TYPE.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and EXAM_TYPE.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and registration.RollNo_Flag<>0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + strorder + " ,EXAM_TYPE.exam_code ";
                        hat.Clear();
                        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
                        hat.Add("strsec", sections.ToString());
                        hat.Add("subjectno", ddlSubject.SelectedValue.ToString());
                        hat.Add("filterwithsection", filterwithsection.ToString());
                        hat.Add("filterwithoutsection", filterwithoutsection.ToString());

                        ds2 = d2.select_method("SELECT _ALL_STUDENT_TEST_SUBJECT_WISE_REPORTS_DETAILS", hat, "sp");
                        dsss = ds2;
                        hat.Clear();
                        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
                        hat.Add("strsec", sections.ToString());
                        hat.Add("subjectno", ddlSubject.SelectedValue.ToString());
                        ds1 = d2.select_method("SELECT_ALL_TEST_IN_SUBJECT", hat, "sp");
                        int ds1count = ds1.Tables[0].Rows.Count - 1;
                        if (ds1.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                int slno = 0;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    dtrow1 = dtable1.NewRow();
                                    slno++;

                                    dtrow1["Sl.No"] = slno.ToString();
                                    dtrow1["Roll No"] = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                                    dtrow1["Reg No"] = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                                    dtrow1["Student Name"] = ds.Tables[0].Rows[i]["stud_name"].ToString();
                                    dtable1.Rows.Add(dtrow1);
                                }
                            }
                        }
                        int col_count = 0;
                        if (ds1.Tables[0].Rows.Count != 0)
                        {
                            dtable2.Columns.Add("Temp");
                            dtrow2 = dtable2.NewRow();
                            dtable2.Rows.Add(dtrow2);
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                string totmarkvalue = "";
                                if (txtConvert_Value.Text == "" || txtConvert_Value.Text == null)
                                {
                                    totmarkvalue = "(" + ds1.Tables[0].Rows[i]["max_mark"].ToString() + ")";
                                }
                                else
                                {
                                    totmarkvalue = "(" + txtConvert_Value.Text.ToString() + ")";
                                }


                                colName = (ds1.Tables[0].Rows[i]["criteria"].ToString() + totmarkvalue);
                                textpass = new System.Text.StringBuilder(colName);
                                AddTableColumn(dtable2, textpass);
                                //dtable2.Columns.Add("" + colName + "");
                                dtable2.Rows[0][colName] = colName;

                            }
                            dtable2.Columns.Remove("Temp");
                        }
                        int stud_count = 0;



                        tot_cal_stu = ds.Tables[0].Rows.Count;
                        int present_cntstudent = 0;
                        if (ds2.Tables[0].Rows.Count != 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                dtrow2 = dtable2.NewRow();
                                string dsroll = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                {
                                    if (stud_count < ds2.Tables[0].Rows.Count)
                                    {
                                        marks_per = ds2.Tables[0].Rows[stud_count]["marks_obtained"].ToString();

                                        //double checkmark = Convert.ToDouble(marks_per);
                                        //if (checkmark >= 0 || checkmark == -2 || checkmark == -3)
                                        //{
                                        //    if (alpasscount.Contains(dsroll) != true)
                                        //    {
                                        //        alpasscount.Add(dsroll);
                                        //    }
                                        //}

                                        if (ds.Tables[0].Rows[i]["Roll_No"].ToString() == ds2.Tables[0].Rows[stud_count]["roll_no"].ToString())
                                        {
                                            string ss = ds2.Tables[0].Rows[stud_count]["roll_no"].ToString();
                                            marks_per = ds2.Tables[0].Rows[stud_count]["marks_obtained"].ToString();
                                            if (Convert.ToDouble((marks_per)) < 0)
                                            {
                                                switch (marks_per)
                                                {
                                                    case "-1":
                                                        marks_per = "AAA";
                                                        break;
                                                    case "-2":
                                                        marks_per = "EL";
                                                        break;
                                                    case "-3":
                                                        marks_per = "EOD";
                                                        break;
                                                    case "-4":
                                                        marks_per = "ML";
                                                        break;
                                                    case "-5":
                                                        marks_per = "SOD";
                                                        break;
                                                    case "-6":
                                                        marks_per = "NSS";
                                                        break;
                                                    case "-7":
                                                        marks_per = "NJ";
                                                        break;
                                                    case "-8":
                                                        marks_per = "S";
                                                        break;
                                                    case "-9":
                                                        marks_per = "L";
                                                        break;
                                                    case "-10":
                                                        marks_per = "NCC";
                                                        break;
                                                    case "-11":
                                                        marks_per = "HS";
                                                        break;
                                                    case "-12":
                                                        marks_per = "PP";
                                                        break;
                                                    case "-13":
                                                        marks_per = "SYOD";
                                                        break;
                                                    case "-14":
                                                        marks_per = "COD";
                                                        break;
                                                    case "-15":
                                                        marks_per = "OOD";
                                                        break;
                                                    case "-16":
                                                        marks_per = "OD";
                                                        break;
                                                    case "-17":
                                                        marks_per = "LA";
                                                        break;
                                                    //***************Added By subburaj 21.08.2014*********//
                                                    case "-18":
                                                        marks_per = "RAA";
                                                        break;
                                                    //***********End************************//
                                                }
                                                dtrow2[j] = marks_per.ToString();
                                            }
                                            else
                                            {
                                                if ((txt_mrk_cnv_value.ToString() != "0") && (txt_mrk_cnv_value.ToString() != string.Empty))
                                                {
                                                    dtrow2[j] = ((Convert.ToDouble(marks_per.ToString()) * Convert.ToDouble(txt_mrk_cnv_value.ToString())) / Convert.ToDouble(ds2.Tables[0].Rows[stud_count]["max_mark"].ToString())).ToString();
                                                }
                                                else //  if(txt_mrk_cnv_value.ToString() == string.Empty)
                                                {
                                                    dtrow2[j] = marks_per.ToString();
                                                }
                                            }
                                            string min_marks = ds2.Tables[0].Rows[stud_count]["min_mark"].ToString();
                                            // 
                                            if (min_marks != "")
                                            {
                                            }
                                            else
                                            {
                                                min_marks = "0";
                                            }
                                            if (double.Parse(ds2.Tables[0].Rows[stud_count]["marks_obtained"].ToString()) < double.Parse(min_marks.ToString()))
                                            {
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].ForeColor = Color.Red;
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Name = "Book Antiqua";
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Size = FontUnit.Medium;
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                        else
                                        {
                                            if (marks_per == "")
                                            {
                                                marks_per = "-";

                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].Text = marks_per.ToString();
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].ForeColor = Color.Red;
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Name = "Book Antiqua";
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Size = FontUnit.Medium;
                                                ////FpSpread1.Sheets[0].Cells[i, 5 + j].HorizontalAlign = HorizontalAlign.Center;

                                                dtrow2[j] = marks_per.ToString();
                                            }
                                            else
                                            {
                                                // FpSpread1.Sheets[0].Cells[i, 4 + j].Text = marks_per.ToString();
                                                marks_per = "0";
                                            }
                                            if (stud_count < ds2.Tables[0].Rows.Count)
                                            {
                                                stud_count--;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        marks_per = "-";

                                        ////FpSpread1.Sheets[0].Cells[i, 5 + j].Text = marks_per.ToString();
                                        ////FpSpread1.Sheets[0].Cells[i, 5 + j].ForeColor = Color.Red;
                                        ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Name = "Book Antiqua";
                                        ////FpSpread1.Sheets[0].Cells[i, 5 + j].Font.Size = FontUnit.Medium;
                                        ////FpSpread1.Sheets[0].Cells[i, 5 + j].HorizontalAlign = HorizontalAlign.Center;

                                        dtrow2[j] = marks_per.ToString();
                                    }
                                    stud_count++;
                                }
                                dtable2.Rows.Add(dtrow2);
                            }
                            fnaltab = MergeTablesByIndex(dtable1, dtable2);
                        }
                        if (ds2.Tables[0].Rows.Count != 0)
                        {
                            //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;//
                            dtrow2 = fnaltab.NewRow();
                            dtrow2["Sl.No"] = "Average";
                        }
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {

                            hat.Clear();
                            hat.Add("exam_code", ds1.Tables[0].Rows[i]["exam_code"].ToString());

                            //check present count...
                            ds2.Tables[0].DefaultView.RowFilter = "exam_code='" + ds1.Tables[0].Rows[i]["exam_code"].ToString() + "'";
                            dvbindmark = ds2.Tables[0].DefaultView;
                            int stcount = 0;
                            for (int b = 0; b < dvbindmark.Count; b++)
                            {
                                double checkmark = Convert.ToDouble(dvbindmark[b]["marks_obtained"].ToString());
                                if (checkmark >= 0 || checkmark == -2 || checkmark == -3)
                                {
                                    stcount++;

                                }

                            }
                            ds5 = d2.select_method("SELECT_SUBJECT_WISE_EXAM_CODE_TOTAL", hat, "sp");

                            if (ds5.Tables[0].Rows.Count != 0)
                            {
                                double.TryParse(Convert.ToString(ds5.Tables[0].Rows[0]["SUM"]),out class_avg) ;//
                                class_avg = class_avg / stcount;
                                class_avg = Math.Round(class_avg, 2);
                                if (Convert.ToString(class_avg) == "NaN")
                                {
                                    dtrow2[i + 4] = "-";
                                }
                                else
                                {
                                    dtrow2[i + 4] = class_avg.ToString();
                                }
                            }
                        }
                        fnaltab.Rows.Add(dtrow2);
                        gview.Visible = true;
                        Button1.Visible = true;
                        gview.Visible = true;

                    }
                }
                else
                {
                    filteration();
                    string filterwithsection = "e.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and e.sections='" + sections.ToString() + "' and e.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and e.exam_code = r.exam_code And registration.roll_no = r.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + strorder + " ";
                    string filterwithoutsection = "e.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and e.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and e.exam_code = r.exam_code And registration.roll_no = r.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + strorder + "";
                    int stud_count = 0;
                    hat.Clear();
                    hat.Add("criteria_no", ddltest.SelectedItem.Value.ToString());
                    hat.Add("subjectno", ddlSubject.SelectedValue.ToString());
                    hat.Add("strsec", sections.ToString());
                    hat.Add("filterwithsection", filterwithsection.ToString());
                    hat.Add("filterwithoutsection", filterwithoutsection.ToString());

                    ds6 = d2.select_method("SELECT_ALL_STUDENT_ONE_TEST", hat, "sp");

                    if (ds6.Tables[0].Rows.Count != 0)
                    {
                        tot_cal_stu = ds.Tables[0].Rows.Count;
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            int slno = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                slno++;

                                dtrow1 = dtable1.NewRow();
                                dtrow1["Sl.No"] = slno.ToString();
                                dtrow1["Roll No"] = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                                dtrow1["Reg No"] = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                                dtrow1["Student Name"] = ds.Tables[0].Rows[i]["stud_name"].ToString();
                                dtable1.Rows.Add(dtrow1);
                            }
                        }

                        hat.Clear();
                        hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
                        hat.Add("strsec", sections.ToString());
                        hat.Add("subjectno", ddlSubject.SelectedValue.ToString());
                        ds1 = d2.select_method("SELECT_ALL_TEST_IN_SUBJECT", hat, "sp");
                        int col_count = 0;

                        Hashtable colhas = new Hashtable();
                        if (ds1.Tables[0].Rows.Count != 0)
                        {
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                if (ds6.Tables[0].Rows[0]["criteria_no"].ToString() == ds1.Tables[0].Rows[i]["criteria_no"].ToString())
                                {

                                    ////FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                    ////col_count = FpSpread1.Sheets[0].ColumnCount;
                                    ////FpSpread1.Sheets[0].Columns[col_count - 1].Width = 250;
                                    string totmarkvalue = "";
                                    if (txtConvert_Value.Text == "" || txtConvert_Value.Text == null)
                                    {
                                        totmarkvalue = "(" + ds1.Tables[0].Rows[i]["max_mark"].ToString() + ")";
                                    }
                                    else
                                    {
                                        totmarkvalue = "(" + txtConvert_Value.Text.ToString() + ")";
                                    }
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Text = (ds1.Tables[0].Rows[i]["criteria"].ToString() + totmarkvalue);
                                    ////// FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Text = (ds1.Tables[0].Rows[i]["criteria"].ToString() + "(" + ds1.Tables[0].Rows[i]["max_mark"].ToString() + ")");
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Font.Bold = true;
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Font.Size = FontUnit.Medium;
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Font.Name = "Book Antiqua";
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].HorizontalAlign = HorizontalAlign.Center;
                                    ////FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Border.BorderColorTop = Color.Black;

                                    colName = ds1.Tables[0].Rows[i]["criteria"].ToString() + totmarkvalue;
                                    dtable2.Columns.Add("" + colName + "");//
                                    colhas.Add(i, colName);
                                }
                            }
                        }

                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 8, 1);
                        //MyImg2 mi4 = new MyImg2();
                        //mi4.ImageUrl = "Handler/Handler5.ashx?";
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].CellType = mi4;
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
                        //FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 120;
                        // FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                        ////FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                        ////FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Result";

                        dtable2.Columns.Add("Result");
                        colhas.Add(colhas.Count, "Result");
                        dtrow2 = dtable2.NewRow();
                        dtable2.Rows.Add(dtrow2);
                        for (int col = 0; col < dtable2.Columns.Count; col++)
                        {
                            dtable2.Rows[0][col] = Convert.ToString(colhas[col]);
                        }

                        string result = "";
                        int totalpasscount = 0;
                        int totalfailcount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dtrow2 = dtable2.NewRow();
                            if (ds6.Tables[0].Rows.Count > stud_count)
                            {
                                if (ds.Tables[0].Rows[i]["roll_no"].ToString() == ds6.Tables[0].Rows[stud_count]["roll_no"].ToString())
                                {
                                    marks_per = ds6.Tables[0].Rows[stud_count]["marks_obtained"].ToString();
                                    if (double.Parse(ds6.Tables[0].Rows[stud_count]["marks_obtained"].ToString()) < 0)
                                    {
                                        switch (marks_per)
                                        {
                                            case "-1":
                                                marks_per = "AAA";
                                                break;
                                            case "-2":
                                                marks_per = "EL";
                                                break;
                                            case "-3":
                                                marks_per = "EOD";
                                                break;
                                            case "-4":
                                                marks_per = "ML";
                                                break;
                                            case "-5":
                                                marks_per = "SOD";
                                                break;
                                            case "-6":
                                                marks_per = "NSS";
                                                break;
                                            case "-7":
                                                marks_per = "NJ";
                                                break;
                                            case "-8":
                                                marks_per = "S";
                                                break;
                                            case "-9":
                                                marks_per = "L";
                                                break;
                                            case "-10":
                                                marks_per = "NCC";
                                                break;
                                            case "-11":
                                                marks_per = "HS";
                                                break;
                                            case "-12":
                                                marks_per = "PP";
                                                break;
                                            case "-13":
                                                marks_per = "SYOD";
                                                break;
                                            case "-14":
                                                marks_per = "COD";
                                                break;
                                            case "-15":
                                                marks_per = "OOD";
                                                break;
                                            case "-16":
                                                marks_per = "OD";
                                                break;
                                            case "-17":
                                                marks_per = "LA";
                                                break;
                                            //***************Added By subburaj 21.08.2014*********//
                                            case "-18":
                                                marks_per = "RAA";
                                                break;
                                            //******END***************//
                                        }
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();
                                        ////FpSpread1.Sheets[0].Cells[i, 5].ForeColor = Color.Red;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                        dtrow2[colName] = marks_per.ToString();
                                        result = "Fail";
                                        totalfailcount++;
                                    }
                                    else
                                    {
                                        //based on mark conversion value display the mark 07.06.12 (mythli)
                                        if ((txt_mrk_cnv_value.ToString() != "0") && (txt_mrk_cnv_value.ToString() != string.Empty))
                                        {
                                            double mimark = Convert.ToDouble(ds6.Tables[0].Rows[stud_count]["min_mark"].ToString());
                                            double chmark = 0;
                                            if (marks_per != "" && double.Parse(marks_per) > 0)
                                            {
                                                chmark = double.Parse(marks_per);
                                            }
                                            else
                                            {
                                                chmark = 0;
                                            }
                                            if (chmark >= mimark)
                                            {
                                                result = "Pass";
                                                totalpasscount++;
                                            }
                                            else
                                            {
                                                result = "Fail";
                                                totalfailcount++;
                                            }
                                            ////FpSpread1.Sheets[0].Cells[i, 5].Text = ((Convert.ToDouble(marks_per.ToString()) * Convert.ToDouble(txt_mrk_cnv_value.ToString())) / Convert.ToDouble(ds6.Tables[0].Rows[stud_count]["max_mark"].ToString())).ToString(); //divide the mark by convert value
                                            ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                            dtrow2[colName] = ((Convert.ToDouble(marks_per.ToString()) * Convert.ToDouble(txt_mrk_cnv_value.ToString())) / Convert.ToDouble(ds6.Tables[0].Rows[stud_count]["max_mark"].ToString())).ToString();
                                        }
                                        else //if (txt_mrk_cnv_value.ToString() == string.Empty)
                                        {
                                            if (double.Parse(ds6.Tables[0].Rows[stud_count]["marks_obtained"].ToString()) < double.Parse(ds6.Tables[0].Rows[stud_count]["min_mark"].ToString()))
                                            {
                                                ////FpSpread1.Sheets[0].Cells[i, 5].ForeColor = Color.Red;
                                                ////FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                                                ////FpSpread1.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                                                ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;
                                                ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();

                                                dtrow2[colName] = marks_per.ToString();

                                                result = "Fail";
                                                totalfailcount++;
                                            }
                                            else
                                            {
                                                ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();
                                                ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                                dtrow2[colName] = marks_per.ToString();//
                                                result = "Pass";
                                                totalpasscount++;
                                            }

                                        }
                                    }

                                    stud_count++;
                                }
                                else
                                {
                                    if (marks_per == "" || marks_per == null)
                                    {
                                        marks_per = "-";

                                        ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();
                                        ////FpSpread1.Sheets[0].Cells[i, 5].ForeColor = Color.Red;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                        dtrow2[colName] = marks_per.ToString();//
                                        result = "Fail";
                                        totalfailcount++;
                                        // stud_count--;
                                    }
                                    else
                                    {
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();
                                        ////FpSpread1.Sheets[0].Cells[i, 5].ForeColor = Color.Red;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                                        ////FpSpread1.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                                        ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                        dtrow2[colName] = marks_per.ToString();//
                                        result = "Fail";
                                        totalfailcount++;
                                    }

                                }

                                //FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = result;//
                                dtrow2["result"] = result;
                            }
                            else
                            {
                                marks_per = "-";

                                ////FpSpread1.Sheets[0].Cells[i, 5].Text = marks_per.ToString();
                                ////FpSpread1.Sheets[0].Cells[i, 5].ForeColor = Color.Red;
                                ////FpSpread1.Sheets[0].Cells[i, 5].Font.Name = "Book Antiqua";
                                ////FpSpread1.Sheets[0].Cells[i, 5].Font.Size = FontUnit.Medium;
                                ////FpSpread1.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;

                                dtrow2[colName] = marks_per.ToString();//
                            }
                            dtable2.Rows.Add(dtrow2);
                        }

                        fnaltab = MergeTablesByIndex(dtable1, dtable2);

                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            if (ds6.Tables[0].Rows[0]["criteria_no"].ToString() == ds1.Tables[0].Rows[i]["criteria_no"].ToString())
                            {
                                hat.Clear();
                                hat.Add("exam_code", ds1.Tables[0].Rows[i]["exam_code"].ToString());

                                //ds6.Tables[0].DefaultView.RowFilter = "exam_code='" + ds1.Tables[0].Rows[i]["exam_code"].ToString() + "'";
                                //dvbindmark = ds6.Tables[0].DefaultView;
                                int stcount = 0;
                                for (int b = 0; b < ds6.Tables[0].Rows.Count; b++)
                                {
                                    double checkmark = Convert.ToDouble(ds6.Tables[0].Rows[b]["marks_obtained"].ToString());
                                    if (checkmark >= 0 || checkmark == -2 || checkmark == -3)
                                    {
                                        stcount++;
                                    }

                                }


                                ds5 = d2.select_method("SELECT_SUBJECT_WISE_EXAM_CODE_TOTAL", hat, "sp");

                                if (ds5.Tables[0].Rows.Count != 0)
                                {
                                    // class_avg = double.Parse(ds5.Tables[0].Rows[0]["SUM"].ToString()) / tot_cal_stu;
                                    if (txtConvert_Value.Text != "")
                                    {
                                        class_avg = double.Parse(ds5.Tables[0].Rows[0]["SUM"].ToString()) / stcount;
                                    }
                                    else
                                    {
                                        class_avg = (double.Parse(ds5.Tables[0].Rows[0]["SUM"].ToString()) / stcount);
                                    }
                                    class_avg = Math.Round(class_avg, 2);

                                    dtrow2 = fnaltab.NewRow();

                                    ////FpSpread1.Sheets[0].RowCount++;

                                    dtrow2["Sl.No"] = "Pass Count";
                                    dtrow2[colName] = totalpasscount.ToString();
                                    fnaltab.Rows.Add(dtrow2);

                                    dtrow2 = fnaltab.NewRow();
                                    dtrow2["Sl.No"] = "Fail Count";
                                    dtrow2[colName] = totalfailcount.ToString();
                                    fnaltab.Rows.Add(dtrow2);


                                    dtrow2 = fnaltab.NewRow();
                                    dtrow2["Sl.No"] = "Average";
                                    dtrow2[colName] = class_avg.ToString();
                                    fnaltab.Rows.Add(dtrow2);
                                }
                            }
                        }
                    }
                }
            }

            if (CheckBox1.Checked == true)
            {

                int col_count = 0;
                //FpSpread1.Sheets[0].RowCount = 0;//
                filteration();
                string filterwithsection = "exam_type.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and exam_type.sections='" + sections.ToString() + "' and exam_type.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and exam_type.exam_code = result.exam_code and marks_obtained between '" + int.Parse((fromtext.Text).ToString()) + "' and '" + int.Parse((Totext.Text).ToString()) + "' And registration.roll_no = result.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR'  and criteriaforinternal.criteria_no=exam_type.criteria_no " + strorder + " ";
                string filterwithoutsection = "exam_type.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and exam_type.subject_no='" + ddlSubject.SelectedValue.ToString() + "' and exam_type.exam_code = result.exam_code and marks_obtained between '" + int.Parse((fromtext.Text).ToString()) + "' and '" + int.Parse((Totext.Text).ToString()) + "' And registration.roll_no = result.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR'  and criteriaforinternal.criteria_no=exam_type.criteria_no " + strorder + " ";

                hat.Clear();
                hat.Add("filterwithsection", filterwithsection.ToString());
                hat.Add("filterwithoutsection", filterwithoutsection.ToString());
                hat.Add("criteria_no", ddltest.SelectedItem.Value.ToString());
                hat.Add("strsec", sections.ToString());
                hat.Add("subjectno", ddlSubject.SelectedValue.ToString());
                if (fromtext.Text != "")
                {
                    hat.Add("marks1", int.Parse((fromtext.Text).ToString()));
                }
                else
                {
                    hat.Add("marks1", 0);
                }
                if (Totext.Text != "")
                {
                    hat.Add("marks2", int.Parse((Totext.Text).ToString()));
                }
                else
                {
                    hat.Add("marks2", 0);
                }
                ds7 = d2.select_method("SELECT_STUDENT_RANGE_IN_TEST", hat, "sp");

                if (ds7.Tables[0].Rows.Count != 0)
                {
                    //gowthman 02Aug2013 FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, col_count - 1].Text = (ds7.Tables[0].Rows[0]["criteria"].ToString() + "(" + ds7.Tables[0].Rows[0]["criteria"].ToString()+")");

                    colName = (ds7.Tables[0].Rows[0]["criteria"].ToString() + "(" + ds7.Tables[0].Rows[0]["max_mark"].ToString() + ")");
                    dtable1.Columns.Add("" + colName + "");
                    dtable1.Rows[0][colName] = colName;

                    int slno = 0;
                    for (int i = 0; i < ds7.Tables[0].Rows.Count; i++)
                    {
                        dtrow1 = dtable1.NewRow();
                        slno++;

                        dtrow1["Sl.No"] = slno.ToString();
                        dtrow1["Roll No"] = ds7.Tables[0].Rows[i]["Roll_No"].ToString();
                        dtrow1["Reg No"] = ds7.Tables[0].Rows[i]["Reg_No"].ToString();
                        dtrow1["Student Name"] = ds7.Tables[0].Rows[i]["stud_name"].ToString();
                        dtrow1[colName] = ds7.Tables[0].Rows[i]["marks_obtained"].ToString();

                        dtable1.Rows.Add(dtrow1);

                    }
                }
                else
                {
                    if (ddlSubject.Text != "")
                    {
                        if (ddlSubject.SelectedValue.ToString() == "-1")
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select Any Test";
                        }
                    }
                }
            }
            if (PrintMaster == false)
            {

            }
            if (fnaltab.Rows.Count > 0)
            {
                gview.DataSource = fnaltab;
            }
            else
            {
                gview.DataSource = dtable1;
            }
            gview.DataBind();
            gview.Visible = true;

            RowHead(gview, 1);

            for (int row = 0; row < gview.Rows.Count; row++)
            {
                for (int cell = 0; cell < gview.HeaderRow.Cells.Count; cell++)
                {
                    if (gview.Rows[row].Cells[0].Text.ToLower() == "average" || gview.Rows[row].Cells[0].Text.ToLower() == "pass count" || gview.Rows[row].Cells[0].Text.ToLower() == "fail count")
                    {
                        gview.Rows[row].Cells[0].ColumnSpan = 4;
                        gview.Rows[row].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        gview.Rows[row].Cells[0].Font.Bold = true;
                        gview.Rows[row].Cells[1].Visible = false;
                        gview.Rows[row].Cells[2].Visible = false;
                        gview.Rows[row].Cells[3].Visible = false;
                    }
                    if (gview.HeaderRow.Cells[cell].Text.ToLower() != "roll no" && gview.HeaderRow.Cells[cell].Text.ToLower() != "reg no" && gview.HeaderRow.Cells[cell].Text.ToLower() != "student name")
                    {
                        gview.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                        gview.Rows[row].Cells[cell].VerticalAlign = VerticalAlign.Middle;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected DataTable MergeTablesByIndex(DataTable t1, DataTable t2)
    {
        if (t1 == null || t2 == null) throw new ArgumentNullException("t1 or t2", "Both tables must not be null");

        DataTable t3 = t1.Clone();  // first add columns from table1
        foreach (DataColumn col in t2.Columns)
        {
            string newColumnName = col.ColumnName;
            int colNum = 1;
            while (t3.Columns.Contains(newColumnName))
            {
                newColumnName = string.Format("{0}_{1}", col.ColumnName, ++colNum);
            }
            t3.Columns.Add(newColumnName, col.DataType);
        }
        var mergedRows = t1.AsEnumerable().Zip(t2.AsEnumerable(),
            (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
        foreach (object[] rowFields in mergedRows)
            t3.Rows.Add(rowFields);

        return t3;
    }

    //protected void onDataBinding(object sender,GridView view)
    //{
    //    int ad = view.Columns.Count;
    //}

    public string Getdate(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        mycon1.Close();
        mycon1.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(exampresent, con);
        SqlCommand cmd5a = new SqlCommand(sqlstr);
        cmd5a.Connection = mycon1;
        SqlDataReader drnew;
        drnew = cmd5a.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
        mycon1.Close();
    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        //con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
        con.Open();
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    public void GetTesttype()
    {
        try
        {
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());

            string Sqlstr;
            Sqlstr = "";
            if (SyllabusQry != "" && SyllabusQry != null)
            {

                Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
                DataSet titles = new DataSet();
                con.Close();
                con.Open();
                sqlAdapter1.Fill(titles);

                ddltest.DataSource = titles;
                ddltest.DataValueField = "Criteria_No";
                ddltest.DataTextField = "Criteria";
                ddltest.DataBind();
                ddltest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "-1"));

                if (ddltest.SelectedIndex == 0)
                {
                    //buttongo();
                }
            }
        }
        catch
        {
        }
    }

    protected void chkonesubject_checkedchanged(object sender, EventArgs e)
    {
        if (chkonesubject.Checked == true)
        {
            GetparticularTesttype();
        }
        else
        {
            GetTesttype();
        }
    }

    public void GetparticularTesttype()
    {
        try
        {
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());

            string Sqlstr;
            Sqlstr = "";
            if (SyllabusQry != "" && SyllabusQry != null)
            {

                Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
                DataSet titles = new DataSet();
                con.Close();
                con.Open();
                sqlAdapter1.Fill(titles);

                ddltest.DataSource = titles;
                ddltest.DataValueField = "Criteria_No";
                ddltest.DataTextField = "Criteria";
                ddltest.DataBind();
                // ddltest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("ALL", "-1"));

                if (ddltest.SelectedIndex == 0)
                {
                    //buttongo();
                }
            }
        }
        catch
        {
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //    head_pnl_set.Visible = false;
        //    set_head_pnl.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        lblnofrmto.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        Button1.Visible = false;

        //FpSpread1.Visible = false;
        gview.Visible = false;
        Button1.Visible = false;
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        Panel4.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        ddltest.Items.Clear();
        con.Open();
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string course_id = ddlDegree.SelectedValue.ToString();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            //DataSet ds = Bind_Dept(course_id, collegecode, usercode);
            //ddlBranch.DataSource = ds;
            //ddlBranch.DataValueField = "degree_code";
            //ddlBranch.DataTextField = "dept_name";
            //ddlBranch.DataBind();
            //con.Close();
            bindbranch();
        }

        bindsem();
        // BindSectionDetail();
        bindsec();
        GetSubject();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //  head_pnl_set.Visible = false;
        //   set_head_pnl.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        lblnorec.Visible = false;
        //FpSpread1.Visible = false;//
        gview.Visible = false;
        Panel4.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        ddltest.Items.Clear();
        bindsem();
        Button1.Visible = false;

        // BindSectionDetail();
        bindsec();
        GetSubject();
        if (!Page.IsPostBack == false)
        {

        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            {
                bindsem();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    public void BindSectionDetail()
    {
        if (ddlSem.SelectedValue != "")
        {
            string branch = ddlBranch.SelectedValue.ToString();
            string batch = ddlBatch.SelectedValue.ToString();
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
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
            }
            else
            {

            }

            SqlDataReader dr_sec;
            dr_sec = cmd.ExecuteReader();
            dr_sec.Read();
            if (dr_sec.HasRows == true)
            {
                if (dr_sec["sections"].ToString() == "")
                {
                    ddlSec.Enabled = false;
                    GetSubject();
                }
                else
                {
                    ddlSec.Enabled = true;
                }
            }
            else
            {
                ddlSec.Enabled = false;
                GetSubject();
            }
        }
    }

    public void bindsem()
    {

        //--------------------semester load
        ddlSem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
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
                    ddlSem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlSem.Items.Clear();
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
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        con.Close();
    }

    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            ddlSem.Items.Clear();
            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSem.Items.Add(i.ToString());

                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }

            }
        }
        else
        {
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //head_pnl_set.Visible = false;
        //set_head_pnl.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        lblnorec.Visible = false;
        //FpSpread1.Visible = false;//
        gview.Visible = false;
        Panel4.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        Button1.Visible = false;
        ddltest.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }

        bindsec();
        GetSubject();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            //FpSpread1.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());//
            gview.PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            // CalculateTotalPages();
            ////if ((Convert.ToInt32(FpSpread1.Sheets[0].PageSize) != 10) && Convert.ToInt32(FpSpread1.Sheets[0].PageSize) != 20 && Convert.ToInt32(FpSpread1.Sheets[0].PageSize) != 30)
            if ((Convert.ToInt32(gview.PageSize) != 10) && Convert.ToInt32(gview.PageSize) != 20 && Convert.ToInt32(gview.PageSize) != 30)
            {
                ////FpSpread1.Height = 100 + (10 * Convert.ToInt32(FpSpread1.Sheets[0].PageSize));
                ////FpSpread1.Width = 100 * Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount);

                gview.Height = 100 + (10 * Convert.ToInt32(gview.PageSize));
                gview.Width = 100 * Convert.ToInt32(gview.PageSize);
            }
            else
            {
                //FpSpread1.Height = 500;//
                gview.Height = 500;
            }
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        //totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);//
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);//
        totalRows = Convert.ToInt32(gview.Rows.Count);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
        //FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
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
                    //FpSpread1.Visible = true;//
                    gview.Visible = true;
                    TextBoxpage.Text = "";
                    Button1.Visible = true;
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
                    //FpSpread1.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;//
                    //FpSpread1.Visible = true;//
                    gview.Visible = true;
                    Button1.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TextBoxother.Text != "")
            {
                //FpSpread1.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());//
                gview.PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                // CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }

    public string filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "ORDER BY registration.Roll_No";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }
        return strorder;
    }

    public void getname()
    {
        try
        {

            string batch = "";
            string degreecode = "";
            sections = ddlSec.SelectedValue.ToString();
            string semester = "";
            string basicinfo = "";



            if (!Page.IsPostBack == false)
            {
                strsec = "";
                if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and exam_type.Sections='" + sections.ToString() + "'";
                }
            }
            filteration();
            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlBranch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSem.SelectedValue.ToString();
            string filterwithsec = " batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + degreecode.ToString() + " and sections=" + "'" + sections.ToString() + "'" + " and RollNo_Flag<>0 and cc=0 and exam_flag <>'DEBAR' and delflag=0 " + " " + strorder.ToString();
            string filterwithoutsec = " batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + degreecode.ToString() + " and RollNo_Flag<>0 and cc=0 and exam_flag <>'DEBAR' and delflag=0  " + " " + strorder.ToString();
            hat.Clear();
            hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
            hat.Add("degreecode", degreecode.ToString());
            hat.Add("strsec", sections.ToString());
            hat.Add("semester", int.Parse(semester.ToString()));
            hat.Add("filterwithsec", filterwithsec.ToString());
            hat.Add("filterwithoutsec", filterwithoutsec.ToString());
            ds = d2.select_method("SELECT_ALL_STUDENT", hat, "sp");

            //FpSpread1.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.White;
            if (ds.Tables[0].Rows.Count > 0)
            {
                //FpSpread1.Visible = false;/
                gview.Visible = false;
                lblnorec.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                LabelE.Visible = false;
                flag = 1;
                Button1.Visible = false;
            }
            //FpSpread1.DataBind();//

            con.Close();
            if (flag == 0)
            {
                //FpSpread1.Visible = false;//
                gview.Visible = false;
                lblnorec.Visible = true;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                LabelE.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Button1.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void GetSubject()
    {
        con.Close();
        con.Open();
        ddlSubject.Items.Clear();
        string sections = ddlSec.SelectedValue.ToString();
        strsec = "";
        if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
        {
            strsec = "";
        }
        else
        {
            strsec = " and exam_type.Sections='" + sections.ToString() + "'";
        }
        string strsem = "";
        string strsem1 = "";
        string regsem = "";
        string sems = "";
        if (ddlSem.SelectedValue != "")
        {
            if (ddlSem.SelectedValue == "")
            {
                strsem = "";
                strsem1 = "";
                regsem = "";
                sems = "";
            }
            else
            {
                strsem = " and semester =" + ddlSem.SelectedValue.ToString() + "";
                strsem1 = "and syllabus_master.semester=" + ddlSem.SelectedValue.ToString() + "";
                regsem = " and registration.current_semester>=" + ddlSem.SelectedValue.ToString() + "";
                sems = "and SM.semester=" + ddlSem.SelectedValue.ToString() + "";
            }
            //string SyllabusYr;
            //string SyllabusQry;
            //SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " " + strsem + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            //SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";
            //if (SyllabusYr != "")
            //{
            if (Session["Staff_Code"].ToString() == "")
            {
                // Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from exam_type,subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + strsem1 + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + regsem + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 " + strsec + " and exam_flag <> 'DEBAR'";
                Sqlstr = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and  S.subtype_no = Sem.subtype_no and promote_count=1  order by subject_code ";
            }
            else if (Session["Staff_Code"].ToString() != "")
            {

                //  Sqlstr = " select distinct s.subject_no,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and staff_code='" + Session["Staff_Code"].ToString() + "' order by st.batch_year,sy.degree_code,semester,st.sections ";
                Sqlstr = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1 and staff_code='" + Session["Staff_Code"].ToString() + "'  order by subject_code "; //new as per ind sub attend
            }
            con.Close();
            if (Sqlstr != "")
            {
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
                DataSet titles = new DataSet();
                con.Open();
                sqlAdapter1.Fill(titles);
                if (titles.Tables[0].Rows.Count > 0)
                {
                    ddlSubject.Enabled = true;
                    ddlSubject.DataSource = titles;
                    ddlSubject.DataValueField = "Subject_No";
                    ddlSubject.DataTextField = "Subject_Name";

                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));

                }
                else
                {
                    ddlSubject.Enabled = false;
                }
                //ddlSubject.SelectedIndex = 0;
            }
            //  }
            //else
            //{
            //    ddlSubject.Items.Clear();
            //    ddlSubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            //    ddlSubject.SelectedIndex = 0;
            //}
            //con.Close();
        }
        else
        {
            ddlSubject.SelectedIndex = 0;
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //   head_pnl_set.Visible = false;
        //  set_head_pnl.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        lblnorec.Visible = false;
        //FpSpread1.Visible = false;//
        gview.Visible = false;
        Button1.Visible = false;
        Panel4.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        GetSubject();
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //    head_pnl_set.Visible = false;
        //   set_head_pnl.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        ddltest.Items.Clear();
        if (ddlSubject.SelectedValue != "--Select--")
        {
            //  mark_entry();
            // getname();
            GetTesttype();

            CheckBox1.Visible = false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        buttongo();
        int widthcnt = 0;
        //for (int col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        ////for (int col_count = 0; col_count < gview.HeaderRow.Cells.Count; col_count++)
        ////{
        ////    //if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
        ////    if (gview.HeaderRow.Cells[col_count].Visible == true)
        ////    {
        ////        widthcnt++;
        ////    }
        ////}
        //FpSpread1.Width = 100 + (100 * widthcnt);//
        gview.Width = 100 + (100 * widthcnt);
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0,1].Border.BorderColorTop = Color.Black;
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0,8].Border.BorderColorTop = Color.Black;
    }

    protected void buttongo()
    {
        try
        {
            btnExcel.Visible = true;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            //FpSpread1.Sheets[0].RowCount = 0;//
            RadioHeader.Visible = false;
            Radiowithoutheader.Visible = false;

            lblnorec.Visible = false;

            TextBoxother.Visible = false;
            TextBoxother.Text = "";
            TextBoxpage.Text = "";
            //FpSpread1.CurrentPage = 0;//

            if (lblnorec.Visible == true)
            {
                //  set_head_pnl.Visible = false;
                //FpSpread1.Visible = false;//
                gview.Visible = false;
                Button1.Visible = false;
                lblnorec.Visible = true;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                LabelE.Visible = false;
                //    head_pnl_set.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;

            }

            ////if (Session["Rollflag"].ToString() == "0")
            ////{
            ////    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            ////}
            ////if (Session["Regflag"].ToString() == "0")
            ////{
            ////    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            ////}
            ////if (Session["Studflag"].ToString() == "0")
            ////{
            ////    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
            ////}
            if (ddlSubject.Text == "" || ddlSubject.SelectedIndex == 0)
            {
            }

            getname();
            GetTest();



            //gowthman 02Aug2013  if ((Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 0) || (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 3))
            if (Convert.ToInt32(gview.Rows.Count) == 0)
            {
                Button1.Visible = false;
                //FpSpread1.Visible = false;//
                gview.Visible = false;
                lblnorec.Text = "No Record(s) Found";
                lblnorec.Visible = true;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                LabelE.Visible = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;

                // set_head_pnl.Visible = false;
                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;

                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
            }
            else
            {
                lblnorec.Visible = false;
                Buttontotal.Visible = true;
                lblrecord.Visible = true;
                DropDownListpage.Visible = true;
                TextBoxother.Visible = false;
                lblpage.Visible = true;
                TextBoxpage.Visible = true;
                LabelE.Visible = true;
                //FpSpread1.Visible = true;//
                gview.Visible = true;
                Button1.Visible = true;


                ////FpSpread1.SaveChanges();
                ////FpSpread1.Sheets[0].PageSize = 10;


                ////FpSpread1.Sheets[0].Columns[0].Width = 120;
                ////FpSpread1.Sheets[0].Columns[1].Width = 120;
                ////FpSpread1.Sheets[0].Columns[2].Width = 200;
                ////FpSpread1.Sheets[0].Columns[3].Width = 150;

                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;

                //FpSpread1.Sheets[0].FrozenColumnCount = 6;
                ////FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                ////FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                ////FpSpread1.Pager.Align = HorizontalAlign.Right;

                if (CheckBox1.Checked != true)
                {
                    // FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    ////FpSpread1.ActiveSheetView.SpanModel.Add((Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1), 0, 1, 4);
                    ////FpSpread1.Sheets[0].SetText(Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1, 0, "Average");
                    ////FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    ////FpSpread1.Sheets[0].RowHeader.Cells[Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1, 0].Text = "";

                }
                Double totalRows = 0;
                //totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);//
                //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);//
                totalRows = Convert.ToInt32(gview.Rows.Count);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                Buttontotal.Text = "Records : " + totalRows + " Pages : " + Session["totalPages"];

                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    //FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //FpSpread1.Height = 335;//
                    DropDownListpage.Enabled = true;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    //FpSpread1.Height = 105;//
                }
                else
                {
                    //FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    //DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());//
                    //FpSpread1.Height = 105 + (10 * Convert.ToInt32(totalRows));//
                }
                //FpSpread1.Height = 200 + (20 * Convert.ToInt32(totalRows));//
                //FpSpread1.Width = 100 + (100 * (FpSpread1.Sheets[0].ColumnCount - 1));//
                //   FpSpread1.Sheets[0].FrozenRowCount = Convert.ToInt32(totalRows);
                //FpSpread1.SaveChanges();//

            }
            if (ddlSubject.Text == "" || ddlSubject.SelectedIndex == 0)
            {
                //FpSpread1.Visible = false;//
                gview.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                LabelE.Visible = false;
                lblnorec.Visible = false;

                RadioHeader.Visible = false;
                Radiowithoutheader.Visible = false;

                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                lblnorec.Visible = false;
                Button1.Visible = false;
            }
            if (ddlSubject.SelectedItem.ToString() != "--Select--" && ddlSubject.SelectedIndex == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Select The Test";
            }
            if (CheckBox1.Checked == true)
            {
                if (fromtext.Text == "")
                {
                    lblnofrmto.Visible = true;
                    lblnofrmto.Text = "Fill From Range";
                    lblnorec.Visible = false;
                    RadioHeader.Visible = false;
                    Radiowithoutheader.Visible = false;

                    btnExcel.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    lblnorec.Visible = false;
                    DropDownListpage.Visible = false;
                    //FpSpread1.Visible = false;//
                    gview.Visible = false;
                    Button1.Visible = false;
                }
                if (Totext.Text == "")
                {
                    lblnofrmto.Visible = true;
                    lblnofrmto.Text = "Fill To Range";
                    lblnorec.Visible = false;
                    RadioHeader.Visible = false;
                    Radiowithoutheader.Visible = false;

                    btnExcel.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    lblnorec.Visible = false;
                    DropDownListpage.Visible = false;
                    //FpSpread1.Visible = false;//
                    gview.Visible = false;
                    Button1.Visible = false;
                }
                if (fromtext.Text == "" && Totext.Text == "")
                {
                    lblnofrmto.Visible = true;
                    lblnofrmto.Text = "Fill From and To Range";
                    lblnorec.Visible = false;
                    RadioHeader.Visible = false;
                    Radiowithoutheader.Visible = false;

                    btnExcel.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    lblnorec.Visible = false;
                    DropDownListpage.Visible = false;
                    //FpSpread1.Visible = false;//
                    gview.Visible = false;
                    Button1.Visible = false;
                }
                if (fromtext.Text != "" && Totext.Text != "")
                {
                    lblnofrmto.Visible = false;
                    if ((Convert.ToInt32(fromtext.Text)) > (Convert.ToInt32(Totext.Text)))
                    {
                        lblnofrmto.Visible = true;
                        lblnofrmto.Text = "from value is greater than To";
                        lblnorec.Visible = false;
                        //    set_head_pnl.Visible = false;
                        RadioHeader.Visible = false;
                        Radiowithoutheader.Visible = false;

                        btnExcel.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        lblnorec.Visible = false;
                        DropDownListpage.Visible = false;
                        //FpSpread1.Visible = false;//
                        gview.Visible = false;
                        Button1.Visible = false;
                    }
                    else
                    {

                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        //   head_pnl_set.Visible = false;
        //   set_head_pnl.Visible = false;


        if (CheckBox1.Checked == true)
        {
            btgGO.Visible = true;
            Panel4.Visible = true;
            CheckBox1.Visible = true;
            chkonesubject.Checked = false;
            chkonesubject.Visible = false;
            lblConvert_Value.Visible = false;
            txtConvert_Value.Visible = false;

        }
        else
        {
            lblnofrmto.Visible = false;
            Panel4.Visible = false;
            CheckBox1.Visible = false;
            Totext.Text = "";
            fromtext.Text = "";

            chkonesubject.Visible = true;
            lblConvert_Value.Visible = true;
            txtConvert_Value.Visible = true;
            // btgGO.Visible = false;
        }
        getname();
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
    }

    protected void ddltest_SelectedIndexChanged(object sender, EventArgs e)
    {

        //    head_pnl_set.Visible = false;
        //    set_head_pnl.Visible = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        if (ddltest.SelectedItem.Text == "ALL")
        {
            //buttongo();
            CheckBox1.Checked = false;
            CheckBox1.Visible = false;
            Panel4.Visible = false;
            Totext.Text = "";
            fromtext.Text = "";
        }
        if (ddltest.SelectedItem.Text != "ALL")
        {
            //FpSpread1.Sheets[0].RowCount = 0;//
            //FpSpread1.Visible = false;//
            gview.Visible = false;
            Button1.Visible = false;
            // buttongo();
            lblnofrmto.Visible = false;
            CheckBox1.Visible = true;
        }
        LabelE.Visible = false;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnofrmto.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        lblnorec.Visible = false;
        //FpSpread1.Visible = false;//
        gview.Visible = false;
        Button1.Visible = false;
        Panel4.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        RadioHeader.Visible = false;
        Radiowithoutheader.Visible = false;

        ddltest.Items.Clear();
        con.Open();

        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string usercode = Session["usercode"].ToString();
            string collegecode = Session["collegecode"].ToString();
            //binddegree();

        }

        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string course_id = ddlDegree.SelectedValue.ToString();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            if (ddlDegree.Text != "")
            {
                //bindbranch();
                //bindsem();
                //bindsec();
                GetSubject();
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Give degree rights to the staff";
            }

        }

        //  BindSectionDetail();



    }

    protected void Totext_TextChanged(object sender, EventArgs e)
    {

    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;
        d2.printexcelreportgrid(gview, reportname);
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void func_header()
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //FarPoint.Web.Spread.EmptyCellType ett = new FarPoint.Web.Spread.EmptyCellType();
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].CellType = ett;
        //collnamenew1 = "";
        //address1 = "";
        //address2 = "";
        //address = "";
        //Phoneno = "";
        //Faxno = "";
        //phnfax = "";
        //district = "";
        //email = "";
        //form_heading_name = "";
        //batch_degree_branch = "";
        //string[] split_batch_deg = new string[3];
        ////'----------for header

        //if (dsprint.Tables[0].Rows.Count > 0)
        //{

        //    if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
        //    {
        //        collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
        //    {
        //        address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
        //        address = address1;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
        //    {
        //        address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
        //        address = address1 + "-" + address2;

        //    }
        //    if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
        //    {
        //        district = dsprint.Tables[0].Rows[0]["address3"].ToString();
        //        address = address1 + "-" + address2 + "-" + district;
        //    }

        //    if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
        //    {
        //        Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
        //        phnfax = "Phone :" + " " + Phoneno;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
        //    {
        //        Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
        //        phnfax = phnfax + "Fax  :" + " " + Faxno;
        //    }

        //    if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
        //    {
        //        email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
        //    {
        //        email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
        //    {
        //        form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
        //    {
        //        batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
        //        split_batch_deg = batch_degree_branch.Split('@');
        //    }
        //    //-to set the left logo
        //    string dis_hdng_batch = "", dis_hdng_sec = "", dis_hdng_test="";
        //    if ((ddlBatch.Text != string.Empty) && (ddlDegree.Text != string.Empty) && (ddlBranch.Text != string.Empty))
        //    {
        //        dis_hdng_batch = "Batch Year " + "- " + ddlBatch.SelectedItem.ToString() + " Course " + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        //    }

        //    if ((ddlSem.Text != string.Empty) && (ddlSec.Text != string.Empty))
        //    {
        //        dis_hdng_sec = "Semester " + "- " + ddlSem.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        //    }

        //    if (ddltest.Text != string.Empty)
        //    {
        //        dis_hdng_test = "Subject: " + ddlSubject.SelectedItem.Text.ToString() + "  ,     Test: " + ddltest.SelectedItem.ToString();
        //    }

        //    for (int hdr_col = 1; hdr_col < FpSpread1.Sheets[0].ColumnCount; hdr_col++)
        //    {
        //        if (final_print_col_cnt == 1)
        //        {
        //            if (FpSpread1.Sheets[0].Columns[hdr_col].Visible == true)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Text = dis_hdng_batch.ToString(); //split_batch_deg[0].ToString();
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, hdr_col].Text = dis_hdng_sec.ToString(); //split_batch_deg[1].ToString();
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, hdr_col].Text = dis_hdng_test.ToString(); //split_batch_deg[2].ToString();

        //                //FpSpread1.Width = 500;

        //                break;

        //            }
        //        }

        //        else if (final_print_col_cnt == FpSpread1.Sheets[0].ColumnCount)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 1, final_print_col_cnt - 2);
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, 1, 1, final_print_col_cnt - 2);

        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = address;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Text = phnfax;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Text = email;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Text = form_heading_name;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Text = dis_hdng_batch.ToString(); //split_batch_deg[0].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Text = dis_hdng_sec.ToString(); //split_batch_deg[1].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Text = dis_hdng_test.ToString(); //split_batch_deg[2].ToString();




        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, hdr_col].HorizontalAlign = HorizontalAlign.Center;

        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorRight = Color.White;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorRight = Color.White;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorRight = Color.White;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorBottom = Color.White;

        //            break;
        //        }
        //        else if (final_print_col_cnt < FpSpread1.Sheets[0].ColumnCount)
        //        {
        //            if (FpSpread1.Sheets[0].Columns[hdr_col].Visible == true)
        //            {
        //                chk_secnd_clmn++;
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, hdr_col, 1, final_print_col_cnt-2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, hdr_col, 1, final_print_col_cnt - 2);
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, hdr_col, 1, final_print_col_cnt - 2);


        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Text = dis_hdng_batch.ToString(); //split_batch_deg[0].ToString();
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, hdr_col].Text = dis_hdng_sec.ToString(); //split_batch_deg[1].ToString();
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, hdr_col].Text = dis_hdng_test.ToString(); //split_batch_deg[2].ToString();

        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, hdr_col].HorizontalAlign = HorizontalAlign.Center;

        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorBottom = Color.White;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorBottom = Color.White;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorRight = Color.White;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorRight = Color.White;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorRight = Color.White;
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorBottom = Color.White;
        //                break;
        //            }
        //        }
        //    }
        //}

        //for (int logo_col = 1; logo_col < FpSpread1.Sheets[0].ColumnCount; logo_col++)
        //{
        //    if (FpSpread1.Sheets[0].Columns[logo_col].Visible == true)
        //    {
        //        right_logo_clmn = logo_col;
        //    }
        //}
        //FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 50;
        //MyImg1 mi4 = new MyImg1();
        //mi4.ImageUrl = "Handler/Handler5.ashx?";
        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, right_logo_clmn, 8, 1);
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].CellType = mi4;
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].Border.BorderColorBottom = Color.Black;

        //FpSpread1.Sheets[0].SheetCornerSpanModel.Add(0, 0, 8, 1);
        //MyImg1 mi3 = new MyImg1();
        //mi3.ImageUrl = "Handler/Handler2.ashx?";
        ////FpSpread1.Sheets[0].SheetCorner.Cells[0, 1].CellType = mi3;
        ////FpSpread1.Sheets[0].SheetCorner.Columns[1].Width = 120;
        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 8, 1);
        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi3;
        //FpSpread1.Sheets[0].Columns[0].Width = 120;
    }

    public void func_Print_Master_Setting()
    {
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "CAMRange.aspx");
        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            ////for (int newlp = 0; newlp <= FpSpread1.Sheets[0].ColumnCount - 1; newlp++)
            ////{
            ////    FpSpread1.Sheets[0].Columns[newlp].Visible = false;
            ////}
            for (int newlp = 0; newlp <= gview.HeaderRow.Cells.Count - 1; newlp++)
            {
                gview.HeaderRow.Cells[newlp].Visible = false;
            }
        }
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != string.Empty)
            {
                string new_hdr_text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                string[] spl_hdr_text = new_hdr_text.Split(',');
                if (spl_hdr_text.GetUpperBound(0) > 0)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.RowCount += spl_hdr_text.GetUpperBound(0) + 2;//
                }
                else
                {
                    //FpSpread1.Sheets[0].ColumnHeader.RowCount++;//
                }
            }
        }


        //'@@@@@@@@@@@@@@ set the headername
        string printvar = "";
        printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();
        string[] split_printvar = printvar.Split(',');
        ////for (int newloop = 0; newloop <= FpSpread1.Sheets[0].ColumnCount - 1; newloop++)
        ////{
        ////    for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
        ////    {
        ////        if (FpSpread1.Sheets[0].ColumnHeader.Cells[0, newloop].Text == split_printvar[splval].ToString())
        ////        {
        ////            final_print_col_cnt++;
        ////            Session["final_print_col_cnt"] = final_print_col_cnt;
        ////            FpSpread1.Sheets[0].Columns[newloop].Visible = true;
        ////            FpSpread1.Sheets[0].SheetCorner.Cells[FpSpread1.Sheets[0].SheetCorner.RowCount - 1, 0].Text = "S.No";
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, newloop].Text = split_printvar[splval].ToString();
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, newloop].HorizontalAlign = HorizontalAlign.Center; ;
        ////            FpSpread1.Sheets[0].ColumnHeader.Rows[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
        ////        }
        ////    }
        ////} //end loop for columnfields

        //////'@@@@@@@@ set the new header name
        ////if ((dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != " ") && (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != ""))
        ////{
        ////    FpSpread1.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        ////    string hdr_nam = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        ////    string[] spl_nwhdrname = hdr_nam.Split(',');
        ////    int strwindexcnt = 1;
        ////    if (spl_nwhdrname.GetUpperBound(0) > 0)
        ////    {
        ////        for (int strw = Convert.ToInt32(Session["sheetcorner"]); strw < FpSpread1.Sheets[0].SheetCorner.RowCount - 1; strw++)
        ////        {
        ////            if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
        ////            {
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Left;
        ////            }
        ////            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
        ////            {
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Center;
        ////            }
        ////            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
        ////            {
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
        ////                FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Right;
        ////            }
        ////            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(strw, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[strw, 0].Border.BorderColorLeft = Color.White;
        ////            strwindexcnt++;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        FpSpread1.Sheets[0].ColumnHeader.RowCount += 2;
        ////        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        ////        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
        ////        if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
        ////        {
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
        ////        }
        ////        else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
        ////        {
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
        ////        }
        ////        else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
        ////        {
        ////            FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Right;
        ////        }
        ////    }
        ////    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 0].Border.BorderColorLeft = Color.White;
        ////}//end loop for new header name
    }

    //'----------------------func for footer
    public void function_footer()
    {
        //----------------start for setting the footer
        ////if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
        ////{

        ////    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
        ////    FpSpread1.Sheets[0].RowCount += 3;
        ////    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        ////    string[] footer_text_split = footer_text.Split(',');

        ////    int count_span = FpSpread1.Sheets[0].ColumnCount / footer_count;

        ////    if (footer_text_split.GetUpperBound(0) > 0)
        ////    {
        ////        for (footer_balanc_col = 0; footer_balanc_col < footer_text_split.GetUpperBound(0) + 1; footer_balanc_col++)
        ////        {
        ////            if (footer_balanc_col == 0)
        ////            {
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col].Text = footer_text_split[footer_balanc_col].ToString();
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col].Font.Size = FontUnit.Medium;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col].Font.Bold = true;
        ////                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col, 1, count_span + 1);
        ////            }
        ////            else
        ////            {
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Text = footer_text_split[footer_balanc_col].ToString();
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Size = FontUnit.Medium;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Bold = true;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].HorizontalAlign = HorizontalAlign.Left;

        ////                //@@@@@@@@@ set the row border color white in footer

        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
        ////                //@@@@@@@ span the columns for foote text
        ////                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span, 1, FpSpread1.Sheets[0].ColumnCount);
        ////                //set the color for the row
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorBottom = Color.White;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorTop = Color.White;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColor = Color.White;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorRight = Color.White;
        ////                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorLeft = Color.White;
        ////                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Border.BorderColorTop = Color.White;
        ////                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
        ////            }

        ////        }
        ////    }
        ////    else
        ////    {

        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = footer_text;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
        ////        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorRight = Color.White;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorTop = Color.White;

        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
        ////        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Border.BorderColorTop = Color.White;



        ////    }
        ////    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 3, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
        ////    //   FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 1, FpSpread1.Sheets[0].ColumnCount);
        ////    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColor = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;
        ////    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Border.BorderColorTop = Color.White;
        ////    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].Border.BorderColorBottom = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorRight = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;

        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Border.BorderColorRight = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Border.BorderColorLeft = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Border.BorderColorBottom = Color.White;

        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Border.BorderColorRight = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Border.BorderColorLeft = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Border.BorderColorBottom = Color.White;

        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 3].Border.BorderColorRight = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 3].Border.BorderColorLeft = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 3].Border.BorderColorBottom = Color.White;

        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Border.BorderColorRight = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Border.BorderColorLeft = Color.White;
        ////    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Border.BorderColorBottom = Color.White;
        ////}
        ////if (dsprint.Tables[0].Rows.Count > 0)
        ////{
        ////    if (dsprint.Tables[0].Rows[0]["column_fields"].ToString() == string.Empty)
        ////    {
        ////        lblnorec.Visible = true;
        ////        lblnorec.Text = "Select Atleast One Column From The TreeView";
        ////        FpSpread1.Visible = false;
        ////        Buttontotal.Visible = false;
        ////        lblrecord.Visible = false;
        ////        DropDownListpage.Visible = false;
        ////        TextBoxother.Visible = false;
        ////        lblpage.Visible = false;
        ////        TextBoxpage.Visible = false;
        ////        btnExcel.Visible = false;
        ////        //Added By Srinath 27/2/2013
        ////        txtexcelname.Visible = false;
        ////        lblrptname.Visible = false;
        ////        Button1.Visible = false;
        ////    }
        ////    else
        ////    {
        ////        lblnorec.Visible = false;
        ////        lblnorec.Text = "";
        ////        FpSpread1.Visible = true;
        ////        Buttontotal.Visible = true;
        ////        lblrecord.Visible = true;
        ////        DropDownListpage.Visible = true;
        ////        TextBoxother.Visible = true;
        ////        lblpage.Visible = true;
        ////        TextBoxpage.Visible = true;
        ////        btnExcel.Visible = true;
        ////        //Added By Srinath 27/2/2013
        ////        txtexcelname.Visible = false;
        ////        lblrptname.Visible = false;
        ////        FpSpread1.Height = 600;
        ////        Button1.Visible = true;
        ////    }
        ////}
    }

    //--------------func for print master settings
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        if (CheckBox1.Checked == true)
        {
            string chkfrom = fromtext.Text;
            string chkto = Totext.Text;
            Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSem.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddltest.SelectedIndex + "," + ddlSubject.SelectedIndex + "$" + chkfrom + "$" + chkto;
        }
        else
        {
            Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSem.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddltest.SelectedIndex + "," + ddlSubject.SelectedIndex;
        }

        string clmnheadrname = "";
        string dis_hdng_batch = "";
        string dis_hdng_sec = "";
        string dis_hdng_test = "";

        //int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;//
        int total_clmn_count = gview.HeaderRow.Cells.Count;
        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (clmnheadrname == "")
            {
                //clmnheadrname = FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;//
            }
            else
            {
                //clmnheadrname = clmnheadrname + "," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;//
            }
        }

        if ((ddlBatch.Text != string.Empty) && (ddlDegree.Text != string.Empty) && (ddlBranch.Text != string.Empty))
        {
            dis_hdng_batch = "Batch Year " + "- " + ddlBatch.SelectedItem.ToString() + " Course " + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        }

        if ((ddlSem.Text != string.Empty) && (ddlSec.Text != string.Empty))
        {
            dis_hdng_sec = "Semester " + "- " + ddlSem.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        }

        if (ddltest.Text != string.Empty)
        {
            dis_hdng_test = ddltest.SelectedItem.ToString();
        }

        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname + ":" + "CAMRange.aspx" + ":" + dis_hdng_batch + "@" + dis_hdng_sec + "@" + dis_hdng_test + ":" + "CAM Range Report");
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 1;
        // string filt_details = "Degree: " + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSem.SelectedItem.ToString() + "-" + "Sec " + ddlSec.SelectedItem.ToString();
        //string date_filt = "From :" + txtFromDate.Text + "-" + "To :" + txtToDate.Text;

        string filt_details = "";
        if (ddlSec.Enabled == true)
        {
            filt_details = "Degree: " + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSem.SelectedItem.ToString() + "-" + "Sec " + ddlSec.SelectedItem.ToString();
        }
        else
        {
            filt_details = "Degree: " + ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSem.SelectedItem.ToString();
        }


        string test = "Test :" + ddltest.SelectedItem.ToString();

        string degreedetails = string.Empty;

        degreedetails = "CAM Subject Range Analysis" + "@" + filt_details + "@ Subject Name:" + ddlSubject.SelectedItem.Text + "@" + test;
        string pagename = "CAMRange.aspx";

        string ss = Session["usercode"].ToString();
        Printcontrol1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        Printcontrol1.Visible = true;
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