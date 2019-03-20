using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class YearwiseResultAnalysis : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string selectQuery = string.Empty;

    string college = string.Empty;
    string batch = string.Empty;
    string degree = string.Empty;
    string dept = string.Empty;
    string sem = string.Empty;
    string sec = string.Empty;
    string testname = string.Empty;

    int i, row, commcount = 0;

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lbl_validation.Visible = false;
            if (!IsPostBack)
            {
                bindclg();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                bindsec();
                bindtestname();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divspread.Visible = false;
                rptprint.Visible = false;
            }
        }
        catch(Exception ex)
        {
        }
    }
    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clrSpread();

            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void ddl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
        binddeg();
        binddept();
        bindsem();
        bindsec();
        bindtestname();
    }
    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
        binddept();
        bindsem();
        bindsec();
        bindtestname();
    }
    protected void ddl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
        bindsem();
        bindsec();
        bindtestname();
    }
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        clrSpread();
        try
        {
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {

                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindtestname();
        }
        catch { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
        try
        {
            i = 0;
            cb_sem.Checked = false;
            commcount = 0;
            txt_sem.Text = "--Select--";
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
            bindtestname();
        }
        catch { }
    }
    protected void cb_sec_OnCheckedChanged(object sender, EventArgs e)
    {
        clrSpread();
        try
        {
            txt_sec.Text = "--Select--";
            if (cb_sec.Checked == true)
            {

                for (i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    protected void cbl_sec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
        try
        {
            i = 0;
            cb_sec.Checked = false;
            commcount = 0;
            txt_sec.Text = "--Select--";
            for (i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec.Items.Count)
                {
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }

        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {

        Printcontrol.Visible = false;

        int semcountcheck = 0;
        int seccountcheck = 0;
        for (i = 0; i < cbl_sem.Items.Count; i++)
        {
            if (cbl_sem.Items[i].Selected == true)
            {
                semcountcheck++;
            }
        }
        for (i = 0; i < cbl_sec.Items.Count; i++)
        {
            if (cbl_sec.Items[i].Selected == true)
            {
                seccountcheck++;
            }
        }
        if (ddl_test.Items.Count > 0 && semcountcheck > 0 && seccountcheck > 0)
        {

            try
            {
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Visible = false;
                divspread.Visible = false;
                rptprint.Visible = false;
                lbl_error.Text = "No Records Found";
                lbl_error.Visible = true;

                #region Get Input
                college = string.Empty;
                batch = string.Empty;
                degree = string.Empty;
                dept = string.Empty;
                sem = string.Empty;
                sec = string.Empty;
                testname = string.Empty;
                int secCount = 0;

                if (ddl_college.Items.Count > 0 && ddl_batch.Items.Count > 0 && ddl_degree.Items.Count > 0 && ddl_dept.Items.Count > 0)
                {
                    college = Convert.ToString(ddl_college.SelectedValue);

                    batch = Convert.ToString(ddl_batch.SelectedValue);

                    degree = Convert.ToString(ddl_degree.SelectedValue);

                    dept = Convert.ToString(ddl_dept.SelectedValue);

                    sem = "";
                    for (i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        if (cbl_sem.Items[i].Selected == true)
                        {
                            if (sem == "")
                            {
                                sem = Convert.ToString(cbl_sem.Items[i].Value);
                            }
                            else
                            {
                                sem += "," + Convert.ToString(cbl_sem.Items[i].Value);
                            }
                        }

                    }

                    sec = "";
                    for (i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        if (cbl_sec.Items[i].Selected == true)
                        {
                            if (sec == "")
                            {
                                sec = Convert.ToString(cbl_sec.Items[i].Value);
                            }
                            else
                            {
                                sec += "','" + Convert.ToString(cbl_sec.Items[i].Value);
                            }
                            secCount++;
                        }

                    }

                    testname = "";
                    if (ddl_test.Items.Count > 0)
                    {
                        testname = Convert.ToString(ddl_test.SelectedValue);
                    }

                }
                #endregion

                if (batch != "" && degree != "" && dept != "" && sem != "" && sec != "" && testname != "")
                {
                    selectQuery = "";
                    selectQuery = "  select distinct r.exam_code as exam_code,sy.semester,e.sections,c.criteria  from exam_type e,subject s,result r,CriteriaForInternal c,syllabus_master sy where e.subject_no=s.subject_no and e.exam_code= r.exam_code and e.criteria_no =c.Criteria_no and sy.syll_code =c.syll_code  and c.criteria ='" + testname + "' and sy.Batch_Year =" + batch + " and sy.degree_code =" + dept + " and sy.semester in (" + sem + ") order by semester,sections asc";

                    //selectQuery += " select s.degree_code,s.semester,c.criteria,c.min_mark,s.Batch_Year from criteriaforinternal c,syllabus_master s where s.syll_code =c.syll_code and criteria in ( '" + testname + "') and s.degree_code in (" + dept + ") and s.Batch_Year in (" + batch + ") and semester in (" + sem + ")";

                    //selectQuery += " select s.degree_code,s.semester,c.criteria,c.min_mark,s.Batch_Year from criteriaforinternal c,syllabus_master s where s.syll_code =c.syll_code and criteria in ( '" + testname + "') and s.degree_code in (" + dept + ") and  semester in (" + sem + ")";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQuery, "Text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        #region column header
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 3;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].Width = 50;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Year/ Semester";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Section";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);


                        FpSpread1.Sheets[0].ColumnCount += 3;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = testname;

                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;


                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = "App";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Pass";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "%";

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;


                        FpSpread1.Sheets[0].ColumnCount += 3;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = "Overall";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 3, 1, 3);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = "App";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Pass";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "%";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                        #endregion
                        #region row values

                        int serialno = 0;
                        int rowindex = 0;
                        Hashtable hat_semStrength = new Hashtable();
                        double semStrength = 0;
                        double semAggregate1 = 0;
                        double semAggregate2 = 0;

                        double appearedAvg = 0;
                        double passAvg = 0;
                        double percentAvg = 0;
                        int semcount = 0;


                        for (int semrow = 0; semrow < cbl_sem.Items.Count; semrow++)
                        {
                            if (cbl_sem.Items[semrow].Selected == true)
                            {
                                serialno++;
                                semStrength = 0;
                                int columnindex = 3;
                                semcount = 0;
                                appearedAvg = 0;
                                passAvg = 0;
                                percentAvg = 0;
                                for (int secrow = 0; secrow < cbl_sec.Items.Count; secrow++)
                                {
                                    if (cbl_sec.Items[secrow].Selected == true)
                                    {
                                        semcount++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].Text = serialno.ToString();
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Name = "Book Antiqua";

                                        FpSpread1.Sheets[0].Cells[rowindex, 1].Text = Convert.ToString(returnYear(cbl_sem.Items[semrow].Text)) + "/" + Convert.ToString(romanLetter(cbl_sem.Items[semrow].Text));
                                        FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[rowindex, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[rowindex, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Name = "Book Antiqua";


                                        FpSpread1.Sheets[0].Cells[rowindex, 2].Text = Convert.ToString(cbl_sec.Items[secrow].Value);
                                        FpSpread1.Sheets[0].Cells[rowindex, 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[rowindex, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[rowindex, 2].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[rowindex, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[rowindex, 2].Font.Name = "Book Antiqua";

                                        //for (int testrow = 0; testrow < cbl_test.Items.Count; testrow++)
                                        //{
                                        //    if (cbl_test.Items[testrow].Selected == true)
                                        //    {
                                        double appeared = 0;
                                        double pass = 0;
                                        double percent = 0;
                                        string sec_new = "";
                                        string sec_new1 = "";
                                        if (cbl_sec.Items[secrow].Value.Trim() != "")
                                        {
                                            sec_new = "and Sections='" + Convert.ToString(cbl_sec.Items[secrow].Value) + "'";
                                        }
                                        else
                                        {
                                            sec_new = "";
                                        }

                                        if (cbl_sec.Items[secrow].Value.Trim() != "")
                                        {
                                            sec_new1 = "and rt.Sections='" + Convert.ToString(cbl_sec.Items[secrow].Value) + "'";
                                        }
                                        else
                                        {
                                            sec_new1 = "";
                                        }

                                        ds.Tables[0].DefaultView.RowFilter = " semester='" + Convert.ToString(cbl_sem.Items[semrow].Text) + "' and criteria='" + Convert.ToString(ddl_test.SelectedValue) + "' " + sec_new + "";
                                        DataView excode = ds.Tables[0].DefaultView;

                                        if (excode.Count > 0)
                                        {
                                            string examcode = "";

                                            for (int z = 0; z < excode.Count; z++)
                                            {
                                                if (examcode == "")
                                                {
                                                    examcode = Convert.ToString(excode[z]["exam_code"]);
                                                }
                                                else
                                                {
                                                    examcode += "," + Convert.ToString(excode[z]["exam_code"]);
                                                }
                                            }


                                            selectQuery = "";
                                            selectQuery = " select isnull(count(distinct rt.roll_no),0) as 'appear' from result r,registration rt,Exam_type e,CriteriaForInternal c where e.exam_code=r.exam_code and e.criteria_no =c.criteria_no and  (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria ='" + Convert.ToString(ddl_test.SelectedValue) + "' and  r.exam_code in(" + examcode + ")";

                                            selectQuery += " select min_mark from criteriaforinternal where criteria ='" + Convert.ToString(ddl_test.SelectedValue) + "'";
                                            DataSet dsNew = new DataSet();
                                            dsNew = d2.select_method_wo_parameter(selectQuery, "Text");
                                            //ds.Tables[0].DefaultView.RowFilter = " degree_code='" + dept + "' and Semester='" + Convert.ToString(cbl_sem.Items[semrow].Text) + "' and criteria='" + testname + "' and Sections='" + Convert.ToString(cbl_sec.Items[secrow].Value) + "'";
                                            //DataView dvAppear = ds.Tables[0].DefaultView;

                                            //ds.Tables[1].DefaultView.RowFilter = "degree_code='" + dept + "' and Semester='" + Convert.ToString(cbl_sem.Items[semrow].Text) + "' and criteria='" + testname + "'";
                                            //DataView dvMin = ds.Tables[1].DefaultView;
                                            if (dsNew.Tables[0].Rows.Count > 0)
                                            {

                                                //string min = Convert.ToString(dvMin[0]["min_mark"]);
                                                //string batch_year = Convert.ToString(dvMin[0]["Batch_Year"]);

                                                string aptest = Convert.ToString(dsNew.Tables[0].Rows[0]["appear"]);
                                                if (aptest != "")
                                                {
                                                    appeared = Convert.ToDouble(aptest);
                                                    pass = appeared;
                                                    if (appeared != 0)
                                                    {
                                                        percent = Math.Round(((pass / appeared) * 100), 2);
                                                    }
                                                }


                                                if (dsNew.Tables[1].Rows.Count > 0)
                                                {

                                                    DataSet ds1 = new DataSet();
                                                    selectQuery = "";
                                                    selectQuery = "   select isnull(count(distinct rt.roll_no),0) as 'Fail' from result r,registration rt,Exam_type e,CriteriaForInternal c where e.exam_code=r.exam_code and e.criteria_no =c.criteria_no and  (marks_obtained<" + Convert.ToString(dsNew.Tables[1].Rows[0][0]) + " and marks_obtained<>'-2' and marks_obtained<>'-3' and marks_obtained<>'-18')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria ='" + Convert.ToString(ddl_test.SelectedValue) + "' and  r.exam_code in(" + examcode + ") and rt.degree_code=" + ddl_dept.SelectedItem.Value + " and rt.batch_year=" + ddl_batch.SelectedItem.Value + " ";
                                                    ds1.Clear();
                                                    ds1 = d2.select_method_wo_parameter(selectQuery, "Text");
                                                    string failtest = Convert.ToString(ds1.Tables[0].Rows[0]["fail"]);
                                                    Double fail = 0;

                                                    if (failtest != "")
                                                    {
                                                        fail = Convert.ToDouble(failtest);
                                                    }
                                                    string query = "select isnull(count(distinct rt.roll_no),0) as 'Allpass' from result r,registration rt,Exam_type e,CriteriaForInternal c where e.exam_code=r.exam_code and e.criteria_no =c.criteria_no and  (marks_obtained >=0 or marks_obtained='-1' or marks_obtained='-3' or marks_obtained='-2')  and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and c.criteria ='" + Convert.ToString(ddl_test.SelectedValue) + "' and  r.exam_code in(" + examcode + ") " + sec_new1 + "";
                                                    string dummyapperd = d2.GetFunction(query);

                                                    pass = Convert.ToDouble(dummyapperd) - fail;

                                                    if (appeared != 0)
                                                    {
                                                        percent = Math.Round(((pass / appeared) * 100), 2);
                                                    }

                                                    appearedAvg += appeared;
                                                    passAvg += pass;

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Text = Convert.ToString(appeared);
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Text = Convert.ToString(pass);
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Text = Convert.ToString(percent);

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].VerticalAlign = VerticalAlign.Middle;

                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Text = Convert.ToString(appeared);
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Text = Convert.ToString(pass);
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Text = Convert.ToString(percent);

                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].VerticalAlign = VerticalAlign.Middle;
                                                }

                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex].Text = "-";
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex].VerticalAlign = VerticalAlign.Middle;

                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Text = "-";
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].VerticalAlign = VerticalAlign.Middle;

                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Text = "-";

                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].VerticalAlign = VerticalAlign.Middle;
                                            }

                                        }
                                        else
                                        {
                                            //Excode Else
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex].Text = "-";
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex].VerticalAlign = VerticalAlign.Middle;

                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Text = "-";
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 1].VerticalAlign = VerticalAlign.Middle;

                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Text = "-";

                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[rowindex, columnindex + 2].VerticalAlign = VerticalAlign.Middle;
                                        }
                                        rowindex++;
                                    }
                                }
                                columnindex += 3;                            //    }
                                //}

                                if (appearedAvg == 0)
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Text = "-";
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Text = Convert.ToString(appearedAvg);
                                    semStrength += appearedAvg;
                                }
                                FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex, secCount, 1);
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].VerticalAlign = VerticalAlign.Middle;

                                if (appearedAvg == 0)
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].Text = "-";
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].Text = Convert.ToString(passAvg);
                                }
                                FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex + 1, secCount, 1);
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 1].VerticalAlign = VerticalAlign.Middle;

                                if (appearedAvg != 0 && passAvg != 0)
                                {
                                    percentAvg = Math.Round((passAvg / appearedAvg) * 100, 2);
                                }

                                if (appearedAvg == 0)
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Text = "-";
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Text = Convert.ToString(percentAvg);
                                }

                                FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex + 2, secCount, 1);
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].VerticalAlign = VerticalAlign.Middle;

                                semAggregate1 += appearedAvg;
                                semAggregate2 += passAvg;

                                hat_semStrength.Add(Convert.ToString(cbl_sem.Items[semrow].Value), Convert.ToString(semStrength));
                            }
                        }
                        string semName = " ";
                        FpSpread1.Sheets[0].RowCount += 2;
                        int colspan1 = 2;
                        //for (int testrow = 0; testrow < cbl_test.Items.Count; testrow++)
                        //{
                        //    if (cbl_test.Items[testrow].Selected == true)
                        //    {
                        colspan1 += 3;
                        //    }
                        //}

                        for (int semrow = 0; semrow < cbl_sem.Items.Count; semrow++)
                        {
                            if (cbl_sem.Items[semrow].Selected == true)
                            {
                                semName += romanLetter(Convert.ToString(cbl_sem.Items[semrow].Value)) + " : " + Convert.ToString(hat_semStrength[Convert.ToString(cbl_sem.Items[semrow].Value)]) + " | ";

                            }
                        }
                        semName = semName.Remove(semName.Length - 2);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(semName);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colspan1);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = Convert.ToString("Year wise Strength");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, 0, 1, colspan1);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString("Aggregate");
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 4, 2, 1);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(semAggregate1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3, 2, 1);

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(semAggregate2);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);

                        if (semAggregate1 != 0)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round((semAggregate2 / semAggregate1) * 100, 2));
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(" ");
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);


                        #endregion
                    }
                    if (FpSpread1.Sheets[0].RowCount > 0)
                    {
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Visible = true;
                        divspread.Visible = true;
                        rptprint.Visible = true;
                        lbl_error.Visible = false;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }
                }
            }

            catch
            {
            }
        }
        else
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Visible = false;
            divspread.Visible = false;
            rptprint.Visible = false;
            lbl_error.Text = "Please Select All The Fields";
            lbl_error.Visible = true;
        }
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            //string degreedetails = String.Format("Department of " + ddl_dept.SelectedItem.Text + " \n INTERNAL ASSESMENT  - YEAR-WISE RESULTS \n Month / year of Test:");

            string degreedetails = "Department of " + ddl_dept.SelectedItem.Text + " $ INTERNAL ASSESSMENT  - YEAR-WISE RESULTS";

            try
            {
                string pagename = "YearwiseResultAnalysis.aspx";
                Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrol.Visible = true;
            }
            catch
            {

            }

        }
        catch
        {

        }

    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {

        }

    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindBtch()
    {
        try
        {

            ddl_batch.Items.Clear();
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";

            DataSet ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "Batch_year";
                ddl_batch.DataValueField = "Batch_year";
                ddl_batch.DataBind();
                ddl_batch.SelectedIndex = ddl_batch.Items.Count - 1;
            }



            binddeg();
            binddept();
        }
        catch { }
    }
    public void binddeg()
    {
        try
        {
            ddl_degree.Items.Clear();

            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());
            if (batch != "")
            {
                ds.Clear();
                ds = d2.BindDegree(singleuser, group_user, ddl_college.SelectedValue.ToString(), usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_degree.DataSource = ds;
                    ddl_degree.DataTextField = "course_name";
                    ddl_degree.DataValueField = "course_id";
                    ddl_degree.DataBind();

                }
            }
            binddept();
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            ddl_dept.Items.Clear();
            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_college.SelectedValue.ToString(), usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "dept_name";
                    ddl_dept.DataValueField = "degree_code";
                    ddl_dept.DataBind();
                }
            }
        }
        catch { }
    }
    public void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "---Select---";

            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());


            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            dept = "";
            dept = Convert.ToString(ddl_dept.SelectedValue.ToString());

            if (batch != "" && degree != "" && dept != "")
            {
                ds.Clear();
                ds = d2.BindSem(dept, batch, ddl_college.SelectedValue.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "ndurations=max(ndurations)";
                    DataView dv = ds.Tables[0].DefaultView;



                    if (dv.Count > 0)
                    {
                        int semcount = 0;
                        string semcountstring = Convert.ToString(dv[0][0]);
                        if (semcountstring != "")
                        {
                            semcount = Convert.ToInt32(semcountstring);
                        }

                        for (i = 1; i <= semcount; i++)
                        {
                            cbl_sem.Items.Add(i.ToString());
                        }
                    }

                    if (cbl_sem.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_sem.Items.Count; i++)
                        {
                            cbl_sem.Items[i].Selected = true;
                        }
                        txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
                        cb_sem.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            cb_sec.Checked = false;
            txt_sec.Text = "---Select---";


            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());


            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            dept = "";
            dept = Convert.ToString(ddl_dept.SelectedValue.ToString());

            if (batch != "" && degree != "" && dept != "")
            {
                ds.Clear();
                ds = d2.BindSectionDetail(batch, dept);

                ListItem itemEmpty = new ListItem("Empty", " ");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_sec.Items.Count; i++)
                        {
                            cbl_sec.Items[i].Selected = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                        cb_sec.Checked = true;
                    }
                }
                else
                {
                    cbl_sec.Items.Add(itemEmpty);
                    cb_sec.Checked = true;
                    txt_sec.Text = "Section(1)";
                    cbl_sec.Items[0].Selected = true;
                }

            }
        }
        catch { }
    }
    public void bindtestname()
    {
        try
        {
            ddl_test.Items.Clear();

            batch = "";
            batch = Convert.ToString(ddl_batch.SelectedValue.ToString());


            degree = "";
            degree = Convert.ToString(ddl_degree.SelectedValue.ToString());

            dept = "";
            dept = Convert.ToString(ddl_dept.SelectedValue.ToString());

            sem = "";
            for (i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (sem == "")
                    {
                        sem = Convert.ToString(cbl_sem.Items[i].Value);
                    }
                    else
                    {
                        sem += "','" + Convert.ToString(cbl_sem.Items[i].Value);
                    }
                }

            }

            if (batch != "" && degree != "" && dept != "" && sem != "")
            {
                ds.Clear();
                selectQuery = "";
                selectQuery = "select distinct syllabus_year from syllabus_master where degree_code in (" + dept + ") and semester in ('" + sem + "') and batch_year in (" + batch + ")";
                ds = d2.select_method_wo_parameter(selectQuery, "Text");

                string sylabusyear = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
                    {
                        if (sylabusyear == "")
                        {
                            sylabusyear = Convert.ToString(ds.Tables[0].Rows[n][0]);
                        }
                        else
                        {
                            sylabusyear += "," + Convert.ToString(ds.Tables[0].Rows[n][0]);
                        }
                    }



                    DataSet ds2 = new DataSet();
                    selectQuery = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code  in (" + dept + ") and semester in ('" + sem + "') and syllabus_year in (" + sylabusyear + ") and batch_year in (" + batch + ") order by criteria";
                    ds2 = d2.select_method_wo_parameter(selectQuery, "Text");

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        ddl_test.DataSource = ds2;
                        ddl_test.DataTextField = "criteria";
                        ddl_test.DataValueField = "criteria";
                        //ddl_test.DataValueField = "criteria_no";
                        ddl_test.DataBind();

                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;

            }
        }
        return romanLettervalue;
    }
    public void clrSpread()
    {
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Visible = false;
        divspread.Visible = false;
        rptprint.Visible = false;
        lbl_error.Text = "";
        lbl_error.Visible = false;
    }
    public string returnYear(string semester)
    {
        string year = string.Empty;

        switch (semester)
        {
            case "1":
            case "2":
                year = "I";
                break;

            case "3":
            case "4":
                year = "II";
                break;
            case "5":
            case "6":
                year = "III";
                break;
            case "7":
            case "8":
                year = "IV";
                break;
            case "9":
            case "10":
                year = "V";
                break;
        }
        return year;
    }

    protected void ddl_test_Change(object sender, EventArgs E)
    {
        try
        {
            FpSpread1.Visible = false;
            divspread.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }
}