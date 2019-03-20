using System; //modified on 05.04.12---mythili,modified logo size on 08.06.12
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Data.SqlClient;
using System.Drawing;

public partial class MarkEntry : System.Web.UI.Page
{
    int T = 0;
    int strdate = 0;
    string str_day = "";
    string Atmonth = "";
    string Atyear = "";
    string dtpFrom = "";
    string newdate = "";
    string exam_code = "";
    // string semester = "";
    ////double pres = 0;
    ////double OD = 0;
    ////double lev = 0;
    ////double ab = 0;
    ////double NoOfAbsent = 0;
    ////double NoOfPresent = 0;
    ////double NoOfOD = 0;
    ////double NoOfLe = 0;
    ////double pass_perc, fail_perc;
    ////double NoOfPass, NoOfFail;
    ////double eod = 0;
    ////double mark_avg = 0.0;
    ////int tot_stud;
    ////string roll_no = "";
    SqlCommand cmd;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection myconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection funconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection newconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection subconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection testconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection loadconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection rdnewconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection leavconn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mrkcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection lcon4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable hat = new Hashtable();
    Hashtable htpass = new Hashtable();
    Hashtable htfail = new Hashtable();
    Hashtable htabsent = new Hashtable();
    Hashtable htpresent = new Hashtable();
    Hashtable htpassperc = new Hashtable();
    Hashtable htclsavg = new Hashtable();

    DAccess2 dacces2 = new DAccess2();

    DataSet ds_load = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds_optim = new DataSet();
    ////string sqlst1 = "";
    ////string sqlst2 = "";
    ////string sqlst3 = "";
    ////string sqlsum = "";
    ////string sqlst99 = "";
    ////string staffnam = "";
    //saravana start
    DAccess2 d2 = new DAccess2();
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
    string strsec = "";
    string mrkcriteria = "";
    int sub_count;
    Boolean Cellclick = false;
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    //saravana end
    //------------------new print master variables
    static Boolean PrintMaster = false;
    DataSet dsprint = new DataSet();


    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    string district = "";
    string email = "";
    string form_heading_name = "";
    string batch_degree_branch = "";

    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    int footer_flag_value = 0;

    int right_logo_clmn = 0;

    //[Serializable()]
    //public class MyImg : ImageCellType
    //{
    //    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    //    {
    //        //''----------strudent photo
    //        //System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
    //        //img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        //img1.Width = Unit.Percentage(75);
    //        //img1.Height = Unit.Percentage(70);
    //        //return img1;

    //        //''------------clg left logo
    //        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
    //        img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img.Width = Unit.Percentage(100);
    //        img.Height = Unit.Percentage(35);
    //        return img;

    //    }
    //}
    //public class MyImg1 : ImageCellType
    //{
    //    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    //    {
    //        //'-------------clg right logo
    //        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
    //        img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img2.Width = Unit.Percentage(100);
    //        img2.Height = Unit.Percentage(35);
    //        return img2;

    //    }
    //}
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
        lblerr.Visible = false;
        if (!IsPostBack)
        {



            rdSubName.Visible = true;
            rdSubCode.Visible = true;
            rdSubCode.Enabled = false;
            rdSubName.Enabled = false;
            FpMarkEntry.Visible = false;
            btnExcel.Visible = false;
            Button1.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblErrTest.Visible = false;


            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            //-----------------for print master settings added on 05.04.12 

            if (Request.QueryString["val"] != null)
            {

                string get_pageload_value = Request.QueryString["val"];
                if (get_pageload_value.ToString() != null)
                {
                    string[] spl_pageload_val = get_pageload_value.Split(',');

                    //'---------- to bind the branch,sem,sec,batch,deg
                    bindbatch();
                    binddegree();
                    if (ddlDegree.Text != "")
                    {

                        bindbranch();
                        bindsem();
                        bindsec();
                    }
                    else
                    {
                        lblnorec.Text = "Give degree rights to the staff";
                        lblnorec.Visible = true;
                    }
                    //---------------------end for branch,sem,sec,batch,deg
                    ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());//assigning the index 
                    ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                    ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                    ddlSemYr.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                    ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                    GetTest();
                    ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                    if (spl_pageload_val.GetUpperBound(0) == 7)
                    {
                        //rdTestWise.Enabled = true;
                        rdTestWise.Checked = true;
                        if (spl_pageload_val[7].ToString() == "Subject Code")
                        {
                            rdSubCode.Enabled = true;
                            rdSubCode.Checked = true;
                        }
                        else
                        {
                            rdSubName.Enabled = true;
                            rdSubName.Checked = true;
                        }
                    }
                    else
                    {
                        rdSubCode.Enabled = false;
                        rdSubName.Enabled = false;
                        rdSubWise.Checked = true;
                    }
                    btnGo_Click(sender, e);
                    func_Print_Master_Setting();
                    func_header();
                }
            }
            else
            {
                //'---------- to bind the branch,sem,sec,batch,deg
                bindbatch();
                binddegree();
                if (ddlDegree.Text != "")
                {

                    bindbranch();
                    bindsem();
                    bindsec();
                }
                else
                {
                    lblnorec.Text = "Give degree rights to the staff";
                    lblnorec.Visible = true;
                }
            }
            fsstaff.Sheets[0].AutoPostBack = true;
            fsstaff.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo styles = new FarPoint.Web.Spread.StyleInfo();
            styles.Font.Size = 10;
            styles.Font.Bold = true;
            styles.Font.Name = "Book Antiqua";
            styles.HorizontalAlign = HorizontalAlign.Center;
            styles.ForeColor = Color.Black;
            styles.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            fsstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(styles);
            fsstaff.Sheets[0].AllowTableCorner = true;
            fsstaff.Sheets[0].RowHeader.Visible = false;

            fsstaff.Sheets[0].DefaultColumnWidth = 50;
            fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
            fsstaff.SheetCorner.Cells[0, 0].Font.Bold = true;

            fsstaff.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            fsstaff.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;

