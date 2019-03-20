<%@ WebHandler Language="C#" Class="principalHandler" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class principalHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest (HttpContext context) 
    {
        MemoryStream memoryStream = new MemoryStream();

       
            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select principal_sign from collinfo";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader MyReader = cmd.ExecuteReader();
            if (MyReader.Read())
            {
                if (MyReader["principal_sign"].ToString() != string.Empty)
                {

                    byte[] file = (byte[])MyReader["principal_sign"];
                    MyReader.Close();

                    connection.Close();

                    memoryStream.Write(file, 0, file.Length);

                    //if (file.Length > 0)
                    //{
                    //    System.Drawing.Image img = System.Drawing.Image.FromStream(memoryStream, false, false);

                    //    img.Save(HttpContext.Current.Server.MapPath("~/college/Principal_Signature.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    //}

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream.Dispose();
                }
            }
        
        
   
     
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}