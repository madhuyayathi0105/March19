<%@ WebHandler Language="C#" Class="ViewLicence" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class ViewLicence : IHttpHandler 
{

    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest(HttpContext context)
    {
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        string staffcod = context.Request.QueryString["Staff_code"].ToString();


        cmd.CommandText = "select File_Name from Temp_Table where Staff_Code ='" + staffcod + "' and type='Front'";

        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();

        if (MyReader.HasRows == true)
        {

            if (MyReader["File_Name"] != "" || MyReader["File_Name"] != null)
            {


                byte[] file = (byte[])MyReader["File_Name"];
                MyReader.Close();

                connection.Close();

                memoryStream.Write(file, 0, file.Length);

                context.Response.Buffer = true;

                context.Response.BinaryWrite(file);

                memoryStream.Dispose();
            }
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