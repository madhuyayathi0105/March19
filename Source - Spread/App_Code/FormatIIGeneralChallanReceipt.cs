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

public class FormatIIGeneralChallanReceipt : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
    public FormatIIGeneralChallanReceipt()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    //Original Receipt
    public string generateOriginal(string txt_rcptno, string txt_date, string txt_dept, CheckBox rb_cash, CheckBox rb_cheque, CheckBox rb_dd, CheckBox rb_card, CheckBox rb_NEFT, string collegecode1, string usercode, ref string lastRecptNo, ref string accidRecpt, RadioButtonList rbl_rollnoNew, DropDownList rbl_rollno, string appnoNew, string outRoll, TextBox txtDept_staff, string rollno, string app_formno, string Regno, string studname, GridView grid_Details, byte BalanceType, DataTable dtMulBnkDetails, CheckBox chk_rcptMulmode, string modeMulti, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, ref bool contentVisible, ref bool CreateReceiptOK, ref bool imgDIVVisible, ref Label lbl_alert, CheckBox cb_CautionDep, CheckBox cb_govt, CheckBox cb_exfees, string mode)
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
        else if (rb_NEFT.Checked)
        {
            mode = "NEFT";
        }
        //Fields to print
        string queryPrint1 = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
        DataSet dsPri = new DataSet();
        dsPri = d2.select_method_wo_parameter(queryPrint1, "Text");
        if (dsPri.Tables.Count > 0)
        {
            if (dsPri.Tables[0].Rows.Count > 0)
            {

                //Footer Div Values
                byte degACR = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsDegreeAcr"]);
                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);
                byte StudOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);


                //Document Settings
                PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.A4);
                PdfPage rcptpage = recptDoc.NewPage();
                Font Fontboldhead = new Font("Arial", 10, FontStyle.Bold);
                Font FontTableHead = new Font("Arial", 7, FontStyle.Bold);
                Font FontTable = new Font("Arial", 7, FontStyle.Regular);
                Font tamilFont = new Font("AMUDHAM.TTF", 10, FontStyle.Regular);

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
                        if (rbl_rollnoNew.SelectedIndex == 0)
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
                            //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
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

                int curY = 130;
                int curX = 30;


                //Rectangle Border
                PdfArea rectArea = new PdfArea(recptDoc, 10, 10, 570, 800);
                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                rcptpage.Add(rectSpace);

                #region Table 1
                //Table1 Format 
                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 5);
                tableparts.VisibleHeaders = false;


                //Table1 Data
                //Line 1
                tableparts.Cell(0, 0).SetContent("Receipt No");
                tableparts.Cell(0, 0).SetFont(FontTableHead);
                tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 1).SetContent(": " + recptNo);
                tableparts.Cell(0, 1).SetFont(FontTableHead);
                tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 4).SetContent("Date");
                tableparts.Cell(0, 4).SetFont(FontTableHead);
                tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 5).SetContent(": " + recptDt);
                tableparts.Cell(0, 5).SetFont(FontTableHead);
                tableparts.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                //Line2
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    tableparts.Cell(1, 0).SetContent("Batch");
                    tableparts.Cell(1, 0).SetFont(FontTableHead);
                    tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                    tableparts.Cell(1, 1).SetContent(": " + batyr);
                    tableparts.Cell(1, 1).SetFont(FontTableHead);
                    tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {

                }
                else if (rbl_rollnoNew.SelectedIndex == 2)
                {

                }
                else
                {
                }


                tableparts.Cell(1, 2).SetContent("Degree / Branch");
                tableparts.Cell(1, 2).SetFont(FontTableHead);
                tableparts.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(1, 3).SetContent(": " + deg.ToUpper());
                tableparts.Cell(1, 3).SetFont(FontTableHead);
                tableparts.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line3
                if (rbl_rollnoNew.SelectedIndex == 0)
                {
                    tableparts.Cell(2, 0).SetContent("RollNo");
                }
                else if (rbl_rollnoNew.SelectedIndex == 1)
                {
                    tableparts.Cell(2, 0).SetContent("StaffId");
                }
                else if (rbl_rollnoNew.SelectedIndex == 2)
                {
                    tableparts.Cell(2, 0).SetContent("VendorId");
                }
                else if (rbl_rollnoNew.SelectedIndex == 3)
                {
                    tableparts.Cell(2, 0).SetContent("MobileNo");
                }
                tableparts.Cell(2, 0).SetFont(FontTableHead);
                tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(2, 1).SetContent(": " + rollno);
                tableparts.Cell(2, 1).SetFont(FontTableHead);
                tableparts.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(2, 2).SetContent("Name");
                tableparts.Cell(2, 2).SetFont(FontTableHead);
                tableparts.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(2, 3).SetContent(": " + studname.ToUpper());
                tableparts.Cell(2, 3).SetFont(FontTableHead);
                tableparts.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(2, 3).ColSpan = 3;

                //Line4
                tableparts.Cell(3, 0).SetContent("Fee For Academic Year");
                tableparts.Cell(3, 0).SetFont(FontTableHead);
                tableparts.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(3, 0).ColSpan = 2;

                tableparts.Cell(3, 2).SetContent(acaYear);
                tableparts.Cell(3, 2).SetFont(FontTableHead);
                tableparts.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(3, 4).SetContent("Type");
                tableparts.Cell(3, 4).SetFont(FontTableHead);
                tableparts.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(3, 5).SetContent(": " + mode);
                tableparts.Cell(3, 5).SetFont(FontTableHead);
                tableparts.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 500, 200));
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

                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 6, 5);
                tableparts1.VisibleHeaders = false;
                //tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                tableparts1.Cell(0, 0).SetContent("S.No");
                tableparts1.Cell(0, 0).SetFont(FontTableHead);
                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts1.Cell(0, 1).SetContent("Particulars");
                tableparts1.Cell(0, 1).SetFont(FontTableHead);
                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Cell(0, 1).ColSpan = 4;

                tableparts1.Cell(0, 5).SetContent("Amount (Rs)");
                tableparts1.Cell(0, 5).SetFont(FontTableHead);
                tableparts1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                //Table2 Data

                #region feedata
                int sno = 0;
                int indx = 0;
                double totalamt = 0;
                double balanamt = 0;
                double curpaid = 0;
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
                        indx++;
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
                        tableparts1.Cell(indx, 1).ColSpan = 4;

                        tableparts1.Cell(indx, 5).SetContent(creditamt);
                        tableparts1.Cell(indx, 5).SetFont(FontTable);
                        tableparts1.Cell(indx, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                    }
                }

                #endregion

                curY += 5 + (int)addtabletopage1.Area.Height;
                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 600));
                rcptpage.Add(addtabletopage2);

                #endregion

                #region Table 3
                //Table3 Format
                PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 4, 6, 5);
                tableparts2.VisibleHeaders = false;
                // tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                //Table3 Header              
                decimal totalamount = (decimal)curpaid;

                tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                tableparts2.Cell(0, 0).SetFont(FontTableHead);
                tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts2.Cell(0, 0).ColSpan = 4;

                tableparts2.Cell(0, 4).SetContent("Total Amount");
                tableparts2.Cell(0, 4).SetFont(FontTableHead);
                tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts2.Cell(0, 5).SetContent(curpaid);
                tableparts2.Cell(0, 5).SetFont(FontTableHead);
                tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts2.Cell(3, 3).SetContent("CASHIER / ACCOUNTANT");
                tableparts2.Cell(3, 3).SetFont(FontTableHead);
                tableparts2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts2.Cell(3, 3).ColSpan = 3;


                curY += 5 + (int)addtabletopage2.Area.Height;
                PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 100));
                rcptpage.Add(addtabletopage3);
                #endregion

                rcptpage.SaveToDocument();

                //save changes
                PdfPage rcptpageOf = rcptpage.CreateCopy();
                PdfPage rcptpageTran = rcptpage.CreateCopy();

                if (officopy != 0)
                {
                    rcptpageOf.SaveToDocument();
                }

                if (transCopy != 0)
                {
                    rcptpageTran.SaveToDocument();
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
                    //this.Form.DefaultButton = "btn_alertclose";
                    lbl_alert.Text = "Receipt Cannont Be Generated";
                }
            }

        }


        return string.Empty;
    }
    //Multiple Receipt
    public void generateMultiple(DataSet dsPri, string collegecode1, string appnoNew, string section, ref PdfDocument recptDoc, ref PdfPage rcptpage, string recptNo, string studname, string recptDt, string Regno, string rollno, string app_formno, RadioButton rb_cash, RadioButton rb_dd, RadioButton rb_cheque, RadioButton rb_card, RadioButton rb_NEFT, string checkDDno, string newbankname, string branch, TextBox txt_date1, TextBox txt_remark, string mode, RadioButtonList rbl_rollnoNew, DropDownList ddl_semrcpt, CheckBoxList cbl_grpheader, RadioButtonList rbl_headerselect, Label lbltype, RadioButton rdo_receipt, RadioButton rdo_sngle, string PayMode, DateTime dtrcpt, string memtype, string receiptno, string dtchkdd, string newbankcode, string usercode, string finYearid, int rcptType, bool InsertUpdateOK, ref bool createPDFOK, byte BalanceType, ref double overallCashAmt, string course)
    {

        Font FontboldheadC = new Font("Arial", 15, FontStyle.Bold);
        Font Fontboldhead = new Font("Arial", 13, FontStyle.Bold);
        Font FontTableHead = new Font("Arial", 8, FontStyle.Bold);
        Font FontTablebody = new Font("Arial", 8, FontStyle.Regular);
        Font FontTable = new Font("Arial", 8, FontStyle.Bold);
        Font tamilFont = new Font("AMUDHAM.TTF", 8, FontStyle.Regular);

        //For UIT
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

                //Footer Div Values

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
                        //    deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
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

                int curY = 130;
                int curX = 30;

                //Rectangle Border
                PdfArea rectArea = new PdfArea(recptDoc, 10, 10, 570, 800);
                PdfRectangle rectSpace = new PdfRectangle(recptDoc, rectArea, Color.Black);
                rcptpage.Add(rectSpace);

                #region Table 1
                //Table1 Format 
                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 5, 6, 5);
                tableparts.VisibleHeaders = false;


                //Table1 Data
                //Line 1
                tableparts.Cell(0, 0).SetContent("Receipt No");
                tableparts.Cell(0, 0).SetFont(FontTableHead);
                tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 1).SetContent(": " + recptNo);
                tableparts.Cell(0, 1).SetFont(FontTableHead);
                tableparts.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 4).SetContent("Date");
                tableparts.Cell(0, 4).SetFont(FontTableHead);
                tableparts.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(0, 5).SetContent(": " + recptDt);
                tableparts.Cell(0, 5).SetFont(FontTableHead);
                tableparts.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                //Line2
                tableparts.Cell(1, 0).SetContent("Batch");
                tableparts.Cell(1, 0).SetFont(FontTableHead);
                tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(1, 1).SetContent(": " + batyr);
                tableparts.Cell(1, 1).SetFont(FontTableHead);
                tableparts.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(1, 2).SetContent("Degree / Branch");
                tableparts.Cell(1, 2).SetFont(FontTableHead);
                tableparts.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(1, 3).SetContent(": " + deg.ToUpper());
                tableparts.Cell(1, 3).SetFont(FontTableHead);
                tableparts.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);

                //Line3
                tableparts.Cell(2, 0).SetContent("RollNo");
                tableparts.Cell(2, 0).SetFont(FontTableHead);
                tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(2, 1).SetContent(": " + rollno);
                tableparts.Cell(2, 1).SetFont(FontTableHead);
                tableparts.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(2, 2).SetContent("Name");
                tableparts.Cell(2, 2).SetFont(FontTableHead);
                tableparts.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(2, 3).SetContent(": " + studname.ToUpper());
                tableparts.Cell(2, 3).SetFont(FontTableHead);
                tableparts.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(2, 3).ColSpan = 3;

                //Line4
                tableparts.Cell(3, 0).SetContent("Fee For Academic Year");
                tableparts.Cell(3, 0).SetFont(FontTableHead);
                tableparts.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                tableparts.Cell(3, 0).ColSpan = 2;

                tableparts.Cell(3, 2).SetContent(acaYear);
                tableparts.Cell(3, 2).SetFont(FontTableHead);
                tableparts.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(3, 4).SetContent("Type");
                tableparts.Cell(3, 4).SetFont(FontTableHead);
                tableparts.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleLeft);


                tableparts.Cell(3, 5).SetContent(": " + mode);
                tableparts.Cell(3, 5).SetFont(FontTableHead);
                tableparts.Cell(3, 5).SetContentAlignment(ContentAlignment.MiddleLeft);


                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, curX, curY, 500, 200));
                rcptpage.Add(addtabletopage1);

                #endregion

                #region Table 2
                //Table2 Format

                string semyear = "";
                if (ddl_semrcpt.Items.Count > 0)
                {
                    semyear = Convert.ToString(ddl_semrcpt.SelectedItem.Value);
                }
                int rows = 1;
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

                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 6, 5);
                tableparts1.VisibleHeaders = false;
                //tableparts1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                tableparts1.Cell(0, 0).SetContent("S.No");
                tableparts1.Cell(0, 0).SetFont(FontTableHead);
                tableparts1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts1.Cell(0, 1).SetContent("Particulars");
                tableparts1.Cell(0, 1).SetFont(FontTableHead);
                tableparts1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts1.Cell(0, 1).ColSpan = 4;

                tableparts1.Cell(0, 5).SetContent("Amount (Rs)");
                tableparts1.Cell(0, 5).SetFont(FontTableHead);
                tableparts1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                //Table2 Data

                #region feedata

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
                        selectQuery = " SELECT A.HeaderFK,HeaderName,A.LedgerFK,priority,LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(DeductAmout,0)   as DeductAmount ,isnull(TotalAmount,0)   as TotalAmount,isnull(ChlTaken,0) as ChlTakAmt,isnull(PaidAmount,0) as PaidAmount,isnull(BalAmount,0) as BalAmount,TextVal,TextCode,ChlGroupHeader FROM FT_FeeAllot A,FM_HeaderMaster H,FS_ChlGroupHeaderSettings S, FM_LedgerMaster L,TextValTable T WHERE A.HeaderFK = H.HeaderPK and a.headerfk = s.headerfk and l.headerfk = s.headerfk  AND A.LedgerFK = L.LedgerPK AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and h.headerpk = s.headerfk  and l.LedgerMode=0   and ChlGroupHeader in('" + headercode + "') and T.TextCode in('" + semyear + "')   and (isnull(TOtalAmount,0)-isnull(paidamount,0))>0  ";
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
                        indx++;
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
                        tableparts1.Cell(indx, 1).ColSpan = 4;

                        tableparts1.Cell(indx, 5).SetContent(creditAmt0);
                        tableparts1.Cell(indx, 5).SetFont(FontTable);
                        tableparts1.Cell(indx, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                        createPDFOK = true;
                    }

                }
                #endregion


                #endregion

                curY += 5 + (int)addtabletopage1.Area.Height;
                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 600));
                rcptpage.Add(addtabletopage2);

                #endregion

                #region Table 3
                //Table3 Format
                PdfTable tableparts2 = recptDoc.NewTable(FontTableHead, 4, 6, 5);
                tableparts2.VisibleHeaders = false;
                // tableparts2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                //Table3 Header              
                decimal totalamount = (decimal)curpaid;
                overallCashAmt += Convert.ToDouble(totalamount);
                tableparts2.Cell(0, 0).SetContent("Received " + DecimalToWords(totalamount).ToString() + " Rupees Only");
                tableparts2.Cell(0, 0).SetFont(FontTableHead);
                tableparts2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                tableparts2.Cell(0, 0).ColSpan = 4;

                tableparts2.Cell(0, 4).SetContent("Total Amount");
                tableparts2.Cell(0, 4).SetFont(FontTableHead);
                tableparts2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts2.Cell(0, 5).SetContent(curpaid);
                tableparts2.Cell(0, 5).SetFont(FontTableHead);
                tableparts2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts2.Cell(3, 3).SetContent("CASHIER / ACCOUNTANT");
                tableparts2.Cell(3, 3).SetFont(FontTableHead);
                tableparts2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleRight);
                tableparts2.Cell(3, 3).ColSpan = 3;


                curY += 5 + (int)addtabletopage2.Area.Height;
                PdfTablePage addtabletopage3 = tableparts2.CreateTablePage(new PdfArea(recptDoc, 40, curY, 500, 100));
                rcptpage.Add(addtabletopage3);
                #endregion

                rcptpage.SaveToDocument();

                //save changes
                PdfPage rcptpageOf = rcptpage.CreateCopy();
                PdfPage rcptpageTran = rcptpage.CreateCopy();
                if (officopy != 0)
                {
                    rcptpageOf.SaveToDocument();
                }

                if (transCopy != 0)
                {
                    rcptpageTran.SaveToDocument();
                }


            }

        }

        #endregion
    }
}