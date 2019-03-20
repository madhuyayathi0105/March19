using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.Drawing;
using System.Web;


/// <summary>
/// Summary description for GeneralAndMCCChallan
/// </summary>
public class GeneralAndMCCChallan : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();
    public GeneralAndMCCChallan()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string generateChallanNo(string usercode, string collegecode1, ref string lastAccId, ref string lastCHlNO)
    {
        string recno = string.Empty;

        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");

            string secondreciptqurey = "SELECT ChallanStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT ChallanAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")");
                recacr = acronymquery;

                int size = Convert.ToInt32(d2.GetFunction("SELECT  ChallanSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + ")"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;

                lastAccId = accountid;
                lastCHlNO = receno.ToString();

            }

            return recno;
        }
        catch (Exception ex) { return recno; }
    }
    //added school setting
    public double checkSchoolSetting(string usercode)
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    //--------------
    private string getAppNoFromApplyn(string app_formno, string collegecode)
    {
        string appno = "0";
        appno = d2.GetFunction("select app_no from applyn where app_formno='" + app_formno + "'  and college_code='" + collegecode + "' ").Trim();
        return appno;
    }
    public string printGeneralAndMCCChallan(CheckBox cb_selcthd, RadioButtonList rbl_headerselect, string collegecode1, string usercode, string lastRecptNo, string accidRecpt, RadioButton rdo_multi, ref TextBox txt_rcptno, FpSpread Fpspread1, TextBox txt_totnoofstudents, TextBox txt_date, TextBox txt_name, DropDownList ddl_semMultiple, DropDownList rbl_rollno, DropDownList ddl_collegebank, ref Label lbl_alert, ref bool imgDIVVisible, CheckBoxList cbl_grpheader, ref TextBox Txt_amt, GridView grid_Details, ref bool contentVisible, ref bool CreateReceiptOK, string lblstaticrollno, string ddlSEM, string ddlTYPE, string ddlDEPT, DropDownList ddl_sem)
    {
        CreateReceiptOK = false;
        contentVisible = false;
        imgDIVVisible = false;
        lastRecptNo = string.Empty;
        accidRecpt = string.Empty;
        StringBuilder contentDiv = new StringBuilder();
        //-----------------

        //--------------

        //Mcc and Others
        try
        {
            int challanType = 1;
            if (!cb_selcthd.Checked)
            {
                challanType = 1;
            }
            else
            {
                challanType = rbl_headerselect.SelectedIndex + 2;
            }
            string roll_admit = string.Empty;

            txt_rcptno.Text = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);

            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (lastRecptNo != "")
            {

                if (rdo_multi.Checked)
                {
                    #region For Multiple students
                    //rbl_rollno.SelectedIndex = 1;
                    int count = 0;
                    bool createPDFOK = false;
                    Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                    Font Fontsmall = new Font("Arial", 8, FontStyle.Bold);
                    Font Fontsmall1 = new Font("Arial", 10, FontStyle.Bold);
                    Font Fontbold1 = new Font("Arial", 10, FontStyle.Bold);
                    Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                    mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));
                    Gios.Pdf.PdfPage myprov_pdfpage = null;

                    for (int row = 1; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        if (txt_totnoofstudents.Text == "")
                        {
                            continue;
                        }
                        int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[row, 1].Value);
                        if (checkval == 1)
                        {
                            count++;

                            roll_admit = Convert.ToString(Fpspread1.Sheets[0].Cells[row, 2].Text);

                            #region Inside Students For loop
                            try
                            {
                                //Basic Data
                                //string roll_admit = lblstaticrollno.Text.Trim();
                                string recptNo = txt_rcptno.Text;
                                string recptDt = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                                string studname = txt_name.Text.Trim();
                                // string course = txt_dept.Text.Trim();
                                string batchYrSem = string.Empty;
                                string Regno = string.Empty;
                                string rollno = string.Empty;
                                string appnoNew = string.Empty;
                                string regno = string.Empty;
                                string degreeCode = string.Empty;
                                string stream = string.Empty;
                                string feeCategory = string.Empty;
                                string app_formno = string.Empty;
                                string smartno = string.Empty;
                                string sem = string.Empty;
                                studname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);

                                feeCategory = Convert.ToString(ddl_semMultiple.SelectedValue);
                                if (checkSchoolSetting(usercode) == 0)
                                {
                                    sem = Convert.ToString(ddl_semMultiple.SelectedValue);
                                }
                                string queryRollApp = "select r.Roll_No,a.app_formno,r.smart_serial_no,a.app_no,r.Reg_No  from Registration r,applyn a where r.App_No=a.app_no  and r.college_code='" + collegecode1 + "'  and r.Roll_Admit='" + roll_admit + "'";
                                DataSet dsRollApp = new DataSet();
                                dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                if (dsRollApp.Tables.Count > 0)
                                {
                                    if (dsRollApp.Tables[0].Rows.Count > 0)
                                    {
                                        rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                        app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                        appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                        Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                        smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);

                                    }
                                }

                                string rolldisplay = "Admission No :";
                                string rollvalue = roll_admit;
                                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                                {
                                    rolldisplay = "Roll No :";
                                    rollvalue = rollno;
                                }
                                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                                {
                                    rolldisplay = "Reg No :";
                                    rollvalue = Regno;
                                }
                                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                                {
                                    rolldisplay = "Admission No :";
                                    rollvalue = roll_admit;
                                }
                                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                                {
                                    rolldisplay = "Smartcard No :";
                                    rollvalue = smartno;
                                }
                                else
                                {
                                    appnoNew = getAppNoFromApplyn(roll_admit, collegecode1);
                                    rolldisplay = "App No :";
                                    rollvalue = app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + appnoNew + "'").Trim();
                                }

                                //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                string colquery = "";
                                if (rolldisplay != "App No :")
                                {
                                    colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                }
                                else
                                {
                                    colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                }
                                string collegename = "";
                                string add1 = "";
                                string add2 = "";
                                string univ = "";
                                string deg = "";
                                string cursem = "";
                                string batyr = "";
                                string bankName = "";
                                string bankPK = "";
                                string bankCity = "";
                                if (ddl_collegebank.Items.Count > 0)
                                {
                                    bankName = ddl_collegebank.SelectedItem.Text.Split('-')[0];
                                    bankPK = ddl_collegebank.SelectedItem.Value;
                                    bankCity = "(" + ddl_collegebank.SelectedItem.Text.Split('-')[2] + ")";
                                }

                                DataSet ds = new DataSet();
                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        //if (degACR == 0)
                                        //{
                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                        //}
                                        //else
                                        //{
                                        // deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                        //}
                                        degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);

                                        cursem = Convert.ToString(ddl_semMultiple.SelectedItem.Text).Split(' ')[1] + " : " + romanLetter(Convert.ToString(ddl_semMultiple.SelectedItem.Text).Split(' ')[0]);

                                        // Modify Multiple Challan Feecategory

                                        //linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");

                                        //if (linkvalue != "")
                                        //{
                                        //    if (linkvalue == "0")
                                        //    {
                                        //        feeCategory = d2.GetFunction("selECT	* from textvaltable where TextCriteria ='FEECA' and textval = '" + cursem + " semester' and college_code=" + collegecode1 + "");
                                        //    }
                                        //    else
                                        //    {
                                        //        feeCategory = d2.GetFunction(" selECT	* from textvaltable where TextCriteria ='FEECA' and textval = '" + returnYearforSem(cursem) + " Year' and college_code=" + collegecode1 + "");
                                        //    }
                                        //}
                                    }
                                }


                                #region PDF Generation


                                // New Code

                                string groupHdr;
                                string[] hdrInGrp0;
                                List<string> hdrInGrp = new List<string>();

                                bool checkedHeaderOK = false;
                                if (!cb_selcthd.Checked)
                                {
                                    #region For Overall
                                    string QHdrForGroup = "	SELECT ChlGroupHeader FROM FM_ChlBankPrintSettings WHERE DegreeCode = '" + degreeCode + "' AND SettingType = 1 and CollegeCode=" + collegecode1 + " ";

                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(QHdrForGroup, "Text");
                                    if (ds.Tables.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                            {
                                                string bnkAcc = "";
                                                checkedHeaderOK = false;
                                                groupHdr = Convert.ToString(ds.Tables[0].Rows[i][0]);
                                                hdrInGrp0 = groupHdr.Split(',');
                                                hdrInGrp.Clear();
                                                foreach (string item in hdrInGrp0)
                                                {
                                                    hdrInGrp.Add(item);
                                                    checkedHeaderOK = true;
                                                }

                                                if (!checkedHeaderOK)
                                                {
                                                    continue;
                                                }

                                                //Add new challan Page in this loop
                                                bool addpageOK = false;
                                                #region TOp portion

                                                int y = 0;

                                                myprov_pdfpage = mychallan.NewPage();

                                                PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                                PdfTextArea ORGI;
                                                if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                                {
                                                    ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                                }
                                                else
                                                {
                                                    ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                                }
                                                PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 90, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                                //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                                PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                                PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                                //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                                PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                                PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                                PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                                //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                                PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                                PdfTextArea FC13;
                                                if (checkSchoolSetting(usercode) != 0)
                                                {
                                                    FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                                }
                                                else
                                                {
                                                    FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                                }

                                                PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                                PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                                PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                double ovrallcredit = 0;
                                                double grandtotal = 0.00;


                                                myprov_pdfpage.Add(FC17);
                                                string text = "";

                                                //First Ends

                                                PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                                PdfTextArea UC1;
                                                if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                                {
                                                    UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                                }
                                                else
                                                {
                                                    UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                                }
                                                PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                                //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                                PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                                PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                                PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                                PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                                PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                                //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                                PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                                PdfTextArea UC13;
                                                if (checkSchoolSetting(usercode) != 0)
                                                {
                                                    UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                                }
                                                else
                                                {
                                                    UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                                }
                                                PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                                PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                                PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                //second End
                                                y = 0;


                                                PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                                PdfTextArea TC1;
                                                if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                                {
                                                    TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                                }
                                                else
                                                {
                                                    TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                                }
                                                PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                                //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                                PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                                PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                                PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                                PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                                PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                                //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                                PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                                PdfTextArea TC13;
                                                if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                                {
                                                    TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                                }
                                                else
                                                {
                                                    TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                                }
                                                PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                                PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                                PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                                PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                                PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                                PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                                myprov_pdfpage.Add(FC10);
                                                myprov_pdfpage.Add(UC10);
                                                myprov_pdfpage.Add(TC10);
                                                y = 0;

                                                #endregion

                                                //End of  New CHallan Top Portion

                                                //Middle portion of the challan
                                                #region Middle Portion challan
                                                int chk = 0;
                                                for (int indx = 0; indx < hdrInGrp.Count; indx++)
                                                {
                                                    //string QhdrId = "SELECT HeaderFK  FROM FS_ChlGroupHeaderSettings s,,FS_HeaderPrivilage P where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and Stream='" + stream + "' and p.HeaderFK = S.HeaderFK and p.usercode='" + usercode + "'";

                                                    //modified by saranya 04dec2017

                                                    string QhdrId = "SELECT s.HeaderFK  FROM FS_ChlGroupHeaderSettings s,FS_HeaderPrivilage P where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and Stream='" + stream + "' and p.HeaderFK = S.HeaderFK and p.usercode='" + usercode + "'";

                                                    string HdrId = "";
                                                    string dispHdr = "";

                                                    DataSet ds1 = new DataSet();
                                                    ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                                    if (ds1.Tables.Count > 0)
                                                    {
                                                        if (ds1.Tables[0].Rows.Count > 0)
                                                        {

                                                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                            {
                                                                if (HdrId == "")
                                                                {
                                                                    HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);

                                                                }
                                                                else
                                                                {
                                                                    HdrId += "," + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                                }
                                                            }

                                                            string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T,FS_HeaderPrivilage PH	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code and A.HeaderFK = PH.HeaderFK  and PH.HeaderFK = S.HeaderFK  AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + " and PH.usercode='" + usercode + "' 	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T,FS_HeaderPrivilage P 	WHERE  C.HeaderFK = p.HeaderFK and  p.HeaderFK = S.HeaderFK and C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ")  and p.usercode='" + usercode + "' and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                                            DataSet ds2 = new DataSet();
                                                            ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                            if (ds2.Tables.Count > 0)
                                                            {
                                                                if (ds2.Tables[0].Rows.Count > 0)
                                                                {
                                                                    dispHdr = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                                    string hdrNme = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]).Trim().ToUpper();
                                                                    double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                                    {
                                                                        totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                                    }
                                                                    // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                                    dispHdr += " (" + bnkAcc + ")";
                                                                    grandtotal = grandtotal + totalAmt;

                                                                    if (grandtotal > 0 || hdrNme == "TUITION FEE")
                                                                    {
                                                                        addpageOK = true;
                                                                        createPDFOK = true;
                                                                        if (totalAmt > 0 || hdrNme == "TUITION FEE")
                                                                        {
                                                                            if (chk == 0)
                                                                            {
                                                                                // chk++;
                                                                                #region Update Challan No
                                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                                txt_rcptno.Text = recptNo;

                                                                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                                {
                                                                                    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + "  and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                                    DataSet dsEachHdr = new DataSet();
                                                                                    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                                    if (dsEachHdr.Tables.Count > 0)
                                                                                    {
                                                                                        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                                        {
                                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot f,FM_LedgerMaster l WHERE l.Ledgerpk=f.ledgerfk  and l.headerfk=f.headerfk  and f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
                                                                                            DataSet dsLedge = new DataSet();
                                                                                            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                                            if (dsLedge.Tables.Count > 0)
                                                                                            {
                                                                                                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                                {
                                                                                                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                                    {
                                                                                                        double remainAmt = 0;
                                                                                                        remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                                        if (remainAmt > 0 || hdrNme == "TUITION FEE")
                                                                                                        {
                                                                                                            string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                                            d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                                            string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                                            d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                }
                                                                                #endregion
                                                                            }
                                                                        }
                                                                    }

                                                                    PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                     new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                    PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                    PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                    myprov_pdfpage.Add(FC18);
                                                                    myprov_pdfpage.Add(FC171);
                                                                    myprov_pdfpage.Add(FC19);


                                                                    PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                     new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                    PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                    PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                    myprov_pdfpage.Add(UC18);
                                                                    myprov_pdfpage.Add(UC19);
                                                                    myprov_pdfpage.Add(UC171);

                                                                    PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                     new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                    PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                    PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                        new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                    myprov_pdfpage.Add(TC18);
                                                                    myprov_pdfpage.Add(TC19);
                                                                    myprov_pdfpage.Add(TC171);
                                                                    y = y + 15;

                                                                }
                                                            }

                                                        }
                                                    }

                                                }
                                                #endregion
                                                //Middle portion of challan End

                                                //Bottom portion of the challan
                                                if (addpageOK)
                                                {

                                                    string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                                    d2.update_method_wo_parameter(updateRecpt, "Text");
                                                    PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                               new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                    PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                    PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                          new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                    PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                    PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                    PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                       new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);

                                                    myprov_pdfpage.Add(FC4);
                                                    myprov_pdfpage.Add(UC4);
                                                    myprov_pdfpage.Add(TC4);
                                                    //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                                    //myprov_pdfpage.Add(FC08, 250, 125);
                                                    //myprov_pdfpage.Add(FC08, 550, 125);
                                                    //myprov_pdfpage.Add(FC08, 900, 125);
                                                    #region Bottom Portion of Challan

                                                    text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                                    PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                    PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                    PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                    PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                    PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                    PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                    PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                    PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                                    PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                                    PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                                    myprov_pdfpage.Add(pr1);

                                                    PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                                    PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                                    myprov_pdfpage.Add(pr2);

                                                    PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                                    PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                                    myprov_pdfpage.Add(pr3);

                                                    Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                    table.VisibleHeaders = false;
                                                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table.Columns[0].SetWidth(100);
                                                    table.Columns[1].SetWidth(60);
                                                    table.Columns[2].SetWidth(60);

                                                    if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                    {
                                                        table.Cell(0, 0).SetContent("Cheque/DD No");
                                                    }
                                                    else
                                                    {
                                                        table.Cell(0, 0).SetContent("DD No");
                                                    }
                                                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(0, 0).SetFont(Fontbold1);
                                                    table.Cell(0, 1).SetContent("Date");
                                                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(0, 1).SetFont(Fontbold1);
                                                    table.Cell(0, 2).SetContent("Amount");
                                                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(0, 2).SetFont(Fontbold1);
                                                    table.Cell(1, 0).SetContent("\n");
                                                    table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(1, 0).SetFont(Fontbold1);
                                                    table.Cell(1, 1).SetContent("\n");
                                                    table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(1, 1).SetFont(Fontbold1);
                                                    table.Cell(1, 2).SetContent("\n");
                                                    table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table.Cell(1, 2).SetFont(Fontbold1);
                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable);

                                                    Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                    table1.VisibleHeaders = false;
                                                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table1.Columns[0].SetWidth(100);
                                                    table1.Columns[1].SetWidth(60);
                                                    table1.Cell(0, 0).SetContent("2000x");
                                                    table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(0, 0).SetFont(Fontbold1);
                                                    table1.Cell(1, 0).SetContent("500x");
                                                    table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(1, 0).SetFont(Fontbold1);
                                                    table1.Cell(2, 0).SetContent("100x");
                                                    table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(2, 0).SetFont(Fontbold1);
                                                    table1.Cell(3, 0).SetContent("50x");
                                                    table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(3, 0).SetFont(Fontbold1);
                                                    table1.Cell(4, 0).SetContent("20x");
                                                    table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(4, 0).SetFont(Fontbold1);
                                                    table1.Cell(5, 0).SetContent("10x");
                                                    table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(5, 0).SetFont(Fontbold1);
                                                    table1.Cell(6, 0).SetContent("5x");
                                                    table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(6, 0).SetFont(Fontbold1);
                                                    table1.Cell(7, 0).SetContent("Coinsx");
                                                    table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(7, 0).SetFont(Fontbold1);
                                                    table1.Cell(8, 0).SetContent("Total");
                                                    table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table1.Cell(8, 0).SetFont(Fontbold1);
                                                    if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                    {
                                                        //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                        //myprov_pdfpage.Add(mobile);aaaa
                                                        PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                        myprov_pdfpage.Add(mblnoPdfTextArea);

                                                    }

                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable1);

                                                    myprov_pdfpage.Add(FC);
                                                    myprov_pdfpage.Add(ORGI);
                                                    myprov_pdfpage.Add(IOB);
                                                    //myprov_pdfpage.Add(FC4);
                                                    myprov_pdfpage.Add(FC5);
                                                    myprov_pdfpage.Add(FC6);
                                                    myprov_pdfpage.Add(FC7);
                                                    myprov_pdfpage.Add(FC8);
                                                    myprov_pdfpage.Add(FC9);
                                                    //myprov_pdfpage.Add(FC10);
                                                    myprov_pdfpage.Add(FC11);
                                                    myprov_pdfpage.Add(FC12);
                                                    myprov_pdfpage.Add(FC13);
                                                    myprov_pdfpage.Add(FC14);
                                                    myprov_pdfpage.Add(FC15);
                                                    myprov_pdfpage.Add(FC16);

                                                    myprov_pdfpage.Add(FC24);
                                                    myprov_pdfpage.Add(FC25);
                                                    myprov_pdfpage.Add(FC26);
                                                    myprov_pdfpage.Add(FC27);
                                                    myprov_pdfpage.Add(FC28);
                                                    myprov_pdfpage.Add(FC29);
                                                    myprov_pdfpage.Add(FC30);
                                                    myprov_pdfpage.Add(FC31);

                                                    myprov_pdfpage.Add(FC32);
                                                    //myprov_pdfpage.Add(FC33);

                                                    //First End
                                                    myprov_pdfpage.Add(UC17);

                                                    PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                    PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                    PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                    PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                    PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                    PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                    PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                    PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                    Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                    table3.VisibleHeaders = false;
                                                    table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table3.Columns[0].SetWidth(100);
                                                    table3.Columns[1].SetWidth(60);
                                                    table3.Columns[2].SetWidth(60);

                                                    if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                    {
                                                        table3.Cell(0, 0).SetContent("Cheque/DD No");
                                                    }
                                                    else
                                                    {
                                                        table3.Cell(0, 0).SetContent("DD No");
                                                    }
                                                    table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(0, 0).SetFont(Fontbold1);
                                                    table3.Cell(0, 1).SetContent("Date");
                                                    table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(0, 1).SetFont(Fontbold1);
                                                    table3.Cell(0, 2).SetContent("Amount");
                                                    table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(0, 2).SetFont(Fontbold1);
                                                    table3.Cell(1, 0).SetContent("\n");
                                                    table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(1, 0).SetFont(Fontbold1);
                                                    table3.Cell(1, 1).SetContent("\n");
                                                    table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(1, 1).SetFont(Fontbold1);
                                                    table3.Cell(1, 2).SetContent("\n");
                                                    table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table3.Cell(1, 2).SetFont(Fontbold1);
                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable3);

                                                    Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                    table14.VisibleHeaders = false;
                                                    table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table14.Columns[0].SetWidth(100);
                                                    table14.Columns[1].SetWidth(60);
                                                    table14.Cell(0, 0).SetContent("2000x");
                                                    table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(0, 0).SetFont(Fontbold1);
                                                    table14.Cell(1, 0).SetContent("500x");
                                                    table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(1, 0).SetFont(Fontbold1);
                                                    table14.Cell(2, 0).SetContent("100x");
                                                    table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(2, 0).SetFont(Fontbold1);
                                                    table14.Cell(3, 0).SetContent("50x");
                                                    table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(3, 0).SetFont(Fontbold1);
                                                    table14.Cell(4, 0).SetContent("20x");
                                                    table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(4, 0).SetFont(Fontbold1);
                                                    table14.Cell(5, 0).SetContent("10x");
                                                    table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(5, 0).SetFont(Fontbold1);
                                                    table14.Cell(6, 0).SetContent("5x");
                                                    table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(6, 0).SetFont(Fontbold1);
                                                    table14.Cell(7, 0).SetContent("Coinsx");
                                                    table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(7, 0).SetFont(Fontbold1);
                                                    table14.Cell(8, 0).SetContent("Total");
                                                    table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table14.Cell(8, 0).SetFont(Fontbold1);
                                                    //
                                                    if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                    {
                                                        //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                        //myprov_pdfpage.Add(mobile);aaaa
                                                        PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                        myprov_pdfpage.Add(mblnoPdfTextArea);

                                                    }
                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable4);

                                                    myprov_pdfpage.Add(UC);
                                                    myprov_pdfpage.Add(UC1);
                                                    myprov_pdfpage.Add(UC2);
                                                    //myprov_pdfpage.Add(UC4);
                                                    myprov_pdfpage.Add(UC5);
                                                    myprov_pdfpage.Add(UC6);
                                                    myprov_pdfpage.Add(UC7);
                                                    myprov_pdfpage.Add(UC8);
                                                    myprov_pdfpage.Add(UC9);
                                                    //myprov_pdfpage.Add(UC10);
                                                    myprov_pdfpage.Add(UC11);
                                                    myprov_pdfpage.Add(UC12);
                                                    myprov_pdfpage.Add(UC13);
                                                    myprov_pdfpage.Add(UC14);
                                                    myprov_pdfpage.Add(UC15);
                                                    myprov_pdfpage.Add(UC16);


                                                    myprov_pdfpage.Add(UC24);
                                                    myprov_pdfpage.Add(UC25);
                                                    myprov_pdfpage.Add(UC26);
                                                    myprov_pdfpage.Add(UC27);
                                                    myprov_pdfpage.Add(UC28);
                                                    myprov_pdfpage.Add(UC29);
                                                    myprov_pdfpage.Add(UC30);
                                                    myprov_pdfpage.Add(UC31);
                                                    myprov_pdfpage.Add(UC32);
                                                    //second End


                                                    myprov_pdfpage.Add(TC17);

                                                    PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                    PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                    PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                    PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                    PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                    PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                    PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                    PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                    Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                    table5.VisibleHeaders = false;
                                                    table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table5.Columns[0].SetWidth(100);
                                                    table5.Columns[1].SetWidth(60);
                                                    table5.Columns[2].SetWidth(60);

                                                    if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                    {
                                                        table5.Cell(0, 0).SetContent("Cheque/DD No");
                                                    }
                                                    else
                                                    {
                                                        table5.Cell(0, 0).SetContent("DD No");
                                                    }
                                                    table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(0, 0).SetFont(Fontbold1);
                                                    table5.Cell(0, 1).SetContent("Date");
                                                    table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(0, 1).SetFont(Fontbold1);
                                                    table5.Cell(0, 2).SetContent("Amount");
                                                    table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(0, 2).SetFont(Fontbold1);
                                                    table5.Cell(1, 0).SetContent("\n");
                                                    table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(1, 0).SetFont(Fontbold1);
                                                    table5.Cell(1, 1).SetContent("\n");
                                                    table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(1, 1).SetFont(Fontbold1);
                                                    table5.Cell(1, 2).SetContent("\n");
                                                    table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                    table5.Cell(1, 2).SetFont(Fontbold1);
                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable31);

                                                    Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                    table15.VisibleHeaders = false;
                                                    table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                    table15.Columns[0].SetWidth(100);
                                                    table15.Columns[1].SetWidth(60);
                                                    table15.Cell(0, 0).SetContent("2000x");
                                                    table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(0, 0).SetFont(Fontbold1);
                                                    table15.Cell(1, 0).SetContent("500x");
                                                    table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(1, 0).SetFont(Fontbold1);
                                                    table15.Cell(2, 0).SetContent("100x");
                                                    table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(2, 0).SetFont(Fontbold1);
                                                    table15.Cell(3, 0).SetContent("50x");
                                                    table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(3, 0).SetFont(Fontbold1);
                                                    table15.Cell(4, 0).SetContent("20x");
                                                    table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(4, 0).SetFont(Fontbold1);
                                                    table15.Cell(5, 0).SetContent("10x");
                                                    table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(5, 0).SetFont(Fontbold1);
                                                    table15.Cell(6, 0).SetContent("5x");
                                                    table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(6, 0).SetFont(Fontbold1);
                                                    table15.Cell(7, 0).SetContent("Coinsx");
                                                    table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(7, 0).SetFont(Fontbold1);
                                                    table15.Cell(8, 0).SetContent("Total");
                                                    table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                    table15.Cell(8, 0).SetFont(Fontbold1);
                                                    table15.Cell(8, 0).SetFont(Fontbold1);
                                                    if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                    {
                                                       
                                                        PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                          new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                        myprov_pdfpage.Add(mblnoPdfTextArea);

                                                    }
                                                    Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                                    myprov_pdfpage.Add(myprov_pdfpagetable5);

                                                    myprov_pdfpage.Add(TC);
                                                    myprov_pdfpage.Add(TC1);
                                                    myprov_pdfpage.Add(TC2);
                                                    //myprov_pdfpage.Add(TC4);
                                                    myprov_pdfpage.Add(TC5);
                                                    myprov_pdfpage.Add(TC6);
                                                    myprov_pdfpage.Add(TC7);
                                                    myprov_pdfpage.Add(TC8);
                                                    myprov_pdfpage.Add(TC9);
                                                    //myprov_pdfpage.Add(TC10);
                                                    myprov_pdfpage.Add(TC11);
                                                    myprov_pdfpage.Add(TC12);
                                                    myprov_pdfpage.Add(TC13);
                                                    myprov_pdfpage.Add(TC14);
                                                    myprov_pdfpage.Add(TC15);
                                                    myprov_pdfpage.Add(TC16);
                                                    myprov_pdfpage.Add(TC17);
                                                    myprov_pdfpage.Add(TC24);
                                                    myprov_pdfpage.Add(TC25);
                                                    myprov_pdfpage.Add(TC26);
                                                    myprov_pdfpage.Add(TC27);
                                                    myprov_pdfpage.Add(TC28);
                                                    myprov_pdfpage.Add(TC29);
                                                    myprov_pdfpage.Add(TC30);
                                                    myprov_pdfpage.Add(TC31);
                                                    myprov_pdfpage.Add(TC32);

                                                    myprov_pdfpage.SaveToDocument();
                                                    #endregion
                                                }
                                                //Bottom portion of the challan End
                                            }
                                        }
                                    }

                                    //New COde END
                                    #endregion
                                }
                                else
                                {
                                    #region For Selected


                                    //End of  New CHallan Top Portion


                                    //groupHdr = Convert.ToString(ds.Tables[0].Rows[i][0]);
                                    //hdrInGrp0 = groupHdr.Split(',');
                                    // hdrInGrp.Clear();
                                    //foreach (string item in hdrInGrp0)
                                    //{
                                    //    hdrInGrp.Add(item);
                                    //    checkedHeaderOK = true;
                                    //}



                                    //Middle portion of the challan
                                    if (rbl_headerselect.SelectedIndex == 0)
                                    {
                                        #region Middle Portion challan
                                        int chk = 0;
                                        for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                        {

                                            if (!cbl_grpheader.Items[indx].Selected)
                                            {
                                                continue;
                                            }

                                            checkedHeaderOK = false;
                                            string bnkAcc = "";
                                            //Add new challan Page in this loop
                                            bool addpageOK = false;
                                            #region TOp portion

                                            int y = 0;

                                            myprov_pdfpage = mychallan.NewPage();

                                            PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea ORGI;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                            }
                                            else
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                            }
                                            PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                            //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                            //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                            PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                            PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                            PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea FC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                            }
                                            PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;


                                            myprov_pdfpage.Add(FC17);
                                            string text = "";

                                            //First Ends

                                            PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea UC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                            }
                                            else
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                            }
                                            PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                            //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                            //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                            PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                            PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            //second End
                                            y = 0;


                                            PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea TC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                            }
                                            else
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                            }
                                            PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                            //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                            //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                            PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                            PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea TC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" +sem);
                                            }

                                            PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(TC10);
                                            y = 0;

                                            #endregion

                                            string QhdrId = "SELECT HeaderFK,ChlGroupHeader  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + cbl_grpheader.Items[indx].Text + "') and stream='" + stream + "'";
                                            string HdrId = "";
                                            string dispHdr = "";

                                            DataSet ds1 = new DataSet();
                                            ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                            if (ds1.Tables.Count > 0)
                                            {
                                                if (ds1.Tables[0].Rows.Count > 0)
                                                {
                                                    dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);
                                                    //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                    dispHdr += " (" + bnkAcc + ")";
                                                    // bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);

                                                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                    {
                                                        if (HdrId == "")
                                                        {
                                                            HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);

                                                        }
                                                        else
                                                        {
                                                            HdrId += "," + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                        }
                                                    }

                                                    string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                                    DataSet ds2 = new DataSet();
                                                    ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                    if (ds2.Tables.Count > 0)
                                                    {
                                                        if (ds2.Tables[0].Rows.Count > 0)
                                                        {
                                                            //  dispHdr = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                            double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                            if (ds2.Tables[1].Rows.Count > 0)
                                                            {
                                                                totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                            }

                                                            grandtotal = grandtotal + totalAmt;


                                                            if (grandtotal > 0)
                                                            {

                                                                addpageOK = true;
                                                                createPDFOK = true;
                                                                if (totalAmt > 0)
                                                                {
                                                                    if (chk == 0)
                                                                    {
                                                                        // chk++;
                                                                        #region Update Challan No
                                                                        recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                        txt_rcptno.Text = recptNo;

                                                                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                        {
                                                                            string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                                            DataSet dsEachHdr = new DataSet();
                                                                            dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                            if (dsEachHdr.Tables.Count > 0)
                                                                            {
                                                                                if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')   order by case when priority is null then 1 else 0 end, priority ";
                                                                                    DataSet dsLedge = new DataSet();
                                                                                    dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                                    if (dsLedge.Tables.Count > 0)
                                                                                    {
                                                                                        if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                        {
                                                                                            for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                            {
                                                                                                double remainAmt = 0;
                                                                                                remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                                if (remainAmt > 0)
                                                                                                {
                                                                                                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                                    string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                                    d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                        }

                                                                        #endregion
                                                                    }
                                                                }
                                                            }

                                                            PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(FC18);
                                                            myprov_pdfpage.Add(FC171);
                                                            myprov_pdfpage.Add(FC19);


                                                            PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(UC18);
                                                            myprov_pdfpage.Add(UC19);
                                                            myprov_pdfpage.Add(UC171);

                                                            PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(TC18);
                                                            myprov_pdfpage.Add(TC19);
                                                            myprov_pdfpage.Add(TC171);
                                                            y = y + 15;

                                                        }
                                                    }

                                                }
                                            }

                                            //Bottom portion of the challan
                                            if (addpageOK)
                                            {
                                                string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);


                                                myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(TC4);
                                                PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                                //myprov_pdfpage.Add(FC08, 250, 125);
                                                //myprov_pdfpage.Add(FC08, 550, 125);
                                                //myprov_pdfpage.Add(FC08, 900, 125);
                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                                PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                                myprov_pdfpage.Add(pr1);

                                                PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                                myprov_pdfpage.Add(pr2);

                                                PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                                myprov_pdfpage.Add(pr3);

                                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table.VisibleHeaders = false;
                                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table.Columns[0].SetWidth(100);
                                                table.Columns[1].SetWidth(60);
                                                table.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table.Cell(0, 0).SetContent("DD No");
                                                }
                                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 0).SetFont(Fontbold1);
                                                table.Cell(0, 1).SetContent("Date");
                                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 1).SetFont(Fontbold1);
                                                table.Cell(0, 2).SetContent("Amount");
                                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 2).SetFont(Fontbold1);
                                                table.Cell(1, 0).SetContent("\n");
                                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 0).SetFont(Fontbold1);
                                                table.Cell(1, 1).SetContent("\n");
                                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 1).SetFont(Fontbold1);
                                                table.Cell(1, 2).SetContent("\n");
                                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table1.VisibleHeaders = false;
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Columns[0].SetWidth(100);
                                                table1.Columns[1].SetWidth(60);
                                                table1.Cell(0, 0).SetContent("2000x");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(0, 0).SetFont(Fontbold1);
                                                table1.Cell(1, 0).SetContent("500x");
                                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(1, 0).SetFont(Fontbold1);
                                                table1.Cell(2, 0).SetContent("100x");
                                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(2, 0).SetFont(Fontbold1);
                                                table1.Cell(3, 0).SetContent("50x");
                                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(3, 0).SetFont(Fontbold1);
                                                table1.Cell(4, 0).SetContent("20x");
                                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(4, 0).SetFont(Fontbold1);
                                                table1.Cell(5, 0).SetContent("10x");
                                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(5, 0).SetFont(Fontbold1);
                                                table1.Cell(6, 0).SetContent("5x");
                                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(6, 0).SetFont(Fontbold1);
                                                table1.Cell(7, 0).SetContent("Coinsx");
                                                table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(7, 0).SetFont(Fontbold1);
                                                table1.Cell(8, 0).SetContent("Total");
                                                table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }


                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable1);

                                                myprov_pdfpage.Add(FC);
                                                myprov_pdfpage.Add(ORGI);
                                                myprov_pdfpage.Add(IOB);
                                                //myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(FC5);
                                                myprov_pdfpage.Add(FC6);
                                                myprov_pdfpage.Add(FC7);
                                                myprov_pdfpage.Add(FC8);
                                                myprov_pdfpage.Add(FC9);
                                                //myprov_pdfpage.Add(FC10);
                                                myprov_pdfpage.Add(FC11);
                                                myprov_pdfpage.Add(FC12);
                                                myprov_pdfpage.Add(FC13);
                                                myprov_pdfpage.Add(FC14);
                                                myprov_pdfpage.Add(FC15);
                                                myprov_pdfpage.Add(FC16);

                                                myprov_pdfpage.Add(FC24);
                                                myprov_pdfpage.Add(FC25);
                                                myprov_pdfpage.Add(FC26);
                                                myprov_pdfpage.Add(FC27);
                                                myprov_pdfpage.Add(FC28);
                                                myprov_pdfpage.Add(FC29);
                                                myprov_pdfpage.Add(FC30);
                                                myprov_pdfpage.Add(FC31);

                                                myprov_pdfpage.Add(FC32);
                                                //myprov_pdfpage.Add(FC33);

                                                //First End
                                                myprov_pdfpage.Add(UC17);

                                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table3.VisibleHeaders = false;
                                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table3.Columns[0].SetWidth(100);
                                                table3.Columns[1].SetWidth(60);
                                                table3.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table3.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table3.Cell(0, 0).SetContent("DD No");
                                                }
                                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 0).SetFont(Fontbold1);
                                                table3.Cell(0, 1).SetContent("Date");
                                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 1).SetFont(Fontbold1);
                                                table3.Cell(0, 2).SetContent("Amount");
                                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 2).SetFont(Fontbold1);
                                                table3.Cell(1, 0).SetContent("\n");
                                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 0).SetFont(Fontbold1);
                                                table3.Cell(1, 1).SetContent("\n");
                                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 1).SetFont(Fontbold1);
                                                table3.Cell(1, 2).SetContent("\n");
                                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table14.VisibleHeaders = false;
                                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table14.Columns[0].SetWidth(100);
                                                table14.Columns[1].SetWidth(60);
                                                table14.Cell(0, 0).SetContent("2000x");
                                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(0, 0).SetFont(Fontbold1);
                                                table14.Cell(1, 0).SetContent("500x");
                                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(1, 0).SetFont(Fontbold1);
                                                table14.Cell(2, 0).SetContent("100x");
                                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(2, 0).SetFont(Fontbold1);
                                                table14.Cell(3, 0).SetContent("50x");
                                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(3, 0).SetFont(Fontbold1);
                                                table14.Cell(4, 0).SetContent("20x");
                                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(4, 0).SetFont(Fontbold1);
                                                table14.Cell(5, 0).SetContent("10x");
                                                table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(5, 0).SetFont(Fontbold1);
                                                table14.Cell(6, 0).SetContent("5x");
                                                table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(6, 0).SetFont(Fontbold1);
                                                table14.Cell(7, 0).SetContent("Coinsx");
                                                table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(7, 0).SetFont(Fontbold1);
                                                table14.Cell(8, 0).SetContent("Total");
                                                table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable4);

                                                myprov_pdfpage.Add(UC);
                                                myprov_pdfpage.Add(UC1);
                                                myprov_pdfpage.Add(UC2);
                                                //myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(UC5);
                                                myprov_pdfpage.Add(UC6);
                                                myprov_pdfpage.Add(UC7);
                                                myprov_pdfpage.Add(UC8);
                                                myprov_pdfpage.Add(UC9);
                                                //myprov_pdfpage.Add(UC10);
                                                myprov_pdfpage.Add(UC11);
                                                myprov_pdfpage.Add(UC12);
                                                myprov_pdfpage.Add(UC13);
                                                myprov_pdfpage.Add(UC14);
                                                myprov_pdfpage.Add(UC15);
                                                myprov_pdfpage.Add(UC16);


                                                myprov_pdfpage.Add(UC24);
                                                myprov_pdfpage.Add(UC25);
                                                myprov_pdfpage.Add(UC26);
                                                myprov_pdfpage.Add(UC27);
                                                myprov_pdfpage.Add(UC28);
                                                myprov_pdfpage.Add(UC29);
                                                myprov_pdfpage.Add(UC30);
                                                myprov_pdfpage.Add(UC31);
                                                myprov_pdfpage.Add(UC32);
                                                //second End


                                                myprov_pdfpage.Add(TC17);

                                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table5.VisibleHeaders = false;
                                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table5.Columns[0].SetWidth(100);
                                                table5.Columns[1].SetWidth(60);
                                                table5.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table5.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table5.Cell(0, 0).SetContent("DD No");
                                                }
                                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 0).SetFont(Fontbold1);
                                                table5.Cell(0, 1).SetContent("Date");
                                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 1).SetFont(Fontbold1);
                                                table5.Cell(0, 2).SetContent("Amount");
                                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 2).SetFont(Fontbold1);
                                                table5.Cell(1, 0).SetContent("\n");
                                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 0).SetFont(Fontbold1);
                                                table5.Cell(1, 1).SetContent("\n");
                                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 1).SetFont(Fontbold1);
                                                table5.Cell(1, 2).SetContent("\n");
                                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table15.VisibleHeaders = false;
                                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table15.Columns[0].SetWidth(100);
                                                table15.Columns[1].SetWidth(60);
                                                table15.Cell(0, 0).SetContent("2000x");
                                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(0, 0).SetFont(Fontbold1);
                                                table15.Cell(1, 0).SetContent("500x");
                                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(1, 0).SetFont(Fontbold1);
                                                table15.Cell(2, 0).SetContent("100x");
                                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(2, 0).SetFont(Fontbold1);
                                                table15.Cell(3, 0).SetContent("50x");
                                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(3, 0).SetFont(Fontbold1);
                                                table15.Cell(4, 0).SetContent("20x");
                                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(4, 0).SetFont(Fontbold1);
                                                table15.Cell(5, 0).SetContent("10x");
                                                table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(5, 0).SetFont(Fontbold1);
                                                table15.Cell(6, 0).SetContent("5x");
                                                table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(6, 0).SetFont(Fontbold1);
                                                table15.Cell(7, 0).SetContent("Coinsx");
                                                table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(7, 0).SetFont(Fontbold1);
                                                table15.Cell(8, 0).SetContent("Total");
                                                table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(8, 0).SetFont(Fontbold1);
                                                table15.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                      new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                                myprov_pdfpage.Add(TC);
                                                myprov_pdfpage.Add(TC1);
                                                myprov_pdfpage.Add(TC2);
                                                //myprov_pdfpage.Add(TC4);
                                                myprov_pdfpage.Add(TC5);
                                                myprov_pdfpage.Add(TC6);
                                                myprov_pdfpage.Add(TC7);
                                                myprov_pdfpage.Add(TC8);
                                                myprov_pdfpage.Add(TC9);
                                                //myprov_pdfpage.Add(TC10);
                                                myprov_pdfpage.Add(TC11);
                                                myprov_pdfpage.Add(TC12);
                                                myprov_pdfpage.Add(TC13);
                                                myprov_pdfpage.Add(TC14);
                                                myprov_pdfpage.Add(TC15);
                                                myprov_pdfpage.Add(TC16);
                                                myprov_pdfpage.Add(TC17);
                                                myprov_pdfpage.Add(TC24);
                                                myprov_pdfpage.Add(TC25);
                                                myprov_pdfpage.Add(TC26);
                                                myprov_pdfpage.Add(TC27);
                                                myprov_pdfpage.Add(TC28);
                                                myprov_pdfpage.Add(TC29);
                                                myprov_pdfpage.Add(TC30);
                                                myprov_pdfpage.Add(TC31);
                                                myprov_pdfpage.Add(TC32);

                                                myprov_pdfpage.SaveToDocument();
                                                #endregion
                                            }
                                            //Bottom portion of the challan End

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        checkedHeaderOK = false;
                                        string bnkAcc = "";
                                        //Add new challan Page in this loop
                                        bool addpageOK = false;
                                        #region TOp portion

                                        int y = 0;

                                        myprov_pdfpage = mychallan.NewPage();

                                        PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea ORGI;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                        }
                                        else
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                        }

                                        PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 90, 50, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                        //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                        PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea FC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                             FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                             FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }
                                        PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        double ovrallcredit = 0;
                                        double grandtotal = 0.00;


                                        myprov_pdfpage.Add(FC17);
                                        string text = "";

                                        //First Ends

                                        PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea UC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                        }
                                        else
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                        }
                                        PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                        PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea UC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                             UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                             UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }
                                        PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        //second End
                                        y = 0;


                                        PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea TC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                        }
                                        else
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                        }
                                        PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea TC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                             TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }
                                        PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        myprov_pdfpage.Add(FC10);
                                        myprov_pdfpage.Add(UC10);
                                        myprov_pdfpage.Add(TC10);

                                        y = 0;

                                        #endregion
                                        if (rbl_headerselect.SelectedIndex == 1)
                                        {
                                            #region Middle Portion challan
                                            int chk = 0;
                                            for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                            {

                                                if (!cbl_grpheader.Items[indx].Selected)
                                                {
                                                    continue;
                                                }

                                                //string QhdrId = "select header_id,ChlHeaderName,BankAccNo from ChlHeaderSettings where Stream = '" + stream + "' and header_id in ('" + cbl_grpheader.Items[indx].Value + "')";
                                                string HdrId = "";
                                                string dispHdr = "";

                                                //DataSet ds1 = new DataSet();
                                                //ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                                //if (ds1.Tables.Count > 0)
                                                //{
                                                //    if (ds1.Tables[0].Rows.Count > 0)
                                                //    {

                                                //        bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                                //    }
                                                //}
                                                dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);
                                                HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);



                                                string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";

                                                DataSet ds2 = new DataSet();
                                                ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                if (ds2.Tables.Count > 0)
                                                {
                                                    if (ds2.Tables[0].Rows.Count > 0)
                                                    {
                                                        double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                        if (ds2.Tables[1].Rows.Count > 0)
                                                        {
                                                            totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                        }
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                        dispHdr += " (" + bnkAcc + ")";
                                                        grandtotal = grandtotal + totalAmt;

                                                        if (grandtotal > 0)
                                                        {

                                                            addpageOK = true;
                                                            createPDFOK = true;
                                                            if (totalAmt > 0)
                                                            {
                                                                if (chk == 0)
                                                                {
                                                                    //chk++;
                                                                    #region Update Challan No
                                                                    recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                    txt_rcptno.Text = recptNo;

                                                                    //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                    //{
                                                                    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                    DataSet dsEachHdr = new DataSet();
                                                                    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                    if (dsEachHdr.Tables.Count > 0)
                                                                    {
                                                                        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')   order by case when priority is null then 1 else 0 end, priority ";
                                                                            DataSet dsLedge = new DataSet();
                                                                            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                            if (dsLedge.Tables.Count > 0)
                                                                            {
                                                                                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                    {
                                                                                        double remainAmt = 0;
                                                                                        remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                        if (remainAmt > 0)
                                                                                        {
                                                                                            string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                            d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                            string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                            d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    //}


                                                                    #endregion
                                                                }
                                                            }
                                                        }
                                                        if (totalAmt > 0)
                                                        {
                                                            PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(FC18);
                                                            myprov_pdfpage.Add(FC171);
                                                            myprov_pdfpage.Add(FC19);


                                                            PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(UC18);
                                                            myprov_pdfpage.Add(UC19);
                                                            myprov_pdfpage.Add(UC171);

                                                            PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(TC18);
                                                            myprov_pdfpage.Add(TC19);
                                                            myprov_pdfpage.Add(TC171);
                                                            y = y + 15;
                                                        }
                                                    }
                                                }



                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Middle Portion challan
                                            int chk = 0;
                                            for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                            {

                                                if (!cbl_grpheader.Items[indx].Selected)
                                                {
                                                    continue;
                                                }

                                                //string QhdrId = "select header_id,ChlHeaderName,BankAccNo from ChlHeaderSettings where Stream = '" + stream + "' and header_id in ('" + cbl_grpheader.Items[indx].Value + "')";
                                                string HdrId = "";
                                                string dispHdr = "";

                                                //DataSet ds1 = new DataSet();
                                                //ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                                //if (ds1.Tables.Count > 0)
                                                //{
                                                //    if (ds1.Tables[0].Rows.Count > 0)
                                                //    {

                                                //        bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                                //    }
                                                //}
                                                dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);
                                                HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);


                                                string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND  A.LedgerFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + " and a.balamount!=0	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.LedgerFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                                DataSet ds2 = new DataSet();
                                                ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                if (ds2.Tables.Count > 0)
                                                {
                                                    if (ds2.Tables[0].Rows.Count > 0)
                                                    {
                                                        double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                        if (ds2.Tables[1].Rows.Count > 0)
                                                        {
                                                            totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                        }
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                        dispHdr += " (" + bnkAcc + ")";
                                                        grandtotal = grandtotal + totalAmt;

                                                        if (grandtotal > 0)
                                                        {

                                                            addpageOK = true;
                                                            createPDFOK = true;
                                                            if (totalAmt > 0)
                                                            {
                                                                if (chk == 0)
                                                                {
                                                                    //chk++;
                                                                    #region Update Challan No
                                                                    recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                    txt_rcptno.Text = recptNo;

                                                                    //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                    //{
                                                                    //string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                                    //DataSet dsEachHdr = new DataSet();
                                                                    //dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                    //if (dsEachHdr.Tables.Count > 0)
                                                                    //{
                                                                    //    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                    //    {
                                                                    string hdrfk = d2.GetFunction("select HeaderFK  from FM_LedgerMaster where LedgerPK ='" + HdrId + "'");
                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority ,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE   l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and   f.HeaderFK = " + hdrfk + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') and LedgerFk='" + HdrId + "' order by case when priority is null then 1 else 0 end, priority";
                                                                    DataSet dsLedge = new DataSet();
                                                                    dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                    if (dsLedge.Tables.Count > 0)
                                                                    {
                                                                        if (dsLedge.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                            {
                                                                                double remainAmt = 0;
                                                                                remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                if (remainAmt > 0)
                                                                                {
                                                                                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + hdrfk + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                    string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + hdrfk + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                    d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    //    }
                                                                    //}

                                                                    //}

                                                                    #endregion
                                                                }
                                                            }
                                                        }
                                                        if (totalAmt > 0)
                                                        {
                                                            PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(FC18);
                                                            myprov_pdfpage.Add(FC171);
                                                            myprov_pdfpage.Add(FC19);


                                                            PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(UC18);
                                                            myprov_pdfpage.Add(UC19);
                                                            myprov_pdfpage.Add(UC171);

                                                            PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                            PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                            PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                            myprov_pdfpage.Add(TC18);
                                                            myprov_pdfpage.Add(TC19);
                                                            myprov_pdfpage.Add(TC171);
                                                            y = y + 15;
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        //Bottom portion of the challan
                                        if (addpageOK)
                                        {
                                            string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                            d2.update_method_wo_parameter(updateRecpt, "Text");
                                            PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                  new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);


                                            myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(TC4);
                                            PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                            //myprov_pdfpage.Add(FC08, 250, 125);
                                            //myprov_pdfpage.Add(FC08, 550, 125);
                                            //myprov_pdfpage.Add(FC08, 900, 125);
                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                            PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                            PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                            PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                            myprov_pdfpage.Add(pr1);

                                            PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                            PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                            myprov_pdfpage.Add(pr2);

                                            PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                            PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                            myprov_pdfpage.Add(pr3);

                                            Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table.VisibleHeaders = false;
                                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table.Columns[0].SetWidth(100);
                                            table.Columns[1].SetWidth(60);
                                            table.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table.Cell(0, 0).SetContent("DD No");
                                            }
                                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 0).SetFont(Fontbold1);
                                            table.Cell(0, 1).SetContent("Date");
                                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 1).SetFont(Fontbold1);
                                            table.Cell(0, 2).SetContent("Amount");
                                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 2).SetFont(Fontbold1);
                                            table.Cell(1, 0).SetContent("\n");
                                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 0).SetFont(Fontbold1);
                                            table.Cell(1, 1).SetContent("\n");
                                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 1).SetFont(Fontbold1);
                                            table.Cell(1, 2).SetContent("\n");
                                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable);

                                            Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table1.VisibleHeaders = false;
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table1.Columns[0].SetWidth(100);
                                            table1.Columns[1].SetWidth(60);
                                            table1.Cell(0, 0).SetContent("2000x");
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(0, 0).SetFont(Fontbold1);
                                            table1.Cell(1, 0).SetContent("500x");
                                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(1, 0).SetFont(Fontbold1);
                                            table1.Cell(2, 0).SetContent("100x");
                                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(2, 0).SetFont(Fontbold1);
                                            table1.Cell(3, 0).SetContent("50x");
                                            table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(3, 0).SetFont(Fontbold1);
                                            table1.Cell(4, 0).SetContent("20x");
                                            table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(4, 0).SetFont(Fontbold1);
                                            table1.Cell(5, 0).SetContent("10x");
                                            table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(5, 0).SetFont(Fontbold1);
                                            table1.Cell(6, 0).SetContent("5x");
                                            table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(6, 0).SetFont(Fontbold1);
                                            table1.Cell(7, 0).SetContent("Coinsx");
                                            table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(7, 0).SetFont(Fontbold1);
                                            table1.Cell(8, 0).SetContent("Total");
                                            table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }


                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable1);

                                            myprov_pdfpage.Add(FC);
                                            myprov_pdfpage.Add(ORGI);
                                            myprov_pdfpage.Add(IOB);
                                            //myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(FC5);
                                            myprov_pdfpage.Add(FC6);
                                            myprov_pdfpage.Add(FC7);
                                            myprov_pdfpage.Add(FC8);
                                            myprov_pdfpage.Add(FC9);
                                            //myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(FC11);
                                            myprov_pdfpage.Add(FC12);
                                            myprov_pdfpage.Add(FC13);
                                            myprov_pdfpage.Add(FC14);
                                            myprov_pdfpage.Add(FC15);
                                            myprov_pdfpage.Add(FC16);

                                            myprov_pdfpage.Add(FC24);
                                            myprov_pdfpage.Add(FC25);
                                            myprov_pdfpage.Add(FC26);
                                            myprov_pdfpage.Add(FC27);
                                            myprov_pdfpage.Add(FC28);
                                            myprov_pdfpage.Add(FC29);
                                            myprov_pdfpage.Add(FC30);
                                            myprov_pdfpage.Add(FC31);

                                            myprov_pdfpage.Add(FC32);
                                            //myprov_pdfpage.Add(FC33);

                                            //First End
                                            myprov_pdfpage.Add(UC17);

                                            PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table3.VisibleHeaders = false;
                                            table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table3.Columns[0].SetWidth(100);
                                            table3.Columns[1].SetWidth(60);
                                            table3.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table3.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table3.Cell(0, 0).SetContent("DD No");
                                            }
                                            table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 0).SetFont(Fontbold1);
                                            table3.Cell(0, 1).SetContent("Date");
                                            table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 1).SetFont(Fontbold1);
                                            table3.Cell(0, 2).SetContent("Amount");
                                            table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 2).SetFont(Fontbold1);
                                            table3.Cell(1, 0).SetContent("\n");
                                            table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 0).SetFont(Fontbold1);
                                            table3.Cell(1, 1).SetContent("\n");
                                            table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 1).SetFont(Fontbold1);
                                            table3.Cell(1, 2).SetContent("\n");
                                            table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable3);

                                            Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table14.VisibleHeaders = false;
                                            table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table14.Columns[0].SetWidth(100);
                                            table14.Columns[1].SetWidth(60);
                                            table14.Cell(0, 0).SetContent("2000x");
                                            table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(0, 0).SetFont(Fontbold1);
                                            table14.Cell(1, 0).SetContent("500x");
                                            table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(1, 0).SetFont(Fontbold1);
                                            table14.Cell(2, 0).SetContent("100x");
                                            table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(2, 0).SetFont(Fontbold1);
                                            table14.Cell(3, 0).SetContent("50x");
                                            table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(3, 0).SetFont(Fontbold1);
                                            table14.Cell(4, 0).SetContent("20x");
                                            table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(4, 0).SetFont(Fontbold1);
                                            table14.Cell(5, 0).SetContent("10x");
                                            table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(5, 0).SetFont(Fontbold1);
                                            table14.Cell(6, 0).SetContent("5x");
                                            table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(6, 0).SetFont(Fontbold1);
                                            table14.Cell(7, 0).SetContent("Coinsx");
                                            table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(7, 0).SetFont(Fontbold1);
                                            table14.Cell(8, 0).SetContent("Total");
                                            table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable4);

                                            myprov_pdfpage.Add(UC);
                                            myprov_pdfpage.Add(UC1);
                                            myprov_pdfpage.Add(UC2);
                                            //myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(UC5);
                                            myprov_pdfpage.Add(UC6);
                                            myprov_pdfpage.Add(UC7);
                                            myprov_pdfpage.Add(UC8);
                                            myprov_pdfpage.Add(UC9);
                                            //myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(UC11);
                                            myprov_pdfpage.Add(UC12);
                                            myprov_pdfpage.Add(UC13);
                                            myprov_pdfpage.Add(UC14);
                                            myprov_pdfpage.Add(UC15);
                                            myprov_pdfpage.Add(UC16);


                                            myprov_pdfpage.Add(UC24);
                                            myprov_pdfpage.Add(UC25);
                                            myprov_pdfpage.Add(UC26);
                                            myprov_pdfpage.Add(UC27);
                                            myprov_pdfpage.Add(UC28);
                                            myprov_pdfpage.Add(UC29);
                                            myprov_pdfpage.Add(UC30);
                                            myprov_pdfpage.Add(UC31);
                                            myprov_pdfpage.Add(UC32);
                                            //second End


                                            myprov_pdfpage.Add(TC17);

                                            PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table5.VisibleHeaders = false;
                                            table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table5.Columns[0].SetWidth(100);
                                            table5.Columns[1].SetWidth(60);
                                            table5.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table5.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table5.Cell(0, 0).SetContent("DD No");
                                            }
                                            table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 0).SetFont(Fontbold1);
                                            table5.Cell(0, 1).SetContent("Date");
                                            table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 1).SetFont(Fontbold1);
                                            table5.Cell(0, 2).SetContent("Amount");
                                            table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 2).SetFont(Fontbold1);
                                            table5.Cell(1, 0).SetContent("\n");
                                            table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 0).SetFont(Fontbold1);
                                            table5.Cell(1, 1).SetContent("\n");
                                            table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 1).SetFont(Fontbold1);
                                            table5.Cell(1, 2).SetContent("\n");
                                            table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable31);

                                            Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table15.VisibleHeaders = false;
                                            table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table15.Columns[0].SetWidth(100);
                                            table15.Columns[1].SetWidth(60);
                                            table15.Cell(0, 0).SetContent("2000x");
                                            table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(0, 0).SetFont(Fontbold1);
                                            table15.Cell(1, 0).SetContent("500x");
                                            table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(1, 0).SetFont(Fontbold1);
                                            table15.Cell(2, 0).SetContent("100x");
                                            table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(2, 0).SetFont(Fontbold1);
                                            table15.Cell(3, 0).SetContent("50x");
                                            table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(3, 0).SetFont(Fontbold1);
                                            table15.Cell(4, 0).SetContent("20x");
                                            table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(4, 0).SetFont(Fontbold1);
                                            table15.Cell(5, 0).SetContent("10x");
                                            table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(5, 0).SetFont(Fontbold1);
                                            table15.Cell(6, 0).SetContent("5x");
                                            table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(6, 0).SetFont(Fontbold1);
                                            table15.Cell(7, 0).SetContent("Coinsx");
                                            table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(7, 0).SetFont(Fontbold1);
                                            table15.Cell(8, 0).SetContent("Total");
                                            table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(8, 0).SetFont(Fontbold1);
                                            table15.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                  new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable5);

                                            myprov_pdfpage.Add(TC);
                                            myprov_pdfpage.Add(TC1);
                                            myprov_pdfpage.Add(TC2);
                                            //myprov_pdfpage.Add(TC4);
                                            myprov_pdfpage.Add(TC5);
                                            myprov_pdfpage.Add(TC6);
                                            myprov_pdfpage.Add(TC7);
                                            myprov_pdfpage.Add(TC8);
                                            myprov_pdfpage.Add(TC9);
                                            //myprov_pdfpage.Add(TC10);
                                            myprov_pdfpage.Add(TC11);
                                            myprov_pdfpage.Add(TC12);
                                            myprov_pdfpage.Add(TC13);
                                            myprov_pdfpage.Add(TC14);
                                            myprov_pdfpage.Add(TC15);
                                            myprov_pdfpage.Add(TC16);
                                            myprov_pdfpage.Add(TC17);
                                            myprov_pdfpage.Add(TC24);
                                            myprov_pdfpage.Add(TC25);
                                            myprov_pdfpage.Add(TC26);
                                            myprov_pdfpage.Add(TC27);
                                            myprov_pdfpage.Add(TC28);
                                            myprov_pdfpage.Add(TC29);
                                            myprov_pdfpage.Add(TC30);
                                            myprov_pdfpage.Add(TC31);
                                            myprov_pdfpage.Add(TC32);

                                            myprov_pdfpage.SaveToDocument();
                                            #endregion
                                        }
                                        //Bottom portion of the challan End
                                    }
                                    //Middle portion of challan End




                                    #endregion
                                }
                                #endregion
                            }
                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); }

                            #endregion
                        }
                    }

                    if (createPDFOK && count > 0)
                    {
                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {
                            string szPath = appPath + "/Report/";
                            string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                            mychallan.SaveToFile(szPath + szFile);
                            //Response.ClearHeaders();
                            //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            //Response.ContentType = "application/pdf";
                            //Response.WriteFile(szPath + szFile);
                            //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");

                            imgDIVVisible = true;
                            //this.Form.DefaultButton = "btn_alertclose";
                            lbl_alert.Text = "Challan Generated";
                            CreateReceiptOK = true;
                            return szFile;
                        }
                    }
                    else
                    {
                        imgDIVVisible = true;
                        //this.Form.DefaultButton = "btn_alertclose";
                        lbl_alert.Text = "Challan Cannot Be Generated";
                    }
                    #endregion
                }
                else
                {
                    #region For Single Student
                    try
                    {
                        //Basic Data
                        roll_admit = lblstaticrollno.Trim();
                        string recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                        string recptDt = txt_date.Text.Trim().Split('/')[1] + "/" + txt_date.Text.Trim().Split('/')[0] + "/" + txt_date.Text.Trim().Split('/')[2];
                        string studname = txt_name.Text.Trim();
                        string course = ddlDEPT;
                        string batchYrSem = string.Empty;
                        string Regno = string.Empty;
                        string rollno = string.Empty;
                        string appnoNew = string.Empty;
                        string regno = string.Empty;
                        string degreeCode = string.Empty;
                        string stream = ddlTYPE;
                        string feeCategory = string.Empty;
                        string app_formno = string.Empty;
                        string smartno = string.Empty;
                        string sem = string.Empty;
                        feeCategory = Convert.ToString(ddlSEM);
                        if (checkSchoolSetting(usercode) == 0)
                        {
                            sem = Convert.ToString(ddl_sem.SelectedItem.Text);
                        }
                        string queryRollApp = "select r.Roll_No,a.app_formno,r.smart_serial_no,a.app_no,r.Reg_No  from Registration r,applyn a where r.App_No=a.app_no  and r.college_code='" + collegecode1 + "'  and r.Roll_Admit='" + roll_admit + "'";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0)
                        {
                            if (dsRollApp.Tables[0].Rows.Count > 0)
                            {
                                rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
                            }
                        }

                        string rolldisplay = "Admission No :";
                        string rollvalue = roll_admit;
                        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
                        {
                            case 0:

                            case1:
                                rolldisplay = "Roll No :";
                                rollvalue = rollno;
                                break;
                            case 1:
                            case2:
                                rolldisplay = "Reg No :";
                                rollvalue = Regno;
                                break;
                            case 2:
                            case3:
                                rolldisplay = "Admission No :";
                                rollvalue = roll_admit;
                                break;
                            case 4:
                                int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'").Trim());
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
                            case 3:
                            case4:
                                appnoNew = getAppNoFromApplyn(roll_admit, collegecode1);
                                rolldisplay = "App No :";
                                rollvalue = app_formno = d2.GetFunction("select app_formno from applyn where app_no='" + appnoNew + "'").Trim();
                                break;
                        }

                        //string colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        string colquery = "";
                        if (rolldisplay != "App No :")
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }
                        else
                        {
                            colquery = "select collname,university,address1+' '+address2+' '+address3 as address1,' - '+pincode as address2 from collinfo where college_code=" + collegecode1 + " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                        }

                        string collegename = "";
                        string add1 = "";
                        string add2 = "";
                        string univ = "";
                        string deg = "";
                        string cursem = "";
                        string batyr = "";
                        string bankName = "";
                        string bankPK = "";
                        string bankCity = "";
                        if (ddl_collegebank.Items.Count > 0)
                        {
                            bankName = ddl_collegebank.SelectedItem.Text.Split('-')[0];
                            bankPK = ddl_collegebank.SelectedItem.Value;
                            bankCity = "(" + ddl_collegebank.SelectedItem.Text.Split('-')[2] + ")";
                        }

                        DataSet ds = new DataSet();
                        ds = d2.select_method_wo_parameter(colquery, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                //if (degACR == 0)
                                //{
                                deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                //}
                                //else
                                //{
                                // deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                //}
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);

                                cursem = Convert.ToString(ddl_sem.SelectedItem.Text).Split(' ')[1] + " : " + romanLetter(Convert.ToString(ddl_sem.SelectedItem.Text).Split(' ')[0]);
                            }
                        }

                        bool createPDFOK = false;
                        #region PDF Generation
                        Font Fontbold = new Font("Arial", 8, FontStyle.Bold);
                        Font Fontsmall = new Font("Arial", 8, FontStyle.Bold);
                        Font Fontsmall1 = new Font("Arial", 10, FontStyle.Bold);
                        Font Fontbold1 = new Font("Arial", 10, FontStyle.Bold);

                        Gios.Pdf.PdfDocument mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
                        // mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(14.0, 8.5));
                        mychallan = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InInches(13.8, 8.5));
                        // New Code



                        if (!cb_selcthd.Checked)
                        {
                            bool checkedHeaderOK = false;
                            string groupHdr;
                            string[] hdrInGrp0;
                            List<string> hdrInGrp = new List<string>();
                            if (Txt_amt.Text == "0" || Txt_amt.Text == "0.00" || Txt_amt.Text == "")
                            {
                                #region For Overall
                                string QHdrForGroup = "	SELECT distinct ChlGroupHeader FROM FM_ChlBankPrintSettings WHERE DegreeCode = '" + degreeCode + "' AND SettingType = 1 and CollegeCode=" + collegecode1 + " ";

                                ds.Clear();
                                ds = d2.select_method_wo_parameter(QHdrForGroup, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            string bnkAcc = "";
                                            checkedHeaderOK = false;
                                            groupHdr = Convert.ToString(ds.Tables[0].Rows[i][0]);
                                            hdrInGrp0 = groupHdr.Split(',');
                                            hdrInGrp.Clear();
                                            foreach (string item in hdrInGrp0)
                                            {
                                                hdrInGrp.Add(item);
                                                checkedHeaderOK = true;
                                            }



                                            if (!checkedHeaderOK)
                                            {
                                                continue;
                                            }

                                            //Add new challan Page in this loop
                                            bool addpageOK = false;
                                            #region TOp portion

                                            int y = 0;

                                            Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                            PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea ORGI;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                            }
                                            else
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                            }

                                            PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                            //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                            //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                            PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                            PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                            PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea FC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);

                                            }
                                            PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;


                                            myprov_pdfpage.Add(FC17);
                                            string text = "";

                                            //First Ends

                                            PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea UC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                            }
                                            else
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                            }
                                            PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                            PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);


                                            PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea UC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);
                                            }
                                            PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            //second End
                                            y = 0;


                                            PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea TC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                            }
                                            else
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                            }
                                            PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                            PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                            PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea TC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);
                                            }
                                            PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(TC10);
                                            y = 0;

                                            #endregion

                                            //End of  New CHallan Top Portion

                                            //Middle portion of the challan
                                            #region Middle Portion challan
                                            int chk = 0;
                                            for (int indx = 0; indx < hdrInGrp.Count; indx++)
                                            {

                                                string QhdrId = "SELECT HeaderFK  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and Stream='" + stream + "'";
                                                string HdrId = "";
                                                string dispHdr = "";

                                                DataSet ds1 = new DataSet();
                                                ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                                if (ds1.Tables.Count > 0)
                                                {
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                        {
                                                            if (HdrId == "")
                                                            {
                                                                HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);

                                                            }
                                                            else
                                                            {
                                                                HdrId += "," + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                            }
                                                        }

                                                        string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                                        DataSet ds2 = new DataSet();
                                                        ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                        if (ds2.Tables.Count > 0)
                                                        {
                                                            if (ds2.Tables[0].Rows.Count > 0)
                                                            {

                                                                dispHdr = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                                string hdrNme = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]).Trim().ToUpper();
                                                                double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                                if (ds2.Tables[1].Rows.Count > 0)
                                                                {
                                                                    totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                                }
                                                                //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                                bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                                dispHdr += " (" + bnkAcc + ")";
                                                                grandtotal = grandtotal + totalAmt;

                                                                if (grandtotal > 0 || hdrNme == "TUITION FEE")
                                                                {
                                                                    addpageOK = true;
                                                                    createPDFOK = true;
                                                                    if (totalAmt > 0 || hdrNme == "TUITION FEE")
                                                                    {
                                                                        if (chk == 0)
                                                                        {
                                                                            //chk++;
                                                                            #region Update Challan No

                                                                            recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                            txt_rcptno.Text = recptNo;


                                                                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                            {
                                                                                string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + " and FeeCategory in ('" + feeCategory + "')GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                                DataSet dsEachHdr = new DataSet();
                                                                                dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                                if (dsEachHdr.Tables.Count > 0)
                                                                                {
                                                                                    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                                    {

                                                                                        string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and FeeCategory in ('" + feeCategory + "') and App_No=" + appnoNew + "  order by case when priority is null then 1 else 0 end, priority ";
                                                                                        DataSet dsLedge = new DataSet();
                                                                                        dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                                        if (dsLedge.Tables.Count > 0)
                                                                                        {
                                                                                            if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                            {
                                                                                                for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                                {
                                                                                                    double remainAmt = 0;
                                                                                                    remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                                    if (remainAmt > 0 || hdrNme == "TUITION FEE")
                                                                                                    {
                                                                                                        string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                                        d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                                        string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                                        d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }

                                                                            }

                                                                            #endregion
                                                                        }
                                                                    }
                                                                }

                                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                 new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                myprov_pdfpage.Add(FC18);
                                                                myprov_pdfpage.Add(FC171);
                                                                myprov_pdfpage.Add(FC19);


                                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                 new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                myprov_pdfpage.Add(UC18);
                                                                myprov_pdfpage.Add(UC19);
                                                                myprov_pdfpage.Add(UC171);

                                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                 new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                    new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                                myprov_pdfpage.Add(TC18);
                                                                myprov_pdfpage.Add(TC19);
                                                                myprov_pdfpage.Add(TC171);
                                                                y = y + 15;

                                                            }
                                                        }

                                                    }
                                                }


                                            }
                                            #endregion
                                            //Middle portion of challan End

                                            //Bottom portion of the challan
                                            if (addpageOK)
                                            {
                                                string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                   new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                                myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(TC4);
                                                PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                          new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);

                                                //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                                //myprov_pdfpage.Add(FC08, 250, 125);
                                                //myprov_pdfpage.Add(FC08, 550, 125);
                                                //myprov_pdfpage.Add(FC08, 900, 125);

                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                                PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                                myprov_pdfpage.Add(pr1);

                                                PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                                myprov_pdfpage.Add(pr2);

                                                PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                                myprov_pdfpage.Add(pr3);

                                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table.VisibleHeaders = false;
                                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table.Columns[0].SetWidth(100);
                                                table.Columns[1].SetWidth(60);
                                                table.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table.Cell(0, 0).SetContent("DD No");
                                                }
                                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 0).SetFont(Fontbold1);
                                                table.Cell(0, 1).SetContent("Date");
                                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 1).SetFont(Fontbold1);
                                                table.Cell(0, 2).SetContent("Amount");
                                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 2).SetFont(Fontbold1);
                                                table.Cell(1, 0).SetContent("\n");
                                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 0).SetFont(Fontbold1);
                                                table.Cell(1, 1).SetContent("\n");
                                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 1).SetFont(Fontbold1);
                                                table.Cell(1, 2).SetContent("\n");
                                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table1.VisibleHeaders = false;
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Columns[0].SetWidth(100);
                                                table1.Columns[1].SetWidth(60);
                                                table1.Cell(0, 0).SetContent("2000x");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(0, 0).SetFont(Fontbold1);
                                                table1.Cell(1, 0).SetContent("500x");
                                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(1, 0).SetFont(Fontbold1);
                                                table1.Cell(2, 0).SetContent("100x");
                                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(2, 0).SetFont(Fontbold1);
                                                table1.Cell(3, 0).SetContent("50x");
                                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(3, 0).SetFont(Fontbold1);
                                                table1.Cell(4, 0).SetContent("20x");
                                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(4, 0).SetFont(Fontbold1);
                                                table1.Cell(5, 0).SetContent("10x");
                                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(5, 0).SetFont(Fontbold1);
                                                table1.Cell(6, 0).SetContent("5x");
                                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(6, 0).SetFont(Fontbold1);
                                                table1.Cell(7, 0).SetContent("Coinsx");
                                                table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(7, 0).SetFont(Fontbold1);
                                                table1.Cell(8, 0).SetContent("Total");
                                                table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(8, 0).SetFont(Fontbold1);

                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }

                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable1);

                                                myprov_pdfpage.Add(FC);
                                                myprov_pdfpage.Add(ORGI);
                                                myprov_pdfpage.Add(IOB);
                                                //myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(FC5);
                                                myprov_pdfpage.Add(FC6);
                                                myprov_pdfpage.Add(FC7);
                                                myprov_pdfpage.Add(FC8);
                                                myprov_pdfpage.Add(FC9);
                                                //myprov_pdfpage.Add(FC10);
                                                myprov_pdfpage.Add(FC11);
                                                myprov_pdfpage.Add(FC12);
                                                myprov_pdfpage.Add(FC13);
                                                myprov_pdfpage.Add(FC14);
                                                myprov_pdfpage.Add(FC15);
                                                myprov_pdfpage.Add(FC16);

                                                myprov_pdfpage.Add(FC24);
                                                myprov_pdfpage.Add(FC25);
                                                myprov_pdfpage.Add(FC26);
                                                myprov_pdfpage.Add(FC27);
                                                myprov_pdfpage.Add(FC28);
                                                myprov_pdfpage.Add(FC29);
                                                myprov_pdfpage.Add(FC30);
                                                myprov_pdfpage.Add(FC31);

                                                myprov_pdfpage.Add(FC32);
                                                //myprov_pdfpage.Add(FC33);

                                                //First End
                                                myprov_pdfpage.Add(UC17);

                                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table3.VisibleHeaders = false;
                                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table3.Columns[0].SetWidth(100);
                                                table3.Columns[1].SetWidth(60);
                                                table3.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table3.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table3.Cell(0, 0).SetContent("DD No");
                                                }
                                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 0).SetFont(Fontbold1);
                                                table3.Cell(0, 1).SetContent("Date");
                                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 1).SetFont(Fontbold1);
                                                table3.Cell(0, 2).SetContent("Amount");
                                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 2).SetFont(Fontbold1);
                                                table3.Cell(1, 0).SetContent("\n");
                                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 0).SetFont(Fontbold1);
                                                table3.Cell(1, 1).SetContent("\n");
                                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 1).SetFont(Fontbold1);
                                                table3.Cell(1, 2).SetContent("\n");
                                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table14.VisibleHeaders = false;
                                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table14.Columns[0].SetWidth(100);
                                                table14.Columns[1].SetWidth(60);
                                                table14.Cell(0, 0).SetContent("2000x");
                                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(0, 0).SetFont(Fontbold1);
                                                table14.Cell(1, 0).SetContent("500x");
                                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(1, 0).SetFont(Fontbold1);
                                                table14.Cell(2, 0).SetContent("100x");
                                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(2, 0).SetFont(Fontbold1);
                                                table14.Cell(3, 0).SetContent("50x");
                                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(3, 0).SetFont(Fontbold1);
                                                table14.Cell(4, 0).SetContent("20x");
                                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(4, 0).SetFont(Fontbold1);
                                                table14.Cell(5, 0).SetContent("10x");
                                                table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(5, 0).SetFont(Fontbold1);
                                                table14.Cell(6, 0).SetContent("5x");
                                                table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(6, 0).SetFont(Fontbold1);
                                                table14.Cell(7, 0).SetContent("Coinsx");
                                                table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(7, 0).SetFont(Fontbold1);
                                                table14.Cell(8, 0).SetContent("Total");
                                                table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable4);

                                                myprov_pdfpage.Add(UC);
                                                myprov_pdfpage.Add(UC1);
                                                myprov_pdfpage.Add(UC2);
                                                //myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(UC5);
                                                myprov_pdfpage.Add(UC6);
                                                myprov_pdfpage.Add(UC7);
                                                myprov_pdfpage.Add(UC8);
                                                myprov_pdfpage.Add(UC9);
                                                //myprov_pdfpage.Add(UC10);
                                                myprov_pdfpage.Add(UC11);
                                                myprov_pdfpage.Add(UC12);
                                                myprov_pdfpage.Add(UC13);
                                                myprov_pdfpage.Add(UC14);
                                                myprov_pdfpage.Add(UC15);
                                                myprov_pdfpage.Add(UC16);


                                                myprov_pdfpage.Add(UC24);
                                                myprov_pdfpage.Add(UC25);
                                                myprov_pdfpage.Add(UC26);
                                                myprov_pdfpage.Add(UC27);
                                                myprov_pdfpage.Add(UC28);
                                                myprov_pdfpage.Add(UC29);
                                                myprov_pdfpage.Add(UC30);
                                                myprov_pdfpage.Add(UC31);
                                                myprov_pdfpage.Add(UC32);
                                                //second End


                                                myprov_pdfpage.Add(TC17);

                                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table5.VisibleHeaders = false;
                                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table5.Columns[0].SetWidth(100);
                                                table5.Columns[1].SetWidth(60);
                                                table5.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table5.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table5.Cell(0, 0).SetContent("DD No");
                                                }
                                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 0).SetFont(Fontbold1);
                                                table5.Cell(0, 1).SetContent("Date");
                                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 1).SetFont(Fontbold1);
                                                table5.Cell(0, 2).SetContent("Amount");
                                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 2).SetFont(Fontbold1);
                                                table5.Cell(1, 0).SetContent("\n");
                                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 0).SetFont(Fontbold1);
                                                table5.Cell(1, 1).SetContent("\n");
                                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 1).SetFont(Fontbold1);
                                                table5.Cell(1, 2).SetContent("\n");
                                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table15.VisibleHeaders = false;
                                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table15.Columns[0].SetWidth(100);
                                                table15.Columns[1].SetWidth(60);
                                                table15.Cell(0, 0).SetContent("2000x");
                                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(0, 0).SetFont(Fontbold1);
                                                table15.Cell(1, 0).SetContent("500x");
                                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(1, 0).SetFont(Fontbold1);
                                                table15.Cell(2, 0).SetContent("100x");
                                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(2, 0).SetFont(Fontbold1);
                                                table15.Cell(3, 0).SetContent("50x");
                                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(3, 0).SetFont(Fontbold1);
                                                table15.Cell(4, 0).SetContent("20x");
                                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(4, 0).SetFont(Fontbold1);
                                                table15.Cell(5, 0).SetContent("10x");
                                                table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(5, 0).SetFont(Fontbold1);
                                                table15.Cell(6, 0).SetContent("5x");
                                                table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(6, 0).SetFont(Fontbold1);
                                                table15.Cell(7, 0).SetContent("Coinsx");
                                                table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(7, 0).SetFont(Fontbold1);
                                                table15.Cell(8, 0).SetContent("Total");
                                                table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {

                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                      new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }

                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                                myprov_pdfpage.Add(TC);
                                                myprov_pdfpage.Add(TC1);
                                                myprov_pdfpage.Add(TC2);
                                                //myprov_pdfpage.Add(TC4);
                                                myprov_pdfpage.Add(TC5);
                                                myprov_pdfpage.Add(TC6);
                                                myprov_pdfpage.Add(TC7);
                                                myprov_pdfpage.Add(TC8);
                                                myprov_pdfpage.Add(TC9);
                                                //myprov_pdfpage.Add(TC10);
                                                myprov_pdfpage.Add(TC11);
                                                myprov_pdfpage.Add(TC12);
                                                myprov_pdfpage.Add(TC13);
                                                myprov_pdfpage.Add(TC14);
                                                myprov_pdfpage.Add(TC15);
                                                myprov_pdfpage.Add(TC16);
                                                myprov_pdfpage.Add(TC17);
                                                myprov_pdfpage.Add(TC24);
                                                myprov_pdfpage.Add(TC25);
                                                myprov_pdfpage.Add(TC26);
                                                myprov_pdfpage.Add(TC27);
                                                myprov_pdfpage.Add(TC28);
                                                myprov_pdfpage.Add(TC29);
                                                myprov_pdfpage.Add(TC30);
                                                myprov_pdfpage.Add(TC31);
                                                myprov_pdfpage.Add(TC32);

                                                myprov_pdfpage.SaveToDocument();
                                                #endregion
                                            }
                                            //Bottom portion of the challan End
                                        }
                                    }
                                }
                                #endregion
                            }
                            else//last modified sudhagar 29.06.2017
                            {
                                #region For Overall
                                //string QHdrForGroup = "select HeaderId from chlpagesettings where College_Code=" + collegecode1 + " and DegreeCode='" + degreeCode + "'";
                                string QHdrForGroup = "	SELECT distinct ChlGroupHeader FROM FM_ChlBankPrintSettings WHERE DegreeCode = '" + degreeCode + "' AND SettingType = 1 and CollegeCode=" + collegecode1 + " ";

                                ds.Clear();
                                ds = d2.select_method_wo_parameter(QHdrForGroup, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            string bnkAcc = "";
                                            checkedHeaderOK = false;
                                            groupHdr = Convert.ToString(ds.Tables[0].Rows[i][0]);
                                            hdrInGrp0 = groupHdr.Split(',');
                                            hdrInGrp.Clear();
                                            foreach (string item in hdrInGrp0)
                                            {
                                                hdrInGrp.Add(item);
                                                checkedHeaderOK = true;
                                            }

                                            if (!checkedHeaderOK)
                                            {
                                                continue;
                                            }

                                            //Add new challan Page in this loop
                                            bool addpageOK = false;
                                            #region TOp portion

                                            int y = 0;

                                            Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                            PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN"); PdfTextArea ORGI;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                            }
                                            else
                                            {
                                                ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                            }
                                            PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                            //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                            //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                            PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                            PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                            PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea FC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {

                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {

                                                FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);

                                            }
                                            PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;


                                            myprov_pdfpage.Add(FC17);
                                            string text = "";

                                            //First Ends

                                            PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea UC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                            }
                                            else
                                            {
                                                UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                            }
                                            PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                            PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                            PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            //second End
                                            y = 0;


                                            PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                            PdfTextArea TC1;
                                            if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                            }
                                            else
                                            {
                                                TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                            }
                                            PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);

                                            PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                            PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                            PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                            PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                            PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                            //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                            PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                            PdfTextArea TC13;
                                            if (checkSchoolSetting(usercode) != 0)
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                            }
                                            else
                                            {
                                                TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                                      new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);
                                            }
                                            PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                            PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                            PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                            PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                            myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(TC10);
                                            y = 0;

                                            #endregion

                                            //End of  New CHallan Top Portion

                                            //Middle portion of the challan
                                            #region Middle Portion challan
                                            int chk = 0;
                                            for (int indx = 0; indx < hdrInGrp.Count; indx++)
                                            {
                                                string QhdrId = "SELECT HeaderFK,ChlGroupHeader  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + hdrInGrp[indx] + "') and stream='" + stream + "'";
                                                string HdrId = "";
                                                string dispHdr = "";

                                                DataSet ds1 = new DataSet();
                                                ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                                if (ds1.Tables.Count > 0)
                                                {
                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                    {
                                                        dispHdr = hdrInGrp[indx];
                                                        string hdrNme = dispHdr.Trim().ToUpper();
                                                        //bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                                        double totalAmt = 0;
                                                        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                        {
                                                            HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                            foreach (GridViewRow row in grid_Details.Rows)
                                                            {
                                                                TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                                                                //TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                                                                //TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                                                                Label lblfinYear = (Label)row.FindControl("lbl_Finyearfk");
                                                                string finYearfk = Convert.ToString(lblfinYear.Text.Trim());
                                                                string finInsert = string.Empty;
                                                                string finInsertValue = string.Empty;
                                                                string finUpdate = string.Empty;
                                                                if (!finYearfk.Contains("&"))
                                                                {
                                                                    finInsert = ",Chl_ActualFinyearFk";
                                                                    finInsertValue = ",'" + finYearfk + "'";
                                                                    finUpdate = " and finyearfk='" + finYearfk + "'";
                                                                }
                                                                TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                                                                Label lblFeeCategory = (Label)row.FindControl("lbl_textCode");
                                                                Label lblFeeCode = (Label)row.FindControl("lbl_feecode");
                                                                Label lblheaderid = (Label)row.FindControl("lbl_hdrid");
                                                                Label lblchltkn = (Label)row.FindControl("lbl_chltkn");
                                                                double remainAmt = 0;
                                                                remainAmt = Convert.ToDouble(txtTotalamt.Text) - Convert.ToDouble(lblchltkn.Text);
                                                                if (remainAmt > 0 || hdrNme == "TUITION FEE")
                                                                {
                                                                    if (lblheaderid.Text == HdrId)
                                                                    {
                                                                        double creditamt = 0;

                                                                        if (txtTobePaidamt.Text != "")
                                                                        {
                                                                            creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                                                                        }

                                                                        if (creditamt > 0 || hdrNme == "TUITION FEE")
                                                                        {
                                                                            if (creditamt <= remainAmt)
                                                                            {
                                                                                //new
                                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                                txt_rcptno.Text = recptNo;

                                                                                string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType" + finInsert + ") VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + HdrId + "," + txtTotalamt.Text + "," + creditamt + "," + lblFeeCategory.Text + "," + finYeaid + "," + bankPK + "," + lblFeeCode.Text + "," + challanType + "" + finInsertValue + ")";
                                                                                d2.select_method_wo_parameter(insertChlNo, "Text");

                                                                                string updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0) +" + creditamt + "  where FeeCategory ='" + lblFeeCategory.Text + "' and HeaderFK ='" + HdrId + "' and LedgerFK ='" + lblFeeCode.Text + "' and App_No='" + appnoNew + "' " + finUpdate + "";
                                                                                d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                                                                totalAmt += creditamt;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        grandtotal = grandtotal + totalAmt;

                                                        // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                        dispHdr += " (" + bnkAcc + ")";
                                                        if (grandtotal > 0 || hdrNme == "TUITION FEE")
                                                        {
                                                            addpageOK = true;
                                                            createPDFOK = true;
                                                            if (totalAmt > 0 || hdrNme == "TUITION FEE")
                                                            {
                                                                if (chk == 0)
                                                                {
                                                                    //chk++;
                                                                    #region Update Challan No

                                                                    //recptNo =generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                    //txt_rcptno.Text = recptNo;
                                                                    //lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
                                                                    //accidRecpt = Convert.ToString(Session["lastAccId"]);

                                                                    //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                    //{
                                                                    //    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                                    //    DataSet dsEachHdr = new DataSet();
                                                                    //    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                    //    if (dsEachHdr.Tables.Count > 0)
                                                                    //    {
                                                                    //        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                    //        {
                                                                    //            string selLedge = "	SELECT HeaderFK,LedgerFk,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')";
                                                                    //            DataSet dsLedge = new DataSet();
                                                                    //            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                    //            if (dsLedge.Tables.Count > 0)
                                                                    //            {
                                                                    //                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                    //                {
                                                                    //                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                    //                    {
                                                                    //                        string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + ")";
                                                                    //                        d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                    //                    }
                                                                    //                }
                                                                    //            }
                                                                    //        }
                                                                    //    }

                                                                    //}

                                                                    #endregion
                                                                }
                                                            }
                                                        }

                                                        PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(FC18);
                                                        myprov_pdfpage.Add(FC171);
                                                        myprov_pdfpage.Add(FC19);


                                                        PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(UC18);
                                                        myprov_pdfpage.Add(UC19);
                                                        myprov_pdfpage.Add(UC171);

                                                        PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                         new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(TC18);
                                                        myprov_pdfpage.Add(TC19);
                                                        myprov_pdfpage.Add(TC171);
                                                        y = y + 15;


                                                    }
                                                }


                                            }
                                            #endregion
                                            //Middle portion of challan End

                                            //Bottom portion of the challan
                                            if (addpageOK)
                                            {
                                                string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                                d2.update_method_wo_parameter(updateRecpt, "Text");
                                                PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                   new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                                PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                                myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(TC4);
                                                PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                          new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                   new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                                //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                                //myprov_pdfpage.Add(FC08, 250, 125);
                                                //myprov_pdfpage.Add(FC08, 550, 125);
                                                //myprov_pdfpage.Add(FC08, 900, 125);

                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                                PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                                PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                                PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                                myprov_pdfpage.Add(pr1);

                                                PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                                PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                                myprov_pdfpage.Add(pr2);

                                                PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                                PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                                myprov_pdfpage.Add(pr3);

                                                Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table.VisibleHeaders = false;
                                                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table.Columns[0].SetWidth(100);
                                                table.Columns[1].SetWidth(60);
                                                table.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table.Cell(0, 0).SetContent("DD No");
                                                }
                                                table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 0).SetFont(Fontbold1);
                                                table.Cell(0, 1).SetContent("Date");
                                                table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 1).SetFont(Fontbold1);
                                                table.Cell(0, 2).SetContent("Amount");
                                                table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(0, 2).SetFont(Fontbold1);
                                                table.Cell(1, 0).SetContent("\n");
                                                table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 0).SetFont(Fontbold1);
                                                table.Cell(1, 1).SetContent("\n");
                                                table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 1).SetFont(Fontbold1);
                                                table.Cell(1, 2).SetContent("\n");
                                                table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable);

                                                Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table1.VisibleHeaders = false;
                                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table1.Columns[0].SetWidth(100);
                                                table1.Columns[1].SetWidth(60);
                                                table1.Cell(0, 0).SetContent("2000x");
                                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(0, 0).SetFont(Fontbold1);
                                                table1.Cell(1, 0).SetContent("500x");
                                                table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(1, 0).SetFont(Fontbold1);
                                                table1.Cell(2, 0).SetContent("100x");
                                                table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(2, 0).SetFont(Fontbold1);
                                                table1.Cell(3, 0).SetContent("50x");
                                                table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(3, 0).SetFont(Fontbold1);
                                                table1.Cell(4, 0).SetContent("20x");
                                                table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(4, 0).SetFont(Fontbold1);
                                                table1.Cell(5, 0).SetContent("10x");
                                                table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(5, 0).SetFont(Fontbold1);
                                                table1.Cell(6, 0).SetContent("5x");
                                                table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(6, 0).SetFont(Fontbold1);
                                                table1.Cell(7, 0).SetContent("Coinsx");
                                                table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(7, 0).SetFont(Fontbold1);
                                                table1.Cell(8, 0).SetContent("Total");
                                                table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table1.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }


                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable1);

                                                myprov_pdfpage.Add(FC);
                                                myprov_pdfpage.Add(ORGI);
                                                myprov_pdfpage.Add(IOB);
                                                //myprov_pdfpage.Add(FC4);
                                                myprov_pdfpage.Add(FC5);
                                                myprov_pdfpage.Add(FC6);
                                                myprov_pdfpage.Add(FC7);
                                                myprov_pdfpage.Add(FC8);
                                                myprov_pdfpage.Add(FC9);
                                                //myprov_pdfpage.Add(FC10);
                                                myprov_pdfpage.Add(FC11);
                                                myprov_pdfpage.Add(FC12);
                                                myprov_pdfpage.Add(FC13);
                                                myprov_pdfpage.Add(FC14);
                                                myprov_pdfpage.Add(FC15);
                                                myprov_pdfpage.Add(FC16);

                                                myprov_pdfpage.Add(FC24);
                                                myprov_pdfpage.Add(FC25);
                                                myprov_pdfpage.Add(FC26);
                                                myprov_pdfpage.Add(FC27);
                                                myprov_pdfpage.Add(FC28);
                                                myprov_pdfpage.Add(FC29);
                                                myprov_pdfpage.Add(FC30);
                                                myprov_pdfpage.Add(FC31);

                                                myprov_pdfpage.Add(FC32);
                                                //myprov_pdfpage.Add(FC33);

                                                //First End
                                                myprov_pdfpage.Add(UC17);

                                                PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table3.VisibleHeaders = false;
                                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table3.Columns[0].SetWidth(100);
                                                table3.Columns[1].SetWidth(60);
                                                table3.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table3.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table3.Cell(0, 0).SetContent("DD No");
                                                }
                                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 0).SetFont(Fontbold1);
                                                table3.Cell(0, 1).SetContent("Date");
                                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 1).SetFont(Fontbold1);
                                                table3.Cell(0, 2).SetContent("Amount");
                                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(0, 2).SetFont(Fontbold1);
                                                table3.Cell(1, 0).SetContent("\n");
                                                table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 0).SetFont(Fontbold1);
                                                table3.Cell(1, 1).SetContent("\n");
                                                table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 1).SetFont(Fontbold1);
                                                table3.Cell(1, 2).SetContent("\n");
                                                table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table3.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable3);

                                                Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table14.VisibleHeaders = false;
                                                table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table14.Columns[0].SetWidth(100);
                                                table14.Columns[1].SetWidth(60);
                                                table14.Cell(0, 0).SetContent("2000x");
                                                table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(0, 0).SetFont(Fontbold1);
                                                table14.Cell(1, 0).SetContent("500x");
                                                table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(1, 0).SetFont(Fontbold1);
                                                table14.Cell(2, 0).SetContent("100x");
                                                table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(2, 0).SetFont(Fontbold1);
                                                table14.Cell(3, 0).SetContent("50x");
                                                table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(3, 0).SetFont(Fontbold1);
                                                table14.Cell(4, 0).SetContent("20x");
                                                table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(4, 0).SetFont(Fontbold1);
                                                table14.Cell(5, 0).SetContent("10x");
                                                table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(5, 0).SetFont(Fontbold1);
                                                table14.Cell(6, 0).SetContent("5x");
                                                table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(6, 0).SetFont(Fontbold1);
                                                table14.Cell(7, 0).SetContent("Coinsx");
                                                table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(7, 0).SetFont(Fontbold1);
                                                table14.Cell(8, 0).SetContent("Total");
                                                table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table14.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {
                                                    //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                    //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                    //myprov_pdfpage.Add(mobile);aaaa
                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                     new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable4);

                                                myprov_pdfpage.Add(UC);
                                                myprov_pdfpage.Add(UC1);
                                                myprov_pdfpage.Add(UC2);
                                                //myprov_pdfpage.Add(UC4);
                                                myprov_pdfpage.Add(UC5);
                                                myprov_pdfpage.Add(UC6);
                                                myprov_pdfpage.Add(UC7);
                                                myprov_pdfpage.Add(UC8);
                                                myprov_pdfpage.Add(UC9);
                                                //myprov_pdfpage.Add(UC10);
                                                myprov_pdfpage.Add(UC11);
                                                myprov_pdfpage.Add(UC12);
                                                myprov_pdfpage.Add(UC13);
                                                myprov_pdfpage.Add(UC14);
                                                myprov_pdfpage.Add(UC15);
                                                myprov_pdfpage.Add(UC16);


                                                myprov_pdfpage.Add(UC24);
                                                myprov_pdfpage.Add(UC25);
                                                myprov_pdfpage.Add(UC26);
                                                myprov_pdfpage.Add(UC27);
                                                myprov_pdfpage.Add(UC28);
                                                myprov_pdfpage.Add(UC29);
                                                myprov_pdfpage.Add(UC30);
                                                myprov_pdfpage.Add(UC31);
                                                myprov_pdfpage.Add(UC32);
                                                //second End


                                                myprov_pdfpage.Add(TC17);

                                                PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                                PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                                PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                                PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                                PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                                PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                                PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                                PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                                Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                                table5.VisibleHeaders = false;
                                                table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table5.Columns[0].SetWidth(100);
                                                table5.Columns[1].SetWidth(60);
                                                table5.Columns[2].SetWidth(60);

                                                if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                                {
                                                    table5.Cell(0, 0).SetContent("Cheque/DD No");
                                                }
                                                else
                                                {
                                                    table5.Cell(0, 0).SetContent("DD No");
                                                }
                                                table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 0).SetFont(Fontbold1);
                                                table5.Cell(0, 1).SetContent("Date");
                                                table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 1).SetFont(Fontbold1);
                                                table5.Cell(0, 2).SetContent("Amount");
                                                table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(0, 2).SetFont(Fontbold1);
                                                table5.Cell(1, 0).SetContent("\n");
                                                table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 0).SetFont(Fontbold1);
                                                table5.Cell(1, 1).SetContent("\n");
                                                table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 1).SetFont(Fontbold1);
                                                table5.Cell(1, 2).SetContent("\n");
                                                table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                table5.Cell(1, 2).SetFont(Fontbold1);
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                                myprov_pdfpage.Add(myprov_pdfpagetable31);

                                                Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                table15.VisibleHeaders = false;
                                                table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                                table15.Columns[0].SetWidth(100);
                                                table15.Columns[1].SetWidth(60);
                                                table15.Cell(0, 0).SetContent("2000x");
                                                table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(0, 0).SetFont(Fontbold1);
                                                table15.Cell(1, 0).SetContent("500x");
                                                table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(1, 0).SetFont(Fontbold1);
                                                table15.Cell(2, 0).SetContent("100x");
                                                table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(2, 0).SetFont(Fontbold1);
                                                table15.Cell(3, 0).SetContent("50x");
                                                table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(3, 0).SetFont(Fontbold1);
                                                table15.Cell(4, 0).SetContent("20x");
                                                table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(4, 0).SetFont(Fontbold1);
                                                table15.Cell(5, 0).SetContent("10x");
                                                table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(5, 0).SetFont(Fontbold1);
                                                table15.Cell(6, 0).SetContent("5x");
                                                table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(6, 0).SetFont(Fontbold1);
                                                table15.Cell(7, 0).SetContent("Coinsx");
                                                table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(7, 0).SetFont(Fontbold1);
                                                table15.Cell(8, 0).SetContent("Total");
                                                table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                                table15.Cell(8, 0).SetFont(Fontbold1);
                                                if (checkSchoolSetting(usercode) == 0)//added by abarna
                                                {

                                                    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                      new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                    myprov_pdfpage.Add(mblnoPdfTextArea);

                                                }
                                                Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                                myprov_pdfpage.Add(myprov_pdfpagetable5);

                                                myprov_pdfpage.Add(TC);
                                                myprov_pdfpage.Add(TC1);
                                                myprov_pdfpage.Add(TC2);
                                                //myprov_pdfpage.Add(TC4);
                                                myprov_pdfpage.Add(TC5);
                                                myprov_pdfpage.Add(TC6);
                                                myprov_pdfpage.Add(TC7);
                                                myprov_pdfpage.Add(TC8);
                                                myprov_pdfpage.Add(TC9);
                                                //myprov_pdfpage.Add(TC10);
                                                myprov_pdfpage.Add(TC11);
                                                myprov_pdfpage.Add(TC12);
                                                myprov_pdfpage.Add(TC13);
                                                myprov_pdfpage.Add(TC14);
                                                myprov_pdfpage.Add(TC15);
                                                myprov_pdfpage.Add(TC16);
                                                myprov_pdfpage.Add(TC17);
                                                myprov_pdfpage.Add(TC24);
                                                myprov_pdfpage.Add(TC25);
                                                myprov_pdfpage.Add(TC26);
                                                myprov_pdfpage.Add(TC27);
                                                myprov_pdfpage.Add(TC28);
                                                myprov_pdfpage.Add(TC29);
                                                myprov_pdfpage.Add(TC30);
                                                myprov_pdfpage.Add(TC31);
                                                myprov_pdfpage.Add(TC32);

                                                myprov_pdfpage.SaveToDocument();
                                                #endregion
                                            }
                                            //Bottom portion of the challan End
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            bool checkedHeaderOK = false;
                            string groupHdr;
                            string[] hdrInGrp0;
                            List<string> hdrInGrp = new List<string>();

                            if (Txt_amt.Text == "0" || Txt_amt.Text == "0.00" || Txt_amt.Text == "")
                            {
                                #region For Selected


                                //End of  New CHallan Top Portion


                                //Middle portion of the challan
                                if (rbl_headerselect.SelectedIndex == 0)
                                {
                                    //Group Header
                                    #region Middle Portion challan
                                    int chk = 0;
                                    for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                    {
                                        if (!cbl_grpheader.Items[indx].Selected)
                                        {
                                            continue;
                                        }

                                        checkedHeaderOK = false;
                                        string bnkAcc = "";
                                        //Add new challan Page in this loop
                                        bool addpageOK = false;
                                        double ovrallcredit = 0;
                                        double grandtotal = 0.00;
                                        string text = "";

                                        #region TOp portion

                                        int y = 0;

                                        Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                        PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea ORGI;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                        }
                                        else
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                        }
                                        PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                        //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                        PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea FC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                            FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);
                                        }
                                        PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");




                                        myprov_pdfpage.Add(FC17);


                                        //First Ends

                                        PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea UC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                        }
                                        else
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                        }
                                        PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                        PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea UC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                            UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + sem);
                                        }
                                        PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        //second End
                                        y = 0;


                                        PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea TC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                        }
                                        else
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                        }
                                        PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        myprov_pdfpage.Add(FC10);
                                        myprov_pdfpage.Add(UC10);
                                        myprov_pdfpage.Add(TC10);
                                        y = 0;

                                        #endregion

                                        string QhdrId = "SELECT HeaderFK,ChlGroupHeader  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + cbl_grpheader.Items[indx].Text + "') and stream='" + stream + "'";
                                        string HdrId = "";
                                        string dispHdr = "";

                                        DataSet ds1 = new DataSet();
                                        ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                        if (ds1.Tables.Count > 0)
                                        {
                                            if (ds1.Tables[0].Rows.Count > 0)
                                            {
                                                dispHdr = Convert.ToString(ds1.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                // bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                {
                                                    if (HdrId == "")
                                                    {
                                                        HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);

                                                    }
                                                    else
                                                    {
                                                        HdrId += "," + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                    }
                                                }

                                                string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                                DataSet ds2 = new DataSet();
                                                ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                                if (ds2.Tables.Count > 0)
                                                {
                                                    if (ds2.Tables[0].Rows.Count > 0)
                                                    {
                                                        // dispHdr = Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                        double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                        if (ds2.Tables[1].Rows.Count > 0)
                                                        {
                                                            totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                        }
                                                        // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                        dispHdr += " (" + bnkAcc + ")";
                                                        grandtotal = grandtotal + totalAmt;

                                                        if (grandtotal > 0)
                                                        {

                                                            addpageOK = true;
                                                            createPDFOK = true;
                                                            if (chk == 0)
                                                            {
                                                                //chk++;
                                                                #region Update Challan No
                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                txt_rcptno.Text = recptNo;

                                                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                {
                                                                    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                                    DataSet dsEachHdr = new DataSet();
                                                                    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                    if (dsEachHdr.Tables.Count > 0)
                                                                    {
                                                                        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority ,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE   l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
                                                                            DataSet dsLedge = new DataSet();
                                                                            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                            if (dsLedge.Tables.Count > 0)
                                                                            {
                                                                                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                                    {
                                                                                        double remainAmt = 0;
                                                                                        remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                        if (remainAmt > 0)
                                                                                        {
                                                                                            string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                            d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                            string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                            d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                }

                                                                #endregion
                                                            }

                                                        }

                                                        PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(FC18);
                                                        myprov_pdfpage.Add(FC171);
                                                        myprov_pdfpage.Add(FC19);


                                                        PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(UC18);
                                                        myprov_pdfpage.Add(UC19);
                                                        myprov_pdfpage.Add(UC171);

                                                        PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(TC18);
                                                        myprov_pdfpage.Add(TC19);
                                                        myprov_pdfpage.Add(TC171);
                                                        y = y + 15;

                                                    }
                                                }

                                            }
                                        }

                                        if (addpageOK)
                                        {
                                            string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                            d2.update_method_wo_parameter(updateRecpt, "Text");
                                            PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                         new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                            myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(TC4);
                                            PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);

                                            //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                            //myprov_pdfpage.Add(FC08, 250, 125);
                                            //myprov_pdfpage.Add(FC08, 550, 125);
                                            //myprov_pdfpage.Add(FC08, 900, 125);
                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                            PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                            PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                            PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                            myprov_pdfpage.Add(pr1);

                                            PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                            PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                            myprov_pdfpage.Add(pr2);

                                            PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                            PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                            myprov_pdfpage.Add(pr3);

                                            Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table.VisibleHeaders = false;
                                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table.Columns[0].SetWidth(100);
                                            table.Columns[1].SetWidth(60);
                                            table.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table.Cell(0, 0).SetContent("DD No");
                                            }
                                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 0).SetFont(Fontbold1);
                                            table.Cell(0, 1).SetContent("Date");
                                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 1).SetFont(Fontbold1);
                                            table.Cell(0, 2).SetContent("Amount");
                                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 2).SetFont(Fontbold1);
                                            table.Cell(1, 0).SetContent("\n");
                                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 0).SetFont(Fontbold1);
                                            table.Cell(1, 1).SetContent("\n");
                                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 1).SetFont(Fontbold1);
                                            table.Cell(1, 2).SetContent("\n");
                                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable);

                                            Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table1.VisibleHeaders = false;
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table1.Columns[0].SetWidth(100);
                                            table1.Columns[1].SetWidth(60);
                                            table1.Cell(0, 0).SetContent("2000x");
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(0, 0).SetFont(Fontbold1);
                                            table1.Cell(1, 0).SetContent("500x");
                                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(1, 0).SetFont(Fontbold1);
                                            table1.Cell(2, 0).SetContent("100x");
                                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(2, 0).SetFont(Fontbold1);
                                            table1.Cell(3, 0).SetContent("50x");
                                            table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(3, 0).SetFont(Fontbold1);
                                            table1.Cell(4, 0).SetContent("20x");
                                            table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(4, 0).SetFont(Fontbold1);
                                            table1.Cell(5, 0).SetContent("10x");
                                            table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(5, 0).SetFont(Fontbold1);
                                            table1.Cell(6, 0).SetContent("5x");
                                            table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(6, 0).SetFont(Fontbold1);
                                            table1.Cell(7, 0).SetContent("Coinsx");
                                            table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(7, 0).SetFont(Fontbold1);
                                            table1.Cell(8, 0).SetContent("Total");
                                            table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }


                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable1);

                                            myprov_pdfpage.Add(FC);
                                            myprov_pdfpage.Add(ORGI);
                                            myprov_pdfpage.Add(IOB);
                                            //myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(FC5);
                                            myprov_pdfpage.Add(FC6);
                                            myprov_pdfpage.Add(FC7);
                                            myprov_pdfpage.Add(FC8);
                                            myprov_pdfpage.Add(FC9);
                                            //myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(FC11);
                                            myprov_pdfpage.Add(FC12);
                                            myprov_pdfpage.Add(FC13);
                                            myprov_pdfpage.Add(FC14);
                                            myprov_pdfpage.Add(FC15);
                                            myprov_pdfpage.Add(FC16);

                                            myprov_pdfpage.Add(FC24);
                                            myprov_pdfpage.Add(FC25);
                                            myprov_pdfpage.Add(FC26);
                                            myprov_pdfpage.Add(FC27);
                                            myprov_pdfpage.Add(FC28);
                                            myprov_pdfpage.Add(FC29);
                                            myprov_pdfpage.Add(FC30);
                                            myprov_pdfpage.Add(FC31);

                                            myprov_pdfpage.Add(FC32);
                                            //myprov_pdfpage.Add(FC33);

                                            //First End
                                            myprov_pdfpage.Add(UC17);

                                            PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table3.VisibleHeaders = false;
                                            table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table3.Columns[0].SetWidth(100);
                                            table3.Columns[1].SetWidth(60);
                                            table3.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table3.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table3.Cell(0, 0).SetContent("DD No");
                                            }
                                            table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 0).SetFont(Fontbold1);
                                            table3.Cell(0, 1).SetContent("Date");
                                            table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 1).SetFont(Fontbold1);
                                            table3.Cell(0, 2).SetContent("Amount");
                                            table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 2).SetFont(Fontbold1);
                                            table3.Cell(1, 0).SetContent("\n");
                                            table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 0).SetFont(Fontbold1);
                                            table3.Cell(1, 1).SetContent("\n");
                                            table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 1).SetFont(Fontbold1);
                                            table3.Cell(1, 2).SetContent("\n");
                                            table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable3);

                                            Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table14.VisibleHeaders = false;
                                            table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table14.Columns[0].SetWidth(100);
                                            table14.Columns[1].SetWidth(60);
                                            table14.Cell(0, 0).SetContent("2000x");
                                            table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(0, 0).SetFont(Fontbold1);
                                            table14.Cell(1, 0).SetContent("500x");
                                            table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(1, 0).SetFont(Fontbold1);
                                            table14.Cell(2, 0).SetContent("100x");
                                            table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(2, 0).SetFont(Fontbold1);
                                            table14.Cell(3, 0).SetContent("50x");
                                            table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(3, 0).SetFont(Fontbold1);
                                            table14.Cell(4, 0).SetContent("20x");
                                            table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(4, 0).SetFont(Fontbold1);
                                            table14.Cell(5, 0).SetContent("10x");
                                            table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(5, 0).SetFont(Fontbold1);
                                            table14.Cell(6, 0).SetContent("5x");
                                            table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(6, 0).SetFont(Fontbold1);
                                            table14.Cell(7, 0).SetContent("Coinsx");
                                            table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(7, 0).SetFont(Fontbold1);
                                            table14.Cell(8, 0).SetContent("Total");
                                            table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(8, 0).SetFont(Fontbold1);
                                            //
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }

                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable4);

                                            myprov_pdfpage.Add(UC);
                                            myprov_pdfpage.Add(UC1);
                                            myprov_pdfpage.Add(UC2);
                                            //myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(UC5);
                                            myprov_pdfpage.Add(UC6);
                                            myprov_pdfpage.Add(UC7);
                                            myprov_pdfpage.Add(UC8);
                                            myprov_pdfpage.Add(UC9);
                                            //myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(UC11);
                                            myprov_pdfpage.Add(UC12);
                                            myprov_pdfpage.Add(UC13);
                                            myprov_pdfpage.Add(UC14);
                                            myprov_pdfpage.Add(UC15);
                                            myprov_pdfpage.Add(UC16);


                                            myprov_pdfpage.Add(UC24);
                                            myprov_pdfpage.Add(UC25);
                                            myprov_pdfpage.Add(UC26);
                                            myprov_pdfpage.Add(UC27);
                                            myprov_pdfpage.Add(UC28);
                                            myprov_pdfpage.Add(UC29);
                                            myprov_pdfpage.Add(UC30);
                                            myprov_pdfpage.Add(UC31);
                                            myprov_pdfpage.Add(UC32);
                                            //second End


                                            myprov_pdfpage.Add(TC17);

                                            PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table5.VisibleHeaders = false;
                                            table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table5.Columns[0].SetWidth(100);
                                            table5.Columns[1].SetWidth(60);
                                            table5.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table5.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table5.Cell(0, 0).SetContent("DD No");
                                            }
                                            table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 0).SetFont(Fontbold1);
                                            table5.Cell(0, 1).SetContent("Date");
                                            table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 1).SetFont(Fontbold1);
                                            table5.Cell(0, 2).SetContent("Amount");
                                            table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 2).SetFont(Fontbold1);
                                            table5.Cell(1, 0).SetContent("\n");
                                            table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 0).SetFont(Fontbold1);
                                            table5.Cell(1, 1).SetContent("\n");
                                            table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 1).SetFont(Fontbold1);
                                            table5.Cell(1, 2).SetContent("\n");
                                            table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable31);

                                            Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table15.VisibleHeaders = false;
                                            table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table15.Columns[0].SetWidth(100);
                                            table15.Columns[1].SetWidth(60);
                                            table15.Cell(0, 0).SetContent("2000x");
                                            table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(0, 0).SetFont(Fontbold1);
                                            table15.Cell(1, 0).SetContent("500x");
                                            table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(1, 0).SetFont(Fontbold1);
                                            table15.Cell(2, 0).SetContent("100x");
                                            table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(2, 0).SetFont(Fontbold1);
                                            table15.Cell(3, 0).SetContent("50x");
                                            table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(3, 0).SetFont(Fontbold1);
                                            table15.Cell(4, 0).SetContent("20x");
                                            table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(4, 0).SetFont(Fontbold1);
                                            table15.Cell(5, 0).SetContent("10x");
                                            table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(5, 0).SetFont(Fontbold1);
                                            table15.Cell(6, 0).SetContent("5x");
                                            table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(6, 0).SetFont(Fontbold1);
                                            table15.Cell(7, 0).SetContent("Coinsx");
                                            table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(7, 0).SetFont(Fontbold1);
                                            table15.Cell(8, 0).SetContent("Total");
                                            table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(8, 0).SetFont(Fontbold1);
                                            //
                                            //if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            //{

                                            //    PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //      new PdfArea(mychallan, 855, y + 380, 160, 500), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                            //    myprov_pdfpage.Add(mblnoPdfTextArea);

                                            //}
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable5);

                                            myprov_pdfpage.Add(TC);
                                            myprov_pdfpage.Add(TC1);
                                            myprov_pdfpage.Add(TC2);
                                            //myprov_pdfpage.Add(TC4);
                                            myprov_pdfpage.Add(TC5);
                                            myprov_pdfpage.Add(TC6);
                                            myprov_pdfpage.Add(TC7);
                                            myprov_pdfpage.Add(TC8);
                                            myprov_pdfpage.Add(TC9);
                                            //myprov_pdfpage.Add(TC10);
                                            myprov_pdfpage.Add(TC11);
                                            myprov_pdfpage.Add(TC12);
                                            myprov_pdfpage.Add(TC13);
                                            myprov_pdfpage.Add(TC14);
                                            myprov_pdfpage.Add(TC15);
                                            myprov_pdfpage.Add(TC16);
                                            myprov_pdfpage.Add(TC17);
                                            myprov_pdfpage.Add(TC24);
                                            myprov_pdfpage.Add(TC25);
                                            myprov_pdfpage.Add(TC26);
                                            myprov_pdfpage.Add(TC27);
                                            myprov_pdfpage.Add(TC28);
                                            myprov_pdfpage.Add(TC29);
                                            myprov_pdfpage.Add(TC30);
                                            myprov_pdfpage.Add(TC31);
                                            myprov_pdfpage.Add(TC32);

                                            myprov_pdfpage.SaveToDocument();
                                            #endregion
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    checkedHeaderOK = false;
                                    string bnkAcc = "";
                                    //Add new challan Page in this loop
                                    bool addpageOK = false;
                                    double ovrallcredit = 0;
                                    double grandtotal = 0.00;
                                    string text = "";
                                    #region TOp portion

                                    int y = 0;

                                    Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                    PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea ORGI;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                    }
                                    else
                                    {
                                        ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                          new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                    }
                                    PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                    //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                    PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                    PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea FC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {
                                        FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                                  new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }

                                    PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");




                                    myprov_pdfpage.Add(FC17);


                                    //First Ends

                                    PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea UC1;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                    }
                                    else
                                    {
                                        UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                    }
                                    PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                    PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea UC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {
                                        UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                                   new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }

                                    PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                    //second End
                                    y = 0;


                                    PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea TC1;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                    }
                                    else
                                    {
                                        TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                    }
                                    PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                    PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea TC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {
                                        TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                          new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }
                                    PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                    PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    myprov_pdfpage.Add(FC10);
                                    myprov_pdfpage.Add(UC10);
                                    myprov_pdfpage.Add(TC10);
                                    y = 0;

                                    #endregion
                                    if (rbl_headerselect.SelectedIndex == 1)
                                    {
                                        //Header
                                        #region Middle Portion challan
                                        int chk = 0;
                                        for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                        {
                                            if (!cbl_grpheader.Items[indx].Selected)
                                            {
                                                continue;
                                            }

                                            string HdrId = "";
                                            string dispHdr = "";
                                            HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);
                                            dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);

                                            string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND A.HeaderFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.HeaderFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";

                                            DataSet ds2 = new DataSet();
                                            ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                            if (ds2.Tables.Count > 0)
                                            {
                                                if (ds2.Tables[0].Rows.Count > 0)
                                                {

                                                    double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                    {
                                                        totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                    }
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                    dispHdr += " (" + bnkAcc + ")";
                                                    grandtotal = grandtotal + totalAmt;

                                                    if (grandtotal > 0)
                                                    {

                                                        addpageOK = true;
                                                        createPDFOK = true;
                                                        if (chk == 0)
                                                        {
                                                            //chk++;
                                                            #region Update Challan No
                                                            recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                            txt_rcptno.Text = recptNo;

                                                            //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                            //{
                                                            string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                            DataSet dsEachHdr = new DataSet();
                                                            dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                            if (dsEachHdr.Tables.Count > 0)
                                                            {
                                                                if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority ,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')   order by case when priority is null then 1 else 0 end, priority ";
                                                                    DataSet dsLedge = new DataSet();
                                                                    dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                    if (dsLedge.Tables.Count > 0)
                                                                    {
                                                                        if (dsLedge.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                            {
                                                                                double remainAmt = 0;
                                                                                remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                if (remainAmt > 0)
                                                                                {
                                                                                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                    string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                    d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            //}

                                                            #endregion
                                                        }

                                                    }
                                                    if (totalAmt > 0)
                                                    {
                                                        PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(FC18);
                                                        myprov_pdfpage.Add(FC171);
                                                        myprov_pdfpage.Add(FC19);


                                                        PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(UC18);
                                                        myprov_pdfpage.Add(UC19);
                                                        myprov_pdfpage.Add(UC171);

                                                        PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(TC18);
                                                        myprov_pdfpage.Add(TC19);
                                                        myprov_pdfpage.Add(TC171);
                                                        y = y + 15;
                                                    }

                                                }
                                            }




                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //Ledger wise
                                        #region Middle Portion challan
                                        int chk = 0;
                                        for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                        {
                                            if (!cbl_grpheader.Items[indx].Selected)
                                            {
                                                continue;
                                            }

                                            //string QhdrId = "select header_id,ChlHeaderName,BankAccNo from ChlHeaderSettings where Stream = '" + stream + "' and header_id in ('" + cbl_grpheader.Items[indx].Value + "')";
                                            string HdrId = "";
                                            string dispHdr = "";

                                            //DataSet ds1 = new DataSet();
                                            //ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                            //if (ds1.Tables.Count > 0)
                                            //{
                                            //    if (ds1.Tables[0].Rows.Count > 0)
                                            //    {

                                            //        bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                            //    }
                                            //}

                                            HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);
                                            dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);



                                            string QFinTot = "	SELECT App_FormNo,Stud_Name,Course_Name+'-'+Dept_Name Degree,TextVal,DispStream,ChlGroupHeader,SUM(TotalAmount) as Totamount	FROM FT_FeeAllot A,applyn P,Degree G,Course U,Department D,FS_ChlGroupHeaderSettings S,TextValTable T	WHERE A.app_no = P.app_no AND P.degree_code = G.Degree_Code AND G.Course_Id = U.Course_Id AND G.college_code = U.college_code	AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code AND A.HeaderFK = S.HeaderFK AND A.FeeCategory = T.TextCode AND Stream = '" + stream + "' AND  A.LedgerFK IN (" + HdrId + ")  AND A.FeeCategory in(" + feeCategory + ") and P.app_no=" + appnoNew + "	GROUP BY App_FormNo,Stud_Name,Course_Name,Dept_Name,TextVal,DispStream,ChlGroupHeader           	    SELECT isnull(SUM(TakenAmt),0) as TakenAmt	FROM FT_ChallanDet C,FS_ChlGroupHeaderSettings S,TextValTable T 	WHERE C.HeaderFK = S.HeaderFK AND C.FeeCategory = T.TextCode	AND Stream = '" + stream + "' AND C.LedgerFK IN (" + HdrId + ") AND C.FeeCategory in (" + feeCategory + ") and C.app_no=" + appnoNew + "  GROUP BY ChlGroupHeader ";
                                            DataSet ds2 = new DataSet();
                                            ds2 = d2.select_method_wo_parameter(QFinTot, "Text");
                                            if (ds2.Tables.Count > 0)
                                            {
                                                if (ds2.Tables[0].Rows.Count > 0)
                                                {
                                                    double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                    if (ds2.Tables[1].Rows.Count > 0)
                                                    {
                                                        totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                    }
                                                    bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                    dispHdr += " (" + bnkAcc + ")";
                                                    grandtotal = grandtotal + totalAmt;

                                                    if (grandtotal > 0)
                                                    {

                                                        addpageOK = true;
                                                        createPDFOK = true;
                                                        if (chk == 0)
                                                        {
                                                            //chk++;
                                                            #region Update Challan No
                                                            recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                            txt_rcptno.Text = recptNo;

                                                            //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                            //{
                                                            //string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                            //DataSet dsEachHdr = new DataSet();
                                                            //dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                            //if (dsEachHdr.Tables.Count > 0)
                                                            //{
                                                            // if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                            //{
                                                            string hdrfk = d2.GetFunction("select HeaderFK  from FM_LedgerMaster where LedgerPK ='" + HdrId + "'");
                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority ,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot   f,FM_LedgerMaster l WHERE   l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + hdrfk + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') and LedgerFk='" + HdrId + "'   order by case when priority is null then 1 else 0 end, priority ";
                                                            DataSet dsLedge = new DataSet();
                                                            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                            if (dsLedge.Tables.Count > 0)
                                                            {
                                                                if (dsLedge.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                    {
                                                                        double remainAmt = 0;
                                                                        remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                        if (remainAmt > 0)
                                                                        {
                                                                            string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + hdrfk + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                            d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                            string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + hdrfk + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                            d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            //}
                                                            // }

                                                            //}

                                                            #endregion
                                                        }

                                                    }
                                                    if (totalAmt > 0)
                                                    {
                                                        PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(FC18);
                                                        myprov_pdfpage.Add(FC171);
                                                        myprov_pdfpage.Add(FC19);


                                                        PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(UC18);
                                                        myprov_pdfpage.Add(UC19);
                                                        myprov_pdfpage.Add(UC171);

                                                        PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                        PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                        new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                        PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                            new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                        myprov_pdfpage.Add(TC18);
                                                        myprov_pdfpage.Add(TC19);
                                                        myprov_pdfpage.Add(TC171);
                                                        y = y + 15;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    //Middle portion of challan End

                                    //Bottom portion of the challan
                                    if (addpageOK)
                                    {
                                        string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");
                                        PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                     new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                        PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                        PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                        myprov_pdfpage.Add(FC4);
                                        myprov_pdfpage.Add(UC4);
                                        myprov_pdfpage.Add(TC4);
                                        PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                        PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                        PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);

                                        //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                        //myprov_pdfpage.Add(FC08, 250, 125);
                                        //myprov_pdfpage.Add(FC08, 550, 125);
                                        //myprov_pdfpage.Add(FC08, 900, 125);
                                        #region Bottom Portion of Challan

                                        text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                        PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                        PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                        PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                        myprov_pdfpage.Add(pr1);

                                        PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                        PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                        myprov_pdfpage.Add(pr2);

                                        PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                        PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                        myprov_pdfpage.Add(pr3);

                                        Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table.VisibleHeaders = false;
                                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table.Columns[0].SetWidth(100);
                                        table.Columns[1].SetWidth(60);
                                        table.Columns[2].SetWidth(60);

                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table.Cell(0, 0).SetContent("DD No");
                                        }
                                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 0).SetFont(Fontbold1);
                                        table.Cell(0, 1).SetContent("Date");
                                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 1).SetFont(Fontbold1);
                                        table.Cell(0, 2).SetContent("Amount");
                                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 2).SetFont(Fontbold1);
                                        table.Cell(1, 0).SetContent("\n");
                                        table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 0).SetFont(Fontbold1);
                                        table.Cell(1, 1).SetContent("\n");
                                        table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 1).SetFont(Fontbold1);
                                        table.Cell(1, 2).SetContent("\n");
                                        table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable);

                                        Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table1.VisibleHeaders = false;
                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table1.Columns[0].SetWidth(100);
                                        table1.Columns[1].SetWidth(60);
                                        table1.Cell(0, 0).SetContent("2000x");
                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(0, 0).SetFont(Fontbold1);
                                        table1.Cell(1, 0).SetContent("500x");
                                        table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(1, 0).SetFont(Fontbold1);
                                        table1.Cell(2, 0).SetContent("100x");
                                        table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(2, 0).SetFont(Fontbold1);
                                        table1.Cell(3, 0).SetContent("50x");
                                        table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(3, 0).SetFont(Fontbold1);
                                        table1.Cell(4, 0).SetContent("20x");
                                        table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(4, 0).SetFont(Fontbold1);
                                        table1.Cell(5, 0).SetContent("10x");
                                        table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(5, 0).SetFont(Fontbold1);
                                        table1.Cell(6, 0).SetContent("5x");
                                        table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(6, 0).SetFont(Fontbold1);
                                        table1.Cell(7, 0).SetContent("Coinsx");
                                        table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(7, 0).SetFont(Fontbold1);
                                        table1.Cell(8, 0).SetContent("Total");
                                        table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(8, 0).SetFont(Fontbold1);
                                     
                                           
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }
                                            //myprov_pdfpage.Add(mblnoPdfTextArea);

                                        


                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable1);

                                        myprov_pdfpage.Add(FC);
                                        myprov_pdfpage.Add(ORGI);
                                        myprov_pdfpage.Add(IOB);
                                        //myprov_pdfpage.Add(FC4);
                                        myprov_pdfpage.Add(FC5);
                                        myprov_pdfpage.Add(FC6);
                                        myprov_pdfpage.Add(FC7);
                                        myprov_pdfpage.Add(FC8);
                                        myprov_pdfpage.Add(FC9);
                                        //myprov_pdfpage.Add(FC10);
                                        myprov_pdfpage.Add(FC11);
                                        myprov_pdfpage.Add(FC12);
                                        myprov_pdfpage.Add(FC13);
                                        myprov_pdfpage.Add(FC14);
                                        myprov_pdfpage.Add(FC15);
                                        myprov_pdfpage.Add(FC16);

                                        myprov_pdfpage.Add(FC24);
                                        myprov_pdfpage.Add(FC25);
                                        myprov_pdfpage.Add(FC26);
                                        myprov_pdfpage.Add(FC27);
                                        myprov_pdfpage.Add(FC28);
                                        myprov_pdfpage.Add(FC29);
                                        myprov_pdfpage.Add(FC30);
                                        myprov_pdfpage.Add(FC31);

                                        myprov_pdfpage.Add(FC32);
                                        //myprov_pdfpage.Add(FC33);

                                        //First End
                                        myprov_pdfpage.Add(UC17);

                                        PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                        Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table3.VisibleHeaders = false;
                                        table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table3.Columns[0].SetWidth(100);
                                        table3.Columns[1].SetWidth(60);
                                        table3.Columns[2].SetWidth(60);

                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table3.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table3.Cell(0, 0).SetContent("DD No");
                                        }
                                        table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 0).SetFont(Fontbold1);
                                        table3.Cell(0, 1).SetContent("Date");
                                        table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 1).SetFont(Fontbold1);
                                        table3.Cell(0, 2).SetContent("Amount");
                                        table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 2).SetFont(Fontbold1);
                                        table3.Cell(1, 0).SetContent("\n");
                                        table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 0).SetFont(Fontbold1);
                                        table3.Cell(1, 1).SetContent("\n");
                                        table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 1).SetFont(Fontbold1);
                                        table3.Cell(1, 2).SetContent("\n");
                                        table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable3);

                                        Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table14.VisibleHeaders = false;
                                        table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table14.Columns[0].SetWidth(100);
                                        table14.Columns[1].SetWidth(60);
                                        table14.Cell(0, 0).SetContent("2000x");
                                        table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(0, 0).SetFont(Fontbold1);
                                        table14.Cell(1, 0).SetContent("500x");
                                        table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(1, 0).SetFont(Fontbold1);
                                        table14.Cell(2, 0).SetContent("100x");
                                        table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(2, 0).SetFont(Fontbold1);
                                        table14.Cell(3, 0).SetContent("50x");
                                        table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(3, 0).SetFont(Fontbold1);
                                        table14.Cell(4, 0).SetContent("20x");
                                        table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(4, 0).SetFont(Fontbold1);
                                        table14.Cell(5, 0).SetContent("10x");
                                        table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(5, 0).SetFont(Fontbold1);
                                        table14.Cell(6, 0).SetContent("5x");
                                        table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(6, 0).SetFont(Fontbold1);
                                        table14.Cell(7, 0).SetContent("Coinsx");
                                        table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(7, 0).SetFont(Fontbold1);
                                        table14.Cell(8, 0).SetContent("Total");
                                        table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(8, 0).SetFont(Fontbold1);
                                        if (checkSchoolSetting(usercode) == 0)//added by abarna
                                        {
                                            //Gios.Pdf.PdfTable mobile3 = mychallan.NewTable(Fontsmall, 1, 1, 1);
                                            //mobile3.Columns[0].SetWidth(60);
                                            //mobile3.VisibleHeaders = false;
                                            //mobile3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            //mobile3.Cell(0, 0).SetContent("Mobile Number:");
                                            //mobile3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //mobile3.Cell(0, 0).SetFont(Fontbold1);
                                            //mobile3.Cell(0, 0).SetContent("\n");
                                            //mobile3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //mobile3.Cell(0, 0).SetFont(Fontbold1);
                                            //Gios.Pdf.PdfTablePage myprov_pdfpagemobile3 = mobile3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 180, 750));
                                            //myprov_pdfpage.Add(myprov_pdfpagemobile3);
                                            //PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 686, y + 530, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No.");
                                            //myprov_pdfpage.Add(mblnoPdfTextArea);
                                            PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                            myprov_pdfpage.Add(mblnoPdfTextArea);


                                        }

                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable4);

                                        myprov_pdfpage.Add(UC);
                                        myprov_pdfpage.Add(UC1);
                                        myprov_pdfpage.Add(UC2);
                                        //myprov_pdfpage.Add(UC4);
                                        myprov_pdfpage.Add(UC5);
                                        myprov_pdfpage.Add(UC6);
                                        myprov_pdfpage.Add(UC7);
                                        myprov_pdfpage.Add(UC8);
                                        myprov_pdfpage.Add(UC9);
                                        //myprov_pdfpage.Add(UC10);
                                        myprov_pdfpage.Add(UC11);
                                        myprov_pdfpage.Add(UC12);
                                        myprov_pdfpage.Add(UC13);
                                        myprov_pdfpage.Add(UC14);
                                        myprov_pdfpage.Add(UC15);
                                        myprov_pdfpage.Add(UC16);


                                        myprov_pdfpage.Add(UC24);
                                        myprov_pdfpage.Add(UC25);
                                        myprov_pdfpage.Add(UC26);
                                        myprov_pdfpage.Add(UC27);
                                        myprov_pdfpage.Add(UC28);
                                        myprov_pdfpage.Add(UC29);
                                        myprov_pdfpage.Add(UC30);
                                        myprov_pdfpage.Add(UC31);
                                        myprov_pdfpage.Add(UC32);
                                        //second End


                                        myprov_pdfpage.Add(TC17);

                                        PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                        Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table5.VisibleHeaders = false;
                                        table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table5.Columns[0].SetWidth(100);
                                        table5.Columns[1].SetWidth(60);
                                        table5.Columns[2].SetWidth(60);

                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table5.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table5.Cell(0, 0).SetContent("DD No");
                                        }
                                        table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 0).SetFont(Fontbold1);
                                        table5.Cell(0, 1).SetContent("Date");
                                        table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 1).SetFont(Fontbold1);
                                        table5.Cell(0, 2).SetContent("Amount");
                                        table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 2).SetFont(Fontbold1);
                                        table5.Cell(1, 0).SetContent("\n");
                                        table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 0).SetFont(Fontbold1);
                                        table5.Cell(1, 1).SetContent("\n");
                                        table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 1).SetFont(Fontbold1);
                                        table5.Cell(1, 2).SetContent("\n");
                                        table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable31);

                                        Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table15.VisibleHeaders = false;
                                        table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table15.Columns[0].SetWidth(100);
                                        table15.Columns[1].SetWidth(60);
                                        table15.Cell(0, 0).SetContent("2000x");
                                        table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(0, 0).SetFont(Fontbold1);
                                        table15.Cell(1, 0).SetContent("500x");
                                        table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(1, 0).SetFont(Fontbold1);
                                        table15.Cell(2, 0).SetContent("100x");
                                        table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(2, 0).SetFont(Fontbold1);
                                        table15.Cell(3, 0).SetContent("50x");
                                        table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(3, 0).SetFont(Fontbold1);
                                        table15.Cell(4, 0).SetContent("20x");
                                        table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(4, 0).SetFont(Fontbold1);
                                        table15.Cell(5, 0).SetContent("10x");
                                        table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(5, 0).SetFont(Fontbold1);
                                        table15.Cell(6, 0).SetContent("5x");
                                        table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(6, 0).SetFont(Fontbold1);
                                        table15.Cell(7, 0).SetContent("Coinsx");
                                        table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(7, 0).SetFont(Fontbold1);
                                        table15.Cell(8, 0).SetContent("Total");
                                        table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(8, 0).SetFont(Fontbold1);
                                        if (checkSchoolSetting(usercode) == 0)//added by abarna
                                        {
                                            //Gios.Pdf.PdfTable mobile = mychallan.NewTable(Fontsmall, 1, 1, 1);
                                            //mobile.VisibleHeaders = false;
                                            //mobile.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            //mobile.Cell(0, 0).SetContent("Mobile Number:");
                                            //mobile.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //mobile.Cell(0, 0).SetFont(Fontbold1);
                                            //mobile.Cell(0, 0).SetContent("\n");
                                            //mobile.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            //mobile.Cell(0, 0).SetFont(Fontbold1);
                                            //PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                 new PdfArea(mychallan, 231, y + 424, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No.");
                                            //myprov_pdfpage.Add(mblnoPdfTextArea);
                                            PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                              new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                            myprov_pdfpage.Add(mblnoPdfTextArea);

                                        }

                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable5);

                                        myprov_pdfpage.Add(TC);
                                        myprov_pdfpage.Add(TC1);
                                        myprov_pdfpage.Add(TC2);
                                        //myprov_pdfpage.Add(TC4);
                                        myprov_pdfpage.Add(TC5);
                                        myprov_pdfpage.Add(TC6);
                                        myprov_pdfpage.Add(TC7);
                                        myprov_pdfpage.Add(TC8);
                                        myprov_pdfpage.Add(TC9);
                                        //myprov_pdfpage.Add(TC10);
                                        myprov_pdfpage.Add(TC11);
                                        myprov_pdfpage.Add(TC12);
                                        myprov_pdfpage.Add(TC13);
                                        myprov_pdfpage.Add(TC14);
                                        myprov_pdfpage.Add(TC15);
                                        myprov_pdfpage.Add(TC16);
                                        myprov_pdfpage.Add(TC17);
                                        myprov_pdfpage.Add(TC24);
                                        myprov_pdfpage.Add(TC25);
                                        myprov_pdfpage.Add(TC26);
                                        myprov_pdfpage.Add(TC27);
                                        myprov_pdfpage.Add(TC28);
                                        myprov_pdfpage.Add(TC29);
                                        myprov_pdfpage.Add(TC30);
                                        myprov_pdfpage.Add(TC31);
                                        myprov_pdfpage.Add(TC32);

                                        myprov_pdfpage.SaveToDocument();
                                        #endregion
                                    }
                                    //Bottom portion of the challan End
                                }


                                #endregion
                            }
                            else
                            {
                                #region For Selected


                                //End of  New CHallan Top Portion

                                //Middle portion of the challan
                                if (rbl_headerselect.SelectedIndex == 0)
                                {

                                    //Group Header
                                    #region Middle Portion challan
                                    int chk = 0;
                                    for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                    {
                                        if (!cbl_grpheader.Items[indx].Selected)
                                        {
                                            continue;
                                        }
                                        double ovrallcredit = 0;
                                        double grandtotal = 0.00;

                                        checkedHeaderOK = false;
                                        string bnkAcc = "";
                                        //Add new challan Page in this loop
                                        bool addpageOK = false;
                                        #region TOp portion

                                        int y = 0;

                                        Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                        PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea ORGI;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                        }
                                        else
                                        {
                                            ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                        }
                                        PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                        //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                        PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea FC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                            FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }

                                        PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");



                                        myprov_pdfpage.Add(FC17);
                                        string text = "";

                                        //First Ends

                                        PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea UC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                        }
                                        else
                                        {
                                            UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                        }
                                        PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                        PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea UC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                            UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                               new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }

                                        PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        //second End
                                        y = 0;


                                        PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                        PdfTextArea TC1;
                                        if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                        }
                                        else
                                        {
                                            TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                        }
                                        PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                        //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                        //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                        PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                        PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                        PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                        PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                        PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                        //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                        //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                        PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                        PdfTextArea TC13;
                                        if (checkSchoolSetting(usercode) != 0)
                                        {
                                            TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                        }
                                        else
                                        {
                                            TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                        }
                                        PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                        PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                        PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                        PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                        myprov_pdfpage.Add(FC10);
                                        myprov_pdfpage.Add(UC10);
                                        myprov_pdfpage.Add(TC10);
                                        y = 0;

                                        #endregion

                                        string QhdrId = "SELECT HeaderFK,ChlGroupHeader  FROM FS_ChlGroupHeaderSettings where ChlGroupHeader in ('" + cbl_grpheader.Items[indx].Text + "') and stream='" + stream + "'";
                                        string HdrId = "";
                                        string dispHdr = "";

                                        DataSet ds1 = new DataSet();
                                        ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                        if (ds1.Tables.Count > 0)
                                        {
                                            if (ds1.Tables[0].Rows.Count > 0)
                                            {
                                                dispHdr = Convert.ToString(ds1.Tables[0].Rows[0]["ChlGroupHeader"]);
                                                //bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                                double totalAmt = 0;
                                                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                {
                                                    HdrId = Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]);
                                                    foreach (GridViewRow row in grid_Details.Rows)
                                                    {
                                                        //TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                                                        //Label lblheaderid = (Label)row.FindControl("lbl_hdrid");
                                                        Label lblfinYear = (Label)row.FindControl("lbl_Finyearfk");
                                                        string finYearfk = Convert.ToString(lblfinYear.Text.Trim());
                                                        string finInsert = string.Empty;
                                                        string finInsertValue = string.Empty;
                                                        string finUpdate = string.Empty;
                                                        if (!finYearfk.Contains("&"))
                                                        {
                                                            finInsert = ",Chl_ActualFinyearFk";
                                                            finInsertValue = ",'" + finYearfk + "'";
                                                            finUpdate = " and finyearfk='" + finYearfk + "'";
                                                        }
                                                        TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                                                        //TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                                                        //TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                                                        TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                                                        Label lblFeeCategory = (Label)row.FindControl("lbl_textCode");
                                                        Label lblFeeCode = (Label)row.FindControl("lbl_feecode");
                                                        Label lblheaderid = (Label)row.FindControl("lbl_hdrid");
                                                        Label lblchltkn = (Label)row.FindControl("lbl_chltkn");
                                                        double remainAmt = 0;
                                                        remainAmt = Convert.ToDouble(txtTotalamt.Text) - Convert.ToDouble(lblchltkn.Text);
                                                        if (remainAmt > 0)
                                                        {

                                                            if (lblheaderid.Text == HdrId)
                                                            {
                                                                double creditamt = 0;

                                                                if (txtTobePaidamt.Text != "")
                                                                {
                                                                    creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                                                                }

                                                                if (creditamt > 0)
                                                                {
                                                                    if (creditamt <= remainAmt)
                                                                    {
                                                                        //new
                                                                        recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                        txt_rcptno.Text = recptNo;


                                                                        string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType" + finInsert + ") VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + HdrId + "," + txtTotalamt.Text + "," + creditamt + "," + lblFeeCategory.Text + "," + finYeaid + "," + bankPK + "," + lblFeeCode.Text + "," + challanType + "" + finInsertValue + ")";
                                                                        d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                        string updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0) +" + creditamt + "  where FeeCategory ='" + lblFeeCategory.Text + "' and HeaderFK ='" + HdrId + "' and LedgerFK ='" + lblFeeCode.Text + "' and App_No='" + appnoNew + "' " + finUpdate + "";
                                                                        d2.update_method_wo_parameter(updateCHlTkn, "Text");


                                                                        totalAmt += creditamt;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                dispHdr += " (" + bnkAcc + ")";
                                                grandtotal = grandtotal + totalAmt;

                                                if (grandtotal > 0)
                                                {

                                                    addpageOK = true;
                                                    createPDFOK = true;
                                                    if (chk == 0)
                                                    {
                                                        //chk++;
                                                        #region Update Challan No
                                                        //recptNo =generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                        //txt_rcptno.Text = recptNo;
                                                        //lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
                                                        //accidRecpt = Convert.ToString(Session["lastAccId"]);
                                                        //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                        //{

                                                        //    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                        //    DataSet dsEachHdr = new DataSet();
                                                        //    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                        //    if (dsEachHdr.Tables.Count > 0)
                                                        //    {
                                                        //        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                        //        {
                                                        //            string selLedge = "	SELECT HeaderFK,LedgerFk,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')";
                                                        //            DataSet dsLedge = new DataSet();
                                                        //            dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                        //            if (dsLedge.Tables.Count > 0)
                                                        //            {
                                                        //                if (dsLedge.Tables[0].Rows.Count > 0)
                                                        //                {
                                                        //                    for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                        //                    {
                                                        //                        string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + ")";
                                                        //                        d2.select_method_wo_parameter(insertChlNo, "Text");
                                                        //                    }
                                                        //                }
                                                        //            }
                                                        //        }
                                                        //    }

                                                        //}

                                                        #endregion
                                                    }

                                                }

                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(FC18);
                                                myprov_pdfpage.Add(FC171);
                                                myprov_pdfpage.Add(FC19);


                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(UC18);
                                                myprov_pdfpage.Add(UC19);
                                                myprov_pdfpage.Add(UC171);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(TC18);
                                                myprov_pdfpage.Add(TC19);
                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;
                                            }
                                        }

                                        if (addpageOK)
                                        {
                                            string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                            d2.update_method_wo_parameter(updateRecpt, "Text");
                                            PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                         new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                            PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                  new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                            myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(TC4);
                                            PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                         new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                            //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                            //myprov_pdfpage.Add(FC08, 250, 125);
                                            //myprov_pdfpage.Add(FC08, 550, 125);
                                            //myprov_pdfpage.Add(FC08, 900, 125);
                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                            PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                            PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                            PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                            myprov_pdfpage.Add(pr1);

                                            PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                            PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                            myprov_pdfpage.Add(pr2);

                                            PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                            PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                            myprov_pdfpage.Add(pr3);

                                            Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table.VisibleHeaders = false;
                                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table.Columns[0].SetWidth(100);
                                            table.Columns[1].SetWidth(60);
                                            table.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table.Cell(0, 0).SetContent("DD No");
                                            }
                                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 0).SetFont(Fontbold1);
                                            table.Cell(0, 1).SetContent("Date");
                                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 1).SetFont(Fontbold1);
                                            table.Cell(0, 2).SetContent("Amount");
                                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(0, 2).SetFont(Fontbold1);
                                            table.Cell(1, 0).SetContent("\n");
                                            table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 0).SetFont(Fontbold1);
                                            table.Cell(1, 1).SetContent("\n");
                                            table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 1).SetFont(Fontbold1);
                                            table.Cell(1, 2).SetContent("\n");
                                            table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable);

                                            Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table1.VisibleHeaders = false;
                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table1.Columns[0].SetWidth(100);
                                            table1.Columns[1].SetWidth(60);
                                            table1.Cell(0, 0).SetContent("2000x");
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(0, 0).SetFont(Fontbold1);
                                            table1.Cell(1, 0).SetContent("500x");
                                            table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(1, 0).SetFont(Fontbold1);
                                            table1.Cell(2, 0).SetContent("100x");
                                            table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(2, 0).SetFont(Fontbold1);
                                            table1.Cell(3, 0).SetContent("50x");
                                            table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(3, 0).SetFont(Fontbold1);
                                            table1.Cell(4, 0).SetContent("20x");
                                            table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(4, 0).SetFont(Fontbold1);
                                            table1.Cell(5, 0).SetContent("10x");
                                            table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(5, 0).SetFont(Fontbold1);
                                            table1.Cell(6, 0).SetContent("5x");
                                            table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(6, 0).SetFont(Fontbold1);
                                            table1.Cell(7, 0).SetContent("Coinsx");
                                            table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(7, 0).SetFont(Fontbold1);
                                            table1.Cell(8, 0).SetContent("Total");
                                            table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table1.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                                //myprov_pdfpage.Add(mobile);aaaa
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                 new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }


                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable1);

                                            myprov_pdfpage.Add(FC);
                                            myprov_pdfpage.Add(ORGI);
                                            myprov_pdfpage.Add(IOB);
                                            //myprov_pdfpage.Add(FC4);
                                            myprov_pdfpage.Add(FC5);
                                            myprov_pdfpage.Add(FC6);
                                            myprov_pdfpage.Add(FC7);
                                            myprov_pdfpage.Add(FC8);
                                            myprov_pdfpage.Add(FC9);
                                            //myprov_pdfpage.Add(FC10);
                                            myprov_pdfpage.Add(FC11);
                                            myprov_pdfpage.Add(FC12);
                                            myprov_pdfpage.Add(FC13);
                                            myprov_pdfpage.Add(FC14);
                                            myprov_pdfpage.Add(FC15);
                                            myprov_pdfpage.Add(FC16);

                                            myprov_pdfpage.Add(FC24);
                                            myprov_pdfpage.Add(FC25);
                                            myprov_pdfpage.Add(FC26);
                                            myprov_pdfpage.Add(FC27);
                                            myprov_pdfpage.Add(FC28);
                                            myprov_pdfpage.Add(FC29);
                                            myprov_pdfpage.Add(FC30);
                                            myprov_pdfpage.Add(FC31);

                                            myprov_pdfpage.Add(FC32);
                                            //myprov_pdfpage.Add(FC33);

                                            //First End
                                            myprov_pdfpage.Add(UC17);

                                            PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table3.VisibleHeaders = false;
                                            table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table3.Columns[0].SetWidth(100);
                                            table3.Columns[1].SetWidth(60);
                                            table3.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table3.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table3.Cell(0, 0).SetContent("DD No");
                                            }
                                            table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 0).SetFont(Fontbold1);
                                            table3.Cell(0, 1).SetContent("Date");
                                            table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 1).SetFont(Fontbold1);
                                            table3.Cell(0, 2).SetContent("Amount");
                                            table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(0, 2).SetFont(Fontbold1);
                                            table3.Cell(1, 0).SetContent("\n");
                                            table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 0).SetFont(Fontbold1);
                                            table3.Cell(1, 1).SetContent("\n");
                                            table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 1).SetFont(Fontbold1);
                                            table3.Cell(1, 2).SetContent("\n");
                                            table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table3.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable3);

                                            Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table14.VisibleHeaders = false;
                                            table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table14.Columns[0].SetWidth(100);
                                            table14.Columns[1].SetWidth(60);
                                            table14.Cell(0, 0).SetContent("2000x");
                                            table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(0, 0).SetFont(Fontbold1);
                                            table14.Cell(1, 0).SetContent("500x");
                                            table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(1, 0).SetFont(Fontbold1);
                                            table14.Cell(2, 0).SetContent("100x");
                                            table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(2, 0).SetFont(Fontbold1);
                                            table14.Cell(3, 0).SetContent("50x");
                                            table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(3, 0).SetFont(Fontbold1);
                                            table14.Cell(4, 0).SetContent("20x");
                                            table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(4, 0).SetFont(Fontbold1);
                                            table14.Cell(5, 0).SetContent("10x");
                                            table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(5, 0).SetFont(Fontbold1);
                                            table14.Cell(6, 0).SetContent("5x");
                                            table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(6, 0).SetFont(Fontbold1);
                                            table14.Cell(7, 0).SetContent("Coinsx");
                                            table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(7, 0).SetFont(Fontbold1);
                                            table14.Cell(8, 0).SetContent("Total");
                                            table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table14.Cell(8, 0).SetFont(Fontbold1);
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {
                                                //Gios.Pdf.PdfTable mobile = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                                //mobile.Cell(0, 0).SetContent("Mobile Number:");
                                                //PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                //                                             new PdfArea(mychallan, 686, y + 530, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No.");
                                                //myprov_pdfpage.Add(mblnoPdfTextArea);
                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 525, y + 380, 160, 500), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);
                                            }
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable4);

                                            myprov_pdfpage.Add(UC);
                                            myprov_pdfpage.Add(UC1);
                                            myprov_pdfpage.Add(UC2);
                                            //myprov_pdfpage.Add(UC4);
                                            myprov_pdfpage.Add(UC5);
                                            myprov_pdfpage.Add(UC6);
                                            myprov_pdfpage.Add(UC7);
                                            myprov_pdfpage.Add(UC8);
                                            myprov_pdfpage.Add(UC9);
                                            //myprov_pdfpage.Add(UC10);
                                            myprov_pdfpage.Add(UC11);
                                            myprov_pdfpage.Add(UC12);
                                            myprov_pdfpage.Add(UC13);
                                            myprov_pdfpage.Add(UC14);
                                            myprov_pdfpage.Add(UC15);
                                            myprov_pdfpage.Add(UC16);


                                            myprov_pdfpage.Add(UC24);
                                            myprov_pdfpage.Add(UC25);
                                            myprov_pdfpage.Add(UC26);
                                            myprov_pdfpage.Add(UC27);
                                            myprov_pdfpage.Add(UC28);
                                            myprov_pdfpage.Add(UC29);
                                            myprov_pdfpage.Add(UC30);
                                            myprov_pdfpage.Add(UC31);
                                            myprov_pdfpage.Add(UC32);
                                            //second End


                                            myprov_pdfpage.Add(TC17);

                                            PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                            PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                            PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                            PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                            PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                            PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                            PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                            PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                            Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                            table5.VisibleHeaders = false;
                                            table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table5.Columns[0].SetWidth(100);
                                            table5.Columns[1].SetWidth(60);
                                            table5.Columns[2].SetWidth(60);

                                            if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                            {
                                                table5.Cell(0, 0).SetContent("Cheque/DD No");
                                            }
                                            else
                                            {
                                                table5.Cell(0, 0).SetContent("DD No");
                                            }
                                            table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 0).SetFont(Fontbold1);
                                            table5.Cell(0, 1).SetContent("Date");
                                            table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 1).SetFont(Fontbold1);
                                            table5.Cell(0, 2).SetContent("Amount");
                                            table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(0, 2).SetFont(Fontbold1);
                                            table5.Cell(1, 0).SetContent("\n");
                                            table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 0).SetFont(Fontbold1);
                                            table5.Cell(1, 1).SetContent("\n");
                                            table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 1).SetFont(Fontbold1);
                                            table5.Cell(1, 2).SetContent("\n");
                                            table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table5.Cell(1, 2).SetFont(Fontbold1);
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                            myprov_pdfpage.Add(myprov_pdfpagetable31);

                                            Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                            table15.VisibleHeaders = false;
                                            table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table15.Columns[0].SetWidth(100);
                                            table15.Columns[1].SetWidth(60);
                                            table15.Cell(0, 0).SetContent("2000x");
                                            table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(0, 0).SetFont(Fontbold1);
                                            table15.Cell(1, 0).SetContent("500x");
                                            table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(1, 0).SetFont(Fontbold1);
                                            table15.Cell(2, 0).SetContent("100x");
                                            table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(2, 0).SetFont(Fontbold1);
                                            table15.Cell(3, 0).SetContent("50x");
                                            table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(3, 0).SetFont(Fontbold1);
                                            table15.Cell(4, 0).SetContent("20x");
                                            table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(4, 0).SetFont(Fontbold1);
                                            table15.Cell(5, 0).SetContent("10x");
                                            table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(5, 0).SetFont(Fontbold1);
                                            table15.Cell(6, 0).SetContent("5x");
                                            table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(6, 0).SetFont(Fontbold1);
                                            table15.Cell(7, 0).SetContent("Coinsx");
                                            table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(7, 0).SetFont(Fontbold1);
                                            table15.Cell(8, 0).SetContent("Total");
                                            table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                            table15.Cell(8, 0).SetFont(Fontbold1);
                                            //
                                            if (checkSchoolSetting(usercode) == 0)//added by abarna
                                            {

                                                PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                  new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                                myprov_pdfpage.Add(mblnoPdfTextArea);

                                            }
                                            Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                            myprov_pdfpage.Add(myprov_pdfpagetable5);

                                            myprov_pdfpage.Add(TC);
                                            myprov_pdfpage.Add(TC1);
                                            myprov_pdfpage.Add(TC2);
                                            //myprov_pdfpage.Add(TC4);
                                            myprov_pdfpage.Add(TC5);
                                            myprov_pdfpage.Add(TC6);
                                            myprov_pdfpage.Add(TC7);
                                            myprov_pdfpage.Add(TC8);
                                            myprov_pdfpage.Add(TC9);
                                            //myprov_pdfpage.Add(TC10);
                                            myprov_pdfpage.Add(TC11);
                                            myprov_pdfpage.Add(TC12);
                                            myprov_pdfpage.Add(TC13);
                                            myprov_pdfpage.Add(TC14);
                                            myprov_pdfpage.Add(TC15);
                                            myprov_pdfpage.Add(TC16);
                                            myprov_pdfpage.Add(TC17);
                                            myprov_pdfpage.Add(TC24);
                                            myprov_pdfpage.Add(TC25);
                                            myprov_pdfpage.Add(TC26);
                                            myprov_pdfpage.Add(TC27);
                                            myprov_pdfpage.Add(TC28);
                                            myprov_pdfpage.Add(TC29);
                                            myprov_pdfpage.Add(TC30);
                                            myprov_pdfpage.Add(TC31);
                                            myprov_pdfpage.Add(TC32);

                                            myprov_pdfpage.SaveToDocument();
                                            #endregion
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {
                                    double ovrallcredit = 0;
                                    double grandtotal = 0.00;

                                    checkedHeaderOK = false;
                                    string bnkAcc = "";
                                    //Add new challan Page in this loop
                                    bool addpageOK = false;
                                    #region TOp portion

                                    int y = 0;

                                    Gios.Pdf.PdfPage myprov_pdfpage = mychallan.NewPage();

                                    PdfTextArea FC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea ORGI;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "ORIGINAL");
                                    }
                                    else
                                    {
                                        ORGI = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                          new PdfArea(mychallan, 270, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Bank Copy");
                                    }
                                    PdfTextArea IOB = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 70, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea FC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea FC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 30, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea FC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    //PdfTextArea FC33 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                    //                                                   new PdfArea(mychallan, 70, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, DateTime.Now.ToString("dd/MM/yyyy"));
                                    PdfTextArea FC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 240, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea FC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                    PdfTextArea FC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 25, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 250, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea FC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea FC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea FC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {

                                        FC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 70, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }
                                    PdfTextArea FC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 20, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea FC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea FC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 290, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea FC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 20, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");




                                    myprov_pdfpage.Add(FC17);
                                    string text = "";

                                    //First Ends

                                    PdfTextArea UC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea UC1;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "DUPLICATE");
                                    }
                                    else
                                    {
                                        UC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 590, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "School Copy");
                                    }
                                    PdfTextArea UC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 400, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea UC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea UC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 360, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea UC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    PdfTextArea UC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 570, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea UC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);

                                    PdfTextArea UC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mychallan, 355, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 550, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea UC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea UC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea UC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {
                                        UC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                                   new PdfArea(mychallan, 400, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }
                                    PdfTextArea UC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 350, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea UC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea UC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 620, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea UC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 350, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                    //second End
                                    y = 0;


                                    PdfTextArea TC = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 30, 150, 20), System.Drawing.ContentAlignment.MiddleLeft, "FEE CHALLAN");
                                    PdfTextArea TC1;
                                    if (checkSchoolSetting(usercode) != 0)//added by abarna 5.03.2018
                                    {
                                        TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "TRIPLICATE");
                                    }
                                    else
                                    {
                                        TC1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 920, 30, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student Copy");
                                    }
                                    PdfTextArea TC2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 720, 90, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankName);
                                    //PdfTextArea TC4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                    //                                                      new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Bank Branch");
                                    PdfTextArea TC5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, 50, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                                    PdfTextArea TC6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 670, 70, 300, 20), System.Drawing.ContentAlignment.MiddleCenter, add1 + add2);

                                    PdfTextArea TC8 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 900, 110, 85, 20), System.Drawing.ContentAlignment.MiddleRight, rolldisplay + rollvalue);
                                    PdfTextArea TC9 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan Date:" + txt_date.Text);
                                    PdfTextArea TC32 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 140, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Student's Name:" + studname);
                                    //PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                    //                                                 new PdfArea(mychallan, 900, 125, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Day");
                                    PdfTextArea TC11 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 145, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea TC12 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 155, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, cursem);
                                    PdfTextArea TC13;
                                    if (checkSchoolSetting(usercode) != 0)
                                    {
                                        TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                           new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg);
                                    }
                                    else
                                    {
                                        TC13 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 760, 155, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, "Class & Group:" + deg + "-" + sem);
                                    }
                                    PdfTextArea TC14 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 680, 160, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                    PdfTextArea TC15 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "PARTICULARS");
                                    PdfTextArea TC16 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 950, 170, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Rs.");
                                    PdfTextArea TC17 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 680, 175, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");

                                    PdfTextArea FC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                          new PdfArea(mychallan, 250, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    PdfTextArea UC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 580, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    PdfTextArea TC10 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 910, 140, 70, 20), System.Drawing.ContentAlignment.MiddleRight, stream);
                                    myprov_pdfpage.Add(FC10);
                                    myprov_pdfpage.Add(UC10);
                                    myprov_pdfpage.Add(TC10);
                                    y = 0;

                                    #endregion
                                    if (rbl_headerselect.SelectedIndex == 1)
                                    {
                                        //Header
                                        #region Middle Portion challan
                                        int chk = 0;
                                        for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                        {
                                            if (!cbl_grpheader.Items[indx].Selected)
                                            {
                                                continue;
                                            }

                                            string HdrId = "";
                                            string dispHdr = "";

                                            dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);
                                            HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);

                                            double totalAmt = 0;


                                            foreach (GridViewRow row in grid_Details.Rows)
                                            {
                                                //TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                                                //Label lblheaderid = (Label)row.FindControl("lbl_hdrid");

                                                TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                                                //TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                                                //TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                                                TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                                                Label lblFeeCategory = (Label)row.FindControl("lbl_textCode");
                                                Label lblFeeCode = (Label)row.FindControl("lbl_feecode");
                                                Label lblheaderid = (Label)row.FindControl("lbl_hdrid");
                                                Label lblchltkn = (Label)row.FindControl("lbl_chltkn");
                                                double remainAmt = 0;
                                                remainAmt = Convert.ToDouble(txtTotalamt.Text) - Convert.ToDouble(lblchltkn.Text);
                                                if (remainAmt > 0)
                                                {


                                                    if (lblheaderid.Text == HdrId)
                                                    {
                                                        double creditamt = 0;

                                                        if (txtTobePaidamt.Text != "")
                                                        {
                                                            creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                                                        }

                                                        if (creditamt > 0)
                                                        {
                                                            if (creditamt <= remainAmt)
                                                            {
                                                                //new
                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                txt_rcptno.Text = recptNo;


                                                                string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + HdrId + "," + txtTotalamt.Text + "," + creditamt + "," + lblFeeCategory.Text + "," + finYeaid + "," + bankPK + "," + lblFeeCode.Text + "," + challanType + ")";
                                                                d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                string updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0) +" + creditamt + "  where FeeCategory ='" + lblFeeCategory.Text + "' and HeaderFK ='" + HdrId + "' and LedgerFK ='" + lblFeeCode.Text + "' and App_No='" + appnoNew + "' ";
                                                                d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                                                totalAmt += creditamt;
                                                            }
                                                        }
                                                    }
                                                }
                                            }


                                            grandtotal = grandtotal + totalAmt;
                                            //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                            bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                            dispHdr += " (" + bnkAcc + ")";

                                            if (grandtotal > 0)
                                            {

                                                addpageOK = true;
                                                createPDFOK = true;
                                                if (chk == 0)
                                                {
                                                    //chk++;
                                                    #region Update Challan No
                                                    //recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                    //txt_rcptno.Text = recptNo;
                                                    //lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
                                                    //accidRecpt = Convert.ToString(Session["lastAccId"]);
                                                    ////for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                    ////{

                                                    //string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                    //DataSet dsEachHdr = new DataSet();
                                                    //dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                    //if (dsEachHdr.Tables.Count > 0)
                                                    //{
                                                    //    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                    //    {
                                                    //        string selLedge = "	SELECT HeaderFK,LedgerFk,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')";
                                                    //        DataSet dsLedge = new DataSet();
                                                    //        dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                    //        if (dsLedge.Tables.Count > 0)
                                                    //        {
                                                    //            if (dsLedge.Tables[0].Rows.Count > 0)
                                                    //            {
                                                    //                for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                    //                {
                                                    //                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + ")";
                                                    //                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}

                                                    ////}

                                                    #endregion
                                                }

                                            }
                                            if (totalAmt > 0)
                                            {
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(FC18);
                                                myprov_pdfpage.Add(FC171);
                                                myprov_pdfpage.Add(FC19);


                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(UC18);
                                                myprov_pdfpage.Add(UC19);
                                                myprov_pdfpage.Add(UC171);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(TC18);
                                                myprov_pdfpage.Add(TC19);
                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;
                                            }


                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //Ledger wise
                                        #region Middle Portion challan
                                        int chk = 0;
                                        for (int indx = 0; indx < cbl_grpheader.Items.Count; indx++)
                                        {
                                            if (!cbl_grpheader.Items[indx].Selected)
                                            {
                                                continue;
                                            }

                                            //string QhdrId = "select header_id,ChlHeaderName,BankAccNo from ChlHeaderSettings where Stream = '" + stream + "' and header_id in ('" + cbl_grpheader.Items[indx].Value + "')";
                                            string HdrId = "";
                                            string dispHdr = "";

                                            //DataSet ds1 = new DataSet();
                                            //ds1 = d2.select_method_wo_parameter(QhdrId, "Text");
                                            //if (ds1.Tables.Count > 0)
                                            //{
                                            //    if (ds1.Tables[0].Rows.Count > 0)
                                            //    {

                                            //        bnkAcc = Convert.ToString(ds1.Tables[0].Rows[0]["BankAccNo"]);
                                            //    }
                                            //}
                                            dispHdr = Convert.ToString(cbl_grpheader.Items[indx].Text);
                                            HdrId = Convert.ToString(cbl_grpheader.Items[indx].Value);

                                            double totalAmt = 0;


                                            foreach (GridViewRow row in grid_Details.Rows)
                                            {
                                                //TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");
                                                ////Label lblheaderid = (Label)row.FindControl("lbl_hdrid");
                                                //Label lblheaderid = (Label)row.FindControl("lbl_feecode");

                                                TextBox txtTotalamt = (TextBox)row.FindControl("txt_tot_amt");
                                                //TextBox txtPaidamt = (TextBox)row.FindControl("txt_paid_amt");
                                                //TextBox txtBalamt = (TextBox)row.FindControl("txt_bal_amt");
                                                TextBox txtTobePaidamt = (TextBox)row.FindControl("txt_tobepaid_amt");

                                                Label lblFeeCategory = (Label)row.FindControl("lbl_textCode");
                                                Label lblFeeCode = (Label)row.FindControl("lbl_feecode");
                                                Label lblheaderid = (Label)row.FindControl("lbl_hdrid");

                                                Label lblchltkn = (Label)row.FindControl("lbl_chltkn");
                                                double remainAmt = 0;
                                                remainAmt = Convert.ToDouble(txtTotalamt.Text) - Convert.ToDouble(lblchltkn.Text);
                                                if (remainAmt > 0)
                                                {
                                                    if (lblFeeCode.Text == HdrId)
                                                    {
                                                        double creditamt = 0;

                                                        if (txtTobePaidamt.Text != "")
                                                        {
                                                            creditamt = Convert.ToDouble(txtTobePaidamt.Text);
                                                        }

                                                        if (creditamt > 0)
                                                        {

                                                            if (creditamt <= remainAmt)
                                                            {
                                                                //new
                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                txt_rcptno.Text = recptNo;


                                                                string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + lblheaderid.Text + "," + txtTotalamt.Text + "," + creditamt + "," + lblFeeCategory.Text + "," + finYeaid + "," + bankPK + "," + lblFeeCode.Text + "," + challanType + ")";
                                                                d2.select_method_wo_parameter(insertChlNo, "Text");

                                                                string updateCHlTkn = " update FT_FeeAllot set ChlTaken =ISNULL( ChlTaken,0) +" + creditamt + "  where FeeCategory ='" + lblFeeCategory.Text + "' and HeaderFK ='" + lblheaderid.Text + "' and LedgerFK ='" + lblFeeCode.Text + "' and App_No='" + appnoNew + "' ";
                                                                d2.update_method_wo_parameter(updateCHlTkn, "Text");

                                                                totalAmt += creditamt;
                                                            }
                                                        }
                                                    }
                                                }
                                            }


                                            grandtotal = grandtotal + totalAmt;

                                            if (grandtotal > 0)
                                            {

                                                addpageOK = true;
                                                createPDFOK = true;
                                                if (chk == 0)
                                                {
                                                    //chk++;
                                                    #region Update Challan No
                                                    //recptNo =generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                    //txt_rcptno.Text = recptNo;
                                                    //lastRecptNo = Convert.ToString(Session["lastCHlNO"]);
                                                    //accidRecpt = Convert.ToString(Session["lastAccId"]);
                                                    ////for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                    ////{

                                                    //string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";
                                                    //DataSet dsEachHdr = new DataSet();
                                                    //dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                    //if (dsEachHdr.Tables.Count > 0)
                                                    //{
                                                    //    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                    //    {
                                                    //        string selLedge = "	SELECT HeaderFK,LedgerFk,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')";
                                                    //        DataSet dsLedge = new DataSet();
                                                    //        dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                    //        if (dsLedge.Tables.Count > 0)
                                                    //        {
                                                    //            if (dsLedge.Tables[0].Rows.Count > 0)
                                                    //            {
                                                    //                for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                    //                {
                                                    //                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + ")";
                                                    //                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}

                                                    ////}

                                                    #endregion
                                                }

                                            }
                                            if (totalAmt > 0)
                                            {
                                                PdfTextArea FC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 25, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea FC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 270, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea FC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 20, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(FC18);
                                                myprov_pdfpage.Add(FC171);
                                                myprov_pdfpage.Add(FC19);


                                                PdfTextArea UC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 355, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea UC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 600, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea UC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 350, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(UC18);
                                                myprov_pdfpage.Add(UC19);
                                                myprov_pdfpage.Add(UC171);

                                                PdfTextArea TC18 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 685, y + 185, 250, 20), System.Drawing.ContentAlignment.MiddleLeft, dispHdr);
                                                PdfTextArea TC19 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                                new PdfArea(mychallan, 930, y + 185, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(totalAmt) + "." + returnDecimalPart(totalAmt));
                                                PdfTextArea TC171 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mychallan, 680, y + 190, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");


                                                myprov_pdfpage.Add(TC18);
                                                myprov_pdfpage.Add(TC19);
                                                myprov_pdfpage.Add(TC171);
                                                y = y + 15;
                                            }


                                        }
                                        #endregion
                                    }
                                    if (addpageOK)
                                    {
                                        string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");
                                        PdfTextArea FC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                                                     new PdfArea(mychallan, 70, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                        PdfTextArea UC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 400, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);
                                        PdfTextArea TC4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                              new PdfArea(mychallan, 720, 60, 200, 20), System.Drawing.ContentAlignment.MiddleCenter, bankCity);

                                        myprov_pdfpage.Add(FC4);
                                        myprov_pdfpage.Add(UC4);
                                        myprov_pdfpage.Add(TC4);
                                        PdfTextArea FC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                     new PdfArea(mychallan, 25, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                        PdfTextArea UC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                        PdfTextArea TC7 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, 110, 200, 20), System.Drawing.ContentAlignment.MiddleLeft, "Challan No:" + recptNo);
                                        //PdfImage FC08 = mychallan.NewImage(generateBarcode(recptNo));

                                        //myprov_pdfpage.Add(FC08, 250, 125);
                                        //myprov_pdfpage.Add(FC08, 550, 125);
                                        //myprov_pdfpage.Add(FC08, 900, 125);
                                        #region Bottom Portion of Challan

                                        text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                        PdfTextArea FC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 25, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea FC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 270, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea FC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 20, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea FC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea FC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea FC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 25, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea FC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea FC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 25, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");
                                        PdfArea tete = new PdfArea(mychallan, 20, 20, 310, y + 300);
                                        PdfRectangle pr1 = new PdfRectangle(mychallan, tete, Color.Black);
                                        myprov_pdfpage.Add(pr1);

                                        PdfArea tete2 = new PdfArea(mychallan, 350, 20, 310, y + 300);
                                        PdfRectangle pr2 = new PdfRectangle(mychallan, tete2, Color.Black);
                                        myprov_pdfpage.Add(pr2);

                                        PdfArea tete3 = new PdfArea(mychallan, 680, 20, 310, y + 300);
                                        PdfRectangle pr3 = new PdfRectangle(mychallan, tete3, Color.Black);
                                        myprov_pdfpage.Add(pr3);

                                        Gios.Pdf.PdfTable table = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table.VisibleHeaders = false;
                                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table.Columns[0].SetWidth(100);
                                        table.Columns[1].SetWidth(60);
                                        table.Columns[2].SetWidth(60);
                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table.Cell(0, 0).SetContent("DD No");
                                        }
                                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 0).SetFont(Fontbold1);
                                        table.Cell(0, 1).SetContent("Date");
                                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 1).SetFont(Fontbold1);
                                        table.Cell(0, 2).SetContent("Amount");
                                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(0, 2).SetFont(Fontbold1);
                                        table.Cell(1, 0).SetContent("\n");
                                        table.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 0).SetFont(Fontbold1);
                                        table.Cell(1, 1).SetContent("\n");
                                        table.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 1).SetFont(Fontbold1);
                                        table.Cell(1, 2).SetContent("\n");
                                        table.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable = table.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable);

                                        Gios.Pdf.PdfTable table1 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table1.VisibleHeaders = false;
                                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table1.Columns[0].SetWidth(100);
                                        table1.Columns[1].SetWidth(60);
                                        table1.Cell(0, 0).SetContent("2000x");
                                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(0, 0).SetFont(Fontbold1);
                                        table1.Cell(1, 0).SetContent("500x");
                                        table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(1, 0).SetFont(Fontbold1);
                                        table1.Cell(2, 0).SetContent("100x");
                                        table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(2, 0).SetFont(Fontbold1);
                                        table1.Cell(3, 0).SetContent("50x");
                                        table1.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(3, 0).SetFont(Fontbold1);
                                        table1.Cell(4, 0).SetContent("20x");
                                        table1.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(4, 0).SetFont(Fontbold1);
                                        table1.Cell(5, 0).SetContent("10x");
                                        table1.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(5, 0).SetFont(Fontbold1);
                                        table1.Cell(6, 0).SetContent("5x");
                                        table1.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(6, 0).SetFont(Fontbold1);
                                        table1.Cell(7, 0).SetContent("Coinsx");
                                        table1.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(7, 0).SetFont(Fontbold1);
                                        table1.Cell(8, 0).SetContent("Total");
                                        table1.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(8, 0).SetFont(Fontbold1);
                                        if (checkSchoolSetting(usercode) == 0)//added by abarna ddeee
                                        {
                                            //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                            //myprov_pdfpage.Add(mobile);aaaa
                                            PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 195, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");//mobile no1
                                            myprov_pdfpage.Add(mblnoPdfTextArea);

                                        }



                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 20, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable1);

                                        myprov_pdfpage.Add(FC);
                                        myprov_pdfpage.Add(ORGI);
                                        myprov_pdfpage.Add(IOB);
                                        //myprov_pdfpage.Add(FC4);
                                        myprov_pdfpage.Add(FC5);
                                        myprov_pdfpage.Add(FC6);
                                        myprov_pdfpage.Add(FC7);
                                        myprov_pdfpage.Add(FC8);
                                        myprov_pdfpage.Add(FC9);
                                        //myprov_pdfpage.Add(FC10);
                                        myprov_pdfpage.Add(FC11);
                                        myprov_pdfpage.Add(FC12);
                                        myprov_pdfpage.Add(FC13);
                                        myprov_pdfpage.Add(FC14);
                                        myprov_pdfpage.Add(FC15);
                                        myprov_pdfpage.Add(FC16);

                                        myprov_pdfpage.Add(FC24);
                                        myprov_pdfpage.Add(FC25);
                                        myprov_pdfpage.Add(FC26);
                                        myprov_pdfpage.Add(FC27);
                                        myprov_pdfpage.Add(FC28);
                                        myprov_pdfpage.Add(FC29);
                                        myprov_pdfpage.Add(FC30);
                                        myprov_pdfpage.Add(FC31);

                                        myprov_pdfpage.Add(FC32);
                                        //myprov_pdfpage.Add(FC33);

                                        //First End
                                        myprov_pdfpage.Add(UC17);

                                        PdfTextArea UC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 355, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea UC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 600, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea UC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 350, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea UC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea UC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea UC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 355, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea UC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 580, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea UC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 355, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                        Gios.Pdf.PdfTable table3 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table3.VisibleHeaders = false;
                                        table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table3.Columns[0].SetWidth(100);
                                        table3.Columns[1].SetWidth(60);
                                        table3.Columns[2].SetWidth(60);

                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table3.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table3.Cell(0, 0).SetContent("DD No");
                                        }
                                        table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 0).SetFont(Fontbold1);
                                        table3.Cell(0, 1).SetContent("Date");
                                        table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 1).SetFont(Fontbold1);
                                        table3.Cell(0, 2).SetContent("Amount");
                                        table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(0, 2).SetFont(Fontbold1);
                                        table3.Cell(1, 0).SetContent("\n");
                                        table3.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 0).SetFont(Fontbold1);
                                        table3.Cell(1, 1).SetContent("\n");
                                        table3.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 1).SetFont(Fontbold1);
                                        table3.Cell(1, 2).SetContent("\n");
                                        table3.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable3 = table3.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable3);

                                        Gios.Pdf.PdfTable table14 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table14.VisibleHeaders = false;
                                        table14.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table14.Columns[0].SetWidth(100);
                                        table14.Columns[1].SetWidth(60);
                                        table14.Cell(0, 0).SetContent("2000x");
                                        table14.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(0, 0).SetFont(Fontbold1);
                                        table14.Cell(1, 0).SetContent("500x");
                                        table14.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(1, 0).SetFont(Fontbold1);
                                        table14.Cell(2, 0).SetContent("100x");
                                        table14.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(2, 0).SetFont(Fontbold1);
                                        table14.Cell(3, 0).SetContent("50x");
                                        table14.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(3, 0).SetFont(Fontbold1);
                                        table14.Cell(4, 0).SetContent("20x");
                                        table14.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(4, 0).SetFont(Fontbold1);
                                        table14.Cell(5, 0).SetContent("10x");
                                        table14.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(5, 0).SetFont(Fontbold1);
                                        table14.Cell(6, 0).SetContent("5x");
                                        table14.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(6, 0).SetFont(Fontbold1);
                                        table14.Cell(7, 0).SetContent("Coinsx");
                                        table14.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(7, 0).SetFont(Fontbold1);
                                        table14.Cell(8, 0).SetContent("Total");
                                        table14.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table14.Cell(8, 0).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable4 = table14.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 350, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable4);
                                        if (checkSchoolSetting(usercode) == 0)//added by abarna 
                                        {
                                            //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                            //myprov_pdfpage.Add(mobile);

                                            //PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                    new PdfArea(mychallan, 231, y + 424, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No.");
                                            //myprov_pdfpage.Add(mblnoPdfTextArea);
                                            PdfTextArea mblnoPdfTextArea2 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 525, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");//2
                                            myprov_pdfpage.Add(mblnoPdfTextArea2);


                                        }
                                     

                                        myprov_pdfpage.Add(UC);
                                        myprov_pdfpage.Add(UC1);
                                        myprov_pdfpage.Add(UC2);
                                        //myprov_pdfpage.Add(UC4);
                                        myprov_pdfpage.Add(UC5);
                                        myprov_pdfpage.Add(UC6);
                                        myprov_pdfpage.Add(UC7);
                                        myprov_pdfpage.Add(UC8);
                                        myprov_pdfpage.Add(UC9);
                                        //myprov_pdfpage.Add(UC10);
                                        myprov_pdfpage.Add(UC11);
                                        myprov_pdfpage.Add(UC12);
                                        myprov_pdfpage.Add(UC13);
                                        myprov_pdfpage.Add(UC14);
                                        myprov_pdfpage.Add(UC15);
                                        myprov_pdfpage.Add(UC16);


                                        myprov_pdfpage.Add(UC24);
                                        myprov_pdfpage.Add(UC25);
                                        myprov_pdfpage.Add(UC26);
                                        myprov_pdfpage.Add(UC27);
                                        myprov_pdfpage.Add(UC28);
                                        myprov_pdfpage.Add(UC29);
                                        myprov_pdfpage.Add(UC30);
                                        myprov_pdfpage.Add(UC31);
                                        myprov_pdfpage.Add(UC32);
                                        //second End


                                        myprov_pdfpage.Add(TC17);

                                        PdfTextArea TC24 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 685, y + 190, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "GRAND TOTAL");
                                        PdfTextArea TC25 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 930, y + 190, 50, 20), System.Drawing.ContentAlignment.MiddleRight, returnIntegerPart(grandtotal) + "." + returnDecimalPart(grandtotal));
                                        PdfTextArea TC26 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                            new PdfArea(mychallan, 680, y + 195, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, "______________________________________________________________________");
                                        PdfTextArea TC27 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 205, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, text.ToString());
                                        PdfTextArea TC28 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 225, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Remitter");
                                        PdfTextArea TC29 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                        new PdfArea(mychallan, 685, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "CASHIER");
                                        PdfTextArea TC30 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 910, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "MANAGER/ACCT");
                                        PdfTextArea TC31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                                    new PdfArea(mychallan, 685, y + 300, 270, 20), System.Drawing.ContentAlignment.MiddleLeft, "Please preserve this challan for claims,if any,at the Bursar's Office");


                                        Gios.Pdf.PdfTable table5 = mychallan.NewTable(Fontsmall, 2, 3, 5);
                                        table5.VisibleHeaders = false;
                                        table5.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table5.Columns[0].SetWidth(100);
                                        table5.Columns[1].SetWidth(60);
                                        table5.Columns[2].SetWidth(60);

                                        if (checkSchoolSetting(usercode) != 0)//change by abarna 06.03.2018
                                        {
                                            table5.Cell(0, 0).SetContent("Cheque/DD No");
                                        }
                                        else
                                        {
                                            table5.Cell(0, 0).SetContent("DD No");
                                        }
                                        table5.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 0).SetFont(Fontbold1);
                                        table5.Cell(0, 1).SetContent("Date");
                                        table5.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 1).SetFont(Fontbold1);
                                        table5.Cell(0, 2).SetContent("Amount");
                                        table5.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(0, 2).SetFont(Fontbold1);
                                        table5.Cell(1, 0).SetContent("\n");
                                        table5.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 0).SetFont(Fontbold1);
                                        table5.Cell(1, 1).SetContent("\n");
                                        table5.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 1).SetFont(Fontbold1);
                                        table5.Cell(1, 2).SetContent("\n");
                                        table5.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table5.Cell(1, 2).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable31 = table5.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 330, 310, 250));
                                        myprov_pdfpage.Add(myprov_pdfpagetable31);

                                        Gios.Pdf.PdfTable table15 = mychallan.NewTable(Fontsmall, 9, 2, 3);
                                        table15.VisibleHeaders = false;
                                        table15.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                        table15.Columns[0].SetWidth(100);
                                        table15.Columns[1].SetWidth(60);
                                        table15.Cell(0, 0).SetContent("2000x");
                                        table15.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(0, 0).SetFont(Fontbold1);
                                        table15.Cell(1, 0).SetContent("500x");
                                        table15.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(1, 0).SetFont(Fontbold1);
                                        table15.Cell(2, 0).SetContent("100x");
                                        table15.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(2, 0).SetFont(Fontbold1);
                                        table15.Cell(3, 0).SetContent("50x");
                                        table15.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(3, 0).SetFont(Fontbold1);
                                        table15.Cell(4, 0).SetContent("20x");
                                        table15.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(4, 0).SetFont(Fontbold1);
                                        table15.Cell(5, 0).SetContent("10x");
                                        table15.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(5, 0).SetFont(Fontbold1);
                                        table15.Cell(6, 0).SetContent("5x");
                                        table15.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(6, 0).SetFont(Fontbold1);
                                        table15.Cell(7, 0).SetContent("Coinsx");
                                        table15.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(7, 0).SetFont(Fontbold1);
                                        table15.Cell(8, 0).SetContent("Total");
                                        table15.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table15.Cell(8, 0).SetFont(Fontbold1);
                                        Gios.Pdf.PdfTablePage myprov_pdfpagetable5 = table15.CreateTablePage(new Gios.Pdf.PdfArea(mychallan, 680, y + 380, 160, 500));
                                        myprov_pdfpage.Add(myprov_pdfpagetable5);
                                        if (checkSchoolSetting(usercode) == 0)//added by abarna 
                                        {
                                            //PdfTextArea mobile = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                        new PdfArea(mychallan, 250, y + 265, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile Number:");
                                            //myprov_pdfpage.Add(mobile);

                                            //PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                            //                                                    new PdfArea(mychallan, 350, y + 350, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No.");
                                            //myprov_pdfpage.Add(mblnoPdfTextArea);
                                            PdfTextArea mblnoPdfTextArea = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                             new PdfArea(mychallan, 850, y + 380, 100, 20), System.Drawing.ContentAlignment.MiddleLeft, "Mobile No:");
                                            myprov_pdfpage.Add(mblnoPdfTextArea);

                                        }
                                        

                                        myprov_pdfpage.Add(TC);
                                        myprov_pdfpage.Add(TC1);
                                        myprov_pdfpage.Add(TC2);
                                        //myprov_pdfpage.Add(TC4);
                                        myprov_pdfpage.Add(TC5);
                                        myprov_pdfpage.Add(TC6);
                                        myprov_pdfpage.Add(TC7);
                                        myprov_pdfpage.Add(TC8);
                                        myprov_pdfpage.Add(TC9);
                                        //myprov_pdfpage.Add(TC10);
                                        myprov_pdfpage.Add(TC11);
                                        myprov_pdfpage.Add(TC12);
                                        myprov_pdfpage.Add(TC13);
                                        myprov_pdfpage.Add(TC14);
                                        myprov_pdfpage.Add(TC15);
                                        myprov_pdfpage.Add(TC16);
                                        myprov_pdfpage.Add(TC17);
                                        myprov_pdfpage.Add(TC24);
                                        myprov_pdfpage.Add(TC25);
                                        myprov_pdfpage.Add(TC26);
                                        myprov_pdfpage.Add(TC27);
                                        myprov_pdfpage.Add(TC28);
                                        myprov_pdfpage.Add(TC29);
                                        myprov_pdfpage.Add(TC30);
                                        myprov_pdfpage.Add(TC31);
                                        myprov_pdfpage.Add(TC32);

                                        myprov_pdfpage.SaveToDocument();
                                        #endregion
                                    }
                                }
                                //Middle portion of challan End

                                //Bottom portion of the challan

                                //Bottom portion of the challan End


                                #endregion
                            }
                        }

                        //New COde END

                        if (createPDFOK)
                        {
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";

                                string szFile = "Challan" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";

                                mychallan.SaveToFile(szPath + szFile);
                                //Response.ClearHeaders();
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                //Response.ContentType = "application/pdf";
                                //Response.WriteFile(szPath + szFile);
                                //Response.AddHeader("Refresh", "1; url=receiptPrint.aspx");
                                //Response.Write("<script>window.open('PrintPage.aspx?name=" + szFile + "', '_blank');</script>");

                                imgDIVVisible = true;
                                //this.Form.DefaultButton = "btn_alertclose";
                                lbl_alert.Text = "Challan Generated";
                                CreateReceiptOK = true;
                                return szFile;
                            }
                        }
                        else
                        {
                            imgDIVVisible = true;
                            //this.Form.DefaultButton = "btn_alertclose";
                            lbl_alert.Text = "Challan Cannot Be Generated";
                        }
                        #endregion

                    }
                    catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "General Mcc Challan"); }

                    #endregion
                }
            }
            else
            {
                imgDIVVisible = true;
                //this.Form.DefaultButton = "btn_alertclose";
                lbl_alert.Text = "Challan Number Not Generated";
            }

            txt_rcptno.Text = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "General Mcc Challan"); }
        return string.Empty;
    }
}