/*
 * Author : Idhris
 * Date Created  : 10-11-2016
 * 
 */

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
using InsproDataAccess;
using System.Globalization;
using System.Configuration;
using System.Data.SqlClient;

public partial class StudentLeaveRequestOff : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staffcodesession = string.Empty;

    SqlConnection mysql = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    // DateTime fromdate = new DateTime();

    static int chosedmode = 0;
    static DAccess2 d22 = new DAccess2();
    static string collegecodestat = string.Empty;
    static string usercodestat = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            staffcodesession = Session["Staff_Code"].ToString().Trim();
            staffcodesession = string.IsNullOrEmpty(staffcodesession) ? usercode : staffcodesession;

            if (!IsPostBack)
            {
                bindclg();
                collegecode = ddl_college.Items.Count > 0 ? ddl_college.SelectedValue : "13";
                setLabelText();
                loadSettings();
                LoadFromSettings();
            }
            collegecode = ddl_college.Items.Count > 0 ? ddl_college.SelectedValue : "13";
            collegecodestat = collegecode;
            usercodestat = usercode;
        }
        catch { }
    }
    private void loadSettings()
    {
        divRequestTab.Visible = false;
        divRequestLink.Visible = false;

        divReportTab.Visible = false;
        divReportLink.Visible = false;

        divApproveTab.Visible = false;
        divApproveRejectLink.Visible = false;

        try
        {

            int repOk = 0;
            int.TryParse(DA.GetFunction("select ReqAppStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo=(select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "')  and CollegeCode='" + collegecode + "' and RequestType='10' "), out repOk);//

            if (repOk > 0 || staffcodesession == usercode)
            {
                divReportTab.Visible = true;
                divReportLink.Visible = true;
                ButtonReport_Click(new object(), new EventArgs());
            }

            int reqOk = 0;
            int.TryParse(DA.GetFunction("select ReqAppStaffAppNo from RQ_RequestHierarchy where ReqStaffAppNo=(select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "')  and CollegeCode='" + collegecode + "' and RequestType='10'"), out reqOk);//and RequestType='10'

            if (reqOk > 0 || staffcodesession == usercode)
            {
                divRequestTab.Visible = true;
                divRequestLink.Visible = true;
                ButtonReq_Click(new object(), new EventArgs());
            }
            int appOk = 0;
            int.TryParse(DA.GetFunction("select ReqAppStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo=(select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "')  and CollegeCode='" + collegecode + "' and RequestType='10' "), out appOk);//
            if (appOk > 0)
            {
                divApproveTab.Visible = true;
                divApproveRejectLink.Visible = true;
                ButtonApprove_Click(new object(), new EventArgs());
            }
        }
        catch { }


        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdate.Attributes.Add("readonly", "readonly");

        txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todate.Attributes.Add("readonly", "readonly");

        txt_fromdateRep.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdateRep.Attributes.Add("readonly", "readonly");

        txt_todateRep.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todateRep.Attributes.Add("readonly", "readonly");

        txt_fromdateApp.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_fromdateApp.Attributes.Add("readonly", "readonly");

        txt_todateApp.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_todateApp.Attributes.Add("readonly", "readonly");
    }
    public void LoadFromSettings()
    {
        try
        {
            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(DA.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }


            int smartDisp = Convert.ToInt32(DA.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + collegecode + ")").Trim());

            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";

            int save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(DA.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard No - smart_serial_no
                rbl_rollno.Items.Add(lst5);
            }

            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    lbl_rollno3.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                case2:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    lbl_rollno3.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                case3:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    lbl_rollno3.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                case4:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    lbl_rollno3.Text = "App No";
                    chosedmode = 3;
                    break;
                case 4:
                    txt_rollno.Attributes.Add("placeholder", "Smartcard No");
                    lbl_rollno3.Text = "SmartCard No";
                    chosedmode = 4;
                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }
        }
        catch { }
    }
    public void bindclg()
    {
        try
        {
            ddl_college.Items.Clear();
            ddlClgRep.Items.Clear();
            ddlClgApp.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
            DataSet ds = DA.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();

                ddlClgRep.DataSource = ds;
                ddlClgRep.DataTextField = "collname";
                ddlClgRep.DataValueField = "college_code";
                ddlClgRep.DataBind();

                ddlClgApp.DataSource = ds;
                ddlClgApp.DataTextField = "collname";
                ddlClgApp.DataValueField = "college_code";
                ddlClgApp.DataBind();

                bindReqStage();
            }
        }
        catch (Exception ex) { }
    }
    public void bindReqStage()
    {
        try
        {
            ddlReqStage.Items.Clear();
            string selectQuery = "select distinct ReqApproveStage from RQ_RequestHierarchy where ReqAppStaffAppNo=(select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "')  and RequestType='10'  and CollegeCode='" + (ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13") + "'";
            DataSet ds = DA.select_method_wo_parameter(selectQuery, "Text");//
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlReqStage.DataSource = ds;
                ddlReqStage.DataTextField = "ReqApproveStage";
                ddlReqStage.DataValueField = "ReqApproveStage";
                ddlReqStage.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_college_OnSelectedIndexchange(object sender, EventArgs e)
    {
        loadSettings();
        LoadFromSettings();
        txt_rollno.Text = "";
        textRoll();
    }

    private DataSet LeaveMaster(string degCode, string collegeCode)
    {
        DataSet dsLeaveDet = new DataSet();
        try
        {
            string LeaveMasterQ = "SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND CollegeCode ='" + collegeCode + "' AND DegreeCode='" + degCode + "'";
            dsLeaveDet = DA.select_method_wo_parameter(LeaveMasterQ, "Text");
        }
        catch { }
        return dsLeaveDet;
    }
    // Key management for scrambling support
    public byte[] ScrambleKey
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                // Use existing key if non provided
                key = ScrambleKey;
            }
            Session["ScrambleKey"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleKey"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateKey();
                key = rc2.Key;
                Session["ScrambleKey"] = key;
            }
            return key;
        }
    }
    // Initialization vector management for scrambling support
    public byte[] ScrambleIV
    {
        set
        {
            byte[] key = value;
            if (null == key)
            {
                key = ScrambleIV;
            }
            Session["ScrambleIV"] = key;
        }
        get
        {
            byte[] key = (byte[])Session["ScrambleIV"];
            if (null == key)
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.GenerateIV();
                key = rc2.IV;
                Session["ScrambleIV"] = key;
            }
            return key;
        }
    }
    public string Encrypt(string message)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();

        //Convert the data to a byte array.
        byte[] toEncrypt = textConverter.GetBytes(message);

        //Get an encryptor.
        ICryptoTransform encryptor = rc2CSP.CreateEncryptor(ScrambleKey, ScrambleIV);

        //Encrypt the data.
        MemoryStream msEncrypt = new MemoryStream();
        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        //Write all data to the crypto stream and flush it.
        // Encode length as first 4 bytes
        byte[] length = new byte[4];
        length[0] = (byte)(message.Length & 0xFF);
        length[1] = (byte)((message.Length >> 8) & 0xFF);
        length[2] = (byte)((message.Length >> 16) & 0xFF);
        length[3] = (byte)((message.Length >> 24) & 0xFF);
        csEncrypt.Write(length, 0, 4);
        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
        csEncrypt.FlushFinalBlock();

        //Get encrypted array of bytes.
        byte[] encrypted = msEncrypt.ToArray();

        // Convert to Base64 string
        string b64 = Convert.ToBase64String(encrypted);

        // Protect against URLEncode/Decode problem
        string b64mod = b64.Replace('+', '@');

        // Return a URL encoded string
        return HttpUtility.UrlEncode(b64mod);
    }
    public string Decrypt(string scrambledMessage)
    {
        UTF8Encoding textConverter = new UTF8Encoding();
        RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
        // URL decode , replace and convert from Base64
        string b64mod = HttpUtility.UrlDecode(scrambledMessage);
        // Replace '@' back to '+' (avoid URLDecode problem)
        string b64 = b64mod.Replace('@', '+');
        // Base64 decode
        byte[] encrypted = Convert.FromBase64String(b64);

        //Get a decryptor that uses the same key and IV as the encryptor.
        ICryptoTransform decryptor = rc2CSP.CreateDecryptor(ScrambleKey, ScrambleIV);

        //Now decrypt the previously encrypted message using the decryptor
        // obtained in the above step.
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        byte[] fromEncrypt = new byte[encrypted.Length - 4];

        //Read the data out of the crypto stream.
        byte[] length = new byte[4];
        csDecrypt.Read(length, 0, 4);
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
        int len = (int)length[0] | (length[1] << 8) | (length[2] << 16) | (length[3] << 24);

        //Convert the byte array back into a string.
        return textConverter.GetString(fromEncrypt).Substring(0, len);
    }
    protected void lblogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { }
    }
    protected void ButtonReq_Click(object sender, EventArgs e)
    {
        divRequestTab.Visible = true;
        divReportTab.Visible = false;
        divApproveTab.Visible = false;

        divRequestLink.Style.Add("background-Color", "rgba(255, 255, 255, .3)");
        divReportLink.Style.Add("background-Color", "#226399");
        divApproveRejectLink.Style.Add("background-Color", "#226399");

        //txt_rqstn_leave.Text = generateReqCode();
        //txt_time_rqstn_leave.Text = DateTime.Now.ToString("dd/MM/yyyy");
        BindReasons();
        BindLeaveType();
        string appNo = (string.IsNullOrEmpty(txtIntAppNo.Text.Trim()) || txtIntAppNo.Text.Trim() == "0") ? string.Empty : txtIntAppNo.Text.Trim();
        string DegCode = txtIntDegCode.Text.Trim();
        string curSem = txt_Sem.Text.Trim();
        string batch = txt_Batc.Text.Trim();
        string rollNo = Session["RollNo"] != null ? Session["RollNo"].ToString() : string.Empty;
        BindGridLeaveDetails(appNo, DegCode, curSem, batch, rollNo, gridLeaveHistory);
        BindGridview();
        //bindStaff();
    }
    private string generateReqCode()
    {
        string reqCode = string.Empty;
        return reqCode;
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = TextToDate(txt_fromdate);
            DateTime todate = TextToDate(txt_todate);

            if (fromdate > todate)
            {
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //Response.Write("<script>alert('From Date Should Not Exceed To Date')</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
            }
            //else
            //{
            //    if (!DaysCHeck(fromdate, todate)) ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Only " + lblRemLeaveAns.Text + " Leaves Remaining')", true); ;
            //}
            BindGridview();
        }
        catch { }
    }
    private bool DaysCHeck(DateTime fromdate, DateTime todate)
    {
        int choosedDays = ((int)(todate - fromdate).TotalDays) + 1;
        int remLeave = 0;
        int.TryParse(lblRemLeaveAns.Text, out remLeave);
        if (choosedDays > remLeave || choosedDays < 0)
        {
            //txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Only " + remLeave + " Leaves Remaining')", true);
            return false;
        }
        return true;
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lblClgApp);
        fields.Add(0);

        lbl.Add(lblClgRep);
        fields.Add(0);

        lbl.Add(lblDegree);
        fields.Add(2);

        lbl.Add(lbl_degree2);
        fields.Add(2);

        lbl.Add(lblBranch);
        fields.Add(3);

        lbl.Add(lblSemester);
        fields.Add(4);

        txt_dept.Attributes.Add("placeholder", lblBranch.Text);
        txt_Sem.Attributes.Add("placeholder", lblSemester.Text);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }
    public string subjectcode(string textcri, string subjename, string collegeCode)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegeCode + " and TextVal='" + subjename + "'";
            DataSet ds = new DataSet();
            ds.Clear();
            ds = DA.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegeCode + "')";
                int result = DA.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegeCode + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = DA.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                    }
                }
            }
        }
        catch
        {

        }
        return subjec_no;
    }
    public void BindReasons()
    {
        ddlLeaveReason.Items.Clear();
        try
        {
            DataSet dsReasons = DA.select_method_wo_parameter("select TextVal,TextCode from textvaltable where TextCriteria='LEVAP' and college_code =" + collegecode + "", "Text");
            if (dsReasons.Tables.Count > 0 && dsReasons.Tables[0].Rows.Count > 0)
            {
                ddlLeaveReason.DataSource = dsReasons;
                ddlLeaveReason.DataTextField = "TextVal";
                ddlLeaveReason.DataValueField = "TextCode";
                ddlLeaveReason.DataBind();
            }
        }
        catch { }
        ddlLeaveReason.Items.Insert(0, "Select");
        ddlLeaveReason.Items.Add("Others");
        txtReasonLeave.Visible = false;
        txtReasonLeave.Text = string.Empty;
    }
    public void BindLeaveType()
    {
        try
        {
            ddl_leave_type.Items.Clear();
            DataSet dsLType = DA.select_method_wo_parameter("select EntryCode,LeaveCode,DispText from AttMasterSetting where CollegeCode='" + collegecode + "' and CalcFlag='1'", "Text");
            if (dsLType.Tables.Count > 0 && dsLType.Tables[0].Rows.Count > 0)
            {
                ddl_leave_type.DataSource = dsLType;
                ddl_leave_type.DataTextField = "DispText";
                ddl_leave_type.DataValueField = "LeaveCode";
                ddl_leave_type.DataBind();
            }
        }
        catch { }
        ddl_leave_type.Items.Insert(0, new ListItem("Select", string.Empty));
    }
    private DataTable getHolidays(string degCode, string curSem)
    {
        DataTable dtHolidays = new DataTable();
        try
        {
            string holidayQ = "select Convert(varchar(10),holiday_date,101) as Holidate,holiday_desc,semester,degree_code,isnull(cast(halforfull as int),0) as halforfull,isnull(cast(morning as int),0) as morning,isnull(cast(evening as int),0) as evening  from holidayStudents  where degree_code = '" + degCode + "' and semester = '" + curSem + "' ";
            DataSet dsHoliday = DA.select_method_wo_parameter(holidayQ, "Text");
            if (dsHoliday.Tables.Count > 0)
            {
                dtHolidays = dsHoliday.Tables[0];
            }
        }
        catch { }
        return dtHolidays;
    }
    private bool isSunday(DateTime dt)
    {
        bool yesSunday = false;
        if (dt.DayOfWeek.ToString().ToLower() == "sunday")
        {
            yesSunday = true;
        }
        return yesSunday;
    }
    public void BindGridview()
    {
        try
        {
            //int remLeave = 0;
            //int.TryParse(lblRemLeaveAns.Text, out remLeave);
            //if (remLeave > 0)
            //{
            div_GV1.Visible = true;
            btnReqSave.Visible = true;
            spanHolidays.InnerHtml = string.Empty;


            string DegCode = txtIntDegCode.Text.Trim();
            string curSem = txt_Sem.Text.Trim();

            DataTable dtHoliDays = getHolidays(DegCode, curSem);




            ArrayList addnew = new ArrayList();
            DateTime fromdate = new DateTime();
            fromdate = TextToDate(txt_fromdate);

            DateTime todate = new DateTime();
            todate = TextToDate(txt_todate);

            TimeSpan c = fromdate - todate;
            DataTable dt = new DataTable();
            dt.Columns.Add("Dummy");
            dt.Columns.Add("Dummy1");
            dt.Columns.Add("Dummy2");
            dt.Columns.Add("Dummy3");


            //if (fromdate != todate)
            //{
            StringBuilder sbHolidays = new StringBuilder();
            for (; fromdate <= todate; )
            {
                string to = Convert.ToString(txt_fromdate.Text);
                string from = Convert.ToString(txt_todate.Text);

                DataView dvholiday = new DataView();
                if (dtHoliDays.Rows.Count > 0)
                {
                    dtHoliDays.DefaultView.RowFilter = "(halforfull='0') and Holidate='" + fromdate.Date.ToString() + "'";
                    dvholiday = dtHoliDays.DefaultView;
                }
                if (dvholiday.Count == 0 && !isSunday(fromdate))
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "1";
                    dr[1] = fromdate.ToString("dd/MM/yyyy");
                    dt.Rows.Add(dr);
                    fromdate = fromdate.AddDays(1);
                }
                else
                {
                    sbHolidays.Append("<tr  style='color:Black;  font-size:Medium;'><td style='width:100px;'>" + fromdate.ToString("dd/MM/yyyy") + "</td><td  style='width:250px;'>" + (isSunday(fromdate) ? "Sunday" : dvholiday[0]["holiday_desc"].ToString()) + "</td></tr>");
                    fromdate = fromdate.AddDays(1);
                }
            }

            //}
            //else
            //{
            //    dr = dt.NewRow();
            //    dr[0] = "1";
            //    dr[1] = fromdate.ToString("dd/MM/yyyy");

            //    dt.Rows.Add(dr);

            //    fromdate = fromdate.AddDays(1);
            //}

            if (dt.Rows.Count > 0)
            {
                GV1.DataSource = dt;
                GV1.DataBind();

                spanHolidays.InnerHtml = sbHolidays.ToString().Trim() != string.Empty ? "<table style='width:350px; margin-top:0px; ' border='1' cellpadding='0' cellspacing='0'><tr><td colspan='2' style='background-color:#0CA6CA; color:Black; font-weight:bold; font-size:Medium;'><center>Holidays</center></td></tr>" + sbHolidays.ToString() + "</table>" : string.Empty;
                GV1.Visible = true;
                div_GV1.Visible = true;
            }
            //}
            //else
            //{
            //    div_GV1.Visible = false;
            //    btnReqSave.Visible = false;
            //}
        }
        catch { }
    }

    public void BindGridLeaveDetails(string appNo, string DegCode, string curSem, string batch, string rollNo, GridView grid)
    {
        try
        {


            DataSet dsSemStartEnd = DA.select_method_wo_parameter("select start_date,end_date from seminfo where degree_code='" + DegCode + "' and semester='" + curSem + "' and batch_year='" + batch + "'", "Text");

            //DataSet dsLType = DA.select_method_wo_parameter("select EntryCode,LeaveCode,DispText from AttMasterSetting where CollegeCode='" + collegecode + "' and CalcFlag='1'", "Text");
            string type = "select distinct  a.EntryCode,LeaveCode,DispText from AttMasterSetting a,leaveMaster l where l.EntryCode=a.EntryCode and l.collegeCode=a.CollegeCode and   l.CollegeCode='" + collegecode + "'   and l.batchyear='" + batch + "' and l.semester='" + curSem + "'";
            DataSet dsLType = DA.select_method_wo_parameter(type, "text");


            if (dsSemStartEnd.Tables.Count > 0 && dsSemStartEnd.Tables[0].Rows.Count > 0 && dsLType.Tables.Count > 0 && dsLType.Tables[0].Rows.Count > 0)
            {
                #region Month retrieval
                DateTime startDate = Convert.ToDateTime(dsSemStartEnd.Tables[0].Rows[0]["start_date"]);
                DateTime endDate = Convert.ToDateTime(dsSemStartEnd.Tables[0].Rows[0]["end_date"]);

                var start = startDate;
                var end = endDate;

                // set end-date to end of month
                end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

                var diff = Enumerable.Range(0, Int32.MaxValue)
                                     .Select(e => start.AddMonths(e))
                                     .TakeWhile(e => e <= end)
                                     .Select(e => e.ToString("MMMM"));
                List<string> lsMonth = diff.ToList<string>();
                #endregion

                Dictionary<byte, string> leaveCodes = new Dictionary<byte, string>();
                for (int lvType = 0; lvType < dsLType.Tables[0].Rows.Count; lvType++)
                {
                    leaveCodes.Add(Convert.ToByte(dsLType.Tables[0].Rows[lvType]["LeaveCode"]), Convert.ToString(dsLType.Tables[0].Rows[lvType]["DispText"]));
                }

                #region Table Formation
                //DataTable dtGrid = new DataTable();
                //dtGrid.Columns.Add("Month");
                ////dtGrid.Columns.Add("MonthVal");

                //for (int lvType = 0; lvType < dsLType.Tables[0].Rows.Count; lvType++)
                //{
                //    dtGrid.Columns.Add(Convert.ToString(dsLType.Tables[0].Rows[lvType]["DispText"]));
                //}
                //for (int mon = 0; mon < lsMonth.Count; mon++)
                //{
                //    dtGrid.Rows.Add(lsMonth[mon]);
                //}
                double leaveTkn = 0;
                grid.DataSource = LeaveCalculation(appNo, rollNo, DegCode, curSem, batch, startDate, endDate, leaveCodes, ref leaveTkn);
                grid.DataBind();
                Session["LeaveConsumed"] = leaveTkn;
                #endregion
            }
        }
        catch { }
    }
    protected void gridLeaveHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = 30;
            e.Row.Cells[1].Width = 100;
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Width = 40;
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.Cells[1].Text == "TOTAL")
            {
                e.Row.Cells[0].Text = string.Empty;
                for (int i = 0; i < e.Row.Cells.Count; i++)
                { e.Row.Cells[i].BackColor = Color.Coral; e.Row.Cells[i].Font.Bold = true; }
            }
        }
    }
    private DataTable LeaveCalculation(string appNo, string RollNo, string DegCode, string curSem, string batch, DateTime fromDate, DateTime toDate, Dictionary<byte, string> leaveCodes, ref double leaveTkn)
    {
        double maxLeave = 0; double.TryParse(lblMaxLeaveAns.Text, out maxLeave);
        double tknLeave = 0; double remLeave = 0;
        DataTable dtFinalAttnd = new DataTable();
        dtFinalAttnd.Columns.Add("Date");
        dtFinalAttnd.Columns.Add("MonVal");
        dtFinalAttnd.Columns.Add("MonName");
        foreach (KeyValuePair<byte, string> leaveCode in leaveCodes)
        {
            dtFinalAttnd.Columns.Add(leaveCode.Key.ToString() + "F");
            dtFinalAttnd.Columns.Add(leaveCode.Key.ToString() + "S");
        }
        List<int> monyear = new List<int>();
        try
        {
            DataSet dsAttndSet = DA.select_method_wo_parameter("select p.degree_code,No_of_Hrs_Per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_I_half_day,min_pres_II_half_day,min_hrs_per_day from PeriodAttndSchedule p,degree r where p.degree_code=r.degree_code and r.college_code='" + collegecode + "' and p.degree_code='" + DegCode + "' and p.semester='" + curSem + "' ", "Text");
            if (dsAttndSet.Tables.Count > 0 && dsAttndSet.Tables[0].Rows.Count > 0)
            {
                byte hrsPerDay = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["No_of_Hrs_Per_day"]);
                byte hrsFHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
                byte hrsSHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);

                byte minHrsDay = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_hrs_per_day"]);
                byte minHrsFHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_pres_I_half_day"]);
                byte minHrsSHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_pres_II_half_day"]);

                DateTime stDate = fromDate;
                DateTime edDate = toDate;

                for (int lcRow = 0; stDate <= edDate; stDate = stDate.AddDays(1), lcRow++)
                {
                    byte curDayVal = (byte)stDate.Day;
                    byte curMonVal = (byte)stDate.Month;
                    int curYearVal = stDate.Year;

                    int attMonthYear = (curYearVal * 12) + curMonVal;

                    if (!monyear.Contains(attMonthYear))
                    {
                        monyear.Add(attMonthYear);
                    }
                }
                string monthyear = string.Empty;

                for (int m = 0; m < monyear.Count; m++)
                {
                    if (monthyear == string.Empty)
                    {
                        monthyear = monyear[m].ToString();
                    }
                    else
                    {
                        monthyear += "," + monyear[m].ToString(); ;
                    }
                }

                string attQ = "select * from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year in (" + monthyear + ") and Att_App_no='" + appNo + "'";
                DataSet dsOvAtt = DA.select_method_wo_parameter(attQ, "Text");

                if (dsOvAtt.Tables.Count > 0 && dsOvAtt.Tables[0].Rows.Count > 0)
                {
                    for (int lcRow = 0; fromDate <= toDate; fromDate = fromDate.AddDays(1), lcRow++)
                    {

                        byte curDayVal = (byte)fromDate.Day;
                        byte curMonVal = (byte)fromDate.Month;
                        int curYearVal = fromDate.Year;

                        int attMonthYear = (curYearVal * 12) + curMonVal;

                        DataRow dr = dtFinalAttnd.NewRow();
                        dr[0] = fromDate;
                        dr[1] = attMonthYear;
                        dr[2] = valmonth(curMonVal.ToString());
                        dtFinalAttnd.Rows.Add(dr);

                        dsOvAtt.Tables[0].DefaultView.RowFilter = " month_year=" + attMonthYear + "";
                        DataView dvAtt = dsOvAtt.Tables[0].DefaultView;
                        if (dvAtt.Count > 0)
                        {
                            byte absCount = 0;
                            //First Half
                            for (int iFHalf = 1; iFHalf <= hrsFHalf; iFHalf++)
                            {
                                string lCode = "";
                                string val = Convert.ToString(dvAtt[0]["D" + curDayVal + "D" + iFHalf + ""]);
                                val = val.Trim() == "" ? "0" : val.Trim();
                                byte attVal = Convert.ToByte(val);

                                if (leaveCodes.ContainsKey(attVal))
                                {
                                    absCount++;
                                    lCode = attVal.ToString() + "F";
                                }

                                if (absCount >= minHrsFHalf)
                                {
                                    dtFinalAttnd.Rows[lcRow][lCode] = "0.5";
                                }
                            }
                            absCount = 0;
                            //Second Half
                            for (int iSHalf = (hrsFHalf + 1); iSHalf <= hrsPerDay; iSHalf++)
                            {
                                string lCode = "";
                                string val = Convert.ToString(dvAtt[0]["D" + curDayVal + "D" + iSHalf + ""]);
                                val = val.Trim() == "" ? "0" : val.Trim();
                                byte attVal = Convert.ToByte(val);

                                if (leaveCodes.ContainsKey(attVal))
                                {
                                    absCount++;
                                    lCode = attVal.ToString() + "S";
                                }

                                if (absCount >= minHrsSHalf)
                                {
                                    dtFinalAttnd.Rows[lcRow][lCode] = "0.5";
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        DataTable dtFinTot = new DataTable();
        try
        {
            dtFinTot.Columns.Add("Month");
            if (dtFinalAttnd.Rows.Count > 0)
            {
                for (int c = 3; c < (dtFinalAttnd.Columns.Count - 1); c += 2)
                {
                    string colName = dtFinalAttnd.Columns[c].ColumnName.ToString();
                    colName = colName.Substring(0, colName.Length - 1);
                    colName = leaveCodes[Convert.ToByte(colName)].ToString();

                    DataColumn newCol = new DataColumn(colName);
                    newCol.DataType = System.Type.GetType("System.Decimal");
                    dtFinTot.Columns.Add(newCol);
                }
                foreach (int MonYear in monyear)
                {
                    dtFinalAttnd.DefaultView.RowFilter = " monval=" + MonYear + "";
                    DataView dvFin = dtFinalAttnd.DefaultView;
                    if (dvFin.Count > 0)
                    {
                        string name = dvFin[0]["monname"].ToString().ToUpper();
                        DataRow dr = dtFinTot.NewRow();
                        dr[0] = name;
                        int a = 1;
                        for (int c = 3; c < (dtFinalAttnd.Columns.Count - 1); c += 2)
                        {
                            dr[a] = 0;
                            a++;
                        }
                        dtFinTot.Rows.Add(dr);
                        Hashtable htTotal = new Hashtable();
                        for (int i = 0; i < dvFin.Count; i++)
                        {
                            for (int c = 3; c < (dtFinalAttnd.Columns.Count - 1); c += 2)
                            {
                                double FHalf = 0; double.TryParse(Convert.ToString(dvFin[i][c]), out FHalf);
                                double SHalf = 0; double.TryParse(Convert.ToString(dvFin[i][c + 1]), out SHalf);

                                string colName = dtFinalAttnd.Columns[c].ColumnName;
                                if (htTotal.Contains(colName))
                                {
                                    htTotal[colName] = Convert.ToDouble(htTotal[colName]) + FHalf + SHalf;
                                }
                                else
                                {
                                    htTotal.Add(colName, (FHalf + SHalf));
                                }
                            }
                        }
                        for (int i = 1; i <= htTotal.Count; i++)
                        {
                            string colName = dtFinTot.Columns[i].ColumnName.ToLower();
                            colName = leaveCodes.FirstOrDefault(x => x.Value.ToLower() == colName).Key.ToString() + "F";
                            string colVal = dtFinTot.Columns[i].ColumnName.ToLower();

                            dtFinTot.Rows[dtFinTot.Rows.Count - 1][colVal] = htTotal.Contains(colName) ? htTotal[colName].ToString().Trim() == string.Empty ? "0" : htTotal[colName] : "0";
                        }
                    }
                }
            }
            #region attendance cal
            string attsetting = "select a.DispText,l.Maxval,l.EntryCode from AttMasterSetting a,leaveMaster l where a.CollegeCode=l.collegeCode and a.EntryCode=l.EntryCode and l.batchyear='" + batch + "'  and l.semester='" + curSem + "' and l.collegeCode='" + collegecode + "'";
            DataTable dtatt = dirAcc.selectDataTable(attsetting);
            if (dtatt.Rows.Count > 0)
            {
                DataTable dtgvdetail = new DataTable();
                dtgvdetail.Columns.Add("DispText");
                dtgvdetail.Columns.Add("EntryCode");
                dtgvdetail.Columns.Add("Maxval");
                dtgvdetail.Columns.Add("Approved");
                dtgvdetail.Columns.Add("bal");
                DataRow dr2 = null;

                foreach (DataRow dr in dtatt.Rows)
                {
                    dr2 = dtgvdetail.NewRow();
                    string dis = Convert.ToString(dr["DispText"]);
                    string ecode = Convert.ToString(dr["EntryCode"]);
                    string maxval = Convert.ToString(dr["Maxval"]);
                    double app = 0;
                    double bal = 0;
                    double maxV = 0;
                    double.TryParse(maxval, out maxV);
                    if (dtFinTot.Rows.Count > 0)
                    {
                        for (int i = 1; i < dtFinTot.Columns.Count; i++)
                        {
                            var result = dtFinTot.AsEnumerable()
                           .Sum(x => Convert.ToDouble(x[dis]));
                            string res = result.ToString();
                            double.TryParse(res, out app);
                        }
                        bal = maxV - app;
                        dr2 = dtgvdetail.NewRow();
                        dr2["DispText"] = dis;
                        dr2["EntryCode"] = ecode;
                        dr2["Maxval"] = maxval;
                        dr2["Approved"] = Convert.ToString(app);
                        dr2["bal"] = Convert.ToString(bal);
                        dtgvdetail.Rows.Add(dr2);
                    }
                }
                if (dtgvdetail.Rows.Count > 0)
                {
                    GridView1.DataSource = dtgvdetail;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                }
            }
            #endregion

            if (dtFinTot.Rows.Count > 0)
            {
                DataRow drr = dtFinTot.NewRow();
                drr[0] = "TOTAL";

                for (int i = 1; i < dtFinTot.Columns.Count; i++)
                {
                    string colName = dtFinTot.Columns[i].ColumnName;
                    //object sumObject;
                    //sumObject = dtFinTot.Compute("Sum(" + colName + ")", string.Empty);

                    var result = dtFinTot.AsEnumerable()
                    .Sum(x => Convert.ToDouble(x[colName]));

                    drr[i] = result.ToString();
                }
                dtFinTot.Rows.Add(drr);
            }
        }
        catch { }

        for (int i = 1; i < dtFinTot.Columns.Count; i++)
        {
            double lv = 0;
            string colName = dtFinTot.Columns[i].ColumnName.ToString();
            object sumObject = dtFinTot.Compute("Sum(" + colName + ")", "");
            double.TryParse(sumObject.ToString(), out lv);

            double total = 0;
            double.TryParse(dtFinTot.Rows[dtFinTot.Rows.Count - 1][colName].ToString(), out total);
            lv -= total;

            tknLeave += lv;
        }
        leaveTkn = tknLeave;
        lblTakenLeaveAns.Text = tknLeave.ToString();
        remLeave = (maxLeave - tknLeave) < 0 ? 0 : (maxLeave - tknLeave);
        lblRemLeaveAns.Text = remLeave.ToString();
        return dtFinTot;
    }
    private int monthval(string month)
    {
        switch (month.ToLower())
        {
            case "january":
                return 1;
            case "february":
                return 2;
            case "march":
                return 3;
            case "april":
                return 4;
            case "may":
                return 5;
            case "june":
                return 6;
            case "july":
                return 7;
            case "august":
                return 8;
            case "september":
                return 9;
            case "october":
                return 10;
            case "november":
                return 11;
            case "december":
                return 1;
        }
        return 0;
    }
    private string valmonth(string month)
    {
        switch (month.Trim())
        {
            case "1":
                return "january";
            case "2":
                return "february";
            case "3":
                return "march";
            case "4":
                return "april";
            case "5":
                return "may";
            case "6":
                return "june";
            case "7":
                return "july";
            case "8":
                return "august";
            case "9":
                return "september";
            case "10":
                return "october";
            case "11":
                return "november";
            case "12":
                return "december";
        }
        return "";
    }
    protected void OnRowDataBound_gv1(object sender, GridViewRowEventArgs e)
    {
    }
    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);

        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }
    protected void ddlLeaveReason_Indexchange(object sender, EventArgs e)
    {
        txtReasonLeave.Text = string.Empty;
        if (ddlLeaveReason.SelectedItem.Text == "Others")
        {
            txtReasonLeave.Visible = true;
        }
        else
        {
            txtReasonLeave.Visible = false;
        }
    }
    public void bindStaff()
    {
        try
        {
            DataSet dsLeaveDet = DA.select_method_wo_parameter("select distinct ReqAppStaffAppNo from RQ_RequestHierarchy where ReqStaffAppNo='" + Session["degree_code"].ToString() + "'", "Text");

            StringBuilder appl_id = new StringBuilder();
            if (dsLeaveDet.Tables.Count > 0)
            {
                for (int i = 0; i < dsLeaveDet.Tables[0].Rows.Count; i++)
                {
                    appl_id.Append(Convert.ToString(dsLeaveDet.Tables[0].Rows[i][0]) + ",");
                }
                if (appl_id.Length > 0)
                {
                    appl_id.Remove(appl_id.Length - 1, 1);
                }
            }
            if (appl_id.Length > 0)
            {
                string sqlcmd = "select distinct s.staff_code,a.appl_id ,s.staff_name,h.dept_name,d.desig_name from staff_appl_master a ,staffmaster s,hrdept_master h,desig_master d,stafftrans st where a.appl_no =s.appl_no and s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode  and s.college_code='" + collegecode + "' and a.appl_id in (" + appl_id.ToString() + ") and resign = 0 and settled = 0 and latestrec=1 order by h.dept_name,s.staff_code";
                sqlcmd = sqlcmd + " select ReqStaffAppNo,ReqAppStaffAppNo,RequestType,FromDays,ToDays from RQ_RequestHierarchy where ReqStaffAppNo='" + Session["degree_code"].ToString() + "' and ReqAppStaffAppNo in (" + appl_id.ToString() + ")";
                DataSet dsload = DA.select_method_wo_parameter(sqlcmd, "Text");

                DataView dv = new DataView();
                if (dsload.Tables.Count > 1 && dsload.Tables[0].Rows.Count > 0 && dsload.Tables[1].Rows.Count > 0)
                {
                    DataTable gridTable = new DataTable();
                    gridTable.Columns.Add("Department");
                    gridTable.Columns.Add("Designation");
                    gridTable.Columns.Add("StaffCode");
                    gridTable.Columns.Add("StaffName");
                    gridTable.Columns.Add("From (Days)");
                    gridTable.Columns.Add("To (Days)");

                    FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle2.ForeColor = Color.Black;
                    darkstyle2.HorizontalAlign = HorizontalAlign.Center;


                    for (int loop = 0; loop < dsload.Tables[0].Rows.Count; loop++)
                    {
                        string appno = dsload.Tables[0].Rows[loop]["appl_id"].ToString();
                        dsload.Tables[1].DefaultView.RowFilter = " ReqAppStaffAppNo ='" + appno + "'";
                        DataView dvSt = dsload.Tables[1].DefaultView;
                        if (dvSt.Count > 0)
                        {
                            for (int dRo = 0; dRo < dvSt.Count; dRo++)
                            {
                                gridTable.Rows.Add(dsload.Tables[0].Rows[loop]["dept_name"].ToString(), dsload.Tables[0].Rows[loop]["desig_name"].ToString(), dsload.Tables[0].Rows[loop]["staff_code"].ToString(), dsload.Tables[0].Rows[loop]["staff_name"].ToString(), dvSt[dRo]["FromDays"].ToString(), dvSt[dRo]["ToDays"].ToString());
                            }
                        }
                    }
                    gridStaffDetails.DataSource = gridTable;
                    gridStaffDetails.DataBind();
                }
            }
        }
        catch { }
    }
    protected void btnSaveRequest_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime fromDate = TextToDate(txt_fromdate);
            DateTime toDate = TextToDate(txt_todate);

            //if (!DaysCHeck(fromDate, toDate)) return;

            int choosedDays = GV1.Rows.Count; //((int)(toDate - fromDate).TotalDays) + 1;

            string reqDate = DateTime.Now.Date.ToString("MM/dd/yyyy");
            string leaveType = ddl_leave_type.SelectedValue.Trim();
            string leaveReason = ddlLeaveReason.SelectedValue.Trim() == "Others" ? subjectcode("LEVAP", txtReasonLeave.Text.Trim(), collegecode) : (ddlLeaveReason.SelectedValue.Trim() == "Select" ? string.Empty : ddlLeaveReason.SelectedValue.Trim());

            string appNo = (string.IsNullOrEmpty(txtIntAppNo.Text.Trim()) || txtIntAppNo.Text.Trim() == "0") ? string.Empty : txtIntAppNo.Text.Trim();
            string DegCode = txtIntDegCode.Text.Trim();
            string curSem = txt_Sem.Text.Trim();
            string batch = txt_Batc.Text.Trim();

            string applId = string.Empty;
            byte reqMode = 0;
            if (staffcodesession == usercode)
            {
                applId = DegCode;
                reqMode = 0;
            }
            else
            {
                applId = DA.GetFunction("select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "'").Trim();
                reqMode = 1;
            }

            if (div_GV1.Visible && leaveType != string.Empty && leaveReason != string.Empty && appNo != string.Empty && DegCode != string.Empty && applId != string.Empty)
            {
                if (inputGridCheck())
                {
                    int saved = 0;
                    byte MornEve = 0;
                    byte isHalf = 0;
                    string isHalfDate = "01/01/1900";
                    foreach (GridViewRow gRow in GV1.Rows)
                    {
                        CheckBox chk_mrng = (CheckBox)gRow.FindControl("chk_mrng");
                        CheckBox chk_evng = (CheckBox)gRow.FindControl("chk_evng");
                        TextBox txtdate = (TextBox)gRow.FindControl("txtdate");

                        if (!chk_mrng.Checked)
                        {
                            isHalf = 1;
                            MornEve = 0;
                            isHalfDate = TextToDate(txtdate).ToString("MM/dd/yyy");
                        }
                        if (!chk_evng.Checked)
                        {
                            isHalf = 1;
                            MornEve = 1;
                            isHalfDate = TextToDate(txtdate).ToString("MM/dd/yyy");
                        }
                    }
                    string insQUery = "insert into SR_Student_Leave_Request (App_no ,Fromdate ,Todate  , TotalLeave , CollegeCode , RequestStatus , IsHalfDay , HalfTime , HalfDayDate , LeaveType, LeaveReason,RequestBy,RequestMode,approvalStage) values ('" + appNo + "' ,'" + fromDate.ToString("MM/dd/yyy") + "' ,'" + toDate.ToString("MM/dd/yyy") + "' , '" + choosedDays + "' , '" + collegecode + "' , '0'  , '" + isHalf + "' , '" + MornEve + "' , '" + isHalfDate + "' , '" + leaveType + "', '" + leaveReason + "','" + applId + "','" + reqMode + "','0')  ";
                    saved = DA.update_method_wo_parameter(insQUery, "Text");
                    if (saved > 0)
                    {
                        ButtonReq_Click(sender, e);
                        //ButtonReport_Click(sender, e);
                        //ButtonApprove_Click(sender, e);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Check Grid Details')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Provide All Details')", true);
            }

        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true); }
    }
    private bool inputGridCheck()
    {
        bool inputOk = true;


        int HalfCnt = 0;
        foreach (GridViewRow gRow in GV1.Rows)
        {
            int moreveHalfCnt = 0;
            CheckBox chk_mrng = (CheckBox)gRow.FindControl("chk_mrng");
            CheckBox chk_evng = (CheckBox)gRow.FindControl("chk_evng");
            if (chk_mrng.Checked)
            {
                moreveHalfCnt++;
            }
            if (chk_evng.Checked)
            {
                moreveHalfCnt++;
            }
            if (moreveHalfCnt == 0)
            {
                inputOk = false;
            }
            else if (moreveHalfCnt == 1)
            {
                HalfCnt++;
            }
        }
        if (HalfCnt > 1)
        {
            inputOk = false;
        }
        return inputOk;
    }
    //Report 
    protected void ddlClgRep_OnSelectedIndexchange(object sender, EventArgs e)
    {
        checkDateRep(sender, e);
    }
    protected void checkDateRep(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = TextToDate(txt_fromdateRep);
            DateTime todate = TextToDate(txt_todateRep);

            if (fromdate > todate)
            {
                txt_fromdateRep.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todateRep.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //Response.Write("<script>alert('From Date Should Not Exceed To Date')</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
            }
            BindGridReport();
        }
        catch { }
    }
    protected void ButtonReport_Click(object sender, EventArgs e)
    {
        divRequestTab.Visible = false;
        divReportTab.Visible = true;
        divApproveTab.Visible = false;

        divRequestLink.Style.Add("background-Color", "#226399");
        divReportLink.Style.Add("background-Color", "rgba(255, 255, 255, .3)");
        divApproveRejectLink.Style.Add("background-Color", "#226399");

        BindGridReport();
    }
    private void BindGridReport()
    {
        gridLeaveReport.DataSource = null;
        gridLeaveReport.DataBind();
        gridLeaveReport.Visible = false;
        btnDeleteRequest.Visible = false;
        try
        {
            string collegeCode = ddlClgRep.Items.Count > 0 ? ddlClgRep.SelectedValue : "13";
            DateTime fromdate = TextToDate(txt_fromdateRep);
            DateTime todate = TextToDate(txt_todateRep);



            string reqby = string.Empty;// "  and RequestMode='0' ";
            string applId = string.Empty;
            if (staffcodesession != usercode)
            {
                applId = DA.GetFunction("select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "'").Trim();
                reqby = "  and  requestby='" + applId + "' ";//RequestMode='1' and
            }
            string appNo = (string.IsNullOrEmpty(txtIntAppNo.Text.Trim()) || txtIntAppNo.Text.Trim() == "0") ? string.Empty : txtIntAppNo.Text.Trim();

            string degHi = "select ReqStaffAppNo from RQ_RequestHierarchy where RequestType='10' and ReqAppStaffAppNo='" + applId + "'";
            DataTable dtdeginfo = dirAcc.selectDataTable(degHi);

            string degCode2 = string.Empty;
            if (dtdeginfo.Rows.Count > 0)
            {
                foreach (DataRow dt in dtdeginfo.Rows)
                {
                    string deg = Convert.ToString(dt["ReqStaffAppNo"]);
                    if (string.IsNullOrEmpty(degCode2))
                        degCode2 = deg;
                    else
                        degCode2 = degCode2 + "," + deg;

                }
            }
            string req = string.Empty;
            string reqst = string.Empty;
            if (!string.IsNullOrEmpty(degCode2))
                req = "  and sr.RequestBy in(" + degCode2 + ")";
            string reqStats = string.Empty;
            if (ddlRepMode.SelectedIndex == 1)
            {
                reqStats = " and RequestStatus='0' ";
            }
            else if (ddlRepMode.SelectedIndex == 2)
            {
                reqStats = " and RequestStatus='1' ";
            }
            else if (ddlRepMode.SelectedIndex == 3)
            {
                reqStats = " and RequestStatus='2' ";
                reqst = " and isApproved='1'";

            }


            DataTable dtGridTable = new DataTable();
            dtGridTable.Columns.Add("LeaveRequestPk");
            dtGridTable.Columns.Add("FromDate");
            dtGridTable.Columns.Add("ToDate");
            dtGridTable.Columns.Add("TotalLeave");
            dtGridTable.Columns.Add("IsHalfDay");
            dtGridTable.Columns.Add("HalfTime");
            dtGridTable.Columns.Add("HalfDayDate");
            dtGridTable.Columns.Add("LeaveDisp");
            dtGridTable.Columns.Add("LeaveType");
            dtGridTable.Columns.Add("LeaveReason");
            dtGridTable.Columns.Add("Reason");
            dtGridTable.Columns.Add("RequestStatus");
            dtGridTable.Columns.Add("RequestStatusName");
            dtGridTable.Columns.Add("AppNo");
            dtGridTable.Columns.Add("StudName");
            dtGridTable.Columns.Add("Branch");
            dtGridTable.Columns.Add("AdmNo");
            dtGridTable.Columns.Add("RegNo");
            dtGridTable.Columns.Add("RollNo");
            dtGridTable.Columns.Add("rejectReson");

            string selQ = "select LeaveRequestPk, CONVERT(varchar(10),FromDate,103) as FromDate, CONVERT(varchar(10),ToDate,103) as ToDate, TotalLeave, RequestStatus, Case when RequestStatus=0 then 'Requested' when RequestStatus=1 then 'Approved'  else 'Rejected' end as RequestStatusName, AprovalBy, AprovalDate,case when IsHalfDay=1 then 'Yes' else 'No' end  IsHalfDay, case when HalfTime=0 then 'Morning' else 'Evening' end  HalfTime, CONVERT(varchar(10),HalfDayDate,103) as HalfDayDate,(select DispText from AttMastersetting am where LeaveCode=LeaveType and am.CollegeCode='" + collegeCode + "') as LeaveDisp, LeaveType, LeaveReason, (select textval from textvaltable where TextCode=Leavereason) as Reason,RequestBy,RequestMode  ,sr.App_no,r.Stud_Name,(c.Course_Name+'-'+dt.Dept_Name) as Branch, r.Reg_No,r.Roll_No,a.app_formno,rejectReson  from SR_Student_Leave_Request sr,Registration r,applyn a,Degree d, course c, Department dt where sr.App_no=a.app_no and r.App_No=sr.App_no and r.App_No=a.app_no and c.Course_Id=d.Course_Id and d.Degree_Code=r.degree_code and dt.Dept_Code=d.Dept_Code  and sr.collegeCode='" + collegeCode + "' and '" + fromdate.ToString("MM/dd/yyy") + "'<=FromDate   " + reqStats + req;//+ reqby
            DataSet dsLeaveDet = DA.select_method_wo_parameter(selQ, "Text");
            if (dsLeaveDet.Tables.Count > 0 && dsLeaveDet.Tables[0].Rows.Count > 0)
            {
                for (int iRow = 0; iRow < dsLeaveDet.Tables[0].Rows.Count; iRow++)
                {
                    DataRow drReq = dtGridTable.NewRow();
                    string reqstatus = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestStatus"]);
                    string reqk = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveRequestPk"]);

                    if (reqstatus == "2")
                    {
                        string rejectdetail = "select LeaveRequestPk,CONVERT(varchar(10),LeaveDate,103) as FromDate,CONVERT(varchar(10),LeaveDate,103) as ToDate,isApproved,Case when isApproved=0 then 'Approved' else 'Rejected' end as RequestStatusName,case when SessionType=1 then 'Yes' when SessionType=2 then 'Yes' else 'No' end IsHalfDay, case when SessionType=2 then 'Evening' when  SessionType=1 then 'Morning' end HalfTime,case when SessionType=2 then CONVERT(varchar(10),LeaveDate,103) when SessionType=1 then   CONVERT(varchar(10),LeaveDate,103) else '' end as  HalfDayDate,case when isApproved=0 then '1' else '2' end as RequestStatus   from student_leaveRequest_details where LeaveRequestPK='" + reqk.ToString() + "' " + reqst + "  ";
                        DataSet rejdetails = DA.select_method_wo_parameter(rejectdetail, "text");
                        for (int j = 0; j < rejdetails.Tables[0].Rows.Count; j++)
                        {
                            drReq = dtGridTable.NewRow();
                            string reqpk = Convert.ToString(rejdetails.Tables[0].Rows[j]["LeaveRequestPk"]);
                            string fromdt = Convert.ToString(rejdetails.Tables[0].Rows[j]["FromDate"]);
                            string todat = Convert.ToString(rejdetails.Tables[0].Rows[j]["ToDate"]);
                            string halfday = Convert.ToString(rejdetails.Tables[0].Rows[j]["IsHalfDay"]).Trim();
                            string halftime = Convert.ToString(rejdetails.Tables[0].Rows[j]["HalfTime"]);
                            string halfdate = Convert.ToString(rejdetails.Tables[0].Rows[j]["HalfDayDate"]);
                            string reqstname = Convert.ToString(rejdetails.Tables[0].Rows[j]["RequestStatusName"]);
                            string totleave = "1";
                            drReq["LeaveRequestPk"] = reqpk;
                            drReq["FromDate"] = fromdt;
                            drReq["ToDate"] = todat;
                            drReq["TotalLeave"] = totleave;
                            drReq["IsHalfDay"] = halfday;
                            drReq["HalfTime"] = halftime;
                            drReq["HalfDayDate"] = halfdate;
                            // drReq["TotalLeave"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["TotalLeave"]);
                            drReq["LeaveDisp"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveDisp"]);
                            drReq["LeaveType"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveType"]);
                            drReq["LeaveReason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveReason"]);
                            drReq["Reason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Reason"]);
                            drReq["RequestStatusName"] = reqstname;
                            drReq["RequestStatus"] = Convert.ToString(rejdetails.Tables[0].Rows[j]["RequestStatus"]);
                            drReq["AppNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["App_no"]);
                            drReq["StudName"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Stud_name"]);
                            drReq["Branch"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Branch"]);
                            drReq["RegNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["reg_no"]);
                            drReq["AdmNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["app_formno"]);
                            drReq["RollNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["roll_no"]);
                            drReq["rejectReson"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["rejectReson"]);
                            dtGridTable.Rows.Add(drReq);
                        }

                    }
                    else
                    {
                        // drReq = dtGridTable.NewRow();
                        drReq["LeaveRequestPk"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveRequestPk"]);
                        drReq["FromDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["FromDate"]);
                        drReq["ToDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["ToDate"]);
                        drReq["TotalLeave"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["TotalLeave"]);
                        string IsHalfDay = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["IsHalfDay"]).Trim();
                        drReq["IsHalfDay"] = IsHalfDay;
                        if (IsHalfDay == "Yes")
                        {
                            drReq["HalfTime"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["HalfTime"]);
                            drReq["HalfDayDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["HalfDayDate"]);
                        }
                        drReq["LeaveDisp"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveDisp"]);
                        drReq["LeaveType"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveType"]);
                        drReq["LeaveReason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveReason"]);
                        drReq["Reason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Reason"]);
                        drReq["RequestStatus"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestStatus"]);
                        drReq["RequestStatusName"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestStatusName"]);
                        drReq["AppNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["App_no"]);
                        drReq["StudName"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Stud_name"]);
                        drReq["Branch"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Branch"]);
                        drReq["RegNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["reg_no"]);
                        drReq["AdmNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["app_formno"]);
                        drReq["RollNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["roll_no"]);
                        drReq["rejectReson"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["rejectReson"]);
                        dtGridTable.Rows.Add(drReq);
                    }

                }
                gridLeaveReport.DataSource = dtGridTable;
                gridLeaveReport.DataBind();
                gridLeaveReport.Visible = true;
                btnDeleteRequest.Visible = true;

                if (gridLeaveReport.Rows.Count > 0)
                {
                    SetUniqueId(gridLeaveReport);
                }
            }
        }
        catch { }
    }
    protected void gridLeaveReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[4].Text = lblDegree.Text + "-" + lblBranch.Text;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblLeaveReqStat = (Label)e.Row.FindControl("lblLeaveReqStat");
            string status = lblLeaveReqStat.Text.Trim();
            if (status == "0")
            {
                e.Row.BackColor = Color.White;
            }
            else if (status == "1")
            {
                e.Row.BackColor = Color.FromArgb(32, 178, 153);
            }
            else if (status == "2")
            {
                e.Row.BackColor = Color.FromArgb(255, 77, 77);
            }
        }
    }
    protected void cbSelHead_CheckedChange(object sender, EventArgs e)
    {
        bool ischecked = false;
        CheckBox cbSelHead = (CheckBox)sender;
        if (cbSelHead.Checked)
        {
            ischecked = true;
        }
        foreach (GridViewRow gRow in gridLeaveReport.Rows)
        {
            CheckBox cbSel = (CheckBox)gRow.FindControl("cbSel");
            cbSel.Checked = ischecked;
        }
    }
    protected void btnDeleteRequest_Click(object sender, EventArgs e)
    {
        string RePks = string.Empty;
        if (checkDelInp(ref RePks))
        {
            int deleted = 0;
            try
            {
                string delQ = "delete from SR_Student_Leave_Request where LeaveRequestPk in (" + RePks + ") and  RequestStatus='0'";
                deleted = DA.update_method_wo_parameter(delQ, "Text");
            }
            catch { }
            if (deleted > 0)
            {
                ButtonReport_Click(sender, e);
                //ButtonApprove_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + deleted + " Records Deleted')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Deleted')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Records')", true);
        }
    }
    private bool checkDelInp(ref string RePks)
    {
        bool delok = false;
        try
        {
            StringBuilder sbRePks = new StringBuilder();
            foreach (GridViewRow gRow in gridLeaveReport.Rows)
            {
                CheckBox cbSel = (CheckBox)gRow.FindControl("cbSel");
                if (cbSel.Checked)
                {
                    delok = true;
                    Label lblReqPk = (Label)gRow.FindControl("lblReqPk");
                    sbRePks.Append(lblReqPk.Text.Trim() + ",");
                }
            }
            if (sbRePks.Length > 0)
            {
                sbRePks.Remove(sbRePks.Length - 1, 1);
                RePks = sbRePks.ToString();
            }
        }
        catch { }
        return delok;
    }
    //Approve
    protected void ddlClgApp_OnSelectedIndexchange(object sender, EventArgs e)
    {
        bindReqStage();
        checkDateApp(sender, e);
    }
    protected void checkDateApp(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = TextToDate(txt_fromdateApp);
            DateTime todate = TextToDate(txt_todateApp);

            if (fromdate > todate)
            {
                txt_fromdateApp.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todateApp.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //Response.Write("<script>alert('From Date Should Not Exceed To Date')</script>");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('From Date Should Not Exceed To Date')", true);
            }
            BindGridApprove();
        }
        catch { }
    }
    protected void ButtonApprove_Click(object sender, EventArgs e)
    {

        divRequestTab.Visible = false;
        divReportTab.Visible = false;
        divApproveTab.Visible = true;

        divRequestLink.Style.Add("background-Color", "#226399");
        divReportLink.Style.Add("background-Color", "#226399");
        divApproveRejectLink.Style.Add("background-Color", "rgba(255, 255, 255, .3)");


        BindGridApprove();
    }
    private void BindGridApprove()
    {
        gridLeaveApprove.DataSource = null;
        gridLeaveApprove.DataBind();
        gridLeaveApprove.Visible = false;
        btnApproveReq.Visible = false;
        btnRejectReq.Visible = false;
        try
        {
            string collegeCode = ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13";
            DateTime fromdate = TextToDate(txt_fromdateApp);
            DateTime todate = TextToDate(txt_todateApp);
            string appNo = (string.IsNullOrEmpty(txtIntAppNo.Text.Trim()) || txtIntAppNo.Text.Trim() == "0") ? string.Empty : txtIntAppNo.Text.Trim();

            string applId = DA.GetFunction("select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and staff_code='" + staffcodesession + "'").Trim();

            int appStage = 0;
            if (ddlReqStage.Items.Count > 0)
            {
                int.TryParse(ddlReqStage.SelectedValue, out appStage);
                //appStage--;
            }

            string reqStage = " and isnull(approvalStage,1)='" + appStage + "'";

            if (applId != string.Empty && appStage >= 0)
            {
                DataSet dsPermis = DA.select_method_wo_parameter("select distinct ReqStaffAppNo from RQ_RequestHierarchy where ReqAppStaffAppNo='" + applId + "' and CollegeCode='" + collegeCode + "' and ReqApproveStage='" + (appStage) + "' and RequestType='10' ; select ReqStaffAppNo,ReqApproveStage,FromDays,ToDays,((ToDays-FromDays)+1) as totDays  from RQ_RequestHierarchy where ReqAppStaffAppNo='" + applId + "'  and CollegeCode='" + collegeCode + "' and ReqApproveStage='" + (appStage) + "' and RequestType='10'  ", "Text");//appStage + 1
                StringBuilder reqStaffSutd = new StringBuilder();
                if (dsPermis.Tables.Count > 1 && dsPermis.Tables[0].Rows.Count > 0 && dsPermis.Tables[1].Rows.Count > 0)
                {
                    for (int pRow = 0; pRow < dsPermis.Tables[0].Rows.Count; pRow++)
                    {
                        reqStaffSutd.Append(Convert.ToString(dsPermis.Tables[0].Rows[pRow][0]).Trim() + "','");
                    }
                    if (reqStaffSutd.Length > 2)
                    {
                        reqStaffSutd.Remove(reqStaffSutd.Length - 3, 3);
                    }

                    DataTable dtstaffDeg = dirAcc.selectDataTable("");

                    string selQ = "select LeaveRequestPk, CONVERT(varchar(10),FromDate,103) as FromDate, CONVERT(varchar(10),ToDate,103) as ToDate, TotalLeave, RequestStatus, Case when RequestStatus=0 then 'Requested' when RequestStatus=1 then 'Approved'  else 'Rejected' end as RequestStatusName, AprovalBy, AprovalDate,case when IsHalfDay=1 then 'Yes' else 'No' end  IsHalfDay, case when HalfTime=0 then 'Morning' else 'Evening' end  HalfTime, CONVERT(varchar(10),HalfDayDate,103) as HalfDayDate,(select DispText from AttMastersetting  am where LeaveCode=LeaveType and am.CollegeCode='" + collegeCode + "' ) as LeaveDisp, LeaveType, LeaveReason, (select textval from textvaltable where TextCode=Leavereason) as Reason,RequestBy,RequestMode  ,sr.App_no,r.Stud_Name,(c.Course_Name+'-'+dt.Dept_Name) as Branch, r.Reg_No,r.Roll_No,a.app_formno  from SR_Student_Leave_Request sr,Registration r,applyn a,Degree d, course c, Department dt,RQ_RequestHierarchy rq where sr.App_no=a.app_no and r.App_No=sr.App_no and r.App_No=a.app_no and c.Course_Id=d.Course_Id and d.Degree_Code=r.degree_code and dt.Dept_Code=d.Dept_Code  and sr.collegeCode='" + collegeCode + "'  and '" + fromdate.ToString("MM/dd/yyy") + "'<=FromDate   and RequestMode='" + ddlStudStaff.SelectedIndex + "'" + reqStage + " and rq.ReqAppStaffAppNo='" + applId + "' and rq.ReqStaffAppNo=r.degree_code and rq.BatchYear=r.Batch_Year and r.degree_code=rq.DegreeCode and rq.Semester=r.Current_Semester and ltrim(rtrim(isnull(rq.Section,'')))=ltrim(rtrim(isnull(r.Sections,'')))";//  and rq.BatchYear  in(2017) and r.degree_code in(45) and rq.Semester in(3) and rq.Section in('B')

                    DataSet dsLeaveDet = DA.select_method_wo_parameter(selQ, "Text");
                    if (dsLeaveDet.Tables.Count > 0 && dsLeaveDet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtGridTable = new DataTable();
                        dtGridTable.Columns.Add("LeaveRequestPk");
                        dtGridTable.Columns.Add("FromDate");
                        dtGridTable.Columns.Add("ToDate");
                        dtGridTable.Columns.Add("TotalLeave");
                        dtGridTable.Columns.Add("IsHalfDay");
                        dtGridTable.Columns.Add("HalfTime");
                        dtGridTable.Columns.Add("HalfDayDate");
                        dtGridTable.Columns.Add("LeaveDisp");
                        dtGridTable.Columns.Add("LeaveType");
                        dtGridTable.Columns.Add("LeaveReason");
                        dtGridTable.Columns.Add("Reason");
                        dtGridTable.Columns.Add("RequestStatus");
                        dtGridTable.Columns.Add("RequestStatusName");
                        dtGridTable.Columns.Add("AppNo");
                        dtGridTable.Columns.Add("StudName");
                        dtGridTable.Columns.Add("Branch");
                        dtGridTable.Columns.Add("AdmNo");
                        dtGridTable.Columns.Add("RegNo");
                        dtGridTable.Columns.Add("RollNo");

                        for (int iRow = 0; iRow < dsLeaveDet.Tables[0].Rows.Count; iRow++)
                        {
                            string reqby = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestBy"]).Trim();
                            string RequestMode = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestMode"]).Trim();
                            string totalLeave = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["TotalLeave"]).Trim();
                            dsPermis.Tables[1].DefaultView.RowFilter = "" + totalLeave + ">=FromDays and " + totalLeave + "<=ToDays and ReqStaffAppNo='" + reqby + "'";
                            DataView dvPermit = dsPermis.Tables[1].DefaultView;
                            if (RequestMode == "0" || RequestMode == "1")//dvPermit.Count > 0 ||  || RequestMode == "1"
                            {
                                DataRow drReq = dtGridTable.NewRow();
                                drReq["LeaveRequestPk"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveRequestPk"]);
                                drReq["FromDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["FromDate"]);
                                drReq["ToDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["ToDate"]);
                                drReq["TotalLeave"] = totalLeave;
                                string IsHalfDay = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["IsHalfDay"]).Trim();
                                drReq["IsHalfDay"] = IsHalfDay;
                                if (IsHalfDay == "Yes")
                                {
                                    drReq["HalfTime"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["HalfTime"]);
                                    drReq["HalfDayDate"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["HalfDayDate"]);
                                }
                                drReq["LeaveDisp"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveDisp"]);
                                drReq["LeaveType"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveType"]);
                                drReq["LeaveReason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["LeaveReason"]);
                                drReq["Reason"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Reason"]);
                                drReq["RequestStatus"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestStatus"]);
                                drReq["RequestStatusName"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["RequestStatusName"]);
                                drReq["AppNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["App_no"]);
                                drReq["StudName"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Stud_name"]);
                                drReq["Branch"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["Branch"]);
                                drReq["RegNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["reg_no"]);
                                drReq["AdmNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["app_formno"]);
                                drReq["RollNo"] = Convert.ToString(dsLeaveDet.Tables[0].Rows[iRow]["roll_no"]);
                                dtGridTable.Rows.Add(drReq);
                            }
                        }
                        gridLeaveApprove.DataSource = dtGridTable;
                        gridLeaveApprove.DataBind();
                        gridLeaveApprove.Visible = true;
                        btnApproveReq.Visible = true;
                        btnRejectReq.Visible = true;

                        if (gridLeaveApprove.Rows.Count > 0)
                        {
                            SetUniqueId(gridLeaveApprove);
                        }
                    }
                }

            }
        }
        catch { }
    }

    protected void gridLeaveApprove_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[4].Text = lblDegree.Text + "-" + lblBranch.Text;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblLeaveReqStat = (Label)e.Row.FindControl("lblLeaveReqStat");
            string status = lblLeaveReqStat.Text.Trim();
            if (status == "0")
            {
                e.Row.BackColor = Color.White;
            }
            else if (status == "1")
            {
                e.Row.BackColor = Color.FromArgb(32, 178, 153);
            }
            else if (status == "2")
            {
                e.Row.BackColor = Color.FromArgb(255, 77, 77);
            }

        }
    }

    protected void cbSelHeadAPp_CheckedChange(object sender, EventArgs e)
    {
        bool ischecked = false;
        CheckBox cbSelHead = (CheckBox)sender;
        if (cbSelHead.Checked)
        {
            ischecked = true;
        }
        foreach (GridViewRow gRow in gridLeaveApprove.Rows)
        {
            CheckBox cbSel = (CheckBox)gRow.FindControl("cbSel");
            cbSel.Checked = ischecked;
        }
    }


    protected void btnApproveReq_Click(object sender, EventArgs e)
    {
        string RePks = string.Empty;
        string appNos = string.Empty;
        string totalDays = string.Empty;
        string LeaveCodes = string.Empty;
        string fromDates = string.Empty;
        string toDates = string.Empty;
        string halfDates = string.Empty;
        string isHalfs = string.Empty;
        string halfSessions = string.Empty;

        if (checkAppInp(ref RePks, ref appNos, ref totalDays, ref  LeaveCodes, ref  fromDates, ref  toDates, ref  halfDates, ref  isHalfs, ref  halfSessions))
        {
            string finyearID = financeYear(ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13");
            int approved = 0;
            int req = 0;
            try
            {


                string[] ReqPks = RePks.Split(',');
                string[] app_No = appNos.Split(',');
                string[] totDay = totalDays.Split(',');

                string[] lCodes = LeaveCodes.Split(',');
                string[] fromDts = fromDates.Split(',');
                string[] toDts = toDates.Split(',');
                string[] halfDts = halfDates.Split(',');
                string[] isHafs = isHalfs.Split(',');
                string[] hafSessions = halfSessions.Split(',');

                int appIndx = 0;

                foreach (string ReqPk in ReqPks)
                {
                    if (true)
                    {
                        string getref = d22.GetFunction("select degree_code from Registration where App_No in (select App_no from SR_Student_Leave_Request  where LeaveRequestPk='" + ReqPk + "')");
                        string maxHI = d22.GetFunction("select  max(ReqApproveStage) from RQ_RequestHierarchy where ReqStaffAppNo='" + getref + "' and RequestType=10");

                        req = DA.update_method_wo_parameter("update SR_Student_Leave_Request set approvalStage=(isnull(approvalStage,1)+1) where LeaveRequestPk=" + ReqPk + "", "Text");
                        DataSet dsinfo = d22.select_method_wo_parameter("select r.App_No,r.Roll_No,r.Reg_No,r.degree_code,r.Batch_Year from SR_Student_Leave_Request l,Registration r  where r.App_No=l.App_no and LeaveRequestPk='" + ReqPk + "'", "text");
                        string appNo = string.Empty;
                        string rollNo = string.Empty;

                        if (dsinfo.Tables.Count > 0 && dsinfo.Tables[0].Rows.Count > 0)
                        {
                            appNo = Convert.ToString(dsinfo.Tables[0].Rows[0]["App_No"]);
                            rollNo = Convert.ToString(dsinfo.Tables[0].Rows[0]["Roll_No"]);
                        }
                        int MAXHI = 0;
                        int.TryParse(maxHI, out MAXHI);
                        int appStage = 0;
                        if (ddlReqStage.Items.Count > 0)
                        {
                            int.TryParse(ddlReqStage.SelectedValue, out appStage);
                        }
                        //if (DA.GetFunction("select ReqApproveStateCOunt from RQ_RequestHierarchy rh,SR_Student_Leave_Request sr where ReqApproveStateCOunt=isnull(approvalStage,1) and ReqStaffAppNo=RequestBy and LeaveRequestPk=" + ReqPk + "").Trim() != "0")
                        if (MAXHI == appStage)
                        {

                            DateTime dtfrom = Convert.ToDateTime(fromDts[appIndx].Split('/')[1] + "/" + fromDts[appIndx].Split('/')[0] + "/" + fromDts[appIndx].Split('/')[2]);
                            DateTime dtTo = Convert.ToDateTime(toDts[appIndx].Split('/')[1] + "/" + toDts[appIndx].Split('/')[0] + "/" + toDts[appIndx].Split('/')[2]);
                            bool isHalf = isHafs[appIndx] == "1" ? true : false;
                            DateTime dtHalf = isHalf ? Convert.ToDateTime(halfDts[appIndx].Split('/')[1] + "/" + halfDts[appIndx].Split('/')[0] + "/" + halfDts[appIndx].Split('/')[2]) : DateTime.Now;
                            int HalfSession = isHalf ? Convert.ToInt32(hafSessions[appIndx]) : 0;
                            int totDays = Convert.ToInt32(totDay[appIndx]);
                            int leaveCode = Convert.ToInt32(lCodes[appIndx]);

                            //markAttendance(txtPopRollNo.Text.Trim(), txtPopAppNo.Text.Trim(), dtfrom, dtTo, totDays, leaveCode, isHalf, dtHalf, HalfSession);

                            markAttendance(rollNo, appNo, dtfrom, dtTo, totDays, leaveCode, isHalf, dtHalf, HalfSession);

                            string appQ = "update SR_Student_Leave_Request set RequestStatus='1' where LeaveRequestPk in (" + ReqPk + ") and  RequestStatus='0'";
                            approved += DA.update_method_wo_parameter(appQ, "Text");
                        }
                        if (req != 0)
                        {
                            ButtonApprove_Click(sender, e);
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Requests Approved')", true);
                        }
                    }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Finance Details')", true);
                    //}
                    appIndx++;
                }
            }
            catch { }
            if (approved > 0)
            {
                //ButtonReport_Click(sender, e);
                ButtonApprove_Click(sender, e);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + approved + " Requests Approved')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Approved')", true);
            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Requests')", true);
        }
    }

    private bool checkAppInp(ref string RePks, ref string appNos, ref string totalDays, ref string LeaveCodes, ref string fromDates, ref string toDates, ref string halfDates, ref string isHalfs, ref string halfSessions)
    {
        bool appok = false;
        try
        {
            StringBuilder sbRePks = new StringBuilder();
            StringBuilder sbAppNos = new StringBuilder();
            StringBuilder sbTotDays = new StringBuilder();

            StringBuilder sbLeaveCodes = new StringBuilder();
            StringBuilder sbFromDates = new StringBuilder();
            StringBuilder sbToDates = new StringBuilder();

            StringBuilder sbIsHalfs = new StringBuilder();
            StringBuilder sbHalfDates = new StringBuilder();
            StringBuilder sbHalfTime = new StringBuilder();

            foreach (GridViewRow gRow in gridLeaveApprove.Rows)
            {
                CheckBox cbSel = (CheckBox)gRow.FindControl("cbSel");
                if (cbSel.Checked)
                {
                    appok = true;
                    Label lblReqPk = (Label)gRow.FindControl("lblReqPk");
                    Label lblAppNo = (Label)gRow.FindControl("lblAppNo");
                    Label lbl_TotDays = (Label)gRow.FindControl("lbl_TotDays");

                    Label lblLeaveCode = (Label)gRow.FindControl("lblLeaveCode");
                    Label lbl_FromDate = (Label)gRow.FindControl("lbl_FromDate");
                    Label lbl_ToDate = (Label)gRow.FindControl("lbl_ToDate");

                    Label lbl_HalfDate = (Label)gRow.FindControl("lbl_HalfDate");
                    Label lbl_isHalf = (Label)gRow.FindControl("lbl_isHalf");
                    Label lbl_HalfTime = (Label)gRow.FindControl("lbl_HalfTime");

                    sbRePks.Append(lblReqPk.Text.Trim() + ",");
                    sbAppNos.Append(lblAppNo.Text.Trim() + ",");
                    sbTotDays.Append(lbl_TotDays.Text.Trim() + ",");

                    sbLeaveCodes.Append(lblLeaveCode.Text.Trim() + ",");
                    sbFromDates.Append(lbl_FromDate.Text.Trim() + ",");
                    sbToDates.Append(lbl_ToDate.Text.Trim() + ",");

                    sbHalfDates.Append(lbl_HalfDate.Text.Trim() + ",");
                    sbIsHalfs.Append((lbl_isHalf.Text.Trim() == "No" ? "0" : "1") + ",");
                    sbHalfTime.Append((lbl_HalfTime.Text.Trim().ToLower() == "morning" ? "0" : "1") + ",");

                }
            }
            if (sbRePks.Length > 0)
            {
                sbRePks.Remove(sbRePks.Length - 1, 1);
                RePks = sbRePks.ToString();
            }
            if (sbAppNos.Length > 0)
            {
                sbAppNos.Remove(sbAppNos.Length - 1, 1);
                appNos = sbAppNos.ToString();
            }
            if (sbTotDays.Length > 0)
            {
                sbTotDays.Remove(sbTotDays.Length - 1, 1);
                totalDays = sbTotDays.ToString();
            }
            if (sbLeaveCodes.Length > 0)
            {
                sbLeaveCodes.Remove(sbLeaveCodes.Length - 1, 1);
                LeaveCodes = sbLeaveCodes.ToString();
            }
            if (sbFromDates.Length > 0)
            {
                sbFromDates.Remove(sbFromDates.Length - 1, 1);
                fromDates = sbFromDates.ToString();
            }
            if (sbToDates.Length > 0)
            {
                sbToDates.Remove(sbToDates.Length - 1, 1);
                toDates = sbToDates.ToString();
            }
            if (sbHalfDates.Length > 0)
            {
                sbHalfDates.Remove(sbHalfDates.Length - 1, 1);
                halfDates = sbHalfDates.ToString();
            }
            if (sbIsHalfs.Length > 0)
            {
                sbIsHalfs.Remove(sbIsHalfs.Length - 1, 1);
                isHalfs = sbIsHalfs.ToString();
            }
            if (sbHalfTime.Length > 0)
            {
                sbHalfTime.Remove(sbHalfTime.Length - 1, 1);
                halfSessions = sbHalfTime.ToString();
            }
        }
        catch { }
        return appok;
    }

    protected void btnRejectReq_Click(object sender, EventArgs e)
    {
        try
        {
            string RePks = string.Empty;
            if (checkRejInp(ref RePks))
            {
                // divPopAlert.Visible = true;
                div1.Visible = true;
                txtnote.Visible = true;
                txtnote.Text = "";
            }
            else
            {
                //divPopAlert.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Requests')", true);
            }

        }
        catch
        {
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtnote.Text))
            {
                string RePks = string.Empty;
                if (checkRejInp(ref RePks))
                {
                    int rejected = 0;
                    try
                    {

                        string appQ = "update SR_Student_Leave_Request set RequestStatus='2',rejectReson='" + txtnote.Text + "' where LeaveRequestPk in (" + RePks + ") ";//and  RequestStatus='0'
                        rejected = DA.update_method_wo_parameter(appQ, "Text");
                    }
                    catch { }
                    if (rejected > 0)
                    {
                        div1.Visible = false;
                        txtnote.Visible = false;
                        ButtonApprove_Click(sender, e);
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + rejected + " Requests Rejected')", true);
                    }
                    else
                    {
                        div1.Visible = false;
                        txtnote.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Rejected')", true);
                    }
                }
                else
                {
                    div1.Visible = false;
                    txtnote.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Requests')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Enter Reason')", true);
            }
        }
        catch
        {

        }
    }

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = false;
            txtnote.Visible = false;
        }
        catch
        {
        }
    }


    private bool checkRejInp(ref string RePks)
    {
        bool appok = false;
        try
        {
            StringBuilder sbRePks = new StringBuilder();
            foreach (GridViewRow gRow in gridLeaveApprove.Rows)
            {
                CheckBox cbSel = (CheckBox)gRow.FindControl("cbSel");
                if (cbSel.Checked)
                {
                    appok = true;
                    Label lblReqPk = (Label)gRow.FindControl("lblReqPk");
                    sbRePks.Append(lblReqPk.Text.Trim() + ",");
                }
            }
            if (sbRePks.Length > 0)
            {
                sbRePks.Remove(sbRePks.Length - 1, 1);
                RePks = sbRePks.ToString();
            }
        }
        catch { }
        return appok;
    }
    //Search
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            //student query
            if (chosedmode == 0)
            {
                query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc";
            }
            else if (chosedmode == 1)
            {
                query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Reg_No asc";
            }
            else if (chosedmode == 2)
            {
                query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Roll_admit asc";
            }
            else if (chosedmode == 4)
            {
                query = "select  top 100 smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by smart_serial_no asc";
            }
            else
            {
                byte studAppSHrtAdm = statStudentAppliedShorlistAdmit();
                string admStudFilter = "";
                switch (studAppSHrtAdm)
                {
                    case 0:
                        admStudFilter = " and isconfirm=1 ";
                        break;
                    case 1:
                        admStudFilter = " and isconfirm=1 and selection_status=1 ";
                        break;
                    case 2:
                        admStudFilter = " and isconfirm=1 and selection_status=1 and admission_status=1 ";
                        break;
                }
                query = "  select  top 100 app_formno from applyn where  app_formno like '" + prefixText + "%' and college_code=" + collegecodestat + " " + admStudFilter + "  order by app_formno asc";
            }


            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%' and r.college_code=" + collegecodestat + "";

        Hashtable studhash = ws.Getnamevalue(query);
        if (studhash.Count > 0)
        {
            foreach (DictionaryEntry p in studhash)
            {
                string studname = Convert.ToString(p.Key);
                name.Add(studname);
            }
        }
        return name;
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Smartno.Visible = false;
        txt_rollno.Visible = true;
        txt_rollno.Text = "";
        txt_Smartno.Text = "";
        txt_dept.Text = "";
        txt_SeatType.Text = "";
        txt_FatherName.Text = "";
        txt_name.Text = "";
        txt_Sem.Text = "";
        txt_Batc.Text = "";
        txtIntAppNo.Text = "";
        txtIntDegCode.Text = "";
        if (Session["RollNo"] != null)
            Session.Remove("RollNo");

        img_stud.ImageUrl = "";
        img_stud.Visible = false;
        //txt_rollno.TextMode = TextBoxMode.SingleLine;
        string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
        string colleges = Convert.ToString(DA.GetFunction(useCOdeSet)).Trim();
        if (colleges == "" || colleges == "0")
        {
            colleges = collegecode;
        }
        int smartDisp = Convert.ToInt32(DA.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code in(" + collegecode + ")").Trim());

        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
            case1:
                txt_rollno.Attributes.Add("placeholder", "Roll No");
                lbl_rollno3.Text = "Roll No";
                chosedmode = 0;
                break;
            case 1:
            case2:
                txt_rollno.Attributes.Add("placeholder", "Reg No");
                lbl_rollno3.Text = "Reg No";
                chosedmode = 1;
                break;
            case 2:
            case3:
                txt_rollno.Attributes.Add("placeholder", "Admin No");
                lbl_rollno3.Text = "Admin No";
                chosedmode = 2;
                break;
            case 3:
            case4:
                txt_rollno.Attributes.Add("placeholder", "App No");
                lbl_rollno3.Text = "App No";
                chosedmode = 3;
                break;
            case 4:
                txt_rollno.Attributes.Add("placeholder", "Smartcard No");
                lbl_rollno3.Text = "SmartCard No";
                chosedmode = 4;
                //txt_rollno.TextMode = TextBoxMode.Password;
                txt_Smartno.Visible = true;
                //txt_rollno.Visible = false;
                switch (smartDisp)
                {
                    case 0:
                        goto case1;
                    case 1:
                        goto case2;
                    case 2:
                        goto case3;
                    case 3:
                        goto case4;
                }
                break;
        }
        textRoll();
    }
    protected void txt_rollno_Changed(object sender, EventArgs e)
    {
        textRoll();
    }
    private void textRoll()
    {
        string appNo = "";
        try
        {
            string name = "";
            string degree = "";
            string stType = "";
            string fname = "";
            string batch = "";
            string degCode = "";
            //lbltype.Text = "";

            string query = "";

            string roll_no = Convert.ToString(txt_rollno.Text.Trim());
            img_stud.ImageUrl = "";
            img_stud.Visible = false;
            string cursemvalue = "1";
            if (roll_no != "")
            {
                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    query = "select r.Roll_No,r.Roll_Admit,r.app_no,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,(select TextVal from TextValTable where TextCode=(select seattype from Applyn where app_no=r.app_no) and TextCriteria='seat' ) as StType,(select parent_name from applyn where app_no=r.app_no) as fname, ISNULL( type,'') as type,R.Current_Semester,r.batch_year  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.college_code='" + collegecode + "'  ";

                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        query += " and r.Roll_No like '" + roll_no + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        query += " and r.Reg_No like '" + roll_no + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        query += " and r.Roll_Admit like '" + roll_no + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                    {
                        //Smart card No
                        query += " and r.smart_serial_no like '" + txt_Smartno.Text.Trim() + "'";
                    }
                    else
                    {
                        query = "";
                    }
                }
                else
                {
                    byte studAppSHrtAdm = StudentAppliedShorlistAdmit();
                    string admStudFilter = "";
                    switch (studAppSHrtAdm)
                    {
                        case 0:
                            admStudFilter = " and a.isconfirm=1 ";
                            break;
                        case 1:
                            admStudFilter = " and a.isconfirm=1 and a.selection_status=1 ";
                            break;
                        case 2:
                            admStudFilter = " and a.isconfirm=1 and a.selection_status=1 and a.admission_status=1 ";
                            break;
                    }
                    query = "select stud_name,c.Course_Name+' - '+ dt.Dept_Name as degree,(select TextVal from TextValTable where TextCode=seattype and TextCriteria='seat' ) as StType,parent_name as fname ,ISNULL( type,'') as type,a.app_no,a.batch_year  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and a.college_code='" + collegecode + "'  and app_formno = '" + roll_no + "' " + admStudFilter;
                }

                DataSet dsStudDetails = DA.select_method_wo_parameter(query, "Text");
                if (dsStudDetails.Tables.Count > 0 && dsStudDetails.Tables[0].Rows.Count > 0)
                {
                    name = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["Stud_Name"]);
                    degree = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["Degree"]);
                    stType = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["stType"]);
                    fname = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["fname"]);
                    //lbltype.Text = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["type"]);
                    appNo = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["app_no"]);
                    batch = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["batch_year"]);
                    cursemvalue = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["Current_Semester"]);

                    degCode = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["degree_code"]);

                    Session["RollNo"] = Convert.ToString(dsStudDetails.Tables[0].Rows[0]["roll_no"]);

                    DataSet dsLeaveDet = new DataSet();
                    dsLeaveDet = LeaveMaster(Convert.ToString(dsStudDetails.Tables[0].Rows[0]["degree_code"]), collegecode);
                    int maxLeave = 0;
                    if (dsLeaveDet.Tables.Count > 0 && dsLeaveDet.Tables[0].Rows.Count > 0)
                    {
                        int.TryParse(Convert.ToString(dsLeaveDet.Tables[0].Rows[0]["MaxLeave"]), out maxLeave);
                    }
                    lblMaxLeaveAns.Text = maxLeave.ToString();
                    lblTakenLeaveAns.Text = "0";
                    lblRemLeaveAns.Text = lblMaxLeaveAns.Text;
                }

                txt_name.Text = name;
                txt_dept.Text = degree;
                txt_SeatType.Text = stType;
                txt_FatherName.Text = fname;
                txt_Sem.Text = cursemvalue;
                txt_Batc.Text = batch;
                txtIntAppNo.Text = appNo;
                txtIntDegCode.Text = degCode;
                img_stud.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no;
                img_stud.Visible = true;
                divReq.Visible = true;
            }
            else
            {
                txt_name.Text = "";
                txt_dept.Text = "";
                txt_SeatType.Text = "";
                txt_FatherName.Text = "";
                txt_Sem.Text = "";
                txt_Batc.Text = "";
                txtIntAppNo.Text = "";
                txtIntDegCode.Text = "";
                if (Session["RollNo"] != null)
                    Session.Remove("RollNo");
                img_stud.Visible = false;
                divReq.Visible = false;
            }

        }
        catch (Exception ex) { }
        ButtonReq_Click(new object(), new EventArgs());
    }
    protected void txt_Smartno_Changed(object sender, EventArgs e)
    {

        if (txt_Smartno.Text.Trim() != "")
        {
            string Q = string.Empty;
            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(DA.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }
            int smartDisp = Convert.ToInt32(DA.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code in (" + collegecode + ")").Trim());
            switch (smartDisp)
            {
                case 0:
                    Q = "select Roll_no from Registration where smart_serial_no ='" + txt_Smartno.Text.Trim() + "'  and r.college_code='" + collegecode + "' ";
                    break;
                case 1:
                    Q = "select Reg_no from Registration where smart_serial_no ='" + txt_Smartno.Text.Trim() + "'  and r.college_code='" + collegecode + "' ";
                    break;
                case 2:
                    Q = "select Roll_admit from Registration where smart_serial_no ='" + txt_Smartno.Text.Trim() + "'  and r.college_code='" + collegecode + "' ";
                    break;
                case 3:
                    Q = "select app_formno from applyn a,Registration r where a.app_no=r.App_No and r.smart_serial_no='" + txt_Smartno.Text.Trim() + "'  and r.college_code='" + collegecode + "' ";
                    break;
            }
            string nu = DA.GetFunction(Q).Trim();
            if (nu == "0")
                txt_rollno.Text = string.Empty;
            else
                txt_rollno.Text = nu;

            textRoll();
        }
        else
        {
            txt_rollno.Text = string.Empty;
            txt_name.Text = "";
            txt_dept.Text = "";
            txt_SeatType.Text = "";
            txt_FatherName.Text = "";
            txt_Sem.Text = "";
            txt_Batc.Text = "";
            txtIntAppNo.Text = "";
            txtIntDegCode.Text = "";
            if (Session["RollNo"] != null)
                Session.Remove("RollNo");
            img_stud.Visible = false;
        }

    }
    protected void txt_name_Changed(object sender, EventArgs e)
    {
        try
        {
            string roll_no = Convert.ToString(txt_name.Text);

            if (roll_no != "")
            {
                try
                {
                    string rollno = roll_no.Split('-')[4];
                    roll_no = rollno;
                }
                catch { roll_no = ""; }
            }

            txt_rollno.Text = roll_no;
            rbl_rollno.SelectedIndex = 0;
            txt_rollno_Changed(sender, e);
        }
        catch { }
    }
    private byte StudentAppliedShorlistAdmit()
    {

        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode + "'";
        byte moveVal = 0;
        byte.TryParse(DA.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    private static byte statStudentAppliedShorlistAdmit()
    {

        string Q = "select LinkValue from New_InsSettings where LinkName='StudentAppliedShorlistAdmit' and user_code ='" + usercodestat + "' --and college_code ='" + collegecodestat + "'";
        byte moveVal = 0;
        byte.TryParse(d22.GetFunction(Q.Trim()), out moveVal);
        return moveVal;
    }
    //Look up student
    protected void btn_roll_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindType();
        bindbatch1();
        binddegree2();
        bindbranch1();
        bindsec2();
        txt_rollno3.Text = "";
        btn_studOK.Visible = false;
        btn_exitstud.Visible = false;
        Fpspread1.Visible = false;
        lbl_errormsg.Visible = false;
    }
    protected void btn_studOK_Click(object sender, EventArgs e)
    {
        if (Fpspread1.Sheets[0].RowCount > 0)
        {
            Fpspread1.SaveChanges();
            string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();

            if (actrow != "" && actcol != "" && actrow != "-1" && actcol != "-1")
            {
                string rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                string rolladmit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string regno1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Text);
                string smartno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Text);

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    //  rollno = rollno;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    rollno = regno1;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    rollno = rolladmit;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    //Smartcard No

                    txt_Smartno.Text = smartno;
                    int smartDisp = Convert.ToInt32(DA.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code  in (" + collegecode + ")").Trim());
                    switch (smartDisp)
                    {
                        case 0:
                            rollno = rollno;
                            break;
                        case 1:
                            rollno = regno1;
                            break;
                        case 2:
                        case 3:
                            rollno = rolladmit;
                            break;
                    }
                }
                else
                {
                    //App no
                    rollno = rolladmit;
                }

                if (rollno.Trim() != string.Empty)
                {
                    popwindow.Visible = false;
                    txt_rollno.Text = rollno.Trim();
                    textRoll();
                }
            }
            Fpspread1.Sheets[0].ActiveRow = -1;
            Fpspread1.Sheets[0].ActiveColumn = -1;
            Fpspread1.SaveChanges();
        }
    }
    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            lbl_errormsg.Visible = true;
            Fpspread1.Visible = false;
            btn_studOK.Visible = false;
            btn_exitstud.Visible = false;
            Fpspread1.SaveChanges();
            #region Base Data
            string itemheader = GetSelectedItemsValueAsString(cbl_branch1);

            string section = GetSelectedItemsValueAsString(cbl_sec2).Trim();

            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);

            string strorderby = DA.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY len(r.Roll_No),r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY len(r.Reg_No),r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY len(r.Roll_No),r.Roll_No,r.Stud_Name";
                }
                else
                {
                    strorderby = "";
                }
            }

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(DA.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }
            int smartDisp = Convert.ToInt32(DA.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code  in (" + collegecode + ")").Trim());
            #endregion
            #region Search Query

            string app_no = "0";
            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3 || (smartDisp != 3 && Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3))
            {

                if (txt_rollno3.Text.Trim() == "")
                {
                    string stream = "";
                    if (ddl_strm.Enabled && ddl_strm.Items.Count > 0)
                    {
                        stream = "  and c.type in('" + ddl_strm.SelectedValue + "') ";
                    }

                    selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No,r.App_No,c.type   from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  and isnull(r.Sections,'') in ('" + section.Trim() + "') " + stream + "  and r.college_code='" + collegecode + "' ";

                }
                else
                {
                    selectquery = "select Roll_No,Roll_Admit,smart_serial_no,Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No,r.App_No,c.type   from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.college_code='" + collegecode + "'  ";


                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        selectquery += "   and Roll_No ='" + txt_rollno3.Text.Trim() + "' ";
                        app_no = DA.GetFunction("select app_no from registration where roll_no='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        selectquery += "  and Reg_No = '" + txt_rollno3.Text.Trim() + "' ";
                        app_no = DA.GetFunction("select app_no from registration where reg_no='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        selectquery += " and Roll_admit = '" + txt_rollno3.Text.Trim() + "' ";
                        app_no = DA.GetFunction("select app_no from registration where roll_admit='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                    {
                        //Smart card no

                        switch (smartDisp)
                        {
                            case 0:
                                selectquery += "   and Roll_No ='" + txt_rollno3.Text.Trim() + "' ";
                                app_no = DA.GetFunction("select app_no from registration where roll_no='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                                break;
                            case 1:
                                selectquery += "  and Reg_No = '" + txt_rollno3.Text.Trim() + "' ";
                                app_no = DA.GetFunction("select app_no from registration where reg_no='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                                break;
                            case 2:
                                selectquery += " and Roll_admit = '" + txt_rollno3.Text.Trim() + "' ";
                                app_no = DA.GetFunction("select app_no from registration where roll_admit='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                                break;
                        }
                        //  selectquery += " and smart_serial_no = '" + txt_rollno3.Text + "' ";
                    }
                    selectquery += strorderby;
                }
            }
            else
            {
                byte studAppSHrtAdm = StudentAppliedShorlistAdmit();
                string admStudFilter = "";
                string admStudFilTer = "";
                switch (studAppSHrtAdm)
                {
                    case 0:
                        admStudFilter = " and r.isconfirm=1  and isnull(r.selection_status,'0')='0' and isnull(r.admission_status,'0')='0'  and r.app_no not in (select app_no from registration where Degree_Code in('" + itemheader + "')  and batch_year in('" + batch_year + "'))";
                        admStudFilTer = " and r.isconfirm=1  and isnull(r.selection_status,'0')='0' and isnull(r.admission_status,'0')='0' ";
                        break;
                    case 1:
                        admStudFilter = " and r.isconfirm=1 and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='0'  and r.app_no not in (select app_no from registration where Degree_Code in('" + itemheader + "')  and batch_year in('" + batch_year + "'))";
                        admStudFilTer = " and r.isconfirm=1 and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='0' ";

                        break;
                    case 2:
                        admStudFilter = " and r.isconfirm=1 and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='1' and r.app_no not in (select app_no from registration where Degree_Code in('" + itemheader + "')  and batch_year in('" + batch_year + "'))";
                        admStudFilTer = " and r.isconfirm=1 and isnull(r.selection_status,'0')='1' and isnull(r.admission_status,'0')='1' ";
                        break;
                }
                if (txt_rollno3.Text.Trim() == "")
                {
                    string stream = "";
                    if (ddl_strm.Enabled && ddl_strm.Items.Count > 0)
                    {
                        stream = "  and c.type in('" + ddl_strm.SelectedValue + "') ";
                    }

                    selectquery = "  select r.app_formno as  Roll_No,r.app_formno as smart_serial_no,r.app_formno as Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.app_formno as Reg_No ,r.app_formno,r.App_No,c.type   from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "' ) " + stream + "  " + admStudFilter + "  and r.college_code='" + collegecode + "'";

                    selectquery = selectquery + " order by Roll_No,d.Degree_Code ";
                }
                else
                {
                    //App no
                    selectquery = "select r.app_formno as  Roll_No,r.app_formno as smart_serial_no,r.app_formno as Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department,r.app_formno as Reg_No ,r.app_formno,r.App_No,c.type   from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id   " + admStudFilter + "  and r.college_code='" + collegecode + "'  and r.app_formno ='" + txt_rollno3.Text + "'  ";
                    app_no = DA.GetFunction("select app_no from registration where roll_admit='" + txt_rollno3.Text.Trim() + "'  and college_code='" + collegecode + "' ");
                }

            }
            DataSet ds = DA.select_method_wo_parameter(selectquery, "Text");
            #endregion
            #region Data Presentation
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 8;

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 80;
                Fpspread1.Sheets[0].Columns[1].Locked = false;
                Fpspread1.Sheets[0].Columns[1].Visible = false;
                Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Admit";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Columns[5].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Columns[6].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                //Fpspread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Columns[7].Width = 270;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    //Smartcard no
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    if (smartDisp == 0)
                        Fpspread1.Sheets[0].Columns[3].Visible = true;
                    else if (smartDisp == 1)
                        Fpspread1.Sheets[0].Columns[4].Visible = true;
                    else if (smartDisp == 2 || smartDisp == 3)
                        Fpspread1.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    //App no
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App No";
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }

                Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[7].Font.Name = "Book Antiqua";

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);

                }

                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                Fpspread1.SaveChanges();

                lbl_errormsg.Visible = false;
                Fpspread1.Visible = true;
                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
            }
            #endregion
        }
        catch { }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        textRoll();
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree2();
        bindbranch1();
        bindsec2();
    }
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            DataSet ds = DA.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    public void bindType()
    {
        try
        {
            ddl_strm.Items.Clear();
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type<>''  and r.college_code='" + collegecode + "'  order by type asc";

            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
                ddl_strm.Enabled = true;
            }
            else
            {
                ddl_strm.Enabled = false;
            }
        }
        catch (Exception ex) { }
    }
    public void binddegree2()
    {
        try
        {
            cbl_degree2.Items.Clear();
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";

            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(DA.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }

            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode + ") ";
            if (ddl_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
            DataSet ds = DA.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = lbl_degree2.Text + "(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch (Exception ex) { }
    }
    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code ";
            }
            if (branch.Trim() != "")
            {
                DataSet ds = DA.select_method_wo_parameter(commname, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }
    public void bindsec2()
    {
        try
        {
            cbl_sec2.Items.Clear();
            txt_sec2.Text = "--Select--";
            ListItem item = new ListItem("Empty", " ");
            if (ddl_batch1.Items.Count > 0)
            {
                string strbatch = Convert.ToString(ddl_batch1.SelectedItem.Value);
                string branch = "";
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "" + "," + "" + "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (branch != "")
                {
                    DataSet dsSec = DA.BindSectionDetail(strbatch, branch);
                    if (dsSec.Tables.Count > 0)
                    {
                        if (dsSec.Tables[0].Rows.Count > 0)
                        {
                            cbl_sec2.DataSource = dsSec;
                            cbl_sec2.DataTextField = "sections";
                            cbl_sec2.DataValueField = "sections";
                            cbl_sec2.DataBind();


                        }
                    }
                    cbl_sec2.Items.Insert(0, item);
                    for (int i = 0; i < cbl_sec2.Items.Count; i++)
                    {
                        cbl_sec2.Items[i].Selected = true;
                    }
                    cb_sec2.Checked = true;
                    txt_sec2.Text = "Section(" + cbl_sec2.Items.Count + ")";

                }
            }


        }
        catch (Exception ex) { }
    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, lbl_degree2.Text);
        bindbranch1();
        bindsec2();
    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, lbl_degree2.Text);
        bindbranch1();
        bindsec2();
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }
    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }
    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }
    //Individual Pop Up Approve, Reject , Delete
    protected void imgViewClose_Click(object sender, EventArgs e)
    {
        divViewPopUp.Visible = false;
    }
    protected void btnViewRep_OpenPopUp(object sender, EventArgs e)
    {

        divViewPopUp.Visible = true;
        spanPopUpHeader.InnerHtml = "Leave Request Report";
        //LoadPopView(gridLeaveReport);
        //LoadPopView(gridLeaveApprove, rowIndex);
        btnDeleteReqPop.Visible = true;
        btnApproveReqPop.Visible = false;
        btnRejectReqPop.Visible = false;
    }

    protected void btnViewApp_OpenPopUp(object sender, EventArgs e)
    {
        divViewPopUp.Visible = true;
        spanPopUpHeader.InnerHtml = "Leave Approve / Reject";
        //LoadPopView(gridLeaveApprove);
        btnDeleteReqPop.Visible = false;
        btnApproveReqPop.Visible = true;
        btnRejectReqPop.Visible = true;

    }

    protected void gridLeaveApprove_RowCommand(object sender, GridViewCommandEventArgs e)//gridLeaveReport_RowCommand
    {
        if (e.CommandName == "View")
        {

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            divViewPopUp.Visible = true;
            spanPopUpHeader.InnerHtml = "Leave Approve / Reject";
            LoadPopView(gridLeaveApprove, rowIndex);
            btnDeleteReqPop.Visible = false;
            btnApproveReqPop.Visible = true;
            btnRejectReqPop.Visible = true;
        }
    }

    protected void gridLeaveReport_RowCommand(object sender, GridViewCommandEventArgs e)//gridLeaveReport_RowCommand
    {
        if (e.CommandName == "View")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            divViewPopUp.Visible = true;
            spanPopUpHeader.InnerHtml = "Leave Approve / Reject";
            LoadPopView(gridLeaveReport, rowIndex);
            btnDeleteReqPop.Visible = false;
            btnApproveReqPop.Visible = false;
            btnRejectReqPop.Visible = false;
        }
    }

    private void LoadPopView(GridView grid, int rowindex)
    {
        try
        {
            imgViewClose.Focus();
            int rowIndx = rowindex;

            Label lblReqPk = (Label)grid.Rows[rowIndx].FindControl("lblReqPk");
            Label lblLeaveReqStat = (Label)grid.Rows[rowIndx].FindControl("lblLeaveReqStat");
            Label lblAppNo = (Label)grid.Rows[rowIndx].FindControl("lblAppNo");
            Label lbl_StudName = (Label)grid.Rows[rowIndx].FindControl("lbl_StudName");
            Label lbl_Branch = (Label)grid.Rows[rowIndx].FindControl("lbl_Branch");
            Label lbl_AdmNo = (Label)grid.Rows[rowIndx].FindControl("lbl_AdmNo");
            Label lbl_RegNo = (Label)grid.Rows[rowIndx].FindControl("lbl_RegNo");
            Label lbl_RollNo = (Label)grid.Rows[rowIndx].FindControl("lbl_RollNo");
            Label lbl_FromDate = (Label)grid.Rows[rowIndx].FindControl("lbl_FromDate");
            Label lbl_ToDate = (Label)grid.Rows[rowIndx].FindControl("lbl_ToDate");
            Label lbl_TotDays = (Label)grid.Rows[rowIndx].FindControl("lbl_TotDays");
            Label lbl_lType = (Label)grid.Rows[rowIndx].FindControl("lbl_lType");
            Label lbl_lReason = (Label)grid.Rows[rowIndx].FindControl("lbl_lReason");
            Label lbl_lStatus = (Label)grid.Rows[rowIndx].FindControl("lbl_lStatus");
            Label lblLeaveCode = (Label)grid.Rows[rowIndx].FindControl("lblLeaveCode");

            Label lbl_HalfDate = (Label)grid.Rows[rowIndx].FindControl("lbl_HalfDate");
            Label lbl_isHalf = (Label)grid.Rows[rowIndx].FindControl("lbl_isHalf");
            Label lbl_HalfTime = (Label)grid.Rows[rowIndx].FindControl("lbl_HalfTime");
            string fdate = string.Empty;
            fdate = lbl_FromDate.Text.ToString();
            string tdate = lbl_ToDate.Text.ToString();

            txtPopHalfDate.Text = lbl_HalfDate.Text;
            txtPopIsHalf.Text = lbl_isHalf.Text.Trim() == "No" ? "0" : "1";
            txtPopHalfSession.Text = lbl_HalfTime.Text.Trim().ToLower() == "morning" ? "0" : "1";

            string appNo = lblAppNo.Text.Trim();
            string DegCode = string.Empty;
            string curSem = string.Empty;
            string batch = string.Empty;
            string rollNo = string.Empty;
            string collegeCode = string.Empty;

            DataSet dsStud = DA.select_method_wo_parameter("select degree_code,current_semester,batch_year,roll_no,college_code from registration where app_no='" + appNo + "'", "Text");
            if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
            {
                DegCode = Convert.ToString(dsStud.Tables[0].Rows[0]["degree_code"]).Trim();
                curSem = Convert.ToString(dsStud.Tables[0].Rows[0]["current_semester"]).Trim();
                batch = Convert.ToString(dsStud.Tables[0].Rows[0]["batch_year"]).Trim();
                rollNo = Convert.ToString(dsStud.Tables[0].Rows[0]["roll_no"]).Trim();
                collegeCode = Convert.ToString(dsStud.Tables[0].Rows[0]["college_code"]).Trim();
            }
            DataSet dsLeaveDet = new DataSet();
            dsLeaveDet = LeaveMaster(DegCode, collegeCode);
            double maxLeave = 0;
            double tknLeave = 0;
            if (dsLeaveDet.Tables.Count > 0 && dsLeaveDet.Tables[0].Rows.Count > 0)
            {
                double.TryParse(Convert.ToString(dsLeaveDet.Tables[0].Rows[0]["MaxLeave"]), out maxLeave);
            }

            BindGridLeaveDetails(appNo, DegCode, curSem, batch, rollNo, gridPopLeavehistory);


            //string selectQ = "select a.DispText,l.Maxval,l.EntryCode from AttMasterSetting a,leaveMaster l where a.CollegeCode=l.collegeCode and a.EntryCode=l.EntryCode and l.batchyear='" + batch + "' and l.eduLevel='UG' and l.semester='" + curSem + "' and l.collegeCode='" + collegeCode + "'";
            //DataTable dtleaveinfo = dirAcc.selectDataTable(selectQ);
            //if (dtleaveinfo.Rows.Count > 0)
            //{
            //    GridView1.DataSource = dtleaveinfo;
            //    GridView1.DataBind();
            //    GridView1.Visible = true;
            //}
            if (Session["LeaveConsumed"] != null)
            {
                double.TryParse(Convert.ToString(Session["LeaveConsumed"]), out tknLeave);
                Session.Remove("LeaveConsumed");
            }

            //spanPopViewStud.InnerHtml = "<table><tr><td>Name</td><td> : " + lbl_StudName.Text.ToUpper() + "</td></tr><tr><td>" + lblDegree.Text+"-"+lblBranch.Text + "</td><td> : " + lbl_Branch.Text + "</td></tr><tr><td>Admission No</td><td> : " + lbl_AdmNo.Text.ToUpper() + "</td></tr><tr><td>Reg No</td><td> : " + lbl_RegNo.Text.ToUpper() + "</td></tr><tr><td>Roll No</td><td> : " + lbl_RollNo.Text.ToUpper() + "</td></tr></table>";

            #region Student Details

            spanPopViewStud.InnerHtml = "<table><tr><td>Name</td><td> : " + lbl_StudName.Text.ToUpper() + "</td><td rowspan='5'><img height='120px' width='100px' alt=' ' src='" + "~/Handler/Handler4.ashx?rollno=" + lbl_RollNo.Text + "' /></td></tr><tr><td>" + lblDegree.Text + "-" + lblBranch.Text + "</td><td> : " + lbl_Branch.Text + "</td></tr>";

            if (grid.Columns[5].Visible)
            {
                spanPopViewStud.InnerHtml += "<tr><td>Admission No</td><td> : " + lbl_AdmNo.Text.ToUpper() + "</td></tr>";
            }
            if (grid.Columns[6].Visible)
            {
                spanPopViewStud.InnerHtml += "<tr><td>Reg No</td><td> : " + lbl_RegNo.Text.ToUpper() + "</td></tr>";
            }
            if (grid.Columns[7].Visible)
            {
                spanPopViewStud.InnerHtml += "<tr><td>Roll No</td><td> : " + lbl_RollNo.Text.ToUpper() + "</td></tr>";
            }

            spanPopViewStud.InnerHtml += "</table>";

            #endregion

            //spanPopViewLeave.InnerHtml = "<table><tr><td>Maximum Leave</td><td> : " + maxLeave + "</td></tr><tr><td>Leave Consumed</td><td> : " + tknLeave + "</td></tr><tr><td>Leaving Remaining</td><td> : " + ((maxLeave - tknLeave) < 0 ? 0 : (maxLeave - tknLeave)) + "</td></tr></table>";

            spanPopViewReqDet.InnerHtml = "<table><tr><td>From </td><td> : " + lbl_FromDate.Text + "</td></tr><tr><td>To</td><td> : " + lbl_ToDate.Text + "</td></tr><tr><td>Total Days</td><td> : " + lbl_TotDays.Text + "</td></tr><tr><td>Reason</td><td style='width:200px;'> : " + lbl_lReason.Text + "</td></tr><tr><td>Status</td><td> : " + lbl_lStatus.Text + "</td></tr></table>";

            txtPopAppNo.Text = appNo;
            txtPopReqPk.Text = lblReqPk.Text.Trim();
            txtPopReqStatus.Text = lblLeaveReqStat.Text.Trim();
            txtPopTotDays.Text = lbl_TotDays.Text.Trim();
            txtPopFromDate.Text = lbl_FromDate.Text;
            txtPopToDate.Text = lbl_ToDate.Text;
            txtPopRollNo.Text = lbl_RollNo.Text;
            txtPopLeaveCode.Text = lblLeaveCode.Text;


            div_GV1.Visible = true;
            btnReqSave.Visible = true;
            spanHolidays.InnerHtml = string.Empty;
            DataTable dtHoliDays = getHolidays(DegCode, curSem);
            ArrayList addnew = new ArrayList();

            DateTime fromdate = new DateTime();

            // fromdate = TextToDate(txt_fromdate);
            // isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
            bool f_date = DateTime.TryParseExact(fdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out fromdate);
            //fromdate = Convert.ToDateTime(fdate);
            DateTime todate = new DateTime();
            bool t_date = DateTime.TryParseExact(tdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out todate);
            // todate = TextToDate(txt_todate);
            //todate = Convert.ToDateTime(tdate);
            TimeSpan c = fromdate - todate;
            DataTable dt = new DataTable();
            dt.Columns.Add("Dummy");
            dt.Columns.Add("Dummy1");
            dt.Columns.Add("ischecked");
            dt.Columns.Add("ischecked1");

            string type = string.Empty;
            StringBuilder sbHolidays = new StringBuilder();
            for (; fromdate <= todate; )
            {

                DataView dvholiday = new DataView();
                if (dtHoliDays.Rows.Count > 0)
                {
                    dtHoliDays.DefaultView.RowFilter = "(halforfull='0') and Holidate='" + fromdate.Date.ToString() + "'";
                    dvholiday = dtHoliDays.DefaultView;
                }
                if (dvholiday.Count == 0 && !isSunday(fromdate))
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "1";
                    dr[1] = fromdate.ToString("dd/MM/yyyy");
                    string leavereq = "select * from student_leaveRequest_details where LeaveRequestPK='" + lblReqPk.Text.ToString() + "' and LeaveDate='" + fromdate.ToString() + "'";
                    DataSet dsleave = DA.select_method_wo_parameter(leavereq, "text");
                    if (dsleave.Tables.Count > 0 && dsleave.Tables[0].Rows.Count > 0)
                    {
                        type = Convert.ToString(dsleave.Tables[0].Rows[0]["SessionType"]);
                    }
                    if (type == "0")
                    {
                        dr[2] = 1;
                        dr[3] = 1;
                    }
                    if (type == "1")
                    {
                        dr[2] = 1;
                        dr[3] = 0;
                    }
                    if (type == "2")
                    {
                        dr[2] = 0;
                        dr[3] = 1;
                    }

                    dt.Rows.Add(dr);
                    fromdate = fromdate.AddDays(1);
                }
                else
                {
                    sbHolidays.Append("<tr  style='color:Black;  font-size:Medium;'><td style='width:100px;'>" + fromdate.ToString("dd/MM/yyyy") + "</td><td  style='width:250px;'>" + (isSunday(fromdate) ? "Sunday" : dvholiday[0]["holiday_desc"].ToString()) + "</td></tr>");
                    fromdate = fromdate.AddDays(1);
                }
            }



            if (dt.Rows.Count > 0)
            {
                GV1.DataSource = dt;
                GV1.DataBind();

                spanHolidays.InnerHtml = sbHolidays.ToString().Trim() != string.Empty ? "<table style='width:350px; margin-top:0px; ' border='1' cellpadding='0' cellspacing='0'><tr><td colspan='2' style='background-color:#0CA6CA; color:Black; font-weight:bold; font-size:Medium;'><center>Holidays</center></td></tr>" + sbHolidays.ToString() + "</table>" : string.Empty;

            }


            // BindGridview();
        }
        catch { }
    }

    protected void btnDeleteReqPop_Click(object sender, EventArgs e)
    {
        string RePks = txtPopReqPk.Text.Trim();
        if (txtPopReqStatus.Text.Trim() == "0")
        {
            int deleted = 0;
            try
            {
                string delQ = "delete from SR_Student_Leave_Request where LeaveRequestPk in (" + RePks + ") and  RequestStatus='0'";
                deleted = DA.update_method_wo_parameter(delQ, "Text");
            }
            catch { }
            if (deleted > 0)
            {
                ButtonReport_Click(sender, e);
                ButtonApprove_Click(sender, e);
                divViewPopUp.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + deleted + " Records Deleted')", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Deleted')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Cannot Delete This Record')", true);
        }
    }

    protected void btnApproveReqPop_Click(object sender, EventArgs e)
    {
        int req = 0;
        string RePks = txtPopReqPk.Text.Trim();
        string finyearID = financeYear(ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13");
        if (txtPopReqStatus.Text.Trim() == "0")
        {
            if (true)
            {
                int approved = 0;
                try
                {
                    string getref = d22.GetFunction("select degree_code from Registration where App_No in (select App_no from SR_Student_Leave_Request  where LeaveRequestPk='" + RePks + "')");
                    string maxHI = d22.GetFunction("select  max(ReqApproveStage) from RQ_RequestHierarchy where ReqStaffAppNo='" + getref + "' and RequestType=10");

                    req = DA.update_method_wo_parameter("update SR_Student_Leave_Request set approvalStage=(isnull(approvalStage,1)+1) where LeaveRequestPk=" + RePks + "", "Text");
                    DataSet dsinfo = d22.select_method_wo_parameter("select r.App_No,r.Roll_No,r.Reg_No,r.degree_code,r.Batch_Year from SR_Student_Leave_Request l,Registration r  where r.App_No=l.App_no and LeaveRequestPk='" + RePks + "'", "text");
                    string appNo = string.Empty;
                    string rollNo = string.Empty;

                    if (dsinfo.Tables.Count > 0 && dsinfo.Tables[0].Rows.Count > 0)
                    {
                        appNo = Convert.ToString(dsinfo.Tables[0].Rows[0]["App_No"]);
                        rollNo = Convert.ToString(dsinfo.Tables[0].Rows[0]["Roll_No"]);
                    }
                    int MAXHI = 0;
                    int.TryParse(maxHI, out MAXHI);
                    int appStage = 0;
                    if (ddlReqStage.Items.Count > 0)
                    {
                        int.TryParse(ddlReqStage.SelectedValue, out appStage);
                    }

                    //DA.update_method_wo_parameter("update SR_Student_Leave_Request set approvalStage=(isnull(approvalStage,1)+1) where LeaveRequestPk=" + RePks + "", "Text");
                    //if (DA.GetFunction("select ReqApproveStateCOunt from RQ_RequestHierarchy rh,SR_Student_Leave_Request sr where ReqApproveStateCOunt=isnull(approvalStage,0) and ReqStaffAppNo=RequestBy and LeaveRequestPk=" + RePks + "").Trim() != "0")
                    if (MAXHI <= appStage)
                    {
                        DateTime dtfrom = TextToDate(txtPopFromDate);
                        DateTime dtTo = TextToDate(txtPopToDate);
                        bool isHalf = txtPopIsHalf.Text.Trim() == "1" ? true : false;
                        DateTime dtHalf = isHalf ? TextToDate(txtPopHalfDate) : DateTime.Now;
                        int HalfSession = isHalf ? Convert.ToInt32(txtPopHalfSession.Text) : 0;
                        int totDays = Convert.ToInt32(txtPopTotDays.Text);
                        int leaveCode = Convert.ToInt32(txtPopLeaveCode.Text);

                        markAttendance(txtPopRollNo.Text.Trim(), txtPopAppNo.Text.Trim(), dtfrom, dtTo, totDays, leaveCode, isHalf, dtHalf, HalfSession);

                        string appQ = "update SR_Student_Leave_Request set RequestStatus='1' where LeaveRequestPk in (" + RePks + ") and  RequestStatus='0'";
                        approved = DA.update_method_wo_parameter(appQ, "Text");
                    }
                    if (req != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Requests Approved')", true);
                    }
                }
                catch { }
                if (approved > 0)
                {
                    ButtonReport_Click(sender, e);
                    ButtonApprove_Click(sender, e);
                    divViewPopUp.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + approved + " Requests Approved')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Approved')", true);
                }
                if (req != 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Requests Approved')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Finance Details')", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Cannot Approve This Record')", true);
        }
    }


    protected void ImageButton2_Click(object sender, EventArgs e)
    {
        div2.Visible = false;
        TextBox1.Visible = false;
        TextBox1.Text = "";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(TextBox1.Text))
            {
                string RePks = txtPopReqPk.Text.Trim();
                if (txtPopReqStatus.Text.Trim() == "0")
                {
                    int rejected = 0;
                    try
                    {
                        string fulldayL = string.Empty;
                        string RePks1 = txtPopReqPk.Text.Trim();
                        DateTime fromdate = new DateTime();
                        DateTime todate = new DateTime();
                        string fdate = txtPopFromDate.Text.ToString();
                        string tdate = txtPopToDate.Text.ToString();
                        bool f_date = DateTime.TryParseExact(fdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out fromdate);
                        bool t_date = DateTime.TryParseExact(tdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out todate);
                        int row = 0;
                        int rejectdate = 0;
                        string studdetail = string.Empty;
                        TextBox from_dt = new TextBox();
                        string insertqry = string.Empty;
                        int inqry = 0;

                        //for (; fromdate <= todate; )
                        //{
                        for (int remv = 0; remv < GV1.Rows.Count; remv++)
                        {


                            from_dt = (TextBox)GV1.Rows[remv].Cells[1].FindControl("txtdate");
                            string fdt = from_dt.Text.ToString();
                            string[] f_dt = fdt.Split('/');
                            string frm_date = f_dt[1] + "/" + f_dt[0] + "/" + f_dt[2];

                            bool checkedmrng = (GV1.Rows[remv].FindControl("chk_mrng") as CheckBox).Checked;
                            bool checkedevng = (GV1.Rows[remv].FindControl("chk_evng") as CheckBox).Checked;

                            if (checkedmrng == true && checkedevng == false)
                            {
                                fulldayL = "1";
                            }
                            else if (checkedevng == true && checkedmrng == false)
                            {
                                fulldayL = "2";
                            }
                            else if (checkedmrng == true && checkedevng == true)
                            {
                                fulldayL = "0";
                            }
                            else
                            {
                                fulldayL = "3";
                            }

                            string studdetails = DA.GetFunctionv("select SessionType from student_leaveRequest_details where LeaveDate='" + frm_date.ToString() + "' and LeaveRequestPK='" + RePks + "'");
                            if (Convert.ToInt32(fulldayL) != Convert.ToInt32(studdetails))
                            {

                                if (studdetails == "0")
                                {
                                    if (Convert.ToInt32(fulldayL) != 3)
                                    {
                                        studdetail = "update student_leaveRequest_details set SessionType='" + fulldayL + "' , isApproved='0' where LeaveDate='" + frm_date.ToString() + "' and LeaveRequestPK='" + RePks + "' ";
                                        if (Convert.ToInt32(fulldayL) == 1)
                                        {
                                            insertqry = "insert into student_leaveRequest_details values('" + RePks + "','" + frm_date.ToString() + "','2','1')";
                                        }
                                        if (Convert.ToInt32(fulldayL) == 2)
                                        {
                                            insertqry = "insert into student_leaveRequest_details values('" + RePks + "','" + frm_date.ToString() + "','1','1')";
                                        }

                                    }
                                    else
                                    {
                                        studdetail = "update student_leaveRequest_details set SessionType='" + fulldayL + "' , isApproved='1' where LeaveDate='" + frm_date.ToString() + "' and LeaveRequestPK='" + RePks + "' ";
                                    }
                                }
                                else
                                {
                                    studdetail = "update student_leaveRequest_details set SessionType='" + fulldayL + "' , isApproved='1' where LeaveDate='" + frm_date.ToString() + "' and LeaveRequestPK='" + RePks + "' ";
                                }


                            }
                            else
                            {
                                studdetail = "update student_leaveRequest_details set  isApproved='0' where LeaveDate='" + frm_date.ToString() + "' and LeaveRequestPK='" + RePks + "' and SessionType='" + fulldayL + "'";
                            }
                            rejectdate = DA.update_method_wo_parameter(studdetail, "text");
                            if (Convert.ToInt32(fulldayL) != 3 && studdetails == "0")
                            {
                                inqry = DA.update_method_wo_parameter(insertqry, "text");
                            }
                        }


                        string appQ = "update SR_Student_Leave_Request set RequestStatus='2',rejectReson='" + TextBox1.Text + "' where LeaveRequestPk in (" + RePks + ") and  RequestStatus='0'";
                        rejected = DA.update_method_wo_parameter(appQ, "Text");
                    }
                    catch { }
                    if (rejected > 0)
                    {
                        ButtonReport_Click(sender, e);
                        ButtonApprove_Click(sender, e);
                        divViewPopUp.Visible = false;
                        div2.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + rejected + " Requests Rejected')", true);
                    }
                    else
                    {
                        div2.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Rejected')", true);
                    }
                }
                else
                {
                    div2.Visible = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Cannot Reject This Record')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('please Enter reason')", true);
            }


        }
        catch
        {
        }
    }

    protected void btnRejectReqPop_Click(object sender, EventArgs e)
    {
        try
        {
            string RePks = txtPopReqPk.Text.Trim();
            if (txtPopReqStatus.Text.Trim() == "0")
            {
                // divPopAlert.Visible = true;
                div2.Visible = true;
                TextBox1.Visible = true;
                TextBox1.Text = "";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Cannot Reject This Record')", true);
            }


        }
        catch
        {
        }
    }
    //Mark Attendance 
    private void markAttendance(string RollNo, string appNo, DateTime dtfrom, DateTime dtTo, int totDays, int leaveCode, bool isHalf, DateTime halfDate, int halfSession)
    {
        try
        {
            string collegeCode = ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13";
            string DegCode = string.Empty;
            string curSem = string.Empty;
            DataSet dsStudDet = DA.select_method_wo_parameter("select degree_code, current_semester from registration where app_no='" + appNo + "'", "Text");
            if (dsStudDet.Tables.Count > 0 && dsStudDet.Tables[0].Rows.Count > 0)
            {
                DegCode = Convert.ToString(dsStudDet.Tables[0].Rows[0]["degree_code"]).Trim();
                curSem = Convert.ToString(dsStudDet.Tables[0].Rows[0]["current_semester"]).Trim();

                DataSet dsAttndSet = DA.select_method_wo_parameter("select p.degree_code,No_of_Hrs_Per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_I_half_day,min_pres_II_half_day,min_hrs_per_day from PeriodAttndSchedule p,degree r where p.degree_code=r.degree_code and r.college_code='" + collegeCode + "' and p.degree_code='" + DegCode + "' and p.semester='" + curSem + "' ", "Text");
                if (dsAttndSet.Tables.Count > 0 && dsAttndSet.Tables[0].Rows.Count > 0)
                {
                    byte hrsPerDay = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["No_of_Hrs_Per_day"]);
                    byte hrsFHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
                    byte hrsSHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);

                    byte minHrsDay = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_hrs_per_day"]);
                    byte minHrsFHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_pres_I_half_day"]);
                    byte minHrsSHalf = Convert.ToByte(dsAttndSet.Tables[0].Rows[0]["min_pres_II_half_day"]);

                    DataTable dtHoliDays = getHolidays(DegCode, curSem);

                    for (int lcRow = 0; dtfrom <= dtTo; dtfrom = dtfrom.AddDays(1), lcRow++)
                    {
                        //Go For Attendance Entry
                        byte curDayVal = (byte)dtfrom.Day;
                        byte curMonVal = (byte)dtfrom.Month;
                        int curYearVal = dtfrom.Year;

                        int attMonthYear = (curYearVal * 12) + curMonVal;

                        bool FstHalf = true;
                        bool SndHalf = true;

                        int mornLeaveCode = leaveCode;
                        int evenLeaveCode = leaveCode;

                        if (isHalf && dtfrom == halfDate)
                        {
                            if (halfSession == 0)
                            {
                                evenLeaveCode = 0;
                                FstHalf = false;
                            }
                            else
                            {
                                mornLeaveCode = 0;
                                SndHalf = false;
                            }
                        }

                        //Check For Holiday and Half Holiday
                        DataView dvholiday = new DataView();
                        if (dtHoliDays.Rows.Count > 0)
                        {
                            dtHoliDays.DefaultView.RowFilter = "(halforfull='0') and Holidate='" + dtfrom.Date.ToString() + "'";
                            dvholiday = dtHoliDays.DefaultView;
                        }
                        if (dvholiday.Count == 0 && !isSunday(dtfrom))
                        {
                            dtHoliDays.DefaultView.RowFilter = "(morning='0' or evening='0') and Holidate='" + dtfrom.Date.ToString() + "'";
                            dvholiday = dtHoliDays.DefaultView;
                            if (dvholiday.Count > 0)
                            {
                                string mornHoliday = Convert.ToString(dvholiday[0]["morning"]).Trim();
                                string evenHoliday = Convert.ToString(dvholiday[0]["evening"]).Trim();

                                if (mornHoliday == "0")
                                {
                                    mornLeaveCode = 0;
                                    FstHalf = false;
                                }
                                else if (evenHoliday == "0")
                                {
                                    evenLeaveCode = 0;
                                    SndHalf = false;
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }






                        //Query Build and Execution

                        StringBuilder sbInsColumnCol = new StringBuilder();
                        StringBuilder sbInsColumnVal = new StringBuilder();
                        StringBuilder sbUpdColumn = new StringBuilder();

                        //First Half
                        if (FstHalf)
                        {
                            for (int iFHalf = 1; iFHalf <= hrsFHalf; iFHalf++)
                            {
                                sbInsColumnCol.Append("D" + curDayVal + "D" + iFHalf + ",");
                                sbInsColumnVal.Append("'" + leaveCode + "',");

                                sbUpdColumn.Append("D" + curDayVal + "D" + iFHalf + "='" + leaveCode + "',");
                            }
                        }
                        //Second Half
                        if (SndHalf)
                        {
                            for (int iSHalf = (hrsFHalf + 1); iSHalf <= hrsPerDay; iSHalf++)
                            {
                                sbInsColumnCol.Append("D" + curDayVal + "D" + iSHalf + ",");
                                sbInsColumnVal.Append("'" + leaveCode + "',");

                                sbUpdColumn.Append("D" + curDayVal + "D" + iSHalf + "='" + leaveCode + "',");
                            }
                        }

                        if (FstHalf || SndHalf)
                        {
                            string insColFinalCol = sbInsColumnCol.ToString() + "Att_App_no,Att_CollegeCode, month_year,roll_no";
                            string insColFinalVal = sbInsColumnVal.ToString() + "'" + appNo + "','" + collegeCode + "','" + attMonthYear + "','" + RollNo + "'";
                            string updColFinal = sbUpdColumn.ToString().TrimEnd(',');

                            string attQ = "if exists (select month_year from attendance where month_year ='" + attMonthYear + "' and Att_App_no='" + appNo + "') update attendance set " + updColFinal + "  where month_year = '" + attMonthYear + "' and Att_App_no='" + appNo + "' else insert into attendance (" + insColFinalCol + ") values (" + insColFinalVal + ")";
                            int attOk = DA.update_method_wo_parameter(attQ, "Text");
                            if (attOk > 0)
                            {
                                string insTempAttend = "if exists (select appno from allstudentattendancereport where AppNo=" + appNo + " and  DateofAttendance='" + dtfrom.ToString("MM/dd/yyyy") + "') update allstudentattendancereport set MLeaveCode=" + mornLeaveCode + ", ELeaveCode=" + evenLeaveCode + " where AppNo=" + appNo + " and  DateofAttendance='" + dtfrom.ToString("MM/dd/yyyy") + "' else insert into allstudentattendancereport (AppNo, DateofAttendance, MLeaveCode, ELeaveCode) values (" + appNo + ",'" + dtfrom.ToString("MM/dd/yyyy") + "'," + mornLeaveCode + "," + evenLeaveCode + ")";
                                DA.update_method_wo_parameter(insTempAttend, "Text");
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    //Finance Settings
    private bool IsFinanceInclude()
    {
        bool include = false;
        try
        {
            string collegeCode = ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13";
            //// and user_code ='" + usercode + "'
            if (Convert.ToInt16(DA.GetFunction("select LinkValue from New_InsSettings where LinkName = 'IncludeFinanceLeaveRequest' and college_code ='" + collegeCode + "'")) > 0)
            {
                include = true;
            }
        }
        catch { include = false; }
        return include;
    }
    private bool financeCheck(string appNo, string finYearID, string totalDays)
    {
        bool finOk = false;
        try
        {
            if (IsFinanceInclude())
            {
                string collegeCode = ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13";
                string degCode = string.Empty;
                string curSem = string.Empty;
                DataSet dsStudDet = DA.select_method_wo_parameter("select degree_code, current_semester from registration where app_no='" + appNo + "'", "Text");
                if (dsStudDet.Tables.Count > 0 && dsStudDet.Tables[0].Rows.Count > 0)
                {
                    degCode = Convert.ToString(dsStudDet.Tables[0].Rows[0]["degree_code"]).Trim();
                    curSem = Convert.ToString(dsStudDet.Tables[0].Rows[0]["current_semester"]).Trim();

                    ListItem feeCategory = getFeeCategory(curSem);

                    string sqlSel = "SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND IsFinance ='1' AND CollegeCode ='" + collegeCode + "' AND DegreeCode ='" + degCode + "' and " + totalDays + ">=FromDay and " + totalDays + "<=ToDay";

                    DataSet dsFinSet = DA.select_method_wo_parameter(sqlSel, "Text");
                    if (dsFinSet.Tables.Count > 0 && dsFinSet.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        sqlSel = "SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND IsFinance ='1' AND CollegeCode ='" + collegeCode + "' AND DegreeCode ='" + degCode + "' and ToDay<=" + totalDays + " order by ToDay desc";
                        dsFinSet.Clear();
                        dsFinSet = DA.select_method_wo_parameter(sqlSel, "Text");
                    }
                    if (dsFinSet.Tables.Count > 0 && dsFinSet.Tables[0].Rows.Count > 0 && feeCategory.Value != "-1")
                    {
                        string amt = Convert.ToString(dsFinSet.Tables[0].Rows[0]["Amount"]).Trim();
                        string lid = Convert.ToString(dsFinSet.Tables[0].Rows[0]["LegerFK"]).Trim();
                        string hid = Convert.ToString(dsFinSet.Tables[0].Rows[0]["HeaderFK"]).Trim();

                        string selectQuery = "";
                        string updateQuery = "";

                        string insertQuery = " INSERT INTO FT_FeeAllot(AllotDate,MemType,PayMode,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,FeeCategory,BalAmount,FinYearFK,FromGovtAmt, DeductReason) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1,1," + appNo
 + "," + lid + "," + hid + "," + amt + "," + amt + "," + feeCategory.Value + "," + amt + "," + finYearID + ",0,0) ";

                        selectQuery = " select App_No from FT_FeeAllot where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCategory.Value + "')  and App_No in('" + appNo + "') ";

                        updateQuery = "  update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "', MemType=1,FeeAmount=isnull(FeeAmount,0)+" + amt + ",BalAmount=isnull(BalAmount,0)+" + amt + ",TotalAmount=isnull(TotalAmount,0)+" + amt + " where LedgerFK in('" + lid + "') and HeaderFK in('" + hid + "') and FeeCategory in('" + feeCategory.Value + "')  and App_No in('" + appNo + "') ";

                        string finalQuery = " if exists ( " + selectQuery + " ) " + updateQuery + " else " + insertQuery + " ";
                        int insok = DA.update_method_wo_parameter(finalQuery, "Text");
                        finOk = true;
                    }
                }
            }
            else
            {
                finOk = true;
            }
        }
        catch { finOk = false; }
        return finOk;
    }
    private string financeYear(string collegeCode)
    {
        string finYeaid = DA.getCurrentFinanceYear(usercode, collegeCode);
        return finYeaid;
    }
    private ListItem getFeeCategory(string Sem)
    {
        string collegeCode = ddlClgApp.Items.Count > 0 ? ddlClgApp.SelectedValue : "13";
        ListItem feeCategory = new ListItem();
        try
        {
            string linkvalue = DA.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegeCode + "'");
            DataSet dsFeecat = new DataSet();
            if (linkvalue == "0")
            {
                dsFeecat = DA.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' and college_code=" + collegeCode + "", "Text");
            }
            else if (linkvalue == "1")
            {
                string year = getYearForSem(Sem);
                dsFeecat = DA.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + year + " Year' and college_code=" + collegeCode + "", "Text");
            }
            else
            {
                dsFeecat = DA.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = 'Term " + Sem + "' and college_code=" + collegeCode + "", "Text");
            }
            if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
            {
                feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
                feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                feeCategory.Text = " ";
                feeCategory.Value = "-1";
            }
        }
        catch
        {
            feeCategory.Text = " ";
            feeCategory.Value = "-1";
        }
        return feeCategory;
    }
    public string getYearForSem(string val)
    {
        string value = "";
        if (val.Trim() == "1" || val.Trim() == "2")
        {
            value = "1";
        }
        if (val.Trim() == "3" || val.Trim() == "4")
        {
            value = "2";
        }
        if (val.Trim() == "5" || val.Trim() == "6")
        {
            value = "3";
        }
        if (val.Trim() == "7" || val.Trim() == "8")
        {
            value = "4";
        }
        if (val.Trim() == "9" || val.Trim() == "10")
        {
            value = "5";
        }
        return value;
    }
    //Roll No, Reg No, Admission No Rights
    private void RollRegAdmNoRights(ref bool rollNo, ref bool regNo, ref bool admNo)
    {
        try
        {
            string grouporusercode = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string Master1 = "select * from Master_Settings where " + grouporusercode + "";

            DataSet dsRRARights = DA.select_method_wo_parameter(Master1, "Text");
            if (dsRRARights.Tables.Count > 0 && dsRRARights.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsRRARights.Tables[0].Rows.Count; i++)
                {
                    if (dsRRARights.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsRRARights.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        rollNo = true;
                    }
                    if (dsRRARights.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsRRARights.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        regNo = true;
                    }

                    if (dsRRARights.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && dsRRARights.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        admNo = true;
                    }
                }
            }
        }
        catch { }
    }
    private void SetUniqueId(GridView grid)
    {
        bool rollNo = false, regNo = false, admNo = false;
        RollRegAdmNoRights(ref rollNo, ref regNo, ref admNo);

        if (!rollNo)
        {
            grid.Columns[7].Visible = false;
        }

        if (regNo)
        {
            grid.Columns[6].Visible = false;
        }

        if (admNo)
        {
            grid.Columns[5].Visible = false;
        }
    }
    //Common Methods
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch (Exception ex) { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetSelectedItemsTextList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Text);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }
    //Last modified 16-12-2016

    protected void lnkdownlaodattachement_Click(object sender, EventArgs e)
    {
        try
        {



            string RePks = txtPopReqPk.Text.Trim();


            byte[] bytes = null;
            string fileName = RePks;
            string contentType = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select attachments,contenttype from SR_Student_Leave_Request where LeaveRequestPK='" + RePks + "' ";

                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {

                            bytes = (byte[])sdr["attachments"];
                            contentType = sdr["ContentType"].ToString();

                        }
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; fileName=" + fileName);
            Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception ex)
        {
            string exx = ex.ToString();
        }
    }
}