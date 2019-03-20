using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for connection
/// </summary>
public class connection
{

    public SqlConnection con = null;
    public string connectionstring;
    public SqlConnection con1 = null;
    public String connectionstring1 = null;

    public SqlConnection CreateConnection()
    {
        try
        {

            connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //connectionstring = "server=67.228.73.106; database=ssf; user id=finance;password=ssf!234;Max pool size=1500;Min pool size=20;Pooling=true";
            con = new SqlConnection();
            con.ConnectionString = connectionstring;
            con.Open();
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Closed)
            {
                connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //connectionstring = "server=67.228.73.106; database=ssf; user id=finance;password=ssf!234;Pooling=false";
                con = new SqlConnection();
                con.ConnectionString = connectionstring;
                con.Open();
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                con = new SqlConnection();
                con.ConnectionString = connectionstring;
                con.Open();
            }
            Console.WriteLine(ex.Message);
        }
        return con;
    }

    public SqlConnection LocalCreateConnection()
    {
        try
        {
            string connectionstring = "DSN";
            con = new SqlConnection();
            con.ConnectionString = connectionstring;
            con.Open();
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Closed)
            {
                string connectionstring = "DSN";
                con = new SqlConnection();
                con.ConnectionString = connectionstring;
                con.Open();
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                string connectionstring = "DSN";
                con = new SqlConnection();
                con.ConnectionString = connectionstring;
                con.Open();
            }
            Console.WriteLine(ex.Message);
        }
        return con;
    }

    public void Close()
    {
        con.Close();
    }

    public bool IsConnected()
    {
        System.Uri Url = new System.Uri("http://www.yahoo.com");

        System.Net.WebRequest WebReq;
        System.Net.WebResponse Resp;
        WebReq = System.Net.WebRequest.Create(Url);

        try
        {
            Resp = WebReq.GetResponse();
            Resp.Close();
            WebReq = null;
            return true;
        }

        catch
        {
            WebReq = null;
            return false;
        }
    }
    public SqlConnection CreateConnection_Biometric()
    {
        try
        {

            connectionstring1 = System.Configuration.ConfigurationManager.ConnectionStrings["Biometric"].ConnectionString;
            //connectionstring = "server=67.228.73.106; database=ssf; user id=finance;password=ssf!234;Max pool size=1500;Min pool size=20;Pooling=true";
            con1 = new SqlConnection();
            con1.ConnectionString = connectionstring1;
            con1.Open();
        }
        catch (Exception ex)
        {
            if (con1.State == ConnectionState.Closed)
            {
                connectionstring1 = System.Configuration.ConfigurationManager.ConnectionStrings["Biometric"].ConnectionString;
                //connectionstring = "server=67.228.73.106; database=ssf; user id=finance;password=ssf!234;Pooling=false";
                con1 = new SqlConnection();
                con1.ConnectionString = connectionstring1;
                con1.Open();
            }
            if (con1.State == ConnectionState.Open)
            {
                con1.Close();
                connectionstring1 = System.Configuration.ConfigurationManager.ConnectionStrings["Biometric"].ConnectionString;
                con1 = new SqlConnection();
                con1.ConnectionString = connectionstring;
                con1.Open();
            }
            Console.WriteLine(ex.Message);
        }
        return con1;
    }

    public void Close_Biometric()
    {
        con1.Close();

    }


}