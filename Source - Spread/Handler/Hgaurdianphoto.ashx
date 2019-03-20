<%@ WebHandler Language="C#" Class="Hgaurdianphoto" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Hgaurdianphoto : IHttpHandler 
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string appno_gaurdian = "";


            appno_gaurdian = context.Request.QueryString["id"].ToString();
            if (appno_gaurdian != "")
            {
                MemoryStream memoryStream_gaurdian = new MemoryStream();

                SqlConnection conn = new SqlConnection(GetConnectionString());
                SqlCommand cmd_gaurdian = new SqlCommand();
                cmd_gaurdian.CommandText = "select g_photo from stdphoto where app_no='" + appno_gaurdian + "'";
                cmd_gaurdian.Connection = conn;
                conn.Open();
                SqlDataReader MyReader_gaurdian = cmd_gaurdian.ExecuteReader();
                if (MyReader_gaurdian.Read())
                {
                    if (MyReader_gaurdian["g_photo"] != null)
                    {
                        byte[] file = (byte[])MyReader_gaurdian["g_photo"];
                        MyReader_gaurdian.Close();

                        conn.Close();

                        memoryStream_gaurdian.Write(file, 0, file.Length);

                        context.Response.Buffer = true;

                        context.Response.BinaryWrite(file);

                        memoryStream_gaurdian.Dispose();

                    }
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