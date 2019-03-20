using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;


public partial class Exam_moderation_forInternal : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable hash = new Hashtable();
    static Hashtable hashtosave = new Hashtable();
    static Hashtable hashforremainingmark = new Hashtable();
    string CollegeCode;
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
            if (!IsPostBack)
            {
                Radiosubjectwise.Checked = true;
                sprdremainmark.CommandBar.Visible = false;
                sprdremainmark.Sheets[0].AutoPostBack = true;
                MonthandYear();
                sprdremainmark.Sheets[0].SheetCorner.RowCount = 1;
                sprdremainmark.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdremainmark.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdremainmark.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdremainmark.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdremainmark.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdremainmark.Sheets[0].DefaultStyle.Font.Bold = false;
                sprdremainmark.Sheets[0].ColumnCount = 5;
                sprdremainmark.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Branch";
                sprdremainmark.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll_no";
                sprdremainmark.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject";
                sprdremainmark.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Internal Mark";
                sprdremainmark.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Remaining Mark";


                sprdsubjectlist.CommandBar.Visible = true;
                sprdsubjectlist.Sheets[0].AutoPostBack = true;
                sprdsubjectlist.Sheets[0].SheetCorner.RowCount = 8;

                sprdsubjectlist.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdsubjectlist.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdsubjectlist.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdsubjectlist.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdsubjectlist.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                sprdsubjectlist.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdsubjectlist.Sheets[0].DefaultStyle.Font.Bold = false;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                sprdsubjectlist.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                sprdsubjectlist.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = sprdsubjectlist.FindControl("Update");
        Control cntCancelBtn = sprdsubjectlist.FindControl("Cancel");
        Control cntCopyBtn = sprdsubjectlist.FindControl("Copy");
        Control cntCutBtn = sprdsubjectlist.FindControl("Clear");
        Control cntPasteBtn = sprdsubjectlist.FindControl("Paste");
        Control cntPageNextBtn = sprdsubjectlist.FindControl("Next");
        Control cntPagePreviousBtn = sprdsubjectlist.FindControl("Prev");
        Control cntPagePrintPDFBtn = sprdsubjectlist.FindControl("PrintPDF");

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

            tc = (TableCell)cntPagePrintPDFBtn.Parent;
            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }
    public void MonthandYear()
    {

        ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
        ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
        ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
        ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
        ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
        ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
        ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
        ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
        ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
        ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
        ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
        ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));


        int year;
        year = Convert.ToInt16(DateTime.Today.Year);
        ddlYear.Items.Clear();
        for (int l = 0; l <= 20; l++)
        {

            ddlYear.Items.Add(Convert.ToString(year - l));

        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }
    public void YearNew()
    {
        int year;
        year = Convert.ToInt16(DateTime.Today.Year);
        ddlYear.Items.Clear();
        for (int l = 0; l <= 20; l++)
        {

            ddlYear.Items.Add(Convert.ToString(year - l));

        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        btnsave.Visible = false;
        sprdsubjectlist.Visible = false;
        sprdremainmark.Visible = false;
        //btnadd.Visible = false;
        //lblapply.Visible = false;
        //TextBox1.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        if (ddlMonth.SelectedValue.ToString() != "0" && ddlYear.SelectedValue.ToString() != "0")
        {

            string degreecodequery = "select distinct c.course_name,c.course_id from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
            SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
            DataSet dsdegreecodequery = new DataSet();
            con1.Close();
            con1.Open();
            dadegreecodequery.Fill(dsdegreecodequery);
            ddldegree.Items.Clear();
            if (dsdegreecodequery.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = dsdegreecodequery;
                ddldegree.DataValueField = "course_id";
                ddldegree.DataTextField = "course_name";
                ddldegree.DataBind();

                string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                DataSet dsbranchquery = new DataSet();
                con1.Close();
                con1.Open();
                dabranchquery.Fill(dsbranchquery);
                ddlbranch.Items.Clear();
                if (dsbranchquery.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.DataSource = dsbranchquery;
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataTextField = "dept_acronym";
                    ddlbranch.DataBind();

                }
                string semesterquery = "select distinct current_semester from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and d.degree_code=" + ddlbranch.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dasemesterquery = new SqlDataAdapter(semesterquery, con1);
                DataSet dssemesterquery = new DataSet();
                con1.Close();
                con1.Open();
                dasemesterquery.Fill(dssemesterquery);
                ddlsem.Items.Clear();
                if (dssemesterquery.Tables[0].Rows.Count > 0)
                {
                    ddlsem.DataSource = dssemesterquery;
                    ddlsem.DataValueField = "current_semester";
                    ddlsem.DataTextField = "current_semester";
                    ddlsem.DataBind();

                }
            }
            else
            {
                int year;
                year = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 20; l++)
                {

                    ddlYear.Items.Add(Convert.ToString(year - l));

                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));

                ddldegree.Items.Clear();
                ddlbranch.Items.Clear();
                ddlsem.Items.Clear();
            }
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        sprdsubjectlist.Visible = false;
        btnsave.Visible = false;
        sprdremainmark.Visible = false;
        //btnadd.Visible = false;
        //lblapply.Visible = false;
        //TextBox1.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        if (ddlMonth.SelectedValue.ToString() != "0" && ddlYear.SelectedValue.ToString() != "0")
        {
            string degreecodequery = "select distinct c.course_name,c.course_id from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and  ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
            SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
            DataSet dsdegreecodequery = new DataSet();
            con1.Close();
            con1.Open();
            dadegreecodequery.Fill(dsdegreecodequery);

            if (dsdegreecodequery.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = dsdegreecodequery;
                ddldegree.DataValueField = "course_id";
                ddldegree.DataTextField = "course_name";
                ddldegree.DataBind();
                string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                DataSet dsbranchquery = new DataSet();
                con1.Close();
                con1.Open();
                dabranchquery.Fill(dsbranchquery);

                if (dsbranchquery.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.DataSource = dsbranchquery;
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataTextField = "dept_acronym";
                    ddlbranch.DataBind();

                }
                string semesterquery = "select distinct current_semester from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and d.degree_code=" + ddlbranch.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dasemesterquery = new SqlDataAdapter(semesterquery, con1);
                DataSet dssemesterquery = new DataSet();
                con1.Close();
                con1.Open();
                dasemesterquery.Fill(dssemesterquery);

                if (dssemesterquery.Tables[0].Rows.Count > 0)
                {
                    ddlsem.DataSource = dssemesterquery;
                    ddlsem.DataValueField = "current_semester";
                    ddlsem.DataTextField = "current_semester";
                    ddlsem.DataBind();

                }
            }
            else
            {
                ddldegree.Items.Clear();
                ddlbranch.Items.Clear();
                ddlsem.Items.Clear();
            }
        }
    }
    protected void spreadbind()
    {
        int radiotype = 2;
        if (Radiosubjectwise.Checked == true)
        {
            radiotype = 1;
            sprdsubjectlist.Sheets[0].ColumnCount = 6;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 0].Text = "Programme";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 1].Text = "Commonto";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 2].Text = "C-Code";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 3].Text = "A";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[3].Width = 50;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 4].Text = "P";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[4].Width = 50;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 5].Text = "%";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[5].Width = 50;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 1, 4);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Actual";
        }
        else if (RadioBranchwise.Checked == true)
        {
            radiotype = 0;
            sprdsubjectlist.Sheets[0].ColumnCount = 5;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 1, 3);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Actual";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 0].Text = "Programme";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 1].Text = "Branch";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[2].Width = 100;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 2].Text = "A";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[2].Width = 50;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 3].Text = "P";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[3].Width = 50;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, 4].Text = "%";
            sprdsubjectlist.Sheets[0].ColumnHeader.Columns[4].Width = 50;

        }

        sprdsubjectlist.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
        sprdsubjectlist.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Center;

        string examcode = "";
        string sem = "";
        string subject_no = "";
        string batchyear = "";
        int my = 0;
        string semfrmmod = "";
        sprdsubjectlist.Sheets[0].RowCount = 0;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        sprdsubjectlist.RowHeader.Visible = false;
        CollegeCode = Session["CollegeCode"].ToString();
        string getdegreequery = "";
        string year = "";
        getdegreequery = "select degree_code,batch_year,exam_code,current_semester from exam_details where exam_month=" + exammonth + " and exam_year=" + examyear + " and current_semester=" + ddlsem.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "";
        SqlDataAdapter dagetdegreequery = new SqlDataAdapter(getdegreequery, con3);
        DataSet dsgetdegreequery = new DataSet();
        dagetdegreequery.Fill(dsgetdegreequery);
        con3.Close();
        con3.Open();
        string degreecode = "";
        string examcode1 = "";
        if (dsgetdegreequery.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsgetdegreequery.Tables[0].Rows.Count; i++)
            {
                degreecode = dsgetdegreequery.Tables[0].Rows[i]["degree_code"].ToString();
                examcode1 = dsgetdegreequery.Tables[0].Rows[i]["exam_code"].ToString();
                batchyear = dsgetdegreequery.Tables[0].Rows[i]["batch_year"].ToString();
                semfrmmod = dsgetdegreequery.Tables[0].Rows[i]["current_semester"].ToString();
                if (ddlmodtype.SelectedValue == "Regular")
                {
                    SqlCommand examcmd = new SqlCommand("ProcmoderationSelectData", con);
                    examcmd.CommandType = CommandType.StoredProcedure;
                    examcmd.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
                    examcmd.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
                    //examcmd.Parameters.AddWithValue("@degreecode", degreecode);
                    examcmd.Parameters.AddWithValue("@examcode", examcode1);
                    examcmd.Parameters.AddWithValue("@flag", radiotype);
                    examcmd.Parameters.AddWithValue("@degreecode", ddlbranch.SelectedValue.ToString());
                    examcmd.Parameters.AddWithValue("@courseid", ddldegree.SelectedValue.ToString());
                    examcmd.Parameters.AddWithValue("@semester", ddlsem.SelectedValue.ToString());
                    //examcmd.Parameters.AddWithValue("@CollegeCode", CollegeCode);
                    SqlDataAdapter examda = new SqlDataAdapter(examcmd);
                    DataSet examds = new DataSet();
                    examda.Fill(examds);

                    //sprdsubjectlist.Sheets[0].RowCount = examds.Tables[0].Rows.Count;
                    int dd = 0;
                    if (examds.Tables[0].Rows.Count > 0)
                    {
                        sprdsubjectlist.Visible = true;
                        btnadd.Visible = true;
                        for (dd = 0; dd < examds.Tables[0].Rows.Count; dd++)
                        {

                            sprdsubjectlist.Sheets[0].RowCount = sprdsubjectlist.Sheets[0].RowCount + 1;
                            sprdsubjectlist.Sheets[0].Rows[sprdsubjectlist.Sheets[0].RowCount - 1].ForeColor = Color.Blue;
                            //sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 0].Text = year;
                            //batchyear = examds.Tables[0].Rows[dd]["batchyear"].ToString();
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 0].Text = examds.Tables[0].Rows[dd]["course_name"].ToString();
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 1].Text = examds.Tables[0].Rows[dd]["dept_acronym"].ToString();
                            examcode = examds.Tables[0].Rows[dd]["exam_code"].ToString();
                            sem = examds.Tables[0].Rows[dd]["current_semester"].ToString();
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 3].Note = examcode;
                            if (Radiosubjectwise.Checked == true)
                            {
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].Note = examds.Tables[0].Rows[dd]["Subject_No"].ToString();
                                subject_no = examds.Tables[0].Rows[dd]["Subject_No"].ToString();
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 2].Text = examds.Tables[0].Rows[dd]["subject_code"].ToString();
                                //sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].Text = examds.Tables[0].Rows[dd]["Subject_Name"].ToString();
                            }

                            //sprdsubjectlist.Sheets[0].Cells[i + dd, 4].Tag = examds.Tables[0].Rows[dd]["subjectcode"].ToString();
                            my = Convert.ToInt32(ddlMonth.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                            string totstdnt = "";
                            if (Radiosubjectwise.Checked == true)
                            {
                                totstdnt = "select count(*) as total from mark_entry where exam_code=" + examcode + " and subject_no=" + subject_no + "";
                            }
                            if (RadioBranchwise.Checked == true)
                            {
                                totstdnt = "select count(*) as total from mark_entry where exam_code=" + examcode + "";
                            }
                            //string totstdnt = "select count(*)as total from registration where degree_code=" + degreecode + " and current_semester=" + sem + " and college_code=" + CollegeCode + " and cc=0 and delflag=0 and exam_flag<>'Debar'";
                            SqlDataAdapter datotstdnt = new SqlDataAdapter(totstdnt, con1);
                            DataSet dstotstdnt = new DataSet();
                            datotstdnt.Fill(dstotstdnt);
                            con1.Close();
                            con1.Open();
                            string totalstudents = "";
                            if (dstotstdnt.Tables[0].Rows.Count > 0)
                            {
                                totalstudents = dstotstdnt.Tables[0].Rows[0]["total"].ToString();
                                if (Radiosubjectwise.Checked == true)
                                {
                                    sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 3].Text = totalstudents;
                                }
                                if (RadioBranchwise.Checked == true)
                                {
                                    sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 2].Text = totalstudents;
                                }
                            }
                            string retrievedata = "select bf_moderation_intmark from moderation where exam_code=" + examcode + " and batch_year=" + batchyear + " and semester=" + semfrmmod + " and degree_code=" + degreecode + "";
                            SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                            DataSet dsretrievedata = new DataSet();
                            con1.Close();
                            daretrievedata.Fill(dsretrievedata);
                            con1.Open();
                            string intmrktype = "";

                            if (dsretrievedata.Tables[0].Rows.Count > 0)
                            {
                                string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["bf_moderation_intmark"].ToString();
                                if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                                {
                                    intmrktype = "internal_mark";
                                }
                                //btnsave.Enabled = false;
                                else
                                {

                                    intmrktype = "actual_internal_mark";
                                }
                            }
                            //===========
                            else
                            {
                                //btnsave.Enabled =true;
                                intmrktype = "internal_mark";
                            }
                            if (Radiosubjectwise.Checked == true)
                            {
                                string passstud = "select count(*) as passed from mark_entry m,subject s where m.subject_no=s.subject_no and exam_code=" + examcode + " and m.subject_no=" + subject_no + " and m." + intmrktype + ">=s.min_int_marks";
                                SqlDataAdapter dapassstud = new SqlDataAdapter(passstud, con1);
                                DataSet dspassstud = new DataSet();
                                dapassstud.Fill(dspassstud);
                                con1.Close();
                                con1.Open();
                                string passedstudents = "";
                                if (dspassstud.Tables[0].Rows.Count > 0)
                                {
                                    passedstudents = dspassstud.Tables[0].Rows[0]["passed"].ToString();
                                    if (Radiosubjectwise.Checked == true)
                                    {
                                        sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].Text = passedstudents;
                                    }
                                    if (RadioBranchwise.Checked == true)
                                    {
                                    }
                                }

                                double perc = 0;
                                if (totalstudents != "" && passedstudents != "")
                                {
                                    string perc2 = "";
                                    decimal perc1 = 0;
                                    perc1 = ((Convert.ToDecimal(passedstudents) / Convert.ToDecimal(totalstudents)) * 100);
                                    perc1 = Math.Round(perc1, 2);
                                    perc = Convert.ToDouble(perc1);
                                    if (Radiosubjectwise.Checked == true)
                                    {
                                        sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(perc);
                                    }
                                    if (RadioBranchwise.Checked == true)
                                    {
                                    }
                                }
                                //}
                            }



                        }
                    }
                }
                //for arrear 
                if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                {
                    SqlCommand examcmdarrear = new SqlCommand("ProcmoderationarrearSelectData", con);
                    examcmdarrear.CommandType = CommandType.StoredProcedure;
                    examcmdarrear.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
                    examcmdarrear.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
                    examcmdarrear.Parameters.AddWithValue("@flag", radiotype);
                    examcmdarrear.Parameters.AddWithValue("@examcode", examcode1);
                    examcmdarrear.Parameters.AddWithValue("@semester", ddlsem.SelectedValue.ToString());
                    examcmdarrear.Parameters.AddWithValue("@degreecode", ddlbranch.SelectedValue.ToString());
                    SqlDataAdapter examdaarrear = new SqlDataAdapter(examcmdarrear);
                    DataSet examdsarrear = new DataSet();
                    examdaarrear.Fill(examdsarrear);

                    if (examdsarrear.Tables[0].Rows.Count > 0)
                    {
                        sprdsubjectlist.Visible = true;
                        btnsave.Visible = true;
                        for (int dd1 = 0; dd1 < examdsarrear.Tables[0].Rows.Count; dd1++)
                        {

                            sprdsubjectlist.Sheets[0].RowCount = sprdsubjectlist.Sheets[0].RowCount + 1;
                            sprdsubjectlist.Sheets[0].Rows[sprdsubjectlist.Sheets[0].RowCount - 1].ForeColor = Color.BlueViolet;
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 0].Text = examdsarrear.Tables[0].Rows[dd1]["course_name"].ToString();
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 1].Text = examdsarrear.Tables[0].Rows[dd1]["dept_acronym"].ToString();
                            examcode = examdsarrear.Tables[0].Rows[dd1]["exam_code"].ToString();
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 3].Note = examcode;
                            if (Radiosubjectwise.Checked == true)
                            {
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 2].Text = examdsarrear.Tables[0].Rows[dd1]["subject_code"].ToString();
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 2].Note = examdsarrear.Tables[0].Rows[dd1]["Subject_Name"].ToString();
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].ForeColor = Color.BlueViolet;
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].Note = examdsarrear.Tables[0].Rows[dd1]["Subject_No"].ToString();
                                subject_no = examdsarrear.Tables[0].Rows[dd1]["Subject_No"].ToString();
                            }

                            my = Convert.ToInt32(ddlMonth.SelectedIndex.ToString()) + Convert.ToInt32(ddlYear.SelectedValue.ToString()) * 12;
                            string totstdnt = "";
                            if (Radiosubjectwise.Checked == true)
                            {
                                totstdnt = "select distinct count(me.roll_no)as total from exam_application ea,exam_appl_details m,mark_entry me where  ea.appl_no=m.appl_no and ea.exam_code=me.exam_code and me.exam_code=" + examcode + " and m.type='*' and   me.subject_no=m.subject_no and me.roll_no=ea.roll_no and  me.subject_no=" + subject_no + "";
                            }
                            if (RadioBranchwise.Checked == true)
                            {
                                totstdnt = "select distinct count(me.roll_no)as total from exam_application ea,exam_appl_details m,mark_entry me where  ea.appl_no=m.appl_no and ea.exam_code=me.exam_code and me.exam_code=" + examcode + "  and m.type='*' and   me.subject_no=m.subject_no and me.roll_no=ea.roll_no";
                            }
                            SqlDataAdapter datotstdnt = new SqlDataAdapter(totstdnt, con1);
                            DataSet dstotstdnt = new DataSet();
                            datotstdnt.Fill(dstotstdnt);
                            con1.Close();
                            con1.Open();
                            string totalstudents = "";
                            if (dstotstdnt.Tables[0].Rows.Count > 0)
                            {
                                totalstudents = dstotstdnt.Tables[0].Rows[0]["total"].ToString();
                                if (Radiosubjectwise.Checked == true)
                                {
                                    sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 3].Text = totalstudents;
                                }
                                if (RadioBranchwise.Checked == true)
                                {
                                    sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 2].Text = totalstudents;
                                }
                            }
                            string retrievedata = "select distinct int_arrear from moderation_remaining_mark where exam_code=" + examcode + "";
                            SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                            DataSet dsretrievedata = new DataSet();
                            con1.Close();
                            daretrievedata.Fill(dsretrievedata);
                            con1.Open();
                            string intmrktype = "";

                            if (dsretrievedata.Tables[0].Rows.Count > 0)
                            {
                                string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["int_arrear"].ToString();
                                if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                                {
                                    intmrktype = "internal_mark";
                                }
                                //btnsave.Enabled = false;
                                else
                                {

                                    intmrktype = "actual_internal_mark";
                                }
                            }
                            //===========
                            else
                            {
                                //btnsave.Enabled =true;
                                intmrktype = "internal_mark";
                            }
                            if (Radiosubjectwise.Checked == true)
                            {
                                string passstud = "select distinct count(me.roll_no) as passed from exam_application ea,exam_appl_details m,mark_entry me,subject s where  ea.appl_no=m.appl_no and ea.exam_code=me.exam_code and me.exam_code=" + examcode + " and m.type=me.type and m.type='*' and   me.subject_no=m.subject_no and me.roll_no=ea.roll_no and  me.subject_no=" + subject_no + " and s.subject_no=me.subject_no and me." + intmrktype + ">=s.min_int_marks and me.total>=s.mintotal";
                                SqlDataAdapter dapassstud = new SqlDataAdapter(passstud, con1);
                                DataSet dspassstud = new DataSet();
                                dapassstud.Fill(dspassstud);
                                con1.Close();
                                con1.Open();
                                string passedstudents = "";
                                if (dspassstud.Tables[0].Rows.Count > 0)
                                {
                                    passedstudents = dspassstud.Tables[0].Rows[0]["passed"].ToString();
                                    if (Radiosubjectwise.Checked == true)
                                    {
                                        sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 4].Text = passedstudents;
                                    }
                                    if (RadioBranchwise.Checked == true)
                                    {
                                    }
                                }
                                double perc = 0;
                                if (totalstudents != "0" && passedstudents != "0")
                                {
                                    string perc2 = "";
                                    decimal perc1 = 0;
                                    perc1 = ((Convert.ToDecimal(passedstudents) / Convert.ToDecimal(totalstudents)) * 100);
                                    perc1 = Math.Round(perc1, 2);
                                    perc = Convert.ToDouble(perc1);
                                    if (Radiosubjectwise.Checked == true)
                                    {
                                        sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(perc);
                                    }
                                    if (RadioBranchwise.Checked == true)
                                    {
                                    }
                                }
                            }
                        }
                    }

                }


            }
        }

        sprdsubjectlist.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        sprdsubjectlist.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        int totalrows = sprdsubjectlist.Sheets[0].RowCount;
        sprdsubjectlist.Sheets[0].PageSize = totalrows * 100;
        sprdsubjectlist.Height = totalrows * 200;
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        btnsave.Visible = true;
        sprdremainmark.Sheets[0].AutoPostBack = true;
        sprdremainmark.Sheets[0].RowCount = 0;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        sprdremainmark.RowHeader.Visible = false;
        CollegeCode = Session["CollegeCode"].ToString();
        string getdegreequery = "";
        getdegreequery = "select degree_code,batch_year,exam_code,current_semester from exam_details where exam_month=" + exammonth + " and exam_year=" + examyear + " and current_semester=" + ddlsem.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "";
        SqlDataAdapter dagetdegreequery = new SqlDataAdapter(getdegreequery, con3);
        DataSet dsgetdegreequery = new DataSet();
        dagetdegreequery.Fill(dsgetdegreequery);
        con3.Close();
        con3.Open();
        string degreecode = "";
        string examcode1 = "";
        string semfrmmod = "";
        string batchyear = "";
        if (dsgetdegreequery.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsgetdegreequery.Tables[0].Rows.Count; i++)
            {
                lblerror.Visible = false;
                degreecode = dsgetdegreequery.Tables[0].Rows[i]["degree_code"].ToString();
                examcode1 = dsgetdegreequery.Tables[0].Rows[i]["exam_code"].ToString();
                batchyear = dsgetdegreequery.Tables[0].Rows[i]["batch_year"].ToString();
                semfrmmod = dsgetdegreequery.Tables[0].Rows[i]["current_semester"].ToString();
                SqlCommand examcmd = new SqlCommand("ProcmoderationSelectData", con);
                examcmd.CommandType = CommandType.StoredProcedure;
                examcmd.Parameters.AddWithValue("@ExamMonth", ddlMonth.SelectedIndex.ToString());
                examcmd.Parameters.AddWithValue("@ExamYear", ddlYear.SelectedItem.Text.ToString());
                examcmd.Parameters.AddWithValue("@examcode", examcode1);
                examcmd.Parameters.AddWithValue("@degreecode", ddlbranch.SelectedValue.ToString());
                examcmd.Parameters.AddWithValue("@courseid", ddldegree.SelectedValue.ToString());
                examcmd.Parameters.AddWithValue("@semester", ddlsem.SelectedValue.ToString());
                int radiotype = 0;
                examcmd.Parameters.AddWithValue("@flag", radiotype);
                SqlDataAdapter examda = new SqlDataAdapter(examcmd);
                DataSet examds = new DataSet();
                examda.Fill(examds);
                int dd = 0;
                if (examds.Tables[0].Rows.Count > 0)
                {
                    sprdremainmark.Visible = true;
                    for (dd = 0; dd < examds.Tables[0].Rows.Count; dd++)
                    {
                        string course = examds.Tables[0].Rows[dd]["course_name"].ToString();
                        string depart = examds.Tables[0].Rows[dd]["dept_acronym"].ToString();
                        //string getroll = "select m.exam_code,m.roll_no,m.subject_no,m.internal_mark,mr.ex_regular from moderation_remaining_mark mr,mark_entry m where mr.exam_code="+examcode1+" and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no order by mr.roll_no";
                        string getroll = "";
                        if (ddlmodtype.SelectedValue.ToString() == "Regular")
                        {
                            getroll = "select m.exam_code,m.roll_no,s.subject_code,isnull(m.actual_internal_mark,'0') as internal_mark ,isnull(mr.ex_regular,'0') as ex_regular ,s.min_int_marks from moderation_remaining_mark mr,mark_entry m,subject s,sub_sem feesub,syllabus_master sy where mr.exam_code=" + examcode1 + " and type<>'*' and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no and feesub.subtype_no=s.subtype_no and ex_regular>0 and actual_internal_mark<min_int_marks and m.subject_no=s.subject_no and sy.syll_code=s.syll_code and sy.semester=" + ddlsem.SelectedValue.ToString() + " and sy.degree_code=" + ddlbranch.SelectedValue.ToString() + " and feesub.subject_type like 'th%' order by mr.roll_no";
                        }
                        if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                        {
                            getroll = "select m.exam_code,m.roll_no,s.subject_code,isnull(m.actual_internal_mark,'0') as internal_mark ,isnull(mr.ex_arrear,'0') as ex_arrear ,s.min_int_marks from moderation_remaining_mark mr,mark_entry m,subject s,sub_sem feesub where mr.exam_code=" + examcode1 + " and type='*' and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no and feesub.subtype_no=s.subtype_no and ex_arrear>0 and actual_internal_mark<min_int_marks and m.subject_no=s.subject_no and feesub.subject_type like 'th%' order by mr.roll_no";
                        }
                        SqlDataAdapter dagetroll = new SqlDataAdapter(getroll, con3);
                        DataSet dsgetroll = new DataSet();
                        dagetroll.Fill(dsgetroll);
                        con3.Close();
                        con3.Open();
                        string rollno = "";
                        string external_regular = "";
                        string mininternal = "";
                        string internalmark = "";
                        if (dsgetroll.Tables[0].Rows.Count > 0)
                        {
                            for (int i1 = 0; i1 < dsgetroll.Tables[0].Rows.Count; i1++)
                            {
                                sprdremainmark.Sheets[0].RowCount = sprdremainmark.Sheets[0].RowCount + 1;
                                rollno = dsgetroll.Tables[0].Rows[i1]["roll_no"].ToString();
                                if (ddlmodtype.SelectedValue.ToString() == "Regular")
                                {
                                    external_regular = dsgetroll.Tables[0].Rows[i1]["Ex_Regular"].ToString();
                                }
                                if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                                {
                                    external_regular = dsgetroll.Tables[0].Rows[i1]["Ex_Arrear"].ToString();
                                }
                                mininternal = dsgetroll.Tables[0].Rows[i1]["min_int_marks"].ToString();
                                if (mininternal == "")
                                {
                                    mininternal = "0";
                                }
                                internalmark = dsgetroll.Tables[0].Rows[i1]["Internal_mark"].ToString();
                                if (internalmark == "")
                                {
                                    internalmark = "0";
                                }
                                if (Convert.ToDouble(internalmark) < Convert.ToInt32(mininternal))
                                {
                                    sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].ForeColor = Color.Red;
                                    sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].Font.Underline = true;
                                    sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].Text = internalmark;
                                }
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 0].Text = course + "-" + depart;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 1].Text = rollno;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 2].Text = dsgetroll.Tables[0].Rows[i1]["Subject_code"].ToString(); ;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].Text = dsgetroll.Tables[0].Rows[i1]["Internal_mark"].ToString(); ;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 4].Text = external_regular;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                sprdremainmark.Sheets[0].Cells[sprdremainmark.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                sprdremainmark.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                sprdremainmark.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                int totalrows = sprdremainmark.Sheets[0].RowCount;
                sprdremainmark.Sheets[0].PageSize = totalrows * 40;
                sprdremainmark.Height = totalrows * 40;
                string intmrktype = "";
                if (ddlmodtype.SelectedValue.ToString() == "Regular")
                {
                    string retrievedata = "select bf_moderation_intmark from moderation where exam_code=" + examcode1 + " and batch_year=" + batchyear + " and semester=" + semfrmmod + " and degree_code=" + degreecode + "";
                    SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                    DataSet dsretrievedata = new DataSet();
                    con1.Close();
                    daretrievedata.Fill(dsretrievedata);
                    con1.Open();
                    if (dsretrievedata.Tables[0].Rows.Count > 0)
                    {
                        string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["bf_moderation_intmark"].ToString();
                        if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                        {
                            Btndelete.Visible = false;
                            btnsave.Visible = false;
                            btnsave.Text = "Save";

                            intmrktype = "internal_mark";
                        }
                        //btnsave.Enabled = false;
                        else
                        {
                            string retrieveapliedmark = "select distinct int_Regular_applied from moderation_remaining_mark where exam_code=" + examcode1 + "";
                            SqlDataAdapter daretrieveapliedmark = new SqlDataAdapter(retrieveapliedmark, con2);
                            DataSet dsretrieveapliedmark = new DataSet();
                            con2.Close();
                            daretrieveapliedmark.Fill(dsretrieveapliedmark);
                            con2.Open();
                            if (dsretrieveapliedmark.Tables[0].Rows.Count > 0)
                            {
                                Btndelete.Visible = true;
                                btnsave.Visible = true;
                                btnsave.Text = "Update";

                                string markappliedformod = dsretrieveapliedmark.Tables[0].Rows[0]["int_Regular_applied"].ToString();
                                TextBox1.Text = markappliedformod;
                            }
                            intmrktype = "actual_internal_mark";
                        }
                    }
                    //===========
                    else
                    {
                        Btndelete.Visible = false;
                        btnsave.Visible = false;
                        btnsave.Text = "Save";

                        //btnsave.Enabled =true;
                        intmrktype = "internal_mark";
                    }



                }
                if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                {
                    string retrievedata = "select distinct int_arrear,int_arrear_applied from moderation_remaining_mark where exam_code=" + examcode1 + "";
                    SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                    DataSet dsretrievedata = new DataSet();
                    con1.Close();
                    daretrievedata.Fill(dsretrievedata);
                    con1.Open();


                    if (dsretrievedata.Tables[0].Rows.Count > 0)
                    {
                        string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["int_arrear"].ToString();
                        string markappliedformod = dsretrievedata.Tables[0].Rows[0]["int_arrear_applied"].ToString();
                        if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                        {
                            Btndelete.Visible = false;
                            btnsave.Visible = false;
                            btnsave.Text = "Save";
                            intmrktype = "internal_mark";
                        }
                        //btnsave.Enabled = false;
                        else
                        {
                            Btndelete.Visible = true;
                            btnsave.Visible = true;
                            btnsave.Text = "Update";
                            TextBox1.Text = markappliedformod;
                            intmrktype = "actual_internal_mark";
                        }
                    }
                    //===========
                    else
                    {
                        Btndelete.Visible = false;
                        btnsave.Visible = false;
                        btnsave.Text = "Save";

                        //btnsave.Enabled =true;
                        intmrktype = "internal_mark";
                    }
                }
            }
        }
        if (sprdremainmark.Sheets[0].RowCount < 1)
        {
            sprdremainmark.Visible = false;
            lblerror.Text = "No Records Found";
            lblerror.Visible = true;
            sprdsubjectlist.Visible = false;
            btnsave.Visible = false;
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
    protected void moderation()
    {
        if (ddlMonth.SelectedValue != "0" && ddlYear.SelectedValue != "0" && ddldegree.SelectedValue != "" && ddlbranch.SelectedValue != "")
        {
            Hashtable checkroll = new Hashtable();
            Hashtable checkrollforbranchwise = new Hashtable();
            checkroll.Clear();
            checkrollforbranchwise.Clear();
            if (Radiosubjectwise.Checked == true)
            {
                sprdsubjectlist.Sheets[0].ColumnCount = 6;
            }
            if (RadioBranchwise.Checked == true)
            {
                sprdsubjectlist.Sheets[0].ColumnCount = 5;
            }
            int gracemark = 0;
            int gracelimit = 0;
            if (TextBox1.Text != "")
            {
                gracelimit = Convert.ToInt32(TextBox1.Text);
            }
            //for (int startsgrace = 1; startsgrace <= Convert.ToInt32(TextBox1.Text); startsgrace++)
            //{
            //gracemark = startsgrace;

            hash.Clear();
            sprdsubjectlist.Sheets[0].ColumnCount = sprdsubjectlist.Sheets[0].ColumnCount + 2;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(6, sprdsubjectlist.Sheets[0].ColumnCount - 2, 1, 2);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[6, sprdsubjectlist.Sheets[0].ColumnCount - 2].Text = "After Int_Moderation";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, sprdsubjectlist.Sheets[0].ColumnCount - 2].Text = "P";
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[7, sprdsubjectlist.Sheets[0].ColumnCount - 1].Text = "%";
            MyImg mi = new MyImg();
            mi.ImageUrl = "~/images/10BIT001.jpeg";
            mi.ImageUrl = "Handler/Handler2.ashx?";
            MyImg1 mi2 = new MyImg1();
            mi2.ImageUrl = "~/images/10BIT001.jpeg";
            mi2.ImageUrl = "Handler/Handler5.ashx?";
            string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(district, ' ') as district,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            con1.Close();
            con1.Open();
            SqlCommand comm = new SqlCommand(str, con1);
            SqlDataReader drr = comm.ExecuteReader();
            drr.Read();
            string coll_name = Convert.ToString(drr["collname"]);
            string coll_address1 = Convert.ToString(drr["address1"]);
            string coll_address2 = Convert.ToString(drr["address2"]);
            string coll_address3 = Convert.ToString(drr["address3"]);
            string district = Convert.ToString(drr["district"]);
            string pin_code = Convert.ToString(drr["pincode"]);
            string catgory = drr["category"].ToString();
            catgory = "(An " + catgory + " Institution" + " " + "-" + "";
            string affliatedby = drr["affliated"].ToString();
            string affliatedbynew = Regex.Replace(affliatedby, ",", " ");
            string affiliated = catgory + " " + "Affiliated to" + " " + affliatedbynew + ")";
            string address = coll_address1 + "," + " " + coll_address2 + "," + " " + district + "-" + " " + pin_code + ".";
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, sprdsubjectlist.Sheets[0].ColumnCount);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Maximum of" + " " + TextBox1.Text + " " + "Marks per Student";
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 5, 1);
            //sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(0, sprdsubjectlist.Sheets[0].ColumnCount, 1, 6);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_name;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 2);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[2, 1].Text = address;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 2);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[3, 1].Text = affiliated;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 2);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[4, 1].Text = "";
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 2);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[4, 1].Text = ddlmodtype.SelectedValue.ToString() + " " + "Internal Moderation" + " " + "-" + " " + ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedValue.ToString();
            //DummySpread.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Salary Summary For-" + monname + "--" + cblbatchyear.Text + "";
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 2);
            //sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, sprdsubjectlist.Sheets[0].ColumnCount - 3);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.FromArgb(64, 64, 255);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.Black;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;

            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[7].BackColor = Color.FromArgb(214, 235, 255);
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[7].Font.Bold = true;
            sprdsubjectlist.Sheets[0].ColumnHeader.Rows[7].Font.Size = FontUnit.Medium;
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, 0].CellType = mi;
            sprdsubjectlist.Sheets[0].ColumnHeaderSpanModel.Add(1, sprdsubjectlist.Sheets[0].ColumnCount - 1, 5, 1);
            sprdsubjectlist.Sheets[0].ColumnHeader.Cells[1, sprdsubjectlist.Sheets[0].ColumnCount - 1].CellType = mi;
            string examcode1 = "select distinct degree_code,batch_year,current_semester,exam_code from exam_details where exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and current_semester=" + ddlsem.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "";
            SqlDataAdapter daexamcode1 = new SqlDataAdapter(examcode1, con1);
            DataSet dsexamcode1 = new DataSet();
            con1.Close();
            daexamcode1.Fill(dsexamcode1);
            con1.Open();
            int semesterflag = 0;
            int examcode = 0;
            string batchyear = "";
            string degreecode = "";
            string semfrmmod = "";
            hashtosave.Clear();
            hashforremainingmark.Clear();
            if (dsexamcode1.Tables[0].Rows.Count > 0)
            {
                int branchwiserrowcount = sprdsubjectlist.Sheets[0].RowCount; ;
                for (int i2 = 0; i2 < dsexamcode1.Tables[0].Rows.Count; i2++)
                {
                    int intialgrace = 0;
                    int flagforreminingflash = 0;
                    int remaininggraace = 0;
                    int flag = 2;
                    int notfirsttime = 0;
                    examcode = Convert.ToInt32(dsexamcode1.Tables[0].Rows[i2]["exam_code"]);
                    degreecode = dsexamcode1.Tables[0].Rows[i2]["degree_code"].ToString();
                    batchyear = dsexamcode1.Tables[0].Rows[i2]["batch_year"].ToString();
                    semfrmmod = dsexamcode1.Tables[0].Rows[i2]["current_semester"].ToString();
                    int branchpass = 0;
                    string roll_no = "";
                    string rollno = "";
                    int internal_mark = 0;
                    int mintotal = 0;
                    int subject_no = 0;
                    int count = 1;
                    int mininternal = 0;
                    int minexternal = 0;
                    string query = "";
                    string intmrktype = "";
                    if (ddlmodtype.SelectedValue.ToString() == "Regular")
                    {
                        string retrievedata = "select bf_moderation_intmark from moderation where exam_code=" + examcode + " and batch_year=" + batchyear + " and semester=" + semfrmmod + " and degree_code=" + degreecode + "";
                        SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                        DataSet dsretrievedata = new DataSet();
                        con1.Close();
                        daretrievedata.Fill(dsretrievedata);
                        con1.Open();
                        if (dsretrievedata.Tables[0].Rows.Count > 0)
                        {
                            string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["bf_moderation_intmark"].ToString();
                            if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                            {
                                intmrktype = "internal_mark";
                            }
                            //btnsave.Enabled = false;
                            else
                            {

                                intmrktype = "actual_internal_mark";
                            }
                        }
                        //===========
                        else
                        {
                            //btnsave.Enabled =true;
                            intmrktype = "internal_mark";
                        }

                        query = "select m.exam_code,m.roll_no,s.subject_code,s.subject_no,isnull(m.internal_mark,0)as internal_mark,isnull(m.actual_internal_mark,0) as actual_internal_mark,s.min_int_marks,s.min_ext_marks,s.mintotal,ex_regular from moderation_remaining_mark mr,mark_entry m,subject s,sub_sem feesub,exam_details e where mr.exam_code=" + examcode + " and e.exam_code=m.exam_code and e.degree_code=" + ddlbranch.SelectedValue.ToString() + " and e.current_semester=" + ddlsem.SelectedValue.ToString() + " and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no and m." + intmrktype + "< s.min_int_marks and feesub.subtype_no=s.subtype_no and m.subject_no=s.subject_no  and type<>'*' and ex_regular>0 and feesub.subject_type like 'th%' order by mr.roll_no,m.internal_mark desc,m.subject_no asc";

                    }
                    if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                    {
                        string retrievedata = "select distinct int_arrear from moderation_remaining_mark where exam_code=" + examcode + "";
                        SqlDataAdapter daretrievedata = new SqlDataAdapter(retrievedata, con1);
                        DataSet dsretrievedata = new DataSet();
                        con1.Close();
                        daretrievedata.Fill(dsretrievedata);
                        con1.Open();


                        if (dsretrievedata.Tables[0].Rows.Count > 0)
                        {
                            string bf_mod_int = dsretrievedata.Tables[0].Rows[0]["int_arrear"].ToString();
                            if (bf_mod_int == null || bf_mod_int == "" || bf_mod_int == " ")
                            {
                                intmrktype = "internal_mark";
                            }
                            //btnsave.Enabled = false;
                            else
                            {

                                intmrktype = "actual_internal_mark";
                            }
                        }
                        //===========
                        else
                        {
                            //btnsave.Enabled =true;
                            intmrktype = "internal_mark";
                        }
                        int sem = 0;
                        sem = Convert.ToInt32(ddlsem.SelectedValue.ToString());
                        //query = "select m.exam_code,m.roll_no,s.subject_code,s.subject_no,m.internal_mark,s.min_int_marks,s.min_ext_marks,s.mintotal,ex_arrear from moderation_remaining_mark mr,mark_entry m,subject s,sub_sem feesub where mr.exam_code=" + examcode + "  and ex_arrear>0 and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no and m.internal_mark< s.min_int_marks and type='*' and feesub.subtype_no=s.subtype_no and m.subject_no=s.subject_no and feesub.subject_type like 'th%' order by mr.roll_no";
                        query = "select m.exam_code,m.roll_no,s.subject_code,s.subject_no,isnull(m.internal_mark,0) as internal_mark,isnull(m.actual_internal_mark,0) as actual_internal_mark,isnull(s.min_int_marks,0) as min_int_marks,s.min_ext_marks,s.mintotal,isnull(ex_arrear,0) as ex_arrear from moderation_remaining_mark mr,syllabus_master sm,mark_entry m,subject s,sub_sem feesub,exam_details e where mr.exam_code=" + examcode + " and e.exam_code=m.exam_code and e.degree_code=" + ddlbranch.SelectedValue.ToString() + " and e.current_semester=" + ddlsem.SelectedValue.ToString() + "  and ex_arrear>0 and mr.exam_code=m.exam_code and mr.roll_no=m.roll_no and m." + intmrktype + "< s.min_int_marks and sm.syll_code=s.syll_code and type='*' and feesub.subtype_no=s.subtype_no and ex_arrear>0 and m.subject_no=s.subject_no and feesub.subject_type like 'th%' order by mr.roll_no,m.internal_mark desc,m.subject_no asc";
                    }
                    SqlCommand cmdstudentlist = new SqlCommand(query, con);
                    SqlDataAdapter daforstudent = new SqlDataAdapter(cmdstudentlist);
                    DataSet dsforstudent = new DataSet();
                    daforstudent.Fill(dsforstudent);
                    if (dsforstudent.Tables[0].Rows.Count > 0)
                    {
                        semesterflag = 1;
                        lblerror.Visible = false;
                        sprdremainmark.Visible = true;
                        sprdsubjectlist.Visible = true;
                        btnsave.Visible = true;
                        int graceexceedlimtremian = 0;
                        for (int i = 0; i < dsforstudent.Tables[0].Rows.Count; i++)
                        {
                            roll_no = dsforstudent.Tables[0].Rows[i]["roll_no"].ToString();

                            if (notfirsttime == 0)
                            {
                                if (ddlmodtype.SelectedValue == "Regular")
                                {
                                    gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[0]["ex_regular"]);
                                }
                                if (ddlmodtype.SelectedValue == "Arrear")
                                {
                                    gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[0]["ex_arrear"]);
                                }
                                intialgrace = gracemark;
                            }
                            if (notfirsttime == 1)
                            {
                                if (rollno != roll_no)
                                {
                                    if (flag == 1)
                                    {
                                        branchpass++;
                                    }

                                    //if (flagforreminingflash == 1)
                                    //{
                                    remaininggraace = gracemark + graceexceedlimtremian;
                                    hashforremainingmark.Add(rollno + "-" + examcode, remaininggraace + "-" + TextBox1.Text + "-" + "internal");
                                    Session["hashforremainingmark"] = hashforremainingmark;
                                    flagforreminingflash = 0;
                                    graceexceedlimtremian = 0;
                                    //}
                                    flag = 0;
                                    if (ddlmodtype.SelectedValue == "Regular")
                                    {
                                        gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["ex_regular"]);
                                    }
                                    if (ddlmodtype.SelectedValue == "Arrear")
                                    {
                                        gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["ex_arrear"]);
                                    }
                                    intialgrace = gracemark;

                                }
                            }

                            subject_no = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["Subject_no"]);
                            internal_mark = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["" + intmrktype + ""]);
                            mintotal = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["Mintotal"]);
                            mininternal = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["Min_int_marks"]);
                            minexternal = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["Min_ext_marks"]);

                            int internalmarknew = 0;
                            int markgiven = 0;
                            if (internal_mark < mininternal)
                            {
                                if (gracemark > gracelimit)
                                {
                                    graceexceedlimtremian = gracemark - gracelimit;
                                    gracemark = gracelimit;
                                }
                                internalmarknew = internal_mark + gracemark;

                            }
                            else
                            {
                                hashtosave.Add(roll_no + "-" + subject_no + "-" + examcode + "-" + intialgrace, markgiven + "-" + mintotal + "-" + mininternal + "-" + minexternal + "-" + internal_mark + "-" + internalmarknew + "-" + gracemark);
                                Session["hashtosave"] = hashtosave;
                                if (checkrollforbranchwise.Contains(roll_no + "-" + subject_no))
                                {
                                    flag = 3;
                                }
                                else
                                {
                                    checkrollforbranchwise.Add(roll_no + "-" + subject_no, 0);
                                    flag = 1;
                                }
                            }
                            if (internalmarknew >= mininternal)
                            {
                                flagforreminingflash = 1;
                                gracemark = internalmarknew - mininternal;
                                markgiven = Convert.ToInt32(mininternal - internal_mark);

                                hashtosave.Add(roll_no + "-" + subject_no + "-" + examcode + "-" + intialgrace, markgiven + "-" + mintotal + "-" + mininternal + "-" + minexternal + "-" + internal_mark + "-" + internalmarknew + "-" + gracemark);
                                Session["hashtosave"] = hashtosave;
                                //if (checkrollforbranchwise.Contains(roll_no + "-" + subject_no))
                                //{
                                //    flag = 3;
                                //}
                                //else
                                //{
                                //    checkrollforbranchwise.Add(roll_no + "-" + subject_no, 0);
                                flag = 1;
                                //}
                                //if (checkroll.Contains(roll_no + "-" + subject_no))
                                //{
                                //}
                                //else
                                //{
                                //    checkroll.Add(roll_no + "-" + subject_no, 0);


                                if (hash.Contains(subject_no))
                                {
                                    int value = Convert.ToInt32(GetCorrespondingKey(subject_no, hash));
                                    value++;
                                    hash[subject_no] = value;
                                }
                                else
                                {
                                    hash.Add(subject_no, count);
                                }

                                //}
                            }
                            //for last roll_no
                            int j = i + 1;
                            if (j == dsforstudent.Tables[0].Rows.Count)
                            {
                                if (flag == 1)
                                {
                                    branchpass++;
                                }
                                //if (flagforreminingflash == 1)
                                //{
                                remaininggraace = gracemark + graceexceedlimtremian;
                                hashforremainingmark.Add(roll_no + "-" + examcode, remaininggraace + "-" + TextBox1.Text + "-" + "internal");
                                Session["hashforremainingmark"] = hashforremainingmark;
                                flagforreminingflash = 0;
                                graceexceedlimtremian = 0;
                                //}
                                flag = 0;

                                if (ddlmodtype.SelectedValue == "Regular")
                                {
                                    gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["ex_regular"]);
                                }
                                if (ddlmodtype.SelectedValue == "Arrear")
                                {
                                    gracemark = Convert.ToInt32(dsforstudent.Tables[0].Rows[i]["ex_arrear"]);
                                }
                                intialgrace = gracemark;
                            }
                            rollno = roll_no;
                            notfirsttime = 1;
                        }

                    }
                    if (RadioBranchwise.Checked == true)
                    {
                        if (branchwiserrowcount != 0)
                        {
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, sprdsubjectlist.Sheets[0].ColumnCount - 2].Text = Convert.ToString(branchpass);
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, sprdsubjectlist.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, sprdsubjectlist.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            double perc = 0;
                            string totalstudents = sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, 2].Text;
                            if (totalstudents != "" && branchpass != 0)
                            {
                                string perc2 = "";
                                decimal perc1 = 0;
                                perc1 = ((Convert.ToDecimal(branchpass) / Convert.ToDecimal(totalstudents)) * 100);
                                perc1 = Math.Round(perc1, 2);
                                perc = Convert.ToDouble(perc1);
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, sprdsubjectlist.Sheets[0].ColumnCount - 1].Text = Convert.ToString(perc);
                            }
                            else
                            {
                                sprdsubjectlist.Sheets[0].Cells[sprdsubjectlist.Sheets[0].RowCount - branchwiserrowcount, sprdsubjectlist.Sheets[0].ColumnCount - 1].Text = "-";
                            }
                            branchwiserrowcount--;
                        }
                    }
                    if (Radiosubjectwise.Checked == true)
                    {
                        for (int i1 = 0; i1 <= sprdsubjectlist.Sheets[0].RowCount - 1; i1++)
                        {
                            sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                            sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 2].Text = "-";
                            sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 1].Text = "-";
                            if (sprdsubjectlist.Sheets[0].Cells[i1, 4].Note != "")
                            {
                                string alreadypass = sprdsubjectlist.Sheets[0].Cells[i1, 4].Text;
                                int subnofrmsprd = Convert.ToInt32(sprdsubjectlist.Sheets[0].Cells[i1, 4].Note);
                                foreach (DictionaryEntry parameter in hash)
                                {
                                    int count1 = 0;
                                    int subject_no1 = Convert.ToInt32(parameter.Key);
                                    count1 = Convert.ToInt32(parameter.Value);

                                    if (subnofrmsprd == subject_no1)
                                    {
                                        string count2 = Convert.ToString(count1 + Convert.ToInt32(alreadypass));
                                        sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 2].Text = count2;
                                        double perc = 0;
                                        string totalstudents = sprdsubjectlist.Sheets[0].Cells[i1, 3].Text;

                                        if (totalstudents != "" && count1 != 0)
                                        {
                                            string perc2 = "";
                                            decimal perc1 = 0;
                                            perc1 = ((Convert.ToDecimal(count2) / Convert.ToDecimal(totalstudents)) * 100);
                                            perc1 = Math.Round(perc1, 2);
                                            perc = Convert.ToDouble(perc1);
                                            sprdsubjectlist.Sheets[0].Cells[i1, sprdsubjectlist.Sheets[0].ColumnCount - 1].Text = Convert.ToString(perc);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            if (semesterflag == 0)
            {
                lblerror.Visible = true;
                sprdremainmark.Visible = false;
                sprdsubjectlist.Visible = false;
                btnsave.Visible = false;

            }
            else
            {
                lblerror.Visible = false;
                sprdremainmark.Visible = true;
                sprdsubjectlist.Visible = true;
                btnsave.Visible = true;

            }
            //}
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {

        foreach (DictionaryEntry parameter1 in hashtosave)
        {
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            string getroll = Convert.ToString(parameter1.Key);
            string getexmcde = Convert.ToString(parameter1.Value);
            string[] splitroll = getroll.Split(new Char[] { '-' });
            string[] splitexmcode = getexmcde.Split(new Char[] { '-' });
            string roll_no = splitroll[0].ToString();
            string Subject_no = splitroll[1].ToString();
            string Examcode = splitroll[2].ToString();
            string moderationmark = splitroll[3].ToString();
            string Passmark = splitexmcode[0].ToString();
            string mintotal = splitexmcode[1].ToString();
            string mininternal = splitexmcode[2].ToString();
            string minexternal = splitexmcode[3].ToString();
            string remainingmark = splitexmcode[6].ToString();
            string getmarkentryquery = "select internal_mark,external_mark,total,result,passorfail from mark_entry where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
            SqlDataAdapter damarkentry = new SqlDataAdapter(getmarkentryquery, con3);
            DataSet dsmarkentry = new DataSet();
            con3.Close();
            con3.Open();
            damarkentry.Fill(dsmarkentry);
            int oldexternamark = 0;
            int oldinternalmark = 0;
            int total = 0;
            int passorfail = 0;
            string result = "";
            int newinternal = 0;
            if (dsmarkentry.Tables[0].Rows.Count > 0)
            {
                oldexternamark = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["external_mark"]);
                oldinternalmark = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["internal_mark"]);
                total = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["total"]);
                newinternal = oldinternalmark + Convert.ToInt32(Passmark);
                int newtotal = oldexternamark + newinternal;
                if ((newinternal >= Convert.ToInt32(mininternal)) && (oldexternamark >= Convert.ToInt32(minexternal)))
                {
                    if (newtotal >= Convert.ToInt32(mintotal))
                    {
                        result = "Pass";
                        passorfail = 1;
                    }
                }
                else
                {
                    result = "Fail";
                    passorfail = 0;
                }
                string updatequery = "Update mark_entry set internal_mark=" + newinternal + ",total=" + newtotal + ",result='" + result + "',passorfail=" + passorfail + " where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
                SqlCommand updatemarkcmd = new SqlCommand(updatequery, con1);
                con1.Close();
                con1.Open();
                updatemarkcmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Mark Saved Successfully')", true);
                //for retriving 
                string selectexamcode = "select degree_code,batch_year,current_semester from exam_details where exam_code='" + Examcode + "'";
                SqlDataAdapter daselectexamcode = new SqlDataAdapter(selectexamcode, con1);
                DataSet dsselectexamcode = new DataSet();
                con1.Close();
                con1.Open();
                daselectexamcode.Fill(dsselectexamcode);
                int degreecode = 0;
                int sem = 0;
                int batchyear = 0;
                if (dsselectexamcode.Tables[0].Rows.Count > 0)
                {
                    degreecode = Convert.ToInt32(dsselectexamcode.Tables[0].Rows[0]["degree_code"]);
                    sem = Convert.ToInt32(dsselectexamcode.Tables[0].Rows[0]["current_semester"]);
                    batchyear = Convert.ToInt32(dsselectexamcode.Tables[0].Rows[0]["batch_year"]);

                    string selecttable = "select * from moderation where exam_code=" + Examcode + " and subject_no=" + Subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                    SqlDataAdapter daselecttable = new SqlDataAdapter(selecttable, con1);
                    DataSet dsselecttable = new DataSet();
                    con1.Close();
                    con1.Open();
                    daselecttable.Fill(dsselecttable);
                    if (dsselecttable.Tables[0].Rows.Count > 0)
                    {

                        string updatequery1 = "";
                        if (ddlmodtype.SelectedValue.ToString() == "Regular")
                        {
                            updatequery1 = "update moderation set roll_no='" + roll_no + "',bf_moderation_intmark=" + oldinternalmark + ",af_moderation_intmrk=" + newinternal + ",passmark='" + Passmark + "',remainingmark=" + remainingmark + ",moderation_mark=" + moderationmark + " where exam_code=" + Examcode + " and subject_no=" + Subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and semester=" + sem + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                        }
                        if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                        {
                            updatequery1 = "update moderation set roll_no='" + roll_no + "',bf_mod_arrear_internal=" + oldinternalmark + ",af_mod_arrear_internal=" + newinternal + ",passmark='" + Passmark + "',remainingmark=" + remainingmark + ",moderation_mark=" + moderationmark + " where exam_code=" + Examcode + " and subject_no=" + Subject_no + " and roll_no='" + roll_no + "' and degree_code=" + degreecode + " and semester=" + sem + " and exam_month=" + exammonth + " and exam_year=" + examyear + "";
                        }
                        SqlCommand createdummycmd = new SqlCommand(updatequery1, con1);
                        con1.Close();
                        con1.Open();
                        createdummycmd.ExecuteNonQuery();
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
                    }
                    else
                    {
                        string insertquery = "";
                        if (ddlmodtype.SelectedValue.ToString() == "Regular")
                        {
                            insertquery = "insert into moderation(batch_year,degree_code,exam_code,subject_no,Semester,roll_no,bf_moderation_intmark,af_moderation_intmrk,passmark,remainingmark,moderation_mark,exam_month,exam_year) values (" + batchyear + "," + degreecode + ",'" + Examcode + "'," + Subject_no + "," + sem + ",'" + roll_no + "'," + oldinternalmark + "," + newinternal + ",'" + Passmark + "'," + remainingmark + "," + moderationmark + "," + exammonth + "," + examyear + ")";
                        }
                        if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                        {
                            insertquery = "insert into moderation(batch_year,degree_code,exam_code,subject_no,Semester,roll_no,bf_mod_arrear_internal,af_mod_arrear_internal,passmark,remainingmark,moderation_mark,exam_month,exam_year) values (" + batchyear + "," + degreecode + ",'" + Examcode + "'," + Subject_no + "," + sem + ",'" + roll_no + "'," + oldinternalmark + "," + newinternal + ",'" + Passmark + "'," + remainingmark + "," + moderationmark + "," + exammonth + "," + examyear + ")";
                        }
                        SqlCommand createdummycmd = new SqlCommand(insertquery, con1);
                        con1.Close();
                        con1.Open();
                        createdummycmd.ExecuteNonQuery();
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
                    }
                }
            }
            string getcammarksquery = "select * from camarks where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
            SqlDataAdapter da4 = new SqlDataAdapter(getcammarksquery, con3);
            DataSet ds4 = new DataSet();
            con3.Close();
            con3.Open();
            da4.Fill(ds4);

            if (ds4.Tables[0].Rows.Count > 0)
            {
                double intmark = 0;
                string updatequery = "update camarks set total=" + newinternal + " where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
                SqlCommand createdummycmd = new SqlCommand(updatequery, con1);
                con1.Close();
                con1.Open();
                createdummycmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
            }
            else
            {
                string insertquery = "insert into camarks (subject_no,roll_no,exam_code,actual_total,total)values(" + Subject_no + ",'" + roll_no + "'," + Examcode + ",0," + newinternal + ")";
                SqlCommand createdummycmd = new SqlCommand(insertquery, con1);
                con1.Close();
                con1.Open();
                createdummycmd.ExecuteNonQuery();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
            }
        }
        foreach (DictionaryEntry parameter1 in hashforremainingmark)
        {
            string getroll = Convert.ToString(parameter1.Key);
            string getremainingmark = Convert.ToString(parameter1.Value);
            string[] splitroll = getroll.Split(new Char[] { '-' });
            string[] splitexmcode = getremainingmark.Split(new Char[] { '-' });
            string roll_no = splitroll[0].ToString();
            string Examcode = splitroll[1].ToString();
            string markremained = splitexmcode[0].ToString();
            string markapplied = splitexmcode[1].ToString();
            string marktype = splitexmcode[2].ToString();
            string selecttable = "select * from moderation_remaining_mark where exam_code=" + Examcode + " and roll_no='" + roll_no + "'";
            SqlDataAdapter daselecttable = new SqlDataAdapter(selecttable, con1);
            DataSet dsselecttable = new DataSet();
            con1.Close();
            con1.Open();
            daselecttable.Fill(dsselecttable);
            if (dsselecttable.Tables[0].Rows.Count > 0)
            {
                string updatequery1 = "";
                if (ddlmodtype.SelectedValue.ToString() == "Regular")
                {
                    updatequery1 = "update moderation_remaining_mark set Int_Regular=" + markremained + ",Int_Regular_applied=" + markapplied + " where exam_code=" + Examcode + "  and roll_no='" + roll_no + "'";
                }
                else if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                {
                    updatequery1 = "update moderation_remaining_mark set Int_arrear=" + markremained + ",Int_arrear_applied=" + markapplied + " where exam_code=" + Examcode + "  and roll_no='" + roll_no + "'";
                }
                SqlCommand createdummycmd = new SqlCommand(updatequery1, con1);
                con1.Close();
                con1.Open();
                createdummycmd.ExecuteNonQuery();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
            }
            else
            {
                string insertquery = "";
                if (ddlmodtype.SelectedValue.ToString() == "Regular")
                {
                    insertquery = "insert into moderation_remaining_mark(exam_code,roll_no,Int_Regular,Int_Regular_applied) values (" + Examcode + ",'" + roll_no + "'," + markremained + "," + markapplied + ")";
                }

                else if (ddlmodtype.SelectedValue.ToString() == "Arrear")
                {
                    insertquery = "insert into moderation_remaining_mark(exam_code,roll_no,Int_Arrear,Int_arrear_applied) values (" + Examcode + ",'" + roll_no + "'," + markremained + "," + markapplied + ")";
                }
                SqlCommand createdummycmd = new SqlCommand(insertquery, con1);
                con1.Close();
                con1.Open();
                createdummycmd.ExecuteNonQuery();
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved')", true);
            }
        }
    }
    protected void ddlmodtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        sprdremainmark.Visible = false;
        btnsave.Visible = false;
        sprdsubjectlist.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        if (ddlmodtype.SelectedValue.ToString() == "Regular")
        {
            //lblsem.Visible = false;
            //ddlsem.Visible = false;
        }
        else if (ddlmodtype.SelectedValue.ToString() == "Arrear")
        {
            //lblsem.Visible = true;
            //ddlsem.Visible = true;
            //string selectexamcode = "select distinct semester from mark_entry m,subject s,exam_details ed,syllabus_master sm,sub_sem ss where  ed.exam_code=m.exam_code and m.attempts>1 and s.subject_no=m.subject_no and ed.batch_year=sm.batch_year and ss.subtype_no=s.subtype_no and subject_type like 'th%' and sm.syll_code=s.syll_code and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " order by semester ";
            //SqlDataAdapter daselectexamcode = new SqlDataAdapter(selectexamcode, con1);
            //DataSet dsselectexamcode = new DataSet();
            //con1.Close();
            //con1.Open();
            //daselectexamcode.Fill(dsselectexamcode);
            //if (dsselectexamcode.Tables[0].Rows.Count > 0)
            //{
            //    ddlsem.DataSource = dsselectexamcode;
            //    ddlsem.DataValueField = "semester";
            //    ddlsem.DataTextField = "semester";
            //    ddlsem.DataBind();
            //}
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        sprdremainmark.Visible = false;
        btnsave.Visible = false;
        sprdsubjectlist.Visible = false;
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        sprdremainmark.Visible = false;
        btnsave.Visible = false;
        sprdsubjectlist.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        string degreecodequery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
        SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
        DataSet dsdegreecodequery = new DataSet();
        con1.Close();
        con1.Open();
        dadegreecodequery.Fill(dsdegreecodequery);

        if (dsdegreecodequery.Tables[0].Rows.Count > 0)
        {
            ddlbranch.DataSource = dsdegreecodequery;
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataTextField = "dept_acronym";
            ddlbranch.DataBind();

        }
        string semesterquery = "select distinct current_semester from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and d.degree_code=" + ddlbranch.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
        SqlDataAdapter dasemesterquery = new SqlDataAdapter(semesterquery, con1);
        DataSet dssemesterquery = new DataSet();
        con1.Close();
        con1.Open();
        dasemesterquery.Fill(dssemesterquery);

        if (dssemesterquery.Tables[0].Rows.Count > 0)
        {
            ddlsem.DataSource = dssemesterquery;
            ddlsem.DataValueField = "current_semester";
            ddlsem.DataTextField = "current_semester";
            ddlsem.DataBind();

        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btndelete.Visible = false;
        TextBox1.Text = "";
        sprdremainmark.Visible = false;
        btnsave.Visible = false;
        sprdsubjectlist.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        string semesterquery = "select distinct current_semester from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and d.degree_code=" + ddlbranch.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
        SqlDataAdapter dasemesterquery = new SqlDataAdapter(semesterquery, con1);
        DataSet dssemesterquery = new DataSet();
        con1.Close();
        con1.Open();
        dasemesterquery.Fill(dssemesterquery);

        if (dssemesterquery.Tables[0].Rows.Count > 0)
        {
            ddlsem.DataSource = dssemesterquery;
            ddlsem.DataValueField = "current_semester";
            ddlsem.DataTextField = "current_semester";
            ddlsem.DataBind();

        }
    }
    protected void btnapply_Click(object sender, EventArgs e)
    {
        btnsave.Visible = true;
        spreadbind();
        moderation();
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        string getdegreequery = "";
        getdegreequery = "select degree_code,batch_year,exam_code,current_semester from exam_details where exam_month=" + exammonth + " and exam_year=" + examyear + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlsem.SelectedValue.ToString() + "";
        SqlDataAdapter dagetdegreequery = new SqlDataAdapter(getdegreequery, con3);
        DataSet dsgetdegreequery = new DataSet();
        dagetdegreequery.Fill(dsgetdegreequery);
        con3.Close();
        con3.Open();
        string degree_code = "";
        string examcode = "";
        string semester = "";
        if (dsgetdegreequery.Tables[0].Rows.Count > 0)
        {
            degree_code = dsgetdegreequery.Tables[0].Rows[0]["degree_code"].ToString();
            examcode = dsgetdegreequery.Tables[0].Rows[0]["exam_code"].ToString();
        }
        // delete moderation table
        string deletemodquery = "delete from moderation where exam_code=" + examcode + "";
        SqlCommand deletemodquerycmd = new SqlCommand(deletemodquery, con1);
        con1.Close();
        con1.Open();
        deletemodquerycmd.ExecuteNonQuery();
        //delete moderation_remaining_table
        string deletemodremainquery = "delete from moderation_remaining_mark where exam_code=" + examcode + "";
        SqlCommand deletemodremainquerycmd = new SqlCommand(deletemodremainquery, con1);
        con1.Close();
        con1.Open();
        deletemodremainquerycmd.ExecuteNonQuery();

        foreach (DictionaryEntry parameter1 in hashtosave)
        {

            string getroll = Convert.ToString(parameter1.Key);
            string getexmcde = Convert.ToString(parameter1.Value);
            string[] splitroll = getroll.Split(new Char[] { '-' });
            string[] splitexmcode = getexmcde.Split(new Char[] { '-' });
            string roll_no = splitroll[0].ToString();
            string Subject_no = splitroll[1].ToString();
            string Examcode = splitroll[2].ToString();
            string moderationmark = splitroll[3].ToString();
            string Passmark = splitexmcode[0].ToString();
            string mintotal = splitexmcode[1].ToString();
            string mininternal = splitexmcode[2].ToString();
            string minexternal = splitexmcode[3].ToString();
            string remainingmark = splitexmcode[6].ToString();
            string getmarkentryquery = "select internal_mark,external_mark,total,result,actual_internal_mark,actual_external_mark,passorfail from mark_entry where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
            SqlDataAdapter damarkentry = new SqlDataAdapter(getmarkentryquery, con3);
            DataSet dsmarkentry = new DataSet();
            con3.Close();
            con3.Open();
            damarkentry.Fill(dsmarkentry);
            int externamark = 0;
            int internalmark = 0;
            int total = 0;
            int passorfail = 0;
            string result = "";

            if (dsmarkentry.Tables[0].Rows.Count > 0)
            {
                externamark = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["actual_external_mark"]);
                internalmark = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["actual_internal_mark"]);
                total = Convert.ToInt32(dsmarkentry.Tables[0].Rows[0]["total"]);
                int newtotal = internalmark + externamark;
                if ((internalmark >= Convert.ToInt32(mininternal)) && (externamark >= Convert.ToInt32(minexternal)))
                {
                    if (newtotal >= Convert.ToInt32(mintotal))
                    {
                        result = "Pass";
                        passorfail = 1;
                    }
                }
                else
                {
                    result = "Fail";
                    passorfail = 0;
                }
                string updatequery = "Update mark_entry set external_mark=" + externamark + ",internal_mark=" + internalmark + ",total=" + newtotal + ",result='" + result + "',passorfail=" + passorfail + " where exam_code=" + Examcode + " and roll_no='" + roll_no + "' and subject_no=" + Subject_no + "";
                SqlCommand updatemarkcmd = new SqlCommand(updatequery, con1);
                con1.Close();
                con1.Open();
                updatemarkcmd.ExecuteNonQuery();

            }
        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Moderation Mark Deleted Successfully')", true);
    }
}