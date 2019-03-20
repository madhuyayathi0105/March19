<%@ WebHandler Language="C#" Class="Veh_Front_Photo" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class Veh_Front_Photo : IHttpHandler {

    public string GetConnectionString()
    {

        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }

    public void ProcessRequest(HttpContext context)
    {
        string Veh_ID = "";

        Veh_ID = context.Request.QueryString["Veh_ID"].ToString();
        MemoryStream memoryStream = new MemoryStream();

        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select v_Front from Vehicle_Insurance where Veh_ID='" + Veh_ID + "'";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows == true)
        {
            try
            {
                if (MyReader["v_Front"] != "" || MyReader["v_Front"] != null)
                {
                    byte[] file = (byte[])MyReader["v_Front"];

                    MyReader.Close();

                    connection.Close();

                    memoryStream.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream.Dispose();
                }
            }
            catch
            {

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