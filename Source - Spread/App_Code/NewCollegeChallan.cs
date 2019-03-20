using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using FarPoint.Web.Spread;

/// <summary>
/// Summary description for NewCollegeChallan
/// </summary>
public class NewCollegeChallan : ReuasableMethods
{
    DAccess2 d2 = new DAccess2();

    public NewCollegeChallan()
    {
    }

    public string printNewCollegeChallan(CheckBox cb_selcthd, RadioButtonList rbl_headerselect, string collegecode1, string usercode, string lastRecptNo, string accidRecpt, RadioButton rdo_multi, ref TextBox txt_rcptno, FpSpread Fpspread1, TextBox txt_totnoofstudents, TextBox txt_date, TextBox txt_name, DropDownList ddl_semMultiple, DropDownList rbl_rollno, DropDownList ddl_collegebank, ref Label lbl_alert, ref bool imgDIVVisible, CheckBoxList cbl_grpheader, ref TextBox Txt_amt, GridView grid_Details, ref bool contentVisible, ref bool CreateReceiptOK, string lblstaticrollno, string ddlSEM, string ddlTYPE, string ddlDEPT)
    {
        CreateReceiptOK = false;
        contentVisible = false;
        imgDIVVisible = false;
        lastRecptNo = string.Empty;
        accidRecpt = string.Empty;
        StringBuilder contentDiv = new StringBuilder();
        //New College
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
            string roll_admit = lblstaticrollno.Trim();

            txt_rcptno.Text = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);

