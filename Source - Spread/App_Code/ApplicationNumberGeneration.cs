using InsproDataAccess;
using System.Data;
using System;
/// <summary>
/// To generate Application Number
/// </summary>
public class ApplicationNumberGeneration 
{
    private InsproDirectAccess dirAccess;

	public ApplicationNumberGeneration()
	{
        dirAccess = new InsproDirectAccess();
	}

    //Generate Region

    /// <summary>
    /// Generate Application number for given size without acronym
    /// </summary>
    /// <param name="serialStartNo">Send the number to start</param>
    /// <param name="size">sened the size needed</param>
    /// <returns>Return application number generated without acronym or empty when not generated </returns>
    private string generateApplicationNumber(int serialStartNo, int size)
    {
        string appNoString = serialStartNo.ToString();

        if (size != appNoString.Length && size > appNoString.Length)
        {
            while (size != appNoString.Length)
            {
                appNoString = "0" + appNoString;
            }
        }

        return appNoString;
    }
    /// <summary>
    /// Generate application number for query sent
    /// </summary>
    /// <param name="Query">Send the Query to get details</param>
    /// <returns>Return application number generated or empty when not generated</returns>
    private string generateApplicationNumber(string Query)
    {
        string app_no = string.Empty;
        try
        {
            DataTable dtAppNoDet = dirAccess.selectDataTable(Query);
            if (dtAppNoDet.Rows.Count > 0)
            {
                string acronym = Convert.ToString(dtAppNoDet.Rows[0]["appcode"]).Trim().ToUpper();
                int startNo = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["app_startwith"]), out startNo);
                int size = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["app_serial"]), out size);
                int lastNo = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["cg_lastno"]), out lastNo);

                int serialStartNo = startNo > lastNo ? startNo : lastNo;

                if (size > 0 && serialStartNo > 0)
                {
                    app_no = acronym + generateApplicationNumber(serialStartNo, size);
                }
            }
        }
        catch { app_no = string.Empty; }
        return app_no;
    }
    /// <summary>
    /// For college and batchwise application number generation
    /// </summary>
    /// <param name="collegeCode">College code of the application to generate</param>
    /// <param name="batchYear">Batch year of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return application number generated or empty when not generated</returns>
    public string getApplicationNumber(string collegeCode, string batchYear, int AppOrAdmNo)
    {
        string app_no = string.Empty;

        app_no = generateApplicationNumber("select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=1 and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'");

        return app_no.ToUpper().Trim();
    }
    /// <summary>
    /// For Education level wise application number generation
    /// </summary>
    /// <param name="collegeCode">Pass Empty</param>
    /// <param name="eduLevel">Education Level of the application to generate</param>
    /// <param name="batchYear">Batch year of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return application number generated or empty when not generated</returns>
    public string getApplicationNumber(string collegeCode, string eduLevel, string batchYear, int AppOrAdmNo)
    {
        string app_no = string.Empty;

        app_no = generateApplicationNumber("select appcode,app_startwith,app_serial,cg_lastno from code_generation where Edulevel='" + eduLevel + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=2 and ISNULL(app_Code_flag,'" + AppOrAdmNo + "')=0");

        return app_no.ToUpper().Trim();
    }
    /// <summary>
    /// For Degreewsie application number generation
    /// </summary>
    /// <param name="collegeCode">College code of the application to generate</param>
    /// <param name="batchYear">Batch year of the application</param>
    /// <param name="degreeCode">Degree code of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return application number generated or empty when not generated</returns>
    public string getApplicationNumber(string collegeCode, string batchYear, int degreeCode, int AppOrAdmNo)
    {
        string app_no = string.Empty;

            app_no = generateApplicationNumber("select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "'  and isnull(cg_generationType,0)=0 and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'");

        return app_no.ToUpper().Trim();
    }
    /// <summary>
    /// For Seatwise, Modewise and Degreewsie application number generation
    /// </summary>
    /// <param name="collegeCode">College code of the application to generate</param>
    /// <param name="batchYear">Batch year of the application</param>
    /// <param name="degreeCode">Degree code of the application</param>
    /// /// <param name="mode">Mode : 1 - Regular, 2 - Lateral and  3 - Transfer student </param>
    /// <param name="seatType">Seat type of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return application number generated or empty when not generated</returns>
    public string getApplicationNumber(string collegeCode, string batchYear, string degreeCode,string mode,string seatType,int AppOrAdmNo)
    {
        string app_no = string.Empty;
        //krishhna kumar.r
        app_no = generateApplicationNumber("select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "' and isnull(cg_generationType,0)=3  and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "' and isnull(cb_mode,0)='" + mode + "'");//and isnull(cb_mode,0)='" + mode + "'

        return app_no.ToUpper().Trim();
    }

    //Update Region

    /// <summary>
    /// Update the application last number
    /// </summary>
    /// <param name="Query"></param>
    /// <param name="updatePartQ"></param>
    /// <returns></returns>
    private bool updateData(string Query, string updatePartQ)
    {
        int updated = 0;
        try
        {
            DataTable dtAppNoDet = dirAccess.selectDataTable(Query);
            if (dtAppNoDet.Rows.Count > 0)
            {
                string acronym = Convert.ToString(dtAppNoDet.Rows[0]["appcode"]).Trim().ToUpper();
                int startNo = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["app_startwith"]), out startNo);
                int size = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["app_serial"]), out size);
                int lastNo = 0;
                int.TryParse(Convert.ToString(dtAppNoDet.Rows[0]["cg_lastno"]), out lastNo);

                int updLastNo = startNo > lastNo ? startNo : lastNo; 
                updLastNo++;

                updated = dirAccess.updateData("Update code_generation set cg_lastno='" + updLastNo + "' "+updatePartQ);
            }
        }
        catch { updated = 0; }
        if (updated > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// For college and batchwise application number generation
    /// </summary>
    /// <param name="collegeCode">College code of the application to generate</param>
    /// <param name="batchYear">Batch year of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return true on updated or false when not updated</returns>
    public bool updateApplicationNumber(string collegeCode, string batchYear, int AppOrAdmNo)
    {
        bool isUpdated = false;
        try
        {
            string selectQ = "select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=1 and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'";
            string updatePartQ = " where college_code='" + collegeCode + "'  and isnull(cg_generationType,0)=1   and batch_year='" + batchYear + "' and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'";
            isUpdated = updateData(selectQ, updatePartQ);
        }
        catch { isUpdated = false; }
        return isUpdated;
    }
    /// <summary>
    /// For Education level wise application number updation
    /// </summary>
    /// <param name="collegeCode">Pass Empty</param>
    /// <param name="eduLevel">Education Level of the application to update</param>
    /// <param name="batchYear">Batch year of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return true on updated or false when not updated</returns>
    public bool updateApplicationNumber(string collegeCode, string eduLevel, string batchYear, int AppOrAdmNo)
    {
        bool isUpdated = false;
        try
        {
            //string selectQ = "select appcode,app_startwith,app_serial,cg_lastno from code_generation where Edulevel='" + eduLevel + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=2 and ISNULL(app_Code_flag,'0')='2'";
            string selectQ = "select appcode,app_startwith,app_serial,cg_lastno from code_generation where Edulevel='" + eduLevel + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=2 and ISNULL(app_Code_flag,'0')='0'";
            string updatePartQ = " where Edulevel='" + eduLevel + "' and batch_year='" + batchYear + "' and isnull(cg_generationType,0)=2   and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'";
            isUpdated = updateData(selectQ, updatePartQ);
        }
        catch { isUpdated = false; }
        return isUpdated;
    }
    /// <summary>
    /// For Degreewsie application number updation
    /// </summary>
    /// <param name="collegeCode">College code of the application to update</param>
    /// <param name="batchYear">Batch year of the application</param>
    /// <param name="degreeCode">Degree code of the application</param>
    ///  <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return true on updated or false when not updated</returns>
    public bool updateApplicationNumber(string collegeCode, string batchYear, int degreeCode, int AppOrAdmNo)
    {
        bool isUpdated = false;
        try
        {
            string selectQ = "select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "'  and isnull(cg_generationType,0)=0 and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'";
            string updatePartQ = " where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "'  and isnull(cg_generationType,0)=0    and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "'";
            isUpdated = updateData(selectQ, updatePartQ);
        }
        catch { isUpdated = false; }
        return isUpdated;
    }
    /// <summary>
    /// For Seatwise, Modewise and Degreewsie application number updation
    /// </summary>
    /// <param name="collegeCode">College code of the application to update</param>
    /// <param name="batchYear">Batch year of the application</param>
    /// <param name="degreeCode">Degree code of the application</param>
    /// /// <param name="mode">Mode : 1 - Regular, 2 - Lateral and  3 - Transfer student </param>
    /// <param name="seatType">Seat type of the application</param>
    /// <param name="AppOrAdmNo">Send Type : 0-Application No , 1 - Admission No</param>
    /// <returns>Return true on updated or false when not updated</returns>
    public bool updateApplicationNumber(string collegeCode, string batchYear, string degreeCode, string mode, string seatType, int AppOrAdmNo)
    {
        bool isUpdated = false;
        try
        {
            string selectQ = "select appcode,app_startwith,app_serial,cg_lastno from code_generation where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "'  and isnull(cg_generationType,0)=3 and ISNULL(app_Code_flag,'0')='1'  and isnull(cb_mode,0)='" + mode + "'";//and isnull(cb_mode,0)='" + mode + "'  and cg_seattype='" + seatType + "'
            string updatePartQ = " where college_code='" + collegeCode + "' and batch_year='" + batchYear + "' and degree_code='" + degreeCode + "' and isnull(cg_generationType,0)=3  and ISNULL(app_Code_flag,'0')='" + AppOrAdmNo + "' and isnull(cb_mode,0)='" + mode + "' and cg_seattype='" + seatType + "'";
            isUpdated = updateData(selectQ, updatePartQ);
        }
        catch { isUpdated = false; }
        return isUpdated;
    }
}