using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Gios.Pdf;
using System.Drawing;
using System.Text;
public class FormatXISanAcademyChallanReceipt : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
	public FormatXISanAcademyChallanReceipt()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
    }
     //Original Receipt
    public string generateOriginal(string txt_rcptno, string txt_date, string txt_dept, CheckBox rb_cash, CheckBox rb_cheque, CheckBox rb_dd, CheckBox rb_card,CheckBox rb_NEFT, string collegecode1, string usercode, ref string lastRecptNo, ref string accidRecpt, RadioButtonList rbl_rollnoNew, DropDownList rbl_rollno, string appnoNew, string outRoll, TextBox txtDept_staff, string rollno, string app_formno, string Regno, string studname, GridView grid_Details, byte BalanceType, DataTable dtMulBnkDetails, CheckBox chk_rcptMulmode, string modeMulti, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, ref bool contentVisible, ref bool CreateReceiptOK, ref bool imgDIVVisible, ref Label lbl_alert, CheckBox cb_CautionDep, CheckBox cb_govt, CheckBox cb_exfees, string mode, TextBox txt_ddno, DropDownList ddl_bkname, TextBox txt_chqno, DataSet dsPri)
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
        if (rbl_rollnoNew.SelectedIndex == 1)
        {
            course = txtDept_staff.Text.Trim();
        }
        string batchYrSem = string.Empty;
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
        }
        else if (rb_dd.Checked)
        {
            mode = "DD";
        }
        else if (rb_card.Checked)
        {
            mode = "Card";
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
                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                //Footer Div Values

                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);

                //Document Settings
                PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(18, 15.2));
                PdfPage rcptpage = recptDoc.NewPage();
                Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                Font FontTableHead = new Font("Arial", 10, FontStyle.Bold);
                Font FontTable = new Font("Arial", 10, FontStyle.Bold);

                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3,phoneno from collinfo where college_code=" + collegecode1 + "  ";
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
                string collegename = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string univ = "";
                string phone = "";
                string deg = "";
                string cursem = "";
                string batyr = "";
                string seatty = "";
                string board = "";
                string mothe = "";
                string fathe = "";
                double deductionamt = 0;
                string fgraduate = d2.GetFunction("select isnull(first_graduate,0) as first_graduate  from applyn where app_no='" + appnoNew + "'");
                if (fgraduate == "0")
                {
                    fgraduate = string.Empty;
                }
                else
                {
                    fgraduate = " FG ";
                }

                string sec = string.Empty;
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
                        phone = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        if (phone.Trim() != "")
                        {
                            phone = "Phone : " + phone;
                        }
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
                            sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
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
                        fgraduate = "";
                    }
                }

                PdfArea rectArea = new PdfArea(recptDoc, 118, 35, 963, 570);
                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                rcptpage.Add(rectSpace);

                //Header Images
                //Line1
                string leftImg = "";
                if (leftLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                    {
                        PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                        rcptpage.Add(LogoImage, 125, 40, 450);
                        leftImg = "<img src='" + "../FinanceLogo/left_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                if (collegeid != 0)
                {
                    PdfTextArea clgText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 245, 45, 350, 20), ContentAlignment.MiddleCenter, collegename);
                    rcptpage.Add(clgText);
                }
                string rghtimg = "";
                if (rightLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                    {

                        PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                        rcptpage.Add(LogoImage1, 550, 40, 450);
                        rghtimg = "<img src='" + "../FInanceLogo/right_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                //Line2
                if (university != 0)
                {
                    PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 65, 350, 20), ContentAlignment.MiddleCenter, univ);
                    rcptpage.Add(uniText);
                }
                //Line3
                string jaiadd1 = "";
                if (address1 != 0 || address2 != 0)
                {
                    if (address2 != 0)
                    {
                        jaiadd1 = add1 + " " + add2;
                    }
                    PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 80, 350, 20), ContentAlignment.MiddleCenter, add1);
                    rcptpage.Add(addText);
                }
                //Line4
                if (address3 != 0)
                {
                    PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 95, 350, 20), ContentAlignment.MiddleCenter, add3);
                    rcptpage.Add(cityText);
                }


                #region Table 1
                //Table1 Format 

                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 2);
                tableparts.VisibleHeaders = false;

                tableparts.Rows[0].SetRowHeight(10);
                tableparts.Rows[1].SetRowHeight(30);
                tableparts.Rows[2].SetRowHeight(20);
                tableparts.Rows[3].SetRowHeight(20);
                tableparts.Rows[4].SetRowHeight(10);
                tableparts.Rows[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                //Table1 Data
                //Line 1
                int rowindextbl1 = 0;

                tableparts.Cell(rowindextbl1, 0).SetContent("");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts.Rows[rowindextbl1].SetRowHeight(10);
                rowindextbl1++;
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("Roll No");
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("Staff Id");
                }
                else if (rbl_rollnoNew.SelectedIndex == 2)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("Vendor Id");
                }
                else if (rbl_rollnoNew.SelectedIndex == 3)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("");
                }

                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).SetContent(": " + outRoll);
                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 2;

                tableparts.Cell(rowindextbl1, 4).SetContent("Receipt No");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptNo);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line2
                rowindextbl1++;
                tableparts.Cell(rowindextbl1, 0).SetContent("Name");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(rowindextbl1, 1).SetContent(": " + studname.ToUpper());
                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                tableparts.Cell(rowindextbl1, 4).SetContent("Date");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptDt);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line3
                rowindextbl1++;

                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("Year/ Major");
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    tableparts.Cell(rowindextbl1, 0).SetContent("Dept");
                }
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    tableparts.Cell(rowindextbl1, 1).SetContent(": " + romanLetter(returnYearforSem(cursem)) + " / " + deg.Split('-')[1].ToUpper() + sec.ToUpper() + fgraduate.ToUpper());
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    tableparts.Cell(rowindextbl1, 1).SetContent(": " + deg.ToUpper());
                }

                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                tableparts.Cell(rowindextbl1, 4).SetContent("Term");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 5).SetContent(": " + acaYear);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                rowindextbl1++;
                tableparts.Cell(rowindextbl1, 0).SetContent("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts.Rows[rowindextbl1].SetRowHeight(10);


                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 10, 10, 480, 150));
                rcptpage.Add(addtabletopage1);

                #endregion


                if (leftImg != "" && rghtimg == "")
                {
                    rghtimg = "<div style='width:80px;height:80px;'></div>";
                }
                StringBuilder sbHtml = new StringBuilder();
                sbHtml.Append("<div style='height: 575px;width:963px;padding-left:114px;'>");
                sbHtml.Append("<table cellpadding='0' cellspacing='0' style='text-align:center; width: 745px;font-size:Arial; ' class='classBold10'><tr><td rowspan='4'>" + leftImg + "</td><td colspan='7' style='text-align:center; font-family:Old English Text MT; font-weight:bold; font-size:18px;'>" + collegename + "</td><td rowspan='4'>" + rghtimg + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + jaiadd1 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + add3 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + phone + "</td></tr></table>");
                sbHtml.Append("<br><table cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td>Roll No</td><td colspan='2'>:&nbsp;" + rollno + "</td><td></td><td>Receipt No</td><td>:&nbsp;" + recptNo + "</td></tr><tr><td>Name</td><td colspan='3'>:&nbsp;" + studname.ToUpper() + "</td><td>Date</td><td>:&nbsp;" + recptDt + "</td><td></td></tr><tr><td>Year/ Major</td><td colspan='3'>:&nbsp;" + romanLetter(returnYearforSem(cursem)) + "</td><td>Term</td><td>:&nbsp;" + acaYear + "</td><td></td></tr></table><br>");

                #region Table 2
                //Table2 Format


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

                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows + 5, 4, 1);
                //tableparts1.SetBorders(Color.Black, 1, BorderType.Rows);
                tableparts1.VisibleHeaders = false;
                tableparts1.Columns[0].SetWidth(57);
                tableparts1.Columns[1].SetWidth(340);
                tableparts1.Columns[2].SetWidth(85);
                tableparts1.Columns[3].SetWidth(28);

                tableparts1.Cell(0, 0).SetContent("S.No");
                tableparts1.Cell(0, 0).SetFont(FontTable);
                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                tableparts1.Cell(0, 1).SetContent("Particulars");
                tableparts1.Cell(0, 1).SetFont(FontTable);
                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                //tableparts1.Cell(indx, 1).ColSpan = 4;

                tableparts1.Cell(0, 2).SetContent("Rs.");
                tableparts1.Cell(0, 2).SetFont(FontTable);
                tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts1.Cell(0, 3).SetContent("Ps.");
                tableparts1.Cell(0, 3).SetFont(FontTable);
                tableparts1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[0].SetRowHeight(20);
                tableparts1.Rows[1].SetRowHeight(10);
                //Table2 Data

                #region feedata
                int sno = 0;
                int indx = 1;
                double totalamt = 0;
                double balanamt = 0;
                double curpaid = 0;
                foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                {
                    pr.ColSpan = 4;
                }

                tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts1.Cell(indx, 0).SetFont(FontTable);
                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[indx].SetRowHeight(10);
                indx++;
                sbHtml.Append("<table Rules='Rows' cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td style='text-align:center;'>S.No</td><td colspan='5'  style='text-align:left;'>Particulars</td><td  style='text-align:right;'>Rs.</td><td  style='text-align:right;'>Ps.</td></tr>");
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

                        tableparts1.Cell(indx, 0).SetContent(sno);
                        tableparts1.Cell(indx, 0).SetFont(FontTable);
                        tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                        tableparts1.Cell(indx, 1).SetContent(lblFeeCategory.Text);
                        tableparts1.Cell(indx, 1).SetFont(FontTable);
                        tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        //tableparts1.Cell(indx, 1).ColSpan = 4;

                        tableparts1.Cell(indx, 2).SetContent(returnIntegerPart(creditamt));
                        tableparts1.Cell(indx, 2).SetFont(FontTable);
                        tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        tableparts1.Cell(indx, 3).SetContent(returnDecimalPart(creditamt));
                        tableparts1.Cell(indx, 3).SetFont(FontTable);
                        tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        indx++;
                        sbHtml.Append("<tr><td style='text-align:center;'>" + sno + "</td><td colspan='5'  style='text-align:left;'>" + lblFeeCategory.Text + "</td><td  style='text-align:right;'>" + returnIntegerPart(creditamt) + "</td><td  style='text-align:right;'>" + returnDecimalPart(creditamt) + "</td></tr>");
                    }
                }



                foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                {
                    pr.ColSpan = 4;
                }

                tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts1.Cell(indx, 0).SetFont(FontTable);
                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[indx].SetRowHeight(10);
                indx++;
                decimal totalamount = (decimal)curpaid;
                tableparts1.Cell(indx, 1).SetContent("Total");
                tableparts1.Cell(indx, 1).SetFont(FontTable);
                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts1.Cell(indx, 2).SetContent("" + returnIntegerPart((double)totalamount));
                tableparts1.Cell(indx, 2).SetFont(FontTable);
                tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts1.Cell(indx, 3).SetContent(returnDecimalPart((double)totalamount));
                tableparts1.Cell(indx, 3).SetFont(FontTable);
                tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                string endstatement = "<br>" + DecimalToWords(totalamount) + " Rupees Only." + "<br>Paid by " + mode + " Rs." + totalamount.ToString() + "/-.";
                string finalstrig = "";

                finalstrig = "<br>Excess Amount  : " + excessRemaining(appnoNew).ToString();
                if (rb_dd.Checked == true)
                {
                    finalstrig = finalstrig + "<br>" + mode + " : " + txt_ddno.Text.ToString() + "         Date  : " + txt_date1.Text.ToString();
                    finalstrig = finalstrig + "<br>Bank Name  : " + ddl_bkname.SelectedItem.Text.ToString();
                }
                if (rb_cheque.Checked == true)
                {
                    finalstrig = "<br>" + mode + " : " + txt_chqno.Text.ToString() + "         Date  : " + txt_date1.Text.ToString();
                    finalstrig = finalstrig + "<br>Bank Name  : " + ddl_bkname.SelectedItem.Text.ToString();
                }
                if (rb_card.Checked == true)
                {
                    finalstrig = "<br>" + mode + " : " + newbankname;
                }
                if (txt_remark.Text.Trim() != string.Empty)
                {
                    finalstrig = finalstrig + "<br>Remarks : " + txt_remark.Text.Trim();
                }
                endstatement = endstatement + finalstrig;

                tableparts1.Cell(indx + 1, 0).SetContent(endstatement);
                tableparts1.Cell(indx + 1, 0).SetFont(FontTable);
                tableparts1.Cell(indx + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts1.Cell(indx + 1, 0).ColSpan = 3;

                #endregion

                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 10, 80, 480, 500));
                rcptpage.Add(addtabletopage2);
                sbHtml.Append("<tr><td colspan='6'  style='text-align:center;'>Total</td><td  style='text-align:right;'>" + returnIntegerPart((double)totalamount) + "</td><td  style='text-align:right;'>" + returnDecimalPart((double)totalamount) + "</td></tr><tr><td colspan='8'  style='text-align:left;'>" + endstatement + "</td></tr>");

                #endregion


                rcptpage.SaveToDocument();

                //save changes
                PdfPage rcptpageOf = rcptpage.CreateCopy();
                PdfPage rcptpageTran = rcptpage.CreateCopy();
                StringBuilder sboffCopy = new StringBuilder();
                StringBuilder sbtranCopy = new StringBuilder();
                if (officopy != 0)
                {
                    rcptpageOf.SaveToDocument();
                    sboffCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Office Copy</td></tr></table></div><br>");
                }

                if (transCopy != 0)
                {
                    sbtranCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Transport Copy</td></tr></table></div><br>");
                    rcptpageTran.SaveToDocument();
                }

                sbHtml.Append("<tr><td colspan='8'  style='text-align:left;'>Student Copy</td></tr></table></div><br>");
                sbHtml.Append(sboffCopy.ToString() + sbtranCopy.ToString());
                contentDiv.Append( sbHtml.ToString());
                sbHtml.Clear();

                //Print

                contentVisible = true;
                CreateReceiptOK = true;
                return contentDiv.ToString();
            }

        }

        return string.Empty;
    }
    //Multiple Receipt
    public string generateMultiple(DataSet dsPri, string collegecode1, string appnoNew, string section, ref PdfDocument recptDoc, ref PdfPage rcptpage, string recptNo, string studname, string recptDt, string Regno, string rollno, string app_formno, RadioButton rb_cash, RadioButton rb_dd, RadioButton rb_cheque, RadioButton rb_card, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, string mode, RadioButtonList rbl_rollnoNew, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, Label lbltype, RadioButton rdo_receipt, RadioButton rdo_sngle, string PayMode, DateTime dtrcpt, string memtype, string receiptno, string dtchkdd, string newbankcode, string usercode, string finYearid, int rcptType, bool InsertUpdateOK, ref bool createPDFOK, byte BalanceType, ref double overallCashAmt, string course,TextBox txt_ddno,DropDownList ddl_bkname,TextBox txt_chqno)
    {
        StringBuilder contentDiv = new StringBuilder();

        Font FontboldheadC = new Font("Arial", 15, FontStyle.Bold);
        Font Fontboldhead = new Font("Arial", 13, FontStyle.Bold);
        Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
        Font FontTablebody = new Font("Arial", 8, FontStyle.Regular);
        Font FontTable = new Font("Arial", 8, FontStyle.Bold);
        Font tamilFont = new Font("AMUDHAM.TTF", 8, FontStyle.Regular);

        //For San Academy
        #region Print Option For Receipt

        //Basic Data

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
        }
        else if (rb_dd.Checked)
        {
            mode = "DD";
        }
        else if (rb_card.Checked)
        {
            mode = "Card";
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
                byte collegeid = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                byte address1 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd1"]);
                byte address2 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd2"]);
                byte address3 = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeAdd3"]);
                byte city = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeDist"]);
                byte state = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeState"]);

                byte university = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeUniversity"]);
                byte rightLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsRightLogo"]);
                byte leftLogo = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsLeftLogo"]);
                //Footer Div Values

                byte studCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(ds.Tables[0].Rows[0]["IsTransportCopy"]);

                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3,phoneno from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + "";
                string collegename = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string phone = "";
                string univ = "";
                string deg = "";
                string cursem = "";
                string batyr = "";
                string seatty = "";
                string board = "";
                string mothe = "";
                string fathe = "";
                double deductionamt = 0;
                string fgraduate = d2.GetFunction("select isnull(first_graduate,0) as first_graduate  from applyn where app_no='" + appnoNew + "'");
                if (fgraduate == "0")
                {
                    fgraduate = string.Empty;
                }
                else
                {
                    fgraduate = " FG ";
                }
                string sec = string.Empty;
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
                        phone = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        if (phone.Trim() != "")
                        {
                            phone = "Phone : " + phone;
                        }
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
                        sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                    }
                }

                PdfArea rectArea = new PdfArea(recptDoc, 118, 35, 963, 570);
                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                rcptpage.Add(rectSpace);

                //Header Images
                //Line1
                string leftImg = "";
                if (leftLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg")))
                    {
                        PdfImage LogoImage = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collegecode1 + ".jpeg"));
                        rcptpage.Add(LogoImage, 125, 40, 450);
                        leftImg = "<img src='" + "../FinanceLogo/left_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                if (collegeid != 0)
                {
                    PdfTextArea clgText = new PdfTextArea(FontboldheadC, Color.Black, new PdfArea(recptDoc, 245, 45, 350, 20), ContentAlignment.MiddleCenter, collegename);
                    rcptpage.Add(clgText);
                }
                string rghtimg = "";
                if (rightLogo != 0)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg")))
                    {

                        PdfImage LogoImage1 = recptDoc.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collegecode1 + ".jpeg"));
                        rcptpage.Add(LogoImage1, 550, 40, 450);
                        rghtimg = "<img src='" + "../FinanceLogo/right_Logo" + collegecode1 + ".jpeg?" + DateTime.Now.Ticks.ToString() + "" + "' style='height:80px; width:80px;'/>";
                    }
                }
                //Line2
                if (university != 0)
                {
                    PdfTextArea uniText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 65, 350, 20), ContentAlignment.MiddleCenter, univ);
                    rcptpage.Add(uniText);
                }
                //Line3
                string jaiadd1 = "";
                if (address1 != 0 || address2 != 0)
                {
                    if (address2 != 0)
                    {
                        jaiadd1 = add1 + " " + add2;
                    }
                    PdfTextArea addText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 80, 350, 20), ContentAlignment.MiddleCenter, add1);
                    rcptpage.Add(addText);
                }
                //Line4
                if (address3 != 0)
                {
                    PdfTextArea cityText = new PdfTextArea(FontTableHead, Color.Black, new PdfArea(recptDoc, 245, 95, 350, 20), ContentAlignment.MiddleCenter, add3);
                    rcptpage.Add(cityText);
                }


                #region Table 1
                //Table1 Format 

                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 2);
                tableparts.VisibleHeaders = false;

                tableparts.Rows[0].SetRowHeight(10);
                tableparts.Rows[1].SetRowHeight(30);
                tableparts.Rows[2].SetRowHeight(20);
                tableparts.Rows[3].SetRowHeight(20);
                tableparts.Rows[4].SetRowHeight(10);
                tableparts.Rows[0].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[1].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[2].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[3].SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Rows[4].SetContentAlignment(ContentAlignment.MiddleLeft);
                //Table1 Data
                //Line 1
                int rowindextbl1 = 0;

                tableparts.Cell(rowindextbl1, 0).SetContent("");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts.Rows[rowindextbl1].SetRowHeight(10);
                rowindextbl1++;

                tableparts.Cell(rowindextbl1, 0).SetContent("Roll No");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).SetContent(": " + rollno);
                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 2;

                tableparts.Cell(rowindextbl1, 4).SetContent("Receipt No");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptNo);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line2
                rowindextbl1++;
                tableparts.Cell(rowindextbl1, 0).SetContent("Name");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(rowindextbl1, 1).SetContent(": " + studname.ToUpper());
                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                tableparts.Cell(rowindextbl1, 4).SetContent("Date");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(rowindextbl1, 5).SetContent(": " + recptDt);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line3
                rowindextbl1++;

                tableparts.Cell(rowindextbl1, 0).SetContent("Year/ Major");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(rowindextbl1, 1).SetContent(": " + romanLetter(returnYearforSem(cursem)) + " / " + deg.Split('-')[1].ToUpper() + sec.ToUpper() + fgraduate.ToUpper());
                tableparts.Cell(rowindextbl1, 1).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 1).ColSpan = 3;

                tableparts.Cell(rowindextbl1, 4).SetContent("Term");
                tableparts.Cell(rowindextbl1, 4).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(rowindextbl1, 5).SetContent(": " + acaYear);
                tableparts.Cell(rowindextbl1, 5).SetFont(FontTableHead);
                tableparts.Cell(rowindextbl1, 5).SetContentAlignment(ContentAlignment.MiddleLeft);

                rowindextbl1++;
                tableparts.Cell(rowindextbl1, 0).SetContent("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts.Cell(rowindextbl1, 0).SetFont(FontTable);
                tableparts.Cell(rowindextbl1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts.Rows[rowindextbl1].SetRowHeight(10);


                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 10, 10, 480, 150));
                rcptpage.Add(addtabletopage1);

                #endregion
                StringBuilder sbHtml = new StringBuilder();
                if (leftImg != "" && rghtimg == "")
                {
                    rghtimg = "<div style='width:80px;height:80px;'></div>";
                }
                sbHtml.Append("<div style='height: 575px;width:963px;padding-left:114px;'>");
                sbHtml.Append("<table cellpadding='0' cellspacing='0' style='text-align:center; width: 745px;font-size:Arial; ' class='classBold10'><tr><td rowspan='4'>" + leftImg + "</td><td colspan='7' style='text-align:center; font-family:Old English Text MT; font-weight:bold; font-size:18px;'>" + collegename + "</td><td rowspan='4'>" + rghtimg + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + jaiadd1 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + add3 + "</td></tr><tr><td colspan='7' style='text-align:center;font-weight:bold; font-size:12px;'>" + phone + "</td></tr></table>");
                sbHtml.Append("<br><table cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td>Roll No</td><td colspan='2'>:&nbsp;" + rollno + "</td><td></td><td>Receipt No</td><td>:&nbsp;" + recptNo + "</td></tr><tr><td>Name</td><td colspan='3'>:&nbsp;" + studname.ToUpper() + "</td><td>Date</td><td>:&nbsp;" + recptDt + "</td><td></td></tr><tr><td>Year/ Major</td><td colspan='3'>:&nbsp;" + romanLetter(returnYearforSem(cursem)) + "</td><td>Term</td><td>:&nbsp;" + acaYear + "</td><td></td></tr></table><br>");

                #region Table 2
                //Table2 Format

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

                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows + 5, 4, 1);
                //tableparts1.SetBorders(Color.Black, 1, BorderType.Rows);
                tableparts1.VisibleHeaders = false;
                tableparts1.Columns[0].SetWidth(57);
                tableparts1.Columns[1].SetWidth(340);
                tableparts1.Columns[2].SetWidth(85);
                tableparts1.Columns[3].SetWidth(28);

                tableparts1.Cell(0, 0).SetContent("S.No");
                tableparts1.Cell(0, 0).SetFont(FontTable);
                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                tableparts1.Cell(0, 1).SetContent("Particulars");
                tableparts1.Cell(0, 1).SetFont(FontTable);
                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                //tableparts1.Cell(indx, 1).ColSpan = 4;

                tableparts1.Cell(0, 2).SetContent("Rs.");
                tableparts1.Cell(0, 2).SetFont(FontTable);
                tableparts1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts1.Cell(0, 3).SetContent("Ps.");
                tableparts1.Cell(0, 3).SetFont(FontTable);
                tableparts1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[0].SetRowHeight(20);
                tableparts1.Rows[1].SetRowHeight(10);


                #region feedata

                int sno = 0;
                int indx = 1;
                double totalamt = 0;
                double balanamt = 0;
                double curpaid = 0;
                double paidamount = 0;
                foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                {
                    pr.ColSpan = 4;
                }

                tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts1.Cell(indx, 0).SetFont(FontTable);
                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[indx].SetRowHeight(10);
                indx++;
                #region Insert Process New

                //For Every Selected Headers
                sbHtml.Append("<table Rules='Rows' cellpadding='2' cellspacing='0' style='text-align:center;padding-top:5px; width: 745px;font-size:Arial;font-weight:bold; font-size:12px;text-align:left; border-width:1px;border-style:solid;' class='classBold10'><tr><td style='text-align:center;'>S.No</td><td colspan='5'  style='text-align:left;'>Particulars</td><td  style='text-align:right;'>Rs.</td><td  style='text-align:right;'>Ps.</td></tr>");
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

                        }
                    }

                    if (creditAmt0 > 0)
                    {
                        sno++;

                        totalamt += Convert.ToDouble(totalAmt0);
                        balanamt += Convert.ToDouble(balAmt0);
                        curpaid += Convert.ToDouble(paidAmt0);

                        deductionamt += Convert.ToDouble(deductAmt0);

                        tableparts1.Cell(indx, 0).SetContent(sno);
                        tableparts1.Cell(indx, 0).SetFont(FontTable);
                        tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);


                        tableparts1.Cell(indx, 1).SetContent(disphdr);
                        tableparts1.Cell(indx, 1).SetFont(FontTable);
                        tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        //tableparts1.Cell(indx, 1).ColSpan = 4;

                        tableparts1.Cell(indx, 2).SetContent(returnIntegerPart(creditAmt0));
                        tableparts1.Cell(indx, 2).SetFont(FontTable);
                        tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                        tableparts1.Cell(indx, 3).SetContent(returnDecimalPart(creditAmt0));
                        tableparts1.Cell(indx, 3).SetFont(FontTable);
                        tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        indx++;
                        createPDFOK = true;

                        sbHtml.Append("<tr><td style='text-align:center;'>" + sno + "</td><td colspan='5'  style='text-align:left;'>" + disphdr + "</td><td  style='text-align:right;'>" + returnIntegerPart(creditAmt0) + "</td><td  style='text-align:right;'>" + returnDecimalPart(creditAmt0) + "</td></tr>");
                    }

                }

                #endregion

                foreach (PdfCell pr in tableparts1.CellRange(indx, 0, indx, 0).Cells)
                {
                    pr.ColSpan = 4;
                }

                tableparts1.Cell(indx, 0).SetContent("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                tableparts1.Cell(indx, 0).SetFont(FontTable);
                tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Rows[indx].SetRowHeight(10);
                indx++;
                decimal totalamount = (decimal)curpaid;
                tableparts1.Cell(indx, 1).SetContent("Total");
                tableparts1.Cell(indx, 1).SetFont(FontTable);
                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts1.Cell(indx, 2).SetContent("" + returnIntegerPart((double)totalamount));
                tableparts1.Cell(indx, 2).SetFont(FontTable);
                tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts1.Cell(indx, 3).SetContent(returnDecimalPart((double)totalamount));
                tableparts1.Cell(indx, 3).SetFont(FontTable);
                tableparts1.Cell(indx, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                string endstatement = "<br>" + DecimalToWords(totalamount) + " Rupees Only." + "<br>Paid by " + mode + " Rs." + totalamount.ToString() + "/-.";
                string finalstrig = "";

                finalstrig = "<br>Excess Amount  : " + excessRemaining(appnoNew).ToString();

                if (rb_dd.Checked == true)
                {
                    finalstrig = finalstrig + "<br>" + mode + " : " + txt_ddno.Text.ToString() + "         Date  : " + txt_date1.Text.ToString();
                    finalstrig = finalstrig + "<br>Bank Name  : " + ddl_bkname.SelectedItem.Text.ToString();
                }
                if (rb_cheque.Checked == true)
                {
                    finalstrig = "<br>" + mode + " : " + txt_chqno.Text.ToString() + "         Date  : " + txt_date1.Text.ToString();
                    finalstrig = finalstrig + "<br>Bank Name  : " + ddl_bkname.SelectedItem.Text.ToString();
                }
                if (rb_card.Checked == true)
                {
                    finalstrig = "<br>" + mode + " : " + newbankname;
                }
                if (txt_remark.Text.Trim() != string.Empty)
                {
                    finalstrig = finalstrig + "<br>Remarks : " + txt_remark.Text.Trim();
                }
                endstatement = endstatement + finalstrig;


                tableparts1.Cell(indx + 1, 0).SetContent(endstatement);
                tableparts1.Cell(indx + 1, 0).SetFont(FontTable);
                tableparts1.Cell(indx + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts1.Cell(indx + 1, 0).ColSpan = 3;


                #endregion

                sbHtml.Append("<tr><td colspan='6'  style='text-align:center;'>Total</td><td  style='text-align:right;'>" + returnIntegerPart((double)totalamount) + "</td><td  style='text-align:right;'>" + returnDecimalPart((double)totalamount) + "</td></tr><tr><td colspan='8'  style='text-align:left;'>" + endstatement + "</td></tr>");

                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 10, 80, 480, 500));
                rcptpage.Add(addtabletopage2);


                #endregion

                rcptpage.SaveToDocument();

                //save changes
                PdfPage rcptpageOf = rcptpage.CreateCopy();
                PdfPage rcptpageTran = rcptpage.CreateCopy();

                StringBuilder sboffCopy = new StringBuilder();
                StringBuilder sbtranCopy = new StringBuilder();
                if (officopy != 0)
                {
                    sboffCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Office Copy</td></tr></table></div><br>");
                    rcptpageOf.SaveToDocument();
                }

                if (transCopy != 0)
                {
                    sbtranCopy.Append(sbHtml.ToString() + "<tr><td colspan='8'  style='text-align:left;'>Transport Copy</td></tr></table></div><br>");
                    rcptpageTran.SaveToDocument();
                }
                sbHtml.Append("<tr><td colspan='8'  style='text-align:left;'>Student Copy</td></tr></table></div><br>");
                sbHtml.Append(sboffCopy.ToString() + sbtranCopy.ToString());
                contentDiv.Append( sbHtml.ToString());
                sbHtml.Clear();

            }

        }

        #endregion
        return contentDiv.ToString();
    }
}