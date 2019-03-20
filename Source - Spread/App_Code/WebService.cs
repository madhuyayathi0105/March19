using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Collections;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    [WebMethod]
    public string coundected_hour(long monyear, int startyear, string rollno, Hashtable hat, ArrayList not)
    {
        int absentcount = 0;
        int counthour = 0;
        string total_count = "";
        string collect_query = "";
        ArrayList aradd = new ArrayList();
        DataView dv = new DataView();
        collect_query = "select distinct * from attendance where roll_no='" + rollno + "' and month_year between '" + startyear + "'  and '" + monyear + "' order by month_year";
        ds.Clear();
        ds = d2.select_method_wo_parameter(collect_query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                aradd.Add(ds.Tables[0].Rows[i]["month_year"].ToString());
            }

            if (aradd.Count > 0)
            {
                for (int jr = 0; jr < aradd.Count; jr++)
                {
                    ds.Tables[0].DefaultView.RowFilter = "month_year='" + aradd[jr].ToString() + "'";
                    dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        for (int k = 2; k < dv.Table.Columns.Count; k++)
                        {
                            string attnd_value = dv[0][k].ToString();
                            if (attnd_value.Trim() != "" && attnd_value.Trim() != null && attnd_value.Trim() != "0")
                            {
                                if (hat.Contains(attnd_value) == true)
                                {
                                    absentcount++;
                                }
                                if (not.Contains(attnd_value) == false)
                                {
                                    counthour++;
                                }
                            }
                        }
                    }
                }
            }

            //semstart_date = ds.Tables[0].Rows[0]["start_date"].ToString();
            //semend_date = ds.Tables[0].Rows[0]["end_date"].ToString();
        }
        if (counthour != 0 || absentcount != 0)
        {
            total_count = Convert.ToString(counthour) + "-" + Convert.ToString(absentcount);
        }
        else
        {
            total_count = "Empty";
        }

        return total_count;
    }

    [WebMethod]

    public string condected_days(string start_date, string cur_date, string roll_no, Hashtable hat1, ArrayList not1, int max, string firsthalf, string secondhalf, Hashtable hat2, string min_perfrist, string min_persecond, Hashtable hat3)
    {
        double absentcount_days = 0;
        double totalcon_days = 0;
        string return_Value = "";

        //  string start_date1 = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
        string[] split_startdate = start_date.Split('/');
        string[] split_curdate = cur_date.Split('-');
        DateTime d1 = Convert.ToDateTime(split_startdate[1].ToString() + "-" + split_startdate[0].ToString() + "-" + split_startdate[2].ToString());
        DateTime d3 = Convert.ToDateTime(split_curdate[1].ToString() + "-" + split_curdate[0].ToString() + "-" + split_curdate[2].ToString());
        while (d1 <= d3)
        {
            string first_date = d1.ToString("dd/MM/yyyy");
            // string first_date = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
            int first_split_present = 0;
            int first_split_absent = 0;
            int notconsider = 0;
            int second_split_present = 0;
            int second_split_absent = 0;
            int firstempty_count = 0;
            int secondempty_count = 0;
            int split_day_hour = 0;
            string date = "";
            string month = "";
            string year = "";
            string month_year = "";
            string datecoulumn = "";
            if (hat3.ContainsKey(first_date) == false)
            {

                string[] split_first_date = first_date.Split('/');
                date = split_first_date[0].ToString();
                date = date.TrimStart('0');
                month = split_first_date[1].ToString();
                year = split_first_date[2].ToString();

                month_year = Convert.ToString((Convert.ToInt32(year) * 12 + Convert.ToInt32(month)));

                for (int h = 1; h <= max; h++)
                {
                    if (datecoulumn == "")
                    {
                        datecoulumn = "[d" + date + "d" + h + "]";
                    }
                    else
                    {
                        datecoulumn = datecoulumn + "," + "[d" + date + "d" + h + "]";
                    }
                }
                string absent_count_query = "Select " + datecoulumn + " from attendance where roll_no ='" + roll_no + "'and month_year in ('" + month_year + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(absent_count_query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                        split_day_hour++;
                        if (split_day_hour <= Convert.ToInt32(firsthalf))
                        {

                            if (hat2.Contains(attendvalue) == true)
                            {
                                first_split_present++;

                            }
                            else if (hat1.Contains(attendvalue) == true)
                            {
                                first_split_absent++;
                            }
                            else if (attendvalue.Trim() == "" || attendvalue.Trim() == null || attendvalue.Trim() == "0")
                            {
                                firstempty_count++;
                            }
                            else if (not1.Contains(attendvalue) == true)
                            {
                                notconsider++;
                            }

                        }
                        else
                        {

                            if (hat2.Contains(attendvalue) == true)
                            {
                                second_split_present++;
                            }
                            else if (hat1.Contains(attendvalue) == true)
                            {
                                second_split_absent++;
                            }
                            else if (attendvalue.Trim() == "" || attendvalue.Trim() == null || attendvalue.Trim() == "0")
                            {
                                secondempty_count++;
                            }
                            else if (not1.Contains(attendvalue) == true)
                            {
                                notconsider++;
                            }

                        }

                    }
                    if (firstempty_count < Convert.ToInt32(min_perfrist))
                    {
                        if (secondempty_count < Convert.ToInt32(min_persecond))
                        {

                            if (first_split_present < Convert.ToInt32(min_perfrist) && second_split_present < Convert.ToInt32(min_persecond))
                            {
                                if (first_split_absent != 0 && second_split_absent != 0)
                                {
                                    absentcount_days++;
                                    totalcon_days++;
                                }
                            }
                            else
                            {
                                totalcon_days++;
                            }
                        }

                    }
                }
            }
            else
            {
                string date1 = "";
                string holidayvalue = "";
                string moringvalue = "";
                string eveingvlaue = "";
                string hashvalue = Convert.ToString(hat3[first_date]);
                string[] splithash = hashvalue.Split('*');
                if (splithash.Length > 0)
                {
                    date1 = Convert.ToString(splithash[0]);
                    holidayvalue = Convert.ToString(splithash[1]);
                    moringvalue = Convert.ToString(splithash[2]);
                    eveingvlaue = Convert.ToString(splithash[3]);
                    if (holidayvalue == "True")
                    {
                        if (moringvalue == "True")
                        {
                            string[] split_first_date = first_date.Split('/');
                            date = split_first_date[0].ToString();
                            date = date.TrimStart('0');
                            month = split_first_date[1].ToString();
                            year = split_first_date[2].ToString();

                            for (int h = 1; h <= Convert.ToInt32(firsthalf); h++)
                            {
                                if (datecoulumn == "")
                                {
                                    datecoulumn = "[d" + date + "d" + h + "]";
                                }
                                else
                                {
                                    datecoulumn = datecoulumn + "," + "[d" + date + "d" + h + "]";
                                }
                            }
                            string absent_count_query = "Select " + datecoulumn + " from attendance where roll_no ='" + roll_no + "'and month_year in ('" + month_year + "')";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(absent_count_query, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                {
                                    string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);



                                    if (hat2.Contains(attendvalue) == true)
                                    {
                                        first_split_present++;

                                    }
                                    else if (hat1.Contains(attendvalue) == true)
                                    {
                                        first_split_absent++;
                                    }
                                    else if (attendvalue.Trim() == "" || attendvalue.Trim() == null || attendvalue.Trim() == "0")
                                    {
                                        firstempty_count++;
                                    }
                                    else if (not1.Contains(attendvalue) == true)
                                    {
                                        notconsider++;
                                    }


                                }
                                if (firstempty_count < Convert.ToInt32(min_perfrist))
                                {
                                    if (first_split_present < Convert.ToInt32(min_perfrist))
                                    {
                                        if (first_split_absent != 0)
                                        {
                                            absentcount_days = absentcount_days + 0.5;
                                            totalcon_days = totalcon_days + 0.5;
                                        }
                                    }
                                    else
                                    {
                                        totalcon_days = totalcon_days + 0.5;
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (eveingvlaue == "True")
                            {
                                string[] split_first_date = first_date.Split('/');
                                date = split_first_date[0].ToString();
                                date = date.TrimStart('0');
                                month = split_first_date[1].ToString();
                                year = split_first_date[2].ToString();
                                int Totalhour = Convert.ToInt32(secondhalf) + Convert.ToInt32(firsthalf);
                                for (int h = Convert.ToInt32(firsthalf) + 1; h <= Convert.ToInt32(Totalhour); h++)
                                {
                                    if (datecoulumn == "")
                                    {
                                        datecoulumn = "[d" + date + "d" + h + "]";
                                    }
                                    else
                                    {
                                        datecoulumn = datecoulumn + "," + "[d" + date + "d" + h + "]";
                                    }
                                }
                                string absent_count_query = "Select " + datecoulumn + " from attendance where roll_no ='" + roll_no + "'and month_year in ('" + month_year + "')";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(absent_count_query, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                    {
                                        string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);


                                        if (hat2.Contains(attendvalue) == true)
                                        {
                                            first_split_present++;

                                        }
                                        else if (hat1.Contains(attendvalue) == true)
                                        {
                                            first_split_absent++;
                                        }
                                        else if (attendvalue.Trim() == "" || attendvalue.Trim() == null || attendvalue.Trim() == "0")
                                        {
                                            firstempty_count++;
                                        }
                                        else if (not1.Contains(attendvalue) == true)
                                        {
                                            notconsider++;
                                        }


                                    }

                                    if (secondempty_count < Convert.ToInt32(min_persecond))
                                    {

                                        if (second_split_present < Convert.ToInt32(min_persecond))
                                        {
                                            if (second_split_absent != 0)
                                            {
                                                absentcount_days = absentcount_days + 0.5;
                                                totalcon_days = totalcon_days + 0.5;
                                            }
                                        }
                                        else
                                        {
                                            totalcon_days = totalcon_days + 0.5;
                                        }
                                    }

                                }
                            }
                        }

                    }

                }
            }
            d1 = d1.AddDays(1);
        }
        if (absentcount_days != 0 || totalcon_days != 0)
        {
            return_Value = Convert.ToString(totalcon_days) + "-" + Convert.ToString(absentcount_days);
        }
        else
        {
            return_Value = "Empty";
        }

        return return_Value;
    }

    [WebMethod]
    public List<string> Getname(string prefixText)
    {
        List<string> name = new List<string>();
        string query = prefixText;
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                name.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }
        return name;
    }

    [WebMethod]
    public Hashtable Getnamevalue(string prefixText)
    {
        Hashtable name = new Hashtable();
        string query = prefixText;
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                name.Add(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString());
            }
        }
        return name;
    }
    //Added by saranya on 27/03/2018 for name search in transfer,refund and studentpayment 
    [WebMethod]
    public Hashtable GetNameSearch(string prefixText)
    {
        Hashtable name = new Hashtable();
        string query = prefixText;
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (!name.ContainsKey(ds.Tables[0].Rows[i][0].ToString()))
                    name.Add(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][1].ToString());
                else
                {
                    string regno = string.Empty;
                    regno = Convert.ToString(name[ds.Tables[0].Rows[i][1].ToString()]);
                    name.Remove(ds.Tables[0].Rows[i][0].ToString());
                    name.Add(ds.Tables[0].Rows[i][0].ToString(), Convert.ToString(regno));
                }
            }
        }
        return name;
    }
}
