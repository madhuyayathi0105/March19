///Insproplus connection class for Database command
///Author : Mohamed Idhris Sheik Dawood
///Date created : 12 October, 2016
///Last modified : 12 October, 2016
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace InsproDataAccess
{
    public class InsproCommand
    {
        /// <summary>
        /// Create New Command type for Text and opens connection
        /// </summary>
        /// <param name="commandQuery">Query to execute</param>
        /// <returns></returns>
        public SqlCommand CreateTextCommand(string commandQuery, InsproConnection insproConnection)
        {
            SqlCommand insproCommand = new SqlCommand(commandQuery);
            insproCommand.CommandType = CommandType.Text;
            insproCommand.Connection = insproConnection.CreateConnection();
            return insproCommand;
        }
        /// <summary>
        /// Create New Command type for Stored Procedure and opens connection
        /// </summary>
        /// <param name="commandQuery">Query to execute</param>
        /// <returns></returns>
        public SqlCommand CreateStoredCommand(string commandQuery, InsproConnection insproConnection, Dictionary<string,string> dicParameter)
        {
            SqlCommand insproCommand = new SqlCommand(commandQuery);
            insproCommand.CommandType = CommandType.StoredProcedure;
            foreach (KeyValuePair<string,string> key in dicParameter)
            {
                insproCommand.Parameters.Add(key.Key, key.Value);
            }
            insproCommand.Connection = insproConnection.CreateConnection();
            return insproCommand;
        }
    }
}
