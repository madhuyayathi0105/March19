<%@ WebHandler Language="C#" Class="Handler6" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Handler6 : IHttpHandler
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();
    }
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            MemoryStream memoryStream = new MemoryStream();
            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select  top 1 banner from collinfo where  banner is not null";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader MyReader = cmd.ExecuteReader();
            MyReader.Read();
            if (MyReader.HasRows == true)
            {
                byte[] file = (byte[])MyReader["banner"];
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


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}