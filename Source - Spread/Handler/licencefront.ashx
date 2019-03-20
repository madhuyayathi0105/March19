<%@ WebHandler Language="C#" Class="licencefront" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;

public class licencefront : IHttpHandler 
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

        //=========hided and modified by gowtham
       // cmd.CommandText = "select Licence_Front from DriverAllotment  where Staff_code ='" + staffcod + "'";
        cmd.CommandText = "select LicFront from StaffPhoto  where Staff_code ='" + staffcod + "'";
        //cmd.CommandText = "select Licence_Front from DriverAllotment  where Staff_Code ='" + staffcod + "'";//Modified by rajasekar 08/09/2018
        //===================end===========================
        
        cmd.Connection = connection;
        connection.Open();
         SqlDataReader MyReader = cmd.ExecuteReader();
         MyReader.Read();

         if (MyReader.HasRows == true)
         {

            // if (MyReader["Licence_Front"] != "" || MyReader["Licence_Front"] != null)
             if (MyReader["LicFront"] != "" || MyReader["LicFront"] != null)
             
             {


                 byte[] file = (byte[])MyReader["LicFront"];
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