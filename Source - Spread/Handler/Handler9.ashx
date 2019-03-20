<%@ WebHandler Language="C#" Class="Handler9" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Handler9 : IHttpHandler 
{

    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest(HttpContext context)
    {
        string appno = "";

        appno = context.Request.QueryString["id"].ToString();

        MemoryStream memoryStream = new MemoryStream();

        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select g_photo from stdphoto where app_no='" + appno + "'";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows == true)
        {
            try
            {
                if (MyReader["g_photo"] != "" || MyReader["g_photo"] != null)
                {
                    byte[] file = (byte[])MyReader["g_photo"];

                    MyReader.Close();

                    connection.Close();

                    memoryStream.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream.Dispose();
                }
            }
            catch
            {
                
            }

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