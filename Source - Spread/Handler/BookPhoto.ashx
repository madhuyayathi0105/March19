<%@ WebHandler Language="C#" Class="BookPhoto" %>

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
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

public class BookPhoto : IHttpHandler
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
        string accNo = context.Request.QueryString["acc_no"].ToString();
        cmd.CommandText = "select photo from BookPhoto where acc_no in('" + accNo + "')";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        if (MyReader.Read())
        {
            byte[] file = (byte[])MyReader["photo"];
            MyReader.Close();
            connection.Close();
            memoryStream.Write(file, 0, file.Length);
            context.Response.Buffer = true;
            if (file.Length > 0)
            {
                context.Response.BinaryWrite(file);
            }
            else
            {
                context.Response.WriteFile("~/images/dummyimg.png");
            }
            memoryStream.Dispose();
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