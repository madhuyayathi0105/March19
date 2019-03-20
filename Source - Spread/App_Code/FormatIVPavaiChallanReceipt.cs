using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Gios.Pdf;
using System.Drawing;
using System.Text;
using System.Collections;
public class FormatIVPavaiChallanReceipt
{
    DAccess2 d2 = new DAccess2();
    public FormatIVPavaiChallanReceipt()
    {

    }
    private double retBalance(string appNo, byte BalanceType)
    {
        double ovBalAMt = 0;
        if (BalanceType == 1)
        {
            double.TryParse(d2.GetFunction(" select sum(isnull(totalAmount,0)-isnull(paidAmount,0)) as BalanceAmt from ft_feeallot where app_no =" + appNo + ""), out ovBalAMt);
        }
        return ovBalAMt;
    }
    protected string getMonth(string monthcode)
    {
        string Month = string.Empty;
        try
        {
            switch (monthcode)
            {
                case "1":
                    Month = "JAN";
                    break;
                case "2":
                    Month = "FEB";
                    break;
                case "3":
                    Month = "MAR";
                    break;
                case "4":
                    Month = "APR";
                    break;
                case "5":
                    Month = "MAY";
                    break;
                case "6":
                    Month = "JUN";
                    break;
                case "7":
                    Month = "JUL";
                    break;
                case "8":
                    Month = "AUG";
                    break;
                case "9":
                    Month = "SEP";
                    break;
                case "10":
                    Month = "OCT";
                    break;
                case "11":
                    Month = "NOV";
                    break;
                case "12":
                    Month = "DEC";
                    break;
                default:
                    Month = "-";
                    break;

            }
        }
        catch { }
        return Month;
    }
    //Original Receipt
    public string generateOriginal(string txt_rcptno, string txt_date, string txt_dept, CheckBox rb_cash, CheckBox rb_cheque, CheckBox rb_dd, CheckBox rb_card, CheckBox rb_NEFT, CheckBox rb_Challan, string collegecode1, string usercode, ref string lastRecptNo, ref string accidRecpt, RadioButtonList rbl_rollnoNew, DropDownList rbl_rollno, string appnoNew, string outRoll, TextBox txtDept_staff, string rollno, string app_formno, string Regno, string studname, GridView grid_Details, byte BalanceType, DataTable dtMulBnkDetails, CheckBox chk_rcptMulmode, string modeMulti, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, ref bool contentVisible, ref bool CreateReceiptOK, ref bool imgDIVVisible, ref Label lbl_alert, CheckBox cb_CautionDep, CheckBox cb_govt, CheckBox cb_exfees, string mode, TextBox txt_ddno, DropDownList ddl_bkname, TextBox txt_chqno, DataSet dsPri, string Roll_admit, string section, string batch_year)
    {
        //Pavai College and School
        string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormatsheet' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
        int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

        string schoolOrCollege = d2.GetFunction("select top 1 value from Master_Settings where settings='schoolorcollege' and  usercode='" + usercode + "'").Trim();
        CreateReceiptOK = false;
        contentVisible = false;
        imgDIVVisible = false;
        lastRecptNo = string.Empty;
        accidRecpt = string.Empty;
        StringBuilder contentDiv = new StringBuilder();
        // string regno = string.Empty;
        //added by abarna for Exclude Copies
        string excludecopy = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ExcludeCopys' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
        string collectedby = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='cb_collectedby' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
        //Basic Data
        //string rollno = txt_rollno.Text.Trim();
        string recptNo = txt_rcptno.Trim();
        string recptDt = txt_date.Trim();
        //string studname = txt_name.Text.Trim();
        string course = txt_dept.Trim();
        string batchYrSem = string.Empty;
        string acaYear = string.Empty;
        string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();

        acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");


        try
        {
            acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
        }
        catch { }

        try
        {
            course = course.Split('-')[0];
        }
        catch { course = ""; }
        // string mode = string.Empty;
        if (modeMulti == string.Empty)
        {
            if (rb_cash.Checked)
            {
                mode = "Cash";
            }
            else if (rb_cheque.Checked)
            {
                mode = "Cheque";
                //mode = "Cheque - No:" + checkDDno;
            }
            else if (rb_dd.Checked)
            {
                mode = "DD";
                //mode = "DD - No:" + checkDDno;
            }
            else if (rb_card.Checked)
            {
                mode = "Card";
                //mode = "Card - " + newbankname;
            }
            else if (rb_NEFT.Checked)
            {
                mode = "NEFT";
                //mode = "Card - " + newbankname;
            }
        }
        else
        {
            mode = modeMulti;
        }

        //Fields to print
        string queryPrint1 = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
        DataSet ds = new DataSet();
        DataSet dsExcess = new DataSet();
        bool boolEx = false;
        ds = d2.select_method_wo_parameter(queryPrint1, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                //Footer Div Values
                byte narration = Convert.ToByte(ds.Tables[0].Rows[0]["IsNarration"]);
                byte commonclg = Convert.ToByte(ds.Tables[0].Rows[0]["isCollegeCom_name"]);
                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);
                byte ColName = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                byte address1 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd1"]);
                byte address2 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd2"]);
                byte address3 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd3"]);
                byte rightLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsRightLogo"]);
                byte leftLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsLeftLogo"]);
                byte university = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeUniversity"]);
                byte district = Convert.ToByte(ds.Tables[0].Rows[0]["iscollegedist"]);
                byte state = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeState"]);
                byte mobile = Convert.ToByte(ds.Tables[0].Rows[0]["isMobile"]);
                byte Email = Convert.ToByte(ds.Tables[0].Rows[0]["isEmail"]);
                byte Web = Convert.ToByte(ds.Tables[0].Rows[0]["isWebsite"]);
                byte hostel = Convert.ToByte(ds.Tables[0].Rows[0]["ishostelname"]);
                string regno = string.Empty;
                //Document Settings

                StringBuilder sbHtml = new StringBuilder();
                StringBuilder sbHtmlCopy = new StringBuilder();
                int officeCopyHeight = 0;
                string colquery = "select com_name,collname,university,address1 ,address2,address3+' - '+pincode as address3,district,state,phoneno,email,website from collinfo where college_code=" + collegecode1 + " ";
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
                    {
                        colquery += " select r.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c,registration r where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + " and r.app_no=a.app_no";
                    }
                    else
                    {
                        colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
                    }
                    string selEx = " select exl.headerfk,headername,ledgername,exl.ledgerfk,exl.excessamt,exl.adjamt,exl.balanceamt from ft_excessdet ex,ft_excessledgerdet exl,fm_headermaster h,fm_ledgermaster l where ex.excessdetpk=exl.excessdetfk  and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk and ex.app_no='" + appnoNew + "' and dailytranscode='" + recptNo + "'";
                    dsExcess = d2.select_method_wo_parameter(selEx, "Text");
                    if (dsExcess.Tables.Count > 0 && dsExcess.Tables[0].Rows.Count > 0)
                    {
                        boolEx = true;
                    }
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    colquery += "  select appl_id ,h.dept_name,h.dept_acronym,h.dept_code,s.staff_name,s.staff_code,a.father_name,t.stftype as staff_type  from staffmaster s,staff_appl_master a,hrdept_master h,stafftrans t,desig_master d where s.appl_no =a.appl_no and s.staff_code =t.staff_code and t.dept_code =h.dept_code and d.desig_code =t.desig_code and s.college_code =h.college_code and d.collegeCode =s.college_code and latestrec ='1' and appl_id ='" + appnoNew + "' and s.college_Code=" + collegecode1 + "  ";
                }
                else if (rbl_rollnoNew.SelectedIndex == 2)
                {
                    colquery += " SELECT VendorContactPK, VenContactType, VenContactName, VenContactDesig, VenContactDept, VendorPhoneNo, VendorExtNo, VendorMobileNo, VendorEmail, VendorFK FROM      IM_VendorContactMaster WHERE VendorContactPK = '" + appnoNew + "' ";
                }
                else if (rbl_rollnoNew.SelectedIndex == 3)
                {
                    colquery += " SELECT VendorCode,vendorname,VendorMobileNo,VendorAddress,VendorCity,VendorCompName,VendorType  from co_vendormaster  WHERE VendorPK = '" + appnoNew + "' ";
                    outRoll = string.Empty;
                }

