using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ClsConnection
/// </summary>
/// 
namespace DalConnection
{
public class ClsConnection
{
  
	public ClsConnection()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static SqlConnection Getconnection()
    {
        return new  SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    }
}
}