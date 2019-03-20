using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default_LoginPage : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();

    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    string collegecode = string.Empty;

    bool Cellclick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            string qry = string.Empty;
            if (Session["Staff_Code"] == null || string.IsNullOrEmpty(Convert.ToString(Session["Staff_Code"]).Trim()))
            {
                qry = "select ModuleName,convert(varchar(20),UpdatedDate,103) UpdatedDate,convert(varchar(20),ClientUpdatedDate,103) ClientUpdatedDate,UpdatedDesc from IPatchStatus where UpdatedDate>ClientUpdatedDate";
                DataSet dsIPatchCheck = new DataSet();
                dsIPatchCheck = d2.select_method_wo_parameter(qry, "text");
                if (dsIPatchCheck.Tables.Count > 0 && dsIPatchCheck.Tables[0].Rows.Count > 0)
                {
                    Response.Redirect("~/IpatchMod/i_patch_master.aspx");//~/IpatchMod/  http://localhost:1705/Insproplus 6.2/IpatchMod/i_patch_master.aspx
                }
                else
                {
                    dsIPatchCheck = new DataSet();
                    dsIPatchCheck = IPatchUpdated();
                    if (dsIPatchCheck.Tables.Count > 0 && dsIPatchCheck.Tables[0].Rows.Count > 0)
                    {
                        Response.Redirect("~/IpatchMod/i_patch_master.aspx");//~/IpatchMod/  http://localhost:1705/Insproplus 6.2/IpatchMod/i_patch_master.aspx
                    }
                }
            }
            bindcollege();
            //Hide By Aruna 04/June/2018 For MCC Slowness=====================
              //Transport_Remainder();
              //sendautimatcisms();
            //===============================================================
        }
    }

    protected void bindcollege()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables != null)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("Institution Name");
                dt.Columns.Add("Institution Code");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                    dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["college_code"]);
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 1)
                {
                    College_Grid.DataSource = dt;
                    College_Grid.DataBind();
                    College_Grid.Visible = true;
                    if (College_Grid.Rows.Count > 0)
                    {
                        for (int i = 0; i < College_Grid.Rows.Count; i++)
                        {
                            string clgcode = ((College_Grid.Rows[i].FindControl("lbl_institutioncode") as Label).Text);
                            if (clgcode == Convert.ToString(Session["collegecode"]))
                            {
                                College_Grid.Rows[i].BackColor = Color.LightGreen;
                                College_Grid.Rows[i].BackColor = Color.LightGreen;
                            }
                        }
                    }
                }
                else
                {
                    College_Grid.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "Default_Loginpage");
        }
    }

    protected void College_Grid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            Session["collegecode"] = (College_Grid.Rows[row].FindControl("lbl_institutioncode") as Label).Text;
            for (int i = 0; i < College_Grid.Rows.Count; i++)
            {
                College_Grid.Rows[i].BackColor = Color.White;
            }
            College_Grid.Rows[row].BackColor = Color.LightGreen;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "Default_Loginpage");
        }
    }

    protected void College_Grid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.College_Grid, "getclgcode$" + e.Row.RowIndex);
            e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.College_Grid, "getclgcode$" + e.Row.RowIndex);
        }
    }

    void Transport_Remainder()
    {
        try
        {
            string strcheckquery = "select * from smsdeliverytrackmaster";
            DataSet dscheck = d2.select_method_wo_parameter(strcheckquery, "text");
            DataView dvcheck = new DataView();
            ds1 = d2.select_method_wo_parameter("select SMS_User_ID,college_code from Track_Value where college_code = '" + Session["collegecode"].ToString() + "'", "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds1.Tables[0].Rows[0]["SMS_User_ID"]);
            }
            string getval = d2.GetUserapi(user_id);
            string[] spret = getval.Split('-');
            if (spret.GetUpperBound(0) == 1)
            {
                SenderID = spret[0].ToString();
                Password = spret[1].ToString();
                Session["api"] = user_id;
                Session["senderid"] = SenderID;
            }
            DataTable dt_get_settings = new DataTable();
            DataSet getsetting = d2.select_method_wo_parameter("Select * from transport_settings where college_code='" + Session["collegecode"].ToString() + "'", "text");
            dt_get_settings = getsetting.Tables[0];
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
                DataSet mob = d2.select_method_wo_parameter("select * from staff_appl_master a,staffmaster m where m.appl_no = a.appl_no and m.college_code = a.college_code", "text");
                DataTable dt_mob_no = new DataTable();
                dt_mob_no = mob.Tables[0];
                if (dt_mob_no.Rows.Count > 0)
                {
                    string mob_no = string.Empty;
                    string email_id = string.Empty;
                    string cur_date = DateTime.Now.ToString("MM/dd/yyyy");
                    string to_date = Convert.ToDateTime(cur_date).AddDays(remain_days).ToString();
                    string[] spl_cur_date = cur_date.Split(' ');
                    string[] spl_to_date = to_date.Split(' ');
                    DataTable dt_intimation_licence = new DataTable();
                    string intimation_licence = "select * from driverallotment where renew_date between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "'  or remainder=1 ";
                    DataSet intimationlice = d2.select_method_wo_parameter(intimation_licence, "text");
                    dt_intimation_licence = intimationlice.Tables[0];
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
                                //flag = "1";
                            }
                            else if (dt_intimation_licence.Rows[i]["remainder"].ToString() == "1" && dt_get_settings.Rows[0]["staff_two"].ToString() != "")
                            {
                                staff_code = dt_get_settings.Rows[0][3].ToString();
                                //flag = "2";
                            }
                            else
                            {
                                staff_code = "";
                            }
                            DataTable dt_last_remain = new DataTable();
                            DataSet lastremain = d2.select_method_wo_parameter("select Last_Remin as Last_Remin from driverallotment where staff_code='" + driv_code + "'", "text");
                            dt_last_remain = lastremain.Tables[0];
                            int diff = 0;
                            if (dt_last_remain.Rows.Count > 0)
                            {
                                if (dt_last_remain.Rows[0]["Last_Remin"] != null)
                                {
                                    if (dt_last_remain.Rows[0]["Last_Remin"].ToString() != "")
                                    {
                                        DateTime last = Convert.ToDateTime(dt_last_remain.Rows[0]["Last_Remin"].ToString());
                                        diff = Convert.ToInt32((Convert.ToDateTime(cur_date) - last).Days);
                                    }
                                    else
                                    {
                                        diff = 1;
                                    }
                                }
                                else
                                {
                                    diff = 1;
                                }
                            }
                            if (diff == 1)
                            {
                                DataView dv_mob_no = new DataView();
                                dt_mob_no.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                dv_mob_no = dt_mob_no.DefaultView;
                                if (dv_mob_no.Count > 0)
                                {
                                    mob_no = dv_mob_no[0]["per_mobileno"].ToString();
                                    email_id = dv_mob_no[0]["email"].ToString();
                                    //Added by srinath 1/8/2014
                                    dscheck.Tables[0].DefaultView.RowFilter = " mobilenos='" + mob_no + "' and date='" + cur_date + "'";
                                    dvcheck = dscheck.Tables[0].DefaultView;
                                    if (dvcheck.Count == 0)
                                    {
                                        string sms_content = "Please renew the driving licence of Mr." + driv_name + "-" + driv_code + "Renew Date:" + renew_date;
                                        string description = "Please renew the driving licence of Mr." + driv_name + "(" + driv_code + ")";
                                        if (send_mail != "")
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
                                        //Modified by srinath 8/2/2014
                                        // string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mob_no + "&message=" + sms_content + "&sender=" + SenderID;
                                        //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mob_no + "&text=" + sms_content + "&priority=ndnd&stype=normal";
                                        //string isstf = "1";
                                        string usercode = Session["UserCode"].ToString();
                                        //smsreport(strpath1, isstf, sms_content,mob_no, usercode);
                                        int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, mob_no, sms_content, "1");
                                        if (Convert.ToDateTime(cur_date) >= Convert.ToDateTime(spl_renew[0].ToString()))
                                        {
                                            flag = "1";
                                        }
                                        else
                                        {
                                            flag = "0";
                                        }
                                        d2.select_method_wo_parameter("update driverallotment set remainder='" + flag + "',Last_Remin='" + cur_date + "' where staff_code='" + driv_code + "'", "text");
                                    }
                                }
                            }
                        }
                    }
                    string ad_intimation_vehicle = "select veh_type,veh_id,nextins_date as ins,nextfcdate as fc,permit_date as permit,remainder from Vehicle_Insurance where CONVERT(Datetime, nextins_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, nextfcdate, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "' or CONVERT(Datetime, permit_date, 120) between '" + cur_date + "' and '" + spl_to_date[0].ToString() + "'  or remainder=1  order by veh_id";
                    DataSet initmation = d2.select_method_wo_parameter(ad_intimation_vehicle, "text");
                    DataTable dt_intimation_vehicle = new DataTable();
                    dt_intimation_vehicle = initmation.Tables[0];
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
                                //flag = "1";
                            }
                            else if (dt_intimation_vehicle.Rows[i]["remainder"].ToString() == "1" && dt_get_settings.Rows[0]["staff_two"].ToString() != "")
                            {
                                staff_code = dt_get_settings.Rows[0][3].ToString();
                                //flag = "2";
                            }
                            else
                            {
                                staff_code = "";
                            }
                            string lastremain = "select Last_Remin from Vehicle_Insurance where veh_id='" + veh_id + "' and " + sql + "='" + date + "'";
                            DataSet ad_last_remainds = d2.select_method_wo_parameter(lastremain, "text");
                            DataTable dt_last_remain = new DataTable();
                            dt_last_remain = ad_last_remainds.Tables[0];
                            int diff = 0;
                            if (dt_last_remain.Rows.Count > 0)
                            {
                                if (dt_last_remain.Rows[0]["Last_Remin"] != null)
                                {
                                    if (dt_last_remain.Rows[0]["Last_Remin"].ToString() != "")
                                    {
                                        DateTime last = Convert.ToDateTime(dt_last_remain.Rows[0]["Last_Remin"].ToString());
                                        diff = Convert.ToInt32((Convert.ToDateTime(cur_date) - last).Days);
                                    }
                                    else
                                    {
                                        diff = 1;
                                    }
                                }
                                else
                                {
                                    diff = 1;
                                }
                            }
                            if (diff == 1)
                            {
                                DataView dv_mob_no = new DataView();
                                dt_mob_no.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                dv_mob_no = dt_mob_no.DefaultView;
                                if (dv_mob_no.Count > 0)
                                {
                                    mob_no = dv_mob_no[0]["per_mobileno"].ToString();
                                    email_id = dv_mob_no[0]["email"].ToString();
                                    //Added by srinath 1/8/2014
                                    dscheck.Tables[0].DefaultView.RowFilter = " mobilenos='" + mob_no + "' and date='" + cur_date + "'";
                                    dvcheck = dscheck.Tables[0].DefaultView;
                                    if (dvcheck.Count == 0)
                                    {
                                        string[] spl_renew = date.Split(' ');
                                        string[] spl_date = spl_renew[0].Split('/');
                                        string renew_date = spl_date[1] + "/" + spl_date[0] + "/" + spl_date[2];
                                        string sms_content = description + " " + veh_id;
                                        string sms_text = sms_content + "Renew Date:" + renew_date;
                                        if (send_mail != "")
                                        {
                                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                            MailMessage mailmsg = new MailMessage();
                                            MailAddress mfrom = new MailAddress(send_mail);
                                            mailmsg.From = mfrom;
                                            mailmsg.To.Add(email_id);
                                            mailmsg.Subject = "Transport Reminder";
                                            mailmsg.IsBodyHtml = true;
                                            mailmsg.Body = mailmsg.Body + sms_content;
                                            mailmsg.Body = mailmsg.Body + "<br><br> Renew Date:" + renew_date + "<br><br>Thank You...";
                                            Mail.EnableSsl = true;
                                            NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                            Mail.UseDefaultCredentials = false;
                                            Mail.Credentials = credentials;
                                            Mail.Send(mailmsg);
                                        }
                                        //modified by srinath 8/2/2014
                                        //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mob_no + "&text=" + sms_text + "&priority=ndnd&stype=normal";
                                        ////string strpath1 = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + mob_no + "&message=" + sms_text + "&sender=" + SenderID;
                                        //string isstf = "1";
                                        string usercode = Session["UserCode"].ToString();
                                        //smsreport(strpath1, isstf, mob_no, mob_no,usercode);
                                        int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, mob_no, mob_no, "1");
                                        if (Convert.ToDateTime(cur_date) >= Convert.ToDateTime(spl_renew[0].ToString()))
                                        {
                                            flag = "1";
                                        }
                                        else
                                        {
                                            flag = "0";
                                        }
                                        string q1 = "update Vehicle_Insurance set remainder='" + flag + "',Last_Remin='" + cur_date + "' where veh_id='" + veh_id + "' and " + sql + "='" + date + "'";
                                        d2.update_method_wo_parameter(q1, "text");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void smsreport(string uril, string isstaff, string content, string mobile, string usercode)
    {
        WebRequest request = WebRequest.Create(uril);
        WebResponse response = request.GetResponse();
        Stream data = response.GetResponseStream();
        StreamReader sr = new StreamReader(data);
        string strvel = sr.ReadToEnd();
        string groupmsgid = "";
        groupmsgid = strvel;
        string date = DateTime.Now.ToString("MM/dd/yyyy");
        int sms = 0;
        string smsreportinsert = "";
        smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + mobile + "','" + groupmsgid + "','" + content + "','" + collegecode + "','" + isstaff + "','" + date + "','" + usercode.ToString() + "')";// Added by jairam 21-11-2014
        sms = d2.insert_method(smsreportinsert, hat, "Text");
    }

    public void sendautimatcisms()
    {
        try
        {
            string send_mail = "";
            string send_pw = "";
            string setval = "select a.staff_code,sa.per_mobileno,sa.email,sa.com_mobileno,a.template,a.emailtemplate,a.user_code,a.staff_code,a.Send_Date,a.sending_Time from Automatic_SMS a,staffmaster s,staff_appl_master sa where a.staff_code=s.staff_code and s.appl_no=sa.appl_no and is_web='1' and (IsSend is null or IsSend<>'1')";
            DataSet dssms = d2.select_method_wo_parameter(setval, "Text");
            if (dssms.Tables[0].Rows.Count > 0)
            {
                string strcheckquery = "select SMS_User_ID,college_code from Track_Value where college_code = '" + Session["collegecode"].ToString() + "'";
                DataSet dscheck = d2.select_method_wo_parameter(strcheckquery, "text");
                if (dscheck.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dscheck.Tables[0].Rows[0]["SMS_User_ID"]);
                }
                string strquery = "select massemail,masspwd from collinfo where college_code = " + Session["collegecode"].ToString() + " ";
                ds1.Dispose();
                ds1.Reset();
                ds1 = d2.select_method(strquery, hat, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                    send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
                }
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[1].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                Boolean emailerrflag = false;
                for (int i = 0; i < dssms.Tables[0].Rows.Count; i++)
                {
                    string mobile = dssms.Tables[0].Rows[i]["per_mobileno"].ToString();
                    string email = dssms.Tables[0].Rows[i]["email"].ToString();
                    string template = dssms.Tables[0].Rows[i]["template"].ToString();
                    string etemplate = dssms.Tables[0].Rows[i]["emailtemplate"].ToString();
                    string usercode = dssms.Tables[0].Rows[i]["user_code"].ToString();
                    string staffcode = dssms.Tables[0].Rows[i]["staff_code"].ToString();
                    string sdate = dssms.Tables[0].Rows[i]["Send_Date"].ToString();
                    string stime = dssms.Tables[0].Rows[i]["sending_Time"].ToString();
                    if (mobile.Trim() != "" && mobile != null)
                    {
                        //string strpath1 = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobile + "&text=" + template + "&priority=ndnd&stype=normal";
                        //string isstf = "1";
                        //smsreport(strpath1, isstf, template, mobile, usercode);
                        int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), usercode, mobile, template, "1");
                    }
                    string solve = "update Automatic_SMS set IsSend='1' where is_web ='1' and staff_code='" + staffcode + "' and sending_Time='" + stime + "' and Send_Date='" + sdate + "'";
                    int val = d2.update_method_wo_parameter(solve, "Text");
                    if (emailerrflag == false)
                    {
                        if (email.Trim() != "" && email != null)
                        {
                            try
                            {
                                string[] spva = etemplate.Split('%');
                                string content = "";
                                string boby = "";
                                if (spva.GetUpperBound(0) >= 1)
                                {
                                    content = spva[0].ToString();
                                    boby = spva[1].ToString();
                                }
                                SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                MailMessage mailmsg = new MailMessage();
                                MailAddress mfrom = new MailAddress(send_mail);
                                mailmsg.From = mfrom;
                                mailmsg.To.Add(email);
                                mailmsg.Subject = content;
                                mailmsg.IsBodyHtml = true;
                                mailmsg.Body = mailmsg.Body + content;
                                mailmsg.Body = mailmsg.Body + "<br><br>" + boby + "<br><br>Thank You...";
                                Mail.EnableSsl = true;
                                NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                Mail.UseDefaultCredentials = false;
                                Mail.Credentials = credentials;
                                Mail.Send(mailmsg);
                            }
                            catch
                            {
                                emailerrflag = true;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    public DataSet IPatchUpdated()
    {
        DataSet dsIPatchCheck = new DataSet();
        try
        {
            string qry = "select ModuleName,convert(varchar(20),UpdatedDate,103) UpdatedDate,convert(varchar(20),ClientUpdatedDate,103) ClientUpdatedDate,UpdatedDesc from IPatchStatus where ClientUpdatedDate<'04/17/2017'";
            dsIPatchCheck = d2.select_method_wo_parameter(qry, "text");
        }
        catch
        {
        }
        return dsIPatchCheck;
    }

}