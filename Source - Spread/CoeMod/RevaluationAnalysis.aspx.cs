using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class RevaluationAnalysis : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string singleuser = "";
    string group_user = "";
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!Page.IsPostBack)
            {
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsection();
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
        catch (Exception ex)
        {
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
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        bindsection();
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
        bindsection();
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {

        }

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
        string collegecode = ddlcollege.SelectedItem.Value.ToString();
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
        string collegecode = ddlcollege.SelectedItem.Value.ToString();
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
    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            ds = d2.BindSem(ddlbranch.SelectedItem.Value, ddlbatch.SelectedItem.Text, Convert.ToString(ddlcollege.SelectedItem.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                string count = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (count != "" && count != "0")
                {
                    for (int co = 1; co <= Convert.ToInt32(count); co++)
                    {
                        ddlsem.Items.Add(Convert.ToString(co));
                    }
                }
            }

        }
        catch
        {
        }
    }

    public void bindsection()
    {
        try
        {
            ddlsec.Items.Clear();
            ds.Clear();
            string strquery = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Enabled = true;
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch
        {

        }
    }


    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;

        FpSpread1.Visible = false;
        rptprint.Visible = false;
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsection();
        }
        catch
        {

        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindsem();
            bindsection();
            FpSpread1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Office of the Controller of Examinations  $Result Analysis Statement (After Revalutaion)" + '@' + "Course & Branch:  " + ddldegree.SelectedItem.Text + "    " + ddlbranch.SelectedItem.Text + " " + '@' + "Month & Year of Exam :  " + ddlMonth.SelectedItem.Text + "     " + ddlYear.SelectedItem.Text + " " + '@' + "Semester:  " + ddlsem.SelectedItem.Text + "";
            string pagename = "RevaluationAnalysis.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void ddlbranch_Change(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsection();
            FpSpread1.Visible = false;
            rptprint.Visible = false;
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
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string month = Convert.ToString(ddlMonth.SelectedItem.Value);
            string year = Convert.ToString(ddlYear.SelectedItem.Text);
            string section = "";
            if (ddlsec.Enabled == true)
            {
                section = "and r.Sections ='" + ddlsec.SelectedItem.Text + "'";
            }
            DataView dv = new DataView();
            if (year.Trim() != "" && month.Trim() != "0")
            {
                //string selectquery = "select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no  from Registration r,mark_entry m,Exam_Details e,subject s  ,subjectchooser sc  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and s.subType_no =sc.subtype_no and s.subject_no =sc.subject_no  and sc.roll_no =r.Roll_No  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and  m.attempts =1  and m.result not like 'A%' and semester =" + ddlsem.SelectedItem.Text + "  " + section + " group by s.subject_code,s.subject_name ,s.subject_no";
                //selectquery = selectquery + "  select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no  from Registration r,mark_entry m,Exam_Details e,subject s  ,subjectchooser sc  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and s.subType_no =sc.subtype_no and s.subject_no =sc.subject_no  and sc.roll_no =r.Roll_No  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and  m.attempts =1  and m.result not like 'A%' and semester =" + ddlsem.SelectedItem.Text + "  " + section + " and result ='Pass' and passorfail ='1' group by s.subject_code,s.subject_name ,s.subject_no";
                //selectquery = selectquery + "  select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no  from Registration r,mark_entry m,Exam_Details e,subject s  ,subjectchooser sc  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and s.subType_no =sc.subtype_no and s.subject_no =sc.subject_no  and sc.roll_no =r.Roll_No  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and  m.attempts =1  and m.result not like 'A%' and semester =" + ddlsem.SelectedItem.Text + "  " + section + " and result ='Fail' and passorfail ='0' group by s.subject_code,s.subject_name ,s.subject_no";
                //selectquery = selectquery + "  select distinct  m.roll_no from Registration r,mark_entry m,Exam_Details e,subject s  ,subjectchooser sc  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and s.subType_no =sc.subtype_no and s.subject_no =sc.subject_no  and sc.roll_no =r.Roll_No  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and  m.attempts =1  and m.result not like 'A%' and semester =" + ddlsem.SelectedItem.Text + "  " + section + "";
                //selectquery = selectquery + "  select distinct  m.roll_no from Registration r,mark_entry m,Exam_Details e,subject s  ,subjectchooser sc  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and s.subType_no =sc.subtype_no and s.subject_no =sc.subject_no  and sc.roll_no =r.Roll_No  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and  m.attempts =1  and m.result not like 'A%' and semester =" + ddlsem.SelectedItem.Text + "  " + section + " and result ='Pass' and passorfail ='1'";
                string getexamcode = d2.GetFunction("select exam_code  from Exam_Details e where  e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Value + " and e.batch_year=" + ddlbatch.SelectedItem.Text + " and e.degree_code=" + ddlbranch.SelectedItem.Value + "  and current_semester =" + ddlsem.SelectedItem.Text + "");
                if (getexamcode.Trim() != "")
                {
                    string selectquery = "select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no from Registration r,mark_entry m,Exam_Details e,subject s  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "    and m.attempts <=1  and m.result not like 'A%' and e.current_semester =" + ddlsem.SelectedItem.Text + "  " + section + " group by s.subject_code,s.subject_name ,s.subject_no";
                    selectquery = selectquery + "  select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no from Registration r,mark_entry m,Exam_Details e,subject s where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and m.attempts <=1  and m.result not like 'A%' and e.current_semester =" + ddlsem.SelectedItem.Text + "  " + section + " and result ='Pass' and passorfail ='1' group by s.subject_code,s.subject_name ,s.subject_no ";
                    selectquery = selectquery + "  select count(m.roll_no)as total, s.subject_code,s.subject_name,s.subject_no from Registration r,mark_entry m,Exam_Details e,subject s where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no and e.batch_year=r.Batch_Year  and e.degree_code=r.degree_code and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and m.attempts <=1  and m.result not like 'A%' and e.current_semester =" + ddlsem.SelectedItem.Text + "  " + section + " and result ='Fail' and passorfail ='0' group by s.subject_code,s.subject_name ,s.subject_no";
                    selectquery = selectquery + "  select distinct m.roll_no from Registration r,mark_entry m,Exam_Details e,subject s where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and m.subject_no=s.subject_no  and e.batch_year=r.Batch_Year and e.batch_year=" + ddlbatch.SelectedItem.Text + " and r.degree_code=" + ddlbranch.SelectedItem.Value + "  and e.Exam_Month=" + ddlMonth.SelectedItem.Value + " and e.Exam_year=" + ddlYear.SelectedItem.Text + "  and m.attempts <=1  and m.result not like 'A%' and e.current_semester =" + ddlsem.SelectedItem.Text + "  " + section + "";
                    selectquery = selectquery + "  select distinct  r.Roll_No  from Registration r,mark_entry m where r.Roll_No =m.roll_no and m.exam_code ='" + getexamcode + "' and  result ='Fail' and passorfail ='0'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 7;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "SUBJECT CODE";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "SUBJECT NAME";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "APPEREAD";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "PASSED";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "FAILED";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "PASS %";
                        FpSpread1.Columns[0].Width = 50;
                        FpSpread1.Columns[1].Width = 100;
                        FpSpread1.Columns[2].Width = 200;
                        FpSpread1.Columns[3].Width = 100;
                        FpSpread1.Columns[4].Width = 100;
                        FpSpread1.Columns[5].Width = 100;
                        FpSpread1.Columns[6].Width = 100;
                        double percentage = 0; double Total = 0; double pass = 0;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["subject_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["subject_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["total"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Total = 0;
                            Total = Convert.ToDouble(ds.Tables[0].Rows[row]["total"]);
                            ds.Tables[1].DefaultView.RowFilter = "subject_no='" + Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]) + "'";
                            dv = ds.Tables[1].DefaultView;
                            pass = 0;
                            if (dv.Count > 0)
                            {
                                pass = Convert.ToDouble(dv[0]["total"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[0]["total"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString("0");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            }
                            ds.Tables[2].DefaultView.RowFilter = "subject_no='" + Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]) + "'";
                            dv = ds.Tables[2].DefaultView;

                            if (dv.Count > 0)
                            {

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[0]["total"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("0");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            percentage = (pass / Total) * 100;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Math.Round(percentage, 2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        Total = Convert.ToDouble(ds.Tables[3].Rows.Count);
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("No.of Students Registered in all the Subjects:           " + Convert.ToString(Total) + "");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                        pass = Convert.ToDouble(ds.Tables[4].Rows.Count);
                        pass = Total - pass;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("No.of Students Passed in all the Subjects:             " + Convert.ToString(pass) + "");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                        percentage = (pass / Total) * 100;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 4, 2, 3);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].Text = Convert.ToString("Over all Percentage:             " + Convert.ToString(Math.Round(percentage, 2)) + "%");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 4].VerticalAlign = VerticalAlign.Middle;

                        FpSpread1.Visible = true;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        rptprint.Visible = true;
                        errorlabl.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        errorlabl.Visible = true;
                        errorlabl.Text = "No Records Found";
                    }
                }
                else
                {
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    errorlabl.Visible = true;
                    errorlabl.Text = "No Records Found";
                }
            }
        }
        catch
        {

        }
    }
}