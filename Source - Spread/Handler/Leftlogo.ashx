<%@ WebHandler Language="C#" Class="Leftlogo" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using System;
using System.Web.Mail;
using System.Xml;
using System.IO;
using System.Data.SqlClient;

public class Leftlogo : IHttpHandler
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string collegecode = "";
            collegecode = context.Request.QueryString["id"].ToString();
            MemoryStream memoryStream = new MemoryStream();
            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select logo1 from collinfo  where college_code='" + collegecode + "'";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader MyReader = cmd.ExecuteReader();
            if (MyReader.Read())
            {

                byte[] file = (byte[])MyReader["logo1"];
                MyReader.Close();

                connection.Close();

                memoryStream.Write(file, 0, file.Length);
                if (file.Length > 0)
                {
                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                }
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