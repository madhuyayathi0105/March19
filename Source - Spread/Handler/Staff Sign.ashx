<%@ WebHandler Language="C#" Class="Staff_Sign" %>

using System;
using System.Web;
using System.IO;
using System.Data.SqlClient;

public class Staff_Sign : IHttpHandler {

    public string GetConnectionString()
    {
        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();
    }
    
    public void ProcessRequest (HttpContext context) 
    {
        string staffcode;
        staffcode = context.Request.QueryString["Staff_Code"];
        MemoryStream memoryStream = new MemoryStream(); SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select StaffSign from staffphoto where staff_code='" + staffcode + "'";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows)
        {
            byte[] file = (byte[])MyReader["StaffSign"];

            memoryStream.Write(file, 0, file.Length);

            context.Response.Buffer = true;

            context.Response.BinaryWrite(file);

            memoryStream.Dispose();
        }

        MyReader.Close();

        connection.Close();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}