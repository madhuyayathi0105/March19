using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;


public partial class attendancechart : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet dstotal = new DataSet();
    DataRow dr;
    string collegecode = "";
    string usercode = "";
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    ArrayList morlist = new ArrayList();
    ArrayList evelist = new ArrayList();
    ArrayList al = new ArrayList();
    ArrayList per = new ArrayList();
    Hashtable ht = new Hashtable();
    Hashtable hat = new Hashtable();
    decimal cound_hrs;
    decimal cound_hrseve;
    decimal cound_hrseve1 = 0;
    decimal present_hrs;
    decimal present_hrs1 = 0;
    decimal present_hrs3;
    decimal present_hrs2 = 0;
    decimal cound_hrs2 = 0;
    string present_values = "";
    string absent_values = "";
    string not_consider = "";
    Hashtable persent = new Hashtable();
    Hashtable conted = new Hashtable();
    Boolean g = false;
    Hashtable datelist = new Hashtable();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("ChartMaster"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/ChartMOD/ChartMaster.aspx");
                return;
            }
        }
        //****************************************************//

        collegecode = Session["collegecode"].ToString();
        usercode = Session["usercode"].ToString();
        if (!IsPostBack)
        {
            college();
            edulevel();
            txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtdate.Attributes.Add("ReadOnly", "ReadOnly");
            TextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy");
            TextBox1.Attributes.Add("ReadOnly", "ReadOnly");
            Chart1.Visible = false;

        }

    }
    
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
        Response.Redirect("~/Default.aspx");
    }
    
    protected void persentage()
    {

        try
        {
            present_calcflag.Clear();
            absent_calcflag.Clear();
            ht.Clear();
            string sqlquery2 = "select distinct LeaveCode,CalcFlag from AttMasterSetting where collegecode='" + collegecode + "'";
            ds3 = da.select_method_wo_parameter(sqlquery2, "text");
            int count_master = (ds3.Tables[0].Rows.Count);
            if (count_master > 0)
            {

                for (count_master = 0; count_master < ds3.Tables[0].Rows.Count; count_master++)
                {

                    if (ds3.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        present_calcflag.Add(ds3.Tables[0].Rows[count_master]["leavecode"].ToString(), ds3.Tables[0].Rows[count_master]["leavecode"].ToString());

                        if (present_values == "")
                        {
                            present_values = ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }
                        else
                        {
                            present_values = present_values + "," + ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }

                    }
                    if (ds3.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {
                        absent_calcflag.Add(ds3.Tables[0].Rows[count_master]["leavecode"].ToString(), ds3.Tables[0].Rows[count_master]["leavecode"].ToString());
                        if (absent_values == "")
                        {
                            absent_values = ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }
                        else
                        {
                            absent_values = absent_values + "," + ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }
                    }
                    if (ds3.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")
                    {
                        if (not_consider == "")
                        {
                            not_consider = ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }
                        else
                        {
                            not_consider = not_consider + "," + ds3.Tables[0].Rows[count_master]["leavecode"].ToString();
                        }
                    }

                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            Boolean sunday_holiday = false;
            lblerrmsg.Visible = false;
            lblerrmsg1.Visible = false;
            decimal conducted = 0;
            decimal presented = 0;
            string context;
            ds.Clear();
            ds1.Clear();
            ds2.Clear();
            ds3.Clear();
            per.Clear();
            string date1 = txtdate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            string date4 = split[2].ToString() + "-" + split[1].ToString() + "-" + split[0].ToString();
            string date = TextBox1.Text.ToString();
            string[] split2 = date.Split(new Char[] { '/' });
            string date3 = split2[2].ToString() + "-" + split2[1].ToString() + "-" + split2[0].ToString();
            DateTime datefrom = Convert.ToDateTime(date4.ToString());
            DateTime dateto = Convert.ToDateTime(date3.ToString());
            TimeSpan t = datefrom.Subtract(dateto);
            decimal samedegreetotal = 0;
            decimal samedegreetotal1 = 0;
            Hashtable calcforday = new Hashtable();
            long days = t.Days;
            if (days > 0)
            {
                lblerrmsg.Text = "From Date Should Be Lesser Than To Date";
                lblerrmsg.Visible = true;
                Chart1.Visible = false;

            }
            else
            {

                ds.Clear();
                persentage();
                if (ddledu.Text == "All")
                {
                    string sqlquery1 = "select distinct r.degree_code,(c.course_name+'-'+ dp.dept_acronym) as dept,r.degree_code from registration r,degree de,course c,department dp,deptprivilages dv,seminfo where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.college_code=" + ddlcollege.SelectedItem.Value + " And r.current_semester = seminfo.semester and r.degree_code=seminfo.degree_code and r.batch_year=seminfo.batch_year  and  r.degree_code=dv.degree_code and dv.Degree_code=de.Degree_code and  user_code=" + usercode + " group by r.degree_code,course_name,dept_acronym order by  dept ASC ";
                    ds3 = da.select_method_wo_parameter(sqlquery1, "text");
                }
                else
                {
                    string sqlquery2 = "select distinct r.degree_code,(c.course_name+'-'+ dp.dept_acronym) as dept,r.degree_code from registration r,degree de,course c,department dp,deptprivilages dv,seminfo where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.college_code=" + ddlcollege.SelectedItem.Value + " and Edu_Level='" + ddledu.Text + "'  And r.current_semester = seminfo.semester and r.degree_code=seminfo.degree_code and r.batch_year=seminfo.batch_year  and  r.degree_code=dv.degree_code and dv.Degree_code=de.Degree_code and  user_code=" + usercode + " group by r.degree_code,course_name,dept_acronym order by  dept ASC ";
                    ds3 = da.select_method_wo_parameter(sqlquery2, "text");
                }


                ds3.Tables[0].Columns.Add("persent", typeof(string));

                string sqlquery = "";

                if (ddledu.Text == "All")
                {
                    sqlquery = "select count(r.roll_no)as strength,(c.course_name+'-'+ dp.dept_acronym) as dept,r.current_semester,r.batch_year,r.degree_code from registration r,degree de,course c,department dp,deptprivilages dv,seminfo where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.college_code=" + ddlcollege.SelectedItem.Value + " And r.current_semester = seminfo.semester and r.degree_code=seminfo.degree_code and r.batch_year=seminfo.batch_year  and  r.degree_code=dv.degree_code and dv.Degree_code=de.Degree_code and  user_code=" + usercode + "  group by r.batch_year,r.degree_code,course_name,dept_acronym,current_semester order by  dept ASC ,current_semester ASC";
                    sqlquery = sqlquery + " " + "select distinct No_of_hrs_per_day as 'PER DAY', no_of_hrs_I_half_day as 'I_HALF_DAY' , no_of_hrs_II_half_day as 'II_HALF_DAY', min_pres_I_half_day as 'MIN PREE I DAY', min_pres_II_half_day as 'MIN PREE II DAY', schorder as 'SCH_ORDER', nodays as'NO_DAYS', percent_eligible_for_exam as 'Eligible_Percent',degree_code,semester from PeriodAttndSchedule";

                }
                else
                {
                    sqlquery = "select count(r.roll_no)as strength,(c.course_name+'-'+ dp.dept_acronym) as dept,r.current_semester,r.batch_year,r.degree_code from registration r,degree de,course c,department dp,deptprivilages dv,seminfo where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.college_code=" + ddlcollege.SelectedItem.Value + " And r.current_semester = seminfo.semester and r.degree_code=seminfo.degree_code and r.batch_year=seminfo.batch_year  and  r.degree_code=dv.degree_code and dv.Degree_code=de.Degree_code and  user_code=" + usercode + " and Edu_Level='" + ddledu.Text + "'  group by r.batch_year,r.degree_code,course_name,dept_acronym,current_semester order by  dept ASC ,current_semester ASC";
                    sqlquery = sqlquery + " " + "select distinct No_of_hrs_per_day as 'PER DAY', no_of_hrs_I_half_day as 'I_HALF_DAY' , no_of_hrs_II_half_day as 'II_HALF_DAY', min_pres_I_half_day as 'MIN PREE I DAY', min_pres_II_half_day as 'MIN PREE II DAY', schorder as 'SCH_ORDER', nodays as'NO_DAYS', percent_eligible_for_exam as 'Eligible_Percent',degree_code,semester from PeriodAttndSchedule";
                }
                ds = da.select_method_wo_parameter(sqlquery, "text");
                string errordate = "";

                if (radiolist1.SelectedItem.Value == "1")
                {
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        int count = 0;
                        int count1 = 0;
                        int degree = Convert.ToInt32(ds3.Tables[0].Rows[0]["degree_code"]);
                        List<DateTime> li = new List<DateTime>();
                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                        {
                            ArrayList al = new ArrayList();
                            errordate = "";
                            decimal persent_totaldegree = 0;
                            decimal conducted_totaldegree = 0;
                            decimal totalpresent_hrs = 0;
                            decimal counducted_hrs = 0;
                            li.Clear();
                            DateTime stdt = DateTime.ParseExact(txtdate.Text, "dd/MM/yyyy", null);
                            DateTime endt = DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);

                            while (stdt <= endt)
                            {

                                if (stdt.ToString("dddd") == "Sunday")
                                {
                                    sunday_holiday = true;

                                    if (errordate == "")
                                    {
                                        errordate = "" + stdt.ToString("dd-MM-yyyy");

                                    }
                                    else
                                    {
                                        errordate = errordate + "," + stdt.ToString("dd-MM-yyyy");
                                    }
                                    stdt = stdt.AddDays(1);



                                }
                                else
                                {
                                    li.Add(stdt);
                                    stdt = stdt.AddDays(1);
                                }
                            }
                            count1 = 0;
                            for (int g = 0; g < li.Count; g++)
                            {
                                if (li[g].ToString("dddd") == "Sunday")
                                {
                                    sunday_holiday = true;
                                }
                                else
                                {
                                    persent_totaldegree = 0;
                                    conducted_totaldegree = 0;
                                    DateTime date2 = li[g];
                                    string strDate = date2.ToString("dd/MM/yyyy");
                                    string[] name = strDate.Split(new char[] { '/' });
                                    int yr = (int.Parse(name[2])) * 12;
                                    int mn = yr + (int.Parse(name[1]));
                                    int dd = Convert.ToInt32(name[0].ToString());
                                    string sql2 = "select d" + dd + "d1,d" + dd + "d2,d" + dd + "d3,d" + dd + "d4,d" + dd + "d5,d" + dd + "d6,d" + dd + "d7,d" + dd + "d8,count(*)count,degree_code,Batch_Year,Current_Semester from registration r,attendance a where degree_code=" + ds3.Tables[0].Rows[i]["degree_code"].ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=a.roll_no and a.month_year=" + mn + " and college_code=" + ddlcollege.SelectedItem.Value + "  group by d" + dd + "d1,d" + dd + "d2,d" + dd + "d3,d" + dd + "d4,d" + dd + "d5,d" + dd + "d6,d" + dd + "d7,d" + dd + "d8 ,degree_code,Batch_Year,Current_Semester";
                                    ds2 = da.select_method_wo_parameter(sql2, "text");
                                    DataView dv3 = new DataView();

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        DataView dv = new DataView();
                                        ds.Tables[0].DefaultView.RowFilter = "degree_code=" + ds3.Tables[0].Rows[i]["degree_code"].ToString() + "";
                                        dv = ds.Tables[0].DefaultView;

                                        if (dv.Count > 0 && dv != null && dv.Table != null)
                                        {
                                            samedegreetotal1 = 0;
                                            count = 0;
                                            for (int jk = 0; jk < dv.Count; jk++)
                                            {
                                                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + datefrom.ToString() + "' and '" + dateto.ToString() + "' and degree_code=" + ds3.Tables[0].Rows[i]["degree_code"].ToString() + " and semester=" + dv[jk]["current_semester"].ToString() + "";
                                                int iscount = 0;
                                                DataSet dsholiday = new DataSet();
                                                dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "text");
                                                if (dsholiday.Tables[0].Rows.Count > 0)
                                                {
                                                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                                                }
                                                hat.Clear();
                                                hat.Add("degree_code", int.Parse(degree.ToString()));
                                                hat.Add("sem", int.Parse(dv[jk]["current_semester"].ToString()));
                                                hat.Add("from_date", datefrom.ToString("yyyy/MM/dd"));
                                                hat.Add("to_date", dateto.ToString("yyyy/MM/dd"));
                                                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                                                hat.Add("iscount", iscount);

                                                dsholiday = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
                                                ArrayList dsholi = new ArrayList();
                                                if (dsholiday.Tables[0].Rows.Count > 0 && dsholiday.Tables != null && dsholiday != null)
                                                {
                                                    for (int ho = 0; ho < dsholiday.Tables[0].Rows.Count; ho++)
                                                    {
                                                        dsholi.Add(dsholiday.Tables[0].Rows[ho][0].ToString());
                                                    }
                                                }
                                                if (!dsholi.Contains(li[g].ToString()))
                                                {

                                                    DataView dv1 = new DataView();
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        ds.Tables[1].DefaultView.RowFilter = "degree_code=" + ds3.Tables[0].Rows[i]["degree_code"].ToString() + " and semester=" + dv[jk]["current_semester"].ToString() + " ";
                                                        dv1 = ds.Tables[1].DefaultView;
                                                        ds2.Tables[0].DefaultView.RowFilter = " batch_year=" + dv[jk]["batch_year"].ToString() + " and current_semester=" + dv[jk]["current_semester"].ToString() + "";
                                                        dv3 = ds2.Tables[0].DefaultView;
                                                        if (dv3.Count > 0)
                                                        {
                                                            persent_totaldegree = 0;
                                                            conducted_totaldegree = 0;
                                                            for (int j = 0; j < dv3.Count; j++)
                                                            {

                                                                cound_hrs2 = 0;
                                                                cound_hrseve1 = 0;
                                                                present_hrs2 = 0;
                                                                present_hrs1 = 0;
                                                                int minmor = Convert.ToInt32(dv1[0][1].ToString());
                                                                int mineve = Convert.ToInt32(dv1[0][0].ToString());
                                                                int minmorattend = Convert.ToInt32(dv1[0][3].ToString());
                                                                int mineveattend = Convert.ToInt32(dv1[0][4].ToString());
                                                                morlist.Clear();
                                                                evelist.Clear();
                                                                for (int k = 0; k <= dv3.Table.Columns.Count - 1; k++)
                                                                {
                                                                    string r = dv3[j][k].ToString();

                                                                    if (k < minmor)
                                                                    {
                                                                        morlist.Add(r);
                                                                    }
                                                                    else if (k < mineve)
                                                                    {
                                                                        evelist.Add(r);

                                                                    }
                                                                    else
                                                                    {

                                                                    }

                                                                }
                                                                int totalmon = Convert.ToInt32(dv3[j]["count"].ToString());
                                                                if (!morlist.Contains("0") && !morlist.Contains(""))
                                                                {
                                                                    cound_hrs = Convert.ToDecimal(totalmon * 0.5);
                                                                    cound_hrs2 = cound_hrs2 + cound_hrs;
                                                                }
                                                                else
                                                                {
                                                                    morlist.Clear();
                                                                }
                                                                if (!evelist.Contains("0") && !evelist.Contains(""))
                                                                {
                                                                    cound_hrseve = Convert.ToDecimal(totalmon * 0.5);
                                                                    cound_hrseve1 = cound_hrseve1 + cound_hrseve;

                                                                }
                                                                else
                                                                {
                                                                    evelist.Clear();
                                                                }

                                                                al.Clear();
                                                                for (int a = 0; a < morlist.Count; a++)
                                                                {
                                                                    string morper = morlist[a].ToString();
                                                                    string mo = present_calcflag.Contains(morper).ToString();
                                                                    if (mo.ToString() == "True")
                                                                    {
                                                                        al.Add(mo);
                                                                        if (al.Count == Convert.ToInt32(minmorattend))
                                                                        {
                                                                            present_hrs = Convert.ToDecimal(totalmon * 0.5);
                                                                            present_hrs1 = present_hrs1 + present_hrs;
                                                                        }
                                                                    }
                                                                }
                                                                al.Clear();
                                                                for (int a = 0; a < evelist.Count; a++)
                                                                {
                                                                    string eveper = evelist[a].ToString();
                                                                    string mo = present_calcflag.Contains(eveper).ToString();
                                                                    if (mo.ToString() == "True")
                                                                    {
                                                                        al.Add(mo);
                                                                        if (al.Count == Convert.ToInt32(mineveattend))
                                                                        {
                                                                            present_hrs3 = Convert.ToDecimal(totalmon * 0.5);
                                                                            present_hrs2 = present_hrs2 + present_hrs3;
                                                                        }
                                                                    }
                                                                }
                                                                al.Clear();
                                                                counducted_hrs = cound_hrs2 + cound_hrseve1;
                                                                totalpresent_hrs = present_hrs1 + present_hrs2;
                                                                conducted_totaldegree = conducted_totaldegree + counducted_hrs;
                                                                persent_totaldegree = persent_totaldegree + totalpresent_hrs;

                                                            }
                                                            if (conducted_totaldegree != 0)
                                                            {
                                                                samedegreetotal = (persent_totaldegree / conducted_totaldegree) * 100;
                                                                if (samedegreetotal != 0)
                                                                {
                                                                    samedegreetotal1 = (samedegreetotal1 + samedegreetotal);
                                                                    count++;
                                                                }
                                                            }



                                                        }


                                                    }
                                                }
                                                else
                                                {
                                                    DateTime df = li[g];
                                                    li.Remove(df);
                                                }
                                            }
                                            if (count != 0)
                                            {
                                                samedegreetotal1 = samedegreetotal1 / count;

                                            }
                                        }
                                    }

                                }

                                string data = ds3.Tables[0].Rows[i][2].ToString();
                                if (persent.ContainsKey(data))
                                {
                                    if (samedegreetotal1.ToString() != "0")
                                    {

                                        conducted = conducted + samedegreetotal1;
                                        // presented = presented + conducted;
                                        persent[data] = conducted;

                                        count1++;
                                        //  conted[data] = conducted;
                                    }
                                    else
                                    {
                                        conducted = conducted + samedegreetotal1;
                                        //   presented = presented + samedegreetotal1;
                                        persent[data] = conducted;
                                        //  conted[data] = conducted;

                                    }

                                }
                                else
                                {
                                    persent.Add(ds3.Tables[0].Rows[i][2].ToString(), samedegreetotal1);
                                    if (conducted.ToString() == "0")
                                    {
                                        if (samedegreetotal1.ToString() != "0")
                                        {
                                            conducted = (conducted + samedegreetotal1);

                                            //  presented = presented + conducted;
                                            persent[data] = conducted;
                                            count1++;
                                        }

                                    }
                                    else
                                    {
                                        conducted = 0;
                                        presented = 0;
                                    }

                                }

                            }
                            string data1 = ds3.Tables[0].Rows[i][2].ToString();
                            if (persent.ContainsKey(data1))
                            {
                                presented = Convert.ToDecimal(persent[data1]);
                                if (presented != 0 && count1 != 0)
                                {
                                    presented = (presented / count1);
                                }
                            }
                            datelist.Add(data1, presented);

                        }
                        for (int w = 0; w < ds3.Tables[0].Rows.Count; w++)
                        {
                            string data1 = ds3.Tables[0].Rows[w]["degree_code"].ToString();
                            if (datelist.ContainsKey(data1))
                            {
                                double pre_per = Convert.ToDouble(datelist[data1]);
                                if (pre_per.ToString() != "NaN" && pre_per.ToString() != "0")
                                {
                                    ds3.Tables[0].Rows[w]["persent"] = Math.Round(pre_per, 2);
                                    g = true;
                                }
                            }

                        }

                    }
                    if (g == true)
                    {

                        Chart1.Series["Series1"]["RadarDrawingStyle"] = "marker";
                        Chart1.Series["Series1"]["CircularLabelsStyle"] = "Horizontal";
                        Chart1.Series["Series1"].BorderWidth = 3;
                        Chart1.Series["Series1"].MarkerSize = 7;
                        Chart1.Series["Series1"].MarkerStyle = MarkerStyle.Star4;
                        Chart1.Series["Series1"].MarkerColor = Color.DarkBlue;
                        Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                        Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100;
                        Chart1.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart1.ForeColor;
                        Chart1.Series["Series1"].IsValueShownAsLabel = true;
                        Chart1.Series["Series1"]["RadarDrawingStyle"] = "Line";
                        Chart1.Series["Series1"].Color = Color.DarkViolet;
                        Chart1.Series["Series1"].XValueMember = "dept";
                        Chart1.Series["Series1"].YValueMembers = "persent";
                        Chart1.Series["Series1"].Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                        Chart1.Series["Series1"].Name = "Attendance Percentage";
                        Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
                        Chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                        Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 9);
                        Chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Black;
                        Chart1.ChartAreas["ChartArea1"].AxisY.LineColor = Color.DeepPink;
                        Chart1.ChartAreas["ChartArea1"].AxisY.LineWidth = 2;
                        Chart1.Legends[0].Font = new Font("Book Antiqua", 9, FontStyle.Bold);
                        Title datitle = Chart1.Titles.Add("Attendance Day Wise Chart");
                        datitle.Font = new Font("Book Antiqua", 14, FontStyle.Bold);
                        datitle.ForeColor = Color.DarkViolet;
                        Chart1.Legends[0].Position.X = 90;
                        Chart1.Legends[0].Position.Y = 10;
                        Chart1.Legends[0].Position.Width = 20;
                        Chart1.Legends[0].Position.Height = 10;
                        Chart1.Height = 500;
                        Chart1.DataSource = ds3;
                        Chart1.DataBind();
                    }
                    else
                    {

                        lblerrmsg1.Visible = true;
                        lblerrmsg1.Text = "No Records Found";
                        Chart1.Visible = false;

                    }
                    if (sunday_holiday == true)
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "" + errordate + " Day is Sunday";

                    }




                }

                else if (radiolist1.SelectedItem.Value == "2")
                {
                    Chart1.Visible = false;
                    string sqlquery1 = "";
                    Boolean falg_check = false;
                    if (ddledu.Text == "All")
                    {
                        sqlquery1 = "select Degree_Code,Course_Name from course c,Degree d where c.Course_Id=d.Course_Id and c.college_code=d.college_code  and d.college_code=" + ddlcollege.SelectedItem.Value + "";
                        sqlquery1 = sqlquery1 + "" + "select distinct No_of_hrs_per_day as 'PER DAY', no_of_hrs_I_half_day as 'I_HALF_DAY' , no_of_hrs_II_half_day as 'II_HALF_DAY', min_pres_I_half_day as 'MIN PREE I DAY', min_pres_II_half_day as 'MIN PREE II DAY', schorder as 'SCH_ORDER', nodays as'NO_DAYS', percent_eligible_for_exam as 'Eligible_Percent',degree_code,semester from PeriodAttndSchedule";

                    }
                    else
                    {
                        sqlquery1 = "select Degree_Code,Course_Name from course c,Degree d where  c.Course_Id=d.Course_Id and c.college_code=d.college_code   and Edu_Level='" + ddledu.Text + "' and d.college_code=" + ddlcollege.SelectedItem.Value + "  ";
                        sqlquery1 = sqlquery1 + "" + "select distinct No_of_hrs_per_day as 'PER DAY', no_of_hrs_I_half_day as 'I_HALF_DAY' , no_of_hrs_II_half_day as 'II_HALF_DAY', min_pres_I_half_day as 'MIN PREE I DAY', min_pres_II_half_day as 'MIN PREE II DAY', schorder as 'SCH_ORDER', nodays as'NO_DAYS', percent_eligible_for_exam as 'Eligible_Percent',degree_code,semester from PeriodAttndSchedule";
                    }
                    ds4 = da.select_method_wo_parameter(sqlquery1, "text");
                    List<DateTime> li = new List<DateTime>();
                    DateTime stdt = DateTime.ParseExact(txtdate.Text, "dd/MM/yyyy", null);
                    DateTime endt = DateTime.ParseExact(TextBox1.Text, "dd/MM/yyyy", null);
                    while (stdt <= endt)
                    {
                        if (stdt.ToString("dddd") == "Sunday")
                        {
                            sunday_holiday = true;

                            if (errordate == "")
                            {
                                errordate = "" + stdt.ToString("dd-MM-yyyy");

                            }
                            else
                            {
                                errordate = errordate + "," + stdt.ToString("dd-MM-yyyy");
                            }
                            stdt = stdt.AddDays(1);



                        }
                        else
                        {
                            li.Add(stdt);
                            stdt = stdt.AddDays(1);
                        }
                    }
                    if (ds4.Tables[0].Rows.Count > 0)
                    {
                        Boolean flag_check = false;
                        for (int l = 0; l < ds4.Tables[0].Rows.Count; l++)
                        {

                            falg_check = false;
                            DataView dv = new DataView();
                            if (ds.Tables[0].Rows.Count > 0 && ds.Tables != null && ds != null)
                            {
                                ds.Tables[0].DefaultView.RowFilter = "degree_code='" + ds4.Tables[0].Rows[l]["degree_code"].ToString() + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (li.Count > 0)
                                {
                                    double total1 = 0.0;
                                    double total2 = 0.0;
                                    double total3 = 0.0;
                                    double total4 = 0.0;
                                    double total5 = 0.0;
                                    double total6 = 0.0;
                                    double total7 = 0.0;
                                    double total8 = 0.0;
                                    double total12 = 0;
                                    double total22 = 0;
                                    double total32 = 0;
                                    double total42 = 0;
                                    double total52 = 0;
                                    double total62 = 0;
                                    double total72 = 0;
                                    double total82 = 0;
                                    string d1 = ""; string d2 = ""; string d3 = ""; string d4 = ""; string d5 = ""; string d6 = ""; string d7 = ""; string d8 = "";
                                    double cond1 = 0; double cond2 = 0; double cond3 = 0; double cond4 = 0; double cond5 = 0; double cond6 = 0; double cond7 = 0; double cond8 = 0;
                                    flag_check = false;
                                    int count = 0;
                                    int count1 = 0;
                                    int count2 = 0;
                                    int count3 = 0;
                                    int count4 = 0;
                                    int count5 = 0;
                                    int count6 = 0;
                                    int count7 = 0;
                                    int count8 = 0;
                                    for (int i = 0; i < li.Count; i++)
                                    {
                                        d1 = ""; d2 = ""; d3 = ""; d4 = ""; d5 = ""; d6 = ""; d7 = ""; d8 = "";
                                        cond1 = 0; cond2 = 0; cond3 = 0; cond4 = 0; cond5 = 0; cond6 = 0; cond7 = 0; cond8 = 0;
                                        string[] name = li[i].ToString("d/MM/yyyy").Split(new char[] { '/' });
                                        //calcuation month_year//
                                        int month_year = (Convert.ToInt32(name[2].ToString()) * 12) + Convert.ToInt32(name[1].ToString());
                                        if (not_consider == "")
                                        {
                                            not_consider = "0";
                                        }
                                        for (int j = 0; j < dv.Count; j++)
                                        {
                                            d1 = ""; d2 = ""; d3 = ""; d4 = ""; d5 = ""; d6 = ""; d7 = ""; d8 = "";
                                            cond1 = 0; cond2 = 0; cond3 = 0; cond4 = 0; cond5 = 0; cond6 = 0; cond7 = 0; cond8 = 0;
                                            total1 = 0; total2 = 0; total3 = 0; total4 = 0; total5 = 0;
                                            total6 = 0;
                                            total7 = 0;
                                            total8 = 0;
                                            DataView dv1 = new DataView();
                                            ds4.Tables[1].DefaultView.RowFilter = "degree_code='" + dv[j]["degree_code"].ToString() + "' and semester ='" + dv[j]["current_semester"].ToString() + "'";
                                            dv1 = ds4.Tables[1].DefaultView;
                                            sqlquery = "select sum(case when  d" + name[0] + "d1 in (" + present_values + ") then 1 else 0 end )d1,sum(case when  d" + name[0] + "d2 in (" + present_values + ") then 1 else 0 end)d2,sum(case when  d" + name[0] + "d3 in (" + present_values + ") then 1 else 0 end)d3,sum(case when  d" + name[0] + "d4 in (" + present_values + ") then 1 else 0 end)d4,sum(case when  d" + name[0] + "d5 in (" + present_values + ") then 1 else 0 end)d5,sum(case when  d" + name[0] + "d6 in (" + present_values + ") then 1 else 0 end )d6,sum(case when  d" + name[0] + "d7 in (" + present_values + ") then 1 else 0 end )d7,sum(case when  d" + name[0] + "d8 in (" + present_values + ") then 1 else 0 end )d8 from attendance a,Registration r where r.Roll_No=a.roll_no and degree_code='" + dv[j]["degree_code"].ToString() + "' and Batch_Year=" + dv[j]["batch_year"].ToString() + " and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + "";
                                            sqlquery = sqlquery + "select  distinct(select COUNT(d" + name[0] + "d1) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d1!=0 and d" + name[0] + "d1 not in(" + not_consider + "))) cd1,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d2) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d2!=0 and d" + name[0] + "d2 not in(" + not_consider + ")))cd2,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d3) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d3!=0 and d" + name[0] + "d3 not in(" + not_consider + ")))cd3,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d4) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d4!=0 and d" + name[0] + "d4 not in(" + not_consider + ")))cd4,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d5) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d5!=0 and d" + name[0] + "d5 not in(" + not_consider + ")))cd5,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d6) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d6!=0 and d" + name[0] + "d6 not in(" + not_consider + ")))cd6,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d7) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d7!=0 and d" + name[0] + "d7 not in(" + not_consider + ")))cd7,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d8) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d8!=0 and d" + name[0] + "d8 not in(" + not_consider + ")))cd8,";
                                            sqlquery = sqlquery + "(select COUNT(d" + name[0] + "d9) from attendance a,Registration r where r.Roll_No=a.roll_no and Batch_Year=" + dv[j]["batch_year"].ToString() + " and degree_code='" + dv[j]["degree_code"].ToString() + "' and Current_Semester=" + dv[j]["current_semester"].ToString() + " and month_year=" + month_year.ToString() + " and college_code=" + ddlcollege.SelectedItem.Value + " and (d" + name[0] + "d9!=0 and d" + name[0] + "d9 not in(" + not_consider + ")))cd9 from attendance";
                                            dstotal = da.select_method_wo_parameter(sqlquery, "text");
                                            //present count//
                                            string s1 = dstotal.Tables[0].Rows[0]["d1"].ToString();
                                            string s2 = dstotal.Tables[0].Rows[0]["d2"].ToString();
                                            string s3 = dstotal.Tables[0].Rows[0]["d3"].ToString();
                                            string s4 = dstotal.Tables[0].Rows[0]["d4"].ToString();
                                            string s5 = dstotal.Tables[0].Rows[0]["d5"].ToString();
                                            string s6 = dstotal.Tables[0].Rows[0]["d6"].ToString();
                                            string s7 = dstotal.Tables[0].Rows[0]["d7"].ToString();
                                            string s8 = dstotal.Tables[0].Rows[0]["d8"].ToString();
                                            if (d1.ToString() == "" || d2.ToString() == "" || d3.ToString() == "" || d4.ToString() == "" || d5.ToString() == "" || d6.ToString() == "" || d7.ToString() == "" || d8.ToString() == "")
                                            {
                                                d1 = dstotal.Tables[0].Rows[0]["d1"].ToString();
                                                d2 = dstotal.Tables[0].Rows[0]["d2"].ToString();
                                                d3 = dstotal.Tables[0].Rows[0]["d3"].ToString();
                                                d4 = dstotal.Tables[0].Rows[0]["d4"].ToString();
                                                d5 = dstotal.Tables[0].Rows[0]["d5"].ToString();
                                                d6 = dstotal.Tables[0].Rows[0]["d6"].ToString();
                                                d7 = dstotal.Tables[0].Rows[0]["d7"].ToString();
                                                d8 = dstotal.Tables[0].Rows[0]["d8"].ToString();
                                            }
                                            else
                                            {
                                                if (s1.ToString() == "" || s2.ToString() == "" || s3.ToString() == "" || s4.ToString() == "" || s5.ToString() == "" || s6.ToString() == "" || s7.ToString() == "" || s8.ToString() == "")
                                                {
                                                    s1 = "0"; s2 = "0"; s3 = "0"; s4 = "0"; s5 = "0"; s6 = "0"; s7 = "0"; s8 = "0";
                                                    d1 = (Convert.ToDouble(d1) + Convert.ToDouble(s1)).ToString();
                                                    d2 = (Convert.ToDouble(d2) + Convert.ToDouble(s2)).ToString();
                                                    d3 = (Convert.ToDouble(d3) + Convert.ToDouble(s3)).ToString();
                                                    d4 = (Convert.ToDouble(d4) + Convert.ToDouble(s4)).ToString();
                                                    d5 = (Convert.ToDouble(d5) + Convert.ToDouble(s5)).ToString();
                                                    d6 = (Convert.ToDouble(d6) + Convert.ToDouble(s6)).ToString();
                                                    d7 = (Convert.ToDouble(d7) + Convert.ToDouble(s7)).ToString();
                                                    d8 = (Convert.ToDouble(d8) + Convert.ToDouble(s8)).ToString();
                                                }
                                                else
                                                {
                                                    d1 = (Convert.ToDouble(d1) + Convert.ToDouble(s1)).ToString();
                                                    d2 = (Convert.ToDouble(d2) + Convert.ToDouble(s2)).ToString();
                                                    d3 = (Convert.ToDouble(d3) + Convert.ToDouble(s3)).ToString();
                                                    d4 = (Convert.ToDouble(d4) + Convert.ToDouble(s4)).ToString();
                                                    d5 = (Convert.ToDouble(d5) + Convert.ToDouble(s5)).ToString();
                                                    d6 = (Convert.ToDouble(d6) + Convert.ToDouble(s6)).ToString();
                                                    d7 = (Convert.ToDouble(d7) + Convert.ToDouble(s7)).ToString();
                                                    d8 = (Convert.ToDouble(d8) + Convert.ToDouble(s8)).ToString();
                                                }
                                            }
                                            //conducted count//
                                            if (cond1.ToString() == "0")
                                            {
                                                cond1 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd1"].ToString());
                                            }
                                            else
                                            {
                                                cond1 = cond1 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd1"].ToString());

                                            }
                                            if (cond2.ToString() == "0")
                                            {
                                                cond2 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd2"].ToString());
                                            }
                                            else
                                            {
                                                cond2 = cond2 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd2"].ToString());

                                            }
                                            if (cond1.ToString() == "0")
                                            {
                                                cond3 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd3"].ToString());
                                            }
                                            else
                                            {
                                                cond3 = cond3 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd3"].ToString());
                                            }
                                            if (cond1.ToString() == "0")
                                            {
                                                cond4 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd4"].ToString());
                                            }
                                            else
                                            {
                                                cond4 = cond4 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd4"].ToString());
                                            }
                                            if (cond5.ToString() == "0")
                                            {
                                                cond5 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd5"].ToString());
                                            }
                                            else
                                            {
                                                cond5 = cond5 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd5"].ToString());

                                            }
                                            if (cond1.ToString() == "0")
                                            {
                                                cond6 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd6"].ToString());
                                            }
                                            else
                                            {
                                                cond6 = cond6 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd6"].ToString());
                                            }
                                            if (cond1.ToString() == "0")
                                            {
                                                cond7 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd7"].ToString());
                                            }
                                            else
                                            {
                                                cond7 = cond7 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd7"].ToString());
                                            }
                                            if (cond1.ToString() == "0")
                                            {
                                                cond8 = Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd8"].ToString());
                                            }
                                            else
                                            {
                                                cond8 = cond8 + Convert.ToDouble(dstotal.Tables[1].Rows[0]["cd8"].ToString());

                                            }
                                            if (d1.ToString() != "" || d2.ToString() != "" || d3.ToString() != "" || d4.ToString() != "" || d5.ToString() != "" || d6.ToString() != "" || d7.ToString() != "" || d8.ToString() != "")
                                            {

                                                string total = "";
                                                total1 = Math.Round((Convert.ToDouble(d1) / cond1) * 100, 1);
                                                total2 = Math.Round((Convert.ToDouble(d2) / cond2) * 100, 1);
                                                total3 = Math.Round((Convert.ToDouble(d3) / cond3) * 100, 1);
                                                total4 = Math.Round((Convert.ToDouble(d4) / cond4) * 100, 1);
                                                total5 = Math.Round((Convert.ToDouble(d5) / cond5) * 100, 1);
                                                total6 = Math.Round((Convert.ToDouble(d6) / cond6) * 100, 1);
                                                total7 = Math.Round((Convert.ToDouble(d7) / cond7) * 100, 1);
                                                total8 = Math.Round((Convert.ToDouble(d8) / cond8) * 100, 1);



                                            }
                                            if (total1.ToString() != "NaN")
                                            {
                                                if (total1 != 0)
                                                {
                                                    count1++;

                                                }
                                                total12 = Math.Round((total12 + total1), 1);
                                            }
                                            if (total2.ToString() != "NaN")
                                            {
                                                if (total2 != 0)
                                                {
                                                    count2++;

                                                }
                                                total22 = Math.Round((total22 + total2), 1);
                                            }
                                            if (total3.ToString() != "NaN")
                                            {
                                                if (total3 != 0)
                                                {
                                                    count3++;

                                                }
                                                total32 = Math.Round((total32 + total3), 1);
                                            }
                                            if (total4.ToString() != "NaN")
                                            {
                                                if (total4 != 0)
                                                {
                                                    count4++;

                                                }
                                                total42 = Math.Round((total42 + total4), 1);
                                            }
                                            if (total5.ToString() != "NaN")
                                            {
                                                if (total5 != 0)
                                                {
                                                    count5++;

                                                }
                                                total52 = Math.Round((total52 + total5), 1);
                                            }
                                            if (total6.ToString() != "NaN")
                                            {
                                                if (total6 != 0)
                                                {
                                                    count6++;

                                                }
                                                total62 = Math.Round((total62 + total6), 1);
                                            }
                                            if (total7.ToString() != "NaN")
                                            {
                                                if (total7 != 0)
                                                {
                                                    count7++;

                                                }
                                                total72 = Math.Round((total72 + total7), 1);
                                            } if (total8.ToString() != "NaN")
                                            {
                                                if (total8 != 0)
                                                {
                                                    count8++;

                                                }
                                                total82 = Math.Round((total82 + total8), 1);
                                            }

                                        }



                                    }
                                    if (count1 != 0)
                                    {
                                        total12 = Math.Round((total12 / count1), 1);
                                        falg_check = true;
                                    }
                                    if (count2 != 0)
                                    {
                                        total22 = Math.Round((total22 / count2), 1);
                                        falg_check = true;
                                    }
                                    if (count3 != 0)
                                    {
                                        total32 = Math.Round((total32 / count3), 1);
                                        falg_check = true;
                                    }
                                    if (count4 != 0)
                                    {
                                        total42 = Math.Round((total42 / count4), 1);
                                        falg_check = true;

                                    }
                                    if (count5 != 0)
                                    {
                                        total52 = Math.Round((total52 / count5), 1);
                                        falg_check = true;
                                    }
                                    if (count6 != 0)
                                    {
                                        total62 = Math.Round((total62 / count6), 1);
                                        falg_check = true;

                                    }
                                    if (count7 != 0)
                                    {
                                        total72 = Math.Round((total72 / count7), 1);
                                        falg_check = true;
                                    }
                                    if (count8 != 0)
                                    {
                                        total82 = Math.Round((total82 / count8), 1);
                                        falg_check = true;
                                    }
                                    if (falg_check == true)
                                    {
                                        if (total12.ToString() != "NaN" || total22.ToString() != "NaN" || total32.ToString() != "NaN" || total42.ToString() != "NaN" || total52.ToString() != "NaN" || total62.ToString() != "NaN" || total72.ToString() != "NaN" || total82.ToString() != "NaN")
                                        {
                                            DataSet chart_dataset = new DataSet();
                                            chart_dataset.Tables.Add("0");
                                            chart_dataset.Tables[0].Columns.Add("Hour", typeof(string));
                                            chart_dataset.Tables[0].Columns.Add("percentage", typeof(string));
                                            int k = 8;
                                            chart_dataset.Tables[0].Rows.Add(1, total12);
                                            chart_dataset.Tables[0].Rows.Add(2, total22);
                                            chart_dataset.Tables[0].Rows.Add(3, total32);
                                            chart_dataset.Tables[0].Rows.Add(4, total42);
                                            chart_dataset.Tables[0].Rows.Add(5, total52);
                                            chart_dataset.Tables[0].Rows.Add(6, total62);
                                            chart_dataset.Tables[0].Rows.Add(7, total72);
                                            chart_dataset.Tables[0].Rows.Add(8, total82);
                                            Chart Chart2 = new Chart();
                                            Chart2.ChartAreas.Add("ChartArea1");
                                            Chart2.Series.Add("Series1");
                                            Chart2.ChartAreas["ChartArea1"].AxisY.Interval = 25;
                                            Chart2.ChartAreas["ChartArea1"].AxisY.Maximum = 110;
                                            //  Chart2.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Chart2.ForeColor;
                                            Chart2.Series["Series1"].IsValueShownAsLabel = true;
                                            Chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                                            Chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                                            Chart2.Series["Series1"].Color = Color.BlueViolet;
                                            Chart2.ChartAreas["ChartArea1"].AxisX.Title = "No.of.Hours";
                                            Chart2.ChartAreas["ChartArea1"].AxisY.Title = "Attendance Percentage";
                                            Chart2.Series["Series1"].XValueMember = "hour";
                                            Chart2.Series["Series1"].YValueMembers = "percentage";
                                            Chart2.Series["Series1"].Font = new System.Drawing.Font("Book Antiqua", 8, FontStyle.Bold);
                                            Chart2.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                                            Chart2.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                                            Chart2.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                                            Chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                                            Chart2.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 8, FontStyle.Bold);
                                            Chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                                            Title radarchart = Chart2.Titles.Add("" + dv[0]["Dept"].ToString() + "(HourWise Attendance)");
                                            radarchart.Font = new Font("Book Antiqua", 10, FontStyle.Bold);
                                            Chart2.DataSource = chart_dataset;
                                            Chart2.DataBind();
                                            panelchart.Controls.Add(Chart2);
                                        }
                                        else
                                        {
                                            Table b = new Table();
                                            TableCell tc4 = new TableCell();
                                            TableRow tr4 = new TableRow();
                                            Label lblerr = new Label();
                                            lblerr.Text = "No Records Found for " + dv[0]["Dept"].ToString() + "";
                                            lblerr.Font.Size = FontUnit.Medium;
                                            lblerr.Font.Name = "Book Antiqua";
                                            lblerr.ForeColor = Color.Red;
                                            lblerr.Font.Bold = true;
                                            //lblerr.Font.Name 
                                            lblerr.Visible = true;
                                            panelerrormesg.Controls.Add(b);
                                            tr4.Cells.Add(tc4);
                                            b.Rows.Add(tr4);
                                            tc4.Controls.Add(lblerr);
                                        }

                                    }
                                    else
                                    {
                                        Table b = new Table();
                                        TableCell tc4 = new TableCell();
                                        TableRow tr4 = new TableRow();
                                        Label lblerr = new Label();
                                        lblerr.Text = "No Records Found for " + dv[0]["Dept"].ToString() + "";
                                        lblerr.Font.Size = FontUnit.Medium;
                                        lblerr.Font.Name = "Book Antiqua";
                                        lblerr.ForeColor = Color.Red;
                                        lblerr.Font.Bold = true;
                                        //lblerr.Font.Name 
                                        lblerr.Visible = true;
                                        panelerrormesg.Controls.Add(b);
                                        tr4.Cells.Add(tc4);
                                        b.Rows.Add(tr4);
                                        tc4.Controls.Add(lblerr);
                                    }

                                }
                                if (sunday_holiday == true)
                                {
                                    lblerrmsg.Visible = true;
                                    lblerrmsg.Text = "" + errordate + " Day is Sunday";

                                }

                            }
                            else
                            {
                                lblerrmsg.Visible = true;
                                lblerrmsg.Text = "No Records Found";
                            }


                        }
                    }
                    else
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "No Degree and Department In This College";
                    }


                }
            }



        }

        catch (Exception ex)
        {
        }



    }
    
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DateTime dtnow = DateTime.Now;
            lblerrmsg.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = TextBox1.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {
                    Chart1.Visible = false;
                    lblerrmsg.Text = "Date Can't Be Greater Than To Date";
                    lblerrmsg.Visible = true;
                    TextBox1.Text = DateTime.Now.ToString("dd/MM/yyy");

                }
                else
                {
                    lblerrmsg.Visible = false;
                    Chart1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
        }

    }
    
    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dtnow = DateTime.Now;
            lblerrmsg.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = txtdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {
                    Chart1.Visible = false;
                    lblerrmsg.Text = "From Date Can't Be Greater Than To Date";
                    lblerrmsg.Visible = true;
                    txtdate.Text = DateTime.Now.ToString("dd/MM/yyy");

                }
                else
                {
                    lblerrmsg.Visible = false;
                    Chart1.Visible = false;
                }
            }

        }
        catch (Exception ex)
        {
        }
    }
    
    protected void college()
    {
        try
        {
            ddlcollege.Items.Insert(0, "All");
            ds = da.select_method_wo_parameter("select collname,college_code,acr from collinfo", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        edulevel();
        Chart1.Visible = false;
    }
    
    public void edulevel()
    {

        ds = da.select_method_wo_parameter("select distinct Edu_Level from course where college_code=" + ddlcollege.SelectedItem.Value + "", "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledu.DataSource = ds;
            ddledu.DataTextField = "Edu_Level";
            ddledu.DataValueField = "Edu_Level";
            ddledu.DataBind();
        }
        ddledu.Items.Insert(0, "All");
    }
}