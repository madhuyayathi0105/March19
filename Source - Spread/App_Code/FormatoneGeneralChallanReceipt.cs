using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Text;

public class FormatoneGeneralChallanReceipt : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
    public FormatoneGeneralChallanReceipt()
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
    private double excessRemaining(string appnoNew, string transcode)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " and  dailytranscode='" + transcode + "' ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
    //Original Receipt
    public string generateOriginal(string txt_rcptno, string txt_date, string txt_dept, CheckBox rb_cash, CheckBox rb_cheque, CheckBox rb_dd, CheckBox rb_card, CheckBox rb_NEFT, string collegecode1, string usercode, ref string lastRecptNo, ref string accidRecpt, RadioButtonList rbl_rollnoNew, DropDownList rbl_rollno, string appnoNew, string outRoll, TextBox txtDept_staff, string rollno, string app_formno, string Regno, string studname, GridView grid_Details, byte BalanceType, DataTable dtMulBnkDetails, CheckBox chk_rcptMulmode, string modeMulti, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, ref bool contentVisible, ref bool CreateReceiptOK, ref bool imgDIVVisible, ref Label lbl_alert, CheckBox cb_CautionDep, CheckBox cb_govt, CheckBox cb_exfees)
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
        string mode = string.Empty;

        //added by saranya 06/12/2017

        string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();

        try
        {
            course = course.Split('-')[0];
        }
        catch { course = ""; }

        if (rb_cash.Checked)
        {
            mode = "Cash";
        }
        else if (rb_cheque.Checked)
        {
            mode = "Cheque";
        }
        else if (rb_dd.Checked)
        {
            mode = "DD";
        }
        else if (rb_card.Checked)
        {
            mode = "Card";
        }
        else if (rb_NEFT.Checked)
        {
            mode = "NEFT";
        }
        //Fields to print
        string queryPrint1 = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(queryPrint1, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                //Header Div Values
                byte collegeid = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeName"]);
                byte address1 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd1"]);
                byte address2 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd2"]);
                byte address3 = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeAdd3"]);
                byte city = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeDist"]);
                byte state = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeState"]);

                byte university = Convert.ToByte(ds.Tables[0].Rows[0]["IsCollegeUniversity"]);
                byte rightLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsRightLogo"]);
                byte leftLogo = Convert.ToByte(ds.Tables[0].Rows[0]["IsLeftLogo"]);
                byte time;
                if (Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0]["IsTime"])))
                {
                    time = 1;
                }
                else
                {
                    time = 0;
                }
                byte degACR = Convert.ToByte(ds.Tables[0].Rows[0]["IsDegreeAcr"]);
                byte degNam = Convert.ToByte(ds.Tables[0].Rows[0]["IsDegreeName"]);
                byte studnam = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudName"]);
                byte year = Convert.ToByte(ds.Tables[0].Rows[0]["IsYear"]);
                byte semester = Convert.ToByte(ds.Tables[0].Rows[0]["IsSemester"]);
                byte regno = Convert.ToByte(ds.Tables[0].Rows[0]["IsRegNo"]);
                byte rolno = Convert.ToByte(ds.Tables[0].Rows[0]["IsRollNo"]);
                byte admno = Convert.ToByte(ds.Tables[0].Rows[0]["IsAdminNo"]);

                byte fathername = Convert.ToByte(ds.Tables[0].Rows[0]["IsFatherName"]);
                byte seattype = Convert.ToByte(ds.Tables[0].Rows[0]["IsSeatType"]);
                //byte setRollAsAdmin = Convert.ToByte(ds.Tables[0].Rows[0]["rollas_adm"]);
                byte boarding = Convert.ToByte(ds.Tables[0].Rows[0]["IsBoarding"]);
                byte mothername = Convert.ToByte(ds.Tables[0].Rows[0]["IsMontherName"]);
                string recptValid = Convert.ToString(ds.Tables[0].Rows[0]["ValidDate"]);


                //Body Div Values
                //byte showAllFees = Convert.ToByte(ds.Tables[0].Rows[0]["showallfee"]);
                byte allotedAmt = Convert.ToByte(ds.Tables[0].Rows[0]["IsAllotedAmt"]);
                byte fineAmt = Convert.ToByte(ds.Tables[0].Rows[0]["IsFineAmt"]);
                byte balAmt = Convert.ToByte(ds.Tables[0].Rows[0]["IsBalanceAmt"]);
                byte semOrYear = Convert.ToByte(ds.Tables[0].Rows[0]["IsSemYear"]);
                byte prevPaidAmt = Convert.ToByte(ds.Tables[0].Rows[0]["IsPrevPaid"]);
                byte excessAmt = Convert.ToByte(ds.Tables[0].Rows[0]["IsExcessAmt"]);
                //byte totDetails = Convert.ToByte(ds.Tables[0].Rows[0]["Total_Details"]);
                byte fineInRow = Convert.ToByte(ds.Tables[0].Rows[0]["IsFineinRow"]);
                //byte totWTselectCol = Convert.ToByte(ds.Tables[0].Rows[0]["TotalSelCol"]);
                byte concession = Convert.ToByte(ds.Tables[0].Rows[0]["IsConcession"]);
                string concessionValue = string.Empty;
                if (concession != 0)
                {
                    concessionValue = Convert.ToString(ds.Tables[0].Rows[0]["ConcessionName"]);
                }


                //Footer Div Values

                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);
                byte narration = Convert.ToByte(ds.Tables[0].Rows[0]["IsNarration"]);
                byte deduction = Convert.ToByte(ds.Tables[0].Rows[0]["IsTotConcession"]);
                byte forclgName = Convert.ToByte(ds.Tables[0].Rows[0]["IsForCollegeName"]);
                byte authSign = Convert.ToByte(ds.Tables[0].Rows[0]["IsAuthSign"]);
                byte validDate = Convert.ToByte(ds.Tables[0].Rows[0]["IsValidUpto"]);
                string authSignValue = string.Empty;
                if (authSign != 0)
                {
                    authSignValue = Convert.ToString(ds.Tables[0].Rows[0]["AuthName"]);

                }

                MemoryStream memorystream = new MemoryStream();
                bool imgsign = false;

                if (!string.IsNullOrEmpty(authSignValue) && Convert.ToString(ds.Tables[0].Rows[0]["SignImage"]) != "")
                {
                    byte[] sigN = (byte[])ds.Tables[0].Rows[0]["SignImage"];
                    memorystream.Write(sigN, 0, sigN.Length);
                    imgsign = true;
                }


                byte studOffiCopy = Convert.ToByte(ds.Tables[0].Rows[0]["PageType"]);
                // byte dispModeWTcash = Convert.ToByte(ds.Tables[0].Rows[0]["DisModeWithCash"]);
                byte signFile = Convert.ToByte(ds.Tables[0].Rows[0]["cashier_sign"]);

                //if (signFile != 0)
                //{
                //if (FileUpload1.HasFile)
                //{

                //}                                                    
                //}


                //Document Settings
                PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);
                PdfPage rcptpage = recptDoc.NewPage();
                Font FontboldheadC = new Font("Arial", 14, FontStyle.Bold);
                Font Fontboldhead = new Font("Arial", 12, FontStyle.Bold);
                Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
                Font FontTablebody = new Font("Arial", 8, FontStyle.Regular);
                Font FontTable = new Font("Arial", 8, FontStyle.Bold);
                Font tamilFont = new Font("AMUDHAM.TTF", 11, FontStyle.Regular);

                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
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
                string sec = "";
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
                            sec = Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
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
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 2)
                        {
                            deg = " - ";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 3)
                        {
                            deg = " - ";
                        }
                    }

                    if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        course = txtDept_staff.Text.Trim();
                    }
                }

                int pagelength = 1;
                int rectHeight = 800;
                if (studOffiCopy == 1)
                {
                    pagelength = 2;
                    rectHeight = 380;  // Jairam Modify 30-06-2016
                }
                PdfPage rcptpageTran = recptDoc.NewPage();
                int curY = 10;
                int curX = 30;
                for (int pl = 1; pl <= pagelength; pl++)
                {

                    if (pl == 2)
                    {
                        curY = 420;
                    }
                    //Rectangle Border
                    PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
                    PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                    rcptpage.Add(rectSpace);


                    //Header Images
                    //Line1
                    if (leftLogo != 0)  // Jairam Modify for SAN 30-06-2016
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                        {
                            PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                            rcptpage.Add(LogoImage, curX, curY, 450);
                        }
                    }
                    if (collegeid != 0)
                    {
                        curX = 80;
                        PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, curX, curY, 450, 20), ContentAlignment.MiddleCenter, collegename);
                        rcptpage.Add(clgText);
                    }
                    if (rightLogo != 0)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                        {
                            curX = 500;
                            PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                            rcptpage.Add(LogoImage1, curX, curY, 450);
                        }
                    }
                    //Line2
                    if (university != 0)
                    {
                        curY += 20;
                        curX = 120;
                        PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                        rcptpage.Add(uniText);
                    }
                    //Line3
                    string jaiadd1 = ""; // Modify jairam 30-06-2016
                    if (address1 != 0 || address2 != 0)
                    {
                        curX = 120;
                        curY += 15;
                        if (address2 != 0)
                        {
                            jaiadd1 = add1 + " " + add2;
                        }
                        PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, jaiadd1);
                        rcptpage.Add(addText);
                    }
                    //Line4
                    if (address3 != 0)
                    {
                        curX = 120;
                        curY += 15;
                        PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                        rcptpage.Add(cityText);
                    }

                    curX = 280;
                    curY += 35;
                    //Text Area For Receipt
                    PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 20, 30), ContentAlignment.MiddleCenter, "RECEIPT");
                    rcptpage.Add(headingText);
                    int curX1 = 265;
                    int curX2 = 315;
                    curY += 21;
                    //PdfLine underLineRecpt = new PdfLine(recptDoc, new Point(curX1, curY), new Point(curX2, curY), Color.Black, 1);
                    //rcptpage.Add(underLineRecpt);


                    #region Table 1
                    //Table1 Format 
                    int rowIn = 0;
                    int colIn = 0;
                    PdfTable tableparts = recptDoc.NewTable(FontTableHead, 7, 7, 5);
                    tableparts.VisibleHeaders = false;


                    //Table1 Data
                    //Line 1
                    tableparts.Cell(rowIn, colIn).SetContent("Receipt No :" + recptNo);
                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(rowIn, colIn).ColSpan = 2;
                    colIn++;

                    //tableparts.Cell(rowIn, colIn).SetContent(":" + recptNo);
                    //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                    //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                    colIn++;
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }

                    #region studname

                    if (rbl_rollnoNew.SelectedIndex == 0)
                    {

                        if (studnam != 0)
                        {
                            tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            colIn++;
                            //tableparts.Cell(rowIn, colIn).SetContent(": " + studname);
                            //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            colIn++;
                            // tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        }
                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }

                        if (regno != 0)
                        {
                            tableparts.Cell(rowIn, colIn).SetContent("RegNo : " + Regno);
                            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            colIn++;
                            //tableparts.Cell(rowIn, colIn).SetContent(": " + Regno);
                            //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            //tableparts.Cell(rowIn, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            colIn++;
                            // tableparts.Cell(rowIn, colIn).ColSpan = 2;

                        }
                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }

                        if (rolno != 0)
                        {
                            tableparts.Cell(rowIn, colIn).SetContent("RollNo : " + rollno);
                            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            colIn++;
                            //tableparts.Cell(rowIn, colIn).SetContent(": " + rollno);
                            //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            colIn++;
                            //  tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        }
                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }

                        if (admno != 0)
                        {
                            tableparts.Cell(rowIn, colIn).SetContent("AdmissionNo : " + app_formno);
                            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tableparts.Cell(rowIn, colIn).ColSpan = 2;
                            colIn++;
                            //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                            //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                            //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                            colIn++;
                            //tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        }

                    }
                    else if (rbl_rollnoNew.SelectedIndex == 1)
                    {
                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }


                        tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(studname);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        // tableparts.Cell(rowIn, colIn).ColSpan = 2;


                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }

                        tableparts.Cell(rowIn, colIn).SetContent("Staff Id : " + app_formno);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;


                    }
                    else if (rbl_rollnoNew.SelectedIndex == 2)
                    {

                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }


                        tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(studname);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }

                        tableparts.Cell(rowIn, colIn).SetContent("Vendor Id : " + app_formno);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;


                    }
                    else if (rbl_rollnoNew.SelectedIndex == 3)
                    {

                        if (colIn == 6)
                        {
                            colIn = 0;
                            rowIn++;
                        }


                        tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(studname);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                    }

                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }
                    #endregion

                    //tableparts.Cell(rowIn, colIn).SetContent("Date");
                    //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                    //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                    colIn++;

                    tableparts.Cell(rowIn, colIn).SetContent("Date : " + recptDt);
                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                    colIn++;
                    //tableparts.Cell(rowIn, colIn).ColSpan = 2;
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }
                    //Line2


                    if (fathername != 0)
                    {
                        tableparts.Cell(rowIn, colIn).SetContent("Father's Name : " + fathe);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + fathe);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        //tableparts.Cell(1, 4).ColSpan = 2;
                    }
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }

                    if (mothername != 0)
                    {
                        tableparts.Cell(rowIn, colIn).SetContent("Mother's Name : " + mothe);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + mothe);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        // tableparts.Cell(1, colIn).ColSpan = 2;
                    }

                    //Line 3
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }
                    string schoolOrCollege = d2.GetFunction("select top 1 value from Master_Settings where settings='schoolorcollege' and  usercode='" + usercode + "'").Trim();
                    string batYrSemHead = string.Empty;
                    string batYrSemCont = string.Empty;
                    if (schoolOrCollege == "0")
                    {
                        if (degACR != 0)
                        {
                            batYrSemHead = "Class/";
                            batYrSemCont = deg;
                            if (sec.Trim() != string.Empty)
                            {
                                batYrSemCont += "-" + sec + "/";
                            }
                        }

                    }
                    else
                    {
                        if (degACR != 0)
                        {
                            batYrSemHead = "Degree/";
                            batYrSemCont = deg + "/";
                        }
                        if (year != 0)
                        {
                            batYrSemHead += "Yr/";
                            batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";

                        }
                        if (semester != 0)
                        {
                            batYrSemHead += "Sem";
                            batYrSemCont += " " + romanLetter(cursem);
                            if (sec.Trim() != string.Empty)
                            {
                                batYrSemCont += "-" + sec;
                            }
                        }
                    }
                    batYrSemHead = batYrSemHead.TrimEnd('/');
                    batYrSemCont = batYrSemCont.TrimEnd('/');

                    if (batYrSemHead != "")
                    {
                        tableparts.Cell(rowIn, colIn).SetContent(batYrSemHead + " : " + batYrSemCont);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + batYrSemCont);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        // tableparts.Cell(2, colIn).ColSpan = 2;
                    }
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }

                    if (seattype != 0)
                    {
                        tableparts.Cell(rowIn, colIn).SetContent("Seat Type : " + seatty);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + seatty);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        //tableparts.Cell(rowIn, 4).ColSpan = 2;
                    }
                    if (colIn == 6)
                    {
                        colIn = 0;
                        rowIn++;
                    }

                    if (boarding != 0)
                    {
                        tableparts.Cell(rowIn, colIn).SetContent("Boarding : " + board);
                        tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        tableparts.Cell(rowIn, colIn).ColSpan = 2;
                        colIn++;
                        //tableparts.Cell(rowIn, colIn).SetContent(": " + board);
                        //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                        //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                        colIn++;
                        // tableparts.Cell(rowIn, colIn).ColSpan = 2;
                    }

                    tableparts.Cell(rowIn, colIn).SetContent("Collected By :" + userName);
                    tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                    tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                    tableparts.Cell(rowIn, colIn).ColSpan = 2;
                    colIn++;

                    curX = 15;
                    curY += 1;
                    PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 600, 200));
                    rcptpage.Add(addtabletopage1);

                    #endregion

                    #region Table 2
                    //Table2 Format

                    int rows = 1;
                    foreach (GridViewRow row in grid_Details.Rows)
                    {
                        CheckBox chkOkPay = (CheckBox)row.FindControl("cb_selectLedger");
                        if (!chkOkPay.Checked)
                            continue;

                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                        TextBox txtScholAmt = (TextBox)row.FindControl("txt_scholar_amt");
                        TextBox txtCautAmt = (TextBox)row.FindControl("txt_deposit_amt");
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

                            double gvtamt = 0;
                            if (cb_govt.Checked)
                            {
                                double.TryParse(txtScholAmt.Text, out gvtamt);
                            }
                            creditamt += gvtamt;
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


                    Hashtable htIndex = new Hashtable();
                    int hInsdx = 2;
                    //Table2 Header
                    int descWidth = 440;

                    //if (semOrYear != 0)
                    //{

                    //    htIndex.Add("semOrYear", hInsdx);
                    //    hInsdx++;
                    //    descWidth -= 60;
                    //}


                    if (allotedAmt != 0)
                    {

                        htIndex.Add("allotedAmt", hInsdx);
                        hInsdx++;
                        descWidth -= 60;
                    }

                    htIndex.Add("Paid Rs", hInsdx);
                    hInsdx++;
                    descWidth -= 60;

                    if (balAmt != 0)
                    {

                        htIndex.Add("balAmt", hInsdx);
                        hInsdx++;
                        descWidth -= 60;
                    }
                    if (semOrYear != 0)
                    {

                        htIndex.Add("semOrYear", hInsdx);
                        hInsdx++;
                        descWidth -= 60;
                    }
                    if (prevPaidAmt != 0)
                    {

                        htIndex.Add("prevPaidAmt", hInsdx);
                        hInsdx++;
                        descWidth -= 70;
                    }

                    if (concession != 0)
                    {

                        htIndex.Add("concession", hInsdx);
                        hInsdx++;
                        descWidth -= 60;
                    }

                    PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, htIndex.Count + 2, 5);
                    tableparts1.VisibleHeaders = false;
                    tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    tableparts1.Cell(0, 0).SetContent("S.No");
                    tableparts1.Cell(0, 0).SetFont(FontTableHead);
                    tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts1.Columns[0].SetWidth(20);

                    tableparts1.Cell(0, 1).SetContent("Description");
                    tableparts1.Cell(0, 1).SetFont(FontTableHead);
                    tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts1.Columns[1].SetWidth(descWidth);

                    //tableparts1.Cell(0, 2).SetContent("Paid Rs");
                    //tableparts1.Cell(0, 2).SetFont(FontTableHead);
                    //tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    //tableparts1.Columns[2].SetWidth(40);

                    //Table2 Data

                    #region feedata
                    int sno = 0;
                    int indx = 0;
                    double totalamt = 0;
                    double balanamt = 0;
                    double curpaid = 0;
                    double oldPaid = 0;
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
                        double.TryParse(Convert.ToString(txtPaidamt.Text), out oldPaid);
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
                        }

                        if (creditamt > 0)
                        {
                            sno++;
                            indx++;
                            totalamt += Convert.ToDouble(txtTotalamt.Text);
                            balanamt += Convert.ToDouble(txtBalamt.Text);
                            curpaid += creditamt;
                            //balanamt += Convert.ToDouble(txtTotalamt.Text) + Convert.ToDouble(txtTobePaidamt.Text) - creditamt;
                            deductionamt += Convert.ToDouble(txtdeductamt.Text);

                            tableparts1.Cell(indx, 0).SetContent(sno);
                            tableparts1.Cell(indx, 0).SetFont(FontTablebody);
                            tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                            tableparts1.Cell(indx, 1).SetContent(lblFeeCategory.Text);
                            tableparts1.Cell(indx, 1).SetFont(FontTablebody);
                            tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                            //tableparts1.Cell(indx, 2).SetContent(creditamt);
                            //tableparts1.Cell(indx, 2).SetFont(FontTablebody);
                            //tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                            if (semOrYear != 0)
                            {
                                if (htIndex.Contains("semOrYear"))
                                {
                                    int ind = Convert.ToInt32(htIndex["semOrYear"]);
                                    tableparts1.Cell(indx, ind).SetContent(lblsem.Text);
                                    tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                    tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    if (indx == 1)
                                    {
                                        tableparts1.Cell(0, ind).SetContent("Category");
                                        tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                        tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableparts1.Columns[ind].SetWidth(50);
                                    }
                                }

                                // htIndex.Add("semOrYear", hInsdx);
                            }


                            if (allotedAmt != 0)
                            {
                                if (htIndex.Contains("allotedAmt"))
                                {
                                    int ind = Convert.ToInt32(htIndex["allotedAmt"]);
                                    tableparts1.Cell(indx, ind).SetContent(txtTotalamt.Text);
                                    tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                    tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (indx == 1)
                                    {
                                        tableparts1.Cell(0, ind).SetContent("Fixed Fee Rs");
                                        tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                        tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableparts1.Columns[ind].SetWidth(50);
                                    }
                                }
                            }
                            if (htIndex.Contains("Paid Rs"))
                            {
                                int ind = Convert.ToInt32(htIndex["Paid Rs"]);
                                tableparts1.Cell(indx, ind).SetContent(creditamt);
                                tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                if (indx == 1)
                                {
                                    tableparts1.Cell(0, ind).SetContent("Paid Rs");
                                    tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                    tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tableparts1.Columns[ind].SetWidth(50);
                                }
                            }

                            if (balAmt != 0)
                            {
                                if (htIndex.Contains("balAmt"))
                                {
                                    int ind = Convert.ToInt32(htIndex["balAmt"]);
                                    tableparts1.Cell(indx, ind).SetContent(txtBalamt.Text);
                                    // tableparts1.Cell(indx, ind).SetContent(totalamt - (oldPaid+creditamt));
                                    tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                    tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (indx == 1)
                                    {
                                        tableparts1.Cell(0, ind).SetContent("Balance Rs");
                                        tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                        tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableparts1.Columns[ind].SetWidth(50);
                                    }
                                }
                            }
                            if (prevPaidAmt != 0)
                            {
                                if (htIndex.Contains("prevPaidAmt"))
                                {
                                    int ind = Convert.ToInt32(htIndex["prevPaidAmt"]);
                                    tableparts1.Cell(indx, ind).SetContent(txtPaidamt.Text);
                                    tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                    tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (indx == 1)
                                    {
                                        tableparts1.Cell(0, ind).SetContent("Already Paid Rs");
                                        tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                        tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableparts1.Columns[ind].SetWidth(70);
                                    }
                                }

                            }

                            if (concession != 0)
                            {
                                if (htIndex.Contains("concession"))
                                {
                                    int ind = Convert.ToInt32(htIndex["concession"]);
                                    tableparts1.Cell(indx, ind).SetContent(txtdeductamt.Text);
                                    tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                                    tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                                    if (indx == 1)
                                    {
                                        tableparts1.Cell(0, ind).SetContent("Deduction Rs");
                                        tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                        tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        tableparts1.Columns[ind].SetWidth(50);
                                    }
                                }

                            }

                        }
                    }

                    #endregion

                    curY += 5 + (int)addtabletopage1.Area.Height;
                    PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 600));
                    rcptpage.Add(addtabletopage2);

                    #endregion

                    #region Table 3
                    //Table3 Format
                    PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 8, 5);
                    tableparts2.VisibleHeaders = false;




                    tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                    //Table3 Header              
                    decimal totalamount = (decimal)curpaid;

                    //Added by saranya on 12March2018 for mahendra
                    double exAmt = 0;
                    double.TryParse(Convert.ToString(excessRemaining(appnoNew, recptNo)), out exAmt);
                    curpaid += exAmt;
                    decimal TotAmount = (decimal)curpaid;
                    //==================================//

                    tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(TotAmount).ToString() + " Rupees Only");//totalamount
                    tableparts2.Cell(0, 0).SetFont(FontTableHead);
                    tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    tableparts2.Cell(0, 0).ColSpan = 6;

                    tableparts2.Cell(0, 6).SetContent("Total");
                    tableparts2.Cell(0, 6).SetFont(FontTableHead);
                    tableparts2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);

                    //Commented by saranya on 12March2018
                    //double exAmt = 0;
                    //double.TryParse(Convert.ToString(excessRemaining(appnoNew, recptNo)), out exAmt);
                    //curpaid += exAmt;

                    tableparts2.Cell(0, 7).SetContent("Rs. " + curpaid + "/-");
                    tableparts2.Cell(0, 7).SetFont(FontTableHead);
                    tableparts2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);

                    //tableparts2.Cell(0, 6).SetContent("Balance");
                    //tableparts2.Cell(0, 6).SetFont(FontTableHead);
                    //tableparts2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);

                    //if (BalanceType == 1)
                    //{
                    //    balanamt = retBalance(appnoNew, BalanceType);
                    //}
                    //tableparts2.Cell(0, 7).SetContent("Rs. " + balanamt + "/-");
                    //tableparts2.Cell(0, 7).SetFont(FontTableHead);
                    //tableparts2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);


                    // ===============added by saranya 24-112017============================//
                    //decimal totalamount = (decimal)curpaid;
                    //curY += (int)addtabletopage2.Area.Height + 5;
                    //PdfTextArea exText1 = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 250, 20), ContentAlignment.MiddleLeft, "Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                    //rcptpage.Add(exText1);
                    //=========================================//
                    curY += (int)addtabletopage2.Area.Height + 5;
                    PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 50));
                    rcptpage.Add(addtabletopage3);
                    #endregion

                    string ddnar = string.Empty;
                    double modeht = 40;
                    if (narration != 0)
                    {
                        if (chk_rcptMulmode.Checked)
                        {
                            mode = modeMulti;
                            for (int z = 0; z < dtMulBnkDetails.Rows.Count; z++)
                            {
                                ddnar += "\n" + (z + 1).ToString() + ")No : " + dtMulBnkDetails.Rows[z][1] + " Bank : " + dtMulBnkDetails.Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Rows[z][2] + " Date  : " + dtMulBnkDetails.Rows[z][3] + " Amount : " + dtMulBnkDetails.Rows[z][4] + "/-";
                            }
                            modeht = dtMulBnkDetails.Rows.Count * 15;
                            modeht += 30;
                        }
                        else
                        {
                            if (!rb_cash.Checked)
                            {
                                if (rb_dd.Checked == true)
                                {
                                    ddnar = ddnar + "\n\nDDNo : " + checkDDno + "   Bank : " + newbankname + "\n\nBranch :" + branch + "   Date  : " + txt_date1.Text.ToString();
                                }
                                else if (rb_cheque.Checked)
                                {
                                    ddnar = ddnar + "\n\nChequeNo : " + checkDDno + "   Bank : " + newbankname + "\n\nBranch :" + branch + "   Date  : " + txt_date1.Text.ToString();
                                }
                                else if (rb_card.Checked == true)
                                {
                                    ddnar = ddnar + "\n\nCard : " + newbankname;
                                }
                                //Added by saranya on 24/04/2018
                                else if (rb_NEFT.Checked == true)
                                {
                                    ddnar = ddnar + "\n\nNeftNo : " + checkDDno;
                                }
                            }
                            modeht += 50;
                        }
                        ddnar += "\n\nRemarks  " + txt_remark.Text.Trim();
                    }

                    //Mode of Pay

                    curY += 5 + (int)addtabletopage3.Area.Height;
                    if (ddnar.Trim() != "") // Modify jairam 30-06-2016
                    {
                        curY += 5;
                    }
                    PdfTextArea modeofpayText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 30, curY, 250, modeht), ContentAlignment.MiddleLeft, "Mode of Pay : " + mode + ddnar);
                    rcptpage.Add(modeofpayText);

                    if (deduction != 0)
                    {
                        PdfTextArea deducText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 130, curY, 200, 20), ContentAlignment.MiddleCenter, "Deduction Amount Rs. : " + deductionamt);
                        rcptpage.Add(deducText);
                    }
                    if (excessAmt != 0)
                    {
                        PdfTextArea exText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Excess Amount Rs. : " + excessRemaining(appnoNew, recptNo).ToString());
                        rcptpage.Add(exText);
                    }
                    if (validDate != 0)
                    {
                        PdfTextArea valdtText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 370, curY, 200, 20), ContentAlignment.MiddleCenter, "Valid upto : " + "(" + recptValid + ")");
                        rcptpage.Add(valdtText);
                    }

                    //Authorizer
                    if (forclgName != 0)
                    {
                        curY += 15;
                        PdfTextArea authorizeText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 250, 20), ContentAlignment.MiddleCenter, "For " + collegename);
                        rcptpage.Add(authorizeText);
                    }

                    if (authSignValue.Trim() != "")
                    {
                        if (signFile == 1)
                        {

                            //sign image
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + authSignValue + ".jpeg")))
                            {
                                if (imgsign)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memorystream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(1500, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + authSignValue + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + authSignValue + ".jpeg"));
                                curY += 15;
                                rcptpage.Add(LogoImage, 490, curY, 1500);
                                memorystream.Dispose();
                                memorystream.Close();
                            }
                            else
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + authSignValue + ".jpeg")))
                                {
                                    PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + authSignValue + ".jpeg"));
                                    // curY += 15;
                                    rcptpage.Add(LogoImage, 490, curY, 1500);
                                }
                            }
                            curY += 15;
                        }
                        PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, authSignValue);

                        rcptpage.Add(authorizeSignText);
                        authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorised Signature");
                    }
                    else
                    {
                        curY += 15;
                        PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorised Signature");
                        rcptpage.Add(authorizeSignText);
                    }
                    PdfPage rcptpageOf = rcptpage.CreateCopy();

                    if (transCopy != 0 && pl == 1)
                    {
                        int cuyy = curY;
                        //if (authSign == 0)
                        //{
                        cuyy += 10;
                        //}
                        rcptpageTran = rcptpage.CreateCopy();
                        PdfTextArea transCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, cuyy, 150, 20), ContentAlignment.MiddleCenter, "Transport Copy ");
                        rcptpageTran.Add(transCopyText);
                    }
                    if (studCopy != 0 || studOffiCopy == 1)
                    {
                        //if (authSign == 0)
                        //{
                        curY += 30;

                        //}
                        string copy = "Student Copy ";
                        if (rbl_rollnoNew.SelectedIndex == 1)
                        {
                            copy = "Staff Copy ";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 2)
                        {
                            copy = "Vendor Copy ";
                        }
                        else if (rbl_rollnoNew.SelectedIndex == 3)
                        {
                            copy = "Others Copy ";
                        }
                        if (pl == 2)
                            copy = "Office Copy ";
                        PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 180, curY, 200, 20), ContentAlignment.MiddleCenter, copy);
                        //  PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 20, 30), ContentAlignment.MiddleCenter, "RECEIPT");
                        //recptDoc, 450, curY, 150, 20   recptDoc, 240, curY, 200, 20
                        rcptpage.Add(studCopyText);
                        if (pl == pagelength)
                        {
                            rcptpage.SaveToDocument();
                        }
                    }
                    //save changes

                    if (pl == pagelength)
                    {
                        if (officopy != 0 && studOffiCopy != 1)
                        {
                            curY += 30;
                            PdfTextArea offCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Office Copy ");
                            rcptpageOf.Add(offCopyText);
                            rcptpageOf.SaveToDocument();
                        }
                    }

                    if (transCopy != 0 && pl == pagelength)
                    {
                        rcptpageTran.SaveToDocument();
                    }

                    curY += 10;
                }
                //Response Write
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Receipt" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                    //Response.Buffer = true;
                    //Response.Clear();
                    recptDoc.SaveToFile(szPath + szFile);
                    CreateReceiptOK = true;
                    return szFile;

                }
                else
                {
                    imgDIVVisible = true;
                    lbl_alert.Text = "Receipt Cannot Be Generated";
                }
            }

        }
        return string.Empty;
    }
    //Multiple Receipt
    public void generateMultiple(DataSet dsPri, string collegecode1, string appnoNew, string section, ref PdfDocument recptDoc, ref PdfPage rcptpage, string recptNo, string studname, string recptDt, string Regno, string rollno, string app_formno, RadioButton rb_cash, RadioButton rb_dd, RadioButton rb_cheque, RadioButton rb_card, RadioButton rb_NEFT, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, string mode, RadioButtonList rbl_rollnoNew, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, Label lbltype, RadioButton rdo_receipt, RadioButton rdo_sngle, string PayMode, DateTime dtrcpt, string memtype, string receiptno, string dtchkdd, string newbankcode, string usercode, string finYearid, int rcptType, bool InsertUpdateOK, ref bool createPDFOK, byte BalanceType, ref double overallCashAmt)
    {

        Font FontboldheadC = new Font("Arial", 15, FontStyle.Bold);
        Font Fontboldhead = new Font("Arial", 13, FontStyle.Bold);
        Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
        Font FontTablebody = new Font("Arial", 8, FontStyle.Regular);
        Font FontTable = new Font("Arial", 8, FontStyle.Bold);
        Font tamilFont = new Font("AMUDHAM.TTF", 8, FontStyle.Regular);

        //Common and MCC
        #region Print option for Receipt
        #region Settings Input
        //Header Div Values
        byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
        byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
        byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
        byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
        byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
        byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

        byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
        byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
        byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
        byte time;
        if (Convert.ToBoolean(Convert.ToString(dsPri.Tables[0].Rows[0]["IsTime"])))
        {
            time = 1;
        }
        else
        {
            time = 0;
        }
        byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
        byte degNam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeName"]);
        byte studnam = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudName"]);
        byte year = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsYear"]);
        byte semester = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemester"]);
        byte regno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRegNo"]);
        byte rolno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRollNo"]);
        byte admno = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAdminNo"]);

        byte fathername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFatherName"]);
        byte seattype = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSeatType"]);
        //byte setRollAsAdmin = Convert.ToByte(dsPri.Tables[0].Rows[0]["rollas_adm"]);
        byte boarding = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBoarding"]);
        byte mothername = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsMontherName"]);
        string recptValid = Convert.ToString(dsPri.Tables[0].Rows[0]["ValidDate"]);


        //Body Div Values
        //byte showAllFees = Convert.ToByte(dsPri.Tables[0].Rows[0]["showallfee"]);
        byte allotedAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAllotedAmt"]);
        byte fineAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineAmt"]);
        byte balAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsBalanceAmt"]);
        byte semOrYear = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsSemYear"]);
        byte prevPaidAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsPrevPaid"]);
        byte excessAmt = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsExcessAmt"]);
        // byte totDetails = Convert.ToByte(dsPri.Tables[0].Rows[0]["Total_Details"]);
        byte fineInRow = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsFineinRow"]);
        //byte totWTselectCol = Convert.ToByte(dsPri.Tables[0].Rows[0]["TotalSelCol"]);
        byte concession = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsConcession"]);
        string concessionValue = string.Empty;
        if (concession != 0)
        {
            concessionValue = Convert.ToString(dsPri.Tables[0].Rows[0]["ConcessionName"]);
        }


        //Footer Div Values

        byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
        byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
        byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
        byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);
        byte deduction = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTotConcession"]);
        byte forclgName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsForCollegeName"]);
        byte authSign = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsAuthSign"]);
        byte validDate = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsValidUpto"]);
        string authSignValue = string.Empty;
        if (authSign != 0)
        {
            authSignValue = Convert.ToString(dsPri.Tables[0].Rows[0]["AuthName"]);

        }

        byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
        // byte dispModeWTcash = Convert.ToByte(dsPri.Tables[0].Rows[0]["DisModeWithCash"]);
        byte signFile = Convert.ToByte(dsPri.Tables[0].Rows[0]["cashier_sign"]);

        //if (signFile != 0)
        //{
        //if (FileUpload1.HasFile)
        //{

        //}                                                    
        //}


        #endregion

        #region Students Input
        string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL(type,''),isnull(r.sections,'') as sections ,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
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
        string stream = "";

        double deductionamt = 0;
        DataSet ds = new DataSet();
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
                if (degACR == 0)
                {
                    deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                }
                else
                {
                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                }
                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                section = Convert.ToString(ds.Tables[1].Rows[0]["sections"]);
            }
        }
        #endregion

        int pagelength = 1;
        int rectHeight = 800;
        if (studOffiCopy == 1)
        {
            pagelength = 2;
            rectHeight = 380;
        }
        PdfPage rcptpageTran = recptDoc.NewPage();
        int curY = 10;
        int curX = 30;
        for (int pl = 1; pl <= pagelength; pl++)
        {

            if (pl == 2)
            {
                curY = 420;
            }

            #region Receipt Header

            //Rectangle Border
            PdfArea rectArea = new PdfArea(recptDoc, 10, curY, 570, rectHeight);
            PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
            rcptpage.Add(rectSpace);

            //Header Images
            //Line1
            if (leftLogo != 0)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                {
                    PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                    rcptpage.Add(LogoImage, curX, curY, 450);
                }
            }
            if (collegeid != 0)
            {
                curX = 80;
                PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, curX, curY + 5, 450, 20), ContentAlignment.MiddleCenter, collegename);
                rcptpage.Add(clgText);
            }
            if (rightLogo != 0)
            {
                if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                {
                    curX = 500;
                    PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                    rcptpage.Add(LogoImage1, curX, curY, 450);
                }
            }
            //Line2
            if (university != 0)
            {
                curY += 20;
                curX = 120;
                PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, univ);
                rcptpage.Add(uniText);
            }
            //Line3
            string jaiadd1 = "";
            if (address1 != 0 || address2 != 0)
            {
                curX = 120;
                curY += 15;
                if (address2 != 0)
                {
                    jaiadd1 = add1 + " " + add2;
                }
                PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add1);
                rcptpage.Add(addText);
            }
            //Line4
            if (address3 != 0)
            {
                curX = 120;
                curY += 15;
                PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, curX, curY, 350, 20), ContentAlignment.MiddleCenter, add3);
                rcptpage.Add(cityText);
            }

            curX = 280;
            curY += 35;
            //Text Area For Receipt
            PdfTextArea headingText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, curX, curY, 20, 30), ContentAlignment.MiddleCenter, "RECEIPT");
            rcptpage.Add(headingText);
            int curX1 = 265;
            int curX2 = 315;
            curY += 21;
            PdfLine underLineRecpt = new PdfLine(recptDoc, new Point(curX1, curY), new Point(curX2, curY), Color.Black, 1);
            rcptpage.Add(underLineRecpt);

            #endregion

            #region Table 1
            //Line2
            int rowIn = 0;
            int colIn = 0;

            //Table1 Format 
            PdfTable tableparts = recptDoc.NewTable(FontTableHead, 7, 7, 5);
            tableparts.VisibleHeaders = false;


            //Table1 Data
            //Line 1
            tableparts.Cell(rowIn, colIn).SetContent("Receipt No");
            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
            colIn++;

            tableparts.Cell(rowIn, colIn).SetContent(": " + recptNo);
            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
            colIn++;

            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }

            if (studnam != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("Name : " + studname);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + studname);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
                // tableparts.Cell(rowIn, colIn).ColSpan = 2;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }

            tableparts.Cell(rowIn, colIn).SetContent("Date");
            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
            tableparts.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleLeft);
            colIn++;

            tableparts.Cell(rowIn, colIn).SetContent(": " + recptDt);
            tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
            tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
            colIn++;
            //tableparts.Cell(0, 7).ColSpan = 2;
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }


            if (regno != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("RegNo : " + Regno);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + Regno);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }
            if (rolno != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("RollNo : " + rollno);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + rollno);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }
            if (admno != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("AdmissionNo : " + app_formno);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + app_formno);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }

            if (fathername != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("Father's Name : " + fathe);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + fathe);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }
            if (mothername != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("Mother's Name : " + mothe);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + mothe);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            //Line 3
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }

            string schoolOrCollege = d2.GetFunction("select top 1 value from Master_Settings where settings='schoolorcollege' and  usercode='" + usercode + "'").Trim();
            string batYrSemHead = string.Empty;
            string batYrSemCont = string.Empty;
            if (schoolOrCollege == "0")
            {
                if (degACR != 0)
                {
                    batYrSemHead = "Class/";
                    batYrSemCont = deg;
                    if (section.Trim() != string.Empty)
                    {
                        batYrSemCont += "-" + section + "/";
                    }
                }
            }
            else
            {
                if (degACR != 0)
                {
                    batYrSemHead = "Degree/";
                    batYrSemCont = deg + "/";
                }
                if (year != 0)
                {
                    batYrSemHead += "Yr/";
                    batYrSemCont += " " + romanLetter(returnYearforSem(cursem)) + "/";

                }
                if (semester != 0)
                {
                    batYrSemHead += "Sem";
                    batYrSemCont += " " + romanLetter(cursem);
                    if (section.Trim() != string.Empty)
                    {
                        batYrSemCont += "-" + section;
                    }
                }
            }
            batYrSemHead = batYrSemHead.TrimEnd('/');
            batYrSemCont = batYrSemCont.TrimEnd('/');

            if (batYrSemHead != "")
            {
                tableparts.Cell(rowIn, colIn).SetContent(batYrSemHead + " : " + batYrSemCont);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + batYrSemCont);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }
            if (seattype != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("Seat Type: " + seatty);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + seatty);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }
            if (colIn == 6)
            {
                colIn = 0;
                rowIn++;
            }
            if (boarding != 0)
            {
                tableparts.Cell(rowIn, colIn).SetContent("Boarding : " + board);
                tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowIn, colIn).ColSpan = 2;
                colIn++;
                //tableparts.Cell(rowIn, colIn).SetContent(": " + board);
                //tableparts.Cell(rowIn, colIn).SetFont(FontTableHead);
                //tableparts.Cell(rowIn, colIn).SetContentAlignment(ContentAlignment.MiddleLeft);
                colIn++;
            }

            curX = 15;
            curY += 1;
            PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 600, 200));
            rcptpage.Add(addtabletopage1);

            #endregion

            #region Table 2
            //Table2 Format

            int rows = 1;

            string semyear = "";
            if (ddl_semrcpt.Items.Count > 0)
            {
                semyear = Convert.ToString(ddl_semrcpt.SelectedItem.Value);
            }
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

                selectQuery = " SELECT isnull(sum(BalAmount),0) as BalAmount FROM FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and l.LedgerMode=0  and T.TextCode in('" + semyear + "') AND A.App_No = " + appnoNew + "   and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";

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

            Hashtable htIndex = new Hashtable();
            int hInsdx = 3;

            //Table2 Header

            int descWidth = 440;

            if (semOrYear != 0)
            {

                htIndex.Add("semOrYear", hInsdx);
                hInsdx++;
                descWidth -= 60;
            }


            if (allotedAmt != 0)
            {

                htIndex.Add("allotedAmt", hInsdx);
                hInsdx++;
                descWidth -= 60;
            }

            if (balAmt != 0)
            {

                htIndex.Add("balAmt", hInsdx);
                hInsdx++;
                descWidth -= 60;
            }
            if (prevPaidAmt != 0)
            {

                htIndex.Add("prevPaidAmt", hInsdx);
                hInsdx++;
                descWidth -= 70;

            }

            if (concession != 0)
            {

                htIndex.Add("concession", hInsdx);
                hInsdx++;
                descWidth -= 60;
            }

            PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, htIndex.Count + 3, 5);
            tableparts1.VisibleHeaders = false;
            tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

            tableparts1.Cell(0, 0).SetContent("S.No");
            tableparts1.Cell(0, 0).SetFont(FontTableHead);
            tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            tableparts1.Columns[0].SetWidth(20);

            tableparts1.Cell(0, 1).SetContent("Description");
            tableparts1.Cell(0, 1).SetFont(FontTableHead);
            tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
            tableparts1.Columns[1].SetWidth(descWidth);

            tableparts1.Cell(0, 2).SetContent("Paid Rs");
            tableparts1.Cell(0, 2).SetFont(FontTableHead);
            tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
            tableparts1.Columns[2].SetWidth(40);

            //Table2 Data

            int sno = 0;
            int indx = 0;
            double totalamt = 0;
            double balanamt = 0;
            double curpaid = 0;
            double paidamount = 0;

            #region Insert Process New

            //For Every Selected Headers

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
                if (dsLedgers.Tables.Count > 0)
                {
                    if (dsLedgers.Tables[0].Rows.Count > 0)
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
                            if (pl == pagelength)
                            {
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
                            }

                            #endregion

                        }

                    }
                }



                if (creditAmt0 > 0)
                {
                    sno++;
                    indx++;
                    totalamt += Convert.ToDouble(totalAmt0);
                    balanamt += Convert.ToDouble(balAmt0);
                    curpaid += Convert.ToDouble(paidAmt0);

                    deductionamt += Convert.ToDouble(deductAmt0);

                    tableparts1.Cell(indx, 0).SetContent(sno);
                    tableparts1.Cell(indx, 0).SetFont(FontTablebody);
                    tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                    tableparts1.Cell(indx, 1).SetContent(disphdr);
                    tableparts1.Cell(indx, 1).SetFont(FontTablebody);
                    tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                    tableparts1.Cell(indx, 2).SetContent(creditAmt0);
                    tableparts1.Cell(indx, 2).SetFont(FontTablebody);
                    tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);

                    if (semOrYear != 0)
                    {
                        if (htIndex.Contains("semOrYear"))
                        {
                            int ind = Convert.ToInt32(htIndex["semOrYear"]);
                            tableparts1.Cell(indx, ind).SetContent(Convert.ToString(ddl_semrcpt.SelectedItem.Text));
                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                            if (indx == 1)
                            {
                                tableparts1.Cell(0, ind).SetContent("Category");
                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tableparts1.Columns[ind].SetWidth(50);
                            }
                        }

                        // htIndex.Add("semOrYear", hInsdx);
                    }


                    if (allotedAmt != 0)
                    {
                        if (htIndex.Contains("allotedAmt"))
                        {
                            int ind = Convert.ToInt32(htIndex["allotedAmt"]);
                            tableparts1.Cell(indx, ind).SetContent(totalAmt0);
                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                            if (indx == 1)
                            {
                                tableparts1.Cell(0, ind).SetContent("Fixed Fee Rs");
                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tableparts1.Columns[ind].SetWidth(50);
                            }
                        }
                    }

                    if (balAmt != 0)
                    {
                        if (htIndex.Contains("balAmt"))
                        {
                            int ind = Convert.ToInt32(htIndex["balAmt"]);
                            tableparts1.Cell(indx, ind).SetContent(balAmt0);
                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                            if (indx == 1)
                            {
                                tableparts1.Cell(0, ind).SetContent("Balance Rs");
                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tableparts1.Columns[ind].SetWidth(50);
                            }
                        }
                    }
                    if (prevPaidAmt != 0)
                    {
                        if (htIndex.Contains("prevPaidAmt"))
                        {
                            int ind = Convert.ToInt32(htIndex["prevPaidAmt"]);
                            tableparts1.Cell(indx, ind).SetContent(alreadyPaid);
                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                            if (indx == 1)
                            {
                                tableparts1.Cell(0, ind).SetContent("Already Paid Rs");
                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tableparts1.Columns[ind].SetWidth(70);
                            }
                        }

                    }

                    if (concession != 0)
                    {
                        if (htIndex.Contains("concession"))
                        {
                            int ind = Convert.ToInt32(htIndex["concession"]);
                            tableparts1.Cell(indx, ind).SetContent(deductAmt0);
                            tableparts1.Cell(indx, ind).SetFont(FontTablebody);
                            tableparts1.Cell(indx, ind).SetContentAlignment(ContentAlignment.MiddleRight);
                            if (indx == 1)
                            {
                                tableparts1.Cell(0, ind).SetContent("Deduction Rs");
                                tableparts1.Cell(0, ind).SetFont(FontTableHead);
                                tableparts1.Cell(0, ind).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tableparts1.Columns[ind].SetWidth(50);
                            }
                        }

                    }

                    createPDFOK = true;
                }

            }
            #endregion

            curY += 5 + (int)addtabletopage1.Area.Height;
            PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 600));
            rcptpage.Add(addtabletopage2);
            #endregion

            #region Table 3
            //Table3 Format
            PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 1, 8, 5);
            tableparts2.VisibleHeaders = false;
            tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

            //Table3 Header              
            decimal totalamount = (decimal)curpaid;
            overallCashAmt += Convert.ToDouble(totalamount);
            tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
            tableparts2.Cell(0, 0).SetFont(FontTableHead);
            tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
            tableparts2.Cell(0, 0).ColSpan = 4;

            tableparts2.Cell(0, 4).SetContent("Total");
            tableparts2.Cell(0, 4).SetFont(FontTableHead);
            tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

            double exAmt = 0;
            double.TryParse(Convert.ToString(excessRemaining(appnoNew, recptNo)), out exAmt);
            curpaid += exAmt;

            tableparts2.Cell(0, 5).SetContent("Rs. " + curpaid + "/-");
            tableparts2.Cell(0, 5).SetFont(FontTableHead);
            tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

            tableparts2.Cell(0, 6).SetContent("Balance");
            tableparts2.Cell(0, 6).SetFont(FontTableHead);
            tableparts2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
            if (BalanceType == 1)
            {
                balanamt = retBalance(appnoNew, BalanceType);
            }
            tableparts2.Cell(0, 7).SetContent("Rs. " + balanamt + "/-");
            tableparts2.Cell(0, 7).SetFont(FontTableHead);
            tableparts2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);


            curY += (int)addtabletopage2.Area.Height + 5;
            PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 30, curY, 520, 50));
            rcptpage.Add(addtabletopage3);
            #endregion

            #region Receipt Footer

            string ddnar = string.Empty;
            double modeht = 40;
            if (narration != 0)
            {
                if (!rb_cash.Checked)
                {
                    if (rb_dd.Checked == true)
                    {
                        ddnar = ddnar + "\n\nDDNo : " + checkDDno + " Bank : " + newbankname + "\n\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                    }
                    else if (rb_cheque.Checked)
                    {
                        ddnar = ddnar + "\n\nChequeNo : " + checkDDno + " Bank : " + newbankname + "\n\nBranch :" + branch + " Date  : " + txt_date1.Text.ToString();
                    }
                    else if (rb_card.Checked)
                    {
                        ddnar = ddnar + "\n\nCard : " + newbankname;
                    }
                    //Added by saranya on 24/04/2018
                    else if (rb_NEFT.Checked == true)
                    {
                        ddnar = ddnar + "\n\nNeftNo : " + checkDDno;
                    }

                }
                ddnar += "\n\n" + txt_remark.Text.Trim();
            }

            //Mode of Pay

            curY += 5 + (int)addtabletopage3.Area.Height;
            if (ddnar.Trim() != "")
            {
                curY += 5;
            }
            PdfTextArea modeofpayText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 30, curY, 250, modeht), ContentAlignment.MiddleLeft, "Mode of Pay : " + mode + ddnar);
            rcptpage.Add(modeofpayText);

            if (deduction != 0)
            {
                PdfTextArea deducText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 130, curY, 200, 20), ContentAlignment.MiddleCenter, "Deduction Amount Rs. : " + deductionamt);
                rcptpage.Add(deducText);
            }
            if (excessAmt != 0)
            {
                PdfTextArea exText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 240, curY, 200, 20), ContentAlignment.MiddleCenter, "Excess Amount Rs. : " + excessRemaining(appnoNew, recptNo).ToString());
                rcptpage.Add(exText);
            }
            if (validDate != 0)
            {
                PdfTextArea valdtText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 370, curY, 200, 20), ContentAlignment.MiddleCenter, "Valid upto : " + "(" + recptValid + ")");
                rcptpage.Add(valdtText);
            }


            //Authorizer
            if (forclgName != 0)
            {
                curY += 15;
                PdfTextArea authorizeText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 350, curY, 250, 20), ContentAlignment.MiddleCenter, "For " + collegename);
                rcptpage.Add(authorizeText);
            }

            if (authSignValue.Trim() != "")
            {
                curY += 15;
                PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, authSignValue);
                rcptpage.Add(authorizeSignText);
            }
            else
            {
                curY += 15;
                PdfTextArea authorizeSignText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Authorised Signature");
                rcptpage.Add(authorizeSignText);
            }

            PdfPage rcptpageOf = rcptpage.CreateCopy();


            if (transCopy != 0 && pl == 1)
            {
                int cuyy = curY;
                //if (authSign == 0)
                //{
                cuyy += 10;
                // }
                rcptpageTran = rcptpage.CreateCopy();
                PdfTextArea transCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, cuyy, 150, 20), ContentAlignment.MiddleCenter, "Transport Copy ");
                rcptpageTran.Add(transCopyText);


            }


            if (studCopy != 0 || studOffiCopy == 1)
            {
                //if (authSign == 0)
                //{
                curY += 10;
                //}
                string copy = "Student Copy ";
                if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    copy = "Staff Copy ";
                }
                else if (rbl_rollnoNew.SelectedIndex == 2)
                {
                    copy = "Vendor Copy ";
                }
                if (pl == 2)
                    copy = "Office Copy ";
                PdfTextArea studCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, copy);
                rcptpage.Add(studCopyText);
                if (pl == pagelength)
                {
                    rcptpage.SaveToDocument();
                }
            }

            //save changes

            if (pl == pagelength)
            {
                if (officopy != 0 && studOffiCopy != 1)
                {
                    PdfTextArea offCopyText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 450, curY, 150, 20), ContentAlignment.MiddleCenter, "Office Copy ");
                    rcptpageOf.Add(offCopyText);
                    rcptpageOf.SaveToDocument();

                }

            }

            if (transCopy != 0 && pl == pagelength)
            {
                rcptpageTran.SaveToDocument();
            }

            curY += 10;

            #endregion
        }
        #endregion
        // return string.Empty;
    }



}