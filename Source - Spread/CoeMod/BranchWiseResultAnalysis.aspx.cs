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



public partial class BranchWiseResultAnalysis : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_name = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_p = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_name_new = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_rank = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_roll = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_rank_till = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_cp = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable hat = new Hashtable();
    static Hashtable arrcount = new Hashtable();
    static Hashtable arrcount1 = new Hashtable();
    string CollegeCode;
    string batchyear = "";
    string current_sem = "";
    string degreecode = "";
    string dept_name = "";
    string examCode = "";
    string rollno = "";
    string rollno1 = "";
    string post = "";
    string query_rank = "";

    int total = 0;
    int allpass = 0, onearrear = 0, twoarrear = 0, threearrear = 0, morethan3 = 0;
    int add_var = 0;
    int semarrearcount = 0;
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
                RadioRegular.Checked = true;
                MonthandYear();
                spreadbind.Visible = false;
                spreadbind.Sheets[0].SheetCorner.RowCount = 7;
                spreadbind.Sheets[0].ColumnCount = 10;
                spreadbind.Sheets[0].RowHeader.Visible = false;
                spreadbind.Sheets[0].AutoPostBack = true;
                spreadbind.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                spreadbind.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                spreadbind.Sheets[0].DefaultStyle.Font.Bold = false;
                spreadbind.CommandBar.Visible = true;
                lblnorec.Visible = false;

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = spreadbind.FindControl("Update");
        Control cntCancelBtn = spreadbind.FindControl("Cancel");
        Control cntCopyBtn = spreadbind.FindControl("Copy");
        Control cntCutBtn = spreadbind.FindControl("Clear");
        Control cntPasteBtn = spreadbind.FindControl("Paste");
        Control cntPageNextBtn = spreadbind.FindControl("Next");
        Control cntPagePreviousBtn = spreadbind.FindControl("Prev");
        Control cntPagePrintPDFBtn = spreadbind.FindControl("PrintPDF");

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



        }

        base.Render(writer);
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlbranch.Items.Clear();
        spreadbind.Visible = false;
        if (ddldegree.SelectedValue.ToString() != "" && ddldegree.SelectedValue.ToString() != " ")
        {
            string exammonth = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            if (ddldegree.SelectedValue != "0")
            {

                string degreecodequery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
                DataSet dsdegreecodequery = new DataSet();
                con1.Close();
                con1.Open();
                dadegreecodequery.Fill(dsdegreecodequery);

                if (dsdegreecodequery.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    for (int i = 0; i < dsdegreecodequery.Tables[0].Rows.Count; i++)
                    {
                        int i1 = 1;
                        ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsdegreecodequery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsdegreecodequery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                        i1++;
                    }


                }
            }
            else if (ddldegree.SelectedValue == "0")
            {
                string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code  and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
                SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                DataSet dsbranchquery = new DataSet();
                con1.Close();
                con1.Open();
                dabranchquery.Fill(dsbranchquery);
                if (dsbranchquery.Tables[0].Rows.Count > 0)
                {
                    ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                    for (int i = 0; i < dsbranchquery.Tables[0].Rows.Count; i++)
                    {
                        int i1 = 1;
                        ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbranchquery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsbranchquery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                        i1++;
                    }


                }
                else
                {
                    ddlbranch.Items.Clear();
                }
            }
        }
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


        int year = Convert.ToInt16(DateTime.Today.Year);
        // year = 2012;
        ddlYear.Items.Clear();
        for (int l = 0; l <= 20; l++)
        {

            ddlYear.Items.Add(Convert.ToString(year - l));

        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        spreadbind.Visible = false;
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        button_click();

    }

    public void button_click()
    {
        lblnorec.Visible = false;
        int sl_no = 1;
        spreadbind.Sheets[0].AutoPostBack = true;
        string exammonth = ddlMonth.SelectedItem.Text;
        string examyear = ddlYear.SelectedValue.ToString();
        string degree_selection = ddldegree.SelectedItem.Text;
        string department_selection = ddlbranch.SelectedItem.Text;
        if (ddlMonth.SelectedValue != "0" && ddlYear.SelectedValue != "0" && ddlbranch.SelectedValue != "" && ddldegree.SelectedValue != "")
        {
            spreadbind.Sheets[0].SheetName = " ";
            spreadbind.Sheets[0].RowCount = 0;
            spreadbind.Sheets[0].Columns[0].Width = 50;
            spreadbind.Sheets[0].Columns[1].Width = 60;
            spreadbind.Sheets[0].Columns[2].Width = 60;
            spreadbind.Sheets[0].Columns[3].Width = 140;
            spreadbind.Sheets[0].Columns[4].Width = 60;
            spreadbind.Sheets[0].Columns[5].Width = 60;
            spreadbind.Sheets[0].Columns[6].Width = 60;
            spreadbind.Sheets[0].Columns[7].Width = 60;
            spreadbind.Sheets[0].Columns[8].Width = 60;
            spreadbind.Sheets[0].Columns[9].Width = 60;

            spreadbind.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Sl.No";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Regulation";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Subject Code";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Subject Name";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 4].Text = "Type";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 5].Text = "No.Of Appeared";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 6].Text = "No.Of Pass";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 7].Text = "No.Of Fail";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 8].Text = "No.Of Absent";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 9].Text = "% of Pass";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 9].HorizontalAlign = HorizontalAlign.Center;

            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            spreadbind.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            spreadbind.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
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

            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 5, 1);
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].Text = coll_name;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, spreadbind.Sheets[0].ColumnCount - 2);
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].Text = address;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, spreadbind.Sheets[0].ColumnCount - 2);
            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].Text = affiliated;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, spreadbind.Sheets[0].ColumnCount - 2);
            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Branch Wise Result Analysis" + " " + "-" + " " + exammonth + " " + examyear;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, spreadbind.Sheets[0].ColumnCount - 2);
            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;

            // spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].Text = degree_selection + " Degree -" + department_selection+" Department";
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].Text = " Degree: " + degree_selection + " - Department: " + department_selection;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.Black;


            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, spreadbind.Sheets[0].ColumnCount - 2);

            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.FromArgb(64, 64, 255);
            spreadbind.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.Black;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.Black;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Rows[6].BackColor = Color.FromArgb(214, 235, 255);
            spreadbind.Sheets[0].ColumnHeader.Rows[6].Font.Bold = true;
            spreadbind.Sheets[0].ColumnHeader.Rows[6].Font.Size = FontUnit.Medium;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 0].CellType = mi;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(1, spreadbind.Sheets[0].ColumnCount - 1, 5, 1);
            spreadbind.Sheets[0].ColumnHeader.Cells[1, spreadbind.Sheets[0].ColumnCount - 1].CellType = mi;

            //======
            string batchyearquery = "";
            int papertype = 0;

            //Modified by srinath 21July2015
            if (ddlbranch.SelectedItem.Text != "All")
            {
                // batchyearquery = "select distinct r.batch_year,r.current_semester,e.exam_code from registration r,exam_details e where e.batch_year=r.batch_year  and r.current_semester=e.current_semester and e.degree_code=r.degree_code and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
                batchyearquery = "select distinct r.batch_year,e.current_semester,e.exam_code from registration r,exam_details e where e.batch_year=r.batch_year  and e.degree_code=r.degree_code and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            else if (ddlbranch.SelectedItem.Text == "All")
            {
                //batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where d.course_id=c.course_id and e.batch_year=r.batch_year  and r.current_semester=e.current_semester and d.degree_code=e.degree_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
                batchyearquery = "select distinct r.batch_year,e.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where d.course_id=c.course_id and e.batch_year=r.batch_year  and d.degree_code=e.degree_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
                // batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,dept_acronym as branch from registration r,exam_details e,department dept,degree d where e.batch_year=r.batch_year and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            if (ddlbranch.SelectedItem.Text == "All" && ddldegree.SelectedItem.Text == "All")
            {
                //batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,c.course_name+' - '+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where e.batch_year=r.batch_year  and r.current_semester=e.current_semester and e.degree_code=r.degree_code and e.degree_code=d.degree_code and c.course_id=d.course_id and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and d.dept_code=dept.dept_code and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
                batchyearquery = "select distinct r.batch_year,e.current_semester,r.degree_code,dept_name,c.course_name+' - '+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where e.batch_year=r.batch_year and e.degree_code=r.degree_code and e.degree_code=d.degree_code and c.course_id=d.course_id and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and d.dept_code=dept.dept_code and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            DataSet dsbatchyearquery = d2.select_method_wo_parameter(batchyearquery, "Text");


            if (dsbatchyearquery.Tables[0].Rows.Count > 0)
            {
                lblnorec.Visible = false;

                for (int batchi = 0; batchi < dsbatchyearquery.Tables[0].Rows.Count; batchi++)
                {
                    batchyear = dsbatchyearquery.Tables[0].Rows[batchi]["batch_year"].ToString();
                    current_sem = dsbatchyearquery.Tables[0].Rows[batchi]["current_semester"].ToString();
                    examCode = dsbatchyearquery.Tables[0].Rows[batchi]["exam_code"].ToString();
                    if (ddlbranch.SelectedItem.Text != "All")
                    {
                        dept_name = ddldegree.SelectedItem.Text + "-" + ddlbranch.SelectedItem.Text;
                        degreecode = ddlbranch.SelectedValue.ToString();
                    }
                    else if (ddlbranch.SelectedItem.Text == "All")
                    {
                        dept_name = dsbatchyearquery.Tables[0].Rows[batchi]["branch"].ToString();
                        degreecode = dsbatchyearquery.Tables[0].Rows[batchi]["degree_code"].ToString();
                    }
                    string subjectquery = "";
                    if (RadioRegular.Checked == true)
                    {
                        papertype = 1;
                        subjectquery = "select distinct subject_name ,subject_code,s.subject_no from subject s,subjectchooser sc,registration r,sub_sem ss where sc.subject_no=s.subject_no and sc.roll_no=r.roll_no and s.subType_no=ss.subType_no and ss.promote_count=1 and r.degree_code='" + degreecode + "' and r.batch_year='" + batchyear + "' and sc.semester='" + current_sem + "'";
                        //subjectquery = "select distinct batch_year,exam_appl_details.subject_no,subject_name ,subject_code,current_semester from exam_appl_details,subject,exam_details,exam_application where exam_appl_details.attempts = 0 and subject.subject_no = exam_appl_details.subject_no and ltrim(rtrim(exam_appl_details.type))='' and exam_appl_details.appl_no=exam_application.appl_no and exam_application.exam_code=exam_details.exam_code and exam_details.batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester=" + current_sem + " and exam_details.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_details.exam_year=" + ddlYear.SelectedValue.ToString() + " order by batch_year,current_semester desc";
                    }
                    else if (RadioArrear.Checked == true)
                    {
                        papertype = 0;
                        subjectquery = "select distinct batch_year,exam_appl_details.subject_no,subject_name ,subject_code,current_semester from exam_appl_details,subject,exam_details,exam_application where  subject.subject_no = exam_appl_details.subject_no  and exam_appl_details.appl_no=exam_application.appl_no and exam_application.exam_code=exam_details.exam_code and exam_details.batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester=" + current_sem + " and exam_details.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_details.exam_year=" + ddlYear.SelectedValue.ToString() + " order by batch_year,current_semester desc";//exam_appl_details.attempts>0 and   and ltrim(rtrim(exam_appl_details.type))='*'
                    }
                    DataSet dssubjectquery = d2.select_method_wo_parameter(subjectquery, "Text");
                    if (dssubjectquery.Tables[0].Rows.Count > 0)
                    {
                        lblnorec.Visible = false;
                        string subject_no = "";
                        string subject_name = "";
                        string subject_code = "";
                        string exam_code = "";
                        string studsemester = "";

                        for (int subjecti = 0; subjecti < dssubjectquery.Tables[0].Rows.Count; subjecti++)
                        {
                            spreadbind.Visible = true;
                            subject_no = dssubjectquery.Tables[0].Rows[subjecti]["subject_no"].ToString();
                            subject_name = dssubjectquery.Tables[0].Rows[subjecti]["subject_name"].ToString();
                            subject_code = dssubjectquery.Tables[0].Rows[subjecti]["subject_code"].ToString();
                            //exam_code = dssubjectquery.Tables[0].Rows[subjecti]["exam_code"].ToString();
                            if (RadioArrear.Checked == true)
                            {
                                studsemester = dssubjectquery.Tables[0].Rows[subjecti]["current_semester"].ToString();
                            }
                            else if (RadioRegular.Checked == true)
                            {
                                studsemester = current_sem;
                            }

                            SqlCommand studinfo = new SqlCommand("procbranchwiseresultanalysis", con);
                            studinfo.CommandType = CommandType.StoredProcedure;
                            studinfo.Parameters.AddWithValue("@degreecode", degreecode);
                            studinfo.Parameters.AddWithValue("@batchyear", batchyear);
                            studinfo.Parameters.AddWithValue("@semester", current_sem);
                            studinfo.Parameters.AddWithValue("@subject_no", subject_no);
                            studinfo.Parameters.AddWithValue("@examcode", examCode);
                            studinfo.Parameters.AddWithValue("@papertype", papertype);
                            SqlDataAdapter studinfoada = new SqlDataAdapter(studinfo);
                            DataSet studinfoads = new DataSet();
                            studinfoada.Fill(studinfoads);

                            if (studinfoads.Tables[0].Rows.Count > 0)
                            {
                                lblnorec.Visible = false;
                                string studentappeared = "";
                                string studentpassed = "";
                                string studentfail = "";
                                string totalstudents = "";
                                double pass_percent = 0;
                                for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                                {
                                    totalstudents = studinfoads.Tables[4].Rows[studproci][0].ToString();
                                    studentappeared = studinfoads.Tables[1].Rows[studproci][0].ToString();
                                    studentpassed = studinfoads.Tables[2].Rows[studproci][0].ToString();
                                    studentfail = studinfoads.Tables[3].Rows[studproci][0].ToString();
                                    spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 1;

                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].Text = batchyear;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Note = subject_no;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = subject_code.ToString();
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = subject_name;



                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].Text = studentappeared;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;


                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = studentpassed;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;


                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].Text = studentfail;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                                    if (int.Parse(studentappeared) != 0)
                                    {
                                        float qqqq = 0;
                                        qqqq = ((float.Parse(studentpassed) / float.Parse(studentappeared)) * 100);
                                        pass_percent = Math.Round(qqqq, 2);
                                    }
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].Text = pass_percent.ToString();


                                    int absent = 0;
                                    string type = "";
                                    if (current_sem == studsemester)
                                    {
                                        type = "Regular";
                                    }
                                    else if (current_sem != studsemester)
                                    {
                                        type = "Arrear";
                                    }

                                    if (totalstudents != "0")
                                    {

                                        absent = Convert.ToInt32(totalstudents) - Convert.ToInt32(studentappeared);
                                    }

                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = type;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                    // spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(absent);
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = studinfoads.Tables[5].Rows[studproci][0].ToString();
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

                                }
                                sl_no++;
                            }
                            else
                            {
                                lblnorec.Visible = true;
                            }


                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                    }
                }


            }
            else
            {
                lblnorec.Visible = true;
            }
            int totalrows = spreadbind.Sheets[0].RowCount;
            spreadbind.Sheets[0].PageSize = (totalrows * 25) + 40;
            spreadbind.Height = (totalrows * 25) + 40;
            spreadbind.Width = 1270;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.Black;
        }
    }
    public void arrear_position()
    {
        spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 3;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 3, 0, 2, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "ARREARS POSITION - " + current_sem + " SEMESTER";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Underline = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Row.Height = 3;
        spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 2;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 0, 2, 1);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].Text = "Description";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 1, 2, 1);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].Text = "All Pass";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].VerticalAlign = VerticalAlign.Middle;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 2, 1, 8);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].Text = "Arrear Details";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = "One Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = "Two Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 4, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = "Three Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 6, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = "More Than Three";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 8, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = "Over all % of Pass";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "No.Of Students";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 4, 1, 2);
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 6, 1, 2);
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 8, 1, 2);

        //********************

        con_name.Close();
        con_name.Open();
        SqlCommand studinfo_new = new SqlCommand("proc_arrear_roll", con_name);
        studinfo_new.CommandType = CommandType.StoredProcedure;
        studinfo_new.Parameters.AddWithValue("@batchyear_p", batchyear);
        studinfo_new.Parameters.AddWithValue("@degreecode_p", degreecode);
        studinfo_new.Parameters.AddWithValue("@cur_sem_p", current_sem);
        SqlDataAdapter ada_roll = new SqlDataAdapter(studinfo_new);
        DataSet ds_roll = new DataSet();
        ada_roll.Fill(ds_roll);



        for (int sub = 0; sub < ds_roll.Tables[0].Rows.Count; sub++)
        {

            rollno = ds_roll.Tables[0].Rows[sub]["roll_no"].ToString();

            string arrsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='pass' and ltrim(rtrim(roll_no))='" + rollno + "') and ltrim(rtrim(roll_no))='" + rollno + "' and Semester >= 1 and Semester <= " + current_sem + " ) order by smas.semester , scode";

            con_p.Close();
            con_p.Open();
            //SqlCommand studinfo = new SqlCommand("arrear_count", con_p);
            SqlCommand studinfo = new SqlCommand(arrsub, con_p);
            //studinfo.CommandType = CommandType.StoredProcedure;
            //studinfo.Parameters.AddWithValue("@rollno_p", rollno);
            //studinfo.Parameters.AddWithValue("@cur_sem_p", current_sem);
            SqlDataAdapter daarrsub = new SqlDataAdapter(studinfo);
            DataSet dsarrsub = new DataSet();
            daarrsub.Fill(dsarrsub);
            post = "";
            if (dsarrsub.Tables[0].Rows.Count > 0)
            {
                semarrearcount = dsarrsub.Tables[0].Rows.Count;
                if (semarrearcount >= 4)
                {
                    semarrearcount = 4;
                }
            }
            else
            {
                semarrearcount = 0;
            }

            if (arrcount.Contains(degreecode + "," + batchyear + "," + semarrearcount))
            {
                string prevroll = Convert.ToString(GetCorrespondingKey(degreecode + "," + batchyear + "," + semarrearcount, arrcount));
                string newroll = rollno + "," + prevroll;
                arrcount[degreecode + "," + batchyear + "," + semarrearcount] = newroll;


            }
            else
            {
                arrcount.Add(degreecode + "," + batchyear + "," + semarrearcount, rollno);
            }
        }

        foreach (DictionaryEntry parameter in arrcount)
        {


            int noofstud = 0;
            string b_year = "";
            string d_code = "";
            string subcount = Convert.ToString(parameter.Key);
            string Rollno = Convert.ToString(parameter.Value);
            string[] splitsubcount = subcount.Split(new char[] { ',' });
            d_code = splitsubcount[0].ToString();
            b_year = splitsubcount[1].ToString();
            subcount = splitsubcount[2].ToString();

            for (int i = 0; i <= 4; i++)
            {
                if (Convert.ToInt32(subcount) == i && b_year == batchyear && d_code == degreecode)
                {
                    string[] split = Rollno.Split(new char[] { ',' });

                    noofstud = split.GetUpperBound(0);
                    noofstud = noofstud + 1;
                    if (i == 0)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        allpass = noofstud;
                    }
                    if (i == 1)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        onearrear = noofstud;
                    }
                    if (i == 2)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        twoarrear = noofstud;
                    }
                    if (i == 3)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = noofstud.ToString();

                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        threearrear = noofstud;
                    }
                    if (i == 4)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        morethan3 = noofstud;
                    }


                }
            }
        }

        total = allpass + onearrear + twoarrear + threearrear + morethan3;
        double overallpasspercent = ((Convert.ToDouble(allpass) / (Convert.ToDouble(total)) * 100));
        float ssq = float.Parse(overallpasspercent.ToString());
        double ss = Math.Round(ssq, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = ss.ToString();
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
        allpass = 0;
        onearrear = 0;
        twoarrear = 0;
        threearrear = 0;
        morethan3 = 0;
        //---------------
        spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 3;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 3, 0, 2, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "CUMULATIVE ARREARS POSITION - TILL CURRENT SEMESTER";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Underline = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Row.Height = 3;
        spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 2;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 0, 2, 1);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].Text = "Description";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 1, 2, 1);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].Text = "All Pass";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 1].VerticalAlign = VerticalAlign.Middle;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 2, 2, 1, 8);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].Text = "Overall Arrear Details";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = "One Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = "Two Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 4, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = "Three Arrear";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 6, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = "More Than Three";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 8, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = "Over all % of Pass";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 2, 8].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;

        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "No.Of Students";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 4, 1, 2);
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 6, 1, 2);
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 8, 1, 2);


        con_name_new.Close();
        con_name_new.Open();
        SqlCommand studinfo_new1 = new SqlCommand("proc_arrear_roll", con_name_new);
        studinfo_new1.CommandType = CommandType.StoredProcedure;
        studinfo_new1.Parameters.AddWithValue("@batchyear_p", batchyear);
        studinfo_new1.Parameters.AddWithValue("@degreecode_p", degreecode);
        studinfo_new1.Parameters.AddWithValue("@cur_sem_p", current_sem);
        SqlDataAdapter ada_roll1 = new SqlDataAdapter(studinfo_new1);
        DataSet ds_roll1 = new DataSet();
        ada_roll1.Fill(ds_roll1);



        for (int sub = 0; sub < ds_roll1.Tables[0].Rows.Count; sub++)
        {

            rollno1 = ds_roll1.Tables[0].Rows[sub]["roll_no"].ToString();

            //string arrsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='pass' and ltrim(rtrim(roll_no))='"+rollno +"') and ltrim(rtrim(roll_no))='"+rollno +"' and Semester >= 1 and Semester <= "+cur_semtbl +" ) order by smas.semester , scode";

            con_p.Close();
            con_p.Open();
            SqlCommand studinfo1 = new SqlCommand("proc_arrear", con_p);
            studinfo1.CommandType = CommandType.StoredProcedure;
            studinfo1.Parameters.AddWithValue("@rollno_p", rollno1);
            studinfo1.Parameters.AddWithValue("@cur_sem_p", current_sem);
            SqlDataAdapter daarrsub1 = new SqlDataAdapter(studinfo1);
            DataSet dsarrsub1 = new DataSet();
            daarrsub1.Fill(dsarrsub1);
            post = "";
            if (dsarrsub1.Tables[0].Rows.Count > 0)
            {
                semarrearcount = dsarrsub1.Tables[0].Rows.Count;
                if (semarrearcount >= 4)
                {
                    semarrearcount = 4;
                }
            }
            else
            {
                semarrearcount = 0;
            }

            if (arrcount1.Contains(degreecode + "," + batchyear + "," + semarrearcount))
            {
                string prevroll = Convert.ToString(GetCorrespondingKey(degreecode + "," + batchyear + "," + semarrearcount, arrcount1));
                string newroll = rollno1 + "," + prevroll;
                arrcount1[degreecode + "," + batchyear + "," + semarrearcount] = newroll;


            }
            else
            {
                arrcount1.Add(degreecode + "," + batchyear + "," + semarrearcount, rollno1);
            }
        }

        foreach (DictionaryEntry parameter in arrcount1)
        {


            int noofstud = 0;
            string b_year = "";
            string d_code = "";

            string subcount = Convert.ToString(parameter.Key);
            string Rollno = Convert.ToString(parameter.Value);
            string[] splitsubcount = subcount.Split(new char[] { ',' });
            d_code = splitsubcount[0].ToString();
            b_year = splitsubcount[1].ToString();
            subcount = splitsubcount[2].ToString();

            for (int i = 0; i <= 4; i++)
            {
                if (Convert.ToInt32(subcount) == i && b_year == batchyear && d_code == degreecode)
                {
                    string[] split = Rollno.Split(new char[] { ',' });

                    noofstud = split.GetUpperBound(0);
                    noofstud = noofstud + 1;
                    if (i == 0)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        allpass = noofstud;
                    }
                    if (i == 1)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        onearrear = noofstud;
                    }
                    if (i == 2)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        twoarrear = noofstud;
                    }
                    if (i == 3)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        threearrear = noofstud;
                    }
                    if (i == 4)
                    {
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = noofstud.ToString();
                        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        morethan3 = noofstud;
                    }


                }
            }


        }

        int total1 = 0;
        total1 = allpass + onearrear + twoarrear + threearrear + morethan3;
        double overallpasspercent1 = ((Convert.ToDouble(allpass) / (Convert.ToDouble(total1)) * 100));
        float ss12 = float.Parse(overallpasspercent1.ToString());
        double ss1 = Math.Round(ss12, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = ss1.ToString();
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;


        //*++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 3;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 3, 0, 2, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "PERFORMANCE";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Underline = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 10);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Row.Height = 3;
        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 5);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "Class Toppers - For this Semester (Top Five)";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 5, 1, 5);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].Text = "Class Toppers - Till this Year / Semester (Top Five)";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].RowCount++;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = "Name";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = "% of Mark";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = "Rank";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = "CP";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 5, 1, 2);
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].Text = "Name";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].Text = "% of Mark";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = "Rank";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].Text = "CP";
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].Font.Bold = true;
        spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;

        //--*********
        int rt = 1;
        string roll_no_new = "";
        string query_cp = "";
        con_rank.Close();
        con_rank.Open();

        //query_rank = "Select  top 5 sum(total) as ObtTotal,sum(maxtotal) as AllTotal,round(((sum(total)/sum(maxtotal))*100),3) as totalperc,registration.Roll_Admit , registration.roll_no, registration.reg_no from registration,Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and registration.roll_no = mark_entry.roll_no  and (result='Pass' or result='pass')and exam_code in(select distinct exam_code from exam_details where  degree_code='" + degreecode + "' and  batch_year='" + batchyear + "' and  current_semester='" + current_sem + "')group by registration.roll_no,registration.reg_no,registration.Roll_Admit order by totalperc desc";
        query_rank = "Select  top 5 sum(total) as ObtTotal,sum(maxtotal) as AllTotal,round(((sum(total)/sum(maxtotal))*100),3) as totalperc,registration.Roll_Admit , registration.roll_no, registration.reg_no from registration,Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and registration.roll_no = mark_entry.roll_no  and (result='Pass' or result='pass')and exam_code in(select distinct exam_code from exam_details where ";
        if (ddlbranch.SelectedItem.Text != "All")
        {
            query_rank = query_rank + "degree_code='" + degreecode + "' and ";

        }

        query_rank = query_rank + " batch_year='" + batchyear + "' and  current_semester='" + current_sem + "')group by registration.roll_no,registration.reg_no,registration.Roll_Admit order by totalperc desc";

        SqlCommand com_rank = new SqlCommand(query_rank, con_rank);
        SqlDataReader dr_rank = com_rank.ExecuteReader();
        while (dr_rank.Read())
        {
            spreadbind.Sheets[0].RowCount++;
            con_roll.Close();
            con_roll.Open();
            roll_no_new = "select stud_name from registration where batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester=" + current_sem + "and delflag=0 and roll_no='" + dr_rank["roll_no"] + "'";
            SqlCommand com_roll = new SqlCommand(roll_no_new, con_roll);
            SqlDataReader dr_roll = com_roll.ExecuteReader();
            dr_roll.Read();
            if (dr_roll.HasRows == true)
            {
                spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 0, 1, 2);
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 0].Text = dr_roll["stud_name"].ToString();
            }

            con_cp.Close();
            con_cp.Open();
            query_cp = "select SUM(g.credit_points) from grade_Master g,registration r where r.batch_year=g.batch_year and r.degree_code=g.degree_code AND R.ROLL_NO='" + dr_rank["roll_no"] + "' and r.batch_year=" + batchyear + " and r.degree_code=" + degreecode + " and r.current_semester=" + current_sem + "";
            SqlCommand com_cp = new SqlCommand(query_cp, con_cp);
            SqlDataReader dr_cp = com_cp.ExecuteReader();
            dr_cp.Read();
            if (dr_cp.HasRows == true)
            {
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = dr_cp[0].ToString();
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

            }

            double sdc = Convert.ToDouble(dr_rank["totalperc"]);
            double a = Math.Round(sdc, 2);
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = a.ToString();
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = rt.ToString();
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;


            rt++;

        }



        //***--------------
        int rt_til = 1;
        int y = 5;
        int rt1 = 1;
        string roll_no_new_till = "", query_till;
        con_rank_till.Close();
        con_rank_till.Open();
        //query_till = "Select  top 5 sum(total) as ObtTotal,sum(maxtotal) as AllTotal,round(((sum(total)/sum(maxtotal))*100),3) as totalperc,registration.Roll_Admit , registration.roll_no, registration.reg_no from registration,Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and registration.roll_no = mark_entry.roll_no  and (result='Pass' or result='pass')and exam_code in(select distinct exam_code from exam_details where  degree_code='" + degreecode + "' and  batch_year='" + batchyear + "' and  current_semester<='" + current_sem + "')group by registration.roll_no,registration.reg_no,registration.Roll_Admit order by totalperc desc";
        query_till = "Select  top 5 sum(total) as ObtTotal,sum(maxtotal) as AllTotal,round(((sum(total)/sum(maxtotal))*100),3) as totalperc,registration.Roll_Admit , registration.roll_no, registration.reg_no from registration,Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and registration.roll_no = mark_entry.roll_no  and (result='Pass' or result='pass')and exam_code in(select distinct exam_code from exam_details where ";
        if (ddlbranch.SelectedItem.Text != "All")
        {
            query_till = query_till + "degree_code='" + degreecode + "' and ";

        }

        query_till = query_till + " batch_year='" + batchyear + "' and  current_semester<='" + current_sem + "')group by registration.roll_no,registration.reg_no,registration.Roll_Admit order by totalperc desc";
        SqlCommand com_rank_till = new SqlCommand(query_till, con_rank_till);
        SqlDataReader dr_rank_till = com_rank_till.ExecuteReader();
        while (dr_rank_till.Read())
        {


            con_roll.Close();
            con_roll.Open();
            roll_no_new_till = "select stud_name from registration where batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester=" + current_sem + "and delflag=0 and roll_no='" + dr_rank_till["roll_no"] + "'";
            SqlCommand com_roll_till = new SqlCommand(roll_no_new_till, con_roll);
            SqlDataReader dr_roll_till = com_roll_till.ExecuteReader();
            dr_roll_till.Read();
            if (dr_roll_till.HasRows == true)
            {
                spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - y, 5, 1, 2);
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 5].Text = dr_roll_till["stud_name"].ToString();
            }

            string query_cp1 = "";
            con_cp.Close();
            con_cp.Open();
            query_cp1 = "select SUM(g.credit_points) from grade_Master g,registration r where r.batch_year=g.batch_year and r.degree_code=g.degree_code AND R.ROLL_NO='" + dr_rank_till["roll_no"] + "' and r.batch_year=" + batchyear + " and r.degree_code=" + degreecode + " and r.current_semester<=" + current_sem + "";
            SqlCommand com_cp1 = new SqlCommand(query_cp1, con_cp);
            SqlDataReader dr_cp1 = com_cp1.ExecuteReader();
            dr_cp1.Read();
            if (dr_cp1.HasRows == true)
            {
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 9].Text = dr_cp1[0].ToString();
                spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 9].HorizontalAlign = HorizontalAlign.Center;

            }
            double sdcx = Convert.ToDouble(dr_rank_till["totalperc"]);
            double b = Math.Round(sdcx, 2);
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 7].Text = b.ToString();
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 8].Text = rt1.ToString();
            spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - y, 8].HorizontalAlign = HorizontalAlign.Center;

            y--;
            rt1++;

        }



    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        spreadbind.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        ddldegree.Items.Clear();
        ddlbranch.Items.Clear();
        string collCode=Convert.ToString(Session["collegecode"]);
        if (ddlMonth.SelectedValue.ToString() != "0" && ddlYear.SelectedValue.ToString() != "0")
        {
            string degreecodequery = "select distinct c.course_name,c.course_id from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and  ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
            SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
            DataSet dsdegreecodequery = new DataSet();
            con1.Close();
            con1.Open();
            dadegreecodequery.Fill(dsdegreecodequery);

            if (dsdegreecodequery.Tables[0].Rows.Count > 0)
            {
                ddldegree.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                for (int i = 0; i < dsdegreecodequery.Tables[0].Rows.Count; i++)
                {
                    int i1 = 1;
                    ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsdegreecodequery.Tables[0].Rows[i]["course_name"].ToString() + "", "" + dsdegreecodequery.Tables[0].Rows[i]["course_id"].ToString() + ""));
                    i1++;
                }

            }
            else
            {
                ddldegree.Items.Clear();
            }
            if (ddldegree.SelectedValue != "")
            {
                if (ddldegree.SelectedValue != "0")
                {
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
                    SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                    DataSet dsbranchquery = new DataSet();
                    con1.Close();
                    con1.Open();
                    dabranchquery.Fill(dsbranchquery);
                    if (dsbranchquery.Tables[0].Rows.Count > 0)
                    {
                        ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                        for (int i = 0; i < dsdegreecodequery.Tables[0].Rows.Count; i++)
                        {
                            int i1 = 1;
                            ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbranchquery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsbranchquery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                            i1++;
                        }


                    }
                    else
                    {
                        ddlbranch.Items.Clear();
                    }
                }
                else if (ddldegree.SelectedValue == "0")
                {
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code  and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
                    SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                    DataSet dsbranchquery = new DataSet();
                    con1.Close();
                    con1.Open();
                    dabranchquery.Fill(dsbranchquery);
                    if (dsbranchquery.Tables[0].Rows.Count > 0)
                    {
                        ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                        for (int i = 0; i < dsbranchquery.Tables[0].Rows.Count; i++)
                        {
                            int i1 = 1;
                            ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbranchquery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsbranchquery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                            i1++;
                        }


                    }
                    else
                    {
                        ddlbranch.Items.Clear();
                    }
                }
            }
        }

    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        spreadbind.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        ddldegree.Items.Clear();
        ddlbranch.Items.Clear();
        string collCode = Convert.ToString(Session["collegecode"]);
        if (ddlMonth.SelectedValue.ToString() != "0" && ddlYear.SelectedValue.ToString() != "0")
        {
            string degreecodequery = "select distinct c.course_name,c.course_id from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and  ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
            SqlDataAdapter dadegreecodequery = new SqlDataAdapter(degreecodequery, con1);
            DataSet dsdegreecodequery = new DataSet();
            con1.Close();
            con1.Open();
            dadegreecodequery.Fill(dsdegreecodequery);

            if (dsdegreecodequery.Tables[0].Rows.Count > 0)
            {
                ddldegree.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                for (int i = 0; i < dsdegreecodequery.Tables[0].Rows.Count; i++)
                {
                    int i1 = 1;
                    ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsdegreecodequery.Tables[0].Rows[i]["course_name"].ToString() + "", "" + dsdegreecodequery.Tables[0].Rows[i]["course_id"].ToString() + ""));
                    i1++;
                }

            }

            else
            {
                ddldegree.Items.Clear();
            }
            if (ddldegree.SelectedValue != "")
            {
                if (ddldegree.SelectedValue != "0")
                {
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
                    SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                    DataSet dsbranchquery = new DataSet();
                    con1.Close();
                    con1.Open();
                    dabranchquery.Fill(dsbranchquery);
                    if (dsbranchquery.Tables[0].Rows.Count > 0)
                    {
                        ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                        for (int i = 0; i < dsbranchquery.Tables[0].Rows.Count; i++)
                        {
                            int i1 = 1;
                            ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbranchquery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsbranchquery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                            i1++;
                        }


                    }
                    else
                    {
                        ddlbranch.Items.Clear();
                    }
                }
                else if (ddldegree.SelectedValue == "0")
                {
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code  and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and d.college_code='" + collCode + "'";
                    SqlDataAdapter dabranchquery = new SqlDataAdapter(branchquery, con1);
                    DataSet dsbranchquery = new DataSet();
                    con1.Close();
                    con1.Open();
                    dabranchquery.Fill(dsbranchquery);
                    if (dsbranchquery.Tables[0].Rows.Count > 0)
                    {
                        ddlbranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                        for (int i = 0; i < dsbranchquery.Tables[0].Rows.Count; i++)
                        {
                            int i1 = 1;
                            ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsbranchquery.Tables[0].Rows[i]["dept_acronym"].ToString() + "", "" + dsbranchquery.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                            i1++;
                        }


                    }
                    else
                    {
                        ddlbranch.Items.Clear();
                    }
                }
            }
        }
    }
    protected void chechbox1_CheckedChanged(object sender, EventArgs e)
    {
        if (chechbox1.Checked == true)
        {
            button_click();
            arrear_position();
        }
        else
        {
            button_click();
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
}