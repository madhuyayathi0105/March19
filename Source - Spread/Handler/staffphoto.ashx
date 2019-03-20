<%@ WebHandler Language="C#" Class="staffphoto" %>

using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;


public class staffphoto : IHttpHandler
{

    public string GetConnectionString()
    {


        return System.Configuration.ConfigurationManager.AppSettings["con"].ToString();

    }
    public void ProcessRequest(HttpContext context)
    {

        string staffcode;
        string appl_id;
        staffcode = context.Request.QueryString["Staff_Code"];
        appl_id = context.Request.QueryString["appl_id"];
        MemoryStream memoryStream = new MemoryStream();

        SqlConnection connection = new SqlConnection(GetConnectionString());
        SqlCommand cmd = new SqlCommand();
        if (staffcode != null)
            cmd.CommandText = "select photo from staffphoto where Staff_code='" + staffcode + "'";
        else
            cmd.CommandText = "select photo from staffphoto where appl_id='" + appl_id + "'";
        cmd.Connection = connection;
        connection.Open();
        SqlDataReader MyReader = cmd.ExecuteReader();
        MyReader.Read();
        if (MyReader.HasRows)
        {
            byte[] file = (byte[])MyReader["photo"];




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