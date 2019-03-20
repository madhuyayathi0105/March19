using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Web.UI;
using System.Linq;
using System.Configuration;
using Gios.Pdf;
using System.Net;
using System.Runtime.Serialization.Json;
using InsproDataAccess;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Linq;

public partial class OnlineFeesTransactionUpdate : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    TransactionInput trInput = new TransactionInput();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int isHeaderwise = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {

            }
        }
        catch (Exception ex) { }
    }
    private int getPaymentType(string collegeCode)
    {
        int retValue = 0;
        string value = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='OnlineApplicationPaymentGateway'  and college_code ='" + collegeCode + "' ").Trim();
        int.TryParse(value, out retValue);
        return retValue;
    }
    private void getSALTandKEY(ref string KEY, ref string SALT)
    {
        SALT = string.Empty;
        KEY = string.Empty;
        try
        {
            string salt = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='MerchantSALTvalue'  and college_code ='" + collegecode + "' ").Trim();
            string key = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='MerchantKEYvalue'  and college_code ='" + collegecode + "' ").Trim();
            if (salt != "0" && salt != string.Empty && key != "0" && key != string.Empty)
            {
                SALT = salt;
                KEY = key;
            }

        }
        catch
        {
            SALT = string.Empty;
            KEY = string.Empty;
        }
    }
    public bool IOBPaymentValidation(TransactionInput trInput, ref string strSucess)
    {
        bool boolCheck = false;
        //variable initialisations
        try
        {

            string Ptxnid = trInput.txnid.ToString().Trim();
            string Pamt = trInput.amt.ToString().Trim();
            string appNo = trInput.appNo.ToString().Trim();
            string prodId = trInput.multiProd.ToString().Trim();
            string tokenId = trInput.tokenid.ToString().Trim();
            string trackID = trInput.trackid.ToString().Trim();
            DataSet dnew = d2.select_method_wo_parameter(" select app_formno,stud_name from applyn where app_no='" + appNo + "'", "Text");
            string StudentName = string.Empty;
            string ApplicationNo = string.Empty;
            if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
            {
                StudentName = Convert.ToString(dnew.Tables[0].Rows[0]["stud_name"]);
                ApplicationNo = Convert.ToString(dnew.Tables[0].Rows[0]["app_formno"]);
            }
            string encryptionkey = "A52D9BC3F2F18D71F93A980EED206CD2"; //"285E80ACE9BD000157F9F5C59C61C4A8";
            string encryptioniv = "qWV5xXpThLbQyR36"; //"AyvsA2M7P36mmQJ9";
            string signkey = "Nu6xoUzd5W4eo2athuWGNK9kdjyc5DlS"; //"eY9iq1bYh1F8C1hwKLBZ7iGfc9FNR3yf";
            //string merchantid = "APIMER";
            //string merchantsubid = "LCECHE";// "LCECHE";
            string merchantid = "APIMER";
            string merchantsubid = "NECKOV";
            string tokenaction = "TXNSTATUS";
            string txninitaction = "TXNINIT";
            //  string feetype = "ALL FEES";
            string[] mulProdIds = trInput.multiProd.Trim().Split(',');
            string feetype = string.Empty;
            if (mulProdIds.Length > 1)
            {
                feetype = "MULTIPLE FEES";
            }
            else
            {
                string headerName = d2.GetFunction("select atomProductId from fm_headermaster where headerpk in('" + Convert.ToString(trInput.headerName) + "')");
                feetype = headerName;
            }
            //string headerName = DA.GetFunction("select atomProductId from fm_headermaster where headerpk in('" + Convert.ToString(trInput.headerName) + "')");


            // string feetype = headerName == "0" ? "ALL FEES" : headerName;

            string totalamt = Pamt;
            string iobpayapiurl = "https://www.iobnet.co.in/iobpay/iobpayRESTService/apitxnstatusservice/gettxnstatus/";
            string merchanttxnid = Ptxnid;// DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss");
            //generating a new token
            Product product = new Product();
            product.merchantid = merchantid;
            product.merchantsubid = merchantsubid;
            product.action = tokenaction;
            product.feetype = feetype;
            product.totalamt = totalamt;
            product.tokenid = tokenId;
            // product.trackid = trackID;//
            // product.replyurl = replyurl;
            JSonHelper helper = new JSonHelper();
            string jsonResult = helper.ConvertObjectToJSon(product);
            string encryptedtokendata = t.Encrypt(jsonResult.ToString(), encryptionkey, encryptioniv);
            byte[] byteArrayNEw = Encoding.ASCII.GetBytes(signkey);
            byte[] byteArrayNEw1 = Encoding.ASCII.GetBytes(encryptedtokendata);
            byte[] signedtokenhmac = test.HashHMAC(byteArrayNEw, byteArrayNEw1); //hasfile./ Hexadecimal
            ProductNEw PN = new ProductNEw();
            StringBuilder SaW = new StringBuilder(signedtokenhmac.Length * 2);
            foreach (byte ba in signedtokenhmac)
            {
                SaW.AppendFormat("{0:x2}", ba);
            }
            string Value = SaW.ToString().ToUpper();
            PN.merchantid = merchantid;
            PN.merchantsubid = merchantsubid;
            PN.action = tokenaction;
            PN.data = encryptedtokendata;
            PN.hmac = Value.ToString();
            string jsonResultNew = helper.ConvertObjectToJSon(PN);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonResultNew);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(iobpayapiurl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            request.AllowAutoRedirect = true;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            //string ValueSecond = response.ToString();
            // dataStream = response.GetResponseStream();
            dataStream = response.GetResponseStream();
            StringBuilder Sb = new StringBuilder();
            using (var sr = new StreamReader(dataStream, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Sb.Append(line);
                }
            }
            string OrginalValue = string.Empty;
            ProductNEwValue PS = new ProductNEwValue();
            PS = helper.ConvertJSonToObject<ProductNEwValue>(Sb.ToString());
            Console.WriteLine("Before Decrypt :: data       " + PS.data);
            encryptedtokendata = t.Decrypt(PS.data, encryptionkey, encryptioniv);
            Console.WriteLine("Decrypted date : " + encryptedtokendata);
            #region Added by Mohamed Idhris 11-05-2017
            string[] decryptedvalues = encryptedtokendata.Split(',');
            string resultToken = string.Empty;
            foreach (string jsonVal in decryptedvalues)
            {
                string[] resultPair = jsonVal.Split(':');
                string temp = resultPair[0].Replace("{", "").Trim('\"').ToLower();
                // string Statu = resultPair[0].Replace("{", "").Trim('\"').ToLower();
                if (temp.ToLower() == "txnstatus")
                {
                    if (resultPair.Length > 1)
                    {
                        resultToken = resultPair[1].Replace("{", "").Trim('\"').ToLower();
                        if (resultToken == "success")
                        {
                            boolCheck = true;
                            strSucess = "success";
                        }
                        else if ((resultToken == "AWAITED") || (resultToken == "awaited") || (resultToken == ""))
                        {
                            boolCheck = false;
                            strSucess = "awaited";
                        }
                        else if ((resultToken == "FAILURE") || (resultToken == "failure"))
                        {
                            boolCheck = true;
                            strSucess = "failure";
                        }
                    }
                    break;
                }

                if (temp == "errorcd")
                {
                    boolCheck = true;
                }
            }

            #endregion
        }
        catch { }
        return boolCheck;

    }
    checkClass ch = new checkClass();
    test t = new test();
    public class Product
    {
        public String merchantid { get; set; }
        public String merchantsubid { get; set; }
        public String action { get; set; }
        public String feetype { get; set; }
        public String totalamt { get; set; }
        public String replyurl { get; set; }
        public String tokenid { get; set; }
        public String trackid { get; set; }
    }
    public class ProductNEw
    {
        public String merchantid { get; set; }
        public String merchantsubid { get; set; }
        public String action { get; set; }
        public String data { get; set; }
        public String hmac { get; set; }
    }
    public class ProductNEwValue
    {
        public String merchantid { get; set; }
        public String merchantsubid { get; set; }
        public String action { get; set; }
        public String data { get; set; }
        public String hmac { get; set; }
        public String requestid { get; set; }
        public String errorcd { get; set; }
        public String errormsg { get; set; }
    }
    public class JSonHelper
    {
        public string ConvertObjectToJSon<T>(T obj)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, obj);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        public T ConvertJSonToObject<T>(string jsonString)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)serializer.ReadObject(ms);
            return obj;
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        if (getPaymentValidation())
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Updates Available')", true);
        }

    }
    protected bool getPaymentValidation()
    {
        bool checkVal = false;
        try
        {
            string transdate = txt_date.Text.ToString();

            int pgMode = getPaymentType(collegecode);
            string key = string.Empty;
            string Salt = string.Empty;
            getSALTandKEY(ref key, ref Salt);

            string querystring = "select transpk as transactionid,appno,collegecode,paidstatus,TokenID,TrackID,payamount,collegecode,headerfk,convert(varchar(10),transdate,103) as transdate from OnlineFeeTransactionMaster om,OnlineFeeTransaction ot where om.transpk=ot.transfk and paidstatus='0' and transdate ='" + transdate + "'  group by transpk,appno,collegecode,paidstatus,TokenID,TrackID,collegecode,headerfk,transdate,payamount";
            DataSet dsVal = d2.select_method_wo_parameter(querystring, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                //string redrURI = ConfigurationManager.AppSettings["REDIR_URI"].Trim();
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    string txnid = Convert.ToString(dsVal.Tables[0].Rows[row]["transactionid"]);
                    string tokenId = Convert.ToString(dsVal.Tables[0].Rows[row]["TokenID"]);
                    string trackID = Convert.ToString(dsVal.Tables[0].Rows[row]["TrackID"]);
                    string collegeCode = Convert.ToString(dsVal.Tables[0].Rows[row]["collegecode"]);
                    string headerfk = Convert.ToString(dsVal.Tables[0].Rows[row]["headerfk"]);
                    string appno = Convert.ToString(dsVal.Tables[0].Rows[row]["appno"]);
                    transdate = Convert.ToString(dsVal.Tables[0].Rows[row]["transdate"]);
                    double amt = 0;
                    double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["payamount"]), out amt);
                    // string amt = string.Empty;
                    string sbAtomProdIdnameamt = string.Empty;

                    // string pgmode = Convert.ToString(pgMode) + "$" + "1";
                    string qString = "TRANSID=" + txnid + "&AMT=" + amt + "&KEY=" + key + "&SALT=" + Salt + "&APPNO=" + appno + "&transdate=" + transdate + "&PGMODE=" + pgMode + "&MPROD=" + sbAtomProdIdnameamt.ToString();
                    trInput.payModeAgent = Convert.ToString(pgMode);
                    trInput.appNo = appno;
                    trInput.key = key;
                    trInput.Salt = Salt;
                    trInput.txnid = txnid;
                    trInput.amt = Convert.ToString(amt);
                    trInput.multiProd = sbAtomProdIdnameamt;
                    trInput.tokenid = tokenId;
                    trInput.trackid = trackID;
                    trInput.headerName = headerfk;
                    trInput.transdate = transdate;
                    string strSucess = string.Empty;
                    if (pgMode == 0)
                    {

                        checkVal = AtomPaymentValidation(trInput, ref strSucess);
                    }
                    else if (pgMode == 4)
                    {
                        checkVal = IOBPaymentValidation(trInput, ref strSucess);
                    }
                    else if (pgMode == 6)
                    {
                        checkVal = SBIpaymentValidation(trInput, ref strSucess);
                    }


                    //string encQstring = AesEncrypt(qString);
                    //Response.Redirect(redrURI + "?transval=" + encQstring);

                    if (pgMode == 0)
                    {
                        if (checkVal && (strSucess == "success" || strSucess == "failure"))
                        {
                            updatePayment(appno, collegeCode, strSucess, txnid, "", trackID, Convert.ToString(amt));
                        }
                    }
                    else if (pgMode == 4)
                    {
                        if (checkVal && (strSucess == "success" || strSucess == "failure"))
                        {
                            updatePayment(appno, collegeCode, strSucess, txnid, "IOB", trackID, Convert.ToString(amt));
                        }
                    }
                    else
                    {
                        if (checkVal && (strSucess == "success" || strSucess == "failure"))
                        {
                            updatePayment(appno, collegeCode, strSucess, txnid, "SBI", trackID, Convert.ToString(amt));
                        }
                    }
                }
            }
            else
                checkVal = true;
            // string encQstring = AesEncrypt(qString);
            //  Response.Redirect(redrURI + "?transval=" + encQstring);
            // pgMode = 0;
        }

        catch { }
        return checkVal;
    }
    public bool SBIpaymentValidation(TransactionInput trInput, ref string strSucess)
    {
        bool boolCheck = false;
        //variable initialisations
        try
        {

            string Ptxnid = trInput.txnid.ToString().Trim();
            string Pamt = trInput.amt.ToString().Trim();
            string appNo = trInput.appNo.ToString().Trim();
            string prodId = trInput.multiProd.ToString().Trim();
            string tokenId = trInput.tokenid.ToString().Trim();
            string trackID = trInput.trackid.ToString().Trim();
            DataSet dnew = d2.select_method_wo_parameter(" select app_formno,stud_name from applyn where app_no='" + appNo + "'", "Text");
            string StudentName = string.Empty;
            string ApplicationNo = string.Empty;
            if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
            {
                StudentName = Convert.ToString(dnew.Tables[0].Rows[0]["stud_name"]);
                ApplicationNo = Convert.ToString(dnew.Tables[0].Rows[0]["app_formno"]);
            }

            string ForChecksum = string.Format("ref_no=" + Ptxnid + "|amount=" + Pamt + "");
            string TestCheckSum = GetMD5Hashname(ForChecksum);
            string TextToEncrypt = ForChecksum + "|checkSum=" + TestCheckSum;
            string textToDecrypt = EncryptSBI(TextToEncrypt, "E:\\CRISTIAN_COLLEGE_1.key");
            string postData = textToDecrypt;
            Console.Write(postData);
            //DoubleVerificationCheck(postData.Trim());
            NameValueCollection requestNameValue = new NameValueCollection();
            NameValueCollection nameValue = new NameValueCollection();

            nameValue.Add("merchant_code", "CRISTIAN_CLG");
            nameValue.Add("encdata", postData);

            string responseMsg = PostRequest("https://merchant.onlinesbi.com/thirdparties/doubleverification.htm", nameValue);
            Console.Write(responseMsg);
            string msg = DecryptSBI(responseMsg, "E:\\CRISTIAN_COLLEGE_1.key");
            Console.Write(msg);
            string txnid = string.Empty;
            string status = string.Empty;
            string amountpaid = string.Empty;
            string mihpayid = string.Empty;
            //lbl_GetMessage.Text = msg;
            string[] s = msg.Split(new char[] { '|' });
            //string[] s = msg.Split(new char[] { '|' });
            string value = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                string[] str = s[i].Split(new char[] { '=' });
                requestNameValue.Add(str[0], str[1]);
                if (str[0] == "ref_no")
                {
                    txnid = str[1];
                }
                if (str[0] == "status")
                {
                    status = str[1];
                }
                if (str[0] == "amount")
                {
                    amountpaid = str[1];
                }
                if (str[0] == "sbi_ref_no")
                {

                }
            }
            if (status.ToUpper().Trim() == "SUCCESS")
            {
                boolCheck = true;
                strSucess = "success";
            }
            else
            {
                boolCheck = true;
                strSucess = "failure";
            }

        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Unable Connect Remote Server')", true);
            boolCheck = true;
        }
        return boolCheck;

    }
    public bool AtomPaymentValidation(TransactionInput trInput, ref string strSucess)
    {
        bool boolCheck = false;
        //variable initialisations
        try
        {
            //double amount = 0;
            string key = trInput.key.Trim();
            string transdate = trInput.transdate.Trim().Split(' ')[0];
            string Ptxnid = trInput.txnid.ToString().Trim();
            string Pamt = trInput.amt.ToString().Trim();
            string appNo = trInput.appNo.ToString().Trim();
            string prodId = trInput.multiProd.ToString().Trim();
            string tokenId = trInput.tokenid.ToString().Trim();
            string trackID = trInput.trackid.ToString().Trim();
            DataSet dnew = d2.select_method_wo_parameter(" select app_formno,stud_name from applyn where app_no='" + appNo + "'", "Text");
            string StudentName = string.Empty;
            string ApplicationNo = string.Empty;
            string[] frdate = transdate.Split('/');
            if (frdate.Length == 3)
                transdate = frdate[2].ToString() + "-" + frdate[1].ToString() + "-" + frdate[0].ToString();
            if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
            {
                StudentName = Convert.ToString(dnew.Tables[0].Rows[0]["stud_name"]);
                ApplicationNo = Convert.ToString(dnew.Tables[0].Rows[0]["app_formno"]);
            }

            //  string feetype = "ALL FEES";
            string[] mulProdIds = trInput.multiProd.Trim().Split(',');
            string feetype = string.Empty;
            if (mulProdIds.Length > 1)
            {
                feetype = "MULTIPLE FEES";
            }
            else
            {
                string headerName = d2.GetFunction("select atomProductId from fm_headermaster where headerpk in('" + Convert.ToString(trInput.headerName) + "')");
                feetype = headerName;
            }
            Pamt = d2.GetFunction("select sum(payamount) from onlinefeetransaction where transfk='" + Ptxnid + "'");
            if (!Pamt.Contains('.'))
            {
                Pamt += ".00";
            }




            string postData = "merchantid=" + key + "&merchanttxnid=" + Ptxnid + "&amt=" + Pamt + "&tdate=" + transdate + "";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;


            //url to xml
            String URLString = "https://payment.atomtech.in/paynetz/vfts?" + postData;
            XmlTextReader reader = new XmlTextReader(URLString);
            StringBuilder x = new StringBuilder();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        //Console.Write("<" + reader.Name);
                        x.Append("<" + reader.Name);
                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            //Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                            x.Append(" " + reader.Name + "='" + reader.Value + "'");
                        //Console.Write(">");
                        x.Append("/>");
                        // Console.WriteLine(">");
                        // x.Append(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        //Console.WriteLine(reader.Value);
                        x.Append(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        // Console.Write("</" + reader.Name);
                        x.Append("</" + reader.Name);
                        // Console.WriteLine(">");
                        x.Append(">");
                        break;
                }
            }
            string xmlfile = x.ToString();

            XDocument XDocument = XDocument.Parse(xmlfile);
            //XmlNodeList xNodelst = xdoc.DocumentElement.SelectNodes("VerifyOutput");//reading node so that we can traverse thorugh the XML

            //foreach (XmlNode xNode in xNodelst)//traversing XML
            //{
            //    litFeed.Text += "read";
            //}
            //Get all nodes
            string txnid = string.Empty;
            string status = string.Empty;
            string mihpayid = string.Empty;
            string amountpaid = string.Empty;
            string mode = string.Empty;
            List<Dictionary<string, string>> orderList = new List<Dictionary<string, string>>();
            List<XNode> xNodes = XDocument.DescendantNodes().ToList();
            foreach (XNode node in xNodes)
            {
                XElement element = node as XElement;
                if (element.Name != "VerifyOutput") continue;
                Dictionary<string, string> dict = new Dictionary<string, string>();

                //For each orderProperty, get all attributes
                foreach (XAttribute attribute in element.Attributes())
                {
                    dict.Add(attribute.Name.ToString(), attribute.Value);
                }
                orderList.Add(dict);
            }
            foreach (Dictionary<string, string> dict in orderList)
            {
                foreach (string keys in dict.Keys)
                {
                    //Console.Write(keys + ": " + dict[key] + ", ");
                    if (keys == "MerchantTxnID")//MerchantID
                    {
                        txnid = dict[keys];
                    }
                    if (keys == "VERIFIED")
                    {
                        status = dict[keys];
                    }
                    if (keys == "AMT")//AMT
                    {
                        amountpaid = dict[keys];
                    }
                    if (keys == "postingmmp_txn")
                    {
                        mihpayid = dict[keys];
                    }
                }


            }
            if (status.ToUpper().Trim() == "SUCCESS")
            {
                boolCheck = true;
                strSucess = "success";

            }
            else if (status.ToUpper().Trim() == "FAILED")
            {
                boolCheck = true;
                strSucess = "failure";
            }
            else if (status.ToUpper().Trim() == "NODATA")
            {
                boolCheck = true;
                strSucess = "NODATA";
            }
            else if (status.ToUpper().Trim() == "INVALID DATE FORMAT")
            {
                boolCheck = true;
                strSucess = "INVALID DATE FORMAT";
            }
            else if (status.ToUpper().Trim() == "INITIATED")//INITIATED
            {
                boolCheck = true;
                strSucess = "INITIATED";
            }
            else if (status.ToUpper().Trim() == "PENDING FROM BANK")
            {
                boolCheck = true;
                strSucess = "PENDING FROM BANK";
            }
            else
            {
                boolCheck = true;
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Unable Connect Remote Server')", true);
            boolCheck = true;
        }
        return boolCheck;

    }
    public void updatePayment(string appNo, string collegeCode, string status, string txnid, string pgType, string mihpayid, string amtPaid)
    {
        try
        {
            collegecode = collegeCode;
            bool paidOk = false;
            double totAmount = 0;
            if (appNo != string.Empty && collegeCode != string.Empty && txnid != string.Empty)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegeCode);
                string receiptno = string.Empty;

                if (status.ToUpper().Trim() == "SUCCESS")
                {
                    #region insert paid
                    DataSet dsPayDetails = new DataSet();
                    string selQ = "select appNo,collegeCode,CONVERT(varchar(10),TransDate,101) as TransDate,feeCat,HeaderFk,ledgerFk,PayAmount,TransId,fineflag from OnlineFeeTransactionMaster om,OnlineFeeTransaction o where TransPk = TransFk and TransPk='" + txnid + "' and PaidStatus='0' and appNo='" + appNo + "'";
                    dsPayDetails = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsPayDetails.Tables.Count > 0 && dsPayDetails.Tables[0].Rows.Count > 0)
                    {
                        string acronym = string.Empty;
                        string hdrSetPK = string.Empty;
                        string hdrs = string.Empty;
                        #region Load Headers
                        for (int hdr = 0; hdr < dsPayDetails.Tables[0].Rows.Count; hdr++)
                        {
                            if (hdrs == string.Empty)
                            {
                                hdrs = Convert.ToString(dsPayDetails.Tables[0].Rows[hdr]["HeaderFk"]);
                            }
                            else
                            {
                                hdrs += "," + Convert.ToString(dsPayDetails.Tables[0].Rows[hdr]["HeaderFk"]);
                            }
                        }
                        #endregion
                        collegecode = collegeCode;
                        string transcode = generateReceiptNo(out acronym, out hdrSetPK, hdrs);
                        receiptno = transcode;
                        if (receiptno.Trim() != string.Empty)
                        {
                            for (int payRow = 0; payRow < dsPayDetails.Tables[0].Rows.Count; payRow++)
                            {
                                string transId = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["transId"]);
                                string transDate = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["TransDate"]);
                                double amt = 0; double.TryParse(Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["PayAmount"]), out amt);
                                totAmount += amt;
                                string hId = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["HeaderFk"]);
                                string lId = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["ledgerFk"]);
                                string feeCat = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["feeCat"]);
                                string fflag = Convert.ToString(dsPayDetails.Tables[0].Rows[payRow]["fineflag"]);//added by abarna
                                string iscollected = "1";
                                string collecteddate = DateTime.Now.ToString("MM/dd/yyyy");
                                string PayMode = "5";

                                //transaction
                                string insertDebit = "if not exists(select transdate,app_no,ledgerfk,headerfk,feecategory,finyearfk from ft_findailytransaction where app_no='" + appNo + "' and ledgerfk='" + lId + "' and headerfk='" + hId + "' and feecategory='" + feeCat + "' and debit='" + amt + "' and paymode='5') INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate) VALUES('" + transDate + "','" + DateTime.Now.ToLongTimeString() + "','" + receiptno + "', 1, " + appNo + ", " + lId + ", " + hId + ", " + feeCat + ", 0, " + (amt) + ", " + PayMode + ", '" + mihpayid + "', '', '','Online -" + pgType + "', 1, '0', 0, '', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'3','" + iscollected + "','" + collecteddate + "','" + iscollected + "','" + collecteddate + "')";

                                d2.update_method_wo_parameter(insertDebit, "Text");
                                //Update process

                                string selectquery = " select  isnull(TotalAmount,0) as TotalAmount,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount,feeallotpk  from FT_FeeAllot where App_No =" + appNo + " and feecategory ='" + feeCat + "' and ledgerfk ='" + lId + "'";

                                DataSet dsPrevAMount = new DataSet();
                                dsPrevAMount = d2.select_method_wo_parameter(selectquery, "Text");
                                if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
                                {
                                    double total = 0;
                                    double paidamt = 0;
                                    double balamt = 0;
                                    double feeallotpk = 0;
                                    total = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["TotalAmount"]);
                                    feeallotpk = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["feeallotpk"]);
                                    if (total > 0)
                                    {
                                        paidamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["PaidAmount"]);
                                        balamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["BalAmount"]);

                                        balamt = (total - paidamt);
                                        if (fflag == "0")
                                        {
                                            string updatequery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +" + (amt) + " ,BalAmount =" + (balamt - amt) + "  where App_No =" + appNo + " and feecategory ='" + feeCat + "' and ledgerfk ='" + lId + "'";

                                            d2.update_method_wo_parameter(updatequery, "Text");
                                        }
                                    }
                                    if (feeallotpk > 0)
                                    {
                                        string updatemon = " select isnull(AllotAmount,0) as AllotAmt,isnull(PaidAmount,0) as PaidAmount,isnull(AllotAmount,0)-isnull(PaidAmount,0) as BalAmount,AllotMonth,AllotYear,(select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend))) as finyearfk FROM fm_finyearmaster fm where a.finyearfk=fm.finyearpk )as finyear,a.finyearfk from FT_FeeallotMonthly a where FeeAllotPK=" + feeallotpk + " and balamount='" + amt + "' and allotamount='" + amt + "'";
                                        DataSet dsMonWiseDet = d2.select_method_wo_parameter(updatemon, "Text");
                                        if (dsMonWiseDet.Tables.Count > 0 && dsMonWiseDet.Tables[0].Rows.Count > 0)
                                        {
                                            #region Monthwise
                                            //string upMon wiseQ = "update FT_FeeallotMonthly set PaidAmount=ISNULL(PaidAmount,0)+" + (creditamt) + ",BalAmount=ISNULL(BalAmount,0)-" + (creditamt) + " where FeeAllotPK=" + feeallotpk + "  and  AllotMonth=" + monwisemon + " and  AllotYear=" + monWiseyea + "";

                                            for (int mon = 0; mon < dsMonWiseDet.Tables[0].Rows.Count; mon++)
                                            {
                                                //  DataRow dr_StudentMonth = tbl_Student.NewRow();

                                                //Added on 04-06-2016
                                                double totamtMon = 0;
                                                double paidMon = 0;
                                                double balamtMon = 0;

                                                double.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["AllotAmt"]), out totamtMon);
                                                double.TryParse(Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["PaidAmount"]), out paidMon);
                                                balamtMon = totamtMon - paidMon;

                                                //Excess Adjust

                                                //Scholarship adjust

                                                //Caution Deposit Adjust

                                                string month = string.Empty;
                                                string year = string.Empty;
                                                string finyearfK = string.Empty;
                                                string finyear = string.Empty;
                                                month = Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["AllotMonth"]);
                                                year = Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["AllotYear"]);


                                                //dr_StudentMonth["BalAmt"] = Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["BalAmount"]);

                                                //dr_StudentMonth["ToBePaid"] = "0";
                                                //dr_StudentMonth["Deduct"] = deductamt;
                                                finyearfK = Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["finyearfk"]);
                                                finyear = Convert.ToString(dsMonWiseDet.Tables[0].Rows[mon]["finyear"]);
                                                //dr_StudentMonth["finyearfk"] = finyearfK;

                                                string upMonwiseQ = "update FT_FeeallotMonthly set PaidAmount=ISNULL(PaidAmount,0)+" + (totamtMon) + ",BalAmount=ISNULL(BalAmount,0)-" + (totamtMon) + " where FeeAllotPK=" + feeallotpk + "  and  AllotMonth=" + month + " and  AllotYear=" + year + "";
                                                d2.update_method_wo_parameter(upMonwiseQ, "Text");
                                            }
                                            #endregion

                                        }
                                    }

                                }
                                //Update status
                                d2.update_method_wo_parameter("update OnlineFeeTransaction set PaidStatus='1' where transId='" + transId + "'", "Text");
                                d2.update_method_wo_parameter("update OnlineFeeTransactionMaster set TrackID='" + mihpayid + "' where Transpk='" + txnid + "' and appno='" + appNo + "'", "Text");
                            }
                            //Update receiptNo
                            #region Update Receipt No
                            transcode = transcode.Remove(0, acronym.Length);
                            int save1 = 0;
                            try
                            {
                                string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and college_code ='" + collegeCode + "' -- and user_code ='" + usercode + "' ";
                                save1 = Convert.ToInt32(d2.GetFunction(insqry1));

                            }
                            catch { save1 = 0; }
                            if (save1 != 5)
                            {
                                string updateRecpt = string.Empty;
                                if (isHeaderwise == 0 || isHeaderwise == 2)
                                {
                                    updateRecpt = " update FM_FinCodeSettings set RcptStNo=(" + transcode + "+1) where collegecode =" + collegeCode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegeCode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                }
                                else
                                {
                                    updateRecpt = "update FM_HeaderFinCodeSettings set RcptStNo=(" + transcode + "+1) where HeaderSettingPK in (" + hdrSetPK + ") and FinyearFK=" + finYearid + " and CollegeCode=" + collegeCode + " and isnull(onlineRec,'')<>'1'";
                                }
                                d2.update_method_wo_parameter(updateRecpt, "Text");
                            }
                            #endregion



                            paidOk = true;


                        }
                        //else
                        //{
                        //    //Update status
                        //    d2.update_method_wo_parameter("update OnlineFeeTransaction set PaidStatus='1' where TransFk='" + txnid + "' and PaidStatus='0' ", "Text");
                        //    d2.update_method_wo_parameter("update OnlineFeeTransactionMaster set TrackID='" + mihpayid + "' where Transpk='" + txnid + "' and appno='" + appNo + "'", "Text");
                        //}
                    }
                    #endregion
                }//added bu sudhagar 21/07/2017 
                else if ((status.ToUpper().Trim() != "FAILURE") || (status.ToUpper().Trim() != "failure"))
                {
                    d2.update_method_wo_parameter("update OnlineFeeTransaction set PaidStatus='2' where TransFk='" + txnid + "' and PaidStatus='0' ", "Text");
                    d2.update_method_wo_parameter("update OnlineFeeTransactionMaster set TrackID='" + mihpayid + "' where Transpk='" + txnid + "' and appno='" + appNo + "'", "Text");
                }
            }
        }
        catch { }
    }
    public string generateReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        int isHeaderwise = 0;
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
        }
        catch { isHeaderwise = 0; }
        try
        {
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 5)
                return string.Empty;
        }
        catch { return string.Empty; }
        if (isHeaderwise == 0 || isHeaderwise == 2)
        {
            return getCommonReceiptNo(out rcpracr, out hdrSetPK);
        }
        else
        {
            return getHeaderwiseReceiptNo(out rcpracr, out hdrSetPK, hdrs);
        }
    }

    private string getCommonReceiptNo(out string rcpracr, out string hdrSetPK)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);

            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;
                rcpracr = recacr;

                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
            }
            return recno;
        }
        catch (Exception ex) { return recno; }
    }

    private string getHeaderwiseReceiptNo(out string rcpracr, out string hdrSetPK, string hdrs)
    {
        hdrSetPK = string.Empty;
        rcpracr = string.Empty;
        string recno = string.Empty;

        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;
            string isheaderFk = hdrs;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);

            DataSet dsFinHedDet = d2.select_method_wo_parameter("select distinct HeaderSettingFk from FM_HeaderFinCodeSettingsDet hs,FM_HeaderFinCodeSettings s where s.HeaderSettingPK=hs.HeaderSettingFK and HeaderFK in (" + isheaderFk + ") and CollegeCode=" + collegecode + " and FinyearFK=" + finYearid + " and isnull(onlineRec,'')<>'1'", "Text");

            if (dsFinHedDet.Tables.Count > 0 && dsFinHedDet.Tables[0].Rows.Count == 1)
            {
                hdrSetPK = Convert.ToString(dsFinHedDet.Tables[0].Rows[0][0]);
                string secondreciptqurey = "select * from FM_HeaderFinCodeSettings where HeaderSettingPK =" + hdrSetPK + " and FinyearFK=" + finYearid + " and CollegeCode=" + collegecode + " and isnull(onlineRec,'')<>'1'";
                DataSet dsrecYr = new DataSet();
                dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
                if (dsrecYr.Tables.Count > 0 && dsrecYr.Tables[0].Rows.Count > 0)
                {
                    recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptStNo"]);
                    if (recnoprev != "")
                    {
                        int recno_cur = Convert.ToInt32(recnoprev);
                        receno = recno_cur;
                    }
                    recacr = Convert.ToString(dsrecYr.Tables[0].Rows[0]["RcptAcr"]);

                    int size = Convert.ToInt32(dsrecYr.Tables[0].Rows[0]["Rcptsize"]);

                    string recenoString = receno.ToString();

                    if (size != recenoString.Length && size > recenoString.Length)
                    {
                        while (size != recenoString.Length)
                        {
                            recenoString = "0" + recenoString;
                        }
                    }
                    recno = recacr + recenoString;
                    rcpracr = recacr;
                }
            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    public void DoubleVerificationCheck(string data)
    {
        NameValueCollection requestNameValue = new NameValueCollection();
        NameValueCollection nameValue = new NameValueCollection();

        nameValue.Add("merchant_code", "CRISTIAN_CLG");
        nameValue.Add("encdata", data);

        string responseMsg = PostRequest("https://merchant.onlinesbi.com/thirdparties/doubleverification.htm", nameValue);
        Console.Write(responseMsg);
        string msg = DecryptSBI(responseMsg, "E:\\CRISTIAN_COLLEGE_1.key");
        Console.Write(msg);
        //lbl_GetMessage.Text = msg;
        string[] s = msg.Split(new char[] { '|' });
        for (int i = 0; i < s.Length; i++)
        {
            string[] str = s[i].Split(new char[] { '=' });
            requestNameValue.Add(str[0], str[1]);
        }
        Console.Write(requestNameValue);

    }
    public static string PostRequest(string uri, NameValueCollection pairs)
    {
        byte[] response = null;
        using (WebClient client = new WebClient())
        {
            response = client.UploadValues(uri, pairs);
        }
        return System.Text.Encoding.UTF8.GetString(response);
    }
    public string GetMD5Hashname(string name)
    {

        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] ba = System.Text.Encoding.ASCII.GetBytes(name);
        ba = md5.ComputeHash(ba);
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
        {
            hex.AppendFormat("{0:x2}", b);
        }
        return hex.ToString();
    }
    public string EncryptSBI(string textToEncrypt, string FilePath)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = GetFileBytesSBI(FilePath);
        byte[] keyBytes = new byte[(16)];
        int len = pwdBytes.Length;
        if ((len > keyBytes.Length))
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }

    public string DecryptSBI(string textToDecrypt, string FilePath)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = GetFileBytesSBI(FilePath);
        byte[] keyBytes = new byte[(16)];
        int len = pwdBytes.Length;
        if ((len > keyBytes.Length))
        {
            len = keyBytes.Length;
        }
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    private byte[] GetFileBytesSBI(string filePath)
    {
        byte[] buffer = null;
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            int length = Convert.ToInt32(fileStream.Length);
            buffer = new byte[(length)];
            int count = 0;
            int sum = 0;

            while ((fileStream.Read(buffer, sum, (length - sum)) > 0))
            {
                sum = (sum + count);
            }
        }
        finally
        {
            fileStream.Close();
        }
        return buffer;
    }
    //-------------------------End of the function-------abarna
    public static string byteToHexString(byte[] byData)
    {
        StringBuilder sb = new StringBuilder((byData.Length * 2));
        for (int i = 0; (i < byData.Length); i++)
        {
            int v = (byData[i] & 255);
            if ((v < 16))
            {
                sb.Append('0');
            }

            sb.Append(v.ToString("X"));

        }

        return sb.ToString();
    }
}