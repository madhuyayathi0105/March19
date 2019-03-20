<%@ WebHandler Language="C#" Class="Rightlogo" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Rightlogo : IHttpHandler
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
            cmd.CommandText = "select logo2 from collinfo  where college_code='" + collegecode + "'";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader MyReader = cmd.ExecuteReader();
            if (MyReader.Read())
            {

                byte[] file = (byte[])MyReader["logo2"];
                MyReader.Close();

                connection.Close();

                memoryStream.Write(file, 0, file.Length);
                if (file.Length > 0)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, false, false);
                    System.Drawing.Image thumb = img.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
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