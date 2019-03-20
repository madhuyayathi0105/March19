using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class map : System.Web.UI.Page
{
    public string connectionstring;
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
    }
   
    // This method is used to convert datatable to json string
    public string ConvertDataTabletoString()
  {
        DataTable dt = new DataTable();
        connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = connectionstring;
           // using (SqlCommand cmd = new SqlCommand("select VehicleID,Speed,title=GoogleLocation,lat=Latitude,lng=longitude,GoogleLocation,COUNT(*) as noofstud from VTSGPRSData V,Registration R WHERE V.VehicleID = R.VehID group by v.VehicleID,GPSFixstatus,Speed,GoogleLocation,Latitude,longitude", con))
            using (SqlCommand cmd = new SqlCommand("select VehicleID,Speed,title=GoogleLocation,lat=Latitude,lng=longitude,GoogleLocation,COUNT(*) as noofstud from VTSGPRSData V,Registration R WHERE V.VehicleID = R.VehID  group by v.VehicleID,GPSFixstatus,Speed,GoogleLocation,Latitude,longitude union all  select '' as VehicleID,Route_ID as Speed,address as title,''lat,''lng,address as GoogleLocation,'' as noofstud from RouteMaster where Veh_ID='TN 123' and sess='M'", con))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);



                if (dt.Rows.Count > 0)
                {
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);
                }
                else
                {
                     errmsg.Text = "No Records Found";
                    return "0";
                 
                }
            }
        }
    }
   
}