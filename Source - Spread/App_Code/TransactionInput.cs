using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for TransactionInput
/// </summary>
public class TransactionInput
{
    public string payModeAgent = string.Empty;
    public string appNo = string.Empty;
    public string app_formNo = string.Empty;
    public string collegeCode = string.Empty;
    public string key = string.Empty;
    public string Salt = string.Empty;
    public string txnid = string.Empty;
    public string amt = string.Empty;
    public string prodInfo = string.Empty;
    public string fname = string.Empty;
    public string mailId = string.Empty;
    public string mobileNo = string.Empty;
    public string courseName = string.Empty;
    public string degreeName = string.Empty;
    public string multiProd = string.Empty;
    public string trackid = string.Empty;
    public string tokenid = string.Empty;
    public string headerName = string.Empty;
    public string transdate = string.Empty;

    public TransactionInput()
    {
        //
        // TODO: Add constructor logic here
        //
    }

}