<%@ WebHandler Language="C#" Class="LicenceBack" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

public class LicenceBack : IHttpHandler {

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
        //======================modified by gowtham==============================
        // cmd.CommandText = "select Licence_Back from DriverAllotment  where Staff_code ='" + staffcod + "' ";
        cmd.CommandText = "select LicBack from StaffPhoto  where Staff_code ='" + staffcod + "'";

        //cmd.CommandText = "select Licence_Back from DriverAllotment  where Staff_Code ='" + staffcod + "'";//Modified by rajasekar 08/09/2018
        //=========================end=====================================

        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();

        if (MyReader.HasRows == true)
        {
            try
            {
                if (MyReader["LicBack"] != "" || MyReader["LicBack"] != null)
                {

                    byte[] file = (byte[])MyReader["LicBack"];
                    MyReader.Close();

                    connection.Close();

                    memoryStream.Write(file, 0, file.Length);

                    context.Response.Buffer = true;

                    context.Response.BinaryWrite(file);

                    memoryStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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