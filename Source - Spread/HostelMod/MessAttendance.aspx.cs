using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Configuration;

public partial class MessAttendance : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string hostelcode = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string query = "";
    static string Hostelcode = "";
    static string studentcardtype = "";
    static string sessionname = "";
    static string sessionfk = ""; string q1 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (studentcardtype.Trim() == "")
        {
            studentcardtype = Convert.ToString(ddl_studtype.SelectedItem.Value);
        }
        studentcardtype = Convert.ToString(ddl_studtype.SelectedItem.Value);
        textboxvisiblefalse_ddl();
        //if (studentcardtype.Trim() == "0")
        //{
        //    txt_rollno.TextMode = TextBoxMode.Password;
        //}
        //else { txt_rollno.TextMode = TextBoxMode.SingleLine; }
        if (ddl_Messname.Items.Count > 0)
            Hostelcode = ddl_Messname.SelectedItem.Value;
        if (ddl_session.Items.Count > 0)
        {
            sessionfk = Convert.ToString(ddl_session.SelectedItem.Value);
            sessionname = Convert.ToString(ddl_session.SelectedItem.Text);
        }
        //lbltime.Text = DateTime.Now.ToString("dddd/MMMM/yyyy h:mm:ss tt");
        if (!IsPostBack)
        {
            bindhostelname();
            bindsession();
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbl_studimage.ImageUrl = "../images/dummyimg.png";
            CalendarExtender1.EndDate = DateTime.Now;
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch { }
    }
    protected void bindsession()
    {
        try
        {
            if (ddl_Messname.Items.Count > 0)
            {
                string deptquery = "select  SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + Convert.ToString(ddl_Messname.SelectedItem.Value) + "') order by SessionMasterPK ";
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                ddl_session.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_session.DataSource = ds;
                    ddl_session.DataTextField = "SessionName";
                    ddl_session.DataValueField = "SessionMasterPK";
                    ddl_session.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void bindhostelname()
    {
        try
        {
            ds.Clear();
            ds = d2.Bindmess_basedonrights(Session["usercode"].ToString(), Session["collegecode"].ToString());
            ddl_Messname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Messname.DataSource = ds;
                ddl_Messname.DataTextField = "MessName";
                ddl_Messname.DataValueField = "MessMasterPK";
                ddl_Messname.DataBind();
                Hostelcode = ddl_Messname.SelectedItem.Value;
            }
            else
            {
                alertmessage.Visible = true;
                lbl_alerterror.Text = "Please Set Mess Rights";
            }
            bindsession();
        }
        catch
        {
        }
    }
    protected void ddl_studtype_selectedindexchanged(object sender, EventArgs e)
    {
        studentcardtype = Convert.ToString(ddl_studtype.SelectedItem.Value);
        txt_rollno.Text = "";
        textboxvisiblefalse_ddl();
    }
    public void ddl_Messname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsession();
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        DateTime date = System.DateTime.Now;
        string scheduledate = Convert.ToString(date.ToString("dddd"));
        string mess_session = contextKey;
        string[] mess_sess = mess_session.Split('-');
        studentcardtype = Convert.ToString(mess_sess[2]);
        if (mess_sess.Length > 1)
        {
            query = "select mess_attendance_set from HostelIns_settings where Schedule_date='" + date.ToString("MM/dd/yyyy") + "' and Session_code='" + Convert.ToString(mess_sess[1]) + "' and Hostel_code='" + Convert.ToString(mess_sess[0]) + "' ";
            name = ws.Getname(query);
            if (name.Count == 0)
            {
                query = "select mess_attendance_set from HostelIns_settings where Schedule_Day='" + scheduledate + "' and Session_code='" + Convert.ToString(mess_sess[1]) + "' and Hostel_code='" + Convert.ToString(mess_sess[0]) + "'";
                name = ws.Getname(query);
            }
        }
        string mess_settings = (name[0].ToString());
        Hostelcode = "select HostelMasterPK,HostelName from HM_HostelMaster where MessMasterFK in (" + Convert.ToString(mess_sess[0]) + ")";
        //if (studentcardtype.Trim() == "0")
        //{
        //    query = " select '' bb ";
        //}
        //else
        if (studentcardtype.Trim() == "1")
        {
            if (mess_settings.Contains("H,D"))
            {
                query = " select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and roll_no like '" + prefixText + "%' ";
            }
            else if (mess_settings.Contains("H"))
            {
                query = " select R.Roll_No from Registration r,HT_HostelRegistration h where r.App_No =h.APP_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterFK in ('" + Hostelcode + "') and R.roll_no like '" + prefixText + "%'";
            }
            else if (mess_settings.Contains("D"))
            {
                query = " select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Type='Day Scholar'";
            }
        }
        else if (studentcardtype.Trim() == "2")
        {
            if (mess_settings.Contains("H,D"))
            {
                query = " select reg_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and reg_no like '" + prefixText + "%' ";
            }
            else if (mess_settings.Contains("H"))
            {
                query = " select R.reg_no from Registration r,HT_HostelRegistration h where r.App_No =h.APP_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterFK in ('" + Hostelcode + "') and R.reg_no like '" + prefixText + "%'";
            }
            else if (mess_settings.Contains("D"))
            {
                query = " select reg_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Type='Day Scholar' and R.reg_no like '" + prefixText + "%'";
            }
        }
        else if (studentcardtype.Trim() == "3")
        {
            if (mess_settings.Contains("H,D"))
            {
                query = " select Stud_Name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Name like '" + prefixText + "%' ";
            }
            else if (mess_settings.Contains("H"))
            {
                query = " select R.Stud_Name from Registration r,HT_HostelRegistration h where r.App_No =h.APP_No and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and h.HostelMasterFK in ('" + Hostelcode + "') and R.Stud_Name like '" + prefixText + "%'";
            }
            else if (mess_settings.Contains("D"))
            {
                query = " select Stud_Name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Type='Day Scholar' and R.Stud_Name like '" + prefixText + "%'";
            }
        }
        else if (studentcardtype.Trim() == "4")
        {
            query = " select s.staff_Code as roll_no from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.staff_Code like '" + prefixText + "%' ";
        }
        else if (studentcardtype.Trim() == "5")
        {
            query = " select s.staff_name as Stud_Name from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%' ";
        }
        if (query.Trim() != "")
        {
            name.Clear();
            name = ws.Getname(query);
        }
        else { }
        return name;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertmessage.Visible = false;
    }
    [WebMethod]
    public static List<string> studroll(string Smart_No, string j)
    {
        string data = string.Empty;
        List<string> details = new List<string>();
        try
        {
            if (Smart_No.Trim() != "")
            {
                DataSet ds = new DataSet(); DataSet ds1 = new DataSet();
                DAccess2 DA = new DAccess2();
                string[] passedvalue = j.Split('-');
                studentcardtype = Convert.ToString(passedvalue[4]);
                string stud_condition = ""; string staff_condition = "";
                //if (studentcardtype.Trim() == "0")
                //{
                //    stud_condition = " and smart_serial_no='" + Smart_No + "'";
                //    staff_condition = " and Smartcard_serial_no='" + Smart_No + "'";
                //}
                //else
                if (studentcardtype.Trim() == "1")
                {
                    stud_condition = " and Roll_No='" + Smart_No + "'";
                }
                else if (studentcardtype.Trim() == "2")
                {
                    stud_condition = " and Reg_No='" + Smart_No + "'";
                }
                else if (studentcardtype.Trim() == "3")
                {
                    stud_condition = " and Stud_Name='" + Smart_No + "'";
                }
                else if (studentcardtype.Trim() == "4")
                {
                    staff_condition = " and s.staff_Code='" + Smart_No + "'";
                }
                else if (studentcardtype.Trim() == "5")
                {
                    staff_condition = " and s.staff_name='" + Smart_No + "'";
                }

                DateTime date = new DateTime();
                string[] split = Convert.ToString(passedvalue[3]).Split('/');
                date = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string scheduledate = Convert.ToString(date.ToString("dddd"));
                string messatt_settings = "";

                messatt_settings = DA.GetFunction("select mess_attendance_set from HostelIns_settings where Schedule_date='" + date.ToString("MM/dd/yyyy") + "' and Session_code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_code='" + Convert.ToString(passedvalue[2]) + "'");
                if (messatt_settings.Trim() == "0")
                    messatt_settings = DA.GetFunction("select mess_attendance_set from HostelIns_settings where Schedule_Day='" + scheduledate + "' and Session_code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_code='" + Convert.ToString(passedvalue[2]) + "'");
                string isstaff = "0";
                System.Web.UI.WebControls.Image lbl_studimage = new System.Web.UI.WebControls.Image();
                lbl_studimage.ImageUrl = "../images/dummyimg.png"; lbl_studimage.Visible = true;
                if (messatt_settings.Length > 0)
                {
                    string[] messatt_set = messatt_settings.Split(',');
                    string q1 = "";
                    if (stud_condition.Trim() != "" || staff_condition.Trim() != "")
                    {
                        ds.Clear();
                        if (messatt_set.Contains("S"))
                        {
                            if (staff_condition.Trim() != "")
                            {
                                q1 = " select sa.appl_id as app_no,s.staff_Code as roll_no,s.staff_name as Stud_Name,desig_name as batch,sa.staff_type as Stud_Type,s.college_code from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 " + staff_condition + "";
                                isstaff = "1";
                                ds = DA.select_method_wo_parameter(q1, "Text");
                            }
                            else
                            {
                                q1 = " select r.app_no,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + "";
                                ds = DA.select_method_wo_parameter(q1, "Text"); isstaff = "0";
                            }
                        }
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            if (stud_condition.Trim() != "")
                            {
                                q1 = " select r.app_no,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + "";
                                ds = DA.select_method_wo_parameter(q1, "Text"); isstaff = "0";
                            }
                        }
                        q1 = " select roll_no,Entry_Date,Session_Code,Hostel_Code,College_Code from HostelMess_Attendance ";
                        ds1.Clear();
                        ds1 = DA.select_method_wo_parameter(q1, "Text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string rollno = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                            details.Add(rollno);
                            details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]));
                            details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]));
                            details.Add(Convert.ToString(ds.Tables[0].Rows[0]["batch"]));

                            lbl_studimage.Visible = true; string type = "";
                            if (isstaff == "1")
                            {
                                type = "1";
                                lbl_studimage.ImageUrl = "../Handler/staffphoto.ashx?staff_code=" + rollno;
                            }
                            else
                            {
                                type = "0";
                                lbl_studimage.ImageUrl = "../Handler/Handler4.ashx?rollno=" + rollno;
                            }
                            details.Add(Convert.ToString(lbl_studimage.ImageUrl));

                            string app_no = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                            string clgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                            DataView alreadyregstud = new DataView();
                            ds1.Tables[0].DefaultView.RowFilter = "Entry_Date='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and Session_Code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_Code='" + Convert.ToString(passedvalue[2]) + "' and College_Code='" + clgcode + "' and Roll_No='" + rollno + "'  ";
                            alreadyregstud = ds1.Tables[0].DefaultView;
                            if (alreadyregstud.Count == 0)
                            {
                                if (app_no.Trim() != "")
                                {
                                    q1 = "";
                                    q1 = "insert into HostelMess_Attendance (roll_no,entry_date,Entry_time,session_name,session_code, is_staff, Hostel_code,college_code,app_no)values('" + rollno + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + Convert.ToString(passedvalue[0]) + "','" + Convert.ToString(passedvalue[1]) + "','" + isstaff + "','" + Convert.ToString(passedvalue[2]) + "','" + clgcode + "','" + app_no + "')";
                                    int up = DA.update_method_wo_parameter(q1, "text");
                                    if (up != 0)
                                    {
                                        details.Add("1");
                                        details.Add(type);
                                    }
                                }
                            }
                            else
                            {
                                details.Clear();
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add(""); lbl_studimage.ImageUrl = "../images/dummyimg.png";
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                                details.Add("2");
                                details.Add("");
                            }
                        }
                        else
                        {
                            details.Clear();
                            details.Add("");
                            details.Add("");
                            details.Add("");
                            details.Add("");
                            details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                            details.Add("0"); details.Add("");
                        }
                    }
                    else
                    {
                        details.Clear();
                        details.Add("");
                        details.Add("");
                        details.Add("");
                        details.Add("");
                        details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                        details.Add("0"); details.Add("");
                    }
                }
            }
            return details;
        }
        catch
        {
            details.Clear();
            details.Add("");
            details.Add("");
            details.Add("");
            details.Add(""); details.Add("");
            details.Add("0");
            return details;
        }
    }

    [WebMethod]
    public static List<string> studsmartno(string Smart_No, string j)
    {
        string data = string.Empty;
        List<string> details = new List<string>();
        try
        {
            if (Smart_No.Trim() != "" && Smart_No.Length >= 10)
            {
                DataSet ds = new DataSet(); DataSet ds1 = new DataSet();
                DAccess2 DA = new DAccess2();
                string[] passedvalue = j.Split('-');
                studentcardtype = Convert.ToString(passedvalue[4]);
                string stud_condition = ""; string staff_condition = "";
                if (studentcardtype.Trim() == "0")
                {
                    stud_condition = " and smart_serial_no='" + Smart_No + "'";
                    staff_condition = " and Smartcard_serial_no='" + Smart_No + "'";

                    DateTime date = new DateTime();
                    string[] split = Convert.ToString(passedvalue[3]).Split('/');
                    date = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    string scheduledate = Convert.ToString(date.ToString("dddd"));
                    string messatt_settings = "";

                    messatt_settings = DA.GetFunction("select mess_attendance_set from HostelIns_settings where Schedule_date='" + date.ToString("MM/dd/yyyy") + "' and Session_code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_code='" + Convert.ToString(passedvalue[2]) + "'");
                    if (messatt_settings.Trim() == "0")
                        messatt_settings = DA.GetFunction("select mess_attendance_set from HostelIns_settings where Schedule_Day='" + scheduledate + "' and Session_code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_code='" + Convert.ToString(passedvalue[2]) + "'");
                    string isstaff = "0";
                    System.Web.UI.WebControls.Image lbl_studimage = new System.Web.UI.WebControls.Image();
                    lbl_studimage.ImageUrl = "../images/dummyimg.png"; lbl_studimage.Visible = true;
                    if (messatt_settings.Length > 0)
                    {
                        string[] messatt_set = messatt_settings.Split(',');
                        string q1 = "";
                        if (stud_condition.Trim() != "" || staff_condition.Trim() != "")
                        {
                            ds.Clear();
                            if (messatt_set.Contains("S"))
                            {
                                if (staff_condition.Trim() != "")
                                {
                                    q1 = " select sa.appl_id as app_no,s.staff_Code as roll_no,s.staff_name as Stud_Name,desig_name as batch,sa.staff_type as Stud_Type,s.college_code from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 " + staff_condition + "";
                                    isstaff = "1";
                                    ds = DA.select_method_wo_parameter(q1, "Text");
                                }
                                else
                                {
                                    q1 = " select r.app_no,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + "";
                                    ds = DA.select_method_wo_parameter(q1, "Text"); isstaff = "0";
                                }
                            }
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                if (stud_condition.Trim() != "")
                                {
                                    q1 = " select r.app_no,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + "";
                                    ds = DA.select_method_wo_parameter(q1, "Text"); isstaff = "0";
                                }
                            }
                            q1 = " select roll_no,Entry_Date,Session_Code,Hostel_Code,College_Code from HostelMess_Attendance ";
                            ds1.Clear();
                            ds1 = DA.select_method_wo_parameter(q1, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string rollno = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                                details.Add(rollno);
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["batch"]));

                                lbl_studimage.Visible = true; string type = "";
                                if (isstaff == "1")
                                {
                                    type = "1";
                                    lbl_studimage.ImageUrl = "../Handler/staffphoto.ashx?staff_code=" + rollno;
                                }
                                else
                                {
                                    type = "0";
                                    lbl_studimage.ImageUrl = "../Handler/Handler4.ashx?rollno=" + rollno;
                                }
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));

                                string app_no = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                                string clgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                                DataView alreadyregstud = new DataView();
                                ds1.Tables[0].DefaultView.RowFilter = "Entry_Date='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and Session_Code='" + Convert.ToString(passedvalue[1]) + "' and Hostel_Code='" + Convert.ToString(passedvalue[2]) + "' and College_Code='" + clgcode + "' and Roll_No='" + rollno + "'  ";
                                alreadyregstud = ds1.Tables[0].DefaultView;
                                if (alreadyregstud.Count == 0)
                                {
                                    if (app_no.Trim() != "")
                                    {
                                        q1 = "";
                                        q1 = "insert into HostelMess_Attendance (roll_no,entry_date,Entry_time,session_name,session_code, is_staff, Hostel_code,college_code,app_no)values('" + rollno + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + Convert.ToString(passedvalue[0]) + "','" + Convert.ToString(passedvalue[1]) + "','" + isstaff + "','" + Convert.ToString(passedvalue[2]) + "','" + clgcode + "','" + app_no + "')";
                                        int up = DA.update_method_wo_parameter(q1, "text");
                                        if (up != 0)
                                        {
                                            details.Add("1");
                                            details.Add(type);
                                        }
                                    }
                                }
                                else
                                {
                                    details.Clear();
                                    details.Add("");
                                    details.Add("");
                                    details.Add("");
                                    details.Add(""); lbl_studimage.ImageUrl = "../images/dummyimg.png";
                                    details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                                    details.Add("2");
                                    details.Add("");
                                }
                            }
                            else
                            {
                                details.Clear();
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                                details.Add("0"); details.Add("");
                            }
                        }
                        else
                        {
                            details.Clear();
                            details.Add("");
                            details.Add("");
                            details.Add("");
                            details.Add("");
                            details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                            details.Add("0"); details.Add("");
                        }
                    }
                }
            }
            return details;
        }
        catch
        {
            details.Clear();
            details.Add("");
            details.Add("");
            details.Add("");
            details.Add(""); details.Add("");
            details.Add("0");
            return details;
        }
    }

    public void textboxvisiblefalse_ddl()
    {
        if (studentcardtype.Trim() == "0")
        {
            txt_smartno.Visible = true; txt_rollno.Visible = false;
        }
        else
        {
            txt_smartno.Visible = false; txt_rollno.Visible = true;
        }
    }
}