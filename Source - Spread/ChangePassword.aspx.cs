using System;
using System.Data;
using System.Collections;
using System.Text;

public partial class ChangePassword : System.Web.UI.Page
{
    string usercode = string.Empty;
    const int basekey = 43;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    const int addtokey = 17;

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        if (!IsPostBack)
        {
          
            lblerr.Visible = false;
            string userquery = "select user_id from usermaster where user_code=" + usercode + "";
            string username = d2.GetFunction(userquery);
            lbluser.Text = username;
        }

    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (txtoldpassword.Text == "")
        {
            string SelectQuerry = "select * from usermaster where user_id='" + lbluser.Text + "' and PassWord='" + txtoldpassword.Text + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(SelectQuerry, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = false;

                if (txtnewpassword.Text == txtconform.Text)
                {
                    string enpass="";
                    if (txtnewpassword.Text != "")
                    {
                        enpass = encryptdata(txtnewpassword.Text);
                    }
                    string savequery = "update usermaster set password='" + enpass + "' where user_code='" + usercode + "'";
                    int sa = d2.update_method_wo_parameter(savequery, "Text");
                    if (sa == 1)
                    {
                        lblerr.Text = "Password Updated Suceessfully";
                        lblerr.Visible = true;
                    }
                    else
                    {
                        lblerr.Text = "Password Updated Failed";
                        lblerr.Visible = true;
                    }
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Password Must Match";
                    txtconform.Text = "";
                    txtnewpassword.Text = "";
                }

            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter Valid Old Password";
            }
        }
        else
        {
          string passwd = txtoldpassword.Text;
           string afterenc = encryptdata(passwd);
            string SelectQuerry = "select * from usermaster where user_id='" + lbluser.Text + "' and PassWord='" + afterenc + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(SelectQuerry, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblerr.Visible = false;
                lblerr.Text = "Hi";

                if (txtnewpassword.Text == txtconform.Text)
                {
                    string enpass = "";
                    if (txtnewpassword.Text != "")
                    {
                         enpass = encryptdata(txtnewpassword.Text);
                    }
                    string savequery = "update usermaster set password='" + enpass + "' where user_code='" + usercode + "'";
                    int sa = d2.update_method_wo_parameter(savequery, "Text");
                    if (sa == 1)
                    {
                        lblerr.Text = "Password Updated Suceessfully";
                        lblerr.Visible = true;
                    }
                    else
                    {
                        lblerr.Text = "Password Updated Failed";
                        lblerr.Visible = true;
                    }
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Password Must Match";
                    txtconform.Text = "";
                    txtnewpassword.Text = "";
                }
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter Correct Old Password";
                txtoldpassword.Text = "";
            }
        }
       
    }
  
    public string encryptdata(string text)
    {

        int counter;
        int daykey;
        string retdata = "";
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
       
        int counter;
        string newkey = "";
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

   
}