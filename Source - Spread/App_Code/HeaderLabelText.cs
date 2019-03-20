using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Collections.Generic;

public class Institution
{
    private string insName;
    private string insDegree;
    private string insStream;
    private string insBranch;
    private string insTerm;
    private byte typeInstitute;//Institute Type : 0-College, 1-School

    public byte TypeInstitute
    {
        get { return typeInstitute; }
        set { typeInstitute = value; }
    }
    public string InsName
    {
        get { return insName; }
        set { insName = value; }
    }
    public string InsDegree
    {
        get { return insDegree; }
        set { insDegree = value; }
    }
    public string InsStream
    {
        get { return insStream; }
        set { insStream = value; }
    }
    public string InsBranch
    {
        get { return insBranch; }
        set { insBranch = value; }
    }
    public string InsTerm
    {
        get { return insTerm; }
        set { insTerm = value; }
    }
    public Institution(string userCode)
    {
        TypeInstitute = getInstituteType(userCode);
        SetInstituteValue(TypeInstitute);
    }
    /// <summary>
    /// Set Values respective to Institution
    /// </summary>
    /// <param name="insVal">Institution Type : 0-College, 1-School</param>
    private void SetInstituteValue(byte insVal)
    {
        TypeInstitute = insVal;
        switch (TypeInstitute)
        {
            case 0:
                InsName = "College";
                InsDegree = "Degree";
                InsStream = "Stream";
                InsBranch = "Branch";
                InsTerm = "Semester";
                break;
            case 1:
                InsName = "School";
                InsDegree = "School Type";
                InsStream = "Stream";
                InsBranch = "Standard";
                InsTerm = "Term";
                break;
        }
    }
    DAccess2 DA = new DAccess2();
    private byte getInstituteType(string userCode)
    {
        byte InsType = 0;
        string sqlschool = "select top 1 value from Master_Settings where settings='schoolorcollege' ";
        if (userCode.Trim() != string.Empty)
        {
            sqlschool += " and " + userCode + "";

            DataSet schoolds = DA.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables.Count > 0 && schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    InsType = 1;
                }
            }
        }
        return InsType;
    }
    public void SetControlText(ref Label lblControl, byte field)
    {
        switch (field)
        {
            case 0:
                lblControl.Text = InsName;
                break;
            case 1:
                lblControl.Text = InsStream;
                break;
            case 2:
                lblControl.Text = InsDegree;
                break;
            case 3:
                lblControl.Text = InsBranch;
                break;
            case 4:
                lblControl.Text = InsTerm;
                break;
        }
    }
}
public class HeaderLabelText
{
    public HeaderLabelText()
    { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="grouporusercode">usercode append query</param>
    /// <param name="lbl">Label Control To Change</param>
    /// <param name="fields">Field Type : 0- Name,1-Stream,2-Degree,3-Branch,4-Term</param>
    public void setLabels(string grouporusercode, ref List<Label> lbl, List<byte> fields)
    {
        Institution insObj = new Institution(grouporusercode);
        for (int lblCnt = 0; lblCnt < lbl.Count; lblCnt++)
        {
            Label lblSend = (Label)lbl[lblCnt];
            insObj.SetControlText(ref lblSend, fields[lblCnt]);
        }
    }
}
