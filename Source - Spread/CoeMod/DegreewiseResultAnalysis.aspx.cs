using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class DegreewiseResultAnalysis : System.Web.UI.Page
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
    DataTable spreaddata = new DataTable();

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
        catch(Exception ex){
        }
    }
    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {

                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            binddeg();
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_batch.Checked = false;
            commcount = 0;
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            binddeg();
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            binddept();
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            i = 0;
            cb_dept.Checked = false;
            commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                txt_dept.Text = "Department(" + commcount.ToString() + ")";
            }
            bindsem();
            bindsec();
            bindtestname();
        }
        catch { }
    }
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
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
        }
        catch { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
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
        }
        catch { }
    }
    protected void cb_sec_OnCheckedChanged(object sender, EventArgs e)
    {
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
            int semCount = 0;
            if (ddl_college.Items.Count > 0)
            {
                college = Convert.ToString(ddl_college.SelectedValue);

                batch = "";
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (batch == "")
                        {
                            batch = Convert.ToString(cbl_batch.Items[i].Text);
                        }
                        else
                        {
                            batch += "," + Convert.ToString(cbl_batch.Items[i].Text);
                        }
                    }

                }

                degree = "";
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (degree == "")
                        {
                            degree = Convert.ToString(cbl_degree.Items[i].Value);
                        }
                        else
                        {
                            degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                        }
                    }

                }

                dept = "";
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    if (cbl_dept.Items[i].Selected == true)
                    {
                        if (dept == "")
                        {
                            dept = Convert.ToString(cbl_dept.Items[i].Value);
                        }
                        else
                        {
                            dept += "," + Convert.ToString(cbl_dept.Items[i].Value);
                        }
                    }

                }

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
                        semCount++;
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
                            sec += "," + Convert.ToString(cbl_sec.Items[i].Value);
                        }
                    }

                }

                testname = "";
                if (ddl_test.Items.Count > 0)
                {
                    testname = Convert.ToString(ddl_test.SelectedValue);
                }

            }
            #endregion

            if (batch != "" && degree != "" && dept != "" && sem != "" && testname != "")
            {
                selectQuery = "";
                selectQuery = "  select isnull(count(distinct rt.roll_no),0) as appeard ,r.degree_code,r.Current_Semester,c.criteria from result rt,registration r,Exam_type e,criteriaforinternal c where e.exam_code =rt.exam_code and e.criteria_no =C.Criteria_no and c.criteria  in ('" + testname + "') and rt.roll_no=r.roll_no and r.degree_code in (" + dept + ") and r.batch_year in (" + batch + ")  and r.Current_Semester in (" + sem + ") and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 group by r.degree_code,r.Current_Semester,c.criteria";

                selectQuery += " select s.degree_code,s.semester,c.criteria,c.min_mark,s.Batch_Year from criteriaforinternal c,syllabus_master s where s.syll_code =c.syll_code and criteria in ( '" + testname + "') and s.degree_code in (" + dept + ") and s.Batch_Year in (" + batch + ") and semester in (" + sem + ")";

                ds.Clear();
                ds = d2.select_method_wo_parameter(selectQuery, "Text");

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
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

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year/ Semester";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                    //for (i = 0; i < cbl_test.Items.Count; i++)
                    //{
                    //    if (cbl_test.Items[i].Selected == true)
                    //    {
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

                    //    }
                    //}
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
                    Hashtable hat_deptStrength = new Hashtable();
                    double deptStrength = 0;
                    double deptAggregate1 = 0;
                    double deptAggregate2 = 0;


                    double appearedAvg = 0;
                    double passAvg = 0;
                    double percentAvg = 0;
                    int semcount = 0;

                    for (int deptrow = 0; deptrow < cbl_dept.Items.Count; deptrow++)
                    {
                        if (cbl_dept.Items[deptrow].Selected == true)
                        {
                            serialno++;
                            deptStrength = 0;
                            int columnindex = 3;
                            semcount = 0;
                            appearedAvg = 0;
                            passAvg = 0;
                            percentAvg = 0;
                            for (int semrow = 0; semrow < cbl_sem.Items.Count; semrow++)
                            {
                                if (cbl_sem.Items[semrow].Selected == true)
                                {
                                    semcount++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].Text = serialno.ToString();
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[rowindex, 0].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[rowindex, 1].Text = cbl_dept.Items[deptrow].Text;
                                    FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[rowindex, 1].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Cells[rowindex, 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[rowindex, 1].Font.Name = "Book Antiqua";


                                    FpSpread1.Sheets[0].Cells[rowindex, 2].Text = Convert.ToString(romanLetter(cbl_sem.Items[semrow].Text));
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

                                    ds.Tables[0].DefaultView.RowFilter = " degree_code='" + Convert.ToString(cbl_dept.Items[deptrow].Value) + "' and Current_Semester='" + Convert.ToString(cbl_sem.Items[semrow].Text) + "' and criteria='" + testname + "'";
                                    DataView dvAppear = ds.Tables[0].DefaultView;

                                    ds.Tables[1].DefaultView.RowFilter = "degree_code='" + Convert.ToString(cbl_dept.Items[deptrow].Value) + "' and Semester='" + Convert.ToString(cbl_sem.Items[semrow].Text) + "' and criteria='" + testname + "'";
                                    DataView dvMin = ds.Tables[1].DefaultView;
                                    if (dvAppear.Count > 0 && dvMin.Count > 0)
                                    {
                                        string min = Convert.ToString(dvMin[0]["min_mark"]);
                                        string batch_year = Convert.ToString(dvMin[0]["Batch_Year"]);
                                        DataSet ds1 = new DataSet();
                                        selectQuery = "";
                                        selectQuery = " select isnull(count(distinct rt.roll_no),0) as fail,r.degree_code,r.Current_Semester,c.criteria from  result rt,registration r,Exam_type e,criteriaforinternal c where e.exam_code =rt.exam_code and e.criteria_no =C.Criteria_no and c.criteria ='" + testname + "' and rt.roll_no=r.roll_no and r.degree_code='" + Convert.ToString(cbl_dept.Items[deptrow].Value) + "' and r.batch_year=" + Convert.ToString(batch_year) + "  and r.Current_Semester in ('" + Convert.ToString(cbl_sem.Items[semrow].Text) + "')  and (rt.marks_obtained<" + min + " and rt.marks_obtained<>'-3' and rt.marks_obtained<>'-2')   and r.cc=0 and r.exam_flag <>'DEBAR' and r.delflag=0 and r.RollNo_Flag<>0 group by r.degree_code,r.Current_Semester,c.criteria ";
                                        ds1.Clear();
                                        ds1 = d2.select_method_wo_parameter(selectQuery, "Text");

                                        if (ds1.Tables[0].Rows.Count > 0)
                                        {
                                            string aptest = Convert.ToString(dvAppear[0]["appeard"]);
                                            string failtest = Convert.ToString(ds1.Tables[0].Rows[0]["fail"]);
                                            Double fail = 0;
                                            if (aptest != "")
                                            {
                                                appeared = Convert.ToDouble(aptest);
                                            }
                                            if (failtest != "")
                                            {
                                                fail = Convert.ToDouble(failtest);
                                            }
                                            pass = appeared - fail;

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
                                            //columnindex += 3;
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
                                    rowindex++;

                                }
                            }
                            columnindex += 3;
                            //    }

                            //}

                            if (appearedAvg == 0)
                            {
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Text = "-";
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex].Text = Convert.ToString(appearedAvg);
                                deptStrength += appearedAvg;
                            }
                            FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex, semCount, 1);
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
                            FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex + 1, semCount, 1);
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

                            FpSpread1.Sheets[0].SpanModel.Add(rowindex - semcount, columnindex + 2, semCount, 1);
                            FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[rowindex - semcount, columnindex + 2].VerticalAlign = VerticalAlign.Middle;
                            // rowindex++;
                            deptAggregate1 += appearedAvg;
                            deptAggregate2 += passAvg;

                            hat_deptStrength.Add(Convert.ToString(cbl_dept.Items[deptrow].Value), Convert.ToString(deptStrength));
                        }
                    }
                    string depName = " ";
                    FpSpread1.Sheets[0].RowCount += 2;



                    int colspan1 = 2;
                    //for (int testrow = 0; testrow < cbl_test.Items.Count; testrow++)
                    //{
                    //    if (cbl_test.Items[testrow].Selected == true)
                    //    {
                    colspan1 += 3;
                    //    }
                    //}

                    for (int deptrow = 0; deptrow < cbl_dept.Items.Count; deptrow++)
                    {
                        if (cbl_dept.Items[deptrow].Selected == true)
                        {
                            depName += d2.GetFunction("select dt.dept_acronym  from Degree d,Department dt where d.Dept_Code =dt.Dept_Code and d.Degree_Code ='" + Convert.ToString(cbl_dept.Items[deptrow].Value) + "'") + " : " + Convert.ToString(hat_deptStrength[Convert.ToString(cbl_dept.Items[deptrow].Value)]) + " | ";

                        }
                    }
                    depName = depName.Remove(depName.Length - 2);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(depName);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colspan1);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = Convert.ToString("Department Strength");
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

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(deptAggregate1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 3, 2, 1);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(deptAggregate2);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);

                    if (deptAggregate1 != 0)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Math.Round((deptAggregate2 / deptAggregate1) * 100, 2));
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
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Degree wise Result Analysis";
            try
            {
                string pagename = "DegreewiseResultAnalysis.aspx";
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
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

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
            bindBtch();
            binddeg();
            binddept();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindBtch()
    {
        try
        {

            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }
            if (batch != "")
            {
                ds.Clear();
                ds = d2.BindDegree(singleuser, group_user, ddl_college.SelectedValue.ToString(), usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_degree.Items.Count; i++)
                        {
                            cbl_degree.Items[i].Selected = true;
                        }
                        txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                        cb_degree.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, ddl_college.SelectedValue.ToString(), usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
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
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "," + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            dept = "";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = Convert.ToString(cbl_dept.Items[i].Value);
                    }
                    else
                    {
                        dept += "," + Convert.ToString(cbl_dept.Items[i].Value);
                    }
                }

            }

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
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "," + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            dept = "";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = Convert.ToString(cbl_dept.Items[i].Value);
                    }
                    else
                    {
                        dept += "," + Convert.ToString(cbl_dept.Items[i].Value);
                    }
                }

            }

            if (batch != "" && degree != "" && dept != "")
            {
                ds.Clear();
                ds = d2.BindSectionDetail(batch, dept);

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
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                    {
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch += "," + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            dept = "";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (dept == "")
                    {
                        dept = Convert.ToString(cbl_dept.Items[i].Value);
                    }
                    else
                    {
                        dept += "," + Convert.ToString(cbl_dept.Items[i].Value);
                    }
                }

            }
            if (batch != "" && degree != "" && dept != "")
            {
                ds.Clear();
                selectQuery = "";
                selectQuery = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,Registration r where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester=r.Current_Semester and r.cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and sy.Batch_Year in(" + batch + ") and sy.degree_code in(" + dept + ") order by criteria";
                ds = d2.select_method_wo_parameter(selectQuery, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_test.DataSource = ds;
                    ddl_test.DataTextField = "criteria";
                    ddl_test.DataValueField = "criteria";
                    ddl_test.DataBind();

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
}