using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DalConnection;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
//using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
/// <summary>
/// Summary description for ClsAttendanceAccess
/// </summary>
/// 
namespace BalAccess
{
    public class ClsAttendanceAccess
    {
        public ClsAttendanceAccess()
        {
           
        }

        #region Attendance

        public static DataSet GetBatchDetail()
        {
            SqlConnection con = ClsConnection.Getconnection();            
            SqlCommand cmd = new SqlCommand("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''order by batch_year", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
           
            da.Fill(ds);
            return ds;
            
        }

        public static DataSet GetDegreeDetail(string collegecode)
        {
            SqlConnection con = ClsConnection.Getconnection();           
            SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode.ToString() + " order by course.course_name ",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet GetBranchDetail(string branch,string collegecode)
        {
            SqlConnection con = ClsConnection.Getconnection();           
            SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " +branch.ToString() + " and degree.college_code= " +collegecode.ToString() + " ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet Getsemster_Detail(string batch_calcode_degree)
        {
            string[] splitval=batch_calcode_degree.Split(new char[] { '/' } );
            DataSet ds1 = new DataSet();
            SqlConnection con = ClsConnection.Getconnection();
            //SqlCommand cmd = new SqlCommand("BatchDetail");
            //cmd.Parameters.Add("@type", SqlDbType.Int).Value = Convert.ToInt32(b);
            //cmd.Parameters.Add("@batch_year", SqlDbType.Int).Value = Convert.ToInt32(c);
            //cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(d);
            //cmd.Parameters.Add("@college_code", SqlDbType.Int).Value = Convert.ToInt32(e);
            //cmd.Parameters.Add("@user_code", SqlDbType.Int).Value = Convert.ToInt32(null);
            //cmd.Parameters.Add("@Courseid", SqlDbType.Int).Value = Convert.ToInt32(null);    
            //cmd.Connection = con;
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code = " + splitval[2].ToString() + " and batch_year = " + splitval[0].ToString() + " and college_code = " + splitval[1].ToString() + " ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds1);
            if (ds1.Tables[0].Rows.Count ==0)
            {
                ds1.Clear();
                SqlCommand cmd1 = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code = " + splitval[2].ToString() + " and college_code =" + splitval[1].ToString() + "", con);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1,batch_calcode_degree);
            }
            return ds1;
        }

        //public static DataSet GetsectionDetail(string b, string c, string d, string e)
        //{
        //    SqlConnection con = ClsConnection.Getconnection();
        //    SqlCommand cmd = new SqlCommand("BatchDetail");
        //    cmd.Parameters.Add("@type", SqlDbType.Int).Value = Convert.ToInt32(b);
        //    cmd.Parameters.Add("@batch_year", SqlDbType.Int).Value = Convert.ToInt32(c);
        //    cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(d);
        //    cmd.Parameters.Add("@college_code", SqlDbType.Int).Value = Convert.ToInt32(e);
        //    cmd.Parameters.Add("@user_code", SqlDbType.Int).Value = Convert.ToInt32(null);
        //    cmd.Parameters.Add("@Courseid", SqlDbType.Int).Value = Convert.ToInt32(null);
        //    cmd.Connection = con;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}

        public static DataSet GetsectionDetail(string batch, string branch)
        {
            
            SqlConnection con = ClsConnection.Getconnection();
            //SqlCommand cmd = new SqlCommand("selectsection");
            //cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(b);
            //cmd.Parameters.Add("@batch_year", SqlDbType.Int).Value = Convert.ToInt32(c);
            //cmd.Parameters.Add("@semster", SqlDbType.Int).Value = Convert.ToInt32(d);
            //cmd.Connection = con;
            //cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("select distinct sections from registration where batch_year=" + batch.ToString() + " and degree_code=" + branch.ToString() + " and delflag=0 and exam_flag<>'Debar'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public static DataSet GetstudentDetails(string b, string c, string d, string e)
        {
            SqlConnection con = ClsConnection.Getconnection();
            SqlCommand cmd = new SqlCommand("GetSection");
          
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = Convert.ToInt32(b);
            cmd.Parameters.Add("@batch_year", SqlDbType.Int).Value = Convert.ToInt32(c);
            cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(d);
            cmd.Parameters.Add("@current_semester", SqlDbType.Int).Value = Convert.ToInt32(e);
            cmd.Parameters.Add("@sections", SqlDbType.Int).Value = Convert.ToInt32(null);
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        public static DataSet GetstudentDetail(string b, string c, string d, string e, string f)
        {
            SqlConnection con = ClsConnection.Getconnection();
            SqlCommand cmd = new SqlCommand("GetSection");
           
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = Convert.ToInt32(b);
            cmd.Parameters.Add("@batch_year", SqlDbType.Int).Value = Convert.ToInt32(c);
            cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(d);
            cmd.Parameters.Add("@current_semester", SqlDbType.Int).Value = Convert.ToInt32(e);         
            cmd.Parameters.Add("@sections", SqlDbType.VarChar,10).Value = f.ToString();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;

        }

        public static DataSet Getdatadatewise(string a,string b)
        {
            SqlConnection con = ClsConnection.Getconnection();
            SqlCommand cmd = new SqlCommand("FilteredSemsterResult");
            cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(a);
            cmd.Parameters.Add("@semester", SqlDbType.Int).Value = Convert.ToInt32(b);
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public static DataSet Getdatadatewised(string a, string b,DateTime c,DateTime d)
        {
            SqlConnection con = ClsConnection.Getconnection();
            SqlCommand cmd = new SqlCommand("FilteredSemsterRes");
            cmd.Parameters.Add("@degree_code", SqlDbType.Int).Value = Convert.ToInt32(a);
            cmd.Parameters.Add("@semester", SqlDbType.Int).Value = Convert.ToInt32(b);
            cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = c;
            cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = d;
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        #endregion
    }
}