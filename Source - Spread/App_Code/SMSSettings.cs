using System.Collections;
using System;
/// <summary>
/// SMS using SMSSettings gateway
/// </summary>

public class SMSSettings : DAccess2
{
    private byte isStaff;
    private bool isGroupSMS;
    private bool isDegreewise;

    private int user_collegecode;
    private int user_degreecode;

    private string user_login_id;
    private string user_login_password;
    private string text_message;
    private string user_usercode;
    private string smsURI;
    private string sender_id;
    private string mobileNos;
    private string admissionNos;


    /// <summary>
    /// User ID for the SMS Gateway
    /// </summary>
    public string User_login_id
    {
        get { return user_login_id; }
        set { user_login_id = value; }
    }
    /// <summary>
    /// Password for the SMS Gateway
    /// </summary>
    public string User_login_password
    {
        get { return user_login_password; }
        set { user_login_password = value; }
    }
    /// <summary>
    /// Message to send
    /// </summary>
    public string Text_message
    {
        get { return text_message; }
        set { text_message = value; }
    }
    /// <summary>
    /// User code of the staff who logged in
    /// </summary>
    public string User_usercode
    {
        get { return user_usercode; }
        set { user_usercode = value; }
    }
    /// <summary>
    /// Whether user is staff(1) or student (0)
    /// </summary>
    public byte IsStaff
    {
        get { return isStaff; }
        set { isStaff = value; }
    }
    /// <summary>
    /// Whether SMS is group or single
    /// </summary>
    public bool IsGroupSMS
    {
        get { return isGroupSMS; }
        set { isGroupSMS = value; }
    }
    /// <summary>
    /// College code of the Student
    /// </summary>
    public int User_collegecode
    {
        get { return user_collegecode; }
        set { user_collegecode = value; }
    }
    /// <summary>
    /// Degree Code of the Student
    /// </summary>
    public int User_degreecode
    {
        get { return user_degreecode; }
        set { user_degreecode = value; }
    }
    /// <summary>
    /// Whether to send SMS based on degreewise settings
    /// </summary>
    public bool IsDegreewise
    {
        get { return isDegreewise; }
        set { isDegreewise = value; }
    }
    /// <summary>
    /// URI for sending the SMS
    /// </summary>
    public string SmsURI
    {
        get { return smsURI; }
        set { smsURI = value; }
    }
    /// <summary>
    /// Sender ID for the SMS
    /// </summary>
    public string Sender_id
    {
        get { return sender_id; }
        set { sender_id = value; }
    }
    /// <summary>
    /// Mobile Nos to send 
    /// </summary>
    public string MobileNos
    {
        get { return mobileNos; }
        set { mobileNos = value; }
    }
    /// <summary>
    /// Students Admission Number
    /// </summary>
    public string AdmissionNos
    {
        get { return admissionNos; }
        set { admissionNos = value; }
    }
    /// <summary>
    /// Set initial values for the SMSSettings
    /// </summary>
    public SMSSettings()
    {
        User_login_id = string.Empty;
        User_login_password = string.Empty;
        Text_message = string.Empty;
        User_usercode = string.Empty;
        IsStaff = 0;
        IsGroupSMS = false;
        User_collegecode = 13;
        User_degreecode = 0;
        IsDegreewise = false;
        SmsURI = string.Empty;
        Sender_id = string.Empty;
        MobileNos = string.Empty;
    }
    /// <summary>
    /// Function to send message
    /// </summary>
    public int sendTextMessage()
    {
        byte sms_settings = getSMSSettings(User_collegecode);
        int sentMessages = 0;
        switch (sms_settings)
        {
            case 0:
                //Common SMS
                User_login_id = Convert.ToString(GetFunction("select SMS_User_ID from Track_Value where college_code='" + User_collegecode + "'"));
                sentMessages = send_sms(User_login_id, User_collegecode.ToString(), User_usercode, MobileNos, Text_message, IsStaff.ToString());
                break;
            case 1:
                //Individual Department wise SMS
                sentMessages = sendNewSMS(User_degreecode.ToString(), User_collegecode.ToString(), User_usercode, MobileNos, Text_message, IsStaff.ToString());
                break;
        }
        return sentMessages;
    }
    /// <summary>
    /// Function to send message
    /// </summary>
    public int sendTextMessage(byte sms_settings)
    {
        int sentMessages = 0;
        switch (sms_settings)
        {
            case 0:
                //Common SMS
                User_login_id = Convert.ToString(GetFunction("select SMS_User_ID from Track_Value where college_code='" + User_collegecode + "'"));
                sentMessages = send_sms(User_login_id, User_collegecode.ToString(), User_usercode, MobileNos, Text_message, IsStaff.ToString());
                break;
            case 1:
                //Individual Department wise SMS
                sentMessages = sendNewSMS(User_degreecode.ToString(), User_collegecode.ToString(), User_usercode, MobileNos, Text_message, IsStaff.ToString());
                break;
        }
        return sentMessages;
    }
    /// <summary>
    /// Get the settings for the College - Individual or Common
    /// </summary>
    /// <param name="collegeCode">Users's college code</param>
    /// <returns></returns>
    public byte getSMSSettings(int collegeCode)
    {
        byte settings = 0;
        byte.TryParse(GetFunction("select LinkValue from New_InsSettings where LinkName='SMSSettingIndividualCommon'  and college_code ='" + collegeCode + "'  "), out settings);
        return settings;
    }
    /// <summary>
    ///  Get the settings for the College - Individual or Common
    /// </summary>
    /// <param name="collegeCode">User's college code</param>
    /// <param name="userCode">User's User code</param>
    /// <returns></returns>
    public byte getSMSSettings(int collegeCode, string userCode)
    {
        byte settings = 0;
        byte.TryParse(GetFunction("select LinkValue from New_InsSettings where LinkName='SMSSettingIndividualCommon' and user_code ='" + userCode + "' and college_code ='" + collegeCode + "'  "), out settings);
        return settings;
    }
}