using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class failreport : System.Web.UI.Page
{

    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string user_code = "";
    string collegecode = "";
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    DataTable dtfail = new DataTable();
    DataTable dtfail1 = new DataTable();
    DataRow drfailrep;
    Boolean headflag = false;
    Boolean finalflag = false;
    Boolean subflag = false;
    string query = "";
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
        lblerr.Visible = false;
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            bindclg();
            bindtest();

            gridfail.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            ddltest.Items.Insert(0, "--Select--");
        }

    }

    public void bindclg()
    {
        try
        {
            ddlclg.Items.Clear();
            hat.Clear();
            user_code = Session["usercode"].ToString();
            collegecode = ddlclg.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string clgname = "select college_code,collname from collinfo";
            if (clgname != "")
            {
                ds = da.select_method(clgname, hat, "Text");
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
            for (int i = 0; i < ddlclg.Items.Count; i++)
            {
                ddlclg.Items[i].Selected = false;
            }
        }
        catch
        {

        }
    }
    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode = ddlclg.SelectedValue.ToString();
        ds.Clear();
        ds.Dispose();
        query = "select distinct c.criteria from Registration r,syllabus_master sy,CriteriaForInternal c where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and sy.syll_code=c.syll_code and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.college_code='" + collegecode + "' order by c.criteria";
        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddltest.DataSource = ds;
            ddltest.DataTextField = "criteria";
            ddltest.DataValueField = "criteria";
            ddltest.DataBind();
            ddltest.Items.Insert(0, "--Select--");

        }
        else
        {
            ddltest.Items.Clear();
            ddltest.Items.Insert(0, "--Select--");
        }
    }

    public void bindtest()
    {
        try
        {
            collegecode = ddlclg.SelectedValue.ToString();
            query = "select distinct c.criteria from Registration r,syllabus_master sy,CriteriaForInternal c where r.degree_code=sy.degree_code and r.Batch_Year=sy.Batch_Year and r.Current_Semester=sy.semester and sy.syll_code=c.syll_code and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.college_code='" + collegecode + "'";
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltest.DataSource = ds;
                ddltest.DataTextField = "criteria";
                ddltest.DataValueField = "criteria";
                ddltest.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        bindspread();
        Printcontrol.Visible = false;
    }
    public void bindspread()
    {
        try
        {


            if (ddltest.SelectedItem.Text == "--Select--")
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Test";
                gridfail.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                errmsg.Visible = false;
            }
            else
            {
                dtfail.Columns.Add("SNo");
                dtfail.Columns.Add("Branch");
                dtfail.Columns.Add("Section");
                dtfail.Columns.Add("Strength");
                dtfail.Columns.Add("1-SUB FAILURE");
                dtfail.Columns.Add("1");
                dtfail.Columns.Add("2-SUB FAILURE");
                dtfail.Columns.Add("2");
                dtfail.Columns.Add("3-SUB FAILURE");
                dtfail.Columns.Add("3");
                dtfail.Columns.Add("4-SUB FAILURE");
                dtfail.Columns.Add("4");
                dtfail.Columns.Add("5-SUB FAILURE");
                dtfail.Columns.Add("5");
                dtfail.Columns.Add("6-SUB FAILURE");
                dtfail.Columns.Add("6");
                dtfail.Columns.Add("7-SUB FAILURE");
                dtfail.Columns.Add("7");
                dtfail.Columns.Add("FAILURE %");

                drfailrep = dtfail.NewRow();
                drfailrep["SNo"] = "S.No";
                drfailrep["Branch"] = "Branch";
                drfailrep["Section"] = "Section";
                drfailrep["Strength"] = "Strength";
                drfailrep["1-SUB FAILURE"] = "1-SUB FAILURE";
                drfailrep["2-SUB FAILURE"] = "2-SUB FAILURE";
                drfailrep["3-SUB FAILURE"] = "3-SUB FAILURE";
                drfailrep["4-SUB FAILURE"] = "4-SUB FAILURE";
                drfailrep["5-SUB FAILURE"] = "5-SUB FAILURE";
                drfailrep["6-SUB FAILURE"] = "6-SUB FAILURE";
                drfailrep["7-SUB FAILURE"] = "7-SUB FAILURE";
                drfailrep["FAILURE %"] = "FAILURE %";
                dtfail.Rows.Add(drfailrep);


                collegecode = ddlclg.SelectedValue.ToString();
                query = "select max(batch_year) as batch_year,d.Duration,d.Exam_System from Registration r,Degree d where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' and r.college_code='" + collegecode + "' and r.college_code=d.college_code group by d.Duration,d.Exam_System";
                ds = da.select_method_wo_parameter(query, "Text");
                int cn = 0;
                string headyr = "";

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    Double overall = 0.0;
                    int year = Convert.ToInt32(ds.Tables[0].Rows[j]["batch_year"]);
                    int semcnt = Convert.ToInt32(ds.Tables[0].Rows[j]["Duration"]);
                    int sem = Convert.ToInt32(ds.Tables[0].Rows[j]["Duration"]);
                    for (int y = 0; y < semcnt / 2; y++)
                    {
                        Double percent = 0.0;
                        overall = 0.0;
                        int cnt = 0;
                        headflag = false;
                        int batchyear = year - y;
                        query = "select distinct c.Course_Name,d.Acronym,d.Degree_Code from Course c,Degree d where c.Course_Id=d.Course_Id and c.college_code=d.college_code and c.college_code='" + collegecode + "' and d.Duration='" + sem + "' order by c.Course_Name";
                        ds1 = da.select_method_wo_parameter(query, "Text");

                        if (y == 0)
                        {
                            headyr = "1st-Year";
                        }
                        else if (y == 1)
                        {
                            headyr = "2nd-Year";
                        }
                        else if (y == 2)
                        {
                            headyr = "3rd-Year";
                        }
                        else if (y == 3)
                        {
                            headyr = "4th-Year";
                        }
                        else
                        {
                            headyr = "5th-Year";
                        }

                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            string course = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                            string branch = ds1.Tables[0].Rows[i]["Acronym"].ToString();
                            query = "select distinct current_semester from registration where degree_code='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and batch_year='" + batchyear.ToString() + "' and cc=0 and delflag=0 and exam_flag<>'debar'";
                            ds2 = da.select_method_wo_parameter(query, "Text");
                            string crsem = "";
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                crsem = ds2.Tables[0].Rows[0]["current_semester"].ToString();
                            }
                            else
                            {
                                crsem = "";
                            }
                            query = "select distinct sections from registration where batch_year ='" + batchyear.ToString() + "' and degree_code ='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and current_semester='" + crsem + "' and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
                            ds2 = da.select_method_wo_parameter(query, "text");
                            if (ds2.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < ds2.Tables[0].Rows.Count; s++)
                                {
                                    query = "select distinct c.criteria,c.criteria_no,r.batch_year,r.degree_code,r.current_semester,r.sections from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0and r.exam_flag<>'debar' AND C.criteria='" + ddltest.SelectedItem.Text.ToString() + "' and r.batch_year='" + batchyear + "'  and r.degree_code ='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and r.Sections='" + ds2.Tables[0].Rows[s]["sections"].ToString() + "'";
                                    ds3 = da.select_method_wo_parameter(query, "text");
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        query = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and cc=0 and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + batchyear + "'  and c.Criteria_no  ='" + ds3.Tables[0].Rows[0]["criteria_no"].ToString() + "' and re.Sections='" + ds2.Tables[0].Rows[s]["sections"].ToString() + "'  and s.subject_no=e.subject_no and marks_obtained < e.min_mark ) as my_table group by roll_no ) as  count_table group by arr order by arr";
                                        ds3 = da.select_method_wo_parameter(query, "text");
                                        int rwcnt = ds3.Tables[0].Rows.Count;
                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {

                                            if (headflag == false)
                                            {
                                                drfailrep = dtfail.NewRow();
                                                drfailrep["SNo"] = headyr.ToString();
                                                dtfail.Rows.Add(drfailrep);


                                                headflag = true;
                                            }
                                            finalflag = true;
                                            subflag = true;
                                            drfailrep = dtfail.NewRow();
                                            cn++;
                                            drfailrep["SNo"] = cn.ToString();
                                            drfailrep["Branch"] = course + "-" + branch;
                                            drfailrep["Section"] = ds2.Tables[0].Rows[s]["sections"].ToString();

                                            Double failcnt = 0;
                                            Dictionary<int, string> arrearVal = new Dictionary<int, string>();


                                            if (ds3.Tables[0].Rows.Count > 0)
                                            {
                                                for (int r = 0; r < ds3.Tables[0].Rows.Count; r++)
                                                {
                                                    arrearVal.Add(Convert.ToInt32(ds3.Tables[0].Rows[r]["arrear"]), ds3.Tables[0].Rows[r]["students"].ToString());
                                                    failcnt = failcnt + Convert.ToDouble(ds3.Tables[0].Rows[r]["students"]);
                                                }


                                                if (arrearVal.Count > 0)
                                                {
                                                    for (int r = 1; r <= 7; r++)
                                                    {
                                                        if (arrearVal.ContainsKey(r))
                                                            drfailrep[r + "-SUB FAILURE"] = arrearVal[r];
                                                        else
                                                            drfailrep[r + "-SUB FAILURE"] = "-";
                                                    }

                                                }

                                                //if (drfailrep["1-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["1-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["2-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["2-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["3-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["3-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["4-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["4-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["5-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["5-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["6-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["6-SUB FAILURE"] = "-";
                                                //}
                                                //if (drfailrep["7-SUB FAILURE"] == "")
                                                //{
                                                //    drfailrep["7-SUB FAILURE"] = "-";
                                                //}



                                                query = "select COUNT( Roll_No) as roll,COUNT (Stud_Name) as name from Registration where Batch_Year='" + batchyear + "' and degree_code='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and Current_Semester='" + crsem + "' and college_code='" + collegecode + "' and Sections='" + ds2.Tables[0].Rows[s]["sections"].ToString() + "' and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 ";
                                                ds4 = da.select_method_wo_parameter(query, "Text");
                                                Double totcnt = Convert.ToDouble(ds4.Tables[0].Rows[0]["roll"]);
                                                percent = Convert.ToDouble(Math.Round(failcnt / totcnt * 100, 2));
                                                drfailrep["Strength"] = totcnt.ToString();
                                                drfailrep["FAILURE %"] = percent + "%";

                                            }
                                            //if (drfailrep["1-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["1-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["2-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["2-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["3-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["3-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["4-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["4-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["5-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["5-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["6-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["6-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["7-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["7-SUB FAILURE"] = "-";
                                            //}

                                            if (finalflag == true)
                                            {
                                                if (overall == 0.0)
                                                {
                                                    overall = percent;
                                                    cnt++;
                                                }
                                                else
                                                {
                                                    overall = overall + percent;
                                                    cnt++;
                                                }
                                            }
                                            dtfail.Rows.Add(drfailrep);
                                            gridfail.Visible = true;
                                            lblerr.Visible = false;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                            errmsg.Visible = false;
                                        }
                                    }

                                }

                            }
                            else
                            {

                                query = "select distinct c.criteria,c.criteria_no,r.batch_year,r.degree_code,r.current_semester,r.sections from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.current_semester=s.semester and c.syll_code=s.syll_code and cc=0 and delflag=0and r.exam_flag<>'debar' AND C.criteria='" + ddltest.SelectedItem.Text.ToString() + "' and r.batch_year='" + batchyear + "'  and r.degree_code ='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "'";
                                ds3 = da.select_method_wo_parameter(query, "text");
                                if (ds3.Tables[0].Rows.Count > 0)
                                {
                                    query = "Select arr arrear,count(1) students from( Select count(roll_no) arr from (select distinct c.criteria,c.criteria_no,r.marks_obtained,s.acronym,e.subject_no,s.subject_name,e.min_mark,e.max_mark,re.Roll_No from criteriaforinternal c, result r,exam_type e,subject s,syllabus_master sn,Registration re where re.Roll_No=r.roll_no and cc=0 and delflag=0 and exam_flag<>'debar'and r.exam_code=e.exam_code and c.Criteria_no=e.criteria_no and e.batch_year='" + batchyear + "'  and c.Criteria_no  ='" + ds3.Tables[0].Rows[0]["criteria_no"].ToString() + "' and s.subject_no=e.subject_no and marks_obtained < e.min_mark ) as my_table group by roll_no ) as  count_table group by arr order by arr";
                                    ds3 = da.select_method_wo_parameter(query, "text");
                                    int rwcnt = ds3.Tables[0].Rows.Count;
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {

                                        if (headflag == false)
                                        {
                                            drfailrep = dtfail.NewRow();
                                            drfailrep["SNo"] = headyr;
                                            headflag = true;
                                            dtfail.Rows.Add(drfailrep);
                                        }
                                        finalflag = true;
                                        subflag = true;
                                        drfailrep = dtfail.NewRow();
                                        cn++;
                                        drfailrep["SNo"] = cn.ToString();
                                        drfailrep["Branch"] = course + "-" + branch;
                                        drfailrep["Section"] = "-";

                                        Double failcnt = 0;
                                        Dictionary<int, string> arrearVal = new Dictionary<int, string>();


                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            for (int r = 0; r < ds3.Tables[0].Rows.Count; r++)
                                            {
                                                arrearVal.Add(Convert.ToInt32(ds3.Tables[0].Rows[r]["arrear"]), ds3.Tables[0].Rows[r]["students"].ToString());
                                                failcnt = failcnt + Convert.ToDouble(ds3.Tables[0].Rows[r]["students"]);
                                            }


                                            if (arrearVal.Count > 0)
                                            {
                                                for (int r = 1; r <= 7; r++)
                                                {
                                                    if (arrearVal.ContainsKey(r))
                                                        drfailrep[r + "-SUB FAILURE"] = arrearVal[r];
                                                    else
                                                        drfailrep[r + "-SUB FAILURE"] = "-";
                                                }

                                            }


                                            //if (drfailrep["1-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["1-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["2-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["2-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["3-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["3-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["4-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["4-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["5-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["5-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["6-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["6-SUB FAILURE"] = "-";
                                            //}
                                            //if (drfailrep["7-SUB FAILURE"] == "")
                                            //{
                                            //    drfailrep["7-SUB FAILURE"] = "-";
                                            //}



                                            query = "select COUNT( Roll_No) as roll,COUNT (Stud_Name) as name from Registration where Batch_Year='" + batchyear + "' and degree_code='" + ds1.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and Current_Semester='" + crsem + "' and college_code='" + collegecode + "'  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 ";
                                            ds4 = da.select_method_wo_parameter(query, "Text");
                                            Double totcnt = Convert.ToDouble(ds4.Tables[0].Rows[0]["roll"]);
                                            percent = Convert.ToDouble(Math.Round(failcnt / totcnt * 100, 2));

                                            drfailrep["Strength"] = totcnt.ToString();
                                            drfailrep["FAILURE %"] = percent + "%";

                                        }
                                        //if (drfailrep["1-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["1-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["2-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["2-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["3-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["3-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["4-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["4-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["5-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["5-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["6-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["6-SUB FAILURE"] = "-";
                                        //}
                                        //if (drfailrep["7-SUB FAILURE"] == "")
                                        //{
                                        //    drfailrep["7-SUB FAILURE"] = "-";
                                        //}
                                        dtfail.Rows.Add(drfailrep);

                                        if (finalflag == true)
                                        {
                                            if (overall == 0.0)
                                            {
                                                overall = percent;
                                                cnt++;
                                            }
                                            else
                                            {
                                                overall = overall + percent;
                                                cnt++;
                                            }
                                        }

                                        gridfail.Visible = true;
                                        lblerr.Visible = false;
                                        btnprintmaster.Visible = true;
                                        lblrptname.Visible = true;
                                        txtexcelname.Visible = true;
                                        btnxl.Visible = true;
                                        errmsg.Visible = false;
                                    }
                                }


                            }


                        }

                        if (subflag == true)
                        {
                            drfailrep = dtfail.NewRow();
                            drfailrep["SNo"] = headyr + " OVER ALL FAILURE %";
                            // gridfail.Rows[0].Cells[0].ColumnSpan = 11;
                            Double fin = Math.Round(overall / Convert.ToDouble(cnt), 2);
                            drfailrep["FAILURE %"] = fin + "%";

                            subflag = false;
                            dtfail.Rows.Add(drfailrep);
                        }

                    }


                }
                dtfail.Columns.Remove("1");
                dtfail.Columns.Remove("2");
                dtfail.Columns.Remove("3");
                dtfail.Columns.Remove("4");
                dtfail.Columns.Remove("5");
                dtfail.Columns.Remove("6");
                dtfail.Columns.Remove("7");

                gridfail.DataSource = dtfail;
                gridfail.DataBind();
                gridfail.Visible = true;


                for (int row = 0; row < gridfail.Rows.Count; row++)
                {

                    string yer = gridfail.Rows[row].Cells[0].Text;
                    if (gridfail.HeaderRow.Cells[0].Text.ToLower() == "sno" && yer.Contains("-Year"))
                    {
                        gridfail.Rows[row].Cells[0].ColumnSpan = 11;
                        gridfail.Rows[row].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        gridfail.Rows[row].Cells[0].BackColor = Color.LightCyan;
                        gridfail.Rows[row].Cells[0].Font.Bold = true;

                        for (int a = 1; a < gridfail.HeaderRow.Cells.Count - 1; a++)
                            gridfail.Rows[row].Cells[a].Visible = false;
                    }


                }

                RowHead(gridfail);
                if (finalflag == false)
                {
                    lblerr.Visible = true;
                    lblerr.Text = "No Records Found";
                    gridfail.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    errmsg.Visible = false;
                }
                else
                {
                    btnprintmaster.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    errmsg.Visible = false;
                    gridfail.Visible = true;
                    lblerr.Visible = false;

                }

            }
        }
        catch
        {
        }
    }

    protected void RowHead(GridView gridfail)
    {
        for (int head = 0; head < 1; head++)
        {
            gridfail.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gridfail.Rows[head].Font.Bold = true;
            gridfail.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {

            Printcontrol.Visible = false;
            string reportname = txtexcelname.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                da.printexcelreportgrid(gridfail, reportname);
                lblrptname.Visible = false;
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
        
            Printcontrol.Visible = true;
            string degreedetails = string.Empty;
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            degreedetails = "Failures Report - " + ddltest.SelectedItem.ToString() + date;
            string pagename = "failreport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(gridfail, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }

}