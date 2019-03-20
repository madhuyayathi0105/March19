<%@ WebHandler Language="C#" Class="ghStaffFoto" %>

using System;
using System.Web;

public class ghStaffFoto : IHttpHandler {

    public string GetConnectionString()
    {
        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();
    }
    public void ProcessRequest(HttpContext context)
    {
        try{
        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
        System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(GetConnectionString());
        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
        string id = context.Request.QueryString["QSstaff_id"].ToString();
        cmd.CommandText = "select photo from StaffPhoto where staff_code='" + id + "')";
        cmd.Connection = connection;
        connection.Open();
        System.Data.SqlClient.SqlDataReader MyReader = cmd.ExecuteReader();
        if (MyReader.Read())
        {
            byte[] file = (byte[])MyReader["photo"];
            MyReader.Close();
            connection.Close();
            memoryStream.Write(file, 0, file.Length);
            context.Response.Buffer = true;
            context.Response.BinaryWrite(file);
            memoryStream.Dispose();
        
        }
        }
        catch { }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}