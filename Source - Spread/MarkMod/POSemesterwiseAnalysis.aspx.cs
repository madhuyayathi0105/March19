using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using InsproDataAccess;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


public partial class POSemesterwiseAnalysis : System.Web.UI.Page
{
    InsproDirectAccess dir = new InsproDirectAccess();
    InsproStoreAccess storAcc = new InsproStoreAccess();
    DataTable dtHeader = new DataTable();
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    string group_code = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    int[] y;
    Dictionary<string, string> dicParameter = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        //if (!Request.FilePath.Contains("CAMHome"))
        //{
        //    string strPreviousPage = "";
        //    if (Request.UrlReferrer != null)
        //    {
        //        strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //    }
        //    if (strPreviousPage == "")
        //    {
        //        Response.Redirect("~/MarkMod/CAMHome.aspx");
        //        return;
        //    }
        //}
        if (!IsPostBack)
        {

            GridView1.Visible = false;
            bindclg();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();

        }
    }

    public void bindclg()
    {
        try
        {
            string columnfield = string.Empty;
            group_code = Session["group_code"].ToString();
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
            dicParameter.Clear();
            dicParameter.Add("column_field", columnfield.ToString());
            dtHeader = storAcc.selectDataTable("bind_college", dicParameter);

            ddlCollege.Items.Clear();

            if (dtHeader.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtHeader;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }

        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        dicParameter.Clear();
        ddlbatch.Items.Clear();
        dtHeader = storAcc.selectDataTable("bind_batch", dicParameter);
        int count = dtHeader.Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = dtHeader;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = dtHeader.Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(dtHeader.Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
        }
    }

    public void BindDegree()
    {

        ddldegree.Items.Clear();
        collegecode = Convert.ToString(ddlCollege.SelectedValue);
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        string sql_s = "select distinct course_name,course_id from course where college_code='" + collegecode + "'";
        dtHeader = dir.selectDataTable(sql_s);
        if (dtHeader.Rows.Count > 0)
        {
            ddldegree.DataSource = dtHeader;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindbranch()
    {
        dicParameter.Clear();
        ddlbranch.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        //dicParameter.Add("single_user", singleuser);
        //dicParameter.Add("group_code", group_user);
        //dicParameter.Add("course_id", ddldegree.SelectedValue);
        //dicParameter.Add("college_code", collegecode);
        //dicParameter.Add("user_code", usercode);
        ds.Dispose();
        ds.Reset();
        ds = da.BindBranchMultiple(singleuser, group_user, ddldegree.SelectedValue.ToString(), collegecode, usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    public void bindsem()
    {
        ddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;

        if (ddlbranch.SelectedValue.ToString() != string.Empty && ddlbatch.SelectedItem.ToString() != string.Empty)
        {

            string SelectQ = ("select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue) + "'");
            dtHeader = dir.selectDataTable(SelectQ);
            if (dtHeader.Rows.Count > 0)
            {
                foreach (DataRow dr in dtHeader.Rows)
                {
                    first_year = Convert.ToBoolean(dr[1].ToString());
                    duration = Convert.ToInt16(dr[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }

                    }
                }

            }
            else
            {
                string selectQ = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Convert.ToString(ddlCollege.SelectedValue) + "";

                ddlsem.Items.Clear();
                dtHeader = dir.selectDataTable(selectQ);
                if (dtHeader.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtHeader.Rows)
                    {
                        first_year = Convert.ToBoolean(dr[1].ToString());
                        duration = Convert.ToInt16(dr[0].ToString());

                        for (i = 1; i <= duration; i++)
                        {
                            if (first_year == false)
                            {
                                ddlsem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlsem.Items.Add(i.ToString());
                            }
                        }
                    }
                }
            }
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        GridView1.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDegree();
        bindbranch();
        bindsem();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        GridView1.Visible = false;
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.Visible = false;
    }

    protected void GO_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint.Visible = false;
            btnprint1.Visible = false;
            GridView1.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            GridView1.Visible = false;
            string clgcode = Convert.ToString(ddlCollege.SelectedValue);
            string degcode = Convert.ToString(ddlbranch.SelectedValue);
            string batchyear = Convert.ToString(ddlbatch.SelectedValue);
            string semester = Convert.ToString(ddlsem.SelectedValue).Trim();
            DataTable dtMark = new DataTable();
            DataTable dtStudent = new DataTable();
            DataTable dtPoSettings = new DataTable();
            DataTable dtSubject = new DataTable();
            DataTable dtReport = new DataTable();
            DataTable dtPoMaster = new DataTable();
            DataTable dtChart = new DataTable();
            DataRow drchart = null;
            DataRow drNew = null;

            dtChart.Columns.Add("Roll No");
            dtChart.Columns.Add("POsettings");
            dtChart.Columns.Add("Mark", typeof(int));

            dtPoMaster = dir.selectDataTable("select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='posettings'");
            dtReport.Columns.Add("S.No");
            dtReport.Columns.Add("Reg no");
            dtReport.Columns.Add("Roll No");
            dtReport.Columns.Add("StudName");
            if (dtPoMaster.Rows.Count > 0)
            {
                foreach (DataRow drCo in dtPoMaster.Rows)
                {
                    string coName = Convert.ToString(drCo["MasterValue"]);
                    dtReport.Columns.Add(coName.Trim().ToUpper());
                }
            }
            drNew = dtReport.NewRow();
            drNew["S.No"] = "S.No";
            drNew["Reg no"] = "Reg no";
            drNew["Roll No"] = "Roll No";
            drNew["StudName"] = "StudName";
            if (dtPoMaster.Rows.Count > 0)
            {
                foreach (DataRow drCo in dtPoMaster.Rows)
                {
                    string coName = Convert.ToString(drCo["MasterValue"]);
                    drNew[coName] = coName.Trim().ToUpper();
                }
                dtReport.Rows.Add(drNew);
            }


            if (!string.IsNullOrEmpty(clgcode) && !string.IsNullOrEmpty(degcode) && !string.IsNullOrEmpty(batchyear) && !string.IsNullOrEmpty(semester))
            {
                string stddet = "select  r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.App_no,r.Sections   from Registration r where r.batch_year='" + batchyear + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + semester + "'  and  RollNo_Flag<>'0' and cc='0' and delflag='0' and exam_flag <> 'DEBAR'  order by r.reg_no,r.roll_no;";
                dtStudent = dir.selectDataTable(stddet);
                if (dtStudent.Rows.Count > 0)
                {
                    string SelSubject = "  select distinct subject.subject_name,subject.subject_code,subject.subject_no from subject,sub_sem,syllabus_master where  subject.subtype_no = sub_sem.subtype_no  and subject.syll_code=syllabus_master.syll_code   and  syllabus_master.degree_code in('" + degcode + "') and syllabus_master.batch_year in('" + batchyear + "') and syllabus_master.semester='" + semester + "'  order by subject.subject_name";//and sub_sem.promote_count<>0 
                    dtSubject = dir.selectDataTable(SelSubject);
                    if (dtSubject.Rows.Count > 0)
                    {
                        string subjectNo = string.Empty;
                        foreach (DataRow dr in dtSubject.Rows)
                        {
                            if (string.IsNullOrEmpty(subjectNo))
                                subjectNo = Convert.ToString(dr["subject_no"]);
                            else
                                subjectNo = subjectNo + "," + Convert.ToString(dr["subject_no"]);
                        }
                        if (!string.IsNullOrEmpty(subjectNo))
                        {
                            string posettings = "select *  from COThresholdSettings c,Master_Settings m where m.settings='COSettings' and c.CourseID=m.masterno and c.Subject_NO in(" + subjectNo + ")";
                            dtPoSettings = dir.selectDataTable(posettings);

                            string stuMark = "select distinct app_no,cono,mark,subject_no from coattainmentmarks where  subject_no in(" + subjectNo + ")";
                            dtMark = dir.selectDataTable(stuMark);

                            if (dtMark.Rows.Count > 0)
                            {
                                if (dtPoSettings.Rows.Count > 0)
                                {
                                    int sno = 0;
                                    foreach (DataRow drStu in dtStudent.Rows)
                                    {
                                        sno++;
                                        string rollNo = Convert.ToString(drStu["Roll_No"]);
                                        string Reg_No = Convert.ToString(drStu["Reg_No"]);
                                        string Roll_Admit = Convert.ToString(drStu["Roll_Admit"]);
                                        string Stud_Name = Convert.ToString(drStu["Stud_Name"]);
                                        string App_no = Convert.ToString(drStu["App_no"]);
                                        string Sections = Convert.ToString(drStu["Sections"]);
                                        drNew = dtReport.NewRow();
                                        drNew["S.No"] = sno.ToString();
                                        drNew["Reg no"] = Reg_No;
                                        drNew["Roll No"] = rollNo;
                                        drNew["StudName"] = Stud_Name;

                                        if (dtReport.Columns.Count > 0)
                                        {
                                            for (int col = 4; col < dtReport.Columns.Count; col++)
                                            {
                                                string colName = Convert.ToString(dtReport.Columns[col]);


                                                int countS = 0;
                                                int countW = 0;
                                                int countM = 0;
                                                double MarkS = 0;
                                                double MarkM = 0;
                                                double markW = 0;
                                                double total = 0;
                                                double totalS = 0; double totalM = 0; double totalW = 0;
                                                dtPoSettings.DefaultView.RowFilter = "" + colName + "=3";
                                                DataTable dtPoSCumCo = dtPoSettings.DefaultView.ToTable();
                                                foreach (DataRow drs in dtPoSCumCo.Rows)
                                                {
                                                    string CoNo = Convert.ToString(drs["masterno"]);
                                                    string SubnO = Convert.ToString(drs["Subject_NO"]);

                                                    dtMark.DefaultView.RowFilter = "app_no='" + App_no + "' and subject_no='" + SubnO + "' and cono='" + CoNo + "'";
                                                    DataView dvMarks = dtMark.DefaultView;
                                                    if (dvMarks.Count > 0)
                                                    {
                                                        countS = countS + 1;
                                                        double Mark = 0;
                                                        double.TryParse(Convert.ToString(dvMarks[0]["mark"]), out Mark);
                                                        MarkS = MarkS + Mark;
                                                    }
                                                }

                                                dtPoSettings.DefaultView.RowFilter = "" + colName + "=2";
                                                DataTable dtPoMCumCo = dtPoSettings.DefaultView.ToTable();
                                                foreach (DataRow drs in dtPoMCumCo.Rows)
                                                {
                                                    string CoNo = Convert.ToString(drs["masterno"]);
                                                    string SubnO = Convert.ToString(drs["Subject_NO"]);

                                                    dtMark.DefaultView.RowFilter = "app_no='" + App_no + "' and subject_no='" + SubnO + "' and cono='" + CoNo + "'";
                                                    DataView dvMarks = dtMark.DefaultView;
                                                    if (dvMarks.Count > 0)
                                                    {
                                                        countM = countM + 1;
                                                        double Mark = 0;
                                                        double.TryParse(Convert.ToString(dvMarks[0]["mark"]), out Mark);
                                                        MarkM = MarkM + Mark;
                                                    }
                                                }
                                                dtPoSettings.DefaultView.RowFilter = "" + colName + "=1";
                                                DataTable dtPoWCumCo = dtPoSettings.DefaultView.ToTable();
                                                foreach (DataRow drs in dtPoWCumCo.Rows)
                                                {
                                                    string CoNo = Convert.ToString(drs["masterno"]);
                                                    string SubnO = Convert.ToString(drs["Subject_NO"]);

                                                    dtMark.DefaultView.RowFilter = "app_no='" + App_no + "' and subject_no='" + SubnO + "' and cono='" + CoNo + "'";
                                                    DataView dvMarks = dtMark.DefaultView;
                                                    if (dvMarks.Count > 0)
                                                    {
                                                        countW = countW + 1;
                                                        double Mark = 0;
                                                        double.TryParse(Convert.ToString(dvMarks[0]["mark"]), out Mark);
                                                        markW = markW + Mark;
                                                    }
                                                }
                                                if (MarkS != 0 && countS != 0)
                                                {
                                                    totalS = MarkS / countS;
                                                    totalS = Math.Round(totalS, 2, MidpointRounding.AwayFromZero);
                                                }
                                                if (markW != 0 && countW != 0)
                                                {
                                                    totalW = markW / countW;
                                                    totalW = Math.Round(totalW, 2, MidpointRounding.AwayFromZero);
                                                }
                                                if (MarkM != 0 && countM != 0)
                                                {
                                                    totalM = MarkM / countM;
                                                    totalM = Math.Round(totalM, 2, MidpointRounding.AwayFromZero);
                                                }
                                                total = (totalS + totalW + totalM) / 3;
                                                total = Math.Round(total, 0, MidpointRounding.AwayFromZero);
                                                drNew[colName] = Convert.ToString(total);

                                                if (total > 0)
                                                {
                                                    drchart = dtChart.NewRow();
                                                    drchart["Mark"] = Convert.ToString(total);
                                                    drchart["Roll No"] = Stud_Name;//
                                                    drchart["POsettings"] = colName;//
                                                    dtChart.Rows.Add(drchart);
                                                }
                                            }
                                            dtReport.Rows.Add(drNew);
                                        }

                                    }


                                    if (dtReport.Rows.Count > 0)
                                    {
                                        GridView1.DataSource = dtReport;
                                        GridView1.DataBind();
                                        GridView1.Visible = true;
                                        btnPrint.Visible = true;
                                        btnprint1.Visible = true;
                                        GridView1.Visible = true;
                                        txtexcelname.Visible = true;
                                        lblrptname.Visible = true;
                                        GridView1.HeaderRow.Visible = false;
                                        GridView1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        GridView1.Rows[0].Visible = true;
                                        for (int i = 0; i < GridView1.Rows.Count; i++)
                                        {
                                            for (int j = 4; j < dtReport.Columns.Count; j++)
                                            {
                                                GridView1.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }


                                        //Get the DISTINCT Countries.
                                        if (dtChart.Rows.Count > 0)
                                        {
                                            List<string> rollNo = (from p in dtChart.AsEnumerable()
                                                                   select p.Field<string>("POsettings")).Distinct().ToList();

                                            List<string> countries = (from p in dtChart.AsEnumerable()
                                                                      select p.Field<string>("POsettings")).Distinct().ToList();

                                            if (Chart1.Series.Count() == 1)
                                            {
                                                Chart1.Series.Remove(Chart1.Series[0]);
                                            }
                                            //foreach (string roll in rollNo)
                                            //{

                                            foreach (string country in countries)
                                            {

                                                y = (from p in dtChart.AsEnumerable()
                                                     where p.Field<string>("POsettings") == country
                                                     orderby p.Field<string>("Roll No") ascending
                                                     select p.Field<int>("Mark")).ToArray();

                                                string[] x = (from p in dtChart.AsEnumerable()
                                                              where p.Field<string>("POsettings") == country
                                                              orderby p.Field<string>("Roll No") ascending
                                                              select p.Field<string>("Roll No")).ToArray();


                                                Chart1.Series.Add(new Series(country));
                                                Chart1.Series[country].IsValueShownAsLabel = true;
                                                Chart1.Series[country].BorderWidth = 3;

                                                //Chart1.Areas("myChartAreaName").AxisX.LabelStyle.Interval = 1;
                                                Chart1.Series[country].ChartType = SeriesChartType.Line;
                                                Chart1.Series[country].Points.DataBindXY(x, y);
                                                //Chart1.ChartAreas[country].AxisX.Interval = 1;
                                            }
                                            // }
                                        }

                                        Chart1.Legends[0].Enabled = true;
                                        Chart1.Visible = true;


                                    }

                                    //btnPrint11();
                                }
                                else
                                {
                                    //No POsettings(s) were found
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('No POsettings(s) were found')", true);
                                }
                            }
                            else
                            {
                                //No Marks(s) were found
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('No POsettings(s) were found')", true);
                            }
                        }
                    }
                    else
                    {
                        //No subject(s) were found
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('No subject(s) were found')", true);
                    }
                }
                else
                {
                    //No student(s) were found
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('No student(s) were found')", true);
                }
            }
            else { }
        }
        catch
        {
        }
    }

    public void btnPrint11()
    {
        string college_code = Convert.ToString(ddlCollege.SelectedValue);
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = da.select_method_wo_parameter(colQ, "Text");
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
        spRoomType.InnerHtml = "Batch : " + Convert.ToString(ddlbatch.SelectedItem.Text) + "<br/>" + "Degree : " + Convert.ToString(ddlbranch.SelectedItem.Text) + "<br/>" + "Semester : " + Convert.ToString(ddlsem.SelectedItem.Text);
        spReportName.InnerHtml = "Po SemesterWise Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
        //spRoomNo.InnerHtml = "Subject: " + Convert.ToString(ddlsubject.SelectedItem.Text);

    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreportgrid(GridView1, reportname);
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

    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        degreedetails = "Overall Po Chart Analysis and Report @" + ddlbatch.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " - " + ddlbranch.SelectedItem.Text + "     Semester-" + ddlsem.SelectedItem.Text.ToString() + "";
        string ss = Convert.ToString(Session["usercode"]);
        GridView1.Visible = true;
        Chart1.Visible = true;
        Printcontrol.loadspreaddetails(GridView1, "POSemesterwiseAnalysis.aspx", degreedetails, 0, ss);
        //Printcontrol.loadspreaddetails(GridView1, "POSemesterwiseAnalysis.aspx", "Overall Po Chart Analysis and Report", 0, ss);
        Printcontrol.Visible = true;
    }

}