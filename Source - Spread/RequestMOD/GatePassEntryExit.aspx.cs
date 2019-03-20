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
using System.Drawing;
using Gios.Pdf;
using InsproDataAccess;
using System.Text;
using System.Web;
using System.Configuration;
public partial class GatePassEntryExit : System.Web.UI.Page
    {
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    static string newroute = "";
    int menutype = 0;
    static string gatepassrights = "";
    string btntype = "";
    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    //static string dat = txt_date.Text.ToString();
    //System.DateTime.Now.ToString("yyyy/MM/dd");
    static string user_id = "";
    static string SenderID = "";
    static string Password = "";
    static string strmsg = "";
    static string mobilenos = "";
    static string isst = "";
    static string sms_mom = "";
    static string sms_dad = "";
    static string sms_stud = "";
    static string sms_req = "";
    static string sms_app = "";
    static string sms_exit = "";
    static string gatepasspk = "";
    static string collegecodeee = "";
    static string vendorcompanyname = "";
    static string vendorcompanyname1 = "";
    static string vendorname1 = "";
    static string vendormobname1 = "";
    static string Groupcode = "";
    static string UserCode = "";
    static string inroll = "";
    static string incode = "";
    static string colg = "";
   static string deptp="";
   static string roollstu = "";
   static string deptar = string.Empty;
   static string che_inout = string.Empty;
   static string che_coimp = string.Empty;
   static string deviceid = string.Empty;
   static int sec = 1;
    Hashtable hat = new Hashtable();
    static Hashtable Hasnew = new Hashtable();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    string fromdate = "";
    string todate = "";
    Font oFont = new Font("IDAutomationHC39M", 16);
    PointF point = new PointF(2f, 2f);
    SolidBrush blackBrush = new SolidBrush(Color.Black);
    SolidBrush whiteBrush = new SolidBrush(Color.White);
  static  string right = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

           
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            colg = collegecode1;
            collegecodeee = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Groupcode = Session["group_code"].ToString();
            UserCode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            // Page.SetFocus(txt_smart);
            Hasnew.Clear();
            txt_apdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_aptime.Text = DateTime.Now.ToLongTimeString();
            txt_expdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_exptime.Text = DateTime.Now.ToLongTimeString();
            if (rb_in.Checked == true)
                right = "In";
            if (rb_out.Checked == true)
                right = "out";
            if (!IsPostBack)
            {
                lblUserCode.Text = Session["usercode"].ToString();
                Hidden3.Value = lblUserCode.Text;
                access1();
                access();
                access2();
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txt_time.Attributes.Add("readonly", "readonly");
                txt_apdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_aptime.Text = DateTime.Now.ToLongTimeString();
                // txt_time.Text = DateTime.Now.ToLongTimeString();
                // rb_out.Checked = true;
                txt_expdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_exptime.Text = DateTime.Now.ToLongTimeString();
                txt_expdate.Attributes.Add("readonly", "readonly");
                txt_exptime.Attributes.Add("readonly", "readonly");
                txt_staff_time.Text = DateTime.Now.ToLongTimeString();
                txt_staff_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_staff_date.Attributes.Add("readonly", "readonly");
                txt_staff_time.Attributes.Add("readonly", "readonly");
                txt_staff_exp.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_staff_exp.Attributes.Add("readonly", "readonly");
                txt_staff_exptime.Text = DateTime.Now.ToLongTimeString();
                txt_staff_exptime.Attributes.Add("readonly", "readonly");
                txt_expcttime.Text = DateTime.Now.ToLongTimeString();
                txt_expctdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_expctdate.Attributes.Add("readonly", "readonly");
                txt_apstaff.Attributes.Add("readonly", "readonly");
                txt_apdate.Attributes.Add("readonly", "readonly");
                txt_aptime.Attributes.Add("readonly", "readonly");
                txt_staffappdate.Attributes.Add("readonly", "readonly");
                txt_staffapptime.Attributes.Add("readonly", "readonly");
                txt_staffapptime.Text = DateTime.Now.ToLongTimeString();
                txt_staffappdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_partime.Text = DateTime.Now.ToLongTimeString();
                txt_pardate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_pardate.Attributes.Add("readonly", "readonly");
                txt_partime.Attributes.Add("readonly", "readonly");
                txt_visittime.Text = DateTime.Now.ToLongTimeString();
                txt_visitdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_visitdate.Attributes.Add("readonly", "readonly");
                txt_visittime.Attributes.Add("readonly", "readonly");
                txt_materialtime.Text = DateTime.Now.ToLongTimeString();
                txt_materialdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_materialdate.Attributes.Add("readonly", "readonly");
                txt_materialtime.Attributes.Add("readonly", "readonly");
                txt_vehicletime.Text = DateTime.Now.ToLongTimeString();
                txt_vehicledate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_vehicledate.Attributes.Add("readonly", "readonly");
                txt_vehicletime.Attributes.Add("readonly", "readonly");
                txt_studtype.Attributes.Add("readonly", "readonly");
                txt_degree.Attributes.Add("readonly", "readonly");
                txt_dept.Attributes.Add("readonly", "readonly");
                txt_sem.Attributes.Add("readonly", "readonly");
                txt_sec.Attributes.Add("readonly", "readonly");
                txt_apdept.Attributes.Add("readonly", "readonly");
                txt_apdesgn.Attributes.Add("readonly", "readonly");
                txt_drivername.Attributes.Add("readonly", "readonly");
                txt_mobile.Attributes.Add("readonly", "readonly");
                txt_route.Attributes.Add("readonly", "readonly");
                txt_desg.Attributes.Add("readonly", "readonly");
                txt_staffdept.Attributes.Add("readonly", "readonly");
                txt_staff_type.Attributes.Add("readonly", "readonly");
                txt_pdegree.Attributes.Add("readonly", "readonly");
                txt_dept1.Attributes.Add("readonly", "readonly");
                txt_sem1.Attributes.Add("readonly", "readonly");
                txt_section.Attributes.Add("readonly", "readonly");
                txt_studtype1.Attributes.Add("readonly", "readonly");
                txt_dpt.Attributes.Add("readonly", "readonly");
                txt_desgtn.Attributes.Add("readonly", "readonly");
                txt_meetstaffdept.Attributes.Add("readonly", "readonly");
                txt_meetstaffdesg.Attributes.Add("readonly", "readonly");
                txt_dpt1.Attributes.Add("readonly", "readonly");
                txt_desg1.Attributes.Add("readonly", "readonly");
                txt_type.Attributes.Add("readonly", "readonly");
                mblno.Attributes.Add("readonly", "readonly");
                //txt_str.Attributes.Add("readonly", "readonly");
                //txt_cty.Attributes.Add("readonly", "readonly");
                //txt_dis.Attributes.Add("readonly", "readonly");
                //txt_phno.Attributes.Add("readonly", "readonly");
                //txt_mno.Attributes.Add("readonly", "readonly");
                txt_suppliername.Attributes.Add("readonly", "readonly");
                txt_addr.Attributes.Add("readonly", "readonly");
                // txt_street1.Attributes.Add("readonly", "readonly");
                txt_city1.Attributes.Add("readonly", "readonly");
                txt_dist.Attributes.Add("readonly", "readonly");
                txt_state1.Attributes.Add("readonly", "readonly");
                txt_contperson.Attributes.Add("readonly", "readonly");
                txt_name3.Attributes.Add("readonly", "readonly");
                txt_mobileno.Attributes.Add("readonly", "readonly");
                txt_measure.Attributes.Add("readonly", "readonly");
                txt_rut.Attributes.Add("readonly", "readonly");
                txt_driver1.Attributes.Add("readonly", "readonly");
                txt_licstatus.Attributes.Add("readonly", "readonly");
                txt_insurstatus.Attributes.Add("readonly", "readonly");
                txt_fcstatus.Attributes.Add("readonly", "readonly");
                txt_depart.Attributes.Add("readonly", "readonly");
                txt_design.Attributes.Add("readonly", "readonly");
                txt_staff_route.Attributes.Add("readonly", "readonly");
                txt_staff_drvname.Attributes.Add("readonly", "readonly");
                txt_staff_route.Attributes.Add("readonly", "readonly");
                txt_staff_mob.Attributes.Add("readonly", "readonly");
                inroll = "";
                loadtimes();
                user_id = d2.GetFunction("select SMS_User_ID from Track_Value where college_code='" + collegecodeee + "'");
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {
                    SenderID = spret[0].ToString();
                    Password = spret[1].ToString();
                    Session["api"] = user_id;
                    Session["senderid"] = SenderID;
                }
                //Added By SaranyaDevi 4.2.2018
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                Newitem();
                TextBox1.Text = hid.Value;
                servertime();
                string timercon = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='gatepass biobased' and user_code ='" + usercode + "' and college_code ='" + collegecodeee + "'");
                if (timercon == "0")
                    tmrTTStat.Enabled = true;
                if (timercon == "1")
                {
                    sec = 0;
                    tmrTTStat.Enabled = false;
                }
                deviceid = d2.GetFunction("select MachineNo from DeviceInfo where DeviceForHostel='3'");
            }
        }
        catch (Exception ex) { }
    }
    public void loadtimes()
    {
        int i;
        for (i = 1; i <= 12; i++)
        {
            ddl_hrs.Items.Add(i.ToString());
        }
        for (i = 0; i < 60; i++)
        {
            if (i < 10)
                ddl_mins.Items.Add("0" + i.ToString());
            else if (i >= 10)
                ddl_mins.Items.Add(i.ToString());
        }
        ddl_ampm.Items.Add("AM");
        ddl_ampm.Items.Add("PM");
    }
    protected void lb_logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void imgbtn_staff_Click(object sender, EventArgs e)
    {
        //div_student.Visible = false;
        //div_staff.Visible = true;
        ////div_studokclear.Visible = true;
        //div_parent.Visible = false;
        //div_material.Visible = false;
        // div_visitor.Visible = false;
        // div_vehicle.Visible = false;
    }
    protected void imgbtn_parents_Click(object sender, EventArgs e)
    {
        // div_parent.Visible = true;
        //div_studokclear.Visible = true;
        //div_staff.Visible = false;
        //div_student.Visible = false;
        //div_meetstaff.Visible = false;
        //div_meetoffice.Visible = true;
        //div_meetothers.Visible = false;
        //div_notadm_stud.Visible = false;
        //div_material.Visible = false;
        //div_visitor.Visible = false;
        //div_vehicle.Visible = false;
        //div_adm_stud.Visible = true;
    }
    protected void imgbtn_visitor_Click(object sender, EventArgs e)
    {
        //div_student.Visible = false;
        //div_staff.Visible = false;
        //div_parent.Visible = false;
        //div_studokclear.Visible = true;
        //div_material.Visible = false;
        // div_visitor.Visible = true;
        //div_vehyes.Visible = false;
        //div_withoutappoint.Visible = false;
        // div_vehicle.Visible = false;
    }
    protected void imgbtn_material_Click(object sender, EventArgs e)
    {
        //div_student.Visible = false;
        //div_staff.Visible = false;
        // div_parent.Visible = false;
        ////div_studokclear.Visible = true;
        //div_material.Visible = true;
        //div_metr_entryby.Visible = true;
        //div_metr_others.Visible = false;
        //div_visitor.Visible = false;
        // div_vehicle.Visible = false;
        //div_ordermaterial.Visible = true;
        //div_material_others.Visible = false;
    }
    protected void imgbtn_vehicle_Click(object sender, EventArgs e)
    {
        //div_student.Visible = false;
        //div_staff.Visible = false;
        // div_parent.Visible = false;
        ////div_studokclear.Visible = true;
        //div_material.Visible = false;
        //  div_visitor.Visible = false;
        //div_vehicle.Visible = true;
        //appstatus_yes.Visible = true;
        //div_othervehicle.Visible = false;
    }
    protected void rb_own_CheckedChanged(object sender, EventArgs e)
    {
        //div_entryexit.Visible = false;
    }
    protected void rb_inst_CheckedChanged(object sender, EventArgs e)
    {
        //div_entryexit.Visible = true;
    }
    protected void rb_vehother_CheckedChanged(object sender, EventArgs e)
    {
    }
    protected void txt_date_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txt_apdate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txt_expdate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txt_time_TextChanged(object sender, EventArgs e)
    {
    }
    public void txt_satffdate_TextChanged(object sender, EventArgs e)
    {
    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["btntype"] = "1";
            div_student.Attributes.Add("style", "display:block");
            div_staff.Attributes.Add("style", "display: none");
            div_parent.Attributes.Add("style", "display:none");
            div_visitor.Attributes.Add("style", "display:none");
            div_material.Attributes.Add("style", "display:none");
            div_vehicle.Attributes.Add("style", "display:none");
            menutype = 1;
            string sql = "";
            int gatetype;
            int approval = 0;
            int byvehicle = 1;
            int clgvehicle = 1;
            DateTime gatepasdate = new DateTime();
            gatepasdate = TextToDate(txt_date);
            DateTime gatepassexitdate = new DateTime();
            gatepassexitdate = TextToDate(txt_apdate);
            string currentdate = Convert.ToString(txt_date.Text);
            DateTime expdate = new DateTime();
            expdate = TextToDate(txt_expctdate);
            int query = 0;
            int query1 = 0;
            string gatepastime = txt_time.Text;
            string exptime = txt_exptime.Text;
            string appcode = d2.GetFunction("select GatepassApproval_Code from GatePass_Approval where college_code Roll_No='" + txt_rollno.Text + "' and ApprovedDate_Exit='" + gatepasdate + "'");
            string appno = d2.GetFunction("select r.App_No from applyn a,Registration r where a.college_code='" + collegecode1 + "' and r.college_code='" + collegecode1 + "' and a.app_no=r.App_No and r.Roll_No='" + txt_rollno.Text + "'");
            string purpose = txt_purpose1.Text;
            string vehid = "";
            string vehtype = "";
            string vehregno = "";
            string mobno = "";
            string collegecode = collegecode1;
            if (rb_in.Checked == true)
            {
                gatetype = 0;
            }
            else
            {
                gatetype = 1;
            }
            if (rb_otherveh.Checked == true)
            {
                byvehicle = 1;
                clgvehicle = 1;
                vehid = "";
                vehtype = "";
                vehregno = "";
                mobno = "";
            }
            else if (rb_own.Checked == true)
            {
                byvehicle = 0;
                clgvehicle = 1;
                vehid = "";
                vehtype = "";
                vehregno = txt_ownvehno.Text;
                mobno = txt_ownmob.Text;
            }
            else if (rb_inst.Checked == true)
            {
                byvehicle = 0;
                clgvehicle = 0;
                vehid = txt_vehicleno.Text;
                vehtype = d2.GetFunction("select Veh_Type from Vehicle_Master where Veh_ID='" + vehid + "'");
                vehregno = d2.GetFunction("select Reg_No from Vehicle_Master where Veh_ID='" + vehid + "'");
                mobno = d2.GetFunction("select Mobile_No from DriverAllotment where Vehicle_Id='" + vehid + "'");
            }
            string appgateentrytime = string.Empty;
            string lateentry = string.Empty;
            string appgateexittime = string.Empty; ;
            string appgateexitdate = string.Empty;
            string exitdate = string.Empty;
            if (rb_in.Checked == true)
            {
                appgateexitdate = d2.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                appgateentrytime = d2.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");

                lateentry = d2.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + gatepasdate + "' and GateReqEntryTime>='" + gatepastime + "'");
                exitdate = d2.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
            }
            //string appgateexitdate = d2.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
            if (rb_out.Checked == true)
            {
                appgateexitdate = d2.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                appgateentrytime = d2.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                exitdate = d2.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
            }
            string[] split = gatepastime.Split(':');
            string hr = split[0];
            string min = split[1];
            string day = split[2];
            int chr = Convert.ToInt32(hr);
            int cmin = Convert.ToInt32(min);
            string[] split1 = appgateentrytime.Split(':');
            string hr1 = split1[0];
            string min1 = split1[1];
            string day1 = split1[2];
            int chr1 = Convert.ToInt32(hr1);
            int cmin1 = Convert.ToInt32(min1);
            string islate = "";
            if (txt_rollno.Text != "")
            {
                approval = 1;
                if (appgateexitdate == currentdate)
                {
                    string pk = d2.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                    if (rb_out.Checked == true)
                    {
                        //string timecheck = d2.GetFunction("select count(GateReqExitTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqExitTime<'" + gatepastime + "'");
                        string timecheck = d2.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  RequisitionPK='" + gatepasspk + "'");
                        string[] exittime = timecheck.Split(':');
                        string gateexithr = exittime[0];
                        string gateexitmin = exittime[1];
                        string gateexitampm = exittime[2];
                        if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                        {
                            if (chr != 12)
                            {
                                imgdiv2.Visible = true;
                                lbl_erroralert.Text = "You Cannot Exit On The Hostel Befor of Exit Time ";
                                return;
                            }
                            else if (chr == 12 && cmin > Convert.ToInt32(gateexithr))
                            {
                                imgdiv2.Visible = true;
                                lbl_erroralert.Text = "You Cannot Exit On The Hostel Befor of Exit Time ";
                                return;
                            }
                        }
                        else if (chr < Convert.ToInt32(gateexithr))
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "You Cannot Exit On The Hostel Befor of Exit Time ";
                            return;
                        }
                        string timecheckexit = d2.GetFunction("select count(GateReqEntryTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqEntryTime>='" + gatepastime + "'");
                        string reqnumb = d2.GetFunction("select RequisitionPK from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqEntryTime>='" + gatepastime + "'");
                        if (timecheckexit == "0" || timecheckexit == "")
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "You Cannot Exit On The Hostel";
                            return;
                        }
                        else
                        {
                            if (pk == "0" || pk == "")
                            {
                                sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,Purpose,ByVehcile,IsCollVeh,VehType,VehId,VehRegNo,College_Code,islate,RequestFk) values('" + menutype + "','" + gatetype + "','" + gatepasdate + "','" + gatepasdate + "','" + gatepastime + "','" + appno + "','" + approval + "','" + appcode + "','" + expdate + "','" + exptime + "','" + purpose + "','" + byvehicle + "','" + clgvehicle + "', '" + vehtype + "','" + vehid + "','" + vehregno + "','" + collegecode + "','0','" + gatepasspk + "')";
                                query = d2.update_method_wo_parameter(sql, "TEXT");
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_erroralert.Text = "Already Exit";
                                return;
                            }
                        }
                    }
                    else if (rb_in.Checked == true)
                    {
                        string outcheck = d2.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                        if (outcheck == "0" || outcheck == "False")
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Student Doesn't Exist On Hostel ";
                            return;
                        }
                        if ((gatepastime == appgateentrytime) || (lateentry != "0"))
                        {
                            islate = "0";
                        }
                        else
                        {
                            islate = "1";
                        }
                        string[] split3 = exitdate.Split('/');
                        string day2 = split3[0];
                        string mo2 = split3[1];
                        string yea2 = split3[2];
                        string exitsdatess = yea2 + '-' + mo2 + '-' + day2;

                        sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + exitsdatess + "' and RequestFk='" + pk + "'";
                        query = d2.update_method_wo_parameter(sql, "TEXT");
                        sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + exitsdatess + "'";
                        query = d2.update_method_wo_parameter(sql, "TEXT");
                        imgdiv2.Visible = true;
                        panel_erroralert.Visible = true;
                        lbl_erroralert.Text = "Saved Successfully";
                    }
                }
                string getIDType = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateEntryExitID=((select max(GateEntryExitID) from GateEntryExit))");
                string memtyp = "1";
                string name = txt_apstaff.Text;
                string designation = txt_apdesgn.Text;
                string dept = txt_apdept.Text;
                string[] arr = name.Split('-');
                string staffcode = arr[1];
                //string staff_name = d2.GetFunction("select appl_no from staff_appl_master where appl_name='" + name + "'");
                //string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_code='" + staff_name + "'");
                if (rb_out.Checked == true)
                {
                    string detquery = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,Staff_Code) values('" + getIDType + "','" + memtyp + "','" + staffcode + "')";
                    query1 = d2.update_method_wo_parameter(detquery, "TEXT");
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Successfully";
                }
                if (sms_exit == "3")
                {
                    //sms();
                }
                if (query != 0)
                {
                    // div_student.Attributes.Add("style", "display:none");
                    txt_apdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_aptime.Text = DateTime.Now.ToLongTimeString();
                    // txt_time.Text = DateTime.Now.ToLongTimeString();
                    txt_expdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_exptime.Text = DateTime.Now.ToLongTimeString();
                    //imgdiv2.Visible = true;
                    //lbl_erroralert.Text = "Saved Successfully";
                }
            }
            div_student.Attributes.Add("style", "display:block");
            //funleave();

        }
        catch (Exception ex)
        {
        }
    }

    protected void Butngo_Click(object sender, EventArgs e)
    {
        try
        {
            funleave();
        }
        catch
        {
        }
    }
    protected void funleave()
    {
        try
        {
            div4.Visible = true;
            Fpspread2.Visible = true;
            Fpspread2.Sheets[0].AutoPostBack = true;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.Sheets[0].ColumnHeader.Visible = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.Font.Name = "Book Antiqua";
            MyStyle.Font.Bold = true;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.White;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            Fpspread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 5;
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.BorderWidth = 2;
            //Fpspread2.Sheets[0].AutoPostBack = true;
            //Fpspread2.Sheets[0].RowHeader.Visible = false;
            //Fpspread2.Sheets[0].ColumnHeader.Visible = false;
            //MyStyle.Font.Size = FontUnit.Medium;
            //MyStyle.Font.Name = "Book Antiqua";
            //MyStyle.Font.Bold = true;
            //MyStyle.HorizontalAlign = HorizontalAlign.Center;
            //MyStyle.ForeColor = Color.Blue;
            //MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //Fpspread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            //Fpspread2.CommandBar.Visible = false;
            //Fpspread2.Sheets[0].ColumnCount = 5;
            //Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "From Date";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "TO Date";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reason";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Status";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            string[] split = txt_fromdate.Text.Split('/');
            string day = split[0];
            string mo = split[1];
            string yea = split[2];
            string fromdates = yea + '-' + mo + '-' + day;
            string[] split1 = txt_todate.Text.Split('/');
            string day1 = split1[0];
            string mo1 = split1[1];
            string yea1 = split1[2];
            string todates = yea1 + '-' + mo1 + '-' + day1;
            if (txt_rollno.Text != "")
            {
                string leavetype = "select GatepassExitdate,GatepassEntrydate,Purpose from GateEntryExit where App_No =(select App_No from Registration where Roll_No='" + txt_rollno.Text + "') and GatepassExitdate<='" + fromdates + "' and GatepassEntrydate<='" + todates + "'";
                ds = d2.select_method_wo_parameter(leavetype, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                    {
                        Fpspread2.Rows.Count++;
                        Fpspread2.Sheets[0].Cells[m, 0].Text = Convert.ToString(m + 1);
                        Fpspread2.Sheets[0].Cells[m, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[m, 1].Text = Convert.ToString(ds.Tables[0].Rows[m]["GatepassExitdate"]);
                        Fpspread2.Sheets[0].Cells[m, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 1].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[m, 2].Text = Convert.ToString(ds.Tables[0].Rows[m]["GatepassEntrydate"]);
                        Fpspread2.Sheets[0].Cells[m, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 2].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[m, 3].Text = Convert.ToString(ds.Tables[0].Rows[m]["Purpose"]);
                        Fpspread2.Sheets[0].Cells[m, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 3].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[m, 0].Text = Convert.ToString(m + 1);
                        Fpspread2.Sheets[0].Cells[m, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 0].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 0].Font.Size = FontUnit.Medium;
                        string datesto = Convert.ToString(ds.Tables[0].Rows[m]["GatepassEntrydate"]);
                        int enday;
                        int enmon;
                        int enyear;
                        int todaday;
                        int todmon;

                        int todyear;

                        //string[] spl1 = exdate.Split('-');
                        //DateTime dtl1 = Convert.ToDateTime(spl1[1] + '-' + spl1[0] + '-' + spl1[2]);
                        //string exmdate = dtl1.ToString("dd");
                        //string exmmonth = dtl1.ToString("MM");
                        //string exmmonthful = dtl1.ToString("MMMM");
                        //string exmyear = dtl1.ToString("yyyy");
                        string[] split5 = datesto.Split('/');
                        DateTime dtl1 = Convert.ToDateTime(split5[0] + '-' + split5[1] + '-' + split5[2]);
                        string day5 = dtl1.ToString("dd");
                        string mo5 = dtl1.ToString("MM");
                        string yea5 = dtl1.ToString("yyyy");
                        int.TryParse(day5, out enday);
                        int.TryParse(mo5, out enmon);
                        int.TryParse(yea5, out enyear);

                        string[] split6 = todates.Split('-');
                        string day6 = split6[2];
                        string mo6 = split6[1];
                        string yea6 = split6[0];
                        int.TryParse(day6, out todaday);
                        int.TryParse(mo6, out todmon);
                        int.TryParse(yea6, out todyear);
                        if (enyear <= todyear)
                        {
                            if (enmon < todmon)
                            {


                                Fpspread2.Sheets[0].Cells[m, 4].Text = "in";


                            }
                            else if (enmon == todmon)
                            {

                                if (enday <= todaday)
                                {
                                    Fpspread2.Sheets[0].Cells[m, 4].Text = "in";
                                }
                                else
                                    Fpspread2.Sheets[0].Cells[m, 4].Text = "out";

                            }
                            else
                            {
                                Fpspread2.Sheets[0].Cells[m, 4].Text = "out";
                            }
                        }
                        else
                        {
                            Fpspread2.Sheets[0].Cells[m, 4].Text = "out";
                        }

                        Fpspread2.Sheets[0].Cells[m, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[m, 4].Font.Name = "Book Antiqua";
                        Fpspread2.Sheets[0].Cells[m, 4].Font.Size = FontUnit.Medium;



                    }
                }

                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Width = 900;
                Fpspread2.Height = 700;
                Fpspread2.SaveChanges();
            }
            else
            {
                div4.Visible = false;
                Fpspread2.Visible = false;
            }
        }

        catch
        {
        }
    }
    public static void sms(string req_pk, string appno, string outin, string clgCode, long gateEntryExitID = 0)
    {
        DAccess2 d2 = new DAccess2();
        DataSet ds = new DataSet();//barath21.04.17
        user_id = d2.GetFunction("select SMS_User_ID from Track_Value where college_code='" + clgCode + "'");
        //string getval = d2.GetUserapi(user_id);
        //string[] spret = getval.Split('-');
        //if (spret.GetUpperBound(0) == 1)
        //{
        //    SenderID = spret[0].ToString();
        //    Password = spret[1].ToString();
        //    Session["api"] = user_id;
        //    Session["senderid"] = SenderID;
        //}
        string rq_fk = d2.GetFunction("select GateEntryExitID from GateEntryExit where requestfk='" + req_pk + "'");
        string purpose = "";
        if (gateEntryExitID != 0)
            purpose = d2.GetFunction("select Purpose from GateEntryExit where GateMemType=1 and GateEntryExitID='" + gateEntryExitID + "'");
        else
            purpose = d2.GetFunction("select Purpose from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        outin = d2.GetFunction("select CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        string outinexittime = d2.GetFunction("select GatepassExittime from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        string outinentrytime = d2.GetFunction("select GatepassEntrytime from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        string appnumber = d2.GetFunction("select App_No from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        string q = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt where   a.app_no=r.app_no   and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and a.app_no='" + appnumber + "'";
        ds = d2.select_method_wo_parameter(q, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string name = Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]);
            string course = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);
            string dept = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            //   strmsg = "Your ward miss " + name + "-" + course + "-" + dept + " approved from exited for weekend or anyother on" + date;
            if (outin == "1" || outin.ToLower().Trim() == "out")
            {
                strmsg = " Your ward Mr/Miss." + name + " exited from hostel to go for " + purpose + " on " + date + " at " + outinexittime;
            }
            else if (outin == "2" || outin.ToLower().Trim() == "in")
            {
                strmsg = " Your  ward Mr/Miss." + name + " entered to hostel on  " + date + " at " + outinentrytime;
            }
        }
        accessNew();
        if (sms_mom == "1")
        {
            string momnum = d2.GetFunction("select parentM_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = momnum;
            // mobilenos = "9585698019";
           // mobilenos = "9487302251";
            if (mobilenos != "")//barath21.04.17
            {
              int m=  d2.send_sms(user_id, clgCode, UserCode, mobilenos, strmsg, "0");
                //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
        if (sms_dad == "2")
        {
            string fathernum = d2.GetFunction("select parentF_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = fathernum;
            // mobilenos = "9585698019";
          //  mobilenos = "9487302251";
            if (mobilenos != "")//barath21.04.17
            {
                int m = d2.send_sms(user_id, clgCode, UserCode, mobilenos, strmsg, "0");   //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
        if (sms_stud == "3")
        {
            string studnum = d2.GetFunction("select Student_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = studnum;
            // mobilenos = "9585698019";
          //  mobilenos = "9487302251";
            if (mobilenos != "")//barath21.04.17
            {
                int m = d2.send_sms(user_id, clgCode, UserCode, mobilenos, strmsg, "0");   //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
    }
    public static void smsreport(string uril, string isstaff)
    {
        try
        {
            DAccess2 d2 = new DAccess2();
            Hashtable hat = new Hashtable();
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel.Trim().ToString();
            int sms = 0;
            string smsreportinsert = "";
            string[] split_id = groupmsgid.Split(' ');
            string[] split_mobileno = mobilenos.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                string group_id = split_id[icount].ToString();
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date )values( '" + split_mobileno[icount] + "','" + group_id + "','" + strmsg + "','" + collegecodeee + "','" + isstaff + "','" + date + "' )";
                sms = d2.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch
        {
        }
    }
    protected void btn_staffok_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["btntype"] = "2";
            div_staff.Attributes.Add("style", "display:block");
            div_student.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_visitor.Attributes.Add("style", "display:none");
            div_material.Attributes.Add("style", "display:none");
            div_vehicle.Attributes.Add("style", "display:none");
            menutype = 2;
            string sql = "";
            int byvehicle = 1;
            int clgvehicle = 1;
            DateTime gatepasdate = new DateTime();
            gatepasdate = TextToDate(txt_staff_date);
            DateTime expdate = new DateTime();
            expdate = TextToDate(txt_staff_exp);
            string gatepastime = txt_staff_time.Text;
            string exptime = txt_staff_exptime.Text;
            string appno1 = txt_staffid.Text;
            //barath 16.03.17 
            string appno = "";
            string app_no = "";
            string getapplappno = d2.GetFunction(" select sm.appl_no+'$'+convert(varchar(10), appl_id) from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no and sm.staff_code='" + appno1 + "'");
            string[] getapplidappno = getapplappno.Split('$');
            if (getapplidappno.Length > 1)
            {
                app_no = Convert.ToString(getapplidappno[0]);
                appno = Convert.ToString(getapplidappno[1]);
            }
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            string rq_pk = d2.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='2' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
            gatepasspk = rq_pk;
            string purpose = txt_staff_purpose.Text;
            string vehid = "";
            string vehtype = "";
            string vehregno = "";
            string mobno = "";
            string collegecode = collegecode1;
            if (rdo_staff_other_trans.Checked == true)
            {
                byvehicle = 1;
                clgvehicle = 1;
                vehid = "";
                vehtype = "";
                vehregno = "";
                mobno = "";
            }
            else if (rdo_staff_own_trans.Checked == true)
            {
                byvehicle = 0;
                clgvehicle = 1;
                vehid = "";
                vehtype = "";
                vehregno = txt_staff_ownvehilno.Text;
                mobno = txt_staff_ownmobno.Text;
            }
            else if (rdo_staff_ins_trans.Checked == true)
            {
                byvehicle = 0;
                clgvehicle = 0;
                vehid = txt_staff_vehilno.Text;
                vehtype = d2.GetFunction("select Veh_Type from Vehicle_Master where Veh_ID='" + vehid + "'");
                vehregno = d2.GetFunction("select Reg_No from Vehicle_Master where Veh_ID='" + vehid + "'");
                mobno = d2.GetFunction("select Mobile_No from DriverAllotment where Vehicle_Id='" + vehid + "'");
            }
            string dropsstageid = "";
            if (txt_staff_drop.Text.Trim() != "")
            {
                dropsstageid = d2.GetFunction(" select distinct s.Stage_id   from Vehicle_Master v ,routemaster r,Stage_Master s where v.Veh_ID = r.Veh_ID and r.Stage_Name = s.Stage_id and s.Stage_Name='" + txt_staff_drop.Text + "'");
            }
            if (txt_staffid.Text != "")
            {
                string gatepassperimissiontype = d2.GetFunction("select value from Master_Settings where settings='Gatepass Request Type'");//  and usercode='"+UserCode+"'");
                if (gatepassperimissiontype.Trim() == "0")
                {
                    #region With request
                    string inn = d2.GetFunction(" select GateType from RQ_Requisition r , GateEntryExit g where g.App_No=r.ReqAppNo and g.GateMemType='2' and ReqAppStatus='1' and ReqAppNo='" + appno + "' and RequisitionPK='" + gatepasspk + "' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK and RequestType=6 ");
                    string FetchData = "";
                    if (inn == "0")
                    {
                        inroll = "1";
                        FetchData = " select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK ='" + gatepasspk + "'";
                    }
                    else
                    {
                        inroll = "0";
                        FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,GateEntryExit gg,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1' and g.ReqAppNo=gg.App_No";
                    }
                    ds = d2.select_method_wo_parameter(FetchData, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray = dnewdate.Split('/');
                            DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
                            string[] splitarray1 = entry.Split('/');
                            DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
                            string appgateentydate = d2.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string appgateentrytime = d2.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string lateentry = d2.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
                            string appgateexitdate = d2.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string appgateexittime = d2.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string[] split1 = appgateentrytime.Split(':');
                            string hr1 = split1[0];
                            string min1 = split1[1];
                            string day1 = split1[2];
                            int chr1 = Convert.ToInt32(hr1);
                            int cmin1 = Convert.ToInt32(min1);
                            string islate = "";
                            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Msg = "0";
                            string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
                            string hr = split[0];
                            string min = split[1];
                            string[] splitNew = min.Split(' ');
                            min = splitNew[0];
                            string day = splitNew[1];
                            int chr = Convert.ToInt32(hr);
                            int cmin = Convert.ToInt32(min);
                            string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                            if (appgateexitdate == currentdate)
                            {
                                string pk = d2.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                                if (inroll == "1")
                                {
                                    string timecheck = d2.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  RequisitionPK='" + gatepasspk + "'");
                                    string[] exittime = timecheck.Split(':');
                                    string gateexithr = exittime[0];
                                    string gateexitmin = exittime[1];
                                    string gateexitampm = exittime[2];
                                    if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && chr1 != 12)
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (pk == "0" || pk == "" && Msg == "0")
                                    {
                                        sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,Purpose,ByVehcile,IsCollVeh,VehType,VehId,VehRegNo,College_Code,GatepassExitdate,GatepassExittime,RequestFk) values('" + menutype + "','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + appno + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','" + purpose + "','" + byvehicle + "','" + clgvehicle + "', '" + vehtype + "','" + vehid + "','" + vehregno + "','" + collegecode + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + gatepasspk + "')";
                                        int ud = d2.update_method_wo_parameter(sql, "TEXT");
                                        if (ud != 0)
                                        {
                                            string gateexitid = d2.GetFunction(" select GateEntryExitid from GateEntryExit where GateMemType='2' and GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No='" + appno + "' and College_Code='" + collegecode + "'");
                                            if (dropsstageid == "")
                                                dropsstageid = "0";
                                            d2.update_method_wo_parameter("insert into GateEntryExitdet (GateEntryExitID,GateMemType,Vehicle_id,Drop_stageId) values('" + gateexitid + "','2','" + vehid + "','" + dropsstageid + "')", "text");
                                            imgdiv2.Visible = true;
                                            imgdiv2.Attributes.Add("style", "display:block");
                                            div_staff.Attributes.Add("style", "display:block");
                                            lbl_erroralert.Text = "Saved Successfully";
                                        }
                                    }
                                }
                                else
                                {
                                    string outcheck = d2.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='2' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                    if (outcheck != "0" || outcheck == "True")
                                    {
                                        if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                        {
                                            islate = "1";
                                        }
                                        else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                        {
                                            if (chr > chr1 && day1 == day)
                                            {
                                                islate = "1";
                                            }
                                            else if (chr == chr1 && cmin > cmin1 && day1 == day)
                                            {
                                                islate = "1";
                                            }
                                            else
                                            {
                                                islate = "0";
                                            }
                                        }
                                        sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='2' and GateType='1' and GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and RequestFk='" + pk + "'";
                                        int qu = d2.update_method_wo_parameter(sql, "TEXT");
                                        if (qu > 0)
                                        {
                                            string gateexitid = d2.GetFunction(" select GateEntryExitid from GateEntryExit where GateMemType='2' and GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No='" + appno + "' and College_Code='" + collegecode + "'");
                                            if (dropsstageid == "")
                                                dropsstageid = "0";
                                            d2.update_method_wo_parameter("insert into GateEntryExitdet (GateEntryExitID,GateMemType,Vehicle_id,Drop_stageId) values('" + gateexitid + "','2','" + vehid + "','" + dropsstageid + "')", "text");
                                            imgdiv2.Visible = true;
                                            imgdiv2.Attributes.Add("style", "display:block");
                                            div_staff.Attributes.Add("style", "display:block");
                                            lbl_erroralert.Text = "Saved Successfully";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                imgdiv2.Attributes.Add("style", "display:block");
                                div_staff.Attributes.Add("style", "display:block");
                                lbl_erroralert.Text = "Please Apply the Request Date";
                            }
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        imgdiv2.Attributes.Add("style", "display:block");
                        div_staff.Attributes.Add("style", "display:block");
                        lbl_erroralert.Text = "Please Apply the Request";
                    }
                    #endregion
                }
                else
                {
                    #region with out request
                    string inn = d2.GetFunction("select GateType from GateEntryExit where app_no='" + appno + "' and gatepassexitdate='" + date + "' and GateMemType='2' order by GateEntryExitid desc");
                    if (inn == "0" || inn == "False")
                    {
                        inroll = "1";
                    }
                    else
                    {
                        inroll = "0";
                    }
                    string FetchData = "  select sa.appl_id,s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,s.college_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.appl_id ='" + appno + "'";
                    ds = d2.select_method_wo_parameter(FetchData, "Text");
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                string clgcode = Convert.ToString(dr["college_code"]);
                                string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                                string gatepasstime = Convert.ToString(DateTime.Now.ToString("h:mm:tt"));
                                if (inroll == "1")
                                {
                                    sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,GatePassTime,College_Code,Purpose,ByVehcile,IsCollVeh,VehType,VehId,VehRegNo) values('2','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + appno + "','1','0','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + CurrentTime + "','0','" + gatepasstime + "','" + clgcode + "','" + purpose + "','" + byvehicle + "','" + clgvehicle + "', '" + vehtype + "','" + vehid + "','" + vehregno + "')";
                                    int ud = d2.update_method_wo_parameter(sql, "TEXT");
                                    if (ud != 0)
                                    {
                                        string gateexitid = d2.GetFunction(" select GateEntryExitid from GateEntryExit where GateMemType='2' and GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No='" + appno + "' and College_Code='" + clgcode + "'");
                                        if (dropsstageid == "")
                                            dropsstageid = "0";
                                        if (rdo_staff_own_trans.Checked == true)
                                            vehid = vehregno;
                                        d2.update_method_wo_parameter("insert into GateEntryExitdet (GateEntryExitID,GateMemType,Vehicle_id,Drop_stageId,MobileNo) values('" + gateexitid + "','2','" + vehid + "','" + dropsstageid + "','" + mobno + "')", "text");
                                        imgdiv2.Visible = true;
                                        imgdiv2.Attributes.Add("style", "display:block");
                                        div_staff.Attributes.Add("style", "display:block");
                                        lbl_erroralert.Text = "Saved Successfully";
                                    }
                                }
                                else
                                {
                                    sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassEntrytime='" + CurrentTime + "',GateType='0',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='2' and GateType='1' and GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' ";
                                    int qu = d2.update_method_wo_parameter(sql, "TEXT");
                                    if (qu > 0)
                                    {
                                        string gateexitid = d2.GetFunction(" select GateEntryExitid from GateEntryExit where GateMemType='2' and GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No='" + appno + "' and College_Code='" + clgcode + "'");
                                        if (dropsstageid == "")
                                            dropsstageid = "0";
                                        if (rdo_staff_own_trans.Checked == true)
                                            vehid = vehregno;
                                        d2.update_method_wo_parameter(" if exists(select*from GateEntryExitdet where GateEntryExitID='" + gateexitid + "' and vehicle_id='" + vehid + "') update GateEntryExitdet set vehicle_id='" + vehid + "',drop_stageid='" + dropsstageid + "' where GateEntryExitID='" + gateexitid + "' and vehicle_id='" + vehid + "' else insert into GateEntryExitdet (GateEntryExitID,GateMemType,Vehicle_id,Drop_stageId,MobileNo) values('" + gateexitid + "','2','" + vehid + "','" + dropsstageid + "','" + mobno + "')", "text");
                                        imgdiv2.Visible = true;
                                        imgdiv2.Attributes.Add("style", "display:block");
                                        div_staff.Attributes.Add("style", "display:block");
                                        lbl_erroralert.Text = "Saved Successfully";
                                    }
                                }
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            imgdiv2.Attributes.Add("style", "display:block");
                            div_staff.Attributes.Add("style", "display:none");
                            lbl_erroralert.Text = "Please Enter Valid Staff code";
                        }
                    }
                    #endregion
                }
                // ScriptManager.RegisterStartupScript(this, GetType(), "staffbtn", "staffbtn();", true); 
            }
            else
            {
                div_staff.Attributes.Add("style", "display:none");
                imgdiv2.Attributes.Add("style", "display:block");
                lbl_erroralert.Text = "Select staff code";
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Attributes.Add("style", "display:block");
            lbl_erroralert.Text = ex.ToString();
        }
    }
    protected void btn_parentok_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["btntype"] = "3";
            div_staff.Attributes.Add("style", "display:none");
            div_student.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:block");
            div_visitor.Attributes.Add("style", "display:none");
            div_material.Attributes.Add("style", "display:none");
            div_vehicle.Attributes.Add("style", "display:none");
            menutype = 3;
            int gatetype;
            if (rb_parin.Checked == true)
            {
                gatetype = 0;
            }
            else
            {
                gatetype = 1;
            }
            string appcode = "";
            string sql = "";
            string detsql = "";
            int IsAdmitted = 0;
            string gdate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime gatepassdate = new DateTime();
            string[] split = gdate.Split('/');
            gatepassdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string gatepasstime = DateTime.Now.ToLongTimeString();
            if (rb_adm_stud.Checked == true)
            {
                string appno = d2.GetFunction("select r.App_No from applyn a,Registration r where a.app_no=r.App_No and r.Roll_No='" + txt_stud_rollno.Text + "'");
                string stud_name = txt_studname.Text;
                string purpose = txt_purposevisit.Text;
                IsAdmitted = 0;
                string relation = txt_studrelation.Text;
                int tomeet = 0;
                string staffcode = "";
                string othername = "";
                string relationship = "";
                string mobileno = "";
                if (rb_meetstaff.Checked == true)
                {
                    tomeet = 0;
                    staffcode = txt_staffid.Text;
                }
                else if (rb_meetoffice.Checked == true)
                {
                    tomeet = 1;
                    string[] staffname = txt_staffname1.Text.Split('-');
                    staffcode = d2.GetFunction("select staff_code from staffmaster where stafF_name='" + staffname[0] + "'");
                }
                else if (rb_meetothers.Checked == true)
                {
                    tomeet = 2;
                    othername = txt_name1.Text;
                    relationship = txt_relation.Text;
                    mobileno = txt_moblno.Text;
                }
                if (rb_parin.Checked == true)
                {
                    if (txt_stud_rollno.Text != "" && txt_studrelation.Text != "")
                    {
                        sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,App_No,purpose,IsAdmitedStud,Relationship,Tomeet,college_code,GatepassEntrydate,GatepassEntrytime) values('" + menutype + "','1','" + gatepassdate + "','" + gatepasstime + "','" + appno + "','" + purpose + "','" + IsAdmitted + "','" + relation + "','" + tomeet + "','" + collegecode1 + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm:tt") + "')";
                        int query = d2.update_method_wo_parameter(sql, "TEXT");
                        string getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and App_No='" + appno + "' and IsAdmitedStud='" + IsAdmitted + "' and GatePassDate='" + gatepassdate + "'");
                        //.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and App_No='" + appno + "' and IsAdmitted='" + IsAdmitted + "'");
                        // ds = da.select_method_wo_parameter(getIDType, "TEXT");
                        detsql = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,staff_code,othername,Relationship,MobileNo) values('" + getID + "','" + menutype + "','" + staffcode + "','" + othername + "','" + relationship + "','" + mobileno + "')";
                        int query1 = d2.update_method_wo_parameter(detsql, "TEXT");
                        div_parent.Attributes.Add("style", "display:block");
                        div_adm_stud.Attributes.Add("style", "display:block");
                        if (query1 != 0)
                        {
                            imgdiv2.Visible = true;
                            imgdiv2.Attributes.Add("style", "display:block");
                            lbl_erroralert.Text = "Saved Successfully";
                        }
                    }
                }
                else
                {
                    string getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and App_No='" + appno + "' and IsAdmitedStud='" + IsAdmitted + "' and GatePassDate='" + gatepassdate + "'");
                    sql = "update GateEntryExit set GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassExittime='" + DateTime.Now.ToString("hh:mm:tt") + "',GateType='0' where GateEntryExitID='" + getID + "' and GateMemType='3' and App_No='" + appno + "' and GateType='1'";
                    int qu = d2.update_method_wo_parameter(sql, "TEXT");
                    div_parent.Attributes.Add("style", "display:block");
                    div_adm_stud.Attributes.Add("style", "display:block");
                    if (qu != 0)
                    {
                        imgdiv2.Visible = true;
                        imgdiv2.Attributes.Add("style", "display:block");
                        lbl_erroralert.Text = "Saved Successfully";
                    }
                }
            }
            else if (rb_notadm_stud.Checked == true)
            {
                IsAdmitted = 1;
                string studname = txt_name2.Text;
                string add1 = txt_addrs.Text;
                //string add2 = txt_street.Text;
                string city = txt_city.Text;
                string district = txt_district.Text;
                string state = txt_state.Text;
                string mobno = txt_mob.Text;
                string purpose = txt_visit.Text;
                if (rb_parin.Checked == true)
                {
                    sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,StudName,Add1,City,District,State,MobileNo,purpose,IsAdmitedStud,college_code,GatepassEntrydate,GatepassEntrytime) values('" + menutype + "','1','" + gatepassdate + "','" + gatepasstime + "','" + studname + "','" + add1 + "','" + city + "','" + district + "','" + state + "','" + mobno + "','" + purpose + "','" + IsAdmitted + "','" + collegecode1 + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm:tt") + "')";
                    int notadmitquery = d2.update_method_wo_parameter(sql, "TEXT");
                    div_parent.Attributes.Add("style", "display:block");
                    div_notadm_stud.Attributes.Add("style", "display:block");
                    if (notadmitquery != 0)
                    {
                        imgdiv2.Visible = true;
                        imgdiv2.Attributes.Add("style", "display:block");
                        lbl_erroralert.Text = "Saved Successfully";
                    }
                }
                else
                {
                    string getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and StudName='" + studname + "' and IsAdmitedStud='" + IsAdmitted + "' and GatePassDate='" + gatepassdate + "'");
                    sql = "update GateEntryExit set GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassExittime='" + DateTime.Now.ToString("hh:mm:tt") + "',GateType='0' where GateEntryExitID='" + getID + "' and GateMemType='3' and StudName='" + studname + "' and GateType='1'";
                    int qu = d2.update_method_wo_parameter(sql, "TEXT");
                    div_parent.Attributes.Add("style", "display:block");
                    div_notadm_stud.Attributes.Add("style", "display:block");
                    if (qu != 0)
                    {
                        imgdiv2.Visible = true;
                        imgdiv2.Attributes.Add("style", "display:block");
                        lbl_erroralert.Text = "Saved Successfully";
                    }
                }
            }
            // ScriptManager.RegisterStartupScript(this, GetType(), "parentsbtn", "parentsbtn()", true); 
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_visitorok_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_mno.Text.Length == 10)
            {
                txt_compname_Changed(sender, e);
                ViewState["btntype"] = "4";
                div_staff.Attributes.Add("style", "display:none");
                div_student.Attributes.Add("style", "display:none");
                div_parent.Attributes.Add("style", "display:none");
                div_visitor.Attributes.Add("style", "display:block");
                div_material.Attributes.Add("style", "display:none");
                div_vehicle.Attributes.Add("style", "display:none");
                menutype = 4;
                string gdate = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime gatepassdate = new DateTime();
                string[] split = gdate.Split('/');
                gatepassdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string gatepasstime = DateTime.Now.ToLongTimeString();
                string sql = "";
                string detsql = "";
                string expectedtime = ddl_hrs.SelectedItem.Text + ":" + ddl_mins.SelectedItem.Text + ":" + "00 " + ddl_ampm.SelectedItem.Text;
                //txt_exprettime.Text;     add ddl values
                string purpose = txt_visit1.Text.Trim();
                int byvehicle = 1;
                string vehtype = "";
                string vehno = "";
                int isreturn = 0;
                int isapproval = 0;
                int visitortype = 0;
                string visitorname = txt_name4.Text.Trim().ToUpper();
                string companyname = txt_compname.Text.Trim().ToUpper();
                string visitordept = txt_dep.Text.Trim();
                string visitordesig = txt_desgn.Text.Trim();
                string gateno = TextBox1.Text;
                string city = txt_cty.Text.Trim();
                string state = txt_stat.Text.Trim();
                string distr = txt_dis.Text.Trim();
                string addr = txt_str.Text.Trim();
                if (rb_staff1.Checked == true)
                {
                    ViewState["To Meet"] = txt_visitormeetstaffname.Text;
                    ViewState["To Meet dept"] = txt_visitormeetstaffdept.Text;
                    //ViewState["To Meet dept"] = txt_visitormeetstaffdept.Text;
                }
                else if (rb_office1.Checked == true)
                {
                    ViewState["To Meet"] = txt_visitormeetoffname.Text;
                    ViewState["To Meet dept"] = txt_visitormeetoffdept.Text;
                }
                else if (rb_others1.Checked == true)
                {
                    ViewState["To Meet"] = txt_visitormeetothername.Text;
                    ViewState["To Meet dept"] = txt_visitormeetotherrel.Text;
                }

                int gatetype;
                if (rb_visitin.Checked == true)
                {
                    gatetype = 0;
                }
                else
                {
                    gatetype = 1;
                }
                if (rb_vehyes.Checked == true)
                {
                    byvehicle = 0;
                    vehtype = txt_vehtype.Text;
                    vehno = txt_vehno1.Text;
                }
                else if (rb_vehno.Checked == true)
                {
                    byvehicle = 1;
                }
                if (rb_ret.Checked == true)
                {
                    isreturn = 0;
                }
                else if (rb_notret.Checked == true)
                {
                    isreturn = 1;
                }
                if (rb_company.Checked == true)
                {
                    visitortype = 0;
                }
                else if (rb_individual.Checked == true)
                {
                    visitortype = 1;
                }
                string tomeet = "";
                if (rb_staff1.Checked == true)
                {
                    tomeet = "0";
                }
                else if (rb_office1.Checked == true)
                {
                    tomeet = "1";
                }
                else if (rb_others1.Checked == true)
                {
                    tomeet = "2";
                }


                if (rb_withap.Checked == true)
                {
                    string vendorpkval = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VenContactName='" + visitorname + "' and VendorMobileNo='" + txt_mno.Text.Trim() + "' order by VendorContactPK desc");
                    string rq_ds = string.Empty;
                    rq_ds = d2.GetFunction("select max(RequisitionPK) from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and RequestDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and ReqApproveStage='1'");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
                    isapproval = 1;
                    string[] staffname = sname.Text.Split('-');
                    string staffcode = d2.GetFunction("select staff_code from staffmaster where stafF_name='" + staffname[0] + "'");
                    if (gatetype == 0)
                    {
                        sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,IsApproval,ExpectedTime,purpose,ByVehcile,VehType,VehRegNo,IsReturn,VisitorName,VisitorType,CompanyName,VisitorDept,VisitorDesig,College_Code,GatepassEntrydate,GatepassEntrytime,MobileNo,gatepassno,City,District,State,Add1,requestfk) values('" + menutype + "','1','" + gatepassdate + "','" + gatepasstime + "','" + isapproval + "','" + expectedtime + "','" + purpose + "','" + byvehicle + "','" + vehtype + "','" + vehno.ToUpper() + "','" + isreturn + "','" + visitorname + "','" + visitortype + "','" + companyname + "','" + visitordept + "','" + visitordesig + "','" + collegecode1 + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + txt_mno.Text.Trim() + "','" + gateno + "','" + city + "','" + distr + "','" + state + "','" + addr + "','" + rq_ds + "')";
                        int query = d2.update_method_wo_parameter(sql, "TEXT");
                        string getID = da.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and GatePassDate='" + gatepassdate + "' and CompanyName='" + companyname + "' and VisitorName='" + visitorname + "'");//and vehRegNo='" + vehno + "' and GatePassTime='" + gatepasstime + "'
                        detsql = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,Staff_Code) values('" + getID + "','" + menutype + "','" + staffcode + "')";
                    }
                    if (gatetype == 1)
                    {
                        string getID = da.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "'   and VisitorName='" + visitorname + "' and gatepassno='" + gateno + "'  order by GateEntryExitID desc");//and vehRegNo='" + vehno + "' GatePassDate='" + gatepassdate + "'  and CompanyName='" + companyname + "'
                        detsql = "update GateEntryExit set GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassExittime='" + DateTime.Now.ToString("hh:mm tt") + "',GateType='0',ByVehcile='" + byvehicle + "',VehType='" + vehtype + "',VehRegNo='" + vehno + "' where GateEntryExitID='" + getID + "' and GateMemType='4' and GateType='1' ";//and GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'
                        if (rb_company.Checked == true)
                        {
                            // detsql += " and CompanyName='" + companyname + "' ";
                        }
                        //int query = d2.update_method_wo_parameter(sql, "TEXT");
                    }
                }
                else if (rb_withoutap.Checked == true)
                {
                    isapproval = 0;
                    if (gatetype == 0)
                    {
                        sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,IsApproval,ExpectedTime,purpose,ByVehcile,VehType,VehRegNo,IsReturn,VisitorName,VisitorType,CompanyName,VisitorDept,VisitorDesig,College_Code,GatepassEntrydate,GatepassEntrytime,tomeet,MobileNo,gatepassno,City,District,State,Add1) values('" + menutype + "','1','" + gatepassdate + "','" + gatepasstime + "','" + isapproval + "','" + expectedtime + "','" + purpose + "','" + byvehicle + "','" + vehtype + "','" + vehno + "','" + isreturn + "','" + visitorname + "','" + visitortype + "','" + companyname + "','" + visitordept + "','" + visitordesig + "','" + collegecode1 + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + tomeet + "','" + txt_mno.Text.Trim() + "','" + gateno + "','" + city + "','" + distr + "','" + state + "','" + addr + "')";
                        int query = d2.update_method_wo_parameter(sql, "TEXT");
                        string getID = da.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and GatePassDate='" + gatepassdate + "' and GatePassTime='" + gatepasstime + "' and vehRegNo='" + vehno + "' and CompanyName='" + companyname + "'");
                        if (rb_staff1.Checked == true)
                        {
                            detsql = " insert into GateEntryExitDet(GateEntryExitID,GateMemType,Staff_Code) values('" + getID + "','" + menutype + "','" + txt_visitormeetstaffid.Text + "')";
                        }
                        else if (rb_office1.Checked == true)
                        {
                            string[] staffname = txt_visitormeetoffname.Text.Split('-');
                            string staffcode = d2.GetFunction("select staff_code from staffmaster where stafF_name='" + staffname[0] + "'");
                            detsql = " insert into GateEntryExitDet(GateEntryExitID,GateMemType,Staff_Code) values('" + getID + "','" + menutype + "','" + staffcode + "')";
                        }
                        else if (rb_others1.Checked == true)
                        {
                            string othername = txt_visitormeetothername.Text;
                            string relationship = txt_visitormeetotherrel.Text;
                            string mobileno = txt_visitormeetothermob.Text;
                            detsql = " insert into GateEntryExitDet(GateEntryExitID,GateMemType,OtherName,Relationship,MobileNo) values('" + getID + "','" + menutype + "','" + othername + "','" + relationship + "','" + mobileno + "')";
                        }
                    }
                    if (gatetype == 1)
                    {
                        string getID = da.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "'   and VisitorName='" + visitorname + "' and gatepassno='" + gateno + "'");//and GatePassDate='" + gatepassdate + "' and CompanyName='" + companyname + "'
                        detsql = "update GateEntryExit set GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassExittime='" + DateTime.Now.ToString("hh:mm tt") + "',GateType='0',ByVehcile='" + byvehicle + "',VehType='" + vehtype + "',VehRegNo='" + vehno + "' where GateEntryExitID='" + getID + "' and GateMemType='4' and GateType='1'  and VisitorName='" + visitorname + "' and gatepassno='" + gateno + "' ";// and GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'
                        if (rb_company.Checked == true)
                        {
                            //detsql += " and CompanyName='" + companyname + "' ";
                        }
                        //int query = d2.update_method_wo_parameter(sql, "TEXT");
                    }
                }
                int detquery = d2.update_method_wo_parameter(detsql, "TEXT");
                if (detquery != 0)
                {
                    div_visitor.Attributes.Add("style", "display:block");
                    imgdiv2.Visible = true;
                    txt_name4.Text = " ";
                    txt_mno.Text = " ";
                    txt_cty.Text = " ";
                    txt_visit1.Text = " ";
                    txt_vehno1.Text = " ";
                    txt_vehtype.Text = " ";
                    txt_dep.Text = " ";
                    txt_desgn.Text = " ";
                    txt_stat.Text = " ";
                    txt_dis.Text = " ";
                    txt_str.Text = " ";
                    imgdiv2.Attributes.Add("style", "display:block");
                    lbl_erroralert.Text = "Saved Successfully";
                    Hiddcen1.Value = TextBox1.Text;
                    Newitem();
                    //btnerrorclose_Click(sender,e);
                    string gatetype1 = Convert.ToString(gatetype);
                  sms1(gateno, gatetype1);
                    //print();
                }
            }
            else
            {
                div_visitor.Attributes.Add("style", "display:block");
                imgdiv2.Visible = true;
                imgdiv2.Attributes.Add("style", "display:block");
                lbl_erroralert.Text = "Please Enter Valid 10 - Digits Mobile Numbers";
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "visitorbtn", "visitorbtn();", true);
        }
        catch (Exception ex)
        {
            div_visitor.Attributes.Add("style", "display:block");
            imgdiv2.Visible = true;
            imgdiv2.Attributes.Add("style", "display:block");
            lbl_erroralert.Text = ex.ToString();
        }
    }
    public void print()
    {
        StringBuilder SbHtml = new StringBuilder();


        string clgaddress = string.Empty;
        string pincode = string.Empty;
        string collName = string.Empty;
        string VisitorName = "";
        string CompanyName = "";
        string GatePassDate = "";
        string MobileNo = "";
        string gateno = "";
        string intime = string.Empty;
        string outtime = string.Empty;
        string Purpose = string.Empty;
        string add1 = string.Empty;
        string city = string.Empty;
        string state = string.Empty;
        string dis = string.Empty;
        int pin = 0;
        string meet = string.Empty;
        string Deptm = string.Empty;
        string expectedtime = string.Empty;
        string strquery = "select *,district+' - '+pincode as districtpin,collname from collinfo where college_code='" + collegecode1 + "'";
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            pincode = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]).Trim();
            collName = Convert.ToString(ds.Tables[0].Rows[0]["collname"]).Trim();
            clgaddress = Convert.ToString(ds.Tables[0].Rows[0]["address3"]) + " , " + Convert.ToString(ds.Tables[0].Rows[0]["district"]) + ((pin != 0) ? (" - " + pin.ToString()) : " - " + pincode);
        }
        DataSet printds_new = new DataSet();
        string sql2 = "select VisitorName,CompanyName,GatePassDate,MobileNo,GatepassEntrytime,ExpectedTime,Purpose,Add1,City,District,state,IsApproval,requestfk from GateEntryExit where gatepassno ='" + Hiddcen1.Value + "' and College_Code='" + collegecode1 + "'";
        printds_new = da.select_method_wo_parameter(sql2, "Text");
        //printds_new.Reset();
        // printds_new = d2.select_method_wo_parameter(strquery, "Text");
        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
        {
            CompanyName = Convert.ToString(printds_new.Tables[0].Rows[0]["CompanyName"]).Trim();
            gateno = Hiddcen1.Value;
            VisitorName = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorName"]).Trim();
            add1 = Convert.ToString(printds_new.Tables[0].Rows[0]["Add1"]).Trim();
            city = Convert.ToString(printds_new.Tables[0].Rows[0]["City"]).Trim();
            dis = Convert.ToString(printds_new.Tables[0].Rows[0]["District"]).Trim();
            state = Convert.ToString(printds_new.Tables[0].Rows[0]["state"]).Trim();
             string approv = string.Empty;
             string dep = string.Empty;
             string depname = string.Empty;
             string stf = string.Empty;
             string stfname = string.Empty;
             string stafname = string.Empty;
             string staff_names = string.Empty;
             string code = string.Empty;
             string code1 = string.Empty;
             DataSet steaff = new DataSet();
             DataSet steaff1 = new DataSet();

             string reqpk = Convert.ToString(printds_new.Tables[0].Rows[0]["requestfk"]);
             approv = Convert.ToString(printds_new.Tables[0].Rows[0]["IsApproval"]);
              if (approv == "False")
                {
                    meet = Convert.ToString(ViewState["To Meet"]);

                    Deptm = Convert.ToString(ViewState["To Meet dept"]);
                }


                else
                {

                    if (reqpk != "")
                    {
                        dep = d2.GetFunction("select MeetDeptCode from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                        if (dep.Contains(','))
                        {
                            string[] spl = dep.Split(',');



                            if (spl.Length > 0)
                            {
                                for (int i = 0; i < spl.Length; i++)
                                {
                                    if (depname == "")
                                    {
                                        depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'");
                                        meet = depname;
                                    }
                                    else
                                    {
                                        depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'") + ',' + depname;
                                        meet = depname;
                                    }


                                }
                            }
                            else
                            {
                                depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + dep + "'");
                                meet = depname;
                            }

                        }

                        if (reqpk != "")
                            stf = d2.GetFunction("select MeetStaffAppNo from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                        if (stf!="")
                        {
                        if (stf.Contains(','))
                        {
                            string[] spl = stf.Split(',');
                            if (spl.Length > 0)
                            {
                                for (int i = 0; i < spl.Length; i++)
                                {
                                    string sql = "select * from staffmaster where appl_no='" + spl[i] + "'";

                                    steaff = d2.select_method_wo_parameter(sql, "text");

                                    if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                                    {

                                        stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                        code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                                    }
                                    if (stafname == "")
                                    {
                                        stafname = stfname;
                                        meet = stafname;
                                        code1 = code;

                                    }
                                    else
                                    {
                                        stafname = stafname + ',' + stfname;
                                        meet = stafname;
                                        code1 = code1 + ',' + code;
                                    }
                                  
                                   
                                }
                            }

                        }
                        else
                        {
                            string sql = "select * from staffmaster where appl_no='" + stf + "'";

                            steaff = d2.select_method_wo_parameter(sql, "text");

                            if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                            {

                                stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                            }
                            if (stafname == "")
                            {
                                stafname = stfname;
                                meet = stafname;
                                code1 = code;

                            }
                         
                        }
                    }
                    }
                }

              if (approv == "False")
              {

                  if (meet != "")
                  {
                      string[] spli = meet.Split('-');
                      if (spli.Length > 0)
                      {
                          meet = spli[0];
                          //if (spli.Length >= 3)
                          //    if (spli[2] != "")
                          //        meet = spli[0] + '-' + spli[2];
                      }
                  }
                  if (Deptm != "")
                  {
                      meet = meet + "-" + Deptm;
                  }
              }
            MobileNo = Convert.ToString(printds_new.Tables[0].Rows[0]["MobileNo"]).Trim();
            intime = Convert.ToString(printds_new.Tables[0].Rows[0]["GatepassEntrytime"]).Trim();
            outtime = Convert.ToString(printds_new.Tables[0].Rows[0]["ExpectedTime"]).Trim();
            expectedtime = ddl_hrs.SelectedItem.Text + ":" + ddl_mins.SelectedItem.Text + "" + ddl_ampm.SelectedItem.Text;
            Purpose = Convert.ToString(printds_new.Tables[0].Rows[0]["Purpose"]).Trim();

        }

        #region I Page
        SbHtml.Append("<html>");
        SbHtml.Append("<body>");
        SbHtml.Append("<div style='height:715px; width: 655px; border:1px solid black; margin:0px; margin-left: 105px;page-break-after: always;'>");

        #region Header

        SbHtml.Append("<div style='width: 910px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<font face='IDAutomationHC39M'size='4'>");
        SbHtml.Append("<div style='width: 945px; height: 5px; border: 0px solid black; margin:0px; margin-left: 370px;'>");
        string barcode = "*" + Hiddcen1.Value + "*";
        SbHtml.Append("<span style='font-weight:bold;'width: 7px; height:5px; border: 0px solid Red'  >" + barcode + "  </span>");
        SbHtml.Append("</div>");
        SbHtml.Append("</font>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='0px' style='width: 645px; height:30px; font-weight: bold;'>");
        SbHtml.Append("<tr style='text-align:right;'>");
        SbHtml.Append("<td>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td rowspan='3'><img src='" + "../college/Left_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
        SbHtml.Append("<td style='text-align:center;'>");
        
        SbHtml.Append("<span> " + collName + "</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td rowspan='3'><img src='" + "../college/right_Logo.jpeg" + "' style='height:80px; width:80px;'/></td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:center;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span> " + clgaddress + "</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:center;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span> VISITOR'S SLIP </span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td colspan='5' style='text-align:right;'>");
        
        
        SbHtml.Append("<span> DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + " </span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr><td colspan='3'><hr style='height:1px; width:600px;'></td></tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");

        #endregion

        #region Student Details
       
        SbHtml.Append("<br>");
        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table cellspacing='0' cellpadding='5' border='1px' style='width: 645px; font-weight: bold;'>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Gatepass No</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td width='400px'>");
        SbHtml.Append("<span>" + gateno + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Visitor Name & Address</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + VisitorName + "<br> " + add1 + " <br>  " + city + "<br> " + dis + " <br>  " + state + " </span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>To Meet</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + meet + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Time In</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + intime + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        //SbHtml.Append("<tr>");
        //SbHtml.Append("<td>");
        //SbHtml.Append("<span>Time Out</span>");


        //SbHtml.Append("</td>");
        //SbHtml.Append("<td>");
        //SbHtml.Append("<span>" + expectedtime + "</span>");

        //SbHtml.Append("</td>");
        //SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Purpose</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + Purpose + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Mobile No</span>");


        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>" + MobileNo + "</span>");

        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        //SbHtml.Append("<tr>");
        //SbHtml.Append("<td>");

        //SbHtml.Append("<span>In this Moderation used to any students are need 1 Mark to get Minimum total. It apply and reach minimum total for that student</span>");
        //SbHtml.Append("</td>");
        //SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        #endregion

        #region FooterDetails

        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<br>");
        SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
        SbHtml.Append("<table border='0px' cellspacing='0' cellpadding='5' style='width: 645px;'>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("</tr>");
        SbHtml.Append("<tr style='text-align:left;'>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Security</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Visitor</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("</td>");
        SbHtml.Append("<td>");
        SbHtml.Append("<span>Concerned Person</span>");
        SbHtml.Append("</td>");
        SbHtml.Append("</tr>");
        SbHtml.Append("</table>");
        SbHtml.Append("</div>");
        SbHtml.Append("</div>");
        SbHtml.Append("</body>");
        SbHtml.Append("</html>");

        contentDiv.InnerHtml = SbHtml.ToString();
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "btn_erroralert", "PrintDiv();", true);

        #endregion

        #endregion
    }


    public static void sms1(string gateno, string gatetype)
    {
        DAccess2 d2 = new DAccess2();
        DataSet ds = new DataSet();//barath21.04.17
       
        user_id = d2.GetFunction("select SMS_User_ID from Track_Value where college_code='" +colg + "'");
        //string getval = d2.GetUserapi(user_id);
        //string[] spret = getval.Split('-');
        //if (spret.GetUpperBound(0) == 1)
        //{
        //    SenderID = spret[0].ToString();
        //    Password = spret[1].ToString();
        //    Session["api"] = user_id;
        //    Session["senderid"] = SenderID;
        //}


        string purpose = "";
        if (gateno != "")
            purpose = d2.GetFunction("select Purpose from GateEntryExit where GateMemType=4 and  gatepassno='" + gateno + "'");

        // string outin = d2.GetFunction("select GateType from GateEntryExit where GateMemType=1 and GateEntryExitID='" + rq_fk + "'");
        string outinexittime = d2.GetFunction("select GatepassExittime from GateEntryExit where GateMemType=4 and gatepassno='" + gateno + "'");
        string outinentrytime = d2.GetFunction("select GatepassEntrytime from GateEntryExit where GateMemType=4 and gatepassno='" + gateno + "'");


        string date = DateTime.Now.ToString("dd/MM/yyyy");
        //   strmsg = "Your ward miss " + name + "-" + course + "-" + dept + " approved from exited for weekend or anyother on" + date;
        //if (gatetype == "1")
        //{
        //    strmsg = " Visitor  exited from College to go for " + purpose + " on " + date + " at " + outinexittime;
        //}
        //else if (gatetype == "0")
        //{
        //    strmsg = " Visitor entered to College on  " + date + " at " + outinentrytime;
        //}



         string dep = string.Empty;
                string depname = string.Empty;
                string stf = string.Empty;
                string stfname = string.Empty;
                string MOb = string.Empty;
                string Mobli = string.Empty;
                string code = string.Empty;
                string code1 = string.Empty;
                DataSet steaff = new DataSet();
                DataSet steaff1 = new DataSet();
                string stafname = string.Empty;
                string mobile = string.Empty;
                string getID = string.Empty;
                string detsql = string.Empty;
                string staff_names = string.Empty;
                string visitorname = string.Empty;
                string visitormobli = string.Empty;
         getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where gatepassno='" + gateno + "' ");
         visitorname = d2.GetFunction("select VisitorName from GateEntryExit where gatepassno='" + gateno + "' ");
         visitormobli = d2.GetFunction("select MobileNo from GateEntryExit where gatepassno='" + gateno + "' ");
        string meet = d2.GetFunction("select tomeet from GateEntryExit where gatepassno='" + gateno + "' ");
        string approv = d2.GetFunction("select isapproval from GateEntryExit where gatepassno='" + gateno + "' ");
         detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
        string reqpk = d2.GetFunction("select requestfk from GateEntryExit where gatepassno='" + gateno + "' ");

        if (approv == "False")
        {
            if (meet == "2")
            {
                detsql = " select OtherName,Relationship,MobileNo from GateEntryExitDet where GateEntryExitID='" + getID + "'";

                DataSet steafs = d2.select_method_wo_parameter(detsql, "text");
                if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                {
                    stafname = Convert.ToString(steafs.Tables[0].Rows[0]["OtherName"]);

                    Mobli = Convert.ToString(steafs.Tables[0].Rows[0]["MobileNo"]);
                   // Mobli = "9487302251";
                    if (gatetype == "1")
                    {
                        strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + " exited from  Security gate  on " + date + " at " + outinexittime;
                    }
                    else if (gatetype == "0")
                    {
                        strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + "  entered from  Security gate  in " + date + " at " + outinentrytime;
                    }
                    if (Mobli != "")//barath21.04.17
                    {
                        d2.send_sms(user_id, colg, UserCode, Mobli, strmsg, "0");
                        //barath 20.04.17
                        //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                        //smsreport(strpath, isst);
                    }
                    
                }


            }
            else
            {
                if (detsql != "")
                {
                    staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                    DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                    if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                    {
                        stafname = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                        code1 = Convert.ToString(steafs.Tables[0].Rows[0]["staff_code"]);

                        Mobli = Convert.ToString(steafs.Tables[0].Rows[0]["com_mobileno"]);
                      //  Mobli = "9487302251";
                        if (gatetype == "1")
                        {
                            strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + " exited from  Security gate  on " + date + " at " + outinexittime;
                        }
                        else if (gatetype == "0")
                        {
                            strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + "  entered from  Security gate  in " + date + " at " + outinentrytime;
                        }
                        
                        if (Mobli != "")//barath21.04.17
                        {
                            int d1=d2.send_sms(user_id, colg, UserCode, Mobli, strmsg, "0");
                            //barath 20.04.17
                            //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                            //smsreport(strpath, isst);
                        }
                    }

                }
            }
        }
        else
        {
            if (reqpk != "")
                        {
                            dep = d2.GetFunction("select MeetDeptCode from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                            if (dep.Contains(','))
                            {
                                string[] spl = dep.Split(',');



                                if (spl.Length > 0)
                                {
                                    for (int i = 0; i < spl.Length; i++)
                                    {
                                        if (depname == "")
                                            depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'");
                                        else
                                            depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + spl[i] + "'") + ',' + depname;


                                    }
                                }
                                else
                                    depname = d2.GetFunction("select Dept_Name  from Department where Dept_Code='" + dep + "'");

                            }

                            if (reqpk != "")
                                stf = d2.GetFunction("select MeetStaffAppNo from  RQ_Requisition where RequisitionPK='" + reqpk + "'");
                            if (stf.Contains(','))
                            {
                                string[] spl = stf.Split(',');
                                if (spl.Length > 0)
                                {
                                    for (int i = 0; i < spl.Length; i++)
                                    {
                                        string sql = "select * from staffmaster where appl_no='" + spl[i] + "'";

                                        steaff = d2.select_method_wo_parameter(sql, "text");

                                        if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                                        {

                                            stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                            code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                                        }
                                        if (stafname == "")
                                        {
                                            stafname = stfname;
                                            code1 = code;

                                        }
                                        else
                                        {
                                            stafname = stafname + ',' + stfname;
                                            code1 = code1 + ',' + code;
                                        }
                                        string sql1 = "select * from staff_appl_master where appl_no='" + spl[i] + "'";
                                        steaff1 = d2.select_method_wo_parameter(sql1, "text");
                                        if (steaff1.Tables.Count > 0 && steaff1.Tables[0].Rows.Count > 0)
                                        {

                                            MOb = Convert.ToString(steaff1.Tables[0].Rows[0]["com_mobileno"]);
                                           // Mobli = "9487302251";
                                            if (gatetype == "1")
                                            {
                                                strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + " exited from  Security gate  on " + date + " at " + outinexittime;
                                            }
                                            else if (gatetype == "0")
                                            {
                                                strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + "  entered from  Security gate  in " + date + " at " + outinentrytime;
                                            }
                                            if (MOb != "")//barath21.04.17
                                            {
                                                d2.send_sms(user_id, colg, UserCode, MOb, strmsg, "0");
                                                //barath 20.04.17
                                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                                //smsreport(strpath, isst);
                                            }

                                        }
                                        
                                    }
                                }

                            }
                            else
                            {
                                string sql = "select * from staffmaster where appl_no='" + stf + "'";

                                steaff = d2.select_method_wo_parameter(sql, "text");

                                if (steaff.Tables.Count > 0 && steaff.Tables[0].Rows.Count > 0)
                                {

                                    stfname = Convert.ToString(steaff.Tables[0].Rows[0]["staff_name"]);
                                    code = Convert.ToString(steaff.Tables[0].Rows[0]["staff_code"]);
                                }
                                if (stafname == "")
                                {
                                    stafname = stfname;
                                    code1 = code;

                                }
                                string sql1 = "select * from staff_appl_master where appl_no='" + stf + "'";
                                steaff1 = d2.select_method_wo_parameter(sql1, "text");
                                if (steaff1.Tables.Count > 0 && steaff1.Tables[0].Rows.Count > 0)
                                {

                                    MOb = Convert.ToString(steaff1.Tables[0].Rows[0]["com_mobileno"]);
                                    Mobli = MOb;
                                   // Mobli = "9487302251";
                                    if (gatetype == "1")
                                    {
                                        strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + " exited from  Security gate  on " + date + " at " + outinexittime;
                                    }
                                    else if (gatetype == "0")
                                    {
                                        strmsg = " Dear Mr/Mrs. " + stafname + "  you have got a Visitor- " + visitorname + " and " + visitormobli + "  entered from  Security gate  in " + date + " at " + outinentrytime;
                                    }
                                    if (Mobli != "")//barath21.04.17
                                    {
                                        d2.send_sms(user_id, colg, UserCode, Mobli, strmsg, "0");
                                        //barath 20.04.17
                                        //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                                        //smsreport(strpath, isst);
                                    }

                                }
                            }
                        }
                    }
           
    }

    protected void btn_materialok_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["btntype"] = "5";
            div_staff.Attributes.Add("style", "display:none");
            div_student.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_visitor.Attributes.Add("style", "display:none");
            div_material.Attributes.Add("style", "display:block");
            div_vehicle.Attributes.Add("style", "display:none");
            menutype = 5;
            string gdate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime gatepassdate = new DateTime();
            string[] split = gdate.Split('/');
            gatepassdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string gatepasstime = DateTime.Now.ToLongTimeString();
            string sql = "";
            string detsql = "";
            string purpose = txt_visit1.Text;
            int IsClgeVehicle = 0;
            string vehtype = "";
            string vehid = "";
            string vehregno = "";
            string broughtby = "";
            int materialtype = 0;
            string PONo = "";
            string suppliercode = "";
            string mobno = "";
            int query = 0;
            int gatetype;
            if (rb_parin.Checked == true)
            {
                gatetype = 0;
            }
            else
            {
                gatetype = 1;
            }
            if (rb_ordmaterial.Checked == true)
            {
                materialtype = 0;
                PONo = txt_purordno.Text;
                string supname = txt_suppliername.Text;
                suppliercode = d2.GetFunction("select vendor_code from vendor_details where vendor_name='" + supname + "'");
                if (rb_materialinsveh.Checked == true)
                {
                    IsClgeVehicle = 0;
                    vehid = txt_vehno.Text;
                    vehtype = txt_vehitype.Text;
                }
                else if (rb_materialotherveh.Checked == true)
                {
                    IsClgeVehicle = 1;
                    vehregno = txt_vehino.Text;
                    vehtype = txt_vehitype1.Text;
                    broughtby = txt_bbyname.Text;
                    mobno = txt_mobno1.Text;
                }
                sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,IsCollVeh,VehType,VehId,VehRegNo,MobileNo,BroughtBy,MaterialType,PONo,SupplierCode,College_Code) values('" + menutype + "','" + gatetype + "','" + gatepassdate + "','" + gatepasstime + "','" + IsClgeVehicle + "','" + vehtype + "','" + vehid + "','" + vehregno + "','" + mobno + "','" + broughtby + "','" + materialtype + "','" + PONo + "','" + suppliercode + "','" + collegecode1 + "')";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                if (query != 0)
                {
                    div_material.Attributes.Add("style", "display:none");
                    imgdiv2.Attributes.Add("style", "display:block");
                    lbl_erroralert.Text = "Saved Successfully";
                }
            }
            else if (rb_other.Checked == true)
            {
                materialtype = 1;
                string itemname = txt_itemname.Text;
                string itemcode = d2.GetFunction("select item_code from item_master where item_name='" + itemname + "'");
                string qty = txt_qty.Text;
                string measure = txt_measure.Text;
                sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,MaterialType,College_Code) values('" + menutype + "','" + gatetype + "','" + gatepassdate + "','" + gatepasstime + "','" + materialtype + "','" + collegecode1 + "')";
                query = d2.update_method_wo_parameter(sql, "TEXT");
                string getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and GatePassDate='" + gatepassdate + "' and MaterialType='" + materialtype + "'");
                detsql = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,Item_Code,Item_Name,ItemQty,unit) values('" + getID + "','" + menutype + "','" + itemcode + "','" + itemname + "','" + qty + "','" + measure + "')";
                int query1 = d2.update_method_wo_parameter(detsql, "TEXT");
                if (query != 0 && query1 != 0)
                {
                    div_material.Attributes.Add("style", "display:none");
                    imgdiv2.Attributes.Add("style", "display:block");
                    lbl_erroralert.Text = "Saved Successfully";
                }
            }
            else if (rb_service.Checked == true)
            {
                materialtype = 2;
            }
            // ScriptManager.RegisterStartupScript(this, GetType(), "materialbtn", "materialbtn();", true); 
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_vehicleok_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["btntype"] = "6";
            div_staff.Attributes.Add("style", "display:none");
            div_student.Attributes.Add("style", "display:none");
            div_parent.Attributes.Add("style", "display:none");
            div_visitor.Attributes.Add("style", "display:none");
            div_material.Attributes.Add("style", "display:none");
            div_vehicle.Attributes.Add("style", "display:block");
            menutype = 6;
            string gdate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime gatepassdate = new DateTime();
            string[] split = gdate.Split('/');
            gatepassdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string gatepasstime = DateTime.Now.ToLongTimeString();
            DateTime expdate = new DateTime();
            expdate = TextToDate(txt_expctdate);
            string exptime = txt_exptime.Text;
            string sql = "";
            string detsql = "";
            int IsClgeVehicle = 0;
            int isapproval = 0;
            string vehid = "";
            string vehregno = "";
            string broughtby = "";
            string name = "";
            string mobno = "";
            string purpose = "";
            int gatetype;
            if (rb_parin.Checked == true)
            {
                gatetype = 0;
            }
            else
            {
                gatetype = 1;
            }
            if (rb_instuveh.Checked == true)
            {
                IsClgeVehicle = 0;
                vehid = txt_vehicleno2.Text;
                purpose = txt_purpos1.Text;
                string[] sname;
                string staffcode = "";
                if (rb_appstyes.Checked == true)
                {
                    isapproval = 0;
                    sname = txt_personname.Text.Split('-');
                    staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + sname[0] + "'");
                }
                else if (rb_appstno.Checked == true)
                {
                    isapproval = 1;
                }
                sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,IsApproval,ExpectedDate,ExpectedTime,purpose,IsCollVeh,VehID,College_Code) values('" + menutype + "','" + gatetype + "','" + gatepassdate + "','" + gatepasstime + "','" + isapproval + "','" + expdate + "','" + exptime + "','" + purpose + "','" + IsClgeVehicle + "','" + vehid + "','" + collegecode1 + "')";
                int query = d2.update_method_wo_parameter(sql, "TEXT");
                string getId = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and GatePassDate='" + gatepassdate + "' and IsCollVeh='" + IsClgeVehicle + "' and IsApproval='" + isapproval + "' and VehID='" + vehid + "' and College_code='" + collegecode1 + "'");
                detsql = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,Staff_code) values('" + getId + "','" + menutype + "','" + staffcode + "')";
                int query1 = d2.update_method_wo_parameter(detsql, "TEXT");
                if (query != 0)
                {
                    div_vehicle.Attributes.Add("style", "display:none");
                    imgdiv2.Attributes.Add("style", "display:block");
                    lbl_erroralert.Text = "Saved Successfully";
                }
            }
            else if (rb_otherveh.Checked == true)
            {
                IsClgeVehicle = 1;
                vehregno = txt_vehicleno1.Text;
                broughtby = txt_brotname.Text;
                //name = txt_name01.Text;
                mobno = txt_mblno1.Text;
                purpose = txt_purpose.Text;
                sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatePassTime,purpose,IsCollVeh,VehRegNo,BroughtBy,College_Code) values('" + menutype + "','" + gatetype + "','" + gatepassdate + "','" + gatepasstime + "','" + purpose + "','" + IsClgeVehicle + "','" + vehregno + "','" + broughtby + "','" + collegecode1 + "')";
                int query = d2.update_method_wo_parameter(sql, "TEXT");
                string getId = d2.GetFunction("select GateEntryExitID from GateEntryExit where GateMemType='" + menutype + "' and GatePassDate='" + gatepassdate + "' and IsCollVeh='" + IsClgeVehicle + "' and VehRegNo='" + vehregno + "' and College_code='" + collegecode1 + "'");
                detsql = "insert into GateEntryExitDet(GateEntryExitID,GateMemType,OtherName,MobileNo) values('" + getId + "','" + menutype + "','" + name + "','" + mobno + "')";
                int query1 = d2.update_method_wo_parameter(detsql, "TEXT");
                if (query != 0)
                {
                    div_vehicle.Attributes.Add("style", "display:none");
                    imgdiv2.Attributes.Add("style", "display:block");
                    lbl_erroralert.Text = "Saved Successfully";
                }
            }
            // ScriptManager.RegisterStartupScript(this, GetType(), "vehiclebtn", "vehiclebtn();", true); 
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnerrorclose_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Attributes.Add("style", "display:none");
            string btntype1 = ViewState["btntype"].ToString();
            // Response.Write("<script>alert('Server')</script>");
            if (btntype1 == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "checkrbowninst", "checkrbowninst();", true);
            }
            else if (btntype1 == "2")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "staffbtn", "staffbtn();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "checkstaffrbowninst", "checkstaffrbowninst();", true);
            }
            else if (btntype1 == "3")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "parentsbtn", "parentsbtn();", true);

                //ScriptManager.RegisterStartupScript(this, GetType(), "checkstudadmitmeet", "checkstudadmitmeet()", true);
            }
            else if (btntype1 == "4")
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "visitorbtn", "visitorbtn();", true);
                if (rb_visitin.Checked == true)
                {
                    print();
                }
                else
                {
                    TextBox1.Enabled = true;
                }
                txt_name4.Text = " ";
                txt_mno.Text = " ";
                txt_cty.Text = " ";
                txt_visit1.Text = " ";
                txt_vehno1.Text = " ";
                txt_vehtype.Text = " ";
                txt_dep.Text = " ";
                txt_desgn.Text = " ";
                txt_stat.Text = " ";
                txt_dis.Text = " ";
                txt_str.Text = " ";
                //  Newitem();


                //ScriptManager.RegisterStartupScript(this, GetType(), "visitorappoint", "visitorappoint();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "rbvisitormeet", "rbvisitormeet();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "visitorreturn", "visitorreturn();", true);
            }
            else if (btntype1 == "5")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "materialbtn", "materialbtn();", true);
                //ScriptManager.RegisterStartupScript(this, GetType(), "materialentryby", "materialentryby();", true);
            }
            else if (btntype1 == "6")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "vehiclebtn", "vehiclebtn();", true);
                // ScriptManager.RegisterStartupScript(this, GetType(), "vehicelapstatus", "vehicelapstatus();", true);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void txt_stafftime_TextChanged(object sender, EventArgs e)
    {
    }
    public void rb_staffappryes_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_staffapprno_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void txt_staffexpdate_TextChanged(object sende, EventArgs e)
    {
    }
    public void rb_staffown_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_staffinst_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_adm_stud_CheckedChanged(object sender, EventArgs e)
    {
        ddl_ampm.Enabled = true;
        ddl_hrs.Enabled = true;
        ddl_mins.Enabled = true;
        //div_adm_stud.Visible = true;
        //div_notadm_stud.Visible = false;
    }
    public void rb_notadm_stud_CheckedChanged(object sender, EventArgs e)
    {
        ddl_ampm.Enabled = false;
        ddl_hrs.Enabled = false;
        ddl_mins.Enabled = false;
        //div_adm_stud.Visible = false;
        //div_notadm_stud.Visible = true;
    }
    //public void rb_meetstaff_CheckedChanged(object sender, EventArgs e)
    //{
    //    div_meetstaff.Visible = true;
    //    div_meetoffice.Visible = false;
    //    div_meetothers.Visible = false;
    //    FpSpread1.Sheets[0].RowCount = 0;
    //    FpSpread1.Sheets[0].ColumnCount = 0;
    //    FpSpread1.CommandBar.Visible = false;
    //    FpSpread1.Sheets[0].AutoPostBack = true;
    //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
    //    FpSpread1.Sheets[0].RowHeader.Visible = false;
    //    FpSpread1.Sheets[0].ColumnCount = 6;
    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.White;
    //    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //    FpSpread1.Visible = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Mobile No";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Designation";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "name";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Gender";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Month&year";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
    //    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
    //}
    public void rb_meetoffice_CheckedChanged(object sender, EventArgs e)
    {
        //div_meetstaff.Visible = false;
        //div_meetothers.Visible = false;
        //div_meetoffice.Visible = true;
    }
    public void rb_meetothers_CheckedChanged(object sender, EventArgs e)
    {
        //div_meetstaff.Visible = false;
        //div_meetoffice.Visible = false;
        //div_meetothers.Visible = true;
    }
    public void rb_ordmaterial_CheckedChanged(object sender, EventArgs e)
    {
        //div_ordermaterial.Visible = true;
        //div_material_others.Visible = false;
    }
    public void rb_other_CheckedChanged(object sender, EventArgs e)
    {
        //div_ordermaterial.Visible = false;
        //div_material_others.Visible = true;
    }
    public void rb_service_CheckedChanged(object sender, EventArgs e)
    {
        //div_ordermaterial.Visible = false;
        //div_material_others.Visible = false;
    }
    public void rb_others_CheckedChanged(object sender, EventArgs e)
    {
        //div_metr_entryby.Visible = false;
        //div_metr_others.Visible = true;
    }
    public void rb_instvehicle_CheckedChanged(object sender, EventArgs e)
    {
        //div_metr_entryby.Visible = true;
        //div_metr_others.Visible = false;
    }
    public void rb_company_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_individual_CheckedChanged(object sender, EventArgs e)
    {
        //TextBox1.Text = hid.Value;
    }
    public void rb_withap_CheckedChanged(object sender, EventArgs e)
    {
        //div_withappoint.Visible = true;
        //div_withoutappoint.Visible = false;
    }
    public void rb_withoutap_CheckedChanged(object sender, EventArgs e)
    {
        //div_withappoint.Visible = false;
        //div_withoutappoint.Visible = true;
    }
    public void rb_ret_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_notret_CheckedChanged(object sender, EventArgs e)
    {
    }
    public void rb_vehyes_CheckedChanged(object sender, EventArgs e)
    {
        //div_vehyes.Visible = true;
    }
    public void rb_vehno_CheckedChanged(object sender, EventArgs e)
    {
        // div_vehyes.Visible = false;
    }
    public void rb_instuveh_CheckedChanged(object sender, EventArgs e)
    {
        //div_instvehicle.Visible = true;
        //div_othervehicle.Visible = false;
    }
    public void rb_otherveh_CheckedChanged(object sender, EventArgs e)
    {
        //div_instvehicle.Visible = false;
        //div_othervehicle.Visible = true;
    }
    public void txt_expctdate_TextChanged(object sender, EventArgs e)
    {
    }
    public void txt_expcttime_TextChanged(object sender, EventArgs e)
    {
    }
    public void rb_appstyes_CheckedChanged(object sender, EventArgs e)
    {
        //appstatus_yes.Visible = true;
    }
    public void rb_appstno_CheckedChanged(object sender, EventArgs e)
    {
        // appstatus_yes.Visible = false;
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {
    }
    [WebMethod]
    public static string CheckRollNo(string Roll_No)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Roll_No.Trim() != "")
            {
                string query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No = '" + Roll_No + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [WebMethod]
    public static string CheckSmNo(string SmartNo)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (SmartNo.Trim() != "")
            {
                string query = "select smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no = '" + SmartNo + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        string cours = string.Empty;
        string deptt = string.Empty;
        if (deptar != "")
            {
                string[] spl = deptar.Split('-');
             cours=spl[0];
             deptt = spl[1];
         }
        if (gatepassrights == "3")
        {
            if (deptar != "")
             query = "select r.Roll_No from Registration r,applyn a,Degree d,course c,Department dt where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and c.Course_Name='" + cours + "' and dt.Dept_Name='" + deptt + "' and  Roll_No like '" + prefixText + "%'";
            else
                query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        else if (gatepassrights == "2")
        {
            if (deptar != "")
              query = "select r.Roll_No from Registration r,applyn a,Degree d,course c,Department dt where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and c.Course_Name='" + cours + "' and dt.Dept_Name='" + deptt + "' and Stud_Type='Day Scholar' and Roll_No like '" + prefixText + "%'";

             else
                query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        else if (gatepassrights == "1")
        {
            if (deptar != "")
              query = "select r.Roll_No from Registration r,applyn a,Degree d,course c,Department dt where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and c.Course_Name='" + cours + "' and dt.Dept_Name='" + deptt + "' and Stud_Type='Hostler' and Roll_No like '" + prefixText + "%'";
            else
                query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        }
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrnostud(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select a.stud_name+'-'+a.parent_name+'-'+c.Course_Name+'-'+dt.Dept_Name from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        //select a.stud_name from applyn a, Registration r where a.app_no=r.app_no and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static Student[] getData(string Roll_No)
    {
        string data = string.Empty;
        List<Student> details = new List<Student>();
        try
        {
            string query = "";
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
            string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            string appno = dd.GetFunction("select App_No from Registration where Roll_No='" + Roll_No + "'");
            if (inroll == "1")
            {
                string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                gatepasspk = rq_pk;
                query = " select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK =" + gatepasspk + "";
            }
            else
            {
                string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='1'  and GatepassEntrydate <='" + date + "'  and App_No='" + appno + "' and GateType ='1'");
                gatepasspk = rq_pk;
                query = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,gg.GatepassExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GatereqEntryDate,103) as 'GateReqEntryDate',g.GatereqEntrytime ,ReqAppStaffAppNo from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g,GateEntryExit gg where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1'";
            }
            ds = dd.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    Student s = new Student();
                    string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
                    string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                    string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                    s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                    img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                    s.photo = Convert.ToString(img2.ImageUrl);
                    s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                    s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                    s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                    s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                    s.statusmsg = "0";
                    // s.staffcode = ds.Tables[0].Rows[a]["approval_staff"].ToString();
                    s.staffname = staffname + "-" + code;
                    s.staffdept = dept;
                    s.staffdesg = desgn;
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                    s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                    s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                    s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                    s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                    s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();

                    s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                    imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                    s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                    imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                    s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                    imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                    s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                    details.Add(s);
                }
            }
            else
            {
                String sql = "select a.stud_name,r.app_no,r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and   r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and  r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and r.Roll_No='" + Roll_No + "'";
                ds = dd.select_method_wo_parameter(sql, "Text");
                Student s = new Student();
                s.Name = ds.Tables[0].Rows[0]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[0]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[0]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                s.RollNo = ds.Tables[0].Rows[0]["Roll_no"].ToString();
                img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                s.photo = Convert.ToString(img2.ImageUrl);
                s.Student_Type = ds.Tables[0].Rows[0]["Stud_Type"].ToString();
                s.Degree = ds.Tables[0].Rows[0]["Course_Name"].ToString();
                s.Department = ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                s.Semester = ds.Tables[0].Rows[0]["Current_Semester"].ToString();
                s.Section = ds.Tables[0].Rows[0]["Sections"].ToString();
                s.statusmsg = "1";
                s.staffcode = "";
                s.staffname = "";
                s.staffdept = "";
                s.staffdesg = "";
                imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                s.staffphoto = "";
                s.appdateExit = "";
                s.apptimeExit = "";
                s.appdateEntry = "";
                s.apptimeEntry = "";
                s.AppNo = ds.Tables[0].Rows[0]["app_no"].ToString();
                imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                details.Add(s);
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static Student[] studentSmartCard(string Smart_No, string j)
    {
        string data = string.Empty;
        string usercode = j;
        List<Student> details = new List<Student>();
        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
        try
        {
            if (Smart_No.Trim() != "" && Smart_No.Length >= 10)
            {
                string query = "";
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                string appno = dd.GetFunction("select App_No from Registration where  smart_serial_no='" + Smart_No + "'");//stud_type='Hostler' and
                string gatepassperimissiontype = dd.GetFunction("select value from Master_Settings where settings='Gatepass Request Type'  and usercode='" + usercode + "'");
                //barath09.11.16
                if (gatepassperimissiontype.Trim() == "0")
                {
                    #region With Request
                    string pkcheck = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                    string inn = dd.GetFunction(" select GateType from RQ_Requisition r , GateEntryExit g where g.App_No=r.ReqAppNo and RequestType='6' and ReqAppStatus='1' and ReqAppNo='" + appno + "' and RequisitionPK='" + pkcheck + "' and GateType='1' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK");
                    if (inn == "0")
                    {
                        inroll = "1";
                    }
                    else
                    {
                        inroll = "0";
                    }
                    if (inroll == "1")
                    {
                        string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                        gatepasspk = rq_pk;
                        query = " select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo,(select mastervalue from co_mastervalues where mastercriteria='GRRea' and mastercode=g.gatereqreason)gatereqreason ,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK =" + gatepasspk + "";
                    }
                    else
                    {
                        string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='1' and App_No='" + appno + "' and GateType ='1'");
                        gatepasspk = rq_pk;
                        query = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,gg.GatepassExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GatereqEntryDate,103) as 'GateReqEntryDate',g.GatereqEntrytime ,ReqAppStaffAppNo,(select mastervalue from co_mastervalues where mastercriteria='GRRea' and mastercode=g.gatereqreason)gatereqreason ,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g,GateEntryExit gg where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1'";
                    }
                    ds = dd.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && gatepasspk.Trim() != "" && ds.Tables[0].Rows.Count > 0)
                    {
                        string staffname = string.Empty;
                        string dept = string.Empty;
                        string staff_code = string.Empty;
                        string desgn = string.Empty;
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            Student s = new Student();
                            string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
                            //string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            //string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                            string Q1 = "select sa.appl_id,sm.staff_code,sa.dept_name,sa.desig_name,sa.appl_name from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'";
                            DataSet staffDetails = new DataSet();
                            staffDetails = dd.select_method_wo_parameter(Q1, "Text");
                            if (staffDetails.Tables != null && staffDetails.Tables[0].Rows.Count > 0)
                            {
                                dept = Convert.ToString(staffDetails.Tables[0].Rows[0]["dept_name"]);
                                desgn = Convert.ToString(staffDetails.Tables[0].Rows[0]["desig_name"]);
                                staff_code = Convert.ToString(staffDetails.Tables[0].Rows[0]["staff_code"]);
                                staffname = Convert.ToString(staffDetails.Tables[0].Rows[0]["appl_name"]);
                            }
                            string purpose = Convert.ToString(ds.Tables[0].Rows[a]["gatereqreason"]);
                            s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                            s.photo = Convert.ToString(img2.ImageUrl);
                            s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                            s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                            s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                            s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                            s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                            imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                            s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                            imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                            s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                            imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                            s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                            //s.statusmsg = Msg.ToString();
                            s.InOut = inroll;
                            string clgcode = Convert.ToString(ds.Tables[0].Rows[a]["college_code"]);
                            string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray = dnewdate.Split('/');
                            DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
                            // string dnewdate1 = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray1 = entry.Split('/');
                            DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
                            string appgateentydate = dd.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string appgateentrytime = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string lateentry = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
                            string appgateexitdate = dd.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string appgateexittime = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string[] split1 = appgateentrytime.Split(':');
                            string hr1 = split1[0];
                            string min1 = split1[1];
                            string day1 = split1[2];
                            int chr1 = Convert.ToInt32(hr1);
                            int cmin1 = Convert.ToInt32(min1);
                            string islate = "";
                            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Msg = "0";
                            string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
                            string hr = split[0];
                            string min = split[1];
                            string[] splitNew = min.Split(' ');
                            min = splitNew[0];
                            string day = splitNew[1];
                            int chr = Convert.ToInt32(hr);
                            int cmin = Convert.ToInt32(min);
                            string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                            if (appgateexitdate == currentdate)
                            {
                                string pk = dd.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                                if (inroll == "1")
                                {
                                    //string timecheck = d2.GetFunction("select count(GateReqExitTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqExitTime<'" + gatepastime + "'");
                                    string timecheck = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  RequisitionPK='" + gatepasspk + "'");
                                    string[] exittime = timecheck.Split(':');
                                    string gateexithr = exittime[0];
                                    string gateexitmin = exittime[1];
                                    string gateexitampm = exittime[2];
                                    if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && chr1 != 12)
                                        {
                                            Msg = "1";
                                        }
                                        //else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        //{
                                        //    Msg = "1";
                                        //}
                                    }
                                    else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (pk == "0" || pk == "" && Msg == "0")
                                    {
                                        string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk,College_Code,purpose) values('1','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + appno + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','0','" + gatepasspk + "','" + clgcode + "','" + purpose + "')";
                                        int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                        s.statusmsg = "0";
                                        sms(gatepasspk, appno, "1", clgcode);
                                    }
                                    else
                                    {
                                        Msg = "1";
                                        s.statusmsg = "1";
                                    }
                                }
                                else
                                {
                                    string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                    if (outcheck == "0" || outcheck == "False")
                                    {
                                        s.statusmsg = "1";
                                    }
                                    if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        islate = "1";
                                        s.statusmsg = "0";
                                    }
                                    else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        if (chr > chr1 && day1 == day)
                                        {
                                            islate = "1";
                                            s.statusmsg = "0";
                                        }
                                        else if (chr == chr1 && cmin > cmin1 && day1 == day)
                                        {
                                            islate = "1";
                                            s.statusmsg = "0";
                                        }
                                        else
                                        {
                                            islate = "0";
                                            s.statusmsg = "0";
                                        }
                                    }
                                    string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and RequestFk='" + pk + "'";
                                    //query = d2.update_method_wo_parameter(sql, "TEXT");
                                    //sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + gatepassexitdate + "'";
                                    int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                    if (qu > 0)
                                    {
                                        sms(gatepasspk, appno, "2", clgcode);
                                    }
                                }
                            }
                            // s.staffcode = ds.Tables[0].Rows[a]["approval_staff"].ToString();
                            s.staffname = staffname + "-" + staff_code;
                            s.staffdept = dept;
                            s.staffdesg = desgn;
                            imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                            s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                            s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                            s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                            s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            s.purpose = purpose;
                            details.Add(s);
                        }
                    }
                    else
                    {
                        Student s = new Student();
                        s.Name = "";
                        s.RollNo = "";
                        img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                        s.photo = Convert.ToString(img2.ImageUrl);
                        s.Student_Type = "";
                        s.Degree = "";
                        s.Department = "";
                        s.Semester = "";
                        s.Section = "";
                        //s.statusmsg = "3";
                        if (appno.Trim() == "" || appno.Trim() == "0")
                        {
                            s.statusmsg = "3";
                        }
                        else if (gatepasspk.Trim() == "")
                        {
                            s.statusmsg = "2";
                        }
                        s.staffcode = "";
                        s.staffname = "";
                        s.staffdept = "";
                        s.staffdesg = "";
                        imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                        s.staffphoto = "";
                        s.appdateExit = "";
                        s.apptimeExit = "";
                        s.appdateEntry = "";
                        s.apptimeEntry = "";
                        s.purpose = "";
                        imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                        imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                        imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;

                        details.Add(s);
                    }
                    #endregion
                }
                else
                {
                    #region With out Request
                    if (appno.Trim() != "0")
                    {
                        string inn = dd.GetFunction("select GateType from GateEntryExit g where app_no='" + appno + "' and GateType='1' and gatepassexitdate='" + date + "'");
                        if (inn == "0")
                        {
                            inroll = "1";
                        }
                        else
                        {
                            inroll = "0";
                        }
                        query = "  select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name,r.college_code   from applyn a,Registration r ,Degree d,course c,Department dt where   a.app_no=r.app_no  and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and r.app_no='" + appno + "' ";
                        ds = dd.select_method_wo_parameter(query, "Text");

                        string Msg = "0"; Student s = new Student();
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                                {
                                    s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                                    s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                                    img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                                    s.photo = Convert.ToString(img2.ImageUrl);
                                    s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                                    s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                                    s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                                    s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                                    s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();

                                    s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                                    imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                                    s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                                    imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                                    s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                                    imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                                    s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                                    //s.statusmsg = Msg.ToString();
                                    s.InOut = inroll;
                                    string clgcode = Convert.ToString(ds.Tables[0].Rows[a]["college_code"]);//dd.GetFunction("select CollegeCode from HT_HostelRegistration where APP_No='" + appno + "'");
                                    if (inroll == "1")
                                    {
                                        string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk,College_Code) values('1','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm tt") + "','" + appno + "','1','0','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:tt") + "','0','" + gatepasspk + "','" + clgcode + "')";
                                        int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                        s.statusmsg = "0";
                                        sms(gatepasspk, appno, "1", clgcode);
                                        Msg = "0";
                                    }
                                    else
                                    {
                                        string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                        if (outcheck == "0" || outcheck == "False")
                                        {
                                            s.statusmsg = "1";
                                        }
                                        string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='1',GatepassEntrytime='" + DateTime.Now.ToString("hh:mm tt") + "',GateType='0' where App_No='" + appno + "' and GateMemType='1' and GateType='1' ";
                                        int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                        if (qu > 0)
                                        {
                                            sms(gatepasspk, appno, "2", clgcode);
                                        }
                                        s.statusmsg = "0";
                                    }
                                    //s.staffname = staffname + "-" + code;
                                    //s.staffdept = dept;
                                    //s.staffdesg = desgn;
                                    //imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                                    //s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                                    //s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                                    //s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                                    //s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                                    //s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                                    details.Add(s);
                                }
                            }
                            else
                            {
                                Msg = "1";
                                s.statusmsg = "1";
                                details.Add(s);
                            }
                        }
                        else
                        {
                            Msg = "1";
                            s.statusmsg = "1";
                            details.Add(s);
                        }
                    }
                    else
                    {
                        Student s = new Student();
                        s.Name = "";
                        s.RollNo = "";
                        //img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                        //s.photo = Convert.ToString(img2.ImageUrl);
                        s.Student_Type = "";
                        s.Degree = "";
                        s.Department = "";
                        s.Semester = "";
                        s.Section = "";
                        s.statusmsg = "2";
                        s.staffcode = "";
                        s.staffname = "";
                        s.staffdept = "";
                        s.staffdesg = "";
                        //imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                        s.staffphoto = "";
                        s.appdateExit = "";
                        s.apptimeExit = "";
                        s.appdateEntry = "";
                        s.apptimeEntry = "";
                        s.purpose = "";
                        details.Add(s);
                    }
                    #endregion
                }
            }
            return details.ToArray();
        }
        catch
        {
            Student s = new Student();
            s.Name = "";
            s.RollNo = "";
            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
            s.photo = Convert.ToString(img2.ImageUrl);
            s.Student_Type = "";
            s.Degree = "";
            s.Department = "";
            s.Semester = "";
            s.Section = "";
            s.statusmsg = "2";
            s.staffcode = "";
            s.staffname = "";
            s.staffdept = "";
            s.staffdesg = "";
            imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
            s.staffphoto = "";
            s.appdateExit = "";
            s.apptimeExit = "";
            s.appdateEntry = "";
            s.apptimeEntry = "";
            s.purpose = "";
            imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
            imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
            imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;


            details.Add(s);
            return details.ToArray();
        }
    }
    [WebMethod]
    public static Student[] studroll(string RollNo, string j)
    {

        string data = string.Empty;
      //  j = "30";
        string usercode = j;
        List<Student> details = new List<Student>();
        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
        //Added By Saranyadevi 4.2.2018
        System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
        try
        {
            if (RollNo.Trim() != "")
            {
                string query = "";
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                string reques=string.Empty;
                string deptpp = string.Empty;
                deptpp = RollNo;
                string[] spl = deptpp.Split('-');
                if (spl.Length > 2)
                    RollNo = spl[1];
                string appno = dd.GetFunction("select app_no from Registration where  roll_no='" + RollNo.Trim() + "'");//stud_type='Hostler' and
                string gatepassperimissiontype = dd.GetFunction("select value from Master_Settings where settings='Gatepass Request Type' and usercode='" + usercode + "'");
                if (gatepassperimissiontype.Trim() == "0")
                {
                    #region With request
                    string pkcheck = string.Empty;

                    //pkcheck = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='0' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                    string checkper =dd.GetFunction("select value from Master_Settings where settings='Leave Approval Permission' and usercode='" + usercode + "' ");
                   
                         reques=" and ReqAppStatus=1";
                    pkcheck = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 " + reques + "  and ReqAppNo='" + appno + "'");//and ReqAppStatus='0'

                    string inn = dd.GetFunction(" select GateType from RQ_Requisition r , GateEntryExit g where g.App_No=r.ReqAppNo and RequestType='6'  " + reques + "  and ReqAppNo='" + appno + "' and RequisitionPK='" + pkcheck + "' and GateType='1' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK");//and ReqAppStatus='0'
                    if (inn == "0")
                    {
                        inroll = "1";
                    }
                    else
                    {
                        inroll = "0";
                    }
                    if (inroll == "1")
                    {
                        //string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='0' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                        string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6  " + reques + "  and ReqAppNo='" + appno + "'");//and ReqAppStatus='0'
                        gatepasspk = rq_pk;
                        query = " select a.stud_name,r.app_no,  r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo,ReqStaffAppNo,(select mastervalue from co_mastervalues where mastercriteria='GRRea' and mastercode=g.gatereqreason)gatereqreason,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR'  " + reques + " and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK ='" + gatepasspk + "'";//and ReqAppStatus='0'
                    }
                    else
                    {
                        string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='1' and App_No='" + appno + "' and GateType ='1'");
                        gatepasspk = rq_pk;
                        query = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,gg.GatepassExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GatereqEntryDate,103) as 'GateReqEntryDate',g.GatereqEntrytime ,ReqAppStaffAppNo,ReqStaffAppNo,(select mastervalue from co_mastervalues where mastercriteria='GRRea' and mastercode=g.gatereqreason)gatereqreason,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g,GateEntryExit gg where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR'  and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1'";//and g.ReqAppStatus='0'
                    }
                    ds = dd.select_method_wo_parameter(query, "Text");
                    if (ds.Tables.Count > 0 && gatepasspk.Trim() != "" && gatepasspk.Trim() != "0" && ds.Tables[0].Rows.Count > 0)
                    {
                        string staffname = string.Empty;
                        string dept = string.Empty;
                        string staff_code = string.Empty;
                        string desgn = string.Empty;
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            Student s = new Student();
                            string staff_applid = ds.Tables[0].Rows[a]["ReqStaffAppNo"].ToString();
                            //string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            //string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                            string Q1 = "select sa.appl_id,sm.staff_code,sa.dept_name,sa.desig_name,sa.appl_name from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'";
                            DataSet staffDetails = new DataSet();
                            staffDetails = dd.select_method_wo_parameter(Q1, "Text");
                            if (staffDetails.Tables != null && staffDetails.Tables[0].Rows.Count > 0)
                            {
                                dept = Convert.ToString(staffDetails.Tables[0].Rows[0]["dept_name"]);
                                desgn = Convert.ToString(staffDetails.Tables[0].Rows[0]["desig_name"]);
                                staff_code = Convert.ToString(staffDetails.Tables[0].Rows[0]["staff_code"]);
                                staffname = Convert.ToString(staffDetails.Tables[0].Rows[0]["appl_name"]);
                            }
                            string purpose = Convert.ToString(ds.Tables[0].Rows[a]["gatereqreason"]);
                            s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Roll_no"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                         

                            s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                            img2.ImageUrl = "../Handler/Handler4.ashx?rollno=" + s.RollNo;
                            s.photo = Convert.ToString(img2.ImageUrl);
                            s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                            s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                            s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                            s.studept = s.Degree + '-' + s.Department;
                            s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                            string clgcode = ds.Tables[0].Rows[a]["college_code"].ToString();
                            s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                            imagefar.ImageUrl = "../Handler/Handler7.ashx?id=" + s.AppNo;
                            imagefar.ImageUrl = "../Handler/Handler7.png?id=" + s.AppNo;
                            s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                            imagemon.ImageUrl = "../Handler/Handler8.ashx?id=" + s.AppNo;
                            s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                            imageguar.ImageUrl = "../Handler/Handler9.ashx?id=" + s.AppNo;
                            s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                         
                            //s.statusmsg = Msg.ToString();
                            s.InOut = inroll;
                            string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray = dnewdate.Split('/');
                            DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
                            // string dnewdate1 = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray1 = entry.Split('/');
                            DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
                            string appgateentydate = dd.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'");//and  MONTH(RequestDate)=MONTH(GateReqEntryDate) 
                            string appgateentrytime = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "' ");//and  MONTH(RequestDate)=MONTH(GateReqEntryDate)
                            string lateentry = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
                            string appgateexitdate = dd.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "' and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");//and  MONTH(RequestDate)=MONTH(GateReqExitDate)
                            string appgateexittime = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "' and  MONTH(RequestDate)=MONTH(GateReqExitDate)");//and  MONTH(RequestDate)=MONTH(GateReqExitDate)
                            string[] split1 = appgateentrytime.Split(':');
                            string hr1 = split1[0];
                            string min1 = split1[1];
                            string day1 = split1[2];
                            int chr1 = Convert.ToInt32(hr1);
                            int cmin1 = Convert.ToInt32(min1);
                            string islate = "";
                            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Msg = "0";
                            string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
                            string hr = split[0];
                            string min = split[1];
                            string[] splitNew = min.Split(' ');
                            min = splitNew[0];
                            string day = splitNew[1];
                            int chr = Convert.ToInt32(hr);
                            int cmin = Convert.ToInt32(min);
                            string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));

                            //magesh 2.6.18
                            string[] splitti = Convert.ToString(DateTime.Now.ToString("hh:mm:tt")).Split(':');
                            string dats = splitti[2];
                            string hrs = splitti[0];
                            string mins = splitti[1];
                            string[] split2 = appgateexittime.Split(':');
                            string hr2 = split2[0];
                            string min2 = split2[1];
                            string day2 = split2[2];
                            int hour = 0;
                            int.TryParse(hr2, out hour);
                            int minsu = 0;
                            int.TryParse(min2, out minsu);
                            int hours = 0;
                            int.TryParse(hrs, out hours);
                            int minsus = 0;
                            int.TryParse(mins, out minsus);
                 
                            //if (appgateexitdate == currentdate)
                            //{
                                string pk = dd.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                                if (sec == 0)
                                {
                                    string timercon = dd.GetFunction("select LinkValue from New_InsSettings where LinkName='gatepass biobased' and user_code ='" + usercode + "' and college_code ='" + collegecodeee + "'");
                                    if (timercon == "0")
                                    {
                                        sec = 1;
                                    }
                                    if (timercon == "1")
                                    {
                                        sec = 0;

                                    }
                                    if (inroll == "1")
                                    {
                                        if (appgateexitdate == currentdate)
                                        {

                                            if ((dats == "AM" && day2 == "PM") || (day2 == dats && hour < hours) || (day2 == dats && minsu <= minsus && hour == hours))
                                            {
                                                if (right == "In")
                                                {
                                                    //string timecheck = d2.GetFunction("select count(GateReqExitTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqExitTime<'" + gatepastime + "'");
                                                    string timecheck = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  RequisitionPK='" + gatepasspk + "'");
                                                    string[] exittime = timecheck.Split(':');
                                                    string gateexithr = exittime[0];
                                                    string gateexitmin = exittime[1];
                                                    string gateexitampm = exittime[2];
                                                    if (appgateexitdate == appgateentydate)
                                                    {
                                                        if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                                                        {
                                                            if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
                                                            {
                                                                Msg = "1";
                                                            }
                                                            else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
                                                            {
                                                                Msg = "1";
                                                            }
                                                        }
                                                        else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
                                                        {
                                                            if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
                                                            {
                                                                Msg = "1";
                                                            }
                                                            else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
                                                            {
                                                                Msg = "1";
                                                            }
                                                        }
                                                        if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                                        {
                                                            if (chr != 12 && chr1 != 12)
                                                            {
                                                                Msg = "1";
                                                            }
                                                            //else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                                            //{
                                                            //    Msg = "1";
                                                            //}
                                                        }
                                                        else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
                                                        {
                                                            if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
                                                            {
                                                                Msg = "1";
                                                            }
                                                            else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                                            {
                                                                Msg = "1";
                                                            }
                                                        }
                                                        if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                                        {
                                                            if (gateexitampm == "AM" && day1 == "PM")
                                                            {
                                                                Msg = "0";
                                                            }
                                                           
                                                            //else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                                            //{
                                                            //    Msg = "1";
                                                            //}
                                                        }

                                                    }
                                                    if (pk == "0" || pk == "" && Msg == "0")
                                                    {
                                                        //string clgcode = dd.GetFunction("select CollegeCode from HT_HostelRegistration where APP_No='" + appno + "'");
                                                        string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk,College_Code,purpose) values('1','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + appno + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','0','" + gatepasspk + "','" + clgcode + "','" + purpose + "')";
                                                        int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                                        s.statusmsg = "0";

                                                        sms(gatepasspk, appno, "1", clgcode);

                                                    }
                                                    else
                                                    {
                                                        Msg = "1";
                                                        s.statusmsg = "1";

                                                    }
                                                }

                                            }
                                            else
                                            {
                                                Msg = "1";
                                                s.statusmsg = "1";


                                            }
                                        }

                                    }
                                    else
                                    {
                                        //if (rb_in.Checked == true)
                                        //    right = "In";
                                        if (right == "out")
                                        {
                                            string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                            if (outcheck == "0" || outcheck == "False")
                                            {
                                                s.statusmsg = "1";

                                            }
                                            if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                            {
                                                islate = "1";
                                                s.statusmsg = "0";

                                            }
                                            else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                            {
                                                if (chr > chr1 && day1 == day)
                                                {
                                                    islate = "1";
                                                    s.statusmsg = "0";

                                                }
                                                else if (chr == chr1 && cmin > cmin1 && day1 == day)
                                                {
                                                    islate = "1";
                                                    s.statusmsg = "0";

                                                }
                                                else
                                                {
                                                    islate = "0";
                                                    s.statusmsg = "0";

                                                }
                                            }
                                            string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and RequestFk='" + pk + "'";
                                            //query = d2.update_method_wo_parameter(sql, "TEXT");
                                            //sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + gatepassexitdate + "'";
                                            int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                            if (qu > 0)
                                            {
                                                sms(gatepasspk, appno, "2", clgcode);
                                            }
                                            s.statusmsg = "0";


                                        }
                                    }
                                }
                                else
                                {
                                    Msg = "1";
                                    s.statusmsg = "5";

                                    details.Add(s);
                                }
                            
                            // s.staffcode = ds.Tables[0].Rows[a]["approval_staff"].ToString();
                            s.staffname = staffname + "-" + staff_code;
                            s.staffdept = dept;
                            s.staffdesg = desgn;
                            imagestaff.ImageUrl = "../Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                            s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                            s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                            s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                            s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            s.purpose = purpose;
                            details.Add(s);
                        }
                    }
                    else
                    {
                        Student s = new Student();
                        s.Name = "";
                        s.RollNo = "";
                        img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                        s.photo = Convert.ToString(img2.ImageUrl);
                        s.Student_Type = "";
                        s.Degree = "";
                        s.Department = "";
                        s.Semester = "";
                        s.Section = "";
                        //s.statusmsg = "3";
                        //if (appno.Trim() == "" || appno.Trim() == "0")
                        //{
                        //    s.statusmsg = "3";
                        //}
                        //else if (gatepasspk.Trim() == "")
                        //{
                        s.statusmsg = "2";
                      
                        // }
                        s.staffcode = "";
                        s.staffname = "";
                        s.staffdept = "";
                        s.staffdesg = "";
                        imagestaff.ImageUrl = "../Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                        s.staffphoto = "";
                        s.appdateExit = "";
                        s.apptimeExit = "";
                        s.appdateEntry = "";
                        s.apptimeEntry = "";
                        s.purpose = "";

                        imagefar.ImageUrl = "../Handler/Handler7.ashx?app_no=" + s.AppNo;
                        imagemon.ImageUrl = "../Handler/Handler8.ashx?app_no=" + s.AppNo;
                        imageguar.ImageUrl = "../Handler/Handler9.ashx?app_no=" + s.AppNo;


                        details.Add(s);
                    }
                    #endregion
                }
                //barath08.11.16
                if (gatepassperimissiontype.Trim() == "1")
                {
                    #region Without Request
                    if (appno.Trim() != "0")
                    {
                        string inn = dd.GetFunction("select GateType from GateEntryExit g where app_no='" + appno + "' and GateType='1' and gatepassexitdate='" + date + "'");
                        if (inn == "0")
                        {
                            inroll = "1";
                        }
                        else
                        {
                            inroll = "0";
                        }
                        query = "  select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt where   a.app_no=r.app_no  and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and r.app_no='" + appno + "' ";

                        ds = dd.select_method_wo_parameter(query, "Text");
                        string Msg = "0"; Student s = new Student();
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                                {
                                    s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Roll_no"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                                    s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                                    img2.ImageUrl = "../Handler/Handler4.ashx?rollno=" + s.RollNo;
                                    s.photo = Convert.ToString(img2.ImageUrl);
                                    s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                                    s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                                    s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                                    s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                                    s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                                    string clgcode = Convert.ToString(ds.Tables[0].Rows[a]["college_code"]);
                                    s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                                    imagefar.ImageUrl = "../Handler/Handler7.ashx?app_no=" + s.AppNo;
                                   // imagefar.ImageUrl = "../Handler/Handler7.png?app_no=" + s.AppNo;
                                    s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                                    imagemon.ImageUrl = "../Handler/Handler8.ashx?app_no=" + s.AppNo;
                                    s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                                    imageguar.ImageUrl = "../Handler/Handler9.ashx?app_no=" + s.AppNo;
                                    s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                                    s.studept = s.Degree + '-' + s.Department;
                                    //s.statusmsg = Msg.ToString();
                                    s.InOut = inroll;



                                    #region(stud photo)

                                    //string stdphtsql = string.Empty;
                                    //stdphtsql = "select * from StdPhoto where app_no='" + s.AppNo + "'";
                                    //MemoryStream memoryStream = new MemoryStream();
                                    //DataSet dsstdpho = new DataSet();
                                    //dsstdpho.Clear();
                                    //dsstdpho.Dispose();
                                    //dsstdpho = dd.select_method_wo_parameter(stdphtsql, "Text");
                                    //if (dsstdpho.Tables.Count > 0 && dsstdpho.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dsstdpho.Tables[0].Rows[0][1]).Trim()))
                                    //{
                                    //    byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                                    //    memoryStream.Write(file, 0, file.Length);
                                    //    if (file.Length > 0)
                                    //    {
                                    //        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    //        System.Drawing.Image thumb = imgx.GetThumbnailImage(190, 190, null, IntPtr.Zero);
                                    //        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + s.AppNo + ".jpeg")))
                                    //        {
                                                
                                    //        }
                                    //        else
                                    //        {
                                    //            thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + s.AppNo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //        }
                                    //    }
                                    //}
                                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + s.AppNo + ".jpeg")))
                                    //{
                                    //    imagefar.ImageUrl = "~/coeimages/" + s.AppNo + ".jpeg";
                                    //}
                                    //else
                                    //{
                                    //    imagefar.ImageUrl = "~/coeimages/NoImage.jpeg";
                                    //}
                                    #endregion
                                    //magesh 30.6.18
                                    
                                    
                                if(sec==0)
                                {
                                    string timercon = dd.GetFunction("select LinkValue from New_InsSettings where LinkName='gatepass biobased' and user_code ='" + usercode + "' and college_code ='" + collegecodeee + "'");
                                    if (timercon == "0")
                                    {
                                        sec = 1;
                                    }
                                    if (timercon == "1")
                                    {
                                        sec = 0;
                                       
                                    }
                                   
                                    if (inroll == "1")
                                    {
                                        //string clgcode = dd.GetFunction("select CollegeCode from HT_HostelRegistration where APP_No='" + appno + "'");
                                        string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk,College_Code) values('1','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm tt") + "','" + appno + "','1','0','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:tt") + "','0','" + gatepasspk + "','" + clgcode + "')";
                                        int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                        s.statusmsg = "0";
                                     
                                        sms(gatepasspk, appno, "1", clgcode);
                                        Msg = "0";
                                    }
                                    else
                                    {
                                        string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                        if (outcheck == "0" || outcheck == "False")
                                        {
                                            s.statusmsg = "1";
                                          
                                        }


                                        string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='1',GatepassEntrytime='" + DateTime.Now.ToString("hh:mm tt") + "',GateType='0' where App_No='" + appno + "' and GateMemType='1' and GateType='1' ";
                                        int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                        if (qu > 0)
                                        {
                                            gatepasspk = dd.GetFunction("select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'");
                                            sms(gatepasspk, appno, "2", clgcode);
                                        }
                                        s.statusmsg = "0";
                                       
                                    }
                                    //s.staffname = staffname + "-" + code;
                                    //s.staffdept = dept;
                                    //s.staffdesg = desgn;
                                    //imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                                    //s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                                    //s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                                    //s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                                    //s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                                    //s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                                    details.Add(s);
                                }
                                else
                                {
                                    Msg = "1";
                                    s.statusmsg = "5";
                                    s.appdateExit = DateTime.Now.ToString("hh:mm tt");
                                    s.apptimeExit = DateTime.Now.ToString("MM/dd/yyyy");

                                    details.Add(s);
                                }
                                     }//magesh 30.6.18
                      
                            }
                            else
                            {
                                Msg = "1";
                                s.statusmsg = "1";
                             
                                details.Add(s);
                            }
                        }
                        else
                        {
                            Msg = "1";
                            s.statusmsg = "1";
                          
                            details.Add(s);
                        }
                       
                    }
                    else
                    {
                        Student s = new Student();
                        s.Name = "";
                        s.RollNo = "";
                        //img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                        //s.photo = Convert.ToString(img2.ImageUrl);
                        s.Student_Type = "";
                        s.Degree = "";
                        s.Department = "";
                        s.Semester = "";
                        s.Section = "";
                        s.statusmsg = "2";
                        s.staffcode = "";
                        s.staffname = "";
                        s.staffdept = "";
                        s.staffdesg = "";
                        //imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                        s.staffphoto = "";
                        s.appdateExit = "";
                        s.apptimeExit = "";
                        s.appdateEntry = "";
                        s.apptimeEntry = "";
                        s.purpose = "";
                        details.Add(s);
                    }
                    #endregion
                }
              
            }
         
            return details.ToArray();
           
        }
        catch
        {
            Student s = new Student();
            s.Name = "";
            s.RollNo = "";
            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
            s.photo = Convert.ToString(img2.ImageUrl);
            s.Student_Type = "";
            s.Degree = "";
            s.Department = "";
            s.Semester = "";
            s.Section = "";
            s.statusmsg = "2";
            s.staffcode = "";
            s.staffname = "";
            s.staffdept = "";
            s.staffdesg = "";
            imagestaff.ImageUrl = "../Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
            s.staffphoto = "";
            s.appdateExit = "";
            s.apptimeExit = "";
            s.appdateEntry = "";
            s.apptimeEntry = "";
            s.purpose = "";
            imagefar.ImageUrl = "../Handler/Handler7.ashx?app_no=" + s.AppNo;
            imagemon.ImageUrl = "../Handler/Handler8.ashx?app_no=" + s.AppNo;
            imageguar.ImageUrl = "../Handler/Handler9.ashx?app_no=" + s.AppNo;

            details.Add(s);
            return details.ToArray();
        }
    }
    [WebMethod]
    public static Student[] studname(string Smart_No, string j)
    {
        string data = string.Empty;
        string jnew = j;
        List<Student> details = new List<Student>();
        try
        {
            if (Smart_No.Trim() != "")
            {
                string query = "";
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                string[] sp = Smart_No.Split('-');
                string appno = dd.GetFunction("select app_no from Registration where stud_name='" + sp[0] + "'");
                string pkcheck = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                //string outt = dd.GetFunction(" select GateType from RQ_Requisition r left join GateEntryExit g on g.App_No=r.ReqAppNo where g.App_No is null and RequestType='6' and ReqAppStatus='1' and ReqAppNo='" + appno + "' and RequisitionPK='" + pkcheck + "' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK");
                //if (outt == "0")
                //{
                //    inroll = "1";
                //}
                //else
                //{
                string inn = dd.GetFunction(" select GateType from RQ_Requisition r , GateEntryExit g where g.App_No=r.ReqAppNo and RequestType='6' and ReqAppStatus='1' and ReqAppNo='" + appno + "' and RequisitionPK='" + pkcheck + "' and GateType='1' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK");
                if (inn == "0")
                {
                    inroll = "1";
                }
                else
                {
                    inroll = "0";
                }
                // }
                if (inroll == "1")
                {
                    string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + appno + "'");
                    gatepasspk = rq_pk;
                    query = " select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK =" + gatepasspk + "";
                }
                else
                {
                    string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='1' and App_No='" + appno + "' and GateType ='1'");
                    gatepasspk = rq_pk;
                    query = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,gg.GatepassExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GatereqEntryDate,103) as 'GateReqEntryDate',g.GatereqEntrytime ,ReqAppStaffAppNo,r.college_code from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g,GateEntryExit gg where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1'";
                }
                ds = dd.select_method_wo_parameter(query, "Text");
                System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
                if (ds.Tables.Count > 0 && gatepasspk.Trim() != "" && gatepasspk.Trim() != "0")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            Student s = new Student();
                            string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
                            string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                            s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                            s.photo = Convert.ToString(img2.ImageUrl);
                            s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                            s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                            s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                            s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                            string clgcode = ds.Tables[0].Rows[a]["college_code"].ToString();

                            s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                            imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                            s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                            imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                            s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                            imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                            s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                            //s.statusmsg = Msg.ToString();
                            s.InOut = inroll;
                            string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray = dnewdate.Split('/');
                            DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
                            // string dnewdate1 = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray1 = entry.Split('/');
                            DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
                            string appgateentydate = dd.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string appgateentrytime = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string lateentry = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
                            string appgateexitdate = dd.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string appgateexittime = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + appno + "'))and ReqAppNo='" + appno + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string[] split1 = appgateentrytime.Split(':');
                            string hr1 = split1[0];
                            string min1 = split1[1];
                            string day1 = split1[2];
                            int chr1 = Convert.ToInt32(hr1);
                            int cmin1 = Convert.ToInt32(min1);
                            string islate = "";
                            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Msg = "0";
                            string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
                            string hr = split[0];
                            string min = split[1];
                            string[] splitNew = min.Split(' ');
                            min = splitNew[0];
                            string day = splitNew[1];
                            int chr = Convert.ToInt32(hr);
                            int cmin = Convert.ToInt32(min);
                            string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                            if (appgateexitdate == currentdate)
                            {
                                string pk = dd.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                                if (inroll == "1")
                                {
                                    //string timecheck = d2.GetFunction("select count(GateReqExitTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqExitTime<'" + gatepastime + "'");
                                    string timecheck = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  RequisitionPK='" + gatepasspk + "'");
                                    string[] exittime = timecheck.Split(':');
                                    string gateexithr = exittime[0];
                                    string gateexitmin = exittime[1];
                                    string gateexitampm = exittime[2];
                                    if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && chr1 != 12)
                                        {
                                            Msg = "1";
                                        }
                                        //else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        //{
                                        //    Msg = "1";
                                        //}
                                    }
                                    else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (pk == "0" || pk == "" && Msg == "0")
                                    {
                                        //string clgcode = dd.GetFunction("select CollegeCode from HT_HostelRegistration where APP_No='" + appno + "'");
                                        string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk,College_Code) values('1','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + appno + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','0','" + gatepasspk + "','" + clgcode + "')";
                                        int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                        s.statusmsg = "0";
                                        sms(gatepasspk, appno, "1", clgcode);
                                    }
                                    else
                                    {
                                        Msg = "1";
                                        s.statusmsg = "1";
                                    }
                                }
                                else
                                {
                                    string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
                                    if (outcheck == "0" || outcheck == "False")
                                    {
                                        s.statusmsg = "1";
                                    }
                                    if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        islate = "1";
                                        s.statusmsg = "0";
                                    }
                                    else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        if (chr > chr1 && day1 == day)
                                        {
                                            islate = "1";
                                            s.statusmsg = "0";
                                        }
                                        else if (chr == chr1 && cmin > cmin1 && day1 == day)
                                        {
                                            islate = "1";
                                            s.statusmsg = "0";
                                        }
                                        else
                                        {
                                            islate = "0";
                                            s.statusmsg = "0";
                                        }
                                    }
                                    string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and RequestFk='" + pk + "'";
                                    //query = d2.update_method_wo_parameter(sql, "TEXT");
                                    //sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + gatepassexitdate + "'";
                                    int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                    if (qu > 0)
                                    {
                                        sms(gatepasspk, appno, "2", clgcode);
                                    }
                                }
                            }
                            // s.staffcode = ds.Tables[0].Rows[a]["approval_staff"].ToString();
                            s.staffname = staffname + "-" + code;
                            s.staffdept = dept;
                            s.staffdesg = desgn;
                            imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                            s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                            s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                            s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                            s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            details.Add(s);
                        }
                    }
                    //else
                    //{
                    //    Student s = new Student();
                    //    s.Name = "";
                    //    s.RollNo = "";
                    //    // img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                    //    s.photo = Convert.ToString("");
                    //    s.Student_Type = "";
                    //    s.Degree = "";
                    //    s.Department = "";
                    //    s.Semester = "";
                    //    s.Section = "";
                    //    if (appno.Trim() == "")
                    //    {
                    //        s.statusmsg = "3";
                    //    }
                    //    else if (gatepasspk.Trim() == "")
                    //    {
                    //        s.statusmsg = "2";
                    //    }
                    //    s.staffcode = "";
                    //    s.staffname = "";
                    //    s.staffdept = "";
                    //    s.staffdesg = "";
                    //    //imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                    //    s.staffphoto = "";
                    //    s.appdateExit = "";
                    //    s.apptimeExit = "";
                    //    s.appdateEntry = "";
                    //    s.apptimeEntry = "";
                    //    details.Add(s);
                    //}
                }
                else
                {
                    Student s = new Student();
                    s.Name = "";
                    s.RollNo = "";
                    img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                    s.photo = Convert.ToString(img2.ImageUrl);
                    s.Student_Type = "";
                    s.Degree = "";
                    s.Department = "";
                    s.Semester = "";
                    s.Section = "";
                    //s.statusmsg = "3";
                    //if (appno.Trim() == "" || appno.Trim() == "0")
                    //{
                    //    s.statusmsg = "3";
                    //}
                    //else if (gatepasspk.Trim() == "")
                    //{
                    s.statusmsg = "2";
                    // }
                    s.staffcode = "";
                    s.staffname = "";
                    s.staffdept = "";
                    s.staffdesg = "";
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                    s.staffphoto = "";
                    s.appdateExit = "";
                    s.apptimeExit = "";
                    s.appdateEntry = "";
                    s.apptimeEntry = "";
                    imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                    imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                    imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;

                    details.Add(s);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static Student[] getData1(string Name)
    {
        string data = string.Empty;
        List<Student> details = new List<Student>();
        try
        {
            string FetchData = "";
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
            System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
            string date = DateTime.Now.ToString("MM/dd/yyyy");
            string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
            string[] split = Name.Split('-');
            string namesplit = split[0];
            string appno = dd.GetFunction("select App_No from Registration where stud_name='" + namesplit + "'");
            string rq_pk = dd.GetFunction("select MAX(RequisitionPK) from RQ_Requisition where MemType='1' and RequestType=6 and ReqAppStatus='1' and ReqAppNo='" + appno + "'");
            // string FetchData = "select g.status,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name,g.approval_staff,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,convert(varchar,g.ApprovedDate_Exit,103) as 'ApprovedDate_Exit',g.ApprovedTime_Exit,convert(varchar,g.ApprovedDate_Entry,103) as 'ApprovedDate_Entry',g.ApprovedTime_Entry from applyn a,Registration r ,Degree d,course c,Department dt,GatePass_Approval g,staffmaster s , staff_appl_master sa,hrdept_master hr,desig_master dm  where r.Roll_No=g.Roll_No  and a.app_no=r.app_no and ISNULL(g.Is_Staff,'0')='0' and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and sa.appl_no=s.appl_no and g.approval_staff=s.staff_code and g.approval_staff is not null and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.Roll_No='" + Roll_No + "' and g.ApprovedDate_Exit='" + dat + "'";
            gatepasspk = rq_pk;
            if (inroll == "1")
            {
                FetchData = " select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>'" + date + "' and and RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))";
            }
            else
            {
                FetchData = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, convert(varchar,gg.GatepassExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GatereqEntryDate,103) as 'GateReqEntryDate',g.GatereqEntrytime ,ReqAppStaffAppNo from applyn a,Registration r ,Degree d,course c,Department dt,RQ_Requisition g,GateEntryExit gg where   a.app_no=r.app_no  and a.app_no =g.ReqAppNo and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and g.ReqAppStatus='1' and  g.ReqAppNo='" + appno + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1' and RequisitionPK=((select max(RequisitionPK) from RQ_Requisition))";
            }
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    Student s = new Student();
                    string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
                    string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                    string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                    s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    s.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                    img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                    s.photo = Convert.ToString(img2.ImageUrl);
                    s.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                    s.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                    s.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    s.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                    s.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                    s.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                    imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                    s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                    imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                    s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                    imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                    s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);

                    s.statusmsg = "0";
                    // s.staffcode = ds.Tables[0].Rows[a]["approval_staff"].ToString();
                    s.staffname = staffname;
                    s.staffdept = dept;
                    s.staffdesg = desgn;
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + staff_code;
                    s.staffphoto = Convert.ToString(imagestaff.ImageUrl);
                    s.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                    s.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                    s.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                    s.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                    details.Add(s);
                }
            }
            else
            {
                String sql = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and   r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and  r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
                ds = dd.select_method_wo_parameter(sql, "Text");

                Student s = new Student();
                s.Name = ds.Tables[0].Rows[0]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[0]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[0]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                s.RollNo = ds.Tables[0].Rows[0]["Roll_no"].ToString();
                img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
                s.photo = Convert.ToString(img2.ImageUrl);
                s.Student_Type = ds.Tables[0].Rows[0]["Stud_Type"].ToString();
                s.Degree = ds.Tables[0].Rows[0]["Course_Name"].ToString();
                s.Department = ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                s.Semester = ds.Tables[0].Rows[0]["Current_Semester"].ToString();
                s.Section = ds.Tables[0].Rows[0]["Sections"].ToString();
                s.AppNo = ds.Tables[0].Rows[0]["app_no"].ToString();
                imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
                s.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
                s.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;
                s.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);
                s.statusmsg = "1";
                s.staffcode = "";
                s.staffname = "";
                s.staffdept = "";
                s.staffdesg = "";
                imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
                s.staffphoto = "";
                s.appdateExit = "";
                s.apptimeExit = "";
                s.appdateEntry = "";
                s.apptimeEntry = "";
                details.Add(s);
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class Student
    {
        public string Name { get; set; }
        public string smartno { get; set; }
        public string RollNo { get; set; }
        public string photo { get; set; }
        public string Student_Type { get; set; }
        public string Degree { get; set; }
        public string Department { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }
        public string statusmsg { get; set; }
        public string InOut { get; set; }
        public string staffcode { get; set; }
        public string staffname { get; set; }
        public string staffdept { get; set; }
        public string staffdesg { get; set; }
        public string staffphoto { get; set; }
        public string appdateEntry { get; set; }
        public string apptimeEntry { get; set; }
        public string appdateExit { get; set; }
        public string apptimeExit { get; set; }
        public string purpose { get; set; }
        public string studept { get; set; }


        //Added By Saranyadevi 4.2.2018
        public string AppNo { get; set; }
        public string Regvisitfarphoto { get; set; }
        public string Regvisitmonphoto { get; set; }
        public string Regvisitgaurphoto { get; set; }

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name from staffmaster where settled=0 and resign=0 and staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static Staff[] getstaffdata(string Name)
    {
        string data = string.Empty;
        List<Staff> staffdetails = new List<Staff>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string FetchData = "select s.staff_name,dm.desig_name,hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code  and settled=0 and resign =0 and s.staff_name like '" + Name + "%'";
            //select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and a.stud_name='" + Name + "'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    Staff sd = new Staff();
                    sd.Name = ds.Tables[0].Rows[a]["staff_name"].ToString();
                    sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
                    sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
                    staffdetails.Add(sd);
                }
            }
            return staffdetails.ToArray();
        }
        catch
        {
            return staffdetails.ToArray();
        }
    }
    public class Staff
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getvehicle(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT Veh_ID FROM Vehicle_Master where Veh_ID like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static Driver[] getdriverdata(string Veh_ID)
    {
        string data = string.Empty;
        List<Driver> driverdetails = new List<Driver>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string FetchData = " SELECT da.Staff_Name,vm.Veh_ID,da.Mobile_No,vm.Route,vm.Veh_Type,convert(varchar,vm.Insurance_Date,103) as 'Insurance_Date',convert(varchar,vm.FC_Date,103) as 'FC_Date',convert(varchar,da.Renew_Date,103) as 'Renew_Date'  FROM Vehicle_Master vm,DriverAllotment da where vm.Veh_ID=da.Vehicle_Id and da.Staff_Name is not null and vm.Veh_ID like '" + Veh_ID + "%'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    Driver d = new Driver();
                    d.Name = ds.Tables[0].Rows[a]["Staff_Name"].ToString();
                    d.Veh_ID = ds.Tables[0].Rows[a]["Veh_ID"].ToString();
                    d.Mobile_No = ds.Tables[0].Rows[a]["Mobile_No"].ToString();
                    d.Route = ds.Tables[0].Rows[a]["Route"].ToString();
                    newroute = d.Route;
                    d.Veh_Type = ds.Tables[0].Rows[a]["Veh_Type"].ToString();
                    d.Insurance_Date = ds.Tables[0].Rows[a]["Insurance_Date"].ToString();
                    d.FC_Date = ds.Tables[0].Rows[a]["FC_Date"].ToString();
                    d.Renew_Date = ds.Tables[0].Rows[a]["Renew_Date"].ToString();
                    driverdetails.Add(d);
                }
            }
            return driverdetails.ToArray();
        }
        catch
        {
            return driverdetails.ToArray();
        }
    }
    public class Driver
    {
        public string Name { get; set; }
        public string Veh_ID { get; set; }
        public string Mobile_No { get; set; }
        public string Route { get; set; }
        public string Veh_Type { get; set; }
        public string Insurance_Date { get; set; }
        public string FC_Date { get; set; }
        public string Renew_Date { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstage(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct s.Stage_Name  from Vehicle_Master v ,routemaster r,Stage_Master s where v.Veh_ID = r.Veh_ID and r.Stage_Name = s.Stage_id and r.Route_ID  = '" + newroute + "'";
        name = ws.Getname(query);
        return name;
    }
    //------------------------------for staff tab----------------------------------------
    [WebMethod]
    public static string CheckStaffCode(string Staff_Code)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Staff_Code != "")
            {
                string query = "select staff_code from staffmaster where settled=0 and resign =0 and staff_code = '" + Staff_Code + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffcode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where settled=0 and resign =0 and staff_code like '" + prefixText + "%'";
        //select distinct s.staff_code from applyn a,Registration r ,Degree d,course c,Department dt,staffmaster s , staff_appl_master sa,hrdept_master hr,desig_master dm  where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and sa.appl_no=s.appl_no  and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and  settled=0 and resign =0 and dt.Dept_Code = hr.dept_code  and s.staff_code like '" + prefixText + "%' and r.Roll_No='" + contextKey + "'";
        //select staff_code from staffmaster where settled=0 and resign =0 and staff_code like '"+prefixText+"%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffnamewithdept(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        //magesh  4.6.18
        //string query = "select s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like '" + prefixText + "%'";
        string query = "select s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and dm.collegeCode=s.college_code and sa.college_code=s.college_code and hr.college_code=s.college_code and s.staff_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static StaffDet[] getstaffdatabyid(string Staff_Code)
    {
        string data = string.Empty;
        List<StaffDet> staffdet = new List<StaffDet>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
            //string FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + Staff_Code + "'";
            string FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + Staff_Code + "' and dm.collegeCode=s.college_code and sa.college_code=s.college_code and hr.college_code=s.college_code";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    StaffDet sd = new StaffDet();
                    sd.Staff_Code = ds.Tables[0].Rows[a]["staff_code"].ToString();
                    sd.Staff_Name = ds.Tables[0].Rows[a]["staff_name"].ToString() + "-" + ds.Tables[0].Rows[a]["desig_name"].ToString() + "-" + ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
                    sd.Staff_Type = ds.Tables[0].Rows[a]["staffcategory"].ToString();
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
                    sd.Photo = Convert.ToString(imagestaff.ImageUrl);
                    sd.Mobile_No = ds.Tables[0].Rows[a]["com_mobileno"].ToString();
                    sd.statusmsg = "";
                    staffdet.Add(sd);
                }
            }
            return staffdet.ToArray();
        }
        catch
        {
            return staffdet.ToArray();
        }
    }
    [WebMethod]
    public static StaffDet[] getstaffdatabyname(string Staff_Name)
    {
        string data = string.Empty;
        List<StaffDet> staffdet = new List<StaffDet>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
            string[] split = Staff_Name.Split('-');
            string namesplit = split[0];
           // string FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like'" + namesplit + "%'";
            string FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0  and dm.collegeCode=s.college_code and sa.college_code=s.college_code and hr.college_code=s.college_code and  s.staff_name like'" + namesplit + "%'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    StaffDet sd = new StaffDet();
                    sd.Staff_Code = ds.Tables[0].Rows[a]["staff_code"].ToString();
                    sd.Staff_Name = ds.Tables[0].Rows[a]["staff_name"].ToString() + "-" + ds.Tables[0].Rows[a]["desig_name"].ToString() + "-" + ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
                    sd.Staff_Type = ds.Tables[0].Rows[a]["staffcategory"].ToString();
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
                    sd.Photo = Convert.ToString(imagestaff.ImageUrl);
                    sd.Mobile_No = ds.Tables[0].Rows[a]["com_mobileno"].ToString();
                    staffdet.Add(sd);
                }
            }
            return staffdet.ToArray();
        }
        catch
        {
            return staffdet.ToArray();
        }
    }
    public class StaffDet
    {
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Staff_Type { get; set; }
        public string Photo { get; set; }
        public string Mobile_No { get; set; }
        public string statusmsg { get; set; }
        public string appdateExit { get; set; }
        public string apptimeExit { get; set; }
        public string appdateEntry { get; set; }
        public string apptimeEntry { get; set; }
        public string checkinorout { get; set; }
    }
    //------------------------------for parents tab----------------------------------------
    [WebMethod]
    public static PStudent[] prnogetdata(string Roll_No)
    {
        string data = string.Empty;
        List<PStudent> details = new List<PStudent>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            if (Roll_No.Trim() != "undefined")
            {
                string FetchData = "select r.Roll_no,a.stud_name,a.parent_name,a.parentF_Mobile, r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.app_no from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and  d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and r.Roll_No='" + Roll_No + "'";
                //select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code   and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
                ds = dd.select_method_wo_parameter(FetchData, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Web.UI.WebControls.Image studphoto = new System.Web.UI.WebControls.Image();
                    System.Web.UI.WebControls.Image fatherphoto = new System.Web.UI.WebControls.Image();
                    System.Web.UI.WebControls.Image motherphoto = new System.Web.UI.WebControls.Image();
                    for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                    {
                        PStudent ps = new PStudent();
                        ps.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                        ps.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                        ps.Father_Name = ds.Tables[0].Rows[a]["parent_name"].ToString();
                        ps.Father_Mobile = ds.Tables[0].Rows[a]["parentF_Mobile"].ToString();
                        ps.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                        ps.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                        ps.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                        ps.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                        ps.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                        ps.app_no = ds.Tables[0].Rows[a]["app_no"].ToString();
                        studphoto.ImageUrl = "Handler/Handler4.ashx?rollno=" + ps.RollNo;
                        ps.Stud_Photo = Convert.ToString(studphoto.ImageUrl);
                        fatherphoto.ImageUrl = "Handler/Hfatherphoto.ashx?app_no=" + ps.app_no;
                        ps.Father_Photo = Convert.ToString(fatherphoto.ImageUrl);
                        motherphoto.ImageUrl = "Handler/Hmotherphoto.ashx?app_no=" + ps.app_no;
                        ps.Mother_Photo = Convert.ToString(motherphoto.ImageUrl);
                        DataSet rq_ds = new DataSet();
                        string query = " select g.GateEntryExitID,g.ExpectedTime,CONVERT(varchar, g.ByVehcile)ByVehcile ,g.VehType,g.VehRegNo,gd.Staff_Code,g.ToMeet,CONVERT(varchar, g.IsReturn)IsReturn,gd.OtherName,gd.Relationship,gd.MobileNo  from GateEntryExit g,GateEntryExitDet gd where gd.GateEntryExitID=g.GateEntryExitID and g.GateMemType='3' and g.GateType='1' and g.App_No='" + Convert.ToString(ds.Tables[0].Rows[a]["app_no"]) + "' and g.GatePassDate='" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "'";
                        rq_ds = dd.select_method_wo_parameter(query, "text");
                        if (rq_ds.Tables[0].Rows.Count > 0)
                        {
                            ps.tomeet = Convert.ToString(rq_ds.Tables[0].Rows[0]["ToMeet"]);
                            ps.staff_code = Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]);
                            if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) != "")
                            {
                                string staff_name = dd.GetFunction("select s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) + "'");
                                ps.staff_name = staff_name;
                            }
                            if (Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]) != "")
                                ps.othername = Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]);
                            if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]) != "")
                                ps.Relationship = Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]);
                            if (Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]) != "")
                                ps.MobileNo = Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]);
                        }
                        details.Add(ps);
                    }
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [WebMethod]
    public static PStudent[] pnamegetdata(string Name)
    {
        string data = string.Empty;
        List<PStudent> details = new List<PStudent>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string[] split = Name.Split('-');
            string namesplit = split[0];
            string FetchData = "select r.Roll_no,a.stud_name,a.parent_name,a.parentF_Mobile, r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.app_no from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and  d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and a.stud_name='" + namesplit + "'";
            //select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code   and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.WebControls.Image studphoto = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image fatherphoto = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image motherphoto = new System.Web.UI.WebControls.Image();
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    PStudent ps = new PStudent();
                    ps.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                    ps.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    ps.Father_Name = ds.Tables[0].Rows[a]["parent_name"].ToString();
                    ps.Father_Mobile = ds.Tables[0].Rows[a]["parentF_Mobile"].ToString();
                    ps.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                    ps.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                    ps.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    ps.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                    ps.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                    ps.app_no = ds.Tables[0].Rows[a]["app_no"].ToString();
                    studphoto.ImageUrl = "Handler/Handler4.ashx?rollno=" + ps.RollNo;
                    ps.Stud_Photo = Convert.ToString(studphoto.ImageUrl);
                    fatherphoto.ImageUrl = "Handler/Hfatherphoto.ashx?app_no=" + ps.app_no;
                    ps.Father_Photo = Convert.ToString(fatherphoto.ImageUrl);
                    motherphoto.ImageUrl = "Handler/Hmotherphoto.ashx?app_no=" + ps.app_no;
                    ps.Mother_Photo = Convert.ToString(motherphoto.ImageUrl);
                    details.Add(ps);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    public class PStudent
    {
        public string RollNo { get; set; }
        public string Name { get; set; }
        public string Father_Name { get; set; }
        public string Father_Mobile { get; set; }
        public string Student_Type { get; set; }
        public string Degree { get; set; }
        public string Department { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }
        public string Stud_Photo { get; set; }
        public string Father_Photo { get; set; }
        public string Mother_Photo { get; set; }
        public string app_no { get; set; }
        public string tomeet { get; set; }
        public string staff_code { get; set; }
        public string othername { get; set; }
        public string Relationship { get; set; }
        public string MobileNo { get; set; }
        public string staff_name { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetFathername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select parent_name from applyn where parent_name like '" + prefixText + "%'";
        //select a.stud_name from applyn a, Registration r where a.app_no=r.app_no and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static PStudent[] getdatafrmparent(string Father_Name)
    {
        string data = string.Empty;
        List<PStudent> details = new List<PStudent>();
        try
        {
            if (Father_Name.Trim() != "")
            {
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string FetchData = "select r.Roll_no,a.stud_name,a.parent_name,a.parentF_Mobile, r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.app_no from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and  d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and a.parent_name='" + Father_Name + "'";
                //select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code   and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
                ds = dd.select_method_wo_parameter(FetchData, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    System.Web.UI.WebControls.Image studphoto = new System.Web.UI.WebControls.Image();
                    System.Web.UI.WebControls.Image fatherphoto = new System.Web.UI.WebControls.Image();
                    System.Web.UI.WebControls.Image motherphoto = new System.Web.UI.WebControls.Image();
                    for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                    {
                        PStudent ps = new PStudent();
                        ps.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                        ps.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                        ps.Father_Name = ds.Tables[0].Rows[a]["parent_name"].ToString();
                        ps.Father_Mobile = ds.Tables[0].Rows[a]["parentF_Mobile"].ToString();
                        ps.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                        ps.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                        ps.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                        ps.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                        ps.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                        ps.app_no = ds.Tables[0].Rows[a]["app_no"].ToString();
                        studphoto.ImageUrl = "Handler/Handler4.ashx?rollno=" + ps.RollNo;
                        ps.Stud_Photo = Convert.ToString(studphoto.ImageUrl);
                        fatherphoto.ImageUrl = "Handler/Hfatherphoto.ashx?app_no=" + ps.app_no;
                        ps.Father_Photo = Convert.ToString(fatherphoto.ImageUrl);
                        motherphoto.ImageUrl = "Handler/Hmotherphoto.ashx?app_no=" + ps.app_no;
                        ps.Mother_Photo = Convert.ToString(motherphoto.ImageUrl);
                        details.Add(ps);
                    }
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetFatherMobileNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct parentF_Mobile from applyn where parentF_Mobile !='' and parentF_Mobile like '" + prefixText + "%'";
        //select a.stud_name from applyn a, Registration r where a.app_no=r.app_no and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static PStudent[] getdatafrmparentmob(string Mobile_No)
    {
        string data = string.Empty;
        List<PStudent> details = new List<PStudent>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string FetchData = "select r.Roll_no,a.stud_name,a.parent_name,a.parentF_Mobile, r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.app_no from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and  d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and a.parentF_Mobile='" + Mobile_No + "'";
            //select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections,a.parent_name from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code   and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name='" + namesplit + "'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.UI.WebControls.Image studphoto = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image fatherphoto = new System.Web.UI.WebControls.Image();
                System.Web.UI.WebControls.Image motherphoto = new System.Web.UI.WebControls.Image();
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    PStudent ps = new PStudent();
                    ps.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                    ps.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    ps.Father_Name = ds.Tables[0].Rows[a]["parent_name"].ToString();
                    ps.Father_Mobile = ds.Tables[0].Rows[a]["parentF_Mobile"].ToString();
                    ps.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                    ps.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                    ps.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                    ps.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                    ps.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                    ps.app_no = ds.Tables[0].Rows[a]["app_no"].ToString();
                    studphoto.ImageUrl = "Handler/Handler4.ashx?rollno=" + ps.RollNo;
                    ps.Stud_Photo = Convert.ToString(studphoto.ImageUrl);
                    fatherphoto.ImageUrl = "Handler/Hfatherphoto.ashx?app_no=" + ps.app_no;
                    ps.Father_Photo = Convert.ToString(fatherphoto.ImageUrl);
                    motherphoto.ImageUrl = "Handler/Hmotherphoto.ashx?app_no=" + ps.app_no;
                    ps.Mother_Photo = Convert.ToString(motherphoto.ImageUrl);
                    details.Add(ps);
                }
            }
            return details.ToArray();
        }
        catch
        {
            return details.ToArray();
        }
    }
    //-------------------------for visitor tab-------------------------------------------------
    [WebMethod]
    public static VisitorStaff[] getvisitorstaffdet(string Staff_Name)
    {
        string data = string.Empty;
        List<VisitorStaff> staffdet = new List<VisitorStaff>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
            string[] split = Staff_Name.Split('-');
            string namesplit = split[0];
            string FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_name like'" + namesplit + "%'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    VisitorStaff sd = new VisitorStaff();
                    sd.Staff_Code = ds.Tables[0].Rows[a]["staff_code"].ToString();
                    sd.Staff_Name = ds.Tables[0].Rows[a]["staff_name"].ToString() + "-" + ds.Tables[0].Rows[a]["desig_name"].ToString() + "-" + ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
                    sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
                    sd.Staff_Type = ds.Tables[0].Rows[a]["staffcategory"].ToString();
                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
                    sd.Photo = Convert.ToString(imagestaff.ImageUrl);
                    sd.Mobile_No = ds.Tables[0].Rows[a]["com_mobileno"].ToString();
                    staffdet.Add(sd);
                }
            }
            return staffdet.ToArray();
        }
        catch
        {
            return staffdet.ToArray();
        }
    }
    public class VisitorStaff
    {
        public string Staff_Code { get; set; }
        public string Staff_Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Staff_Type { get; set; }
        public string Mobile_No { get; set; }
        public string Photo { get; set; }
    }
    [WebMethod]
    public static string CheckCompanyName(string company_name)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (company_name != "")
            {
                string query = "select company_name from company_details where company_name like '" + company_name + "%'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcompname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VendorCompName from CO_VendorMaster where  VendorCompName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void txt_compname_Changed(object sender, EventArgs e)
    {
        vendorcompanyname1 = Convert.ToString(txt_compname.Text);
        vendorcompanyname = d2.GetFunction("select VendorPK from CO_VendorMaster where VendorCompName='" + vendorcompanyname1 + "'");
        div_visitor.Attributes.Add("style", "display:block");
        //txt_name4.Text = "";
        //txt_desgn.Text = "";
        //txt_dep.Text = "";
        //txt_visit1.Text = "";
        //txt_vehtype.Text = "";
        //txt_vehno1.Text = "";
        //txt_mno.Text = "";
        //txt_phno.Text = "";
        //txt_str.Text = "";
        //txt_cty.Text = "";
        //txt_dis.Text = "";
        //txt_stat.Text = "";
        //txt_visitormeetstaffid.Text = "";
        //txt_visitormeetstaffname.Text = "";
        //txt_visitormeetstaffdept.Text = "";
        //txt_visitormeetstaffdesg.Text = "";
        txt_name4.Focus();
      
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getcomppername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select VenContactName from IM_VendorContactMaster where  VendorFK='" + vendorcompanyname + "' and  VenContactName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    [WebMethod]
    public static VisitorCompany[] getvisitorcompdata(string Company_Name)
    {
        string data = string.Empty;
         List<VisitorCompany> compdet = new List<VisitorCompany>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string COMPANY = dd.GetFunction("select vendorpk from CO_VendorMaster where VendorName='" + vendorname1 + "' and VendorMobileNo='" + vendormobname1 + "' order by VendorPK desc");
            string vendorpkval = dd.GetFunction("select VendorContactPK from IM_VendorContactMaster where VenContactName='" + vendorname1 + "' and VendorMobileNo='" + vendormobname1 + "' order by VendorContactPK desc");
            DataSet rq_ds = new DataSet();
            if (che_inout=="in")
              rq_ds = dd.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and RequestDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
            else
                rq_ds = dd.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
            string rq_pk = "";
            string status = ""; string Remarks = ""; string reqstaff = "";
            if (rq_ds.Tables[0].Rows.Count > 0)
            {
                rq_pk = Convert.ToString(rq_ds.Tables[0].Rows[0]["RequisitionPK"]);
                status = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppStatus"]);
                Remarks = Convert.ToString(rq_ds.Tables[0].Rows[0]["Remarks"]);
                reqstaff = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppNo"]);
            }
            else
            {
                status = "0";
            }
            string  staffname = dd.GetFunction("select ReqAppStaffAppNo from RQ_RequestionApprove where RequisitionFK='" + rq_pk + "'");
            string query = "select v.VendorCode,v.VendorCompName,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and VendorFK='" + COMPANY + "' and VendorContactPK='" + vendorpkval + "' ";
            ds = dd.select_method_wo_parameter(query, "Text");
            VisitorCompany vc = new VisitorCompany();
            if (ds.Tables[0].Rows.Count == 0)
            {
                if (vendorpkval != "")
                {
                    if (status == "0")
                    {
                        vc.Appointment = "0";
                        vc.Company_Name = vendorcompanyname1;
                        vc.Company_Name = che_coimp;
                        vc.Company_Nameperson = Company_Name;
                    }
                }
                compdet.Add(vc);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    //VisitorCompany vc = new VisitorCompany();
                    string state = string.Empty;
                    string dis = string.Empty;
                    state = dd.GetFunction("select mastervalue from CO_MasterValues where mastercriteria='State' and mastercode ='" + Convert.ToString(ds.Tables[0].Rows[a]["VendorState"]) + "'");
                    dis = dd.GetFunction("select mastervalue from CO_MasterValues where mastercriteria='District' and mastercode ='" + Convert.ToString(ds.Tables[0].Rows[a]["VendorDist"]) + "'");
                    vc.Company_Name = ds.Tables[0].Rows[a]["VendorCompName"].ToString();
                  //  vc.Company_Nameperson = Company_Name;
                    vc.Company_Nameperson = ds.Tables[0].Rows[a]["VenContactName"].ToString();
                    vc.Company_designation = ds.Tables[0].Rows[a]["VenContactDesig"].ToString();
                    vc.Company_department = ds.Tables[0].Rows[a]["VenContactDept"].ToString();
                    vc.Company_street = ds.Tables[0].Rows[a]["VendorAddress"].ToString();
                    vc.Company_City = ds.Tables[0].Rows[a]["VendorCity"].ToString();
                    if (dis != "0")
                        vc.Company_District = dis;
                    else
                        vc.Company_District = "";
                     if (state != "0")
                    vc.Company_State = state;
                     else
                         vc.Company_State = "";
                    vc.phone_no = ds.Tables[0].Rows[a]["VendorPhoneNo"].ToString();
                    vc.mobile_no = ds.Tables[0].Rows[a]["VendorMobileNo"].ToString();
                    vc.purposeofvisit = Remarks;
                    rq_ds.Clear();
                    rq_ds = dd.select_method_wo_parameter("select g.GateEntryExitID,g.ExpectedTime,CONVERT(varchar, g.ByVehcile)ByVehcile,g.VehType,g.VehRegNo,gd.Staff_Code,g.ToMeet,CONVERT(varchar, g.IsReturn)IsReturn ,gd.OtherName,gd.Relationship,gd.MobileNo from GateEntryExit g,GateEntryExitDet gd where gd.GateEntryExitID=g.GateEntryExitID and g.GateMemType='4' and GatePassDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and CompanyName='" + Convert.ToString(ds.Tables[0].Rows[a]["VendorCompName"]) + "' and VisitorName='" +  ds.Tables[0].Rows[a]["VenContactName"].ToString() + "' and GateType='1'", "text");
                    if (rq_ds.Tables[0].Rows.Count > 0)
                    {
                        vc.vehical = Convert.ToString(rq_ds.Tables[0].Rows[0]["ByVehcile"]);
                        vc.VehType = Convert.ToString(rq_ds.Tables[0].Rows[0]["VehType"]);
                        vc.VehRegNo = Convert.ToString(rq_ds.Tables[0].Rows[0]["VehRegNo"]);
                        vc.Staff_Code = Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]);
                        vc.tomeet = Convert.ToString(rq_ds.Tables[0].Rows[0]["ToMeet"]);
                        vc.returnvisitor = Convert.ToString(rq_ds.Tables[0].Rows[0]["IsReturn"]);
                        if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) != "")
                        {
                            string staff_name = dd.GetFunction("select s.staff_name+'-'+dm.desig_name+'-'+hr.dept_name from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) + "'");
                            vc.staff_name = staff_name;
                        }
                        if (Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]) != "")
                            vc.othername = Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]);
                        if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]) != "")
                            vc.Relationship = Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]);
                        if (Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]) != "")
                            vc.MobileNo = Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]);
                    }
                    if (vendorpkval != "0")
                    {
                        if (status == "1")
                        {

                            vc.Appointment = "1";

                            vc.statusmsgvis1 = "1";
                            string staff_applid = reqstaff;
                            //string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            //string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                            string Q1 = "select sa.appl_id,sm.staff_code,sa.dept_name,sa.desig_name,sa.appl_name,staff_type,sa.com_mobileno from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'";
                            DataSet staffDetails = new DataSet();
                            staffDetails = dd.select_method_wo_parameter(Q1, "Text");
                            if (staffDetails.Tables != null && staffDetails.Tables[0].Rows.Count > 0)
                            {
                                vc.staff_dept = Convert.ToString(staffDetails.Tables[0].Rows[0]["dept_name"]);
                                vc.staff_design = Convert.ToString(staffDetails.Tables[0].Rows[0]["desig_name"]);
                                vc.staff_type = Convert.ToString(staffDetails.Tables[0].Rows[0]["staff_type"]);
                                vc.staffname = Convert.ToString(staffDetails.Tables[0].Rows[0]["appl_name"]);
                                vc.staff_mob = Convert.ToString(staffDetails.Tables[0].Rows[0]["com_mobileno"]);
                            }
                            //magesh 8.6.18
                            //string staffdetails = dd.GetFunction("select isnull(appl_name,'')+'$'+isnull(dept_name,'')+'$'+isnull(staff_type,'')+'$'+convert(varchar,com_mobileno)+'$'+isnull(desig_name,'') from staff_appl_master where appl_id='" + staffname + "'");
                            //string[] staffdet = staffdetails.Split('$');
                            //if (staffdet.Length > 1)
                            //{
                            //    vc.staffname = Convert.ToString(staffdet[0]);
                            //    vc.staff_dept = Convert.ToString(staffdet[1]);
                            //    vc.staff_type = Convert.ToString(staffdet[2]);
                            //    vc.staff_mob = Convert.ToString(staffdet[3]);
                            //    vc.staff_design = Convert.ToString(staffdet[4]);
                            //}
                        }
                    }
                    if (vendorpkval != "")
                    {
                        if (status == "0")
                        {
                            vc.statusmsgvis1 = "0";
                        }
                    }
                    //if (vendorpkval != "0")
                    //{
                    //    if (status == "0")
                    //    {
                    //        vc.statusmsgvis1 = "1";
                    //    }
                    //}
                    compdet.Add(vc);
                }
            }
           
            return compdet.ToArray();
            vendorname1 = "";
            vendormobname1 = "";
        }
        catch
        {
            return compdet.ToArray();
        }
    }
    [WebMethod]
    public static VisitorCompany[] getvisitorcompdatamobileno(string mobileNo)
    {
        string data = string.Empty;
        List<VisitorCompany> compdet = new List<VisitorCompany>();
        try
        {
            if (mobileNo.Length == 10)
            {
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                DataSet rq_ds = new DataSet();
                rq_ds = dd.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks from IM_VendorContactMaster c,RQ_Requisition r where c.VendorFK=r.VendorFK and c.VendorContactPK=r.VendorContactFK and vendormobileno='" + mobileNo + "' order by RequisitionPK desc", "Text");
                string rq_pk = "";
                string status = ""; string Remarks = "";
                if (rq_ds.Tables[0].Rows.Count > 0)
                {
                    rq_pk = Convert.ToString(rq_ds.Tables[0].Rows[0]["RequisitionPK"]);
                    status = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppStatus"]);
                    Remarks = Convert.ToString(rq_ds.Tables[0].Rows[0]["Remarks"]);
                }
                else
                {
                    status = "0";
                }
                string staffname = dd.GetFunction("select ReqAppStaffAppNo from RQ_RequestionApprove where RequisitionFK='" + rq_pk + "'");
                string query = "select v.VendorCode,v.VendorCompName,v.VendorAddress,v.VendorCity,v.VendorStreet,v.VendorDist,v.VendorState,v.VendorPin,vc.VenContactName,vc.VenContactDept,vc.VenContactDesig,vc.VendorPhoneNo,vc.VendorExtNo,vc.VendorMobileNo,vc.VendorEmail from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and vc.vendormobileno ='" + mobileNo + "' order by VendorContactPK desc";
                ds = dd.select_method_wo_parameter(query, "Text");
                VisitorCompany vc = new VisitorCompany();
                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (rq_pk == "")
                    {
                        if (status == "0")
                        {
                            vc.Appointment = "0";
                            vc.Company_Name = vendorcompanyname1;
                            vc.mobile_no = mobileNo;
                        }
                    }
                    compdet.Add(vc);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                    {
                        //VisitorCompany vc = new VisitorCompany();
                        vc.Company_Name = ds.Tables[0].Rows[a]["VendorCompName"].ToString();
                        vc.Company_Nameperson = Convert.ToString(ds.Tables[0].Rows[0]["VenContactName"]);
                        vc.Company_designation = ds.Tables[0].Rows[a]["VenContactDesig"].ToString();
                        vc.Company_department = ds.Tables[0].Rows[a]["VenContactDept"].ToString();
                        vc.Company_street = ds.Tables[0].Rows[a]["VendorStreet"].ToString();
                        vc.Company_City = ds.Tables[0].Rows[a]["VendorCity"].ToString();
                        vc.Company_District = ds.Tables[0].Rows[a]["VendorDist"].ToString();
                        vc.Company_State = ds.Tables[0].Rows[a]["VendorState"].ToString();
                        vc.phone_no = ds.Tables[0].Rows[a]["VendorPhoneNo"].ToString();
                        vc.mobile_no = ds.Tables[0].Rows[a]["VendorMobileNo"].ToString();
                        vc.purposeofvisit = Remarks;
                    }
                }
                rq_ds.Clear();
                rq_ds = dd.select_method_wo_parameter("select g.GateEntryExitID,g.ExpectedTime,CONVERT(varchar, g.ByVehcile)ByVehcile,g.VehType,g.VehRegNo,gd.Staff_Code,g.ToMeet,CONVERT(varchar, g.IsReturn)IsReturn ,gd.OtherName,gd.Relationship,gd.MobileNo from GateEntryExit g,GateEntryExitDet gd where gd.GateEntryExitID=g.GateEntryExitID and g.GateMemType='4' and GatePassDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "'  and g.MobileNo='" + mobileNo + "' and GateType='1'", "text");//and CompanyName='" + Convert.ToString(ds.Tables[0].Rows[a]["VendorCompName"]) + "'
                if (rq_ds.Tables[0].Rows.Count > 0)
                {
                    vc.vehical = Convert.ToString(rq_ds.Tables[0].Rows[0]["ByVehcile"]);
                    vc.VehType = Convert.ToString(rq_ds.Tables[0].Rows[0]["VehType"]);
                    vc.VehRegNo = Convert.ToString(rq_ds.Tables[0].Rows[0]["VehRegNo"]);
                    vc.Staff_Code = Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]);
                    vc.tomeet = Convert.ToString(rq_ds.Tables[0].Rows[0]["ToMeet"]);
                    vc.returnvisitor = Convert.ToString(rq_ds.Tables[0].Rows[0]["IsReturn"]);
                    if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) != "")
                    {
                        string staff_name = dd.GetFunction("select isnull(s.staff_name,'')+'-'+isnull(dm.desig_name,'')+'-'+isnull(hr.dept_name,'') from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + Convert.ToString(rq_ds.Tables[0].Rows[0]["Staff_Code"]) + "'");
                        vc.staffname = staff_name;
                    }
                    if (Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]) != "")
                        vc.othername = Convert.ToString(rq_ds.Tables[0].Rows[0]["OtherName"]);
                    if (Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]) != "")
                        vc.Relationship = Convert.ToString(rq_ds.Tables[0].Rows[0]["Relationship"]);
                    if (Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]) != "")
                        vc.MobileNo = Convert.ToString(rq_ds.Tables[0].Rows[0]["MobileNo"]);
                }
                if (rq_pk != "")
                {
                    if (status == "1")
                    {
                        vc.Appointment = "1";
                        string staffdetails = dd.GetFunction("select isnull(appl_name,'')+'$'+isnull(dept_name,'')+'$'+isnull(staff_type,'')+'$'+convert(varchar,com_mobileno)+'$'+isnull(desig_name,'') from staff_appl_master where appl_id='" + staffname + "'");
                        string[] staffdet = staffdetails.Split('$');
                        if (staffdet.Length > 1)
                        {
                            vc.staffname = Convert.ToString(staffdet[0]);
                            vc.staff_dept = Convert.ToString(staffdet[1]);
                            vc.staff_type = Convert.ToString(staffdet[2]);
                            vc.staff_mob = Convert.ToString(staffdet[3]);
                            vc.staff_design = Convert.ToString(staffdet[4]);
                        }
                    }
                }
                compdet.Add(vc);
                //    }
                //}
            }
            return compdet.ToArray();
        }
        catch
        {
            return compdet.ToArray();
        }
    }
    public class VisitorCompany
    {
        public string Company_Code { get; set; }
        public string Company_Name { get; set; }
        public string Company_Nameperson { get; set; }
        public string Company_designation { get; set; }
        public string Company_department { get; set; }
        public string Company_street { get; set; }
        public string Company_City { get; set; }
        public string Company_District { get; set; }
        public string Company_State { get; set; }
        public string phone_no { get; set; }
        public string mobile_no { get; set; }
        public string Appointment { get; set; }
        public string staffname { get; set; }
        public string staff_dept { get; set; }
        public string staff_design { get; set; }
        public string staff_type { get; set; }
        public string staff_mob { get; set; }
        public string purposeofvisit { get; set; }
        public string vehical { get; set; }
        public string VehType { get; set; }
        public string VehRegNo { get; set; }
        public string Staff_Code { get; set; }
        public string returnvisitor { get; set; }
        public string tomeet { get; set; }
        public string staff_name { get; set; }
        public string othername { get; set; }
        public string Relationship { get; set; }
        public string MobileNo { get; set; }
        public string statusmsgvis1 { get; set; }
    }
    //------------------------for material tab-----------------------------------------------
    [WebMethod]
    public static string CheckPONo(string order_code)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (order_code != "")
            {
                string query = "select OrderCode from IT_PurchaseOrder where OrderCode like '" + order_code + "%'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getorderno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        //string query = "select distinct(p.OrderCode) from IT_PurchaseOrder p,purchaseorder_items pi where p.OrderCode=pi.order_code and p.OrderCode like '" + prefixText + " %'";
        string query = "select OrderCode from IT_PurchaseOrder";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static MaterialPurchase[] getmaterialpurchasedata(string Order_Code)
    {
        string data = string.Empty;
        List<MaterialPurchase> purchase = new List<MaterialPurchase>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string vendorpk = dd.GetFunction("select VendorFK from IT_PurchaseOrder where OrderCode='" + Order_Code + "'");
            string FetchData = "select i.ItemName,i.ItemCode,(AppQty - isnull(RejQty,0))as total,VendorAddress,VendorStreet,VendorState,VendorCity, VendorDist,VendorPin,VendorCompName,CO.VendorMobileNo from  IT_PurchaseOrder IT,CO_VendorMaster CO,IT_PurchaseOrderDetail p,IM_ItemMaster i WHERE VendorPK='" + vendorpk + "' AND VendorType=1 AND OrderCode='" + Order_Code + "' AND VendorPK=IT.VendorFK and PurchaseOrderFK=1 and ItemFK=ItemPK";
            //select p.order_code,V.vendor_name,v.vendor_address,v.Vendor_City,v.Vendor_District,v.Vendor_District,v.Vendor_State,pi.item_code ,i.item_name,pi.app_qty,vc.ContactMobileNo,vc.Contact_Desig,vc.Contact_Name  from purchase_order p,purchaseorder_items pi,vendor_details v,item_master i,Vendor_ContactDetails vc where p.order_code =pi.order_code and p.vendor_code =v.vendor_code and vc.Vendor_Code=v.vendor_code and pi.item_code =i.item_code and p.order_code ='" + Order_Code + "'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    MaterialPurchase mp = new MaterialPurchase();
                    //mp.Order_Code = ds.Tables[0].Rows[a]["OrderCode"].ToString();
                    mp.Vendor_Name = ds.Tables[0].Rows[a]["VendorCompName"].ToString();
                    mp.vendor_address = ds.Tables[0].Rows[a]["VendorAddress"].ToString();
                    mp.Vendor_City = ds.Tables[0].Rows[a]["VendorCity"].ToString();
                    mp.Vendor_District = ds.Tables[0].Rows[a]["VendorDist"].ToString();
                    mp.Vendor_State = ds.Tables[0].Rows[a]["VendorState"].ToString();
                    //mp.Contact_Desig = ds.Tables[0].Rows[a]["Contact_Desig"].ToString();
                    //mp.Contact_Name = ds.Tables[0].Rows[a]["Contact_Name"].ToString();
                    mp.ContactMobileNo = ds.Tables[0].Rows[a]["VendorMobileNo"].ToString();
                    mp.pin = ds.Tables[0].Rows[a]["VendorPin"].ToString();
                    mp.item_code = ds.Tables[0].Rows[a]["ItemCode"].ToString();
                    mp.item_name = ds.Tables[0].Rows[a]["ItemName"].ToString();
                    mp.app_qty = ds.Tables[0].Rows[a]["total"].ToString();
                    purchase.Add(mp);
                }
            }
            return purchase.ToArray();
        }
        catch
        {
            return purchase.ToArray();
        }
    }
    public class MaterialPurchase
    {
        public string Order_Code { get; set; }
        public string Vendor_Name { get; set; }
        public string vendor_address { get; set; }
        public string Vendor_street { get; set; }
        public string Vendor_City { get; set; }
        public string Vendor_District { get; set; }
        public string Vendor_State { get; set; }
        public string Contact_Desig { get; set; }
        public string Contact_Name { get; set; }
        public string ContactMobileNo { get; set; }
        public string pin { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string app_qty { get; set; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getitemname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct item_name from item_master where item_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [WebMethod]
    public static ItemDetails[] getitemdata(string item_name)
    {
        string data = string.Empty;
        List<ItemDetails> itemdet = new List<ItemDetails>();
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            Hashtable hat = new Hashtable();
            string FetchData = "select item_code,item_name,item_unit from item_master where item_name like '" + item_name + "'";
            ds = dd.select_method_wo_parameter(FetchData, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                {
                    ItemDetails id = new ItemDetails();
                    id.item_code = ds.Tables[0].Rows[a]["item_code"].ToString();
                    id.item_name = ds.Tables[0].Rows[a]["item_name"].ToString();
                    id.item_unit = ds.Tables[0].Rows[a]["item_unit"].ToString();
                    itemdet.Add(id);
                }
            }
            return itemdet.ToArray();
        }
        catch
        {
            return itemdet.ToArray();
        }
    }
    public class ItemDetails
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_unit { get; set; }
    }
    //----------------------------------------------for vehicle---------------------------------
    [WebMethod]
    public static string CheckVehicleID(string Veh_ID)
    {
        string returnValue = string.Empty;
        try
        {
            DataSet ds = new DataSet();
            DAccess2 dd = new DAccess2();
            bool flage = false;
            if (Veh_ID != "")
            {
                string query = "SELECT * FROM Vehicle_Master where Veh_ID = '" + Veh_ID + "'";
                ds = dd.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    flage = true;
                }
                if (flage == true)
                {
                    returnValue = "0";
                }
                else
                {
                    returnValue = "1";
                }
            }
            else
            {
                returnValue = "0";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    //------------------------------for convert text to date-----------------------------
    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);
        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }
    public void access1()
    {
        try
        {
            string query = "";
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Hostler DayScholar Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Hostler DayScholar Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    if (val == "1,2")
                    {
                        gatepassrights = "3";
                    }
                    else if (val == "1")
                    {
                        gatepassrights = "1";
                    }
                    else if (val == "2")
                    {
                        gatepassrights = "2";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void access()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Gate Pass Tab Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Gate Pass Tab Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    string len = split.Length.ToString();
                    if (len == "1")
                    {
                        values = val;
                        if (val == "1")
                        {
                            //studenttd.Visible = true;
                            studenttd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            //studenttd.Visible = false;
                            //imgbtn_student.Visible = false;
                            //Label12.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (val == "2")
                        {
                            //stafftd.Visible = true;
                            //imgbtn_staff.Visible = true;
                            //Label13.Visible = true;
                            stafftd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            //stafftd.Visible = false;
                            //imgbtn_staff.Visible = false;
                            //Label13.Visible = false;
                        }
                        if (val == "3")
                        {
                            //parenttd.Visible = true;
                            //imgbtn_parents.Visible = true;
                            //Label14.Visible = true;
                            parenttd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            parenttd.Attributes.Add("style", "display:none");
                            //parenttd.Visible = false;
                            //imgbtn_parents.Visible = false;
                        }
                        if (val == "4")
                        {
                            //visitortd.Visible = true;
                            visitortd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            visitortd.Attributes.Add("style", "display:none");
                            //visitortd.Visible = false;
                        }
                        if (val == "5")
                        {
                            //materialtd.Visible = true;
                            materialtd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            materialtd.Attributes.Add("style", "display:none");
                            // materialtd.Visible = false;
                        }
                        if (val == "6")
                        {
                            // vehicletd.Visible = true;
                            vehicletd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            //vehicletd.Visible = false;
                            vehicletd.Attributes.Add("style", "display:none");
                        }
                    }
                    // ******************** length 2**************
                    if (len == "2")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        if (sp1 == "1" || sp2 == "1")
                        {
                            //studenttd.Visible = true;
                            studenttd.Attributes.Add("style", "display:block");
                        }
                        else
                        {
                            //studenttd.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "2" || sp2 == "2")
                        {
                            //stafftd.Visible = true;
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            // stafftd.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3")
                        {
                            // parenttd.Visible = true;
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //parenttd.Visible = false;
                            parenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "4" || sp2 == "4")
                        {
                            //visitortd.Visible = true;
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //visitortd.Visible = false;
                            visitortd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "5" || sp2 == "5")
                        {
                            // materialtd.Visible = true;
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            // materialtd.Visible = false;
                            materialtd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "6" || sp2 == "6")
                        {
                            //vehicletd.Visible = true;
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //vehicletd.Visible = false;
                            vehicletd.Attributes.Add("style", "display:none");
                        }
                    }
                    //  *************************** length 3*****************
                    else if (len == "3")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1")
                        {
                            //studenttd.Visible = true;
                            studenttd.Attributes.Add("style", "display:block");
                            studenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //studenttd.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2")
                        {
                            //stafftd.Visible = true;
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //stafftd.Visible = false;
                            stafftd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3")
                        {
                            //parenttd.Visible = true;
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            // parenttd.Visible = false;
                            parenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4")
                        {
                            //visitortd.Visible = true;
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //visitortd.Visible = false;
                            visitortd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5")
                        {
                            // materialtd.Visible = true;
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //materialtd.Visible = false;
                            materialtd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6")
                        {
                            //vehicletd.Visible = true;
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //vehicletd.Visible = false;
                            //imgbtn_vehicle.Visible = false;
                            vehicletd.Attributes.Add("style", "display:none");
                        }
                    }
                    //    *********************** length 4*****************
                    else if (len == "4")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1")
                        {
                            studenttd.Attributes.Add("style", "display:block");
                            studenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //studenttd.Visible = true;
                        }
                        else
                        {
                            //studenttd.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2")
                        {
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //stafftd.Visible = true;
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            //stafftd.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3")
                        {
                            //parenttd.Visible = true;
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //parenttd.Visible = false;
                            parenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4")
                        {
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            // visitortd.Visible = true;
                        }
                        else
                        {
                            visitortd.Attributes.Add("style", "display:none");
                            //visitortd.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5")
                        {
                            // materialtd.Visible = true;
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //materialtd.Visible = false;
                            materialtd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6")
                        {
                            //vehicletd.Visible = true;
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //vehicletd.Visible = false;
                            vehicletd.Attributes.Add("style", "display:none");
                        }
                    }
                    else if (len == "5")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1")
                        {
                            //studenttd.Visible = true;
                            studenttd.Attributes.Add("style", "display:block");
                            studenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            //studenttd.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2")
                        {
                            //stafftd.Visible = true;
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            //stafftd.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3")
                        {
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //parenttd.Visible = true;
                        }
                        else
                        {
                            parenttd.Attributes.Add("style", "display:none");
                            //parenttd.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4")
                        {
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            // visitortd.Visible = true;
                        }
                        else
                        {
                            visitortd.Attributes.Add("style", "display:none");
                            // visitortd.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5")
                        {
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //materialtd.Visible = true;
                        }
                        else
                        {
                            materialtd.Attributes.Add("style", "display:none");
                            //materialtd.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6")
                        {
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //vehicletd.Visible = true;
                        }
                        else
                        {
                            vehicletd.Attributes.Add("style", "display:none");
                            //vehicletd.Visible = false;
                        }
                    }
                    else if (len == "6")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        string sp6 = (split[5]);
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1" || sp6 == "1")
                        {
                            //studenttd.Visible = true;
                            studenttd.Attributes.Add("style", "display:block");
                            studenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            // studenttd.Visible = false;
                            studenttd.Attributes.Add("style", "display:none");
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2" || sp6 == "2")
                        {
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //stafftd.Visible = true;
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            // stafftd.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3" || sp6 == "3")
                        {
                            //parenttd.Visible = true;
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                        }
                        else
                        {
                            parenttd.Attributes.Add("style", "display:none");
                            // parenttd.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4" || sp6 == "4")
                        {
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //visitortd.Visible = true;
                        }
                        else
                        {
                            visitortd.Attributes.Add("style", "display:none");
                            //visitortd.Visible = true;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5" || sp6 == "5")
                        {
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //materialtd.Visible = true;
                        }
                        else
                        {
                            materialtd.Attributes.Add("style", "display:none");
                            //materialtd.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6" || sp6 == "6")
                        {
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //vehicletd.Visible = true;
                        }
                        else
                        {
                            vehicletd.Attributes.Add("style", "display:none");
                            //vehicletd.Visible = false;
                        }
                    }
                    else if (len == "7")
                    {
                        string sp1 = (split[0]);
                        string sp2 = (split[1]);
                        string sp3 = (split[2]);
                        string sp4 = (split[3]);
                        string sp5 = (split[4]);
                        string sp6 = (split[5]);
                        string sp7 = (split[6]);
                        if (sp1 == "1" || sp2 == "1" || sp3 == "1" || sp4 == "1" || sp5 == "1" || sp6 == "1" || sp7 == "1")
                        {
                            studenttd.Attributes.Add("style", "display:block");
                            studenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //studenttd.Visible = true;
                        }
                        else
                        {
                            studenttd.Attributes.Add("style", "display:none");
                            //studenttd.Visible = false;
                        }
                        if (sp1 == "2" || sp2 == "2" || sp3 == "2" || sp4 == "2" || sp5 == "2" || sp6 == "2" || sp7 == "2")
                        {
                            stafftd.Attributes.Add("style", "display:block");
                            stafftd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //stafftd.Visible = true;
                        }
                        else
                        {
                            stafftd.Attributes.Add("style", "display:none");
                            //stafftd.Visible = false;
                        }
                        if (sp1 == "3" || sp2 == "3" || sp3 == "3" || sp4 == "3" || sp5 == "3" || sp6 == "3" || sp7 == "3")
                        {
                            parenttd.Attributes.Add("style", "display:block");
                            parenttd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //parenttd.Visible = true;
                        }
                        else
                        {
                            parenttd.Attributes.Add("style", "display:none");
                            //parenttd.Visible = false;
                        }
                        if (sp1 == "4" || sp2 == "4" || sp3 == "4" || sp4 == "4" || sp5 == "4" || sp6 == "4" || sp7 == "4")
                        {
                            visitortd.Attributes.Add("style", "display:block");
                            visitortd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //visitortd.Visible = true;
                        }
                        else
                        {
                            visitortd.Attributes.Add("style", "display:none");
                            //visitortd.Visible = false;
                        }
                        if (sp1 == "5" || sp2 == "5" || sp3 == "5" || sp4 == "5" || sp5 == "5" || sp6 == "5" || sp7 == "5")
                        {
                            materialtd.Attributes.Add("style", "display:block");
                            materialtd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //materialtd.Visible = true;
                        }
                        else
                        {
                            materialtd.Attributes.Add("style", "display:none");
                            //materialtd.Visible = false;
                        }
                        if (sp1 == "6" || sp2 == "6" || sp3 == "6" || sp4 == "6" || sp5 == "6" || sp6 == "6" || sp7 == "6")
                        {
                            vehicletd.Attributes.Add("style", "display:block");
                            vehicletd.Attributes.Add("style", " margin-top: -10px; margin-left: 10px;");
                            //vehicletd.Visible = true;
                        }
                        else
                        {
                            vehicletd.Attributes.Add("style", "display:none");
                            //vehicletd.Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    public static void accessNew()
    {
        try
        {
            DAccess2 dnew = new DAccess2();
            DataSet dsms = new DataSet();
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if (Groupcode.Trim() != "" && Groupcode.Trim() != "0")
            {
                Master1 = Groupcode;
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and Group_code ='" + Master1 + "'";
            }
            else if (UserCode.Trim() != "")
            {
                Master1 = UserCode;
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and usercode ='" + Master1 + "'";
            }
            dsms = dnew.select_method_wo_parameter(query, "Text");
            if (dsms.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsms.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(dsms.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_mom = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_dad = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_stud = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];
                        sms_mom = "1";
                        sms_dad = "2";
                        sms_stud = "3";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void access2()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_mom = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_dad = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_stud = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];
                        sms_mom = "1";
                        sms_dad = "2";
                        sms_stud = "3";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void access3()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='SMS Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='SMS Rights' and usercode ='" + Master1 + "'";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_req = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_req = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_app = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_exit = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_req = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_app = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_exit = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];
                        sms_req = "1";
                        sms_app = "2";
                        sms_exit = "3";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void rb_in_CheckedChanged(object sender, EventArgs e)
    {
        studenttd.BgColor = "#C4C4C4";
        div_student.Attributes.Add("style", "display:block");
        if (rb_out.Checked == true)
        {
            inroll = "1";
        }
        else
        {
            inroll = "0";
        }
        inoutclear();
    }
    public void rb_out_CheckedChanged(object sender, EventArgs e)
    {
        studenttd.BgColor = "#C4C4C4";
        div_student.Attributes.Add("style", "display:block");
        if (rb_out.Checked == true)
        {
            inroll = "1";
        }
        else
        {
            inroll = "0";
        }
        inoutclear();
    }
    public void inoutclear()
    {
        txt_rollno.Text = "";
        txt_smart.Text = "";
        txt_name.Text = "";
        txt_purpose1.Text = "";
        txt_purpose1.Text = "";
        txt_studtype.Text = "";
        txt_degree.Text = "";
        txt_dept.Text = "";
        txt_sem.Text = "";
        txt_sec.Text = "";
        txt_apstaff.Text = "";
        txt_apdept.Text = "";
        txt_apdesgn.Text = "";
    }
    [WebMethod]
    public static StaffDet[] getstaffsmartcard(string Smart_No)
    {
        string data = string.Empty;
        List<StaffDet> staffdet = new List<StaffDet>();
        try
        {
            if (Smart_No.Trim() != "" && Smart_No.Length >= 10)
            {
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string FetchData = "";
                System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                //barath 16.03.17 
                string appno = "";
                string applid = "";
                string getapplappno = dd.GetFunction(" select sm.appl_no+'$'+convert(varchar(10), appl_id) from staffmaster sm,staff_appl_master sa where sm.appl_no=sa.appl_no and sm.smartcard_serial_no='" + Smart_No + "'");
                string[] getapplidappno = getapplappno.Split('$');
                if (getapplidappno.Length > 1)
                {
                    appno = Convert.ToString(getapplidappno[0]);
                    applid = Convert.ToString(getapplidappno[1]);
                }
                string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='2' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + applid + "'");
                gatepasspk = rq_pk;
                string gatepassperimissiontype = dd.GetFunction("select value from Master_Settings where settings='Gatepass Request Type'");//  and usercode='"+UserCode+"'");
                if (gatepassperimissiontype.Trim() == "0")
                {
                    #region withrequest
                    string inn = dd.GetFunction(" select GateType from RQ_Requisition r , GateEntryExit g where g.App_No=r.ReqAppNo and g.GateMemType='2' and ReqAppStatus='1' and ReqAppNo='" + applid + "' and RequisitionPK='" + gatepasspk + "' and GateReqEntryDate>='" + date + "'  and  RequestFk=RequisitionPK and RequestType=6 ");
                    if (inn == "0")
                    {
                        inroll = "1";
                        FetchData = " select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + applid + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK ='" + gatepasspk + "'";
                    }
                    else
                    {
                        inroll = "0";
                        FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,GateEntryExit gg,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + applid + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1' and g.ReqAppNo=gg.App_No";
                    }
                    //if (incode == "1")
                    //{
                    //    //string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='2' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + applid + "'");
                    //    //gatepasspk = rq_pk;
                    //}
                    //else
                    //{
                    //    //string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='2' and App_No='" + applid + "' and GateType ='1'");
                    //    //gatepasspk = rq_pk;
                    //}
                    ds = dd.select_method_wo_parameter(FetchData, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            StaffDet sd = new StaffDet();
                            string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
                            string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                            sd.Staff_Code = ds.Tables[0].Rows[a]["staff_code"].ToString();
                            sd.Staff_Name = ds.Tables[0].Rows[a]["staff_name"].ToString() + "-" + ds.Tables[0].Rows[a]["desig_name"].ToString() + "-" + ds.Tables[0].Rows[a]["dept_name"].ToString();
                            sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
                            sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
                            sd.Staff_Type = ds.Tables[0].Rows[a]["staffcategory"].ToString();
                            imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
                            sd.Photo = Convert.ToString(imagestaff.ImageUrl);
                            sd.Mobile_No = ds.Tables[0].Rows[a]["com_mobileno"].ToString();
                            sd.checkinorout = Convert.ToString(inroll);
                            //sd.statusmsg = "0";
                            string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            sd.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
                            sd.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
                            sd.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
                            sd.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
                            string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
                            string[] splitarray = dnewdate.Split('/');
                            DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
                            string[] splitarray1 = entry.Split('/');
                            DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
                            string appgateentydate = dd.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string appgateentrytime = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
                            string lateentry = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
                            string appgateexitdate = dd.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string appgateexittime = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
                            string[] split1 = appgateentrytime.Split(':');
                            string hr1 = split1[0];
                            string min1 = split1[1];
                            string day1 = split1[2];
                            int chr1 = Convert.ToInt32(hr1);
                            int cmin1 = Convert.ToInt32(min1);
                            string islate = "";
                            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
                            string Msg = "0";
                            string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
                            string hr = split[0];
                            string min = split[1];
                            string[] splitNew = min.Split(' ');
                            min = splitNew[0];
                            string day = splitNew[1];
                            int chr = Convert.ToInt32(hr);
                            int cmin = Convert.ToInt32(min);
                            string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                            if (appgateexitdate == currentdate)
                            {
                                string pk = dd.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
                                if (incode == "1")
                                {
                                    string timecheck = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + applid + "' and  RequisitionPK='" + gatepasspk + "'");
                                    string[] exittime = timecheck.Split(':');
                                    string gateexithr = exittime[0];
                                    string gateexitmin = exittime[1];
                                    string gateexitampm = exittime[2];
                                    if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr != 12 && chr1 != 12)
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
                                    {
                                        if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                        else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
                                        {
                                            Msg = "1";
                                        }
                                    }
                                    if (pk == "0" || pk == "" && Msg == "0")
                                    {
                                        //    string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk) values('2','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + applid + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','0','" + gatepasspk + "')";
                                        //int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                        sd.statusmsg = "0";
                                        //sms(gatepasspk, appno, "1");
                                    }
                                    else
                                    {
                                        Msg = "1";
                                        sd.statusmsg = "1";
                                    }
                                }
                                else
                                {
                                    string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + applid + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + applid + "'))");
                                    if (outcheck == "0" || outcheck == "False")
                                    {
                                        sd.statusmsg = "1";
                                    }
                                    if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        islate = "1";
                                        sd.statusmsg = "0";
                                    }
                                    else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
                                    {
                                        if (chr > chr1 && day1 == day)
                                        {
                                            islate = "1";
                                            sd.statusmsg = "0";
                                        }
                                        else if (chr == chr1 && cmin > cmin1 && day1 == day)
                                        {
                                            islate = "1";
                                            sd.statusmsg = "0";
                                        }
                                        else
                                        {
                                            islate = "0";
                                            sd.statusmsg = "0";
                                        }
                                    }
                                    //string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + applid + "' and GateMemType='2' and GateType='1' and RequestFk='" + pk + "'";
                                    //query = d2.update_method_wo_parameter(sql, "TEXT");
                                    //sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + gatepassexitdate + "'";
                                    //int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                    //if (qu > 0)
                                    //{
                                    //    sms(gatepasspk, appno, "2");
                                    //}
                                }
                            }
                            staffdet.Add(sd);
                        }
                    }
                    else
                    {
                        StaffDet sd = new StaffDet();
                        if (appno.Trim() == "" || appno.Trim() == "0")
                        {
                            sd.statusmsg = "3";
                        }
                        else if (gatepasspk.Trim() == "")
                        {
                            sd.statusmsg = "2";
                        }
                        sd.Staff_Code = "";
                        sd.Staff_Name = "";
                        sd.Department = "";
                        sd.Designation = "";
                        sd.Staff_Type = "";
                        imagestaff.ImageUrl = "";
                        sd.Photo = "";
                        sd.Mobile_No = "";
                        staffdet.Add(sd);
                    }
                    #endregion
                }
                else
                {
                    #region with out request
                    string inn = dd.GetFunction("select GateType from GateEntryExit where app_no='" + applid + "' and gatepassexitdate='" + date + "' and GateMemType='2' order by GateEntryExitid desc");
                    if (inn == "0" || inn == "False")
                    {
                        inroll = "1";
                    }
                    else
                    {
                        inroll = "0";
                    }
                    FetchData = "  select sa.appl_id,s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,s.college_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and sa.appl_id ='" + applid + "'";
                    ds = dd.select_method_wo_parameter(FetchData, "Text");
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                StaffDet sd = new StaffDet();
                                string staff_applid = Convert.ToString(dr["appl_id"]);
                                string staffname = Convert.ToString(dr["staff_name"]);
                                string code = Convert.ToString(dr["staff_code"]);
                                string dept = Convert.ToString(dr["dept_name"]);
                                string desgn = Convert.ToString(dr["desig_name"]);
                                string clgcode = Convert.ToString(dr["college_code"]);
                                sd.Staff_Code = code;
                                sd.Staff_Name = staffname + "-" + desgn + "-" + dept;
                                sd.Department = dept;
                                sd.Designation = desgn;
                                sd.Staff_Type = Convert.ToString(dr["staffcategory"]);
                                imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
                                sd.Photo = Convert.ToString(imagestaff.ImageUrl);
                                sd.Mobile_No = Convert.ToString(dr["com_mobileno"]);
                                sd.appdateExit = "";
                                sd.apptimeExit = "";
                                sd.appdateEntry = "";
                                sd.apptimeEntry = "";
                                sd.checkinorout = Convert.ToString(inroll);
                                string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
                                string gatepasstime = Convert.ToString(DateTime.Now.ToString("h:mm:tt"));
                                if (inroll == "1")
                                {
                                    //string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,GatePassTime,College_Code) values('2','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + applid + "','1','0','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + CurrentTime + "','0','" + gatepasstime + "','" + clgcode + "')";
                                    //int ud = dd.update_method_wo_parameter(sql, "TEXT");
                                    //if (ud != 0)
                                    //{
                                    //   sd.statusmsg = "0";
                                    //sms(gatepasspk, appno, "1");
                                    //}
                                    //else
                                    //{
                                    //    sd.statusmsg = "1";
                                    //}
                                }
                                else
                                {
                                    //string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='0',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + applid + "' and GateMemType='2' and GateType='1' ";
                                    //int qu = dd.update_method_wo_parameter(sql, "TEXT");
                                    //if (qu > 0)
                                    //{
                                    //    //sms(gatepasspk, appno, "2");
                                    //}
                                    //else
                                    //{
                                    //    sd.statusmsg = "1";
                                    //}
                                }
                                sd.statusmsg = "4";
                                staffdet.Add(sd);
                            }
                        }
                        else
                        {
                            StaffDet sd = new StaffDet();
                            if (appno.Trim() == "" || appno.Trim() == "0")
                            {
                                sd.statusmsg = "3";
                            }
                            else if (gatepasspk.Trim() == "")
                            {
                                sd.statusmsg = "2";
                            }
                            sd.Staff_Code = "";
                            sd.Staff_Name = "";
                            sd.Department = "";
                            sd.Designation = "";
                            sd.Staff_Type = "";
                            imagestaff.ImageUrl = "";
                            sd.Photo = "";
                            sd.Mobile_No = "";
                            staffdet.Add(sd);
                        }
                    }
                    else
                    {
                        StaffDet sd = new StaffDet();
                        if (appno.Trim() == "" || appno.Trim() == "0")
                        {
                            sd.statusmsg = "3";
                        }
                        else if (gatepasspk.Trim() == "")
                        {
                            sd.statusmsg = "2";
                        }
                        sd.Staff_Code = "";
                        sd.Staff_Name = "";
                        sd.Department = "";
                        sd.Designation = "";
                        sd.Staff_Type = "";
                        imagestaff.ImageUrl = "";
                        sd.Photo = "";
                        sd.Mobile_No = "";
                        staffdet.Add(sd);
                    }
                    #endregion
                }
            }
            return staffdet.ToArray();
        }
        catch
        {
            return staffdet.ToArray();
        }
    }
    //[WebMethod]
    //public static StaffDet[] getstaffsmartcard(string Smart_No)
    //{
    //    string data = string.Empty;
    //    List<StaffDet> staffdet = new List<StaffDet>();
    //    try
    //    {
    //        if (Smart_No.Trim() != "" && Smart_No.Length >= 10)
    //        {
    //            DataSet ds = new DataSet();
    //            DAccess2 dd = new DAccess2();
    //            Hashtable hat = new Hashtable();
    //            string FetchData = "";
    //            System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
    //            string date = DateTime.Now.ToString("MM/dd/yyyy");
    //            string appno = dd.GetFunction("select appl_no from staffmaster where smartcard_serial_no='" + Smart_No + "'");
    //            string applid = dd.GetFunction("select appl_id from staff_appl_master  where appl_no='" + appno + "'");
    //            if (incode == "1")
    //            {
    //                string rq_pk = dd.GetFunction("select Max(RequisitionPK) from RQ_Requisition where MemType='2' and RequestType=6 and ReqAppStatus='1' and GateReqEntryDate<='" + date + "' and ReqAppNo='" + applid + "'");
    //                gatepasspk = rq_pk;
    //                FetchData = " select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + applid + "' and  GateReqEntryDate>='" + date + "' and RequisitionPK ='" + gatepasspk + "'";
    //            }
    //            else
    //            {
    //                string rq_pk = dd.GetFunction(" select Max(RequestFk) from GateEntryExit where GateMemType='2' and App_No='" + applid + "' and GateType ='1'");
    //                gatepasspk = rq_pk;
    //                FetchData = "select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno,convert(varchar,g.GateReqExitDate,103) as 'GateReqExitDate',g.GateReqExitTime, convert(varchar,g.GateReqEntryDate,103) as 'GateReqEntryDate',g.GateReqEntryTime ,ReqAppStaffAppNo from RQ_Requisition g,GateEntryExit gg,staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and g.ReqAppNo=sa.appl_id and g.ReqAppStatus='1' and  g.ReqAppNo='" + applid + "' and  GateReqEntryDate>='" + date + "' and g.ReqAppNo=gg.App_No and  gg.gatetype='1' and g.ReqAppNo=gg.App_No";
    //            }
    //            ds = dd.select_method_wo_parameter(FetchData, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
    //                {
    //                    StaffDet sd = new StaffDet();
    //                    string staff_applid = ds.Tables[0].Rows[a]["ReqAppStaffAppNo"].ToString();
    //                    string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
    //                    string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
    //                    string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
    //                    string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
    //                    string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
    //                    sd.Staff_Code = ds.Tables[0].Rows[a]["staff_code"].ToString();
    //                    sd.Staff_Name = ds.Tables[0].Rows[a]["staff_name"].ToString() + "-" + ds.Tables[0].Rows[a]["desig_name"].ToString() + "-" + ds.Tables[0].Rows[a]["dept_name"].ToString();
    //                    sd.Department = ds.Tables[0].Rows[a]["dept_name"].ToString();
    //                    sd.Designation = ds.Tables[0].Rows[a]["desig_name"].ToString();
    //                    sd.Staff_Type = ds.Tables[0].Rows[a]["staffcategory"].ToString();
    //                    imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + sd.Staff_Code;
    //                    sd.Photo = Convert.ToString(imagestaff.ImageUrl);
    //                    sd.Mobile_No = ds.Tables[0].Rows[a]["com_mobileno"].ToString();
    //                    //sd.statusmsg = "0";
    //                    string entry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
    //                    string exit = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
    //                    sd.appdateExit = ds.Tables[0].Rows[a]["GateReqExitDate"].ToString();
    //                    sd.apptimeExit = ds.Tables[0].Rows[a]["GateReqExitTime"].ToString();
    //                    sd.appdateEntry = ds.Tables[0].Rows[a]["GateReqEntryDate"].ToString();
    //                    sd.apptimeEntry = ds.Tables[0].Rows[a]["GateReqEntryTime"].ToString();
    //                    string dnewdate = Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitDate"]);
    //                    string[] splitarray = dnewdate.Split('/');
    //                    DateTime dsnew = Convert.ToDateTime(splitarray[1] + "/" + splitarray[0] + "/" + splitarray[2]);
    //                    string[] splitarray1 = entry.Split('/');
    //                    DateTime dsnew1 = Convert.ToDateTime(splitarray1[1] + "/" + splitarray1[0] + "/" + splitarray1[2]);
    //                    string appgateentydate = dd.GetFunction("select convert(varchar,GateReqEntryDate,103) as GateReqEntryDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
    //                    string appgateentrytime = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) ");
    //                    string lateentry = dd.GetFunction("select GateReqEntryTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqEntryDate) and GateReqEntryDate<='" + dsnew.ToString("MM/dd/yyyy") + "' and GateReqEntryTime>='" + Convert.ToString(ds.Tables[0].Rows[a]["GateReqExitTime"]) + "'");
    //                    string appgateexitdate = dd.GetFunction("select convert(varchar,GateReqExitDate,103) as GateReqExitDate from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
    //                    string appgateexittime = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and RequisitionPK=((select max(RequisitionPK)from RQ_Requisition where ReqAppNo='" + applid + "'))and ReqAppNo='" + applid + "'and  MONTH(RequestDate)=MONTH(GateReqExitDate) ");
    //                    string[] split1 = appgateentrytime.Split(':');
    //                    string hr1 = split1[0];
    //                    string min1 = split1[1];
    //                    string day1 = split1[2];
    //                    int chr1 = Convert.ToInt32(hr1);
    //                    int cmin1 = Convert.ToInt32(min1);
    //                    string islate = "";
    //                    string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
    //                    string Msg = "0";
    //                    string[] split = Convert.ToString(DateTime.Now.ToString("hh:mm tt")).Split(':');
    //                    string hr = split[0];
    //                    string min = split[1];
    //                    string[] splitNew = min.Split(' ');
    //                    min = splitNew[0];
    //                    string day = splitNew[1];
    //                    int chr = Convert.ToInt32(hr);
    //                    int cmin = Convert.ToInt32(min);
    //                    string CurrentTime = Convert.ToString(DateTime.Now.ToString("hh:mm:tt"));
    //                    if (appgateexitdate == currentdate)
    //                    {
    //                        string pk = dd.GetFunction("select max(RequestFk) from GateEntryExit where RequestFk='" + gatepasspk + "'");
    //                        if (incode == "1")
    //                        {
    //                            //string timecheck = d2.GetFunction("select count(GateReqExitTime) as c from RQ_Requisition where RequestType=6 and ReqAppNo='" + appno + "' and  GateReqExitTime<'" + gatepastime + "'");
    //                            string timecheck = dd.GetFunction("select GateReqExitTime from RQ_Requisition where RequestType=6 and ReqAppNo='" + applid + "' and  RequisitionPK='" + gatepasspk + "'");
    //                            string[] exittime = timecheck.Split(':');
    //                            string gateexithr = exittime[0];
    //                            string gateexitmin = exittime[1];
    //                            string gateexitampm = exittime[2];
    //                            if (chr < Convert.ToInt32(gateexithr) && day == gateexitampm)
    //                            {
    //                                if (chr != 12 && Convert.ToInt32(gateexithr) != 12)
    //                                {
    //                                    Msg = "1";
    //                                }
    //                                else if (chr == 12 && cmin > Convert.ToInt32(gateexitmin))
    //                                {
    //                                    Msg = "1";
    //                                }
    //                            }
    //                            else if (chr == Convert.ToInt32(gateexithr) && day == gateexitampm)
    //                            {
    //                                if (chr != 12 && cmin <= Convert.ToInt32(gateexitmin))
    //                                {
    //                                    Msg = "1";
    //                                }
    //                                else if (chr == 12 && cmin <= Convert.ToInt32(gateexitmin))
    //                                {
    //                                    Msg = "1";
    //                                }
    //                            }
    //                            if (chr1 < Convert.ToInt32(chr) && day == gateexitampm)
    //                            {
    //                                if (chr != 12 && chr1 != 12)
    //                                {
    //                                    Msg = "1";
    //                                }
    //                            }
    //                            else if (chr1 == Convert.ToInt32(chr) && day == gateexitampm)
    //                            {
    //                                if (chr1 != 12 && cmin1 <= Convert.ToInt32(cmin))
    //                                {
    //                                    Msg = "1";
    //                                }
    //                                else if (chr == 12 && cmin1 <= Convert.ToInt32(cmin))
    //                                {
    //                                    Msg = "1";
    //                                }
    //                            }
    //                            if (pk == "0" || pk == "" && Msg == "0")
    //                            {
    //                                string sql = "insert into GateEntryExit(GateMemType,GateType,GatePassDate,GatepassExitdate,GatepassExittime,App_No,IsApproval,GatePassApproval_code,ExpectedDate,ExpectedTime,islate,RequestFk) values('2','1','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("hh:mm tt") + "','" + applid + "','1','0','" + dsnew1.ToString("MM/dd/yyyy") + "','" + exit + "','0','" + gatepasspk + "')";
    //                                //int ud = dd.update_method_wo_parameter(sql, "TEXT");
    //                                sd.statusmsg = "0";
    //                                //sms(gatepasspk, appno, "1");
    //                            }
    //                            else
    //                            {
    //                                Msg = "1";
    //                                sd.statusmsg = "1";
    //                            }
    //                        }
    //                        else
    //                        {
    //                            string outcheck = dd.GetFunction("select GateType from GateEntryExit where  App_No='" + appno + "' and GateMemType='1' and GateEntryExitID=((select max(GateEntryExitID)from GateEntryExit where App_No='" + appno + "'))");
    //                            if (outcheck == "0" || outcheck == "False")
    //                            {
    //                                sd.statusmsg = "1";
    //                            }
    //                            if (dsnew1 < Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
    //                            {
    //                                islate = "1";
    //                                sd.statusmsg = "0";
    //                            }
    //                            else if (dsnew1 == Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy")))
    //                            {
    //                                if (chr > chr1 && day1 == day)
    //                                {
    //                                    islate = "1";
    //                                    sd.statusmsg = "0";
    //                                }
    //                                else if (chr == chr1 && cmin > cmin1 && day1 == day)
    //                                {
    //                                    islate = "1";
    //                                    sd.statusmsg = "0";
    //                                }
    //                                else
    //                                {
    //                                    islate = "0";
    //                                    sd.statusmsg = "0";
    //                                }
    //                            }
    //                            string sql = "update GateEntryExit set GatepassEntrydate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',islate='" + islate + "',GatepassEntrytime='" + CurrentTime + "',GateType='0' where App_No='" + applid + "' and GateMemType='2' and GateType='1' and RequestFk='" + pk + "'";
    //                            //query = d2.update_method_wo_parameter(sql, "TEXT");
    //                            //sql = "update GateEntryExit set GatepassEntrydate='" + gatepasdate + "',islate='" + islate + "',GatepassEntrytime='" + gatepastime + "',GateType='" + gatetype + "',GatePassDate='" + gatepasdate + "',GatePassTime='" + gatepastime + "',ByVehcile='" + byvehicle + "',IsCollVeh='" + clgvehicle + "',VehType='" + vehtype + "',VehId='" + vehid + "',VehRegNo='" + vehregno + "' where App_No='" + appno + "' and GateMemType='1' and GateType='1' and GatepassExitdate='" + gatepassexitdate + "'";
    //                            //int qu = dd.update_method_wo_parameter(sql, "TEXT");
    //                            //if (qu > 0)
    //                            //{
    //                            //    sms(gatepasspk, appno, "2");
    //                            //}
    //                        }
    //                    }
    //                    staffdet.Add(sd);
    //                }
    //            }
    //            else
    //            {
    //                StaffDet sd = new StaffDet();
    //                if (appno.Trim() == "" || appno.Trim() == "0")
    //                {
    //                    sd.statusmsg = "3";
    //                }
    //                else if (gatepasspk.Trim() == "")
    //                {
    //                    sd.statusmsg = "2";
    //                }
    //                sd.Staff_Code = "";
    //                sd.Staff_Name = "";
    //                sd.Department = "";
    //                sd.Designation = "";
    //                sd.Staff_Type = "";
    //                imagestaff.ImageUrl = "";
    //                sd.Photo = "";
    //                sd.Mobile_No = "";
    //                staffdet.Add(sd);
    //            }
    //        }
    //        return staffdet.ToArray();
    //    }
    //    catch
    //    {
    //        return staffdet.ToArray();
    //    }
    //}
    public void rdo_staff_in_CheckedChanged(object sender, EventArgs e)
    {
        stafftd.BgColor = "#C4C4C4";
        div_staff.Attributes.Add("style", "display:block");
        Page.SetFocus(txt_staff_smartcard);
        if (rdo_staff_in.Checked == true)
        {
            incode = "0";
        }
    }
    public void rdo_staff_out_CheckedChanged(object sender, EventArgs e)
    {
        stafftd.BgColor = "#C4C4C4";
        div_staff.Attributes.Add("style", "display:block");
        Page.SetFocus(txt_staff_smartcard);
        if (rdo_staff_out.Checked == true)
        {
            incode = "1";
        }
    }
    public void imgbtn_student_Click(object sender, EventArgs e)
    {
        studenttd.BgColor = "#c4c4c4";
        div_student.Attributes.Add("style", "display:block");
    }
    public void Newitem()
    {
        try
        {
            //clear();
            string newitemcode = "";
            string selectquery = "select gatepassAcr,gatepassStNo,gatepassSize  from gatepass_no where college_code='" + collegecode1 + "'  order by FromDate desc";//where Latestrec =1"
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["gatepassAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["gatepassStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["gatepassSize"]);
                if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                {
                    selectquery = " select distinct top (1) gatepassno  from GateEntryExit where gatepassno like '" + Convert.ToString(itemacronym) + "[0-9]%' and College_Code='" + collegecode1 + "'order by gatepassno desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["gatepassno"]);
                        string itemacr = Convert.ToString(itemacronym);
                        int len = itemacr.Length;
                        itemcode = itemcode.Remove(0, len);
                        int len1 = Convert.ToString(itemcode).Length;
                        string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                        len = Convert.ToString(newnumber).Length;
                        len1 = len1 - len;
                        if (len1 == 2)
                        {
                            newitemcode = "00" + newnumber;
                        }
                        else if (len1 == 1)
                        {
                            newitemcode = "0" + newnumber;
                        }
                        else if (len1 == 3)
                        {
                            newitemcode = "000" + newnumber;
                        }
                        else if (len1 == 4)
                        {
                            newitemcode = "0000" + newnumber;
                        }
                        else if (len1 == 5)
                        {
                            newitemcode = "00000" + newnumber;
                        }
                        else if (len1 == 6)
                        {
                            newitemcode = "000000" + newnumber;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(newnumber);
                        }
                        if (newitemcode.Trim() != "")
                        {
                            newitemcode = itemacr + "" + newitemcode;
                        }
                    }
                    else
                    {
                        string itemacr = Convert.ToString(itemstarno);
                        int len = itemacr.Length;
                        string items = Convert.ToString(itemsize);
                        int len1 = Convert.ToInt32(items);
                        int size = len1 - len;
                        if (size == 2)
                        {
                            newitemcode = "00" + itemstarno;
                        }
                        else if (size == 1)
                        {
                            newitemcode = "0" + itemstarno;
                        }
                        else if (size == 3)
                        {
                            newitemcode = "000" + itemstarno;
                        }
                        else if (size == 4)
                        {
                            newitemcode = "0000" + itemstarno;
                        }
                        else if (size == 5)
                        {
                            newitemcode = "00000" + itemstarno;
                        }
                        else if (size == 6)
                        {
                            newitemcode = "000000" + itemstarno;
                        }
                        else
                        {
                            newitemcode = Convert.ToString(itemstarno);
                        }
                        newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    }
                    TextBox1.Text = Convert.ToString(newitemcode);
                    hid.Value = Convert.ToString(newitemcode);
                    //poperrjs.Visible = true;
                    //btnsave.Visible = true;
                    //SelectdptGrid.Visible = false;
                    //btnupdate.Visible = false;
                    // btndelete.Visible = false;
                    // bindstore();
                    // bindunitddl();
                    // loadheadername();
                    //loadsubheadername();
                    // loaditem();
                    // bind_subheader();
                }
                else
                {
                    imgdiv2.Visible = true;
                    //lbl_alert.Text = "Please Update Code Master";
                }
            }
        }
        catch
        {

        }
    }

    protected void imgbtn_visitor1_Click(object sender, EventArgs e)
    {
        try
        {
            Newitem();
        }
        catch
        {
        }
    }
    protected void TextBox1_Changed(object sender, EventArgs e)
    {
        try
        
        {
            string gst = "select VisitorName,CompanyName,GatePassDate,MobileNo,GateMemType,GateType,GatePassTime,IsApproval,ExpectedTime,purpose,ByVehcile,VehType,VehRegNo,IsReturn,VisitorName,VisitorType,VisitorDept,VisitorDesig,College_Code,GatepassEntrydate,GatepassEntrytime,tomeet,City,District,State,Add1 from GateEntryExit where gatepassno ='" + TextBox1.Text + "' and College_Code='" + collegecode1 + "'";
            DataSet printds_new = da.select_method_wo_parameter(gst, "Text");
            if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
            {
                string videg = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorDesig"]).Trim();
                txt_desgn.Text = videg;
                string videgp = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorDept"]).Trim();
                txt_dep.Text = videgp;
                string VehType = Convert.ToString(printds_new.Tables[0].Rows[0]["VehType"]).Trim();
                txt_vehtype.Text = VehType;
                string VehRegNo = Convert.ToString(printds_new.Tables[0].Rows[0]["VehRegNo"]).Trim();
                txt_vehno1.Text = VehRegNo;
                string purpose = Convert.ToString(printds_new.Tables[0].Rows[0]["purpose"]).Trim();
                txt_visit1.Text = purpose;
                string CompanyNamess = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorName"]).Trim();
                txt_name4.Text = CompanyNamess;
                string CompanyNames = Convert.ToString(printds_new.Tables[0].Rows[0]["CompanyName"]).Trim();
             //  txt_compname.Text = CompanyNames;
                che_coimp = CompanyNames;
                string Mobilesss = Convert.ToString(printds_new.Tables[0].Rows[0]["MobileNo"]).Trim();
                txt_mno.Text = Mobilesss;
                vendormobname1=Mobilesss;
                vendorname1 = CompanyNamess;

                string city = Convert.ToString(printds_new.Tables[0].Rows[0]["City"]).Trim();
                string state = Convert.ToString(printds_new.Tables[0].Rows[0]["State"]).Trim();
                string addr = Convert.ToString(printds_new.Tables[0].Rows[0]["Add1"]).Trim();
                string distr = Convert.ToString(printds_new.Tables[0].Rows[0]["District"]).Trim();
                txt_cty.Text = city;
                txt_stat.Text = state;
                txt_dis.Text=distr;
               txt_str.Text=addr;
             //  txt_phno.Visible = false;
             //  lbl_phno.Visible = false;
                  string gatepassperimissiontype = d2.GetFunction("select value from Master_Settings where settings='Gatepass Request Type' and usercode='" + usercode + "'");
                    string checkper = d2.GetFunction("select value from Master_Settings where settings='Leave Approval Permission' and usercode='" + usercode + "' ");
                    if (checkper != "3")
                    {
                        string COMPANY = d2.GetFunction("select vendorpk from CO_VendorMaster where VendorName='" + CompanyNamess + "' and VendorMobileNo='" + Mobilesss + "' order by VendorPK desc");
                        string vendorpkval = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VenContactName='" + CompanyNamess + "' and VendorMobileNo='" + Mobilesss + "' order by VendorContactPK desc");
                    //    lbl_phno.Visible = true;
                        DataSet rq_ds = new DataSet();
                        if (rb_visitout.Checked == false)
                        {
                            rq_ds = d2.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and RequestDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
                            che_inout = "in";
                        }

                        else
                        {
                            rq_ds = d2.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
                            che_inout = "out";
                        }
                        string rq_pk = "";
                        string status = ""; string Remarks = ""; string reqstaff = "";

                        if (rq_ds.Tables[0].Rows.Count > 0)
                        {
                            rq_pk = Convert.ToString(rq_ds.Tables[0].Rows[0]["RequisitionPK"]);
                            status = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppStatus"]);
                            Remarks = Convert.ToString(rq_ds.Tables[0].Rows[0]["Remarks"]);
                            reqstaff = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppNo"]);
                            Hidden1.Value = "1";
                        }
                        else
                            Hidden1.Value = "0";
                        string Q1 = string.Empty;
                        if (reqstaff != "")
                        {
                            Q1 = "select sa.appl_id,sm.staff_code,sa.dept_name,sa.desig_name,sa.appl_name,staff_type from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + reqstaff + "'";
                            DataSet staffDetails = new DataSet();
                            staffDetails = d2.select_method_wo_parameter(Q1, "Text");
                            if (staffDetails.Tables != null && staffDetails.Tables[0].Rows.Count > 0)
                            {
                                txt_dpt1.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["dept_name"]);
                                txt_desg1.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["desig_name"]);
                                txt_type.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["staff_type"]);
                                sname.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["appl_name"]);
                            }
                        }
                    }
                          string approv = Convert.ToString(printds_new.Tables[0].Rows[0]["isapproval"]);
                          string meet = Convert.ToString(printds_new.Tables[0].Rows[0]["tomeet"]);
                             string dep = string.Empty;
                string depname = string.Empty;
                string stf = string.Empty;
                string stfname = string.Empty;
                string MOb = string.Empty;
                string Mobli = string.Empty;
                string code = string.Empty;
                string code1 = string.Empty;
                DataSet steaff = new DataSet();
                DataSet steaff1 = new DataSet();
                string stafname = string.Empty;
                string mobile = string.Empty;
                string getID = string.Empty;
                string detsql = string.Empty;
                string staff_names = string.Empty;

                if (approv == "False")
                {
                    div_withoutappoint.Visible = true;
                    getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where gatepassno='" + TextBox1.Text + "' ");
                    if (meet == "0")
                    {
                        detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                        if (detsql != "")
                        {
                            //select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0  and dm.collegeCode=s.college_code and sa.college_code=s.college_code and hr.college_code=s.college_code and 
                            staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,hr.dept_name,dm.desig_name,dm.staffcategory from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                            DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                            if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                            {
                                txt_visitormeetstaffname.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                                txt_visitormeetstaffid.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_code"]);
                                txt_visitormeetstaffdept.Text = Convert.ToString(steafs.Tables[0].Rows[0]["dept_name"]);
                                txt_visitormeetstaffdesg.Text = Convert.ToString(steafs.Tables[0].Rows[0]["desig_name"]);
                                
                            }

                        }
                    }
                    if (meet == "1")
                    {
                        detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                        if (detsql != "")
                        {
                            staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,hr.dept_name,dm.desig_name,dm.staffcategory from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                            DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                            if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                            {
                                txt_visitormeetoffname.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                                txt_visitormeetoffdept.Text = Convert.ToString(steafs.Tables[0].Rows[0]["dept_name"]);
                                txt_visitormeetoffdesg.Text = Convert.ToString(steafs.Tables[0].Rows[0]["desig_name"]);
                               
                            }

                        }
                    }
                    if (meet == "2")
                    {
                        detsql = " select OtherName,Relationship,MobileNo from GateEntryExitDet where GateEntryExitID='" + getID + "'";

                        DataSet steafs = d2.select_method_wo_parameter(detsql, "text");
                        if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                        {
                            txt_visitormeetothername.Text = Convert.ToString(steafs.Tables[0].Rows[0]["OtherName"]);

                            txt_visitormeetothermob.Text = Convert.ToString(steafs.Tables[0].Rows[0]["MobileNo"]);
                            txt_visitormeetotherrel.Text = Convert.ToString(steafs.Tables[0].Rows[0]["Relationship"]);
                          

                        }


                    }
                }
               



                         // txt_mno_Changed(sender, e);
                      
                  
                  
            }

        //    txt_compname_Changed(sender, e);
            txt_mno_Changed(sender, e);
            txt_mno.Focus();
            imgdiv2.Visible = false;
            txt_name4.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_mno.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_cty.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_visit1.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_vehno1.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_vehtype.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_dep.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_desgn.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_stat.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_dis.BackColor = ColorTranslator.FromHtml("#ffffcc");
            txt_str.BackColor = ColorTranslator.FromHtml("#ffffcc");
           
            VisitorCompany vc = new VisitorCompany();
            vc.Appointment = "0";
        
            List<VisitorCompany> compdet = new List<VisitorCompany>();
            compdet.Add(vc);
            div_withoutappoint.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "vis", "vis();", true);
            div_visitormeetstaff.Visible = true;
           
        }
        catch
        {
        }
    }
    public void txt_name_TextChanged(object sender, EventArgs e)
    {
        try
        {
            deptp = txt_name.Text;
            string[] spl = deptp.Split('-');
            txt_rollno.Text = spl[1];
            ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno()", "getsmartrollno('" + txt_rollno.Text + "');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
         
        }
        catch
        {
        }
    }
    //public void Txtde_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        deptp = Txtde.Text;
    //        string[] spl = deptp.Split('-');
    //        txt_rollno.Text = spl[1];

    //    }
    //    catch
    //    {
    //    }
    //}
    public void servertime()
    {
        string server = "SELECT distinct cast(datepart(m,getdate()) as nvarchar) + '/' + cast(datepart(d,getdate()) as nvarchar) + '/' + cast(datepart(yyyy,getdate()) as nvarchar) ,cast(datepart(hh,getdate()) as nvarchar) + ':' + cast(datepart(n,getdate()) as nvarchar) + ':' + cast(datepart(s,getdate()) as nvarchar) as time";
        DataSet ds=d2.select_method_wo_parameter(server,"text");
        string tim = Convert.ToString(ds.Tables[0].Rows[0]["time"]);
        
        txt_visittime.Text = tim;
        txt_time.Text = tim;
    }
    public void txt_mno_Changed(object sender, EventArgs e)
    {
        try
        {
           string app = string.Empty;
            string query = string.Empty;
            string staffcodesession = Session["Staff_Code"].ToString();

            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
            vendorname1 = txt_name4.Text;
            vendormobname1 = txt_mno.Text;
            string[] split1 = currentdate.Split('/');
            string date = split1[0];
            string month = split1[1];
            string year = split1[2];
            string dat= split1[2]+'-'+ split1[1]+'-'+split1[0];
           
                string gatepassperimissiontype = d2.GetFunction("select value from Master_Settings where settings='Gatepass Request Type' and usercode='" + usercode + "'");
                if (gatepassperimissiontype.Trim() == "0")
                {
                    string checkper = d2.GetFunction("select value from Master_Settings where settings='Leave Approval Permission' and usercode='" + usercode + "' ");
                    if (checkper != "3")
                    {
                        app = d2.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staffcodesession + "'");
                        query = "SELECT r.RequisitionPK,r.ReqApproveStage,ReqAppStatus,CASE WHEN r.RequestType = 3 THEN 'Visitor Appointment Request' END RequestType,RequestCode,ReqAppNo,CONVERT(VARCHAR(11),RequestDate,103) as RequestDate,CONVERT(VARCHAR(11),ReqExpectedDate,103) as ReqExpectedDate,ReqExpectedTime,v.VendorPhoneNo,VendorCompName,VenContactName,Remarks,c.VenContactDesig,c.VendorMobileNo,c.VenContactDept FROM RQ_Requisition R,CO_VendorMaster V,IM_VendorContactMaster C,RQ_RequestHierarchy rh WHERE R.VendorFK = V.VendorPK AND V.VendorPK = C.VendorFK and r.RequestType=3 and r.ReqAppStatus=1 and  VendorContactFK=VendorContactPK and r.RequestType =rh.RequestType and r.ReqAppNo =rh.ReqStaffAppNo and RequestDate ='" + dat + "' and rh.ReqAppStaffAppNo ='" + app + "'";
                    }
                    else
                    {
                        app = d2.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + staffcodesession + "'");
                        query = "SELECT r.RequisitionPK,r.ReqApproveStage,ReqAppStatus,CASE WHEN r.RequestType = 3 THEN 'Visitor Appointment Request' END RequestType,RequestCode,ReqAppNo,CONVERT(VARCHAR(11),RequestDate,103) as RequestDate,CONVERT(VARCHAR(11),ReqExpectedDate,103) as ReqExpectedDate,ReqExpectedTime,VendorCompName,VenContactName,v.VendorPhoneNo,Remarks,c.VendorMobileNo,c.VenContactDesig,c.VenContactDept,v.VendorAddress FROM RQ_Requisition R,CO_VendorMaster V,IM_VendorContactMaster C WHERE R.VendorFK = V.VendorPK AND V.VendorPK = C.VendorFK and r.RequestType=3 and r.ReqAppStatus=1 and  VendorContactFK=VendorContactPK   and RequestDate ='" + dat + "' and r.ReqAppNo ='" + app + "' and VenContactName='" + txt_name4.Text + "' and c.VendorMobileNo='" + txt_mno.Text + "' ";
                        //query = "select r.VendorFK,VendorContactFK,r.MeetToStaff,r.MeetStaffAppNo,CONVERT(VARCHAR(11),ReqExpectedDate,103) as ReqExpectedDate,CONVERT(VARCHAR(11),RequestDate,103) as RequestDate,im.VendorCompName ,v.VenContactName ,v.VenContactDesig,v.VenContactDept,v.VendorMobileNo,v.VendorPhoneNo ,v.VendorEmail,r.MeetToDept ,r.ReqExpectedTime ,r.MeetDeptCode ,Remarks,im.VendorAddress  from RQ_Requisition r,IM_VendorContactMaster v,CO_VendorMaster im where r.VendorFK =im.VendorPK and r.VendorContactFK =v.VendorContactPK and im.VendorPK =v.VendorFK and RequestType=3 and RequestCode='" + req_no + "' ";
                        txt_name4.BackColor = Color.White;
                        txt_mno.BackColor = Color.White;
                        txt_cty.BackColor = ColorTranslator.FromHtml("#ffffcc");

                        txt_dep.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_desgn.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_stat.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_dis.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_str.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_phno.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        txt_compname.BackColor = ColorTranslator.FromHtml("#ffffcc");
                        TextBox1.BackColor = ColorTranslator.FromHtml("#ffffcc");

                    }
                    DataSet printds_new = da.select_method_wo_parameter(query, "Text");
                    if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
                    {
                        string videg = Convert.ToString(printds_new.Tables[0].Rows[0]["VenContactDesig"]).Trim();
                        txt_desgn.Text = videg;
                        string videgp = Convert.ToString(printds_new.Tables[0].Rows[0]["VenContactDept"]).Trim();
                        txt_dep.Text = videgp;
                        string pho = Convert.ToString(printds_new.Tables[0].Rows[0]["VendorPhoneNo"]).Trim();
                        txt_phno.Text = pho;
                        //string VehType = Convert.ToString(printds_new.Tables[0].Rows[0]["VehType"]).Trim();
                        //txt_vehtype.Text = VehType;
                        //string VehRegNo = Convert.ToString(printds_new.Tables[0].Rows[0]["VehRegNo"]).Trim();
                        //txt_vehno1.Text = VehRegNo;
                        string purpose = Convert.ToString(printds_new.Tables[0].Rows[0]["Remarks"]).Trim();
                        txt_visit1.Text = purpose;
                        //string CompanyNamess = Convert.ToString(printds_new.Tables[0].Rows[0]["VisitorName"]).Trim();
                        //txt_name4.Text = CompanyNamess;
                        string Mobilesss = Convert.ToString(printds_new.Tables[0].Rows[0]["VendorMobileNo"]).Trim();
                        txt_mno.Text = Mobilesss;
                        //string city = Convert.ToString(printds_new.Tables[0].Rows[0]["City"]).Trim();
                        //string state = Convert.ToString(printds_new.Tables[0].Rows[0]["State"]).Trim();
                        // string addr = Convert.ToString(printds_new.Tables[0].Rows[0]["VendorAddress"]).Trim();
                        //string distr = Convert.ToString(printds_new.Tables[0].Rows[0]["District"]).Trim();
                        //txt_cty.Text = city;
                        //txt_stat.Text = state;
                        //txt_dis.Text = distr;
                        // txt_str.Text = addr;
                        txt_compname.Text = Convert.ToString(printds_new.Tables[0].Rows[0]["VendorCompName"]).Trim();
                     txt_compname.Text = "";
                    }
                    else
                    {
                        txt_name4.BackColor = Color.White;
                        txt_mno.BackColor = Color.White;
                        txt_cty.BackColor = Color.White;

                        txt_dep.BackColor = Color.White;
                        txt_desgn.BackColor = Color.White;
                        txt_stat.BackColor = Color.White;
                        txt_dis.BackColor = Color.White;
                        txt_str.BackColor = Color.White;
                        txt_phno.BackColor = Color.White;
                        txt_compname.BackColor = Color.White;
                        TextBox1.BackColor = ColorTranslator.FromHtml("#ffffcc");
                    }

                }
                else//added by rajasekar 19/07/2018
                {
                    rb_staff1.Visible = false;
                    rb_office1.Visible = false;
                    rb_others1.Visible = false;
                    lbl_visitormeetstaffid.Visible = false;
                    txt_visitormeetstaffid.Visible = false;
                    lbl_visitormeetstaffname.Visible = false;
                    txt_visitormeetstaffname.Visible = false;
                    lbl_visitormeetstaffdept.Visible = false;
                    txt_visitormeetstaffdept.Visible = false;
                    lbl_visitormeetstaffdesg.Visible = false;
                    txt_visitormeetstaffdesg.Visible = false;
                }
            string checkpers = d2.GetFunction("select value from Master_Settings where settings='Leave Approval Permission' and usercode='" + usercode + "' ");
            if (checkpers == "3")
            {
                string COMPANY = d2.GetFunction("select vendorpk from CO_VendorMaster where VendorName='" + txt_name4.Text + "' and VendorMobileNo='" + txt_mno.Text + "' order by VendorPK desc");
                string vendorpkval = d2.GetFunction("select VendorContactPK from IM_VendorContactMaster where VenContactName='" + txt_name4.Text + "' and VendorMobileNo='" + txt_mno.Text + "' order by VendorContactPK desc");
              //  lbl_phno.Visible = true;
                DataSet rq_ds = new DataSet();
                if (rb_visitout.Checked == false)
                {
                    rq_ds = d2.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and RequestDate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
                    che_inout = "in";
                }

                else
                {
                    rq_ds = d2.select_method_wo_parameter("select RequisitionPK,ReqAppStatus,Remarks,ReqAppNo from RQ_Requisition where RequestType=3  and VendorContactFK='" + vendorpkval + "' and ReqApproveStage='1' order by RequisitionPK desc", "Text");//magesh 7.6.18 and VendorFK='" + COMPANY + "'
                    che_inout = "out";
                }
                string rq_pk = "";
                string status = ""; string Remarks = ""; string reqstaff = "";

                if (rq_ds.Tables[0].Rows.Count > 0)
                {
                    rq_pk = Convert.ToString(rq_ds.Tables[0].Rows[0]["RequisitionPK"]);
                    status = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppStatus"]);
                    Remarks = Convert.ToString(rq_ds.Tables[0].Rows[0]["Remarks"]);
                    reqstaff = Convert.ToString(rq_ds.Tables[0].Rows[0]["ReqAppNo"]);
                    Hidden1.Value = "1";
                }
                else
                    Hidden1.Value = "0";
                string Q1 = string.Empty;
                if (reqstaff != "")
                {
                    Q1 = "select sa.appl_id,sm.staff_code,sa.dept_name,sa.desig_name,sa.appl_name,staff_type from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + reqstaff + "'";
                    DataSet staffDetails = new DataSet();
                    staffDetails = d2.select_method_wo_parameter(Q1, "Text");
                    if (staffDetails.Tables != null && staffDetails.Tables[0].Rows.Count > 0)
                    {
                        txt_dpt1.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["dept_name"]);
                        txt_desg1.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["desig_name"]);
                        txt_type.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["staff_type"]);
                        sname.Text = Convert.ToString(staffDetails.Tables[0].Rows[0]["appl_name"]);
                    }
                }
                //string approv = Convert.ToString(printds_new.Tables[0].Rows[0]["isapproval"]);
                //string meet = Convert.ToString(printds_new.Tables[0].Rows[0]["tomeet"]);
                string dep = string.Empty;
                string depname = string.Empty;
                string stf = string.Empty;
                string stfname = string.Empty;
                string MOb = string.Empty;
                string Mobli = string.Empty;
                string code = string.Empty;
                string code1 = string.Empty;
                DataSet steaff = new DataSet();
                DataSet steaff1 = new DataSet();
                string stafname = string.Empty;
                string mobile = string.Empty;
                string getID = string.Empty;
                string detsql = string.Empty;
                string staff_names = string.Empty;

                if (reqstaff == "")
                {
                    Hidden1.Value = "0";
                    rb_staff1.Checked = true;
                }
                //if (approv =="False")
                //{
                //    getID = d2.GetFunction("select GateEntryExitID from GateEntryExit where gatepassno='" +TextBox1.Text +"' ");
                //     if (meet == "0")
                //     {
                //         detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                //         if (detsql != "")
                //         {
                //             //select s.staff_code,s.staff_name,hr.dept_name,dm.desig_name,dm.staffcategory,sa.com_mobileno from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0  and dm.collegeCode=s.college_code and sa.college_code=s.college_code and hr.college_code=s.college_code and 
                //             staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,hr.dept_name,dm.desig_name,dm.staffcategory from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                //             DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                //             if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                //             {
                //                 txt_visitormeetstaffname.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                //                 txt_visitormeetstaffid.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_code"]);
                //                 txt_visitormeetstaffdept.Text = Convert.ToString(steafs.Tables[0].Rows[0]["dept_name"]);
                //                 txt_visitormeetstaffdesg.Text = Convert.ToString(steafs.Tables[0].Rows[0]["desig_name"]);
                //             }

                //         }
                //     }
                //     if (meet == "1")
                //     {
                //         detsql = d2.GetFunction(" select Staff_Code from GateEntryExitDet where GateEntryExitID='" + getID + "'");
                //         if (detsql != "")
                //         {
                //             staff_names = ("select s.staff_name,s.staff_code,sa.com_mobileno,hr.dept_name,dm.desig_name,dm.staffcategory from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code='" + detsql + "'");
                //             DataSet steafs = d2.select_method_wo_parameter(staff_names, "text");
                //             if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                //             {
                //                 txt_visitormeetoffname.Text = Convert.ToString(steafs.Tables[0].Rows[0]["staff_name"]);
                //                 txt_visitormeetoffdept.Text = Convert.ToString(steafs.Tables[0].Rows[0]["dept_name"]);
                //                 txt_visitormeetoffdesg.Text = Convert.ToString(steafs.Tables[0].Rows[0]["desig_name"]);
                //                 rb_office1.Checked = true;
                //             }

                //         }
                //     }
                //     if (meet == "2")
                //     {
                //         detsql = " select OtherName,Relationship,MobileNo from GateEntryExitDet where GateEntryExitID='" + getID + "'";

                //         DataSet steafs = d2.select_method_wo_parameter(detsql, "text");
                //             if (steafs.Tables.Count > 0 && steafs.Tables[0].Rows.Count > 0)
                //             {
                //                 txt_visitormeetothername.Text = Convert.ToString(steafs.Tables[0].Rows[0]["OtherName"]);

                //                 txt_visitormeetothermob.Text = Convert.ToString(steafs.Tables[0].Rows[0]["MobileNo"]);
                //                 txt_visitormeetotherrel.Text = Convert.ToString(steafs.Tables[0].Rows[0]["Relationship"]);
                //                 rb_others1.Checked = true;

                //             }


                //     }
                //}
            }
                txt_compname_Changed(sender, e);
              //  txt_str.Focus();
                ScriptManager.RegisterStartupScript(this, GetType(), "visitorcompdet", "visitorcompdet(" + txt_compname.Text + ");", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "vis", "vis();", true);
           
        }
        catch
        {
        }
    }



    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getdept(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select   Course_Name +'-'+ dept_name as dept from course c,department where c.college_code='"+colg+"' and  Course_Name like '" + prefixText + "%'    ";

        name = ws.Getname(query);
        return name;
    }

   
    [WebMethod]

    public static List<string> namede(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string cours = string.Empty;
        string depttt = string.Empty;
        if (deptar != "")
        {
            string[] spl = deptar.Split('-');
            cours = spl[0];
            depttt = spl[1];
        }
        string query=string.Empty;
        //Page page = (Page)HttpContext.Current.Handler;
        //TextBox TextBox1 = (TextBox)page.FindControl("Txtde");

        if (cours != "" && depttt!="")
            query = "select a.stud_name+'-'+r.Roll_No+'-'+a.parent_name+'-'+c.Course_Name+'-'+dt.Dept_Name as stuname  from Registration r,applyn a,Degree d,course c,Department dt where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and c.Course_Name='" + cours + "' and dt.Dept_Name='" + depttt + "' and  a.stud_name like '" + prefixText + "%'";//and c.Course_Name='" + cours + "' and dt.Dept_Name='" + deptt + "'
        else
            query = "select a.stud_name+'-'+r.Roll_No+'-'+a.parent_name+'-'+c.Course_Name+'-'+dt.Dept_Name as stuname  from Registration r,applyn a,Degree d,course c,Department dt where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code  and  a.stud_name like '" + prefixText + "%'";//and c.Course_Name='" + cours + "' and dt.Dept_Name='" + deptt + "'



            name = ws.Getname(query);
            return name;
       
    }





    [WebMethod]
    public static Student[] studrolls(string deptt, string j)
    {

        string data = string.Empty;
        string usercode = j;
        List<Student> details = new List<Student>();
        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagestaff = new System.Web.UI.WebControls.Image();
        //Added By Saranyadevi 4.2.2018
        System.Web.UI.WebControls.Image imagefar = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imagemon = new System.Web.UI.WebControls.Image();
        System.Web.UI.WebControls.Image imageguar = new System.Web.UI.WebControls.Image();
        try
        {
            if (deptt.Trim() != "")
            {
                string query = "";
                DataSet ds = new DataSet();
                DAccess2 dd = new DAccess2();
                Hashtable hat = new Hashtable();
                string dat = System.DateTime.Now.ToString("yyyy/MM/dd");
                string date = DateTime.Now.ToString("MM/dd/yyyy");
                string reques = string.Empty;
                 deptp = deptt;
                string[] spl = deptp.Split('-');
                string studetail = "select a.stud_name,r.app_no, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections, a.parent_name, r.college_code from applyn a,Registration r ,Degree d,course c,Department dt where   a.app_no=r.app_no  and  r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and  d.Dept_Code=dt.Dept_Code and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR'  and r.Roll_no='" + spl[1] + "' ";
               
                    ds = dd.select_method_wo_parameter(studetail, "Text");
                    if (ds.Tables.Count > 0  && ds.Tables[0].Rows.Count > 0)
                    {
                        string staffname = string.Empty;
                        string dept = string.Empty;
                        string staff_code = string.Empty;
                        string desgn = string.Empty;
                        for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                        {
                            Student m= new Student();
                            //string staff_applid = ds.Tables[0].Rows[a]["ReqStaffAppNo"].ToString();
                            //string staffname = dd.GetFunction("select appl_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string code = dd.GetFunction("select staff_code from staff_appl_master sa, staffmaster sm where sa.appl_no=sm.appl_no and appl_id='" + staff_applid + "'");
                            //string dept = dd.GetFunction("select dept_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string desgn = dd.GetFunction("select desig_name from staff_appl_master where appl_id='" + staff_applid + "'");
                            //string staff_code = dd.GetFunction("select staff_code from staffmaster where staff_name='" + staffname + "'");
                           
                            //s.Name = ds.Tables[0].Rows[a]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[a]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[a]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            m.RollNo = ds.Tables[0].Rows[a]["Roll_no"].ToString();
                            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + m.RollNo;
                            m.photo = Convert.ToString(img2.ImageUrl);
                            m.Student_Type = ds.Tables[0].Rows[a]["Stud_Type"].ToString();
                            m.Degree = ds.Tables[0].Rows[a]["Course_Name"].ToString();
                            m.Department = ds.Tables[0].Rows[a]["Dept_Name"].ToString();
                            m.Semester = ds.Tables[0].Rows[a]["Current_Semester"].ToString();
                            m.Section = ds.Tables[0].Rows[a]["Sections"].ToString();
                            string clgcode = ds.Tables[0].Rows[a]["college_code"].ToString();
                            m.AppNo = ds.Tables[0].Rows[a]["app_no"].ToString();
                            imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + m.AppNo;
                            m.Regvisitfarphoto = Convert.ToString(imagefar.ImageUrl);
                            imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + m.AppNo;
                            m.Regvisitmonphoto = Convert.ToString(imagemon.ImageUrl);
                            imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + m.AppNo;
                            m.Regvisitgaurphoto = Convert.ToString(imageguar.ImageUrl);

                            details.Add(m);

                            //s.statusmsg = Msg.ToString();
                        }
                    }
              
                
            }
            return details.ToArray();
        }
        catch
        {
            Student s = new Student();
            s.Name = "";
            s.RollNo = "";
            img2.ImageUrl = "Handler/Handler4.ashx?rollno=" + s.RollNo;
            s.photo = Convert.ToString(img2.ImageUrl);
            s.Student_Type = "";
            s.Degree = "";
            s.Department = "";
            s.Semester = "";
            s.Section = "";
            s.statusmsg = "2";
            s.staffcode = "";
            s.staffname = "";
            s.staffdept = "";
            s.staffdesg = "";
            imagestaff.ImageUrl = "~/Handler/staffphoto.ashx?Staff_Code=" + s.staffcode;
            s.staffphoto = "";
            s.appdateExit = "";
            s.apptimeExit = "";
            s.appdateEntry = "";
            s.apptimeEntry = "";
            s.purpose = "";
            imagefar.ImageUrl = "~/Handler/Handler7.ashx?app_no=" + s.AppNo;
            imagemon.ImageUrl = "~/Handler/Handler8.ashx?app_no=" + s.AppNo;
            imageguar.ImageUrl = "~/Handler/Handler9.ashx?app_no=" + s.AppNo;

            details.Add(s);
            return details.ToArray();
        }
    }
   
    [WebMethod]
    public static string ProcessIT(string names)
    {
        string result =  names;
        deptar = names;
        return result;
    }

    protected void tmrTTStat_OnTick(object sender, EventArgs e)
    {
        try
        {
            //showTimeTable(lblAppNo.Text.Trim());
            //string sec = Convert.ToString(ViewState["sections"]);
            //if (sec != "")
            //{
            //    sec = " and st.sections='" + sec + "'";
            //}

            LoadElectiveSubjects();


           
                    string j = string.Empty;

                    
            j = lblUserCode.Text;
                    //Hidden2.Value= "1";
                    // studroll(txt_rollno.Text, j);

                    // Hidden3.Value=nameinout;
                    // Hidden4.Value = rollnoinout;
                    // Hidden5.Value = stanameinout;
                    // Hidden6.Value = stadeoptinout;
                    // Hidden7.Value = deptinout;
                    // //iphotoinout= s.photo;
                    // //inginout= img2.ImageUrl;
                    // Hidden8.Value = Student_Typeinout;
                    // Hidden9.Value = Degreeinout;
                    // Hidden10.Value = Departmentinout;
                    // Hidden11.Value = AppNoinout;
                    // Hidden12.Value= msginout;
                    //  txt_rollno_OnTextChanged(sender, e);
                  
                    string RollNo = string.Empty;
                   RollNo = txt_rollno.Text;
                  //  ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno()", "getsmartrollno(" + txt_rollno.Text + ")();", true);
                    #region(stud photo)
                    //if (txt_rollno.Text != "")
                    //{
                    //    string stdphtsql = string.Empty;
                    //    stdphtsql = "select * from StdPhoto where app_no='" + Label2.Text + "'";
                    //    MemoryStream memoryStream = new MemoryStream();
                    //    DataSet dsstdpho = new DataSet();
                    //    dsstdpho.Clear();
                    //    dsstdpho.Dispose();
                    //    dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
                    //    if (dsstdpho.Tables.Count > 0 && dsstdpho.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dsstdpho.Tables[0].Rows[0][1]).Trim()))
                    //    {
                    //        byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                    //        memoryStream.Write(file, 0, file.Length);
                    //        if (file.Length > 0)
                    //        {
                    //            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                    //            System.Drawing.Image thumb = imgx.GetThumbnailImage(190, 190, null, IntPtr.Zero);
                    //            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + Label2.Text + ".jpeg")))
                    //            {

                    //            }
                    //            else
                    //            {
                    //                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + Label2.Text + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    //            }
                    //        }
                    //    }
                    //    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + Label2.Text + ".jpeg")))
                    //    {
                    //        imageregvisitfar1.ImageUrl = "~/coeimages/" + Label2.Text + ".jpeg";
                    //    }
                    //    else
                    //    {
                    //        imageregvisitfar1.ImageUrl = "~/coeimages/NoImage.jpeg";
                    //    }
                    //    Page_Load(sender,e);
                    //}
                    #endregion



                   ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno()", "getsmartrollno('" + txt_rollno.Text + "');", true);
                 

        //  txt_rollno_OnTextChanged(sender, e);
         // ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno()", "getsmartrollno(" + txt_rollno.Text + ");", true);
        
                }
          
        catch { }
    }

    public void txt_rollno_OnTextChanged(object sender, EventArgs e)
    
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno()", "getsmartrollno('" + txt_rollno.Text + "');", true);
 ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
    }
    //protected void LoadElectiveSubjects()
    //{
    //    try
    //    {
    //        string subcode = string.Empty;
    //       string staffcode = string.Empty;
    //        string Date = DateTime.Now.ToString("dd/MM/yyyy");
    //        string year = DateTime.Now.Year.ToString();
    //        string Month = DateTime.Now.Month.ToString();


    //        string sql = "select distinct top 1 userid,LogDate,d.inout from DeviceLogs_" + Month + "_" + year + " d where CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";



    //        DataSet printds_new = da.select_method_wo_parameter_Biometric(sql, "Text");
    //        for (int m = 0; m < printds_new.Tables[0].Rows.Count; m++)
    //        {
    //        string sql1 = "select distinct top 1 finger_id,r.roll_no,r.app_no from Registration r where r.finger_id ='" + Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]) + "'";

    //            DataSet printds_new1 = da.select_method_wo_parameter(sql1, "Text");
    //            if (printds_new1.Tables.Count > 0 && printds_new1.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < printds_new1.Tables[0].Rows.Count; i++)
    //                {
                       
    //                    //string inoutchk = "0";
    //               string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
    //                 string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]).Trim();
    //                  // string finid = "101";
    //                    //string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();

    //                    if (inoutchk == "" || inoutchk == "0")
    //                    {
    //                        string getroll = Convert.ToString(printds_new1.Tables[0].Rows[0]["roll_no"]).Trim();
    //                        txt_rollno.Text = getroll;
    //                        string getroll1 = Convert.ToString(printds_new1.Tables[0].Rows[0]["app_no"]).Trim();
    //                        Label2.Text = getroll1;
    //                        //string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'";
    //                        //int query = d2.update_method_wo_parameter(sql2, "TEXT");




    //                    }
    //                    else
    //                    {
    //                        Label2.Text = "";

    //                        txt_rollno.Text = "";
    //                        ScriptManager.RegisterStartupScript(this, GetType(), "studentclear()", "studentclear()", true);

    //                    }
    //                }
    //            }
    //       }

    //    }
    //    catch
    //    {
    //    }
    //}
    //public void btn_Question_Bank_popup_Click(object sender, EventArgs e)
    //{
    //    string j = string.Empty;
    //    sec = 0;
    //    j = lblUserCode.Text;
      
    //    string date = DateTime.Now.ToString("MM/dd/yyyy");
    //     string saql=string.Empty;
    //    if( Label2.Text !="")
    //     saql = "update GateEntryExit set stu_Relationship='2' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No=" + Label2.Text + " order by GateEntryExitID desc)";
    //                                    int ud = d2.update_method_wo_parameter(saql, "TEXT");

       
    //    string Date = DateTime.Now.ToString("dd/MM/yyyy");
    //    string year = DateTime.Now.Year.ToString();
    //    string Month = DateTime.Now.Month.ToString();

        
    //        //string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";
    //    string sql = "select distinct top 1 userid,LogDate,d.inout from DeviceLogs_" + Month + "_" + year + " d where  CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";
    //    DataSet printds_new = da.select_method_wo_parameter_Biometric(sql, "Text");
    //        for (int m = 0; m < printds_new.Tables[0].Rows.Count; m++)
    //        {
    //            string sql1 = "select distinct top 1 finger_id,r.roll_no,r.app_no from Registration r where r.finger_id = '" + Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]) + "'";

    //            DataSet printds_new1 = da.select_method_wo_parameter(sql1, "Text");
    //            // DataSet printds_new = da.select_method_wo_parameter1(sql, "Text");
    //            if (printds_new1.Tables.Count > 0 && printds_new1.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < printds_new1.Tables[0].Rows.Count; i++)
    //                {
    //                    string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
    //                    string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]).Trim();

    //                    string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
    //                    string sql2 = "update DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'";
    //                    int query = d2.update_method_wo_parameter_Biometric(sql2, "TEXT");
    //                }
    //            }
    //        }
    //        ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
    //        ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
           
           
    //}

    //public void btn_Question_Bank_popup1_Click(object sender, EventArgs e)
    //{
    //    string j = string.Empty;
    //    sec = 0;
    //    j = lblUserCode.Text;
      
    //    string date = DateTime.Now.ToString("MM/dd/yyyy");
    //      string saql=string.Empty;
    //    if( Label2.Text !="")
    //     saql = "update GateEntryExit set stu_Relationship='3' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No=" + Label2.Text + " order by GateEntryExitID desc)";
    //    int ud = d2.update_method_wo_parameter(saql, "TEXT");


    //    string Date = DateTime.Now.ToString("dd/MM/yyyy");
    //    string year = DateTime.Now.Year.ToString();
    //    string Month = DateTime.Now.Month.ToString();


    // // string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";


    // string sql = "select distinct top 1 userid,LogDate,d.inout from DeviceLogs_" + Month + "_" + year + " d where  CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";
    // DataSet printds_new = da.select_method_wo_parameter_Biometric(sql, "Text");
    //        for (int m = 0; m < printds_new.Tables[0].Rows.Count; m++)
    //        {
    //            string sql1 = "select distinct top 1 finger_id,r.roll_no,r.app_no from Registration r where r.finger_id = '" + Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]) + "'";

    //            DataSet printds_new1 = da.select_method_wo_parameter(sql1, "Text");
    //            // DataSet printds_new = da.select_method_wo_parameter1(sql, "Text");
    //            if (printds_new1.Tables.Count > 0 && printds_new1.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < printds_new1.Tables[0].Rows.Count; i++)
    //                {
    //                    string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
    //                    string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]).Trim();

    //                    string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
    //                    string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'";
    //                    int query = d2.update_method_wo_parameter_Biometric(sql2, "TEXT");
    //                }
    //            }
    //    }

    //    ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
    //    ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
    //}

    //public void btn_Question_Bank_popup3_Click(object sender, EventArgs e)
    //{
    //    string j = string.Empty;
    //    sec = 0;
    //    j = lblUserCode.Text;
      
    //    string date = DateTime.Now.ToString("MM/dd/yyyy");
    //      string saql=string.Empty;
    //    if( Label2.Text !="")
    //     saql = "update GateEntryExit set stu_Relationship='1' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No="+ Label2.Text +" order by GateEntryExitID desc)";
    //    int ud = d2.update_method_wo_parameter(saql, "TEXT");


    //    string Date = DateTime.Now.ToString("dd/MM/yyyy");
    //    string year = DateTime.Now.Year.ToString();
    //    string Month = DateTime.Now.Month.ToString();


    //  //  string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";



    //   string sql = "select distinct top 1 userid,LogDate,d.inout from DeviceLogs_" + Month + "_" + year + " d where  CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  order by LogDate desc";
    //   DataSet printds_new = da.select_method_wo_parameter_Biometric(sql, "Text");
    //        for (int m = 0; m < printds_new.Tables[0].Rows.Count; m++)
    //        {
    //            string sql1 = "select distinct top 1 finger_id,r.roll_no,r.app_no from Registration r where r.finger_id = '" + Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]) + "'";

    //            DataSet printds_new1 = da.select_method_wo_parameter(sql1, "Text");
    //            // DataSet printds_new = da.select_method_wo_parameter1(sql, "Text");
    //            if (printds_new1.Tables.Count > 0 && printds_new1.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < printds_new1.Tables[0].Rows.Count; i++)
    //                {
    //                    string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
    //                    string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["userid"]).Trim();

    //                    string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
    //                    string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'";
    //                    int query = d2.update_method_wo_parameter_Biometric(sql2, "TEXT");
    //                }
    //            }
    //        }

    //    ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
    //    ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
    //}

    protected void LoadElectiveSubjects()
    {
        try
        {
            string subcode = string.Empty;
            string staffcode = string.Empty;
            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            string year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Month.ToString();


            string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "' and  d.DeviceId='" + deviceid + "' order by LogDate desc";
         //   string sql = "select distinct top 1 finger_id,r.roll_no,r.app_no from Registration r where r.finger_id ='9012'";




            DataSet printds_new = da.select_method_wo_parameter(sql, "Text");
            if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
      {
                for (int i = 0; i < printds_new.Tables[0].Rows.Count; i++)
             {
               string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
                   string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["finger_id"]).Trim();

                    //string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
 //string inoutchk = "0";
           // string finid = "101";
                    if (inoutchk == "" || inoutchk == "0")
                    {
                        string getroll = Convert.ToString(printds_new.Tables[0].Rows[0]["roll_no"]).Trim();
                        txt_rollno.Text = getroll;
                        roll.Value = getroll;
                        roollstu = getroll;
                        string getroll1 = Convert.ToString(printds_new.Tables[0].Rows[0]["app_no"]).Trim();
                        Label2.Text = getroll1;
                        //string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'";
                        //int query = d2.update_method_wo_parameter(sql2, "TEXT");




                    }
                    else
                    {
                        Label2.Text = "";

                        txt_rollno.Text = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "studentclear()", "studentclear()", true);

                    }
            }
           }

        }
        catch
        {
        }
    }
    public void btn_Question_Bank_popup_Click(object sender, EventArgs e)
    {
        string j = string.Empty;
        sec = 0;
        j = lblUserCode.Text;

        string date = DateTime.Now.ToString("MM/dd/yyyy");
        string saql = string.Empty;
        if (Label2.Text != "")
            saql = "update GateEntryExit set stu_Relationship='2' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No=" + Label2.Text + " order by GateEntryExitID desc)";
        int ud = d2.update_method_wo_parameter(saql, "TEXT");
        string rol=d2.GetFunction("select roll_no FROM registration where App_No= " + Label2.Text + "");
        txt_rollno.Text=rol;

        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        string year = DateTime.Now.Year.ToString();
        string Month = DateTime.Now.Month.ToString();


        string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  and  d.DeviceId='" + deviceid + "' order by LogDate desc";



        DataSet printds_new = da.select_method_wo_parameter(sql, "Text");
        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < printds_new.Tables[0].Rows.Count; i++)
            {
                string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
                string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["finger_id"]).Trim();

                string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
                string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'  and  DeviceId='" + deviceid + "' ";
                int query = d2.update_method_wo_parameter(sql2, "TEXT");
            }
        }

        //ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
        txt_rollno_OnTextChanged(sender, e);



    }

    public void btn_Question_Bank_popup1_Click(object sender, EventArgs e)
    {
        string j = string.Empty;
        sec = 0;
        j = lblUserCode.Text;

        string date = DateTime.Now.ToString("MM/dd/yyyy");
        string saql = string.Empty;
        if (Label2.Text != "")
            saql = "update GateEntryExit set stu_Relationship='3' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No=" + Label2.Text + " order by GateEntryExitID desc)";
        int ud = d2.update_method_wo_parameter(saql, "TEXT");
        string rol = d2.GetFunction("select roll_no FROM registration where App_No= " + Label2.Text + "");
        txt_rollno.Text = rol;

        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        string year = DateTime.Now.Year.ToString();
        string Month = DateTime.Now.Month.ToString();


        string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  and  d.DeviceId='" + deviceid + "'  order by LogDate desc";



        DataSet printds_new = da.select_method_wo_parameter(sql, "Text");
        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < printds_new.Tables[0].Rows.Count; i++)
            {
                string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
                string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["finger_id"]).Trim();

                string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
                string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'  and DeviceId='" + deviceid + "'";
                int query = d2.update_method_wo_parameter(sql2, "TEXT");
            }
        }
      //  ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
        txt_rollno_OnTextChanged(sender, e);
    }

    public void btn_Question_Bank_popup3_Click(object sender, EventArgs e)
    {
        string j = string.Empty;
        sec = 0;
        j = lblUserCode.Text;

        string date = DateTime.Now.ToString("MM/dd/yyyy");
        string saql = string.Empty;
        if (Label2.Text != "")
            saql = "update GateEntryExit set stu_Relationship='1' where  GatePassDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and  GatepassExitdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and App_No= " + Label2.Text + " and GateEntryExitID=(select top 1 GateEntryExitID from GateEntryExit where App_No=" + Label2.Text + " order by GateEntryExitID desc)";
        int ud = d2.update_method_wo_parameter(saql, "TEXT");

        string rol = d2.GetFunction("select roll_no FROM registration where App_No= " + Label2.Text + "");
        txt_rollno.Text = rol;
        string Date = DateTime.Now.ToString("dd/MM/yyyy");
        string year = DateTime.Now.Year.ToString();
        string Month = DateTime.Now.Month.ToString();


        string sql = "select distinct top 1 finger_id,LogDate,r.roll_no,d.inout,r.app_no from app91..DeviceLogs_" + Month + "_" + year + " d,Registration r where r.finger_id = d.userid and CONVERT(varchar(20),d.LogDate,103)='" + Date + "'  and  d.DeviceId='" + deviceid + "'  order by LogDate desc";



        DataSet printds_new = da.select_method_wo_parameter(sql, "Text");
        if (printds_new.Tables.Count > 0 && printds_new.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < printds_new.Tables[0].Rows.Count; i++)
            {
                string inoutchk = Convert.ToString(printds_new.Tables[0].Rows[0]["inout"]).Trim();
                string finid = Convert.ToString(printds_new.Tables[0].Rows[0]["finger_id"]).Trim();

                string logdate = Convert.ToString(printds_new.Tables[0].Rows[0]["LogDate"]).Trim();
                string sql2 = "update app91..DeviceLogs_" + Month + "_" + year + " set inout='1'  where userid = '" + finid + "' and LogDate='" + logdate + "'  and  DeviceId='" + deviceid + "'";
                int query = d2.update_method_wo_parameter(sql2, "TEXT");
            }
        }

      //  ScriptManager.RegisterStartupScript(this, GetType(), "getsmartrollno", "getsmartrollno(" + txt_rollno.Text + ");", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
        txt_rollno_OnTextChanged(sender, e);
    }

      public void btn_visitorok1_Click(object sender, EventArgs e)
    {
        try
        {
  txt_mno_Changed(sender, e);
            //if (rb_visitin.Checked == true)
            //{
            //    print();
            //}
        }
        catch
        {
        }
    }

      public void Btnexit_Click(object sender, EventArgs e)
      {
          try
          {
              Div1.Visible = true;
              Label4.Visible = false;
              ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
          }
          catch
          {
          }
      }

      public void Bon2_Click(object sender, EventArgs e)
      {
          try
          {
              Div1.Visible = false;
              Label4.Visible = false;
              ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
          }
          catch
          {
          }
      }


      public void Btnn2_Click(object sender, EventArgs e)
      {
          try
          {
              if (TextBox3.Text == "1947")
              {
                  Label4.Visible = false;
                  Response.Redirect("~/RequestMOD/RequestHome.aspx");
              }
              else
              {
                  Label4.Visible = true;
                  ScriptManager.RegisterStartupScript(this, GetType(), "checkmain", "checkmain();", true);
              }
             
          }
          catch
          {
          }
      }

}


/*
 * 09.11.16 smartcard and roll no settings based run 6.00
 
 */

