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



public partial class SubjectWiseResultAnalysis2 : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
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
                RadioRegular.Checked = true;
                MonthandYear();
                spreadbind.Visible = false;
                spreadbind.Sheets[0].SheetCorner.RowCount = 8;
                spreadbind.Sheets[0].ColumnCount = 16;
                spreadbind.Sheets[0].RowHeader.Visible = false;
                spreadbind.Sheets[0].AutoPostBack = true;
                spreadbind.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                spreadbind.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                spreadbind.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                spreadbind.Sheets[0].DefaultStyle.Font.Bold = false;
                spreadbind.CommandBar.Visible = true;

            }
        }
        catch
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

    protected void btngo_Click(object sender, EventArgs e)
    {
        Hashtable commonsub = new Hashtable();
        Hashtable addsamerow = new Hashtable();
        Hashtable commonsem = new Hashtable();
        spreadbind.Sheets[0].AutoPostBack = true;
        string exammonth = ddlMonth.SelectedItem.Text;
        string examyear = ddlYear.SelectedValue.ToString();
        spreadbind.Visible = false;
        if (ddlMonth.SelectedValue != "0" && ddlYear.SelectedValue != "0" && ddlbranch.SelectedValue != "" && ddldegree.SelectedValue != "")
        {
            spreadbind.Sheets[0].SheetName = " ";
            spreadbind.Sheets[0].RowCount = 0;
            spreadbind.Sheets[0].Columns[0].Width = 100;
            spreadbind.Sheets[0].Columns[1].Width = 120;
            spreadbind.Sheets[0].Columns[2].Width = 80;
            spreadbind.Sheets[0].Columns[3].Width = 80;
            spreadbind.Sheets[0].Columns[4].Width = 190;
            spreadbind.Sheets[0].Columns[5].Width = 100;
            spreadbind.Sheets[0].Columns[6].Width = 60;
            spreadbind.Sheets[0].Columns[7].Width = 80;
            spreadbind.Sheets[0].Columns[8].Width = 50;
            spreadbind.Sheets[0].Columns[9].Width = 50;
            spreadbind.Sheets[0].Columns[10].Width = 50;
            spreadbind.Sheets[0].Columns[11].Width = 50;
            spreadbind.Sheets[0].Columns[12].Width = 50;
            spreadbind.Sheets[0].Columns[13].Width = 50;
            spreadbind.Sheets[0].Columns[14].Width = 50;
            spreadbind.Sheets[0].Columns[15].Width = 50;
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 0].Text = "Regulation";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Branch";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Semester";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 3].Text = "Subject Code";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 4].Text = "Subject Name";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 5].Text = "Common Subject Branch";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 6].Text = "Type";
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 7].Text = "Students Registered";
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 0, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 1, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 4, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 5, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 6, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 7, 2, 1);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 8, 1, 2);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 10, 1, 2);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 12, 1, 2);
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 14, 1, 2);


            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 8, 2, 2);//**
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 8].Text = "No.Of Appeared";

            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 10, 2, 2);//**
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 10].Text = "No.Of Pass";

            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 12, 2, 2);//**
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 12].Text = "No.Of Fail";

            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(6, 14, 2, 2);//**
            spreadbind.Sheets[0].ColumnHeader.Cells[6, 14].Text = "No.Of Absent";



            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 8].Text = "No.";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 9].Text = "%";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 10].Text = "No.";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 11].Text = "%";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 12].Text = "No.";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 13].Text = "%";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 14].Text = "No.";
            //spreadbind.Sheets[0].ColumnHeader.Cells[7, 15].Text = "%";
            spreadbind.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Rows[7].HorizontalAlign = HorizontalAlign.Center;
            //=header
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
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, spreadbind.Sheets[0].ColumnCount - 3);
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].Text = address;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, spreadbind.Sheets[0].ColumnCount - 3);
            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].Text = affiliated;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, spreadbind.Sheets[0].ColumnCount - 3);
            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Subject Wise Result Analysis" + " " + "-" + " " + exammonth + " " + examyear;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, spreadbind.Sheets[0].ColumnCount - 3);
            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;


            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, spreadbind.Sheets[0].ColumnCount - 3);

            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadbind.Sheets[0].ColumnHeader.Cells[5, 1].ForeColor = Color.FromArgb(64, 64, 255);
            spreadbind.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorRight = Color.White;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 14].Border.BorderColorBottom = Color.Black;
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
            spreadbind.Sheets[0].ColumnHeader.Rows[7].BackColor = Color.FromArgb(214, 235, 255);
            spreadbind.Sheets[0].ColumnHeader.Rows[7].Font.Bold = true;
            spreadbind.Sheets[0].ColumnHeader.Rows[7].Font.Size = FontUnit.Medium;
            spreadbind.Sheets[0].ColumnHeader.Cells[1, 0].CellType = mi;
            spreadbind.Sheets[0].ColumnHeaderSpanModel.Add(1, spreadbind.Sheets[0].ColumnCount - 2, 5, 2);
            spreadbind.Sheets[0].ColumnHeader.Cells[1, spreadbind.Sheets[0].ColumnCount - 2].CellType = mi;

            //======
            FarPoint.Web.Spread.TextCellType celltype = new FarPoint.Web.Spread.TextCellType();
            spreadbind.Sheets[0].Columns[3].CellType = celltype;
            string batchyearquery = "";
            int papertype = 0;
            if (ddlbranch.SelectedItem.Text != "All")
            {
                batchyearquery = "select distinct r.batch_year,r.current_semester,e.exam_code from registration r,exam_details e where e.batch_year=r.batch_year and e.degree_code=r.degree_code and r.current_semester=e.current_semester and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            else if (ddlbranch.SelectedItem.Text == "All")
            {
                batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,course_name+'-'+dept_acronym as branch,e.exam_code from registration r,exam_details e,department dept,degree d,course c where d.course_id=c.course_id and e.batch_year=r.batch_year and d.degree_code=e.degree_code and r.current_semester=e.current_semester and c.course_id=" + ddldegree.SelectedValue.ToString() + " and r.current_semester=e.current_semester and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + "  and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
                //batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,dept_acronym as branch from registration r,exam_details e,department dept,degree d where e.batch_year=r.batch_year and e.degree_code=r.degree_code and r.degree_code=dept.dept_code  and r.college_code=" + Session["collegecode"].ToString() + " and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            if (ddlbranch.SelectedItem.Text == "All" && ddldegree.SelectedItem.Text == "All")
            {
                batchyearquery = "select distinct r.batch_year,r.current_semester,r.degree_code,dept_name,c.course_name+' - '+dept_acronym as branch,exam_code from registration r,exam_details e,department dept,degree d,course c where e.batch_year=r.batch_year and r.current_semester=e.current_semester and e.degree_code=r.degree_code and e.degree_code=d.degree_code and c.course_id=d.course_id and r.degree_code=dept.dept_code  and r.college_code=13 and e.exam_month=" + ddlMonth.SelectedValue.ToString() + " and e.exam_year=" + ddlYear.SelectedValue.ToString() + "  and e.coll_code=" + Session["collegecode"].ToString() + " and cc=0 and d.dept_code=dept.dept_code and delflag=0 and exam_flag <> 'DEBAR' order by r.batch_year desc";
            }
            SqlDataAdapter dabatchyearquery = new SqlDataAdapter(batchyearquery, con1);
            DataSet dsbatchyearquery = new DataSet();
            con1.Close();
            con1.Open();
            dabatchyearquery.Fill(dsbatchyearquery);

            if (dsbatchyearquery.Tables[0].Rows.Count > 0)
            {

                string batchyear = "";
                string current_sem = "";
                string degreecode = "";
                string dept_name = "";
                string examCode = "";
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
                        subjectquery = "select distinct subject_name ,subject_code,subject.subject_no from subject,subjectchooser,registration where subjectchooser.subject_no=subject.subject_no and subjectchooser.semester=" + current_sem + " and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + degreecode + " and registration.batch_year=" + batchyear + "";
                        //subjectquery = "select distinct subject_name ,subject_code,subject.subject_no from exam_appl_details,subject,exam_details,exam_application where exam_appl_details.attempts = 0 and subject.subject_no = exam_appl_details.subject_no and ltrim(rtrim(exam_appl_details.type))='' and exam_appl_details.appl_no=exam_application.appl_no and exam_application.exam_code=exam_details.exam_code and degree_code=" + degreecode + " and current_semester=" + current_sem + " and exam_details.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_details.exam_year=" + ddlYear.SelectedValue.ToString() + " order by subject_code";
                    }
                    if (RadioArrear.Checked == true)
                    {
                        papertype = 0;
                        subjectquery = "select distinct subject_name ,subject_code,subject.subject_no from exam_appl_details,subject,exam_details,exam_application where exam_appl_details.attempts > 0 and subject.subject_no = exam_appl_details.subject_no and ltrim(rtrim(exam_appl_details.type))='*' and exam_appl_details.appl_no=exam_application.appl_no and exam_application.exam_code=exam_details.exam_code and degree_code=" + degreecode + " and current_semester<" + current_sem + " and exam_details.exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_details.exam_year=" + ddlYear.SelectedValue.ToString() + " order by subject_code";
                    }
                    //string subjectquery = "select distinct batch_year,exam_appl_details.subject_no,subject_name ,subject_code,exam_details.exam_code,current_semester from exam_appl_details,subject,exam_details,exam_application where exam_appl_details.attempts = 0 and subject.subject_no = exam_appl_details.subject_no and ltrim(rtrim(exam_appl_details.type))='' and exam_appl_details.appl_no=exam_application.appl_no and exam_application.exam_code=exam_details.exam_code and exam_details.batch_year=" + batchyear + " and degree_code=" + degreecode + " and current_semester<=" + current_sem + " and exam_details.exam_month="+ddlMonth.SelectedValue.ToString()+" and exam_details.exam_year="+ddlYear.SelectedValue.ToString()+" order by batch_year,current_semester desc";
                    SqlDataAdapter dasubjectquery = new SqlDataAdapter(subjectquery, con1);
                    DataSet dssubjectquery = new DataSet();
                    con1.Close();
                    con1.Open();
                    dasubjectquery.Fill(dssubjectquery);

                    if (dssubjectquery.Tables[0].Rows.Count > 0)
                    {
                        string subject_no = "";
                        string subject_name = "";
                        string subject_code = "";
                        string exam_code = "";
                        string studsemester = "";

                        for (int subjecti = 0; subjecti < dssubjectquery.Tables[0].Rows.Count; subjecti++)
                        {
                            spreadbind.Visible = true;
                            int subjectRowflag = 0;
                            subject_no = dssubjectquery.Tables[0].Rows[subjecti]["subject_no"].ToString();
                            subject_name = dssubjectquery.Tables[0].Rows[subjecti]["subject_name"].ToString();
                            subject_code = dssubjectquery.Tables[0].Rows[subjecti]["subject_code"].ToString();
                            //exam_code = dssubjectquery.Tables[0].Rows[subjecti]["exam_code"].ToString();
                            //studsemester = dssubjectquery.Tables[0].Rows[subjecti]["current_semester"].ToString();
                            studsemester = current_sem;
                            if (commonsub.Contains(subject_code + "," + current_sem))
                            {
                                int Flag = 1;
                                string commondept_name = Convert.ToString(GetCorrespondingKey(subject_code + "," + current_sem, commonsub));
                                string[] commondept_namesplit = commondept_name.Split(new Char[] { ',' });
                                if (Convert.ToInt32(commondept_namesplit.GetUpperBound(0)) > 0)
                                {
                                    int uppervalue = Convert.ToInt32(commondept_namesplit.GetUpperBound(0));
                                    if (uppervalue >= 1)
                                    {
                                        for (int upperloop = 0; upperloop <= uppervalue; upperloop++)
                                        {
                                            if (dept_name == commondept_namesplit[upperloop].ToString())
                                            {
                                                Flag = 0;
                                            }

                                        }
                                    }
                                    if (Flag == 1)
                                    {
                                        subjectRowflag = 1;
                                        commondept_name = commondept_name + "," + dept_name;
                                        commonsub[subject_code + "," + current_sem] = commondept_name + "," + current_sem;
                                    }


                                }
                                else
                                {
                                    if (commondept_name != dept_name)
                                    {
                                        subjectRowflag = 1;
                                        commondept_name = commondept_name + "," + dept_name;
                                        commonsub[subject_code + "," + current_sem] = commondept_name;
                                    }
                                    //else if (commondept_name == dept_name)
                                    //{
                                    //    subjectRowflag = 1;

                                    //}
                                }



                            }
                            else
                            {
                                commonsub.Add(subject_code + "," + current_sem, dept_name);

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
                                string studentappeared = "";
                                string studentpassed = "";
                                string studentfail = "";
                                string totalstudents = "";
                                string registeredstud = "";
                                string absentstud = "";
                                for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                                {

                                    totalstudents = studinfoads.Tables[0].Rows[studproci][0].ToString();
                                    studentappeared = studinfoads.Tables[1].Rows[studproci][0].ToString();
                                    studentpassed = studinfoads.Tables[2].Rows[studproci][0].ToString();
                                    studentfail = studinfoads.Tables[3].Rows[studproci][0].ToString();
                                    registeredstud = studinfoads.Tables[4].Rows[studproci][0].ToString();
                                    absentstud = studinfoads.Tables[5].Rows[studproci][0].ToString();
                                    spreadbind.Sheets[0].RowCount = spreadbind.Sheets[0].RowCount + 1;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].Text = ddldegree.SelectedItem.Text;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].Text = current_sem;
                                    //spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = current_sem;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Text = subject_code;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 3].Note = subject_no;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 4].Text = subject_name;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].Text = registeredstud;

                                    spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 8, 1, 2);//**
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].Text = studentappeared;

                                    spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 10, 1, 2);//**
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 10].Text = studentpassed;

                                    spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 11, 1, 2);//** 
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 11].Text = studentpassed;

                                    spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 12, 1, 2);//**
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 12].Text = studentfail;
                                    spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 14, 1, 2);//**
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 14].Text = absentstud;
                                    int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                                    string passpercent = "0";
                                    string failpercent = "0";
                                    int absent = 0;
                                    int absentpercent = 0;
                                    int studapprdpercent = 0;
                                    string type = "";
                                    if (RadioRegular.Checked == true)
                                    {
                                        type = "Regular";
                                    }
                                    else if (RadioArrear.Checked == true)
                                    {
                                        type = "Arrear";
                                    }
                                    if (studentappeared != "0")
                                    {
                                        double studapprdpercent1 = 0;
                                        studapprdpercent1 = (Convert.ToDouble(studentappeared) / Convert.ToDouble(totalstudents)) * 100;
                                        double studapprdpercent2 = Math.Round(studapprdpercent1, 2);
                                        studapprdpercent = Convert.ToInt32(studapprdpercent2);
                                    }
                                    if (totalstudents != "0")
                                    {
                                        double absentpercent1 = 0;
                                        absent = Convert.ToInt32(totalstudents) - Convert.ToInt32(studentappeared);
                                        absentpercent1 = (absent / Convert.ToDouble(totalstudents)) * 100;
                                        double absentpercent2 = Math.Round(absentpercent1, 2);
                                        absentpercent = Convert.ToInt32(absentpercent2);
                                    }
                                    if (studentpassed != "0")
                                    {
                                        double passpercent1 = 0;
                                        passpercent1 = Convert.ToDouble((Convert.ToDouble(studentpassed) / total) * 100);
                                        double passpercent2 = Math.Round(passpercent1, 2);
                                        passpercent = Convert.ToString(passpercent2);
                                    }
                                    if (studentfail != "0")
                                    {
                                        double failpercent1 = 0;
                                        failpercent1 = Convert.ToDouble((Convert.ToDouble(studentfail) / total) * 100);
                                        double failpercent2 = Math.Round(failpercent1, 2);
                                        failpercent = Convert.ToString(failpercent2);
                                    }
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 6].Text = type;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(studapprdpercent);
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 11].Text = passpercent;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 13].Text = failpercent;


                                    //spreadbind.Sheets[0].SpanModel.Add(spreadbind.Sheets[0].RowCount - 1, 14, 1, 2);//**
                                    //spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(absent);
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(absentpercent);
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                                    spreadbind.Sheets[0].Cells[spreadbind.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                                    if (subjectRowflag == 1)
                                    {
                                        addsamerow.Add(subject_code + "," + totalstudents + "," + studentappeared + "," + studentpassed + "," + studentfail + "," + absent, studapprdpercent + "," + passpercent + "," + failpercent + "," + absentpercent);
                                        spreadbind.Sheets[0].Rows[spreadbind.Sheets[0].RowCount - 1].Visible = false;
                                    }

                                }
                            }

                        }
                    }
                }

                foreach (DictionaryEntry parameter in commonsub)
                {
                    for (int comsubi = 0; comsubi < spreadbind.Sheets[0].RowCount; comsubi++)
                    {
                        string subject_codefrmcommonsub = Convert.ToString(parameter.Key);
                        string[] splittosemester = subject_codefrmcommonsub.Split(new Char[] { ',' });
                        string subcodefrmcommonsub = splittosemester[0].ToString();
                        string semfrmcommonsub = splittosemester[1].ToString();
                        string deptfrmcommonsub = Convert.ToString(parameter.Value);
                        string subject_codefrmsprd = spreadbind.Sheets[0].Cells[comsubi, 3].Text;
                        string semfrmsprd = spreadbind.Sheets[0].Cells[comsubi, 2].Text;
                        if (subject_codefrmsprd == subcodefrmcommonsub && semfrmsprd == semfrmcommonsub)
                        {
                            spreadbind.Sheets[0].Cells[comsubi, 5].Text = deptfrmcommonsub;
                        }

                    }


                }
                foreach (DictionaryEntry parameter in addsamerow)
                {
                    for (int comsubi = 0; comsubi < spreadbind.Sheets[0].RowCount; comsubi++)
                    {
                        string subject_codeandtotals = Convert.ToString(parameter.Key);
                        string otherpercents = Convert.ToString(parameter.Value);
                        string subject_codefrmsprd = spreadbind.Sheets[0].Cells[comsubi, 3].Text;
                        string[] splittotmarks = subject_codeandtotals.Split(new Char[] { ',' });
                        string[] splittotper = otherpercents.Split(new Char[] { ',' });
                        string addsamesubcode = splittotmarks[0].ToString();
                        double srtotmar = Convert.ToDouble(splittotmarks[1]);
                        double srstudapp = Convert.ToDouble(splittotmarks[2]);
                        double srstupass = Convert.ToDouble(splittotmarks[3]);
                        double srstufail = Convert.ToDouble(splittotmarks[4]);
                        double srstuabs = Convert.ToDouble(splittotmarks[5]);
                        double srapprpcnt = Convert.ToDouble(splittotper[0]);
                        double srPasspcnt = Convert.ToDouble(splittotper[1]);
                        double srfailprcnt = Convert.ToDouble(splittotper[2]);
                        double srabsentprct = Convert.ToDouble(splittotper[3]);
                        if (subject_codefrmsprd == addsamesubcode)
                        {
                            double oldtotmar = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 7].Text);
                            double oldstudapp = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 8].Text);
                            double oldstudpass = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 10].Text);
                            double oldstudfail = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 12].Text);
                            double oldstudabs = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 14].Text);
                            double oldapprpcnt = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 9].Text);
                            double oldpasspcnt = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 11].Text);
                            double oldfailpcnt = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 13].Text);
                            double oldabsenprct = Convert.ToDouble(spreadbind.Sheets[0].Cells[comsubi, 15].Text);

                            spreadbind.Sheets[0].Cells[comsubi, 7].Text = Convert.ToString((oldtotmar + srtotmar));
                            spreadbind.Sheets[0].Cells[comsubi, 8].Text = Convert.ToString(oldstudapp + srstudapp);

                            double totstudapprd = oldstudapp + srstudapp;
                            double totstud = oldtotmar + srtotmar;
                            if (totstudapprd != 0 && totstud != 0)
                            {
                                double studapprdpercent1 = 0;
                                studapprdpercent1 = Convert.ToDouble((Convert.ToDouble(totstudapprd) / Convert.ToDouble(totstud)) * 100);
                                double studapprdpercent2 = Math.Round(studapprdpercent1, 2);
                                spreadbind.Sheets[0].Cells[comsubi, 9].Text = Convert.ToString(studapprdpercent2);
                            }
                            spreadbind.Sheets[0].Cells[comsubi, 10].Text = Convert.ToString(oldstudpass + srstupass);
                            spreadbind.Sheets[0].Cells[comsubi, 12].Text = Convert.ToString(oldstudfail + srstufail);
                            double studpassed = oldstudpass + srstupass;
                            double studfailed = oldstudfail + srstufail;
                            double totfailpass = studpassed + studfailed;
                            if (totfailpass != 0 && studpassed != 0)
                            {
                                double studpassedpercent1 = 0;
                                studpassedpercent1 = Convert.ToDouble((Convert.ToDouble(studpassed) / Convert.ToDouble(totfailpass)) * 100);
                                double studpassedpercent2 = Math.Round(studpassedpercent1, 2);
                                spreadbind.Sheets[0].Cells[comsubi, 11].Text = Convert.ToString(studpassedpercent2);

                            }
                            if (totfailpass != 0 && studfailed != 0)
                            {
                                double studfailedpercent1 = 0;
                                studfailedpercent1 = Convert.ToDouble((Convert.ToDouble(studfailed) / Convert.ToDouble(totfailpass)) * 100);
                                double studfailedpercent2 = Math.Round(studfailedpercent1, 2);
                                spreadbind.Sheets[0].Cells[comsubi, 13].Text = Convert.ToString(studfailedpercent2);
                            }
                            spreadbind.Sheets[0].Cells[comsubi, 14].Text = Convert.ToString(oldstudabs + srstuabs);
                            double totabs = oldstudabs + srstuabs;
                            if (totabs != 0 && totstud != 0)
                            {
                                double totabspercent1 = 0;
                                totabspercent1 = Convert.ToDouble((Convert.ToDouble(totabs) / Convert.ToDouble(totstud)) * 100);
                                double totabspercent2 = Math.Round(totabspercent1, 2);
                                spreadbind.Sheets[0].Cells[comsubi, 15].Text = Convert.ToString(totabspercent2);
                            }

                        }

                    }
                }

            }
            int totalrows = spreadbind.Sheets[0].RowCount;
            spreadbind.Sheets[0].PageSize = (totalrows * 25) + 40;
            spreadbind.Height = (totalrows * 25) + 40;
            spreadbind.Width = 1210;
        }
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        spreadbind.Visible = false;
        string exammonth = ddlMonth.SelectedValue.ToString();
        string examyear = ddlYear.SelectedValue.ToString();
        ddldegree.Items.Clear();
        ddlbranch.Items.Clear();

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
                ddldegree.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
                for (int i = 0; i < dsdegreecodequery.Tables[0].Rows.Count; i++)
                {
                    int i1 = 1;
                    ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + dsdegreecodequery.Tables[0].Rows[i]["course_name"].ToString() + "", "" + dsdegreecodequery.Tables[0].Rows[i]["course_id"].ToString() + ""));
                    i1++;
                }
                ////ddldegree.DataSource = dsdegreecodequery;
                ////ddldegree.DataValueField = "course_id";
                ////ddldegree.DataTextField = "course_name";
                ////ddldegree.DataBind();

            }
            else
            {
                ddldegree.Items.Clear();
            }
            if (ddldegree.SelectedValue != "")
            {
                if (ddldegree.SelectedValue != "0")
                {
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + "";
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
            string degreecodequery = "select distinct c.course_name,c.course_id from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and  ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and c.college_code='" + collCode + "'";
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
                    string branchquery = "select distinct d.degree_code,dept_acronym from exam_details ed,degree d,course c,department dp where ed.degree_code=d.degree_code and d.course_id=c.course_id and dp.dept_code=d.dept_code and c.course_id=" + ddldegree.SelectedValue.ToString() + " and ed.exam_month=" + exammonth + " and ed.exam_year=" + examyear + " and c.college_code='" + collCode + "'";
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
    }
}