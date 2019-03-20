<%@ WebHandler Language="C#" Class="Hmotherphoto" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Data;

public class Hmotherphoto : IHttpHandler 
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest (HttpContext context) 
    {
        try
        {
            string appno_mother = "";

            appno_mother = context.Request.QueryString["id"].ToString();
            MemoryStream memoryStream_mother = new MemoryStream();

            SqlConnection conn_mother = new SqlConnection(GetConnectionString());
            SqlCommand cmd_mother = new SqlCommand();
            cmd_mother.CommandText = "select m_photo from stdphoto where app_no='" + appno_mother + "'";
            cmd_mother.Connection = conn_mother;
            conn_mother.Open();
            SqlDataReader MyReader_mother = cmd_mother.ExecuteReader();
            if (MyReader_mother.Read())
            {
                if (MyReader_mother["m_photo"] != null)
                {
                    byte[] file = (byte[])MyReader_mother["m_photo"];
                    MyReader_mother.Close();

                    conn_mother.Close();

                    memoryStream_mother.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream_mother.Dispose();

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