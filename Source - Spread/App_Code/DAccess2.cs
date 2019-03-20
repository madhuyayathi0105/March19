//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.IO;
//using System.Collections;
//using System.Data.SqlClient;
//using FarPoint.Web.Spread;
//using System.Net;
//using System.Web.UI.WebControls;
//using System.Collections.Generic;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using FarPoint.Web.Spread;
/// <summary>
/// Summary description for DAccess2
/// </summary>
public class DAccess2
{
    int i;
    connection connection = new connection();
    DataSet daload = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    SqlDataAdapter adaload;
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Boolean doubleday = false;
    public int insert_method(string sqlcmd, Hashtable ht, string sptype)
    {
        try
        {
            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            foreach (DictionaryEntry parameter in ht)
            {
                cmd.Parameters.Add((string)parameter.Key, parameter.Value);
            }
            i = cmd.ExecuteNonQuery();
            return i;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public int update_method_wo_parameter(string sqlcmd, string sptype)
    {
        try
        {
            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            i = cmd.ExecuteNonQuery();
            return i;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet select_method(string sqlcmd, Hashtable ht, string sptype)
    {
        DataSet ds = new DataSet();
        try
        {
            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            foreach (DictionaryEntry parameter in ht)
            {
                cmd.Parameters.Add((string)parameter.Key, parameter.Value);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
        return ds;
    }

    public DataSet select_method_wo_parameter(string sqlcmd, string sptype)
    {
        DataSet ds = new DataSet();
        try
        {

            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
        return ds;
    }

    public DataSet loaddepartment(string college)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string sqlcmddepartment = "SELECT h.Dept_Code,h.Dept_Name FROM HRDept_Master h,department d WHERE h.dept_code=d.dept_code and d.College_Code=h.College_Code and h.College_Code = '" + college + "' order by h.Dept_Name";
            cmd = new SqlCommand(sqlcmddepartment);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet loaddepartment_academic(string college)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string sqlcmddepartment = "SELECT Dept_Code,Dept_Name FROM department WHERE isacademic=1 and College_Code = '" + college + "'";
            cmd = new SqlCommand(sqlcmddepartment);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet loaddesignation(string college)
    {
        try
        {
            string sqlcmddesignation = "SELECT Desig_Code,Desig_Name FROM Desig_Master WHERE collegeCode =  '" + college + "' order by Desig_Name";
            cmd = new SqlCommand(sqlcmddesignation);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet loadcategory(string college)
    {
        try
        {
            string sqlcmdcategory = "SELECT Category_Code,category_name FROM StaffCategorizer WHERE college_code =  '" + college + "' order by category_name";
            cmd = new SqlCommand(sqlcmdcategory);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet loadstafftype(string college)
    {
        try
        {
            string sqlcmdstafftype = "SELECT DISTINCT T.StfType FROM StaffTrans T,StaffMaster M WHERE T.Staff_Code = M.Staff_Code AND T.Latestrec = 1 AND M.College_Code = '" + college + "'";
            cmd = new SqlCommand(sqlcmdstafftype);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindBatch()
    {
        string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            cmd = new SqlCommand("bind_degree");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection.CreateConnection();
            foreach (DictionaryEntry parameter in hat)
            {
                cmd.Parameters.Add((string)parameter.Key, parameter.Value);
            }
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", course_id);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            cmd = new SqlCommand("bind_branch");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection.CreateConnection();
            foreach (DictionaryEntry parameter in hat)
            {
                cmd.Parameters.Add((string)parameter.Key, parameter.Value);
            }
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            string[] batchsplit = strbatch.Split(new Char[] { ',' });
            int batchcnt = Convert.ToInt32(batchsplit.GetLongLength(0));
            string[] branchsplit = strbranch.Split(new Char[] { ',' });
            int branchcnt = Convert.ToInt32(branchsplit.GetLongLength(0));
            daload.Dispose();
            daload.Reset();
            if (batchcnt == 1 && branchcnt == 1)
            {
                string strsql = "select distinct sections from registration where batch_year =" + strbatch + " and degree_code =" + strbranch + " and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
                cmd = new SqlCommand(strsql);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(daload);
            }
            else
            {
                string strsql1 = "select distinct sections from registration where batch_year in(" + strbatch + ") and degree_code in(" + strbranch + ") and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(daload);
            }
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code in (" + strbranch + ") and batch_year in (" + strbatchyear + ") and college_code=" + collegecode + "";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                //return daload;
            }
            else
            {
                daload.Dispose();
                daload.Reset();
                string strsql1 = "select distinct duration,first_year_nonsemester  from degree where degree_code in (" + strbranch + ") and college_code=" + collegecode + "";
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(daload);
                if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
                {
                    //return daload;
                }
            }
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    //public DataSet BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    //{
    //    try
    //    {
    //        if (course_id.ToString().Trim() != "")
    //        {
    //            if (singleuser == "True")
    //            {
    //                daload.Dispose();
    //                daload.Reset();
    //                string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " ";
    //                cmd = new SqlCommand(strquery);
    //                cmd.Connection = connection.CreateConnection();
    //                adaload = new SqlDataAdapter(cmd);
    //                adaload.Fill(daload);
    //            }
    //            else
    //            {
    //                daload.Dispose();
    //                daload.Reset();
    //                string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
    //                cmd = new SqlCommand(strquery1);
    //                cmd.Connection = connection.CreateConnection();
    //                adaload = new SqlDataAdapter(cmd);
    //                adaload.Fill(daload);
    //            }
    //        }
    //        return daload;
    //    }
    //    catch (SqlException ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        connection.Close();
    //    }
    //}

    public DataSet BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            if (course_id.ToString().Trim() != "")
            {
                if (singleuser == "True")
                {
                    daload.Dispose();
                    daload.Reset();
                    string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
                    cmd = new SqlCommand(strquery);
                    cmd.Connection = connection.CreateConnection();
                    adaload = new SqlDataAdapter(cmd);
                    adaload.Fill(daload);
                }
                else
                {
                    daload.Dispose();
                    daload.Reset();
                    string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                    cmd = new SqlCommand(strquery1);
                    cmd.Connection = connection.CreateConnection();
                    adaload = new SqlDataAdapter(cmd);
                    adaload.Fill(daload);
                }
            }
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindSubject(string strbatch, string strbranch, string strsem, string strsec)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and  r.current_semester=s.semester and r.degree_code in ( " + strbranch + ") and r.batch_year in (" + strbatch + ")";
        string strsyllcode = string.Empty;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < daload.Tables[0].Rows.Count; count++)
                {
                    if (strsyllcode == "")
                    {
                        strsyllcode = "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                    else
                    {
                        strsyllcode = strsyllcode + "," + "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                }
                ds1.Dispose();
                ds1.Reset();
                string strsql1 = "";
                if (strsec.ToString().Trim() != "" && strsec.ToString().Trim() != "-1")
                {
                    strsql1 = "select distinct subject_name from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") and registration.sections in (" + strsec + ")   and subject.syll_code in (" + strsyllcode + ") order by subject_name";
                }
                else
                {
                    strsql1 = "select distinct subject_name from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ")   and subject.syll_code in (" + strsyllcode + ") order by subject_name";
                }
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(ds1);
            }
            return ds1;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindTest(string strbatch, string strbranch)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and  r.current_semester=s.semester and r.degree_code in ( " + strbranch + ") and r.batch_year in (" + strbatch + ")";
        string strsyllcode = string.Empty;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < daload.Tables[0].Rows.Count; count++)
                {
                    if (strsyllcode == "")
                    {
                        strsyllcode = "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                    else
                    {
                        strsyllcode = strsyllcode + "," + "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                }
            }
            if (strsyllcode != "")
            {
                ds1.Dispose();
                ds1.Reset();
                string strsql1 = "select distinct criteria from criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and c.syll_code in (" + strsyllcode + ") order by criteria";
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(ds1);
            }
            return ds1;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet retriveoveralldetailsp(string examcode, int minmark, string section)
    {
        daload.Dispose();
        daload.Reset();
        hat.Clear();
        hat.Add("exam_code", examcode);
        hat.Add("min_marks", minmark);
        hat.Add("section", section);
        cmd = new SqlCommand("Proc_All_Subject_Details");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = connection.CreateConnection();
        foreach (DictionaryEntry parameter in hat)
        {
            cmd.Parameters.Add((string)parameter.Key, parameter.Value);
        }
        adaload = new SqlDataAdapter(cmd);
        adaload.Fill(daload);
        return daload;
    }

    public DataSet BindSubjecttest(string strbatch, string strbranch, string strsem, string strsec)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select syll_code from syllabus_master where degree_code='" + strbranch + "' and semester ='" + strsem + "' and batch_year='" + strbatch + "'";
        string strsyllcode = string.Empty;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < daload.Tables[0].Rows.Count; count++)
                {
                    if (strsyllcode == "")
                    {
                        strsyllcode = "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                }
                ds1.Dispose();
                ds1.Reset();
                string strsql3 = "select distinct subjectchooser.subject_no,subject_name,subject_code,acronym,subject_type from subject,subjectchooser,registration,sub_sem,exam_type e where e.subject_no=subject.subject_no and  sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no   and registration.degree_code='" + strbranch + "' and registration.batch_year='" + strbatch + "' " + strsec + "  and subject.syll_code  in (" + strsyllcode + ") order by subjectchooser.subject_no";
                cmd = new SqlCommand(strsql3);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(ds1);
            }
            return ds1;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Bindtest(string strbatch, string strbranch, string strsem, string strsec1)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select syll_code from syllabus_master where degree_code='" + strbranch + "' and semester ='" + strsem + "' and batch_year='" + strbatch + "'";
        string strsyllcode = string.Empty;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < daload.Tables[0].Rows.Count; count++)
                {
                    if (strsyllcode == "")
                    {
                        strsyllcode = "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                }
                ds1.Dispose();
                ds1.Reset();
                cmd = new SqlCommand("select distinct c.criteria,c.criteria_no from criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and c.syll_code=" + strsyllcode + " and  c.criteria_no=e.criteria_no " + strsec1 + " order by c.criteria_no");
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(ds1);
            }
            return ds1;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Bindtransport(string sqlstrroute, string sqlstrvechile, string sqlstrplace)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "SELECT Route_Name,Stage_Name,Veh_ID,NofTravrs FROM RouteMaster R,Vehicle_Master V WHERE R.Route_ID = V.Route AND Sess = 'M' AND Arr_Time = 'Halt' " + sqlstrroute + " " + sqlstrvechile + " " + sqlstrplace + " order by Route_ID";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Bindplace()
    {
        daload.Dispose();
        daload.Reset();
        //string strsql = "Select Distinct s.Stage_Name, s.stage_id from RouteMaster r,stage_master s WHERE cast(s.stage_id as varchar(100) )=cast(r.stage_name as varchar(100)) and Sess = 'M' AND Arr_Time = 'Halt'";
        string strsql = "Select Distinct s.Stage_Name, s.stage_id from RouteMaster r,stage_master s WHERE cast(s.stage_id as varchar(100) )=cast(r.stage_name as varchar(100)) and Sess = 'M'";//rajasekar4
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindVechile()
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "Select Distinct Veh_ID from Vehicle_Master order by Veh_ID asc";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Bindroute()
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "Select Distinct Route_ID,Route_Name from RouteMaster group by Route_ID, Route_Name";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindmultSem(string collegecode)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + collegecode + "";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
            }
            else
            {
                daload.Dispose();
                daload.Reset();
                string strsql1 = "select distinct duration,first_year_nonsemester  from degree where college_code=" + collegecode + "";
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(daload);
                if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
                {
                }
            }
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindSectionDetailmult(string collegecode)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct sections from registration where college_code='" + collegecode + "' and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Bindstudenttransport(string sqlbatchquery, string sqlbranchquery, string sqlsecquery, string studtype)
    {
        daload.Dispose();
        daload.Reset();
        //string strsql = "SELECT ROW_NUMBER() OVER (ORDER BY  r.Roll_no) As SrNo,R.Roll_No,isnull(Reg_No,'-') as reg_no,R.Stud_Name,CASE WHEN A.Sex = 0 THEN 'Male' ELSE 'Female' END Gender,R.Stud_Type,isnull(Bus_RouteID,'-') as Bus_RouteID,isnull(VehID,'-')as VehID,isnull((select stage_name from stage_master s where str(s.stage_id)=str(Boarding)),'-') as Boarding,isnull(Hostel_Name,'-') as  Hostel_Name,isnull(Room_Name,'-') as Room_Name FROM Registration R INNER JOIN Applyn A ON R.App_No = A.App_No LEFT JOIN Hostel_StudentDetails S ON S.Roll_Admit = R.Roll_Admit LEFT JOIN Hostel_Details H ON S.Hostel_Code = H.Hostel_Code where cc=0 and delflag=0 and exam_flag!='debar' " + sqlbatchquery + "  " + sqlbranchquery + " " + sqlsecquery + " order by r.Roll_No ";

        string strsql = "";
        if (studtype == "All")//added by rajasekar 10/09/2018
        {
            strsql = "SELECT ROW_NUMBER() OVER (ORDER BY  r.Roll_no) As SrNo,R.Roll_No,isnull(Reg_No,'-') as reg_no,R.Stud_Name,CASE WHEN A.Sex = 0 THEN 'Male' ELSE 'Female' END Gender,R.Stud_Type,isnull(Bus_RouteID,'-') as Bus_RouteID,isnull(VehID,'-')as VehID,isnull((select stage_name from stage_master s where str(s.stage_id)=str(Boarding)),'-') as Boarding,isnull(HostelName,'-') as  Hostel_Name,isnull((select  Room_Name from Room_Detail rd where rd.RoomPK=s.RoomFK),'-') as Room_Name  FROM Registration R INNER JOIN Applyn A ON R.App_No = A.App_No LEFT JOIN HT_HostelRegistration S ON S.APP_No = R.App_No LEFT JOIN HM_HostelMaster H ON S.HostelMasterFK = HostelMasterPK where cc=0 and delflag=0 and exam_flag!='debar'" + sqlbatchquery + "  " + sqlbranchquery + " " + sqlsecquery + " order by r.Roll_No ";//modified by rajasekar 10/09/2018
        }
        else
        {
            strsql = "SELECT ROW_NUMBER() OVER (ORDER BY  r.Roll_no) As SrNo,R.Roll_No,isnull(Reg_No,'-') as reg_no,R.Stud_Name,CASE WHEN A.Sex = 0 THEN 'Male' ELSE 'Female' END Gender,R.Stud_Type,isnull(Bus_RouteID,'-') as Bus_RouteID,isnull(VehID,'-')as VehID,isnull((select stage_name from stage_master s where str(s.stage_id)=str(Boarding)),'-') as Boarding,isnull(HostelName,'-') as  Hostel_Name,isnull((select  Room_Name from Room_Detail rd where rd.RoomPK=s.RoomFK),'-') as Room_Name  FROM Registration R INNER JOIN Applyn A ON R.App_No = A.App_No LEFT JOIN HT_HostelRegistration S ON S.APP_No = R.App_No LEFT JOIN HM_HostelMaster H ON S.HostelMasterFK = HostelMasterPK where cc=0 and delflag=0 and exam_flag!='debar' and R.Stud_Type='" + studtype + "'" + sqlbatchquery + "  " + sqlbranchquery + " " + sqlsecquery + " order by r.Roll_No ";//modified by rajasekar 10/09/2018
        }

        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet SubjectOverAllCount(string examcode, int minmark)
    {
        daload.Dispose();
        daload.Reset();
        hat.Clear();
        hat.Add("exam_code", examcode);
        hat.Add("min_marks", minmark);
        cmd = new SqlCommand("Proc_All_Subject_Count");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = connection.CreateConnection();
        foreach (DictionaryEntry parameter in hat)
        {
            cmd.Parameters.Add((string)parameter.Key, parameter.Value);
        }
        adaload = new SqlDataAdapter(cmd);
        adaload.Fill(daload);
        return daload;
    }

    public DataSet BindstudentPhoto(string sqlphototquery)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = sqlphototquery;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet binddesi(string college)
    {
        try
        {
            string strbinddesi = "select  distinct desig_name,desig_code from desig_master where collegeCode='" + college + "' order by desig_name";
            cmd = new SqlCommand(strbinddesi);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet bindstaffnme(string college, string course_id, string dept_id)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string strsqlstaffname = " select distinct  sm.staff_name,sm.staff_code,(sm.staff_name+'-'+sm.staff_code) as staffnamecode from stafftrans s,staffmaster sm,staff_selector st where s.staff_code=sm.staff_code and college_code='" + college + "'  and s.dept_code in(" + dept_id + ") and s.desig_code in(" + course_id + ") and resign = 0 and settled = 0 and st.staff_code=sm.staff_code order by sm.staff_name";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }
    }

    public string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode, bool transferflag = false)
    {
        string calculate = "";
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = "";
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            double gpacal1 = 0;
            string strsubcrd = "";
            int gtempejval = 0;
            string syll_code = "";
            string examcodevalg = "";
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = "";
            string strtotgrac = "";
            string sqlcmdgraderstotal = "";
            int attemptswith = 0;
            string strattmaxmark = "";
            int attmpt = 0, maxmark = 0;
            strattmaxmark = GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
            string[] semecount = strattmaxmark.Split(new Char[] { '-' });
            if (semecount.GetUpperBound(0) == 1)
            {
                attmpt = Convert.ToInt32(semecount[0].ToString());
                maxmark = Convert.ToInt32(semecount[1].ToString());
                flag = true;
            }
            else
            {
                flag = false;
            }
            sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            dggradetot = select_method(sqlcmdgraderstotal, hat, "Text");
            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            if (!transferflag) //modified by prabha feb 10 2018
                strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";

            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' ";
            if (strsubcrd != null && strsubcrd != "")
            {
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                inte = Convert.ToDouble(dr_subcrd["internal_mark"].ToString());
                                exte = Convert.ToDouble(dr_subcrd["external_mark"].ToString());
                                attemptswith = Convert.ToInt32(dr_subcrd["attempts"].ToString());
                                if (flag == true)
                                {
                                    if (attmpt > attemptswith)//ATTEMPTS compared with attempts in coe settings if attempts lower than coe settings
                                    {
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        inte = 0;
                                        strtot = exte;// total only consider extermarks only
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dr_subcrd["grade"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                                //magesh 23/2/18
                                strgrade = "";
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        if (gpacal1 == 0)
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = Convert.ToDouble(strgrade) * creditval;
                            }
                        }
                        else
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                            }
                        }
                    }
                }
            }
            creditval = 0;
            strgrade = "";
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            calculate = Convert.ToString(finalgpa1);
            creditsum1 = 0;
            gpacal1 = 0;
            finalgpa1 = 0;
        }
        catch (Exception vel)
        {
            string exce = vel.ToString();
        }
        if (calculate == "NaN")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }

    //For Particular Exam Month And Year
    public string Calulat_GPA(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = "";
        string strgrade = "";
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = "";
        string examcodeval = "";
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = "";
        string strgradetempgrade = "";
        DataSet dggradetot = new DataSet();
        try
        {
            dggradetot.Dispose();
            daload.Reset();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }
        //dggradetot = GetFunctionv("select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "");
        examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
        }
        if (strsubcrd != "" && strsubcrd != null)
        {
            SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
            con_subcrd.Close();
            con_subcrd.Open();
            SqlDataReader dr_subcrd;
            dr_subcrd = cmd_subcrd.ExecuteReader();
            while (dr_subcrd.Read())
            {
                if (dr_subcrd.HasRows)
                {
                    if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
                    {
                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                            foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                            {
                                if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                {
                                    strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                    strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                    if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if ((dr_subcrd["grade"].ToString() != string.Empty))
                    {
                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                            foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                            {
                                strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                    if (strgrade != "" && strgrade != null)
                    {
                        if (dr_subcrd["credit_points"].ToString() != null && dr_subcrd["credit_points"].ToString() != "")
                        {
                            creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            if (creditsum1 == 0)
                            {
                                creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            }
                            else
                            {
                                creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                            }
                        }
                        if (gpacal1 == 0)
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                        else
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        return finalgpa1.ToString();
    }

    //For Particular Semester
    public string Calulat_GPA_Semwise(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = "";
        string strgrade = "";
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = "";
        string examcodeval = "";
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = "";
        string strgradetempgrade = "";
        string syll_code = "";
        DataSet dggradetot = new DataSet();
        try
        {
            dggradetot.Dispose();
            daload.Reset();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }

        string CheckingQuery = string.Empty;
        //dggradetot = GetFunctionv("select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "");

        examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");//madhumathi 
        syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            //strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";

            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and Exam_code = '" + examcodeval + "' ";//added by madhumathi

            //  CheckingQuery = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Fail' or result='fail' or result='AAA') ";
            CheckingQuery = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Fail' or result='fail' or result='AAA') and Exam_code = '" + examcodeval + "' ";//added by madhumathi
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";


            CheckingQuery = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Fail' or result='fail' or result='AAA')";
        }
        if (strsubcrd != "" && strsubcrd != null && CheckingQuery.Trim() != "")
        {
            bool ArrerCheckFlag = false;
            DataSet DtArrerCheck = select_method_wo_parameter(CheckingQuery, "Text");
            if (DtArrerCheck.Tables.Count > 0 && DtArrerCheck.Tables[0].Rows.Count > 0)
            {
                ArrerCheckFlag = true;
            }
            string val1 = GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");//Rajkumar on 28/5/2018
            if (val1.Trim() == "true" || val1.Trim() == "1")
                ArrerCheckFlag = false;
            if (!ArrerCheckFlag)
            {
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                        if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dr_subcrd["grade"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        if (strgrade != "" && strgrade != null)
                        {
                            if (dr_subcrd["credit_points"].ToString() != null && dr_subcrd["credit_points"].ToString() != "")
                            {
                                creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                if (creditsum1 == 0)
                                {
                                    creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                }
                                else
                                {
                                    creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                }
                            }
                            if (gpacal1 == 0)
                            {
                                gpacal1 = Convert.ToDouble(strgrade) * creditval;
                            }
                            else
                            {
                                gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                            }
                        }
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        return finalgpa1.ToString();
    }

    public string GetFunctionv(string sqlstr)
    {
        string strveretuen = "";
        try
        {
            cmd = new SqlCommand(sqlstr);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection.CreateConnection();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                strveretuen = Convert.ToString(ds.Tables[0].Rows[0][0]);
                return strveretuen;
            }
            else
            {
                return strveretuen;
            }
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public string GetFunction(string sqlQuery)
    {
        try
        {
            string sqlstr;
            sqlstr = sqlQuery;
            con_subcrd.Close();
            con_subcrd.Open();
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_subcrd);
            SqlDataReader drnew;
            SqlCommand funcmd = new SqlCommand(sqlstr);
            funcmd.Connection = con_subcrd;
            drnew = funcmd.ExecuteReader();
            drnew.Read();
            if (drnew.HasRows == true)
            {
                return drnew[0].ToString();
            }
            else
            {
                return "0";
            }
        }
        catch (Exception ex)
        {
            return "0";
        }
        finally
        {
            con_subcrd.Close();
        }
    }

    public void printexcelreport(FpSpread excelprintspread, string ExcelreportName)
    {
        string print = "";
        string appPath = HttpContext.Current.Server.MapPath("~");
        string strexcelname = "";
        if (appPath != "")
        {
            string date = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_tt");
            strexcelname = ExcelreportName.ToString().Trim() + '_' + date.Trim();
            appPath = appPath.Replace("\\", "/");
            if (strexcelname != "")
            {
                print = strexcelname;
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls";
                excelprintspread.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.WriteFile(szPath + szFile);
            }
        }
    }

    public DataSet BindparticularstaffSubject(string strbatch, string strbranch, string strsem, string strsec, string staffcode)
    {
        daload.Dispose();
        daload.Reset();
        string strsql = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' and  r.current_semester=s.semester and r.degree_code in ( " + strbranch + ") and r.batch_year in (" + strbatch + ")";
        string strsyllcode = string.Empty;
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            if (daload != null && daload.Tables[0] != null && daload.Tables[0].Rows.Count > 0)
            {
                for (int count = 0; count < daload.Tables[0].Rows.Count; count++)
                {
                    if (strsyllcode == "")
                    {
                        strsyllcode = "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                    else
                    {
                        strsyllcode = strsyllcode + "," + "'" + daload.Tables[0].Rows[count]["syll_code"].ToString() + "'";
                    }
                }
                ds1.Dispose();
                ds1.Reset();
                string strsql1 = "";
                if (strsec.ToString().Trim() != "" && strsec.ToString().Trim() != "-1")
                {
                    //string strsql3 = "select distinct subjectchooser.subject_no,subject_name,subject_code,acronym,subject_type from subject,subjectchooser,registration,sub_sem,exam_type e where e.subject_no=subject.subject_no and  sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no   and registration.degree_code='" + strbranch + "' and registration.batch_year='" + strbatch + "' " + strsec + "  and subject.syll_code  in (" + strsyllcode + ") order by subjectchooser.subject_no";
                    strsql1 = "select distinct  subject_name , subject.subject_no,subject_type,subject_code,acronym from subject,subjectchooser,registration,sub_sem,staff_selector stsel where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") " + strsec + " and subject.subject_no= stsel.subject_no and stsel.staff_code='" + staffcode + "'  and subject.syll_code in (" + strsyllcode + ") order by subject_type,subject_name";
                }
                else
                {
                    strsql1 = "select distinct subject_name,subject.subject_no,subject_type,subject_code,acronym from subject,subjectchooser,registration,sub_sem,staff_selector stsel where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") and subject.subject_no= stsel.subject_no and stsel.staff_code='" + staffcode + "'  and subject.syll_code in (" + strsyllcode + ") order by subject_type,subject_name";
                }
                cmd = new SqlCommand(strsql1);
                cmd.Connection = connection.CreateConnection();
                adaload = new SqlDataAdapter(cmd);
                adaload.Fill(ds1);
            }
            return ds1;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public string GetUserapi(string user_id)
    {
        try
        {
            user_id = user_id.Trim().ToUpper();
            string SenderID = "";
            string Password = "";
            if (user_id == "AAACET")
            {
                SenderID = "AAACET";
                Password = "AAACET";
            }
            else if (user_id == "AECSMS")
            {
                SenderID = "AECSMS";
                Password = "AECSMS";
            }
            else if (user_id == "AALIME")
            {
                SenderID = "AALIME";
                Password = "AALIME";
            }
            else if (user_id == "AMSAAC")
            {
                SenderID = "AMSAAC";
                Password = "AMSAAC";
            }
            else if (user_id == "ACETVM")
            {
                SenderID = "ACETVM";
                Password = "ACETVM";
            }
            else if (user_id == "AGNICT")
            {
                SenderID = "AGNICT";
                Password = "AGNICT";
            }
            else if (user_id == "AMSPTC")
            {
                SenderID = "AMSPTC";
                Password = "AMSPTC";
            }
            else if (user_id == "ANGE")
            {
                SenderID = "ANGELS";
                Password = "ANGELS";
            }
            else if (user_id == "ARASUU")
            {
                SenderID = "ARASUU";
                Password = "ARASUU";
            }
            else if (user_id == "DAVINC")
            {
                SenderID = "DAVINC";
                Password = "DAVINC";
            }
            else if (user_id == "EASACG")
            {
                SenderID = "EASACG";
                Password = "EASACG";
            }
            else if (user_id == "ECESMS")
            {
                SenderID = "ECESMS";
                Password = "ECESMS";
            }
            else if (user_id == "ESECED")
            {
                SenderID = "ESECED";
                Password = "ESECED";
            }
            else if (user_id == "ESENGG")
            {
                SenderID = "ESENGG";
                Password = "ESENGG";
            }
            else if (user_id == "ESEPTC")
            {
                SenderID = "ESEPTC";
                Password = "ESEPTC";
            }
            else if (user_id == "ESMSCH")
            {
                SenderID = "ESMSCH";
                Password = "ESMSCH";
            }
            else if (user_id == "GKMCET")
            {
                SenderID = "GKMCET";
                Password = "GKMCET";
            }
            else if (user_id == "IJAYAM")
            {
                SenderID = "IJAYAM";
                Password = "IJAYAM";
            }
            else if (user_id == "JJAAMC")
            {
                SenderID = "JJAAMC";
                Password = "JJAAMC";
            }
            else if (user_id == "JMCTRY")
            {
                SenderID = "JMCTRY";
                Password = "JMCTRY";
            }
            else if (user_id == "KINGSE")
            {
                SenderID = "KINGSE";
                Password = "KINGSE";
            }
            else if (user_id == "KNMHSS")
            {
                SenderID = "KNMHSS";
                Password = "KNMHSS";
            }
            else if (user_id == "KSRIET")
            {
                SenderID = "KSRIET";
                Password = "KSRIET";
            }
            else if (user_id == "KTVRKP")
            {
                SenderID = "KTVRKP";
                Password = "KTVRKP";
            }
            else if (user_id == "MPNMJS")
            {
                SenderID = "MPNMJS";
                Password = "MPNMJS";
            }
            else if (user_id == "NANDHA")
            {
                SenderID = "NANDHA";
                Password = "NANDHA";
            }
            else if (user_id == "NECARE")
            {
                SenderID = "NECARE";
                Password = "NECARE";
            }
            else if (user_id == "NSNCET")
            {
                SenderID = "NSNCET";
                Password = "NSNCET";
            }
            else if (user_id == "PETENG")
            {
                SenderID = "PETENG";
                Password = "PETENG";
            }
            else if (user_id == "PMCTEC")
            {
                SenderID = "PMCTEC";
                Password = "PMCTEC";
            }
            else if (user_id == "PPGITS")
            {
                SenderID = "PPGITS";
                Password = "PPGITS";
            }
            else if (user_id == "PROFCL")
            {
                SenderID = "PROFCL";
                Password = "PROFCL";
            }
            else if (user_id == "RVSCHN")
            {
                SenderID = "RVSCHN";
                Password = "RVSCHN";
            }
            else if (user_id == "PSVCET")
            {
                SenderID = "PSVCET";
                Password = "PSVCET";
            }
            else if (user_id == "SASTH")
            {
                SenderID = "SASTHA";
                Password = "SASTHA";
            }
            else if (user_id == "SCTSBS")
            {
                SenderID = "SCTSBS";
                Password = "SCTSBS";
            }
            else if (user_id == "SCTSCE")
            {
                SenderID = "SCTSCE";
                Password = "SCTSCE";
            }
            else if (user_id == "SCTSEC")
            {
                SenderID = "SCTSEC";
                Password = "SCTSEC";
            }
            else if (user_id == "SKCETC")
            {
                SenderID = "SKCETC";
                Password = "SKCETC";
            }
            else if (user_id == "SRECCG")
            {
                SenderID = "SRECCG";
                Password = "SRECCG";
            }
            else if (user_id == "SLAECT")
            {
                SenderID = "SLAECT";
                Password = "SLAECT";
            }
            else if (user_id == "SSCENG")
            {
                SenderID = "SSCENG";
                Password = "SSCENG";
            }
            else if (user_id == "SSMCEE")
            {
                SenderID = "SSMCEE";
                Password = "SSMCEE";
            }
            else if (user_id == "SVICET")
            {
                SenderID = "SVICET";
                Password = "SVICET";
            }
            else if (user_id == "SVCTCG")
            {
                SenderID = "SVCTCG";
                Password = "SVCTCG";
            }
            else if (user_id == "SVSCBE")
            {
                SenderID = "SVSCBE";
                Password = "SVSCBE";
            }
            else if (user_id == "SAIRAM")  //Added By Jeyaprakash
            {
                SenderID = "SAIRAM";
                Password = "SAIRAM";
            }
            else if (user_id == "TECENG")
            {
                SenderID = "TECENG";
                Password = "TECENG";
            }
            else if (user_id == "TJENGG")
            {
                SenderID = "TJENGG";
                Password = "TJENGG";
            }
            else if (user_id == "TSMJCT")
            {
                SenderID = "TSMJCT";
                Password = "TSMJCT";
            }
            else if (user_id == "UITCBE")
            {
                SenderID = "UITCBE";
                Password = "UITCBE";
            }
            else if (user_id == "VCWSMS")
            {
                SenderID = "VCWSMS";
                Password = "VCWSMS";
            }
            else if (user_id == "VRSCET")
            {
                SenderID = "VRSCET";
                Password = "VRSCET";
            }
            else if (user_id == "AUDIIT")
            {
                SenderID = "AUDIIT";
                Password = "AUDIIT";
            }
            else if (user_id == "SAENGG")
            {
                SenderID = "SAENGG";
                Password = "SAENGG";
            }
            else if (user_id == "SAIRA")
            {
                SenderID = "SAIRA";
                Password = "SAIRA";
            }
            else if (user_id == "STANES")  //STANE Modified By Jeyaprakash
            {
                SenderID = "STANES";
                Password = "STANES";
            }
            else if (user_id == "MBCBSE")
            {
                SenderID = "MBCBSE";
                Password = "MBCBSE";
            }
            else if (user_id == "HIETPT")
            {
                SenderID = "HIETPT";
                Password = "HIETPT";
            }
            else if (user_id == "SVPITM")
            {
                SenderID = "SVPITM";
                Password = "SVPITM";
            }
            else if (user_id == "AUDCET")
            {
                SenderID = "AUDCET";
                Password = "AUDCET";
            }
            else if (user_id == "AUDWOM")
            {
                SenderID = "AUDWOM";
                Password = "AUDWOM";
            }
            else if (user_id == "COENIU")
            {
                SenderID = "COENIU";
                Password = "COENIU";
            }
            else if (user_id == "AUDIPG")
            {
                SenderID = "AUDIPG";
                Password = "AUDIPG";
            }
            else if (user_id == "MCCDAY")
            {
                SenderID = "MCCDAY";
                Password = "MCCDAY";
            }
            else if (user_id == "MCCSFS")
            {
                SenderID = "MCCSFS";
                Password = "MCCSFS";
            }
            else if (user_id == "PETPTC")
            {
                SenderID = "PETPTC";
                Password = "PE•PT•";
            }
            else if (user_id == "RVCENG")
            {
                SenderID = "RVCENG";
                Password = "RVCENG";
            }
            else if (user_id == "JMHRSS")
            {
                SenderID = "JMHRSS";
                Password = "JMHRSS";
            }
            else if (user_id == "JECERP")
            {
                SenderID = "JECERP";
                Password = "JECERP";
            }
            else if (user_id == "JHSSCB")
            {
                SenderID = "JHSSCB";
                Password = "JHSSCB";
            }
            else if (user_id == "APTMMR")
            {
                SenderID = "APTMMR";
                Password = "APTMMR";
            }
            else if (user_id == "EGSSMS")
            {
                SenderID = "EGSSMS";
                Password = "EGSSMS";
            }
            else if (user_id == "CITAIN")
            {
                SenderID = "CITAIN";
                Password = "CITAIN";
            }
            else if (user_id == "SHMHSS")
            {
                SenderID = "SHMHSS";
                Password = "SHMHSS";
            }
            else if (user_id == "SVSCHL")
            {
                SenderID = "SVschl";
                Password = "SVschl";
            }
            else if (user_id == "KECERP")
            {
                SenderID = "KECERP";
                Password = "KECERP";
            }
            else if (user_id == "SCOTTC")
            {
                SenderID = "SCOTTC";
                Password = "SCOTTC";
            }
            else if (user_id == "VELITH")
            {
                SenderID = "VELITH";
                Password = "VELITH";
            }
            else if (user_id == "INDIRA")
            {
                SenderID = "INDIRA";
                Password = "INDIRA";
            }
            else if (user_id == "DHIRAJ")
            {
                SenderID = "DHIRAJ";
                Password = "DHIRAJ";
            }
            else if (user_id == "MAMCET")
            {
                SenderID = "MAMCET";
                Password = "MAMCET";
            }
            else if (user_id == "SCOTT")
            {
                SenderID = "SCOTTc";
                Password = "SCOTTc";
            }
            else if (user_id == "GNANMG")
            {
                SenderID = "GNANMG";
                Password = "GNANMG";
            }
            else if (user_id == "NEWCLG")
            {
                SenderID = "NEWCLG";
                Password = "NEWCLG";
            }
            else if (user_id == "eSNCET")
            {
                SenderID = "eSNCET";
                Password = "eSNCET";
            }
            else if (user_id == "AVMSKL")
            {
                SenderID = "AVMSKL";
                Password = "AVMSKL";
            }
            else if (user_id == "JMCBSE")
            {
                SenderID = "JMCBSE";
                Password = "JMCBSE";
            }
            else if (user_id == "KCGERP")
            {
                SenderID = "KCGERP";
                Password = "KCGERP";
            }
            else if (user_id == "SMKFIT")
            {
                SenderID = "SMKFIT";
                Password = "SMKFIT";
            }
            else if (user_id == "ASBEAM")
            {
                SenderID = "ASBEAM";
                Password = "ASBEAM";
            }
            else if (user_id == "APECAD")
            {
                SenderID = "APECAD";
                Password = "APECAD";
            }
            else if (user_id == "PALPAP")
            {
                SenderID = "PALPAP";
                Password = "PALPAP";
            }
            else if (user_id == "LCLIBA")
            {
                SenderID = "LCLIBA";
                Password = "LCLIBA";
            }
            else if (user_id == "STCCED")
            {
                SenderID = "STCCED";
                Password = "STCCED";
            }
            else if (user_id == "SANPAL")
            {
                SenderID = "SANPAL";
                Password = "SANPAL";
            }
            else if (user_id == "SANVEL")
            {
                SenderID = "SANVEL";
                Password = "SANVEL";  //o$4Zf$5I Modified By Jeyaprakash
            }
            else if (user_id == "SSCBSE")
            {
                SenderID = "SSCBSE";
                Password = "SSCBSE";
            }
            else if (user_id == "FXENGG")
            {
                SenderID = "FXENGG";
                Password = "FXENGG";
            }
            else if (user_id == "PATCOL")
            {
                SenderID = "PATCOL";
                Password = "PATCOL";
            }
            else if (user_id == "LICETC")
            {
                SenderID = "LICETc";
                Password = "LICETc";
            }
            else if (user_id == "VELSM")
            {
                SenderID = "VELSMS";
                Password = "VELSMS";
            }
            else if (user_id == "DCESMS")
            {
                SenderID = "DCESMS";
                Password = "DCESMS";
            }
            else if (user_id == "MCCCHS")
            {
                SenderID = "MCCCHS";
                Password = "MCCCHS";
            }
            else if (user_id == "SVSCBE")
            {
                SenderID = "SVSCBE";
                Password = "SVSCBE";
            }
            else if (user_id == "TEMPL")
            {
                SenderID = "TEMPLE";
                Password = "TEMPLE";
            }
            else if (user_id == "JBASWC")
            {
                SenderID = "JBASWC";
                Password = "JBASWC";
            }
            else if (user_id == "SASTRA")
            {
                SenderID = "SASTRA";
                Password = "SASTRA";
            }
            else if (user_id == "SCEERP")
            {
                SenderID = "SCEERP";
                Password = "SCEERP";
            }
            else if (user_id == "SACERP")//added by sudhagar 20.07.2017
            {
                SenderID = "SACERP";
                Password = "SACERP";
            }
            else if (user_id == "PETPTC")//ADDED BY ABARNA 23.01.2018
            {
                SenderID = "PETPTC";
                Password = "PETPTC";
            }
            else if (user_id == "SNSTEC")//ADDED BY ABARNA 23.01.2018
            {
                SenderID = "SNSTEC";
                Password = "SNSTEC";
            }
            else if (user_id == "MARTI")//ADDED BY ABARNA 11.04.2018
            {
                SenderID = "MARTIN";
                Password = "SMEC1@sm";
            }
            else if (user_id == "SNSENG")//ADDED BY ABARNA 5.04.2018
            {
                SenderID = "SNSENG";
                Password = "SNSENG";
            }
            else if (user_id == "SNSCAS")//ADDED BY ABARNA 5.04.2018
            {
                SenderID = "SNSCAS";
                Password = "SNSCAS";
            }
            else if (user_id == "SNSACD")//ADDED BY ABARNA 5.04.2018
            {
                SenderID = "SNSACD";
                Password = "SNSACD";
            }
            else if (user_id == "VCSMMS")//ADDED BY ABARNA 5.04.2018
            {
                SenderID = "VCSMMS";
                Password = "VCSMMS";
            }
            else if (user_id == "PATRIC")//ADDED BY ABARNA 22.06.2018
            {
                SenderID = "PATRIC";
                Password = "PATRIC";
            }
            else if (user_id == "JITCMS")//ADDED BY ABARNA 07.07.2018
            {
                SenderID = "JITCMS";
                Password = "JITCMS";
            }
            else if (user_id == "SKPVIS")
            {
                SenderID = "SKPVIS";
                Password = "SKPVIS";
            }
            else if (user_id == "SKPVMS")
            {
                SenderID = "SKPVMS";
                Password = "SKPVMS";
            }
            else if (user_id == "SONACT")
            {
                SenderID = "SONACT";
                Password = "SONACT";
            }
            else if (user_id == "SKCET")
            {
                SenderID = "SKCET";
                Password = "SKCETC";
            }
            string retval = SenderID + '-' + Password;
            return retval;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    //{
    //    try
    //    {
    //        string Day_Order = "";
    //        int holiday = 0;
    //        if (no_days == "")
    //            return "";
    //        string start_date = "";
    //        if (sdate != "")
    //        {
    //            start_date = sdate;
    //            DateTime dt1 = Convert.ToDateTime(start_date);
    //            DateTime dt2 = Convert.ToDateTime(curday);
    //            string currentdate = dt1.ToString("MM/dd/yyyy");
    //            string startdate = dt2.ToString("MM/dd/yyyy");
    //            dt1 = Convert.ToDateTime(currentdate);
    //            dt2 = Convert.ToDateTime(startdate);
    //            TimeSpan ts = dt2 - dt1;
    //            string query1 = "select count(*) as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'";
    //            string holday = GetFunction(query1);
    //            if (holday != "")
    //                holiday = Convert.ToInt32(holday);
    //            int dif_days = ts.Days;
    //            string leave = GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
    //            if (leave != null && leave != "0")
    //            {
    //                dif_days = dif_days + 1;
    //            }
    //            int dayorderchangedate = 0;
    //            try
    //            {
    //                string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "')) ;";
    //                strdayorder = strdayorder + " select CONVERT(nvarchar(15),holiday_date,101) as hdate from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date<='" + dt2.ToString("yyyy-MM-dd") + "' ";
    //                DataSet dsdayorderchange = select_method_wo_parameter(strdayorder, "Text");
    //                if (dsdayorderchange.Tables[0].Rows.Count > 0)
    //                {
    //                    Hashtable hatholidc = new Hashtable();
    //                    for (int hda = 0; hda < dsdayorderchange.Tables[1].Rows.Count; hda++)
    //                    {
    //                        string hdater = dsdayorderchange.Tables[1].Rows[hda]["hdate"].ToString();
    //                        if (!hatholidc.Contains(hdater))
    //                        {
    //                            hatholidc.Add(hdater, hdater);
    //                        }
    //                    }
    //                    for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
    //                    {
    //                        DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
    //                        DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
    //                        for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
    //                        {
    //                            if (!hatholidc.Contains(dtdcst.ToString("MM/dd/yyyy")))
    //                            {
    //                                if (dtdcst <= dt2)
    //                                {
    //                                    dayorderchangedate = dayorderchangedate + 1;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                holiday = holiday + dayorderchangedate;
    //            }
    //            catch
    //            {
    //            }
    //            int nodays = Convert.ToInt32(no_days);
    //            int order = (dif_days - holiday) % nodays;
    //            order = order + 1;
    //            if (stastdayorder.ToString().Trim() != "")
    //            {
    //                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
    //                {
    //                    order = order + (Convert.ToInt16(stastdayorder) - 1);
    //                    if (order == (nodays + 1))
    //                        order = 1;
    //                    else if (order > nodays)
    //                        order = order % nodays;
    //                }
    //            }
    //            if (order.ToString() == "0")
    //            {
    //                order = Convert.ToInt32(no_days);
    //            }
    //            string finddayorder = "";
    //            if (order == 1)
    //                finddayorder = "mon";
    //            else if (order == 2) finddayorder = "tue";
    //            else if (order == 3) finddayorder = "wed";
    //            else if (order == 4) finddayorder = "thu";
    //            else if (order == 5) finddayorder = "fri";
    //            else if (order == 6) finddayorder = "sat";
    //            else if (order == 7) finddayorder = "sun";
    //            if (order >= 1)
    //            {
    //                Day_Order = Convert.ToString(order) + "-" + Convert.ToString(finddayorder);
    //            }
    //            else
    //            {
    //                Day_Order = "";
    //            }
    //            return finddayorder;
    //        }
    //        else
    //            return "";
    //    }
    //    catch
    //    {
    //        return "";
    //    }
    //}


    // 4sep2018 Mageshwari public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    //{
    //    try
    //    {
    //        string Day_Order = "";
    //        int holiday = 0;
    //        if (no_days == "")
    //            return "";
    //        string start_date = "";
    //        if (sdate != "")
    //        {
    //            start_date = sdate;
    //            DateTime dt1 = Convert.ToDateTime(start_date);
    //            DateTime dt2 = Convert.ToDateTime(curday);
    //            string currentdate = dt1.ToString("MM/dd/yyyy");
    //            string startdate = dt2.ToString("MM/dd/yyyy");
    //            dt1 = Convert.ToDateTime(currentdate);
    //            dt2 = Convert.ToDateTime(startdate);
    //            TimeSpan ts = dt2 - dt1;
    //           //rajkumar9feb2018 string query1 = "select count(*) as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
    //            string query1 = "select count(distinct holiday_date)  as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
    //            //====================

    //            DateTime holidaychk = new DateTime();
    //            holidaychk = dt2.AddDays(-1);
    //            string doublecun1 = GetFunction("select COUNT (distinct doubledate) from doubledayorder where doubledate ='" + holidaychk + "' and batchYear='" + batch_year + "' and degreecode='" + deg_code + "'");
    //            int dif_days = ts.Days;

    //            string holday = GetFunction(query1);
    //            if (stastdayorder.ToString().Trim() == "0")
    //            {
    //                if (doublecun1 != "" && doublecun1 != "0")
    //                {
    //                    if (holday != "")
    //                    {
    //                       // holday = Convert.ToString(Convert.ToInt32(holday) - 1);
    //                      //  dif_days = dif_days + 1; 

    //                    }
    //                }
    //            }
    //            if (holday != "")
    //                holiday = Convert.ToInt32(holday);
    //            //int dif_days = ts.Days;
    //            string leave = GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
    //            if (leave != null && leave != "0")
    //            {
    //                dif_days = dif_days + 1;
    //            }
    //            int dayorderchangedate = 0;
    //            try
    //            {
    //                string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "')) ;";
    //                strdayorder = strdayorder + " select CONVERT(nvarchar(15),holiday_date,101) as hdate from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date<='" + dt2.ToString("yyyy-MM-dd") + "' ";
    //                DataSet dsdayorderchange = select_method_wo_parameter(strdayorder, "Text");
    //                if (dsdayorderchange.Tables[0].Rows.Count > 0)
    //                {
    //                    Hashtable hatholidc = new Hashtable();
    //                    for (int hda = 0; hda < dsdayorderchange.Tables[1].Rows.Count; hda++)
    //                    {
    //                        string hdater = dsdayorderchange.Tables[1].Rows[hda]["hdate"].ToString();
    //                        if (!hatholidc.Contains(hdater))
    //                        {
    //                            hatholidc.Add(hdater, hdater);
    //                        }
    //                    }
    //                    for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
    //                    {
    //                        DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
    //                        DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
    //                        for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
    //                        {
    //                            if (!hatholidc.Contains(dtdcst.ToString("MM/dd/yyyy")))
    //                            {
    //                                if (dtdcst <= dt2)
    //                                {
    //                                    dayorderchangedate = dayorderchangedate + 1;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                holiday = holiday + dayorderchangedate;
    //            }
    //            catch
    //            {
    //            }
    //            int nodays = Convert.ToInt32(no_days);

    //             //magesh 6.8.18
    //            int dayss = 1;
    //            DateTime cur_day = new DateTime();
    //            cur_day = dt2.AddDays(-dayss);
    //            string chkdoubleday = GetFunction("select * from doubledayorder where doubleDate='" + cur_day + "' and batchYear='" + batch_year +"' and degreecode='" + deg_code + "'");
    //            if (chkdoubleday != "" && chkdoubleday != "0")
    //            {
    //                if (Convert.ToString(System.Web.HttpContext.Current.Session["doubledayshk"]) == "false")
    //                {
    //                    doubleday = true;

    //                }
    //                else
    //                {
    //                    doubleday = false;

    //                }
    //            }
    //            else
    //            {
    //                doubleday = false;

    //            }


    //            string doublecun = GetFunction("select COUNT (distinct doubledate) from doubledayorder where  batchYear='" + batch_year + "' and degreecode='" + deg_code + "' and doubledate between '" + currentdate + "' and '" + cur_day + "' and doubledate<'" + cur_day + "' ");
    //            int cundouble = 0;
    //           int order=0;
    //            int.TryParse(doublecun, out cundouble);
    //            if (stastdayorder.ToString().Trim() == "0")
    //            {
    //                if (doubleday == true)
    //                    cundouble = cundouble + 1;
    //                order = ((dif_days - holiday) + cundouble) % nodays;
    //            }
    //            else
    //                order = (dif_days - holiday) % nodays;
    //            order = order + 1;
    //            if (stastdayorder.ToString().Trim() != "")
    //            {
    //                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
    //                {
    //                    order = order + (Convert.ToInt16(stastdayorder) - 1);
    //                    if (order == (nodays + 1))
    //                        order = 1;
    //                    else if (order > nodays)
    //                        order = order % nodays;
    //                }
    //            }
    //            if (order.ToString() == "0")
    //            {
    //                order = Convert.ToInt32(no_days);
    //            }
    //            string finddayorder = "";
    //            if (order == 1)
    //                finddayorder = "mon";
    //            else if (order == 2) finddayorder = "tue";
    //            else if (order == 3) finddayorder = "wed";
    //            else if (order == 4) finddayorder = "thu";
    //            else if (order == 5) finddayorder = "fri";
    //            else if (order == 6) finddayorder = "sat";
    //            else if (order == 7) finddayorder = "sun";
    //            if (order >= 1)
    //            {
    //                Day_Order = Convert.ToString(order) + "-" + Convert.ToString(finddayorder);
    //            }
    //            else
    //            {
    //                Day_Order = "";
    //            }
    //            return finddayorder;
    //        }
    //        else
    //            return "";
    //    }
    //    catch
    //    {
    //        return "";
    //    }
    //}


    public DataTable select_method_wop_table(string sqlcmd, string sptype)
    {
        try
        {
            cmd = new SqlCommand(sqlcmd);
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);
            return ds;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Examyear()
    {
        daload.Clear();
        string strsql = "select distinct Exam_year from exam_details where Exam_year<>0 order by Exam_year desc";
        try
        {
            cmd = new SqlCommand(strsql);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet Exammonth(string year)
    {
        daload.Clear();
        string strsqlNew = "select distinct Exam_month,upper(convert(varchar(3),DateAdd(month,Exam_month,-1))) as monthName from exam_details where Exam_year='" + year + "' and isnull(Exam_month,'')<>''";
        try
        {
            cmd = new SqlCommand(strsqlNew);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user_id">user id</param>
    /// <param name="collegecode">College Code</param>
    /// <param name="usercode">User Code</param>
    /// <param name="mobilenos">mobile no list</param>
    /// <param name="message">What Message to be Sent</param>
    /// <param name="isstaff">Staff or Student</param>
    /// <param name="smsSendFor">sms for eg. Absentees,OD,Timetable Alter,Transfort,Change CAM Test,ect</param>
    /// <param name="studentAppNo">Student's AppNo</param>
    /// <returns></returns>
    /// 
    //Hide by barath
    //public int send_sms(string user_id, string collegecode, string usercode, string mobilenos, string message, string isstaff, string smsSendFor = null, string studentAppNo = null)
    //{
    //    int smssend = 0;
    //    try
    //    {
    //        string SenderID = "";
    //        string Password = "";
    //        string getval = GetUserapi(user_id);
    //        string[] spret = getval.Split('-');
    //        if (spret.GetUpperBound(0) == 1)
    //        {
    //            SenderID = spret[0].ToString();
    //            Password = spret[1].ToString();
    //        }
    //        string strpath = "http://unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + message + "";
    //        WebRequest request = WebRequest.Create(strpath);
    //        request.Timeout = 10000;
    //        request.Proxy = null;
    //        WebResponse response = request.GetResponse();
    //        Stream data = response.GetResponseStream();
    //        StreamReader sr = new StreamReader(data);
    //        string strvel = sr.ReadToEnd();
    //        string groupmsgid = "";
    //        groupmsgid = strvel;
    //        string date = DateTime.Now.ToString("MM/dd/yyyy");
    //        int sms = 0;
    //        string smsreportinsert = "";
    //        string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
    //        string[] spmessp = strvel.Split(new Char[] { ',' });
    //        for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
    //        {
    //            smssend++;
    //            string smsid = "";
    //            if (spmessp.GetUpperBound(0) >= icount)
    //            {
    //                smsid = spmessp[icount].ToString();
    //            }
    //            string mobile = split_mobileno[icount].ToString();

    //            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id,smsFor,studentAppNo)values( '" + mobile + "','" + smsid + "',N'" + message + "','" + collegecode + "','" + isstaff + "','" + date + "','" + usercode.ToString() + "'," + ((smsSendFor == null) ? "''" : "'" + smsSendFor + "'") + "," + ((studentAppNo == null) ? "''" : "'" + studentAppNo + "'") + ")";
    //            sms = update_method_wo_parameter(smsreportinsert, "Text");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        sendErrorMail(ex, collegecode, "Daccess2");
    //    }
    //    return smssend;
    //}
    //Add by jayaram
    public int send_sms(string user_id, string collegecode, string usercode, string mobilenos, string message, string isstaff, string smsSendFor = null, string studentAppNo = null, string TamilOrEng = null)
    {
       
        int smssend = 0;
        try
        {
          
            string SenderID = "";
            string Password = "";
            string strpath = "";
            string getval = GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                SenderID = spret[0].ToString();
                Password = spret[1].ToString();
            }
            if (TamilOrEng == "1")//Modified by saranya on 17/9/2018
            {
                //Aruna 04/07/2018 
                strpath = "http://www.unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + message + "&unicode=1"; //&unicode=1 for tamil font
            }
            else
            {
                strpath = "http://www.unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + message + "";
            }

            WebRequest request = WebRequest.Create(strpath);
            request.Timeout = 1000000;
            request.Proxy = null;
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel;
            //Added by saranyadevi 21.11.2018
            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            int sms = 0;
            string smsreportinsert = "";
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            string[] spmessp = strvel.Split(new Char[] { ',' });

            string[] Appno = new string[0];//barath 08.08.17
            if (!string.IsNullOrEmpty(studentAppNo))
            {
                Appno = studentAppNo.Split(',');
            }
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smssend++;
                string smsid = "";
                if (spmessp.GetUpperBound(0) >= icount)
                {
                    smsid = spmessp[icount].ToString();
                }
                string mobile = split_mobileno[icount].ToString();
                if (Appno.Length > 0 && icount < Appno.Length)
                {
                    studentAppNo = Convert.ToString(Appno[icount]);
                }
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id,smsFor,studentAppNo)values( '" + mobile + "','" + smsid + "',N'" + message + "','" + collegecode + "','" + isstaff + "','" + date + "','" + usercode.ToString() + "'," + ((smsSendFor == null) ? "''" : "'" + smsSendFor + "'") + "," + ((studentAppNo == null) ? "''" : "'" + studentAppNo + "'") + ")";
                sms = update_method_wo_parameter(smsreportinsert, "Text");
            }
        }
        catch (Exception ex)
        {
            //sendErrorMail(ex, collegecode, "Daccess2");
        }
        return smssend;
    }
    //******************************* Added by Jairam For Common College***************************
    public DataSet BindCollege()
    {
        try
        {
            string itemname = "select college_code,collname  from collinfo";
            ds1.Clear();
            ds1 = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds1;
    }

    //************************************ Hostel Module ************************************
    public DataSet BindHostel(string college)
    {
        try
        {
            ds.Clear();
            string itemname = "select Hostel_code,Hostel_Name  from Hostel_Details  where college_code in ('" + college + "') order by Hostel_code ";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindMess(string college)
    {
        try
        {
            ds.Clear();
            string itemname = "select MessID,MessName,MessAcr from MessMaster where College_Code=" + college + " order by MessID asc";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindBuilding(string buildingCode)
    {
        try
        {
            string itemname = "select code,Building_Name from Building_Master where code in (" + buildingCode + ")";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string GetBuildingCode(string HostelCode)
    {
        string build = "";
        try
        {
            string itemname = "select Building_Code from Hostel_Details where Hostel_code IN ('" + HostelCode + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string build1 = ds.Tables[0].Rows[i]["Building_Code"].ToString();
                    if (build1.Trim() != "")
                    {
                        if (build == "")
                        {
                            build = build1;
                        }
                        else
                        {
                            build = build + "," + build1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return build;
    }

    public DataSet BindFloor(string buildingName)
    {
        try
        {
            string itemname = "select distinct Floor_Name,Floorpk  from Floor_Master where Building_Name in('" + buildingName + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindRoom(string FloorName, string BuildingName)
    {
        try
        {
            string itemname = "select distinct Room_Name,Roompk from Room_Detail where Building_Name in('" + BuildingName + "') and floor_name in('" + FloorName + "') order by Room_Name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindRoomtype(string FloorName, string BuildingName)
    {
        try
        {
            string itemname = "select distinct Room_type from Room_Detail where Building_Name in('" + BuildingName + "') and floor_name in('" + FloorName + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindStore(string collegecode)
    {
        try
        {
            string itemname = "select Store_Code,Store_Name  from StoreMaster where College_Code ='" + collegecode + "' order by Store_Name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemHeaderWithRights()
    {
        try
        {
            string itemname = "select distinct itemheader_code ,itemheader_name  from item_master where  Is_Hostel ='0'";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemHeaderWithOutRights()
    {
        try
        {
            string itemname = "select distinct itemheader_code ,itemheader_name  from item_master order by itemheader_code";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCode(string HeaderCode)
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name   from item_master  where itemheader_code in ('" + HeaderCode + "') and Is_Hostel ='0' order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodewithsubheader(string HeaderCode, string subheadercode)
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name   from item_master  where itemheader_code in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')  and Is_Hostel ='0' order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeAll(string HeaderCode)
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name   from item_master  where itemheader_code in ('" + HeaderCode + "') order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeWithOutParameter()
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name  from item_master  order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindMenuName(string college)
    {
        try
        {
            string itemname = "select distinct MenuCode,MenuName  from MenuMaster where College_Code ='" + college + "' order by MenuName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindVendorName()
    {
        try
        {
            string itemname = "select distinct vendor_code, vendor_name from Vendor_Details order by vendor_code";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindState(string Collegecode)
    {
        try
        {
            string itemname = "select TextCode,TextVal  from TextValTable where TextCriteria='State' and college_code ='" + Collegecode + "'";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindSession(string HostelCode)
    {
        try
        {
            string itemname = "select distinct Session_Code,Session_Name  from Session_Master where Hostel_Code in ('" + HostelCode + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeWithOutParameterhostel()
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name  from item_master where  Is_Hostel ='0'  order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string Gethostelcode(string messname)
    {
        string build = "";
        try
        {
            string itemname = "   select Hostel_Code  from MessMaster m,MessDetail md where m.MessID =md.MessID and m.MessID in ('" + messname + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string build1 = ds.Tables[0].Rows[i]["Hostel_Code"].ToString();
                    if (build1.Trim() != "")
                    {
                        if (build == "")
                        {
                            build = build1;
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return build;
    }

    public DataSet BindItemCodewithsubheaderMaster(string HeaderCode, string subheadercode)
    {
        try
        {
            string itemname = "select distinct item_code  ,item_name   from item_master  where itemheader_code in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')  order by item_name";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    //// sudhagar 
    public DataSet selectheader(string collegecode, string batch1, string deptdegcode, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string selqry = "SELECT A.HeaderFK,HeaderName,SUM(FeeAmount) as Allot,SUM(DeductAmout) as consustion ,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as total,SUM(PaidAmount) as paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount  FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and A.HeaderFK = H.HeaderPK AND P.IsConfirm = 1 AND Admission_Status = 0 and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                // selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                // selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                //  selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName";
            selqry = selqry + " UNION SELECT A.HeaderFK,HeaderName,SUM(FeeAmount) as Allot ,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM  FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H WHERE A.App_No = P.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and  g.college_code = d.college_code and p.degree_code = g.degree_code and A.HeaderFK = H.HeaderPK AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                //selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                // selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                // selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName";
            selqry = selqry + " SELECT A.HeaderFK,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0 and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                //  selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                // selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                // selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName,PayMode";
            selqry = selqry + " UNION SELECT A.HeaderFK,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                // selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                // selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                //selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,PayMode";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and em.App_No = a.App_No   AND P.IsConfirm = 1  AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                // selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                //  selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                // selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet selectledger(string collegecode, string batch1, string deptdegcode, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string selqry = "SELECT A.HeaderFK,HeaderName,LedgerName,LedgerFK, SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H,FM_LedgerMaster L WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY A.HeaderFK,HeaderName,LedgerFK,LedgerName";
            selqry = selqry + "  UNION SELECT A.HeaderFK,HeaderName,LedgerName,LedgerFK, SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H,FM_LedgerMaster L WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and p.degree_code = g.degree_code and g.college_code = d.college_code and A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND P.IsConfirm = 1 AND Admission_Status = 1   and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName,LedgerFK,LedgerName";
            selqry = selqry + " SELECT HeaderName,PayMode,LedgerFK,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and A.HeaderFK = H.HeaderPK AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName,PayMode,LedgerFK";
            selqry = selqry + " UNION SELECT HeaderName,PayMode,LedgerFK,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D,FM_HeaderMaster H WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and A.HeaderFK = H.HeaderPK AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK,HeaderName,PayMode,LedgerFK";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK,h.LedgerPK from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_LedgerMaster H,FM_HeaderMaster fh WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no and  g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and h.LedgerPK =ex.LedgerFK and ex.HeaderFK=h.HeaderFK and h.HeaderFK=fh.HeaderPK and em.App_No = a.App_No   AND P.IsConfirm = 1  and a.LedgerFK=h.LedgerPK   AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK ,h.LedgerPK";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet selectbatchyr(string collegecode, string batch1, string deptdegcode, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string selqry = "SELECT batch_year,SUM(FeeAmount)as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY batch_year";
            selqry = selqry + " UNION SELECT p.batch_year,SUM(FeeAmount)as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and p.degree_code = g.degree_code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //  selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY p.batch_year ";
            selqry = selqry + " SELECT batch_year,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0 and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY PayMode,batch_year";
            selqry = selqry + " UNION SELECT p.batch_year ,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no  AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY p.batch_year,PayMode";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK,h.LedgerPK,p.Batch_Year from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_LedgerMaster H,FM_HeaderMaster fh WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and h.LedgerPK =ex.LedgerFK and ex.HeaderFK=h.HeaderFK and h.HeaderFK=fh.HeaderPK and em.App_No = a.App_No   AND P.IsConfirm = 1  and a.LedgerFK=h.LedgerPK   AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY A.HeaderFK ,h.LedgerPK,p.Batch_Year";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet selectdegree(string collegecode, string batch1, string deptdegcode, string degcourseid, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string selqry = "SELECT Course_Name DegreeName,G.Course_Id,SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (degcourseid != "")
            {
                selqry = selqry + " and c.Course_Id in ('" + degcourseid + "')";
            }
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name";
            selqry = selqry + " UNION SELECT Course_Name,G.Course_Id,SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and p.degree_code = g.degree_code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (degcourseid != "")
            {
                selqry = selqry + " and c.Course_Id in ('" + degcourseid + "')";
            }
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and a.HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name  ";
            selqry = selqry + " SELECT Course_Name,G.Course_Id,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0 and G.college_code ='" + collegecode + "'";
            if (degcourseid != "")
            {
                selqry = selqry + " and c.Course_Id in ('" + degcourseid + "')";
            }
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY G.Course_Id,Course_Name,PayMode";
            selqry = selqry + " UNION SELECT Course_Name,G.Course_Id,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (degcourseid != "")
            {
                selqry = selqry + " and c.Course_Id in ('" + degcourseid + "')";
            }
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name,PayMode ";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK,h.LedgerPK,p.Batch_Year,C.Course_Id from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_LedgerMaster H,FM_HeaderMaster fh WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and h.LedgerPK =ex.LedgerFK and ex.HeaderFK=h.HeaderFK and h.HeaderFK=fh.HeaderPK and em.App_No = a.App_No   AND P.IsConfirm = 1  and a.LedgerFK=h.LedgerPK   AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY A.HeaderFK ,h.LedgerPK,p.Batch_Year ,C.Course_Id";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet selectdept(string collegecode, string batch1, string deptdegcode, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            daload.Dispose();
            daload.Reset();
            string selqry = "SELECT Course_Name,Dept_Name,G.Dept_Code, SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0   and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name,G.Dept_Code,Dept_Name ";
            selqry = selqry + "  UNION SELECT Course_Name,Dept_Name,G.Dept_Code,SUM(FeeAmount) as Allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and p.degree_code = g.degree_code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //  selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name,G.Dept_Code,Dept_Name ";
            selqry = selqry + " SELECT Course_Name,Dept_Name,G.Dept_Code,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "  GROUP BY G.Course_Id,Course_Name,G.Dept_Code,Dept_Name,PayMode";
            selqry = selqry + " UNION SELECT Course_Name,Dept_Name,G.Dept_Code,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D WHERE A.App_No = P.app_no  AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY G.Course_Id,Course_Name,G.Dept_Code,Dept_Name,PayMode ";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK,h.LedgerPK,p.Batch_Year,C.Course_Id,d.Dept_Code from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_LedgerMaster H,FM_HeaderMaster fh WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and h.LedgerPK =ex.LedgerFK and ex.HeaderFK=h.HeaderFK and h.HeaderFK=fh.HeaderPK and em.App_No = a.App_No   AND P.IsConfirm = 1  and a.LedgerFK=h.LedgerPK   AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY A.HeaderFK ,h.LedgerPK,p.Batch_Year ,C.Course_Id,d.Dept_Code";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet selectsem(string collegecode, string batch1, string deptdegcode, string sem, string sec, string stud, string seatype, string studtype, string headervalue, string ledgervalue, int datewise, string fromdate, string todate)
    {
        try
        {
            string selqry = "SELECT TextVal,SUM(FeeAmount) as allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,TextValTable T WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and p.degree_code = g.degree_code and g.college_code = d.college_code AND A.FeeCategory = T.TextCode AND P.IsConfirm = 1 AND Admission_Status = 0  and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY TextVal ";
            selqry = selqry + "  UNION SELECT TextVal,SUM(FeeAmount) as allot,SUM(DeductAmout) as Consustion,SUM(FromGovtAmt) as Government,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,TextValTable T WHERE A.App_No = P.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 AND A.FeeCategory = T.TextCode and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY TextVal";
            selqry = selqry + " SELECT TextVal,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D,TextValTable T WHERE A.App_No = P.app_no AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 0 AND A.FeeCategory = T.TextCode and G.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + "and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + " and g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + "and A.FeeCategory in ('" + sem + "')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + " and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "  GROUP BY TextVal, PayMode";
            selqry = selqry + " UNION SELECT TextVal,PayMode,SUM(Debit) as total FROM FT_FinDailyTransaction A,Applyn P,Degree G,Course C,Department D,TextValTable T WHERE A.App_No = P.app_no  AND P.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND P.IsConfirm = 1 AND Admission_Status = 1 AND A.FeeCategory = T.TextCode and g.college_code ='" + collegecode + "'";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                //selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and HeaderFK in ('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + " and LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND  a.TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + "GROUP BY TextVal,PayMode";
            selqry = selqry + " select SUM(Ex.Excessamt) as ExcessAmount,A.HeaderFK,h.LedgerPK,p.Batch_Year,C.Course_Id,d.Dept_Code,A.FeeCategory from FT_ExcessDet EM,FT_ExcessLedgerDet Ex ,FT_FeeAllot A,Applyn P,Degree G,Course C,Department D,FM_LedgerMaster H,FM_HeaderMaster fh WHERE EM.ExcessDetPK = Ex.ExcessDetfk and em.App_No = p.app_no  and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and h.LedgerPK =ex.LedgerFK and ex.HeaderFK=h.HeaderFK and h.HeaderFK=fh.HeaderPK and em.App_No = a.App_No   AND P.IsConfirm = 1  and a.LedgerFK=h.LedgerPK   AND Admission_Status = 1 and g.college_code ='" + collegecode + "' ";
            if (batch1 != "")
            {
                selqry = selqry + " and p.Batch_Year in ('" + batch1 + "')";
            }
            if (deptdegcode != "")
            {
                selqry = selqry + "  and  g.Degree_Code in ('" + deptdegcode + "')";
            }
            if (sem != "")
            {
                selqry = selqry + " and A.FeeCategory in ('" + sem + "')";
            }
            if (sec != "")
            {
                // selqry = selqry + " and ISNULL( r.Sections,'') in ('" + sec + "','')";
            }
            if (stud != "")
            {
                selqry = selqry + "and p.mode in ('" + stud + "')";
            }
            if (seatype != "")
            {
                selqry = selqry + "and  p.seattype in ('" + seatype + "')";
            }
            if (studtype != "")
            {
                selqry = selqry + "  and p.Stud_Type in ('" + studtype + "')";
            }
            if (headervalue != "")
            {
                selqry = selqry + " and Ex.HeaderFK in('" + headervalue + "')";
            }
            if (ledgervalue != "")
            {
                selqry = selqry + "  and Ex.LedgerFK in('" + ledgervalue + "')";
            }
            if (datewise == 1)
            {
                if (fromdate != "" && todate != "")
                {
                    selqry = selqry + "  AND PayStartDate between '" + fromdate + "' and '" + todate + "'";
                }
            }
            selqry = selqry + " GROUP BY A.HeaderFK ,h.LedgerPK,p.Batch_Year ,C.Course_Id,d.Dept_Code,A.FeeCategory";
            cmd = new SqlCommand(selqry);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(daload);
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    //public DataSet BindItemHeaderWithRights_inv()
    //{
    //    try
    //    {
    //        string itemname = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster where  ForHostelItem ='0'";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public DataSet BindItemCode_inv(string HeaderCode)
    //{
    //    try
    //    {
    //        string itemname = "select distinct itemcode  ,itemname   from IM_ItemMaster  where ItemHeaderCode  in ('" + HeaderCode + "') and ForHostelItem ='0' order by itemname";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public DataSet BindItemHeaderWithOutRights_inv()
    //{
    //    try
    //    {
    //        string itemname = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster order by ItemHeaderCode";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public DataSet BindItempkwithsubheader_inv(string HeaderCode, string subheadercode)
    //{
    //    try
    //    {
    //        string itemname = "select distinct itempk  ,itemname   from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')  and ForHostelItem ='0' order by itemname";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public DataSet BindSession_inv(string HostelCode)
    //{
    //    try
    //    {
    //        string itemname = "select SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in ('" + HostelCode + "')";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public DataSet BindHostel_inv()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelName ";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public string GetBuildingCode_inv(string HostelCode)
    //{
    //    string build = "";
    //    try
    //    {
    //        string itemname = "select HostelBuildingFK from HM_HostelMaster where HostelMasterPK IN ('" + HostelCode + "')";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string build1 = ds.Tables[0].Rows[i]["HostelBuildingFK"].ToString();
    //                if (build1.Trim() != "")
    //                {
    //                    if (build == "")
    //                    {
    //                        build = build1;
    //                    }
    //                    else
    //                    {
    //                        build = build + "," + build1;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //    return build;
    //}
    //public DataSet BindFloor_new(string buildingName)
    //{
    //    try
    //    {
    //        string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildingName + "')";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}
    //public string Gethostelcode_inv(string messname)
    //{
    //    string build = "";
    //    try
    //    {
    //        string itemname = "select distinct HostelMasterPK from HM_HostelMaster where MessMasterFK in('" + messname + "')";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(itemname, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string build1 = ds.Tables[0].Rows[i]["HostelMasterPK"].ToString();
    //                if (build1.Trim() != "")
    //                {
    //                    if (build == "")
    //                    {
    //                        build = build1;
    //                    }
    //                    else
    //                    {
    //                        build = build + "'" + "," + "'" + build1;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //    return build;
    //}

    /*New Hostel Inventory Barath  05.05.16*/
    public DataSet Bindmess_inv(string collegecode)
    {
        try
        {
            string selectQuery = "select MessMasterPK,MessName,MessAcr from HM_MessMaster where CollegeCode='" + collegecode + "' order by MessName asc";
            ds.Clear();
            ds = select_method_wo_parameter(selectQuery, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string getitempk(string itemcode)
    {
        string itempk1 = "";
        try
        {
            itempk1 = GetFunction("select itempk from IM_ItemMaster where itemcode='" + itemcode + "'");
        }
        catch
        {
        }
        return itempk1;
    }

    public string getvenpk(string vencode)
    {
        string itempk1 = "";
        try
        {
            itempk1 = GetFunction("select vendorpk from CO_VendorMaster where vendorcode='" + vencode + "'");
        }
        catch
        {
        }
        return itempk1;
    }

    public DataSet BindItemHeaderWithRights_inv()
    {
        try
        {
            string itemname = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster order by ItemHeaderName";//where  ForHostelItem ='0'
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemHeaderWithOutRights_inv()
    {
        try
        {
            string itemname = "select distinct ItemHeaderCode ,ItemHeaderName  from IM_ItemMaster order by ItemHeaderName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCode_inv(string HeaderCode)
    {
        try
        {
            string itemname = "select distinct itemcode  ,itemname   from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "')  order by itemname";//and ForHostelItem ='0'
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodewithsubheader_inv(string HeaderCode, string subheadercode)
    {
        try
        {
            string itemname = "select distinct itemcode  ,itemname   from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')   order by itemname";//and ForHostelItem ='0'
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeAll_inv(string HeaderCode)
    {
        try
        {
            string itemname = "select distinct itemcode  ,itemname   from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "') order by itemname";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeWithOutParameter_inv()
    {
        try
        {
            string itemname = "select distinct itemcode  ,itemname  from IM_ItemMaster  order by itemname";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindVendorName_inv()
    {
        try
        {
            string itemname = "select distinct VendorCode, VendorCompName from CO_VendorMaster where vendortype=1 order by VendorCode";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindState_inv(string Collegecode)
    {
        try
        {
            string itemname = " select mastercode,mastervalue from CO_MasterValues where mastercriteria='State' and CollegeCode ='" + Collegecode + "' order by MasterValue";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindDistrict_inv(string Collegecode)
    {
        try
        {
            string itemname = "select mastercode,mastervalue from CO_MasterValues where mastercriteria='District' and CollegeCode ='" + Collegecode + "' order by MasterValue";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodeWithOutParameterhostel_inv()
    {
        try
        {
            string itemname = "select distinct itemcode  ,itemname  from IM_ItemMaster  order by itemname";//where  ForHostelItem ='0' 
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItemCodewithsubheaderMaster_inv(string HeaderCode, string subheadercode)
    {
        try
        {
            string itemname = "select distinct ItemCode ,ItemName  from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')  order by ItemName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    //09.01.16
    public DataSet BindVendorNamevendorpk_inv()
    {
        try
        {
            string itemname = "select vendorcompname,VendorPK from CO_VendorMaster where vendortype=1 order by VendorCompName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindStore_inv(string collegecode)
    {
        try
        {
            string itemname = "select distinct StorePK,StoreName  from IM_StoreMaster where CollegeCode ='" + collegecode + "' order by StoreName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindItempkwithsubheader_inv(string HeaderCode, string subheadercode)
    {
        try
        {
            string itemname = "select distinct itempk  ,itemname   from IM_ItemMaster  where ItemHeaderCode in ('" + HeaderCode + "') and subheader_code in ('" + subheadercode + "')   order by itemname";//and ForHostelItem ='0'
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    //18.01.16
    //public DataSet Bindmess_inv(string collegecode)
    //{
    //    try
    //    {
    //        string selectQuery = "select MessMasterPK,MessName,MessAcr from HM_MessMaster where CollegeCode='" + collegecode + "' order by MessMasterPK asc";
    //        ds.Clear();
    //        ds = select_method_wo_parameter(selectQuery, "Text");
    //    }
    //    catch
    //    {
    //    }
    //    return ds;
    //}

    public string Gethostelcode_inv(string messname)
    {
        string build = "";
        try
        {
            string itemname = "select distinct HostelMasterPK from HM_HostelMaster where MessMasterFK in('" + messname + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string build1 = ds.Tables[0].Rows[i]["HostelMasterPK"].ToString();
                    if (build1.Trim() != "")
                    {
                        if (build == "")
                        {
                            build = build1;
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return build;
    }

    public DataSet BindSession_inv(string HostelCode)
    {
        try
        {
            string itemname = "select SessionMasterPK,SessionName from HM_SessionMaster where MessMasterFK in ('" + HostelCode + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public DataSet BindHostel_inv()
    {
        try
        {
            ds.Clear();
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster  order by HostelName ";//CollegeCode in ('" + college + "')
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string GetBuildingCode_inv(string HostelCode)
    {
        string build = "";
        try
        {
            string itemname = "select HostelBuildingFK from HM_HostelMaster where HostelMasterPK IN ('" + HostelCode + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string build1 = ds.Tables[0].Rows[i]["HostelBuildingFK"].ToString();
                    if (build1.Trim() != "")
                    {
                        if (build == "")
                        {
                            build = build1;
                        }
                        else
                        {
                            build = build + "," + build1;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return build;
    }

    public DataSet BindFloor_new(string buildingName)
    {
        try
        {
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildingName + "')";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string getappno(string RollNo)
    {
        string itempk1 = "";
        try
        {
            itempk1 = GetFunction("select App_No from Registration where Roll_No='" + RollNo + "'");
        }
        catch
        {
        }
        return itempk1;
    }

    //18.04.16 barath
    public DataSet Bindmess_basedonrights(string usercode, string collegecode)
    {
        try
        {
            string selectQuery = GetFunction("select value from Master_Settings where settings='Mess Rights'  and usercode='" + usercode + "' and value<>''");
            string selectQuery1 = "select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc";
            ds.Clear();
            ds = select_method_wo_parameter(selectQuery1, "Text");
        }
        catch
        {
        }
        return ds;
    }

    public string sendErrorMail(Exception e, string collcode, string pageName)
    {
        string sentMail = "Mail Not Sent";
        try
        {
            string userid = "palpaporange@gmail.com";
            string userpd = "palpap1234";
            string collegeName = GetFunction("select collname from collinfo  where college_code='" + collcode + "'").Trim() + "-" + DateTime.Now;
            System.Net.Mail.SmtpClient Mail = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage();
            System.Net.Mail.MailAddress mfrom = new System.Net.Mail.MailAddress(userid);
            mailmsg.From = mfrom;
            mailmsg.To.Add(userid);
            mailmsg.Subject = "Error from Try Catch";
            mailmsg.IsBodyHtml = true;
            mailmsg.Body = e.Message + " " + e.StackTrace + "<br>" + collegeName + "<br>" + pageName;
            Mail.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userid, userpd);
            Mail.UseDefaultCredentials = false;
            Mail.Credentials = credentials;
            Mail.Send(mailmsg);
            sentMail = "Mail Sent";
        }
        catch { }
        return sentMail;
    }

    public DataSet BindHostelbaseonmessrights_inv(string MessMasterFK)
    {
        try
        {
            ds.Clear();
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where MessMasterFK in (" + MessMasterFK + ") order by hostelname ";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        { }
        return ds;
    }

    public DataSet BindStorebaseonrights_inv(string storepk)
    {
        try
        {
            string itemname = "select distinct StorePK,StoreName  from IM_StoreMaster where StorePK in(" + storepk + ") order by StoreName";
            ds.Clear();
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        { }
        return ds;
    }

    public DataSet BindCollegebaseonrights(string usercode, byte isUser = 1)
    {
        try
        {
            string qryUserRights = string.Empty;
            ds.Clear();
            if (!string.IsNullOrEmpty(usercode))
            {
                if (isUser == 1)
                {
                    qryUserRights = " and user_code in('" + usercode + "') ";
                }
                else if (isUser == 0)
                {
                    string groupCode = string.Empty;
                    string[] groupUser = usercode.Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    else
                    {
                        groupCode = usercode;
                    }
                    qryUserRights = " and group_code in('" + groupCode + "') ";
                }
                else
                {
                    qryUserRights = string.Empty;
                }
            }

            string itemname = " select c.college_code,c.collname from collinfo c,collegeprivilages cp where c.college_code=cp.college_code  " + qryUserRights + " order by c.college_code ";
            ds = select_method_wo_parameter(itemname, "Text");
        }
        catch
        {
        }
        return ds;
    }

    //Added by Idhris -- 03-11-2016
    public int sendNewSMS(string DegreeCode, string collegecode, string usercode, string mobilenos, string message, string isstaff, string smsSendFor = null)
    {
        int smssend = 0;
        try
        {
            string SMSURL = GetSMSURL(collegecode, DegreeCode);
            //http://www.nethra91.com/api/smsapi.php?pinid1=78&un=VELMAIN&pass=91velmain123mno=9042774994&cod1=1&msg=test
            string Mes = HttpContext.Current.Server.UrlEncode(message);
            if (SMSURL.Length > 25)
            {
                string strpath = SMSURL + "&mno=" + mobilenos + "&cod1=1&msg=" + Mes + "";
                WebRequest request = WebRequest.Create(strpath);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader sr = new StreamReader(data);
                string strvel = sr.ReadToEnd();
                string groupmsgid = "";
                groupmsgid = strvel;
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                int sms = 0;
                string smsreportinsert = "";
                string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
                string[] spmessp = strvel.Split(new Char[] { ',' });
                for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
                {
                    smssend++;
                    string smsid = "";
                    if (spmessp.GetUpperBound(0) >= icount)
                    {
                        smsid = spmessp[icount].ToString();
                    }
                    string mobile = split_mobileno[icount].ToString();
                    smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id,smsFor)values( '" + mobile + "','" + smsid + "',N'" + message + "','" + collegecode + "','" + isstaff + "','" + date + "','" + usercode.ToString() + "'," + ((smsSendFor == null) ? "''" : "'" + smsSendFor + "'") + ")";
                    sms = update_method_wo_parameter(smsreportinsert, "Text");
                }
            }
        }
        catch
        {
        }
        return smssend;
    }

    public string GetSMSURL(string collegeCode, string DegreeCode)
    {
        string SMSURL = string.Empty;
        try
        {
            string selQuery = "SELECT SMSAPI_URL,USERID,SENDERID,PASSWORD FROM sms_mastersettings WHERE COLLEGECODE='" + collegeCode + "' AND DEGREECODE='" + DegreeCode + "'";
            DataSet dsUserDet = new DataSet();
            dsUserDet = select_method_wo_parameter(selQuery, "Text");
            if (dsUserDet.Tables.Count > 0 && dsUserDet.Tables[0].Rows.Count > 0)
            {
                string UserID = Convert.ToString(dsUserDet.Tables[0].Rows[0]["USERID"]).Trim();
                string SenderID = Convert.ToString(dsUserDet.Tables[0].Rows[0]["SENDERID"]).Trim();
                string SenderPasswd = Convert.ToString(dsUserDet.Tables[0].Rows[0]["PASSWORD"]).Trim();
                string SMSLink = Convert.ToString(dsUserDet.Tables[0].Rows[0]["SMSAPI_URL"]).Trim();
                SMSURL = SMSLink + "?pinid1=78&un=" + UserID + "&pass=" + SenderPasswd;
                // SMSURL = SMSLink + "?token=" + SenderID;
            }
        }
        catch { SMSURL = string.Empty; }
        return SMSURL;
    }

    public string GetSMSREPORTURL(string collegeCode, string DegreeCode) //Add By Aruna 7/7/2017
    {
        string SMSURL_RPT = string.Empty;
        try
        {
            string selQuery = "SELECT SmsreporterAPI_Url,USERID,SENDERID,PASSWORD FROM sms_mastersettings WHERE COLLEGECODE='" + collegeCode + "' AND DEGREECODE='" + DegreeCode + "'";
            DataSet dsUserDet = new DataSet();
            dsUserDet = select_method_wo_parameter(selQuery, "Text");
            if (dsUserDet.Tables.Count > 0 && dsUserDet.Tables[0].Rows.Count > 0)
            {
                string UserID = Convert.ToString(dsUserDet.Tables[0].Rows[0]["USERID"]).Trim();
                string SenderID = Convert.ToString(dsUserDet.Tables[0].Rows[0]["SENDERID"]).Trim();
                string SenderPasswd = Convert.ToString(dsUserDet.Tables[0].Rows[0]["PASSWORD"]).Trim();
                string SMSLink = Convert.ToString(dsUserDet.Tables[0].Rows[0]["SMSAPI_URL"]).Trim();
                SMSURL_RPT = SMSLink + "?pinid1=78&un=" + UserID + "&pass=" + SenderPasswd;
                // SMSURL = SMSLink + "?token=" + SenderID;
            }
        }
        catch { SMSURL_RPT = string.Empty; }
        return SMSURL_RPT;
    }
    //Last Modified by Idhris -- 31-12-2016
    private string fetDegcode = string.Empty;

    public string featDegreeCode
    {
        get { return fetDegcode; }
        set { fetDegcode = value; }
    }

    //public DataSet loadFeecategory(string collegecode, string usercode, ref string linkName)
    //{
    //    DataSet dsset = new DataSet();
    //    try
    //    {
    //        string linkValue = string.Empty;
    //        string SelectQ = string.Empty;
    //        linkValue = GetFunction("select LinkValue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
    //        if (!string.IsNullOrEmpty(linkValue) && linkValue != "0")
    //        {
    //            SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //            dsset.Clear();
    //            dsset = select_method_wo_parameter(SelectQ, "Text");
    //            linkName = "SemesterandYear";
    //        }
    //        else
    //        {
    //            linkValue = GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
    //            if (!string.IsNullOrEmpty(linkValue) && linkValue == "0")
    //            {
    //                SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                dsset.Clear();
    //                dsset = select_method_wo_parameter(SelectQ, "Text");
    //                linkName = "Semester";
    //            }
    //            else if (!string.IsNullOrEmpty(linkValue) && linkValue == "1")
    //            {
    //                SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                dsset.Clear();
    //                dsset = select_method_wo_parameter(SelectQ, "Text");
    //                linkName = "Year";
    //            }
    //            else if (!string.IsNullOrEmpty(linkValue) && linkValue == "2")
    //            {
    //                // SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Term%' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                SelectQ = "select distinct textval,TextCode,len(textval) from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term%' and textval not like '-1%' and t.college_code ='" + collegecode + "' ";
    //                if (!string.IsNullOrEmpty(featDegreeCode))
    //                    SelectQ += "  and f.degree_code in('" + featDegreeCode + "') ";
    //                SelectQ += " order by len(textval),textval asc";
    //                dsset.Clear();
    //                dsset = select_method_wo_parameter(SelectQ, "Text");
    //                linkName = "Term";
    //            }
    //        }
    //    }
    //    catch { dsset.Clear(); }
    //    return dsset;
    //}
    public DataSet loadFeecategory(string collegecode, string usercode, ref string linkName)
    {
        DataSet dsset = new DataSet();
        try
        {

            //string linkName = string.Empty;
            string linkValue = string.Empty;
            string SelectQ = string.Empty;
            string strVal = string.Empty;
            linkValue = "select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' Order By LinkValue";
            DataSet dsVal = select_method_wo_parameter(linkValue, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    double linkVal = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["LinkValue"]), out linkVal);
                    switch (Convert.ToString(linkVal))
                    {
                        case "0":
                            strVal = " textval like '%Semester'";
                            linkName = "Semester";
                            break;
                        case "1":
                            if (strVal == string.Empty)
                            {
                                strVal = " textval like '%Year'";
                                linkName = "Year";
                            }

                            else
                            {
                                strVal += " or textval like '%Year'";
                                linkName = "Year";
                            }
                            break;
                        case "2":
                            if (strVal == string.Empty)
                            {
                                strVal = "  textval like  '%Term%'";
                                linkName = "Term";
                            }
                            else
                            {
                                strVal += " or textval like '%Term%'";
                                linkName = "Term";
                            }
                            break;
                        default:
                            strVal = " textval like '%Semester'";
                            linkName = "Semester";
                            break;
                    }
                }
            }
            SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (" + strVal + ") and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
            dsset = select_method_wo_parameter(SelectQ, "Text");

        }
        catch { dsset.Clear(); }

        return dsset;
    }
    //Function added by Idhris 16-12-2016
    /// <summary>
    /// To get Current selected financial year for a particular user and returns empty when not set
    /// </summary>
    /// <param name="userCode">Usercode of the login user</param>
    /// <param name="collegeCode">College code of the login user</param>
    /// <returns>Current selected financial year</returns>
    public string getCurrentFinanceYear(string userCode, string collegeCode)
    {
        string value = string.Empty;
        string ddCollected = "select LinkValue from InsSettings where LinkName='Current Financial Year' and  FinuserCode ='" + userCode + "' and college_code ='" + collegeCode + "'";
        value = GetFunction(ddCollected).Trim();
        value = value == "0" ? string.Empty : value;
        return value;
    }

    public void BindPaymodeToDropDown(DropDownList ddlpaymode, string usercode, string collegecode)
    {
        try
        {
            int inclpayRights = 0;
            string payValue = string.Empty;
            Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
            inclpayRights = paymodeRightsCheck(usercode, collegecode, ref  payValue);
            if (inclpayRights == 1 && payValue != "0")
            {
                string[] splvalue = payValue.Split(',');
                if (splvalue.Length > 0)
                {
                    dtpaymode = dtPaymodeValue();
                    for (int row = 0; row < splvalue.Length; row++)
                    {
                        if (dtpaymode.ContainsKey(Convert.ToInt32(splvalue[row])))
                        {
                            string modestr = dtpaymode[Convert.ToInt32(splvalue[row])];
                            ddlpaymode.Items.Add(new System.Web.UI.WebControls.ListItem(modestr, Convert.ToString(splvalue[row])));
                        }
                    }
                }
            }
            else
                ddlpaymode.Items.Clear();
        }
        catch { ddlpaymode.Items.Clear(); }
    }

    public void BindPaymodeToCheckboxList(CheckBoxList cblpaymode, string usercode, string collegecode)
    {
        try
        {
            int inclpayRights = 0;
            string payValue = string.Empty;
            Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
            inclpayRights = paymodeRightsCheck(usercode, collegecode, ref  payValue);
            if (inclpayRights == 1 && payValue != "0")
            {
                string[] splvalue = payValue.Split(',');
                if (splvalue.Length > 0)
                {
                    dtpaymode = dtPaymodeValue();
                    for (int row = 0; row < splvalue.Length; row++)
                    {
                        if (dtpaymode.ContainsKey(Convert.ToInt32(splvalue[row])))
                        {
                            string modestr = dtpaymode[Convert.ToInt32(splvalue[row])];
                            cblpaymode.Items.Add(new System.Web.UI.WebControls.ListItem(modestr, Convert.ToString(splvalue[row])));
                        }
                    }
                }
            }
            else
                cblpaymode.Items.Clear();
        }
        catch { cblpaymode.Items.Clear(); }
    }

    //private int paymodeRightsCheck(string usercode, string collegecode, ref string payValue)
    //{
    //    int paymodRghts = 0;
    //    Int32.TryParse(Convert.ToString(GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettings' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'")), out paymodRghts);
    //    if (paymodRghts == 1)
    //    {
    //        payValue = Convert.ToString(GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettingsValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' "));
    //    }
    //    return paymodRghts;
    //}

    private int paymodeRightsCheck(string usercode, string collegecode, ref string payValue)
    {
        int paymodRghts = 0;
        string selQ = "select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettings' and user_code ='" + usercode + "' ";
        if (!collegecode.Trim().Contains(","))
            selQ += " and college_code ='" + collegecode + "'";

        Int32.TryParse(Convert.ToString(GetFunction(selQ)), out paymodRghts);
        if (paymodRghts == 1)
        {
            string selVal = " select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettingsValue' and user_code ='" + usercode + "'";
            if (!collegecode.Trim().Contains(","))
                selVal += " and college_code ='" + collegecode + "'";
            payValue = Convert.ToString(GetFunction(selVal));
        }
        return paymodRghts;
    }

    private Dictionary<int, string> dtPaymodeValue()
    {
        Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
        dtpaymode.Add(1, "Cash");
        dtpaymode.Add(2, "Cheque");
        dtpaymode.Add(3, "DD");
        dtpaymode.Add(4, "Challan");
        dtpaymode.Add(5, "Online");
        dtpaymode.Add(6, "Card");
        dtpaymode.Add(7, "NEFT");
        return dtpaymode;
    }
    public int update_method_with_parameter(string sqlcmd, Hashtable htt, string sptype)
    {
        try
        {
            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection();
            foreach (DictionaryEntry parameter in htt)
            {
                cmd.Parameters.Add((string)parameter.Key, parameter.Value);
            }
            i = cmd.ExecuteNonQuery();
            return i;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public DataSet BindBranchMultiples(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            if (course_id.ToString().Trim() != "")
            {
                if (singleuser == "True")
                {
                    daload.Dispose();
                    daload.Reset();
                    string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + course_id + "') and degree.college_code in('" + collegecode + "')  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
                    cmd = new SqlCommand(strquery);
                    cmd.Connection = connection.CreateConnection();
                    adaload = new SqlDataAdapter(cmd);
                    adaload.Fill(daload);
                }
                else
                {
                    daload.Dispose();
                    daload.Reset();
                    string strquery1 = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + course_id + "') and degree.college_code in('" + collegecode + "')  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                    cmd = new SqlCommand(strquery1);
                    cmd.Connection = connection.CreateConnection();
                    adaload = new SqlDataAdapter(cmd);
                    adaload.Fill(daload);
                }
            }
            return daload;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close();
        }
    }

    public string getUserCode(string groupCode, string userCode, byte type)
    {
        string strUser = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(groupCode) && groupCode != "-1" && groupCode != "0")
            {
                strUser = " group_code='" + groupCode + "'";
            }
            else
            {
                strUser = " user_code='" + userCode + "'";
            }
        }
        catch
        {
        }
        return strUser;
    }


    public void insertUserActionLog(string entryCode, string batchYear, string degreeCode, string semester, string section, string TimeOfAttendance, string DateOfAttendence, string PageName, string ctsname, int UsrAction)
    {
        try
        {

            if (section != "" && section.Trim().ToLower() != "all" && section != null && section != "0")
            {
                section = ":Sections -" + section;
            }
            string version = Convert.ToString(System.Web.HttpContext.Current.Session["version"]);// added by madhumathi
            string details = "" + degreeCode + ":Sem - " + semester + ":Batch Year -" + batchYear + " " + section + "";
            string modules = "0";
            string act_diff = " ";
            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam,version) values ('" + entryCode + "','" + PageName + "','" + UsrAction + "','" + TimeOfAttendance + "','" + DateOfAttendence + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "','" + version + "')";// added by madhumathi(version)
            int a = update_method_wo_parameter(strlogdetails, "Text");
        }
        catch
        {

        }
    }


    //Added By SaranyaDevi20.1.2018 
    #region Common Checkbox and Checkboxlist Event

    public string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    public string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }

    public void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch

        { }
    }

    public void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch
        {

        }
    }

    #endregion


    //========Added by saranya on 11/04/2018==================//

    public void insertEinanceUserActionLog(string entryCode, string PageName, int UsrAction, string TimeOfAttendance, string DateOfAttendence, string details, string ctsname, string Ip_Address)
    {
        try
        {
            string modules = "0";
            string act_diff = " ";
            string version = Convert.ToString(System.Web.HttpContext.Current.Session["version"]);// added by madhumathi
            if (Ip_Address == "")
            {
                Ip_Address = "0";
            }
            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam,IpAddress,version) values ('" + entryCode + "','" + PageName + "','" + UsrAction + "','" + TimeOfAttendance + "','" + DateOfAttendence + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "','" + Ip_Address + "','" + version + "')"; // added by madhumathi(version)
            int a = update_method_wo_parameter(strlogdetails, "Text");
        }
        catch
        {

        }
    }
    public string numberformat(string amt)//added by abarna on 13.04.2018
    {
        double separatecomma = 0;
        double.TryParse(amt, out separatecomma);
        amt = string.Format("{0:0,0}", separatecomma);

        return amt;
    }

    public void userTimeOut(string entryCode) //Rajkumar 31-may-2018
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        int a = update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + entryCode + "'", "Text");
    }
    public DataSet select_method_wo_parameter_Biometric(string sqlcmd, string sptype)
    {
        DataSet ds = new DataSet();
        try
        {

            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection_Biometric();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close_Biometric();
        }
        return ds;
    }
    public int update_method_wo_parameter_Biometric(string sqlcmd, string sptype)
    {
        try
        {
            cmd = new SqlCommand(sqlcmd);
            cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); //Aruna 31/May/2017
            if (sptype == "sp")
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }
            cmd.Connection = connection.CreateConnection_Biometric();
            i = cmd.ExecuteNonQuery();
            return i;
        }
        catch (SqlException ex)
        {
            throw ex;
        }
        finally
        {
            connection.Close_Biometric();
        }
    }

    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        try
        {
            string Day_Order = "";
            int holiday = 0;
            if (no_days == "")
                return "";
            string start_date = "";
            if (sdate != "")
            {
                DateTime dt3 = Convert.ToDateTime(curday);

                DataSet nextda = select_method_wo_parameter("select top 1 From_Date,DayOrder from tbl_consider_day_order where Degree_code='" + deg_code + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and From_Date<'" + dt3.ToString("yyyy-MM-dd") + "'and asperday='3' order by from_date desc", "text");
                if (nextda.Tables[0].Rows.Count > 0 && nextda.Tables.Count > 0)
                {
                    foreach (DataRow row in nextda.Tables[0].Rows)
                    {

                        start_date = Convert.ToString(row["From_Date"]);
                        stastdayorder = Convert.ToString(row["DayOrder"]);

                    }
                }
                else
                    start_date = sdate;
                DateTime dt1 = Convert.ToDateTime(start_date);
                DateTime dt2 = Convert.ToDateTime(curday);
                string currentdate = dt1.ToString("MM/dd/yyyy");
                string startdate = dt2.ToString("MM/dd/yyyy");
                dt1 = Convert.ToDateTime(currentdate);
                dt2 = Convert.ToDateTime(startdate);
                TimeSpan ts = dt2 - dt1;
                //rajkumar9feb2018 string query1 = "select count(*) as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
                string query1 = "select count(distinct holiday_date)  as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";
                //====================

                DateTime holidaychk = new DateTime();
                holidaychk = dt2.AddDays(-1);
                string doublecun1 = GetFunction("select COUNT (distinct doubledate) from doubledayorder where doubledate ='" + holidaychk + "' and batchYear='" + batch_year + "' and degreecode='" + deg_code + "'");
                int dif_days = ts.Days;

                string holday = GetFunction(query1);
                if (stastdayorder.ToString().Trim() == "0")
                {
                    if (doublecun1 != "" && doublecun1 != "0")
                    {
                        if (holday != "")
                        {
                            // holday = Convert.ToString(Convert.ToInt32(holday) - 1);
                            //  dif_days = dif_days + 1; 

                        }
                    }
                }
                if (holday != "")
                    holiday = Convert.ToInt32(holday);
                //int dif_days = ts.Days;
                string leave = GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
                if (leave != null && leave != "0")
                {
                    dif_days = dif_days + 1;
                }
                int dayorderchangedate = 0;
                try
                {
                    string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "')) ;";
                    strdayorder = strdayorder + " select CONVERT(nvarchar(15),holiday_date,101) as hdate from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date<='" + dt2.ToString("yyyy-MM-dd") + "' ";
                    DataSet dsdayorderchange = select_method_wo_parameter(strdayorder, "Text");
                    if (dsdayorderchange.Tables[0].Rows.Count > 0)
                    {
                        Hashtable hatholidc = new Hashtable();
                        for (int hda = 0; hda < dsdayorderchange.Tables[1].Rows.Count; hda++)
                        {
                            string hdater = dsdayorderchange.Tables[1].Rows[hda]["hdate"].ToString();
                            if (!hatholidc.Contains(hdater))
                            {
                                hatholidc.Add(hdater, hdater);
                            }
                        }
                        string strdayorders = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "'  ;";//and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'))
                        DataSet aspday = select_method_wo_parameter(strdayorders, "Text");
                        for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
                        {
                            DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
                            DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
                            for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
                            {
                                if (!hatholidc.Contains(dtdcst.ToString("MM/dd/yyyy")))
                                {
                                    if (dtdcst <= dt2)
                                    {


                                        string assper = Convert.ToString(dsdayorderchange.Tables[0].Rows[doc]["asperday"]);
                                        //string nextday = Convert.ToString(dsdayorderchange.Tables[0].Rows[doc]["nextday"]);
                                        //string skipday = Convert.ToString(dsdayorderchange.Tables[0].Rows[doc]["skipday"]);

                                        if (assper == "1")
                                        {
                                        }
                                        else if (assper == "3")
                                        {
                                        }
                                        else
                                        {
                                            dayorderchangedate = dayorderchangedate + 1;
                                        }
                                        //else if (nextday == "1")
                                        //{
                                        //    dayorderchangedate = dayorderchangedate + 1;
                                        //}
                                        //else
                                        //{

                                        //    dayorderchangedate = dayorderchangedate + 1;
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    holiday = holiday + dayorderchangedate;
                }
                catch
                {
                }
                int nodays = Convert.ToInt32(no_days);

                //magesh 6.8.18
                int dayss = 1;
                DateTime cur_day = new DateTime();
                cur_day = dt2.AddDays(-dayss);
                string chkdoubleday = GetFunction("select * from doubledayorder where doubleDate='" + cur_day + "' and batchYear='" + batch_year + "' and degreecode='" + deg_code + "'");
                if (chkdoubleday != "" && chkdoubleday != "0")
                {
                    if (Convert.ToString(System.Web.HttpContext.Current.Session["doubledayshk"]) == "false")
                    {
                        doubleday = true;

                    }
                    else
                    {
                        doubleday = false;

                    }
                }
                else
                {
                    doubleday = false;

                }


                string doublecun = GetFunction("select COUNT (distinct doubledate) from doubledayorder where  batchYear='" + batch_year + "' and degreecode='" + deg_code + "' and doubledate between '" + currentdate + "' and '" + cur_day + "' and doubledate<'" + cur_day + "' ");
                int cundouble = 0;
                int order = 0;
                int.TryParse(doublecun, out cundouble);
                // if (stastdayorder.ToString().Trim() == "0")
                if (Convert.ToString(System.Web.HttpContext.Current.Session["SchOrderdouble"]) == "0")//magesh 17.9.18
                {
                    if (doubleday == true)
                        cundouble = cundouble + 1;
                    order = ((dif_days - holiday) + cundouble) % nodays;
                }
                else
                    order = (dif_days - holiday) % nodays;
                order = order + 1;
                if (stastdayorder.ToString().Trim() != "")
                {
                    if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                    {
                        order = order + (Convert.ToInt16(stastdayorder) - 1);
                        if (order == (nodays + 1))
                            order = 1;
                        else if (order > nodays)
                            order = order % nodays;
                    }
                }
                if (order.ToString() == "0")
                {
                    order = Convert.ToInt32(no_days);
                }
                string finddayorder = "";
                if (order == 1)
                    finddayorder = "mon";
                else if (order == 2) finddayorder = "tue";
                else if (order == 3) finddayorder = "wed";
                else if (order == 4) finddayorder = "thu";
                else if (order == 5) finddayorder = "fri";
                else if (order == 6) finddayorder = "sat";
                else if (order == 7) finddayorder = "sun";
                if (order >= 1)
                {
                    Day_Order = Convert.ToString(order) + "-" + Convert.ToString(finddayorder);
                }
                else
                {
                    Day_Order = "";
                }
                return finddayorder;
            }
            else
                return "";
        }
        catch
        {
            return "";
        }
    }
    public string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "mon";
                break;
            case 2:
                dayName = "tue";
                break;
            case 3:
                dayName = "wed";
                break;
            case 4:
                dayName = "thu";
                break;
            case 5:
                dayName = "fri";
                break;
            case 6:
                dayName = "sat";
                break;
            case 7:
                dayName = "sun";
                break;
            default:
                break;
        }
        return dayName;
    }

    public void printexcelreportgrid(GridView excelprintgrid, string ExcelreportName)
    {
        string print = "";
        string appPath = HttpContext.Current.Server.MapPath("~");
        string strexcelname = "";
        if (appPath != "")
        {
            string date = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss_tt");
            strexcelname = ExcelreportName.ToString().Trim() + '_' + date.Trim();
            appPath = appPath.Replace("\\", "/");
            if (strexcelname != "")
            {
                print = strexcelname;
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                System.Web.HttpContext.Current.Response.ClearContent();
                System.Web.HttpContext.Current.Response.ClearHeaders();
                System.Web.HttpContext.Current.Response.Charset = "";
                appPath = appPath.Replace("\\", "/");
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + szPath + szFile);
                excelprintgrid.GridLines = GridLines.Both;
                excelprintgrid.HeaderStyle.Font.Bold = true;
                excelprintgrid.RenderControl(htmltextwrtter);
                System.Web.HttpContext.Current.Response.Write(strwritter.ToString());
                System.Web.HttpContext.Current.Response.End();
            }
        }
    }
    public string ServerDate()
    {
        string serverdate = "";
        DAccess2 d2 = new DAccess2();
        string qry = "SELECT distinct cast(datepart(m,getdate()) as nvarchar) + '/' + cast(datepart(d,getdate()) as nvarchar) + '/' + cast(datepart(yyyy,getdate()) as nvarchar) as Date ,cast(datepart(hh,getdate()) as nvarchar) + ':' + cast(datepart(n,getdate()) as nvarchar) + ':' + cast(datepart(s,getdate()) as nvarchar)";
        DataSet dsdate = new DataSet();
        dsdate = d2.select_method_wo_parameter(qry, "text");
        string dtDate = Convert.ToString(dsdate.Tables[0].Rows[0]["Date"]);
        serverdate = dtDate;
        return serverdate;
    }

    public string ServerTime()
    {
        string servertime = "";
        DAccess2 d2 = new DAccess2();
        string qry = "SELECT distinct cast(datepart(m,getdate()) as nvarchar) + '/' + cast(datepart(d,getdate()) as nvarchar) + '/' + cast(datepart(yyyy,getdate()) as nvarchar) as Date ,cast(datepart(hh,getdate()) as nvarchar) + ':' + cast(datepart(n,getdate()) as nvarchar) + ':' + cast(datepart(s,getdate()) as nvarchar) as time";
        DataSet dsdate = new DataSet();
        dsdate = d2.select_method_wo_parameter(qry, "text");
        string dttime = Convert.ToString(dsdate.Tables[0].Rows[0]["time"]);
        servertime = dttime;
        return servertime;
    }
}