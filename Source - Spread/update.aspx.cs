using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HostelMod_update : System.Web.UI.Page
{
    string q1 = "";
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btn_update_click(object sender, EventArgs e)
    {
        try
        {
            q1 = " select count(app_no),app_no  from registration group by app_no having count(app_no)>1";
            ds.Clear(); int rowaffected = 0;
            ds = d2.select_method_wo_parameter(q1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dup in ds.Tables[0].Rows)
                {
                    q1 = "  select degree_code,batch_year,app_no,app_formno from applyn where app_formno =(select app_formno from applyn where app_no ='" + Convert.ToString(dup["app_no"]) + "')";
                    q1 += " select degree_code,batch_year,app_no,IdNEw from registration  ";
                    ds1.Clear();
                    ds1 = d2.select_method_wo_parameter(q1, "text");

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds1.Tables[0].Rows)
                        {
                            DataView dv = new DataView();
                            ds1.Tables[1].DefaultView.RowFilter = " app_no='" + Convert.ToString(dup["app_no"]) + "' and degree_code ='" + Convert.ToString(dr["degree_code"]) + "' and batch_year='" + Convert.ToString(dr["batch_year"]) + "'";
                            dv = ds1.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                foreach (DataRowView drv in dv)
                                {
                                    q1 = "update registration set app_no='" + Convert.ToString(dr["app_no"]) + "' where degree_code ='" + Convert.ToString(dr["degree_code"]) + "' and batch_year='" + Convert.ToString(dr["batch_year"]) + "' and app_no='" + Convert.ToString(dup["app_no"]) + "' and IdNEw='" + Convert.ToString(drv["IdNEw"]) + "'";

                                    rowaffected += d2.update_method_wo_parameter(q1, "text");
                                }
                            }
                        }
                    }
                }
            }
            lbl_error.Text = Convert.ToString("No of Rows Affected (" + rowaffected + ")");
            lbl_error.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            lbl_error.Text = Convert.ToString(ex);
            lbl_error.ForeColor = System.Drawing.Color.Red;
        }
    }
}