            //fsstaff.Sheets[0].AutoPostBack = true;
            fsstaff.Sheets[0].ColumnCount = 6;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Student Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[3].Label = "Degree";
            fsstaff.Sheets[0].ColumnHeader.Columns[4].Label = "Department";
            fsstaff.Sheets[0].ColumnHeader.Columns[5].Label = "Marks";
            fsstaff.Sheets[0].Columns[0].Width = 50;
            fsstaff.Sheets[0].Columns[1].Width = 100;
            fsstaff.Sheets[0].Columns[2].Width = 200;
            fsstaff.Sheets[0].Columns[3].Width = 100;
            fsstaff.Sheets[0].Columns[4].Width = 200;
            fsstaff.Sheets[0].Columns[5].Width = 80;
            fsstaff.Sheets[0].Columns[0].Locked = true;
            fsstaff.Sheets[0].Columns[1].Locked = true;
            fsstaff.Sheets[0].Columns[2].Locked = true;
            fsstaff.Sheets[0].Columns[3].Locked = true;
            fsstaff.Sheets[0].Columns[4].Locked = true;
            fsstaff.Sheets[0].Columns[5].Locked = true;
        }
    }
    //started by annyutha
    protected void FpMarkEntry_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    protected void FpMarkEntry_SelectedIndexChanged(Object sender, EventArgs e)
    {

        if (Cellclick == true)
        {
            lblerroe1.Visible = true;
            Label2.Visible = false;
            TextBox1.Visible = true;
            btnExcel1.Visible = true;
            btnprintmaster1.Visible = true;
            TextBox1.Text = "";
            lblerrorstudent.Visible = false;
            fsstaff.Sheets[0].RowCount = 0;
            Hashtable studentdetail = new Hashtable();
            string activerow = "";
            string activecol = "";
            activerow = FpMarkEntry.ActiveSheetView.ActiveRow.ToString();
            activecol = FpMarkEntry.ActiveSheetView.ActiveColumn.ToString();
            int ar;
            int ac;
            ar = Convert.ToInt32(activerow.ToString());
            ac = Convert.ToInt32(activecol.ToString());
            string section2 = "";
            if (ddlSec.Enabled == true)
            {
                section2 = ddlSec.SelectedValue;
                if (section2.ToString() == "All" || section2.ToString() == "" || section2.ToString() == "-1")
                {
                    section2 = "";
                }
                else
                {
                    section2 = ddlSec.SelectedValue;
                }
            }
            if (ar != -1)
            {
                string column = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                if (column != "Range")
                {
                    string row = FpMarkEntry.Sheets[0].GetText(ar, 0);
                    string noofstudent = FpMarkEntry.Sheets[0].GetText(ar, ac);
                    string column2 = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, ac].Text;
                    if (row == "0-9" || row == "10-19" || row == "20-29" || row == "30-39" || row == "40-45" || row == "45-49" || row == "50-54" || row == "55-59" || row == "60-69" || row == "70-79" || row == "80-89" || row == "90-100")
                    {
                        string[] split = row.Split('-');
                        studentdetail.Clear();
                        studentdetail.Add("batchyear", ddlBatch.SelectedValue.ToString());
                        if (rdTestWise.Checked == true)
                        {
                            string column1 = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, ac].Note;

                            studentdetail.Add("subjectno", column1);
                            studentdetail.Add("criteriano", ddlTest.SelectedValue.ToString());
                            studentdetail.Add("exam_code", "");
                        }
                        else if (rdSubWise.Checked == true)
                        {
                            string column1 = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, ac - 1].Tag.ToString();
                            studentdetail.Add("subjectno", ddlTest.SelectedValue.ToString());
                            studentdetail.Add("criteriano", column1);
                            studentdetail.Add("exam_code", column1);
                        }
                        studentdetail.Add("strsec", section2.ToString());
                        studentdetail.Add("minmark", split[0].ToString());
                        studentdetail.Add("maxmark", split[1].ToString());
                        ds = d2.select_method("Proc_SubjectDisplayname", studentdetail, "sp");
                        mdl_full_employee_details.Show();
                        panel8.Visible = true;

                        if (rdTestWise.Checked == true)
                        {
                            Label1.Text = ddlTest.SelectedItem.ToString() + "-" + column2 + "- Range " + row + " Student Details";
                        }
                        else if (rdSubWise.Checked == true)
                        {
                            Label1.Text = column2 + "-" + ddlTest.SelectedItem.ToString() + "- Range " + row + " Student Details";
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            fsstaff.Visible = true;
                            int sno = 0;
                            for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                            {
                                sno++;
                                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                                string name = ds.Tables[0].Rows[rolcount]["Stud_Name"].ToString();
                                string code = ds.Tables[0].Rows[rolcount]["roll_no"].ToString();
                                string dep = ds.Tables[0].Rows[rolcount]["Dept_Name"].ToString();
                                string deg = ds.Tables[0].Rows[rolcount]["Course_Name"].ToString();
                                string mark = ds.Tables[0].Rows[rolcount]["marks_obtained"].ToString();
                                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = code;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = name;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].Text = deg;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 4].Text = dep;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 5].Text = mark;
                                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                fsstaff.Sheets[0].AutoPostBack = false;
                            }
                            int rowcount = fsstaff.Sheets[0].RowCount;
                            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                            fsstaff.SaveChanges();

                        }
                        else
                        {
                            lblerrorstudent.Visible = true;
                            lblerrorstudent.Text = "No Records Founds";
                            fsstaff.Visible = false;
                            TextBox1.Visible = false;
                            btnExcel1.Visible = false;
                            btnprintmaster1.Visible = false;
                            lblerroe1.Visible = false;
                        }
                    }
                    else
                    {
                        panel8.Visible = false;
                    }
                }
                else
                {
                    panel8.Visible = false;
                }
            }
            Cellclick = false;
        }
    }

    protected void exitpop_Click(object sender, EventArgs e)
    {
        panel8.Visible = false;
    }
    //end by annyutha
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
    //'----------------------- func for show pdf button
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpMarkEntry.FindControl("Update");
        Control cntCancelBtn = FpMarkEntry.FindControl("Cancel");
        Control cntCopyBtn = FpMarkEntry.FindControl("Copy");
        Control cntCutBtn = FpMarkEntry.FindControl("Clear");
        Control cntPasteBtn = FpMarkEntry.FindControl("Paste");
        Control cntPageNextBtn = FpMarkEntry.FindControl("Next");
        Control cntPagePreviousBtn = FpMarkEntry.FindControl("Prev");
        Control cntPagePrintBtn = FpMarkEntry.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePrintBtn.Parent;
            tr.Cells.Remove(tc);
        }
        base.Render(writer);
    }
    //---------------------------------------------------------------- function for bind the Batch
    public void BindBatch()
    {
        string sqlstr = "";
        int max_bat = 0;
        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "Batch_year";
            ddlBatch.DataValueField = "Batch_year";
            ddlBatch.DataBind();

            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstr));
            ddlBatch.SelectedValue = max_bat.ToString();
            // ddlBatch.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }
    //---------------------------------------------------------------- function for bind the degree
    public void BindDegree()
    {

        string collegecode = Session["collegecode"].ToString();
        //string degree = ddlDegree.SelectedValue.ToString();
        DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
        }
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

    }
    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        ddlBranch.Items.Clear();
        con.Close();
        con.Open();
        //string course_id = ddlDegree.SelectedValue.ToString();
        //string collegecode = Session["collegecode"].ToString();
        //string usercode = Session["UserCode"].ToString();
        //DataSet ds = Bind_Dept(course_id, collegecode, usercode);
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlBranch.DataSource = ds;
        //    ddlBranch.DataValueField = "degree_code"; 
        //    ddlBranch.DataTextField = "dept_name";
        //    ddlBranch.DataBind();
        //    con.Close();
        //}
        bindbranch();
        lblErrTest.Visible = false;
        if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
        {
            bindsec();
        }
        if (rdTestWise.Checked == true)
        {
            GetTest();
        }

    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        clear();

        if (!Page.IsPostBack == false)
        {
            ddlSemYr.Items.Clear();
        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                //  Get_Semester();
                bindsem();
                bindsec();
                if (rdTestWise.Checked == true)
                {
                    GetTest();
                }
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    //---------------------------------------------------------------- function for bind the semester
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
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
        }
    }
    //---------------------------------------------------------------- function for bind the Test
    public void GetTest()
    {
        myconn.Close();
        myconn.Open();
        string SyllabusYr;
        string SyllabusQry;
        if ((ddlBranch.SelectedValue.ToString() != "") && (ddlSemYr.SelectedValue.ToString() != "") && (ddlBatch.SelectedValue.ToString() != ""))
        {
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            if (SyllabusYr != "")
            {
                string Sqlstr;
                Sqlstr = "";
                Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, myconn);
                DataSet titles = new DataSet();
                myconn.Close();
                myconn.Open();
                sqlAdapter1.Fill(titles);
                ddlTest.DataSource = titles;
                ddlTest.DataValueField = "Criteria_No";
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataBind();
            }
        }
    }
    //---------------------------------------------------------------- function for bind the Subject
    public void GetSubject()
    {
        rdSubCode.Visible = true;
        rdSubName.Visible = true;
        myconn.Open();
        string SyllabusYr;
        string SyllabusQry;
        if ((ddlBranch.SelectedValue.ToString() != "") && (ddlSemYr.SelectedValue.ToString() != "") && (ddlBatch.SelectedValue.ToString() != ""))
        {
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSemYr.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = "";
            string sections = "";
            string strsec = "";
            sections = ddlSec.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + sections.ToString() + "'";
            }

            if (SyllabusYr != "")
            {
                if (Session["Staff_Code"].ToString() == "")
                {
                    Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 " + strsec.ToString() + " and exam_flag <> 'DEBAR'";
                }
                else if (Session["Staff_Code"].ToString() != "")
                {
                    Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,staff_selector,usermaster,registration,subjectchooser,syllabus_master where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and  subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and  syllabus_master.semester=" + ddlSemYr.SelectedValue.ToString() + " and syllabus_master.batch_year= " + ddlBatch.SelectedValue.ToString() + " and syllabus_master.syllabus_year= " + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and  registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSemYr.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and staff_selector.staff_code=usermaster.staff_code and subject.subject_no=staff_selector.subject_no and usermaster.staff_code='" + Session["Staff_Code"].ToString() + "'" + strsec.ToString() + "";
                }
                if (Sqlstr != "")
                {
                    SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, myconn);
                    DataSet titles = new DataSet();
                    myconn.Close();
                    myconn.Open();
                    sqlAdapter1.Fill(titles);
                    ddlTest.DataSource = titles;
                    ddlTest.DataValueField = "Subject_No";
                    ddlTest.DataTextField = "Subject_Name";
                    ddlTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- -Select- -", "-1"));
                    ddlTest.DataBind();
                }
            }
            myconn.Close();
        }
    }

    public void bindsem()
    {

        //--------------------semester load
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
        lblErrTest.Visible = false;
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        con.Close();
    }
    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }
    //---------------------------------------------------------------- function for bind the Section
    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //    ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
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
            ddlSec.Enabled = false;

        }
        lblErrTest.Visible = false;
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //---------------

    }
    ////---------------------------------------------------------------- Function defn for getfunction
    public string GetFunction(string sqlQuery)
    {
        string sqlstr = "";
        sqlstr = sqlQuery;
        getconn.Close();
        getconn.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getconn);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = getconn;
        drnew = funcmd.ExecuteReader();
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
    //-----------------------------------------------------------
    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        lblErrTest.Visible = false;
        rdSubWise_CheckedChanged(sender, e);
        BindSectionDetail();
        if (rdTestWise.Checked == true)
        {
            GetTest();
        }

    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        //binddegree();
        if (ddlDegree.Text != "")
        {
            //bindbranch();

            //bindsem();

            //bindsec();
        }
        else
        {
            lblnorec.Text = "Give degree rights to the staff";
            lblnorec.Visible = true;
        }
        lblErrTest.Visible = false;
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013        
        string reportname = txtexcelname.Text;
        if (reportname.ToString() != "")
        {
            d2.printexcelreport(FpMarkEntry, reportname);
            lblerr.Visible = false;
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013        
        string reportname = TextBox1.Text;
        if (reportname.ToString() != "")
        {
            d2.printexcelreport(fsstaff, reportname);
            Label2.Visible = false;
        }
        else
        {
            Label2.Text = "Please Enter Your Report Name";
            Label2.Visible = true;
            panel8.Visible = true;
            mdl_full_employee_details.Show();
        }
    }
    //------------------------------------------------------- Defn for Go Button
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnExcel.Visible = true;
            Button1.Visible = true;
            FpMarkEntry.Visible = true;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            // pnlSubject.Visible = true;
            rdSubCode.Visible = true;
            rdSubName.Visible = true;
            lblnorec.Visible = false;
            FpMarkEntry.Sheets[0].RowCount = 23;
            FpMarkEntry.Sheets[0].ColumnCount = 1;
            FpMarkEntry.Sheets[0].Columns[0].Width = 100;
            FpMarkEntry.Sheets[0].ColumnHeader.RowCount = 1;//rowcount 7- 02.03.12

            FpMarkEntry.Sheets[0].AutoPostBack = true;
            FpMarkEntry.CommandBar.Visible = true;
            FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
            MyStyle.Font.Bold = true;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpMarkEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpMarkEntry.Sheets[0].AllowTableCorner = true;
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].SheetCorner.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;

            //FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;

            FpMarkEntry.Sheets[0].FrozenColumnCount = 2;

            FpMarkEntry.ActiveSheetView.ColumnHeader.DefaultStyle = MyStyle;
            FpMarkEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMarkEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpMarkEntry.Sheets[0].RowHeader.Width = 50;



            FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.Black;
            FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorTop = Color.Black;
            //FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Background = Color.AliceBlue;

            FpMarkEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpMarkEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(MyStyle);
            FpMarkEntry.Sheets[0].AllowTableCorner = true;


            //'-------------------------------------------------------------------------------
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Range";
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
            FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            FpMarkEntry.Sheets[0].Cells[0, 0].Text = "0-9";
            FpMarkEntry.Sheets[0].Cells[1, 0].Text = "10-19";
            FpMarkEntry.Sheets[0].Cells[2, 0].Text = "20-29";
            FpMarkEntry.Sheets[0].Cells[3, 0].Text = "30-39";
            FpMarkEntry.Sheets[0].Cells[4, 0].Text = "40-45";
            FpMarkEntry.Sheets[0].Cells[5, 0].Text = "45-49";
            FpMarkEntry.Sheets[0].Cells[6, 0].Text = "50-54";
            FpMarkEntry.Sheets[0].Cells[7, 0].Text = "55-59";
            FpMarkEntry.Sheets[0].Cells[8, 0].Text = "60-69";
            FpMarkEntry.Sheets[0].Cells[9, 0].Text = "70-79";
            FpMarkEntry.Sheets[0].Cells[10, 0].Text = "80-89";
            FpMarkEntry.Sheets[0].Cells[11, 0].Text = "90-100";
            FpMarkEntry.Sheets[0].Cells[12, 0].Text = "Total No Of Candidates";
            FpMarkEntry.Sheets[0].Cells[13, 0].Text = "Presentees";
            FpMarkEntry.Sheets[0].Cells[14, 0].Text = "Absentees";
            FpMarkEntry.Sheets[0].Cells[15, 0].Text = "Leave";
            FpMarkEntry.Sheets[0].Cells[16, 0].Text = "OD";
            FpMarkEntry.Sheets[0].Cells[17, 0].Text = "EOD";
            FpMarkEntry.Sheets[0].Cells[18, 0].Text = "Passed";
            FpMarkEntry.Sheets[0].Cells[19, 0].Text = "Failed";
            FpMarkEntry.Sheets[0].Cells[20, 0].Text = "Average";
            FpMarkEntry.Sheets[0].Cells[21, 0].Text = "Pass %";
            FpMarkEntry.Sheets[0].Cells[22, 0].Text = "Staff";

            FpMarkEntry.Sheets[0].Cells[0, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[1, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[2, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[3, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[4, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[5, 0].ForeColor = Color.Red;
            FpMarkEntry.Sheets[0].Cells[19, 0].ForeColor = Color.Red;

            FpMarkEntry.Sheets[0].Rows[0].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[2].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[3].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[4].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[5].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[6].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[7].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[8].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[9].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[10].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[11].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[12].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[13].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[14].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[15].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[16].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[17].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[18].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[19].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[20].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[21].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].Rows[22].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.Black;
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.Black;
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
            FpMarkEntry.Sheets[0].SheetCorner.Columns[0].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SheetName = " ";

            string subjectno = "";
            FpMarkEntry.Sheets[0].RowCount = 23;
            FpMarkEntry.Sheets[0].ColumnCount = 1;
            Session["sheetcorner"] = FpMarkEntry.Sheets[0].SheetCorner.RowCount;
            DegCode = ddlBranch.SelectedValue.ToString();
            semester = ddlSemYr.SelectedValue.ToString();
            batchyear = ddlBatch.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();


            if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }

            if (DegCode != "")
            {
                bindstud = "select distinct registration.Roll_No as RollNumber, registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + DegCode + "   and registration.batch_year=" + batchyear + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " order by  roll_no ";
                myconn.Close();
                myconn.Open();
                SqlCommand cmdstud = new SqlCommand(bindstud, myconn);
                SqlDataAdapter dastud = new SqlDataAdapter(cmdstud);
                dastud.Fill(ds5);
            }
            //------------------------------------------
            if ((rdTestWise.Checked != true) || (rdSubWise.Checked != true))
            {
                lblErrTest.Visible = true;
                FpMarkEntry.Visible = false;
                btnExcel.Visible = false;
                Button1.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
            }
            //---------------------------------------------- Condition for select the TestWise or SubjectWise
            if (rdTestWise.Checked == true)
            {
                btnExcel.Visible = true;
                Button1.Visible = true;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                FpMarkEntry.Visible = true;
                lblErrTest.Visible = false;
                //  pnlSubject.Visible = true;
                rdSubCode.Visible = true;
                rdSubName.Visible = true;
                //'------------------ calling the func for setsub----load the subject
                SetSub();

                for (int head = 1; head <= FpMarkEntry.Sheets[0].ColumnCount - 1; head++)
                {
                    FpMarkEntry.Sheets[0].RowCount += 1;

                    subjectno = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, head].Note;
                    FpMarkEntry.Sheets[0].SetText(FpMarkEntry.Sheets[0].RowCount - 1, 0, GetFunction("select subject_code from subject where subject_no='" + Convert.ToInt32(subjectno.ToString()) + "'") + "-" + GetFunction("select subject_name from subject where subject_no='" + Convert.ToInt32(subjectno.ToString()) + "'"));
                    FpMarkEntry.Sheets[0].SpanModel.Add(FpMarkEntry.Sheets[0].RowCount - 1, 0, 1, FpMarkEntry.Sheets[0].ColumnCount);
                    FpMarkEntry.Sheets[0].Rows[FpMarkEntry.Sheets[0].RowCount - 1].Border.BorderColor = Color.Black;

                }

            }
            else if (rdSubWise.Checked == true)
            {
                btnExcel.Visible = true;
                Button1.Visible = true;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                FpMarkEntry.Visible = true;
                lblErrTest.Visible = false;
                //  pnlSubject.Visible = true;
                rdSubCode.Visible = true;
                rdSubName.Visible = true;

                //'-------------------calling the func for load test
                TestLoad();

            }


            FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.Black;

            if (FpMarkEntry.Sheets[0].ColumnCount > 1)
            {

                FpMarkEntry.Width = FpMarkEntry.Sheets[0].ColumnCount * 120;
            }
            else
            {
                FpMarkEntry.Visible = false;
                btnExcel.Visible = false;
                Button1.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                if (lblErrTest.Visible == false)
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Record(s) Found";
                }
                FpMarkEntry.Width = FpMarkEntry.Sheets[0].ColumnCount * 100;
            }
            if (Convert.ToInt32(FpMarkEntry.Sheets[0].ColumnCount) == 2)
            {
                FpMarkEntry.Sheets[0].Columns[0].Width = 200;
                FpMarkEntry.Width = 450;
            }
            FpMarkEntry.Width = FpMarkEntry.Sheets[0].ColumnCount * 120 + 200;

            //-----------------------new added 05.04.12
            collnamenew1 = "";
            address1 = "";
            address2 = "";
            address = "";
            Phoneno = "";
            Faxno = "";
            phnfax = "";
            district = "";
            email = "";

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
                        address = address1 + "-" + address2 + "-" + district;
                        Phoneno = collegename["phoneno"].ToString();
                        Faxno = collegename["faxno"].ToString();
                        phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                        email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                    }
                }
                con.Close();
            }


            FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Visible = true;

        }
        catch
        {

        }
    }
    //--------------------------------------------------- for display the records whether subjectcode wise or subject name wise
    public void SetSub()
    {

        FpMarkEntry.Sheets[0].ColumnCount = 1;

        string sections = "";
        string strsec = "";
        string sqlsyll = "";
        string batchyear = "";
        string semester = "";
        string DegCode = "";
        DegCode = ddlBranch.SelectedValue.ToString();
        semester = ddlSemYr.SelectedValue.ToString();
        batchyear = ddlBatch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();

        //'---------------------------------------------- chk the condition for section
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and sections='" + sections.ToString() + "'";
        }
        int Syll_Code;
        string sqlexam = "";
        string subjectno = "";
        string subjectname = "";


        string subjectcode = "";

        if (rdSubCode.Checked == true)//'-------------- chk the condition whether subcode or subname
        {

            // sqlsyll = "select * from syllabus_master where degree_code in (select degree_code from degree where degree_code = " + ddlBranch.DataValueField + " And college_code=" + Session["collegecode"] + ")  and semester =" + semester + " and batch_year =" + batchyear + "";
            Syll_Code = GiveSyllCode(Convert.ToInt32(DegCode.ToString()), Convert.ToInt32(semester.ToString()), Convert.ToInt32(batchyear.ToString()));
            if (ddlTest.SelectedValue.ToString() != "")
            {
                sqlexam = "select distinct s.subject_no ,s.subject_code , s.subject_name,exam_type.exam_date,exam_type.exam_code as examcode from subject as s,subjectchooser as sc,exam_type  where s.subject_no = sc.subject_no and s.subject_no=exam_type.subject_no and s.syll_code =" + Syll_Code + " " + strsec + " and sc.semester = " + semester + "   and exam_type.criteria_no='" + ddlTest.SelectedValue + "' order by exam_type.exam_date";
                subconn.Close();
                subconn.Open();
                SqlCommand cmdsyll = new SqlCommand(sqlexam, subconn);
                SqlDataReader drsyllb;
                drsyllb = cmdsyll.ExecuteReader();

                while (drsyllb.Read())
                {
                    dtpFrom = drsyllb["exam_date"].ToString();
                    exam_code = drsyllb["examcode"].ToString();
                    string[] spldate = dtpFrom.Split(new char[] { ' ' });
                    newdate = spldate[0].ToString();
                    if (drsyllb.HasRows == true)
                    {
                        FpMarkEntry.Sheets[0].ColumnCount += 1;
                        FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 170;
                        string[] split_d = newdate.Split(new char[] { '/' });
                        str_day = split_d[1].ToString();
                        Atmonth = split_d[0].ToString();
                        Atyear = split_d[2].ToString();
                        string[] split_yr = split_d[2].Split(new char[] { '/' });
                        string[] YrFinal = split_yr[0].Split(new char[] { ' ' });
                        string Atyr = YrFinal[0].ToString();
                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyr) * 12);

                        string newdateconcat = "";
                        newdateconcat = split_d[1] + "/" + split_d[0] + "/" + split_d[2];

                        subjectno = Convert.ToString(drsyllb["subject_no"]);
                        subjectcode = Convert.ToString(drsyllb["subject_code"]);
                        //FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 100;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Tag = exam_code;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = str_day;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = dtpFrom;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = strdate.ToString();

                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = subjectno;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Text = subjectcode + " " + newdateconcat;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        int lmark = Convert.ToInt32(subjectno);

                        //'--------------------------------------- fun for load the marks
                        LoadMarks(lmark);


                    }
                }
            }
        }
        else if (rdSubName.Checked == true)//'---------------- chk the condition whether subcode or subname
        {
            //   sqlsyll = "select * from syllabus_master where degree_code in (select degree_code from degree where degree_code = " + ddlBranch.DataValueField + " And college_code=" + Session["collegecode"] + ")  and semester =" + semester + " and batch_year =" + batchyear + "";
            Syll_Code = GiveSyllCode(Convert.ToInt32(DegCode.ToString()), Convert.ToInt32(semester.ToString()), Convert.ToInt32(batchyear.ToString()));
            //If Syll_Code = -1 Then Exit Sub
            if (ddlTest.SelectedValue.ToString() != "")
            {
                sqlexam = "select distinct exam_type.subject_no ,s.subject_code , s.subject_name,exam_type.exam_date,exam_type.exam_code as examcode from subject as s,subjectchooser as sc,exam_type  where s.subject_no = sc.subject_no and s.subject_no=exam_type.subject_no and s.syll_code =" + Syll_Code + " " + strsec + " and sc.semester = " + semester + "   and exam_type.criteria_no=" + ddlTest.SelectedValue + " order by exam_type.exam_date";
                subconn.Close();
                subconn.Open();
                SqlCommand cmdexam = new SqlCommand(sqlexam, subconn);
                SqlDataReader drexam;
                drexam = cmdexam.ExecuteReader();
                while (drexam.Read())
                {
                    exam_code = drexam["examcode"].ToString();
                    dtpFrom = drexam["exam_date"].ToString();
                    string[] spldate = dtpFrom.Split(new char[] { ' ' });
                    newdate = spldate[0].ToString();
                    if (drexam.HasRows == true)
                    {
                        subjectname = "";
                        FpMarkEntry.Sheets[0].ColumnCount += 1;
                        FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 100;
                        string[] split_d = newdate.Split(new char[] { '/' });
                        str_day = split_d[1].ToString();
                        Atmonth = split_d[0].ToString();
                        Atyear = split_d[2].ToString();

                        string newdateconcat = "";
                        newdateconcat = split_d[1] + "/" + split_d[0] + "/" + split_d[2];

                        string[] split_yr = split_d[2].Split(new char[] { '/' });
                        string[] YrFinal = split_yr[0].Split(new char[] { ' ' });
                        string Atyr = YrFinal[0].ToString();
                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyr) * 12);
                        subjectno = Convert.ToString(drexam["subject_no"]);

                        subjectname = "";
                        subjectname = Convert.ToString(drexam["subject_name"]);
                        FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 270;
                        FpMarkEntry.Sheets[0].Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = subjectname;

                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Tag = exam_code;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = str_day;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = dtpFrom;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = strdate.ToString();

                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = subjectno;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Text = subjectname + " " + newdateconcat;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        int lmark = Convert.ToInt32(subjectno);
                        //'----------------------------- fun def for load the marks
                        LoadMarks(lmark);



                    }
                }
            }
        }

        FpMarkEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.Black;
        //FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
    }
    //------------------------------------------------ Func defn for TestLoad 
    public void TestLoad()
    {
        rdSubCode.Visible = true;
        rdSubName.Visible = true;
        string batchyear = "";
        string semester = "";

        string sections = "";
        string strsec = "";
        string subjectno = "";

        sections = ddlSec.SelectedValue.ToString();
        //'---------------------------------------------- chk the condition for section
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and sections='" + sections.ToString() + "'";
        }


        semester = ddlSemYr.SelectedValue.ToString();
        batchyear = ddlBatch.SelectedValue.ToString();
        subjectno = ddlTest.SelectedValue.ToString();


        //'--------------------------------------------------------------------------------
        FpMarkEntry.Sheets[0].ColumnCount = 1;
        string varsyllcode1 = "";
        varsyllcode1 = GetFunction("Select Syll_Code from Syllabus_Master where Degree_Code =" + ddlBranch.SelectedValue + " and Semester = " + semester + " and Batch_Year = " + batchyear + "");
        if (subjectno != "")
        {
            //'------------------------------------ load the test

            testconn.Close();
            testconn.Open();
            SqlCommand cmdvarsyll = new SqlCommand("Proc_TestLoad", testconn);
            cmdvarsyll.CommandType = CommandType.StoredProcedure;
            cmdvarsyll.Parameters.Add("@subjectno", SqlDbType.NVarChar).Value = subjectno.ToString();
            cmdvarsyll.Parameters.Add("@strsec", SqlDbType.NVarChar).Value = sections.ToString();
            cmdvarsyll.Parameters.Add("@syllcode", SqlDbType.NVarChar).Value = varsyllcode1.ToString();

            SqlDataReader drvarsyll;
            drvarsyll = cmdvarsyll.ExecuteReader();
            while (drvarsyll.Read())
            {
                exam_code = drvarsyll["examcode"].ToString();
                dtpFrom = drvarsyll["exam_date"].ToString();
                string[] spldate = dtpFrom.Split(new char[] { ' ' });
                newdate = spldate[0].ToString();

                string criteria = "";
                criteria = drvarsyll["criteria"].ToString();
                if (drvarsyll.HasRows == true)
                {
                    string[] split_d = newdate.Split(new char[] { '/' });
                    str_day = split_d[1].ToString();
                    Atmonth = split_d[0].ToString();
                    Atyear = split_d[2].ToString();
                    string newdateconcat = "";
                    newdateconcat = split_d[1] + "/" + split_d[0] + "/" + split_d[2];
                    string[] split_yr = split_d[2].Split(new char[] { '/' });
                    string[] YrFinal = split_yr[0].Split(new char[] { ' ' });
                    string Atyr = YrFinal[0].ToString();
                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyr) * 12);
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Tag = exam_code;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = str_day;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = dtpFrom;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = strdate.ToString();

                    FpMarkEntry.Sheets[0].ColumnCount += 1;
                    FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 100;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Text = criteria + " " + newdateconcat;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    mrkcriteria = drvarsyll["criteria_no"].ToString();
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note = mrkcriteria;
                    MarksLoad(Convert.ToInt32(mrkcriteria));

                    //MyImg mi3 = new MyImg();
                    //mi3.ImageUrl = "Handler/Handler2.ashx?";
                    ////'------------------span the 3 rows to display the img----------------
                    //FpMarkEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 9, 1);
                    //FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].CellType = mi3;
                    //FpMarkEntry.Sheets[0].SheetCorner.Columns[0].Width = 250;
                    //FpMarkEntry.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColor = Color.Black;

                    //MyImg1 mi4 = new MyImg1();
                    //mi4.ImageUrl = "Handler/Handler5.ashx?";
                    //FpMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, FpMarkEntry.Sheets[0].ColumnCount - 1, 9, 1);
                    //FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].CellType = mi4;
                    //FpMarkEntry.Sheets[0].Columns[FpMarkEntry.Sheets[0].ColumnCount - 1].Width = 100;
                    //FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
                }
                else
                {
                    lblnorec.Visible = true;
                }
            }
        }


    }

    ////-------------------------------------------------------- Func defn for MarksLoad 
    public void MarksLoad(int crt_no)
    {
        rdSubCode.Visible = true;
        rdSubName.Visible = true;
        string batchyear = "";
        string sections = "";
        string strsec = "";
        string subjectno = "";
        string degcode = "";
        int totfailcnt = 0;

        degcode = ddlBranch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and exam_type.sections='" + sections.ToString() + "'";
        }

        subjectno = ddlTest.SelectedValue.ToString();
        semester = ddlSemYr.SelectedValue.ToString();
        batchyear = ddlBatch.SelectedValue.ToString();
        exam_code = Convert.ToString(FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 2].Tag);
        mrkcriteria = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note;

        hat.Clear();
        hat.Add("batchyear", batchyear.ToString());
        hat.Add("subjectno", subjectno.ToString());
        hat.Add("criteriano", mrkcriteria.ToString());
        hat.Add("strsec", sections.ToString());
        ds = d2.select_method("Proc_SubjectDisplay", hat, "sp");

        //'--------------------------------------------to display the count of the marks (0-9)----------------
        i = 0;
        fail_count = 0;
        pass_count = 0;
        if (ds.Tables[0].Rows.Count != 0)
        {

            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[0].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[0].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (10-19)----------------

        if (ds.Tables[1].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[1].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[1].Rows[0]["MarkCt"].ToString());
        }
        //'--------------------------------------------to display the count of the marks (20-29)----------------
        if (ds.Tables[2].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[2].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[2].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (30-39)----------------
        if (ds.Tables[3].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[3].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[3].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (40-45)----------------
        if (ds.Tables[4].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[4].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[4].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (45-49)----------------
        if (ds.Tables[5].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[5].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            fail_count += int.Parse(ds.Tables[5].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (50-54)----------------
        if (ds.Tables[6].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[6].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[6].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (55-59)----------------
        if (ds.Tables[7].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[7].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[7].Rows[0]["MarkCt"].ToString());
        }
        //    //'--------------------------------------------to display the count of the marks (60-69)----------------
        if (ds.Tables[8].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[8].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[8].Rows[0]["MarkCt"].ToString());
        }

        //    //'--------------------------------------------to display the count of the marks (70-79)----------------
        if (ds.Tables[9].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[9].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[9].Rows[0]["MarkCt"].ToString());
        }

        //    //'--------------------------------------------to display the count of the marks (80-89)----------------
        if (ds.Tables[10].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[10].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[10].Rows[0]["MarkCt"].ToString());
        }

        //    //'--------------------------------------------to display the count of the marks (90-100)----------------
        if (ds.Tables[11].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[11].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;
            pass_count += int.Parse(ds.Tables[11].Rows[0]["MarkCt"].ToString());
        }
        ////'-------------------------------------------- to calculate the no.of students-------------------------------

        if (ds.Tables[12].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[12].Rows[0]["MarkCt"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            tot_cal_stu = int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString());
            i++;
        }

        //'---------------------------------------to calculate the presentees


        int totpasscount = Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());
        totpasscount = totpasscount + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString());
        totfailcnt = pass_count + totpasscount;
        totpasscount = totpasscount + pass_count + fail_count;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, (totpasscount).ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        i++;

        //'---------------------------------------to calculate the absentees

        no_of_absent = double.Parse((pass_count + fail_count).ToString());
        //no_of_absent += double.Parse(ds.Tables[15].Rows[0]["LEAVE"].ToString());
        // no_of_absent += double.Parse(ds.Tables[13].Rows[0]["EL"].ToString());
        no_of_absent += double.Parse(ds.Tables[14].Rows[0]["EOD"].ToString());
        //no_of_absent += double.Parse(ds.Tables[17].Rows[0]["OD"].ToString());
        no_of_absent += double.Parse(ds.Tables[18].Rows[0]["NJ"].ToString());

        no_of_absent = double.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - double.Parse(no_of_absent.ToString());
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, no_of_absent.ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        //'---------------------------------------to calculate the leave
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[15].Rows[0]["LEAVE"].ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        //'---------------------------------------to calculate the od
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[17].Rows[0]["OD"].ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        //'---------------------------------------to calculate the eod
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[14].Rows[0]["EOD"].ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        //-------------------------------------------Pass 

        //modified by srinath 15/5/2014
        string sect = "";
        if (sections.ToString() != "All" && sections.ToString() != "" && sections.ToString() != "-1")
        {
            sect = " and e.Sections='" + sections + "' and reg.Sections=e.Sections";
        }
        string passval = "select count(distinct r.roll_no) from result r,exam_type e,registration reg where r.roll_no=reg.Roll_No and r.exam_code=e.exam_code and reg.Batch_Year=e.batch_year and e.subject_no='" + subjectno + "' and e.Batch_Year='" + batchyear + "' and e.criteria_no='" + mrkcriteria + "' " + sect + " and reg.CC=0 and reg.DelFlag=0 and r.marks_obtained>=e.min_mark and reg.Exam_Flag<>'debar'  ";
        int subtotpass = Convert.ToInt32(d2.GetFunction(passval));

        //int tot = pass_count + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString()) + Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());
        int tot = subtotpass + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString()) + Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, tot.ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        // }

        //    //-----------------------------------------Fail
        //modified by srinath 15/5/2014
        int totfails = int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - tot;
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, totfails.ToString());
        // FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, (fail_count + no_of_absent + int.Parse(ds.Tables[17].Rows[0]["OD"].ToString()) + int.Parse(ds.Tables[18].Rows[0]["NJ"].ToString())).ToString());
        //FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ((int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - totfailcnt)).ToString());

        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        //    //'-----------------------------------------------for avg and pass perc----------------------------

        if (ds.Tables[16].Rows.Count != 0)
        {
            pass_fail_tot_count = pass_count + fail_count;
            class_avg = double.Parse(ds.Tables[16].Rows[0]["MarkCt"].ToString()) / double.Parse(pass_fail_tot_count.ToString());
            class_avg = Math.Round(class_avg, 2);

            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, class_avg.ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;
        }
        st_avg_pass = tot_cal_stu.ToString();

        // avg_pass = double.Parse(pass_count.ToString()) / double.Parse(pass_fail_tot_count.ToString());
        if (chkIncludeAbsent.Checked)
            avg_pass = double.Parse(tot.ToString()) / double.Parse(pass_fail_tot_count.ToString() + no_of_absent);
        else
            avg_pass = double.Parse(tot.ToString()) / double.Parse(pass_fail_tot_count.ToString());
        avg_pass = avg_pass * 100;
        avg_pass = Math.Round(avg_pass, 2);
        FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, avg_pass.ToString());
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
        i++;
        if (ds.Tables[19].Rows.Count != 0)
        {
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[19].Rows[0]["STAFF_NAME"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;
        }
    }
    //----------------------------------------------------------------- Func defn for GiveSyllCode 
    public int GiveSyllCode(int DegCode, int Semester, int Batch)
    {
        myconn.Close();
        myconn.Open();
        string sqlsyllcode = "";
        int GiveSyllCode;
        GiveSyllCode = -1;
        sqlsyllcode = "Select Syll_Code from Syllabus_master where Degree_Code = " + DegCode + " and Semester = " + Semester + " and Batch_Year = " + Batch + "";
        SqlCommand cmdsyll = new SqlCommand(sqlsyllcode, myconn);
        SqlDataReader drsyll;
        drsyll = cmdsyll.ExecuteReader();
        drsyll.Read();
        if (drsyll.HasRows == true)
        {
            if (drsyll[0].ToString() != "\0")
            {
                GiveSyllCode = Convert.ToInt32(drsyll["Syll_Code"].ToString());
            }
        }
        return GiveSyllCode;
    }

    //-------------------------------------------------------------------- Func defn for LoadMarks 
    public void LoadMarks(long mark)//the parameter s a subno
    {

        //   TestLoad();
        string batch_year = "";
        string sections = "";
        string strsec = "";
        string degcode = "";
        int totfailcnt = 0;
        semester = ddlSemYr.SelectedValue.ToString();
        degcode = ddlBranch.SelectedValue.ToString();
        batch_year = ddlBatch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and exam_type.sections='" + sections.ToString() + "'";
        }

        exam_code = Convert.ToString(FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Tag);
        mrkcriteria = FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, FpMarkEntry.Sheets[0].ColumnCount - 1].Note;
        hat.Clear();
        hat.Add("criteriano", ddlTest.SelectedValue.ToString());
        hat.Add("strsec", sections.ToString());
        ds1 = d2.select_method("Proc_Test_Display", hat, "sp");
        if (ds1.Tables[0].Rows.Count != 0)
        {
            hat.Clear();
            hat.Add("batchyear", ddlBatch.SelectedValue.ToString());
            //hat.Add("subjectno", ds1.Tables[0].Rows[sub_count]["subject_no"].ToString());
            hat.Add("subjectno", mrkcriteria.ToString());
            hat.Add("criteriano", ddlTest.SelectedValue.ToString());
            hat.Add("strsec", sections.ToString());

            ds = d2.select_method("Proc_SubjectDisplay", hat, "sp");

            //'--------------------------------------------to display the count of the marks (0-9)----------------
            i = 0;
            fail_count = 0;
            pass_count = 0;
            if (ds.Tables[0].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[0].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[0].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (10-19)----------------

            if (ds.Tables[1].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[1].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[1].Rows[0]["MarkCt"].ToString());
            }
            //'--------------------------------------------to display the count of the marks (20-29)----------------
            if (ds.Tables[2].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[2].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[2].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (30-39)----------------
            if (ds.Tables[3].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[3].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[3].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (40-45)----------------
            if (ds.Tables[4].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[4].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[4].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (45-49)----------------
            if (ds.Tables[5].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[5].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                fail_count += int.Parse(ds.Tables[5].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (50-54)----------------
            if (ds.Tables[6].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[6].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[6].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (55-59)----------------
            if (ds.Tables[7].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[7].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[7].Rows[0]["MarkCt"].ToString());
            }
            //    //'--------------------------------------------to display the count of the marks (60-69)----------------
            if (ds.Tables[8].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[8].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[8].Rows[0]["MarkCt"].ToString());
            }

            //    //'--------------------------------------------to display the count of the marks (70-79)----------------
            if (ds.Tables[9].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[9].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[9].Rows[0]["MarkCt"].ToString());
            }

            //    //'--------------------------------------------to display the count of the marks (80-89)----------------
            if (ds.Tables[10].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[10].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[10].Rows[0]["MarkCt"].ToString());
            }

            //    //'--------------------------------------------to display the count of the marks (90-100)----------------
            if (ds.Tables[11].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[11].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                i++;
                pass_count += int.Parse(ds.Tables[11].Rows[0]["MarkCt"].ToString());
            }
            ////'-------------------------------------------- to calculate the no.of students-------------------------------

            if (ds.Tables[12].Rows.Count != 0)
            {
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[12].Rows[0]["MarkCt"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                tot_cal_stu = int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString());
                i++;
            }
            //'---------------------------------------to calculate the presentees

            int totpasscount = Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());

            totpasscount = totpasscount + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString());
            totfailcnt = pass_count;
            totpasscount = pass_count + fail_count;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, (totpasscount).ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            i++;



            //'---------------------------------------to calculate the absentees





            no_of_absent = double.Parse((pass_count + fail_count).ToString());
            no_of_absent += double.Parse(ds.Tables[15].Rows[0]["LEAVE"].ToString());
            // no_of_absent += double.Parse(ds.Tables[13].Rows[0]["EL"].ToString());
            no_of_absent += double.Parse(ds.Tables[14].Rows[0]["EOD"].ToString());
            no_of_absent += double.Parse(ds.Tables[17].Rows[0]["OD"].ToString());
            no_of_absent += double.Parse(ds.Tables[18].Rows[0]["NJ"].ToString());


            no_of_absent = double.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - double.Parse(no_of_absent.ToString());
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, no_of_absent.ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;

            //'---------------------------------------to calculate the leave

            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[15].Rows[0]["LEAVE"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;
            //'---------------------------------------to calculate the od

            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[17].Rows[0]["OD"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;
            //'---------------------------------------to calculate the eod

            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[14].Rows[0]["EOD"].ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;

            //'---------------------------------------to calculate the pass

            //modified by srinath 15/5/2014
            string sect = "";
            if (sections.ToString() != "All" && sections.ToString() != "" && sections.ToString() != "-1")
            {
                sect = " and e.Sections='" + sections + "' and reg.Sections=e.Sections";
            }
            string passval = "select count(distinct r.roll_no) from result r,exam_type e,registration reg where r.roll_no=reg.Roll_No and r.exam_code=e.exam_code and reg.Batch_Year=e.batch_year and e.subject_no='" + mrkcriteria + "' and e.Batch_Year='" + batchyear + "' and e.criteria_no='" + ddlTest.SelectedValue.ToString() + "' " + sect + " and reg.CC=0 and reg.DelFlag=0 and r.marks_obtained>=e.min_mark and reg.Exam_Flag<>'debar'  ";
            int subtotpass = Convert.ToInt32(d2.GetFunction(passval));

            //  int tot = pass_count + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString()) + Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());
            int tot = subtotpass + Convert.ToInt32(ds.Tables[14].Rows[0]["EOD"].ToString()) + Convert.ToInt32(ds.Tables[13].Rows[0]["EL"].ToString());
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, tot.ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;
            //-----------------------------------------Fail

            //modified by srinath 15/5/2014

            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ((int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - tot)).ToString());
            // FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, (int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - pass_count - no_of_absent).ToString());
            //FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ((int.Parse(ds.Tables[12].Rows[0]["MarkCt"].ToString()) - totfailcnt)).ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;


            //    //'-----------------------------------------------for avg and pass perc----------------------------

            if (ds.Tables[16].Rows.Count != 0)
            {
                pass_fail_tot_count = pass_count + fail_count;
                if (ds.Tables[16].Rows[0]["MarkCt"].ToString() != "")
                {
                    class_avg = double.Parse(ds.Tables[16].Rows[0]["MarkCt"].ToString()) / double.Parse(pass_fail_tot_count.ToString());
                }
                class_avg = Math.Round(class_avg, 2);
                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, class_avg.ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                i++;
            }

            //modified by srinath 15/5/2014
            st_avg_pass = tot_cal_stu.ToString();
            // avg_pass = double.Parse(pass_count.ToString()) / double.Parse(pass_fail_tot_count.ToString());
            avg_pass = double.Parse(tot.ToString()) / double.Parse(pass_fail_tot_count.ToString());
            avg_pass = avg_pass * 100;
            avg_pass = Math.Round(avg_pass, 2);
            FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, avg_pass.ToString());
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
            i++;


            if (ds.Tables[19].Rows.Count != 0)
            {

                FpMarkEntry.Sheets[0].SetText(i, FpMarkEntry.Sheets[0].ColumnCount - 1, ds.Tables[19].Rows[0]["STAFF_NAME"].ToString());
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpMarkEntry.Sheets[0].Cells[i, FpMarkEntry.Sheets[0].ColumnCount - 1].Border.BorderColor = Color.Black;
                i++;
            }
            sub_count++;
        }
    }
    //-----------------------------------------------func to get the hash key---------------------------------
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {

        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }

        return null;
    }
    //--------------------------------------------------------------find present absent--------------
    public string findabsentpresent(DateTime exam_date, string roll_no, string examcode, string subno)
    {
        double studpresn = 0;
        double studabsen = 0;
        double studod = 0;
        double studlev = 0;
        string srtprd = "";
        string hr = "";
        long monthyear = (Convert.ToInt64(exam_date.ToString("yyyy")) * 12) + Convert.ToInt64(exam_date.ToString("MM"));
        srtprd = GetFunction("select start_period from exam_type where exam_code='" + examcode + "'");

        if (srtprd != string.Empty)
        {

            lcon3.Open();
            string sqlhour;
            string strcalflag = "";
            sqlhour = "select d" + exam_date.Day + "d" + srtprd + "  from attendance where month_year='" + monthyear + "' and  roll_no='" + roll_no + "'";

            SqlCommand cmdhour = new SqlCommand(sqlhour, lcon3);
            SqlDataReader drhour;
            drhour = cmdhour.ExecuteReader();
            if (drhour.HasRows == true)
            {
                while (drhour.Read())
                {
                    hr = drhour[0].ToString();
                    if (hr != string.Empty)
                    {
                        strcalflag = GetFunction("select Calcflag from AttMasterSetting where LeaveCode='" + hr.ToString() + "'");
                    }
                    if ((hr == "1"))//calc present------------------
                    {
                        if ((strcalflag == "0") && (strcalflag != null) && (strcalflag != string.Empty))
                        {

                            studpresn += 1;
                            if (htpresent.Contains(Convert.ToInt32(subno)))
                            {
                                int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htpresent));
                                val++;//absent count
                                htpresent[Convert.ToInt32(subno)] = val;
                            }
                            else
                            {

                                htpresent.Add(Convert.ToInt32(subno), studpresn);

                            }
                        }
                    }
                    else//-------------calc absent------------------------
                    {

                        studabsen += 1;
                        if (htabsent.Contains(Convert.ToInt32(subno)))
                        {
                            int val = Convert.ToInt32(GetCorrespondingKey(Convert.ToInt32(subno), htabsent));
                            val++;//absent count
                            htabsent[Convert.ToInt32(subno)] = val;
                        }
                        else
                        {

                            htabsent.Add(Convert.ToInt32(subno), studabsen);

                        }
                    }

                    if ((hr == "3"))
                    {
                        studod += 1;
                    }
                    else if (hr == "10")
                    {
                        studlev += 1;
                    }

                }
            }

            drhour.Close();
            lcon3.Close();
        }


        string cat = studpresn.ToString() + "," + studabsen.ToString() + "," + studlev.ToString();
        return cat;
    }

    //-------------------------------------------------------------func defn for Result---------------
    public string result(string st)
    {
        myconn.Close();
        myconn.Open();
        string result = "";
        SqlDataReader drr;
        SqlCommand commmand = new SqlCommand(st, myconn);
        drr = commmand.ExecuteReader();
        drr.Read();
        if (drr.HasRows == true)
        {
            if (drr.GetValue(0).ToString() != "\0")
            {
                result = drr[0].ToString();
            }
            else
            {
                result = "0";
            }
        }
        else if (drr.HasRows == false)
        {
            result = "";
        }
        return result;
    }

    //----------------------------------------------------------------------- TestWise SelectedChanged
    protected void rdTestWise_CheckedChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpMarkEntry.Visible = false;
        lblErrTest.Visible = false;
        if (rdTestWise.Enabled == true)
        {
            // pnlSubject.Visible = true;
            rdSubName.Visible = true;
            rdSubCode.Visible = true;

            rdSubName.Enabled = true;
            rdSubCode.Enabled = true;

            rdSubCode.Checked = true;
            lblTest.Text = "Test";
            GetTest();
        }

        else if (rdSubWise.Enabled == true)
        {
            rdSubName.Visible = false;
            rdSubCode.Visible = false;
            // pnlSubject.Visible = false;
            lblTest.Text = "Subject";
            GetSubject();
        }
    }
    //-------------------------------------------------------------------------- SubjectWise SelectedChanged
    protected void rdSubWise_CheckedChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpMarkEntry.Visible = false;
        lblErrTest.Visible = false;
        if (rdSubWise.Enabled == true)
        {
            //  pnlSubject.Visible = true;
            rdSubName.Visible = true;
            rdSubCode.Visible = true;
            rdSubCode.Enabled = false;
            rdSubName.Enabled = false;
            lblTest.Text = "Subject";
            GetSubject();
        }

        else if (rdTestWise.Enabled == true)
        {
            //  pnlSubject.Visible = true;
            rdSubName.Visible = true;
            rdSubCode.Visible = true;
            rdSubCode.Enabled = true;
            rdSubName.Enabled = true;
            lblTest.Text = "Test";
            GetTest();
        }
    }


    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void rdSubName_CheckedChanged(object sender, EventArgs e)
    {
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }
    protected void ddlTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpMarkEntry.Visible = false;
        btnExcel.Visible = false;
        Button1.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
    }


    public void func_Print_Master_Setting()
    {
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "MarkEntry.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            for (int newlp = 0; newlp <= FpMarkEntry.Sheets[0].ColumnCount - 1; newlp++)
            {
                FpMarkEntry.Sheets[0].Columns[newlp].Visible = false;
            }
            FpMarkEntry.Sheets[0].ColumnHeader.Cells[6, 0].Text = " ";
            //FpMarkEntry.Sheets[0].ColumnHeader.Cells[7, 0].Text = " ";
            if ((dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != " ") && (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != ""))
            {
                string hdr_nam = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                string[] spl_nwhdrname = hdr_nam.Split(',');
                int strwindexcnt = 1;
                if (spl_nwhdrname.GetUpperBound(0) > 0)
                {
                    FpMarkEntry.Sheets[0].ColumnHeader.Rows[9].Visible = false;
                    int shtcnrrwcnt = spl_nwhdrname.GetUpperBound(0) + 2;
                    FpMarkEntry.Sheets[0].SheetCorner.RowCount += shtcnrrwcnt;
                    for (int strw = Convert.ToInt32(Session["sheetcorner"]); strw < FpMarkEntry.Sheets[0].SheetCorner.RowCount - 2; strw++)
                    {
                        if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
                        {
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Left;

                        }
                        else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
                        {
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Center;

                        }
                        else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
                        {
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].Text = spl_nwhdrname[strwindexcnt - 1].ToString();
                            FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].HorizontalAlign = HorizontalAlign.Right;

                        }
                        FpMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(strw, 0, 1, FpMarkEntry.Sheets[0].ColumnCount);
                        strwindexcnt++;

                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[strw, 0].Border.BorderColorBottom = Color.Black;
                    }
                }
                else
                {
                    FpMarkEntry.Sheets[0].SheetCorner.RowCount += 2;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Border.BorderColorBottom = Color.Black;
                    FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                    FpMarkEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0, 1, FpMarkEntry.Sheets[0].ColumnCount);
                    if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
                    {
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
                    {
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
                    {
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
                FpMarkEntry.Sheets[0].ColumnHeader.Rows[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 3].Visible = false;

            }

            string printvar = "";
            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();
            string[] split_printvar = printvar.Split(',');
            for (int newloop = 0; newloop <= FpMarkEntry.Sheets[0].ColumnCount - 1; newloop++)
            {
                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                {
                    if (FpMarkEntry.Sheets[0].ColumnHeader.Cells[0, newloop].Text == split_printvar[splval].ToString())
                    {
                        FpMarkEntry.Sheets[0].Columns[newloop].Visible = true;
                        final_print_col_cnt++;

                        FpMarkEntry.Sheets[0].SheetCorner.Cells[FpMarkEntry.Sheets[0].SheetCorner.RowCount - 1, 0].Text = "S.No";
                        FpMarkEntry.Sheets[0].SheetCorner.Cells[FpMarkEntry.Sheets[0].SheetCorner.RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 1, newloop].Text = split_printvar[splval].ToString();
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 1, newloop].HorizontalAlign = HorizontalAlign.Center;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 1, newloop].Border.BorderColorBottom = Color.Black;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 1, newloop].Border.BorderColorTop = Color.Black;
                        FpMarkEntry.Sheets[0].ColumnHeader.Cells[FpMarkEntry.Sheets[0].ColumnHeader.RowCount - 1, newloop].Border.BorderColor = Color.Black;
                    }
                }
            }


            //---------------------------------------------
            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
            {

                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
                FpMarkEntry.Sheets[0].RowCount++;
                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
                string[] footer_text_split = footer_text.Split(',');
                footer_text = "";

                if (final_print_col_cnt < footer_count)
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

                    for (int col_count = 0; col_count < FpMarkEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpMarkEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            FpMarkEntry.Sheets[0].SpanModel.Add((FpMarkEntry.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
                            FpMarkEntry.Sheets[0].Cells[(FpMarkEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text;
                            break;
                        }
                    }

                }

                else if (final_print_col_cnt == footer_count)
                {
                    for (int col_count = 0; col_count < FpMarkEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpMarkEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            FpMarkEntry.Sheets[0].Cells[(FpMarkEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
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
                    split_col_for_footer = final_print_col_cnt / footer_count;
                    footer_balanc_col = final_print_col_cnt % footer_count;

                    for (int col_count = 0; col_count < FpMarkEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpMarkEntry.Sheets[0].Columns[col_count].Visible == true)
                        {
                            if (temp_count == 0)
                            {
                                FpMarkEntry.Sheets[0].SpanModel.Add((FpMarkEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
                            }
                            else
                            {

                                FpMarkEntry.Sheets[0].SpanModel.Add((FpMarkEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

                            }
                            FpMarkEntry.Sheets[0].Cells[(FpMarkEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
                            //FpMarkEntry.Sheets[0].Cells[(FpMarkEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
                            //FpMarkEntry.Sheets[0].Cells[(FpMarkEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;


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
    }

    public void func_header()
    {

    }


    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string filt_details = "";
        Session["column_header_row_count"] = 1;
        if (ddlSec.Enabled == false)
        {
            filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString();
        }
        else
        {
            filt_details = ddlBatch.SelectedItem.ToString() + "-" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "-" + "Sem " + ddlSemYr.SelectedItem.ToString() + "-" + "Sec " + ddlSec.SelectedItem.ToString();
        }
        string test = "";
        if (rdTestWise.Checked == true)
        {
            test = "Test :" + ddlTest.SelectedItem.ToString();
        }
        else if (rdSubWise.Checked == true)
        {
            test = "Subject:" + ddlTest.SelectedItem.ToString();
        }
        string degreedetails = string.Empty;
        degreedetails = "CAM R3-Mark Analysis For Monthly/Model Examinations Report" + "@" + filt_details + "@" + test;
        string pagename = " ";
        pagename = "MarkEntry.aspx";

        Printcontrol.loadspreaddetails(FpMarkEntry, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dater = DateTime.Now.ToString();
            string[] split = dater.Split('/');
            string dateset = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();

            string degreedetails = "" + Label1.Text + " " + '@' + "Date :" + dateset;
            string pagename = "MarkEntry.aspx";
            Printcontrol.loadspreaddetails(fsstaff, pagename, degreedetails);
            Printcontrol.Visible = true;
            panel8.Visible = false;
        }
        catch (Exception ex)
        {
        }

    }
}

