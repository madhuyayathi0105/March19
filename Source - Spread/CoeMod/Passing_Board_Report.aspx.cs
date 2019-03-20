using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;
//using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
//using System.Web.UI.DataVisualization.Charting;

public partial class Passing_Board_Report : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hast = new Hashtable();
    DataTable dt = new DataTable();
    string grouporusercode = "";

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
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            lblmsg.Visible = false;
            if (!IsPostBack)
            {
                bindcollege();
                bindbatch();
                binddegree();
                binddept();
                bindsem();
                bindSubject();
                bindMonthandYear();
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                Printcontrol.Visible = false;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dssett = da.select_method_wo_parameter(Master1, "Text");
                for (int i = 0; i < dssett.Tables[0].Rows.Count; i++)
                {
                    if (dssett.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dssett.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dssett.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dssett.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void log_OnClick(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void bindcollege()
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
            hast.Clear();
            hast.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hast, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindbatch()
    {
        try
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

            }

            ddlbatch.Text = "batch(" + 1 + ")";




        }
        catch (Exception ex)
        {

        }
    }


    protected void binddegree()
    {
        try
        {
            ds.Clear();
            ds = da.BindDegree(Session["single_user"].ToString(), Session["group_code"].ToString(), Session["collegecode"].ToString(), Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }

        }

        catch (Exception ex)
        {

        }
    }

    protected void binddept()
    {
        try
        {
            hast.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            //collegecode = ddldegree.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hast.Add("single_user", singleuser);
            hast.Add("group_code", group_user);
            hast.Add("course_id", ddldegree.SelectedValue);
            hast.Add("college_code", collegecode);
            hast.Add("user_code", usercode);
            ds = da.select_method("bind_branch", hast, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddldept.DataSource = ds;
                ddldept.DataTextField = "dept_name";
                ddldept.DataValueField = "degree_code";
                ddldept.DataBind();
            }
        }
        catch (Exception ex)
        {

        }
    }


    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            ds.Clear();
            if (ddldept.SelectedValue != "")
            {
                ds = da.BindSem(ddldept.SelectedValue, ddlbatch.SelectedValue, ddlcollege.SelectedValue);

                int count5 = ds.Tables[0].Rows.Count;
                if (count5 > 0)
                {
                    count5 = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    for (int i = 1; i <= count5; i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
            else
            {
                ddlsem.Enabled = false;
            }

        }
        catch (Exception ex)
        {

        }
    }


    public void bindSubject()
    {
        ddlsubject.Items.Clear();
        string sql = "";
        //if (ddlmonth.SelectedValue != "" && ddlyear.SelectedValue != "")
        //{
        //    sql = "SELECT Subject_No,Subject_Code,Subject_Name FROM Subject S,Syllabus_Master Y,Exam_Details D where s.syll_code = y.syll_code and y.degree_code = d.degree_code and y.Batch_Year = d.batch_year and y.semester = d.current_semester and d.degree_code ='" + ddldept.SelectedValue + "' and d.batch_year = '" + ddlbatch.SelectedValue + "' and d.current_semester = '" + ddlsem.SelectedValue + "' and d.exam_code = (select exam_code from Exam_Details where degree_code = '" + ddldept.SelectedValue + "' and batch_year ='" + ddlbatch.SelectedValue + "'  and current_semester = '" + ddlsem.SelectedValue + "')";
        //}
        //    sql = "select distinct s.subject_name,s.subject_code,s.subject_no from exmtt_det et,exmtt e,subject s where  s.subject_no=et.subject_no and e.exam_code=et.exam_code and    e.exam_Month='" + ddlmonth.SelectedValue + "' and e.Exam_Year='" + ddlyear.SelectedValue + "' order by s.subject_name,s.subject_code";
        //}
        //else
        //{
        //    sql = "select distinct s.subject_name,s.subject_code,s.subject_no from exmtt_det et,exmtt e,subject s where  s.subject_no=et.subject_no and e.exam_code=et.exam_code  order by s.subject_name,s.subject_code";
        //}
        //sql = "select distinct s.subject_name,s.subject_code,s.subject_no from Exam_Details e,mark_entry m,subject s where  s.subject_no=m.subject_no and e.exam_code=m.exam_code and  e.exam_Month='" + ddlmonth.SelectedValue + "' and e.Exam_Year='" + ddlyear.SelectedValue + "' and e.batch_year='" + ddlbatch.SelectedValue + "' and e.degree_code='" + ddldept.SelectedValue + "' and e.current_semester='" + ddlsem.SelectedValue + "' order by s.subject_name,s.subject_code";

        //ds = da.select_method_wo_parameter("select distinct s.subject_name from exmtt_det et,exmtt e,subject s where  s.subject_no=et.subject_no and e.exam_code=et.exam_code and    e.exam_Month='" + ddlexm.SelectedValue + "' and e.Exam_Year='" + ddlyear.SelectedValue + "' order by s.subject_name", "Text");
        if (ddlmonth.Items.Count > 0 && ddlyear.Items.Count > 0)
        {
            sql = "SELECT Subject_No,Subject_Code,Subject_Name FROM Subject S,Syllabus_Master Y,Exam_Details D where s.syll_code = y.syll_code and y.degree_code = d.degree_code and y.Batch_Year = d.batch_year and y.semester = d.current_semester and d.degree_code ='" + ddldept.SelectedValue + "' and d.batch_year = '" + ddlbatch.SelectedValue + "' and d.current_semester = '" + ddlsem.SelectedValue + "' and d.exam_code = (select exam_code from Exam_Details where degree_code = '" + ddldept.SelectedValue + "' and batch_year ='" + ddlbatch.SelectedValue + "'  and current_semester = '" + ddlsem.SelectedValue + "')";
            sql = "SELECT distinct ed.degree_code,ed.batch_year,ed.current_semester,ead.subject_no,s.subject_name  FROM Exam_Details ED,exam_application EA,exam_appl_details EAD,subject s  where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and s.subject_no=ead.subject_no and ed.degree_code='" + ddldept.SelectedValue + "'  and ed.batch_year = '" + ddlbatch.SelectedValue + "' and ed.current_semester='" + ddlsem.SelectedValue + "' and  ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.Text.ToString() + "' order by ed.batch_year,ed.degree_code,ed.current_semester,ead.subject_no ";
            ds = da.select_method_wo_parameter(sql, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
            }
        }
    }


    public void bindMonthandYear()
    {
        try
        {
            // ddlexm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));


            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            ddlyear.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {

                ddlyear.Items.Add(Convert.ToString(year - l));

            }
            // ddlyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {

        }
    }

    protected void lblbatch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;

    }


    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlbatch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddept();
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlsem_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }
    protected void ddlsubject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }

    protected void ddlyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }
    protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        bindSubject();
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;
    }



    protected void lblrepttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        Printcontrol.Visible = false;

    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].SheetName = " ";
            FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].AllowTableCorner = true;

            FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            FpSpread1.Pager.Align = HorizontalAlign.Right;
            FpSpread1.Pager.Font.Bold = true;
            FpSpread1.Pager.Font.Name = "Book Antiqua";
            FpSpread1.Pager.ForeColor = Color.DarkGreen;
            FpSpread1.Pager.BackColor = Color.Beige;
            FpSpread1.Pager.BackColor = Color.AliceBlue;
            FpSpread1.Pager.PageCount = 5;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            Printcontrol.Visible = false;
            txtexcelname.Text = "";

            if (ddlsubject.Items.Count == 0)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Select The Subject And Then Proceed";
                return;
            }

            string SQL = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Current_Semester,m.external_mark,m.exam_code,ed.Exam_Month,ed.Exam_year, m.subject_no,m.internal_mark,m.evaluation1,m.evaluation2,m.evaluation3,result from Registration r,Exam_Details ed,mark_entry m where r.degree_code=ed.degree_code and r.Batch_Year=ed.batch_year and m.roll_no=r.Roll_No and ed.exam_code=m.exam_code and ed.degree_code='" + ddldept.SelectedValue + "' and ed.batch_year='" + ddlbatch.SelectedValue + "' and ed.current_semester='" + ddlsem.SelectedValue + "' and ed.Exam_Month='" + ddlmonth.SelectedValue + "' and ed.Exam_year='" + ddlyear.SelectedItem.ToString() + "' and m.subject_no='" + ddlsubject.SelectedValue.ToString() + "' order by r.Reg_No";
            ds = da.select_method_wo_parameter(SQL, "Text");

            string getedu = da.GetFunction("select c.edu_level from degree d,course c where d.course_id=c.course_id and d.degree_code='" + ddldept.SelectedValue.ToString() + "'");

            string deficit = "select min_ext_marks,max_ext_marks,mintotal,maxtotal from subject where subject_no='" + ddlsubject.SelectedValue + "'";
            DataSet ds2 = da.select_method_wo_parameter(deficit, "Text");
            string minmark = "";
            string maxmark = "";
            string mintotal = "";
            string maxtotal = "";
            if (ds2.Tables[0].Rows.Count > 0)
            {
                minmark = Convert.ToString(ds2.Tables[0].Rows[0]["min_ext_marks"]);
                maxmark = Convert.ToString(ds2.Tables[0].Rows[0]["max_ext_marks"]);
                mintotal = Convert.ToString(ds2.Tables[0].Rows[0]["mintotal"]);
                maxtotal = Convert.ToString(ds2.Tables[0].Rows[0]["maxtotal"]);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                int srno = 0;
                int markdiff = 0;
                string diff = da.GetFunction("select value from COE_Master_Settings where settings='Mark Difference'");
                if (diff.Trim() != "" && diff != null)
                {
                    markdiff = Convert.ToInt32(diff);
                }
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Excel.Visible = true;
                Print.Visible = true;
                if (ddlreptype.SelectedItem.Text == "Before Evaluation")
                {
                    FpSpread1.Sheets[0].ColumnCount = 12;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread1.Sheets[0].Columns[0].Width = 50;
                    FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpSpread1.Sheets[0].Columns[1].Width = 150;
                    FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Registration No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpSpread1.Sheets[0].Columns[2].Width = 150;
                    FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    FpSpread1.Sheets[0].Columns[3].Width = 250;
                    FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "External";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    FpSpread1.Sheets[0].Columns[4].Width = 50;
                    FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Internal";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    FpSpread1.Sheets[0].Columns[5].Width = 50;
                    FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Final Marks";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Passing Min " + getedu + "-" + minmark + "/" + maxmark + "";
                    FpSpread1.Sheets[0].Columns[6].Width = 50;
                    FpSpread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "CA Marks";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                    FpSpread1.Sheets[0].Columns[7].Width = 50;
                    FpSpread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 8].Text = "Passing Min  " + getedu + "-" + mintotal + "/" + maxtotal + "";
                    FpSpread1.Sheets[0].Columns[8].Width = 50;
                    FpSpread1.Sheets[0].Columns[8].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Result";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "P/FAIL/AAA";
                    FpSpread1.Sheets[0].Columns[9].Width = 50;
                    FpSpread1.Sheets[0].Columns[9].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Deficit";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);
                    FpSpread1.Sheets[0].Columns[10].Width = 50;
                    FpSpread1.Sheets[0].Columns[10].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Remarks";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
                    FpSpread1.Sheets[0].Columns[11].Width = 50;
                    FpSpread1.Sheets[0].Columns[11].Font.Name = "Book Antiqua";

                    if (Session["Rollflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                    }



                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        srno++;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();

                        string ev1 = Convert.ToString(ds.Tables[0].Rows[i]["evaluation1"]);
                        string ev2 = Convert.ToString(ds.Tables[0].Rows[i]["evaluation2"]);
                        Double ev3 = 0;
                        Double evama1 = 0;
                        Double evama2 = 0;
                        double finalmark = 0;
                        string totmarkval = "";
                        if (ev1.Trim() != "-1" && ev2.Trim() != "-1" && ev1.Trim() != "-3" && ev2.Trim() != "-3")
                        {
                            if (ev2.ToString() != "" && ev2 != null)
                            {
                                evama2 = Convert.ToDouble(ev2);
                            }
                            if (ev1.ToString() != "" && ev1 != null)
                            {
                                evama1 = Convert.ToDouble(ev1);
                            }
                            if (evama1 > evama2)
                            {
                                ev3 = evama1 - evama2;
                            }
                            else if (evama1 < evama2)
                            {
                                ev3 = evama2 - evama1;
                            }

                            if (markdiff >= Convert.ToDouble(ev3))
                            {
                                double fmark = evama1 + evama2;
                                finalmark = fmark / 2;
                                finalmark = Math.Round(finalmark, 0, MidpointRounding.AwayFromZero);
                                totmarkval = finalmark.ToString();
                            }
                        }
                        else
                        {
                            if (ev1 == "-1")
                            {
                                ev1 = "AAA";
                            }
                            if (ev2 == "-1")
                            {
                                ev2 = "AAA";
                            }
                            totmarkval = "AAA";
                            if (ev1 == "-3")
                            {
                                ev1 = "RA";
                            }
                            if (ev2 == "-3")
                            {
                                ev2 = "RA";
                            }
                            if (ev2 == "-3" || ev1 == "RA")
                            {
                                totmarkval = "RA";
                            }
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ev1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ev2;
                        string intmatr = ds.Tables[0].Rows[i]["internal_mark"].ToString();
                        string extmark = ds.Tables[0].Rows[i]["external_mark"].ToString();
                        string fintotmark = "";
                        if (extmark.Trim() != "-1")
                        {
                            if (intmatr.Trim() != "" && intmatr != null && extmark.Trim() != "" && extmark != null)
                            {
                                Double finmark = Convert.ToDouble(intmatr) + Convert.ToDouble(extmark);
                                finmark = Math.Round(finmark, 0, MidpointRounding.AwayFromZero);
                                fintotmark = finmark.ToString();

                            }
                        }
                        else if (extmark.Trim() == "-1")
                        {
                            extmark = "AAA";
                            fintotmark = "AAA";
                        }
                        else if (extmark.Trim() == "-3")
                        {
                            extmark = "RA";
                            fintotmark = "RA";
                        }

                        if (ddlreptype.SelectedItem.Text == "Before Evaluation")
                        {
                            if (totmarkval == "")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "";
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = totmarkval;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = fintotmark;
                                if (totmarkval.Trim() != "AAA" && totmarkval.Trim() != "RA")
                                {
                                    if (mintotal.Trim() != "" && totmarkval.Trim() != "")
                                    {
                                        if (Convert.ToDouble(totmarkval) < Convert.ToDouble(mintotal))
                                        {
                                            Double todefict = Convert.ToDouble(mintotal) - Convert.ToDouble(totmarkval);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = todefict.ToString();
                                        }
                                    }
                                }
                            }

                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = "sfasfas fsaasfasf              fasf ";
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].ForeColor = Color.White;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = extmark;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = fintotmark;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["evaluation3"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].ForeColor = Color.Black;
                            if (totmarkval.Trim() != "AAA" && totmarkval.Trim() != "RA")
                            {
                                if (mintotal.Trim() != "" && fintotmark.Trim() != "")
                                {
                                    if (Convert.ToDouble(fintotmark) < Convert.ToDouble(mintotal))
                                    {
                                        Double todefict = Convert.ToDouble(mintotal) - Convert.ToDouble(fintotmark);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = todefict.ToString();
                                    }
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["internal_mark"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = ds.Tables[0].Rows[i]["result"].ToString();

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnCount = 13;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    FpSpread1.Sheets[0].Columns[0].Width = 50;
                    FpSpread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    FpSpread1.Sheets[0].Columns[1].Width = 150;
                    FpSpread1.Sheets[0].Columns[1].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Registration No";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    FpSpread1.Sheets[0].Columns[2].Width = 150;
                    FpSpread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    FpSpread1.Sheets[0].Columns[3].Width = 250;
                    FpSpread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "External";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    FpSpread1.Sheets[0].Columns[4].Width = 50;
                    FpSpread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Internal";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    FpSpread1.Sheets[0].Columns[5].Width = 50;
                    FpSpread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "III Evaluation";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                    FpSpread1.Sheets[0].Columns[6].Width = 50;
                    FpSpread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";


                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Final Marks";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 7].Text = "Passing Min " + getedu + "-" + minmark + "/" + maxmark + "";
                    FpSpread1.Sheets[0].Columns[7].Width = 50;
                    FpSpread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "CA Marks";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                    FpSpread1.Sheets[0].Columns[8].Width = 50;
                    FpSpread1.Sheets[0].Columns[8].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 9].Text = "Passing Min  " + getedu + "-" + mintotal + "/" + maxtotal + "";
                    FpSpread1.Sheets[0].Columns[9].Width = 50;
                    FpSpread1.Sheets[0].Columns[9].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Result";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 10].Text = "P/FAIL/AAA";
                    FpSpread1.Sheets[0].Columns[10].Width = 50;
                    FpSpread1.Sheets[0].Columns[10].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Deficit";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);
                    FpSpread1.Sheets[0].Columns[11].Width = 50;
                    FpSpread1.Sheets[0].Columns[11].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Remarks";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);
                    FpSpread1.Sheets[0].Columns[11].Width = 50;
                    FpSpread1.Sheets[0].Columns[11].Font.Name = "Book Antiqua";

                    if (Session["Rollflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                    }
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        srno++;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();

                        string ev1 = Convert.ToString(ds.Tables[0].Rows[i]["evaluation1"]);
                        string ev2 = Convert.ToString(ds.Tables[0].Rows[i]["evaluation2"]);
                        Double ev3 = 0;
                        Double evama1 = 0;
                        Double evama2 = 0;
                        double finalmark = 0;
                        string totmarkval = "";
                        if (ev1.Trim() != "-1" && ev2.Trim() != "-1" && ev1.Trim() != "-3" && ev2.Trim() != "-3")
                        {
                            if (ev2.ToString() != "" && ev2 != null)
                            {
                                evama2 = Convert.ToDouble(ev2);
                            }
                            if (ev1.ToString() != "" && ev1 != null)
                            {
                                evama1 = Convert.ToDouble(ev1);
                            }
                            if (evama1 > evama2)
                            {
                                ev3 = evama1 - evama2;
                            }
                            else if (evama1 < evama2)
                            {
                                ev3 = evama2 - evama1;
                            }

                            if (markdiff >= Convert.ToDouble(ev3))
                            {
                                double fmark = evama1 + evama2;
                                finalmark = fmark / 2;
                                totmarkval = finalmark.ToString();
                            }
                        }
                        else
                        {
                            if (ev1 == "-1")
                            {
                                ev1 = "AAA";
                            }
                            if (ev2 == "-1")
                            {
                                ev2 = "AAA";
                            }
                            totmarkval = "AAA";
                            if (ev1 == "-3")
                            {
                                ev1 = "RA";
                            }
                            if (ev2 == "-3")
                            {
                                ev2 = "RA";
                            }
                            if (ev2 == "-3" || ev1 == "RA")
                            {
                                totmarkval = "RA";
                            }
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ev1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ev2;
                        string thrideval = ds.Tables[0].Rows[i]["evaluation3"].ToString();
                        if (thrideval.Trim() != "0")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["evaluation3"]);
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Black;
                        string intmatr = ds.Tables[0].Rows[i]["internal_mark"].ToString();
                        string extmark = ds.Tables[0].Rows[i]["external_mark"].ToString();
                        string fintotmark = "";
                        if (extmark.Trim() != "-1" && extmark.Trim() != "-3")
                        {
                            if (intmatr.Trim() != "" && intmatr != null && extmark.Trim() != "" && extmark != null)
                            {
                                Double finmark = Convert.ToDouble(intmatr) + Convert.ToDouble(extmark);
                                fintotmark = finmark.ToString();
                            }
                        }
                        else if (extmark.Trim() == "-1")
                        {
                            extmark = "AAA";
                            fintotmark = "AAA";
                        }
                        else if (extmark.Trim() == "-3")
                        {
                            extmark = "RA";
                            fintotmark = "RA";
                        }

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = extmark;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = fintotmark;
                        if (totmarkval.Trim() != "AAA" && totmarkval.Trim() != "RA")
                        {
                            if (mintotal.Trim() != "" && fintotmark.Trim() != "")
                            {
                                if (Convert.ToDouble(fintotmark) < Convert.ToDouble(mintotal))
                                {
                                    Double todefict = Convert.ToDouble(mintotal) - Convert.ToDouble(fintotmark);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = todefict.ToString();
                                }
                            }
                        }
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = "sfasfas                                                               asds";
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].ForeColor = Color.White;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[i]["internal_mark"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = ds.Tables[0].Rows[i]["result"].ToString();

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            else
            {
                lblmsg.Visible = true;
                lblmsg.Text = "No Records Found";
            }
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }


    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            try
            {
                string reportname = txtexcelname.Text;

                if (reportname.ToString().Trim() != "")
                {
                    da.printexcelreport(FpSpread1, reportname);
                }
                else
                {
                    lblmsg.Text = "Please Enter Your Report Name";
                    lblmsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.ToString();
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {
            string conductedhours_ptn = FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Note.ToString();
            string rename = " (After III Evaluation)";
            if (ddlreptype.SelectedItem.Text == "Before Evaluation")
            {
                rename = " (Before III Evaluation)";
            }
            string strsubcode = da.GetFunction("Select Subject_code from subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'");

            string sthe1 = "Course : " + ddldegree.SelectedItem.ToString() + "";
            string sthe2 = "Department :" + ddldept.SelectedItem.ToString() + "";
            string sthe3 = "Subject Code : " + strsubcode + "";
            string sthe4 = "Semester : " + ddlsem.SelectedItem.ToString() + "";
            string sthe5 = "Batch : " + ddlbatch.SelectedItem.ToString() + "";

            int len1 = sthe1.Length;
            int len2 = sthe2.Length;
            int len3 = sthe3.Length;
            int len4 = sthe4.Length;
            int maxlen = 0;
            if (maxlen < len1)
            {
                maxlen = len1;
            }
            if (maxlen < len2)
            {
                maxlen = len2;
            }
            if (maxlen < len3)
            {
                maxlen = len3;
            }
            if (maxlen < len4)
            {
                maxlen = len4;
            }
            string empsopace = "          ";
            for (int i = 1; i <= 4; i++)
            {
                if (i == 1)
                {
                    for (int st = len1; st < maxlen; st++)
                    {
                        sthe1 = sthe1 + "  ";
                    }
                    sthe1 = sthe1 + empsopace;
                }
                if (i == 2)
                {
                    for (int st = len2; st < maxlen; st++)
                    {
                        sthe2 = sthe2 + "  ";
                    }
                    sthe2 = sthe2 + empsopace;
                }
                if (i == 3)
                {
                    for (int st = len3; st < maxlen; st++)
                    {
                        sthe3 = sthe3 + "  ";
                    }
                    sthe3 = sthe3 + empsopace;
                }
                if (i == 4)
                {
                    for (int st = len4; st < maxlen; st++)
                    {
                        sthe4 = sthe4 + "  ";
                    }
                    sthe4 = sthe4 + empsopace;
                }

            }

            string degreedetails = "Office of the Controller of Examinations $Passing Board Report For Examination  - " + ddlmonth.SelectedItem.ToString() + "-" + ddlyear.SelectedItem.ToString() + " " + rename + "@" + sthe1 + sthe2 + "@" + sthe3 + sthe4 + sthe5 + "";
            Printcontrol.loadspreaddetails(FpSpread1, "Passing_Board_Report.aspx", degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
}