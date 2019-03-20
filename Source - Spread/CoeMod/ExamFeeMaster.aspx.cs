using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using FarPoint.Web.Spread;

public partial class ExamFeeMaster : System.Web.UI.Page
{
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string led_codes, led_codes_str;
    string CollegeCode;
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    DataSet ds_load = new DataSet();
    DAccess2 daccess2 = new DAccess2();
    static string newdate = "";
    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(110);
            //img.Height = Unit.Percentage(80);
            return img;


        }
    }
    public class MyImg1 : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(80);
            img.Height = Unit.Percentage(90);
            return img;


        }
    }
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
            if (!Page.IsPostBack)
            {
                // txtapplfee.MaxLength = 6;
                txtregexmfee.MaxLength = 6;
                txtArrexmfee.MaxLength = 6;
                txtretotfee.MaxLength = 6;
                txtrevalfee.MaxLength = 6;
                txtrechalfee.MaxLength = 6;
                DateTime currentdate = System.DateTime.Now;
                newdate = currentdate.ToString();
                sprdexamfee.Visible = false;
                sprdexamfee.CommandBar.Visible = false;
                sprdexamfee.Sheets[0].AutoPostBack = false;
                sprdexamfee.Sheets[0].SheetCorner.RowCount = 1;
                sprdexamfee.Sheets[0].RowHeader.Visible = false;
                sprdexamfee.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdexamfee.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdexamfee.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdexamfee.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdexamfee.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdexamfee.Sheets[0].DefaultStyle.Font.Bold = false;
                sprdexamfee.Sheets[0].ColumnCount = 11;
                sprdexamfee.Sheets[0].Columns[0].Locked = true;
                sprdexamfee.Sheets[0].Columns[1].Locked = true;
                sprdexamfee.Sheets[0].Columns[2].Locked = true;
                sprdexamfee.Sheets[0].Columns[3].Locked = true;
                sprdexamfee.Sheets[0].Columns[4].Locked = true;
                sprdexamfee.Sheets[0].Columns[0].Width = 80;
                sprdexamfee.Sheets[0].Columns[1].Width = 80;
                sprdexamfee.Sheets[0].Columns[2].Width = 80;
                sprdexamfee.Sheets[0].Columns[3].Width = 80;
                sprdexamfee.Sheets[0].Columns[4].Width = 150;
                sprdexamfee.Sheets[0].Columns[5].Width = 100;
                sprdexamfee.Sheets[0].Columns[6].Width = 100;
                sprdexamfee.Sheets[0].Columns[7].Width = 100;
                sprdexamfee.Sheets[0].Columns[8].Width = 100;
                sprdexamfee.Sheets[0].Columns[9].Width = 100;
                sprdexamfee.Sheets[0].Columns[10].Width = 100;
                FarPoint.Web.Spread.IntegerCellType intcell1 = new IntegerCellType();
                sprdexamfee.Sheets[0].Columns[5].CellType = intcell1;
                sprdexamfee.Sheets[0].Columns[6].CellType = intcell1;
                sprdexamfee.Sheets[0].Columns[7].CellType = intcell1;
                sprdexamfee.Sheets[0].Columns[8].CellType = intcell1;
                sprdexamfee.Sheets[0].Columns[9].CellType = intcell1;
                sprdexamfee.Sheets[0].Columns[10].CellType = intcell1;
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Batch";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Branch";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Semester";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Application Fee";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Regular Exam Fee";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Arrear Exam Fee";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Re-Total Fee";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Re-Valuation Fee";
                sprdexamfee.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Re-Challenge Fee";
                bindbatch();

                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();
                binddegree();
                bindbranch();
                con.Close();
                con.Open();
                SqlCommand header = new SqlCommand("select header_name,header_id from acctheader ", con);
                SqlDataReader acchead = header.ExecuteReader();
                if (acchead.HasRows == true)
                {
                    ddlheader.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                    int i1 = 1;
                    while (acchead.Read())
                    {
                        ddlheader.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + acchead["header_name"].ToString() + "", "" + acchead["header_id"].ToString() + ""));
                        i1++;
                    }
                    ddlheader.Items.Add("All");

                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        DataSet ds = new DataSet();
        mysql.Close();
        mysql.Open();

        if (ddlheader.SelectedItem.Text != "All")
        {
            SqlDataAdapter sd = new SqlDataAdapter("select distinct fee_type,fee_code from fee_info where header_id=" + ddlheader.SelectedValue + " and (fee_type not like 'Cash' and fee_type not like 'Income & Expenditure' and fee_type not like 'Misc' ) and fee_type not in(select distinct bankname from bank_master1) and   (receipt_flag=0 or receipt_flag is null)", mysql);
            sd.Fill(ds);
        }

        else if (ddlheader.SelectedItem.Text == "All")
        {
            SqlDataAdapter sd = new SqlDataAdapter("select distinct fee_type,fee_code from fee_info where (fee_type not like 'Cash' and fee_type not like 'Income & Expenditure' and fee_type not like 'Misc' ) and fee_type not in(select distinct bankname from bank_master1) and   (receipt_flag=0 or receipt_flag is null)", mysql);
            sd.Fill(ds);
        }

        ddlledger.Items.Clear();
        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
        {
            ddlledger.DataSource = ds;
            ddlledger.DataTextField = "fee_type";
            ddlledger.DataValueField = "fee_code";
            ddlledger.DataBind();
            //DropDownList1

        }

        mysql.Close();
        cblledger_SelectedIndexChanged(sender, e);
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
    }
    protected void ddlledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
        btnsave.Visible = false;
        ddlexamtype.SelectedIndex = 0;
        binddegree();

        ddlsubject.Items.Clear();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
        ddlexamtype.SelectedIndex = 0;
        bindbranch();

        ddlsubject.Items.Clear();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
        ddlexamtype.SelectedIndex = 0;

        ddlsubject.Items.Clear();
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
        ddlexamtype.SelectedIndex = 0;
        ddlsubject.Items.Clear();
    }
    protected void ddlexamtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        sprdexamfee.Visible = false;
        btnsave.Visible = false;
        btnset.Visible = false;
        txtapplfee.Text = "";
        txtregexmfee.Text = "";
        txtArrexmfee.Text = "";
        txtretotfee.Text = "";
        txtrevalfee.Text = "";
        txtrechalfee.Text = "";
        string batchyear = ddlbatch.SelectedValue.ToString();
        string degreecode = ddlbranch.SelectedValue.ToString();
        string allbatch = "";
        if (ddlbatch.SelectedItem.Text != "All")
        {
            allbatch = " and syllabus_master.batch_year=" + batchyear + "";
        }
        else
        {
            allbatch = "";
        }
        string allbranch = "";
        if (ddlbranch.SelectedItem.Text != "All")
        {
            allbranch = "and syllabus_master.degree_code=" + degreecode + "";
        }
        else
        {
            allbranch = "";
        }

        ddlsubject.Items.Clear();
        if (ddlexamtype.SelectedItem.Text == "Theory")
        {
            //string bindsubject = "select distinct subject_name,subject.subject_no from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.semester=registration.current_semester and registration.degree_code=syllabus_master.degree_code and subjectchooser.roll_no=registration.roll_no  and registration.batch_year=syllabus_master.batch_year  "+allbranch +" "+allbatch +" and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'th%'  and RollNo_Flag<>0 and exam_flag <> 'DEBAR' ";
            string bindsubject = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and syllabus_master.batch_year=" + ddlbatch.SelectedValue.ToString() + " and registration.current_semester=subjectchooser.semester and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'th%' and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            SqlDataAdapter dabindsubject = new SqlDataAdapter(bindsubject, con);
            DataSet dsbindsubject = new DataSet();
            con.Close();
            con.Open();
            dabindsubject.Fill(dsbindsubject);
            if (dsbindsubject.Tables[0].Rows.Count > 0)
            {

                ddlsubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                int i1 = 1;
                for (int i = 0; i < dsbindsubject.Tables[0].Rows.Count; i++)
                {
                    ddlsubject.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbindsubject.Tables[0].Rows[i]["subject_name"].ToString() + "", "" + dsbindsubject.Tables[0].Rows[i]["subject_no"].ToString() + ""));
                    i1++;
                }
                //i1++;
                //ddlsubject.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", ""+i1+""));
                ddlsubject.Items.Add("All");
            }
        }
        else if (ddlexamtype.SelectedItem.Text == "Practical")
        {
            //Modified By Srinath 16/10/2014
            //string bindsubject = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.semester=registration.current_semester and syllabus_master.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.current_semester=subjectchooser.semester  and syllabus_master.batch_year=" + ddlbatch.SelectedValue.ToString() + " and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'pr%' and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            string bindsubject = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and syllabus_master.batch_year=" + ddlbatch.SelectedValue.ToString() + " and registration.current_semester=subjectchooser.semester and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'pr%' and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            SqlDataAdapter dabindsubject = new SqlDataAdapter(bindsubject, con);
            DataSet dsbindsubject = new DataSet();
            con.Close();
            con.Open();
            dabindsubject.Fill(dsbindsubject);
            if (dsbindsubject.Tables[0].Rows.Count > 0)
            {


                ddlsubject.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ", "0"));
                int i1 = 1;
                for (int i = 0; i < dsbindsubject.Tables[0].Rows.Count; i++)
                {
                    ddlsubject.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbindsubject.Tables[0].Rows[i]["subject_name"].ToString() + "", "" + dsbindsubject.Tables[0].Rows[i]["subject_no"].ToString() + ""));
                    i1++;
                }
                //i1++;
                //ddlsubject.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "" + i1 + ""));
                ddlsubject.Items.Add("All");
            }
        }
    }
    protected void DropDow_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            int i1 = 1;
            for (int i = 0; i < count; i++)
            {
                ddlbatch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + ""));
                i1++;
            }
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }
    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (ddldegree.SelectedItem.Text != "All")
        {
            hat.Clear();

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds_load = daccess2.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {

                int i1 = 0;
                for (int i = 0; i < count2; i++)
                {
                    ddlbranch.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["dept_name"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                    i1 = i;
                }
                i1++;
                ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
        }
        else if (ddldegree.SelectedItem.Text == "All")
        {
            string bindbranch = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " ";
            SqlDataAdapter dabindbranch = new SqlDataAdapter(bindbranch, con);
            DataSet dsbindbranch = new DataSet();
            con.Close();
            con.Open();
            dabindbranch.Fill(dsbindbranch);
            if (dsbindbranch.Tables[0].Rows.Count > 0)
            {
                int i1 = 0;
                for (int i = 0; i < dsbindbranch.Tables[0].Rows.Count; i++)
                {
                    ddlbranch.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + dsbindbranch.Tables[0].Rows[i]["dept_name"].ToString() + "", "" + dsbindbranch.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                    i1 = i;
                }
                i1++;
                ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
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
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess2.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {


            int i1 = 0;
            for (int i = 0; i < count1; i++)
            {
                ddldegree.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["course_name"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["course_id"].ToString() + ""));
                i1 = i;
            }
            i1++;
            ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
        }
    }
    //public void bindsem()
    //{
    //    try
    //    {
    //        //--------------------semester load
    //        ddlsem.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        con.Close();
    //        con.Open();
    //        SqlDataReader dr;
    //        if (ddlbranch.SelectedItem.Text != "All")
    //        {
    //            cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
    //            dr = cmd.ExecuteReader();
    //            dr.Read();
    //            if (dr.HasRows == true)
    //            {
    //                first_year = Convert.ToBoolean(dr[1].ToString());
    //                duration = Convert.ToInt16(dr[0].ToString());
    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {
    //                        ddlsem.Items.Add(i.ToString());
    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {
    //                        ddlsem.Items.Add(i.ToString());
    //                    }

    //                }
    //                //i++;
    //                //ddlsem.Items.Insert(i, new System.Web.UI.WebControls.ListItem("All"," "));
    //                ddlsem.Items.Add("All");
    //            }
    //            else
    //            {
    //                dr.Close();
    //                SqlDataReader dr1;
    //                cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
    //                ddlsem.Items.Clear();
    //                dr1 = cmd.ExecuteReader();
    //                dr1.Read();
    //                if (dr1.HasRows == true)
    //                {
    //                    first_year = Convert.ToBoolean(dr1[1].ToString());
    //                    duration = Convert.ToInt16(dr1[0].ToString());

    //                    for (i = 1; i <= duration; i++)
    //                    {
    //                        if (first_year == false)
    //                        {
    //                            ddlsem.Items.Add(i.ToString());
    //                        }
    //                        else if (first_year == true && i != 2)
    //                        {
    //                            ddlsem.Items.Add(i.ToString());
    //                        }

    //                    }
    //                    //i++;
    //                    //ddlsem.Items.Insert(i, new System.Web.UI.WebControls.ListItem("All", " "));
    //                    ddlsem.Items.Add("All");
    //                }

    //                dr1.Close();
    //            }
    //        }
    //        //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
    //        con.Close();
    //        if (ddlbranch.SelectedItem.Text == "All")
    //        {
    //            con.Close();
    //            con.Open();
    //            SqlDataReader dr2;
    //            cmd = new SqlCommand("select top 1 duration,first_year_nonsemester from degree where college_code=" + Session["collegecode"] + " order by duration desc", con);
    //            dr2 = cmd.ExecuteReader();
    //            dr2.Read();
    //            if (dr2.HasRows == true)
    //            {
    //                first_year = Convert.ToBoolean(dr2[1].ToString());
    //                duration = Convert.ToInt16(dr2[0].ToString());
    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {
    //                        ddlsem.Items.Add(i.ToString());
    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {
    //                        ddlsem.Items.Add(i.ToString());
    //                    }

    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    public void bindsubject()
    {

    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        txtapplfee.MaxLength = 6;
        txtregexmfee.MaxLength = 6;
        txtArrexmfee.MaxLength = 6;
        txtretotfee.MaxLength = 6;
        txtrevalfee.MaxLength = 6;
        txtrechalfee.MaxLength = 6;
        string appfees = txtapplfee.Text;
        string regexfees = txtregexmfee.Text;
        string arrexfees = txtArrexmfee.Text;
        string retotfees = txtretotfee.Text;
        string revalfees = txtrevalfee.Text;
        string rechalfees = txtrechalfee.Text;
        for (int rowcount = 0; rowcount < sprdexamfee.Sheets[0].RowCount; rowcount++)
        {
            for (int colcount = 5; colcount < sprdexamfee.Sheets[0].ColumnCount; colcount++)
            {
                string coltoptext = sprdexamfee.Sheets[0].ColumnHeader.Cells[0, colcount].Text;
                if (coltoptext == "Application Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = appfees;

                }
                if (coltoptext == "Regular Exam Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = regexfees;

                }
                if (coltoptext == "Arrear Exam Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = arrexfees;
                }
                if (coltoptext == "Re-Total Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = retotfees;
                }
                if (coltoptext == "Re-Valuation Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = revalfees;
                }
                if (coltoptext == "Re-Challenge Fee")
                {
                    sprdexamfee.Sheets[0].Cells[rowcount, colcount].Text = rechalfees;
                }
                sprdexamfee.Sheets[0].Cells[rowcount, colcount].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsave.Visible = false;
        sprdexamfee.Visible = true;
        sprdexamfee.Sheets[0].RowCount = 0;
        string batchyear = ddlbatch.SelectedValue.ToString();

        string subject = ddlsubject.SelectedItem.Text;
        string subjectno = ddlsubject.SelectedValue.ToString();
        string degreecode = ddlbranch.SelectedValue.ToString();
        string allbatch = "";
        if (ddlbatch.SelectedItem.Text != "All")
        {
            allbatch = " and syllabus_master.batch_year=" + batchyear + "";
        }
        else
        {
            allbatch = "";
        }
        string allbranch = "";
        if (ddlbranch.SelectedItem.Text != "All")
        {
            allbranch = "and syllabus_master.degree_code=" + degreecode + "";
        }
        else
        {
            allbranch = "";
        }
        string allsubject = "";
        if (ddlsubject.SelectedValue == "All")
        {
            if (ddlexamtype.SelectedValue == "Theory")
            {
                allsubject = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code  and d.course_id=c.course_id and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batchyear + " and subject.subject_no =subjectchooser.subject_no and registration.current_semester=syllabus_master.semester and sub_sem.subject_type like 'th%' and registration.current_semester=subjectchooser.semester and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batchyear + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            }
            else if (ddlexamtype.SelectedValue == "Practical")
            {
                allsubject = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code  and d.course_id=c.course_id and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batchyear + " and subject.subject_no =subjectchooser.subject_no and registration.current_semester=syllabus_master.semester and sub_sem.subject_type like 'pr%' and registration.current_semester=subjectchooser.semester and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batchyear + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            }
        }
        if (ddlsubject.SelectedValue != "All")
        {
            if (ddlexamtype.SelectedValue == "Theory")
            {
                allsubject = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code  and d.course_id=c.course_id and subject.subject_no=" + ddlsubject.SelectedValue.ToString() + " and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batchyear + " and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'th%' and registration.current_semester=subjectchooser.semester and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batchyear + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            }
            else if (ddlexamtype.SelectedValue == "Practical")
            {
                allsubject = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code  and d.course_id=c.course_id and subject.subject_no=" + ddlsubject.SelectedValue.ToString() + " and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batchyear + " and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'pr%' and registration.current_semester=subjectchooser.semester and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batchyear + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
            }
        }
        //string allsubject = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code and  subject.syll_code=syllabus_master.syll_code  "+allbatch +" "+allbranch +" and d.course_id=c.course_id and syllabus_master.degree_code=registration.degree_code and syllabus_master.batch_year=registration.batch_year and registration.degree_code=syllabus_master.degree_code and registration.current_semester=syllabus_master.semester and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'th%' and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
        SqlDataAdapter daallsubject = new SqlDataAdapter(allsubject, con);
        DataSet dsallsubject = new DataSet();
        con.Close();
        con.Open();
        daallsubject.Fill(dsallsubject);
        if (dsallsubject.Tables[0].Rows.Count > 0)
        {
            btnset.Visible = true;
            btnsave.Visible = true;
            string subject_no = "";
            for (int subcount = 0; subcount < dsallsubject.Tables[0].Rows.Count; subcount++)
            {
                subject_no = dsallsubject.Tables[0].Rows[subcount]["subject_no"].ToString();
                string retrivesubsem = "select fee_per_paper,arr_fee,re_tot,re_val,improvement_fee from sub_sem,subject where subject.subtype_no=sub_sem.subtype_no and subject.syll_code=sub_sem.syll_code and subject_no=" + subject_no + "";
                SqlDataAdapter daretrivesubsem = new SqlDataAdapter(retrivesubsem, con);
                DataSet dsretrivesubsem = new DataSet();
                con.Close();
                con.Open();
                daretrivesubsem.Fill(dsretrivesubsem);
                string regfee = "";
                string arrfee = "";
                string retotfee = "";
                string revalfee = "";
                string imprfee = "";
                if (dsretrivesubsem.Tables[0].Rows.Count > 0)
                {
                    regfee = dsretrivesubsem.Tables[0].Rows[0]["fee_per_paper"].ToString();
                    arrfee = dsretrivesubsem.Tables[0].Rows[0]["arr_fee"].ToString();
                    retotfee = dsretrivesubsem.Tables[0].Rows[0]["re_tot"].ToString();
                    revalfee = dsretrivesubsem.Tables[0].Rows[0]["re_val"].ToString();
                    imprfee = dsretrivesubsem.Tables[0].Rows[0]["improvement_fee"].ToString();
                }
                sprdexamfee.Sheets[0].RowCount = sprdexamfee.Sheets[0].RowCount + 1;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 0].Text = dsallsubject.Tables[0].Rows[subcount]["batch_year"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 1].Text = dsallsubject.Tables[0].Rows[subcount]["course_name"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 2].Text = dsallsubject.Tables[0].Rows[subcount]["acronym"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 2].Note = dsallsubject.Tables[0].Rows[subcount]["degree_code"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 3].Text = dsallsubject.Tables[0].Rows[subcount]["current_semester"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 4].Text = dsallsubject.Tables[0].Rows[subcount]["subject_name"].ToString();
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 6].Text = regfee;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 7].Text = arrfee;//Modified By Srinath 16/10/2014 retotfee 
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 8].Text = retotfee;//Modified By Srinath 16/10/2014 regfee 
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 9].Text = revalfee;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 10].Text = imprfee;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                sprdexamfee.Sheets[0].Cells[sprdexamfee.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
            }
        }


        int rowcount = sprdexamfee.Sheets[0].RowCount;
        sprdexamfee.Height = (rowcount * 40) + 40;
        sprdexamfee.Sheets[0].PageSize = (rowcount * 40) + 40;

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        string appfee = "0";
        string regularfee = "0";
        string arrearfee = "0";
        string retotfee = "0";
        string revalfee = "0";
        string rechallengefee = "0";
        if (txtapplfee.Text != "")
        {
            appfee = txtapplfee.Text;
        }
        if (txtregexmfee.Text != "")
        {
            regularfee = txtregexmfee.Text;
        }
        if (txtArrexmfee.Text != "")
        {
            arrearfee = txtArrexmfee.Text;
        }
        if (txtretotfee.Text != "")
        {
            retotfee = txtretotfee.Text;
        }
        if (txtrevalfee.Text != "")
        {
            revalfee = txtrevalfee.Text;
        }
        if (txtrechalfee.Text != "")
        {
            rechallengefee = txtrechalfee.Text;
        }
        if (sprdexamfee.Sheets[0].RowCount >= 1)
        {
            string batch_year = "";
            string semester = "";
            string degreecode = "";
            string prevbatch = "";
            string prevsem = "";
            string prevdegreecode = "";
            if (ddlledger.SelectedItem.Text != "" && ddlledger.SelectedItem.Text != " ")
            {
                string feecode = ddlledger.SelectedValue.ToString();
                for (int rcount = 0; rcount < sprdexamfee.Sheets[0].RowCount; rcount++)
                {
                    batch_year = sprdexamfee.Sheets[0].Cells[rcount, 0].Text;
                    semester = sprdexamfee.Sheets[0].Cells[rcount, 3].Text;
                    degreecode = sprdexamfee.Sheets[0].Cells[rcount, 2].Note;
                    string textvalsemester = semester + " " + "Semester";
                    if (prevbatch != batch_year || prevdegreecode != degreecode || semester != prevsem)
                    {
                        int feeamount = Convert.ToInt32(appfee) + Convert.ToInt32(regularfee) + Convert.ToInt32(arrearfee) + Convert.ToInt32(retotfee) + Convert.ToInt32(revalfee) + Convert.ToInt32(rechallengefee);
                        string flagstatus = "false";

                        prevdegreecode = degreecode;
                        prevsem = semester;
                        prevbatch = batch_year;
                        SqlCommand cmdrollinfo = new SqlCommand("procexamfeerolladmit", con);
                        cmdrollinfo.CommandType = CommandType.StoredProcedure;
                        cmdrollinfo.Parameters.AddWithValue("@batch_year", batch_year);
                        cmdrollinfo.Parameters.AddWithValue("@semester", Convert.ToInt32(semester));
                        cmdrollinfo.Parameters.AddWithValue("@degree_code", Convert.ToInt32(degreecode));
                        cmdrollinfo.Parameters.AddWithValue("@feecode", Convert.ToInt32(feecode));
                        cmdrollinfo.Parameters.AddWithValue("@textvalsemester", textvalsemester);
                        SqlDataAdapter dacmdrollinfo = new SqlDataAdapter(cmdrollinfo);
                        DataSet dscmdrollinfo = new DataSet();
                        dacmdrollinfo.Fill(dscmdrollinfo);
                        string textcode = "0";
                        if (dscmdrollinfo.Tables[1].Rows.Count > 0)
                        {
                            textcode = dscmdrollinfo.Tables[1].Rows[0]["textcode"].ToString();
                        }
                        string header_id = "";
                        if (dscmdrollinfo.Tables[2].Rows.Count > 0)
                        {
                            header_id = dscmdrollinfo.Tables[2].Rows[0]["header_id"].ToString();
                        }
                        if (dscmdrollinfo.Tables[0].Rows.Count > 0)
                        {
                            string rolladmit = "";
                            for (int totroll = 0; totroll < dscmdrollinfo.Tables[0].Rows.Count; totroll++)
                            {
                                rolladmit = dscmdrollinfo.Tables[0].Rows[totroll]["roll_admit"].ToString();
                                string setfeeallot = "select * from fee_allot where   fee_code=" + feecode + " and roll_admit='" + rolladmit + "'";
                                SqlDataAdapter dasetfeeallot = new SqlDataAdapter(setfeeallot, con1);
                                DataSet dssetfeeallot = new DataSet();
                                con1.Close();
                                con1.Open();
                                dasetfeeallot.Fill(dssetfeeallot);
                                if (dssetfeeallot.Tables[0].Rows.Count > 0)
                                {
                                    string createdummy = "update fee_allot set roll_admit='" + rolladmit + "',fee_code=" + feecode + ",allotdate='" + newdate + "',flag_status='" + flagstatus + "',fee_amount=" + feeamount + ",fee_category=" + textcode + "  where   fee_code=" + feecode + " and roll_admit='" + rolladmit + "'";
                                    SqlCommand createdummycmd = new SqlCommand(createdummy, con1);
                                    con1.Close();
                                    con1.Open();
                                    createdummycmd.ExecuteNonQuery();
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                }
                                else
                                {
                                    string insertdummy = "insert into fee_allot (roll_admit,fee_code,allotdate,flag_status,fee_amount,fee_category) values('" + rolladmit + "'," + feecode + ",'" + newdate + "','" + flagstatus + "'," + feeamount + "," + textcode + ")";
                                    SqlCommand createdummycmd = new SqlCommand(insertdummy, con3);
                                    con3.Close();
                                    con3.Open();
                                    createdummycmd.ExecuteNonQuery();
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                                }

                                string setfeestatus = "select * from fee_status where roll_admit='" + rolladmit + "' and header_id=" + header_id + "";
                                SqlDataAdapter dasetfeestatus = new SqlDataAdapter(setfeestatus, con2);
                                DataSet dssetfeestatus = new DataSet();
                                con2.Close();
                                con2.Open();
                                dasetfeestatus.Fill(dssetfeestatus);
                                if (dssetfeestatus.Tables[0].Rows.Count > 0)
                                {
                                    string createdummy = "update fee_status set roll_admit='" + rolladmit + "',amount=" + feeamount + ",amount_paid=0,balance=" + feeamount + ",flag_status='" + flagstatus + "',fee_category=" + textcode + ",header_id=" + header_id + " where roll_admit='" + rolladmit + "' and header_id=" + header_id + " ";
                                    SqlCommand createdummycmd = new SqlCommand(createdummy, con1);
                                    con1.Close();
                                    con1.Open();
                                    createdummycmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    string insertdummy = "insert into fee_allot (roll_admit,fee_code,allotdate,flag_status,fee_amount,fee_category) values('" + rolladmit + "'," + feecode + ",'" + newdate + "','" + flagstatus + "'," + feeamount + "," + textcode + ")";
                                    SqlCommand createdummycmd = new SqlCommand(insertdummy, con3);
                                    con3.Close();
                                    con3.Open();
                                    createdummycmd.ExecuteNonQuery();
                                }

                            }
                        }
                        string examfeepaid = "select * from examfeepaidstatus where batch_year=" + batch_year + " and degree_code=" + degreecode + " and coll_code=" + Session["collegecode"].ToString() + " and semester=" + semester + " and fee_code=" + feecode + "";
                        SqlDataAdapter daexamfeepaid = new SqlDataAdapter(examfeepaid, con2);
                        DataSet dsexamfeepaid = new DataSet();
                        con2.Close();
                        con2.Open();
                        daexamfeepaid.Fill(dsexamfeepaid);
                        if (dsexamfeepaid.Tables[0].Rows.Count > 0)
                        {
                            string createdummy = "update examfeepaidstatus set batch_year=" + batch_year + ",coll_code=" + Session["collegecode"].ToString() + ",degree_code=" + degreecode + ",semester=" + semester + " ,fee_code=" + feecode + " where batch_year=" + batch_year + " and coll_code=" + Session["collegecode"].ToString() + " and degree_code=" + degreecode + " and semester=" + semester + " and fee_code=" + feecode + " ";
                            SqlCommand createdummycmd = new SqlCommand(createdummy, con1);
                            con1.Close();
                            con1.Open();
                            createdummycmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string insertdummy = "insert into examfeepaidstatus (batch_year,degree_code,coll_code,semester,fee_code)values(" + batch_year + "," + degreecode + "," + Session["collegecode"].ToString() + "," + semester + "," + feecode + ")";
                            SqlCommand createdummycmd = new SqlCommand(insertdummy, con3);
                            con3.Close();
                            con3.Open();
                            createdummycmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
    protected void cblledger_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtledger_TextChanged(object sender, EventArgs e)
    {

    }
    protected void cbselectall_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void btnset_Click(object sender, EventArgs e)
    {
        string regularfee = "0";
        string arrearfee = "0";
        string retotfee = "0";
        string revalfee = "0";
        string rechallengefee = "0";
        if (sprdexamfee.Sheets[0].RowCount >= 1)
        {
            string batch_year = "";
            string semester = "";
            string degreecode = "";
            string subjectname = "";
            string feecode = ddlledger.SelectedValue.ToString();
            for (int rcount = 0; rcount < sprdexamfee.Sheets[0].RowCount; rcount++)
            {
                batch_year = sprdexamfee.Sheets[0].Cells[rcount, 0].Text;
                semester = sprdexamfee.Sheets[0].Cells[rcount, 3].Text;
                degreecode = sprdexamfee.Sheets[0].Cells[rcount, 2].Note;
                subjectname = sprdexamfee.Sheets[0].Cells[rcount, 4].Text;
                if (txtregexmfee.Text != "")
                {
                    regularfee = txtregexmfee.Text;
                }
                if (txtArrexmfee.Text != "")
                {
                    arrearfee = txtArrexmfee.Text;
                }
                if (txtretotfee.Text != "")
                {
                    retotfee = txtretotfee.Text;
                }
                if (txtrevalfee.Text != "")
                {
                    revalfee = txtrevalfee.Text;
                }
                if (txtrechalfee.Text != "")
                {
                    rechallengefee = txtrechalfee.Text;
                }
                string getsubjectno = "";
                if (ddlexamtype.SelectedValue == "Theory")
                {
                    getsubjectno = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code and  subject.syll_code=syllabus_master.syll_code and d.course_id=c.course_id and subject.subject_name like '" + subjectname + "' and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batch_year + " and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'th%' and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batch_year + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
                }
                else if (ddlexamtype.SelectedValue == "Practical")
                {
                    getsubjectno = "select distinct registration.batch_year,registration.current_semester,c.course_name,d.acronym,d.degree_code,subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,degree d,course c where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and registration.degree_code=d.degree_code and  subject.syll_code=syllabus_master.syll_code and d.course_id=c.course_id and subject.subject_name like '" + subjectname + "' and syllabus_master.degree_code=" + degreecode + "  and syllabus_master.batch_year=" + batch_year + " and subject.subject_no =subjectchooser.subject_no and sub_sem.subject_type like 'pr%' and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and  registration.batch_year=" + batch_year + " and RollNo_Flag<>0 and cc=0  and exam_flag <> 'DEBAR'";
                }

                SqlDataAdapter dagetsubjectno = new SqlDataAdapter(getsubjectno, con);
                DataSet dsgetsubjectno = new DataSet();
                con.Close();
                con.Open();
                dagetsubjectno.Fill(dsgetsubjectno);
                if (dsgetsubjectno.Tables[0].Rows.Count > 0)
                {
                    string subno = dsgetsubjectno.Tables[0].Rows[0]["subject_no"].ToString();
                    string checksubsem = "select subject.subtype_no,subject.syll_code from sub_sem,subject where subject_no=" + subno + " and subject.subtype_no=sub_sem.subtype_no and subject.syll_code=sub_sem.syll_code";
                    SqlDataAdapter dachecksubsem = new SqlDataAdapter(checksubsem, con);
                    DataSet dschecksubsem = new DataSet();
                    con.Close();
                    con.Open();
                    dachecksubsem.Fill(dschecksubsem);
                    if (dschecksubsem.Tables[0].Rows.Count > 0)
                    {

                        string subtype = dschecksubsem.Tables[0].Rows[0]["subtype_no"].ToString();
                        string syllcode = dschecksubsem.Tables[0].Rows[0]["syll_code"].ToString();
                        string createdummy = "update sub_sem set fee_per_paper=" + regularfee + ",improvement_fee=" + rechallengefee + ",arr_fee=" + arrearfee + ",re_tot=" + retotfee + ",re_val=" + revalfee + " where subtype_no=" + subtype + " and syll_code=" + syllcode + "";
                        SqlCommand createdummycmd = new SqlCommand(createdummy, con1);
                        con1.Close();
                        con1.Open();
                        createdummycmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string insertdummy = "insert into fee_allot (fee_per_paper,improvement_fee,arr_fee,re_tot,re_val) values(" + regularfee + "," + rechallengefee + "," + arrearfee + "," + retotfee + "," + revalfee + ")";
                        SqlCommand createdummycmd = new SqlCommand(insertdummy, con3);
                        con3.Close();
                        con3.Open();
                        createdummycmd.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}