using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;

public partial class UnivresultAnalysis : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string gpa = string.Empty;
    string creitpoint = string.Empty;
    string sections = string.Empty;
    int ExamCode = 0;
    string current_sem = string.Empty;
    string strsubject = string.Empty;
    int allpasscount = 0;
    int allappeared = 0;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label2.Visible = false;
            Label5.Visible = false;
            lblerror.Visible = false;
            lblerrormsg.Visible = false;
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
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                Label5.Visible = false;
                Chart1.Visible = false;
                btnExcel1.Visible = false;
                btnPrint1.Visible = false;
                chkIncludeDiscontinue.Checked = false;
                flow.Visible = false;
                Fpspread1.Visible = false;
                dvconsolidated.Style.Add("display", "none");
                dvsubjectwise.Style.Add("display", "none");
                lastdiv.Style.Add("display", "none");
                rbbeforeandafterrevaluation.Enabled = false;
                rbformat.Enabled = false;
                rbmoderation.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
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
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            ddlsem.Items.Clear();
            has.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            ddlsem.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            has.Clear();
            collegecode = ddlcollege.SelectedItem.Value;
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("college_code", collegecode);
            ds = da.select_method("bind_sem", has, "sp");
            int count3 = ds.Tables[0].Rows.Count;
            if (count3 > 0)
            {
                ddlsem.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlsem.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                count3 = ds.Tables[1].Rows.Count;
                if (count3 > 0)
                {
                    ddlsem.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlsem.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlsem.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindsec()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            ddlSec.Items.Clear();
            hat.Clear();
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue);
            ds = da.select_method("bind_sec", hat, "sp");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
            ddlSec.Items.Add("ALL");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindcollege()
    {
        try
        {
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
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
            ds = da.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void grd2()
    {
        try
        {
            Totalgrd.Visible = false;
            if (txtTop.Text != "")
            {
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("S.NO", typeof(string));
                dt1.Columns.Add("REGISTER NO", typeof(string));
                dt1.Columns.Add("STUDENTS NAME", typeof(string));
                dt1.Columns.Add("CREDIT", typeof(string));
                dt1.Columns.Add("GPA", typeof(string));
                dt1.Columns.Add("RANK", typeof(string));
                ds.Dispose();
                ds = da.select_method("select * from sysobjects where name='tbl_Topperrank' and Type='U'", hat, "text ");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int q = da.insert_method("drop table tbl_Topperrank", hat, "text");
                    int p = da.insert_method("create table tbl_Topperrank (roll_no nvarchar(50),cgpa float (8),stud_name nvarchar(200),degree nvarchar(500))", hat, "text");
                }
                else
                {
                    int p = da.insert_method("create table tbl_Topperrank (roll_no nvarchar(50),cgpa float (8),stud_name nvarchar(200),degree nvarchar(500))", hat, "text");
                }
                ds.Dispose();
                ds = da.select_method("if exists(select name from sysobjects where xtype='p' and name='sp_ins_upd_topperrank' )drop proc sp_ins_upd_topperrank", hat, "text ");
                int s = da.insert_method("CREATE   procedure sp_ins_upd_topperrank (@RollNumber varchar(50), @cgpa varchar(20), @stud_name varchar(20), @degree varchar(200)) as declare @cou_nt int set @cou_nt=(select count(Roll_no)from tbl_Topperrank where Roll_no=@RollNumber) if @cou_nt=0 BEGIN insert into tbl_Topperrank(Roll_no,cgpa,stud_name,degree)values(@RollNumber,@cgpa,@stud_name,@degree) End Else BEGIN update  tbl_Topperrank set cgpa=@cgpa where Roll_no=@RollNumber End", hat, "Text");
                ds.Dispose();
                string strsec = string.Empty;
                if (ddlSec.Enabled == true)
                {
                    if (ddlSec.SelectedItem.ToString().Trim().ToLower() != "all" && ddlSec.SelectedValue.ToString() != "-1" && ddlSec.SelectedItem.ToString() != "")
                    {
                        strsec = " and sections='" + ddlSec.SelectedValue.ToString() + "'";
                    }
                }
                string degreecode = ddlbranch.SelectedValue.ToString();
                string batch = ddlbatch.SelectedValue.ToString();
                string sem = ddlsem.SelectedValue.ToString();
                string collegecode = ddlcollege.SelectedValue.ToString();
                string strgetstudegtails = "SELECT Roll_No,Reg_No,Stud_Name from Registration  where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + strsec + " ;";
                strgetstudegtails = strgetstudegtails + " select Exam_Month,Exam_year,exam_code from Exam_Details where  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and Current_Semester='" + ddlsem.SelectedValue.ToString() + "'";
                ds = da.select_method_wo_parameter(strgetstudegtails, "Text");
                string strexammonth = string.Empty;
                string strexamyear = string.Empty;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    strexammonth = ds.Tables[1].Rows[0]["Exam_Month"].ToString();
                    strexamyear = ds.Tables[1].Rows[0]["Exam_year"].ToString();
                }
                else
                {
                    Label5.Visible = true;
                    Label5.Text = "No Exam Conducted";
                }
                int ghk = 0;
                for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    string roll_value = ds.Tables[0].Rows[rolcount]["Roll_No"].ToString();
                    string regno = ds.Tables[0].Rows[rolcount]["Reg_No"].ToString();
                    string name = ds.Tables[0].Rows[rolcount]["Stud_Name"].ToString();
                    string getexamncode = da.GetFunction("select exam_code from Exam_Details where batch_year='" + batch + "' and degree_code='" + degreecode + "' and Exam_Month='" + strexammonth + "' and Exam_year='" + strexamyear + "'");
                    if (getexamncode != null && getexamncode.Trim() != "" & getexamncode.Trim() != "0")
                    {
                        int failcount = Convert.ToInt32(da.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code ='" + getexamncode + "'  and roll_no='" + roll_value + "'  and result in ('Fail','AAA','WHD') and attempts=1 "));
                        if (failcount == 0)
                        {
                            gpa = string.Empty;
                            creitpoint = string.Empty;
                            Calulat_GPA(roll_value, degreecode, batch, strexammonth, strexamyear, collegecode);
                            if (gpa != "0" && gpa != "")
                            {
                                ghk = 1;
                                string steval = regno + '-' + creitpoint;
                                hat.Clear();
                                hat.Add("RollNumber", roll_value);
                                hat.Add("cgpa", gpa.ToString());
                                hat.Add("stud_name", name.ToString());
                                hat.Add("degree", steval.ToString());
                                int o = da.insert_method("sp_ins_upd_topperrank", hat, "sp");
                            }
                        }
                    }
                }
                int count = 0;
                string query = string.Empty;
                query = "select Top " + txtTop.Text + " row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,dense_rank() OVER (ORDER BY  cgpa desc)as rank from tbl_Topperrank order by cgpa desc";
                ds.Dispose();
                ds.Reset();
                ds = da.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (grd.Visible == true)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            count++;
                            DataRow dtrow = dt1.NewRow();
                            dtrow[0] = i + 1;
                            string regno = string.Empty;
                            string creval = string.Empty;
                            string[] spba = ds.Tables[0].Rows[i]["degree"].ToString().Split('-');
                            if (spba.GetUpperBound(0) == 1)
                            {
                                regno = spba[0].ToString();
                                creval = spba[1].ToString();
                            }
                            dtrow[1] = regno;
                            dtrow[2] = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                            dtrow[3] = creval;
                            dtrow[4] = ds.Tables[0].Rows[i]["cgpa"].ToString();
                            dtrow[5] = count;
                            dt1.Rows.Add(dtrow);
                            Totalgrd.DataSource = dt1;
                            Totalgrd.DataBind();
                            Totalgrd.Visible = true;
                        }
                    }
                }
                else
                {
                    Totalgrd.Visible = false;
                    Totalgrd.Visible = false;
                }
                if (ghk == 0)
                {
                    Label2.Visible = true;
                    Label2.Text = "No GPA Calculate So Topper List Can't Visible";
                }
            }
        }
        catch (Exception ex)
        {
            Label5.Text = ex.Message;
            Label5.Visible = true;
        }
    }

    public void grd3()
    {
        try
        {
            if (grd.Visible == true)
            {
                int total = 0;
                double passpercent1 = 0;
                double passpercent = 0;
                double passpercent2 = 0;
                DataView dvv = new DataView();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                Hashtable ht = new Hashtable();
                ArrayList checkarray = new ArrayList();
                dt1.Columns.Add("SUBJECT CODE", typeof(string));
                dt1.Columns.Add("STAFF NAME", typeof(string));
                dt1.Columns.Add("SUBJECT NAME", typeof(string));
                dt1.Columns.Add("PASS", typeof(double));
                dt2.Columns.Add("SUBJECT NAME", typeof(string));
                dt2.Columns.Add("PASS", typeof(double));
                string str3 = "select distinct s.subject_code,sm.staff_name,s.subject_name,sb.subject_type from subject s,syllabus_master sy,staff_selector st,staffmaster sm, sub_sem sb where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subType_no=sb.subType_no and  sy.syll_code=sb.syll_code and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and sy.degree_code='" + ddlbranch.SelectedItem.Value + "' and sb.promote_count='1' and sy.semester='" + ddlsem.SelectedItem.Value + "'";
                if (ddlSec.SelectedItem.Text != "ALL")
                {
                    str3 = str3 + "and st.Sections='" + ddlSec.SelectedItem.Value + "'";
                }
                str3 = str3 + " order by sb.subject_type desc";
                ds = da.select_method_wo_parameter(str3, "Text");
                //magesh 14/2/18
                //string passfail = "select distinct subject_code,count(result) as pass,passorfail from mark_entry m,registration r,subject s,Exam_Details e where m.roll_no=r.roll_no and  s.subject_no=m.subject_no and e.exam_code=m.exam_code and m.attempts=1   and   r.degree_code='" + ddlbranch.SelectedItem.Value + "' and r.batch_year='" + ddlbatch.SelectedItem.Value + "'";
                string passfail = "select distinct subject_code,count(result) as pass,passorfail from mark_entry m,registration r,subject s,Exam_Details e where m.roll_no=r.roll_no and result in ('pass','Fail') and  s.subject_no=m.subject_no and e.exam_code=m.exam_code and m.attempts=1   and   r.degree_code='" + ddlbranch.SelectedItem.Value + "' and r.batch_year='" + ddlbatch.SelectedItem.Value + "'";
                
                if (ddlSec.SelectedItem.Text != "ALL")
                {
                    passfail = passfail + "and r.Sections='" + ddlSec.SelectedItem.Value + "' ";
                }
                passfail = passfail + "group by subject_code,result,passorfail";
                ds1 = da.select_method_wo_parameter(passfail, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        int check = 0;
                        for (int h = 0; h < ds.Tables[0].Rows.Count; h++)
                        {
                            DataRow dr = dt1.NewRow();
                            string subjectcode = (ds.Tables[0].Rows[h]["subject_code"].ToString());
                            ds1.Tables[0].DefaultView.RowFilter = "subject_code='" + subjectcode + "' and  passorfail in('false','true')";
                            dvv = ds1.Tables[0].DefaultView;
                            if (dvv.Count == 2)
                            {
                                total = Convert.ToInt32(dvv[0]["pass"].ToString()) + Convert.ToInt32(dvv[1]["pass"].ToString());
                                passpercent1 = 0;
                                if (dvv[1]["passorfail"].ToString() == "True")
                                {
                                    passpercent1 = Convert.ToDouble((Convert.ToDouble(dvv[1]["pass"].ToString()) / total) * 100);
                                }
                                else if (dvv[1]["passorfail"].ToString() == "False")
                                {
                                    passpercent1 = Convert.ToDouble((Convert.ToDouble(dvv[0]["pass"].ToString()) / total) * 100);
                                }
                                passpercent2 = Math.Round(passpercent1, 2);
                                passpercent = (passpercent2);
                            }
                            else if (dvv.Count == 1)
                            {
                                total = Convert.ToInt32(dvv[0]["pass"].ToString());
                                passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(dvv[0]["pass"].ToString()) / total) * 100);
                                passpercent2 = Math.Round(passpercent1, 2);
                                passpercent = (passpercent2);
                            }
                            dr[0] = ds.Tables[0].Rows[h]["subject_code"].ToString();
                            dr[1] = ds.Tables[0].Rows[h]["staff_name"].ToString();
                            dr[2] = ds.Tables[0].Rows[h]["subject_name"].ToString();
                            dr[3] = passpercent;
                            if (!checkarray.Contains(ds.Tables[0].Rows[h]["subject_name"].ToString()))
                            {
                                check++;
                                DataRow dr2 = dt2.NewRow();
                                checkarray.Add(ds.Tables[0].Rows[h]["subject_name"].ToString());
                                dr2[0] = ds.Tables[0].Rows[h]["subject_name"].ToString();
                                dr2[1] = passpercent.ToString();
                                dt2.Rows.Add(dr2);
                            }
                            else
                            {
                                int percentage = Convert.ToInt32(dt2.Rows[check - 1]["PASS"]);
                                if (percentage < passpercent)
                                {
                                    check--;
                                    dt2.Rows.RemoveAt(check);
                                    DataRow dr2 = dt2.NewRow();
                                    checkarray.Add(ds.Tables[0].Rows[h]["subject_name"].ToString());
                                    dr2[0] = ds.Tables[0].Rows[h]["subject_name"].ToString();
                                    dr2[1] = passpercent.ToString();
                                    dt2.Rows.Add(dr2);
                                    check++;
                                }
                            }
                            dt1.Rows.Add(dr);
                        }
                        staffgvd.DataSource = dt1;
                        staffgvd.DataBind();
                        staffgvd.Visible = true;
                        btnPrint1.Visible = true;
                        btnExcel1.Visible = true;
                        Chart1.DataSource = dt2;
                        Chart1.DataBind();
                        Chart1.Visible = true;
                        Chart1.Enabled = false;
                        Chart1.ChartAreas[0].AxisX.RoundAxisValues();
                        Chart1.ChartAreas[0].AxisX.Minimum = 0;
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        Chart1.Series["Series1"].IsValueShownAsLabel = true;
                        Chart1.Series[0].ChartType = SeriesChartType.Column;
                        Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisX.Title = "SUBJECT NAME";
                        Chart1.ChartAreas[0].AxisY.Title = "PASS%";
                        Chart1.Series["Series1"].XValueMember = "SUBJECT NAME";
                        Chart1.Series["Series1"].YValueMembers = "PASS";
                        Chart1.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                        Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        staffgvd.Visible = false;
                        Chart1.Visible = false;
                        flow.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    //public void grdrow()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt.Columns.Add("S.NO", typeof(string));
    //        dt.Columns.Add("PARTICULARS", typeof(string));
    //        dt.Columns.Add("TOTAL", typeof(double));
    //        dt.Columns.Add("G-DS", typeof(double));
    //        dt.Columns.Add("G-HOSTEL", typeof(double));
    //        dt.Columns.Add("B-DS", typeof(double));
    //        dt.Columns.Add("B-HOSTEL", typeof(double));
    //        DataRow dtrow = dt.NewRow();
    //        DataRow dtrow1 = dt.NewRow();
    //        DataRow dtrow2 = dt.NewRow();
    //        DataRow dtrow3 = dt.NewRow();
    //        DataRow dtrow4 = dt.NewRow();
    //        DataRow dtrow5 = dt.NewRow();
    //        DataRow dtrow6 = dt.NewRow();
    //        DataRow dtrow7 = dt.NewRow();
    //        dtrow[0] = "1";
    //        dtrow[1] = "NO OF STUDENTS APPEARED";
    //        dtrow1[0] = "2";
    //        dtrow1[1] = "NO OF STUDENTS FAILED";
    //        dtrow2[0] = "3";
    //        dtrow2[1] = "NO OF STUDENTS PASSED IN ALL SUBJECTS";
    //        dtrow3[0] = "4";
    //        dtrow3[1] = "NO OF STUDENTS FAILED IN ONE SUBJECT";
    //        dtrow4[0] = "5";
    //        dtrow4[1] = "NO OF STUDENTS FAILED IN TWO SUBJECTS";
    //        dtrow5[0] = "6";
    //        dtrow5[1] = "NO OF STUDENTS FAILED IN 3 & ABOVE SUBJECT";
    //        dtrow6[0] = "7";
    //        dtrow6[1] = "PASS PERCENTAGE %";
    //        //string str1 = "select COUNT(distinct m.roll_no) as appeared, r.degree_code from mark_entry m,Exam_Details e,Registration r   where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "'     and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result!='AAA' and m.roll_no not in(select distinct m.roll_no from mark_entry m,Exam_Details e,Registration r   where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "'    and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result='AAA')";
    //        string str1 = "select COUNT(distinct m.roll_no) as appeared, r.degree_code from mark_entry m,Registration r   where   m.roll_no=r.Roll_No  and r.college_code='" + ddlcollege.SelectedItem.Value + "'     and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result!='AAA' and m.roll_no not in(select distinct m.roll_no from mark_entry m,Registration r   where   m.roll_no=r.Roll_No  and r.college_code='" + ddlcollege.SelectedItem.Value + "'    and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result='AAA')";
    //        if (ddlSec.SelectedItem.Text != "ALL")
    //        {
    //            str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //        }
    //        str1 = str1 + " group by r.degree_code ";
    //        str1 = str1 + "select COUNT(distinct r.roll_no) as fail, r.degree_code from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "'  and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','AAA','WHD') and m.attempts=1 ";
    //        if (ddlSec.SelectedItem.Text != "ALL")
    //        {
    //            str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //        }
    //        str1 = str1 + "  group by r.degree_code ";
    //        //str1 = str1 + " select COUNT(distinct r.roll_no) as Pass, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no not in(select distinct r1.roll_no from mark_entry m1,Exam_Details e1,Registration r1   where e1.exam_code=m1.exam_code and m1.roll_no=r1.Roll_No and e1.batch_year=r1.Batch_Year and r1.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r1.college_code='" + ddlcollege.SelectedItem.Value + "' and e1.current_semester='" + ddlsem.SelectedItem.Value + "' and r1.cc=0 and  r1.exam_flag <>'DEBAR'  and r1.delflag=0 and result in ('fail','AAA') and m1.attempts=1 )  ";
    //        // Added by Priya
    //        str1 = str1 + " select COUNT(distinct r.roll_no) as Pass, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "' and  r.cc=0 and r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1  and m.roll_no not in(select m1.roll_no from mark_entry m1 where m1.roll_no=r.Roll_No and m1.result in ('fail','AAA') and m1.attempts=1 )   ";
    //        if (ddlSec.SelectedItem.Text != "ALL")
    //        {
    //            str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //        }
    //        str1 = str1 + " group by r.degree_code ";
    //        str1 = str1 + "select COUNT( r.roll_no) as count1,a.sex,r.Stud_Type from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and  r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','WHD') and m.attempts=1 ";
    //        if (ddlSec.SelectedItem.Text != "ALL")
    //        {
    //            str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //        }
    //        str1 = str1 + " group by a.sex,r.Stud_Type ,m.roll_no ";
    //        ds1 = da.select_method_wo_parameter(str1, "Text");
    //        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
    //        {
    //            dtrow[2] = ds1.Tables[0].Rows[0]["appeared"].ToString();
    //            if (ds1.Tables[1].Rows.Count > 0)
    //            {
    //                dtrow1[2] = ds1.Tables[1].Rows[0]["Fail"].ToString();
    //            }
    //            else
    //            {
    //                dtrow1[2] = "0";
    //                dtrow1[3] = "0";
    //                dtrow1[4] = "0";
    //                dtrow1[5] = "0";
    //                dtrow1[6] = "0";
    //            }
    //            if (ds1.Tables[2].Rows.Count > 0)
    //            {
    //                dtrow2[2] = ds1.Tables[2].Rows[0]["pass"].ToString();
    //            }
    //            else
    //            {
    //                dtrow2[2] = "0";
    //                dtrow2[3] = "0";
    //                dtrow2[4] = "0";
    //                dtrow2[5] = "0";
    //                dtrow2[6] = "0";
    //            }
    //            DataView vd = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='1'";
    //            vd = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //            {
    //                dtrow3[2] = vd.Count;
    //            }
    //            else
    //            {
    //                dtrow3[2] = "0";
    //            }
    //            DataView spl = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Hostler') and  sex = ('1')";
    //            spl = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //            {
    //                dtrow3[4] = vd.Count;
    //            }
    //            else
    //            {
    //                dtrow3[4] = "0";
    //            }
    //            DataView sp2 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Day Scholar') and  sex = ('1')";
    //            sp2 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //            {
    //                dtrow3[3] = vd.Count;
    //            }
    //            else
    //            {
    //                dtrow3[3] = "0";
    //            }
    //            DataView sp3 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Hostler') and  sex = ('0')";
    //            sp3 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //            {
    //                dtrow3[6] = vd.Count;
    //            }
    //            else
    //            {
    //                dtrow3[6] = "0";
    //            }
    //            DataView sp4 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Day Scholar') and  sex = ('0')";
    //            sp4 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow3[5] = vd.Count;
    //            else
    //                dtrow3[5] = "0";
    //            DataView vd1 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='2'";
    //            vd1 = ds1.Tables[3].DefaultView;
    //            if (vd1.Count > 0)
    //                dtrow4[2] = vd1.Count;
    //            else
    //                dtrow4[2] = "0";
    //            DataView spl1 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Hostler') and  sex = ('1')";
    //            spl1 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow4[4] = vd.Count;
    //            else
    //                dtrow4[4] = "0";
    //            DataView sp21 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Day Scholar') and  sex = ('1')";
    //            sp21 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow4[3] = vd.Count;
    //            else
    //                dtrow4[3] = "0";
    //            DataView sp31 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Hostler') and  sex = ('0')";
    //            sp31 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow4[6] = vd.Count;
    //            else
    //                dtrow4[6] = "0";
    //            DataView sp41 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Day Scholar') and  sex = ('0')";
    //            sp41 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow4[5] = vd.Count;
    //            else
    //                dtrow4[5] = "0";
    //            DataView vd2 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2')";
    //            vd2 = ds1.Tables[3].DefaultView;
    //            if (vd2.Count > 0)
    //                dtrow5[2] = vd2.Count;
    //            else
    //                dtrow5[2] = "0";
    //            DataView sp1l1 = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Hostler') and  sex = ('1')";
    //            sp1l1 = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow5[4] = vd.Count;
    //            else
    //                dtrow5[4] = "0";
    //            DataView sp21k = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Day Scholar') and  sex = ('1')";
    //            sp21k = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow5[3] = vd.Count;
    //            else
    //                dtrow5[3] = "0";
    //            DataView sp311k = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Hostler') and  sex = ('0')";
    //            sp311k = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow5[6] = vd.Count;
    //            else
    //                dtrow5[6] = "0";
    //            DataView sp411k = new DataView();
    //            ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Day Scholar') and  sex = ('0')";
    //            sp411k = ds1.Tables[3].DefaultView;
    //            if (vd.Count > 0)
    //                dtrow5[5] = vd.Count;
    //            else
    //                dtrow5[5] = "0";
    //            string appeared = " select COUNT(distinct m.roll_no) as appeared, r.degree_code,r.Stud_Type,a.sex from mark_entry m,Exam_Details e,Registration r, applyn a   where  e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "'    and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "'   and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result!='AAA' and m.roll_no not in(select distinct m.roll_no from mark_entry m,Exam_Details e,Registration r   where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result='AAA') ";
    //            if (ddlSec.SelectedItem.Text != "ALL")
    //            {
    //                appeared = appeared + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //            }
    //            appeared = appeared + "group by r.degree_code,r.Stud_Type,a.sex";
    //            ds = da.select_method_wo_parameter(appeared, "text");
    //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //            {
    //                DataView rsap = new DataView();
    //                ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('1')";
    //                rsap = ds.Tables[0].DefaultView;
    //                if (rsap.Count > 0)
    //                {
    //                    dtrow[3] = rsap[0]["appeared"].ToString();
    //                }
    //                else
    //                {
    //                    dtrow[3] = "0";
    //                }
    //                DataView rsap1 = new DataView();
    //                ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('0')";
    //                rsap1 = ds.Tables[0].DefaultView;
    //                if (rsap1.Count > 0)
    //                {
    //                    dtrow[5] = rsap1[0]["appeared"].ToString();
    //                }
    //                else
    //                {
    //                    dtrow[5] = "0";
    //                }
    //                DataView rsaps = new DataView();
    //                ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('1')";
    //                rsaps = ds.Tables[0].DefaultView;
    //                if (rsaps.Count > 0)
    //                {
    //                    dtrow[4] = rsaps[0]["appeared"].ToString();
    //                }
    //                else
    //                {
    //                    dtrow[4] = "0";
    //                }
    //                DataView rsaps1 = new DataView();
    //                ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('0')";
    //                rsaps1 = ds.Tables[0].DefaultView;
    //                if (rsaps1.Count > 0)
    //                {
    //                    dtrow[6] = rsaps1[0]["appeared"].ToString();
    //                }
    //                else
    //                {
    //                    dtrow[6] = "0";
    //                }
    //                string fail = "  select COUNT(distinct r.roll_no) as fail, r.degree_code,r.Stud_Type,a.sex from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedValue + "' and e.degree_code='" + ddlbranch.SelectedValue + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Text + "' and  r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','AAA','WHD') and m.attempts=1";
    //                if (ddlSec.SelectedItem.Text != "ALL")
    //                {
    //                    fail = fail + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //                }
    //                fail = fail + "group by r.degree_code,r.Stud_Type,a.sex ";
    //                ds = da.select_method_wo_parameter(fail, "text");
    //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                {
    //                    DataView rs = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('1')";
    //                    rs = ds.Tables[0].DefaultView;
    //                    if (rs.Count > 0)
    //                    {
    //                        dtrow1[3] = rs[0]["fail"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dtrow1[3] = "0";
    //                    }
    //                    DataView rs1 = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('1')";
    //                    rs1 = ds.Tables[0].DefaultView;
    //                    if (rs1.Count > 0)
    //                    {
    //                        dtrow1[4] = rs1[0]["fail"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dtrow1[4] = "0";
    //                    }
    //                    DataView rs12 = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex =('0')";
    //                    rs12 = ds.Tables[0].DefaultView;
    //                    if (rs12.Count > 0)
    //                    {
    //                        dtrow1[5] = rs12[0]["fail"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dtrow1[5] = "0";
    //                    }
    //                    DataView rs13 = new DataView();
    //                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('0')";
    //                    rs13 = ds.Tables[0].DefaultView;
    //                    if (rs13.Count > 0)
    //                    {
    //                        dtrow1[6] = rs13[0]["fail"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dtrow1[6] = "0";
    //                    }
    //                    //string pass = "select COUNT(distinct r.roll_no) as Pass, r.degree_code,a.sex,r.Stud_Type from mark_entry m,Exam_Details e,Registration r,applyn a  where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and r.App_No=a.app_no  and e.batch_year=r.Batch_Year  and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no not in(select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "'  and r.cc=0 and  r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )   ";
    //                    //Added by Priya
    //                    string pass = "select COUNT(distinct r.roll_no) as Pass, r.degree_code,r.Stud_Type,a.sex from mark_entry m, Registration r,applyn a where m.roll_no=r.Roll_No and r.App_No=a.app_no and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and r.cc=0 and   r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and r.roll_no not in(select m1.roll_no from mark_entry m1,Exam_Details e where e.exam_code=m1.exam_code and m1.roll_no=r.Roll_No and m1.result in ('fail','AAA') and e.batch_year=" + ddlbatch.SelectedItem.Value + " and e.degree_code=" + ddlbranch.SelectedItem.Value + "  and m1.attempts=1 )  ";
    //                    if (ddlSec.SelectedItem.Text != "ALL")
    //                    {
    //                        pass = pass + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
    //                    }
    //                    pass = pass + "group by r.degree_code,a.sex,r.Stud_Type";
    //                    ds = da.select_method_wo_parameter(pass, "text");
    //                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        DataView rs15 = new DataView();
    //                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex in ('1')";
    //                        rs15 = ds.Tables[0].DefaultView;
    //                        if (rs15.Count > 0)
    //                        {
    //                            dtrow2[3] = rs15[0]["pass"].ToString();
    //                        }
    //                        else
    //                        {
    //                            dtrow2[3] = "0";
    //                        }
    //                        DataView rs16 = new DataView();
    //                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex in ('1')";
    //                        rs16 = ds.Tables[0].DefaultView;
    //                        if (rs16.Count > 0)
    //                        {
    //                            dtrow2[4] = rs16[0]["pass"].ToString();
    //                        }
    //                        else
    //                        {
    //                            dtrow2[4] = "0";
    //                        }
    //                        DataView rs17 = new DataView();
    //                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex in ('0')";
    //                        rs17 = ds.Tables[0].DefaultView;
    //                        if (rs17.Count > 0)
    //                        {
    //                            dtrow2[5] = rs17[0]["pass"].ToString();
    //                        }
    //                        else
    //                        {
    //                            dtrow2[5] = "0";
    //                        }
    //                        DataView rs18 = new DataView();
    //                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex in ('0')";
    //                        rs18 = ds.Tables[0].DefaultView;
    //                        if (rs18.Count > 0)
    //                        {
    //                            dtrow2[6] = rs18[0]["pass"].ToString();
    //                        }
    //                        else
    //                        {
    //                            dtrow2[6] = "0";
    //                        }
    //                        double tot123 = Convert.ToDouble(dtrow[2]);
    //                        double totapp = Convert.ToDouble(dtrow2[2]);
    //                        if (tot123 > 0)
    //                        {
    //                            double totfi = totapp / tot123;
    //                            totfi = totfi * 100;
    //                            totfi = Math.Round(totfi, 2);
    //                            dtrow6[2] = totfi;
    //                        }
    //                        else
    //                        {
    //                            dtrow6[2] = "0";
    //                        }
    //                        double tot = Convert.ToDouble(dtrow[3]);
    //                        double totap = Convert.ToDouble(dtrow2[3]);
    //                        if (tot > 0)
    //                        {
    //                            double totfi = totap / tot;
    //                            totfi = totfi * 100;
    //                            totfi = Math.Round(totfi, 2);
    //                            dtrow6[3] = totfi;
    //                        }
    //                        else
    //                        {
    //                            dtrow6[3] = "0";
    //                        }
    //                        double totfinal1 = 0;
    //                        double tot1 = Convert.ToDouble(dtrow[4]);
    //                        double totapp1 = Convert.ToDouble(dtrow2[4]);
    //                        if (tot1 > 0)
    //                        {
    //                            totfinal1 = totapp1 / tot1;
    //                            totfinal1 = totfinal1 * 100;
    //                            totfinal1 = Math.Round(totfinal1, 2);
    //                            dtrow6[4] = totfinal1;
    //                        }
    //                        else
    //                        {
    //                            dtrow6[4] = "0";
    //                        }
    //                        double tot12 = Convert.ToDouble(dtrow[5]);
    //                        double totapp2 = Convert.ToDouble(dtrow2[5]);
    //                        if (tot12 > 0)
    //                        {
    //                            double totfinal2 = totapp2 / tot12;
    //                            totfinal2 = totfinal2 * 100;
    //                            totfinal2 = Math.Round(totfinal2, 2);
    //                            dtrow6[5] = totfinal2;
    //                        }
    //                        else
    //                        {
    //                            dtrow6[5] = "0";
    //                        }
    //                        double tot1234 = Convert.ToDouble(dtrow[6]);
    //                        double totapp3 = Convert.ToDouble(dtrow2[6]);
    //                        if (tot1234 > 0)
    //                        {
    //                            double totfinal3 = totapp3 / tot1234;
    //                            totfinal3 = totfinal3 * 100;
    //                            totfinal3 = Math.Round(totfinal3, 2);
    //                            dtrow6[6] = totfinal3;
    //                        }
    //                        else
    //                        {
    //                            dtrow6[6] = "0";
    //                        }
    //                    }
    //                    else
    //                    {
    //                        dtrow6[6] = "0";
    //                        dtrow6[5] = "0";
    //                        dtrow6[4] = "0";
    //                        dtrow6[3] = "0";
    //                        dtrow6[2] = "0";
    //                        dtrow2[6] = "0";
    //                        dtrow2[5] = "0";
    //                        dtrow2[4] = "0";
    //                        dtrow2[3] = "0";
    //                    }
    //                    //}
    //                    //else
    //                    //{
    //                    //    dtrow6[6] = "0";
    //                    //    dtrow6[5] = "0";
    //                    //    dtrow6[4] = "0";
    //                    //    dtrow6[3] = "0";
    //                    //    dtrow6[2] = "0";
    //                    //    dtrow2[6] = "0";
    //                    //    dtrow2[5] = "0";
    //                    //    dtrow2[4] = "0";
    //                    //    dtrow2[3] = "0";
    //                }
    //                dt.Rows.Add(dtrow);
    //                dt.Rows.Add(dtrow1);
    //                dt.Rows.Add(dtrow2);
    //                dt.Rows.Add(dtrow3);
    //                dt.Rows.Add(dtrow4);
    //                dt.Rows.Add(dtrow5);
    //                dt.Rows.Add(dtrow6);
    //                grd.DataSource = dt;
    //                grd.DataBind();
    //                grd.Visible = true;
    //                btnExcel1.Visible = true;
    //                btnPrint1.Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            lblerror.Text = "No Records Found";
    //            lblerror.Visible = true;
    //            btnExcel1.Visible = false;
    //            btnPrint1.Visible = false;
    //            Chart1.Visible = false;
    //            grd.Visible = false;
    //            flow.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Label5.Text = ex.Message;
    //        Label5.Visible = true;
    //    }
    //}

    public void grdrow()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("S.NO", typeof(string));
            dt.Columns.Add("PARTICULARS", typeof(string));
            dt.Columns.Add("TOTAL", typeof(double));
            dt.Columns.Add("G-DS", typeof(double));
            dt.Columns.Add("G-HOSTEL", typeof(double));
            dt.Columns.Add("B-DS", typeof(double));
            dt.Columns.Add("B-HOSTEL", typeof(double));
            DataRow dtrow = dt.NewRow();
            DataRow dtrow1 = dt.NewRow();
            DataRow dtrow2 = dt.NewRow();
            DataRow dtrow3 = dt.NewRow();
            DataRow dtrow4 = dt.NewRow();
            DataRow dtrow5 = dt.NewRow();
            DataRow dtrow6 = dt.NewRow();
            DataRow dtrow7 = dt.NewRow();
            dtrow[0] = "1";
            dtrow[1] = "NO OF STUDENTS APPEARED";
            dtrow1[0] = "2";
            dtrow1[1] = "NO OF STUDENTS FAILED";
            dtrow2[0] = "3";
            dtrow2[1] = "NO OF STUDENTS PASSED IN ALL SUBJECTS";
            dtrow3[0] = "4";
            dtrow3[1] = "NO OF STUDENTS FAILED IN ONE SUBJECT";
            dtrow4[0] = "5";
            dtrow4[1] = "NO OF STUDENTS FAILED IN TWO SUBJECTS";
            dtrow5[0] = "6";
            dtrow5[1] = "NO OF STUDENTS FAILED IN 3 & ABOVE SUBJECT";
            dtrow6[0] = "7";
            dtrow6[1] = "PASS PERCENTAGE %";
            string degree_code = ddlbranch.SelectedValue.ToString();
            string batch_year = ddlbatch.SelectedValue.ToString();
            current_sem = ddlsem.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            //Get Exam Code
            ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(current_sem), Convert.ToInt32(batch_year));
            //end
            string str1 = "select COUNT(distinct m.roll_no) as appeared, r.degree_code from mark_entry m,";
            str1 += " Registration r   where  m.roll_no=r.Roll_No and m.exam_code=" + ExamCode + "  and ";
            str1 += " r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' ";
            str1 += " and r.degree_code='" + ddlbranch.SelectedItem.Value + "'  ";
            str1 += " and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result!='AAA' and m.roll_no ";
            str1 += " not in(select distinct m.roll_no from mark_entry m,Registration r   where ";
            str1 += " m.roll_no=r.Roll_No and ";
            str1 += " r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' ";
            str1 += " and r.degree_code='" + ddlbranch.SelectedItem.Value + "' and m.exam_code=" + ExamCode + " ";
            str1 += " and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result='AAA')";
            if (ddlSec.SelectedItem.Text != "ALL")
            {
                str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
            }
            str1 = str1 + " group by r.degree_code ";
            //  str1 = str1 + "select COUNT(distinct r.roll_no) as fail, r.degree_code from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='fail' and m.attempts=1 and m.roll_no not in(select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('AAA','WHD') and m.attempts=1 )";
            str1 = str1 + "select COUNT(distinct r.roll_no) as fail, r.degree_code from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','AAA','WHD') and m.attempts=1 ";
            if (ddlSec.SelectedItem.Text != "ALL")
            {
                str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
            }
            str1 = str1 + "  group by r.degree_code ";

            str1 = str1 + " select COUNT(distinct r.roll_no) as Pass, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no not in(select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and  r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )  ";

            //str1 = str1 + " select COUNT(distinct r.roll_no) as Pass, r.degree_code from mark_entry m, ";
            //str1 += " Registration r where  m.roll_no=r.Roll_No  ";
            //str1 += " and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "'   ";
            //str1 += " and r.degree_code='" + ddlbranch.SelectedItem.Value + "'";//added rajkumar
            //str1 += " and r.college_code='" + ddlcollege.SelectedItem.Value + "' and m.exam_code=" + ExamCode + " ";

            //str1 += " and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and ";
            //str1 += " m.roll_no not in(select distinct r.roll_no from mark_entry m,Registration r  ";
            //str1 += " where m.roll_no=r.Roll_No and ";
            //str1 += " r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "'";
            //str1 += " and r.degree_code='" + ddlbranch.SelectedItem.Value + "'";//added rajkumar
            //str1 += " and m.exam_code=" + ExamCode + " and r.cc=0 and  r.exam_flag <>'DEBAR' ";
            //str1 += " and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )  ";

            if (ddlSec.SelectedItem.Text != "ALL")
            {
                str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
            }
            str1 = str1 + " group by r.degree_code ";


            str1 = str1 + "select COUNT( r.roll_no) as count1,a.sex,r.Stud_Type from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedItem.Value + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','WHD') and m.attempts=1 ";
            if (ddlSec.SelectedItem.Text != "ALL")
            {
                str1 = str1 + "and r.Sections='" + ddlSec.SelectedItem.Text + "'";
            }
            str1 = str1 + " group by a.sex,r.Stud_Type ,m.roll_no ";
            ds1 = da.select_method_wo_parameter(str1, "Text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                dtrow[2] = ds1.Tables[0].Rows[0]["appeared"].ToString();
                dtrow1[2] = ds1.Tables[1].Rows[0]["Fail"].ToString();
                if (ds1.Tables[2].Rows.Count > 0)
                {
                    dtrow2[2] = ds1.Tables[2].Rows[0]["pass"].ToString();
                }
                else
                {
                    dtrow2[2] = "0";
                }
                DataView vd = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='1'";
                vd = ds1.Tables[3].DefaultView;
                dtrow3[2] = vd.Count;
                DataView spl = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Hostler') and  sex = ('1')";
                spl = ds1.Tables[3].DefaultView;
                dtrow3[4] = vd.Count;
                DataView sp2 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Day Scholar') and  sex = ('1')";
                sp2 = ds1.Tables[3].DefaultView;
                dtrow3[3] = vd.Count;
                DataView sp3 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Hostler') and  sex = ('0')";
                sp3 = ds1.Tables[3].DefaultView;
                dtrow3[6] = vd.Count;
                DataView sp4 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='1' and Stud_Type = ('Day Scholar') and  sex = ('0')";
                sp4 = ds1.Tables[3].DefaultView;
                dtrow3[5] = vd.Count;
                DataView vd1 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='2'";
                vd1 = ds1.Tables[3].DefaultView;
                dtrow4[2] = vd1.Count;
                DataView spl1 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Hostler') and  sex = ('1')";
                spl1 = ds1.Tables[3].DefaultView;
                dtrow4[4] = vd.Count;
                DataView sp21 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Day Scholar') and  sex = ('1')";
                sp21 = ds1.Tables[3].DefaultView;
                dtrow4[3] = vd.Count;
                DataView sp31 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Hostler') and  sex = ('0')";
                sp31 = ds1.Tables[3].DefaultView;
                dtrow4[6] = vd.Count;
                DataView sp41 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1='2' and Stud_Type = ('Day Scholar') and  sex = ('0')";
                sp41 = ds1.Tables[3].DefaultView;
                dtrow4[5] = vd.Count;
                DataView vd2 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2')";
                vd2 = ds1.Tables[3].DefaultView;
                dtrow5[2] = vd2.Count;
                DataView sp1l1 = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Hostler') and  sex = ('1')";
                sp1l1 = ds1.Tables[3].DefaultView;
                dtrow5[4] = vd.Count;
                DataView sp21k = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Day Scholar') and  sex = ('1')";
                sp21k = ds1.Tables[3].DefaultView;
                dtrow5[3] = vd.Count;
                DataView sp311k = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Hostler') and  sex = ('0')";
                sp311k = ds1.Tables[3].DefaultView;
                dtrow5[6] = vd.Count;
                DataView sp411k = new DataView();
                ds1.Tables[3].DefaultView.RowFilter = "count1 not in ('1','2') and Stud_Type = ('Day Scholar') and  sex = ('0')";
                sp411k = ds1.Tables[3].DefaultView;
                dtrow5[5] = vd.Count;

                string appeared = " select COUNT(distinct m.roll_no) as appeared, r.degree_code,r.Stud_Type,a.sex from mark_entry m,Exam_Details e,Registration r, applyn a   where  e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "'    and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "'  and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result!='AAA' and m.roll_no not in(select distinct m.roll_no from mark_entry m,Exam_Details e,Registration r   where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.degree_code='" + ddlbranch.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Value + "' and r.cc=0 and r.exam_flag <>'DEBAR'   and r.delflag=0 and m.attempts=1 and m.result='AAA') ";

                if (ddlSec.SelectedItem.Text != "ALL")
                {
                    appeared = appeared + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
                }
                appeared = appeared + "group by r.degree_code,r.Stud_Type,a.sex";
                ds = da.select_method_wo_parameter(appeared, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataView rsap = new DataView();
                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('1')";
                    rsap = ds.Tables[0].DefaultView;
                    if (rsap.Count > 0)
                    {
                        dtrow[3] = rsap[0]["appeared"].ToString();
                    }
                    else
                    {
                        dtrow[3] = "0";
                    }
                    DataView rsap1 = new DataView();
                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('0')";
                    rsap1 = ds.Tables[0].DefaultView;
                    if (rsap1.Count > 0)
                    {
                        dtrow[5] = rsap1[0]["appeared"].ToString();
                    }
                    else
                    {
                        dtrow[5] = "0";
                    }
                    DataView rsaps = new DataView();
                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('1')";
                    rsaps = ds.Tables[0].DefaultView;
                    if (rsaps.Count > 0)
                    {
                        dtrow[4] = rsaps[0]["appeared"].ToString();
                    }
                    else
                    {
                        dtrow[4] = "0";
                    }
                    DataView rsaps1 = new DataView();
                    ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('0')";
                    rsaps1 = ds.Tables[0].DefaultView;
                    if (rsaps1.Count > 0)
                    {
                        dtrow[6] = rsaps1[0]["appeared"].ToString();
                    }
                    else
                    {
                        dtrow[6] = "0";
                    }
                    string fail = "  select COUNT(distinct r.roll_no) as fail, r.degree_code,r.Stud_Type,a.sex from mark_entry m,Exam_Details e,Registration r,applyn a where e.exam_code=m.exam_code and r.App_No=a.app_no and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code='" + ddlcollege.SelectedValue + "' and e.degree_code='" + ddlbranch.SelectedValue + "' and  r.Batch_Year='" + ddlbatch.SelectedItem.Text + "' and e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in('fail','AAA','WHD') and m.attempts=1";
                    if (ddlSec.SelectedItem.Text != "ALL")
                    {
                        fail = fail + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
                    }
                    fail = fail + "group by r.degree_code,r.Stud_Type,a.sex ";
                    ds = da.select_method_wo_parameter(fail, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataView rs = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex = ('1')";
                        rs = ds.Tables[0].DefaultView;
                        if (rs.Count > 0)
                        {
                            dtrow1[3] = rs[0]["fail"].ToString();
                        }
                        else
                        {
                            dtrow1[3] = "0";
                        }
                        DataView rs1 = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('1')";
                        rs1 = ds.Tables[0].DefaultView;
                        if (rs1.Count > 0)
                        {
                            dtrow1[4] = rs1[0]["fail"].ToString();
                        }
                        else
                        {
                            dtrow1[4] = "0";
                        }
                        DataView rs12 = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex =('0')";
                        rs12 = ds.Tables[0].DefaultView;
                        if (rs12.Count > 0)
                        {
                            dtrow1[5] = rs12[0]["fail"].ToString();
                        }
                        else
                        {
                            dtrow1[5] = "0";
                        }
                        DataView rs13 = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex = ('0')";
                        rs13 = ds.Tables[0].DefaultView;
                        if (rs13.Count > 0)
                        {
                            dtrow1[6] = rs13[0]["fail"].ToString();
                        }
                        else
                        {
                            dtrow1[6] = "0";
                        }


                        //string pass = "select COUNT(distinct r.roll_no) as Pass, r.degree_code,a.sex,r.Stud_Type from mark_entry m, ";
                        //pass += " Registration r,applyn a  where  m.roll_no=r.Roll_No ";
                        //pass += " and r.App_No=a.app_no  and  r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' ";
                        //pass += " and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";
                        //pass += " and r.degree_code='" + ddlbranch.SelectedValue + "' ";//Rajkumar added
                        //pass += " and m.exam_code=" + ExamCode + " and r.cc=0 and    r.exam_flag <>'DEBAR' ";
                        //pass += " and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no ";
                        //pass += " not in(select distinct r.roll_no from mark_entry m,Registration r  ";
                        //pass += " where  m.roll_no=r.Roll_No and  ";
                        //pass += " r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";
                        //pass += " and r.degree_code='" + ddlbranch.SelectedValue + "' ";//Rajkumar added
                        //pass += " and m.exam_code=" + ExamCode + "  and r.cc=0 and  r.exam_flag <>'DEBAR' ";
                        //pass += " and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )   ";

                        string pass = "  select COUNT(distinct r.roll_no) as Pass, r.degree_code,a.sex,r.Stud_Type from applyn a, mark_entry m,Exam_Details e,Registration r where a.app_no=r.App_No and e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and e.degree_code='" + ddlbranch.SelectedValue + "'  and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no not in(select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and r.college_code='" + ddlcollege.SelectedItem.Value + "' and e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )";

                        if (ddlSec.SelectedItem.Text != "ALL")
                        {
                            pass = pass + " and r.Sections='" + ddlSec.SelectedItem.Text + "'";
                        }
                        pass = pass + "group by r.degree_code,a.sex,r.Stud_Type";

                        ds = da.select_method_wo_parameter(pass, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            DataView rs15 = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex in ('1')";
                            rs15 = ds.Tables[0].DefaultView;
                            if (rs15.Count > 0)
                            {
                                dtrow2[3] = rs15[0]["pass"].ToString();
                            }
                            else
                            {
                                dtrow2[3] = "0";
                            }
                            DataView rs16 = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex in ('1')";
                            rs16 = ds.Tables[0].DefaultView;
                            if (rs16.Count > 0)
                            {
                                dtrow2[4] = rs16[0]["pass"].ToString();
                            }
                            else
                            {
                                dtrow2[4] = "0";
                            }
                            DataView rs17 = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Day Scholar') and  sex in ('0')";
                            rs17 = ds.Tables[0].DefaultView;
                            if (rs17.Count > 0)
                            {
                                dtrow2[5] = rs17[0]["pass"].ToString();
                            }
                            else
                            {
                                dtrow2[5] = "0";
                            }
                            DataView rs18 = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "Stud_Type = ('Hostler') and  sex in ('0')";
                            rs18 = ds.Tables[0].DefaultView;
                            if (rs18.Count > 0)
                            {
                                dtrow2[6] = rs18[0]["pass"].ToString();
                            }
                            else
                            {
                                dtrow2[6] = "0";
                            }
                            double tot123 = Convert.ToDouble(dtrow[2]);
                            double totapp = Convert.ToDouble(dtrow2[2]);
                            if (tot123 > 0)
                            {
                                double totfi = totapp / tot123;
                                totfi = totfi * 100;
                                totfi = Math.Round(totfi, 2);
                                dtrow6[2] = totfi;
                            }
                            else
                            {
                                dtrow6[2] = "0";
                            }
                            double tot = Convert.ToDouble(dtrow[3]);
                            double totap = Convert.ToDouble(dtrow2[3]);
                            if (tot > 0)
                            {
                                double totfi = totap / tot;
                                totfi = totfi * 100;
                                totfi = Math.Round(totfi, 2);
                                dtrow6[3] = totfi;
                            }
                            else
                            {
                                dtrow6[3] = "0";
                            }
                            double totfinal1 = 0;
                            double tot1 = Convert.ToDouble(dtrow[4]);
                            double totapp1 = Convert.ToDouble(dtrow2[4]);
                            if (tot1 > 0)
                            {
                                totfinal1 = totapp1 / tot1;
                                totfinal1 = totfinal1 * 100;
                                totfinal1 = Math.Round(totfinal1, 2);
                                dtrow6[4] = totfinal1;
                            }
                            else
                            {
                                dtrow6[4] = "0";
                            }
                            double tot12 = Convert.ToDouble(dtrow[5]);
                            double totapp2 = Convert.ToDouble(dtrow2[5]);
                            if (tot12 > 0)
                            {
                                double totfinal2 = totapp2 / tot12;
                                totfinal2 = totfinal2 * 100;
                                totfinal2 = Math.Round(totfinal2, 2);
                                dtrow6[5] = totfinal2;
                            }
                            else
                            {
                                dtrow6[5] = "0";
                            }
                            double tot1234 = Convert.ToDouble(dtrow[6]);
                            double totapp3 = Convert.ToDouble(dtrow2[6]);
                            if (tot1234 > 0)
                            {
                                double totfinal3 = totapp3 / tot1234;
                                totfinal3 = totfinal3 * 100;
                                totfinal3 = Math.Round(totfinal3, 2);
                                dtrow6[6] = totfinal3;
                            }
                            else
                            {
                                dtrow6[6] = "0";
                            }
                        }
                        else
                        {
                            dtrow6[6] = "0";
                            dtrow6[5] = "0";
                            dtrow6[4] = "0";
                            dtrow6[3] = "0";
                            dtrow6[2] = "0";
                            dtrow2[6] = "0";
                            dtrow2[5] = "0";
                            dtrow2[4] = "0";
                            dtrow2[3] = "0";
                        }
                    }
                    dt.Rows.Add(dtrow);
                    dt.Rows.Add(dtrow1);
                    dt.Rows.Add(dtrow2);
                    dt.Rows.Add(dtrow3);
                    dt.Rows.Add(dtrow4);
                    dt.Rows.Add(dtrow5);
                    dt.Rows.Add(dtrow6);
                    grd.DataSource = dt;
                    grd.DataBind();
                    grd.Visible = true;
                    btnExcel1.Visible = true;
                    btnPrint1.Visible = true;
                }
            }
            else
            {
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
                btnExcel1.Visible = false;
                btnPrint1.Visible = false;
                Chart1.Visible = false;
                grd.Visible = false;
                flow.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Label5.Text = ex.Message;
            Label5.Visible = true;
        }
    }

    protected void grd_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell2 = new TableCell();
            HeaderCell2.Text = string.Empty;
            HeaderCell2.ColumnSpan = 3;
            HeaderCell2.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell2);
            grd.Controls[0].Controls.AddAt(0, HeaderGridRow);
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "GIRLS";
            HeaderCell.ColumnSpan = 2;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            grd.Controls[0].Controls.AddAt(0, HeaderGridRow);
            TableCell HeaderCell1 = new TableCell();
            HeaderCell1.Text = "BOYS";
            HeaderCell1.ColumnSpan = 2;
            HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell1);
            grd.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            if (rbconsolidatesubject.SelectedValue == "1")
            {
                lastdiv.Style.Add("display", "none");
                dvconsolidated.Style.Add("display", "block");
                dvsubjectwise.Style.Add("display", "none");
                grdrow();
                grd2();
                grd3();
            }
            else if (rbconsolidatesubject.SelectedValue == "2")
            {
                dvconsolidated.Style.Add("display", "none");
                dvsubjectwise.Style.Add("display", "block");
                lastdiv.Style.Add("display", "none");
                Fpspread1.Visible = false;
                Printcontrol.Visible = false;
                txtexcelname.Text = string.Empty;
                if (rbformat.SelectedValue == "1")
                {
                    bindbranchwiseunivresultanalysis_format1();
                }
                else
                {
                    bindbranchwiseunivresultanalysis_format2();
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            bindsem();
            bindsec();
            grd.Visible = false;
            Totalgrd.Visible = false;
            btnExcel1.Visible = false;
            btnPrint1.Visible = false;
            staffgvd.Visible = false;
            txtTop.Text = string.Empty;
            flow.Visible = false;
            Chart1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            grd.Visible = false;
            Totalgrd.Visible = false;
            btnExcel1.Visible = false;
            btnPrint1.Visible = false;
            staffgvd.Visible = false;
            txtTop.Text = string.Empty;
            flow.Visible = false;
            Chart1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void Totalgrd_OnPreRender(object sender, EventArgs e)
    {
        foreach (GridViewRow row in Totalgrd.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StaffAnalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            if (staffgvd.Rows.Count > 0)
            {
                staffgvd.AllowPaging = false;
                staffgvd.HeaderRow.Style.Add("width", "15%");
                staffgvd.HeaderRow.Style.Add("font-size", "10px");
                staffgvd.HeaderRow.Style.Add("text-align", "center");
                staffgvd.Style.Add("font-family", "Bood Antiqua;");
                staffgvd.Style.Add("font-size", "10px");
                staffgvd.RenderControl(hw);
                staffgvd.DataBind();
            }
            string rk = "select Staff_Name from Semester_Schedule S,staffmaster M WHERE S.class_advisor  = M.staff_code AND degree_code = '" + ddlbranch.SelectedValue + "' AND batch_year = '" + ddlbatch.SelectedValue + "' AND Semester ='" + ddlsem.SelectedValue + "' AND LastRec = 1";
            ds = da.select_method_wo_parameter(rk, "text");
            string dha = string.Empty;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dha = ds.Tables[0].Rows[0]["Staff_Name"].ToString();
            }
            string staff = string.Empty;
            if (dha != "")
            {
                staff = dha;
            }
            Label lb = new Label();
            lb.Text = "<br/><br/>" + "CLASS ADVISOR" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + staff + "<br/><br/><br/><br/><br/><br/>" + "HOD" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "PRINCIPAL" + "";
            lb.Style.Add("height", "1000px");
            lb.Style.Add("text-decoration", "none");
            lb.Style.Add("font-family", "Book Antiqua");
            lb.Style.Add("font-size", "10px");
            lb.Style.Add("text-align", "left");
            lb.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void txtTop_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string val = txtTop.Text;
            if (val != "0" && val != "00")
            {
                Label5.Visible = false;
            }
            else
            {
                Label5.Text = "Please Enter value Greater than Zero";
                Label5.Visible = true;
                grd.Visible = false;
                Totalgrd.Visible = false;
                btnExcel1.Visible = false;
                btnPrint1.Visible = false;
                staffgvd.Visible = false;
                flow.Visible = false;
                Chart1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            grd.Visible = false;
            Totalgrd.Visible = false;
            btnExcel1.Visible = false;
            btnPrint1.Visible = false;
            staffgvd.Visible = false;
            txtTop.Text = string.Empty;
            flow.Visible = false;
            Chart1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsec();
            grd.Visible = false;
            Totalgrd.Visible = false;
            btnExcel1.Visible = false;
            btnPrint1.Visible = false;
            staffgvd.Visible = false;
            txtTop.Text = string.Empty;
            flow.Visible = false;
            Chart1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnExcel1_click(object sender, EventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition",
                "attachment;filename=OverAllTop.xls");
            Response.ContentType = "applicatio/excel";
            StringWriter sw = new StringWriter(); ;
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            grd.RenderControl(htm);
            staffgvd.RenderControl(htm);
            Totalgrd.RenderControl(htm);
            Response.Write(sw.ToString());
            Response.End();
            Response.Clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void btnPrint1_click(object sender, EventArgs e)
    {
        try
        {
            string degree_code = ddlbranch.SelectedValue.ToString();
            string batch_year = ddlbatch.SelectedValue.ToString();
            current_sem = ddlsem.SelectedValue.ToString();
            string branch = ddlbranch.SelectedItem.ToString();
            if (ddlSec.SelectedItem.Text != "ALL")
            {
                sections = "&nbsp;-&nbsp;" + ddlSec.SelectedValue.ToString();
            }
            else
            {
                sections = string.Empty;
            }
            string degreedetails = "Degree:&nbsp;" + batch_year + "&nbsp;-&nbsp;" + ddldegree.SelectedItem.ToString() + "&nbsp;-&nbsp;" + branch + "&nbsp;-&nbsp;" + current_sem + sections + " ";
            grd3();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=OverAllTop.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();
            string collegename = string.Empty;
            DataSet dscol = d2.select_method_wo_parameter("Select collname,address1,address2,address3,category,university from Collinfo where college_code='" + collegecode + "' ", "Text");
            if (dscol.Tables[0].Rows.Count > 0)
            {
                lb.Text = dscol.Tables[0].Rows[0]["collname"].ToString() + "<br> ";
                lb.Style.Add("height", "100px");
                lb.Style.Add("text-decoration", "none");
                lb.Style.Add("font-family", "Book Antiqua;");
                lb.Style.Add("font-size", "18px");
                lb.Style.Add("text-align", "center");
                lb.RenderControl(hw);
                string address = string.Empty;
                if (dscol.Tables[0].Rows[0]["address1"].ToString().Trim() != "")
                {
                    address = dscol.Tables[0].Rows[0]["address1"].ToString();
                }
                if (dscol.Tables[0].Rows[0]["address2"].ToString().Trim() != "")
                {
                    if (address == "")
                    {
                        address = dscol.Tables[0].Rows[0]["address2"].ToString();
                    }
                    else
                    {
                        address = address + ", " + dscol.Tables[0].Rows[0]["address2"].ToString();
                    }
                }
                if (dscol.Tables[0].Rows[0]["address3"].ToString().Trim() != "")
                {
                    if (address == "")
                    {
                        address = dscol.Tables[0].Rows[0]["address3"].ToString();
                    }
                    else
                    {
                        address = address + ", " + dscol.Tables[0].Rows[0]["address3"].ToString();
                    }
                }
                if (address.Trim() != "")
                {
                    lb.Text = address + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "12px");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(hw);
                }
                address = string.Empty;
                if (dscol.Tables[0].Rows[0]["category"].ToString().Trim() != "")
                {
                    address = dscol.Tables[0].Rows[0]["category"].ToString();
                }
                if (dscol.Tables[0].Rows[0]["university"].ToString().Trim() != "")
                {
                    if (address == "")
                    {
                        address = dscol.Tables[0].Rows[0]["university"].ToString();
                    }
                    else
                    {
                        address = address + " by " + dscol.Tables[0].Rows[0]["university"].ToString();
                    }
                }
                if (address.Trim() != "")
                {
                    lb.Text = address + "<br> ";
                    lb.Style.Add("height", "100px");
                    lb.Style.Add("text-decoration", "none");
                    lb.Style.Add("font-family", "Book Antiqua;");
                    lb.Style.Add("font-size", "12px");
                    lb.Style.Add("text-align", "center");
                    lb.RenderControl(hw);
                }
            }
            lb.Text = "University Result Analysis";
            lb.Style.Add("height", "100px");
            lb.Style.Add("text-decoration", "none");
            lb.Style.Add("font-family", "Book Antiqua;");
            lb.Style.Add("font-size", "14px");
            lb.Style.Add("text-align", "center");
            lb.RenderControl(hw);
            Label lb1 = new Label();
            lb1.Text = "<br> Consolidated Report";
            lb1.Style.Add("height", "100px");
            lb1.Style.Add("text-decoration", "none");
            lb1.Style.Add("font-family", "Book Antiqua;");
            lb1.Style.Add("font-size", "12px");
            lb1.Style.Add("text-align", "center");
            lb1.RenderControl(hw);
            Label lb2 = new Label();
            lb2.Text = "<br>" + degreedetails;
            lb2.Style.Add("height", "100px");
            lb2.Style.Add("text-decoration", "none");
            lb2.Style.Add("font-family", "Book Antiqua;");
            lb2.Style.Add("font-size", "10px");
            lb2.Style.Add("text-align", "left");
            lb2.RenderControl(hw);
            Label lb3 = new Label();
            lb3.Text = "<br><br>";
            lb3.Style.Add("height", "200px");
            lb3.Style.Add("text-decoration", "none");
            lb3.Style.Add("font-family", "Book Antiqua;");
            lb3.Style.Add("font-size", "10px");
            lb3.Style.Add("text-align", "left");
            lb3.RenderControl(hw);
            if (grd.Rows.Count > 0)
            {
                grd.AllowPaging = false;
                grd.HeaderRow.Style.Add("width", "15%");
                grd.HeaderRow.Style.Add("font-size", "8px");
                grd.HeaderRow.Style.Add("text-align", "center");
                grd.Style.Add("font-family", "Bood Antiqua;");
                grd.Style.Add("font-size", "6px");
                grd.RenderControl(hw);
                grd.DataBind();
            }
            StringWriter sw1 = new StringWriter();
            HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
            if (Totalgrd.Rows.Count > 0)
            {
                Label lb5 = new Label();
                lb5.Text = "<br>";
                lb5.Style.Add("height", "200px");
                lb5.Style.Add("text-decoration", "none");
                lb5.Style.Add("font-family", "Book Antiqua;");
                lb5.Style.Add("font-size", "10px");
                lb5.Style.Add("text-align", "left");
                lb5.RenderControl(hw1);
                Totalgrd.AllowPaging = false;
                Totalgrd.HeaderRow.Style.Add("width", "15%");
                Totalgrd.HeaderRow.Style.Add("font-size", "8px");
                Totalgrd.HeaderRow.Style.Add("text-align", "center");
                Totalgrd.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                Totalgrd.Style.Add("font-size", "6px");
                Totalgrd.RenderControl(hw1);
                Totalgrd.DataBind();
            }
            StringWriter sw2 = new StringWriter();
            HtmlTextWriter hw2 = new HtmlTextWriter(sw2);
            if (staffgvd.Rows.Count > 0)
            {
                Label lb6 = new Label();
                lb6.Text = "<br>";
                lb6.Style.Add("height", "200px");
                lb6.Style.Add("text-decoration", "none");
                lb6.Style.Add("font-family", "Book Antiqua;");
                lb6.Style.Add("font-size", "10px");
                lb6.Style.Add("text-align", "left");
                lb6.RenderControl(hw2);
                staffgvd.AllowPaging = false;
                staffgvd.HeaderRow.Style.Add("width", "15%");
                staffgvd.HeaderRow.Style.Add("font-size", "8px");
                staffgvd.HeaderRow.Style.Add("text-align", "center");
                staffgvd.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                staffgvd.Style.Add("font-size", "6px");
                staffgvd.RenderControl(hw2);
                staffgvd.DataBind();
            }
            Label lb4 = new Label();
            if (Chart1.Visible == true)
            {
                lb4.Text = "<br>STAFF PERFORMANCE RESULT ANALYSIS CHART";
                lb4.Style.Add("height", "100px");
                lb4.Style.Add("text-decoration", "none");
                lb4.Style.Add("font-family", "Book Antiqua;");
                lb4.Style.Add("font-size", "8px");
                lb4.Style.Add("font-weight", "bold");
                lb4.Style.Add("text-align", "center");
                lb4.RenderControl(hw2);
            }
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                string getpath = HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg").ToString();
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(getpath);
                jpg.ScaleToFit(60f, 40f);
                jpg.Alignment = Element.ALIGN_LEFT;
                jpg.IndentationLeft = 9f;
                jpg.SpacingAfter = 9f;
                pdfDoc.Add(jpg);
            }
            StringReader sr = new StringReader(sw.ToString() + sw1.ToString() + sw2.ToString());
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);
            if (Chart1.Visible == true)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Chart1.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }
            //StringWriter swf = new StringWriter();
            //HtmlTextWriter hwf = new HtmlTextWriter(swf);
            //lb4.Text = "<br>HOD";
            //lb4.Style.Add("height", "100px");
            //lb4.Style.Add("text-decoration", "none");
            //lb4.Style.Add("font-family", "Book Antiqua;");
            //lb4.Style.Add("font-size", "8px");
            //lb4.Style.Add("font-weight", "bold");
            //lb4.Style.Add("text-align", "center");
            //lb4.RenderControl(hwf);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            Label5.Text = ex.ToString();
            Label5.Visible = true;
        }
    }

    protected void Totalgrd_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "TOPPER LIST";
            HeaderCell.ColumnSpan = 6;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            Totalgrd.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void staffgvd_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "STAFF PERFORMANCE RESULT ANALYSIS";
            HeaderCell.ColumnSpan = 5;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            staffgvd.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    public void Calulat_GPA(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        try
        {
            string ccva = string.Empty;
            string strgrade = string.Empty;
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            double gpacal1 = 0;
            string strsubcrd = string.Empty;
            string examcodeval = string.Empty;
            double strtot = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strtotgrac = string.Empty;
            string strgradetempgrade = string.Empty;
            DataSet dggradetot = new DataSet();
            try
            {
                dggradetot.Dispose();
                string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
                dggradetot = da.select_method_wo_parameter(strsqlstaffname, "Text");
            }
            catch (SqlException qle)
            {
                throw qle;
            }
            examcodeval = da.GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
            ccva = da.GetFunction("select cc from registration where roll_no='" + RollNo + "'");
            if (ccva == "False")
            {
                strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
            }
            else if (ccva == "True")
            {
                strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
            }
            if (strsubcrd != "" && strsubcrd != null)
            {
                DataSet dssubcrd = da.select_method_wo_parameter(strsubcrd, "Text");
                if (dssubcrd.Tables[0].Rows.Count > 0)
                {
                    for (int sr = 0; sr < dssubcrd.Tables[0].Rows.Count; sr++)
                    {
                        if ((dssubcrd.Tables[0].Rows[sr]["total"].ToString() != string.Empty) && (dssubcrd.Tables[0].Rows[sr]["total"].ToString() != "0"))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dssubcrd.Tables[0].Rows[sr]["total"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                        if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dssubcrd.Tables[0].Rows[sr]["grade"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dssubcrd.Tables[0].Rows[sr]["grade"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        if (strgrade != "" && strgrade != null)
                        {
                            if (dssubcrd.Tables[0].Rows[sr]["credit_points"].ToString() != null && dssubcrd.Tables[0].Rows[sr]["credit_points"].ToString() != "")
                            {
                                creditval = Convert.ToDouble(dssubcrd.Tables[0].Rows[sr]["credit_points"].ToString());
                                if (creditsum1 == 0)
                                {
                                    creditsum1 = Convert.ToDouble(dssubcrd.Tables[0].Rows[sr]["credit_points"].ToString());
                                }
                                else
                                {
                                    creditsum1 = creditsum1 + Convert.ToDouble(dssubcrd.Tables[0].Rows[sr]["credit_points"].ToString());
                                }
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
            }
            if (creditsum1 != 0)
            {
                finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            }
            gpa = finalgpa1.ToString();
            creitpoint = creditsum1.ToString();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsec();
            grd.Visible = false;
            Totalgrd.Visible = false;
            btnExcel1.Visible = false;
            btnPrint1.Visible = false;
            staffgvd.Visible = false;
            txtTop.Text = string.Empty;
            flow.Visible = false;
            Chart1.Visible = false;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch)
    {
        string GetUnivExamCode = string.Empty;
        string strExam_code = string.Empty;
        strExam_code = d2.GetFunction("Select Exam_Code from Exam_Details where Degree_Code = " + DegreeCode.ToString() + " and Current_Semester = " + Semester.ToString() + " and Batch_Year = " + Batch.ToString() + "");
        if (strExam_code.Trim() != "" && strExam_code != null)
        {
            GetUnivExamCode = strExam_code;
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

    //public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch)
    //{
    //    string GetUnivExamCode = string.Empty;
    //    string degree_code = string.Empty;
    //    string current_sem = string.Empty;
    //    string batch_year = string.Empty;
    //    string strExam_code = string.Empty;
    //    //Added By Malang Raja
    //    //string qryExamCode = "Select Exam_Code from Exam_Details where Degree_Code ='" + DegreeCode.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Exam_year='" + ddlYear.SelectedValue.ToString() + "' and Batch_Year ='" + Batch.ToString() + "' and current_semester='" + Semester + "'";
    //    //DataSet dsExamCodeNew = new DataSet();
    //    //dsExamCodeNew = d2.select_method_wo_parameter(qryExamCode, "text");
    //    strExam_code = "Select Exam_Code from Exam_Details where Degree_Code ='" + DegreeCode.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Exam_year='" + ddlYear.SelectedValue.ToString() + "' and Batch_Year ='" + Batch.ToString() + "'";
    //    DataSet ds = new DataSet();
    //    ds = d2.select_method_wo_parameter(strExam_code, "text");
    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //        foreach (DataRow dr_examcode in ds.Tables[0].Rows)
    //        {
    //            string examCode = Convert.ToString(dr_examcode["Exam_Code"]).Trim();
    //            if (!string.IsNullOrEmpty(examCode))
    //            {
    //                GetUnivExamCode = examCode;
    //            }
    //        }
    //    }
    //    if (!string.IsNullOrEmpty(GetUnivExamCode))
    //    {
    //        return Convert.ToInt32(GetUnivExamCode);
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}

    protected void rbbeforeandafterrevaluation_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvconsolidated.Style.Add("display", "none");
        dvsubjectwise.Style.Add("display", "none");
        lastdiv.Style.Add("display", "none");
    }

    protected void rbconsolidatesubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbconsolidatesubject.SelectedValue == "1")
            {
                dvconsolidated.Style.Add("display", "none");
                dvsubjectwise.Style.Add("display", "none");
                lastdiv.Style.Add("display", "none");
                txtTop.Enabled = true;
                txtTop.Text = string.Empty;
                rbbeforeandafterrevaluation.Enabled = false;
                rbformat.Enabled = false;
                rbmoderation.Enabled = false;
            }
            else if (rbconsolidatesubject.SelectedValue == "2")
            {
                btnPrint1.Visible = false;
                btnExcel1.Visible = false;
                txtTop.Text = string.Empty;
                dvconsolidated.Style.Add("display", "none");
                dvsubjectwise.Style.Add("display", "none");
                lastdiv.Style.Add("display", "none");
                txtTop.Enabled = false;
                rbbeforeandafterrevaluation.Enabled = true;
                rbformat.Enabled = true;
                rbmoderation.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void rbformat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            dvconsolidated.Style.Add("display", "none");
            dvsubjectwise.Style.Add("display", "none");
            lastdiv.Style.Add("display", "none");
            if (rbformat.SelectedValue == "1")
            {
                rbmoderation.Visible = false;
                rbbeforeandafterrevaluation.Visible = true;
            }
            else if (rbformat.SelectedValue == "2")
            {
                rbmoderation.Enabled = true;
                rbmoderation.Visible = true;
                rbbeforeandafterrevaluation.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindbranchwiseunivresultanalysis_format1()
    {
        try
        {
            Fpspread1.Width = 980;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.Sheets[0].ColumnHeader.Visible = true;
            Fpspread1.Sheets[0].AutoPostBack = false;
            txtexcelname.Text = string.Empty;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 1;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 3;
            Fpspread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#339999");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            darkstyle.Border.BorderColor = ColorTranslator.FromHtml("#FFFFFF");
            darkstyle.Border.BorderSize = 1;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread1.ActiveSheetView.Columns.Default.Border.BorderColor = System.Drawing.Color.Black;
            Fpspread1.Sheets[0].AutoPostBack = true;
            string tempsubtype = string.Empty;
            int startrow = 0;
            int spanrow = 0;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 150;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = " ";
            string degree_code = ddlbranch.SelectedValue.ToString();
            string batch_year = ddlbatch.SelectedValue.ToString();
            current_sem = ddlsem.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            //Get Exam Code
            ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(current_sem), Convert.ToInt32(batch_year));
            //end
            string qryDiscontinue = string.Empty;
            if (!chkIncludeDiscontinue.Checked)
            {
                qryDiscontinue = " and r.cc='0' and r.Exam_Flag<>'debar' and r.delFlag='0'";
            }
            string qrySections = string.Empty;
            if (ddlSec.Enabled == true)
            {
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                {
                    qrySections = " and r.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                }
                else
                {
                    qrySections = string.Empty;
                }
            }
            else
            {
                qrySections = string.Empty;
            }
            strsubject = "Select distinct subject.mintotal as mintot,subject.mintotal as mintot,subject.min_int_marks as mimark, subject.min_ext_marks as mxmark,subject.maxtotal as maxtot,subject.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points,sub_sem.lab as chlab,subject.subtype_no as typeno from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " and attempts=1  order by semester desc,subject.subtype_no  asc";
            DataSet dssubjectload = d2.select_method_wo_parameter(strsubject, "text");
            //if (dssubjectload.Tables[0].Rows.Count > 0)
            if (dssubjectload.Tables.Count > 0 && dssubjectload.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Visible = true;
                lastdiv.Style.Add("display", "block");
                for (int s = 0; s < dssubjectload.Tables[0].Rows.Count; s++)
                {
                    Fpspread1.Sheets[0].ColumnCount += 1;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Text = dssubjectload.Tables[0].Rows[s]["Subtype"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Tag = dssubjectload.Tables[0].Rows[s]["Subject_No"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 3, Fpspread1.Sheets[0].ColumnCount - 1].Note = dssubjectload.Tables[0].Rows[s]["mintot"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Text = dssubjectload.Tables[0].Rows[s]["Subject_Code"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Tag = dssubjectload.Tables[0].Rows[s]["mintot"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Note = dssubjectload.Tables[0].Rows[s]["mimark"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = dssubjectload.Tables[0].Rows[s]["subacr"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Tag = dssubjectload.Tables[0].Rows[s]["maxtot"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Note = dssubjectload.Tables[0].Rows[s]["mxmark"].ToString();
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 2, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    if (tempsubtype != dssubjectload.Tables[0].Rows[s]["Subtype"].ToString() || s == dssubjectload.Tables[0].Rows.Count - 1)
                    {
                        if (tempsubtype != "")
                        {
                            if (s == dssubjectload.Tables[0].Rows.Count - 1)
                            {
                                spanrow++;
                            }
                            Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, startrow, 1, spanrow);
                        }
                        startrow = Fpspread1.Sheets[0].ColumnCount - 1;
                        spanrow = 1;
                    }
                    else
                    {
                        spanrow++;
                    }
                    tempsubtype = dssubjectload.Tables[0].Rows[s]["Subtype"].ToString();
                }
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF PASSES";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF FAILURES";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO APPEARED";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL NO OF ABSENTEES";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "PERCENTAGE OF PASS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "NO OF 1ST CLASS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "PERCENTAGE OF 1ST CLASS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "OVERALL PASS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Rows.Count++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "OVERALL PASS %";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //  storedprocedure universityprocbranchwiseresult Start
                string semester = ddlsem.SelectedValue.ToString();
                double mintotal = 0;
                double minintmarks = 0;
                double minextmarks = 0;
                int getgradeflag = 0;
                double gminintmark = 0;
                double gmaxintmark = 0;
                int subnum = 0;
                string spsection = string.Empty;
                string beforeorafter = string.Empty;
                string exammonth = string.Empty;
                string examyear = string.Empty;
                DataView dvbgrade = new DataView();
                DataView dvfailcnt = new DataView();
                ArrayList asubcode = new ArrayList();
                Hashtable htstaffdetails = new Hashtable();
                // Get Exam-Month and Exam-Year
                string getmonthyear = " select Exam_year,Exam_month from Exam_Details where batch_year =" + batch_year + " and degree_code=" + degree_code + " and current_semester=" + current_sem + "";
                DataSet dsmonthyear = new DataSet();
                dsmonthyear = d2.select_method_wo_parameter(getmonthyear, "text");
                exammonth = dsmonthyear.Tables[0].Rows[0]["Exam_month"].ToString();
                examyear = dsmonthyear.Tables[0].Rows[0]["Exam_year"].ToString();
                //Grade Flag
                string getgrade = d2.GetFunction("select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exammonth + " and exam_year= " + examyear + " ");
                if (getgrade.Trim() != "" && getgrade != null)
                {
                    getgradeflag = Convert.ToInt32(getgrade);
                }
                if (ddlSec.Enabled == false)
                {
                    spsection = string.Empty;
                }
                else
                {
                    //if (ddlSec.SelectedItem.Text == "ALL")
                    //{
                    //    spsection = string.Empty;
                    //}
                    //else 
                    spsection = string.Empty;
                    if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                    {
                        spsection = ddlSec.SelectedItem.Text.Trim();
                    }
                }
                //Get Grade
                //int bindgraderownumber = 0;
                //int v = 0;
                //string grade = "select Mark_Grade from Grade_Master where degree_code=" + degree_code + " and batch_year='" + batch_year + "'order by Frange desc";
                //ds.Clear();
                //ds = d2.select_method(grade, hat, "Text");
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    //Fpspread1.Sheets[0].RowCount++;
                //    if (v == 0)
                //    {
                //        bindgraderownumber = Fpspread1.Sheets[0].RowCount - 1;
                //    }
                //    v++;
                //    for (int g = 0; g < ds.Tables[0].Rows.Count; g++)
                //    {
                //        Fpspread1.Sheets[0].RowCount++;
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Number of " + " ' " + ds.Tables[0].Rows[g]["Mark_Grade"].ToString() + " ' " + "grade";
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[g]["Mark_Grade"].ToString();
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Small;              
                //        alv.Add(ds.Tables[0].Rows[g]["Mark_Grade"].ToString());
                //    }
                //}
                int tempoverallscount = 0;
                string subno = string.Empty;
                for (int column = 1; column < Fpspread1.Sheets[0].Columns.Count; column++)
                {
                    //Get Tag and Note
                    tempoverallscount++;
                    mintotal = Convert.ToDouble(Fpspread1.Sheets[0].ColumnHeader.Cells[0, column].Note.ToString());
                    minintmarks = Convert.ToDouble(Fpspread1.Sheets[0].ColumnHeader.Cells[1, column].Note.ToString());
                    minextmarks = Convert.ToDouble(Fpspread1.Sheets[0].ColumnHeader.Cells[2, column].Note.ToString());
                    subnum = Convert.ToInt32(Fpspread1.Sheets[0].ColumnHeader.Cells[0, column].Tag.ToString());
                    string sco = Fpspread1.Sheets[0].ColumnHeader.Cells[1, column].Text;
                    Boolean passflag = false;
                    string subjectnumber = string.Empty;
                    if (column == 1)
                    {
                        gminintmark = minintmarks;
                        gmaxintmark = minextmarks;
                    }
                    if (subnum != 0)
                    {
                        subno = subno + "," + Convert.ToInt32(subnum).ToString();
                        if (column == Fpspread1.Sheets[0].Columns.Count - 1)
                        {
                            passflag = true;
                            subjectnumber = subno.Remove(0, 1);
                        }
                    }
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        beforeorafter = "1";
                    }
                    else
                    {
                        beforeorafter = string.Empty;
                    }
                    DataSet studinfoads = new DataSet();
                    hashmark.Clear();
                    hashmark.Add("degreecode", degree_code);
                    hashmark.Add("batchyear", batch_year);
                    hashmark.Add("semester", current_sem);
                    hashmark.Add("subject_no", subnum);
                    hashmark.Add("examcode", ExamCode);
                    hashmark.Add("gradetype", getgradeflag);
                    hashmark.Add("sections", spsection);
                    hashmark.Add("mintotal", mintotal - 1);
                    hashmark.Add("minintmark", gminintmark);
                    hashmark.Add("minextmark", gmaxintmark);
                    hashmark.Add("beforeorafter", beforeorafter);
                    studinfoads = d2.select_method("universityprocbranchwiseresult", hashmark, "sp");
                    if (studinfoads.Tables.Count > 0 && studinfoads.Tables[0].Rows.Count > 0)
                    {
                        string studentappeared = string.Empty;
                        string studentpassed = string.Empty;
                        string studentfail = string.Empty;
                        string totalstudents = string.Empty;
                        string registeredstud = string.Empty;
                        string absentstud = string.Empty;
                        string passpercent = string.Empty;
                        string firstclasspercent = string.Empty;
                        int firstclasscount = 0;
                        string allpassper = "0";
                        for (int studproci = 0; studproci < studinfoads.Tables[0].Rows.Count; studproci++)
                        {
                            string pss = string.Empty;
                            totalstudents = studinfoads.Tables[0].Rows[studproci][0].ToString();
                            studentappeared = studinfoads.Tables[1].Rows[studproci][0].ToString();
                            //No Of Pass
                            if (Convert.ToInt32(getgradeflag) == 2)//Grade
                            {
                                string actulgradeorgrade = string.Empty;
                                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                                {
                                    actulgradeorgrade = "m.Actual_Grade";
                                }
                                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                                {
                                    actulgradeorgrade = "m.grade";
                                }
                                if (spsection != "")
                                {
                                    pss = d2.GetFunction("select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and  " + actulgradeorgrade + "     not in (select mark_grade from grade_master where  frange<" + mintotal + " and degree_code=" + degree_code + "     and batch_year=" + batch_year + ")  and r.Sections='" + spsection + "' and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result<>'pass' and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " and r.Sections='" + spsection + "')    ");
                                }
                                else
                                {
                                    pss = d2.GetFunction("select count(result) as pass from mark_entry m,registration r where m.roll_no=r.roll_no   and m.attempts>=1 and subject_no =  " + subnum + " and exam_code =" + ExamCode + " and " + actulgradeorgrade + "      not in (select mark_grade from grade_master where frange<" + mintotal + " and   degree_code=" + degree_code + "     and batch_year=" + batch_year + ")and r.Roll_No not in(  select r.roll_no from mark_entry m, registration r where m.roll_no=r.roll_no and result<>'pass' and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " )   ");
                                }
                            }
                            else if (Convert.ToInt32(getgradeflag) == 3)//Mark
                            {
                                // Mark System - Testing                               
                                string actualinternal = string.Empty;
                                string actualexternal = string.Empty;
                                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                                {
                                    actualinternal = "actual_internal_mark";
                                    actualexternal = "actual_external_mark";
                                }
                                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                                {
                                    actualinternal = "internal_mark";
                                    actualexternal = "external_mark";
                                }
                                if (spsection != "")
                                {
                                    pss = d2.GetFunction("select COUNT(distinct m.roll_no) as pass,subject_no from mark_entry m,Registration r where m.roll_no=r.roll_no and  r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + " and r.Sections='" + spsection + "'  and m.attempts>=1  and exam_code=" + ExamCode + " and subject_no =  " + subnum + " and " + actualexternal + " >=  " + gmaxintmark + "  and " + actualinternal + " >= " + gminintmark + " and result='pass' group by subject_no");
                                }
                                else
                                {
                                    pss = d2.GetFunction("select COUNT(distinct m.roll_no) as pass,subject_no from mark_entry m,Registration r where m.roll_no=r.roll_no and  r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + "   and m.attempts>=1  and exam_code=" + ExamCode + " and subject_no =  " + subnum + " and " + actualexternal + " >=  " + gmaxintmark + "  and " + actualinternal + " >= " + gminintmark + " and result='pass' group by subject_no");
                                }
                            }
                            if (pss.Trim() != "" && pss != null)
                            {
                                studentpassed = pss;
                            }
                            else
                            {
                                studentpassed = "0";
                            }
                            //End
                            //No Of Fail
                            if (spsection != "")
                            {
                                string Fail = d2.GetFunction("select count(result) as failcount from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('Fail','WHD','AAA') and r.Sections='" + spsection + "'  and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ");
                                if (Fail.Trim() != "" && Fail != null)
                                {
                                    studentfail = Fail;
                                }
                                else
                                {
                                    studentfail = "0";
                                }
                            }
                            else
                            {
                                string Fail = d2.GetFunction("select count(result) as failcount from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('Fail','WHD','AAA') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ");
                                if (Fail.Trim() != "" && Fail != null)
                                {
                                    studentfail = Fail;
                                }
                                else
                                {
                                    studentfail = "0";
                                }
                            }
                            //End
                            //Total No Students
                            registeredstud = studinfoads.Tables[4].Rows[studproci][0].ToString();
                            //End
                            //No of Absentees
                            if (spsection != "")
                            {
                                string abs = d2.GetFunction("select count(result) as absc from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA') and r.Sections='" + spsection + "'  and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ");
                                if (abs.Trim() != "" && abs != null)
                                {
                                    absentstud = abs;
                                }
                                else
                                {
                                    absentstud = "0";
                                }
                            }
                            else
                            {
                                string abs = d2.GetFunction("select count(result) as absc from mark_entry m, registration r where m.roll_no=r.roll_no and result in ('AAA','UA') and passorfail=0 and subject_no = " + subnum + " and  exam_code =" + ExamCode + " ");
                                if (abs.Trim() != "" && abs != null)
                                {
                                    absentstud = abs;
                                }
                                else
                                {
                                    absentstud = "0";
                                }
                            }
                            //End
                            int total = Convert.ToInt32(studentpassed) + Convert.ToInt32(studentfail);
                            //No of First class
                            if (ddlSec.Enabled == false)
                            {
                                spsection = string.Empty;
                            }
                            else
                            {
                                if (ddlSec.SelectedItem.Text == "ALL")
                                {
                                    spsection = string.Empty;
                                }
                                else
                                {
                                    spsection = ddlSec.SelectedItem.Text;
                                }
                            }
                            string fclasscnt = string.Empty;
                            if (Convert.ToInt32(getgradeflag) == 2)//grade
                            {
                                string actualgrade = string.Empty;
                                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                                {
                                    actualgrade = "m.Actual_Grade";
                                }
                                else
                                {
                                    actualgrade = "m.grade";
                                }
                                if (spsection != "")
                                {
                                    fclasscnt = d2.GetFunction("select  COUNT (grade) as firstclass,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no =m.subject_no and m.exam_code=" + ExamCode + " and m.roll_no=r.Roll_No	and r.Sections='" + spsection + "' and " + actualgrade + " in(select g.Mark_Grade from grade_master g where degree_code=" + degree_code + " and batch_year=" + batch_year + " and " + actualgrade + " =g.Mark_Grade and Frange>=60 and s.subject_no=" + subnum + ") group by m.subject_no");
                                }
                                else
                                {
                                    fclasscnt = d2.GetFunction("select  COUNT (grade) as firstclass from mark_entry m,syllabus_master sm,subject s where sm.syll_code=s.syll_code  and s.subject_no =m.subject_no and m.exam_code='" + ExamCode + "' and " + actualgrade + " in(select g.Mark_Grade from grade_master g where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and " + actualgrade + "=g.Mark_Grade and Frange>=60 and s.subject_no='" + subnum + "') ");
                                }
                            }
                            else if (Convert.ToInt32(getgradeflag) == 3)//Mark
                            {
                                string actualtotal = string.Empty;
                                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                                {
                                    actualtotal = "actual_total";
                                }
                                else
                                {
                                    actualtotal = "total";
                                }
                                if (spsection != "")
                                {
                                    fclasscnt = d2.GetFunction("select COUNT(distinct m.roll_no) as pass from mark_entry m,Registration r where m.roll_no=r.roll_no and  r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + " and r.Sections='" + spsection + "'   and m.attempts>=1  and exam_code=" + ExamCode + " and subject_no =  " + subnum + " and " + actualtotal + " >=  60  and result='pass' group by subject_no");
                                }
                                else
                                {
                                    fclasscnt = d2.GetFunction("select COUNT(distinct m.roll_no) as pass from mark_entry m,Registration r where m.roll_no=r.roll_no and  r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + "  and m.attempts>=1  and exam_code=" + ExamCode + " and subject_no =  " + subnum + " and " + actualtotal + " >=  60  and result='pass' group by subject_no");
                                }
                            }
                            //End
                            //Percentage of Pass 
                            string firstclass = "0";
                            if (fclasscnt.Trim() != "" && fclasscnt != null)
                            {
                                firstclass = fclasscnt;
                            }
                            if (fclasscnt != "")
                            {
                                firstclasscount = Convert.ToInt16(firstclass);
                            }
                            else
                            {
                                firstclasscount = 0;
                            }
                            if (studentpassed != "0")
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(studentpassed) / Convert.ToDouble(total)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                passpercent = Convert.ToString(passpercent2);
                            }
                            else
                            {
                                passpercent = "0";
                            }
                            //End
                            //Percentage of First Class
                            if (firstclasscount != 0)
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(firstclasscount) / Convert.ToDouble(total)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                string passpercent2_infi = String.Format("{0:0,0.00}", float.Parse(passpercent2.ToString()));
                                if (passpercent2_infi == "NaN")
                                {
                                    passpercent2 = 0;
                                }
                                else if (passpercent2_infi == "Infinity")
                                {
                                    passpercent2 = 0;
                                }
                                firstclasspercent = Convert.ToString(passpercent2);
                            }
                            else
                            {
                                firstclasspercent = "0";
                            }
                            //End
                        }
                        int row = 0;
                        Fpspread1.Sheets[0].Cells[row, column].Text = studentpassed.ToString();
                        Fpspread1.Sheets[0].Cells[row, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 1, column].Text = studentfail.ToString();
                        Fpspread1.Sheets[0].Cells[row + 1, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 1, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row + 1, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 2, column].Text = studentappeared.ToString();
                        Fpspread1.Sheets[0].Cells[row + 2, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row + 2, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 2, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 3, column].Text = absentstud.ToString();
                        Fpspread1.Sheets[0].Cells[row + 3, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row + 3, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 3, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 4, column].Text = passpercent.ToString();
                        Fpspread1.Sheets[0].Cells[row + 4, column].Font.Size = FontUnit.Medium;
                        htstaffdetails.Add(subnum, passpercent);
                        Fpspread1.Sheets[0].Cells[row + 4, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 4, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 5, column].Text = firstclasscount.ToString();
                        Fpspread1.Sheets[0].Cells[row + 5, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row + 5, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 5, column].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[row + 6, column].Text = firstclasspercent.ToString();
                        Fpspread1.Sheets[0].Cells[row + 6, column].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[row + 6, column].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[row + 6, column].Font.Name = "Book Antiqua";
                        //Overall Pass
                        if (passflag == true)
                        {
                            passflag = false;
                            string passcount = string.Empty;
                            if (spsection != "")
                            {
                                passcount = d2.GetFunction("select count(distinct m.roll_no) as overallpass from mark_entry m,registration r where  m.roll_no=r.roll_no and m.exam_code=" + ExamCode + " and m.attempts<=1 and r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + " and r.Sections='" + spsection + "' and m.subject_no in(" + subjectnumber + ") and m.roll_no  not in(select m1.roll_no from mark_entry m1,Registration r1 where m1.exam_code=" + ExamCode + " and r1.Roll_No=m1.roll_no and r1.degree_code=" + degree_code + " and r1.Batch_Year=" + batch_year + " and r1.Sections='" + spsection + "'  and m1.attempts<=1 and result<>'pass' and m1.subject_no in(" + subjectnumber + "))");
                            }
                            else
                            {
                                passcount = d2.GetFunction("select count(distinct m.roll_no) as overallpass from mark_entry m,registration r where  m.roll_no=r.roll_no and m.exam_code=" + ExamCode + " and m.attempts<=1 and r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + "  and m.subject_no in(" + subjectnumber + ") and m.roll_no  not in(select m1.roll_no from mark_entry m1,Registration r1 where m1.exam_code=" + ExamCode + " and r1.Roll_No=m1.roll_no and r1.degree_code=" + degree_code + " and r1.Batch_Year=" + batch_year + "   and m1.attempts<=1 and result<>'pass' and m1.subject_no in(" + subjectnumber + "))");
                            }
                            if (passcount.Trim() != "" && passcount != null)
                            {
                                allpasscount = Convert.ToInt32(passcount);
                            }
                            else
                            {
                                allpasscount = 0;
                            }
                            //End
                            //Overall Appear
                            string allappear = string.Empty;
                            if (spsection != "")
                            {
                                allappear = d2.GetFunction("select count(distinct m.roll_no) as allappear from mark_entry m,registration r where m.roll_no=r.roll_no and m.exam_code=" + ExamCode + " and m.attempts<=1 and r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + " and r.Sections='" + spsection + "' and  m.subject_no in(" + subjectnumber + ") and m.roll_no  not in(select m1.roll_no from mark_entry m1,Registration r1 where m1.exam_code=" + ExamCode + " and r1.Roll_No=m1.roll_no and r1.degree_code=" + degree_code + " and r1.Batch_Year=" + batch_year + " and r1.Sections='" + spsection + "' and m1.attempts<=1 and result='AAA' and m1.subject_no in(" + subjectnumber + "))");
                            }
                            else
                            {
                                allappear = d2.GetFunction("select count(distinct m.roll_no) as allappear from mark_entry m,registration r where m.roll_no=r.roll_no and m.exam_code=" + ExamCode + " and m.attempts<=1 and r.degree_code=" + degree_code + " and r.Batch_Year=" + batch_year + "  and  m.subject_no in(" + subjectnumber + ") and m.roll_no  not in(select m1.roll_no from mark_entry m1,Registration r1 where m1.exam_code=" + ExamCode + " and r1.Roll_No=m1.roll_no and r1.degree_code=" + degree_code + " and r1.Batch_Year=" + batch_year + "  and m1.attempts<=1 and result='AAA' and m1.subject_no in(" + subjectnumber + "))");
                            }
                            if (allappear.Trim() != "" && allappear != null)
                            {
                                allappeared = Convert.ToInt32(allappear);
                            }
                            else
                            {
                                allappeared = 0;
                            }
                            //End
                            //Percentage of overall
                            if (allpasscount != 0)
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(allpasscount) / Convert.ToDouble(allappeared)) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                string passpercent2_infi = String.Format("{0:0,0.00}", float.Parse(passpercent2.ToString()));
                                if (passpercent2_infi == "NaN")
                                {
                                    passpercent2 = 0;
                                }
                                else if (passpercent2_infi == "Infinity")
                                {
                                    passpercent2 = 0;
                                }
                                allpassper = Convert.ToString(passpercent2);
                            }
                            //End
                            Fpspread1.Sheets[0].Cells[row + 7, 1].Text = allpasscount.ToString();
                            Fpspread1.Sheets[0].SpanModel.Add(row + 7, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                            Fpspread1.Sheets[0].Cells[row + 7, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 7, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[row + 7, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[row + 8, 1].Text = allpassper;
                            Fpspread1.Sheets[0].SpanModel.Add(row + 8, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                            Fpspread1.Sheets[0].Cells[row + 8, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[row + 8, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[row + 8, 1].Font.Name = "Book Antiqua";
                        }
                    }
                    // Bind Grade
                    //    string marksql =string.Empty;
                    //    string dsmark =string.Empty;
                    //    DataTable dgrades = new DataTable();
                    //    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    //    {
                    //        marksql = "Select mark_entry.Actual_Grade,mark_entry.subject_no as subn,COUNT(roll_no) as cnt from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = '" + ExamCode + "'   group by  mark_entry.Actual_Grade,mark_entry.subject_no";
                    //        dsmark = "Actual_Grade";
                    //    }
                    //    else
                    //    {
                    //        marksql = "Select mark_entry.grade,mark_entry.subject_no as subn,COUNT(roll_no) as cnt from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = '" + ExamCode + "'   group by  mark_entry.grade,mark_entry.subject_no";
                    //        dsmark = "grade";
                    //    }
                    //    DataSet dscheckresult = d2.select_method(marksql, hat, "Text");
                    //    dgrades = dscheckresult.Tables[0];
                    //    if (alv.Count > 0)
                    //    {
                    //        for (int cv = 0; cv < alv.Count; cv++)
                    //        {
                    //            string cgrade = dscheckresult.Tables[0].Rows[column - 1][dsmark].ToString();
                    //            string subjectno = dscheckresult.Tables[0].Rows[column - 1]["subn"].ToString();
                    //            string gradecount = dscheckresult.Tables[0].Rows[column - 1]["cnt"].ToString();
                    //            dgrades.DefaultView.RowFilter = "subn='" + subnum + "' and " + dsmark + " ='" + alv[cv].ToString() + "' ";
                    //            dvbgrade = dgrades.DefaultView;
                    //            if (dvbgrade.Count > 0)
                    //            {
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Text = Convert.ToString(dvbgrade[0]["cnt"]);
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].HorizontalAlign = HorizontalAlign.Center;
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Font.Size = FontUnit.Medium;
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Font.Name = "Book Antiqua";
                    //            }
                    //            else
                    //            {
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Text = "0";
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].HorizontalAlign = HorizontalAlign.Center;
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Font.Size = FontUnit.Medium;
                    //                Fpspread1.Sheets[0].Cells[bindgraderownumber + cv + 1, column].Font.Name = "Book Antiqua";
                    //            }
                    //        }
                    //    }
                }
                // NO OF FAILED SUBJECT COUNT
                Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 2;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].Text = "NO OF FAILED SUBJECT COUNT";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "NO OF STUDENTS FAILED";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                string failcount = " select roll_no,count(subject_no) as fail from mark_entry where exam_code='" + ExamCode + "' and result='fail' group by roll_no order by fail";
                DataSet dsfailcnt = d2.select_method(failcount, hat, "Text");
                int colval = 0;
                Hashtable hashfailcount = new Hashtable();
                for (int subtag = 0; subtag < Fpspread1.Sheets[0].ColumnHeader.Columns.Count - 1; subtag++)
                {
                    colval++;
                    string failsubject = Fpspread1.Sheets[0].ColumnHeader.Cells[0, subtag + 1].Tag.ToString();
                    dsfailcnt.Tables[0].DefaultView.RowFilter = "fail='" + colval + "' ";
                    dvfailcnt = dsfailcnt.Tables[0].DefaultView;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, subtag + 1].Text = colval.ToString();
                    if (dvfailcnt.Count > 0)
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, subtag + 1].Text = dvfailcnt.Count.ToString();
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, subtag + 1].Text = "0";
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, subtag + 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, subtag + 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 2, subtag + 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, subtag + 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, subtag + 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, subtag + 1].Font.Name = "Book Antiqua";
                }
                int d = Fpspread1.Sheets[0].RowCount;
                //Bind Staff Details
                //Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                //Fpspread1.Sheets[0].SpanModel.Add(d, 0, 1, Fpspread1.Sheets[0].ColumnCount);
                //string bindstaffdetails =string.Empty;
                //if (ddlSec.SelectedItem.Text == "ALL" || ddlSec.Enabled == false)
                //{
                //    //bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,staffmaster sm where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and st.sections=r.sections and semester in('" + ddlSemYr.SelectedItem.Text + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and sy.degree_code='" + degree_code + "' and sm.staff_code=st.staff_code  order by st.batch_year,sy.degree_code ,s.subject_no,semester,st.sections";
                //    bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,staffmaster sm,sub_sem sb where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subType_no=sb.subType_no and sy.syll_code=sb.syll_code and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + degree_code + "' and sy.semester in ('" + semester + "') and sb.promote_count=1 order by s.subject_no";
                //}
                //else
                //{
                //    //bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,registration r,staffmaster sm where r.degree_code=sy.degree_code and r.batch_year=sy.batch_year and r.current_semester=sy.semester and s.syll_code=sy.syll_code and st.subject_no=s.subject_no  and st.batch_year=sy.batch_year and st.sections=r.sections and semester in('" + ddlSemYr.SelectedItem.Text + "') and r.cc=0 and delflag=0 and exam_flag<>'Debar' and sy.degree_code='" + degree_code + "' and sm.staff_code=st.staff_code and st.sections='" + ddlSec.SelectedItem.Text + "'  order by st.batch_year,sy.degree_code ,s.subject_no,semester,st.sections";
                //    bindstaffdetails = "select distinct st.staff_code,sm.staff_name, s.subject_no,s.acronym,s.subject_code,s.subject_name,s.syll_code,st.batch_year,semester,st.sections,sy.degree_code from subject s,syllabus_master sy,staff_selector st,staffmaster sm,sub_sem sb where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subType_no=sb.subType_no and sy.syll_code=sb.syll_code and sy.Batch_Year=st.batch_year and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + degree_code + "' and sy.semester in ('" + semester + "') and st.sections='" + spsection + "' and sb.promote_count=1 order by s.subject_no";
                //}
                //DataSet dsstaff = d2.select_method(bindstaffdetails, hat, "Text");
                //int ntcount = 0;
                //if (dsstaff.Tables[0].Rows.Count > 0)
                //{
                //    int sprcount = Fpspread1.Sheets[0].RowCount;
                //    Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                //    //Fpspread1.Sheets[0].ColumnCount++;
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "SubCode";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "SubName";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "Staff Name";
                //    Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 2, 1, 3);
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "%";
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                //    //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 8, 1, 2);   
                //    int spanstaffnamerow = Fpspread1.Sheets[0].RowCount - 2;
                //    for (int col = 0; col < dsstaff.Tables[0].Rows.Count; col++)
                //    {
                //        ntcount++;
                //        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                //        string subcode = dsstaff.Tables[0].Rows[col]["subject_code"].ToString();
                //        string subname = dsstaff.Tables[0].Rows[col]["acronym"].ToString();
                //        string staffname = dsstaff.Tables[0].Rows[col]["staff_name"].ToString();
                //        string subnumber = dsstaff.Tables[0].Rows[col]["subject_no"].ToString();
                //        string passperc =string.Empty;
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = subcode.ToString();
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = subname.ToString();
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = staffname.ToString();
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                //        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 2, 1, 3);
                //        int subnohash = Convert.ToInt32(subnumber);
                //        if (htstaffdetails.ContainsKey(subnohash))
                //        {
                //            passperc = Convert.ToString(htstaffdetails[subnohash]);
                //        }
                //        else
                //        {
                //            passperc = "0";
                //        }
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(passperc);
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                //        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                //    }
                //}
                //            
                if (dssubjectload.Tables.Count > 0 && dssubjectload.Tables[0].Rows.Count < 2)
                {
                    Fpspread1.Sheets[0].ColumnCount++;
                    Fpspread1.Sheets[0].ColumnCount++;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                    for (int sp = 0; sp < Fpspread1.Sheets[0].RowCount; sp++)
                    {
                        Fpspread1.Sheets[0].SpanModel.Add(sp, 1, 1, Fpspread1.Sheets[0].ColumnCount - 1);
                    }
                }
                else if (dssubjectload.Tables.Count > 0 && dssubjectload.Tables[0].Rows.Count < 3)
                {
                    Fpspread1.Sheets[0].ColumnCount++;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 1);
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                    Fpspread1.Sheets[0].SpanModel.Add(0, 3, Fpspread1.Sheets[0].RowCount, 1);
                }
                Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                int startrowcnt = Fpspread1.Sheets[0].RowCount - 1;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = "No Of Students";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].BackColor = ColorTranslator.FromHtml("#339999");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = "No Of Pass";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].BackColor = ColorTranslator.FromHtml("#339999");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = "% Of Pass";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].BackColor = ColorTranslator.FromHtml("#339999");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                string seattypequery = string.Empty;
                //if (ddlSec.Enabled == true)
                if (spsection != "")
                {
                    seattypequery = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no and r.sections='" + ddlSec.SelectedItem.Text + "' and a.seattype=tv.textcode";
                }
                else
                {
                    seattypequery = "select distinct seattype,tv.textval as textval from applyn a,registration r,textvaltable tv where r.degree_code='" + degree_code + "' and r.batch_year='" + batch_year + "' and r.app_no=a.app_no  and a.seattype=tv.textcode";
                }
                DataSet dsseat = d2.select_method(seattypequery, hat, "Text");
                int colcount = 0;
                int dc = colcount - 1;
                int ccountcheck = 0;
                int checkrcou = 0;
                checkrcou = Fpspread1.Sheets[0].RowCount - 1;
                if (dsseat.Tables.Count > 0 && dsseat.Tables[0].Rows.Count > 0)
                {
                    for (int t = 0; t < dsseat.Tables[0].Rows.Count; t++)
                    {
                        ccountcheck++;
                        Fpspread1.Sheets[0].RowCount = Fpspread1.Sheets[0].RowCount + 1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Tag = dsseat.Tables[0].Rows[t]["seattype"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = dsseat.Tables[0].Rows[t]["textval"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                    }
                }
                dc = dc + ccountcheck;
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = "DAYSCHOLAR";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = "HOSTLER";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = "BOYS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Text = "GIRLS";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].ForeColor = ColorTranslator.FromHtml("#ffffff");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].BackColor = ColorTranslator.FromHtml("#4DA6A6");
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, colcount].Border.BorderColor = ColorTranslator.FromHtml("#ffffff");
                for (int man = 0; man < ccountcheck + 1; man++)
                {
                    checkrcou++;
                    string quotaval = string.Empty;
                    string dayscholar = "Day Scholar";
                    string hostler = "Hostler";
                    string girls = "1";
                    string boys = "0";
                    string beforeorafter1 = string.Empty;
                    if (rbbeforeandafterrevaluation.SelectedValue == "1")
                    {
                        beforeorafter1 = "1";
                    }
                    else
                    {
                        beforeorafter1 = string.Empty;
                    }
                    if (ddlSec.Enabled == false)
                    {
                        spsection = string.Empty;
                    }
                    else
                    {
                        //if (ddlSec.SelectedItem.Text == "ALL")
                        //{
                        //    spsection = string.Empty;
                        //}
                        //else
                        spsection = string.Empty;
                        if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                        {
                            spsection = ddlSec.SelectedItem.Text;
                        }
                    }
                    if (man <= dc)
                    {
                        quotaval = Fpspread1.Sheets[0].Cells[checkrcou, 0].Tag.ToString();
                    }
                    else
                    {
                        quotaval = string.Empty;
                    }
                    hashmark.Clear();
                    DataSet dsstudinfodata = new DataSet();
                    hashmark.Add("degreecode", degree_code);
                    hashmark.Add("batchyear", batch_year);
                    hashmark.Add("semester", current_sem);
                    hashmark.Add("examcode", ExamCode);
                    hashmark.Add("sections", spsection);
                    hashmark.Add("dayscholar", dayscholar);
                    hashmark.Add("hostler", hostler);
                    hashmark.Add("quota", quotaval);
                    hashmark.Add("girls", girls);
                    hashmark.Add("boys", boys);
                    hashmark.Add("mintot", mintotal - 1);
                    hashmark.Add("beforeorafter", beforeorafter1);
                    hashmark.Add("markorgrade", getgradeflag);
                    hashmark.Add("minintmark", gminintmark);
                    hashmark.Add("minextmark", gmaxintmark);
                    dsstudinfodata = d2.select_method("universitymarkresultanalysis", hashmark, "sp");
                    if (dsstudinfodata.Tables.Count > 0 && dsstudinfodata.Tables[0].Rows.Count > 0)
                    {
                        string allpassper = string.Empty;
                        if (quotaval != "")
                        {
                            int totcount = Convert.ToInt16(dsstudinfodata.Tables[0].Rows[0][0].ToString());
                            int totpasscount = Convert.ToInt16(dsstudinfodata.Tables[1].Rows[0][0].ToString());
                            double passpercent1 = 0;
                            passpercent1 = Convert.ToDouble((Convert.ToDouble(totpasscount) / totcount) * 100);
                            double passpercent2 = Math.Round(passpercent1, 2);
                            allpassper = Convert.ToString(passpercent2);
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Text = totcount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Text = totpasscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Text = allpassper.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Font.Name = "Book Antiqua";
                        }
                        else
                        {
                            int dayscount = 0;
                            int dayscholarpasscount = 0;
                            string dayscholarperc = string.Empty;
                            int hostlercount = 0;
                            int hostlerpasscount = 0;
                            string hostlerperc = string.Empty;
                            int girlscount = 0;
                            int girlspasscount = 0;
                            string girlsperc = string.Empty;
                            int boyscount = 0;
                            int boyspasscount = 0;
                            string boysperc = string.Empty;
                            dayscount = Convert.ToInt16(dsstudinfodata.Tables[0].Rows[0][0].ToString());
                            dayscholarpasscount = Convert.ToInt16(dsstudinfodata.Tables[1].Rows[0][0].ToString());
                            if (dayscount != 0)
                            {
                                double passpercent1 = 0;
                                passpercent1 = Convert.ToDouble((Convert.ToDouble(dayscholarpasscount) / dayscount) * 100);
                                double passpercent2 = Math.Round(passpercent1, 2);
                                dayscholarperc = Convert.ToString(passpercent2);
                            }
                            else
                            {
                                dayscholarperc = "0";
                            }
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Text = dayscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Text = dayscholarpasscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Text = dayscholarperc.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou, 3].Font.Name = "Book Antiqua";
                            hostlercount = Convert.ToInt16(dsstudinfodata.Tables[2].Rows[0][0].ToString());
                            hostlerpasscount = Convert.ToInt16(dsstudinfodata.Tables[3].Rows[0][0].ToString());
                            double passpercenthos = 0;
                            if (hostlercount != 0)
                            {
                                passpercenthos = Convert.ToDouble((Convert.ToDouble(hostlerpasscount) / hostlercount) * 100);
                                double passpercent2hos = Math.Round(passpercenthos, 2);
                                hostlerperc = Convert.ToString(passpercent2hos);
                            }
                            else
                            {
                                hostlerperc = "0";
                            }
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Text = hostlercount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Text = hostlerpasscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Text = hostlerperc.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Name = "Book Antiqua";
                            boyscount = Convert.ToInt16(dsstudinfodata.Tables[6].Rows[0][0].ToString());
                            boyspasscount = Convert.ToInt16(dsstudinfodata.Tables[7].Rows[0][0].ToString());
                            if (boyscount != 0)
                            {
                                double passpercentboys = 0;
                                passpercentboys = Convert.ToDouble((Convert.ToDouble(boyspasscount) / boyscount) * 100);
                                double passpercent2boys = Math.Round(passpercentboys, 2);
                                boysperc = Convert.ToString(passpercent2boys);
                            }
                            else
                            {
                                boysperc = "0";
                            }
                            checkrcou++;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Text = boyscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Text = boyspasscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Text = boysperc.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Name = "Book Antiqua";
                            girlscount = Convert.ToInt16(dsstudinfodata.Tables[4].Rows[0][0].ToString());
                            girlspasscount = Convert.ToInt16(dsstudinfodata.Tables[5].Rows[0][0].ToString());
                            if (girlscount != 0)
                            {
                                double passpercentgirls = 0;
                                passpercentgirls = Convert.ToDouble((Convert.ToDouble(girlspasscount) / girlscount) * 100);
                                double passpercent2gir = Math.Round(passpercentgirls, 2);
                                girlsperc = Convert.ToString(passpercent2gir);
                            }
                            else
                            {
                                girlsperc = "0";
                            }
                            checkrcou++;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Text = girlscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Text = girlspasscount.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Text = girlsperc.ToString();
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[checkrcou + 1, 3].Font.Name = "Book Antiqua";
                        }
                    }
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
            }
            else
            {
                Fpspread1.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    public void bindbranchwiseunivresultanalysis_format2()
    {
        try
        {
            Fpspread1.Visible = true;
            Fpspread1.Width = 980;
            Fpspread1.CommandBar.Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.Sheets[0].ColumnHeader.Visible = true;
            Fpspread1.Sheets[0].AutoPostBack = false;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 0;
            Fpspread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#339999");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            darkstyle.Border.BorderColor = ColorTranslator.FromHtml("#FFFFFF");
            darkstyle.Border.BorderSize = 1;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.Font.Bold = true;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            Fpspread1.ActiveSheetView.Columns.Default.Border.BorderColor = System.Drawing.Color.Black;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].ColumnHeader.RowCount++;
           
            Fpspread1.Sheets[0].ColumnCount++;
            bool visfalg = false;
            for (int c = 0; c < chklscolumn.Items.Count; c++)
            {
                if (chklscolumn.Items[c].Selected == true)
                {
                    visfalg = true;
                }
            }
           

            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "S.No";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 30;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            
            //**added by Mullai
            if (chklscolumn.Items[0].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "STAFF NAME";
                Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 150;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }
            if (chklscolumn.Items[1].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "DESIGNATION";
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 120;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                
            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }
            //**
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "SUB CODE";
            Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);          
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 120;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "SUBJECT NAME";
            Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 250;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            if (chklscolumn.Items[2].Selected == true)
            {
                //added by rajasekar 19/07/2018
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "BRANCH";
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 150;                
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }

            if (chklscolumn.Items[3].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "SEM & SEC.";
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 100;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }

           
            if (chklscolumn.Items[4].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "HIGHEST MARKS";
                Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 100;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }

            if (chklscolumn.Items[5].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "LOWEST MARKS";
                Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 100;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }

            if (chklscolumn.Items[6].Selected == true)
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "CLASS AVERAGE";
                Fpspread1.Sheets[0].SetColumnMerge(Fpspread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 100;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }
            //===================================//
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "APPLIED";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "APPEARED";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "PASSED";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "FAILED";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            Fpspread1.Sheets[0].ColumnCount++;
            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "PASS %";
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
            Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            if (chklscolumn.Items[7].Selected == true)
            {
                //added by rajasekar 19/07/2018
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = "FAIL %";
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Width = 80;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            }
            else
            {
                Fpspread1.Sheets[0].ColumnCount++;
                Fpspread1.Sheets[0].Columns[Fpspread1.Sheets[0].ColumnCount - 1].Visible = false;

            }
            //===========================//


            



            
            string degree_code = ddlbranch.SelectedValue.ToString();
            string batch_year = ddlbatch.SelectedValue.ToString();
            current_sem = ddlsem.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            //Get Exam Code
            ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(current_sem), Convert.ToInt32(batch_year));
            double mintotal = 0;
            int subnum = 0;
            string spsection = string.Empty;
            string qryDiscontinue = string.Empty;
            if (!chkIncludeDiscontinue.Checked)
            {
                qryDiscontinue = " and r.cc='0' and r.Exam_Flag<>'debar' and r.delFlag='0'";
            }
            string qrySections = string.Empty;
            string stsection = string.Empty;
            string qryRedoSection = string.Empty;
            if (ddlSec.Enabled == true)
            {
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                {
                    qrySections = " and r.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    qryRedoSection = " and sr.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    stsection = " and st.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                }
                else
                {
                    qrySections = string.Empty;
                    qryRedoSection = string.Empty;
                    stsection = string.Empty;
                }
            }
            else
            {
                qrySections = string.Empty;
                qryRedoSection = string.Empty;
                stsection = string.Empty;
            }
            //Modified By Malang Raja T On Jan 9 2017
            //strsubject = "Select distinct subject.mintotal as mintot,subject.mintotal as mintot,subject.min_int_marks as mimark, subject.min_ext_marks as mxmark,subject.maxtotal as maxtot,subject.acronym as subacr,subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points,sub_sem.lab as chlab,subject.subtype_no as typeno from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code ='" + ExamCode + "' and syllabus_master.semester='" + current_sem + "' order by semester desc,subject.subtype_no  asc";
            strsubject = "Select distinct s.mintotal as mintot,s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym as subacr,s.subject_name,s.subject_code as Subject_Code,m.subject_no as Subject_No,sm.semester,ss.subject_type as Subtype,credit_points,ss.lab as chlab,s.subtype_no as typeno from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,Registration r where r.Roll_No=m.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and  sm.syll_code=s.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and m.Exam_Code ='" + ExamCode + "' and sm.semester='" + current_sem + "' " + qrySections + qryDiscontinue + " ";
            string orderBy = " order by sm.semester desc,s.subtype_no asc";
            string subjectListRedo = "Select distinct s.mintotal as mintot,s.mintotal as mintot,s.min_int_marks as mimark, s.min_ext_marks as mxmark,s.maxtotal as maxtot,s.acronym as subacr,s.subject_name,s.subject_code as Subject_Code,m.subject_no as Subject_No,sm.semester,ss.subject_type as Subtype,credit_points,ss.lab as chlab,s.subtype_no as typeno from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and r.Roll_No=m.roll_no and sr.BatchYear=sm.Batch_Year and sm.degree_code=sr.DegreeCode and  sm.syll_code=s.syll_code and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and m.Exam_Code ='" + ExamCode + "' and sm.semester='" + current_sem + "' " + qryRedoSection + qryDiscontinue + "";
            strsubject += "union " + subjectListRedo + orderBy;
            DataSet dssubjectload = d2.select_method_wo_parameter(strsubject, "text");
            int slno = 0;
            int column = 0;
            if (dssubjectload.Tables.Count > 0 && dssubjectload.Tables[0].Rows.Count > 0)
            {
                string passpercent = string.Empty;
                string failpercent = string.Empty;
                double gminintmark = 0;
                double gminextmark = 0;
                int dvpassview = 0;
                int dvfailview = 0;
                int dvappearview = 0;
                string subcode = "0";
                ArrayList arraypass = new ArrayList();
                ArrayList arrayfail = new ArrayList();
                ArrayList subnnumtable = new ArrayList();
                ArrayList arrayappear = new ArrayList();
                ArrayList arraybeforepass = new ArrayList();
                DataView dvstudpass = new DataView();
                DataView dvstudfail = new DataView();
                DataView dvappear = new DataView();
                Hashtable hatpass = new Hashtable();
                Hashtable hatfail = new Hashtable();
                Fpspread1.Visible = true;
                lastdiv.Style.Add("display", "block");
                if (ddlSec.Enabled == false)
                {
                    spsection = string.Empty;
                }
                else
                {
                    if (ddlSec.SelectedItem.Text.Trim().ToLower() == "all" || ddlSec.SelectedItem.Text.Trim().ToLower() == "" || ddlSec.SelectedItem.Text.Trim().ToLower() == "-1")
                    {
                        spsection = string.Empty;
                    }
                    else
                    {
                        spsection = ddlSec.SelectedItem.Text;
                    }
                }

                bool staffSelector = false;
                string staffselsettings = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='"+Convert.ToString(Session["collegecode"])+"'");
                string[] splitstaffselsettings = staffselsettings.Split('-');
                if (splitstaffselsettings.Length == 2)
                {
                    int batchyearsetting = 0;
                    int.TryParse(Convert.ToString(splitstaffselsettings[1]).Trim(), out batchyearsetting);
                    if (splitstaffselsettings[0].ToString() == "1")
                    {
                        if (Convert.ToInt32(batch_year.ToString()) >= batchyearsetting)
                        {
                            staffSelector = true;
                        }
                    }
                }
                string staffname = string.Empty;
                for (int s = 0; s < dssubjectload.Tables[0].Rows.Count; s++)
                {
                    slno++;
                    if (staffSelector == false)
                    {
                        staffname = "select distinct s.subject_code,s.subject_name,s.subject_no,st.Sections,st.staff_code,sm.staff_name,sam.desig_name,sam.desig_code from staff_selector st,subject s,staffmaster sm,staff_appl_master sam where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subject_no ='" + Convert.ToString(dssubjectload.Tables[0].Rows[s]["Subject_No"]).Trim() + "' " + stsection + " and sam.appl_no=sm.appl_no group by s.subject_code,s.subject_name,s.subject_no,st.Sections,st.staff_code,sm.staff_name,sam.desig_name,sam.desig_code ";//order by s.subtype_no asc
                    }
                    else
                    {
                        staffname = "select distinct s.subject_code,s.subject_name,s.subject_no,st.Sections,sc.staffcode,sm.staff_name,sam.desig_name,sam.desig_code from staff_selector st,subject s,staffmaster sm,staff_appl_master sam,subjectChooser sc where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subject_no ='" + Convert.ToString(dssubjectload.Tables[0].Rows[s]["Subject_No"]).Trim() + "' " + stsection + " and sam.appl_no=sm.appl_no and sc.staffcode=sm.staff_code and st.staff_code=sc.staffcode and s.subject_no=sc.subject_no and st.subject_no=sc.subject_no group by s.subject_code,s.subject_name,s.subject_no,st.Sections,sc.staffcode,sm.staff_name,sam.desig_name,sam.desig_code ";//order by s.subtype_no asc

                    }

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(staffname, "text");
                    string stname=string.Empty;
                    string subname = string.Empty;
                    string subcod = string.Empty;
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables.Count > 0)
                    {
                        for (int s1 = 0; s1 < ds.Tables[0].Rows.Count; s1++)
                        {
                            string staf_name = Convert.ToString(ds.Tables[0].Rows[s1]["staff_name"]).Trim();
                            string staffdesigcode = Convert.ToString(ds.Tables[0].Rows[s1]["desig_code"]).Trim();
                            string designame = "select desig_name from desig_master where desig_code='"+staffdesigcode+"'";
                            DataSet designam = new DataSet();
                            string staffdesig = string.Empty;
                            designam = d2.select_method_wo_parameter(designame, "text");
                            if (designam.Tables[0].Rows.Count > 0 && designam.Tables.Count > 0)
                            {
                                staffdesig = designam.Tables[0].Rows[0]["desig_name"].ToString();
                            }

                         
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Text = slno.ToString();                                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 1].Text = Convert.ToString(staf_name);                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 2].Text = Convert.ToString(staffdesig);                                                       Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Tag = dssubjectload.Tables[0].Rows[s]["mimark"].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Note = dssubjectload.Tables[0].Rows[s]["mxmark"].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Text = dssubjectload.Tables[0].Rows[s]["subject_code"].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Tag = dssubjectload.Tables[0].Rows[s]["Subject_No"].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Note = dssubjectload.Tables[0].Rows[s]["mintot"].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 4].Text = dssubjectload.Tables[0].Rows[s]["subject_name"].ToString();
                           
                                //added by rajasekar 19/07/2018
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 5].Text = ddlbranch.SelectedItem.ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 5].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 5].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 5].HorizontalAlign = HorizontalAlign.Left;
                            
                            string semsec = "";
                            if (ddlsem.Text == "1")
                                semsec = "I - " + ddlSec.Text;
                            else if (ddlsem.Text == "2")
                                semsec = "II - " + ddlSec.Text;
                            else if (ddlsem.Text == "3")
                                semsec = "III - " + ddlSec.Text;
                            else if (ddlsem.Text == "4")
                                semsec = "IV - " + ddlSec.Text;
                            else if (ddlsem.Text == "5")
                                semsec = "V - " + ddlSec.Text;
                            else if (ddlsem.Text == "6")
                                semsec = "VI - " + ddlSec.Text;
                            else if (ddlsem.Text == "7")
                                semsec = "VII - " + ddlSec.Text;
                            else if (ddlsem.Text == "8")
                                semsec = "VIII - " + ddlSec.Text;
                           
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 6].Text = semsec.ToString();

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 6].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 6].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 6].HorizontalAlign = HorizontalAlign.Left;

                                string hightrange = "0";
                                string lowtrange = "0";
                                string highgrade = "0";
                                string lowgrade="0";
                                if (staffSelector == false)
                                {
                                    hightrange = d2.GetFunction("select MAX(Trange) from Grade_Master where Degree_Code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r,staff_selector s where r.Roll_No=m.roll_no and m.subject_no=s.subject_no  and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staff_code"]).Trim() + "' and Frange>='50')");

                                    highgrade = d2.GetFunction("select mark_grade from grade_master where  trange ='" + hightrange + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");

                                    lowtrange = d2.GetFunction("select MIN(Trange) from Grade_Master where Degree_Code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r,staff_selector s where r.Roll_No=m.roll_no and m.subject_no=s.subject_no  and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staff_code"]).Trim() + "' and Frange>='50')");

                                    lowgrade = d2.GetFunction("select mark_grade from grade_master where  trange ='" + lowtrange + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");
                                    
                                }
                                else
                                {
                                    hightrange = d2.GetFunction("select MAX(Trange) from Grade_Master where Degree_Code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r,staff_selector s,subjectChooser sc where sc.subject_no=s.subject_no and r.Roll_No=sc.roll_no and m.roll_no=sc.roll_no and  r.Roll_No=m.roll_no and m.subject_no=s.subject_no and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staffcode"]).Trim() + "' and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and Frange>='50')");

                                    highgrade = d2.GetFunction("select mark_grade from grade_master where  trange ='" + hightrange + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");

                                    lowtrange = d2.GetFunction("select MIN(Trange) from Grade_Master where Degree_Code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "' and Mark_Grade in(select distinct m.grade from mark_entry m,Registration r,staff_selector s,subjectChooser sc where sc.subject_no=s.subject_no and r.Roll_No=sc.roll_no and m.roll_no=sc.roll_no and  r.Roll_No=m.roll_no and m.subject_no=s.subject_no and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staffcode"]).Trim() + "' and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and Frange>='50')");

                                    lowgrade = d2.GetFunction("select mark_grade from grade_master where  trange ='" + lowtrange + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");
                                }


                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 7].Text = highgrade.ToString();

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 7].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 7].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 7].HorizontalAlign = HorizontalAlign.Left;

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 8].Text = lowgrade.ToString();

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 8].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 8].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 8].HorizontalAlign = HorizontalAlign.Left;


                                string averagetrange = "0";
                                string averagegrade = "0";
                                DataSet trange = new DataSet();
                                
                                if (staffSelector == false)
                                {
                                    if (spsection != "")
                                    {
                                        averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r,staff_selector s where r.Batch_Year='" + ddlbatch.SelectedItem + "' and r.degree_code='" + ddlbranch.SelectedValue + "' and r.college_code='" + ddlcollege.SelectedValue + "' and r.Sections='" + spsection + "' and r.Roll_No=m.roll_no and m.subject_no=s.subject_no  and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staff_code"]).Trim() + "' group by m.grade";
                                    }
                                    else
                                    {
                                        averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r,staff_selector s where r.Batch_Year='" + ddlbatch.SelectedItem + "' and r.degree_code='" + ddlbranch.SelectedValue + "' and r.college_code='" + ddlcollege.SelectedValue + "' and r.Roll_No=m.roll_no and m.subject_no=s.subject_no  and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "' and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staff_code"]).Trim() + "' group by m.grade";
                                    }
                                   

                                }
                                else
                                {
                                     if (spsection != "")
                                    {
                                        averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r,staff_selector s,subjectChooser sc where r.Batch_Year='" + ddlbatch.SelectedItem + "' and r.degree_code='" + ddlbranch.SelectedValue + "' and r.college_code='" + ddlcollege.SelectedValue + "' and r.Sections='" + spsection + "' and  sc.subject_no=s.subject_no and r.Roll_No=sc.roll_no and m.roll_no=sc.roll_no and  r.Roll_No=m.roll_no and m.subject_no=s.subject_no and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staffcode"]).Trim() + "' and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "'group by m.grade ";

                                    }
                                    else
                                    {
                                        averagetrange = "select count(m.grade) as tot,grade from mark_entry m,Registration r,staff_selector s,subjectChooser sc where r.Batch_Year='" + ddlbatch.SelectedItem + "' and r.degree_code='" + ddlbranch.SelectedValue + "' and r.college_code='" + ddlcollege.SelectedValue + "' and  sc.subject_no=s.subject_no and r.Roll_No=sc.roll_no and m.roll_no=sc.roll_no and  r.Roll_No=m.roll_no and m.subject_no=s.subject_no and s.staff_code='" + Convert.ToString(ds.Tables[0].Rows[s1]["staffcode"]).Trim() + "' and s.subject_no='" + Convert.ToString(ds.Tables[0].Rows[s1]["subject_no"]).Trim() + "'group by m.grade ";
                                    }
                                   
                                }
                                
                                trange = d2.select_method_wo_parameter(averagetrange, "text");
                                int count = 0;
                                string range="0";
                                double totmark = 0;
                                double totmark1 = 0;
                                double avemark = 0;
                                if (trange.Tables[0].Rows.Count > 0 && trange.Tables.Count > 0)
                                {
                                    for (int ss = 0; ss < trange.Tables[0].Rows.Count; ss++)
                                    {
                                        range = d2.GetFunction("select trange from grade_master where  mark_grade ='" + Convert.ToString(trange.Tables[0].Rows[ss]["grade"]).Trim() + "' and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");
                                        totmark += (Convert.ToDouble(range) * Convert.ToDouble(trange.Tables[0].Rows[ss]["tot"]));
                                        count += Convert.ToInt32(trange.Tables[0].Rows[ss]["tot"]);
                                    }

                                    totmark1 = count * 100;

                                    avemark= (totmark / totmark1) ;
                                    avemark = Convert.ToInt32(avemark * 100);
                                    
                                }
                                

                                averagegrade = d2.GetFunction("select mark_grade from grade_master where '" + Convert.ToString(avemark) + "' between frange and trange and degree_code='" + ddlbranch.SelectedValue + "' and batch_year='" + ddlbatch.SelectedItem + "' and College_Code='" + ddlcollege.SelectedValue + "'");



                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 9].Text = averagegrade.ToString();

                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 9].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 9].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 9].HorizontalAlign = HorizontalAlign.Left;


                            //==============================//
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 4].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 4].HorizontalAlign = HorizontalAlign.Left;

                            //added by rajasekar 19/07/2018

                         

                           

                            //============================//
                            subnum = Convert.ToInt32(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Tag.ToString());
                            mintotal = Convert.ToDouble(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Note.ToString());
                            gminintmark = Convert.ToDouble(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Tag.ToString());
                            gminextmark = Convert.ToDouble(Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column].Note.ToString());
                            subcode = Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, column + 3].Text.ToString();
                            subnnumtable.Add(subnum);
                            stname = staf_name;
                        }
                    }
                }
                string strallappear = "0";
                string redoAllAppeared = "0";
                if (spsection != "")
                {
                    strallappear = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and m1.result like '%AA')");
                    redoAllAppeared = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "'  and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and m1.result like '%AA')");
                }
                else
                {
                    strallappear = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and m1.result like '%AA')");
                    redoAllAppeared = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and m1.result like '%AA')");
                }
                int withoutRedoAppeared = 0;
                int withRedoAppeared = 0;
                int overAllAppeared = 0;
                int.TryParse(strallappear, out withoutRedoAppeared);
                int.TryParse(redoAllAppeared, out withRedoAppeared);
                overAllAppeared = withoutRedoAppeared + withRedoAppeared;
                strallappear = Convert.ToString(overAllAppeared).Trim();
                //Applied
                string appeared = string.Empty;
                string redoAppeared = string.Empty;
                if (spsection != "")
                {
                    appeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "'  group by s.subject_name,m.subject_no";
                    redoAppeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "'  group by s.subject_name,m.subject_no";
                }
                else
                {
                    appeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "'  and r.Batch_Year='" + batch_year + "'  and m.exam_code='" + ExamCode + "' group by s.subject_name,m.subject_no";
                    redoAppeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "'  group by s.subject_name,m.subject_no";
                }
                DataSet dsappeared = new DataSet();
                DataSet dsRedoAppeared = new DataSet();
                DataView dvRedoAppeared = new DataView();
                dsappeared = d2.select_method_wo_parameter(appeared, "text");
                dsRedoAppeared = d2.select_method_wo_parameter(redoAppeared, "text");
                if (dsappeared.Tables.Count > 0 && dsappeared.Tables[0].Rows.Count > 0)
                {
                    for (int ap = 0; ap < subnnumtable.Count; ap++)
                    {
                        string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[ap, column + 3].Tag).Trim();
                        dsappeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                        dvappear = dsappeared.Tables[0].DefaultView;
                        int num = 0;
                        int.TryParse(subnumber.Trim(), out num);
                        dvRedoAppeared = new DataView();
                        if (dvappear.Count > 0)
                        {
                            dvappearview = 0;
                            int.TryParse(Convert.ToString(dvappear[0]["appear"]).Trim(), out dvappearview);
                            int redoCount = 0;
                            if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                            {
                                dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                            }
                            if (dvRedoAppeared.Count > 0)
                            {
                                redoCount = 0;
                                int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out redoCount);
                            }
                            dvappearview += redoCount;
                            if (subnnumtable.Contains(num))
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 10].Text = Convert.ToString(dvappearview).Trim();
                                // arrayappear.Add(dvappearview);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                                // arrayappear.Add("0");
                            }
                        }
                        else if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                        {
                            dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                            if (dvRedoAppeared.Count > 0)
                            {
                                dvappearview = 0;
                                int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out dvappearview);
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[ap, column + 10].Text = Convert.ToString(dvappearview).Trim();
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                                }
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                                //arrayappear.Add("0");
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                            //arrayappear.Add("0");
                        }
                        Fpspread1.Sheets[0].Cells[ap, column + 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[ap, column + 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[ap, column + 10].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                {
                    for (int ap = 0; ap < subnnumtable.Count; ap++)
                    {
                        string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[ap, column + 3].Tag).Trim();
                        dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                        dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                        int num = 0;
                        int.TryParse(subnumber.Trim(), out num);
                        if (dvRedoAppeared.Count > 0)
                        {
                            dvappearview = 0;
                            int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out dvappearview);
                            if (subnnumtable.Contains(num))
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 10].Text = Convert.ToString(dvappearview).Trim();
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[ap, column + 10].Text = "0";
                            //arrayappear.Add("0");
                        }
                        Fpspread1.Sheets[0].Cells[ap, column + 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[ap, column + 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[ap, column + 10].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else
                {
                    for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                    {
                        arrayappear.Add("0");
                        Fpspread1.Sheets[0].Cells[col, column + 10].Text = "0";
                        Fpspread1.Sheets[0].Cells[col, column + 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[col, column + 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[col, column + 10].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                // Total No of Appear                                                      
                if (spsection != "")
                {
                    appeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "'  and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.result not like '%AA' and (isnull(external_Mark,0) >=0 or m.result='WHD') group by s.subject_name,m.subject_no";
                    redoAppeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and  s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "'  and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.result not like '%AA' and (isnull(external_Mark,0) >=0 or m.result='WHD') group by s.subject_name,m.subject_no";
                }
                else
                {
                    appeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "'  and m.exam_code='" + ExamCode + "' and m.result not like '%AA' and (isnull(external_Mark,0) >=0 or m.result='WHD') group by s.subject_name,m.subject_no";
                    redoAppeared = "select count(m.roll_no) as appear,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and  s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.result not like '%AA' and (isnull(external_Mark,0) >=0 or m.result='WHD') group by s.subject_name,m.subject_no";
                }
                dsappeared = d2.select_method_wo_parameter(appeared, "text");
                dsRedoAppeared = d2.select_method_wo_parameter(redoAppeared, "text");
                if (dsappeared.Tables.Count > 0 && dsappeared.Tables[0].Rows.Count > 0)
                {
                    for (int ap = 0; ap < subnnumtable.Count; ap++)
                    {
                        string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[ap, column + 3].Tag).Trim();
                        dsappeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                        dvappear = dsappeared.Tables[0].DefaultView;
                        int num = 0;// Convert.ToInt32(subnumber);
                        int.TryParse(subnumber.Trim(), out num);
                        if (dvappear.Count > 0)
                        {
                            dvappearview = 0;// Convert.ToInt32(dvappear[0]["appear"].ToString());
                            int.TryParse(Convert.ToString(dvappear[0]["appear"]).Trim(), out dvappearview);
                            dvRedoAppeared = new DataView();
                            int redoCount = 0;
                            if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                            {
                                dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                            }
                            if (dvRedoAppeared.Count > 0)
                            {
                                redoCount = 0;
                                int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out redoCount);
                            }
                            dvappearview += redoCount;
                            if (subnnumtable.Contains(num))
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 11].Text = Convert.ToString(dvappearview).Trim();
                                arrayappear.Add(dvappearview);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 11].Text = "0";
                                arrayappear.Add("0");
                            }
                        }
                        else if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                        {
                            dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                            dvappearview = 0;
                            if (dvRedoAppeared.Count > 0)
                            {
                                int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out dvappearview);
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[ap, column + 11].Text = Convert.ToString(dvappearview).Trim();
                                    arrayappear.Add(dvappearview);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[ap, column + 11].Text = "0";
                                    arrayappear.Add("0");
                                }
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[ap, column + 11].Text = "0";
                            arrayappear.Add("0");
                        }
                        Fpspread1.Sheets[0].Cells[ap, column + 11].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[ap, column + 11].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[ap, column + 11].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else if (dsRedoAppeared.Tables.Count > 0 && dsRedoAppeared.Tables[0].Rows.Count > 0)
                {
                    for (int ap = 0; ap < subnnumtable.Count; ap++)
                    {
                        string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[ap, column + 3].Tag).Trim();
                        dsRedoAppeared.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                        dvRedoAppeared = dsRedoAppeared.Tables[0].DefaultView;
                        dvappearview = 0;
                        int num = 0;
                        int.TryParse(subnumber.Trim(), out num);
                        if (dvRedoAppeared.Count > 0)
                        {
                            int.TryParse(Convert.ToString(dvRedoAppeared[0]["appear"]).Trim(), out dvappearview);
                            if (subnnumtable.Contains(num))
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 11].Text = Convert.ToString(dvappearview).Trim();
                                arrayappear.Add(dvappearview);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[ap, column + 11].Text = "0";
                                arrayappear.Add("0");
                            }
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[ap, column + 11].Text = "0";
                            arrayappear.Add("0");
                        }
                        Fpspread1.Sheets[0].Cells[ap, column + 11].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[ap, column + 11].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[ap, column + 11].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                else
                {
                    for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                    {
                        arrayappear.Add("0");
                        Fpspread1.Sheets[0].Cells[col, column + 11].Text = "0";
                        Fpspread1.Sheets[0].Cells[col, column + 11].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[col, column + 11].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[col, column + 11].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                //Before Moderation
                int strbeforepass = 0;
                DataSet dsbeforepass = new DataSet();
                DataView dvbeforepass = new DataView();
                DataSet dsRedoPass = new DataSet();
                DataView dvRedoPass = new DataView();
                if (rbmoderation.SelectedValue == "1")
                {
                    string beforepass = string.Empty;
                    string redoPass = string.Empty;
                    if (spsection != "")
                    {
                        beforepass = "select count(m.roll_no) as modpass,s.subject_name,m.subject_no from moderation m,subject s,Registration r  where m.subject_no =s.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' group by s.subject_name,m.subject_no";
                        redoPass = "select count(m.roll_no) as modpass,s.subject_name,m.subject_no from moderation m,subject s,Registration r,StudentRedoDetails sr  where sr.Stud_AppNo=r.App_No and m.subject_no =s.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' group by s.subject_name,m.subject_no";
                    }
                    else
                    {
                        beforepass = "select count(m.roll_no) as modpass,s.subject_name,m.subject_no from moderation m,subject s,Registration r  where m.subject_no =s.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and m.exam_code='" + ExamCode + "' group by s.subject_name,m.subject_no";
                        redoPass = "select count(m.roll_no) as modpass,s.subject_name,m.subject_no from moderation m,subject s,Registration r,StudentRedoDetails sr  where sr.Stud_AppNo=r.App_No and m.subject_no =s.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "' group by s.subject_name,m.subject_no"; ;
                    }
                    dsbeforepass = d2.select_method_wo_parameter(beforepass, "text");
                    dsRedoPass = d2.select_method_wo_parameter(redoPass, "text");
                    if (dsbeforepass.Tables.Count > 0 && dsbeforepass.Tables[0].Rows.Count > 0)
                    {
                        for (int b = 0; b < subnnumtable.Count; b++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[b, column + 3].Tag).Trim();
                            dsbeforepass.Tables[0].DefaultView.RowFilter = "subject_no = '" + subnumber + " '";
                            dvbeforepass = dsbeforepass.Tables[0].DefaultView;
                            int num = 0;
                            int.TryParse(subnumber.Trim(), out num);
                            if (dvbeforepass.Count > 0)
                            {
                                dvRedoPass = new DataView();
                                int redoCount = 0;
                                if (dsRedoPass.Tables.Count > 0 && dsRedoPass.Tables[0].Rows.Count > 0)
                                {
                                    dsRedoPass.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                    dvRedoPass = dsRedoPass.Tables[0].DefaultView;
                                }
                                if (dvRedoPass.Count > 0)
                                {
                                    redoCount = 0;
                                    int.TryParse(Convert.ToString(dvRedoPass[0]["modpass"]).Trim(), out redoCount);
                                }
                                if (subnnumtable.Contains(num))
                                {
                                    int count = 0;
                                    int.TryParse(Convert.ToString(dvbeforepass[0]["modpass"]).Trim(), out count);
                                    arraybeforepass.Add(count + redoCount);
                                }
                                else
                                {
                                    arraybeforepass.Add(0);
                                }
                            }
                            else if (dsRedoPass.Tables.Count > 0 && dsRedoPass.Tables[0].Rows.Count > 0)
                            {
                                dsRedoPass.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoPass = dsRedoPass.Tables[0].DefaultView;
                                if (dvRedoPass.Count > 0)
                                {
                                    if (subnnumtable.Contains(num))
                                    {
                                        int count = 0;
                                        int.TryParse(Convert.ToString(dvRedoPass[0]["modpass"]).Trim(), out count);
                                        arraybeforepass.Add(count);
                                    }
                                    else
                                    {
                                        arraybeforepass.Add(0);
                                    }
                                }
                                else
                                {
                                    arraybeforepass.Add(0);
                                }
                            }
                            else
                            {
                                arraybeforepass.Add(0);
                            }
                        }
                    }
                    else if (dsRedoPass.Tables.Count > 0 && dsRedoPass.Tables[0].Rows.Count > 0)
                    {
                        for (int b = 0; b < subnnumtable.Count; b++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[b, column + 3].Tag).Trim();
                            dsRedoPass.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoPass = dsRedoPass.Tables[0].DefaultView;
                            int num = 0;
                            int.TryParse(subnumber.Trim(), out num);
                            if (dvRedoPass.Count > 0)
                            {
                                if (subnnumtable.Contains(num))
                                {
                                    int count = 0;
                                    int.TryParse(Convert.ToString(dvRedoPass[0]["modpass"]).Trim(), out count);
                                    arraybeforepass.Add(count);
                                }
                                else
                                {
                                    arraybeforepass.Add(0);
                                }
                            }
                            else
                            {
                                arraybeforepass.Add(0);
                            }
                        }
                    }
                    else
                    {
                        for (int b = 0; b < subnnumtable.Count; b++)
                        {
                            arraybeforepass.Add(0);
                        }
                    }
                }
                string allpasscount = "0";
                string allRedoPassCount = "0";
                if (spsection != "")
                {
                    allpasscount = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and (m1.result like '%AA' or m1.result='Fail')");
                    allRedoPassCount = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r,StudentRedoDetails sr  where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and (m1.result like '%AA' or m1.result='Fail')");
                }
                else
                {
                    allpasscount = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and (m1.result like '%AA' or m1.result='Fail'))");
                    allRedoPassCount = d2.GetFunction("select Count(distinct m.roll_no) as appear from mark_entry m,subject s,Registration r,StudentRedoDetails sr  where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.roll_no not in(select roll_no from mark_entry m1 where m1.exam_code='" + ExamCode + "' and (m1.result like '%AA' or m1.result='Fail')");
                }
                int overallWithoutRedoPass = 0;
                int overallWithRedoPass = 0;
                int overallRedoPass = 0;
                int.TryParse(allpasscount, out overallWithoutRedoPass);
                int.TryParse(allRedoPassCount, out overallWithRedoPass);
                overallRedoPass = overallWithoutRedoPass + overallWithRedoPass;
                allpasscount = Convert.ToString(overallRedoPass).Trim();
                // Total No of Passes
                string psscnt = string.Empty;
                string redoPassCount = string.Empty;
                if (spsection != "")
                {
                    psscnt = "select count(m.roll_no) as pass,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.result='pass' group by s.subject_name,m.subject_no";
                    redoPassCount = "select count(m.roll_no) as pass,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "' and m.result='pass' group by s.subject_name,m.subject_no";
                }
                else
                {
                    psscnt = "select count(m.roll_no) as pass,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "'  and m.exam_code='" + ExamCode + "' and m.result='pass' group by s.subject_name,m.subject_no";
                    redoPassCount = "select count(m.roll_no) as pass,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "' and m.result='pass' group by s.subject_name,m.subject_no";
                }
                DataSet dsstudentpasscnt = new DataSet();
                DataSet dsRedoPassCount = new DataSet();
                DataView dvRedoPassCount = new DataView();
                dsstudentpasscnt = d2.select_method_wo_parameter(psscnt, "text");
                dsRedoPassCount = d2.select_method_wo_parameter(redoPassCount, "text");
                if (rbmoderation.SelectedValue == "1")
                {
                    if (dsstudentpasscnt.Tables.Count > 0 && dsstudentpasscnt.Tables[0].Rows.Count > 0)
                    {
                        for (int p = 0; p < subnnumtable.Count; p++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[p, column + 3].Tag).Trim();
                            dsstudentpasscnt.Tables[0].DefaultView.RowFilter = "subject_no = '" + subnumber + " '";
                            dvstudpass = dsstudentpasscnt.Tables[0].DefaultView;
                            if (dvstudpass.Count > 0)
                            {
                                dvpassview = 0;
                                int.TryParse(Convert.ToString(dvstudpass[0]["pass"]).Trim(), out dvpassview);
                                strbeforepass = 0;
                                int.TryParse(Convert.ToString(arraybeforepass[p]), out strbeforepass);
                                int num = 0;
                                int.TryParse(subnumber.Trim(), out num);
                                dvRedoPassCount = new DataView();
                                int redoCount = 0;
                                if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                                {
                                    dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                    dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                                }
                                if (dvRedoPassCount.Count > 0)
                                {
                                    redoCount = 0;
                                    int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out redoCount);
                                }
                                dvpassview += redoCount;
                                if (subnnumtable.Contains(num))
                                {
                                    int total = 0;
                                    if (dvpassview >= strbeforepass)
                                    {
                                        total = dvpassview - strbeforepass;
                                    }
                                    else if (strbeforepass >= dvpassview)
                                    {
                                        total = strbeforepass - dvpassview;
                                    }
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = total.ToString();
                                    arraypass.Add(total);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    arraypass.Add("0");
                                }
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                            {
                                dvpassview = 0;
                                dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                                if (dvRedoPassCount.Count > 0)
                                {
                                    dvpassview = 0;
                                    int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out dvpassview);
                                    strbeforepass = 0; ;
                                    int.TryParse(Convert.ToString(arraybeforepass[p]), out strbeforepass);
                                    int num = 0;
                                    int.TryParse(subnumber.Trim(), out num);
                                    if (subnnumtable.Contains(num))
                                    {
                                        int total = 0;
                                        if (dvpassview >= strbeforepass)
                                        {
                                            total = dvpassview - strbeforepass;
                                        }
                                        else if (strbeforepass >= dvpassview)
                                        {
                                            total = strbeforepass - dvpassview;
                                        }
                                        Fpspread1.Sheets[0].Cells[p, column + 12].Text = total.ToString();
                                        arraypass.Add(total);
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                        arraypass.Add("0");
                                    }
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    arraypass.Add("0");
                                }
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                arraypass.Add("0");
                                Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                    {
                        for (int p = 0; p < subnnumtable.Count; p++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[p, column + 3].Tag).Trim();
                            dvpassview = 0;
                            dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                            if (dvRedoPassCount.Count > 0)
                            {
                                dvpassview = 0;
                                int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out dvpassview);
                                strbeforepass = 0; ;
                                int.TryParse(Convert.ToString(arraybeforepass[p]), out strbeforepass);
                                int num = 0;
                                int.TryParse(subnumber.Trim(), out num);
                                if (subnnumtable.Contains(num))
                                {
                                    int total = 0;
                                    if (dvpassview >= strbeforepass)
                                    {
                                        total = dvpassview - strbeforepass;
                                    }
                                    else if (strbeforepass >= dvpassview)
                                    {
                                        total = strbeforepass - dvpassview;
                                    }
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = total.ToString();
                                    arraypass.Add(total);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    arraypass.Add("0");
                                }
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                arraypass.Add("0");
                            }
                            Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                        {
                            arraypass.Add("0");
                            strbeforepass = Convert.ToInt32(arraybeforepass[col]);
                            int total = 0;
                            if (Convert.ToInt32(arraypass[col]) >= strbeforepass)
                            {
                                total = Convert.ToInt32(arraypass[col]) - strbeforepass;
                            }
                            else if (strbeforepass >= Convert.ToInt32(arraypass[col]))
                            {
                                total = strbeforepass - Convert.ToInt32(arraypass[col]);
                            }
                            Fpspread1.Sheets[0].Cells[col, column + 12].Text = total.ToString();
                            Fpspread1.Sheets[0].Cells[col, column + 12].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[col, column + 12].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[col, column + 12].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else if (rbmoderation.SelectedValue == "2")
                {
                    if (dsstudentpasscnt.Tables.Count > 0 && dsstudentpasscnt.Tables[0].Rows.Count > 0)
                    {
                        for (int p = 0; p < subnnumtable.Count; p++)
                        {
                            string subnumber = Fpspread1.Sheets[0].Cells[p, column + 3].Tag.ToString();
                            dsstudentpasscnt.Tables[0].DefaultView.RowFilter = "subject_no = '" + subnumber + " '";
                            dvstudpass = dsstudentpasscnt.Tables[0].DefaultView;
                            dvRedoPassCount = new DataView();
                            if (dvstudpass.Count > 0)
                            {
                                dvpassview = 0;// Convert.ToInt32(dvstudpass[0]["pass"].ToString());
                                int.TryParse(Convert.ToString(dvstudpass[0]["pass"]).Trim(), out dvpassview);
                                int num = 0;// Convert.ToInt32(subnumber);
                                int.TryParse(subnumber.Trim(), out num);
                                int redoCount = 0;
                                if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                                {
                                    dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                    dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                                }
                                if (dvRedoPassCount.Count > 0)
                                {
                                    redoCount = 0;
                                    int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out redoCount);
                                }
                                dvpassview += redoCount;
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 10].Text = Convert.ToString(dvpassview).Trim();
                                    arraypass.Add(dvpassview);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    arraypass.Add("0");
                                }
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                            {
                                dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                                if (dvRedoPassCount.Count > 0)
                                {
                                    dvpassview = 0;// Convert.ToInt32(dvstudpass[0]["pass"].ToString());
                                    int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out dvpassview);
                                    int num = 0;// Convert.ToInt32(subnumber);
                                    int.TryParse(subnumber.Trim(), out num);
                                    if (subnnumtable.Contains(num))
                                    {
                                        Fpspread1.Sheets[0].Cells[p, column + 12].Text = Convert.ToString(dvpassview).Trim();
                                        arraypass.Add(dvpassview);
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                        arraypass.Add("0");
                                    }
                                }
                                else
                                {
                                    arraypass.Add("0");
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                    Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            else
                            {
                                arraypass.Add("0");
                                Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                            Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (dsRedoPassCount.Tables.Count > 0 && dsRedoPassCount.Tables[0].Rows.Count > 0)
                    {
                        for (int p = 0; p < subnnumtable.Count; p++)
                        {
                            string subnumber = Fpspread1.Sheets[0].Cells[p, column + 3].Tag.ToString();
                            dsRedoPassCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoPassCount = dsRedoPassCount.Tables[0].DefaultView;
                            if (dvRedoPassCount.Count > 0)
                            {
                                dvpassview = 0;// Convert.ToInt32(dvstudpass[0]["pass"].ToString());
                                int.TryParse(Convert.ToString(dvRedoPassCount[0]["pass"]).Trim(), out dvpassview);
                                int num = 0;// Convert.ToInt32(subnumber);
                                int.TryParse(subnumber.Trim(), out num);
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = Convert.ToString(dvpassview).Trim();
                                    arraypass.Add(dvpassview);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                    arraypass.Add("0");
                                }
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                arraypass.Add("0");
                                Fpspread1.Sheets[0].Cells[p, column + 12].Text = "0";
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[p, column + 12].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[p, column + 12].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    else
                    {
                        for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                        {
                            arraypass.Add("0");
                            Fpspread1.Sheets[0].Cells[col, column + 12].Text = "0";
                            Fpspread1.Sheets[0].Cells[col, column + 12].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[col, column + 12].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[col, column + 12].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                // Total No of Failures                                       
                string fail = string.Empty;
                string RedoFail = string.Empty;
                if (spsection != "")
                {
                    fail = "select count(m.roll_no) as fail,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and r.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "'and (m.result='fail'  or m.result='WHD') and isnull(external_mark,'0')>=0  group by s.subject_name,m.subject_no";
                    RedoFail = "select count(m.roll_no) as fail,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and sr.Sections='" + spsection + "' and m.exam_code='" + ExamCode + "'and (m.result='fail'  or m.result='WHD') and isnull(external_mark,'0')>=0  group by s.subject_name,m.subject_no";
                }
                else
                {
                    fail = "select count(m.roll_no) as fail,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r where s.subject_no=m.subject_no and m.roll_no=r.Roll_No and r.degree_code='" + degree_code + "' and r.Batch_Year='" + batch_year + "'  and m.exam_code='" + ExamCode + "' and (m.result='fail'  or m.result='WHD') and  isnull(external_mark,'0')>=0 group by s.subject_name,m.subject_no";
                    RedoFail = "select count(m.roll_no) as fail,s.subject_name,m.subject_no from mark_entry m,subject s,Registration r,StudentRedoDetails sr where sr.Stud_AppNo=r.App_No and s.subject_no=m.subject_no and m.roll_no=r.Roll_No and sr.DegreeCode='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.exam_code='" + ExamCode + "'and (m.result='fail'  or m.result='WHD') and isnull(external_mark,'0')>=0  group by s.subject_name,m.subject_no";
                }
                DataSet dsstudentfailcnt = new DataSet();
                DataSet dsRedoFailCount = new DataSet();
                DataView dvRedoFail = new DataView();
                dsstudentfailcnt = d2.select_method_wo_parameter(fail, "text");
                dsRedoFailCount = d2.select_method_wo_parameter(RedoFail, "text");
                if (rbmoderation.SelectedValue == "1")
                {
                    if (dsstudentfailcnt.Tables.Count > 0 && dsstudentfailcnt.Tables[0].Rows.Count > 0)
                    {
                        for (int f = 0; f < subnnumtable.Count; f++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[f, column + 3].Tag).Trim();
                            dsstudentfailcnt.Tables[0].DefaultView.RowFilter = "subject_no='" + subnumber + "'";
                            dvstudfail = dsstudentfailcnt.Tables[0].DefaultView;
                            if (dvstudfail.Count > 0)
                            {
                                dvfailview = 0;
                                int.TryParse(Convert.ToString(dvstudfail[0]["fail"]).Trim(), out dvfailview);
                                int num = 0;
                                int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                dvRedoFail = new DataView();
                                int redoCount = 0;
                                if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                                {
                                    dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                    dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                                }
                                if (dvRedoFail.Count > 0)
                                {
                                    redoCount = 0;
                                    int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out redoCount);
                                }
                                dvfailview += redoCount;
                                if (subnnumtable.Contains(num))
                                {
                                    arrayfail.Add(dvfailview);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                    arrayfail.Add("0");
                                }
                            }
                            else if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                            {
                                dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                                if (dvRedoFail.Count > 0)
                                {
                                    dvfailview = 0;
                                    int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out dvfailview);
                                    int num = 0;
                                    int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                    if (subnnumtable.Contains(num))
                                    {
                                        arrayfail.Add(dvfailview);
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                        arrayfail.Add("0");
                                    }
                                }
                                else
                                {
                                    arrayfail.Add("0");
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                }
                            }
                            else
                            {
                                arrayfail.Add("0");
                                Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                            }
                            int total = 0;
                            strbeforepass = Convert.ToInt32(arraybeforepass[f]);
                            if (Convert.ToInt32(arrayfail[f]) >= strbeforepass)
                            {
                                total = Convert.ToInt32(arrayfail[f]) + strbeforepass;
                            }
                            else if (strbeforepass >= Convert.ToInt32(arrayfail[f]))
                            {
                                total = strbeforepass + Convert.ToInt32(arrayfail[f]);
                            }
                            Fpspread1.Sheets[0].Cells[f, column + 13].Text = total.ToString();
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[f, column + 13].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                    {
                        for (int f = 0; f < subnnumtable.Count; f++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[f, column + 3].Tag).Trim();
                            dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                            if (dvRedoFail.Count > 0)
                            {
                                dvfailview = 0;
                                int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out dvfailview);
                                int num = 0;
                                int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                if (subnnumtable.Contains(num))
                                {
                                    arrayfail.Add(dvfailview);
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                    arrayfail.Add("0");
                                }
                            }
                            else
                            {
                                arrayfail.Add("0");
                                Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                            }
                        }
                    }
                    else
                    {
                        for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                        {
                            arrayfail.Add("0");
                            int total = 0;
                            strbeforepass = 0;// Convert.ToInt32(arraybeforepass[col]);
                            int.TryParse(Convert.ToString(arraybeforepass[col]).Trim(), out strbeforepass);
                            if (Convert.ToInt32(arrayfail[col]) >= strbeforepass)
                            {
                                total = Convert.ToInt32(arrayfail[col]) + strbeforepass;
                            }
                            else if (strbeforepass >= Convert.ToInt32(arrayfail[col]))
                            {
                                total = strbeforepass + Convert.ToInt32(arrayfail[col]);
                            }
                            Fpspread1.Sheets[0].Cells[col, column + 13].Text = total.ToString();
                            Fpspread1.Sheets[0].Cells[col, column + 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[col, column + 13].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[col, column + 13].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else if (rbmoderation.SelectedValue == "2")
                {
                    if (dsstudentfailcnt.Tables.Count > 0 && dsstudentfailcnt.Tables[0].Rows.Count > 0)
                    {
                        for (int f = 0; f < subnnumtable.Count; f++)
                        {
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[f, column + 3].Tag).Trim();
                            dsstudentfailcnt.Tables[0].DefaultView.RowFilter = "subject_no='" + subnumber + "'";
                            dvstudfail = dsstudentfailcnt.Tables[0].DefaultView;
                            dvRedoFail = new DataView();
                            if (dvstudfail.Count > 0)
                            {
                                dvfailview = 0;
                                int.TryParse(Convert.ToString(dvstudfail[0]["fail"]).Trim(), out dvfailview);
                                int num = 0;
                                int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                int redoCount = 0;
                                if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                                {
                                    dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                    dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                                }
                                if (dvRedoFail.Count > 0)
                                {
                                    redoCount = 0;
                                    int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out redoCount);
                                }
                                dvfailview += redoCount;
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = Convert.ToString(dvfailview).Trim();
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                }
                                arrayfail.Add(dvfailview);
                            }
                            else if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                            {
                                dvRedoFail = new DataView();
                                dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                                dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                                if (dvRedoFail.Count > 0)
                                {
                                    dvfailview = 0;
                                    int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out dvfailview);
                                    int num = 0;
                                    int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                    if (subnnumtable.Contains(num))
                                    {
                                        Fpspread1.Sheets[0].Cells[f, column + 13].Text = Convert.ToString(dvfailview).Trim();
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                    }
                                    arrayfail.Add(dvfailview);
                                }
                                else
                                {
                                    arrayfail.Add("0");
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                }
                                Fpspread1.Sheets[0].Cells[f, column + 13].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[f, column + 13].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[f, column + 13].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                arrayfail.Add("0");
                                Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                            }
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[f, column + 13].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (dsRedoFailCount.Tables.Count > 0 && dsRedoFailCount.Tables[0].Rows.Count > 0)
                    {
                        for (int f = 0; f < subnnumtable.Count; f++)
                        {
                            dvRedoFail = new DataView();
                            string subnumber = Convert.ToString(Fpspread1.Sheets[0].Cells[f, column + 3].Tag).Trim();
                            dsRedoFailCount.Tables[0].DefaultView.RowFilter = "subject_no ='" + subnumber + "' ";
                            dvRedoFail = dsRedoFailCount.Tables[0].DefaultView;
                            if (dvRedoFail.Count > 0)
                            {
                                dvfailview = 0;
                                int.TryParse(Convert.ToString(dvRedoFail[0]["fail"]).Trim(), out dvfailview);
                                int num = 0;
                                int.TryParse(Convert.ToString(subnumber).Trim(), out num);
                                if (subnnumtable.Contains(num))
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = Convert.ToString(dvfailview).Trim();
                                }
                                else
                                {
                                    Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                                }
                                arrayfail.Add(dvfailview);
                            }
                            else
                            {
                                arrayfail.Add("0");
                                Fpspread1.Sheets[0].Cells[f, column + 13].Text = "0";
                            }
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[f, column + 13].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[f, column + 13].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else
                    {
                        for (int col = 0; col < Fpspread1.Sheets[0].RowCount; col++)
                        {
                            arrayfail.Add("0");
                            Fpspread1.Sheets[0].Cells[col, column + 13].Text = "0";
                            Fpspread1.Sheets[0].Cells[col, column + 13].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[col, column + 13].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[col, column + 13].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                //Percentage of Pass start
                for (int percent = 0; percent < Fpspread1.Sheets[0].RowCount; percent++)
                {
                    int pass = 0;
                    int totalpass = 0;
                    //Get Values of Pass and Appear
                    //pass = Convert.ToInt32(arraypass[percent]);
                    int.TryParse(Convert.ToString(arraypass[percent]).Trim(), out pass);
                    double appearedcnt = 0;// Convert.ToDouble(arrayappear[percent]);
                    double.TryParse(Convert.ToString(arrayappear[percent]).Trim(), out appearedcnt);
                    if (pass != 0)
                    {
                        totalpass = pass;
                    }
                    else
                    {
                        totalpass = 0;
                    }
                    //Percentage Calculation
                    if (appearedcnt != 0)
                    {
                        double passpercent1 = 0;
                        passpercent1 = Convert.ToDouble((Convert.ToDouble(totalpass) / appearedcnt) * 100);
                        double passpercent2 = Math.Round(passpercent1, 2, MidpointRounding.AwayFromZero);
                        //passpercent = Convert.ToString(passpercent2);
                        passpercent = string.Format("{0:0.00}", passpercent2);
                    }
                    else
                    {
                        passpercent = "0.00";
                    }
                    //string passPercentage = string.Empty;
                    //passPercentage = string.Format("{0:0.00}", passpercent);
                    Fpspread1.Sheets[0].Cells[percent, column + 14].CellType = new FarPoint.Web.Spread.TextCellType();
                    Fpspread1.Sheets[0].Cells[percent, column + 14].Text = passpercent;
                    Fpspread1.Sheets[0].Cells[percent, column + 14].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[percent, column + 14].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[percent, column + 14].HorizontalAlign = HorizontalAlign.Center;
                    //End
                }
               
                //added by rajasekar 19/07/2018
                //Percentage of fail start
                    for (int percent = 0; percent < Fpspread1.Sheets[0].RowCount; percent++)
                    {
                        int fail1 = 0;
                        int totalfail = 0;
                        //Get Values of Pass and Appear
                        //pass = Convert.ToInt32(arraypass[percent]);
                        int.TryParse(Convert.ToString(arrayfail[percent]).Trim(), out fail1);
                        double appearedcnt = 0;// Convert.ToDouble(arrayappear[percent]);
                        double.TryParse(Convert.ToString(arrayappear[percent]).Trim(), out appearedcnt);
                        if (fail1 != 0)
                        {
                            totalfail = fail1;
                        }
                        else
                        {
                            totalfail = 0;
                        }
                        //Percentage Calculation
                        if (appearedcnt != 0)
                        {
                            double failpercent1 = 0;
                            failpercent1 = Convert.ToDouble((Convert.ToDouble(totalfail) / appearedcnt) * 100);
                            double failpercent2 = Math.Round(failpercent1, 2, MidpointRounding.AwayFromZero);
                            //passpercent = Convert.ToString(passpercent2);
                            failpercent = string.Format("{0:0.00}", failpercent2);
                        }
                        else
                        {
                            failpercent = "0.00";
                        }
                        //string passPercentage = string.Empty;
                        //passPercentage = string.Format("{0:0.00}", passpercent);
                        Fpspread1.Sheets[0].Cells[percent, column + 15].CellType = new FarPoint.Web.Spread.TextCellType();
                        Fpspread1.Sheets[0].Cells[percent, column + 15].Text = failpercent;
                        Fpspread1.Sheets[0].Cells[percent, column + 15].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[percent, column + 15].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[percent, column + 15].HorizontalAlign = HorizontalAlign.Center;
                        //End
                    
                
            }

                //=================================================================//



                //Fpspread1.Sheets[0].RowCount++;
                //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 8);
                //Fpspread1.Sheets[0].RowCount++;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount-1, 0].Text = "No of Student Appeared";
                //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 6);
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = strallappear;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                //Double overall = 0;
                //if (strallappear != "" && strallappear != "0" && allpasscount != "" && allpasscount != "0")
                //{
                //    overall = Convert.ToDouble((Convert.ToDouble(allpasscount)) / (Convert.ToDouble(strallappear)) * 100);
                //    overall = Math.Round(overall, 2, MidpointRounding.AwayFromZero);
                //}
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = overall.ToString();
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                //Fpspread1.Sheets[0].RowCount++;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "No of Student Passed";
                //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 6);
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = allpasscount;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 2, 7, 2, 1);

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
            }
            else
            {
                Fpspread1.Visible = false;
                lblerror.Visible = true;
                lblerror.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnexcelsubject_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
            }
            else
            {
                lblerrormsg.Text = "Please Enter Your Report Name";
                lblerrormsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            dvsubjectwise.Style.Add("display", "block");
            lastdiv.Style.Add("display", "block");
            Printcontrol.Visible = true;
            Fpspread1.Visible = true;
            string degree_code = ddlbranch.SelectedValue.ToString();
            string batch_year = ddlbatch.SelectedItem.ToString();
            current_sem = ddlsem.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            string filt_details = string.Empty;
            string sec_details = string.Empty;
            string semdetails = string.Empty;
            if (ddlSec.SelectedItem.Text.Trim().ToLower() == "all" || ddlSec.SelectedItem.Text.Trim().ToLower() == "" || ddlSec.SelectedItem.Text.Trim().ToLower() == "-1")
            {
                sec_details = string.Empty;
            }
            else
            {
                sec_details = " - " + ddlSec.SelectedItem.Text;
            }
            string collegeHeaderName = txtCollegeHeader.Text.Trim();
            string reportName = string.Empty;
            string aided = string.Empty;
            aided = da.GetFunctionv("select distinct type from Course c where c.college_code='" + Convert.ToString(ddlcollege.SelectedValue).Trim() + "'");
            if (!string.IsNullOrEmpty(txtReportName.Text.Trim()))
            {
                reportName = "$" + txtReportName.Text.Trim() + ((chkReportWithStream.Checked) ? " ( " + aided + " )" : "");
            }
            else
            {
                reportName = string.Empty;
            }
            if (rbformat.SelectedValue == "1")
            {
                string reval = string.Empty;
                if (rbbeforeandafterrevaluation.SelectedValue == "1")
                {
                    reval = " - Before Revaluation";
                }
                else if (rbbeforeandafterrevaluation.SelectedValue == "2")
                {
                    reval = " - After Revaluation";
                }
                semdetails = " Sem - " + ddlsem.SelectedItem.ToString();
                filt_details = "Degree: " + batch_year + " - " + ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + " - " + semdetails + sec_details;
                string degreedetails = "Subjectwise Result Analysis" + reval + "@" + filt_details + "";
                string pagename = "UnivresultAnalysis.aspx";
                Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            else
            {
                if (batch_year != "" && ddlbranch.SelectedValue.ToString() != "" && current_sem != "")
                {
                    string exammonyearva = string.Empty;
                    string exammonth = "select Exam_year,Exam_month from Exam_Details where batch_year ='" + batch_year + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester='" + current_sem + "'";
                    DataSet dsexamno = d2.select_method_wo_parameter(exammonth, "Text");
                    if (dsexamno.Tables.Count > 0 && dsexamno.Tables[0].Rows.Count > 0)
                    {
                        string monva = dsexamno.Tables[0].Rows[0]["Exam_month"].ToString();
                        if (monva.Trim() != "")
                        {
                            if (monva == "1")
                            {
                                exammonyearva = "January";
                            }
                            else if (monva == "2")
                            {
                                exammonyearva = "February";
                            }
                            else if (monva == "3")
                            {
                                exammonyearva = "March";
                            }
                            else if (monva == "4")
                            {
                                exammonyearva = "April";
                            }
                            else if (monva == "5")
                            {
                                exammonyearva = "May";
                            }
                            else if (monva == "6")
                            {
                                exammonyearva = "June";
                            }
                            else if (monva == "7")
                            {
                                exammonyearva = "July";
                            }
                            else if (monva == "8")
                            {
                                exammonyearva = "August";
                            }
                            else if (monva == "9")
                            {
                                exammonyearva = "September";
                            }
                            else if (monva == "10")
                            {
                                exammonyearva = "October";
                            }
                            else if (monva == "11")
                            {
                                exammonyearva = "November";
                            }
                            else if (monva == "12")
                            {
                                exammonyearva = "December";
                            }
                        }
                        exammonyearva = exammonyearva + "   " + dsexamno.Tables[0].Rows[0]["Exam_year"].ToString(); ;
                    }
                    string moderation = string.Empty;
                    if (rbmoderation.SelectedValue == "1")
                    {
                        moderation = "(Before Moderation)";
                    }
                    else
                    {
                        moderation = "(After Moderation)";
                    }
                    filt_details = "Degree & Branch: " + ddldegree.SelectedItem.ToString() + "  &   " + ddlbranch.SelectedItem.ToString();
                    sec_details = " Semester             :" + ddlsem.SelectedItem.ToString() + sec_details + "";
                    string degreedetails = ((string.IsNullOrEmpty(collegeHeaderName)) ? "" : collegeHeaderName + "$") + "Office of the Controller of Examinations" + ((string.IsNullOrEmpty(reportName.Trim())) ? "$Result Analysis Statement   " + moderation : reportName) + "" + "@ " + filt_details + "                                                                                        Month & Year of Exam : " + exammonyearva + "@" + sec_details + "";
                    string pagename = "UnivresultAnalysis.aspx";
                    Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
                    Printcontrol.Visible = true;
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Batch Year, Degree, Semester";
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.ToString();
        }
    }

    protected void rbmoderation_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvconsolidated.Style.Add("display", "none");
        dvsubjectwise.Style.Add("display", "none");
        lastdiv.Style.Add("display", "none");
    }

    public void clear()
    {
        Fpspread1.Visible = false;
        txtexcelname.Visible = false;       
        Printcontrol.Visible = false;
        txtexcelname.Text = string.Empty;
        lblrptname.Visible = false;
        btnexcelsubject.Visible = false;
        lblerrormsg.Visible = false;
        btnprintmaster.Visible = false;
       
    }

    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

}