using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class OverallResultwithinternalexternal : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string singleuser = "";
    string group_user = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("ChartMaster"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/ChartMOD/ChartMaster.aspx");
                return;
            }
        }
        //****************************************************//
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!Page.IsPostBack)
        {
            bindbatch();
            binddegree();
            bindbranch();
            FpSpread1.Visible = false;


            //  FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 8;
            FpSpread1.Sheets[0].ColumnCount = 12;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.CommandBar.Visible = true;


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
            for (int l = 0; l <= 10; l++)
            {
                ddlYear.Items.Add(Convert.ToString(year - l));
            }
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        errorlabl.Visible = false;
        lblvalidation1.Visible = false;
    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        Chart1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        Chart1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        string selectquery = " select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selectquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataBind();
        }

    }
    public void binddegree()
    {
        ////degree
        ddldegree.Items.Clear();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        ds.Clear();
        ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "course_id";
            ddldegree.DataTextField = "course_name";
            ddldegree.DataBind();
        }
        //bindbranch();

    }
    public void bindbranch()
    {

        ddlbranch.Items.Clear();

        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddldegree.SelectedValue.ToString();
        ds.Clear();
        ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    protected void ddlbrach_Change(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            Chart1.Visible = false;
            FpSpread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        Chart1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        Chart1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string month = Convert.ToString(ddlMonth.SelectedItem.Value);
            string year = Convert.ToString(ddlYear.SelectedItem.Text);
            if (year.Trim() != "" && month.Trim() != "0")
            {
                string seleqtquery = " select isnull(sum(m.internal_mark),'0') as Internal,isnull(sum(m.external_mark),'0') as External1,count(m.roll_no)as total,s.subject_code,s.subject_name,max_ext_marks ,max_int_marks from Registration r,mark_entry m,Exam_Details e,subject s where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + " and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + " and m.attempts <=1 and m.result not like 'A%' group by s.subject_code,s.subject_name,max_ext_marks ,max_int_marks ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(seleqtquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 4;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Subject Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Internal";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "External";
                    FpSpread1.Columns[0].Width = 50;
                    FpSpread1.Columns[1].Width = 200;
                    FpSpread1.Columns[2].Width = 100;
                    FpSpread1.Columns[3].Width = 100;

                    Chart1.Series.Clear();

                    Chart1.Series.Add("Internal");
                    Chart1.Series[0].BorderWidth = 2;
                    Chart1.Series.Add("External");
                    Chart1.Series[1].BorderWidth = 2;

                    Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                    Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                    DataTable newdata = new DataTable();
                    DataRow dr;
                    DataRow dr1;

                    dr = newdata.NewRow();
                    dr1 = newdata.NewRow();

                    for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                    {

                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = (Convert.ToString(a + 1));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = (Convert.ToString(ds.Tables[0].Rows[a]["subject_name"]));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        double max = (Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["total"]) * Convert.ToDouble(ds.Tables[0].Rows[a]["max_int_marks"])));
                        double inter = Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["Internal"]) / Convert.ToDouble(max) * Convert.ToDouble(100));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = (Convert.ToString(Math.Round(inter, 2)));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        max = (Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["total"]) * Convert.ToDouble(ds.Tables[0].Rows[a]["max_ext_marks"])));
                        inter = Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["External1"]) / Convert.ToDouble(max) * Convert.ToDouble(100));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = (Convert.ToString(Math.Round(inter, 2)));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        newdata.Columns.Add(Convert.ToString(ds.Tables[0].Rows[a]["subject_code"]));
                        if (newdata.Columns.Count > 0)
                        {
                            for (int i = newdata.Columns.Count - 1; i < newdata.Columns.Count; i++)
                            {
                                max = (Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["total"]) * Convert.ToDouble(ds.Tables[0].Rows[a]["max_int_marks"])));
                                inter = Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["Internal"]) / Convert.ToDouble(max) * Convert.ToDouble(100));
                                dr[i] = Convert.ToString(Math.Round(inter, 2));
                                max = (Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["total"]) * Convert.ToDouble(ds.Tables[0].Rows[a]["max_ext_marks"])));
                                inter = Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[a]["External1"]) / Convert.ToDouble(max) * Convert.ToDouble(100));
                                dr1[i] = Convert.ToString(Math.Round(inter, 2));

                            }
                        }
                        //Chart1.Series[0].Points.AddXY(Convert.ToString(ds.Tables[0].Rows[a]["subject_code"]), Convert.ToString(ds.Tables[0].Rows[a]["Internal"]));
                        //Chart1.Series[0].IsValueShownAsLabel = true;

                        //Chart1.Series[1].Points.AddXY(Convert.ToString(ds.Tables[0].Rows[a]["subject_code"]), Convert.ToString(ds.Tables[0].Rows[a]["External"]));
                        //Chart1.Series[1].IsValueShownAsLabel = true;
                    }

                    newdata.Rows.Add(dr);
                    newdata.Rows.Add(dr1);
                    FpSpread1.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    rptprint.Visible = true;
                    if (newdata.Rows.Count > 0)
                    {
                        for (int chart_i = 0; chart_i < newdata.Columns.Count; chart_i++)
                        {
                            for (int chart_j = 0; chart_j < newdata.Rows.Count; chart_j++)
                            {
                                string subnncode = Convert.ToString(newdata.Columns[chart_i]);
                                string m1 = newdata.Rows[chart_j][chart_i].ToString();
                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                                Chart1.Series[chart_j].IsXValueIndexed = true;
                                //Chart1.Series[chart_j].XValueMember = Convert.ToString(subnncode);
                                //Chart1.Series[chart_j].YValueMembers = Convert.ToString(m1);
                            }
                        }
                    }

                    Chart1.Visible = true;
                }
                else
                {
                    errorlabl.Visible = true;
                    Chart1.Visible = false;
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    errorlabl.Text = "No Records Found";
                }

            }
            else
            {
                errorlabl.Visible = true;
                Chart1.Visible = false;
                FpSpread1.Visible = false;
                rptprint.Visible = false;
                errorlabl.Text = "Please Select All Fields";
            }
        }
        catch
        {

        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = string.Empty;
            string pagename = "OverallResultwithinternalexternal.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }

    }
}