using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;

public partial class Login : System.Web.UI.Page
{
    const int basekey = 43;
    const int addtokey = 17;

    DAccess2 da = new DAccess2();

    SqlConnection fcon = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con6 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con7 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection Fincon = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection conobj = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlCommand cmd = new SqlCommand();

    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();

    string strworkingkey = "", strsenderid = "";
    string collegecode = string.Empty;
    Hashtable hat = new Hashtable();

    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strsmsuserid = string.Empty;
        lblmsgcredit.Visible = false;
        lblmsgcredit.Text = "";


        if (Session["IsLogin"] != null) //Aruna For another user login
        {
            if (Session["IsLogin"].ToString() == "1")
            {
                Response.Redirect("Login_Check.aspx");
            }
        }

        if (!IsPostBack)
        {

            Session["column_header_row_count"] = "";
            if (Request.Cookies["UName"] != null)
                txtuname.Text = Request.Cookies["UName"].Value;
            if (Request.Cookies["PWD"] != null)
                txtpassword.Attributes.Add("value", Request.Cookies["PWD"].Value);
            if (Request.Cookies["UName"] != null && Request.Cookies["PWD"] != null)
                CheckBox1.Checked = true;

            try
            {
                string colcount = "";

                colcount = GetFunction("select distinct count(college_code) from collinfo");
                img_college.ImageUrl = "Handler/Handler.ashx?";
                img_com.ImageUrl = "Handler/Handler6.ashx?";
                SqlCommand cmd = new SqlCommand();
                if (Convert.ToInt16(colcount) > 1)
                {
                    cmd.CommandText = "select  top 1 isnull(com_name,'')as com_name,banner,college_code from collinfo where com_name is not null and com_name<>''";
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader MyReader = cmd.ExecuteReader();
                    if (MyReader.Read())
                    {
                        if (MyReader.HasRows == true)
                        {
                            collegename.Text = MyReader.GetValue(0).ToString();
                            clgename.Text = MyReader.GetValue(0).ToString();
                            collegecode = MyReader.GetValue(2).ToString();
                            Session["clgcode"] = collegecode;

                            if ((MyReader.GetValue(1).ToString() == "") || (MyReader.GetValue(1).ToString() == null))
                            {
                                collegename.Visible = true;
                            }
                            else
                            {
                                collegename.Visible = false;
                            }
                        }
                    }
                    MyReader.Close();
                    con.Close();
                }
                else
                {
                    cmd.CommandText = "select  top 1 collname,category,affliatedby,address1,address3,phoneno,pincode,faxno,banner,college_code from collinfo";
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader MyReader = cmd.ExecuteReader();
                    if (MyReader.Read())
                    {
                        collegename.Text = MyReader.GetValue(0).ToString();
                        clgename.Text = MyReader.GetValue(0).ToString();
                        collegecode = MyReader.GetValue(9).ToString();
                        Session["clgcode"] = collegecode;

                        address.Text = MyReader.GetValue(3).ToString() + ", " + MyReader.GetValue(4).ToString() + " - " + MyReader.GetValue(6).ToString() + "   Ph : " + MyReader.GetValue(5).ToString() + "   FAX : " + MyReader.GetValue(7).ToString();
                        category.Text = MyReader.GetValue(1).ToString() + " Institution Affiliated to " + MyReader.GetValue(2).ToString();

                        if ((MyReader.GetValue(8).ToString() == "") || (MyReader.GetValue(8).ToString() == null))
                        {
                            collegename.Visible = true;
                            address.Visible = true;
                            category.Visible = true;
                        }
                        else
                        {
                            collegename.Visible = false;
                            address.Visible = false;
                            category.Visible = false;
                        }
                    }

                    MyReader.Close();
                    con.Close();
                }
            }
            catch
            {
            }
        }

