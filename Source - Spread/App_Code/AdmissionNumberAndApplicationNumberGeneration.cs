using InsproDataAccess;
using System.Data;
using System;
using System.Text;
using System.Collections.Generic;
public class AdmissionNumberAndApplicationNumberGeneration
{
    private InsproDirectAccess d2;
    private InsproStoreAccess sp;
    DAccess2 da = new DAccess2();
    /// <summary>
    /// NumberGenerationType 0 then Admission Number
    /// NumberGenerationType 1 then Application Number
    /// Admission Number then param AppNo
    /// Application Number then param Semester,degreecode,collegecode,batchyear,seattype
    /// </summary>
    /// <param name="NumberGenerationType"> </param>
    /// <param name="Semester"></param>
    /// <param name="DegreeCode"></param>
    /// <param name="CollegeCode"></param>
    /// <param name="BatchYear"></param>
    /// <param name="SeatType"></param>
    /// <param name="appno"></param>
    /// <returns></returns>
    public string AdmissionNoAndApplicationNumberGeneration(int NumberGenerationType = 0, string Semester = "", string DegreeCode = "", string CollegeCode = "", string BatchYear = "", string SeatType = "", string appno = null, string Mode = null)
    {
        try
        {
            d2 = new InsproDirectAccess();
            sp = new InsproStoreAccess();
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet autoGenDS = new DataSet();
            string admissionNo = string.Empty;
            string GenerationNo = string.Empty;
            string GenerationNum = string.Empty;
            string query = string.Empty;
            string RunningSeries = string.Empty;
            /*if (NumberGenerationType == 0)
            {
                DataTable GetStudCollegeCode = d2.selectDataTable("select college_code from applyn where app_no='" + appno + "'");
                if (GetStudCollegeCode.Rows.Count > 0)
                    CollegeCode = Convert.ToString(GetStudCollegeCode.Rows[0]["college_code"]);
                query = "select d.Acronym as degreeAcr,c.Coll_acronymn,a.current_semester,rtrim(ltrim(regcode))DegreeRegcode,a.degree_code,a.batch_year,c.acr,a.college_code,t.textval as SeatType,priority2 as SeatTypeNo from degree d,collinfo c,applyn a left join  textvaltable t on a.college_code=t.college_code and TextCriteria='seat ' and a.seattype=t.TextCode   where a.degree_code=d.Degree_Code and a.college_code=d.college_code and c.college_code=a.college_code and a.app_no='" + appno + "'";//a.degree_code
            }
            if (NumberGenerationType == 1)
            {
                query = "select d.Acronym as degreeAcr,c.Coll_acronymn,'" + Semester + "' current_semester,rtrim(ltrim(regcode))DegreeRegcode,'" + BatchYear + "' batch_year,c.acr'" + CollegeCode + "' college_code,t.textval as SeatType,priority2 as SeatTypeNo,d.degree_code from degree d,collinfo c left join textvaltable t on c.college_code=t.college_code and TextCriteria='seat ' where d.college_code=c.college_code and d.Degree_Code='" + DegreeCode + "' and c.college_code='" + CollegeCode + "' and t.TextCode='" + SeatType + "'";//'" + DegreeCode + "'degree_code
            }
            query += "  select NumberLength,DifferentRange,HeaderCode,MasterValue,PrefixOrSufix,StartNo,t.MasterCriteria1,NumberType,g.collegeCode,MasterCriteria1 from AdmissionNoGeneration G,CO_MasterValues t where g.HeaderCode=t.MasterCode and g.collegecode='" + CollegeCode + "' and NumberType='" + NumberGenerationType + "'  order by frange,trange";
            ds = d2.selectDataSet(query);*/
            Dictionary<string, string> ParametersDic = new Dictionary<string, string>();
            ParametersDic.Add("@NumberType", Convert.ToString(NumberGenerationType));
            ParametersDic.Add("@AppNo", appno);
            ParametersDic.Add("@BatchYear", BatchYear);
            ParametersDic.Add("@Semester", Semester);
            ParametersDic.Add("@DegreeCode", DegreeCode);
            ParametersDic.Add("@SeattypeNo", SeatType);
            ParametersDic.Add("@CollegeCode", CollegeCode);
            ds = sp.selectDataSet("AdmissionNumberGeneration", ParametersDic);
            if (ds.Tables[1].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string CodeValue = string.Empty;
                string Code = string.Empty;
                int StartNo = 0;
                if (NumberGenerationType == 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //==========Rajkumar 7/3/2018
                        if (string.IsNullOrEmpty(DegreeCode))
                            DegreeCode = Convert.ToString(dr["degree_code"]);
                        if (string.IsNullOrEmpty(CollegeCode))
                            CollegeCode = Convert.ToString(dr["college_code"]);
                        if (string.IsNullOrEmpty(SeatType))
                            SeatType = Convert.ToString(dr["SeatTypeNo"]);
                        if (string.IsNullOrEmpty(Semester))
                        Semester = Convert.ToString(dr["current_semester"]);
                        if (string.IsNullOrEmpty(BatchYear))
                        BatchYear = Convert.ToString(dr["batch_year"]);
                        //Lateral Entry or Transfer
                        if (!string.IsNullOrEmpty(Mode))
                            if (Mode == "2" || Mode == "3")
                            {
                                int Lateralyear = 0;
                                int.TryParse(BatchYear, out Lateralyear);
                                BatchYear = Convert.ToString(Lateralyear + 1);
                            }
                    }
                }
              
                ParametersDic.Clear();
                ParametersDic.Add("@NumberType", Convert.ToString(NumberGenerationType));
                ParametersDic.Add("@BatchYear", BatchYear);
                ParametersDic.Add("@Semester", Semester);
                ParametersDic.Add("@DegreeCode", DegreeCode);
                ParametersDic.Add("@SeattypeNo", SeatType);
                ParametersDic.Add("@CollegeCode", CollegeCode);
                autoGenDS = sp.selectDataSet("AdmissionNumberGenerationSettings", ParametersDic);
                if (autoGenDS.Tables[0].Rows.Count == 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        int MasterCriteriaValue = 0;
                        int PrefixSuffix = 0;
                        int rangeDiff = 0;
                        int Length = 0;
                        string Value = string.Empty;
                        CodeValue = string.Empty;
                        int.TryParse(Convert.ToString(dr["MasterCriteria1"]), out MasterCriteriaValue);
                        int.TryParse(Convert.ToString(dr["PrefixOrSufix"]), out PrefixSuffix);
                        int.TryParse(Convert.ToString(dr["DifferentRange"]), out rangeDiff);
                        #region Code Generation
                        switch (MasterCriteriaValue)
                        {
                            case 1:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["Coll_acronymn"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 2:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["acr"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 3:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["SeatTypeNo"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                    CodeValue = Convert.ToString(ds.Tables[0].Rows[0]["SeatTypeNo"]);
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 4:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["degreeAcr"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 5:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["DegreeRegcode"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 6:
                                string year = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);
                                CodeValue = returnYearforSem(year);
                                break;
                            case 7:
                                Value = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                                Length = 0;
                                int.TryParse(Convert.ToString(Value.Length), out Length);
                                if (Length >= rangeDiff)
                                {
                                    if (PrefixSuffix == 1)
                                        CodeValue = Convert.ToString(Value.Substring(0, rangeDiff));
                                    else if (PrefixSuffix == 2)
                                        CodeValue = Convert.ToString(Value.Substring(Length - rangeDiff, rangeDiff));
                                    if (!string.IsNullOrEmpty(Mode))
                                    {
                                        if (Mode == "2" || Mode == "3")
                                        {
                                            int Lateralyear = 0;
                                            int.TryParse(CodeValue, out Lateralyear);
                                            CodeValue = Convert.ToString(Lateralyear + 1);
                                        }
                                    }
                                }
                                else
                                    sb.Append("Cannot form the digit The range is greater");
                                break;
                            case 8:
                                CodeValue = Convert.ToString(ds.Tables[0].Rows[0]["current_semester"]);
                                break;
                            case 9:
                                Code = admissionNo;
                                int.TryParse(Convert.ToString(dr["StartNo"]), out StartNo);
                                //RunningSeries = StartNo.ToString().PadLeft(rangeDiff+1, '0');
                                RunningSeries = generateApplicationNumber(StartNo, rangeDiff);
                                //RunningSeries = StartNo; //+ 1;
                                CodeValue = RunningSeries;
                                break;
                        }
                        #endregion
                        admissionNo += CodeValue;
                        GenerationNo += Code;
                    }
                }
                else
                {
                    RunningSeries = Convert.ToString(autoGenDS.Tables[0].Rows[0]["RunningSeries"]);
                    GenerationNum = Convert.ToString(autoGenDS.Tables[0].Rows[0]["GenerationNumber"]);
                    int Runlen = 0;
                    int RunningSerial = 0;
                    int.TryParse(Convert.ToString(autoGenDS.Tables[0].Rows[0]["RunningSeriesLength"]), out Runlen);
                    int.TryParse(RunningSeries, out RunningSerial);
                    RunningSeries = generateApplicationNumber(RunningSerial, Runlen);
                    admissionNo = Convert.ToString(GenerationNum + RunningSeries);
                }
             
                ParametersDic.Clear();
                ParametersDic.Add("@NumberType", Convert.ToString(NumberGenerationType));
                ParametersDic.Add("@BatchYear", BatchYear);
                ParametersDic.Add("@Semester", Semester);
                ParametersDic.Add("@DegreeCode", DegreeCode);
                ParametersDic.Add("@SeattypeNo", SeatType);
                ParametersDic.Add("@CollegeCode", CollegeCode);
                ParametersDic.Add("@RunningSeries", RunningSeries);
                ParametersDic.Add("@StartNo", Convert.ToString(StartNo));
                ParametersDic.Add("@GenerateNo", GenerationNo);
                sp.insertData("AdmissionNumberGenerationUpdate", ParametersDic);
            }
            return admissionNo;
        }
        catch (Exception e)
        {
            da.sendErrorMail(e, Convert.ToString(CollegeCode), "AdmissionNumber Genderation");
            return " ";
        }
    }
    public string generateApplicationNumber(int serialStartNo, int size)
    {
        string appNoString = serialStartNo.ToString();
        if (size != appNoString.Length && size > appNoString.Length)
        {
            while (size != appNoString.Length)
                appNoString = "0" + appNoString;
        }
        return appNoString;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
}
