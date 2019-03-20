using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
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
    //added by rajasekar 08/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    

    //============================//

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

            
            Showgrid.Visible = false;
            divspread.Visible = false;
            rptprint.Visible = false;
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
            btnPrint11();
            Showgrid.Visible = false;
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

                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    dtl.Columns.Add("S.No", typeof(string));

                    dtl.Rows[0][0] = "S.No";
                  

                    dtl.Columns.Add("Department", typeof(string));

                    dtl.Rows[0][1] = "Department";

                    dtl.Columns.Add("Year/ Semester", typeof(string));

                    dtl.Rows[0][2] = "Year/ Semester";

                    dtl.Columns.Add("App", typeof(string));


                    dtl.Rows[0][3] = testname;

                    dtl.Rows[1][3] = "App";

                    dtl.Columns.Add("Pass", typeof(string));

                    dtl.Rows[1][4] = "Pass";

                    dtl.Columns.Add("%", typeof(string));

                    dtl.Rows[1][5] = "%";

                    dtl.Columns.Add(" App ", typeof(string));


                    dtl.Rows[0][6] = "Overall";

                    dtl.Rows[1][6] = "App";

                    dtl.Columns.Add(" Pass ", typeof(string));

                    dtl.Rows[1][7] = " Pass ";

                    dtl.Columns.Add(" % ", typeof(string));

                    dtl.Rows[1][8] = " % ";


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
                                   

                                    dtrow = dtl.NewRow();
                                    dtl.Rows.Add(dtrow);

                                    


                                    dtl.Rows[rowindex +2][0] = serialno.ToString();

                                    


                                    dtl.Rows[rowindex +2][1] = cbl_dept.Items[deptrow].Text;

                                    


                                    dtl.Rows[rowindex +2][2] = Convert.ToString(romanLetter(cbl_sem.Items[semrow].Text));

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

                                            

                                            dtl.Rows[rowindex +2][columnindex] = Convert.ToString(appeared);



                                            

                                            dtl.Rows[rowindex +2][columnindex + 1] = Convert.ToString(pass);


                                            

                                            dtl.Rows[rowindex +2][columnindex + 2] = Convert.ToString(percent);
                                            //columnindex += 3;
                                        }
                                        else
                                        {
                                            

                                            dtl.Rows[rowindex +2][columnindex] = Convert.ToString(appeared);



                                            

                                            dtl.Rows[rowindex +2][columnindex + 1] = Convert.ToString(pass);


                                            


                                            dtl.Rows[rowindex +2][columnindex + 2] = Convert.ToString(percent);

                                        }
                                    }
                                    else
                                    {
                                        

                                        dtl.Rows[rowindex +2][columnindex] = "-";

                                        

                                        dtl.Rows[rowindex +2][columnindex + 1] = "-";

                                        

                                        dtl.Rows[rowindex +2][columnindex + 2] = "-";
                                    }
                                    rowindex++;

                                }
                            }
                            columnindex += 3;
                            //    }

                            //}

                            if (appearedAvg == 0)
                            {
                                

                                dtl.Rows[(rowindex - semcount)+2][columnindex] = "-";
                            }
                            else
                            {
                               

                                dtl.Rows[(rowindex - semcount)+2][columnindex] = Convert.ToString(appearedAvg);
                                deptStrength += appearedAvg;
                            }
                            

                            if (appearedAvg == 0)
                            {
                                
                                dtl.Rows[(rowindex - semcount)+2][columnindex + 1] = "-";
                            }
                            else
                            {
                                

                                dtl.Rows[(rowindex - semcount)+2][columnindex + 1] = Convert.ToString(passAvg);

                            }
                            
                            if (appearedAvg != 0 && passAvg != 0)
                            {
                                percentAvg = Math.Round((passAvg / appearedAvg) * 100, 2);
                            }

                            if (appearedAvg == 0)
                            {
                                
                                dtl.Rows[(rowindex - semcount)+2][columnindex + 2] = "-";
                            }
                            else
                            {
                                

                                dtl.Rows[(rowindex - semcount)+2][columnindex + 2] = Convert.ToString(percentAvg);
                            }

                            
                            // rowindex++;
                            deptAggregate1 += appearedAvg;
                            deptAggregate2 += passAvg;

                            hat_deptStrength.Add(Convert.ToString(cbl_dept.Items[deptrow].Value), Convert.ToString(deptStrength));
                        }
                    }
                    string depName = " ";
                    
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);


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
                    

                    dtl.Rows[(dtl.Rows.Count - 1)][0] = Convert.ToString(depName);

                    

                    dtl.Rows[(dtl.Rows.Count - 2)][0] = Convert.ToString("Department Strength");

                    

                    dtl.Rows[(dtl.Rows.Count - 2)][dtl.Columns.Count - 4] = Convert.ToString("Aggregate");

                    

                    dtl.Rows[(dtl.Rows.Count - 2)][dtl.Columns.Count - 3] = Convert.ToString(deptAggregate1);


                    

                    dtl.Rows[(dtl.Rows.Count - 2)][dtl.Columns.Count - 2] = Convert.ToString(deptAggregate2);


                    
                    if (deptAggregate1 != 0)
                    {
                        

                        dtl.Rows[(dtl.Rows.Count - 2)][dtl.Columns.Count - 1] = Convert.ToString(Math.Round((deptAggregate2 / deptAggregate1) * 100, 2));
                    }
                    else
                    {
                       

                        dtl.Rows[(dtl.Rows.Count - 2)][dtl.Columns.Count - 1] = Convert.ToString(" ");
                    }
                    
                    #endregion
                }
                if ( dtl.Rows.Count > 0)
                {
                    Showgrid.DataSource = dtl;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    Showgrid.HeaderRow.Visible = false;
                    divspread.Visible = true;
                    rptprint.Visible = true;
                    lbl_error.Visible = false;
                    

                    int dtrowcount = dtl.Rows.Count;
                    int rowspanstart = 0;
                    


                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {
                        int rowspancount = 0;
                        

                        if (i != dtrowcount - 1)
                        {

                            if (rowspanstart == i)
                            {
                                for (int k = rowspanstart + 1; Showgrid.Rows[i].Cells[0].Text == Showgrid.Rows[k].Cells[0].Text; k++)
                                {
                                    rowspancount++;
                                    if (k == dtrowcount - 1)
                                        break;
                                }
                                rowspanstart++;
                            }
                            


                            if (rowspancount != 0)
                            {
                                rowspanstart = rowspanstart + rowspancount;
                                
                                Showgrid.Rows[i].Cells[0].RowSpan = rowspancount + 1;
                                for (int a = i; a < rowspanstart - 1; a++)
                                    Showgrid.Rows[a + 1].Cells[0].Visible = false;

                                
                                Showgrid.Rows[i].Cells[1].RowSpan = rowspancount + 1;
                                for (int a = i; a < rowspanstart - 1; a++)
                                    Showgrid.Rows[a + 1].Cells[1].Visible = false;


                                
                                Showgrid.Rows[i].Cells[6].RowSpan = rowspancount + 1;
                                for (int a = i; a < rowspanstart - 1; a++)
                                    Showgrid.Rows[a + 1].Cells[6].Visible = false;



                                Showgrid.Rows[i].Cells[7].RowSpan = rowspancount + 1;
                                for (int a = i; a < rowspanstart - 1; a++)
                                    Showgrid.Rows[a + 1].Cells[7].Visible = false;

                                Showgrid.Rows[i].Cells[8].RowSpan = rowspancount + 1;
                                for (int a = i; a < rowspanstart - 1; a++)
                                    Showgrid.Rows[a + 1].Cells[8].Visible = false;



                            }

                            if (i == Showgrid.Rows.Count - 2)
                            {
                                Showgrid.Rows[i].Cells[5].Font.Bold = true;
                                Showgrid.Rows[i].Cells[5].RowSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    Showgrid.Rows[a + i].Cells[5].Visible = false;

                                Showgrid.Rows[i].Cells[6].Font.Bold = true;
                                Showgrid.Rows[i].Cells[6].RowSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    Showgrid.Rows[a + i].Cells[6].Visible = false;

                                Showgrid.Rows[i].Cells[7].Font.Bold = true;
                                Showgrid.Rows[i].Cells[7].RowSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    Showgrid.Rows[a + i].Cells[7].Visible = false;

                                Showgrid.Rows[i].Cells[8].Font.Bold = true;
                                Showgrid.Rows[i].Cells[8].RowSpan = 2;
                                for (int a = 1; a < 2; a++)
                                    Showgrid.Rows[a + i].Cells[8].Visible = false;
                            }




                        }

                        for (int j = 0; j < dtl.Columns.Count; j++)
                        {
                            if (i < 2)
                            {

                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].Font.Name = "Book Antiqua";
                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                if (i == 0)
                                {
                                    if (j == 0 || j == 1 || j == 2)
                                    {

                                        Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                        for (int a = i; a < 1; a++)
                                            Showgrid.Rows[a + 1].Cells[j].Visible = false;


                                    }

                                    else if (j == 3 || j == 6)
                                    {
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = 3;
                                        for (int a = j + 1; a < j+3; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;
                                    }

                                }
                            }
                            else
                            {
                                if (j != 1)
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                }
                                if (j == 0 || j == 1 || j == 2)
                                {
                                    Showgrid.Rows[i].Cells[j].Font.Bold = true;


                                    if (i == Showgrid.Rows.Count - 2 && j == 0)
                                    {
                                        Showgrid.Rows[i].Cells[0].ColumnSpan = 5;
                                        for (int a = 1; a < 5; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;
                                    }

                                    if (i == Showgrid.Rows.Count - 1 && j == 0)
                                    {
                                        Showgrid.Rows[i].Cells[0].ColumnSpan = 5;
                                        for (int a = 1; a < 5; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;
                                    }
                                }

                            }

                        }





                    }

                    
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
                //Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
                string ss = null;
                Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
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
                d2.printexcelreportgrid(Showgrid, reportname);
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

    public void btnPrint11()
    {
        string college_code = Convert.ToString(ddl_college.SelectedValue);
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "Degree wise Result Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
        

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
                //ds = d2.BindSem(dept, batch, ddl_college.SelectedValue.ToString());
                string sems = "select distinct r.current_semester from Registration r where  college_code='" + ddl_college.SelectedValue.ToString() + "' and batch_year in('" + batch + "')  and   CC='0' and DelFlag='0' and Exam_Flag<>'debar'   order by r.current_semester";
                ds = d2.select_method_wo_parameter(sems, "text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "current_semester";
                    cbl_sem.DataValueField = "current_semester";
                    cbl_sem.DataBind();
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

    
    public override void VerifyRenderingInServerForm(Control control)
    { }
}