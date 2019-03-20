using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class LibraryPatch : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DataSet data = new DataSet();
    DataSet dataLoad = new DataSet();
    DataSet dsStudent = new DataSet();
    DataSet dsLibrary = new DataSet();
    DataSet dsAtt = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void BtnPatch_OnClick(object sender, EventArgs e)
    {
        try
        {
            string Sql = "";
            string fdate = "07/01/2017";
            string tdate = "06/30/2018";
            string sql = "";
            int insert = 0;
            double count = 0;
            double totcount = 0;
            double stuCount = 0;

            //=====================E-Access entry=======================//
            Sql = "select lib_queryhit.lib_date as 'Entry Date',sum(lib_count) as 'No of Hits'from lib_queryhit where lib_date between '2017-07-01' and '2018-06-30' group by lib_date order by lib_date";
            ds = d2.select_method_wo_parameter(Sql, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string date = Convert.ToString(ds.Tables[0].Rows[i]["Entry Date"]);
                    count = Convert.ToDouble(ds.Tables[0].Rows[i]["No of Hits"]);
                    if (count < 60)//97
                    {
                        totcount = 60 - count;
                        sql = "Insert into lib_queryhit(lib_date,lib_count,is_staff,department) values ('" + date + "','" + totcount + "',0,'All')";
                        insert = d2.update_method_wo_parameter(sql, "text");
                    }
                }
            }

            //==============================================================//

            //============Student Entry====================================//
            totcount = 0;
            stuCount = 0;
            for (DateTime k = Convert.ToDateTime(fdate); k <= Convert.ToDateTime(tdate); )
            {
                string dateval = Convert.ToString(k);
                Sql = "SELECT * FROM Holiday_Library WHERE Holiday_Date ='" + dateval + "' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count == 0)
                {
                    string sel = "select entry_date as 'Entry Date',count(*) as 'Hit Status' from libusers where usercat='Student' and entry_date='" + dateval + "'  group by entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToDouble(ds.Tables[0].Rows[0]["Hit Status"]);
                        if (count <= 230)//298
                        {
                            totcount = 230 - count;
                            sql = " select * from Registration where Batch_Year in('2017','2016','2015') and cc=0 and Exam_Flag<>'debar' and DelFlag=0 ";
                            data.Clear();
                            data = d2.select_method_wo_parameter(sql, "text");
                            if (data.Tables[0].Rows.Count > 0)
                            {
                                for (int ij = 0; ij < data.Tables[0].Rows.Count; ij++)
                                {
                                    if (stuCount <= totcount)
                                    {
                                        string intStudDegCode = Convert.ToString(data.Tables[0].Rows[ij]["degree_code"]);
                                        string intCurSem = Convert.ToString(data.Tables[0].Rows[ij]["Current_Semester"]);
                                        Sql = "SELECT * FROM HolidayStudents WHERE Degree_Code=" + intStudDegCode + " AND Semester =" + intCurSem + " AND HalfOrFull=0 AND Holiday_Date ='" + dateval + "' ";
                                        dataLoad = d2.select_method_wo_parameter(Sql, "text");
                                        if (dataLoad.Tables[0].Rows.Count == 0)
                                        {
                                            string rollno = Convert.ToString(data.Tables[0].Rows[ij]["Roll_No"]);
                                            Sql = "SELECT R.Roll_No,App_No,R.Stud_Name,Course_Name+' - '+Dept_Name Course_Name,Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND R.Roll_No ='" + rollno + "' ";
                                            dsStudent.Clear();
                                            dsStudent = d2.select_method_wo_parameter(Sql, "text");
                                            if (dsStudent.Tables[0].Rows.Count > 0)
                                            {
                                                string name = Convert.ToString(dsStudent.Tables[0].Rows[0]["Stud_Name"]);
                                                string course = Convert.ToString(dsStudent.Tables[0].Rows[0]["Course_Name"]);
                                                string servertime = d2.ServerTime();
                                                //DateTime dtSerTime = Convert.ToDateTime(servertime);
                                                //string[] date11 = Convert.ToString(dtSerTime).Split(' ');
                                                //string strServerTime = date11[1] + " " + date11[2];
                                                string[] arr = servertime.Split(':');
                                                int hour = Convert.ToInt32(arr[0]);
                                                int min = Convert.ToInt32(arr[1]);
                                                int sec = Convert.ToInt32(arr[2]);
                                                string[] dat = dateval.Split('/');
                                                int day = Convert.ToInt32(dat[1]);
                                                int month = Convert.ToInt32(dat[0]);
                                                int year = Convert.ToInt32(dat[2].Split(' ')[0]);
                                                DateTime ExitTime = new DateTime(year, month, day, hour, min, sec);
                                                ExitTime = ExitTime.AddHours(1);
                                                string date11 = ExitTime.ToString("dd/MM/yyyy HH:mm:ss");
                                                string[] timeval = Convert.ToString(date11).Split(' ');
                                                string strTime = timeval[1];//+ " " + timeval[2]
                                                string SelQry = "select * from libusers where usercat='Student' and entry_date='" + dateval + "' and roll_no='" + rollno + "'";
                                                dsLibrary.Clear();
                                                dsLibrary = d2.select_method_wo_parameter(SelQry, "text");
                                                if (dsLibrary.Tables[0].Rows.Count == 0)
                                                {
                                                    int monthyear = (year * 12) + month;
                                                    string StrDay = "d" + day + "d1";
                                                    string SelectQry = "  select " + StrDay + " from attendance where roll_no='" + rollno + "' and month_year='" + monthyear + "'";
                                                    dsAtt.Clear();
                                                    dsAtt = d2.select_method_wo_parameter(SelectQry, "text");
                                                    if (dsAtt.Tables[0].Rows.Count > 0)
                                                    {
                                                        string Present = Convert.ToString(dsAtt.Tables[0].Rows[0][0]);
                                                        if (!string.IsNullOrEmpty(Present) && Present == "1")
                                                        {
                                                            Sql = "INSERT INTO LibUsers(roll_no,stud_name,dept_name,current_semester,entry_date,entry_time,exit_time,usercat,lib_code,visitor_details,IsManual) Values('" + rollno + "','" + name + "','" + course + "'," + intCurSem + ",'" + dateval + "','" + servertime + "','" + strTime + "','Student','1','',0)";
                                                            insert = d2.update_method_wo_parameter(Sql, "TEXT");
                                                            stuCount++;
                                                        }
                                                    }
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
                        totcount = 0;
                        stuCount = 0;
                        sql = " select * from Registration where Batch_Year in('2017','2016','2015') and cc=0 and Exam_Flag<>'debar' and DelFlag=0 ";
                        data.Clear();
                        data = d2.select_method_wo_parameter(sql, "text");
                        if (data.Tables[0].Rows.Count > 0)
                        {
                            for (int ij = 0; ij < data.Tables[0].Rows.Count; ij++)
                            {
                                if (totcount <= 230)//298
                                {
                                    string intStudDegCode = Convert.ToString(data.Tables[0].Rows[ij]["degree_code"]);
                                    string intCurSem = Convert.ToString(data.Tables[0].Rows[ij]["Current_Semester"]);
                                    Sql = "SELECT ISNULL(COUNT(*),0) TotHolDays FROM HolidayStudents WHERE Degree_Code=" + intStudDegCode + " AND Semester =" + intCurSem + " AND HalfOrFull=0 AND Holiday_Date ='" + dateval + "' ";
                                    dataLoad.Clear();
                                    dataLoad = d2.select_method_wo_parameter(Sql, "text");
                                    if (dataLoad.Tables[0].Rows.Count == 0)
                                    {
                                        string rollno = Convert.ToString(data.Tables[0].Rows[ij]["Roll_No"]);
                                        Sql = "SELECT R.Roll_No,App_No,R.Stud_Name,Course_Name+' - '+Dept_Name Course_Name,Current_Semester FROM Registration R,Degree G,Course C,Department D WHERE R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.Dept_Code = D.Dept_Code AND R.Roll_No ='" + rollno + "' ";
                                        dsStudent.Clear();
                                        dsStudent = d2.select_method_wo_parameter(Sql, "text");
                                        if (dsStudent.Tables[0].Rows.Count > 0)
                                        {
                                            string name = Convert.ToString(dsStudent.Tables[0].Rows[0]["Stud_Name"]);
                                            string course = Convert.ToString(dsStudent.Tables[0].Rows[0]["Course_Name"]);
                                            string servertime = d2.ServerTime();
                                            //DateTime dtSerTime = Convert.ToDateTime(servertime);
                                            //string[] date11 = Convert.ToString(dtSerTime).Split(' ');
                                            //string strServerTime = date11[1] + " " + date11[2];
                                            string[] arr = servertime.Split(':');
                                            int hour = Convert.ToInt32(arr[0]);
                                            int min = Convert.ToInt32(arr[1]);
                                            int sec = Convert.ToInt32(arr[2]);
                                            string[] dat = dateval.Split('/');
                                            int day = Convert.ToInt32(dat[1]);
                                            int month = Convert.ToInt32(dat[0]);
                                            int year = Convert.ToInt32(dat[2].Split(' ')[0]);
                                            DateTime ExitTime = new DateTime(year, month, day, hour, min, sec);
                                            ExitTime = ExitTime.AddHours(1);
                                            string date11 = ExitTime.ToString("dd/MM/yyyy HH:mm:ss");
                                            string[] timeval = Convert.ToString(date11).Split(' ');
                                            string strTime = timeval[1];//+ " " + timeval[2]
                                            string SelQry = "select * from libusers where usercat='Student' and entry_date='" + dateval + "' and roll_no='" + rollno + "'";
                                            dsLibrary.Clear();
                                            dsLibrary = d2.select_method_wo_parameter(SelQry, "text");
                                            if (dsLibrary.Tables[0].Rows.Count == 0)
                                            {
                                                int monthyear = (year * 12) + month;
                                                string StrDay = "d" + day + "d1";
                                                string SelectQry = "  select " + StrDay + " from attendance where roll_no='" + rollno + "' and month_year='" + monthyear + "'";
                                                dsAtt.Clear();
                                                dsAtt = d2.select_method_wo_parameter(SelectQry, "text");
                                                if (dsAtt.Tables[0].Rows.Count > 0)
                                                {
                                                    string Present = Convert.ToString(dsAtt.Tables[0].Rows[0][0]);
                                                    if (!string.IsNullOrEmpty(Present) && Present == "1")
                                                    {
                                                        Sql = "INSERT INTO LibUsers(roll_no,stud_name,dept_name,current_semester,entry_date,entry_time,exit_time,usercat,lib_code,visitor_details,IsManual) Values('" + rollno + "','" + name + "','" + course + "'," + intCurSem + ",'" + dateval + "','" + servertime + "','" + strTime + "','Student','1','',0)";
                                                        insert = d2.update_method_wo_parameter(Sql, "TEXT");
                                                        stuCount++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                DateTime DueDate = k.AddDays(1);
                k = DueDate;
                stuCount = 0;
            }
            //==============================================================//

            //=========================Staff Entry===========================//
            totcount = 0;
            stuCount = 0;

            for (DateTime kj = Convert.ToDateTime(fdate); kj <= Convert.ToDateTime(tdate); )
            {
                string dateval = Convert.ToString(kj);
                Sql = "SELECT * FROM Holiday_Library WHERE Holiday_Date ='" + dateval + "' ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(Sql, "text");
                if (dsload.Tables[0].Rows.Count == 0)
                {
                    string sel = "select entry_date as 'Entry Date',count(*) as 'Hit Status' from libusers where usercat='Staff' and entry_date='" + dateval + "'  group by entry_date";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        count = Convert.ToDouble(ds.Tables[0].Rows[0]["Hit Status"]);
                        if (count <= 35)//56
                        {
                            totcount = 35 - count;
                            sql = "select t.staff_code,t.stfstatus,staff_name,h.dept_name,g.desig_name,stftype,category_name,c.category_code,bsalary,basicpay,grade_pay,pay_band ,IsMPFAmt,MPFAmount,MPFPer,t.allowances,t.deductions from stafftrans t,staffmaster m,hrdept_master h,desig_master g,staffcategorizer c, staff_appl_master sa where m.appl_no=sa.appl_no and t.staff_code = m.staff_code and t.dept_code = h.dept_code and t.desig_code = g.desig_code  and t.category_code = c.category_code and m.college_code = h.college_code and m.college_code = g.collegeCode and m.college_code = c.college_code and t.latestrec = 1 and resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and sa.interviewstatus ='appointed' and stftype='Teaching' ";//
                            data.Clear();
                            data = d2.select_method_wo_parameter(sql, "text");
                            if (data.Tables[0].Rows.Count > 0)
                            {
                                for (int ij = 0; ij < data.Tables[0].Rows.Count; ij++)
                                {
                                    if (stuCount <= totcount)
                                    {
                                        Sql = "SELECT * FROM holidayStaff where stftype='Teaching' AND HalfOrFull=0 AND Holiday_Date ='" + dateval + "' ";
                                        dataLoad.Clear();
                                        dataLoad = d2.select_method_wo_parameter(Sql, "text");
                                        if (dataLoad.Tables[0].Rows.Count == 0)
                                        {
                                            string rollno = Convert.ToString(data.Tables[0].Rows[ij]["staff_code"]);
                                            string name = Convert.ToString(data.Tables[0].Rows[ij]["staff_name"]);
                                            string course = Convert.ToString(data.Tables[0].Rows[ij]["dept_name"]);
                                            string servertime = d2.ServerTime();
                                            string[] arr = servertime.Split(':');
                                            int hour = Convert.ToInt32(arr[0]);
                                            int min = Convert.ToInt32(arr[1]);
                                            int sec = Convert.ToInt32(arr[2]);
                                            string[] dat = dateval.Split('/');
                                            int day = Convert.ToInt32(dat[1]);
                                            int month = Convert.ToInt32(dat[0]);
                                            int year = Convert.ToInt32(dat[2].Split(' ')[0]);
                                            DateTime ExitTime = new DateTime(year, month, day, hour, min, sec);
                                            ExitTime = ExitTime.AddHours(1);
                                            string date11 = ExitTime.ToString("dd/MM/yyyy HH:mm:ss");
                                            string[] timeval = Convert.ToString(date11).Split(' ');
                                            string strTime = timeval[1];//+ " " + timeval[2]
                                            string SelQry = "select * from libusers where usercat='Staff' and entry_date='" + dateval + "' and roll_no='" + rollno + "'";
                                            dsLibrary.Clear();
                                            dsLibrary = d2.select_method_wo_parameter(SelQry, "text");
                                            if (dsLibrary.Tables[0].Rows.Count == 0)
                                            {
                                                string monthyear = month + "/" + year;
                                                string SelectQry = " select [" + day + "] from staff_attnd where mon_year='" + monthyear + "' and staff_code='" + rollno + "'";
                                                dsAtt.Clear();
                                                dsAtt = d2.select_method_wo_parameter(SelectQry, "text");
                                                if (dsAtt.Tables[0].Rows.Count > 0)
                                                {
                                                    string Present = Convert.ToString(dsAtt.Tables[0].Rows[0][0]);
                                                    if (!string.IsNullOrEmpty(Present))
                                                    {
                                                        string[] atten = Present.Split('-');
                                                        if (atten[0].ToLower() == "p")
                                                        {
                                                            Sql = "INSERT INTO LibUsers(roll_no,stud_name,dept_name,current_semester,entry_date,entry_time,exit_time,usercat,lib_code,visitor_details,IsManual) Values('" + rollno + "','" + name + "','" + course + "',0,'" + dateval + "','" + servertime + "','" + strTime + "','Staff','1','',0)";
                                                            insert = d2.update_method_wo_parameter(Sql, "TEXT");
                                                            stuCount++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                DateTime DueDate = kj.AddDays(1);
                kj = DueDate;
                stuCount = 0;
            }

            //==============================================================//
        }
        catch (Exception ex) { }
    }

}