        try
        {
            //string strsenderquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + Session["clgcode"].ToString() + "'";
            //ds1.Dispose();
            //ds1.Reset();
            //ds1 = d2.select_method(strsenderquery, hat, "Text");
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            //}
            //GetUserapi(user_id);


            //if (SenderID != "" && Password != "")
            //{
            //    lblmsgcredit.Visible = true;                
            //    WebRequest request = WebRequest.Create("http://inter.onlinespeedsms.in/api/balance.php?user=" + user_id.ToLower() + "&password=" + Password + "&type=4");
            //    WebResponse response = request.GetResponse();
            //    Stream data = response.GetResponseStream();
            //    StreamReader sr = new StreamReader(data);
            //    string strvel = sr.ReadToEnd();

            //    lblmsgcredit.Text = strvel.ToString();
            //    string[] strrrvel = strvel.Split(' ');
            //    int getuprbnd = strrrvel.GetUpperBound(0);
            //    lblmsgcredit.Text = "SMS Available Credits :" + strrrvel[getuprbnd];
            //}
        }
        catch
        {
        }

    }
    //modified by srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        } 
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }

    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            strsenderid = "AUDIIT";
    //            strworkingkey = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            strsenderid = "SAENGG";
    //            strworkingkey = "SAENGG";
    //        }

    //        else if (user_id == "STANE")
    //        {
    //            strsenderid = "STANES";
    //            Password = "STANES";
    //        }

    //        else if (user_id == "MBCBSE")
    //        {
    //            strsenderid = "MBCBSE";
    //            strworkingkey = "MBCBSE";
    //        }

    //        else if (user_id == "HIETPT")
    //        {
    //            strsenderid = "HIETPT";
    //            strworkingkey = "HIETPT";
    //        }

    //        else if (user_id == "SVPITM")
    //        {
    //            strsenderid = "SVPITM";
    //            strworkingkey = "SVPITM";
    //        }

    //        else if (user_id == "AUDCET")
    //        {
    //            strsenderid = "AUDCET";
    //            strworkingkey = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            strsenderid = "AUDWOM";
    //            strworkingkey = "AUDWOM";
    //        }

    //        else if (user_id == "AUDIPG")
    //        {
    //            strsenderid = "AUDIPG";
    //            strworkingkey = "AUDIPG";
    //        }

    //        else if (user_id == "MCCDAY")
    //        {
    //            strsenderid = "MCCDAY";
    //            strworkingkey = "MCCDAY";
    //        }

    //        else if (user_id == "MCCSFS")
    //        {
    //            strsenderid = "MCCSFS";
    //            strworkingkey = "MCCSFS";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }

    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }

    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }

    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }

    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }

    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }

    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }

    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }

    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }

    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }

    //        else if (user_id == "SCHCLG")
    //        {
    //            SenderID = "SCHCLG";
    //            Password = "SCHCLG";
    //        }

    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }

    //        else if (user_id == "SRECTD")
    //        {
    //            SenderID = "SRECTD";
    //            Password = "SRECTD";
    //        }
    //        else if (user_id == "EICTPC")
    //        {
    //            SenderID = "EICTPC";
    //            Password = "EICTPC";
    //        }
    //        else if (user_id == "SHACLG")
    //        {
    //            SenderID = "SHACLG";
    //            Password = "SHACLG";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "TECAAA")
    //        {
    //            SenderID = "TECAAA";
    //            Password = "TECAAA";
    //        }
    //        else if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVISTE")
    //        {
    //            SenderID = "SVISTE";
    //            Password = "SVISTE";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //         else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }

    //    else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //  else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //   else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //  else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //   else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //    else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //    else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }



    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }


    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public int getmaxdays(int mno, int year)
    {

        int maxdays = 0;
        if ((mno == 2) && (year % 4 == 0))
        {
            maxdays = 29;
            return maxdays;
        }


        else if ((mno == 1) || (mno == 3) || (mno == 5) || (mno == 7) || (mno == 8) || (mno == 10) || (mno == 12))
        {
            maxdays = 31;
            return maxdays;
        }
        else if ((mno == 4) || (mno == 6) || (mno == 9) || (mno == 11))
        {
            maxdays = 30;
            return maxdays;
        }

        else if ((mno == 2) || (year % 4) != 0)
        {
            maxdays = 28;
            return maxdays;
        }
        return maxdays;
    }
    public string getcurrentyear(string sem)
    {
        int sem1 = 0;
        int cursem = int.Parse(sem);
        if (cursem % 2 == 0)
        {
            sem1 = cursem / 2;
        }
        else
        {
            sem1 = (cursem + 1) / 2;
        }
        return sem1 + "";
    }
    public void advancepayment(int collegecode)
    {
        Boolean movetonextrec = false;
        string tmp_rcpt_no = "";
        int chk_month = 0;
        con.Close();
        con.Open();
        SqlDataAdapter da20 = new SqlDataAdapter("select * from rcptprint_settings where collegecode=" + Session["collegecode"].ToString() + "", con);
        DataSet ds20 = new DataSet();
        da20.Fill(ds20);
        if (ds20.Tables[0].Rows.Count > 0)
        {
            chk_month = (ds20.Tables[0].Rows[0]["get_monthly_fee"].ToString() == "True") ? 1 : 0;
        }

        string rcptnum = "";
        con1.Close();
        con1.Open();
        string sqlOthers = "select  fee_code,adv_date,mode from advance_payment_settings where collegecode=" + collegecode + "";
        SqlDataAdapter rss = new SqlDataAdapter(sqlOthers, con1);
        DataSet rs = new DataSet();
        rss.Fill(rs);
        if (rs.Tables[0].Rows.Count > 0)
        {
            for (int setval = 0; setval < rs.Tables[0].Rows.Count; setval++)
            {

                string adv_date = rs.Tables[0].Rows[setval]["adv_date"].ToString();
                int max_date = getmaxdays(DateTime.Now.Month, DateTime.Now.Year);
                if ((rs.Tables[0].Rows[setval]["adv_date"].ToString() == "31"))
                {
                    adv_date = max_date.ToString();
                }
                if (DateTime.Now.Month == 2)
                {
                    if ((rs.Tables[0].Rows[setval]["adv_date"].ToString() == "31") || (rs.Tables[0].Rows[setval]["adv_date"].ToString() == "30") || (rs.Tables[0].Rows[setval]["adv_date"].ToString() == "29"))
                    {
                        adv_date = max_date.ToString();
                    }
                }

                if ((Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Monthwise") && (DateTime.Now.Day >= int.Parse(adv_date)))
                {
                    goto l1;
                }
                else if ((Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Monthwise") && (DateTime.Now.Day < int.Parse(adv_date)))
                {
                    goto l2;
                }

            l1: string feecode = rs.Tables[0].Rows[setval]["fee_code"].ToString();
                mysql1.Close();
                mysql1.Open();
                sqlOthers = "select  distinct sum(balance) as balance,roll_admit from advance_payment where debit=0 and college_code=" + collegecode + " and balance<>0 group by roll_admit";
                SqlDataAdapter rssqlOthers = new SqlDataAdapter(sqlOthers, mysql1);
                DataSet rsFinDelete = new DataSet();
                rssqlOthers.Fill(rsFinDelete);
                if (rsFinDelete.Tables[0].Rows.Count > 0)
                {
                    for (int ival = 0; ival < rsFinDelete.Tables[0].Rows.Count; ival++)
                    {
                        if (rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString().Trim() != "")
                        {
                            string currsem = GetFunction("select distinct current_semester from registration where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "'");
                            if (currsem.Trim().ToString() != "")
                            {
                                string curyear = getcurrentyear(currsem);
                                string currsem1 = currsem + " Semester";
                                string curyear1 = curyear + " Year";
                                if (currsem != "")
                                    con1.Close();
                                con1.Open();
                                SqlCommand moders = new SqlCommand("select distinct textcode,textval from textvaltable where textcriteria='FEECA' and (textval like '" + currsem1 + "' or textval like '" + curyear1 + "')  and college_code=" + collegecode + "", con1);
                                SqlDataReader mode_rs = moders.ExecuteReader();
                                currsem = "";
                                while (mode_rs.Read())
                                {
                                    if (mode_rs.HasRows == true)
                                    {
                                        if (currsem == "")
                                            currsem = mode_rs["textcode"].ToString();
                                        else
                                            currsem = currsem + "," + mode_rs["textcode"].ToString();
                                    }
                                }

                                int cur_mon = DateTime.Now.Month;
                                if (chk_month == 1 && cur_mon != 1)
                                    cur_mon = cur_mon - 1;

                                mysql1.Close();
                                mysql1.Open();
                                DataSet rsFin = new DataSet();
                                double amt = 0;
                                string cur_sem_new = "";
                                double paid_Prev_amt = 0;
                                if (Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Monthwise")
                                {

                                    mysql.Close();
                                    mysql.Open();
                                    sqlOthers = "select fee_monamt,fee_amount,fee_category,total from fee_allotmonthly where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and fee_code=" + feecode + " and fee_category in (" + currsem + ")";
                                    SqlDataAdapter rssql = new SqlDataAdapter(sqlOthers, mysql);
                                    rsFin.Reset();
                                    rsFin.Clear();
                                    rsFin.Dispose();
                                    rssql.Fill(rsFin);
                                    if (rsFin.Tables[0].Rows.Count > 0)
                                    {
                                        cur_sem_new = rsFin.Tables[0].Rows[0]["fee_category"].ToString();
                                        string fee_monamt = rsFin.Tables[0].Rows[0][0].ToString();
                                        string[] monamt = fee_monamt.Split('/');

                                        for (int i = 0; i < monamt.GetUpperBound(0); i++)
                                        {
                                            string[] feemon = monamt[i].Split(';');
                                            if (int.Parse(feemon[0]) == cur_mon)
                                            {
                                                amt = Convert.ToDouble(feemon[1]);
                                                i = monamt.GetUpperBound(0);
                                            }
                                        }

                                        mysql.Close();
                                        mysql.Open();
                                        sqlOthers = "select  isnull(sum(credit),0) as paid from advance_payment_transaction where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and  debit=0 and college_code=" + Session["collegecode"] + " and fee_code=" + feecode + " and month=" + cur_mon + " and fee_category in (" + cur_sem_new + ")";
                                        SqlDataAdapter rssq = new SqlDataAdapter(sqlOthers, mysql1);
                                        DataSet rsF = new DataSet();
                                        rssq.Fill(rsF);
                                        if (rsF.Tables[0].Rows.Count > 0)
                                        {
                                            paid_Prev_amt = double.Parse(rsF.Tables[0].Rows[0]["paid"].ToString());
                                        }
                                    }
                                    if (amt > 0)
                                    {
                                        if (paid_Prev_amt > 0)
                                        {
                                            if (paid_Prev_amt >= amt)
                                            {
                                                goto l4;
                                            }
                                            else if (amt > paid_Prev_amt)
                                            {
                                                amt = amt - paid_Prev_amt;
                                            }
                                        }

                                    }
                                }
                                else if (Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Regular")
                                {
                                    mysql.Close();
                                    mysql.Open();
                                    sqlOthers = "select distinct fee_allot.fee_code,fee_info.fee_type,isnull(fee_allot.fee_amount,0)-isnull(fee_allot.deduct,0)[fee_amount],fee_info.header_id,fee_allot.fee_category,textvaltable.textval,fee_allot.allotdate,(select  top 1 balance from stud_payment_trans where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "'  and stud_payment_trans.fee_code=fee_allot.fee_code and stud_payment_trans.fee_category=fee_allot.fee_category";
                                    sqlOthers = sqlOthers + " and stud_payment_trans.trans_id=(select max(trans_id) from stud_payment_trans where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and stud_payment_trans.fee_code=fee_allot.fee_code and stud_payment_trans.fee_category=fee_allot.fee_category )) as total,";
                                    sqlOthers = sqlOthers + " DueDate,fee_allot.flag_status,fee_allot.fine,fee_allot.modeofpay  from fee_allot,fee_info,fee_status,acctheader,acctinfo,textvaltable,ledger_info where fee_allot.fee_code=fee_info.fee_code  and ledger_info.fee_code=fee_info.fee_code   and fee_allot.roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "'   and fee_status.roll_admit=fee_allot.roll_admit ";
                                    sqlOthers = sqlOthers + " and fee_info.header_id=acctheader.header_id and acctheader.acct_id=acctinfo.acct_id  and acctinfo.college_code=13 and textvaltable.textcode=fee_allot.fee_category    and isnull(ledger_info.credit_status,1)=1 and (textvaltable.textcriteria='FEECA' or textvaltable.textcriteria='ExmCt' or textvaltable.textcriteria='TranF')";
                                    sqlOthers = sqlOthers + " and fee_allot.fee_code=" + feecode + " and fee_allot.fee_category in (" + currsem + ")  and fee_allot.modeofpay='Regular'  and fee_allot.flag_status='false' and DueDate<>'' and DueDate is not null order by fee_allot.fee_category,fee_info.header_id";
                                    SqlDataAdapter rssql = new SqlDataAdapter(sqlOthers, mysql);
                                    rsFin.Reset();
                                    rsFin.Clear();
                                    rsFin.Dispose();
                                    rssql.Fill(rsFin);

                                    if (rsFin.Tables[0].Rows.Count > 0)
                                    {

                                        //"8/4/2012 12:00:00 AM"
                                        string due_date = "";
                                        string sysdate = "";
                                        sysdate = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;

                                        if (Convert.ToString(rsFin.Tables[0].Rows[0]["DueDate"]) != "")
                                        {
                                            string duedate = Convert.ToString(rsFin.Tables[0].Rows[0]["DueDate"]);
                                            string[] fin_due = duedate.Split(new Char[] { ' ' });
                                            if (fin_due.GetUpperBound(0) > 0)
                                            {
                                                string[] fin_due1 = fin_due[0].Split(new Char[] { '/' });
                                                due_date = fin_due1[0] + '/' + fin_due1[1] + '/' + fin_due1[2];
                                            }
                                        }



                                        if (Convert.ToDateTime(sysdate) >= Convert.ToDateTime(due_date))
                                        {
                                            goto l3;
                                        }
                                        else if (Convert.ToDateTime(sysdate) < Convert.ToDateTime(due_date))
                                        {
                                            goto l4;
                                        }
                                    l3: cur_sem_new = rsFin.Tables[0].Rows[0]["fee_category"].ToString();
                                        if ((rsFin.Tables[0].Rows[0]["total"].ToString() != "") && (rsFin.Tables[0].Rows[0]["total"].ToString() != "0"))
                                        {
                                            amt = Convert.ToDouble(rsFin.Tables[0].Rows[0]["total"].ToString());
                                        }
                                        else
                                        {
                                            amt = Convert.ToDouble(rsFin.Tables[0].Rows[0]["fee_amount"].ToString());
                                        }
                                    }
                                }
                                if (amt > 0)
                                {

                                    double adv_bal = double.Parse(rsFinDelete.Tables[0].Rows[ival]["balance"].ToString());
                                    double new_bal = 0, bal_trans = 0;
                                    double paid = 0;
                                    string trans_date = "", college_code = "", cal_date = "", name = "", description = "", credit = "", debit = "", mode = "", bankname = "", ddno = "", dddate = "", voucherno = "", vouchertype = "", fee_code = "", ledger_code = "", collected = "", collected_date = "", studorothers = "", fee_category = "", staff_code = "", branch = "", receiptcancel = "", accno = "", depositat = "", Deposit = "", user_code = "", loan_self = "", fine = "", installment = "", bankoroffice = "", enq_no = "", Additional_purchase = "";
                                    double actual_paid = 0;
                                    double update_balance = 0;
                                    double tmp_amt = 0;
                                    string rcpt_str = "";
                                    string rcpt_str1 = "";
                                    conobj.Close();
                                    conobj.Open();

                                    sqlOthers = "select * from advance_payment where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and debit=0 and college_code=" + collegecode + " and balance<>0 order by cal_date,len(voucherno),voucherno";
                                    SqlDataAdapter newad = new SqlDataAdapter(sqlOthers, conobj);
                                    DataSet newrs = new DataSet();
                                    newad.Fill(newrs);
                                    if (adv_bal >= amt)
                                    {
                                        adv_bal = amt;
                                        credit = Convert.ToString(amt);
                                    }
                                    else if (adv_bal < amt)
                                    {
                                        credit = Convert.ToString(adv_bal);
                                    }

                                    string sysdate1 = "";
                                    sysdate1 = DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year;
                                    string voucher_no_str = "";
                                    voucher_no_str = GetFunction("select distinct voucherno from advance_payment_transaction where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and cal_date='" + sysdate1 + "'");

                                    if (voucher_no_str == "")
                                    {
                                        rcptnum = Auto_rcptno(rs.Tables[0].Rows[setval]["fee_code"].ToString());

                                    }
                                    else
                                    {
                                        rcptnum = voucher_no_str.ToString();

                                    }
                                    if (rcptnum.ToString().Trim() != "")
                                    {
                                        update_balance = adv_bal;
                                        tmp_amt = amt;
                                        if (newrs.Tables[0].Rows.Count > 0)
                                        {
                                            for (int newrsval = 0; newrsval < newrs.Tables[0].Rows.Count; newrsval++)
                                            {
                                                double tmp_bal = double.Parse(newrs.Tables[0].Rows[newrsval]["balance"].ToString());
                                                rcpt_str = "";
                                                if (tmp_amt == 0)
                                                    goto l7;
                                                if (tmp_amt >= tmp_bal)
                                                {
                                                    paid = double.Parse(newrs.Tables[0].Rows[newrsval]["credit"].ToString());
                                                    rcpt_str = Convert.ToString(newrs.Tables[0].Rows[newrsval]["adjust_rcptno"]);

                                                    if ((rcpt_str.ToString().Trim() == "") && (rcptnum.ToString().Trim() != ""))
                                                    {
                                                        rcpt_str = rcptnum.ToString();
                                                        tmp_rcpt_no = rcptnum.ToString();
                                                    }
                                                    else if ((rcpt_str.ToString().Trim() != "") && (rcptnum.ToString().Trim() != ""))
                                                    {
                                                        if ((tmp_rcpt_no.ToString() != rcptnum.ToString()) && (rcpt_str.ToString() != rcptnum.ToString()))
                                                        {
                                                            string isvoucherno = "";
                                                            isvoucherno = GetFunction("select count(*) from advance_payment  where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and adjust_rcptno like '%" + rcptnum.ToString() + "%'");
                                                            if ((isvoucherno.ToString() == "") || (isvoucherno.ToString() == "0"))
                                                            {
                                                                rcpt_str = rcpt_str + "," + rcptnum.ToString();
                                                            }
                                                        }
                                                    }

                                                    new_bal = 0;
                                                    tmp_amt = tmp_amt - tmp_bal;
                                                    actual_paid = tmp_bal; //paid;
                                                    con.Close();
                                                    con.Open();
                                                    SqlCommand fincon12 = new SqlCommand("update advance_payment set balance=0,paid=" + paid + " , adjust_rcptno='" + rcpt_str + "' where roll_admit='" + newrs.Tables[0].Rows[newrsval]["roll_admit"].ToString() + "' and fee_code=" + newrs.Tables[0].Rows[newrsval]["fee_code"].ToString() + " and voucherno='" + newrs.Tables[0].Rows[newrsval]["voucherno"].ToString() + "' and college_code=" + collegecode + "", con);
                                                    fincon12.ExecuteNonQuery();
                                                }
                                                else if (tmp_bal >= tmp_amt)
                                                {
                                                    if (Convert.ToString(newrs.Tables[0].Rows[newrsval]["paid"]) != "")
                                                        paid = Convert.ToDouble(newrs.Tables[0].Rows[newrsval]["paid"].ToString()) + tmp_amt;
                                                    else
                                                        paid = Convert.ToDouble(tmp_amt);

                                                    rcpt_str = Convert.ToString(newrs.Tables[0].Rows[newrsval]["adjust_rcptno"]);

                                                    if ((rcpt_str.ToString().Trim() == "") && (rcptnum.ToString().Trim() != ""))
                                                    {
                                                        rcpt_str = rcptnum.ToString();
                                                        tmp_rcpt_no = rcptnum.ToString();
                                                    }
                                                    else if ((rcpt_str.ToString().Trim() != "") && (rcptnum.ToString().Trim() != ""))
                                                    {
                                                        if ((tmp_rcpt_no.ToString() != rcptnum.ToString()) && (rcpt_str.ToString() != rcptnum.ToString()))
                                                        {
                                                            string isvoucherno = "";
                                                            isvoucherno = GetFunction("select count(*) from advance_payment  where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and adjust_rcptno like '%" + rcptnum.ToString() + "%'");
                                                            if ((isvoucherno.ToString() == "") || (isvoucherno.ToString() == "0"))
                                                            {
                                                                rcpt_str = rcpt_str + "," + rcptnum.ToString();
                                                            }
                                                        }
                                                    }
                                                    new_bal = tmp_bal - tmp_amt;
                                                    actual_paid = tmp_amt;
                                                    tmp_amt = 0;
                                                    con.Close();
                                                    con.Open();
                                                    SqlCommand fincon12 = new SqlCommand("update advance_payment set balance=" + new_bal + ",paid=" + paid + " , adjust_rcptno='" + rcpt_str + "' where roll_admit='" + newrs.Tables[0].Rows[newrsval]["roll_admit"].ToString() + "' and fee_code=" + newrs.Tables[0].Rows[newrsval]["fee_code"].ToString() + " and voucherno='" + newrs.Tables[0].Rows[newrsval]["voucherno"].ToString() + "' and college_code=" + collegecode + "", con);
                                                    fincon12.ExecuteNonQuery();
                                                }



                                                if ((rcpt_str1.ToString().Trim() == "") && (newrs.Tables[0].Rows[newrsval]["voucherno"].ToString().Trim() != ""))
                                                {
                                                    rcpt_str1 = newrs.Tables[0].Rows[newrsval]["voucherno"].ToString() + ":" + actual_paid;
                                                }
                                                else if ((rcpt_str1.ToString().Trim() != "") && (newrs.Tables[0].Rows[newrsval]["voucherno"].ToString().Trim() != ""))
                                                {
                                                    rcpt_str1 = rcpt_str1 + "," + newrs.Tables[0].Rows[newrsval]["voucherno"].ToString() + ":" + actual_paid;
                                                }


                                                college_code = newrs.Tables[0].Rows[newrsval]["college_code"].ToString();
                                                cal_date = "";
                                                name = newrs.Tables[0].Rows[newrsval]["name"].ToString();
                                                description = newrs.Tables[0].Rows[newrsval]["description"].ToString();
                                                debit = "";
                                                mode = newrs.Tables[0].Rows[newrsval]["mode"].ToString();
                                                bankname = newrs.Tables[0].Rows[newrsval]["bankname"].ToString();
                                                ddno = newrs.Tables[0].Rows[newrsval]["ddno"].ToString();
                                                dddate = newrs.Tables[0].Rows[newrsval]["dddate"].ToString();
                                                voucherno = "";
                                                vouchertype = newrs.Tables[0].Rows[newrsval]["vouchertype"].ToString();
                                                fee_code = newrs.Tables[0].Rows[newrsval]["fee_code"].ToString();
                                                ledger_code = newrs.Tables[0].Rows[newrsval]["ledger_code"].ToString();
                                                collected = newrs.Tables[0].Rows[newrsval]["collected"].ToString();
                                                collected_date = newrs.Tables[0].Rows[newrsval]["collected_date"].ToString();
                                                studorothers = newrs.Tables[0].Rows[newrsval]["studorothers"].ToString();
                                                //fee_category = newrs.Tables[0].Rows[newrsval]["fee_category"].ToString();
                                                fee_category = cur_sem_new.ToString();
                                                staff_code = newrs.Tables[0].Rows[newrsval]["staff_code"].ToString();
                                                branch = newrs.Tables[0].Rows[newrsval]["branch"].ToString();
                                                receiptcancel = newrs.Tables[0].Rows[newrsval]["roll_admit"].ToString();
                                                accno = newrs.Tables[0].Rows[newrsval]["accno"].ToString();
                                                depositat = newrs.Tables[0].Rows[newrsval]["depositat"].ToString();
                                                Deposit = newrs.Tables[0].Rows[newrsval]["Deposit"].ToString();
                                                user_code = newrs.Tables[0].Rows[newrsval]["user_code"].ToString();
                                                loan_self = newrs.Tables[0].Rows[newrsval]["loan_self"].ToString();
                                                fine = newrs.Tables[0].Rows[newrsval]["fine"].ToString();
                                                installment = newrs.Tables[0].Rows[newrsval]["installment"].ToString();
                                                bankoroffice = newrs.Tables[0].Rows[newrsval]["bankoroffice"].ToString();
                                                enq_no = newrs.Tables[0].Rows[newrsval]["enq_no"].ToString();
                                                Additional_purchase = newrs.Tables[0].Rows[newrsval]["Additional_purchase"].ToString();
                                                trans_date = newrs.Tables[0].Rows[newrsval]["trans_date"].ToString();

                                            }

                                        }

                                    l7: cal_date = DateTime.Now.Date.ToString();
                                        voucherno = rcptnum;
                                        collected = "1";
                                        con.Close();
                                        con.Open();
                                        SqlCommand fincon111 = new SqlCommand("insert into advance_payment_transaction (trans_date,college_code,cal_date,name,description,credit,debit,mode,bankname,ddno,dddate,voucherno,vouchertype,fee_code,ledger_code,collected,collected_date,studorothers,fee_category,staff_code,branch,receiptcancel,accno,depositat,Deposit,user_code,loan_self,fine,installment,bankoroffice,enq_no,Additional_purchase,month,roll_admit,advance_rcptno) values ('" + trans_date + "'," + college_code + ",'" + cal_date + "','" + name + "','" + description + "'," + credit + ",0,'" + mode + "','" + bankname + "','" + ddno + "','" + dddate + "','" + voucherno + "',1," + feecode + "," + ledger_code + "," + collected + ",''," + studorothers + "," + fee_category + ",'" + staff_code + "','" + branch + "',1,'',''," + Deposit + "," + user_code + ",'" + loan_self + "'," + fine + "," + installment + ",'" + bankoroffice + "','" + enq_no + "','" + Additional_purchase + "'," + cur_mon + ",'" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "','" + rcpt_str1 + "')", con);
                                        fincon111.ExecuteNonQuery();

                                        con.Close();
                                        con.Open();
                                        SqlCommand fincon222 = new SqlCommand("insert into dailytransaction (trans_date,college_code,cal_date,name,description,credit,debit,mode,bankname,ddno,dddate,voucherno,vouchertype,fee_code,ledger_code,collected,collected_date,studorothers,fee_category,staff_code,branch,receiptcancel,accno,depositat,Deposit,user_code,loan_self,fine,installment,bankoroffice,enq_no,Additional_purchase,adjust_flag,roll_admit,advance_rcptno) values ('" + trans_date + "'," + college_code + ",'" + cal_date + "','" + name + "','" + description + "'," + credit + ",0,'" + mode + "','" + bankname + "','" + ddno + "','" + dddate + "','" + voucherno + "',1," + feecode + "," + ledger_code + "," + collected + ",''," + studorothers + "," + fee_category + ",'" + staff_code + "','" + branch + "',1,'',''," + Deposit + "," + user_code + ",'" + loan_self + "'," + fine + "," + installment + ",'" + bankoroffice + "','" + enq_no + "','" + Additional_purchase + "','" + false + "','" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "','" + rcpt_str1 + "')", con);
                                        fincon222.ExecuteNonQuery();




                                        string headeridd = GetFunction("select header_id from fee_info where fee_code=" + feecode + "");
                                        //-------------------------------------------------------------------------------------
                                        string Prev_balance = "0";
                                        con.Close();
                                        con.Open();
                                        sqlOthers = "select max(trans_id),balance from stud_payment where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and header_id=" + headeridd + "  group by header_id,balance";
                                        SqlDataAdapter da = new SqlDataAdapter(sqlOthers, con);
                                        DataSet ds = new DataSet();
                                        da.Fill(ds);
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            Prev_balance = ds.Tables[0].Rows[0]["balance"].ToString();
                                        }
                                        else
                                        {
                                            con.Close();
                                            da.Dispose();
                                            ds.Clear();
                                            ds.Dispose();
                                            ds.Reset();
                                            con.Open();
                                            sqlOthers = "select isnull(sum(total),0) as total from fee_allot ft,fee_info f where ft.fee_code=f.fee_code and ft.roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and f.header_id=" + headeridd + "";
                                            SqlDataAdapter da1 = new SqlDataAdapter(sqlOthers, con);
                                            da1.Fill(ds);
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                Prev_balance = ds.Tables[0].Rows[0]["total"].ToString();
                                            }

                                        }
                                        string final_balance = "0";
                                        if (Convert.ToDouble(Prev_balance.ToString()) >= Convert.ToDouble(credit.ToString()))
                                        {
                                            final_balance = Convert.ToString(Convert.ToDouble(Prev_balance.ToString()) - Convert.ToDouble(credit.ToString()));
                                        }

                                        //------------------------------------------------------------------------------------
                                        con.Close();
                                        con.Open();
                                        SqlCommand fincon333 = new SqlCommand("insert into stud_payment (trans_date,college_code,roll_admit,amount,cal_date,paid,balance,mode,ddno,dddate,bank,branch,remarks,rcpt_no,fin_userid,fine,receiptcancel,user_code,Additional_purchase,header_id ) values ('" + cal_date + "'," + Session["collegecode"].ToString() + ",'" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "'," + Prev_balance.ToString() + ",'" + cal_date + "'," + credit + "," + final_balance + ",'" + mode + "','" + ddno + "','" + dddate + "','" + bankname + "','" + branch + "','','" + voucherno + "','" + Session["usercode"].ToString() + "','',1," + Session["usercode"].ToString() + ",''," + headeridd + ")", con);
                                        fincon333.ExecuteNonQuery();

                                        Prev_balance = "0";
                                        final_balance = "0";
                                        con.Close();
                                        da.Dispose();
                                        ds.Clear();
                                        ds.Dispose();
                                        ds.Reset();
                                        con.Open();
                                        sqlOthers = "select max(trans_id),balance from stud_payment_trans where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and fee_code=" + feecode + " and fee_category=" + cur_sem_new + " and college_code=" + Session["collegecode"].ToString() + "  group by fee_code,fee_category,balance";
                                        SqlDataAdapter da2 = new SqlDataAdapter(sqlOthers, con);
                                        da2.Fill(ds);
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            Prev_balance = ds.Tables[0].Rows[0]["balance"].ToString();
                                        }
                                        else
                                        {
                                            if (Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Regular")
                                            {
                                                Prev_balance = rsFin.Tables[0].Rows[0]["fee_amount"].ToString();
                                            }
                                            else if (Convert.ToString(rs.Tables[0].Rows[setval]["mode"]) == "Monthwise")
                                            {
                                                Prev_balance = rsFin.Tables[0].Rows[0]["total"].ToString();
                                            }
                                        }

                                        if (Convert.ToDouble(Prev_balance.ToString()) >= Convert.ToDouble(credit.ToString()))
                                        {
                                            final_balance = Convert.ToString(Convert.ToDouble(Prev_balance) - Convert.ToDouble(credit.ToString()));
                                        }

                                        con.Close();
                                        con.Open();
                                        string maxtransid = GetFunction("select max(trans_id) from stud_payment where   roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and college_code= " + Session["collegecode"].ToString() + " and header_id=" + headeridd + "");
                                        SqlCommand fincon444 = new SqlCommand("insert into stud_payment_trans (trans_date,college_code,roll_admit,trans_id,fee_code,amount,balance,fee_category,user_code,Additional_purchase) values ('" + cal_date + "'," + Session["collegecode"].ToString() + ",'" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "','" + maxtransid + "','" + feecode + "'," + credit + "," + final_balance + ",'" + cur_sem_new + "','" + Session["usercode"].ToString() + "','')", con);
                                        fincon444.ExecuteNonQuery();

                                        con.Close();
                                        con.Open();
                                        SqlCommand feestat = new SqlCommand("select * from fee_status where roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and header_id =" + headeridd + " and fee_category=" + cur_sem_new + "", con);
                                        SqlDataReader fee_status = feestat.ExecuteReader();
                                        fee_status.Read();
                                        int paidval = 0, pbal = 0;
                                        if (fee_status.HasRows == true)
                                        {
                                            paidval = int.Parse(credit);
                                            paid = paidval + int.Parse(fee_status["amount_paid"].ToString());
                                            pbal = int.Parse(fee_status["Amount"].ToString()) - Convert.ToInt32(paid);
                                            Fincon.Close();
                                            Fincon.Open();
                                            SqlCommand fincon1 = new SqlCommand();
                                            SqlCommand fincon2 = new SqlCommand();
                                            if (pbal == 0)
                                            {
                                                fincon1 = new SqlCommand("update fee_status set amount_paid=" + paid + ",balance = " + pbal + ",flag_status='true',college_code=" + Session["collegecode"] + " where  roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and header_id =" + headeridd + " and fee_category=" + cur_sem_new + "", Fincon);
                                                fincon2 = new SqlCommand("update fee_allot set flag_status='true',college_code=" + Session["collegecode"].ToString() + " where fee_code=" + feecode + " and roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and fee_category=" + cur_sem_new + "", Fincon);
                                                fincon1.ExecuteNonQuery();
                                                fincon2.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                fincon1 = new SqlCommand("update fee_status set amount_paid=" + paid + ",balance = " + pbal + ",flag_status='false',college_code=" + Session["collegecode"] + " where  roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and header_id =" + headeridd + " and fee_category=" + cur_sem_new + "", Fincon);
                                                fincon1.ExecuteNonQuery();
                                                if (Convert.ToDouble(final_balance.ToString()) == 0)
                                                {
                                                    fincon2 = new SqlCommand("update fee_allot set flag_status='true',college_code=" + Session["collegecode"].ToString() + " where fee_code=" + feecode + " and roll_admit='" + rsFinDelete.Tables[0].Rows[ival]["roll_admit"].ToString() + "' and fee_category=" + cur_sem_new + "", Fincon);
                                                    fincon2.ExecuteNonQuery();
                                                }
                                            }


                                        }

                                    }
                                }
                            }
                        }
                    l4: movetonextrec = true;
                    }
                }

            l2: movetonextrec = true;
            }
        }
    }
    public string Auto_rcptno_old(string fee_cde)
    {
        string st, et, st1, et1, recval, nt, nt1, txtrcptno = "";
        long newval1, newval;
        string fin_startdate = "", fin_enddate = "", fin_endyear = "", fin_startyear = "";
        con.Close();
        con.Open();
        DataSet dssfin = new DataSet();
        //SqlDataAdapter fin = new SqlDataAdapter("select value from master_settings where settings='Starting Date' and usercode=" + Session["usercode"].ToString() + "", con);
        SqlDataAdapter fin = new SqlDataAdapter("select max(value) from master_settings where settings='Starting Date'", con);
        fin.Fill(dssfin);
        if (dssfin.Tables[0].Rows.Count > 0)
        {
            if ((dssfin.Tables[0].Rows[0][0].ToString() != null) && (dssfin.Tables[0].Rows[0][0].ToString() != "0") && (dssfin.Tables[0].Rows[0][0].ToString() != ""))
            {
                string fin_date = dssfin.Tables[0].Rows[0][0].ToString();
                string[] fin_datesplit = fin_date.Split(new Char[] { '-' });
                if (fin_datesplit.GetUpperBound(0) > 0)
                {
                    fin_startdate = fin_datesplit[0].ToString();
                    string[] fin_datesplit1 = fin_startdate.Split(new Char[] { '/' });
                    fin_startyear = fin_datesplit1[2].ToString();
                    fin_startdate = fin_datesplit1[1].ToString() + "-" + fin_datesplit1[0].ToString() + "-" + fin_datesplit1[2].ToString();
                    fin_enddate = fin_datesplit[1].ToString();
                    string[] fin_datesplit2 = fin_enddate.Split(new Char[] { '/' });
                    fin_endyear = fin_datesplit2[2].ToString();
                    fin_enddate = fin_datesplit2[1].ToString() + "-" + fin_datesplit2[0].ToString() + "-" + fin_datesplit2[2].ToString();
                }
            }
        }

        string acct_id = "";
        con.Close();
        con.Open();
        DataSet dsscol1 = new DataSet();
        string str1 = "select distinct acctheader.acct_id from acctheader,acctinfo where acctheader.acct_id=acctinfo.acct_id and college_code=" + Session["collegecode"].ToString() + "";
        SqlDataAdapter col1 = new SqlDataAdapter(str1, con);
        col1.Fill(dsscol1);
        if (dsscol1.Tables[0].Rows.Count > 0)
        {
            acct_id = dsscol1.Tables[0].Rows[0][0].ToString();
        }

        bool chk_head = false, ddlheader = false;
        string ddlheaderid = "";
        con.Close();
        con.Open();
        SqlDataAdapter da20 = new SqlDataAdapter("select header_rcpt from rcptprint_settings where collegecode=" + Session["collegecode"].ToString() + "", con);
        DataSet ds20 = new DataSet();
        da20.Fill(ds20);
        if (ds20.Tables[0].Rows.Count > 0)
        {
            chk_head = (ds20.Tables[0].Rows[0]["header_rcpt"].ToString() == "True") ? true : false;
        }
        if (chk_head == true)
        {
            string str = "select header_id from fee_info where fee_code=" + fee_cde + "";
            con.Close();
            con.Open();
            da20 = new SqlDataAdapter(str, con);
            ds20 = new DataSet();
            da20.Fill(ds20);
            if (ds20.Tables[0].Rows.Count > 0)
            {
                str = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + ds20.Tables[0].Rows[0][0].ToString() + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + ds20.Tables[0].Rows[0][0].ToString() + "'";
                con1.Close();
                con1.Open();
                da20 = new SqlDataAdapter(str, con1);
                DataSet ds202 = new DataSet();
                da20.Fill(ds202);
                if (ds202.Tables[0].Rows.Count > 0)
                {
                    ddlheader = true;
                    ddlheaderid = ds20.Tables[0].Rows[0][0].ToString();
                }
            }

        }

        //if (chksession == true)
        {


            con.Close();
            con.Open();
            DataSet dss = new DataSet();
            string s1 = "select @@trancount";
            SqlDataAdapter rcptda = new SqlDataAdapter(s1, con);
            rcptda.Fill(dss);
            if (dss.Tables[0].Rows.Count > 0)
            {
                if (dss.Tables[0].Rows[0][0].ToString() == "1")
                {

                }
            }

            con1.Close();
            con1.Open();
            DataSet dss1 = new DataSet();
            string s11 = "select linkvalue from inssettings where linkname='Receipt Generation' and college_code=" + Session["collegecode"];
            SqlDataAdapter rcptda1 = new SqlDataAdapter(s11, con1);
            rcptda1.Fill(dss1);
            if (dss1.Tables[0].Rows.Count > 0)
            {
                if (dss1.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
                {
                    string finacode = "";
                    //chk_head = true;
                    if (chk_head == true)
                    {
                        if (ddlheader == true)
                        {
                            finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + ddlheaderid + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + ddlheaderid + "'";
                            //rs2.Open "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" & genForAcad.finyearstart & "' and '" & genForAcad.finyearend & "' and college_code=" & genForAcad.collegecode & " and header_id='" & Account_com.ItemData(Account_com.ListIndex) & "') and modifydate between '" & genForAcad.finyearstart & "' and '" & genForAcad.finyearend & "' and college_code=" & genForAcad.collegecode & " and header_id='" & Account_com.ItemData(Account_com.ListIndex) & "'", FinCon, adOpenStatic, adLockPessimistic

                        }
                        else
                        {
                            finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and (isacchead=0 or isacchead is null) and (header_id is null or header_id='')";
                        }
                    }
                    else
                    {
                        finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and (isacchead=0 or isacchead is null) and (header_id is null or header_id='')";
                    }
                    con2.Close();
                    con2.Open();
                    DataSet dss2 = new DataSet();
                    SqlDataAdapter rcptda12 = new SqlDataAdapter(finacode, con2);
                    rcptda12.Fill(dss2);
                    string sql = "";
                    if (dss2.Tables[0].Rows.Count > 0)
                    {
                        if (chk_head == true)
                        {
                            if (ddlheader == true)
                            {
                                sql = "select  sum(adj_receipt) from acctheader,acctinfo,account_info with(rowlock,xlock) where account_info.acct_id=acctinfo.acct_id and acctheader.acct_id=acctinfo.acct_id and acctinfo.acct_id=" + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and acctheader.header_id=account_info.header_id  and (account_info.header_id=" + ddlheaderid + " or  account_info.header_id is null or   account_info.header_id ='' )";
                            }
                            else
                            {
                                sql = "select  sum(adj_receipt) from account_info with(rowlock,xlock) where acct_id=" + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'  and (header_id is null or header_id='')";
                            }
                        }
                        else
                        {
                            sql = "select  sum(adj_receipt) from account_info with(rowlock,xlock) where acct_id=" + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'  and (header_id is null or header_id='')";
                        }
                    }
                    if (sql != "")
                    {
                        con3.Close();
                        con3.Open();
                        DataSet dss3 = new DataSet();
                        SqlDataAdapter rcptda13 = new SqlDataAdapter(sql, con3);
                        rcptda13.Fill(dss3);
                        if (dss3.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(dss2.Tables[0].Rows[0]["adjust_no"]) == null || Convert.ToString(dss2.Tables[0].Rows[0]["adjust_no"]) == "")
                            {
                                con4.Close();
                                con4.Open();
                                SqlCommand com = new SqlCommand("update account_info set adj_receipt=1 where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' ", con4);
                                com.ExecuteNonQuery();
                            }
                        }
                    }
                    if (sql != "")
                    {

                        con3.Close();
                        con3.Open();
                        DataSet dss3 = new DataSet();
                        SqlDataAdapter rcptda13 = new SqlDataAdapter(sql, con3);
                        rcptda13.Fill(dss3);
                        if (dss3.Tables[0].Rows.Count > 0)
                        {

                            txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + (dss3.Tables[0].Rows[0][0].ToString() == "" ? 0 : int.Parse(dss3.Tables[0].Rows[0][0].ToString())));
                            con4.Close();
                            con4.Open();
                            DataSet dss4 = new DataSet();
                            string sqlrcpt = "select * from advance_payment_transaction where voucherno='" + txtrcptno + "'";
                            SqlDataAdapter rcptda14 = new SqlDataAdapter(sqlrcpt, con4);
                            rcptda14.Fill(dss4);
                            if (dss4.Tables[0].Rows.Count > 0)
                            {
                                newval = (dss3.Tables[0].Rows[0][0].ToString() != "") ? int.Parse(dss3.Tables[0].Rows[0][0].ToString()) + 1 : 1;
                                if (chk_head == true)
                                {
                                    con5.Close();
                                    con5.Open();
                                    DataSet dss5 = new DataSet();
                                    string sqlrcpt5 = "";
                                    if (ddlheader == true)
                                    {
                                        sqlrcpt5 = "select * from account_info where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and header_id=" + ddlheaderid + "";
                                    }
                                    else
                                    {
                                        sqlrcpt5 = "select * from account_info where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'";
                                    }
                                    SqlDataAdapter rcptda15 = new SqlDataAdapter(sqlrcpt5, con5);
                                    rcptda15.Fill(dss5);
                                    if (dss5.Tables[0].Rows.Count == 0)
                                    {
                                        con6.Close();
                                        con6.Open();
                                        SqlCommand com;
                                        if (ddlheader == true)
                                        {
                                            com = new SqlCommand("insert into account_info (acct_id,adj_receipt,finyear_start,finyear_end,Header_Id)values(" + acct_id + "," + newval + ",'" + fin_startdate + "','" + fin_enddate + "'," + ddlheaderid + ")", con6);
                                        }
                                        else
                                        {
                                            com = new SqlCommand("insert into account_info (acct_id,adj_receipt,finyear_start,finyear_end,Header_Id)values(" + acct_id + "," + newval + ",'" + fin_startdate + "','" + fin_enddate + "','')", con6);
                                        }
                                        com.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        con6.Close();
                                        con6.Open();
                                        if (ddlheader == true)
                                        {
                                            SqlCommand com = new SqlCommand("update account_info set adj_receipt=" + newval + " where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and  header_id='" + ddlheaderid + "'", con6);
                                            com.ExecuteNonQuery();
                                        }

                                    }
                                }
                                else
                                {
                                    con6.Close();
                                    con6.Open();
                                    if (ddlheader == true)
                                    {
                                        SqlCommand com = new SqlCommand("update account_info set adj_receipt=" + newval + " where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and (header_id is null or header_id='')", con6);
                                        com.ExecuteNonQuery();
                                    }

                                }

                                txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + newval);
                            }
                            else
                            {
                                con5.Close();
                                con5.Open();
                                DataSet dss5 = new DataSet();
                                string sqlrcpt5 = "select * from banktransaction where voucherno='" + txtrcptno + "'";
                                SqlDataAdapter rcptda15 = new SqlDataAdapter(sqlrcpt5, con5);
                                rcptda15.Fill(dss5);
                                if (dss5.Tables[0].Rows.Count > 0)
                                {
                                    newval = int.Parse(dss3.Tables[0].Rows[0][0].ToString()) + 1;
                                    if (chk_head == true)
                                    {
                                        con6.Close();
                                        con6.Open();
                                        DataSet dss51 = new DataSet();
                                        string sqlrcpt51 = "select * from account_info where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and header_id=" + ddlheaderid + "";
                                        SqlDataAdapter rcptda151 = new SqlDataAdapter(sqlrcpt51, con6);
                                        rcptda151.Fill(dss51);
                                        if (dss51.Tables[0].Rows.Count == 0)
                                        {
                                            con7.Close();
                                            con7.Open();
                                            SqlCommand com = new SqlCommand("insert into account_info (acct_id,adj_receipt,finyear_start,finyear_end,Header_Id)values(" + acct_id + "," + newval + ",'" + fin_startdate + "','" + fin_enddate + "'," + ddlheaderid + ")", con6);
                                            com.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        con6.Close();
                                        con6.Open();
                                        SqlCommand com = new SqlCommand("update account_info set adj_receipt=" + newval + " where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and (header_id is null or header_id='')", con6);
                                        com.ExecuteNonQuery();
                                    }
                                    txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()));
                                }

                            }

                        }
                        else
                        {
                            txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()));
                        }
                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Receipt Number is Empty')", true);
                    //    FpSpread1.Enabled = false;
                    //    pamount.Enabled = false;
                    //    ppaymode.Enabled = false;
                    //    btnsave.Enabled = false;
                    //}
                }

                else
                {
                    string finacode = "";
                    finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "'";
                    con2.Close();
                    con2.Open();
                    DataSet dss2 = new DataSet();
                    SqlDataAdapter rcptda12 = new SqlDataAdapter(finacode, con2);
                    rcptda12.Fill(dss2);
                    string sql = "";
                    if (dss2.Tables[0].Rows.Count > 0)
                    {
                        sql = "select distinct adj_receipt from acctheader,acctinfo,account_info with(rowlock,xlock) where account_info.acct_id=acctinfo.acct_id and acctheader.acct_id=acctinfo.acct_id  and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' ";
                        con3.Close();
                        con3.Open();
                        DataSet dss3 = new DataSet();
                        SqlDataAdapter rcptda13 = new SqlDataAdapter(sql, con3);
                        rcptda13.Fill(dss3);
                        if (dss3.Tables[0].Rows.Count > 0)
                        {
                            txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + int.Parse((dss3.Tables[0].Rows[0][0].ToString() == "" ? "0" : dss3.Tables[0].Rows[0][0].ToString())));
                            con4.Close();
                            con4.Open();
                            DataSet dss4 = new DataSet();
                            string sqlrcpt = "select * from advance_payment_transaction where voucherno='" + txtrcptno + "'";
                            SqlDataAdapter rcptda14 = new SqlDataAdapter(sqlrcpt, con4);
                            rcptda14.Fill(dss4);
                            if (dss4.Tables[0].Rows.Count > 0)
                            {
                                newval = int.Parse(dss3.Tables[0].Rows[0][0].ToString()) + 1;
                                con5.Close();
                                con5.Open();
                                string str5 = "update account_info set adj_receipt=" + newval + " where  finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'";
                                SqlCommand com = new SqlCommand(str5, con5);
                                com.ExecuteNonQuery();
                                txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + newval);
                            }
                            else
                            {
                                con5.Close();
                                con5.Open();
                                DataSet dss45 = new DataSet();
                                string sqlrcpt5 = "select * from banktransaction where voucherno='" + txtrcptno + "'";
                                SqlDataAdapter rcptda145 = new SqlDataAdapter(sqlrcpt5, con5);
                                rcptda145.Fill(dss45);
                                if (dss45.Tables[0].Rows.Count > 0)
                                {
                                    newval = int.Parse(dss3.Tables[0].Rows[0][0].ToString()) + 1;
                                    con6.Close();
                                    con6.Open();
                                    string str5 = "update account_info set adj_receipt=" + newval + " where  finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'";
                                    SqlCommand com = new SqlCommand(str5, con6);
                                    com.ExecuteNonQuery();
                                    txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + newval);
                                }
                            }
                        }
                        else
                        {
                            txtrcptno = dss2.Tables[0].Rows[0]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()));
                        }
                    }

                }

            }
        }
        return txtrcptno;
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        try
        {
            bool isregin = false;
            string isOTP = string.Empty;
            Session["column_header_row_count"] = "1";
            Session["current_college_code"] = "";
            Session["password"] = "";
            Session["Version"] = "Ins-2018.1";
            if (CheckBox1.Checked == true)
            {
                Response.Cookies["UName"].Value = txtuname.Text;
                Response.Cookies["PWD"].Value = txtpassword.Text;
                Response.Cookies["UName"].Expires = DateTime.Now.AddMonths(2);
                Response.Cookies["PWD"].Expires = DateTime.Now.AddMonths(2);
            }
            else
            {
                Response.Cookies["UName"].Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies["PWD"].Expires = DateTime.Now.AddMonths(-1);
            }

            if (txtuname.Text.Length == 0 && txtpassword.Text.Length == 0)
            {


            }
            else
            {
                string passwd = "";
                string afterenc = "";
                passwd = txtpassword.Text;
                isOTP = d2.GetFunction("select otpconfirm from usermaster where user_id='" + txtuname.Text.Trim() + "'");
                if (txtpassword.Text == "")
                {
                    cmd.CommandText = "select * from usermaster where user_id='" + txtuname.Text.Replace("'", "''") + "' and PassWord='" + txtpassword.Text.Replace("'", "''") + "'";
                    cmd.Connection = con;
                    con.Open();

                    SqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows == true)
                    {
                        //sridharan 03.03.2015
                        string staffcode = txtuname.Text.Trim();
                        string dtime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //string sricmd = "insert into logindetails values('" + dtime + "','" + staffcode + "','1')";
                        //int log = da.update_method_wo_parameter(sricmd, "Text");//aruna 20feb2018
                        isOTP = d2.GetFunction("select otpconfirm from usermaster where user_id='" + staffcode + "'");
                        //if (isOTP == "1")
                        //{
                        //}
                        //else
                        //{
                        if (dr["is_Staff"].ToString() == "True")
                        {
                            isregin = isresign(Convert.ToString(dr["Staff_Code"]));
                            Session["Staff_Code"] = dr["Staff_Code"].ToString();
                            string sricmd = "insert into logindetails values('" + dtime + "','" + staffcode + "','1')"; // 0-admin 1-staff 2-student
                            int log = da.update_method_wo_parameter(sricmd, "Text");//aruna 20feb2018

                        }
                        else
                        {
                            Session["UserName"] = dr["user_id"].ToString();
                            Session["Staff_Code"] = "";
                            string sricmd = "insert into logindetails values('" + dtime + "','" + staffcode + "','0')"; // 0-admin 1-staff 2-student
                            int log = da.update_method_wo_parameter(sricmd, "Text");//aruna 20feb2018
                        }
                        Session["Single_User"] = dr["singleuser"].ToString();
                        Session["collegecode"] = dr["college_code"].ToString();
                        string collcode = Session["collegecode"].ToString();

                        Session["group_code"] = 0;
                        if (dr["group_code"].ToString() == "" || dr["group_code"].ToString() == "-1")
                        {
                            Session["group_code"] = 0;
                        }
                        else
                        {
                            Session["group_code"] = dr["group_code"].ToString();
                        }

                        Fincon.Close();
                        Fincon.Open();
                        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        //SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToShortDateString() + "' ,'" + DateTime.Now.ToShortDateString() + "',0,'','')", Fincon);
                        // SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToShortDateString() + "' ,'" + intime + "','',0,'" + collcode + "')", Fincon);//Modified by srinath 23/8/2013
                        SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToString("MM/dd/yyyy") + "' ,'" + intime + "','',0,'" + collcode + "')", Fincon);//Modified by srinath 23/8/2013
                        fincon12.ExecuteNonQuery();


                        fcon.Close();
                        fcon.Open();
                        DataSet dss = new DataSet();
                        //string s1 = "select max(entry_code) from UserEELog where user_code=" + dr["user_code"].ToString() + " and DOA='" + DateTime.Now.ToShortDateString() + "'";
                        string s1 = "select max(entry_code) from UserEELog where user_code=" + dr["user_code"].ToString() + " and DOA='" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
                        SqlDataAdapter categoryda = new SqlDataAdapter(s1, fcon);
                        categoryda.Fill(dss);
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (dss.Tables[0].Rows[0][0].ToString() != "")
                            {
                                Session["Entry_Code"] = dss.Tables[0].Rows[0][0].ToString();
                            }
                        }

                        Session["IsLogin"] = "1";
                        //  Session["collegecode"] = dr["college_code"].ToString();
                        Session["current_college_code"] = dr["college_code"].ToString();
                        Session["InternalCollegeCode"] = dr["college_code"].ToString();
                        Session["UserCode"] = dr["user_code"].ToString();
                        Session["UserName"] = dr["user_id"].ToString();

                        //Session["group_code"] = 0;
                        //if (dr["group_code"].ToString() == "" || dr["group_code"].ToString() == "-1")
                        //{
                        //    Session["group_code"] = 0;
                        //}
                        //else
                        //{
                        //    Session["group_code"] = dr["group_code"].ToString();
                        //}
                        if (txtpassword.Text != "")
                            Session["password"] = txtpassword.Text.Trim();
                        else
                            Session["password"] = "";

                        //===========================================================================================================
                        Session["prntvissble"] = "false";
                        //if (Session["usercode"].ToString() != "")
                        //{
                        //    string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + " and settings='print_master_setting'";
                        //    setcon.Close();
                        //    setcon.Open();
                        //    SqlDataReader mtrdr;

                        //    SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                        //    mtrdr = mtcmd.ExecuteReader();
                        //    if (mtrdr.HasRows)
                        //    {
                        //        while (mtrdr.Read())
                        //        {
                        //            if (mtrdr["settings"].ToString() == "print_master_setting" && mtrdr["value"].ToString() == "1")
                        //            {
                        //                Session["prntvissble"] = "false";// "true";
                        //            }
                        //            else
                        //            {
                        //                Session["prntvissble"] = "false";
                        //            }
                        //        }
                        //    }
                        //}
                        //==========================================================================================================

                        //Hide By Aruna 04/June/2018 For MCC Slowness=====================
                        //advancepayment(int.Parse(Session["collegecode"].ToString()));
                        //if (Session["collegecode"] != null)
                        //{
                        //    Transport_Remainder();
                        //}
                        //===============================================================


                        Session["dashprod"] = "1";
                        Session["MID"] = "wrong";
                        if (!isregin)
                        {
                            if (isOTP == "1" || isOTP == "True")
                            {
                                try
                                {
                                    staffcode = txtuname.Text.Trim();
                                    string mobileno = string.Empty;
                                    if (dr["is_Staff"].ToString() == "True")
                                    {
                                        mobileno = d2.GetFunction("select per_mobileno from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code and staff_code='" + staffcode + "'");
                                    }
                                    else
                                    {
                                        mobileno = d2.GetFunction("select phone_no from usermaster where user_id='" + staffcode + "'");
                                    }
                                    if (!string.IsNullOrEmpty(mobileno))
                                    {
                                        string numbers = "123456789";
                                        string characters = numbers;
                                        string otp = string.Empty;
                                        for (int i = 0; i < 5; i++)
                                        {
                                            string character = string.Empty;
                                            do
                                            {
                                                int index = new Random().Next(0, characters.Length);
                                                character = characters.ToCharArray()[index].ToString();
                                            } while (otp.IndexOf(character) != -1);
                                            otp += character;
                                        }
                                        string otpStr = "Your Login OTP is : " + otp;
                                        bool checkflage = false;
                                        string user_id = string.Empty;
                                        string SenderID = string.Empty;
                                        string Password = string.Empty;
                                        //string collegeCode = Convert.ToString(ddlClg.SelectedValue);
                                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                                        //checkflage = true;//comment it
                                        DataSet ds = new DataSet();
                                        ds = d2.select_method_wo_parameter(ssr, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
                                        }

                                        if (user_id != string.Empty)
                                        {
                                            string getval = d2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[0].ToString();
                                            }
                                            int sec = d2.send_sms(user_id.Trim(), Session["collegecode"].ToString(), Session["UserCode"].ToString(), mobileno, otpStr, "1");
                                            if (sec > 0)
                                            {
                                                checkflage = true;
                                                // Div1.Visible = true;
                                            }
                                        }
                                        if (checkflage)
                                        {
                                            //string appl_no = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                                            //int a = d2.update_method_wo_parameter("update staff_appl_master SET OTPNumber='" + otp + "' where appl_no='" + appl_no + "'", "Text");
                                            int a = d2.update_method_wo_parameter("update usermaster SET OTPNumber='" + otp + "' where user_id='" + txtuname.Text + "'", "Text");

                                            Div1.Visible = true;
                                            Session["IsLogin"] = "0";
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                Response.Redirect("Default_LoginPage.aspx");
                            }
                        }
                        else
                        {
                            pwdvalidation.Visible = true;
                            txtuname.Text = "";
                            dr.Close();
                            con.Close();
                        }

                    }
                    else
                    {
                        pwdvalidation.Visible = true;
                        txtuname.Text = "";
                        dr.Close();
                        con.Close();
                    }
                }
                else
                {

                    afterenc = encryptdata(passwd);
                    con.Open();
                    cmd.CommandText = "select * from usermaster where user_id='" + txtuname.Text.Replace("'", "''") + "' and PassWord='" + afterenc.Replace("'", "''") + "'";
                    cmd.Connection = con;

                    SqlDataReader dr;
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows == true)
                    {
                        Session["collegecode"] = dr["college_code"].ToString();
                        Fincon.Close();
                        Fincon.Open();
                        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        //SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToShortDateString() + "' ,'" + DateTime.Now.ToShortDateString() + "',0,'','')", Fincon);
                        string collcode = Session["collegecode"].ToString();
                        //SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToShortDateString() + "' ,'" + intime + "','','0','" + collcode + "')", Fincon);//Modified by srinath 24/8/2013
                        SqlCommand fincon12 = new SqlCommand("Insert into UserEELog (User_Code,DOA,In_Time,Out_Time,LogOff,SelCollege) values(" + dr["user_code"].ToString() + ",'" + DateTime.Now.ToString("MM/dd/yyyy") + "' ,'" + intime + "','','0','" + collcode + "')", Fincon);//Modified by srinath 24/8/2013
                        fincon12.ExecuteNonQuery();
                        fcon.Close();
                        fcon.Open();
                        DataSet dss = new DataSet();
                        string s1 = "select max(entry_code) from UserEELog where user_code=" + dr["user_code"].ToString() + " and DOA='" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
                        SqlDataAdapter categoryda = new SqlDataAdapter(s1, fcon);
                        categoryda.Fill(dss);
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            if (dss.Tables[0].Rows[0][0].ToString() != "")
                            {
                                Session["Entry_Code"] = dss.Tables[0].Rows[0][0].ToString();
                            }
                        }


                        if (dr["is_Staff"].ToString() == "True")
                        {
                            isregin = isresign(Convert.ToString(dr["Staff_Code"]));
                            Session["Staff_Code"] = dr["Staff_Code"].ToString();
                        }
                        else
                        {
                            Session["UserName"] = dr["user_id"].ToString();
                            Session["Staff_Code"] = "";
                        }
                        Session["Single_User"] = dr["singleuser"].ToString();
                        Session["IsLogin"] = "1";
                        // Session["collegecode"] = dr["college_code"].ToString();
                        Session["current_college_code"] = dr["college_code"].ToString();
                        Session["InternalCollegeCode"] = dr["college_code"].ToString();
                        Session["UserCode"] = dr["user_code"].ToString();
                        Session["UserName"] = dr["user_id"].ToString();
                        Session["group_code"] = 0;
                        if (dr["group_code"].ToString() == "" || dr["group_code"].ToString() == "-1")
                        {
                            Session["group_code"] = 0;
                        }
                        else
                        {
                            Session["group_code"] = dr["group_code"].ToString();
                        }


                        if (txtpassword.Text != "")
                            Session["password"] = txtpassword.Text.Trim();
                        else
                            Session["password"] = "";

                        //===========================================================================================================
                        Session["prntvissble"] = "false";
                        //if (Session["usercode"].ToString() != "")
                        //{
                        //    string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + " and settings='print_master_setting'";
                        //    setcon.Close();
                        //    setcon.Open();
                        //    SqlDataReader mtrdr;

                        //    SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                        //    mtrdr = mtcmd.ExecuteReader();
                        //    if (mtrdr.HasRows)
                        //    {
                        //        while (mtrdr.Read())
                        //        {
                        //            if (mtrdr["settings"].ToString() == "print_master_setting" && mtrdr["value"].ToString() == "1")
                        //            {
                        //                Session["prntvissble"] = "false";// "true";
                        //            }
                        //            else
                        //            {
                        //                Session["prntvissble"] = "false";
                        //            }
                        //        }
                        //    }
                        //}
                        //==========================================================================================================

                        //Hide By Aruna 04/June/2018 For MCC Slowness=====================
                        //advancepayment(int.Parse(Session["collegecode"].ToString()));
                        //if (Session["collegecode"] != null)
                        //{
                        //    Transport_Remainder();
                        //}
                        //===============================================================
                        Session["dashprod"] = "1";
                        if (!isregin)
                        {
                            if (isOTP == "1" || isOTP =="True")
                            {
                                try
                                {
                                    string staffcode = txtuname.Text.Trim();
                                    string mobileno = string.Empty;
                                    if (dr["is_Staff"].ToString() == "True")
                                    {
                                        mobileno = d2.GetFunction("select per_mobileno from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code and staff_code='" + staffcode + "'");
                                    }
                                    else
                                    {
                                        mobileno = d2.GetFunction("select phone_no from usermaster where user_id='" + staffcode + "'");
                                    }
                                    if (!string.IsNullOrEmpty(mobileno))
                                    {
                                        string numbers = "123456789";
                                        string characters = numbers;
                                        string otp = string.Empty;
                                        for (int i = 0; i < 5; i++)
                                        {
                                            string character = string.Empty;
                                            do
                                            {
                                                int index = new Random().Next(0, characters.Length);
                                                character = characters.ToCharArray()[index].ToString();
                                            } while (otp.IndexOf(character) != -1);
                                            otp += character;
                                        }
                                        string otpStr = "Your Login OTP is : " + otp;
                                        bool checkflage = false;
                                        string user_id = string.Empty;
                                        string SenderID = string.Empty;
                                        string Password = string.Empty;
                                        //string collegeCode = Convert.ToString(ddlClg.SelectedValue);
                                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                                        DataSet ds = new DataSet();
                                        ds = d2.select_method_wo_parameter(ssr, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
                                        }

                                        if (user_id != string.Empty)
                                        {
                                            string getval = d2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {
                                                SenderID = spret[0].ToString();
                                                Password = spret[0].ToString();
                                            }
                                            int sec = d2.send_sms(user_id.Trim(), Session["collegecode"].ToString(), Session["UserCode"].ToString(), mobileno, otpStr, "1");
                                            if (sec > 0)
                                            {
                                                checkflage = true;
                                                // Div1.Visible = true;
                                            }
                                        }
                                        if (checkflage)
                                        {
                                            //string appl_no = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                                            //int a = d2.update_method_wo_parameter("update staff_appl_master SET OTPNumber='" + otp + "' where appl_no='" + appl_no + "'", "Text");
                                            int a = d2.update_method_wo_parameter("update usermaster SET OTPNumber='" + otp + "' where user_id='" + txtuname.Text + "'", "Text");
                                            Div1.Visible = true;
                                            Session["IsLogin"] = "0";
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                            else
                            {
                                Response.Redirect("Default_LoginPage.aspx");
                            }
                        }
                        else
                        {
                            pwdvalidation.Visible = true;
                            dr.Close();
                            con.Close();
                        }

                    }
                    else
                    {
                        pwdvalidation.Visible = true;
                        dr.Close();
                        con.Close();
                    }
                }


            }
        }
        catch
        {

        }

    }

    public string encryptdata(string text)
    {

        int counter;
        string daynum;
        int daykey;
        string retdata = "";
        string encrypt;
        string ascvar;
        int temporary;
        string encdata = "";
        System.Text.Encoding asc = System.Text.Encoding.ASCII;

        if (text == "")
        {
            encdata = "";
        }
        daykey = Generatekey();
        retdata = Convert.ToChar((daykey.ToString()).Length).ToString().Trim();
        retdata = retdata + encryptkey(daykey.ToString());

        for (counter = 0; counter < text.Length; counter++)
        {
            byte[] tbyte;
            string midsub = text.Substring(counter, 1);
            tbyte = asc.GetBytes(midsub);
            long tempbyte = tbyte[0];
            long temp = (tempbyte + daykey) % 256;

            string data = Encoding.Default.GetString(new[] { (byte)temp });

            retdata = retdata + data;

        }
        encdata = retdata;
        return encdata;
    }

    public int Generatekey()
    {

        int millisecond;
        millisecond = Convert.ToInt32((0) % (100));

        int generatekey = millisecond + addtokey;
        return (generatekey);
    }

    public string encryptkey(string key)
    {
        string ascvar;
        int counter;
        string newkey = "";
        // string encryptkey="";
        string rightstring = "";
        System.Text.Encoding asc = System.Text.Encoding.ASCII;
        for (counter = 0; counter < key.Length; counter++)
        {
            byte[] tbyte;

            string s = key.Substring(counter, 1);
            tbyte = asc.GetBytes(s);
            long tempbyte = tbyte[0];

            string temp = Convert.ToChar(tempbyte + basekey).ToString();

            newkey = newkey + temp;
        }

        return (newkey);
    }

    public string GetFunction(string sqlQuery)
    {

        string sqlstr;
        sqlstr = sqlQuery;
        con4.Close();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con4);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con4;
        con4.Open();
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }

    }

    public string Auto_rcptno(string fee_cde)
    {
        long newval1, newval;
        long lastrecord = 0;
        string txtrcptno = "";
        string finacode = "";
        string fin_startdate = "", fin_enddate = "", fin_endyear = "", fin_startyear = "", headerid = "";
        con.Close();
        con.Open();
        DataSet dssfin = new DataSet();
        SqlDataAdapter fin = new SqlDataAdapter("select value from master_settings where settings='Starting Date' and usercode=" + Session["usercode"].ToString() + "", con);

        fin.Fill(dssfin);
        if (dssfin.Tables[0].Rows.Count > 0)
        {
            if ((dssfin.Tables[0].Rows[0][0].ToString() != null) && (dssfin.Tables[0].Rows[0][0].ToString() != "0") && (dssfin.Tables[0].Rows[0][0].ToString() != ""))
            {
                string fin_date = dssfin.Tables[0].Rows[0][0].ToString();
                string[] fin_datesplit = fin_date.Split(new Char[] { '-' });
                if (fin_datesplit.GetUpperBound(0) > 0)
                {
                    fin_startdate = fin_datesplit[0].ToString();
                    string[] fin_datesplit1 = fin_startdate.Split(new Char[] { '/' });
                    fin_startyear = fin_datesplit1[2].ToString();
                    fin_startdate = fin_datesplit1[1].ToString() + "-" + fin_datesplit1[0].ToString() + "-" + fin_datesplit1[2].ToString();
                    fin_enddate = fin_datesplit[1].ToString();
                    string[] fin_datesplit2 = fin_enddate.Split(new Char[] { '/' });
                    fin_endyear = fin_datesplit2[2].ToString();
                    fin_enddate = fin_datesplit2[1].ToString() + "-" + fin_datesplit2[0].ToString() + "-" + fin_datesplit2[2].ToString();
                }
            }
        }

        string acct_id = "";
        con.Close();
        con.Open();
        DataSet dsscol1 = new DataSet();
        string str1 = "select distinct account_info.acct_id from account_info,acctinfo where account_info.acct_id=acctinfo.acct_id and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and college_code=" + Session["collegecode"].ToString() + " and college_code=" + Session["collegecode"] + "";
        SqlDataAdapter col1 = new SqlDataAdapter(str1, con);
        col1.Fill(dsscol1);
        if (dsscol1.Tables[0].Rows.Count > 0)
        {
            acct_id = dsscol1.Tables[0].Rows[0][0].ToString();
        }

        bool chk_head = false, ddlheader = false;
        string ddlheaderid = "";
        con.Close();
        con.Open();
        SqlDataAdapter da20 = new SqlDataAdapter("select header_rcpt from rcptprint_settings where collegecode=" + Session["collegecode"].ToString() + "", con);
        DataSet ds20 = new DataSet();
        da20.Fill(ds20);
        if (ds20.Tables[0].Rows.Count > 0)
        {
            chk_head = (ds20.Tables[0].Rows[0]["header_rcpt"].ToString() == "True") ? true : false;
        }

        string str = "select header_id from fee_info where fee_code=" + fee_cde + "";
        con.Close();
        con.Open();
        da20 = new SqlDataAdapter(str, con);
        ds20 = new DataSet();
        da20.Fill(ds20);
        if (ds20.Tables[0].Rows.Count > 0)
        {
            headerid = ds20.Tables[0].Rows[0][0].ToString();
        }

        con1.Close();
        con1.Open();
        DataSet dss1 = new DataSet();
        string s11 = "select linkvalue from inssettings where linkname='Receipt Generation' and college_code=" + Session["collegecode"];
        SqlDataAdapter rcptda1 = new SqlDataAdapter(s11, con1);
        rcptda1.Fill(dss1);
        if (dss1.Tables[0].Rows.Count > 0)
        {
            if (dss1.Tables[0].Rows[0]["linkvalue"].ToString() == "0")
            {
                if (chk_head == true)
                {
                    finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + headerid + "') and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and header_id='" + headerid + "' and ltrim(adjust_acr)<>'' and adjust_acr is not null";
                }
                else
                {
                    finacode = "select * from Finacode_settings where modifydate=(Select max(modifydate) from Finacode_settings where modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and (isacchead=0 or isacchead is null) and (header_id='' or header_id is null)) and modifydate between '" + fin_startdate + "' and '" + fin_enddate + "' and college_code=" + Session["collegecode"].ToString() + " and (isacchead=0 or isacchead is null) and (header_id='' or header_id is null) and ltrim(adjust_acr)<>'' and adjust_acr is not null";
                }
                con2.Close();
                con2.Open();
                DataSet dss2 = new DataSet();
                SqlDataAdapter rcptda12 = new SqlDataAdapter(finacode, con2);
                rcptda12.Fill(dss2);
                string sql = "";
                if (dss2.Tables[0].Rows.Count > 0)
                {

                    if (chk_head == true)
                    {
                        sql = "select  sum(distinct adj_receipt) from acctheader,acctinfo,account_info with(rowlock,xlock) where account_info.acct_id=acctinfo.acct_id and acctheader.acct_id=acctinfo.acct_id  and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'  and (account_info.header_id='" + headerid + "')";
                    }
                    else
                    {
                        sql = "select  sum(distinct adj_receipt) from account_info with(rowlock,xlock) where acct_id=" + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31'  and (header_id is null or header_id='')";
                    }
                    if (sql != "")
                    {
                        Session["rept_acro"] = dss2.Tables[0].Rows[dss2.Tables[0].Rows.Count - 1]["adjust_acr"].ToString().Trim().ToUpper();
                        con3.Close();
                        con3.Open();
                        DataSet dss3 = new DataSet();
                        SqlDataAdapter rcptda13 = new SqlDataAdapter(sql, con3);
                        rcptda13.Fill(dss3);
                        if (dss3.Tables[0].Rows.Count > 0)
                        {
                            txtrcptno = dss2.Tables[0].Rows[dss2.Tables[0].Rows.Count - 1]["adjust_acr"].ToString().Trim().ToUpper() + (int.Parse(dss2.Tables[0].Rows[0]["adjust_no"].ToString()) + (dss3.Tables[0].Rows[0][0].ToString() == "" ? 0 : int.Parse(dss3.Tables[0].Rows[0][0].ToString())));
                            con4.Close();
                            con4.Open();
                            DataSet dss4 = new DataSet();
                            //string sqlrcpt = "select * from dailytransaction where voucherno='" + txtrcptno + "'";
                            string sqlrcpt = "select * from advance_payment_transaction where voucherno='" + txtrcptno + "'";
                            SqlDataAdapter rcptda14 = new SqlDataAdapter(sqlrcpt, con4);
                            rcptda14.Fill(dss4);
                            if (dss4.Tables[0].Rows.Count > 0)
                            {
                                newval = (dss3.Tables[0].Rows[0][0].ToString() != "") ? int.Parse(dss3.Tables[0].Rows[0][0].ToString()) + 1 : 1;
                                if (chk_head == true)
                                {
                                    con5.Close();
                                    con5.Open();
                                    DataSet dss5 = new DataSet();
                                    string sqlrcpt5 = "";
                                    sqlrcpt5 = "select * from account_info where  finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and header_id='" + headerid + "'";
                                    SqlDataAdapter rcptda15 = new SqlDataAdapter(sqlrcpt5, con5);
                                    rcptda15.Fill(dss5);
                                    if (dss5.Tables[0].Rows.Count == 0)
                                    {
                                        con6.Close();
                                        con6.Open();
                                        SqlCommand com;
                                        com = new SqlCommand("insert into account_info (acct_id,adj_receipt,finyear_start,finyear_end,Header_Id)values(" + acct_id + "," + newval + ",'" + fin_startdate + "','" + fin_enddate + "'," + headerid + ")", con6);
                                        com.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        con6.Close();
                                        con6.Open();
                                        SqlCommand com = new SqlCommand("update account_info set adj_receipt=" + newval + " where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and  header_id='" + headerid + "'", con6);
                                        com.ExecuteNonQuery();

                                    }
                                    txtrcptno = dss2.Tables[0].Rows[dss2.Tables[0].Rows.Count - 1]["adjust_acr"].ToString().Trim().ToUpper() + (newval + 1);
                                    Session["adjust_no"] = dss2.Tables[0].Rows[0]["adjust_no"].ToString();

                                }
                                else
                                {
                                    con6.Close();
                                    con6.Open();
                                    SqlCommand com = new SqlCommand("update account_info set adj_receipt=" + newval + " where acct_id = " + acct_id + " and finyear_start='" + fin_startyear + "-04-01' and finyear_end='" + fin_endyear + "-03-31' and (header_id is null or header_id='')", con6);
                                    com.ExecuteNonQuery();
                                    txtrcptno = dss2.Tables[0].Rows[dss2.Tables[0].Rows.Count - 1]["adjust_acr"].ToString().Trim().ToUpper() + (newval + 1);
                                    Session["adjust_no"] = dss2.Tables[0].Rows[0]["adjust_no"].ToString();
                                }


                            }


                        }
                    }
                }

            }
        }
        return txtrcptno;
    }

    void Transport_Remainder()
    {
        try
        {
            con.Close();
            con.Open();
            SqlCommand cmd_get_user = new SqlCommand("select SMS_User_ID,college_code from Track_Value where college_code = '" + Session["collegecode"].ToString() + "'", con);
            SqlDataAdapter ad_get_user = new SqlDataAdapter(cmd_get_user);
            ad_get_user.Fill(ds1);
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            }
            //modified by srinath 1/8/2014
            //GetUserapi(user_id);
            string getval = d2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                SenderID = spret[0].ToString();
                Password = spret[0].ToString();
                Session["api"] = user_id;
                Session["senderid"] = SenderID;
            }
            con.Close();
            con.Open();
            SqlCommand cmd_get_settings = new SqlCommand("Select * from transport_settings where college_code='" + Session["collegecode"].ToString() + "'", con);
            SqlDataAdapter ad_get_settings = new SqlDataAdapter(cmd_get_settings);
            DataTable dt_get_settings = new DataTable();
            ad_get_settings.Fill(dt_get_settings);
            if (dt_get_settings.Rows.Count > 0)
            {
                string send_mail = string.Empty;
                string send_pw = string.Empty;
                string strquery = "select massemail,masspwd from collinfo where college_code = '" + Session["collegecode"].ToString() + "'";
                ds1.Dispose();
                ds1.Reset();
                ds1 = d2.select_method(strquery, hat, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                }
                string staff_code = string.Empty;
                int remain_days = Convert.ToInt32(dt_get_settings.Rows[0][1].ToString());
                con.Close();
                con.Open();
                SqlCommand cmd_mob_no = new SqlCommand("select * from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code", con);
                SqlDataAdapter ad_mob_no = new SqlDataAdapter(cmd_mob_no);
                DataTable dt_mob_no = new DataTable();
                ad_mob_no.Fill(dt_mob_no);

                if (dt_mob_no.Rows.Count > 0)
                {
                    string mob_no = string.Empty;
                    string email_id = string.Empty;
                    string cur_date = DateTime.Now.ToString("MM/dd/yyyy");
                    string to_date = Convert.ToDateTime(cur_date).AddDays(remain_days).ToString();
                    string[] spl_cur_date = cur_date.Split(' ');
                    string[] spl_to_date = to_date.Split(' ');
                    con.Close();
                    con.Open();
                    SqlCommand cmd_intimation_licence = new SqlCommand("select * from driverallotment where renew_date between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "'", con);
                    SqlDataAdapter ad_intimation_licence = new SqlDataAdapter(cmd_intimation_licence);
                    DataTable dt_intimation_licence = new DataTable();
                    ad_intimation_licence.Fill(dt_intimation_licence);

                    if (dt_intimation_licence.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_intimation_licence.Rows.Count; i++)
                        {
                            staff_code = "";
                            string flag = "0";
                            string driv_name = dt_intimation_licence.Rows[i]["Staff_name"].ToString();
                            string driv_code = dt_intimation_licence.Rows[i]["Staff_Code"].ToString();
                            string[] spl_renew = dt_intimation_licence.Rows[i]["Renew_Date"].ToString().Split(' ');
                            string[] spl_date = spl_renew[0].Split('/');
                            string renew_date = spl_date[1] + "/" + spl_date[0] + "/" + spl_date[2];
                            if (dt_intimation_licence.Rows[i]["remainder"].ToString() == "0")
                            {
                                staff_code = dt_get_settings.Rows[0][2].ToString();
                                flag = "1";
                            }
                            else if (dt_intimation_licence.Rows[i]["remainder"].ToString() == "1" && dt_get_settings.Rows[0]["staff_two"].ToString() != "")
                            {
                                staff_code = dt_get_settings.Rows[0][3].ToString();
                                flag = "2";
                            }
                            else
                                staff_code = "";
                            con.Close();
                            con.Open();
                            SqlCommand cmd_last_remain = new SqlCommand("select Last_Remin as Last_Remin from driverallotment where staff_code='" + driv_code + "'", con);
                            SqlDataAdapter ad_last_remain = new SqlDataAdapter(cmd_last_remain);
                            DataTable dt_last_remain = new DataTable();
                            ad_last_remain.Fill(dt_last_remain);
                            int diff = 0;
                            if (dt_last_remain.Rows.Count > 0)
                            {
                                DateTime last = Convert.ToDateTime(dt_last_remain.Rows[0]["Last_Remin"].ToString());
                                diff = Convert.ToInt32((Convert.ToDateTime(cur_date) - last).Days);
                            }
                            if (flag == "1" || (flag == "2" && diff == 1))
                            {
                                DataView dv_mob_no = new DataView();
                                dt_mob_no.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                dv_mob_no = dt_mob_no.DefaultView;
                                if (dv_mob_no.Count > 0)
                                {
                                    mob_no = dv_mob_no[0]["per_mobileno"].ToString();
                                    email_id = dv_mob_no[0]["email"].ToString();
                                    string sms_content = "Please renew the driving licence of Mr." + driv_name + "-" + driv_code + "Renew Date:" + renew_date;
                                    //modified by srinath 8/2/2014
                                    //  string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mob_no + "&message=" + sms_content + "&sender=" + SenderID;
                                    //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mob_no + "&text=" + sms_content + "&priority=ndnd&stype=normal";
                                    //send_mail = "sudhagarpvs@gmail.com";
                                    //mob_no = "9789824009";
                                    int strpath1 = d2.send_sms(user_id, Convert.ToString(Session["collegecode"]), Convert.ToString(Session["UserCode"]), mob_no, sms_content, "1");
                                    //string isstf = "1";
                                    //smsreport(strpath1, isstf, sms_content, mob_no);
                                    string description = "Please renew the driving licence of Mr." + driv_name + "(" + driv_code + ")";
                                    try
                                    {
                                        SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                        MailMessage mailmsg = new MailMessage();
                                        MailAddress mfrom = new MailAddress(send_mail);
                                        mailmsg.From = mfrom;
                                        mailmsg.To.Add(email_id);
                                        mailmsg.Subject = "Transport Reminder";
                                        mailmsg.IsBodyHtml = true;
                                        //mailmsg.Body = "Hi ";
                                        //mailmsg.Body = mailmsg.Body + staff_namew;
                                        mailmsg.Body = mailmsg.Body + description + "<br><br>Renew Date:" + renew_date;
                                        mailmsg.Body = mailmsg.Body + "<br><br>Thank You...";
                                        Mail.EnableSsl = true;
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                    }
                                    catch { }
                                    con.Close();
                                    con.Open();

                                    SqlCommand cmd_update = new SqlCommand("update driverallotment set remainder='" + flag + "',Last_Remin='" + cur_date + "' where staff_code='" + driv_code + "'", con);
                                    cmd_update.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    con.Close();
                    con.Open();

                    SqlCommand cmd_intimation_vehicle = new SqlCommand("select veh_type,veh_id,nextins_date as ins,nextfcdate as fc,permit_date as permit,remainder from Vehicle_Insurance where CONVERT(Datetime, nextins_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, nextfcdate, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, permit_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' order by veh_id", con);
                    SqlDataAdapter ad_intimation_vehicle = new SqlDataAdapter(cmd_intimation_vehicle);
                    DataTable dt_intimation_vehicle = new DataTable();
                    ad_intimation_vehicle.Fill(dt_intimation_vehicle);

                    if (dt_intimation_vehicle.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_intimation_vehicle.Rows.Count; i++)
                        {
                            string flag = "0";
                            string description = string.Empty;
                            string sql = string.Empty;
                            string date = string.Empty;
                            staff_code = "";

                            string veh_id = dt_intimation_vehicle.Rows[i]["veh_id"].ToString();

                            if (dt_intimation_vehicle.Rows[i]["ins"] != null)
                            {
                                if (dt_intimation_vehicle.Rows[i]["ins"].ToString() != "")
                                {
                                    description = "Please Renew the Vehicle Insurance of";
                                    date = dt_intimation_vehicle.Rows[i]["ins"].ToString();
                                    sql = "nextins_date";
                                }

                            }

                            if (dt_intimation_vehicle.Rows[i]["fc"] != null)
                            {
                                if (dt_intimation_vehicle.Rows[i]["fc"].ToString() != "")
                                {
                                    description = "Please Renew the Vehicle FC of";
                                    date = dt_intimation_vehicle.Rows[i]["fc"].ToString();
                                    sql = "nextfcdate";
                                }
                            }

                            if (dt_intimation_vehicle.Rows[i]["permit"] != null)
                            {
                                if (dt_intimation_vehicle.Rows[i]["permit"].ToString() != "")
                                {
                                    description = "Please Renew the Vehicle Permit of";
                                    date = dt_intimation_vehicle.Rows[i]["permit"].ToString();
                                    sql = "permit_date";
                                }
                            }

                            if (dt_intimation_vehicle.Rows[i]["remainder"].ToString() == "0")
                            {
                                staff_code = dt_get_settings.Rows[0][2].ToString();
                                flag = "1";
                            }
                            else if (dt_intimation_vehicle.Rows[i]["remainder"].ToString() == "1" && dt_get_settings.Rows[0]["staff_two"].ToString() != "")
                            {
                                staff_code = dt_get_settings.Rows[0][3].ToString();
                                flag = "2";
                            }
                            else
                            {
                                staff_code = "";
                            }

                            con.Close();
                            con.Open();

                            SqlCommand cmd_last_remain = new SqlCommand("select isnull(Last_Remin,0) as Last_Remin from Vehicle_Insurance where veh_id='" + veh_id + "' and " + sql + "='" + date + "'", con);
                            SqlDataAdapter ad_last_remain = new SqlDataAdapter(cmd_last_remain);
                            DataTable dt_last_remain = new DataTable();
                            ad_last_remain.Fill(dt_last_remain);

                            int diff = 0;

                            if (dt_last_remain.Rows.Count > 0)
                            {
                                DateTime last = Convert.ToDateTime(dt_last_remain.Rows[0]["Last_Remin"].ToString());

                                diff = Convert.ToInt32((Convert.ToDateTime(cur_date) - last).Days);

                            }

                            if (flag == "1" || (flag == "2" && diff == 1))
                            {

                                DataView dv_mob_no = new DataView();
                                dt_mob_no.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                dv_mob_no = dt_mob_no.DefaultView;
                                if (dv_mob_no.Count > 0)
                                {
                                    mob_no = dv_mob_no[0]["per_mobileno"].ToString();
                                    email_id = dv_mob_no[0]["email"].ToString();

                                    string[] spl_renew = date.Split(' ');
                                    string[] spl_date = spl_renew[0].Split('/');

                                    string renew_date = spl_date[1] + "/" + spl_date[0] + "/" + spl_date[2];

                                    string sms_content = description + " " + veh_id;

                                    string sms_text = "Renew Date:" + renew_date;
                                    //Modified By srinath 8/2/2014
                                    // string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mob_no + "&text=" + sms_text + "&priority=ndnd&stype=normal";
                                    //string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mob_no + "&message=" + sms_text + "&sender=" + SenderID;
                                    // string isstf = "1";
                                    //send_mail = "sudhagarpvs@gmail.com";
                                    //mob_no = "9789824009";
                                    int strpath1 = d2.send_sms(user_id, Convert.ToString(Session["collegecode"]), Convert.ToString(Session["UserCode"]), mob_no, sms_content, "1");
                                    //  smsreport(strpath1, isstf, sms_text, mob_no);

                                    try
                                    {
                                        SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                        MailMessage mailmsg = new MailMessage();
                                        MailAddress mfrom = new MailAddress(send_mail);
                                        mailmsg.From = mfrom;
                                        mailmsg.To.Add(email_id);
                                        mailmsg.Subject = "Transport Reminder";
                                        mailmsg.IsBodyHtml = true;
                                        //mailmsg.Body = "Hi ";
                                        //mailmsg.Body = mailmsg.Body + staff_namew;
                                        mailmsg.Body = mailmsg.Body + sms_content;
                                        mailmsg.Body = mailmsg.Body + "<br><br> Renew Date:" + renew_date + "<br><br>Thank You...";
                                        Mail.EnableSsl = true;
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.UseDefaultCredentials = false;
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                    }
                                    catch { }

                                    con.Close();
                                    con.Open();

                                    SqlCommand cmd_update = new SqlCommand("update Vehicle_Insurance set remainder='" + flag + "',Last_Remin='" + cur_date + "' where veh_id='" + veh_id + "' and " + sql + "='" + date + "'", con);
                                    cmd_update.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    public void smsreport(string uril, string isstaff, string content, string mobile)
    {
        WebRequest request = WebRequest.Create(uril);
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        StreamReader sr = new StreamReader(data);
        string strvel = sr.ReadToEnd();

        string groupmsgid = "";
        groupmsgid = strvel;
        string date = DateTime.Now.ToString("dd/MM/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");

        int sms = 0;
        string smsreportinsert = "";

        smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date ,sender_id)values( '" + mobile + "','" + groupmsgid + "','" + content + "','" + collegecode + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')"; // Added by jairam 21-11-2014
        sms = d2.insert_method(smsreportinsert, hat, "Text");

    }

    public bool isresign(string staffcode)
    {
        try
        {
            DataSet dtstaff = new DataSet();
            string resign = string.Empty;
            string settled = string.Empty;
            string SelectQ = "  select resign,settled,staff_code from staffmaster where staff_code='" + staffcode + "'";
            dtstaff = d2.select_method_wo_parameter(SelectQ, "Text");
            if (dtstaff.Tables[0].Rows.Count > 0)
            {
                resign = Convert.ToString(dtstaff.Tables[0].Rows[0]["resign"]);
                settled = Convert.ToString(dtstaff.Tables[0].Rows[0]["settled"]);
            }
            if (resign.Trim() == "1" || settled.Trim() == "1" || resign.Trim().ToLower() == "true" || settled.Trim().ToLower() == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
           // string otp = d2.GetFunction("select OTPNumber from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code and staff_code='" + txtuname.Text + "'");
            string otp = d2.GetFunction("select OTPNumber from usermaster where user_id='" + txtuname.Text + "'");
            //string appNo = d2.GetFunction("select app_no from Registration where Reg_No='" + txtuname.Text + "'");
            if (!string.IsNullOrEmpty(otp) && otp != "0")
            {
                if (otp.Trim() == txtOtp.Text.Trim())
                {
                    Div1.Visible = false;
                    // Response.Redirect("IndReport.aspx?app=" + Encrypt(appNo) + "&Type=Student");
                    Response.Redirect("Default_LoginPage.aspx");
                }
                else
                {
                    // Response.Redirect("Default.aspx");
                }

            }
            else
            {

                Div1.Visible = false;
                //Response.Redirect("Default.aspx");
            }
        }
        catch
        {
            //Div1.Visible = false;
            //Response.Redirect("Default.aspx");
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Div1.Visible = false;
    }
}


