<%@ WebHandler Language="C#" Class="VisitorPhoto" %>

using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class VisitorPhoto : IHttpHandler
{

    public string GetConnectionString()
    {
        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();
    }
    public void ProcessRequest(HttpContext context)
    {
        string Visitorcode = context.Request.QueryString["VisitorID"];
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        if (Visitorcode != null)
            cmd.CommandText = "select VisitorPhoto from VisitorsPhoto where VisitorID='" + Visitorcode + "'";

        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows)
        {
            byte[] file = (byte[])MyReader["VisitorPhoto"];
            memoryStream.Write(file, 0, file.Length);
            context.Response.Buffer = true;
            context.Response.BinaryWrite(file);
            memoryStream.Dispose();
        }
        MyReader.Close();
        connection.Close();
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}