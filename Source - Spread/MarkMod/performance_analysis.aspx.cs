using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using InsproDataAccess;
using System.Configuration;


public partial class performance_analysis : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable has = new Hashtable();
    Dictionary<int, string> testname = new Dictionary<int, string>();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "", bran = "", buildvalue = "", build = "";
    int cout = 0;
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();

    #region Kowshi
    DataTable dtperformance = new DataTable();
    DataRow dranalysis;
    static ArrayList rowtest = new ArrayList();
    static ArrayList rowhead = new ArrayList();
    #endregion


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
        lbl_err.Visible = false;
        if (!IsPostBack)
        {
            clear();
            bindbatch();
            binddegree();
            bindbranch();
            bindtestname();
        }
    }

    public void binddegree()
    {
        try
        {
            txt_degree.Text = "---Select---";
            chk_degree.Checked = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = Session["collegecode"].ToString();
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();

                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    Chklst_degree.Items[h].Selected = true;
                }
                txt_degree.Text = "Degree" + "(" + Chklst_degree.Items.Count + ")";
                chk_degree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindbranch()
    {
        try
        {
            string degreecode = "";
            txt_branch.Text = "---Select---";
            chk_branch.Checked = false;
            chklst_branch.Items.Clear();
            for (int h = 0; h < Chklst_degree.Items.Count; h++)
            {
                if (Chklst_degree.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = Chklst_degree.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + Chklst_degree.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degreecode, collegecode = Session["collegecode"].ToString(), Session["usercode"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();
                    for (int h = 0; h < chklst_branch.Items.Count; h++)
                    {
                        chklst_branch.Items[h].Selected = true;
                    }
                    txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
                    chk_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    public void bindtestname()
    {
        try
        {
            chklsttest.Items.Clear();
            txttest.Text = "---Select---";
            chktest.Checked = false;
            string testbatchyear = "";
            for (int j = 0; j < Chklst_batch.Items.Count; j++)
            {
                if (Chklst_batch.Items[j].Selected == true)
                {
                    if (testbatchyear == "")
                    {
                        testbatchyear = "'" + Chklst_batch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbatchyear = testbatchyear + ",'" + Chklst_batch.Items[j].Value.ToString() + "'";
                    }
                }
            }

            string testbranch = "";
            for (int j = 0; j < chklst_branch.Items.Count; j++)
            {
                if (chklst_branch.Items[j].Selected == true)
                {
                    if (testbranch == "")
                    {
                        testbranch = "'" + chklst_branch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbranch = testbranch + ",'" + chklst_branch.Items[j].Value.ToString() + "'";
                    }
                }
            }
            if (testbatchyear.Trim() != "" && testbranch.Trim() != "")
            {
                string Sqlstr = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,Registration r where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester=r.Current_Semester and r.cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and sy.Batch_Year in(" + testbatchyear + ") and sy.degree_code in(" + testbranch + ") order by criteria";
                DataSet titles = new DataSet();
                titles.Clear();
                titles.Dispose();
                titles = da.select_method_wo_parameter(Sqlstr, "Test");
                if (titles.Tables[0].Rows.Count > 0)
                {
                    chklsttest.DataSource = titles;
                    chklsttest.DataValueField = "Criteria";
                    chklsttest.DataTextField = "Criteria";
                    chklsttest.DataBind();
                    for (int i = 0; i < chklsttest.Items.Count; i++)
                    {
                        chklsttest.Items[i].Selected = true;
                    }
                    txttest.Text = "Test(" + (chklsttest.Items.Count) + ")";
                    chktest.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chktest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chktest.Checked == true)
            {
                for (int i = 0; i < chklsttest.Items.Count; i++)
                {
                    chklsttest.Items[i].Selected = true;
                }
                txttest.Text = "Test(" + chklsttest.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < chklsttest.Items.Count; i++)
                {
                    chklsttest.Items[i].Selected = false;
                }
                txttest.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chklsttest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            chktest.Checked = false;
            txttest.Text = "---Select---";
            for (int i = 0; i < chklsttest.Items.Count; i++)
            {
                if (chklsttest.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txttest.Text = "Test(" + cout + ")";
                if (cout == chklsttest.Items.Count)
                {
                    chktest.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_degree.Text = "--Select--";
            int seatcount = 0;
            chk_degree.Checked = false;

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
            if (seatcount > 0)
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                if (seatcount == Chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            bindbranch();
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (chklst_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (Chk_batch.Checked == true)
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }

            binddegree();
            bindbranch();
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void Chlk_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_batch.Text = "--Select--";
            int seatcount = 0;
            Chk_batch.Checked = false;
            string buildvalue = "";
            string build = "";

            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
                    build = Chklst_batch.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }

            if (seatcount > 0)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                if (seatcount == Chklst_batch.Items.Count)
                {
                    Chk_batch.Checked = true;
                }
            }
            binddegree();
            bindbranch();
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }


    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            int seatcount = 0;
            chk_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount > 0)
            {
                txt_branch.Text = "Branch(" + seatcount.ToString() + ")";
                if (seatcount == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string Chklstbatchvalue = "";
            string bind1 = "";

            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    if (chk_degree.Checked == true)
                    {
                        Chklst_degree.Items[i].Selected = true;
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
                }
                txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                    Chklst_degree.ClearSelection();
                }
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
                chk_branch.Checked = false;
            }

            bindbranch();
            bindtestname();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    public void bindbatch()
    {
        try
        {
            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;

            if (count > 0)
            {
                Chklst_batch.DataSource = ds;
                Chklst_batch.DataTextField = "batch_year";
                Chklst_batch.DataValueField = "batch_year";
                Chklst_batch.DataBind();
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                }
                if (count > 0)
                {
                    txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
                    if (Chklst_batch.Items.Count == count)
                    {
                        Chk_batch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void btngo1(object sender, EventArgs e)
    {
        try
        {
            clear();
            rowhead.Clear();
            rowtest.Clear();
            DataSet dscreteria = new DataSet();
            dtperformance.Columns.Add("SNo");
            dtperformance.Columns.Add("Batch");
            dtperformance.Columns.Add("Department");
            rowtest.Add("SNo");
            rowtest.Add("Batch");
            rowtest.Add("Department");

            rowhead.Add("SNo");
            rowhead.Add("Batch");
            rowhead.Add("Department");

            dranalysis = dtperformance.NewRow();
            dtperformance.Rows.Add(dranalysis);
            dranalysis = dtperformance.NewRow();
            dtperformance.Rows.Add(dranalysis);

            string batchva = "";
            string strbatch = "";
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    if (batchva == "")
                    {
                        batchva = Chklst_batch.Items[i].Text;
                    }
                    else
                    {
                        batchva = batchva + ',' + Chklst_batch.Items[i].Text;
                    }
                }
            }
            if (batchva.Trim() != "")
            {
                strbatch = " and r.batch_year in(" + batchva + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Batch And Then Proceed";
                return;
            }

            string degrees = "";
            string degreequery = "";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    if (degrees == "")
                    {
                        degrees = chklst_branch.Items[i].Value;
                    }
                    else
                    {
                        degrees = degrees + ',' + chklst_branch.Items[i].Value;
                    }
                }
            }
            if (degrees.Trim() != "")
            {
                degreequery = " and r.Degree_code in(" + degrees + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Degree, Branch And Then Proceed";
                return;
            }


            string strcriexamcode = "select distinct sy.Batch_Year,sy.degree_code,sy.semester,c.criteria,c.criteria_no,e.exam_code,e.min_mark from syllabus_master sy,CriteriaForInternal c,Exam_type e,Registration r where sy.syll_code=c.syll_code ";
            strcriexamcode = strcriexamcode + " and c.Criteria_no=e.criteria_no and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' " + strbatch + " " + degreequery + "";
            DataSet dscriterquery = da.select_method_wo_parameter(strcriexamcode, "Text");

            string testva = "";
            string testquery = "";
            for (int i = 0; i < chklsttest.Items.Count; i++)
            {
                if (chklsttest.Items[i].Selected == true)
                {
                    if (testva == "")
                    {
                        testva = "'" + chklsttest.Items[i].Value + "'";
                    }
                    else
                    {
                        testva = batchva + ",'" + chklsttest.Items[i].Value + "'";
                    }
                }
            }
            if (testva.Trim() != "")
            {
                testquery = " and c.criteria in(" + testva + ")";
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Test And Then Proceed";
                return;
            }
            int testcnt = 1;

            for (int i = 0; i < chklsttest.Items.Count; i++)
            {
                if (chklsttest.Items[i].Selected == true)
                {
                    dranalysis = dtperformance.NewRow();
                    string valuess = chklsttest.Items[i].Text;
                    string textcode = chklsttest.Items[i].Value;
                    testname.Add(testcnt, valuess.ToString());

                    rowtest.Add(valuess);
                    testcnt = testcnt + 3;
                    textpass = new System.Text.StringBuilder("Pass%");

                    AddTableColumn(dtperformance, textpass);

                    rowhead.Add("Pass%");
                }

            }

            Boolean rowflag = false;
            int sno = 0;
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Degree", typeof(string));
            dt2.Columns.Add("PASS", typeof(double));
           
            for (int b = 0; b < Chklst_batch.Items.Count; b++)
            {
               
                if (Chklst_batch.Items[b].Selected == true)
                {
                    string batchyear = Chklst_batch.Items[b].Text;
                    for (int d = 0; d < chklst_branch.Items.Count; d++)
                    {
                        if (chklst_branch.Items[d].Selected == true)
                        {
                            string degreecode = chklst_branch.Items[d].Value.ToString();
                            int col = 2;
                            Boolean bolrow = false;


                            for (int i = 0; i < chklsttest.Items.Count; i++)
                            {
                                if (chklsttest.Items[i].Selected == true)
                                {
                                    col++;
                                    //string strstuquery = "select isnull(count(distinct re.roll_no),0) as 'appear' from Registration r,Result re,CriteriaForInternal c,syllabus_master sy,Exam_type e where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and c.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_No=re.roll_no and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and e.sections=r.sections and c.criteria='" + chklsttest.Items[i].Text + "' and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "'  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3' or marks_obtained='-1')";
                                    //string stucount = da.GetFunction(strstuquery);

                                    //string strappearquery = "select isnull(count(distinct re.roll_no),0) as 'appear' from Registration r,Result re,CriteriaForInternal c,syllabus_master sy,Exam_type e where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and c.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_No=re.roll_no and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0  and e.sections=r.sections and c.criteria='" + chklsttest.Items[i].Text + "' and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "' and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') ";
                                    //string apperarcount = da.GetFunction(strappearquery);

                                    //string strfailquery = "select isnull(count(distinct re.roll_no),0) as 'failcount' from Registration r,Result re,CriteriaForInternal c,syllabus_master sy,Exam_type e where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and c.syll_code=sy.syll_code and e.batch_year=r.Batch_Year and c.criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Roll_No=re.roll_no and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and e.sections=r.sections and c.criteria='" + chklsttest.Items[i].Text + "' and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "' and r.RollNo_Flag<>0  and (marks_obtained<e.min_mark or marks_obtained='-2' or marks_obtained='-3')";
                                    //string strfailcount = da.GetFunction(strfailquery);

                                    string minmark = "";
                                    dscriterquery.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and criteria='" + chklsttest.Items[i].Text + "'";
                                    DataView dvcriteria = dscriterquery.Tables[0].DefaultView;
                                    string in_sec_examcode = "";
                                    for (int ci = 0; ci < dvcriteria.Count; ci++)
                                    {
                                        if (in_sec_examcode == "")
                                        {
                                            in_sec_examcode = dvcriteria[ci]["exam_code"].ToString();
                                        }
                                        else
                                        {
                                            in_sec_examcode = in_sec_examcode + "," + dvcriteria[ci]["exam_code"].ToString();
                                        }
                                        minmark = dvcriteria[ci]["min_mark"].ToString();
                                    }
                                    in_sec_examcode = " in(" + in_sec_examcode + ")";
                                    string strstuquery = "select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code " + in_sec_examcode.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3'or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 ";
                                    string stucount = da.GetFunction(strstuquery);

                                    string strappearquery = "select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code " + in_sec_examcode.ToString() + "  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0";
                                    string apperarcount = da.GetFunction(strappearquery);

                                    string strfailquery = "select isnull(count(distinct rt.roll_no),0) from result rt,registration r,Exam_type e  where rt.exam_Code " + in_sec_examcode.ToString() + " and rt.roll_no=r.roll_no and r.Sections=e.sections and rt.exam_code=e.exam_code and r.degree_code='" + degreecode + "' and r.batch_year=" + batchyear + " and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2' and rt.marks_obtained<>'-18') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0  ";
                                    string strfailcount = da.GetFunction(strfailquery);
                                   
                                    int apperarstu = 0;
                                    int passstu = 0;
                                    int failstu = 0;
                                    int val = 0;
                                    if (apperarcount.Trim() != "0" || strfailcount.Trim() != "0")
                                    {
                                        rowflag = true;
                                        apperarstu = Convert.ToInt32(apperarcount);
                                        failstu = Convert.ToInt32(strfailcount);
                                        passstu = Convert.ToInt32(stucount) - failstu;
                                        Double getpercentage = Convert.ToDouble(passstu) / Convert.ToDouble(apperarstu) * 100;
                                        getpercentage = Math.Round(getpercentage, 2, MidpointRounding.AwayFromZero);
                                       
                                        if (bolrow == false)
                                        {
                                            sno++;
                                            dranalysis = dtperformance.NewRow();
                                            val = dtperformance.Rows.Count;
                                          
                                            dranalysis["SNo"] = Convert.ToString(sno); 
                                            dranalysis["Batch"] = batchyear.ToString();
                                            dranalysis["Department"] = chklst_branch.Items[d].Text.ToString();
                                            bolrow = true;
                                            dtperformance.Rows.Add(dranalysis);
                                        }
                                        dtperformance.Rows[dtperformance.Rows.Count - 1][col] = getpercentage.ToString();
                                        DataRow dr2 = dt2.NewRow();
                                        dr2[0] = batchyear + " - " + chklst_branch.Items[d].Text.ToString() + " - " + chklsttest.Items[i].Text;
                                        dr2[1] = getpercentage.ToString();
                                        dt2.Rows.Add(dr2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            gridperfomance.DataSource = dtperformance;
            gridperfomance.DataBind();
            RowHead(gridperfomance, 2);
            RowHeadSpan(gridperfomance);
            gridperfomance.Visible = true;
            if (rowflag == true)
            {
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
                Chart1.ChartAreas[0].AxisX.Title = "Degree";
                Chart1.ChartAreas[0].AxisY.Title = "PASS%";
                Chart1.Series["Series1"].XValueMember = "Degree";
                Chart1.Series["Series1"].YValueMembers = "PASS";
                Chart1.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Book Antiqua", 8f);
                Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;

                gridperfomance.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Records Found";
            }

        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#008080");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    protected void RowHeadSpan(GridView gview)
    {
        for (int row = 2; row > 0; row--)
        {
            GridViewRow roww = gview.Rows[row];
            GridViewRow previousRow = gview.Rows[row - 1];
            for (int cell = 0; cell < gview.Rows[row].Cells.Count; cell++)
            {
                if (gview.HeaderRow.Cells[cell].Text.Trim() == "SNo" ||
                    gview.HeaderRow.Cells[cell].Text.Trim() == "Batch" || gview.HeaderRow.Cells[cell].Text.Trim() == "Department")
                {
                    if (roww.Cells[cell].Text == previousRow.Cells[cell].Text)
                    {
                        if (previousRow.Cells[cell].RowSpan == 0)
                        {
                            if (roww.Cells[cell].RowSpan == 0)
                            {
                                previousRow.Cells[cell].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[cell].RowSpan = roww.Cells[cell].RowSpan + 1;
                            }
                            roww.Cells[cell].Visible = false;
                        }
                    }
                }
            }
        }
    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                {
                    for (int cell = 0; cell < e.Row.Cells.Count; cell++)
                    {
                        e.Row.Cells[cell].Text = Convert.ToString(rowtest[cell]);
                    }
                }
                if (e.Row.RowIndex == 1)
                {
                    for (int cell = 0; cell < e.Row.Cells.Count; cell++)
                    {
                        e.Row.Cells[cell].Text = Convert.ToString(rowhead[cell]);
                    }
                }
            }
        }
        catch
        {


        }

    }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "Internal Exam Perforamnce Analysis Report";
        string pagename = "performance_analysis.aspx";
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(gridperfomance, pagename, degreedetails, 0, ss);

        Printcontrol.Visible = true;
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreportgrid(gridperfomance, reportname);
                lbl_err.Visible = false;
            }
            else
            {
                lbl_err.Text = "Please Enter Your Report Name";
                lbl_err.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }


    public void clear()
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = "";
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        gridperfomance.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
    }
}