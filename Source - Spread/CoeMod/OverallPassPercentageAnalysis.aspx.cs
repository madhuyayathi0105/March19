using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;


public partial class OverallPassPercentageAnalysis : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable addyear = new Hashtable();
    ArrayList avgarray = new ArrayList();
    int column = 5;
    string group_user = "", singleuser = "", usercode = "", collegecode = "";
    string bran = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Label5.Visible = false;
            Label7.Visible = false;
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
                bindbranch(bran);
                bindsemester();
                btnExcel.Visible = false;
                btnPrint.Visible = false;
                Chart1.Visible = false;
            }
            if (txt_degree.Text == "--Select--" && txt_branch.Text == "--Select--")
            {
                grdover.Visible = false;
                btnExcel.Visible = false;
                btnPrint.Visible = false;
                Chart1.Visible = false;
                lblYear.Visible = false;
                Label1.Visible = false;
                Chart2.Visible = false;
                Label2.Visible = false;
                Chart3.Visible = false;
                Label3.Visible = false;
                Chart4.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
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
            ds = da.select_method("bind_college", hat, "sp");
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
        catch (Exception e)
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
            ddlbatch.Items.Insert(0, "All");
        }
        catch (Exception ex)
        {
        }
    }

    public void binddegree()
    {
        try
        {
            txt_degree.Text = "Degree(" + (5) + ")";
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

                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();
            }
            int v = 0;
            if (count1 > 0)
            {
                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    v++;
                    Chklst_degree.Items[h].Selected = true;
                    txt_degree.Text = "Degree " + "(" + v + ")";
                }
            }
        }
        catch (Exception ex)
        { }

    }

    public void bindbranch(string mainvalue)
    {
        try
        {
            int count = 0;
            if (mainvalue.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), mainvalue, ddlcollege.SelectedItem.Value, Session["usercode"].ToString());
            }
            else
            {
                ds = da.select_method_wo_parameter("select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code='" + ddlcollege.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code", "text");

            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_branch.DataSource = ds;
                chklst_branch.DataTextField = "dept_name";
                chklst_branch.DataValueField = "degree_code";
                chklst_branch.DataBind();
            }
            if (chklst_branch.Items.Count > 0)
            {

                for (int h = 0; h < chklst_branch.Items.Count; h++)
                {
                    count++;
                    chklst_branch.Items[h].Selected = true;
                }
                txt_branch.Text = "Dept(" + (count) + ")";
            }

        }
        catch (Exception ex)
        {

        }

    }
    public void bindsemester()
    {
        try
        {
            ddlsemfrom.Items.Clear();
            ddlsemto.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = "";
            string strbranch = "";
            string strbatch = "";
            for (int b = 0; b < chklst_branch.Items.Count; b++)
            {
                if (chklst_branch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = chklst_branch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + chklst_branch.Items[b].Value;
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }
            if (ddlbatch.SelectedItem.Text != "All")
            {
                strbatch = "  and batch_year=" + ddlbatch.SelectedItem.Text + "";
            }
            if (strbranch.Trim() != "")
            {
                strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlcollege.SelectedValue.ToString() + " " + strbatch + "  " + strbranch + " order by NDurations desc";
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsemfrom.Items.Add(i.ToString());
                            ddlsemto.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsemfrom.Items.Add(i.ToString());
                            ddlsemto.Items.Add(i.ToString());
                        }

                    }
                    ddlsemfrom.Enabled = true;
                    ddlsemto.Enabled = true;
                }
                else
                {
                    strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlcollege.SelectedValue.ToString() + " " + strbranch + " order by duration desc";
                    ds.Reset();
                    ds.Dispose();
                    ds = da.select_method_wo_parameter(strquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                        duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlsemfrom.Items.Add(i.ToString());
                                ddlsemto.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlsemfrom.Items.Add(i.ToString());
                                ddlsemto.Items.Add(i.ToString());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlsemfrom_SelectedIndexChanged1(object sender, EventArgs e)
    {
        grdover.Visible = false;
        Chart1.Visible = false;
        Chart2.Visible = false;
        Chart3.Visible = false;
        Chart4.Visible = false;
        lblYear.Visible = false;
        Label1.Visible = false;
        Label2.Visible = false;
        Label3.Visible = false;
        btnExcel.Visible = false;
        btnPrint.Visible = false;
    }
    protected void ddlsemto_SelectedIndexChanged1(object sender, EventArgs e)
    {
        grdover.Visible = false;
        Chart1.Visible = false;
        Chart2.Visible = false;
        Chart3.Visible = false;
        Chart4.Visible = false;
        lblYear.Visible = false;
        Label1.Visible = false;
        Label2.Visible = false;
        Label3.Visible = false;
        btnExcel.Visible = false;
        btnPrint.Visible = false;
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
        }
    }
    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chk_degree.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_degree.Text = "--Select--";
                    build = Chklst_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + build;

                    }
                }
            }

            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            bindbranch(buildvalue);
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                //   ddlsemfrom.Items.Clear();
                //   ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }

            if (seatcount == Chklst_degree.Items.Count)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                chk_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
            }

        }

        catch (Exception ex)
        { }
    }
    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {

                    chklst_branch.Items[i].Selected = true;
                    txt_branch.Text = "Dept(" + (chklst_branch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                    txt_branch.Text = "--Select--";
                }

            }

            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                // ddlsemfrom.Items.Clear();
                // ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }

    }



    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string Chklstbatchvalue = "";
            string bind1 = "";
            chklst_branch.ClearSelection();
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {

                    if (chk_degree.Checked == true)
                    {
                        Chklst_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
                        bind1 = Chklst_degree.Items[i].Value.ToString();
                        if (Chklstbatchvalue == "")
                        {
                            Chklstbatchvalue = bind1;
                        }
                        else
                        {
                            Chklstbatchvalue = Chklstbatchvalue + "," + bind1;
                        }

                    }
                    else if (chk_degree.Checked == false)
                    {
                    }
                }


            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    Chklst_degree.ClearSelection();
                    chk_branch.Checked = false;
                }
            }

            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            bindbranch(Chklstbatchvalue);
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                //  ddlsemfrom.Items.Clear();
                //  ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            chk_branch.Checked = false;
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == chklst_branch.Items.Count)
            {
                txt_branch.Text = "Branch(" + seatcount.ToString() + ")";
                chk_branch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_branch.Text = "--Select--";

            }
            else
            {
                txt_branch.Text = "Dept(" + seatcount.ToString() + ")";
            }

            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                ///  ddlsemfrom.Items.Clear();
                //  ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void grd()
    {
        string branch = "";
        string branchval1 = "";

        try
        {
            if (Convert.ToInt32(ddlsemfrom.SelectedItem.Text) <= Convert.ToInt32(ddlsemto.SelectedItem.Text))
            {
                if (txt_degree.Text != "--Select--" && txt_branch.Text != "--Select--")
                {
                    ArrayList addcolumn = new ArrayList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("DEPARTMENT", typeof(string));
                    dt.Columns.Add("I Year", typeof(string));
                    dt.Columns.Add("I Year Pass", typeof(string));
                    dt.Columns.Add("I Year Percentage", typeof(string));
                    dt.Columns.Add("II Year", typeof(string));
                    dt.Columns.Add("II Year Pass", typeof(string));
                    dt.Columns.Add("II Year Percentage", typeof(string));
                    dt.Columns.Add("III Year", typeof(string));
                    dt.Columns.Add("III Year Pass", typeof(string));
                    dt.Columns.Add("III Year Percentage", typeof(string));
                    dt.Columns.Add("IV Year", typeof(string));
                    dt.Columns.Add("IV Year Pass", typeof(string));
                    dt.Columns.Add("IV Year Percentage", typeof(string));
                    addcolumn.Add("I Year");
                    addcolumn.Add("II Year");
                    addcolumn.Add("III Year");
                    addcolumn.Add("IV Year");
                    DataRow dtrow = null;
                    string str1 = "";
                    for (int i = 0; i < chklst_branch.Items.Count; i++)
                    {
                        if (chklst_branch.Items[i].Selected == true)
                        {
                            btnExcel.Visible = true;
                            btnPrint.Visible = true;
                            Chart1.Visible = true;
                            Chart2.Visible = true;
                            Chart3.Visible = true;
                            Chart4.Visible = true;
                            grdover.Visible = true;
                            branch = chklst_branch.Items[i].Text;
                            branchval1 = chklst_branch.Items[i].Value;
                            if (ddlbatch.SelectedItem.Text != "All")
                            {

                                str1 = " select COUNT(distinct m.roll_no) as pass,d.Acronym,y.semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and result  in('Pass') and passorfail  in(1)  and y.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and y.degree_code='" + branchval1 + "'  ";

                                if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                {
                                    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                }
                                str1 = str1 + " and rt.roll_no not in( select distinct rt.roll_no   from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and result  in('Fail','AAA','WHD')  and y.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and y.degree_code='" + branchval1 + "'";
                                if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                {
                                    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + ")";
                                }
                                str1 = str1 + " group by d.Acronym,y.semester,rt.Batch_Year";

                                str1 = str1 + " select COUNT(distinct m.roll_no) as fail,d.Acronym,y.semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and result  in('fail','AAA','WHD') and passorfail  in(0)  and y.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and y.degree_code='" + branchval1 + "' ";
                                if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                {
                                    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                }
                                str1 = str1 + " group by d.Acronym,y.semester,rt.Batch_Year  ";
                                str1 = str1 + " select COUNT(distinct m.roll_no) as present,d.Acronym,y.semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0    and y.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and y.degree_code='" + branchval1 + "'  ";
                                if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                {
                                    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                }
                                str1 = str1 + " and m.roll_no not in(select distinct m.roll_no from subject s, syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and y.Batch_Year='" + ddlbatch.SelectedItem.Value + "' and y.degree_code='" + branchval1 + "' and result='AAA'";
                                if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                {
                                    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + ")";
                                }
                                str1 = str1 + " group by d.Acronym,y.semester,rt.Batch_Year ";
                                ds = da.select_method_wo_parameter(str1, "text");
                            }
                            else
                            {
                                str1 = " select COUNT(distinct m.roll_no) as pass,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and result  in('Pass') and passorfail  in(1)    and y.degree_code='" + branchval1 + "' ";
                                //if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                //{
                                //    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                //}
                                str1 = str1 + " and rt.roll_no not in( select distinct rt.roll_no   from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and result  in('Fail','AAA','WHD') and y.degree_code='" + branchval1 + "') ";
                                //if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                //{
                                //    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + ")";
                                //}
                                str1 = str1 + " group by d.Acronym,rt.Current_Semester,rt.Batch_Year ";
                                str1 = str1 + " select COUNT(distinct m.roll_no) as fail,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and result  in('fail','AAA','WHD') and passorfail  in(0)    and y.degree_code='" + branchval1 + "'";
                                //if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                //{
                                //    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                //}
                                str1 = str1 + " group by d.Acronym,rt.Current_Semester,rt.Batch_Year  ";
                                //   str1 = str1 + " select COUNT(distinct m.roll_no) as present,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and y.degree_code='" + branchval1 + "'  and m.roll_no not in(select distinct m.roll_no from subject s, syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0   and y.degree_code='" + branchval1 + "' and result='AAA')  ";
                                str1 = str1 + " select COUNT(distinct m.roll_no) as present,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and  y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and y.degree_code='" + branchval1 + "'  ";
                                //if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                //{
                                //    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + "";
                                //}
                                str1 = str1 + " and m.roll_no not in(select distinct m.roll_no from subject s, syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no  and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0   and y.degree_code='" + branchval1 + "' and result='AAA' )";
                                //if (ddlsemfrom.SelectedItem.Text != "" && ddlsemto.SelectedItem.Text != "")
                                //{
                                //    str1 = str1 + " and y.semester between " + ddlsemfrom.SelectedItem.Text + " and " + ddlsemto.SelectedItem.Text + ")";
                                //}
                                str1 = str1 + " group by d.Acronym,rt.Current_Semester,rt.Batch_Year ";
                                // str1 = " select COUNT(distinct m.roll_no) as pass,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no  and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and result  in('Pass') and passorfail  in(1)    and y.degree_code='" + branchval1 + "' and  rt.roll_no not in( select distinct rt.roll_no   from mark_entry m,Registration rt where rt.Roll_No=m.roll_no and   result  in('AAA','Fail','WHD')and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.cc=0 and rt.degree_code='" + branchval1 + "') group by d.Acronym, rt.Current_Semester,rt.Batch_Year  select COUNT(distinct m.roll_no) as fail,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and result  in('fail','AAA','WHD') and passorfail  in(0)    and y.degree_code='" + branchval1 + "'group by d.Acronym,rt.Current_Semester,rt.Batch_Year  select COUNT(distinct m.roll_no) as present,d.Acronym,rt.Current_Semester,rt.Batch_Year from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and y.degree_code='" + branchval1 + "'  and m.roll_no not in(select distinct m.roll_no from subject s, syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and y.Batch_Year=rt.Batch_Year and y.degree_code=rt.degree_code  and d.Degree_Code=rt.degree_code and d.college_code=rt.college_code and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0   and y.degree_code='" + branchval1 + "' and result='AAA')  group by d.Acronym,rt.Current_Semester,rt.Batch_Year ";
                                ds = da.select_method_wo_parameter(str1, "text");
                            }


                            
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    dtrow = dt.NewRow();
                                    //added by rajasekar 20/08/2018
                                    dtrow[1] = "0";
                                    dtrow[2] = "0";
                                    dtrow[3] = "0";

                                    dtrow[4] = "0";
                                    dtrow[5] = "0";
                                    dtrow[6] = "0";

                                    dtrow[7] = "0";
                                    dtrow[8] = "0";
                                    dtrow[9] = "0";


                                    dtrow[10] = "0";
                                    dtrow[11] = "0";
                                    dtrow[12] = "0";
                                    //========================//
                                    for (int ik = 0; ik < ds.Tables[2].Rows.Count; ik++)
                                    {
                                        int sem = 0;
                                        int batch = 0;
                                        if (ddlbatch.SelectedItem.Text == "All")
                                        {
                                            sem = Convert.ToInt32(ds.Tables[2].Rows[ik]["Current_Semester"].ToString());
                                            batch = Convert.ToInt32(ds.Tables[2].Rows[ik]["Batch_Year"].ToString());
                                        }

                                        else
                                        {
                                            sem = Convert.ToInt32(ds.Tables[2].Rows[ik]["semester"].ToString());
                                            batch = Convert.ToInt32(ds.Tables[2].Rows[ik]["Batch_Year"].ToString());
                                        }
                                        // int Batch = Convert.ToInt32(ds.Tables[2].Rows[ik]["Batch_Year"].ToString());
                                        dtrow[0] = ds.Tables[2].Rows[ik]["Acronym"].ToString();
                                        if (sem == 1 || sem == 2)
                                        {

                                            double final_pperc11 = 0;
                                            DataView rsap = new DataView();
                                            if (ddlbatch.SelectedItem.Text == "All")
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "Current_Semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            else
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            rsap = ds.Tables[0].DefaultView;
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (rsap.Count > 0)
                                                {
                                                    if (!has.ContainsKey("1"))
                                                    {
                                                        has.Add("1", "I Year");
                                                        addyear.Add("I Year", "Appear");
                                                        addyear.Add("I Year Pass", "Pass");
                                                        addyear.Add("I Year Percentage", "Percentage");
                                                    }

                                                    final_pperc11 = (Convert.ToDouble(rsap[0]["pass"].ToString()) / (Convert.ToDouble(ds.Tables[2].Rows[ik]["present"].ToString()))) * 100;
                                                    final_pperc11 = Math.Round(final_pperc11, 2);
                                                    string setval = final_pperc11.ToString();
                                                    string[] spva = setval.Split('.');
                                                    if (spva.GetUpperBound(0) == 1)
                                                    {
                                                        if (spva[1].Length == 1)
                                                        {
                                                            setval = setval + "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        setval = setval + ".00";
                                                    }
                                                    dtrow[1] = ds.Tables[2].Rows[ik]["present"].ToString();
                                                    dtrow[2] = rsap[0]["pass"].ToString();
                                                    dtrow[3] = setval;
                                                    lblYear.Text = "I YEAR";
                                                    lblYear.ForeColor = System.Drawing.Color.Black;
                                                    dtrow[3] = rsap;
                                                    dtrow[3] = setval;
                                                }
                                                

                                            }
                                            else
                                            {
                                                dtrow[1] = "0";
                                                dtrow[2] = "0";
                                                dtrow[3] = "0";
                                                dtrow[3] = final_pperc11;
                                            }


                                        }
                                        else if (sem == 3 || sem == 4)
                                        {

                                            double fi = 0;
                                            DataView rsap1 = new DataView();
                                            if (ddlbatch.SelectedItem.Text == "All")
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "Current_Semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            else
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            rsap1 = ds.Tables[0].DefaultView;
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (rsap1.Count > 0)
                                                {
                                                    if (!has.ContainsKey("2"))
                                                    {

                                                        has.Add("2", "II Year");
                                                        addyear.Add("II Year", "Appear");
                                                        addyear.Add("II Year Pass", "Pass");
                                                        addyear.Add("II Year Percentage", "Percentage");
                                                    }

                                                    fi = (Convert.ToDouble(rsap1[0]["pass"].ToString()) / (Convert.ToDouble(ds.Tables[2].Rows[ik]["present"].ToString()))) * 100;
                                                    //string pass = Convert.ToString(rsap1[0]["pass"].ToString());
                                                    //string present = Convert.ToString(ds.Tables[2].Rows[0]["present"].ToString());
                                                    //fi = Convert.ToDouble(pass) / Convert.ToDouble(present) * 100;
                                                    fi = Math.Round(fi, 2);
                                                    string setval = fi.ToString();
                                                    string[] spva = setval.Split('.');
                                                    if (spva.GetUpperBound(0) == 1)
                                                    {
                                                        if (spva[1].Length == 1)
                                                        {
                                                            setval = setval + "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        setval = setval + ".00";
                                                    }
                                                    dtrow[4] = ds.Tables[2].Rows[ik]["present"].ToString();
                                                    dtrow[5] = rsap1[0]["pass"].ToString();
                                                    dtrow[6] = setval;
                                                    Label1.Text = "II YEAR";
                                                    Label1.ForeColor = System.Drawing.Color.Black;
                                                    dtrow[6] = setval;
                                                }
                                                


                                            }
                                            else
                                            {
                                                dtrow[4] = "0";
                                                dtrow[5] = "0";
                                                dtrow[6] = "0";
                                                dtrow[6] = fi;
                                            }


                                        }
                                        else if (sem == 5 || sem == 6)
                                        {

                                            double fi1 = 0;
                                            DataView rsap2 = new DataView();
                                            if (ddlbatch.SelectedItem.Text == "All")
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "Current_Semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            else
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            rsap2 = ds.Tables[0].DefaultView;
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (rsap2.Count > 0)
                                                {
                                                    if (!has.ContainsKey("3"))
                                                    {

                                                        has.Add("3", "III Year");
                                                        addyear.Add("III Year", "Appear");
                                                        addyear.Add("III Year Pass", "Pass");
                                                        addyear.Add("III Year Percentage", "Percentage");
                                                    }

                                                    fi1 = (Convert.ToDouble(rsap2[0]["pass"].ToString()) / (Convert.ToDouble(ds.Tables[2].Rows[ik]["present"].ToString()))) * 100;
                                                    fi1 = Math.Round(fi1, 2);
                                                    string setval = fi1.ToString();
                                                    string[] spva = setval.Split('.');
                                                    if (spva.GetUpperBound(0) == 1)
                                                    {
                                                        if (spva[1].Length == 1)
                                                        {
                                                            setval = setval + "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        setval = setval + ".00";
                                                    }
                                                    dtrow[7] = ds.Tables[2].Rows[ik]["present"].ToString();
                                                    dtrow[8] = rsap2[0]["pass"].ToString();
                                                    dtrow[9] = setval;
                                                    Label2.Text = "III YEAR";
                                                    Label2.ForeColor = System.Drawing.Color.Black;

                                                    dtrow[9] = setval;

                                                }
                                                


                                            }
                                            else
                                            {
                                                dtrow[7] = "0";
                                                dtrow[8] = "0";
                                                dtrow[9] = "0";
                                                dtrow[9] = fi1;
                                            }

                                        }
                                        else if (sem == 7 || sem == 8)
                                        {


                                            double fi11 = 0;
                                            DataView rsap3 = new DataView();
                                            if (ddlbatch.SelectedItem.Text == "All")
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "Current_Semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            else
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "semester = " + sem + " and Batch_Year=" + batch + " ";
                                            }
                                            rsap3 = ds.Tables[0].DefaultView;
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (rsap3.Count > 0)
                                                {
                                                    if (!has.ContainsKey("4"))
                                                    {

                                                        has.Add("4", "IV Year");
                                                        addyear.Add("IV Year", "Appear");
                                                        addyear.Add("IV Year Pass", "Pass");
                                                        addyear.Add("IV Year Percentage", "Percentage");
                                                    }
                                                    fi11 = (Convert.ToDouble(rsap3[0]["pass"].ToString()) / (Convert.ToDouble(ds.Tables[2].Rows[ik]["present"].ToString()))) * 100;
                                                    fi11 = Math.Round(fi11, 2);
                                                    string setval = fi11.ToString();
                                                    string[] spva = setval.Split('.');
                                                    if (spva.GetUpperBound(0) == 1)
                                                    {
                                                        if (spva[1].Length == 1)
                                                        {
                                                            setval = setval + "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        setval = setval + ".00";
                                                    }
                                                    dtrow[10] = ds.Tables[2].Rows[ik]["present"].ToString();
                                                    dtrow[11] = rsap3[0]["pass"].ToString();
                                                    dtrow[12] = setval;
                                                    Label3.Text = "IV YEAR";
                                                    Label3.ForeColor = System.Drawing.Color.Black;

                                                    dtrow[12] = setval;

                                                }
                                                else
                                                {
                                                    dtrow[10] = "0";
                                                    dtrow[11] = "0";
                                                    dtrow[12] = "0";
                                                    dtrow[12] = fi11;
                                                }


                                            }
                                            else
                                            {
                                                dtrow[10] = "0";
                                                dtrow[11] = "0";
                                                dtrow[12] = "0";
                                                dtrow[12] = fi11;
                                            }


                                        }

                                    }
                                    dt.Rows.Add(dtrow);
                                }
                                else//added by rajasekar 18/08/2018
                                {
                                    dtrow = dt.NewRow();
                                    string acr = da.GetFunctionv("select Acronym from Degree where Degree_Code='" + chklst_branch.Items[i].Value + "' and college_code='" + ddlcollege.SelectedValue + "'");
                                    dtrow[0] = acr;
                                    dtrow[1] = "0";
                                    dtrow[2] = "0";
                                    dtrow[3] = "0";

                                    dtrow[4] = "0";
                                    dtrow[5] = "0";
                                    dtrow[6] = "0";

                                    dtrow[7] = "0";
                                    dtrow[8] = "0";
                                    dtrow[9] = "0";


                                    dtrow[10] = "0";
                                    dtrow[11] = "0";
                                    dtrow[12] = "0";

                                    dt.Rows.Add(dtrow);
                                    

                                }
                            }
                            else//added by rajasekar 18/08/2018
                            {
                                string present = "0";
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    present = ds.Tables[2].Rows[0]["present"].ToString();
                                }

                                dtrow = dt.NewRow();
                                string acr = da.GetFunctionv("select Acronym from Degree where Degree_Code='" + chklst_branch.Items[i].Value + "' and college_code='" + ddlcollege.SelectedValue + "'");
                                dtrow[0] = acr;
                                dtrow[1] = present;
                                dtrow[2] = "0";
                                dtrow[3] = "0";


                                
                                    dtrow[4] = "0";
                                    dtrow[5] = "0";
                                    dtrow[6] = "0";
                                
                                    dtrow[7] = "0";
                                    dtrow[8] = "0";
                                    dtrow[9] = "0";
                               
                                
                                    dtrow[10] = "0";
                                    dtrow[11] = "0";
                                    dtrow[12] = "0";
                               
                                dt.Rows.Add(dtrow);
                            }
                        }

                    }

                    if (dt.Rows.Count > 0)
                    {
                        int check = 0;
                        for (int h = 0; h < 4; h++)
                        {
                            check++;
                            if (!has.ContainsKey(Convert.ToString(check)))
                            {
                                string column_name = Convert.ToString(addcolumn[h]);
                                dt.Columns.Remove(column_name);
                                dt.Columns.Remove(column_name + " Pass");
                                dt.Columns.Remove(column_name + " Percentage");
                            }
                        }
                        ViewState["temp_table"] = dt.Columns.Count;
                        ViewState["tbl"] = dt;
                        double avg = 0;
                        int count_value = 0;
                        if (dt.Columns.Count > 0)
                        {
                            for (int col = 1; col < dt.Columns.Count; col++)
                            {
                                avg = 0;
                                count_value = 0;
                                for (int row = 0; row < dt.Rows.Count; row++)
                                {
                                    string avgvalue = Convert.ToString(dt.Rows[row][col]);
                                    if (avgvalue.Trim() != "")
                                    {
                                        avg = avg + Convert.ToDouble(avgvalue);
                                        count_value++;
                                    }
                                }
                                if (col == 3 || col == 6 || col == 9 || col == 12)
                                {
                                    avg = avg / Convert.ToDouble(count_value);
                                }
                                avgarray.Add(avg);
                            }
                        }


                        grdover.DataSource = dt;
                        grdover.DataBind();
                        grdover.Visible = true;

                        if (dt.Rows.Count > 0)
                        {
                            int cols = 0;
                            int check1 = 0;
                            for (int h = 0; h < 4; h++)
                            {
                                check1++;
                                if (has.ContainsKey(Convert.ToString(check1)))
                                {
                                    cols++;
                                    int colva = cols * 3 - 1;
                                    grdover.HeaderRow.Cells[colva].ColumnSpan = 3;
                                    grdover.HeaderRow.Cells[colva + 1].Visible = false;
                                    grdover.HeaderRow.Cells[colva + 2].Visible = false;
                                }
                            }
                        }

                        bool chart1 = false;
                        bool chart2 = false;
                        bool chart3 = false;
                        bool chart4 = false;
                        if (dt.Rows.Count > 0)
                        {
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                for (int j = 1; j < dt.Columns.Count; j++)
                                {
                                    String colName = dt.Columns[j].ColumnName;
                                    if (colName == "I Year")
                                    {
                                        chart1 = true;
                                        Chart1.Series[0].Points.AddXY(dt.Rows[r][0], dt.Rows[r][j + 2]);
                                        Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        Chart1.ChartAreas[0].AxisX.RoundAxisValues();
                                        Chart1.ChartAreas[0].AxisX.Minimum = 0;
                                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                                        Chart1.Series["Series1"].IsValueShownAsLabel = true;
                                        Chart1.ChartAreas[0].AxisX.Title = "DEPARTMENT";
                                        Chart1.ChartAreas[0].AxisY.Title = "PERCENTAGE";
                                        Chart1.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Red;
                                        Chart1.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Green;
                                        // Chart1.ChartAreas[0].AxisY.Maximum = 100;
                                        // Title title = Chart1.Titles.Add("I YEAR");
                                        // title.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
                                        // title.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                                        Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart1.Visible = true;
                                        lblYear.Visible = true;
                                    }
                                    if (colName == "II Year")
                                    {
                                        chart2 = true;
                                        Chart2.Series[0].Points.AddXY(dt.Rows[r][0], dt.Rows[r][j + 2]);
                                        Chart2.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        Chart2.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        Chart2.ChartAreas[0].AxisX.RoundAxisValues();
                                        Chart2.ChartAreas[0].AxisX.Minimum = 0;
                                        Chart2.ChartAreas[0].AxisX.Interval = 1;
                                        Chart2.Series["Series1"].IsValueShownAsLabel = true;
                                        Chart2.ChartAreas[0].AxisX.Title = "DEPARTMENT";
                                        Chart2.ChartAreas[0].AxisY.Title = "PERCENTAGE";
                                        Chart2.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Red;
                                        Chart2.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Green;
                                        Chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart2.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                                        Chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart2.Visible = true;
                                        Label1.Visible = true;
                                    }
                                    if (colName == "III Year")
                                    {
                                        chart3 = true;
                                        Chart3.Series[0].Points.AddXY(dt.Rows[r][0], dt.Rows[r][j + 2]);
                                        Chart3.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        Chart3.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        Chart3.ChartAreas[0].AxisX.RoundAxisValues();
                                        Chart3.ChartAreas[0].AxisX.Minimum = 0;
                                        Chart3.ChartAreas[0].AxisX.Interval = 1;
                                        Chart3.Series["Series1"].IsValueShownAsLabel = true;
                                        Chart3.ChartAreas[0].AxisX.Title = "DEPARTMENT";
                                        Chart3.ChartAreas[0].AxisY.Title = "PERCENTAGE";
                                        Chart3.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Red;
                                        Chart3.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Green;
                                        Chart3.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart3.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                                        Chart3.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart3.Visible = true;
                                        Label2.Visible = true;
                                    }
                                    if (colName == "IV Year")
                                    {
                                        chart4 = true;
                                        Chart4.Series[0].Points.AddXY(dt.Rows[r][0], dt.Rows[r][j + 2]);
                                        Chart4.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        Chart4.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        Chart4.ChartAreas[0].AxisX.RoundAxisValues();
                                        Chart4.ChartAreas[0].AxisX.Minimum = 0;
                                        Chart4.ChartAreas[0].AxisX.Interval = 1;
                                        Chart4.Series["Series1"].IsValueShownAsLabel = true;
                                        Chart4.ChartAreas[0].AxisX.Title = "DEPARTMENT";
                                        Chart4.ChartAreas[0].AxisY.Title = "PERCENTAGE";
                                        Chart4.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Red;
                                        Chart4.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Green;
                                        Chart4.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart4.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                                        Chart4.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                                        Chart4.Visible = true;
                                        Label3.Visible = true;
                                    }


                                }
                            }
                            if (chart1 == false)
                            {
                                Chart1.Visible = false;
                                lblYear.Visible = false;
                            }
                            if (chart2 == false)
                            {
                                Chart2.Visible = false;
                                Label1.Visible = false;
                            }
                            if (chart3 == false)
                            {
                                Chart3.Visible = false;
                                Label2.Visible = false;
                            }
                            if (chart4 == false)
                            {
                                Chart4.Visible = false;
                                Label3.Visible = false;
                            }
                        }
                        else
                        {
                            btnExcel.Visible = false;
                            btnPrint.Visible = false;
                            Chart1.Visible = false;
                            Chart2.Visible = false;
                            Chart3.Visible = false;
                            Chart4.Visible = false;
                            grdover.Visible = false;
                            lblYear.Visible = false;
                            Label1.Visible = false;
                            Label2.Visible = false;
                            Label3.Visible = false;
                        }


                    }
                    else
                    {
                        Label5.Text = "No Records Found";
                        Label5.Visible = true;
                        grdover.Visible = false;
                        lblYear.Visible = false;
                        Label1.Visible = false;
                        Label2.Visible = false;
                        Label3.Visible = false;
                        btnExcel.Visible = false;
                        btnPrint.Visible = false;
                        Chart1.Visible = false;
                        Chart2.Visible = false;
                        Chart3.Visible = false;
                        Chart4.Visible = false;
                    }
                }
            }
            else
            {
                Label7.Text = "To Sem Must Be Greater Than or Equal To From Sem";
                Label7.Visible = true;
                grdover.Visible = false;
                lblYear.Visible = false;
                Label1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                btnExcel.Visible = false;
                btnPrint.Visible = false;
                Chart1.Visible = false;
                Chart2.Visible = false;
                Chart3.Visible = false;
                Chart4.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //Label5.Text = ex.ToString();
            //Label5.Visible = true;
        }
    }
    protected void BtnGo_Click(object sender, EventArgs e)
    {
        if (txt_degree.Text != "--Select--" && txt_branch.Text != "--Select--")
        {
            grd();
        }
    }
    protected void grdover_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            int tempt = Convert.ToInt32(ViewState["temp_table"]);
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "PASS PERCENTAGE ANALYSIS";
            HeaderCell.ColumnSpan = tempt + 1;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            grdover.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }
    protected void grdover_DataBound(object sender, EventArgs e)
    {
        try
        {
            //Removed By Srinath 22 July 2015
            //string str2 = "select value from master_settings where settings='Academic year'";
            //ds = da.select_method_wo_parameter(str2, "text");
            //int tempt = Convert.ToInt32(ViewState["temp_table"]);
            //GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            //TableHeaderCell cell = new TableHeaderCell();
            //string[] vl;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    vl = ds.Tables[0].Rows[0]["value"].ToString().Split(',');

            //    cell.Text = "ACADAMIC YEAR-" + vl[0].ToString() + " - " + vl[1].ToString();
            //}
            //else
            //{
            //    cell.Text = "ACADAMIC YEAR-";
            //}
            //cell.ColumnSpan = tempt + 1;
            //row.Controls.Add(cell);
            //cell.HorizontalAlign = HorizontalAlign.Center;
            //grdover.HeaderRow.Parent.Controls.AddAt(0, row);
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdover_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                int NumCells = e.Row.Cells.Count;
                for (int i = 0; i < NumCells - 1; i++)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            int col = 0;
            for (int j = 1; j <= Convert.ToInt32(ViewState["temp_table"]); j++)
            {
                e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int count = 0;
                double overall = 0;
                e.Row.Cells[1].Text = "%";
                if (avgarray.Count > 0)
                {
                    for (int cou = 0; cou < avgarray.Count; cou++)
                    {
                        string agvalue = Convert.ToString(avgarray[cou]);
                        if (cou == 2 || cou == 5 || cou == 8 || cou == 11)
                        {
                            if (agvalue.Trim() != "")
                            {
                                overall = overall + Convert.ToDouble(agvalue);
                            }
                        }
                        string setval = Convert.ToString(Math.Round(Convert.ToDouble(agvalue), 2, MidpointRounding.AwayFromZero));
                        if (cou == 2 || cou == 5 || cou == 8 || cou == 11)
                        {
                            string[] spva = setval.Split('.');
                            if (spva.GetUpperBound(0) == 1)
                            {
                                if (spva[1].Length == 1)
                                {
                                    setval = setval + "0";
                                }
                            }
                            else
                            {
                                setval = setval + ".00";
                            }
                        }
                        e.Row.Cells[cou + 2].Text = setval;
                        count++;
                    }
                }
                GridViewRow gvr = e.Row;
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    int index = grdover.Rows.Count;
                    GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
                    TableCell cell = null;
                    cell = new TableCell();
                    cell.Text = "OverAll %";
                    cell.ColumnSpan = 2;
                    row.Cells.Add(cell);
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    grdover.Controls[0].Controls.Add(row);

                    overall = overall / (count / 3);
                    cell = new TableCell();
                    cell.Text = Convert.ToString(Math.Round(Convert.ToDouble(overall), 2, MidpointRounding.AwayFromZero)) + "%";
                    cell.ColumnSpan = count;
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    row.Cells.Add(cell);
                    grdover.Controls[0].Controls.Add(row);

                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable tempt = (DataTable)ViewState["tbl"];
                int index = grdover.Rows.Count;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableCell cell = null;
                cell = new TableCell();
                cell.Text = "";
                cell.ColumnSpan = 2;
                row.Cells.Add(cell);
                cell.HorizontalAlign = HorizontalAlign.Center;
                grdover.Controls[0].Controls.Add(row);
                if (addyear.Count > 0)
                {
                    for (int ad = 0; ad < addyear.Count; ad++)
                    {
                        string name = Convert.ToString(tempt.Columns[ad + 1].ColumnName);
                        cell = new TableCell();
                        cell.Text = Convert.ToString(addyear[name]);
                        cell.ColumnSpan = 1;
                        row.Cells.Add(cell);
                        cell.HorizontalAlign = HorizontalAlign.Center;
                        grdover.Controls[0].Controls.Add(row);
                    }
                }
            }


        }
        catch (Exception ex)
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition",
                "attachment;filename=PassPercentageAnalysis.xls");
            Response.ContentType = "applicatio/excel";
            BtnGo_Click(sender, e);
            StringWriter sw = new StringWriter(); ;
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            grdover.RenderControl(htm);
            grdover.DataBind();
            Response.Write(sw.ToString());
            Response.End();
            Response.Clear();

        }
        catch (Exception ex)
        {
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
            Response.AddHeader("content-disposition", "attachment;filename=PassPercentageAnalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdover.AllowPaging = false;
            grdover.Rows[0].HorizontalAlign = HorizontalAlign.Center;
            grdover.HeaderRow.Style.Add("width", "15%");
            grdover.HeaderRow.Style.Add("font-size", "10px");
            grdover.HeaderRow.Style.Add("text-align", "center");
            grdover.Style.Add("text-decoration", "none");
            grdover.Style.Add("font-family", "Book Antiqua;");
            grdover.Style.Add("font-size", "8px");
            BtnGo_Click(sender, e);
            grdover.RenderControl(hw);
            grdover.DataBind();

            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            htmlparser.Parse(sr);

            if (Chart1.Visible == true)
            {
                StringWriter sw1 = new StringWriter();
                HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
                Label lb1 = new Label();
                lb1.Text = "<br>" + lblYear.Text.ToString();
                lb1.Style.Add("height", "100px");
                lb1.Style.Add("text-decoration", "none");
                lb1.Style.Add("font-family", "Book Antiqua;");
                lb1.Style.Add("font-size", "8px");
                lb1.Style.Add("font-weight", "bold");
                lb1.Style.Add("text-align", "center");
                lb1.RenderControl(hw1);
                StringReader sr1 = new StringReader(sw1.ToString());
                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                htmlparser.Parse(sr1);

                using (MemoryStream stream = new MemoryStream())
                {

                    Chart1.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            if (Chart2.Visible == true)
            {
                StringWriter sw2 = new StringWriter();
                HtmlTextWriter hw2 = new HtmlTextWriter(sw2);
                Label lb2 = new Label();
                lb2.Text = "<br>" + Label1.Text.ToString();
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "8px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(hw2);
                StringReader sr2 = new StringReader(sw2.ToString());
                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                htmlparser.Parse(sr2);

                using (MemoryStream stream = new MemoryStream())
                {
                    Chart2.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            if (Chart3.Visible == true)
            {
                StringWriter sw3 = new StringWriter();
                HtmlTextWriter hw3 = new HtmlTextWriter(sw3);
                Label lb3 = new Label();
                lb3.Text = "<br>" + Label2.Text.ToString();
                lb3.Style.Add("height", "100px");
                lb3.Style.Add("text-decoration", "none");
                lb3.Style.Add("font-family", "Book Antiqua;");
                lb3.Style.Add("font-size", "8px");
                lb3.Style.Add("font-weight", "bold");
                lb3.Style.Add("text-align", "center");
                lb3.RenderControl(hw3);
                StringReader sr3 = new StringReader(sw3.ToString());
                HTMLWorker htmlparser3 = new HTMLWorker(pdfDoc);
                htmlparser3.Parse(sr3);

                using (MemoryStream stream = new MemoryStream())
                {
                    Chart3.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            if (Chart4.Visible == true)
            {
                StringWriter sw4 = new StringWriter();
                HtmlTextWriter hw4 = new HtmlTextWriter(sw4);
                Label lb2 = new Label();
                lb2.Text = "<br>" + Label3.Text.ToString();
                lb2.Style.Add("height", "100px");
                lb2.Style.Add("text-decoration", "none");
                lb2.Style.Add("font-family", "Book Antiqua;");
                lb2.Style.Add("font-size", "8px");
                lb2.Style.Add("font-weight", "bold");
                lb2.Style.Add("text-align", "center");
                lb2.RenderControl(hw4);
                StringReader sr4 = new StringReader(sw4.ToString());
                HTMLWorker htmlparser1 = new HTMLWorker(pdfDoc);
                htmlparser.Parse(sr4);

                using (MemoryStream stream = new MemoryStream())
                {

                    Chart4.SaveImage(stream, ChartImageFormat.Png);
                    iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
                    chartImage.ScalePercent(75f);
                    pdfDoc.Add(chartImage);
                }
            }

            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlbatch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {

            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                //  ddlsemfrom.Items.Clear();
                //  ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbatch();
            binddegree();
            bindbranch(bran);
            grdover.Visible = false;
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            lblYear.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            Label3.Visible = false;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
            if (ddlbatch.SelectedItem.Text != "All")
            {
                ddlsemfrom.Enabled = true;
                ddlsemto.Enabled = true;
                bindsemester();
            }
            else
            {
                //  ddlsemfrom.Items.Clear();
                //   ddlsemto.Items.Clear();
                ddlsemfrom.Enabled = false;
                ddlsemto.Enabled = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void grdover_PreRender(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in grdover.Rows)
        //{
        //    if (row.RowType == DataControlRowType.DataRow)
        //    {
        //        row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
        //    }
        //}
    }
}