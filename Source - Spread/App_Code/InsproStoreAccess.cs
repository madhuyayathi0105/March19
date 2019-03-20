///Insproplus connection class for Database stored procedure interactions
///Author : Mohamed Idhris Sheik Dawood
///Date created : 12 October, 2016
///Last modified : 12 October, 2016
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace InsproDataAccess
{
    public class InsproStoreAccess
    {
        InsproConnection insproConnection = new InsproConnection();
        InsproCommand insproCommand = new InsproCommand();
        /// <summary>
        /// Function that retrieves single Datatable from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to retrieve Datatable</param>
        /// <returns>Return a single DataTable</returns>
        public DataTable selectDataTable(string selectQuery, Dictionary<string,string> dicParameter)
        {
            DataTable dtSelectedTable = new DataTable();
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(selectQuery, insproConnection, dicParameter);
                SqlDataAdapter insproDataAdapter = new SqlDataAdapter(insproCom);
                insproDataAdapter.Fill(dtSelectedTable);
            }
            catch { }
            finally { insproConnection.CloseConnection(); }
            return dtSelectedTable;
        }
        /// <summary>
        /// Function that retrieves single DataSet from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to retrieve Dataset</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return number of tables selected in a Dataset</returns>
        public DataSet selectDataSet(string selectQuery, Dictionary<string, string> dicParameter)
        {
            DataSet dtSelectedTableSet = new DataSet();
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(selectQuery, insproConnection, dicParameter);
                SqlDataAdapter insproDataAdapter = new SqlDataAdapter(insproCom);
                insproDataAdapter.Fill(dtSelectedTableSet);
            }
            catch { }
            finally { insproConnection.CloseConnection(); }
            return dtSelectedTableSet;
        }
        /// <summary>
        /// Function that  Insert Data into database using insert criteria
        /// </summary>
        /// <param name="selectQuery">Insert query to insert data</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return number of rows inserted</returns>
        public int insertData(string insertQuery, Dictionary<string, string> dicParameter)
        {
            int insertedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(insertQuery, insproConnection, dicParameter);
                insertedValue = insproCom.ExecuteNonQuery();
            }
            catch { insertedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return insertedValue;
        }
        /// <summary>
        /// Function that updates Data into database using update criteria
        /// </summary>
        /// <param name="selectQuery">Update query to update data</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return number of rows updated</returns>
        public int updateData(string updateQuery, Dictionary<string, string> dicParameter)
        {
            int updatedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(updateQuery, insproConnection, dicParameter);
                updatedValue = insproCom.ExecuteNonQuery();
            }
            catch { updatedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return updatedValue;
        }
        /// <summary>
        /// Function that deletes Data into database using delete criteria
        /// </summary>
        /// <param name="selectQuery">Delete query to delete data</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return number of rows deleted</returns>
        public int deleteData(string deleteQuery, Dictionary<string, string> dicParameter)
        {
            int deletedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(deleteQuery, insproConnection, dicParameter);
                deletedValue = insproCom.ExecuteNonQuery();
            }
            catch { deletedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return deletedValue;
        }
        /// <summary>
        /// Function that  Select scalar string value from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to select value</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return a string value</returns>
        public string selectScalarString(string selectQuery, Dictionary<string, string> dicParameter)
        {
            string selectedValue = string.Empty;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(selectQuery, insproConnection, dicParameter);
                selectedValue = Convert.ToString(insproCom.ExecuteScalar()).Trim();
            }
            catch { selectedValue = string.Empty; }
            finally { insproConnection.CloseConnection(); }
            return selectedValue == null ? string.Empty : selectedValue;
        }
        /// <summary>
        /// Function that  Select scalar integer value from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to select value</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return a integer value</returns>
        public int selectScalarInt(string selectQuery, Dictionary<string, string> dicParameter)
        {
            int selectedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(selectQuery, insproConnection, dicParameter);
                string selectedString = Convert.ToString(insproCom.ExecuteScalar()).Trim();
                int.TryParse(selectedString, out selectedValue);
            }
            catch { selectedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return selectedValue;
        }
        /// <summary>
        /// Function that  Select scalar double value from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to select value</param>
        /// <param name="dicParameter">Command parameters for stored procedure in Dictionary(<string,string>) format</param>
        /// <returns>Return a double value</returns>
        public double selectScalarDouble(string selectQuery, Dictionary<string, string> dicParameter)
        {
            double selectedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateStoredCommand(selectQuery, insproConnection, dicParameter);
                string selectedString = Convert.ToString(insproCom.ExecuteScalar()).Trim();
                double.TryParse(selectedString, out selectedValue);
            }
            catch { selectedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return selectedValue;
        }
    }
}