                colquery += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode ,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + appnoNew + "'";
                regno = d2.GetFunction("select Reg_no from registration where App_No ='" + appnoNew + "'");
                Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                DataView dv = new DataView();
                string collegename = "";
                string commname = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string univ = "";
                string deg = "";
                string cursem = "";
                string batyr = "";
                string seatty = "";
                string board = "";
                string mothe = "";
                string fathe = "";
                string dist = "";
                string stat = "";
                string mob = "";
                string mail = "";
                string website = "";
                string MemType = string.Empty;
                string classdisplay = "Class Name ";
                string TermorSem = string.Empty;
                if (schoolOrCollege == "0")
                {
                    classdisplay = "Class Name ";
                    TermorSem = "Term";
                }
                else
                {
                    classdisplay = "Dept Name ";
                    TermorSem = "Semester";
                }
                double deductionamt = 0;
                ds.Clear();
                ds = d2.select_method_wo_parameter(colquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        commname = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                        dist = Convert.ToString(ds.Tables[0].Rows[0]["district"]);
                        stat = Convert.ToString(ds.Tables[0].Rows[0]["state"]);
                        mob = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        mail = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (rbl_rollnoNew.SelectedIndex == 0)
                        {
                            //if (degACR == 0)
                            //{
                            // deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                            //}
                            //else
                            //{
                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                            //}
                            cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                            batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                            seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                            board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                            mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                            fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                            //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                            if (schoolOrCollege == "0")
                            {
                                MemType = "Admission No";
                            }
                            else
                            {
                                MemType = rbl_rollno.SelectedItem.Text.Trim();
                                if (Convert.ToInt32(rbl_rollno.SelectedValue) == 0)
                                {
                                    Roll_admit = rollno;
                                }
                                else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 1)
                                {
                                    Roll_admit = Regno;
                                }
                                else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 2)
                                {
                                    //Roll_admit = Roll_admit;
                                }
                                else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 3)
                                {
                                    Roll_admit = app_formno;
                                }
                            }
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 1)
                        {
                            //if (degACR == 0)
                            //{
                            // deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_name"]);
                            //}
                            //else
                            //{
                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                            //}
                            //cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                            //batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                            seatty = Convert.ToString(ds.Tables[1].Rows[0]["staff_type"]);
                            //board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                            //mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                            fathe = Convert.ToString(ds.Tables[1].Rows[0]["father_name"]);
                            //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                            MemType = "Staff Code";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 2)
                        {
                            deg = " - ";
                            MemType = "Vendor Code";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 3)
                        {
                            deg = " - ";
                            MemType = "Others";
                        }
                    }

                    if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        course = txtDept_staff.Text.Trim();
                    }
                }
                if (save1 == 1)
                {
                    #region Receipt Header
                    #region Receipt Header

                    //Header Images
                    //Line1
                    string Hllogo = string.Empty;
                    if (leftLogo != 0)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                        {
                            Hllogo = "<img src='" + "../FinanceLogo/Left_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                        }
                    }
                    string Hcol = string.Empty;
                    if (commonclg != 0)
                    {
                        Hcol = commname;
                    }
                    string Hrlogo = string.Empty;
                    if (rightLogo != 0)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                        {
                            Hrlogo = "<img src='" + "../FinanceLogo/Right_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                        }
                    }
                    //Hrlogo = "<div style='height:80px; width:100px; border:1px solid black;'><div style='margin-top:30px;font-size:20px;'><b>" + Regex.Replace(recptNo, @"[\d-]", string.Empty).ToUpper() + "</b></div></div>";
                    //Line2
                    string Huniv = string.Empty;
                    if (university != 0)
                    {
                        Huniv = univ;
                    }
                    //Line3
                    string Hadd1add2 = string.Empty;
                    if (address1 != 0 || address2 != 0)
                    {
                        if (address2 != 0)
                        {
                            add1 += " " + add2;
                        }
                        Hadd1add2 = add1;
                    }
                    //Line4
                    string Hadd3 = string.Empty;
                    if (address3 != 0)
                    {
                        //Hadd3 = add3;
                        Hadd1add2 = Hadd1add2.TrimEnd('.', ',') + "," + add3;
                    }

                    //sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:center; width: 585px; ' class='classBold10'><tr><td rowspan='5'>" + Hllogo + "</td><td style='text-align:center; font-weight:bold; font-size:14px;'>" + Hcol + "</td><td  rowspan='5'>" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + "</td></tr><tr><td style='text-align:center;'>" + Hadd3 + "</td></tr><tr><td style='text-align:center; font-weight:bold; font-size:14px;'><u>RECEIPT</u></td></tr></table></center>");
                    // sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:center; width: 585px; ' class='classBold10'><tr><td rowspan='5'>" + Hllogo + "</td><td style='text-align:center; font-weight:bold; font-size:14px;'>" + Hcol + "</td><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + "</td></tr><tr><td style='text-align:center; font-weight:bold; font-size:14px;'><u>RECEIPT</u></td></tr></table></center>");

                    #endregion
                    string degString = string.Empty;
                    //Line3
                    if (rbl_rollnoNew.SelectedIndex == 0)
                    {
                        degString = deg;//.Split('-')[0].ToUpper();
                    }
                    else if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        degString = deg;
                    }
                    string deptstring = string.Empty;
                    if (rbl_rollnoNew.SelectedIndex == 0)
                    {
                        deptstring = deg.Split('-')[1].ToUpper();
                    }
                    else if (rbl_rollnoNew.SelectedIndex == 1)
                    {

                    }

                    string[] className = degString.Split('-');
                    if (className.Length > 1)
                    {
                        //degString = className[1];
                    }
                    string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");

                    sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                    sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                    //  sbHtmlCopy.Append("<div style=' width:790px; height:450px;'></div>");
                    sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                    if (commonclg == 1)
                    {
                        sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='width: 785px; height:95px '><tr><td rowspan='5'>" + Hllogo + "</td><center><td style='text-align:center; font-weight:bold; font-size:28px;'>" + Hcol + "</td></center><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + dist + "," + stat + "</td></tr><tr><td style='text-align:center;'>Mobile No:" + mob + ",Email:" + mail + ",Website:" + website + "</td></tr></table></center>");
                        sbHtmlCopy.Append("<center><table cellpadding='0' cellspacing='0' style='width: 785px; height:95px '><tr><td rowspan='5'>" + Hllogo + "</td><center><td style='text-align:center; font-weight:bold; font-size:28px;'>" + Hcol + "</td></center><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + dist + "," + stat + "</td></tr><tr><td style='text-align:center;'>Mobile No:" + mob + ",Email:" + mail + ",Website:" + website + "</td></tr></table></center>");
                    }
                    if (ColName == 1)
                    {
                        sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                        sbHtml.Append("<br/>");

                        sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                        sbHtmlCopy.Append("<br/>");
                    }

                    if (acaYear != "0")
                    {
                        sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; '><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:100px; '>Academic Year</td><td style='width:120px; ' >: " + acaYear + "</td><td style='width:160px;  '>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");//<td style='width:80px; '>Collected By </td><td style='width:240px; '  >: " + userName + "</td>
                        if (hostel == 1)
                        {
                            string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                            if (studtype.ToLower() == "hostler")
                            {
                                string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                                if (hostelpk != "" && hostelpk != "0")
                                {
                                    string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                                    sbHtml.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name</td><td style='width:160px;' >: " + Hostelname + "</td>");
                                }
                            }

                        }
                        sbHtml.Append("</tr></table>");

                        sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:100px; '>Academic Year </td><td style='width:120px; ' >: " + acaYear + "</td><td style='width:160px;  '>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");//<td style='width:80px; '>Collected By </td><td style='width:240px; ' >: " + userName + "</td>
                        if (hostel == 1)
                        {
                            string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                            if (studtype.ToLower() == "hostler")
                            {
                                string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                                if (hostelpk != "" && hostelpk != "0")
                                {
                                    string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                                    sbHtmlCopy.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name </td><td style='width:160px;' >: " + Hostelname  + "</td>");
                                }
                            }

                        }
                        sbHtmlCopy.Append("</tr></table>");
                    }
                    else
                    {
                        sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:240px; ' >: " + userName + "</td><td style='width:100px; '>Reg No </td><td style='width:120px; ' >: " + regno + "</td><td style='width:160px; text-align:right;'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");
                        if (hostel == 1)
                        {
                            string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                            if (studtype.ToLower() == "hostler")
                            {
                                string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                                if (hostelpk != "" && hostelpk != "0")
                                {
                                    string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                                    sbHtml.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name </td><td style='width:160px;' >: " + Hostelname + "</td>");
                                }
                            }

                        }
                        sbHtml.Append("</tr></table>");

                        sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:240px; '>: " + userName + "</td><td style='width:100px; '>Reg No </td><td style='width:120px; ' >: " + regno + "</td><td style='width:100px; text-align:right; '>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");
                        if (hostel == 1)
                        {
                            string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                            if (studtype.ToLower() == "hostler")
                            {
                                string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                                if (hostelpk != "" && hostelpk != "0")
                                {
                                    string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                                    sbHtmlCopy.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name</td><td style='width:160px;' >: " + Hostelname + "</td>");
                                }
                            }

                        }
                        sbHtmlCopy.Append("</tr></table>");
                    }

                    #endregion

                    #region Receipt Body

                    int rows = 0;
                    foreach (GridViewRow row in grid_Details.Rows)
                    {
                        CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                        if (!chkOkPay.Checked)
                            continue;
                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                        double creditamt = 0;

                        if (txtTobePaidamt.Text != "")
                        {
                            creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                            TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                            double exgridamt = 0;
                            if (cb_exfees.Checked)
                            {
                                double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                            }
                            creditamt += exgridamt;
                            TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                            double gvtamt = 0;
                            if (cb_govt.Checked)
                            {
                                double.TryParse(txtScholAmt.Text, out gvtamt);
                            }
                            creditamt += gvtamt;
                            TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                            double curCautamt = 0;
                            if (cb_CautionDep.Checked)
                            {
                                double.TryParse(txtCautAmt.Text, out curCautamt);
                            }
                            creditamt += curCautamt;
                            if (creditamt > 0)
                            {
                                rows++;
                            }
                        }
                    }

                    sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                    sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                    Hashtable htHdrAmt = new Hashtable();
                    Hashtable htHdrName = new Hashtable();

                    int sno = 0;
                    int indx = 0;
                    double totalamt = 0;
                    double balanamt = 0;
                    double curpaid = 0;
                    int ledgCnt = 0;
                    foreach (GridViewRow row in grid_Details.Rows)
                    {
                        CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                        if (!chkOkPay.Checked)
                            continue;

                        TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                        TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                        TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                        TextBox txtdeductamt = (TextBox)row.FindControl("txt_deduct_amt");

                        Label lblFeeCategory = (Label)row.FindControl("lbl_feetype");
                        Label lblsem = (Label)row.FindControl("lbl_textval");
                        Label lblfeecat = (Label)row.FindControl("lbl_textCode");

                        double creditamt = 0;

                        if (txtTobePaidamt.Text != "")
                        {
                            creditamt = Convert.ToDouble
    (txtTobePaidamt.Text);
                            TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                            double exgridamt = 0;
                            if (cb_exfees.Checked)
                            {
                                double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                            }
                            creditamt += exgridamt;
                            TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                            double gvtamt = 0;
                            if (cb_govt.Checked)
                            {
                                double.TryParse(txtScholAmt.Text, out gvtamt);
                            }
                            creditamt += gvtamt;
                            TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                            double curCautamt = 0;
                            if (cb_CautionDep.Checked)
                            {
                                double.TryParse(txtCautAmt.Text, out curCautamt);
                            }
                            creditamt += curCautamt;
                        }

                        if (creditamt > 0)
                        {
                            sno++;
                            ledgCnt++;

                            totalamt += Convert.ToDouble(txtTotalamt.Text);
                            balanamt += Convert.ToDouble(txtBalamt.Text);
                            curpaid += creditamt;
                            //balanamt += Convert.ToDouble(txtTotalamt.Text) + Convert.ToDouble(txtTobePaidamt.Text) - creditamt;
                            deductionamt += Convert.ToDouble(txtdeductamt.Text);
                            indx++;

                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + lblFeeCategory.Text + "-" + "(" + lblsem.Text + ")" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + lblFeeCategory.Text + "-" + "(" + lblsem.Text + ")" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                        }
                    }
                    if (BalanceType == 1)
                    {
                        balanamt = retBalance(appnoNew, BalanceType);
                    }
                    if (boolEx)
                    {
                        for (int row = 0; row < dsExcess.Tables[0].Rows.Count; row++)
                        {
                            double creditamt = 0;
                            double.TryParse(Convert.ToString(dsExcess.Tables[0].Rows[row]["excessamt"]), out creditamt);
                            if (creditamt > 0)
                            {
                                sno++;
                                string hdName = Convert.ToString(dsExcess.Tables[0].Rows[row]["ledgername"]) + "(Advance)";
                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + hdName + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + hdName + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                curpaid += creditamt;
                            }
                        }
                    }


                    #region DDNarration
                    string ddnar = string.Empty;
                    string payModeStr = string.Empty;
                    //double modeht = 40;
                    if (narration != 0)
                    {
                        if (chk_rcptMulmode.Checked)
                        {
                            mode = modeMulti;
                            for (int z = 0; z < dtMulBnkDetails.Rows.Count; z++)
                            {
                                //  ddnar += "\n" + (z + 1).ToString() + ")No : " + dtMulBnkDetails.Rows[z][1] + " Bank : " + dtMulBnkDetails.Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Rows[z][2] + " Date  : " + dtMulBnkDetails.Rows[z][3] + " Amount : " + dtMulBnkDetails.Rows[z][4] + "/-";
                                ddnar += "\n" + dtMulBnkDetails.Rows[z][5] + " No : " + dtMulBnkDetails.Rows[z][1] + " Bank : " + dtMulBnkDetails.Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Rows[z][2] + " Date  : " + dtMulBnkDetails.Rows[z][3] + " Amount : " + dtMulBnkDetails.Rows[z][4] + "/-";
                            }
                            //modeht = dtMulBnkDetails.Rows.Count * 15;
                            //modeht += 20;
                        }
                        else
                        {
                            if (!rb_cash.Checked)
                            {
                                if (rb_dd.Checked == true)
                                {
                                    ddnar = ddnar + "\nDDNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                    //mode = "DD - No:" + checkDDno;
                                    payModeStr = "-(subject to realiation)";
                                }
                                else if (rb_cheque.Checked)
                                {
                                    ddnar = ddnar + "\nChequeNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                    // mode = "Cheque - No:" + checkDDno;
                                    payModeStr = "-(subject to realiation)";
                                }
                                else if (rb_Challan.Checked == true)//added by abarna 11.06.2018
                                {
                                    if (checkDDno != "")
                                    {
                                        ddnar = "\nChallanNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                        //ddnew = "\nChallanNo : " + checkDDno + "\nBank : " + newbankname + "\nBranch :" + branch + "\nDate  : " + txt_date1.Text.ToString();
                                    }
                                    else//modified 24.05.2018
                                    {
                                        ddnar = "\nBank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                        //ddnew = "\nBank : " + newbankname + "\nBranch :" + branch + "\nDate  : " + txt_date1.Text.ToString();
                                    }
                                }
                                else if (rb_NEFT.Checked)//ADDED BY ABARNA 11.06.2018
                                {
                                    ddnar = "\nNeft No:" + checkDDno + "";
                                }
                                else if (rb_card.Checked)
                                {
                                    ddnar = "\nCard : " + newbankname;
                                }
                            }
                        }
                        ddnar += "\n" + txt_remark.Text.Trim();
                        //modified by sudhagar 04.10.2017
                        //if (excessRemaining(appnoNew) > 0)
                        //    ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(appnoNew);
                    }
                    #endregion

                    //mode added by sudhagar
                    double totalamount = curpaid;
                    double showDate = 0;
                    string showDateStr = string.Empty;

                    double.TryParse(Convert.ToString(d2.GetFunction("select linkvalue from New_InsSettings where LinkName='showdatetime' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'")), out showDate);
                    if (showDate == 1)
                        showDateStr = "" + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToShortTimeString()) + "<br/>fees once paid wiil not be refunded" + payModeStr + "";

                    if (excludecopy == "0")
                    {
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                        sbHtml.Append("</table></div><br>");

                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                    }
                    else
                    {
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:500px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                        sbHtml.Append("</table></div><br>");

                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:500px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                    }

                    if (ledgCnt == 1)
                        officeCopyHeight = 290; //270;
                    else if (ledgCnt == 2)
                        officeCopyHeight = 260; //240;
                    else if (ledgCnt == 3)
                        officeCopyHeight = 230;//210;
                    else if (ledgCnt == 4)
                        officeCopyHeight = 200;//180;
                    else if (ledgCnt == 5)
                        officeCopyHeight = 165;// 150;
                    sbHtmlCopy.Append("</table></div><br>");
                    sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());

                    #endregion
                }
                else
                {
                    #region Receipt Header
                    string heightsize = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormatSheetTextboxValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                    string degString = string.Empty;
                    //Line3
                    if (rbl_rollnoNew.SelectedIndex == 0)
                    {
                        degString = deg;//.Split('-')[0].ToUpper();
                    }
                    else if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        degString = deg;
                    }
                    string deptstring = string.Empty;
                    if (rbl_rollnoNew.SelectedIndex == 0)
                    {
                        deptstring = deg.Split('-')[1].ToUpper();
                    }
                    else if (rbl_rollnoNew.SelectedIndex == 1)
                    {

                    }

                    string[] className = degString.Split('-');
                    if (className.Length > 1)
                    {
                        //degString = className[1];
                    }
                    string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                    //sbHtml.Append("<div style='max-height:300px;height:300px;width:795px; border:0px solid black; margin-left:5px;page-break-after: always;'><table cellpadding='0' 
                    sbHtml.Append("<div style='height:560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                    sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                    //  sbHtmlCopy.Append("<div style=' width:790px; height:450px;'></div>");
                    sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                    if (ColName == 1)
                    {
                        sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                        sbHtml.Append("<br/>");

                        sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                        sbHtmlCopy.Append("<br/>");
                    }
                    if (acaYear != "0")
                    {

                        sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; font-size:" + heightsize + "px; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:120px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:179px; '>Academic Year</td><td style='width:200px; ' >: " + acaYear + "</td><td style='width:237px; text-align:right; '>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");//class='classBold10'<td style='width:120px; '>Collected By </td><td style='width:240px; '  >: " + userName + "</td> change


                        sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black;  font-size:" + heightsize + "px; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:120px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><td style='width:179px; '>Academic Year </td><td style='width:200px; ' >: " + acaYear + "</td><td style='width:237px; text-align:right; '>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");//<tr><td style='width:120px; '>Collected By </td><td style='width:240px; ' >: " + userName + "</td>

                    }
                    else
                    {
                        if (collectedby == "1")
                        {
                            sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black;  font-size:13px;' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");//<td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td>


                            sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black;  font-size:13px;' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");//<td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td>
                        }
                        else
                        {
                            sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; font-size:" + heightsize + "px; ' class='classBold10'><tr><td style='text-align:center; font-size:15px; '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");


                            sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black;  font-size:" + heightsize + "px; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                        }
                    }

                    #endregion

                    #region Receipt Body

                    int rows = 0;
                    foreach (GridViewRow row in grid_Details.Rows)
                    {
                        CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                        if (!chkOkPay.Checked)
                            continue;
                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                        double creditamt = 0;

                        if (txtTobePaidamt.Text != "")
                        {
                            creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                            TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                            double exgridamt = 0;
                            if (cb_exfees.Checked)
                            {
                                double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                            }
                            creditamt += exgridamt;
                            TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                            double gvtamt = 0;
                            if (cb_govt.Checked)
                            {
                                double.TryParse(txtScholAmt.Text, out gvtamt);
                            }
                            creditamt += gvtamt;
                            TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                            double curCautamt = 0;
                            if (cb_CautionDep.Checked)
                            {
                                double.TryParse(txtCautAmt.Text, out curCautamt);
                            }
                            creditamt += curCautamt;
                            if (creditamt > 0)
                            {
                                rows++;
                            }
                        }
                    }

                    sbHtml.Append("<div><table  style='width:785px; font-size:17px;  border:1px solid black; ' border='1' rules='rows'   cellpadding='5' class='classBold10'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");//change class

                    sbHtmlCopy.Append("<div><table  style='width:785px;  font-size:17px; border:1px solid black; ' border='1' rules='rows' cellpadding='5' class='classBold10'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");//change

                    Hashtable htHdrAmt = new Hashtable();
                    Hashtable htHdrName = new Hashtable();

                    int sno = 0;
                    int indx = 0;
                    double totalamt = 0;
                    double balanamt = 0;
                    double curpaid = 0;
                    int ledgCnt = 0;
                    foreach (GridViewRow row in grid_Details.Rows)
                    {
                        CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                        if (!chkOkPay.Checked)
                            continue;

                        TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                        TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                        TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                        TextBox txtdeductamt = (TextBox)row.FindControl("txt_deduct_amt");

                        Label lblFeeCategory = (Label)row.FindControl("lbl_feetype");
                        Label lblsem = (Label)row.FindControl("lbl_textval");
                        Label lblfeecat = (Label)row.FindControl("lbl_textCode");

                        double creditamt = 0;

                        if (txtTobePaidamt.Text != "")
                        {
                            creditamt = Convert.ToDouble
    (txtTobePaidamt.Text);
                            TextBox txtExcessGridAmt = (TextBox)row.FindControl("txt_gridexcess_amt");
                            double exgridamt = 0;
                            if (cb_exfees.Checked)
                            {
                                double.TryParse(txtExcessGridAmt.Text, out exgridamt);
                            }
                            creditamt += exgridamt;
                            TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                            double gvtamt = 0;
                            if (cb_govt.Checked)
                            {
                                double.TryParse(txtScholAmt.Text, out gvtamt);
                            }
                            creditamt += gvtamt;
                            TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");

                            double curCautamt = 0;
                            if (cb_CautionDep.Checked)
                            {
                                double.TryParse(txtCautAmt.Text, out curCautamt);
                            }
                            creditamt += curCautamt;
                        }

                        if (creditamt > 0)
                        {
                            sno++;
                            ledgCnt++;

                            totalamt += Convert.ToDouble(txtTotalamt.Text);
                            balanamt += Convert.ToDouble(txtBalamt.Text);
                            curpaid += creditamt;
                            //balanamt += Convert.ToDouble(txtTotalamt.Text) + Convert.ToDouble(txtTobePaidamt.Text) - creditamt;
                            deductionamt += Convert.ToDouble(txtdeductamt.Text);
                            indx++;

                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + lblFeeCategory.Text + "-" + "(" + lblsem.Text + ")" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + lblFeeCategory.Text + "-" + "(" + lblsem.Text + ")" + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                        }
                    }
                    if (BalanceType == 1)
                    {
                        balanamt = retBalance(appnoNew, BalanceType);
                    }
                    if (boolEx)
                    {
                        for (int row = 0; row < dsExcess.Tables[0].Rows.Count; row++)
                        {
                            double creditamt = 0;
                            double.TryParse(Convert.ToString(dsExcess.Tables[0].Rows[row]["excessamt"]), out creditamt);
                            if (creditamt > 0)
                            {
                                sno++;
                                string hdName = Convert.ToString(dsExcess.Tables[0].Rows[row]["ledgername"]) + "(Advance)";
                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + hdName + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + hdName + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                curpaid += creditamt;
                            }
                        }
                    }


                    #region DDNarration
                    string ddnar = string.Empty;
                    string payModeStr = string.Empty;
                    //double modeht = 40;
                    if (narration != 0)
                    {
                        if (chk_rcptMulmode.Checked)
                        {
                            mode = modeMulti;
                            for (int z = 0; z < dtMulBnkDetails.Rows.Count; z++)
                            {
                                //  ddnar += "\n" + (z + 1).ToString() + ")No : " + dtMulBnkDetails.Rows[z][1] + " Bank : " + dtMulBnkDetails.Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Rows[z][2] + " Date  : " + dtMulBnkDetails.Rows[z][3] + " Amount : " + dtMulBnkDetails.Rows[z][4] + "/-";
                                ddnar += "\n" + dtMulBnkDetails.Rows[z][5] + " No : " + dtMulBnkDetails.Rows[z][1] + " Bank : " + dtMulBnkDetails.Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Rows[z][2] + " Date  : " + dtMulBnkDetails.Rows[z][3] + " Amount : " + dtMulBnkDetails.Rows[z][4] + "/-";
                            }
                            //modeht = dtMulBnkDetails.Rows.Count * 15;
                            //modeht += 20;
                        }
                        else
                        {
                            if (!rb_cash.Checked)
                            {
                                if (rb_dd.Checked == true)
                                {
                                    ddnar = ddnar + "\nDDNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                    //mode = "DD - No:" + checkDDno;
                                    payModeStr = "-(subject to realiation)";
                                }
                                else if (rb_cheque.Checked)
                                {
                                    ddnar = ddnar + "\nChequeNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                    // mode = "Cheque - No:" + checkDDno;
                                    payModeStr = "-(subject to realiation)";
                                }
                                else if (rb_Challan.Checked == true)//added by abarna 11.06.2018
                                {
                                    if (checkDDno != "")
                                    {
                                        ddnar = "\nChallanNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                        //ddnew = "\nChallanNo : " + checkDDno + "\nBank : " + newbankname + "\nBranch :" + branch + "\nDate  : " + txt_date1.Text.ToString();
                                    }
                                    else//modified 24.05.2018
                                    {
                                        ddnar = "\nBank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                        //ddnew = "\nBank : " + newbankname + "\nBranch :" + branch + "\nDate  : " + txt_date1.Text.ToString();
                                    }
                                }
                                else if (rb_NEFT.Checked)//ADDED BY ABARNA 11.06.2018
                                {
                                    ddnar = "\nNeft No:" + checkDDno + "";
                                }
                                else if (rb_card.Checked)
                                {
                                    ddnar = "\nCard : " + newbankname;
                                }
                            }
                        }
                        ddnar += "\n" + txt_remark.Text.Trim();
                        //modified by sudhagar 04.10.2017
                        //if (excessRemaining(appnoNew) > 0)
                        //    ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(appnoNew);
                    }
                    #endregion

                    //mode added by sudhagar
                    double totalamount = curpaid;
                    double showDate = 0;
                    string showDateStr = string.Empty;

                    double.TryParse(Convert.ToString(d2.GetFunction("select linkvalue from New_InsSettings where LinkName='showdatetime' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'")), out showDate);
                    if (showDate == 1)
                        showDateStr = "" + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToShortTimeString()) + "<br/>fees once paid wiil not be refunded" + payModeStr + "";

                    if (excludecopy == "0")
                    {
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                        sbHtml.Append("</table></div><br>");

                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                    }
                    else
                    {
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:" + heightsize + "px; ' class='classBold10'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:17px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:" + heightsize + "px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:500px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");//change

                        sbHtml.Append("</table></div><br>");

                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:" + heightsize + "px; ' class='classBold10'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:17px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/>" + showDateStr + "<br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:" + heightsize + "px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:500px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                    }

                    if (ledgCnt == 1)
                        officeCopyHeight = 290; //270;
                    else if (ledgCnt == 2)
                        officeCopyHeight = 260; //240;
                    else if (ledgCnt == 3)
                        officeCopyHeight = 230;//210;
                    else if (ledgCnt == 4)
                        officeCopyHeight = 200;//180;
                    else if (ledgCnt == 5)
                        officeCopyHeight = 165;// 150;
                    sbHtmlCopy.Append("</table></div><br>");
                    sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());

                    #endregion
                }

                contentDiv.Append(sbHtml.ToString() + (studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");

                #region New Print
                //contentDiv.InnerHtml += sbHtml.ToString();
                CreateReceiptOK = true;
                return contentDiv.ToString();
                #endregion

            }

        }
        return string.Empty;
    }
    //Multiple Receipt
    public string generateMultiple(DataSet dsPri, string collegecode1, string appnoNew, string section, ref PdfDocument recptDoc, ref PdfPage rcptpage, string recptNo, string studname, string recptDt, string Regno, string rollno, string app_formno, RadioButton rb_cash, RadioButton rb_dd, RadioButton rb_cheque, RadioButton rb_card, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, string mode, RadioButtonList rbl_rollnoNew, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, Label lbltype, RadioButton rdo_receipt, RadioButton rdo_sngle, string PayMode, DateTime dtrcpt, string memtype, string receiptno, string dtchkdd, string newbankcode, string usercode, string finYearid, int rcptType, bool InsertUpdateOK, ref bool createPDFOK, byte BalanceType, ref double overallCashAmt, string course, TextBox txt_ddno, DropDownList ddl_bkname, TextBox txt_chqno, string Roll_admit, DropDownList rbl_rollno)
    {
        string schoolOrCollege = d2.GetFunction("select top 1 value from Master_Settings where settings='schoolorcollege' and  usercode='" + usercode + "'").Trim();
        StringBuilder contentDiv = new StringBuilder();
        //Pavai College and School
        #region Print Option For Receipt

        //Basic Data
        string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
        string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
        try
        {
            acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
        }
        catch { }
        try
        {
            course = course.Split('-')[0];
        }
        catch { course = ""; }
        // string mode = string.Empty;

        if (rb_cash.Checked)
        {
            mode = "Cash";
        }
        else if (rb_cheque.Checked)
        {
            mode = "Cheque";
            //mode = "Cheque - No:" + checkDDno;
        }
        else if (rb_dd.Checked)
        {
            mode = "DD";
            //mode = "DD - No:" + checkDDno;
        }
        else if (rb_card.Checked)
        {
            mode = "Card";
            //mode = "Card - "+newbankname;
        }

        //Fields to print
        string queryPrint1 = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(queryPrint1, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                //Footer Div Values
                byte narration = Convert.ToByte(ds.Tables[0].Rows[0]["IsNarration"]);
                byte commonclg = Convert.ToByte(ds.Tables[0].Rows[0]["isCollegeCom_name"]);
                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);
                byte ColName = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeName"]);
                byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                byte address1 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd1"]);
                byte address2 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd2"]);
                byte address3 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd3"]);
                byte rightLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsRightLogo"]);
                byte leftLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsLeftLogo"]);
                byte university = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeUniversity"]);
                byte district = Convert.ToByte(ds.Tables[0].Rows[0]["iscollegedist"]);
                byte state = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeState"]);
                byte mobile = Convert.ToByte(ds.Tables[0].Rows[0]["isMobile"]);
                byte Email = Convert.ToByte(ds.Tables[0].Rows[0]["isEmail"]);
                byte Web = Convert.ToByte(ds.Tables[0].Rows[0]["isWebsite"]);
                byte hostel = Convert.ToByte(ds.Tables[0].Rows[0]["ishostelname"]);
                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3,district,state,phoneno,email,website,com_name from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                string collegename = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string univ = "";
                string deg = "";
                string cursem = "";
                string batyr = "";
                string seatty = "";
                string board = "";
                string mothe = "";
                string fathe = "";
                string dist = "";
                string stat = "";
                string mob = "";
                string mail = "";
                string website = "";
                string commname = "";
                double deductionamt = 0;
                ds.Clear();
                ds = d2.select_method_wo_parameter(colquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                        commname = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                        dist = Convert.ToString(ds.Tables[0].Rows[0]["district"]);
                        stat = Convert.ToString(ds.Tables[0].Rows[0]["state"]);
                        mob = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        mail = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //if (degACR == 0)
                        //{
                        //deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                        //}
                        //else
                        //{
                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                        //}
                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                    }
                }
                string MemType = string.Empty;
                string classdisplay = "Class Name ";
                string TermorSem = "Semester";
                if (schoolOrCollege == "0")
                {
                    classdisplay = "Class Name ";
                    TermorSem = "Term";
                }
                else
                {
                    classdisplay = "Dept Name ";
                    TermorSem = "Semester";
                }

                if (schoolOrCollege == "0")
                {
                    MemType = "Admission No";
                }
                else
                {
                    MemType = rbl_rollnoNew.SelectedItem.Text.Trim();
                    if (Convert.ToInt32(rbl_rollno.SelectedValue) == 0)
                    {
                        Roll_admit = rollno;
                    }
                    else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 1)
                    {
                        Roll_admit = Regno;
                    }
                    else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 2)
                    {
                        //Roll_admit = Roll_admit;
                    }
                    else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 3)
                    {
                        Roll_admit = app_formno;
                    }
                }

                string degString = string.Empty;
                //Line3
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    degString = deg;//.Split('-')[0].ToUpper();
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    degString = deg;
                }
                string[] className = degString.Split('-');
                if (className.Length > 1)
                {
                    //degString = className[1];
                }
                int officeCopyHeight = 0;
                StringBuilder sbHtml = new StringBuilder();
                StringBuilder sbHtmlCopy = new StringBuilder();
                #region Receipt Header
                #region Receipt Header

                //Header Images
                //Line1
                string Hllogo = string.Empty;
                if (leftLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                    {
                        Hllogo = "<img src='" + "../FinanceLogo/Left_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                string Hcol = string.Empty;
                if (commonclg != 0)
                {
                    Hcol = commname;
                }
                string Hrlogo = string.Empty;
                if (rightLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                    {
                        Hrlogo = "<img src='" + "../FinanceLogo/Right_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                //Hrlogo = "<div style='height:80px; width:100px; border:1px solid black;'><div style='margin-top:30px;font-size:20px;'><b>" + Regex.Replace(recptNo, @"[\d-]", string.Empty).ToUpper() + "</b></div></div>";
                //Line2
                string Huniv = string.Empty;
                if (university != 0)
                {
                    Huniv = univ;
                }
                //Line3
                string Hadd1add2 = string.Empty;
                if (address1 != 0 || address2 != 0)
                {
                    if (address2 != 0)
                    {
                        add1 += " " + add2;
                    }
                    Hadd1add2 = add1;
                }
                //Line4
                string Hadd3 = string.Empty;
                if (address3 != 0)
                {
                    //Hadd3 = add3;
                    Hadd1add2 = Hadd1add2.TrimEnd('.', ',') + "," + add3;
                }

                //sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:center; width: 585px; ' class='classBold10'><tr><td rowspan='5'>" + Hllogo + "</td><td style='text-align:center; font-weight:bold; font-size:14px;'>" + Hcol + "</td><td  rowspan='5'>" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + "</td></tr><tr><td style='text-align:center;'>" + Hadd3 + "</td></tr><tr><td style='text-align:center; font-weight:bold; font-size:14px;'><u>RECEIPT</u></td></tr></table></center>");
                // sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='text-align:center; width: 585px; ' class='classBold10'><tr><td rowspan='5'>" + Hllogo + "</td><td style='text-align:center; font-weight:bold; font-size:14px;'>" + Hcol + "</td><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + "</td></tr><tr><td style='text-align:center; font-weight:bold; font-size:14px;'><u>RECEIPT</u></td></tr></table></center>");

                #endregion
                string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");

                // sbHtml.Append("<div style='height: 560px;width:795px; border:1px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;page-break-after: always;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                // sbHtml.Append("<div style=' width:790px; height:100px;'></div>");
                sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                //sbHtmlCopy.Append("<div style=' width:790px; height:450px;'></div>");
                sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                if (commonclg == 1)
                {
                    sbHtml.Append("<center><table cellpadding='0' cellspacing='0' style='width: 785px; height:95px '><tr><td rowspan='5'>" + Hllogo + "</td><center><td style='text-align:center; font-weight:bold; font-size:28px;'>" + Hcol + "</td></center><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + dist + "," + stat + "</td></tr><tr><td style='text-align:center;'>Mobile No:" + mob + ",Email:" + mail + ",Website:" + website + "</td></tr></table></center>");
                    sbHtmlCopy.Append("<center><table cellpadding='0' cellspacing='0' style='width: 785px; height:95px '><tr><td rowspan='5'>" + Hllogo + "</td><center><td style='text-align:center; font-weight:bold; font-size:28px;'>" + Hcol + "</td></center><td  rowspan='5' >" + Hrlogo + "</td></tr><tr><td  style='text-align:center;'>" + Huniv + "</td></tr><tr><td  style='text-align:center;'>" + Hadd1add2 + Hadd3 + dist + "," + stat + "</td></tr><tr><td style='text-align:center;'>Mobile No:" + mob + ",Email:" + mail + ",Website:" + website + "</td></tr></table></center>");
                }
                if (ColName == 1)
                {
                    //sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                    //sbHtml.Append("<br/>");

                    //sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                    //sbHtmlCopy.Append("<br/>");
                    sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                    sbHtml.Append("<br/>");

                    sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                    sbHtmlCopy.Append("<br/>");
                }
                //sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  ;'  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>Receipt No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>" + MemType + " </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                //sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  ;'  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>Receipt No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>" + MemType + " </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");
                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");
                if (hostel == 1)
                {
                    string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                    if (studtype.ToLower () == "hostler")
                    {
                        string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                        if (hostelpk != "" && hostelpk != "0")
                        {
                            string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                            sbHtml.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name</td><td style='width:160px;' >: " + Hostelname  + "</td>");
                        }
                    }

                }
                sbHtml.Append("</tr></table>");


                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>" + MemType + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Receipt No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + " </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermorSem + " </td><td style='width:160px; '>: " + cursem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td>");
                if (hostel == 1)
                {
                    string studtype = d2.GetFunction("select stud_type from registration where app_no='" + appnoNew + "'");
                    if (studtype.ToLower() == "hostler")
                    {
                        string hostelpk = d2.GetFunction("select HostelMasterFK from HT_HostelRegistration where APP_No='" + appnoNew + "' and ISNULL (IsVacated,'0')='0' and ISNULL (IsDiscontinued,'0')='0'");
                        if (hostelpk != "" && hostelpk != "0")
                        {
                            string Hostelname = d2.GetFunction("select HostelName from HM_HostelMaster where HostelMasterPK='" + hostelpk + "'");
                            sbHtml.Append("<td style='width:140px; text-align:right; ' colspan='1'>Hostel Name</td><td style='width:160px;' >: " + Hostelname  + "</td>");
                        }
                    }

                }
                sbHtml.Append("</tr></table>");

                #endregion

                #region Receipt Body
                //sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                //sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");
                sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                string semyear = "";
                if (ddl_semrcpt.Items.Count > 0)
                {
                    semyear = Convert.ToString(ddl_semrcpt.SelectedItem.Value);
                }
                int rows = 0;
                string selectQuery = "";
                List<string> lstgrpHeaderValu = new List<string>();
                List<string> lstgrpHeaderName = new List<string>();

                lstgrpHeaderValu = GetSelectedItemsValueList(cbl_grpheader);
                lstgrpHeaderName = GetSelectedItemsTextList(cbl_grpheader);
                #region To Count Rows
                for (int j = 0; j < lstgrpHeaderValu.Count; j++)
                {
                    string BalNOT0 = string.Empty;
                    #region Load Ledgers
                    string headercode = "";

                    headercode = Convert.ToString(lstgrpHeaderValu[j]);

                    selectQuery = " SELECT isnull(sum(BalAmount),0) as BalAmount FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and l.LedgerMode=0  and T.TextCode in('" + semyear + "') AND A.App_No = " + appnoNew + "  and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";
                    if (rbl_headerselect.SelectedIndex == 0)
                    {
                        //Group Header
                        selectQuery = " SELECT isnull(sum(BalAmount),0) as BalAmount FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + headercode + "') and T.TextCode in('" + semyear + "')   and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";
                        if (rdo_receipt.Checked || rdo_sngle.Checked)
                        {
                            selectQuery += " AND A.App_No = " + appnoNew + " ";
                        }

                        if (lbltype.Text != "")
                        {
                            selectQuery += "  and Stream ='" + lbltype.Text.Trim() + "' ";
                        }

                    }
                    else if (rbl_headerselect.SelectedIndex == 1)
                    {
                        //Header
                        selectQuery += "  and A.HeaderFK in (" + headercode + ") ";
                    }
                    else
                    {
                        //Ledger
                        selectQuery += "  and A.LedgerFK  in (" + headercode + ")  ";
                    }


                    if (selectQuery.Trim() != "")
                    {
                        BalNOT0 = d2.GetFunction(selectQuery);
                        double balChk = 0;
                        double.TryParse(BalNOT0, out balChk);
                        if (balChk > 0)
                        {
                            rows++;
                        }
                    }
                    #endregion

                }

                #endregion

                int sno = 0;
                int indx = 0;
                double totalamt = 0;
                double balanamt = 0;
                double curpaid = 0;
                double paidamount = 0;
                int ledgCnt = 0;
                #region Insert Process New

                //For Every Selected Headers
                Hashtable htHdrAmt = new Hashtable();
                Hashtable htHdrName = new Hashtable();
                for (int j = 0; j < lstgrpHeaderValu.Count; j++)
                {
                    string disphdr = string.Empty;
                    double allotamt0 = 0;
                    double deductAmt0 = 0;
                    double totalAmt0 = 0;
                    double paidAmt0 = 0;
                    double balAmt0 = 0;
                    double creditAmt0 = 0;
                    double alreadyPaid = 0;

                    #region Load Ledgers

                    string headercode = "";
                    disphdr = Convert.ToString(lstgrpHeaderName[j]);
                    headercode = Convert.ToString(lstgrpHeaderValu[j]);

                    selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,priority,LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0) as   DeductAmount,isnull(TotalAmount,0) as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount, isnull(BalAmount,0) as BalAmount,TextVal,TextCode FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and l.LedgerMode=0  and T.TextCode in('" + semyear + "') AND A.App_No = " + appnoNew + "  and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";

                    if (rbl_headerselect.SelectedIndex == 0)
                    {
                        //Group Header
                        selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,priority,LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)   as DeductAmount ,isnull(TotalAmount,0)   as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount,TextVal,TextCode,ChlGroupHeader FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + headercode + "') and T.TextCode in('" + semyear + "')  and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";
                        if (rdo_receipt.Checked || rdo_sngle.Checked)
                        {
                            selectQuery += " AND A.App_No = " + appnoNew + " ";
                        }

                        if (lbltype.Text != "")
                        {
                            selectQuery += "  and Stream ='" + lbltype.Text.Trim() + "' ";
                        }

                    }
                    else if (rbl_headerselect.SelectedIndex == 1)
                    {
                        //Header
                        selectQuery += "  and A.HeaderFK in (" + headercode + ") ";
                    }
                    else
                    {
                        //Ledger
                        selectQuery += "  and A.LedgerFK  in (" + headercode + ")  ";
                    }

                    selectQuery += "  order by case when priority is null then 1 else 0 end, priority ";

                    #endregion

                    DataSet dsLedgers = new DataSet();
                    dsLedgers = d2.select_method_wo_parameter(selectQuery, "Text");
                    if (dsLedgers.Tables.Count > 0 && dsLedgers.Tables[0].Rows.Count > 0)
                    {
                        for (int lgri = 0; lgri < dsLedgers.Tables[0].Rows.Count; lgri++)
                        {
                            string feecat1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["TextCode"]);
                            string headerfk1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["HeaderFK"]);
                            string ledgerfk1 = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["LedgerFK"]);
                            double feeamt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["FeeAmount"]);
                            double deductAmt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["DeductAmount"]);
                            double totalamt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["TotalAmount"]);
                            double paidAmt1 = Convert.ToDouble(dsLedgers.Tables[0].Rows[lgri]["PaidAmount"]);
                            double balAmt1 = totalamt1 - paidAmt1;
                            double creditAmt1 = balAmt1;
                            alreadyPaid += paidAmt1;
                            creditAmt0 += creditAmt1;
                            totalAmt0 += totalamt1;
                            balAmt0 += balAmt1 - creditAmt1;
                            paidAmt0 += creditAmt1;
                            deductAmt0 += deductAmt1;

                            #region Ledger Insert Update

                            if (creditAmt1 > 0)
                            {

                                string iscollected = "0";
                                string collecteddate = "";
                                if (PayMode == "1" || PayMode == "6")
                                {
                                    iscollected = "1";
                                    collecteddate = (dtrcpt).ToString();
                                }
                                //else if (PayMode == "2")
                                //{
                                //    iscollected = AutoClearCheck();
                                //    collecteddate = (dtrcpt).ToString();
                                //}
                                //else if (PayMode == "3")
                                //{
                                //    iscollected = isCollectedForDD();
                                //    collecteddate = (dtrcpt).ToString();
                                //}
                                string insertDebit = "INSERT INTO FT_FinDailyTransaction(TransDate,TransTime,TransCode,MemType,App_No,LedgerFK,HeaderFK,FeeCategory,Credit,Debit,PayMode,DDNo,DDDate,DDBankCode,DDBankBranch,TransType,IsInstallmentPay,InstallmentNo,Narration,PayAt,PayThrough,IsArrearCollect,ArearFinYearFK,EntryUserCode,FinYearFK,Receipttype,IsCollected,CollectedDate,IsDeposited,DepositedDate) VALUES('" + dtrcpt + "','" + DateTime.Now.ToLongTimeString() + "','" + receiptno + "', " + memtype + ", " + appnoNew + ", " + ledgerfk1 + ", " + headerfk1 + ", " + feecat1 + ", 0, " + creditAmt1 + ", " + PayMode + ", '" + checkDDno + "', '" + dtchkdd + "', '" + newbankcode + "','" + branch + "', 1, '0', 0, '" + txt_remark.Text.Trim() + "', '0', '0', '0', 0, " + usercode + ", " + finYearid + ",'" + rcptType + "','" + iscollected + "','" + collecteddate + "','" + iscollected + "','" + collecteddate + "')";

                                d2.update_method_wo_parameter(insertDebit, "Text");
                                //Update process

                                string selectquery = " select  isnull(TotalAmount,0) as TotalAmount,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount  from FT_FeeAllot where App_No =" + appnoNew + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "' and istransfer='0'";

                                DataSet dsPrevAMount = new DataSet();
                                dsPrevAMount = d2.select_method_wo_parameter(selectquery, "Text");
                                if (dsPrevAMount.Tables.Count > 0)
                                {
                                    if (dsPrevAMount.Tables[0].Rows.Count > 0)
                                    {
                                        double total = 0;
                                        double paidamt = 0;
                                        double balamt = 0;

                                        total = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["TotalAmount"]);

                                        if (total > 0)
                                        {
                                            paidamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["PaidAmount"]);
                                            balamt = Convert.ToDouble(dsPrevAMount.Tables[0].Rows[0]["BalAmount"]);

                                            balamt = (total - paidamt);

                                            string updatequery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +" + creditAmt1 + " ,BalAmount =" + (balamt - creditAmt1) + "  where App_No =" + appnoNew + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "' and istransfer='0'";
                                            d2.update_method_wo_parameter(updatequery, "Text");

                                            InsertUpdateOK = true;
                                        }

                                    }
                                }
                            }

                            #endregion

                            ////New Header wise display
                            //string hdrName = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["HeaderName"]);
                            //if (!htHdrName.Contains(headerfk1))
                            //{
                            //    htHdrName.Add(headerfk1, hdrName);
                            //}
                            //if (htHdrAmt.Contains(headerfk1))
                            //{
                            //    double tempHdrAmt = 0;
                            //    double.TryParse(htHdrAmt[headerfk1].ToString(), out tempHdrAmt);
                            //    htHdrAmt[headerfk1] = tempHdrAmt + creditAmt1;
                            //}
                            //else
                            //{
                            //    htHdrAmt.Add(headerfk1, creditAmt1);
                            //}

                        }
                    }

                    if (creditAmt0 > 0)
                    {
                        sno++;
                        ledgCnt++;
                        totalamt += Convert.ToDouble(totalAmt0);
                        balanamt += Convert.ToDouble(balAmt0);
                        curpaid += Convert.ToDouble(paidAmt0);

                        deductionamt += Convert.ToDouble(deductAmt0);

                        indx++;
                        createPDFOK = true;

                        //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                        //sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                        sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                    }

                }
                #endregion
                if (BalanceType == 1)
                {
                    balanamt = retBalance(appnoNew, BalanceType);
                }
                //sno = 0;
                //foreach (DictionaryEntry hdrVal in htHdrAmt)
                //{

                //    string hdrPk = hdrVal.Key.ToString();
                //    string disphdr = htHdrName.Contains(hdrPk) ? htHdrName[hdrPk].ToString() : string.Empty;
                //    string hdrAmt = hdrVal.Value.ToString();
                //    double creditAmt0 = 0; double.TryParse(hdrAmt, out creditAmt0);
                //    if (creditAmt0 > 0)
                //    {
                //        sno++;
                //        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                //    }
                //}

                #region ddNarration
                string ddnar = string.Empty;
                //double modeht = 40;
                if (narration != 0)
                {
                    if (!rb_cash.Checked)
                    {
                        if (rb_dd.Checked == true)
                        {
                            ddnar = ddnar + "\nDDNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                        }
                        else if (rb_cheque.Checked)
                        {
                            ddnar = ddnar + "\nChequeNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                        }
                        else if (rb_card.Checked)
                        {
                            ddnar = ddnar + "\n\nCard : " + newbankname;
                        }
                    }
                    ddnar += "\n" + txt_remark.Text.Trim();

                    if (excessRemaining(appnoNew) > 0)
                    {
                        ddnar += " Excess Amount Rs. : " + excessRemaining(appnoNew);
                    }
                }
                #endregion

                double totalamount = curpaid;


                //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                //sbHtml.Append("</table></div><br>");

                //sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                //sbHtmlCopy.Append("</table></div><br>");
                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                sbHtml.Append("</table></div>");

                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                if (ledgCnt == 1)
                    officeCopyHeight = 290; //270;
                else if (ledgCnt == 2)
                    officeCopyHeight = 150; //240;
                else if (ledgCnt == 3)
                    officeCopyHeight = 230;//210;
                else if (ledgCnt == 4)
                    officeCopyHeight = 200;//180;
                else if (ledgCnt == 5)
                    officeCopyHeight = 165;// 150;
                sbHtmlCopy.Append("</table></div><br>");
                sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());

                //#endregion

                //contentDiv.Append(sbHtml.ToString() + (studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");



                #endregion

                contentDiv.Append(sbHtml.ToString() + (studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>");

            }
        }
        #endregion

        return contentDiv.ToString();
    }
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
    public void isContainsDecimal(double myValue)
    {
        bool hasFractionalPart = (myValue - Math.Round(myValue) != 0);
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }

    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " and ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
    }
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
}