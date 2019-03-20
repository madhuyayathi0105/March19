<%@ WebHandler Language="C#" Class="Class_Advisor" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Class_Advisor : IHttpHandler {

    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    
    public void ProcessRequest (HttpContext context)
    {
        string staffcode = "";
        staffcode = context.Request.QueryString["id"].ToString();
        
        MemoryStream memoryStream = new MemoryStream();
        if (staffcode.ToString() != "")
        {
           
            SqlConnection connection = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select staffsign from staffphoto where staff_code='" + staffcode.ToString() + "'";
            cmd.Connection = connection;
            connection.Open();
            SqlDataReader MyReader = cmd.ExecuteReader();
            if (MyReader.Read())
            {
                if (MyReader["staffsign"].ToString() != string.Empty)
                {

                    byte[] file = (byte[])MyReader["staffsign"];
                    MyReader.Close();

                    connection.Close();

                    memoryStream.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream.Dispose();
                }
            }
        }
     
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}