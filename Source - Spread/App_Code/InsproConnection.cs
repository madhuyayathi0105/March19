///Insproplus connection class for Database Connection
///Author : Mohamed Idhris Sheik Dawood
///Date created : 12 October, 2016
///Last modified : 12 October, 2016
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace InsproDataAccess
{
    public class InsproConnection
    {
        SqlConnection insproConnection;
        private string insproConString;
        public string InsproConString
        {
            get { return insproConString; }
            set { insproConString = value; }
        }
        public SqlConnection CreateConnection()
        {
            insproConnection = null;
            try
            {
                insproConString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                insproConnection = new SqlConnection();
                insproConnection.ConnectionString = insproConString;
                OpenConnection(insproConnection);
            }
            catch { insproConnection = null; }
            return insproConnection;
        }
        public SqlConnection OpenConnection(SqlConnection newInsproConnection)
        {
            if (newInsproConnection.State == ConnectionState.Closed)
            {
                newInsproConnection.Open();
            }
            return newInsproConnection;
        }
        public void CloseConnection()
        {
            if (insproConnection!=null && insproConnection.State == ConnectionState.Open)
            {
                insproConnection.Close();
            }
        }
    }
}
