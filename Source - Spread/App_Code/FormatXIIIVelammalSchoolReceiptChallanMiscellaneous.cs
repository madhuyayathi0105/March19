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

public class FormatXIIIVelammalSchoolReceiptChallanMiscellaneous : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
    public FormatXIIIVelammalSchoolReceiptChallanMiscellaneous()
    {
        //
        // TODO: Add constructor logic here
        //
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
    public string generateOriginal(string txt_rcptno, string txt_date, string txt_dept, CheckBox rb_cash, CheckBox rb_cheque, CheckBox rb_dd, CheckBox rb_card, string collegecode1, string usercode, ref string lastRecptNo, ref string accidRecpt, RadioButtonList rbl_rollnoNew, DropDownList rbl_rollno, string appnoNew, string outRoll, TextBox txtDept_staff, string rollno, string app_formno, string Regno, string studname, GridView grid_Details, byte BalanceType, DataTable dtMulBnkDetails, CheckBox chk_rcptMulmode, string modeMulti, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, ref bool contentVisible, ref bool CreateReceiptOK, ref bool imgDIVVisible, ref Label lbl_alert, CheckBox cb_CautionDep, CheckBox cb_govt, CheckBox cb_exfees, string mode, TextBox txt_ddno, DropDownList ddl_bkname, TextBox txt_chqno, DataSet dsPri, string Roll_admit, string section, string batch_year)
    {
        CreateReceiptOK = false;
        contentVisible = false;
        imgDIVVisible = false;
        lastRecptNo = string.Empty;
        accidRecpt = string.Empty;
        StringBuilder contentDiv = new StringBuilder();

        //Basic Data
        //string rollno = txt_rollno.Text.Trim();
        string recptNo = txt_rcptno.Trim();
        string recptDt = txt_date.Trim();
        //string studname = txt_name.Text.Trim();
        string course = txt_dept.Trim();
        string batchYrSem = string.Empty;

        string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");

        string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
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
        }
        else
        {
            mode = modeMulti;
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

                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);

                //Document Settings

                StringBuilder sbHtml = new StringBuilder();

                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
                    {
                        colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                    }
                    else
                    {
                        colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
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
                Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                DataView dv = new DataView();
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
                double deductionamt = 0;
                string clgType = string.Empty;
                ds.Clear();
                bool studfinFk = false;
                string strMem = string.Empty;
                ds = d2.select_method_wo_parameter(colquery, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);

                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (rbl_rollnoNew.SelectedIndex == 0)
                        {
                            clgType = d2.GetFunction("select distinct type from course c,degree d,registration r where c.course_id=d.course_id and d.degree_code=r.degree_code and d.college_code = r.college_code  and d.college_code='" + collegecode1 + "'   and r.app_no='" + appnoNew + "'");
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
                            strMem = "Admission No";
                            studfinFk = true;
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
                            strMem = "Staff Code";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 2)
                        {
                            deg = " - ";
                            strMem = "Vendor Code";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 3)
                        {
                            deg = " - ";
                            strMem = "Other Code";
                        }
                    }

                    if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        course = txtDept_staff.Text.Trim();
                    }
                }

                #region Receipt Header

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
                    degString = className[1];
                    if (((degString.Trim() == "XI STD") || (degString.Trim() == "XISTD") || (degString.Trim().ToLower() == "XISTD".ToLower()) || (degString.Trim().ToLower() == "XI STD".ToLower())) || ((degString.Trim() == "XI STD") || (degString.Trim() == "XISTD") || (degString.Trim().ToLower() == "XISTD".ToLower()) || (degString.Trim().ToLower() == "XI STD".ToLower())))
                    {
                        if (!string.IsNullOrEmpty(seatty))
                            degString += "-(" + seatty + ")";
                    }
                }

                sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  ;'  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>Bill No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>" + strMem + " </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>Class Name </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>Section </td><td style='width:160px; '>: " + section + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                #endregion

                #region Receipt Body

                int rows = 0;
                foreach (GridViewRow row in grid_Details.Rows)
                {
                    CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                    TextBox txtFeeamt = (TextBox)row.FindControl("txt_fee_amt");
                    double feeAmt = 0;
                    double.TryParse(Convert.ToString(txtFeeamt.Text), out feeAmt);
                    if ((!chkOkPay.Checked && feeAmt == 0) || (chkOkPay.Checked && feeAmt == 0))
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

                sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>SI.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                Hashtable htHdrAmt = new Hashtable();
                Hashtable htHdrName = new Hashtable();

                int sno = 0;
                int indx = 0;
                double totalamt = 0;
                double balanamt = 0;
                double curpaid = 0;
                Hashtable htfinyearfk = new Hashtable();
                foreach (GridViewRow row in grid_Details.Rows)
                {
                    CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");

                    TextBox txtFeeamt = (TextBox)row.FindControl("txt_fee_amt");
                    double feeAmt = 0;
                    double.TryParse(Convert.ToString(txtFeeamt.Text), out feeAmt);
                    if ((!chkOkPay.Checked && feeAmt == 0) || (chkOkPay.Checked && feeAmt == 0))
                        continue;

                    TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                    TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                    TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                    TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                    TextBox txtdeductamt = (TextBox)row.FindControl("txt_deduct_amt");

                    Label lblFeeCategory = (Label)row.FindControl("lbl_feetype");
                    Label lblsem = (Label)row.FindControl("lbl_textval");
                    Label lblfeecat = (Label)row.FindControl("lbl_textCode");
                    Label lblfinYearFk = new Label();
                    if (studfinFk)//actual finyear fk only for student
                    {
                        lblfinYearFk = (Label)row.FindControl("lblfinfk");
                    }
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

                        totalamt += Convert.ToDouble(txtTotalamt.Text);
                        balanamt += Convert.ToDouble(txtBalamt.Text);
                        curpaid += creditamt;
                        //balanamt += Convert.ToDouble(txtTotalamt.Text) + Convert.ToDouble(txtTobePaidamt.Text) - creditamt;
                        deductionamt += Convert.ToDouble(txtdeductamt.Text);
                        indx++;

                        //New Header wise display

                        //Label lbl_hdrName = (Label)row.FindControl("lbl_hdrName");
                        //Label lbl_hdrid = (Label)row.FindControl("lbl_hdrid");
                        Label lbl_hdrName = (Label)row.FindControl("lbl_feetype");
                        Label lbl_hdrid = (Label)row.FindControl("lbl_feecode");
                        string hdrFK = lbl_hdrid.Text.Trim();
                        string hdrName = lbl_hdrName.Text.Trim();
                        string feecode = Convert.ToString
(lblfeecat.Text);
                        feecode += "$" + hdrFK;
                        if (!htHdrName.Contains(hdrFK))
                        {
                            htHdrName.Add(hdrFK, hdrName);
                        }
                        if (htHdrAmt.Contains(hdrFK))
                        {
                            double tempHdrAmt = 0;
                            double.TryParse(htHdrAmt[hdrFK].ToString(), out tempHdrAmt);
                            htHdrAmt[hdrFK] = tempHdrAmt + creditamt;
                        }
                        else
                        {
                            htHdrAmt.Add(hdrFK, creditamt);
                        }

                        //added by sudhagar

                        if (!htfeecat.ContainsKey(feecode))
                            htfeecat.Add(feecode, hdrFK);

                        if (!htfeeAmt.ContainsKey(feecode))
                            htfeeAmt.Add(feecode, creditamt);
                        else
                        {
                            double tempHdrAmt = 0;
                            double.TryParse(htfeeAmt[feecode].ToString(), out tempHdrAmt);
                            htfeeAmt[feecode] = tempHdrAmt + creditamt;
                        }
                        //added by sudhagar finyear taken
                        if (studfinFk && !htfinyearfk.ContainsKey(hdrFK))
                        {
                            htfinyearfk.Add(hdrFK, Convert.ToString(lblfinYearFk.Text));
                        }
                        //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + lblFeeCategory.Text + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditamt) + "." + returnDecimalPart(creditamt) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                    }
                }
                if (BalanceType == 1)
                {
                    balanamt = retBalance(appnoNew, BalanceType);
                }

                //sudhagar
                sno = 0;
                foreach (KeyValuePair<string, string> hdrVal in htfeecat)
                {
                    sno++;
                    string hdfee = hdrVal.Key.ToString();
                    string Feecode = Convert.ToString(hdfee.Split('$')[0]);
                    string hdrPk = hdrVal.Value.ToString();
                    string disphdr = htHdrName.Contains(hdrPk) ? htHdrName[hdrPk].ToString() : string.Empty;
                    string hdrAmt = htfeeAmt.ContainsKey(hdfee) ? htfeeAmt[hdfee].ToString() : string.Empty;
                    double creditAmt0 = 0; double.TryParse(hdrAmt, out creditAmt0);
                    string MnthName = string.Empty;
                    int AddYear = 0;
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ds.Tables[2].DefaultView.RowFilter = "FeeCategory='" + Convert.ToString(Feecode) + "'";
                        dv = ds.Tables[2].DefaultView;
                        if (dv.Count > 0)
                        {
                            MnthName = getMonth(Convert.ToString(dv[0]["monthcode"]));
                            int MnCode = Convert.ToInt32(Convert.ToString(dv[0]["monthcode"]));
                            string monthYr = Convert.ToString(dv[0]["MonthYear"]);
                            //if (MnCode < 6)
                            //    AddYear = Convert.ToInt32(batch_year) + 1;
                            //else
                            //    AddYear = Convert.ToInt32(batch_year);
                           // if (!string.IsNullOrEmpty(monthYr))
                              //  AddYear = Convert.ToInt32(monthYr);
                            string finyear = Convert.ToString(htfinyearfk[hdrPk]);
                            AddYear = Convert.ToInt32(d2.GetFunction("select distinct (convert(varchar(10),datepart(year,finyearstart))) as finyearfk from fm_finyearmaster where collegecode='" + collegecode1 + "' and finyearpk='" + finyear + "'"));
                            //added by sudhagar
                            if (clgType.Trim() == "CBSE")
                            {
                                if (Convert.ToString(MnthName) != "" && Convert.ToString(MnthName) == "MAR" && AddYear > 0)
                                    AddYear += 1;
                            }
                            else
                            {
                                if (Convert.ToString(MnthName) != "" && Convert.ToString(MnthName) == "FEB" && AddYear > 0)
                                    AddYear += 1;
                            }
                            ////Aruna 19/May/2017==========
                            //if (Convert.ToString(MnthName) != "" && Convert.ToString(MnthName) == "MAR")
                            //{
                            //    if (AddYear > 0)
                            //    {
                            //        AddYear += 1;
                            //    }
                            //}
                            //===========================
                        }
                    }
                    if (!string.IsNullOrEmpty(MnthName) && MnthName.Trim() != "-")
                        disphdr += "  (" + MnthName + " - " + AddYear + ")";

                    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                }
                #region old

                // sno = 0;
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
                #endregion

                //double totalamount = curpaid;

                //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:14px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3' style='text-align:right; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                //sbHtml.Append("</table></div><br>");
                #region DDNarration
                string ddnar = string.Empty;
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
                            }
                            else if (rb_cheque.Checked)
                            {
                                ddnar = ddnar + "\nChequeNo : " + checkDDno + " Bank : " + newbankname + "\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                                // mode = "Cheque - No:" + checkDDno;
                            }
                            else if (rb_card.Checked)
                            {
                                ddnar = "\nCard : " + newbankname;
                            }
                        }
                    }
                    ddnar += "\n" + txt_remark.Text.Trim();
                }
                #endregion

                //mode added by sudhagar
                double totalamount = curpaid;

                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:14px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:right; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                sbHtml.Append("</table></div><br>");

                //sbHtml.Append("<table style='width:350px; height:20px;padding-left:10px;' class='classBold10'><tr><td>" + mode.ToUpper() + ddnar + "</td></tr>");
                //sbHtml.Append("<table style='width:580px; height:20px;padding-left:330px;' class='classBold10'><tr><td>" + balanamt + "</td></tr></table>");



                //sbHtml.Append("<table style='width:580px; height:28px;padding-left:2px;' class='classBold10'><tr><td style='width:490px;'></td><td style='width:60px;text-align:right;'>" + totalamount + "</td><td style='width:30px;'></td></tr><tr><td>Received Rupees " + DecimalToWords((decimal)totalamount) + " Only.</td><td colspan='2'></td></tr></table>");
                #endregion

                contentDiv.Append(sbHtml.ToString());

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
    public string generateMultiple(DataSet dsPri, string collegecode1, string appnoNew, string section, ref PdfDocument recptDoc, ref PdfPage rcptpage, string recptNo, string studname, string recptDt, string Regno, string rollno, string app_formno, RadioButton rb_cash, RadioButton rb_dd, RadioButton rb_cheque, RadioButton rb_card, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, string mode, RadioButtonList rbl_rollnoNew, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, Label lbltype, RadioButton rdo_receipt, RadioButton rdo_sngle, string PayMode, DateTime dtrcpt, string memtype, string receiptno, string dtchkdd, string newbankcode, string usercode, string finYearid, int rcptType, bool InsertUpdateOK, ref bool createPDFOK, byte BalanceType, ref double overallCashAmt, string course, TextBox txt_ddno, DropDownList ddl_bkname, TextBox txt_chqno, string Roll_admit)
    {
        StringBuilder contentDiv = new StringBuilder();
        //VELAMMAL VIDYALAYA
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

                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);

                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
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

                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
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
                    degString = className[1];
                }
                StringBuilder sbHtml = new StringBuilder();
                #region Receipt Header

                sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");

                sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  ;'  colspan='6'>Receipt </td></tr><tr><td style='width:80px; '>Bill No </td><td style='width:240px; '>: " + recptNo + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Admission No </td><td style='width:160px; '>: " + Roll_admit + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>Class Name </td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>Section </td><td style='width:160px; '>: " + section + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                #endregion

                #region Receipt Body
                sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>SI.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

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

                                string selectquery = " select  isnull(TotalAmount,0) as TotalAmount,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount  from FT_FeeAllot where App_No =" + appnoNew + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "'";

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

                                            string updatequery = "update FT_FeeAllot set PaidAmount=isnull(PaidAmount,0) +" + creditAmt1 + " ,BalAmount =" + (balamt - creditAmt1) + "  where App_No =" + appnoNew + " and feecategory ='" + feecat1 + "' and ledgerfk ='" + ledgerfk1 + "'";
                                            d2.update_method_wo_parameter(updatequery, "Text");

                                            InsertUpdateOK = true;
                                        }

                                    }
                                }
                            }

                            #endregion

                            //New Header wise display
                            string hdrName = Convert.ToString(dsLedgers.Tables[0].Rows[lgri]["HeaderName"]);
                            if (!htHdrName.Contains(headerfk1))
                            {
                                htHdrName.Add(headerfk1, hdrName);
                            }
                            if (htHdrAmt.Contains(headerfk1))
                            {
                                double tempHdrAmt = 0;
                                double.TryParse(htHdrAmt[headerfk1].ToString(), out tempHdrAmt);
                                htHdrAmt[headerfk1] = tempHdrAmt + creditAmt1;
                            }
                            else
                            {
                                htHdrAmt.Add(headerfk1, creditAmt1);
                            }

                        }
                    }

                    if (creditAmt0 > 0)
                    {
                        sno++;

                        totalamt += Convert.ToDouble(totalAmt0);
                        balanamt += Convert.ToDouble(balAmt0);
                        curpaid += Convert.ToDouble(paidAmt0);

                        deductionamt += Convert.ToDouble(deductAmt0);

                        indx++;
                        createPDFOK = true;

                        //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                    }

                }
                #endregion
                if (BalanceType == 1)
                {
                    balanamt = retBalance(appnoNew, BalanceType);
                }
                sno = 0;
                foreach (DictionaryEntry hdrVal in htHdrAmt)
                {

                    string hdrPk = hdrVal.Key.ToString();
                    string disphdr = htHdrName.Contains(hdrPk) ? htHdrName[hdrPk].ToString() : string.Empty;
                    string hdrAmt = hdrVal.Value.ToString();
                    double creditAmt0 = 0; double.TryParse(hdrAmt, out creditAmt0);
                    if (creditAmt0 > 0)
                    {
                        sno++;
                        sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                    }
                }
                double totalamount = curpaid;
                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:14px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3' style='text-align:right; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                sbHtml.Append("</table></div><br>");

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
                }
                #endregion
                //sbHtml.Append("<table style='width:350px; height:20px;padding-left:10px;' class='classBold10'><tr><td>" + mode.ToUpper() + ddnar + "</td></tr>");
                //sbHtml.Append("<table style='width:580px; height:20px;padding-left:330px;' class='classBold10'><tr><td>" + balanamt + "</td></tr></table>");


                //sbHtml.Append("<table style='width:580px; height:28px;padding-left:2px;' class='classBold10'><tr><td style='width:490px;'></td><td style='width:60px;text-align:right;'>" + totalamount + "</td><td style='width:30px;'></td></tr><tr><td>Received Rupees " + DecimalToWords((decimal)totalamount) + " Only.</td><td colspan='2'></td></tr></table>");

                //sbHtml.Append("</td></tr></table></div>");

                #endregion

                contentDiv.Append(sbHtml.ToString());
            }
        }
        #endregion

        return contentDiv.ToString();
    }
}