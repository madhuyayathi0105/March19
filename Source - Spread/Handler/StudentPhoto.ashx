<%@ WebHandler Language="C#" Class="StudentPhoto" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mail;
using System.Xml;


public class StudentPhoto : IHttpHandler
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest(HttpContext context)
    {


        MemoryStream memoryStream = new MemoryStream();

        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        string roll = context.Request.QueryString["rollno"].ToString();
        cmd.CommandText = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + roll + "')";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        if (MyReader.Read())
        {

            byte[] file = (byte[])MyReader["photo"];
            MyReader.Close();

            connection.Close();

                       
            memoryStream.Write(file, 0, file.Length);
            if (file.Length > 0)
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, false, false);

                img.Save(HttpContext.Current.Server.MapPath("~/college/Student_Photo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            context.Response.Buffer = true;

            context.Response.BinaryWrite(file);

            memoryStream.Dispose();
        }



    }




    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

   
}

 