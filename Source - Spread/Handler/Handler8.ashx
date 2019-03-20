<%@ WebHandler Language="C#" Class="Handler8" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Handler8 : IHttpHandler 
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
        cmd.CommandText = "select m_photo from stdphoto where app_no='" + appno + "'";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows == true)
        {
            try
            {
                if (MyReader["m_photo"] != "" || MyReader["m_photo"] != null)
                {
                    byte[] file = (byte[])MyReader["m_photo"];

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