            string finYeaid = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (lastRecptNo != "")
            {
                string shift = "";
                string acaYear = System.DateTime.Now.Year.ToString();
                shift = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();
                if (shift == "0" || shift == "")
                {
                    shift = "";
                }
                else
                {
                    shift = "(" + shift + ")";
                }
                string counterName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
                if (counterName.Trim() == "0")
                    counterName = string.Empty;

                string colName = string.Empty;
                //colName = d2.GetFunction("select collname from collinfo where college_code=" + collegecode1 + "").Trim();
                //if (colName == "0" || colName == "")
                //    colName = string.Empty;
                //if (colName != string.Empty)
                //{
                //    string tempCName = colName.ToUpper().Replace(" ", "");
                //    if (tempCName.Contains("NEWCOLLEGE"))
                //    {
                //colName = "THE NEW COLLEGE (AUTONOMOUS) CH-14";
                //    }
                //}
                //added by sudhagar 29.03.2017
                string hstlName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeHostelName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");

                string incShift = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeShiftName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
                string hstincName = string.Empty;
                string hstlfees = string.Empty;
                if (!string.IsNullOrEmpty(incShift) && incShift != "0")
                    shift = "";
                if (!string.IsNullOrEmpty(hstlName) && hstlName != "0")
                {
                    hstincName = hstlName;
                    hstlfees = hstlName;
                }
                else
                {
                    hstincName = "AUTONOMOUS";
                    hstlfees = "COLLEGE";
                }
                colName = "THE NEW COLLEGE (" + hstincName + ") CH-14";

                string useIFSC = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                string parName = string.Empty;
                parName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code =" + collegecode1 + "").Trim();
                if (parName == "0" || parName == "")
                    parName = "Particulars";
                else
                    parName = "Particulars - " + parName;

                string useDegAcr = d2.GetFunction("select LinkValue from New_InsSettings where LinkName= 'DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ").Trim();

                int useDenom = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'").Trim());

                if (rdo_multi.Checked)
                {
                    #region For Multiple students
                    //rbl_rollno.SelectedIndex = 1;
                    int count = 0;
                    bool createPDFOK = false;

                    StringBuilder sbHtml = new StringBuilder();
                    for (int row = 1; row < Fpspread1.Sheets[0].RowCount; row++)
                    {
                        sbHtml.Clear();
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

                                studname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(row), 6].Text);

                                feeCategory = Convert.ToString(ddl_semMultiple.SelectedValue);
                                string queryRollApp = "";
                                if (rbl_rollno.SelectedItem.Text.Trim() == "App No")
                                {
                                    queryRollApp = "select '' Roll_No,app_formno,'' smart_serial_no,app_no, '' Reg_No  from applyn where app_formno='" + roll_admit + "'  and college_code='" + collegecode1 + "' ";
                                }
                                else
                                {
                                    queryRollApp = "select r.Roll_No,a.app_formno,r.smart_serial_no,a.app_no,r.Reg_No  from Registration r,applyn a where r.App_No=a.app_no  and r.college_code='" + collegecode1 + "'  and r.Roll_Admit='" + roll_admit + "'";
                                }
                                DataSet dsRollApp = new DataSet();
                                dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                                if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                                {
                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                    smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
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
                                }// jairam
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
                                string bankAddress = "";
                                if (ddl_collegebank.Items.Count > 0)
                                {
                                    bankName = ddl_collegebank.SelectedItem.Text.Split('-')[0];
                                    bankPK = ddl_collegebank.SelectedItem.Value;
                                    bankAddress = d2.GetFunction("select Street+', '+(select MasterValue from CO_MasterValues where MasterCode=District)+'-'+PinCode as address from FM_FinBankMaster where BankPK=" + bankPK + "");
                                    bankAddress = "(" + bankAddress + ")";
                                    bankCity = d2.GetFunction("select Upper(BankBranch) as city from FM_FinBankMaster where BankPK=" + bankPK + "") + " Branch";
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
                                        if (useDegAcr == "0")
                                        {
                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                        }
                                        else
                                        {
                                            deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                        }
                                        degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                        stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                        acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                        try
                                        {
                                            acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                        }
                                        catch { }
                                        string Termdisp = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();

                                        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                        if (Termdisp.Trim() == "SHIFT I")
                                        {
                                            try
                                            {
                                                double cursemester = Convert.ToDouble(cursem);

                                                if (cursemester % 2 == 1)
                                                {
                                                    cursem = romanLetter(cursemester.ToString()) + " & " + romanLetter((cursemester + 1).ToString());
                                                }
                                                else
                                                {
                                                    cursem = romanLetter((cursemester - 1).ToString()) + " & " + romanLetter(cursemester.ToString());
                                                }
                                            }
                                            catch { }
                                            cursem = "Term : " + cursem;

                                        }
                                        else
                                        {
                                            cursem = "Term : " + romanLetter(cursem);
                                        }
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

                                                int y = 0;

                                                double ovrallcredit = 0;
                                                double grandtotal = 0.00;

                                                string text = "";


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
                                                                                    string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + "  and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                                    DataSet dsEachHdr = new DataSet();
                                                                                    dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                                    if (dsEachHdr.Tables.Count > 0)
                                                                                    {
                                                                                        if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                                        {
                                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
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

                                                    #region Bottom Portion of Challan

                                                    text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";
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

                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;



                                            string text = "";

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
                                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
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

                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


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
                                        double ovrallcredit = 0;
                                        double grandtotal = 0.00;

                                        string text = "";

                                        //First Ends

                                        y = -30;

                                        #endregion
                                        if (rbl_headerselect.SelectedIndex == 1)
                                        {
                                            int heght = 380;
                                            #region Middle Portion challan
                                            int chk = 0;
                                            int hdrsno = 0;
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
                                                        //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");
                                                        if (useIFSC == "0")
                                                            bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' ");
                                                        else
                                                            bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' ");

                                                        dispHdr += " (" + bnkAcc + ")";
                                                        grandtotal = grandtotal + totalAmt;

                                                        if (grandtotal > 0)
                                                        {

                                                            addpageOK = true;
                                                            createPDFOK = true;
                                                            if (totalAmt > 0)
                                                            {
                                                                hdrsno++;

                                                                y = y + 5;

                                                                #region Update Challan No
                                                                recptNo = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);
                                                                txt_rcptno.Text = recptNo;


                                                                if (hdrsno == 1)
                                                                {
                                                                    #region HTML Generation

                                                                    sbHtml.Append("<div style='padding-left:50px;height: 780px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 1056px; ' class='classRegular'>");

                                                                    sbHtml.Append("<tr><td  style='font-size:16px;text-align:center;font-weight:bold;'>BANK COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>COLLEGE COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>STUDENT COPY</td></tr>");

                                                                    sbHtml.Append("<tr class='classBold10'><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td></tr>");

                                                                    sbHtml.Append("<tr class='classBold10' style='text-align:center;font-size:12px;'><td ><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td><td></td><td><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table ></td><td></td><td><table class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td></tr>");

                                                                    sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                                                    #endregion

                                                                }

                                                                StringBuilder tempHtml = new StringBuilder();

                                                                //for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                //{
                                                                string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + HdrId + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                DataSet dsEachHdr = new DataSet();
                                                                dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                if (dsEachHdr.Tables.Count > 0)
                                                                {
                                                                    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        string selLedge = "	SELECT f.HeaderFK,LedgerFk, priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
                                                                        DataSet dsLedge = new DataSet();
                                                                        dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                        if (dsLedge.Tables.Count > 0)
                                                                        {
                                                                            if (dsLedge.Tables[0].Rows.Count > 0)
                                                                            {
                                                                                int ledsno = 0;
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
                                                                                        ledsno++;
                                                                                        y = y + 7;
                                                                                        string ledidd = Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]);
                                                                                        string legername = d2.GetFunction("select ledgername from FM_LedgerMaster where Ledgerpk=" + ledidd + "");


                                                                                        tempHtml.Append("<br><span class='classRegular' style='font-size:9px; width:320px;PADDING-LEFT:10PX;'>" + ledsno + "." + legername + "</span>");
                                                                                        heght -= 10;

                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                //}


                                                                #endregion



                                                                y = y + 15;

                                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td></tr>");
                                                                heght -= 13;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion
                                            #region Denomionation and Particulars

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td></tr>");
                                            sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td></tr>");
                                            //sbHtml.Append("<tr><td style='border:none;'>&nbsp;</td><tr>");
                                            if (useDenom == 1)
                                            {
                                                //College
                                                sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                                sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td></tr>");

                                            }
                                            if (useDenom == 2)
                                            {
                                                //Bank
                                                sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td></td></tr>");
                                            }
                                            if (useDenom == 3)
                                            {
                                                //Student
                                                sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                                sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                            }
                                            if (useDenom == 4)
                                            {
                                                //All

                                                sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                            }
                                            if (useDenom == 5)
                                            {
                                                //College and Bank
                                                sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td></td></tr>");

                                            }
                                            if (useDenom == 6)
                                            {
                                                //Student and Bank     

                                                sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");
                                            }
                                            if (useDenom == 7)
                                            {
                                                //College and Student

                                                sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                                sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                            }

                                            #endregion

                                            sbHtml.Append("</table></div>");
                                            if (grandtotal > 0)
                                            {
                                                contentDiv.Append(sbHtml.ToString());
                                            }
                                            sbHtml.Clear();
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
                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority ,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + hdrfk + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') and LedgerFk='" + HdrId + "'  order by case when priority is null then 1 else 0 end, priority ";
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
                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                            #endregion
                                        }
                                        //Bottom portion of the challan End
                                    }
                                    //Middle portion of challan End
                                    #endregion
                                }
                                #endregion
                            }
                            catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "New College Challan"); }

                            #endregion
                        }
                    }

                    if (createPDFOK && count > 0)
                    {
                        #region New Print
                        imgDIVVisible = true;
                        lbl_alert.Text = "Challan Generated";
                        contentVisible = true;
                        CreateReceiptOK = true;
                        return contentDiv.ToString();
                        #endregion
                    }
                    else
                    {
                        imgDIVVisible = true;
                        lbl_alert.Text = "Challan Already Taken";
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

                        feeCategory = Convert.ToString(ddlSEM);
                        string queryRollApp = "";
                        if (rbl_rollno.SelectedItem.Text.Trim() == "App No")
                        {
                            queryRollApp = "select '' Roll_No,app_formno,'' smart_serial_no,app_no, '' Reg_No  from applyn where app_formno='" + roll_admit + "'  and college_code='" + collegecode1 + "' ";
                        }
                        else
                        {
                            queryRollApp = "select r.Roll_No,a.app_formno,r.smart_serial_no,a.app_no,r.Reg_No  from Registration r,applyn a where r.App_No=a.app_no  and r.college_code='" + collegecode1 + "'  and r.Roll_Admit='" + roll_admit + "'";
                        }
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                        {
                            rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                            app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                            appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                            Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                            smartno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["smart_serial_no"]);
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
                        string bankAddress = "";
                        if (ddl_collegebank.Items.Count > 0)
                        {
                            bankName = ddl_collegebank.SelectedItem.Text.Split('-')[0];
                            bankPK = ddl_collegebank.SelectedItem.Value;
                            bankAddress = d2.GetFunction("select Street+', '+(select MasterValue from CO_MasterValues where MasterCode=District)+'-'+PinCode as address from FM_FinBankMaster where BankPK=" + bankPK + "");
                            bankAddress = "(" + bankAddress + ")";
                            bankCity = d2.GetFunction("select Upper(BankBranch) as city from FM_FinBankMaster where BankPK=" + bankPK + "") + " Branch";
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
                                if (useDegAcr == "0")
                                {
                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                }
                                else
                                {
                                    deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                }
                                degreeCode = Convert.ToString(ds.Tables[1].Rows[0]["Degree_code"]);
                                cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                stream = Convert.ToString(ds.Tables[1].Rows[0]["type"]);
                                //if (cursem == "1" || cursem == "3" || cursem == "5" || cursem == "7" || cursem == "9")
                                //{
                                //    acaYear = acaYear + "-" + (System.DateTime.Now.Year + 1).ToString();
                                //}
                                //else
                                //{
                                //    acaYear = (System.DateTime.Now.Year - 1).ToString() + "-" + acaYear;
                                //}

                                acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                try
                                {
                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                }
                                catch { }
                                //cursem = Convert.ToString(ddl_sem.SelectedItem.Text).Split(' ')[1] + " : " + romanLetter(Convert.ToString(ddl_sem.SelectedItem.Text).Split(' ')[0]);
                                //cursem = "Term : " + romanLetter(Convert.ToString(ddl_sem.SelectedItem.Text).Split(' ')[0]) + "   Academic Year : " + acaYear;

                                string Termdisp = d2.GetFunction("select UPPER(type) from course where college_code=" + collegecode1 + "").Trim();

                                string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                if (Termdisp.Trim() == "SHIFT I")
                                {
                                    string deptName = d2.GetFunction("select distinct dt.dept_name from degree d,course c,department dt,registration r where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code='" + collegecode1 + "' and app_no='" + appnoNew + "'");
                                    try
                                    {
                                        string txtval = d2.GetFunction(" select textval from textvaltable where textcriteria='FEECA' and college_code='" + collegecode1 + "' and textcode='" + feeCategory + "'");
                                        if (txtval != "0" && !string.IsNullOrEmpty(txtval))
                                            cursem = txtval.Split(' ')[0];
                                        double cursemester = Convert.ToDouble(cursem);

                                        if (cursemester % 2 == 1)
                                        {
                                            cursem = romanLetter(cursemester.ToString()) + " & " + romanLetter((cursemester + 1).ToString());
                                        }
                                        else
                                        {
                                            cursem = romanLetter((cursemester - 1).ToString()) + " & " + romanLetter(cursemester.ToString());
                                        }
                                    }
                                    catch { }
                                    cursem = "Term : " + cursem;
                                    if (deptName.ToUpper() == "COMPUTER SCIENCE" || deptName.ToLower() == "computer science" | deptName.ToLower() == "Computer Science")
                                    {
                                        string txtval = d2.GetFunction(" select textval from textvaltable where textcriteria='FEECA' and college_code='" + collegecode1 + "' and textcode='" + feeCategory + "'");
                                        if (txtval != "0" && !string.IsNullOrEmpty(txtval))
                                            cursem = "Term : " + romanLetter(txtval.Split(' ')[0]);
                                        else
                                            cursem = "Term : " + romanLetter(cursem);
                                    }

                                }
                                else
                                {
                                    string txtval = d2.GetFunction(" select textval from textvaltable where textcriteria='FEECA' and college_code='" + collegecode1 + "' and textcode='" + feeCategory + "'");
                                    if (txtval != "0" && !string.IsNullOrEmpty(txtval))
                                        cursem = "Term : " + romanLetter(txtval.Split(' ')[0]);
                                    else
                                        cursem = "Term : " + romanLetter(cursem);
                                }
                            }
                        }

                        bool createPDFOK = false;
                        #region PDF Generation


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



                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;



                                            string text = "";


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
                                                                double totalAmt = Convert.ToDouble(ds2.Tables[0].Rows[0]["Totamount"]);
                                                                if (ds2.Tables[1].Rows.Count > 0)
                                                                {
                                                                    totalAmt -= Convert.ToDouble(ds2.Tables[1].Rows[0]["TakenAmt"]);
                                                                }
                                                                //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                                bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
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


                                                                            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                                                            {
                                                                                string QEachHdr = "SELECT HeaderFK,isnull(SUM(BalAmount),0) as BalAmount,isnull(SUM(TotalAmount)-SUM(ChlTaken),0) ChallanAmt,FeeCategory	FROM FT_FeeAllot WHERE HeaderFK = " + Convert.ToString(ds1.Tables[0].Rows[j]["HeaderFK"]) + " and App_No=" + appnoNew + " and FeeCategory in ('" + feeCategory + "')GROUP BY HeaderFK,BalAmount,FeeCategory";

                                                                                DataSet dsEachHdr = new DataSet();
                                                                                dsEachHdr = d2.select_method_wo_parameter(QEachHdr, "Text");
                                                                                if (dsEachHdr.Tables.Count > 0)
                                                                                {
                                                                                    if (dsEachHdr.Tables[0].Rows.Count > 0)
                                                                                    {

                                                                                        string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and FeeCategory in ('" + feeCategory + "') and App_No=" + appnoNew + "  order by case when priority is null then 1 else 0 end, priority ";
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

                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";

                                                #endregion
                                            }
                                            //Bottom portion of the challan End
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region For Overall
                                //string QHdrForGroup = "select HeaderId from chlpagesettings where College_Code=" + collegecode1 + " and DegreeCode='" + degreeCode + "'";
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



                                            double ovrallcredit = 0;
                                            double grandtotal = 0.00;



                                            string text = "";

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
                                                        }

                                                        grandtotal = grandtotal + totalAmt;

                                                        // bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                        dispHdr += " (" + bnkAcc + ")";
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

                                                                    //recptNo = generateChallanNo();
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


                                                #region Bottom Portion of Challan

                                                text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


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

                            contentDiv.Clear();
                            StringBuilder sbHtml = new StringBuilder();
                            sbHtml.Clear();
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
                                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "')  order by case when priority is null then 1 else 0 end, priority ";
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


                                                        y = y + 15;

                                                    }
                                                }

                                            }
                                        }

                                        if (addpageOK)
                                        {
                                            string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                            d2.update_method_wo_parameter(updateRecpt, "Text");

                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


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


                                    y = -30;

                                    #endregion
                                    if (rbl_headerselect.SelectedIndex == 1)
                                    {
                                        #region HTML Generation

                                        sbHtml.Append("<div style='padding-left:50px;height: 780px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 1056px; ' class='classRegular'>");


                                        sbHtml.Append("<tr><td  style='font-size:16px;text-align:center;font-weight:bold;'>BANK COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>COLLEGE COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>STUDENT COPY</td></tr>");

                                        sbHtml.Append("<tr class='classBold10'><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td  style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td  style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td  style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td></tr>");

                                        sbHtml.Append("<tr class='classBold10' style='text-align:center; font-size:12px;'><td ><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td><td></td><td><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table ></td><td></td><td><table class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td></tr>");

                                        sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                        #endregion


                                        //Header
                                        #region Middle Portion challan
                                        int chk = 0;
                                        int hdrsno = 0;
                                        int heght = 380;


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
                                                    //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + Convert.ToString(ds2.Tables[0].Rows[0]["ChlGroupHeader"]) + "' AND BankPK = '" + bankPK + "'");

                                                    if (useIFSC == "0")
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                                    else
                                                        bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");

                                                    dispHdr += " (" + bnkAcc + ")";
                                                    grandtotal = grandtotal + totalAmt;

                                                    if (totalAmt > 0)
                                                    {
                                                        hdrsno++;

                                                        y = y + 5;


                                                        StringBuilder tempHtml = new StringBuilder();

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
                                                                    string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk   and  f.HeaderFK = " + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') order by case when priority is null then 1 else 0 end, priority ";
                                                                    DataSet dsLedge = new DataSet();
                                                                    dsLedge = d2.select_method_wo_parameter(selLedge, "Text");
                                                                    if (dsLedge.Tables.Count > 0)
                                                                    {
                                                                        if (dsLedge.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            int ledsno = 0;
                                                                            for (int hdri = 0; hdri < dsLedge.Tables[0].Rows.Count; hdri++)
                                                                            {
                                                                                double remainAmt = 0;
                                                                                remainAmt = Convert.ToDouble(Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]));
                                                                                if (remainAmt > 0)
                                                                                {
                                                                                    ledsno++;
                                                                                    string insertChlNo = "INSERT INTO FT_ChallanDet(ChallanNo,ChallanDate,App_No,HeaderFK,FeeAmount,TakenAmt,FeeCategory,FinYearFK,BankFk,LedgerFK,challanType) VALUES('" + recptNo + "','" + recptDt + "'," + appnoNew + "," + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["BalAmount"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["ChallanAmt"]) + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "," + finYeaid + "," + bankPK + "," + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "," + challanType + ")";
                                                                                    d2.select_method_wo_parameter(insertChlNo, "Text");
                                                                                    string updateCHlTkn = " update FT_FeeAllot set ChlTaken = +" + remainAmt + "  where FeeCategory ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["FeeCategory"]) + "' and HeaderFK ='" + Convert.ToString(dsEachHdr.Tables[0].Rows[0]["HeaderFK"]) + "' and LedgerFK ='" + Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]) + "' and App_No='" + appnoNew + "' ";
                                                                                    d2.update_method_wo_parameter(updateCHlTkn, "Text");
                                                                                    string ledidd = Convert.ToString(dsLedge.Tables[0].Rows[hdri]["LedgerFk"]);
                                                                                    string ledname = d2.GetFunction("select ledgername from FM_LedgerMaster where Ledgerpk=" + ledidd + "");

                                                                                    y = y + 7;

                                                                                    tempHtml.Append("<br><span class='classRegular' style='font-size:9px; width:320px;PADDING-LEFT:10PX;'>" + ledsno + "." + ledname + "</span>");
                                                                                    heght -= 10;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            //}

                                                            #endregion
                                                        }

                                                        sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td></tr>");
                                                        heght -= 13;


                                                        y = y + 15;

                                                    }

                                                }
                                            }
                                        }

                                        #region Denomionation and Particulars

                                        sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td></tr>");

                                        sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td></tr>");
                                        sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td></tr>");
                                        //sbHtml.Append("<tr><td style='border:none;'>&nbsp;</td><tr>");
                                        if (useDenom == 1)
                                        {
                                            //College
                                            sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td></tr>");
                                        }
                                        if (useDenom == 2)
                                        {
                                            //Bank
                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td></td></tr>");
                                        }
                                        if (useDenom == 3)
                                        {
                                            //Student
                                            sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                        }
                                        if (useDenom == 4)
                                        {
                                            //All

                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                        }
                                        if (useDenom == 5)
                                        {
                                            //College and Bank
                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td></td></tr>");

                                        }
                                        if (useDenom == 6)
                                        {
                                            //Student and Bank     

                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                        }
                                        if (useDenom == 7)
                                        {
                                            //College and Student
                                            sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                        }

                                        #endregion
                                        sbHtml.Append("</table></div>");
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
                                                            string selLedge = "	SELECT f.HeaderFK,LedgerFk,priority,isnull(BalAmount,0) as BalAmount,isnull(TotalAmount,0)-isnull(ChlTaken,0) ChallanAmt,FeeCategory	FROM FT_FeeAllot  f,FM_LedgerMaster l WHERE  l.Ledgerpk=f.ledgerfk   and l.headerfk=f.headerfk  and   f.HeaderFK = " + hdrfk + " and App_No=" + appnoNew + "  and FeeCategory in ('" + feeCategory + "') and LedgerFk='" + HdrId + "' order by case when priority is null then 1 else 0 end, priority ";
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

                                        #region Bottom Portion of Challan

                                        text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


                                        #endregion
                                    }
                                    //Bottom portion of the challan End
                                }
                                contentDiv.Append(sbHtml.ToString());

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


                                        string text = "";


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
                                                        //recptNo = generateChallanNo();
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


                                                y = y + 15;
                                            }
                                        }

                                        if (addpageOK)
                                        {
                                            string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                            d2.update_method_wo_parameter(updateRecpt, "Text");

                                            #region Bottom Portion of Challan

                                            text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


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


                                    y = 0;
                                    y = -30;

                                    #endregion
                                    if (rbl_headerselect.SelectedIndex == 1)
                                    {
                                        #region HTML Generation

                                        sbHtml.Append("<div style='padding-left:50px;height: 780px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 1056px; ' class='classRegular'>");

                                        sbHtml.Append("<tr><td  style='font-size:16px;text-align:center;font-weight:bold;'>BANK COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>COLLEGE COPY</td><td></td><td  style='font-size:16px;text-align:center;font-weight:bold;'>STUDENT COPY</td></tr>");

                                        sbHtml.Append("<tr class='classBold10'><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><table  Rules='None' class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 2px solid black;'><tr><td  style='font-size:16px;'><center><b>" + bankName.ToUpper() + "</b></center></td></tr><tr><td><center>" + bankAddress + "</center></td></tr><tr><td><center>" + counterName + "</center></td></tr><tr><td><center>" + bankCity + "</center></td></tr><tr><td style='font-size:15px;'><center><b>" + colName + "</b></center></td></tr><tr><td><center>" + hstlfees + " FEES CHALLAN " + shift + "</center></td></tr></table></td></tr>");

                                        sbHtml.Append("<tr class='classBold10' style='text-align:center;font-size:12px;'><td ><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td><td></td><td><table  class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table ></td><td></td><td><table class='classBold10' cellpadding='0' cellspacing='0' style='width:380px; border: 1px solid black;'><tr><td style='width:120px; border: 1px solid black;'><b>CH NO.:" + recptNo + "</b></td><td style='width:180px; border: 1px solid black;'>Receipt No:<br/>(Office Use Only)</td><td style='width:80px; border: 1px solid black;text-align:right;'>Date:" + txt_date.Text + "&nbsp;&nbsp;</td><tr></table></td></tr>");

                                        sbHtml.Append("<tr class='classBold10'><td ><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr  style='border: 1px solid black;'><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td><td></td><td><table  Rules='Rows' class='classBold10' cellpadding='1' cellspacing='0' style='width:380px;border: 1px solid black;'><tr><td colspan='2'  style='border: 1px solid black;'><b>Student\'s Name:" + studname + "</b></td></tr><tr><td  colspan='2'  style='border: 1px solid black;'><b>" + rolldisplay + rollvalue + "</b><span style='padding-left:60px; width:200px; '>Class :  " + deg + "</span></td></tr><tr><tr><td  style='width:80px;border: 1px solid black;'>" + cursem + "</td><td style='border: 1px solid black;'>Academic Year :" + acaYear + "</td></tr><tr><td  style='border: 1px solid black;width:220px;'>" + parName + "</td><td  style='border: 1px solid black;text-align:right;'>Amount Rs.</td></tr></table></td></tr>");
                                        #endregion

                                        //Header
                                        #region Middle Portion challan
                                        int chk = 0;
                                        int hdrsno = 0;
                                        int heght = 380;


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

                                            List<string> LedgerNames = new List<string>();

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
                                                try
                                                {
                                                    if (lblFeeCategory.Text.Contains("$"))
                                                    {
                                                        //string[] splVal = lblFeeCategory.Text.Split('$');
                                                        lblFeeCategory.Text = lblFeeCategory.Text.Split('$')[0];
                                                    }
                                                    
                                                }
                                                catch { }
                                                Label lblFeeType = (Label)row.FindControl("lbl_feetype");
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

                                                                LedgerNames.Add(lblFeeType.Text);
                                                            }
                                                        }
                                                    }
                                                }
                                            }


                                            grandtotal = grandtotal + totalAmt;
                                            //bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "' AND BankPK = '" + bankPK + "'");
                                            if (useIFSC == "0")
                                                bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");
                                            else
                                                bnkAcc = "A/c No " + d2.GetFunction("SELECT AccNo+'-IFSC '+IFSCCode FROM FS_ChlGroupHeaderSettings S,FM_FinBankMaster B WHERE S.BankFK = B.BankPK AND ChlGroupHeader = '" + dispHdr + "'");


                                            dispHdr += " (" + bnkAcc + ")";

                                            if (grandtotal > 0)
                                            {

                                                addpageOK = true;
                                                createPDFOK = true;
                                            }
                                            if (totalAmt > 0)
                                            {
                                                hdrsno++;

                                                y = y + 5;


                                                StringBuilder tempHtml = new StringBuilder();

                                                int ledsno = 0;
                                                for (int ldr = 0; ldr < LedgerNames.Count; ldr++)
                                                {
                                                    ledsno++;
                                                    y = y + 7;

                                                    tempHtml.Append("<br><span class='classRegular' style='font-size:9px; width:320px;PADDING-LEFT:10PX;'>" + ledsno + "." + Convert.ToString(LedgerNames[ldr]) + "</span>");
                                                    heght -= 10;
                                                }

                                                sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>" + hdrsno + "." + dispHdr + tempHtml.ToString() + "</td><td style='text-align:right;width:60px;'>" + returnIntegerPart(totalAmt) + "&nbsp;</td></td><td style='width:20px;'>&nbsp;" + returnDecimalPart(totalAmt) + "</tr></table></td></tr>");
                                                heght -= 13;


                                                y = y + 15;
                                            }


                                        }

                                        #region Denomionation and Particulars

                                        sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'   BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  BORDER=1><tr><td style='width:300px;'>Total</td><td style='text-align:right;width:60px;'>" + Math.Round((decimal)Convert.ToDouble(grandtotal), 2) + "&nbsp;</td><td style='width:20px;'>&nbsp;00" + "</td></tr><tr><td colspan='3'>" + "(" + DecimalToWords((decimal)Convert.ToDouble(grandtotal)) + " Rupees Only)" + "</td></tr></table></td></tr>");

                                        sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td>Signature of Remitter<br><br>Mobile No :</td><td></td></tr></table></td></tr>");
                                        sbHtml.Append("<tr><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0'  RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td><td></td><td><table style='width:380px;'  cellpadding='0' cellspacing='0' ><tr><td style='width:380px;border:solid 1px #000000;height:" + (heght - 170) + "px;'></td></tr></table><table class='classBold' style='border: 1px solid black;width:380px;font-size:12px;' cellpadding='0' cellspacing='0' RULES='ROWS'><tr><td><br/>Signature of Cashier</td><td style='text-align:right;'><br/>Manager / Acct.</td></tr></table></td></tr>");
                                        //sbHtml.Append("<tr><td style='border:none;'>&nbsp;</td><tr>");
                                        if (useDenom == 1)
                                        {
                                            //College
                                            sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td></tr>");
                                        }
                                        if (useDenom == 2)
                                        {
                                            //Bank
                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td></td></tr>");
                                        }
                                        if (useDenom == 3)
                                        {
                                            //Student
                                            sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");

                                        }
                                        if (useDenom == 4)
                                        {
                                            //All

                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                        }
                                        if (useDenom == 5)
                                        {
                                            //College and Bank
                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td><td></td><td></td></tr>");

                                        }
                                        if (useDenom == 6)
                                        {
                                            //Student and Bank     

                                            sbHtml.Append("<tr><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td></tr>");
                                        }
                                        if (useDenom == 7)
                                        {
                                            //College and Student

                                            sbHtml.Append("<tr><td></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td><td></td><td><table  class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td colspan='5'><center>PARTICULARS OF DEMAND DRAFT AND DENOMINATION</center></td></tr><tr><td>Name of Bank</td><td>Place of Bank</td><td>Draft Number</td><td>Date</td><td>Amount</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr>");

                                            sbHtml.Append("<tr><td></td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table> </td><td></td><td><table class='classBold' style='border: 1px solid black;width:380px;' cellpadding='0' cellspacing='0' border='1' ><tr><td style='width:80px;'>1000 X</td><td  style='width:80px;'>&nbsp</td><td  style='width:220px;font-size:12px;text-align:center;' rowspan='9'>Bank Seal</td></tr><tr><td>500 X</td><td>&nbsp</td></tr><tr><td>100 X</td><td>&nbsp</td></tr><tr><td>50 X</td><td>&nbsp</td></tr><tr><td>20 X</td><td>&nbsp</td></tr><tr><td>10 X</td><td>&nbsp</td></tr><tr><td>5 X</td><td>&nbsp</td></tr><tr><td>Coins X</td><td>&nbsp</td></tr><tr><td>Total</td><td></td></tr></table></td></tr>");
                                        }

                                        #endregion

                                        sbHtml.Append("</table></div>");
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
                                                try
                                                {
                                                    if (lblFeeCategory.Text.Contains("$"))
                                                    {
                                                        //string[] splVal = lblFeeCategory.Text.Split('$');
                                                        lblFeeCategory.Text = lblFeeCategory.Text.Split('$')[0];
                                                    }

                                                }
                                                catch { }
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
                                                    //recptNo = generateChallanNo();
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

                                                y = y + 15;
                                            }


                                        }
                                        #endregion
                                    }
                                    if (addpageOK)
                                    {
                                        string updateRecpt = " update FM_FinCodeSettings set ChallanStNo=" + lastRecptNo + "+1 where CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYeaid + " and CollegeCode=" + collegecode1 + ")";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");

                                        #region Bottom Portion of Challan

                                        string text = "(" + DecimalToWords((decimal)grandtotal) + " Rupees Only)";


                                        #endregion
                                    }
                                }
                                //Middle portion of challan End

                                //Bottom portion of the challan

                                //Bottom portion of the challan End

                                contentDiv.Append(sbHtml.ToString());
                                #endregion
                            }
                        }

                        //New COde END

                        if (createPDFOK)
                        {
                            #region New Print
                            imgDIVVisible = true;
                            lbl_alert.Text = "Challan Generated";
                            contentVisible = true;
                            CreateReceiptOK = true;
                            return contentDiv.ToString();
                            #endregion
                        }
                        else
                        {
                            imgDIVVisible = true;
                            //this.Form.DefaultButton = "btn_alertclose";
                            lbl_alert.Text = "Challan Already Taken";
                        }
                        #endregion


                        //imgAlert.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        d2.sendErrorMail(ex, collegecode1, "ChallanReceipt");
                        imgDIVVisible = true;
                        //this.Form.DefaultButton = "btn_alertclose";
                        lbl_alert.Text = "Inadequate Details";
                    }

                    #endregion
                }
            }
            else
            {
                imgDIVVisible = true;
                lbl_alert.Text = "Challan Number Not Generated";
            }

            txt_rcptno.Text = generateChallanNo(usercode, collegecode1, ref accidRecpt, ref lastRecptNo);

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "New COllege Challan");
            imgDIVVisible = true;
            lbl_alert.Text = "Inadequate Details";
        }
        return string.Empty;
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
    private string getAppNoFromApplyn(string app_formno, string collegecode)
    {
        string appno = "0";
        appno = d2.GetFunction("select app_no from applyn where app_formno='" + app_formno + "'  and college_code='" + collegecode + "' ").Trim();
        return appno;
    }
}