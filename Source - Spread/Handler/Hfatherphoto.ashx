<%@ WebHandler Language="C#" Class="Hfatherphoto" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Hfatherphoto : IHttpHandler
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string appno_father = "";

            appno_father = context.Request.QueryString["id"].ToString();
            MemoryStream memoryStream_father = new MemoryStream();

            SqlConnection conn_father = new SqlConnection(GetConnectionString());
            SqlCommand cmd_father = new SqlCommand();
            cmd_father.CommandText = "select f_photo from stdphoto where app_no='" + appno_father + "'";
            cmd_father.Connection = conn_father;
            conn_father.Open();
            SqlDataReader MyReader_father = cmd_father.ExecuteReader();
            if (MyReader_father.Read())
            {
                if (MyReader_father["f_photo"] != null)
                {
                    byte[] file = (byte[])MyReader_father["f_photo"];
                    MyReader_father.Close();

                    conn_father.Close();

                    memoryStream_father.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream_father.Dispose();

                }
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