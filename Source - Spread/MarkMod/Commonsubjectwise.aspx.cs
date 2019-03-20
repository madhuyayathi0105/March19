using System;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;


public partial class Commonsubjectwise : System.Web.UI.Page
{

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    //added by rajasekar 08/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    ArrayList falsecol = new ArrayList();
    int firstrowscount = 0;
    


    //============================//

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            // errmsg.Visible = false;
            if (!IsPostBack)
            {

                BindBatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                bindtestname();
                Subject();
                criteria();
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Printcontrol.Visible = false;

                string grouporusercode = "";

                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }

                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }

        }
        catch (Exception ex) { }
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            Hashtable hatmaxsubmark = new Hashtable();
            Hashtable hatminsubmark = new Hashtable();
            Hashtable subjavgmark = new Hashtable();
            DataTable dtChart1 = new DataTable();
            DataColumn dc;
            DataRow drp;
            DataRow drpt;
            DataTable dtChart2 = new DataTable();
            DataColumn dcs;
            DataRow s_grage;
            //   Chart1.Series[0].BorderWidth = 2;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Printcontrol.Visible = false;
            errmsg.Visible = true;
            errmsg.Text = "";
            int srno = 0;

            S_grads.Visible = false;
            Showgrid.Visible = false;

            string sec = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (sec == "")
                    {
                        sec = "" + cbl_sec.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        sec += "','" + cbl_sec.Items[i].Text.ToString() + "";
                    }
                }
            }

            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }

            string test = "";
            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                if (Cbl_test.Items[i].Selected == true)
                {
                    if (test == "")
                    {
                        test = "" + Cbl_test.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        test = test + "','" + Cbl_test.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (txt_degree.Text == "--Select--" || txt_branch.Text == "---Select---" || Txt_Test.Text == "--Select--")
            {
                Showgrid.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select All Fields";

                rptprint1.Visible = false;
                S_grads.Visible = false;
                chart_passpercentage.Visible = false;
                return;
            }

            if (txt_sec.Enabled != false)
            {
                if (txt_sec.Text == "--Select--")
                {
                    Showgrid.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select All Fields";

                    rptprint1.Visible = false;
                    S_grads.Visible = false;
                    chart_passpercentage.Visible = false;
                    return;
                }
            }
            if (txtoptiminpassmark.Text != "" && txtoptiminpassmark.Text != null)
            {
                if (Convert.ToInt32(txtoptiminpassmark.Text) == 0)
                {
                    Showgrid.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Enter The Optional Min Pass Mark Greater Than 0.";

                    rptprint1.Visible = false;
                    S_grads.Visible = false;
                    chart_passpercentage.Visible = false;
                    txtoptiminpassmark.Text = "";
                    return;
                }
                if (100 < Convert.ToInt32(txtoptiminpassmark.Text))
                {
                    Showgrid.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Enter The Optional Min Pass Mark Less Than or Equal to 100.";

                    rptprint1.Visible = false;
                    S_grads.Visible = false;
                    chart_passpercentage.Visible = false;
                    txtoptiminpassmark.Text = "100";
                    return;
                }
            }
            if (test == "")
            {
                return;
                //  rptprint1.Visible = false;
            }
            string subject = ddl_subject.SelectedItem.Value.ToString();
            string bach_year = ddl_Batchyear.SelectedItem.Value.ToString();

            Showgrid.Visible = false;

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);


            dtl.Columns.Add("S.No", typeof(string));

            dtl.Rows[0][0] = "S.No";

            dtl.Columns.Add("Class", typeof(string));

            dtl.Rows[0][1] = "Class";

            dtl.Columns.Add("Roll.No", typeof(string));

            dtl.Rows[0][2] = "Roll.No";

            dtl.Columns.Add("Reg.No", typeof(string));

            dtl.Rows[0][3] = "Reg.No";

            dtl.Columns.Add("Student Type", typeof(string));

            dtl.Rows[0][4] = "Student Type";

            dtl.Columns.Add("Student Name", typeof(string));

            dtl.Rows[0][5] = "Student Name";

            if (Session["Rollflag"].ToString() == "1")
            {
                
            }
            else
            {
                
                falsecol.Add("2");
           
            }

            if (Session["Regflag"].ToString() == "1")
            {
                
            }
            else
            {
                
                falsecol.Add("3");
            }

            if (Session["Studflag"].ToString() == "1")
            {
                
            }
            else
            {
                
                falsecol.Add("4");
            }

            dc = new DataColumn();
            //dc.ColumnName = string.Empty;
            dtChart1.Columns.Add(" ");
            dtChart2.Columns.Add(" ");
            int cc = 6;
            // dtChart2.Columns.Add(dc);
            for (int t = 0; t < Cbl_test.Items.Count; t++)
            {
                if (Cbl_test.Items[t].Selected == true)
                {
                    


                    dtl.Columns.Add(Cbl_test.Items[t].Text.ToString(), typeof(string));

                    dtl.Rows[0][cc] = Cbl_test.Items[t].Text.ToString();
                    cc++;

                    dc = new DataColumn();
                    dc.ColumnName = Cbl_test.Items[t].Text.ToString();
                    dcs = new DataColumn();
                    dcs.ColumnName = Cbl_test.Items[t].Text.ToString();
                    dtChart1.Columns.Add(dc);
                    dtChart2.Columns.Add(dcs);

                }
            }

            


            

            string batchyear = ddl_Batchyear.SelectedItem.ToString();
            string degreecode = degree_code.ToString();

            string sem = ddl_semester.SelectedValue.ToString();

            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "");
            int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration where Batch_Year='" + batchyear + "' and degree_code in('" + degreecode + "') and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by sem"));

            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY batch_year,r.degree_code,serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY batch_year,r.degree_code,r.sections,Roll_No,Stud_Name";
                }
            }

            //string strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type,(C.Course_Name +'-'+dt.dept_acronym+'-'+Sections) as Degreedetails from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id Batch_Year='" + batchyear + "' and degree_code in ('" + degreecode + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
            //string strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code in ('" + degreecode + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
            string strquery = "";
            if (sec != "")
            {
                //magesh 25.9.18
              // strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type,(C.Course_Name +'-'+dt.dept_acronym+'-'+Sections) as Degreedetails from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and Batch_Year='" + batchyear + "' and r.degree_code in ('" + degreecode + "') and sections in ('','" + sec + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
                strquery = "select r.Roll_No,Reg_No,Stud_Name,Stud_Type,(C.Course_Name +'-'+dt.dept_acronym+'-'+Sections) as Degreedetails from Registration r,Degree d,Department dt,Course c,subjectChooser sc  where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and Batch_Year='" + batchyear + "' and r.degree_code in ('" + degreecode + "') and sections in ('','" + sec + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and r.roll_no=sc.roll_no   and subject_no in(select subject_no from subject  where subject_code='" + ddl_subject.SelectedItem.Value + "') " + strorder + " ";//magesh 25.9.18
            }
            else
            {
                //magesh 25.9.18
                //strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type,(C.Course_Name +'-'+dt.dept_acronym+'-'+Sections) as Degreedetails from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and Batch_Year='" + batchyear + "' and r.degree_code in ('" + degreecode + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
                strquery = "select r.Roll_No,Reg_No,Stud_Name,Stud_Type,(C.Course_Name +'-'+dt.dept_acronym+'-'+Sections) as Degreedetails from Registration r,Degree d,Department dt,Course c,subjectChooser sc  where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and Batch_Year='" + batchyear + "' and r.degree_code in ('" + degreecode + "') and sections in ('','" + sec + "')  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and r.roll_no=sc.roll_no   and subject_no in(select subject_no from subject  where subject_code='" + ddl_subject.SelectedItem.Value + "') " + strorder + " ";//magesh 25.9.18
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            //  FpSpread1.Sheets[0].RowCount++;
            // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

            
            //FarPoint.Web.Spread.GeneralCellType txt3 =
            if (ds.Tables[0].Rows.Count > 0)
            {
                // FpSpread1.Visible = true;
                

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    srno++;
                    
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    dtl.Rows[dtl.Rows.Count - 1][0] = srno.ToString();

                    

                    string rollno = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                    string degdetails = ds.Tables[0].Rows[i]["Degreedetails"].ToString();
                    string regno = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                    string stype = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
                    string sname = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                    string[] degsplit = degdetails.Split('-');
                    string degree = "";
                    if (degsplit.Length == 3)
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                        if (degsplit[1] != "" && degsplit[1] != null)
                        {
                            degree += " - " + degsplit[1].ToString();
                        }
                        if (degsplit[2] != "" && degsplit[2] != null)
                        {
                            degree += " - " + degsplit[2].ToString();
                        }
                    }
                    else if (degsplit.Length == 2)
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                        if (degsplit[1] != "" && degsplit[1] != null)
                        {
                            degree += " - " + degsplit[1].ToString();
                        }
                    }
                    else
                    {
                        if (degsplit[0] != "" && degsplit[0] != null)
                        {
                            degree = degsplit[0].ToString();
                        }
                    }
                    if ((srno % 2) == 0)
                    {
                        
                    }

                    dtl.Rows[dtl.Rows.Count - 1][1] = degree;

                    dtl.Rows[dtl.Rows.Count - 1][2] = rollno;

                    dtl.Rows[dtl.Rows.Count - 1][3] = regno;

                    dtl.Rows[dtl.Rows.Count - 1][4] = stype;

                    dtl.Rows[dtl.Rows.Count - 1][5] = sname;

                    
                }


                firstrowscount = dtl.Rows.Count;

                string query = "";
                if (sec != "")
                {
                    query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,e.min_mark from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in ('" + degree_code + "') and c.criteria in('" + test + "') and r.sections in ('','" + sec + "') ";
                }
                else
                {
                    query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,e.min_mark from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in ('" + degree_code + "') and c.criteria in('" + test + "')";
                }
                ds = d2.select_method_wo_parameter(query, "Text");
                DataView dvnew = new DataView();
                string filterquery = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int r = 1; r < dtl.Rows.Count; r++)
                    {
                        Showgrid.Visible = true;
                        rptprint1.Visible = true;
                        query = "";
                        string rollno = dtl.Rows[r][2].ToString();

                        

                        filterquery = "Roll_No='" + rollno + "'";

                        //ds.Tables[0].DefaultView.RowFilter = "" + rollno + "";
                        ds.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                        dvnew = ds.Tables[0].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            for (int dr = 0; dr < dvnew.Count; dr++)
                            {
                                string testname = dvnew[dr]["criteria"].ToString();
                                string mark = dvnew[dr]["marks_obtained"].ToString();
                                string minmark = dvnew[dr]["min_mark"].ToString();
                                for (int c = 0; c < dtl.Columns.Count; c++)
                                {
                                    string fptestbane = dtl.Columns[c].ColumnName.ToString();

                                    

                                    if (fptestbane.Trim().ToLower() == testname.Trim().ToLower())
                                    {
                                        string marks_per = dvnew[dr]["marks_obtained"].ToString();
                                        //Convert.ToInt32(Math.Rount(mark,0)
                                        double tmark = 0;
                                        double.TryParse(mark, out tmark);
                                        if (tmark < 0)
                                        {
                                            
                                            switch (mark)
                                            {
                                                case "-1":
                                                    mark = "AAA";

                                                    break;
                                                case "-2":
                                                    mark = "EL";

                                                    break;
                                                case "-3":
                                                    mark = "EOD";

                                                    break;
                                                case "-4":
                                                    mark = "ML";

                                                    break;
                                                case "-5":
                                                    mark = "SOD";

                                                    break;
                                                case "-6":
                                                    mark = "NSS";

                                                    break;
                                                case "-7":
                                                    mark = "NJ";

                                                    break;
                                                case "-8":
                                                    mark = "S";

                                                    break;
                                                case "-9":
                                                    mark = "L";

                                                    break;
                                                case "-10":
                                                    mark = "NCC";

                                                    break;
                                                case "-11":
                                                    mark = "HS";

                                                    break;
                                                case "-12":
                                                    mark = "PP";

                                                    break;
                                                case "-13":
                                                    mark = "SYOD";

                                                    break;
                                                case "-14":
                                                    mark = "COD";

                                                    break;
                                                case "-15":
                                                    mark = "OOD";

                                                    break;
                                                case "-16":
                                                    mark = "OD";
                                                    break;
                                                case "-17":
                                                    mark = "LA";

                                                    break;

                                                case "-18":
                                                    mark = "RAA";

                                                    break;
                                            }
                                            

                                            dtl.Rows[r][c] = mark + "$Pink";
                                        }
                                        else
                                        {
                                            

                                            dtl.Rows[r][c] = mark;

                                            tmark = 0;
                                            double.TryParse(mark, out tmark);
                                            if (tmark < Convert.ToInt32(minmark))
                                            {
                                                
                                                dtl.Rows[r][c] = dtl.Rows[r][c].ToString() + "#Red";
                                            }
                                            
                                        }

                                        int num = 0;
                                        if (int.TryParse(mark, out num))
                                        {
                                            if (mark.Trim() != "")
                                            {
                                                Double markval = Convert.ToDouble(mark);
                                                if (markval >= 0)
                                                {
                                                    if (hatmaxsubmark.Contains(testname))
                                                    {
                                                        Double getmark = Convert.ToDouble(hatmaxsubmark[testname]);
                                                        if (markval > getmark)
                                                        {
                                                            hatmaxsubmark[testname] = markval;
                                                        }
                                                    }

                                                    else
                                                    {
                                                        hatmaxsubmark.Add(testname, markval);
                                                    }

                                                    if (hatminsubmark.Contains(testname))
                                                    {
                                                        Double getmark = Convert.ToDouble(hatminsubmark[testname]);
                                                        if (markval < getmark)
                                                        {
                                                            hatminsubmark[testname] = markval;
                                                        }
                                                    }

                                                    else
                                                    {
                                                        hatminsubmark.Add(testname, markval);
                                                    }


                                                    if (subjavgmark.Contains(testname))
                                                    {
                                                        Double getmark = Convert.ToDouble(subjavgmark[testname]);

                                                        if (markval >= 0)
                                                        {

                                                            subjavgmark[testname] = getmark + markval;
                                                        }
                                                        //else
                                                        //{
                                                        //    subjavgmark.Add(testname, markval);
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        subjavgmark.Add(testname, markval);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }



                    //for (int cr = 0; cr < cbl_Criteria.Items.Count; cr++)
                    //{

                    

                    

                    

                    
                    if (cbl_Criteria.Items[0].Selected == true)
                    {

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "Total No Of Students";

                        
                        ds.Clear();
                        if (sec != "")
                        {
                            query = "select r.Roll_No,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3 or re.marks_obtained =-1)";
                        }
                        else
                        {
                            query = "select r.Roll_No,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3 or re.marks_obtained =-1)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvpres = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                string test1 = dtl.Columns[cl].ColumnName;

                                

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvpres = ds.Tables[0].DefaultView;
                                if (dvpres.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvpres.Count);
                                   

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                }
                                else
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                            }
                        }
                    }
                    

                    ds.Clear();

                    if (cbl_Criteria.Items[1].Selected == true)
                    {
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Present";

                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvprescount = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName;

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvprescount = ds.Tables[0].DefaultView;
                                if (dvprescount.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvprescount.Count);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl]=Convert.ToString(pres_count);
                                }
                                else
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                            }
                        }
                    }
                    

                    if (cbl_Criteria.Items[2].Selected == true)
                    {
                        ds.Clear();
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No. of Students Absent";


                        

                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained='-1'";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and re.marks_obtained='-1'";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvabs = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName;

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvabs = ds.Tables[0].DefaultView;
                                if (dvabs.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvabs.Count);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                }
                                else
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                            }
                        }
                    }
                    //  "NO OF STUDENTS PASSED FOR " + txtoptiminpassmark.Text + "%:"
                    query = "";
                    ds.Clear();
                    if (cbl_Criteria.Items[3].Selected == true)
                    {
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        

                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataView dvab60 = new DataView();
                            string min_mark = ds.Tables[0].Rows[0]["min_mark"].ToString();
                            
                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Passed (For " + min_mark + ")";



                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName;

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvab60 = ds.Tables[0].DefaultView;
                                if (dvab60.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvab60.Count);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                }
                                else
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                               
                            }
                        }
                        else
                        {
                            string min_mark = ds.Tables[0].Rows[0]["min_mark"].ToString();
                            

                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Passed (For " + min_mark + ")";


                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                            }
                        }
                    }

                    query = "";
                    ds.Clear();
                    if (cbl_Criteria.Items[4].Selected == true)
                    {
                        
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        

                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17')";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17')";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvfl = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string min_mark = ds.Tables[0].Rows[0]["min_mark"].ToString();
                            

                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Failed (For " + min_mark + ")";



                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName;

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvfl = ds.Tables[0].DefaultView;
                                if (dvfl.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvfl.Count);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                }
                                else
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                                
                            }
                        }
                        else
                        {
                            string min_mark = "0";
                            
                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Failed (For " + min_mark + ")";


                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                            }
                        }
                    }

                    query = "";
                    if (cbl_Criteria.Items[5].Selected == true)
                    {
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);

                        ds.Clear();
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string min_mark = ds.Tables[1].Rows[0]["min_mark"].ToString();
                            
                            dtl.Rows[dtl.Rows.Count - 1][0] = "Pass Percentage (For " + min_mark + ")";

                            

                            DataView dvpasscont = new DataView();
                            DataView dvprescent = new DataView();
                            drp = dtChart1.NewRow();
                            

                            drp[0] = dtl.Rows[dtl.Rows.Count - 1][0].ToString();
                            int ps = 1;
                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName;

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                ds.Tables[1].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";

                                dvprescent = ds.Tables[0].DefaultView;
                                dvpasscont = ds.Tables[1].DefaultView;
                                if (dvpasscont.Count > 0 && dvprescent.Count > 0)
                                {
                                    int passcount = Convert.ToInt32(dvpasscont.Count);
                                    int presscount = Convert.ToInt32(dvprescent.Count);
                                    Double final_pperc = (Convert.ToDouble(passcount) / Convert.ToDouble(presscount)) * 100;

                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(Math.Round(final_pperc, 2));

                                    

                                    drp[ps] = dtl.Rows[dtl.Rows.Count - 1][cl].ToString();

                                    

                                    
                                }
                                else
                                {
                                    

                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                    

                                    drp[ps] = dtl.Rows[dtl.Rows.Count - 1][cl].ToString();

                                    
                                }
                                ps++;
                            }
                            dtChart1.Rows.Add(drp);
                        }
                    }

                    query = "";
                    ds.Clear();
                    if (cbl_Criteria.Items[3].Selected == true)
                    {
                        if (txtoptiminpassmark.Text != "")
                        {
                            
                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);


                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Passed (For " + txtoptiminpassmark.Text.ToString() + ")";


                            

                            

                            
                            if (sec != "")
                            {
                                query = "select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text + "' or re.marks_obtained=-2 or re.marks_obtained=-3)";
                            }
                            else
                            {
                                query = "select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text + "' or re.marks_obtained=-2 or re.marks_obtained=-3)";
                            }
                            ds = d2.select_method_wo_parameter(query, "Text");
                            DataView dvab60 = new DataView();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int cl = 6; cl < dtl.Columns.Count; cl++)
                                {
                                    

                                    string test1 = dtl.Columns[cl].ColumnName;

                                    ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                    dvab60 = ds.Tables[0].DefaultView;
                                    if (dvab60.Count > 0)
                                    {
                                        int pres_count = Convert.ToInt32(dvab60.Count);
                                        

                                        dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                    }
                                    else
                                    {
                                        

                                        dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                    }
                                    
                                }
                            }
                            else
                            {
                                for (int cl = 6; cl < dtl.Columns.Count; cl++)
                                {
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                }
                            }
                        }
                    }

                    ds.Clear();
                    if (cbl_Criteria.Items[4].Selected == true)
                    {
                        if (txtoptiminpassmark.Text != "")
                        {
                            

                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);

                            dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students Failed (For " + txtoptiminpassmark.Text.ToString() + ")";
                            


                            
                            if (cbl_Criteria.Items[4].Selected == true)
                            {
                                if (sec != "")
                                {

                                    query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained<'" + txtoptiminpassmark.Text + "' and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17') ";
                                }
                                else
                                {
                                    query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained<'" + txtoptiminpassmark.Text + "' and re.marks_obtained<>'-2' and re.marks_obtained<>'-3' and re.marks_obtained<>'-1' and re.marks_obtained<>'-4' and re.marks_obtained<>'-5' and re.marks_obtained<>'-6' and re.marks_obtained<>'-7' and re.marks_obtained<>'-8' and re.marks_obtained<>'-9' and re.marks_obtained<>'-10' and re.marks_obtained<>'-11' and re.marks_obtained<>'-12' and re.marks_obtained<>'-13' and re.marks_obtained<>'-14' and re.marks_obtained<>'-15' and re.marks_obtained<>'-16' and re.marks_obtained<>'-17') ";
                                }
                                ds = d2.select_method_wo_parameter(query, "Text");
                                DataView dvabs = new DataView();
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int cl = 6; cl < dtl.Columns.Count; cl++)
                                    {
                                        

                                        string test1 = dtl.Columns[cl].ColumnName;

                                        ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                        dvabs = ds.Tables[0].DefaultView;
                                        if (dvabs.Count > 0)
                                        {
                                            int pres_count = Convert.ToInt32(dvabs.Count);
                                            
                                            dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                        }
                                        else
                                        {
                                            

                                            dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                        }
                                        
                                    }
                                }
                                else
                                {
                                    for (int cl = 6; cl < dtl.Columns.Count; cl++)
                                    {
                                        

                                        dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                    }
                                }
                            }
                        }
                    }


                    query = "";
                    ds.Clear();
                    if (cbl_Criteria.Items[5].Selected == true)
                    {
                        if (txtoptiminpassmark.Text != "")
                        {
                            

                            dtrow = dtl.NewRow();
                            dtl.Rows.Add(dtrow);

                            if (sec != "")
                            {
                                query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or re.marks_obtained=-2 or re.marks_obtained=-3)";
                            }
                            else
                            {
                                query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and (re.marks_obtained>='" + txtoptiminpassmark.Text.ToString() + "' or re.marks_obtained=-2 or re.marks_obtained=-3)";
                            }
                            ds = d2.select_method_wo_parameter(query, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][0] = "Pass Percentage (For " + txtoptiminpassmark.Text.ToString() + ")";

                                

                                DataView dvpasscont = new DataView();
                                DataView dvprescent = new DataView();
                                int drpts = 1;
                                drpt = dtChart1.NewRow();
                                

                                drpt[0] = dtl.Rows[dtl.Rows.Count - 1][0].ToString();

                                for (int cl = 6; cl < dtl.Columns.Count; cl++)
                                {
                                    

                                    string test1 = dtl.Columns[cl].ColumnName.ToString();

                                    ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                    ds.Tables[1].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                    dvprescent = ds.Tables[0].DefaultView;
                                    dvpasscont = ds.Tables[1].DefaultView;
                                    if (dvpasscont.Count > 0 && dvprescent.Count > 0)
                                    {
                                        int passcount = Convert.ToInt32(dvpasscont.Count);
                                        int presscount = Convert.ToInt32(dvprescent.Count);
                                        Double final_pperc = (Convert.ToDouble(passcount) / Convert.ToDouble(presscount)) * 100;
                                        //dr[FpEntry.Sheets[0].ColumnHeader.Cells[0, i + 1].Text.ToString()] = Convert.ToString(Math.Round(final_pperc, 2));

                                        

                                        dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(Math.Round(final_pperc, 2));
                                        
                                        drpt[drpts] = dtl.Rows[dtl.Rows.Count - 1][cl].ToString();

                                        

                                    }
                                    else
                                    {
                                        

                                        dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                        
                                        drpt[drpts] = dtl.Rows[dtl.Rows.Count - 1][cl].ToString();
                                        
                                    }
                                    
                                    drpts++;
                                }
                                dtChart1.Rows.Add(drpt);
                            }
                        }

                    }

                    if (cbl_Criteria.Items[6].Selected == true)
                    {
                        ds.Clear();
                        
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "Total Mark";


                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            

                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            string cri_no = "";
                            if (sec != "")
                            {
                                query = "select sum(re.marks_obtained)as total,c.criteria from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test1 + "') and re.marks_obtained>=0 group by criteria";
                            }
                            else
                            {
                                query = "select sum(re.marks_obtained)as total,c.criteria from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test1 + "') and re.marks_obtained>=0 group by criteria";
                            }
                            ds = d2.select_method_wo_parameter(query, "Text");
                            DataView dvtot = new DataView();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvtot = ds.Tables[0].DefaultView;
                                if (dvtot.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dvtot[0]["total"]);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                }
                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                            }
                        }

                    }

                    if (cbl_Criteria.Items[7].Selected == true)
                    {
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);

                        dtl.Rows[dtl.Rows.Count - 1][0] = "Subject Average";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select sum(re.marks_obtained)as total,c.criteria from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and re.marks_obtained>=0 group by criteria";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3);select sum(re.marks_obtained)as total,c.criteria from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and re.marks_obtained>=0 group by criteria";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvprescount = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName.ToString();
                                double subjtotel = 0;
                                double subavg = 0;
                                int pres_count = 0;
                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dvprescount = ds.Tables[0].DefaultView;
                                if (dvprescount.Count > 0)
                                {
                                    pres_count = Convert.ToInt32(dvprescount.Count);
                                }
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ds.Tables[1].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                    dvprescount = ds.Tables[1].DefaultView;
                                    if (dvprescount.Count > 0)
                                    {
                                        subjtotel = Convert.ToInt32(dvprescount[0]["total"]);
                                    }
                                }
                                subavg = subjtotel / pres_count;
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(Math.Round(subavg, 2));

                               
                                if (subjavgmark.Contains(test1))
                                {
                                    //subjtotel = Convert.ToDouble(subjavgmark[test1]);
                                    //subavg = subjtotel / srno;
                                    //  subavg = Math.Round(subavg, 0, MidpointRounding.AwayFromZero);
                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cl].Text = Convert.ToString(subavg);
                                    
                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(Math.Round(subavg, 2));

                                    
                                }
                                else
                                {
                                    
                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(Math.Round(0.0, 2));

                                    
                                }

                            }
                        }
                    }

                    if (cbl_Criteria.Items[8].Selected == true)
                    {

                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);

                        dtl.Rows[dtl.Rows.Count - 1][0] = "Max. Mark";



                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            

                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            string markva = "";
                            if (hatmaxsubmark.Contains(test1))
                            {
                                markva = Convert.ToString(hatmaxsubmark[test1]);

                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = markva;

                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                            }
                        }
                    }
                    if (cbl_Criteria.Items[9].Selected == true)
                    {
                        
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);

                        dtl.Rows[dtl.Rows.Count - 1][0] = "Min Mark";


                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            string minval = "";

                            if (hatminsubmark.Contains(test1))
                            {
                                minval = Convert.ToString(hatminsubmark[test1]);
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = minval;

                                

                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                
                            }
                        }

                    }


                    if (cbl_Criteria.Items[10].Selected == true)
                    {
                        query = "";
                        ds.Clear();
                        
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);

                        dtl.Rows[dtl.Rows.Count - 1][0] = "No. Of Students (S GRADE) >90%";
                        

                        s_grage = dtChart2.NewRow();
                        //and sections in ('','"+sec+"')
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained >=90 ";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,c.criteria,c.Criteria_no,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and re.marks_obtained >=90 ";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dvsg = new DataView();
                        s_grage[0] = "No. Of Students (S Grade ) >90%";
                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dvsg = ds.Tables[0].DefaultView;
                            if (dvsg.Count > 0)
                            {
                                // int pres_count = Convert.ToInt32(dvnew[0]["maxmark"]);
                                int pres_count = Convert.ToInt32(dvsg.Count);
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                
                            }
                            else
                            {
                               

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                
                            }
                            

                            s_grage[cl - 5] = dtl.Rows[dtl.Rows.Count - 1][cl].ToString();
                        }
                        dtChart2.Rows.Add(s_grage);
                    }

                    query = "";
                    ds.Clear();
                    

                    

                    

                   
                    if (cbl_Criteria.Items[11].Selected == true)
                    {
                        

                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.Of Students >80%-89%";

                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained between 80 and 89";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and re.marks_obtained between 80 and 89";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv80 = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int cl = 6; cl < dtl.Columns.Count; cl++)
                            {
                                

                                string test1 = dtl.Columns[cl].ColumnName.ToString();

                                ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                                dv80 = ds.Tables[0].DefaultView;
                                if (dv80.Count > 0)
                                {
                                    int pres_count = Convert.ToInt32(dv80.Count);
                                    

                                    dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                    
                                }
                                else
                                {
                                    
                                    dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                    
                                }
                            }
                        }
                    }

                    query = "";
                    ds.Clear();
                    
                    if (cbl_Criteria.Items[12].Selected == true)
                    {
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No. of Students >70%-79%";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained between 70 and 79";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and re.marks_obtained between 70 and 79";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv70 = new DataView();

                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dv70 = ds.Tables[0].DefaultView;
                            if (dv70.Count > 0)
                            {
                                int pres_count = Convert.ToInt32(dv70.Count);
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                
                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                

                            }

                        }
                    }
                    query = "";
                    ds.Clear();
                    
                    if (cbl_Criteria.Items[13].Selected == true)
                    {
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students >60%-69%";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained between 60 and 69";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and re.marks_obtained between 60 and 69";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv70 = new DataView();

                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dv70 = ds.Tables[0].DefaultView;
                            if (dv70.Count > 0)
                            {
                                int pres_count = Convert.ToInt32(dv70.Count);

                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);
                                

                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";
                                
                            }
                        }
                    }

                    query = "";
                    ds.Clear();
                    
                    if (cbl_Criteria.Items[14].Selected == true)
                    {
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students >50%-59%";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained between 50 and 59";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar'  and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and re.marks_obtained between 50 and 59";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv70 = new DataView();


                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dv70 = ds.Tables[0].DefaultView;
                            if (dv70.Count > 0)
                            {
                                int pres_count = Convert.ToInt32(dv70.Count);
                                
                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                
                            }
                            else
                            {
                                
                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                
                            }
                        }
                    }

                    query = "";
                    ds.Clear();
                    
                    if (cbl_Criteria.Items[15].Selected == true)
                    {
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students <49%";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained <49 and re.marks_obtained>=0";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and re.marks_obtained <49 and re.marks_obtained>=0";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv70 = new DataView();

                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dv70 = ds.Tables[0].DefaultView;
                            if (dv70.Count > 0)
                            {
                                int pres_count = Convert.ToInt32(dv70.Count);

                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                

                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                
                            }
                        }
                    }

                    query = "";
                    ds.Clear();
                    
                    if (cbl_Criteria.Items[16].Selected == true)
                    {
                        dtrow = dtl.NewRow();
                        dtl.Rows.Add(dtrow);
                        dtl.Rows[dtl.Rows.Count - 1][0] = "No.of Students <45% ( University Cut Off )";
                        
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and re.marks_obtained <45 and re.marks_obtained>=0";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "')  and c.criteria in('" + test + "') and re.marks_obtained <45 and re.marks_obtained>=0";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        DataView dv45 = new DataView();

                        for (int cl = 6; cl < dtl.Columns.Count; cl++)
                        {
                            
                            string test1 = dtl.Columns[cl].ColumnName.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "" + "criteria='" + test1 + "' " + "";
                            dv45 = ds.Tables[0].DefaultView;
                            if (dv45.Count > 0)
                            {
                                int pres_count = Convert.ToInt32(dv45.Count);
                                
                                dtl.Rows[dtl.Rows.Count - 1][cl] = Convert.ToString(pres_count);

                                
                            }
                            else
                            {
                                

                                dtl.Rows[dtl.Rows.Count - 1][cl] = "0";

                                
                            }
                        }

                    }
                    if (cbl_Criteria.Items[17].Selected == true && cbl_Criteria.Items[3].Selected == true)
                    {
                        ChartPassPercent.Visible = true;
                        query = "";
                        ds.Clear();
                        if (sec != "")
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and r.sections in ('','" + sec + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        else
                        {
                            query = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>=0 or re.marks_obtained=-2 or re.marks_obtained=-3) select r.Roll_No,s.subject_name, e.min_mark,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and cc=0 and DelFlag=0 and Exam_Flag<>'debar' and s.subject_code='" + ddl_subject.SelectedItem.Value + "' and sy.Batch_Year='" + ddl_Batchyear.SelectedItem.Text.ToString() + "' and sy.semester='" + ddl_semester.SelectedItem.Text.ToString() + "' and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        ChartPassPercent.Series.Clear();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string min_mark = ds.Tables[1].Rows[0]["min_mark"].ToString();
                            ChartPassPercent.Series.Add("Pass%(Actual Min.Mark " + min_mark + ")");
                            ChartPassPercent.Series.Add("Pass%(Optional Min.Mark " + txtoptiminpassmark.Text + ")");

                            if (dtChart1.Rows.Count > 0)
                            {
                                chart_passpercentage.DataSource = dtChart1;
                                chart_passpercentage.DataBind();
                                for (int r = 0; r < dtChart1.Rows.Count; r++)
                                {
                                    for (int c = 1; c < dtChart1.Columns.Count; c++)
                                    {
                                        ChartPassPercent.Series[r].Points.AddXY(dtChart1.Columns[c].ToString(), dtChart1.Rows[r][c].ToString());
                                        ChartPassPercent.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        ChartPassPercent.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                        ChartPassPercent.Series[r].IsValueShownAsLabel = true;
                                        ChartPassPercent.Series[r].IsXValueIndexed = true;

                                        ChartPassPercent.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        ChartPassPercent.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    }
                                    chart_passpercentage.Rows[r].HorizontalAlign = HorizontalAlign.Center;
                                    chart_passpercentage.Rows[r].Font.Bold = true;
                                }

                                //chart_passpercentage.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                //chart_passpercentage.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                //chart_passpercentage.Rows[0].Font.Bold = true;
                                //chart_passpercentage.Rows[0].Font.Bold = true;
                                //chart_passpercentage.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                                chart_passpercentage.Visible = true;
                            }
                        }
                    }

                    if (cbl_Criteria.Items[18].Selected == true && cbl_Criteria.Items[10].Selected == true)
                    {
                        S_GRADE.Series.Clear();
                        S_GRADE.Series.Add("No.of_Student (S GRADE) >90%");
                        if (dtChart2.Rows.Count > 0)
                        {
                            S_GRADE.Visible = true;
                            for (int r = 0; r < dtChart2.Rows.Count; r++)
                            {
                                for (int c = 1; c < dtChart2.Columns.Count; c++)
                                {
                                    S_GRADE.Series[r].Points.AddXY(dtChart2.Columns[c].ToString(), dtChart2.Rows[r][c].ToString());
                                    S_GRADE.Series[r].IsValueShownAsLabel = true;
                                    S_GRADE.Series[r].IsXValueIndexed = true;
                                    S_GRADE.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    S_GRADE.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    S_GRADE.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    S_GRADE.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                                }
                            }
                            S_grads.Visible = true;
                            GridViewchart.Visible = true;
                            GridViewchart.DataSource = dtChart2;
                            GridViewchart.DataBind();
                            GridViewchart.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                            GridViewchart.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                            GridViewchart.Rows[0].Font.Bold = true;
                            GridViewchart.Rows[0].Font.Bold = true;
                            GridViewchart.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Left;
                            // GridViewchart.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                    


                    if (dtl.Rows.Count > 0)
                    {
                        Showgrid.DataSource = dtl;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;
                        Showgrid.HeaderRow.Visible = false;
                        int dtrowcount = firstrowscount;
                        
                        int rowspanstart1 = 0;
                        int spancolvis=0;
                        for (int dd = 0; dd < falsecol.Count; dd++)
                        {
                            int g = Convert.ToInt32(falsecol[dd]);
                            if (g < 6)
                                spancolvis++;
                            Showgrid.HeaderRow.Cells[g].Visible = false;
                            for (int j = 0; j < Showgrid.Rows.Count; j++)
                                Showgrid.Rows[j].Cells[g].Visible = false;
                        }



                        for (int i = 0; i < Showgrid.Rows.Count; i++)
                        {
                            
                            int rowspancount1 = 0;


                            if (firstrowscount > i && i != 0)
                                {
                                    if (((i + 1) % 2) != 0)
                                    {


                                        Showgrid.Rows[i].BackColor = System.Drawing.Color.LightGray;
                                    }
                                }
                            
                                if (i < dtrowcount)
                                {

                                    if (rowspanstart1 == i)
                                    {
                                        for (int k = rowspanstart1 + 1; Showgrid.Rows[i].Cells[1].Text == Showgrid.Rows[k].Cells[1].Text; k++)
                                        {
                                            rowspancount1++;
                                            if (k == dtrowcount - 1)
                                                break;
                                        }
                                        rowspanstart1++;
                                    }
                                    if (rowspancount1 != 0)
                                    {
                                        rowspanstart1 = rowspanstart1 + rowspancount1;

                                        Showgrid.Rows[i].Cells[1].RowSpan = rowspancount1 + 1;
                                        for (int a = i; a < rowspanstart1 - 1; a++)
                                            Showgrid.Rows[a + 1].Cells[1].Visible = false;

                                    }


                                }
                            
                            for (int j = 0; j < dtl.Columns.Count; j++)
                            {
                                if (i == 0)
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                }
                                else
                                {
                                    if (j != 2 && j != 3 && j != 4 && j != 5)
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                    string rrr = Showgrid.Rows[i].Cells[j].Text;
                                    string[] splitval6 = rrr.Split('$');
                                    string[] splitval2 = rrr.Split('#');
                                    if (splitval6.Length > 1 || splitval2.Length > 1)
                                    {
                                        if (splitval6.Length > 1)
                                        {
                                            Showgrid.Rows[i].Cells[j].Text = splitval6[0].ToString();
                                            Showgrid.Rows[i].Cells[j].BackColor = System.Drawing.Color.LightPink;

                                        }
                                        else if (splitval2.Length > 1)
                                        {
                                            Showgrid.Rows[i].Cells[j].Text = splitval2[0].ToString();

                                            Showgrid.Rows[i].Cells[j].ForeColor = System.Drawing.Color.Red;
                                            Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;

                                        }


                                    }
                                    else
                                        Showgrid.Rows[i].Cells[j].Text = rrr;


                                    if (firstrowscount <= i && j == 0)
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = (6 - spancolvis);

                                        for (int a = 1; a < (6); a++)
                                        {
                                            Showgrid.Rows[i].Cells[a + j].Visible = false;
                                        }
                                    }

                                }

                            }





                        }

                        //for (int p = 0; p < spancolvis; p++)
                        //{
                        //    for (int y = firstrowscount; y < Showgrid.Rows.Count; y++)
                        //        Showgrid.Rows[y].Cells[p + 5].Visible = false;
                        //}



                       

                           
                    }
                }

                else
                {
                    Showgrid.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    rptprint1.Visible = false;
                    S_grads.Visible = false;
                    ChartPassPercent.Visible = false;
                    chart_passpercentage.Visible = false;
                }
            }
            else
            {
                Showgrid.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                rptprint1.Visible = false;
                S_grads.Visible = false;
                ChartPassPercent.Visible = false;
                chart_passpercentage.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        errmsg.Visible = false;
        Showgrid.Visible = false;
        Printcontrol.Visible = false;
    }

    public void BindBatch()
    {
        try
        {
            ds = d2.select_method_wo_parameter("bind_batch", "sp");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Batchyear.DataSource = ds;
                ddl_Batchyear.DataTextField = "batch_year";
                ddl_Batchyear.DataValueField = "batch_year";
                ddl_Batchyear.DataBind();
                ddl_Batchyear.SelectedIndex = ddl_Batchyear.Items.Count - 1;

            }
            //bindbranch();
            //bindsem();
            //Subject();
            //bindtestname();
        }
        catch
        {
        }


    }

    protected void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            S_grads.Visible = false;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            Printcontrol.Visible = false;
            clear();
            int count = 0;
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
            bindsec();
            bindtestname();
            Subject();
        }
        catch (Exception ex)
        {
        }

    }

    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int commcount = 0;
            clear();
            S_grads.Visible = false;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
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
                txt_degree.Text = "Degree (" + commcount.ToString() + ")";
            }
            bindbranch();
            bindsem();
            bindsec();
            bindtestname();
            Subject();
        }
        catch (Exception ex)
        {

        }

    }

    public void binddegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            txt_degree.Text = "---Select---";
            cb_degree.Checked = false;
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);


            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();

                for (int h = 0; h < cbl_degree.Items.Count; h++)
                {
                    cbl_degree.Items[h].Selected = true;
                }
                txt_degree.Text = "Degree" + "(" + cbl_degree.Items.Count + ")";
                cb_degree.Checked = true;
            }

            //bindbranch();
            //bindsem();
            //Subject();
            //bindtestname();
        }
        catch
        {

        }
    }

    protected void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            clear();
            S_grads.Visible = false;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            txt_branch.Text = "--Select--";
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            if (cb_branch.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Department(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem();
            bindsec();
            bindtestname();
            Subject();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_grads.Visible = false;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            int commcount = 0;
            cb_branch.Checked = false;
            txt_branch.Text = "--Select--";
            int commcount1 = 0;

            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {

                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Department(" + commcount.ToString() + ")";

            }
            bindsem();
            bindsec();
            bindtestname();
            Subject();
        }
        catch (Exception ex)
        {

        }
    }

    //public void bindbranch()
    //{
    //    try
    //    {
    //        cbl_branch.Items.Clear();

    //        string course_id = "";
    //        if (cbl_degree.Items.Count > 0)
    //        {
    //            for (int row = 0; row < cbl_degree.Items.Count; row++)
    //            {
    //                if (cbl_degree.Items[row].Selected == true)
    //                {
    //                    if (course_id == "")
    //                    {
    //                        course_id = Convert.ToString(cbl_degree.Items[row].Value);
    //                    }
    //                    else
    //                    {
    //                        course_id = course_id + "," + Convert.ToString(cbl_degree.Items[row].Value);
    //                    }
    //                }
    //            }

    //        }

    //        string query = "";
    //        if (course_id != "")
    //        {
    //            ds.Clear();


    //            query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code in ('" + collegecode + "')";

    //            ds = d2.select_method_wo_parameter(query, "Text");
    //            //   ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_branch.DataSource = ds;
    //                cbl_branch.DataTextField = "dept_name";
    //                cbl_branch.DataValueField = "degree_code";
    //                cbl_branch.DataBind();
    //                if (cbl_branch.Items.Count > 0)
    //                {
    //                    for (int row = 0; row < cbl_branch.Items.Count; row++)
    //                    {
    //                        cbl_branch.Items[row].Selected = true;
    //                    }
    //                    cb_branch.Checked = true;
    //                    txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
    //                }

    //            }
    //        }
    //        else
    //        {
    //            cb_branch.Checked = false;
    //            txt_branch.Text = "--Select--";
    //        }
    //    }

    //    catch
    //    {
    //    }
    //}

    public void bindbranch()
    {
        try
        {
            string degreecode = "";
            txt_branch.Text = "---Select---";
            cb_branch.Checked = false;
            cbl_branch.Items.Clear();
            for (int h = 0; h < cbl_degree.Items.Count; h++)
            {
                if (cbl_degree.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = cbl_degree.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + cbl_degree.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degreecode, collegecode = Session["collegecode"].ToString(), Session["usercode"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    for (int h = 0; h < cbl_branch.Items.Count; h++)
                    {
                        cbl_branch.Items[h].Selected = true;
                    }
                    txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
                    cb_branch.Checked = true;
                }
            }
        }
        catch
        {

        }
    }

    public void bindsem()
    {
        try
        {
            string degreecode = "";
            ddl_semester.Items.Clear();
            for (int h = 0; h < cbl_branch.Items.Count; h++)
            {
                if (cbl_branch.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = cbl_branch.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + cbl_branch.Items[h].Value;
                    }
                }
            }
            string strgetfuncuti = d2.GetFunction("select max(Duration) from Degree");
            if (degreecode.Trim() != "")
            {
                strgetfuncuti = d2.GetFunction("select max(Duration) from Degree where Degree_Code in(" + degreecode + ")");
            }
            for (int loop_val = 1; loop_val <= Convert.ToInt16(strgetfuncuti); loop_val++)
            {
                ddl_semester.Items.Add(loop_val.ToString());
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void bindtestname()
    {
        try
        {
            Txt_Test.Text = "--Select--";
            Cb_test.Checked = false;
            Cbl_test.Items.Clear();

            string testbatchyear = "";
            testbatchyear = ddl_Batchyear.SelectedItem.Value.ToString();

            string testbranch = "";
            for (int j = 0; j < cbl_branch.Items.Count; j++)
            {
                if (cbl_branch.Items[j].Selected == true)
                {
                    if (testbranch == "")
                    {
                        testbranch = "'" + cbl_branch.Items[j].Value.ToString() + "'";
                    }
                    else
                    {
                        testbranch = testbranch + ",'" + cbl_branch.Items[j].Value.ToString() + "'";
                    }
                }
            }

            if (testbatchyear.Trim() != "" && testbranch.Trim() != "")
            {
                // string Sqlstr = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,Registration r where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester='" + ddl_semester.SelectedItem.ToString() + "' and r.cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0 and sy.Batch_Year = '" + testbatchyear + "' and sy.degree_code in(" + testbranch + ") order by criteria";
                // string Sqlstr = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,Registration r where sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and sy.degree_code=r.degree_code and sy.Batch_Year=r.Batch_Year and sy.semester='" + ddl_semester.SelectedItem.ToString() + "' and r.cc=0 and r.Exam_Flag<>'debar' and r.DelFlag=0  and sy.degree_code in(" + testbranch + ") order by criteria";


                //string sylyear = d2.GetFunction(" select syllabus_year from syllabus_master where degree_code in(" + testbranch + ") and semester ='" + ddl_semester.SelectedItem.Value + "' and batch_year='" + testbatchyear + "'");
                //string strtest = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in (" + testbranch +") and semester ='" + ddl_semester.SelectedItem.Value + "' and batch_year='" + testbatchyear + "' and syllabus_year='" + sylyear + "' order by criteria ";


                string Sqlstr = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar'   and r.batch_year='" + testbatchyear + "' and r.degree_code in(" + testbranch + ") and  s.semester='" + ddl_semester.SelectedItem.ToString() + "' order by criteria asc";

                ds = d2.select_method_wo_parameter(Sqlstr, "Text");
                DataSet titles = new DataSet();
                titles.Clear();
                titles.Dispose();
                titles = d2.select_method_wo_parameter(Sqlstr, "Test");
                if (titles.Tables[0].Rows.Count > 0)
                {
                    Cbl_test.DataSource = titles;
                    //  Cbl_test.DataValueField = "criteria_no";
                    Cbl_test.DataTextField = "criteria";
                    Cbl_test.DataBind();
                }
                if (Cbl_test.Items.Count > 0)
                {
                    for (int row = 0; row < Cbl_test.Items.Count; row++)
                    {
                        Cbl_test.Items[row].Selected = true;
                        Cb_test.Checked = true;
                    }
                    Txt_Test.Text = "Test(" + Cbl_test.Items.Count + ")";
                }
                else
                {

                    Txt_Test.Text = "--Select--";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void Subject()
    {
        try
        {
            ds.Clear();
            ddl_subject.Items.Clear();
            ddl_subject.Items.Insert(0, "--Select--");
            string Year = "";

            Year = ddl_Batchyear.SelectedItem.Value.ToString();
            string branchcode = "";
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "','" + Convert.ToString(cbl_branch.Items[i].Value);
                        }
                    }
                }
            }

            string sem = "";

            sem = ddl_semester.SelectedItem.Value.ToString();
            if (branchcode != "")
            {

                string sub_name = "";
                sub_name = " select distinct s.subject_name, s.subject_code from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and ss.promote_count=1 and sy.Batch_Year='" + Year + "' and sy.degree_code in ('" + branchcode + "') and sy.semester in ('" + sem + "') order by s.subject_name,s.subject_code";
                //  ds = d2.select_method_wo_parameter(sub_name, "Text");
                // sub_name = "select distinct   s.subject_no,s.subject_name from exam_type e,subject s,result r  where e.subject_no=s.subject_no and e.exam_code= r.exam_code and criteria_no in ('" + criteria + "')";

                ds = d2.select_method_wo_parameter(sub_name, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_subject.DataSource = ds;
                    ddl_subject.DataTextField = "subject_name";
                    ddl_subject.DataValueField = "subject_code";
                    ddl_subject.DataBind();
                }

            }


        }
        catch (Exception ex)
        {
        }

    }

    public void bindsec()
    {
        DataSet ds = new DataSet();
        string batch = "";
        string branch = "";
        txt_sec.Text = "---Select---";
        cb_Sec.Checked = false;
        cbl_sec.Items.Clear();
        batch = ddl_Batchyear.SelectedValue.ToString();
        for (int h = 0; h < cbl_branch.Items.Count; h++)
        {
            if (cbl_branch.Items[h].Selected == true)
            {
                if (branch == "")
                {
                    branch = cbl_branch.Items[h].Value;
                }
                else
                {
                    branch = branch + ',' + cbl_branch.Items[h].Value;
                }
            }
        }
        if (branch.Trim() != "")
        {
            ds = d2.BindSectionDetail(batch, branch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sec.DataSource = ds;
                cbl_sec.DataTextField = "sections";
                cbl_sec.DataValueField = "sections";
                cbl_sec.DataBind();
                for (int h = 0; h < cbl_sec.Items.Count; h++)
                {
                    cbl_sec.Items[h].Selected = true;
                }
                txt_sec.Text = "section(" + (cbl_sec.Items.Count) + ")";
                txt_sec.Enabled = true;
                cb_Sec.Checked = true;
            }
            else
            {
                txt_sec.Text = "---Select---";
                txt_sec.Enabled = false;
            }
        }
    }

    protected void cb_Sec_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        clear();
        txt_sec.Text = "--Select--";
        S_GRADE.Visible = false;
        S_grads.Visible = false;
        GridViewchart.Visible = false;
        rptprint1.Visible = false;
        Printcontrol.Visible = false;
        ChartPassPercent.Visible = false;
        chart_passpercentage.Visible = false;
        Showgrid.Visible = false;
        if (cb_Sec.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                cbl_sec.Items[i].Selected = true;
            }
            txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                cbl_sec.Items[i].Selected = false;
            }
            txt_sec.Text = "--Select--";
        }
    }

    protected void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChartPassPercent.Visible = false;
        clear();
        S_GRADE.Visible = false;
        S_grads.Visible = false;
        GridViewchart.Visible = false;
        rptprint1.Visible = false;
        chart_passpercentage.Visible = false;
        Printcontrol.Visible = false;
        Showgrid.Visible = false;
        int commcount = 0;
        cb_Sec.Checked = false;
        txt_sec.Text = "--Select--";
        int commcount1 = 0;

        for (int i = 0; i < cbl_sec.Items.Count; i++)
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

                cb_Sec.Checked = true;
            }
            txt_sec.Text = "Section(" + commcount.ToString() + ")";

        }
    }

    protected void Cb_test_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            Printcontrol.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            int cout = 0;
            Txt_Test.Text = "--Select--";
            if (Cb_test.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_test.Items.Count; i++)
                {
                    Cbl_test.Items[i].Selected = true;
                }
                Txt_Test.Text = "Test(" + (Cbl_test.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_test.Items.Count; i++)
                {
                    Cbl_test.Items[i].Selected = false;
                }
                Txt_Test.Text = "--Select--";
            }
            Subject();

        }

        catch (Exception ex)
        {

        }
    }

    protected void Cbl_test_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;

            Printcontrol.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            int commcount = 0;
            Txt_Test.Text = "--Select--";
            Cb_test.Checked = false;

            for (int i = 0; i < Cbl_test.Items.Count; i++)
            {
                if (Cbl_test.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_test.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_test.Items.Count)
                {

                    Cb_test.Checked = true;
                }
                Txt_Test.Text = "Test(" + commcount.ToString() + ")";

            }
            Subject();

        }

        catch (Exception ex)
        {

        }
    }

    protected void ddl_subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            //  bindtestname();
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
        }
        catch
        {
        }

    }

    protected void ddl_Batchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            bindtestname();
            Subject();
        }
        catch
        {

        }

    }

    protected void ddl_semester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            Printcontrol.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            bindsec();
            bindtestname();
            Subject();
        }
        catch
        {
        }

    }

    public void criteria()
    {
        if (cbl_Criteria.Items.Count > 0)
        {
            for (int row = 0; row < cbl_Criteria.Items.Count; row++)
            {
                cbl_Criteria.Items[row].Selected = true;
                cb_Criteria.Checked = true;
            }
            TextBox1.Text = "Criteria(" + cbl_Criteria.Items.Count + ")";

        }
    }

    protected void cbl_Criteria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            S_GRADE.Visible = false;
            S_grads.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            ChartPassPercent.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            int commcount = 0;
            TextBox1.Text = "--Select--";
            cb_Criteria.Checked = false;

            for (int i = 0; i < cbl_Criteria.Items.Count; i++)
            {
                if (cbl_Criteria.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_Criteria.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_Criteria.Items.Count)
                {

                    cb_Criteria.Checked = true;
                }
                TextBox1.Text = "Criteria(" + commcount.ToString() + ")";

            }
            //bindhostelname();
        }

        catch (Exception ex)
        {

        }
    }

    protected void cb_Criteria_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            ChartPassPercent.Visible = false;
            S_grads.Visible = false;
            S_GRADE.Visible = false;
            GridViewchart.Visible = false;
            rptprint1.Visible = false;
            chart_passpercentage.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            int cout = 0;
            TextBox1.Text = "--Select--";
            if (cb_Criteria.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_Criteria.Items.Count; i++)
                {
                    cbl_Criteria.Items[i].Selected = true;
                }
                TextBox1.Text = "Criteria (" + (cbl_Criteria.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_Criteria.Items.Count; i++)
                {
                    cbl_Criteria.Items[i].Selected = false;
                }
                TextBox1.Text = "--Select--";
            }

        }

        catch (Exception ex)
        {

        }
    }

    //protected void TextBox1_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //    }
    //    catch
    //    {
    //    }        

    //}

    protected void btn_exit_Click(object sender, EventArgs e)
    {

    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (Showgrid.Visible == true)
                {
                    
                    d2.printexcelreportgrid(Showgrid, reportname);
                }

                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "Consolidated Subject Wise Report";
            string pagename = "Commonsubjectwise.aspx";
            dptname = dptname + "@ " + "Subject : " + ddl_subject.SelectedItem.ToString();
            //if (FpSpread1.Visible == true)
            //{
            //    Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            //}
            string ss = Session["usercode"].ToString();
            string degreedetails = "";
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch
        {
        }
    }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}

    //protected void btnprintmaster1_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        StringWriter sw1 = new StringWriter();
    //        HtmlTextWriter hw1 = new HtmlTextWriter(sw1);
    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", "attachment;filename=Commonsubject.pdf");
    //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);
    //        Label lb = new Label();
    //        btn_go_Click(sender, e);
    //        string collegename = "";
    //        DataTable dt = (DataTable)FpSpread1.Sheets[0].DataSource;
    //       // DataTable dt = new DataTable();

    //       // DataTable dt = new DataTable();
    //        //for (int r = 0; r <FpSpread1.Sheets[0].RowCount; r++)
    //        //{
    //        //    dt.Rows.Add(FpSpread1.Sheets[0].Rows[r]);


    //        //} 
    //        GridView grd = new GridView();
    //        grd.DataSource = dt;
    //        grd.DataBind();

    //        if (grd.Rows.Count > 0)
    //        {
    //            grd.AllowPaging = false;
    //            grd.HeaderRow.Style.Add("width", "15%");
    //            grd.HeaderRow.Style.Add("font-size", "8px");
    //            grd.HeaderRow.Style.Add("text-align", "center");
    //            grd.Style.Add("font-family", "Bood Antiqua;");
    //            grd.Style.Add("font-size", "6px");
    //            grd.RenderControl(hw);
    //            grd.DataBind();
    //        }

    //        Label lb4 = new Label();
    //        if (ChartPassPercent.Visible == true)
    //        {

    //            lb4.Text = "<br>";
    //            lb4.Style.Add("height", "100px");
    //            lb4.Style.Add("text-decoration", "none");
    //            lb4.Style.Add("font-family", "Book Antiqua;");
    //            lb4.Style.Add("font-size", "8px");
    //            lb4.Style.Add("font-weight", "bold");
    //            lb4.Style.Add("text-align", "center");
    //            lb4.RenderControl(hw);
    //        }

    //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 5f, 0f);
    //        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
    //        pdfDoc.Open();
    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
    //        {
    //            string getpath = HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg").ToString();
    //            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(getpath);
    //            jpg.ScaleToFit(60f, 40f);
    //            jpg.Alignment = Element.ALIGN_LEFT;
    //            jpg.IndentationLeft = 9f;
    //            jpg.SpacingAfter = 9f;
    //            pdfDoc.Add(jpg);
    //        }

    //        StringReader sr = new StringReader(sw.ToString() + sw1.ToString() + sw1.ToString());
    //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);

    //        htmlparser.Parse(sr);
    //        if (ChartPassPercent.Visible == true)
    //        {
    //            using (MemoryStream stream = new MemoryStream())
    //            {
    //                ChartPassPercent.SaveImage(stream, ChartImageFormat.Png);
    //                iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
    //                chartImage.ScalePercent(75f);
    //                pdfDoc.Add(chartImage);
    //            }
    //        }
    //        if (S_GRADE.Visible == true)
    //        {
    //            using (MemoryStream stream = new MemoryStream())
    //            {
    //                S_GRADE.SaveImage(stream, ChartImageFormat.Png);
    //                iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
    //                chartImage.ScalePercent(75f);
    //                pdfDoc.Add(chartImage);
    //            }
    //        }

    //        //StringWriter swf = new StringWriter();
    //        //HtmlTextWriter hwf = new HtmlTextWriter(swf);
    //        //lb4.Text = "<br>HOD";
    //        //lb4.Style.Add("height", "100px");
    //        //lb4.Style.Add("text-decoration", "none");
    //        //lb4.Style.Add("font-family", "Book Antiqua;");
    //        //lb4.Style.Add("font-size", "8px");
    //        //lb4.Style.Add("font-weight", "bold");
    //        //lb4.Style.Add("text-align", "center");
    //        //lb4.RenderControl(hwf);


    //        pdfDoc.Close();
    //        Response.Write(pdfDoc);
    //        Response.End();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }



    public void btnPrint11()
    {
        string college_code = Convert.ToString(Session["collegecode"].ToString());
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
        spReportName.InnerHtml = "Consolidated Subject Wise Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
    
    public override void VerifyRenderingInServerForm(Control control)
    { }

}