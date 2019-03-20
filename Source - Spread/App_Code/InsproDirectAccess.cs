///Insproplus connection class for Direct Database Access
///Author : Mohamed Idhris Sheik Dawood
///Date created : 12 October, 2016
///Last modified : 12 October, 2016
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
namespace InsproDataAccess
{
    public class InsproDirectAccess
    {
        InsproConnection insproConnection = new InsproConnection();
        InsproCommand insproCommand = new InsproCommand();
        /// <summary>
        /// Function that retrieves single Datatable from database using select criteria
        /// </summary>
        /// <param name="selectQuery">Select query to retrieve Datatable</param>
        /// <returns>Return a single DataTable</returns>
        public DataTable selectDataTable(string selectQuery)
        {
            DataTable dtSelectedTable = new DataTable();
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(selectQuery, insproConnection);
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
        /// <returns>Return number of tables selected in a Dataset</returns>
        public DataSet selectDataSet(string selectQuery)
        {
            DataSet dtSelectedTableSet = new DataSet();
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(selectQuery, insproConnection);
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
        /// <returns>Return number of rows inserted</returns>
        public int insertData(string insertQuery)
        {
            int insertedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(insertQuery, insproConnection);
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
        /// <returns>Return number of rows updated</returns>
        public int updateData(string updateQuery)
        {
            int updatedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(updateQuery, insproConnection);
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
        /// <returns>Return number of rows deleted</returns>
        public int deleteData(string deleteQuery)
        {
            int deletedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(deleteQuery, insproConnection);
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
        /// <returns>Return a string value</returns>
        public string selectScalarString(string selectQuery)
        {
            string selectedValue = string.Empty;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(selectQuery, insproConnection);
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
        /// <returns>Return a integer value</returns>
        public int selectScalarInt(string selectQuery)
        {
            int selectedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(selectQuery, insproConnection);
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
        /// <returns>Return a double value</returns>
        public double selectScalarDouble(string selectQuery)
        {
            double selectedValue = 0;
            try
            {
                SqlCommand insproCom = insproCommand.CreateTextCommand(selectQuery, insproConnection);
                string selectedString = Convert.ToString(insproCom.ExecuteScalar()).Trim();
                double.TryParse(selectedString, out selectedValue);
            }
            catch { selectedValue = 0; }
            finally { insproConnection.CloseConnection(); }
            return selectedValue;
        }

        public int send_sms(string user_id, string collegecode, string usercode, string mobilenos, string message, string isstaff)
        {
            int smssend = 0;
            try
            {
                string SenderID = "";
                string Password = "";
                string getval = GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[1].ToString();
                }

                string strpath = "  http://unicel.in/SendSMS/sendmsg.php?uname=" + user_id + "&pass=" + Password + "&send=" + SenderID + "&dest=" + mobilenos + "&msg=" + message + "";

                WebRequest request = WebRequest.Create(strpath);
                WebResponse response = request.GetResponse();
                Stream data = response.GetResponseStream();
                StreamReader sr = new StreamReader(data);
                string strvel = sr.ReadToEnd();

                string groupmsgid = "";
                groupmsgid = strvel;
                string date = DateTime.Now.ToString("MM/dd/yyyy");

                int sms = 0;
                string smsreportinsert = "";
                string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
                string[] spmessp = strvel.Split(new Char[] { ',' });
                for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
                {
                    smssend++;
                    string smsid = "";
                    if (spmessp.GetUpperBound(0) >= icount)
                    {
                        smsid = spmessp[icount].ToString();
                    }
                    string mobile = split_mobileno[icount].ToString();
                    smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + mobile + "','" + smsid + "','" + message + "','" + collegecode + "','" + isstaff + "','" + date + "','" + usercode.ToString() + "')";
                    sms = updateData(smsreportinsert);
                }
            }
            catch
            {
            }
            return smssend;
        }
        public string GetUserapi(string user_id)
        {
            try
            {
                user_id = user_id.Trim().ToUpper();
                string SenderID = "";
                string Password = "";
                if (user_id == "AAACET")
                {
                    SenderID = "AAACET";
                    Password = "AAACET";
                }
                else if (user_id == "AALIME")
                {
                    SenderID = "AALIME";
                    Password = "AALIME";
                }
                else if (user_id == "AMSAAC")
                {
                    SenderID = "AMSAAC";
                    Password = "AMSAAC";
                }
                else if (user_id == "ACETVM")
                {
                    SenderID = "ACETVM";
                    Password = "ACETVM";
                }
                else if (user_id == "AGNICT")
                {
                    SenderID = "AGNICT";
                    Password = "AGNICT";
                }
                else if (user_id == "AMSPTC")
                {
                    SenderID = "AMSPTC";
                    Password = "AMSPTC";
                }
                else if (user_id == "ANGE")
                {
                    SenderID = "ANGELS";
                    Password = "ANGELS";
                }
                else if (user_id == "ARASUU")
                {
                    SenderID = "ARASUU";
                    Password = "ARASUU";
                }
                else if (user_id == "DAVINC")
                {
                    SenderID = "DAVINC";
                    Password = "DAVINC";
                }
                else if (user_id == "EASACG")
                {
                    SenderID = "EASACG";
                    Password = "EASACG";
                }
                else if (user_id == "ECESMS")
                {
                    SenderID = "ECESMS";
                    Password = "ECESMS";
                }
                else if (user_id == "ESECED")
                {
                    SenderID = "ESECED";
                    Password = "ESECED";
                }
                else if (user_id == "ESENGG")
                {
                    SenderID = "ESENGG";
                    Password = "ESENGG";
                }
                else if (user_id == "ESEPTC")
                {
                    SenderID = "ESEPTC";
                    Password = "ESEPTC";
                }
                else if (user_id == "ESMSCH")
                {
                    SenderID = "ESMSCH";
                    Password = "ESMSCH";
                }
                else if (user_id == "GKMCET")
                {
                    SenderID = "GKMCET";
                    Password = "GKMCET";
                }
                else if (user_id == "IJAYAM")
                {
                    SenderID = "IJAYAM";
                    Password = "IJAYAM";
                }
                else if (user_id == "JJAAMC")
                {
                    SenderID = "JJAAMC";
                    Password = "JJAAMC";
                }
                else if (user_id == "JMCTRY")
                {
                    SenderID = "JMCTRY";
                    Password = "JMCTRY";
                }
                else if (user_id == "KINGSE")
                {
                    SenderID = "KINGSE";
                    Password = "KINGSE";
                }
                else if (user_id == "KNMHSS")
                {
                    SenderID = "KNMHSS";
                    Password = "KNMHSS";
                }
                else if (user_id == "KSRIET")
                {
                    SenderID = "KSRIET";
                    Password = "KSRIET";
                }
                else if (user_id == "KTVRKP")
                {
                    SenderID = "KTVRKP";
                    Password = "KTVRKP";
                }
                else if (user_id == "MPNMJS")
                {
                    SenderID = "MPNMJS";
                    Password = "MPNMJS";
                }
                else if (user_id == "NANDHA")
                {
                    SenderID = "NANDHA";
                    Password = "NANDHA";
                }
                else if (user_id == "NECARE")
                {
                    SenderID = "NECARE";
                    Password = "NECARE";
                }
                else if (user_id == "NSNCET")
                {
                    SenderID = "NSNCET";
                    Password = "NSNCET";
                }
                else if (user_id == "PETENG")
                {
                    SenderID = "PETENG";
                    Password = "PETENG";
                }
                else if (user_id == "PMCTEC")
                {
                    SenderID = "PMCTEC";
                    Password = "PMCTEC";
                }
                else if (user_id == "PPGITS")
                {
                    SenderID = "PPGITS";
                    Password = "PPGITS";
                }
                else if (user_id == "PROFCL")
                {
                    SenderID = "PROFCL";
                    Password = "PROFCL";
                }
                else if (user_id == "RVSCHN")
                {
                    SenderID = "RVSCHN";
                    Password = "RVSCHN";
                }
                else if (user_id == "PSVCET")
                {
                    SenderID = "PSVCET";
                    Password = "PSVCET";
                }
                else if (user_id == "SASTH")
                {
                    SenderID = "SASTHA";
                    Password = "SASTHA";
                }
                else if (user_id == "SCTSBS")
                {
                    SenderID = "SCTSBS";
                    Password = "SCTSBS";
                }
                else if (user_id == "SCTSCE")
                {
                    SenderID = "SCTSCE";
                    Password = "SCTSCE";
                }
                else if (user_id == "SCTSEC")
                {
                    SenderID = "SCTSEC";
                    Password = "SCTSEC";
                }
                else if (user_id == "SKCETC")
                {
                    SenderID = "SKCETC";
                    Password = "SKCETC";
                }
                else if (user_id == "SRECCG")
                {
                    SenderID = "SRECCG";
                    Password = "SRECCG";
                }
                else if (user_id == "SLAECT")
                {
                    SenderID = "SLAECT";
                    Password = "SLAECT";
                }
                else if (user_id == "SSCENG")
                {
                    SenderID = "SSCENG";
                    Password = "SSCENG";
                }
                else if (user_id == "SSMCEE")
                {
                    SenderID = "SSMCEE";
                    Password = "SSMCEE";
                }
                else if (user_id == "SVICET")
                {
                    SenderID = "SVICET";
                    Password = "SVICET";
                }
                else if (user_id == "SVCTCG")
                {
                    SenderID = "SVCTCG";
                    Password = "SVCTCG";
                }
                else if (user_id == "SVSCBE")
                {
                    SenderID = "SVSCBE";
                    Password = "SVSCBE";
                }
                else if (user_id == "TECENG")
                {
                    SenderID = "TECENG";
                    Password = "TECENG";
                }
                else if (user_id == "TJENGG")
                {
                    SenderID = "TJENGG";
                    Password = "TJENGG";
                }
                else if (user_id == "TSMJCT")
                {
                    SenderID = "TSMJCT";
                    Password = "TSMJCT";
                }
                else if (user_id == "UITCBE")
                {
                    SenderID = "UITCBE";
                    Password = "UITCBE";
                }
                else if (user_id == "VCWSMS")
                {
                    SenderID = "VCWSMS";
                    Password = "VCWSMS";
                }
                else if (user_id == "VRSCET")
                {
                    SenderID = "VRSCET";
                    Password = "VRSCET";
                }
                else if (user_id == "AUDIIT")
                {
                    SenderID = "AUDIIT";
                    Password = "AUDIIT";
                }
                else if (user_id == "SAENGG")
                {
                    SenderID = "SAENGG";
                    Password = "SAENGG";
                }
                else if (user_id == "SAIRA")
                {
                    SenderID = "SAIRA";
                    Password = "SAIRA";
                }
                else if (user_id == "STANE")
                {
                    SenderID = "STANES";
                    Password = "STANES";
                }
                else if (user_id == "MBCBSE")
                {
                    SenderID = "MBCBSE";
                    Password = "MBCBSE";
                }

                else if (user_id == "HIETPT")
                {
                    SenderID = "HIETPT";
                    Password = "HIETPT";
                }

                else if (user_id == "SVPITM")
                {
                    SenderID = "SVPITM";
                    Password = "SVPITM";
                }

                else if (user_id == "AUDCET")
                {
                    SenderID = "AUDCET";
                    Password = "AUDCET";
                }
                else if (user_id == "AUDWOM")
                {
                    SenderID = "AUDWOM";
                    Password = "AUDWOM";
                }
                else if (user_id == "COENIU")
                {
                    SenderID = "COENIU";
                    Password = "COENIU";
                }
                else if (user_id == "AUDIPG")
                {
                    SenderID = "AUDIPG";
                    Password = "AUDIPG";
                }

                else if (user_id == "MCCDAY")
                {
                    SenderID = "MCCDAY";
                    Password = "MCCDAY";
                }

                else if (user_id == "MCCSFS")
                {
                    SenderID = "MCCSFS";
                    Password = "MCCSFS";
                }
                else if (user_id == "PETPTC")
                {
                    SenderID = "PETPTC";
                    Password = "PE•PT•";
                }
                else if (user_id == "RVCENG")
                {
                    SenderID = "RVCENG";
                    Password = "RVCENG";
                }
                else if (user_id == "JMHRSS")
                {
                    SenderID = "JMHRSS";
                    Password = "JMHRSS";
                }
                else if (user_id == "JECERP")
                {
                    SenderID = "JECERP";
                    Password = "JECERP";
                }
                else if (user_id == "JHSSCB")
                {
                    SenderID = "JHSSCB";
                    Password = "JHSSCB";
                }
                else if (user_id == "APTMMR")
                {
                    SenderID = "APTMMR";
                    Password = "APTMMR";
                }
                else if (user_id == "EGSSMS")
                {
                    SenderID = "EGSSMS";
                    Password = "EGSSMS";
                }
                else if (user_id == "CITAIN")
                {
                    SenderID = "CITAIN";
                    Password = "CITAIN";
                }
                else if (user_id == "SHMHSS")
                {
                    SenderID = "SHMHSS";
                    Password = "SHMHSS";
                }
                else if (user_id == "SVschl")
                {
                    SenderID = "SVschl";
                    Password = "SVschl";
                }
                else if (user_id == "KECERP")
                {
                    SenderID = "KECERP";
                    Password = "KECERP";
                }
                else if (user_id == "SCOTTC")
                {
                    SenderID = "SCOTTC";
                    Password = "SCOTTC";
                }
                else if (user_id == "VELITH")
                {
                    SenderID = "VELITH";
                    Password = "VELITH";
                }
                else if (user_id == "INDIRA")
                {
                    SenderID = "INDIRA";
                    Password = "INDIRA";
                }
                else if (user_id == "DHIRAJ")
                {
                    SenderID = "DHIRAJ";
                    Password = "DHIRAJ";
                }
                else if (user_id == "MAMCET")
                {
                    SenderID = "MAMCET";
                    Password = "MAMCET";
                }
                else if (user_id == "SCOTT")
                {
                    SenderID = "SCOTTc";
                    Password = "SCOTTc";
                }
                else if (user_id == "GNANMG")
                {
                    SenderID = "GNANMG";
                    Password = "GNANMG";
                }
                else if (user_id == "NEWCLG")
                {
                    SenderID = "NEWCLG";
                    Password = "NEWCLG";
                }
                else if (user_id == "eSNCET")
                {
                    SenderID = "eSNCET";
                    Password = "eSNCET";
                }
                else if (user_id == "AVMSKL")
                {
                    SenderID = "AVMSKL";
                    Password = "AVMSKL";
                }
                else if (user_id == "JMCBSE")
                {
                    SenderID = "JMCBSE";
                    Password = "JMCBSE";
                }
                else if (user_id == "KCGERP")
                {
                    SenderID = "KCGERP";
                    Password = "KCGERP";
                }
                else if (user_id == "SMKFIT")
                {
                    SenderID = "SMKFIT";
                    Password = "SMKFIT";
                }
                else if (user_id == "ASBEAM")
                {
                    SenderID = "ASBEAM";
                    Password = "ASBEAM";
                }
                else if (user_id == "APECAD")
                {
                    SenderID = "APECAD";
                    Password = "APECAD";
                }
                else if (user_id == "PALPAP")
                {
                    SenderID = "PALPAP";
                    Password = "PALPAP";
                }
                else if (user_id == "LCLIBA")
                {
                    SenderID = "LCLIBA";
                    Password = "LCLIBA";
                }
                else if (user_id == "STCCED")
                {
                    SenderID = "STCCED";
                    Password = "STCCED";
                }
                else if (user_id == "SANPAL")
                {
                    SenderID = "SANPAL";
                    Password = "SANPAL";
                }
                else if (user_id == "SANVEL")
                {
                    SenderID = "SANVEL";
                    Password = "o$4Zf$5I";
                }
                string retval = SenderID + '-' + Password;
                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
