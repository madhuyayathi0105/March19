using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InsproDataAccess;
using System.Data;
using System.IO;
using System.Collections;

/// <summary>
/// Summary description for GetStudentData
/// </summary>
public class GetStudentData
{
    InsproDirectAccess dirAcc=new InsproDirectAccess();
    DAccess2 da = new DAccess2();
    //public GetStudentData()
    //{
    public DataTable getStudentData(string colCode, string batch, string degcode, string cursem, string sec, string subno, string staffcode, string elect, string lab, string labDay, string labhr, string ttname, string isAlter, string curDate)
    {
        DataTable dtStud = new DataTable();
        try
        {
            DataSet dtnewstu = new DataSet();
            string qrySection = string.Empty;
            string qryStaffSection = string.Empty;
            string qrySection1 = string.Empty;
            string qryStaffSection1 = string.Empty;
            string[] sections = sec.Split(',');
            sec = string.Empty;
            foreach (string curSec in sections)
            {
                if (!string.IsNullOrEmpty(curSec.Trim()) && curSec.Trim().ToLower() != "all")
                {
                    if (sec == string.Empty)
                    {
                        sec = "'" + curSec + "'";
                    }
                    else
                    {
                        sec += ",'" + curSec + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(sec))
            {
                qrySection = " and LTRIM(RTRIM(isnull(r.Sections,''))) in (" + sec + ")";
                qryStaffSection = " and LTRIM(RTRIM(isnull(ss.Sections,''))) in (" + sec + ")";
                qrySection1 = " and LTRIM(RTRIM(isnull(r.Sections,''''))) in (" + sec + ")";
                qryStaffSection1 = " and LTRIM(RTRIM(isnull(ss.Sections,''''))) in (" + sec + ")";
            }
            string orderBy = orderByStudents();
            bool staffSelector = false;
            string qryStudeStaffSelector = string.Empty;  //colCode
            string qryStudeStaffSelector1 = string.Empty; 
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + colCode + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batch.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            if (staffSelector)
            {
                qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
                qryStudeStaffSelector1 = " and sc.staffcode=''" + staffcode + "''";
            }
          
            
            Hashtable hat=new Hashtable();
            hat.Add("subno", subno);
            hat.Add("staffcode", staffcode);
            hat.Add("colCode", colCode );
            hat.Add("batch",batch);
            hat.Add("degcode",degcode);
            hat.Add("cursem",cursem);
            hat.Add("curDate",curDate);
            hat.Add("qrySection",qrySection1);
            hat.Add("qryStaffSection",qryStaffSection1);
            hat.Add("qryStudeStaffSelector",qryStudeStaffSelector1);
            hat.Add("orderBy",orderBy);
         
            dtnewstu = da.select_method("getStudent1",hat,"sp");

            if (elect == "true")
            {
                dtnewstu.Clear();
                dtnewstu = da.select_method("getStudent2", hat, "sp");
            }
            else if (lab == "true")
            {
                Hashtable hatlab = new Hashtable();
                hatlab.Add("subno", subno);
                hatlab.Add("staffcode", staffcode);
                hatlab.Add("colCode", colCode);
                hatlab.Add("batch", batch);
                hatlab.Add("degcode", degcode);
                hatlab.Add("cursem", cursem);
                hatlab.Add("curDate", curDate);
                hatlab.Add("labDay", labDay);
                hatlab.Add("labhr", labhr);
                hatlab.Add("ttname", ttname);
                hatlab.Add("qrySection", qrySection1);
                hatlab.Add("qryStaffSection", qryStaffSection1);
                hatlab.Add("qryStudeStaffSelector", qryStudeStaffSelector1);
                hatlab.Add("orderBy", orderBy);

                dtnewstu.Clear();
              
                if (isAlter.ToLower().Trim() == "false")
                {
                    dtnewstu = da.select_method("getStudent3", hatlab, "sp");
                }
                else
                {
                    dtnewstu = da.select_method("getStudent4", hatlab, "sp");
                }

            }

            dtStud = dtnewstu.Tables[0].DefaultView.ToTable();
            //dtStud = dirAcc.selectDataTable(selQ);
        }
        catch { dtStud.Clear(); }
        return dtStud;
    }


      private string orderByStudents()
    {
        string orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
        orderBySetting = orderBySetting.Trim();
        string orderBy = "ORDER BY r.roll_no";
        switch (orderBySetting)
        {
            case "0":
                orderBy = "ORDER BY r.roll_no";
                break;
            case "1":
                orderBy = "ORDER BY r.Reg_No";
                break;
            case "2":
                orderBy = "ORDER BY r.Stud_Name";
                break;
            case "0,1,2":
                orderBy = "ORDER BY r.roll_no,r.Reg_No,r.stud_name";
                break;
            case "0,1":
                orderBy = "ORDER BY r.roll_no,r.Reg_No";
                break;
            case "1,2":
                orderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                break;
            case "0,2":
                orderBy = "ORDER BY r.roll_no,r.Stud_Name";
                break;
            default:
                orderBy = "ORDER BY r.roll_no";
                break;
        }
        return orderBy;
    }

      public DataTable StudentData(string staffcode, string curDate)
      {
          DataTable dtStud = new DataTable();
          try
          {
              DataSet dtnewstu = new DataSet();
              string orderBy = orderByStudents();
              Hashtable hat = new Hashtable();
              hat.Add("staffcode", staffcode);
              hat.Add("curDate", curDate);
              hat.Add("orderBy", orderBy);
              dtnewstu = da.select_method("getStudentNew", hat, "sp");
              dtStud = dtnewstu.Tables[0].DefaultView.ToTable();
              //dtStud = dirAcc.selectDataTable(selQ);
          }
          catch { dtStud.Clear(); }
          return dtStud;
      }
	
}