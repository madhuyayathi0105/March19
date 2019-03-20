using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.Configuration;

public partial class DepartmentWiseInternaltestAnalysis : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable has = new Hashtable();
    Dictionary<int, string> testname = new Dictionary<int, string>();
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    int count = 0;
    DataTable dtdepart = new DataTable();

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
            bindsem();
            bindtestname();
            //for (int c = 0; c < chklscolumn.Items.Count; c++)
            //{
            //    chklscolumn.Items[c].Selected = true;
            //}
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_batch.DataSource = ds;
                Chklst_batch.DataTextField = "batch_year";
                Chklst_batch.DataValueField = "batch_year";
                Chklst_batch.DataBind();
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                    count++;
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
    public void binddegree()
    {
        try
        {
            Chklst_degree.Items.Clear();
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
    public void bindsem()
    {
        try
        {
            string degreecode = "";
            ddlsem.Items.Clear();
            for (int h = 0; h < chklst_branch.Items.Count; h++)
            {
                if (chklst_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = chklst_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + chklst_branch.Items[h].Value;
                    }
                }
            }
            string strgetfuncuti = da.GetFunction("select max(Duration) from Degree");
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = da.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            for (int loop_val = 1; loop_val <= Convert.ToInt16(strgetfuncuti); loop_val++)
            {
                ddlsem.Items.Add(loop_val.ToString());
            }

        }
        catch (Exception ex)
        {
            lbl_err.Text = ex.ToString();
            lbl_err.Visible = true;
        }
    }
    public void bindtestname()
    {
        try
        {
            ddltest.Items.Clear();
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
                string Sqlstr = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,Registration r where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester='" + ddlsem.SelectedItem.ToString() + "' and r.cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and sy.Batch_Year in(" + testbatchyear + ") and sy.degree_code in(" + testbranch + ") order by criteria";
                DataSet titles = new DataSet();
                titles.Clear();
                titles.Dispose();
                titles = da.select_method_wo_parameter(Sqlstr, "Test");
                if (titles.Tables[0].Rows.Count > 0)
                {
                    ddltest.DataSource = titles;
                    ddltest.DataValueField = "Criteria";
                    ddltest.DataTextField = "Criteria";
                    ddltest.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }

    protected void ddltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lbl_err.Visible = true;
            lbl_err.Text = ex.ToString();
        }
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            bindtestname();
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
            chk_degree.Checked = false;
            count = 0;
            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = "Degree(" + count.ToString() + ")";
                if (count == Chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            bindbranch();
            bindsem();
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
            bindsem();
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
            bindsem();
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
            count = 0;
            Chk_batch.Checked = false;
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }

            if (count > 0)
            {
                txt_batch.Text = "Batch(" + count.ToString() + ")";
                if (count == Chklst_batch.Items.Count)
                {
                    Chk_batch.Checked = true;
                }
            }
            binddegree();
            bindbranch();
            bindsem();
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
            count = 0;
            chk_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_branch.Text = "Branch(" + count.ToString() + ")";
                if (count == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
            bindsem();
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
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (Chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
            bindtestname();
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
        gridview1.Visible = false;
        lbl_err.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            DataRow dranal;
            clear();
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
            if (testbatchyear.Trim() == "")
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Batch And Then Proceed";
                return;
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
            if (testbranch.Trim() == "")
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Select The Degree and Branch And Then Proceed";
                return;
            }

            if (ddltest.Items.Count == 0)
            {
                lbl_err.Visible = true;
                lbl_err.Text = "No Test Conducted";
                return;
            }
            string strquery = "select distinct r.Batch_Year,r.degree_code,sy.semester,r.Sections,c.Criteria_no from Registration r,syllabus_master sy,CriteriaForInternal c,Exam_type e where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.batch_year=r.Batch_Year and e.sections=r.Sections and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ") and sy.semester='" + ddlsem.SelectedItem.ToString() + "' and c.criteria='" + ddltest.SelectedItem.ToString() + "' order by r.degree_code,r.Batch_Year desc,sy.semester,r.Sections ";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method_wo_parameter(strquery, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                gridview1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                btnPrint.Visible = true;

                ArrayList arrColHdrNames1 = new ArrayList();
                ArrayList arrColHdrNames2 = new ArrayList();
                Dictionary<int, string> diccolname = new Dictionary<int, string>();


                diccolname.Add(0, "No of Students");
                diccolname.Add(1, "No of Girl Students");
                diccolname.Add(2, "No of Boys Students");
                diccolname.Add(3, "No of Girl Hostel Students");
                diccolname.Add(4, "No of Boys Hostel Students");
                diccolname.Add(5, "No of Girl Day Scholar Students");
                diccolname.Add(6, "No of Boys Day Scholar Students");

                arrColHdrNames1.Add("SNo");
                arrColHdrNames1.Add("Year/Dept");
                arrColHdrNames1.Add("Strength");
                arrColHdrNames1.Add("Strength");
                arrColHdrNames1.Add("Strength");


                arrColHdrNames2.Add("SNo");
                arrColHdrNames2.Add("Year/Dept");
                arrColHdrNames2.Add("Boys");
                arrColHdrNames2.Add("Girls");
                arrColHdrNames2.Add("Total");



                dtdepart.Columns.Add("SNo");
                dtdepart.Columns.Add("Year/Dept");
                dtdepart.Columns.Add("Boys");
                dtdepart.Columns.Add("Girls");
                dtdepart.Columns.Add("Total");

                for (int i = 0; i < 7; i++)
                {
                    string conlname = diccolname[i];
                    arrColHdrNames1.Add(conlname);
                    arrColHdrNames1.Add(conlname);
                    arrColHdrNames1.Add(conlname);
                    arrColHdrNames1.Add(conlname);

                    arrColHdrNames2.Add("Appear");
                    arrColHdrNames2.Add("Pass");
                    arrColHdrNames2.Add("Fail");
                    arrColHdrNames2.Add("Pass %");

                    textpass = new System.Text.StringBuilder("Appear");

                    AddTableColumn(dtdepart, textpass);
                    textpass = new System.Text.StringBuilder("Pass");

                    AddTableColumn(dtdepart, textpass);
                    textpass = new System.Text.StringBuilder("Fail");

                    AddTableColumn(dtdepart, textpass);
                    textpass = new System.Text.StringBuilder("Pass %");

                    AddTableColumn(dtdepart, textpass);

                }

                DataRow drHdr1 = dtdepart.NewRow();
                DataRow drHdr2 = dtdepart.NewRow();
                for (int grCol = 0; grCol < dtdepart.Columns.Count; grCol++)
                {
                    drHdr1[grCol] = arrColHdrNames1[grCol];
                    drHdr2[grCol] = arrColHdrNames2[grCol];

                }

                dtdepart.Rows.Add(drHdr1);
                dtdepart.Rows.Add(drHdr2);


                gridview1.Visible = true;

                int srno = 0;
                string strstucountquery = "select isnull(count(r.roll_no),0) as stucount,r.degree_code,r.batch_year,c.Course_Name,de.Dept_Name,r.sections,a.sex from Registration r,applyn a,Degree d,Department de,Course c where r.App_No=a.app_no and d.Degree_Code=r.degree_code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + testbatchyear + ") and r.degree_code in (" + testbranch + ") group by r.degree_code,r.batch_year,c.Course_Name,de.Dept_Name,r.sections,a.sex";
                DataSet dsstu = da.select_method_wo_parameter(strstucountquery, "text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string batchyear = ds.Tables[0].Rows[i]["batch_year"].ToString();
                    string degree = ds.Tables[0].Rows[i]["degree_code"].ToString();
                    string section = ds.Tables[0].Rows[i]["sections"].ToString();
                    string criteriano = ds.Tables[0].Rows[i]["Criteria_no"].ToString();
                    string secval = "";
                    if (section.Trim() != "" && section.Trim() != "-1")
                    {
                        secval = " and sections='" + section + "'";
                    }
                    dsstu.Tables[0].DefaultView.RowFilter = "batch_year='" + batchyear + "' and degree_code='" + degree + "' " + secval + "";
                    DataView dvstucount = dsstu.Tables[0].DefaultView;
                    if (dvstucount.Count > 0)
                    {
                        dranal = dtdepart.NewRow();
                        srno++;
                        string degreedetails = dvstucount[0]["batch_year"].ToString() + " - " + dvstucount[0]["Course_Name"].ToString() + " - " + dvstucount[0]["Dept_Name"].ToString();
                        if (section.Trim() != "" && section.Trim() != "-1")
                        {
                            degreedetails = degreedetails + " - " + section;
                        }

                        dranal["SNo"] = srno.ToString();
                        dranal["Year/Dept"] = degreedetails;

                        //textpass = new System.Text.StringBuilder(srno.ToString());

                        //AddTableColumn(dtdepart, textpass);

                        int noofboys = 0;
                        int noofgirls = 0;
                        int nooftotal = 0;
                        for (int s = 0; s < dvstucount.Count; s++)
                        {
                            string sex = dvstucount[s]["sex"].ToString();
                            string stucount = dvstucount[s]["stucount"].ToString();
                            if (sex == "0")
                            {
                                noofboys = Convert.ToInt32(stucount);
                            }
                            else
                            {
                                noofgirls = Convert.ToInt32(stucount);
                            }
                        }
                        nooftotal = noofboys + noofgirls;
                        dranal["Boys"] = noofboys.ToString();
                        dranal["Girls"] = noofgirls.ToString();
                        dranal["Total"] = nooftotal.ToString();
                        dtdepart.Rows.Add(dranal);

                        string minmark = "";
                        string sec_examcode = "select distinct r.exam_code as exam_code,min_mark from exam_type e,subject s,result r where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no='" + criteriano + "'  " + secval + "  ";
                        string examcode = "";
                        DataSet dsexam = da.select_method_wo_parameter(sec_examcode, "text");
                        for (int ext = 0; ext < dsexam.Tables[0].Rows.Count; ext++)
                        {
                            minmark = dsexam.Tables[0].Rows[ext]["min_mark"].ToString();
                            if (examcode == "")
                            {
                                examcode = "'" + dsexam.Tables[0].Rows[ext]["exam_code"].ToString() + "'";
                            }
                            else
                            {
                                examcode = examcode + ",'" + dsexam.Tables[0].Rows[ext]["exam_code"].ToString() + "'";
                            }
                        }
                        if (examcode.Trim() != "")
                        {
                            Double overstuc = Convert.ToDouble(da.GetFunction("select isnull(count(distinct rt.roll_no),0) as 'allpass_count' from result r,registration rt where r.exam_code in(" + examcode + ")  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3'or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + secval + ""));
                            Double overappera = Convert.ToDouble(da.GetFunction("select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt where r.exam_code in(" + examcode + ")  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 " + secval + " "));
                            Double overfail = Convert.ToDouble(da.GetFunction("select isnull(count(distinct rt.roll_no),0) from result rt,registration r where rt.exam_Code in(" + examcode + ") and rt.roll_no=r.roll_no and r.degree_code='" + degree + "' and r.batch_year='" + batchyear + "' " + secval + " and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0  "));
                            Double overallpass = overstuc - overfail;
                            Double overallpss = overallpass / overappera * 100;
                            overallpss = Math.Round(overallpss, 2, MidpointRounding.AwayFromZero);
                            if (overallpss.ToString().Trim().ToLower() == "nan" || overallpss.ToString().Trim().ToLower() == "infinity")
                            {
                                overallpss = 0;
                            }

                            dtdepart.Rows[dtdepart.Rows.Count - 1][5] = overappera.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][6] = overallpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][7] = overfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][8] = overallpss.ToString();



                            string strdetail = "select isnull(count(distinct rt.roll_no),0) as 'appear',rt.Stud_Type,a.sex from result r,registration rt,applyn a where a.app_no=rt.App_No and r.exam_code  in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  " + secval + " group by rt.Stud_Type,a.sex ";
                            strdetail = strdetail + " select isnull(count(distinct rt.roll_no),0) as 'allpass_count',rt.Stud_Type,a.sex from result r,registration rt,applyn a where a.app_no=rt.App_No and r.exam_code in(" + examcode + ") and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3'or marks_obtained='-1')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  " + secval + " group by rt.Stud_Type,a.sex ";
                            strdetail = strdetail + " select r.Stud_Type,a.sex,count(rt.marks_obtained) as nooffailure,count(distinct rt.roll_no) as fail from result rt,registration r,applyn a where a.app_no=r.App_No and rt.exam_code in(" + examcode + ") and rt.roll_no=r.roll_no and r.degree_code='" + degree + "' and r.batch_year='" + batchyear + "' " + secval + " and (rt.marks_obtained<" + minmark + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2') and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 group by r.Stud_Type,a.sex,rt.roll_no";
                            DataSet dsdetails = da.select_method_wo_parameter(strdetail, "text");

                            Double gappear = 0, gtotal = 0, gfail = 0, gpass = 0;
                            Double bappear = 0, btotal = 0, bfail = 0, bpass = 0;
                            Double ghappear = 0, ghtotal = 0, ghfail = 0, ghpass = 0;
                            Double bhappear = 0, bhtotal = 0, bhfail = 0, bhpass = 0;
                            Double gdappear = 0, gdtotal = 0, gdfail = 0, gdpass = 0;
                            Double bdappear = 0, bdtotal = 0, bdfail = 0, bdpass = 0;

                            int onf = 0, tw0f = 0, thref = 0;
                            for (int aa = 0; aa < dsdetails.Tables[0].Rows.Count; aa++)
                            {
                                string asex = dsdetails.Tables[0].Rows[aa]["sex"].ToString().Trim();
                                string astype = dsdetails.Tables[0].Rows[aa]["Stud_Type"].ToString().Trim().ToLower();
                                string acount = dsdetails.Tables[0].Rows[aa]["appear"].ToString();

                                if (asex == "1")
                                {
                                    gappear = gappear + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        gdappear = gdappear + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        ghappear = ghappear + Convert.ToDouble(acount);
                                    }
                                }
                                else
                                {
                                    bappear = bappear + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        bdappear = bdappear + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        bhappear = bhappear + Convert.ToDouble(acount);
                                    }
                                }
                            }

                            for (int ast = 0; ast < dsdetails.Tables[1].Rows.Count; ast++)
                            {
                                string asex = dsdetails.Tables[1].Rows[ast]["sex"].ToString().Trim();
                                string astype = dsdetails.Tables[1].Rows[ast]["Stud_Type"].ToString().Trim().ToLower();
                                string acount = dsdetails.Tables[1].Rows[ast]["allpass_count"].ToString();
                                if (asex == "1")
                                {
                                    gtotal = gtotal + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        gdtotal = gdtotal + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        ghtotal = ghtotal + Convert.ToDouble(acount);
                                    }
                                }
                                else
                                {
                                    btotal = btotal + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        bdtotal = bdtotal + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        bhtotal = bhtotal + Convert.ToDouble(acount);
                                    }
                                }
                            }

                            for (int af = 0; af < dsdetails.Tables[2].Rows.Count; af++)
                            {
                                string asex = dsdetails.Tables[2].Rows[af]["sex"].ToString().Trim();
                                string astype = dsdetails.Tables[2].Rows[af]["Stud_Type"].ToString().Trim().ToLower();
                                string acount = dsdetails.Tables[2].Rows[af]["fail"].ToString();
                                string nfs = dsdetails.Tables[2].Rows[af]["nooffailure"].ToString();

                                if (nfs == "1")
                                {
                                    onf++;
                                }
                                else if (nfs == "1")
                                {
                                    tw0f++;
                                }
                                else
                                {
                                    thref++;
                                }

                                if (asex == "1")
                                {
                                    gfail = gfail + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        gdfail = gdfail + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        ghfail = ghfail + Convert.ToDouble(acount);
                                    }
                                }
                                else
                                {
                                    bfail = bfail + Convert.ToDouble(acount);
                                    if (astype == "day scholar")
                                    {
                                        bdfail = bdfail + Convert.ToDouble(acount);
                                    }
                                    else
                                    {
                                        bhfail = bhfail + Convert.ToDouble(acount);
                                    }
                                }
                            }
                            //Total Number of Girl Students

                            gpass = gtotal - gfail;
                            Double gallpassperec = gpass / gappear * 100;
                            if (gallpassperec.ToString().Trim().ToLower() == "nan" || gallpassperec.ToString().Trim().ToLower() == "infinity")
                            {
                                gallpassperec = 0;
                            }
                            gallpassperec = Math.Round(gallpassperec, 2, MidpointRounding.AwayFromZero);

                            dtdepart.Rows[dtdepart.Rows.Count - 1][9] = gappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][10] = gpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][11] = gfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][12] = gallpassperec.ToString();

                            //Total Number of Boy Students
                            bpass = btotal - bfail;
                            Double ballpassperec = bpass / bappear * 100;
                            if (ballpassperec.ToString().Trim().ToLower() == "nan" || ballpassperec.ToString().Trim().ToLower() == "infinity")
                            {
                                ballpassperec = 0;
                            }
                            ballpassperec = Math.Round(ballpassperec, 2, MidpointRounding.AwayFromZero);

                            dtdepart.Rows[dtdepart.Rows.Count - 1][13] = bappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][14] = bpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][15] = bfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][16] = ballpassperec.ToString();



                            //Total Number of Girl Hostel Students

                            ghpass = ghtotal - ghfail;
                            Double ghpassper = ghpass / ghappear * 100;
                            if (ghpassper.ToString().Trim().ToLower() == "nan" || ghpassper.ToString().Trim().ToLower() == "infinity")
                            {
                                ghpassper = 0;
                            }
                            ghpassper = Math.Round(ghpassper, 2, MidpointRounding.AwayFromZero);


                            dtdepart.Rows[dtdepart.Rows.Count - 1][17] = ghappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][18] = ghpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][19] = ghfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][20] = ghpassper.ToString();

                            //Total Number of Boys Hostel Students";

                            bhpass = bhtotal - bhfail;
                            Double bhpassper = bhpass / bhappear * 100;
                            if (bhpassper.ToString().Trim().ToLower() == "nan" || bhpassper.ToString().Trim().ToLower() == "infinity")
                            {
                                bhpassper = 0;
                            }
                            bhpassper = Math.Round(bhpassper, 2, MidpointRounding.AwayFromZero);

                            dtdepart.Rows[dtdepart.Rows.Count - 1][21] = bhappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][22] = bhpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][23] = bhfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][24] = bhpassper.ToString();


                            //Total Number of Girl Day Scholar Students";

                            gdpass = gdtotal - gdfail;
                            Double gdpassper = gdpass / gdappear * 100;
                            if (gdpassper.ToString().Trim().ToLower() == "nan" || gdpassper.ToString().Trim().ToLower() == "infinity")
                            {
                                gdpassper = 0;
                            }
                            gdpassper = Math.Round(gdpassper, 2, MidpointRounding.AwayFromZero);


                            dtdepart.Rows[dtdepart.Rows.Count - 1][25] = gdappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][26] = gdpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][27] = gdfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][28] = gdpassper.ToString();

                            //Total Number of Boys Day Scholar Students";
                            bdpass = bdtotal - bdfail;
                            Double bdpassper = bdpass / bdappear * 100;
                            if (bdpassper.ToString().Trim().ToLower() == "nan" || bdpassper.ToString().Trim().ToLower() == "infinity")
                            {
                                bdpassper = 0;
                            }
                            bdpassper = Math.Round(bdpassper, 2, MidpointRounding.AwayFromZero);

                            dtdepart.Rows[dtdepart.Rows.Count - 1][29] = bdappear.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][30] = bdpass.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][31] = bdfail.ToString();
                            dtdepart.Rows[dtdepart.Rows.Count - 1][32] = bdpassper.ToString();

                        }
                    }
                }

                if (dtdepart.Columns.Count > 0 && dtdepart.Rows.Count > 2)
                {
                    gridview1.DataSource = dtdepart;
                    gridview1.DataBind();
                    gridview1.Visible = true;


                    int rowcnt = gridview1.Rows.Count - 2;
                    //Rowspan
                    for (int rowIndex = gridview1.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = gridview1.Rows[rowIndex];
                        GridViewRow previousRow = gridview1.Rows[rowIndex + 1];
                        gridview1.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        gridview1.Rows[rowIndex].Font.Bold = true;
                        gridview1.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }

                        }


                    }

                    //ColumnSpan
                    for (int rowIndex = gridview1.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {


                        for (int cell = gridview1.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = gridview1.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = gridview1.Rows[rowIndex].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;

                                }
                            }
                        }

                    }
                }
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
    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < dtdepart.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int j = 0; j < dtdepart.Columns.Count; j++)
                {
                    if (j != 1)
                        e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
                }
            }

        }
        catch
        {


        }

    }


    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreportgrid(gridview1, reportname);
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

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string ss = Session["usercode"].ToString();
        string degreedetails = "Department Wise Internal Exam Result Analysis";
        string pagename = "DepartmentWiseInternaltestAnalysis.aspx";
        Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Department Wise Internal Exam Result Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}