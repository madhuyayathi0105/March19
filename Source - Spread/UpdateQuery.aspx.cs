using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UpdateQuery : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        ds.Clear();
        bool check = false;
        string Query = "SELECT  roll_no, month_year, COUNT(*) as Total FROM  attendance  GROUP BY    roll_no, month_year HAVING  COUNT(*) > 1 ";
        ds = d2.select_method_wo_parameter(Query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 1)
        {
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                string Inserquery = " insert into insproplus..attendance select * from attendance where roll_no='" + Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]) + "' and month_year='" + Convert.ToString(ds.Tables[0].Rows[row]["month_year"]) + "'";
                int i = d2.update_method_wo_parameter(Inserquery, "Text");
                if (i > 0)
                {
                    check = true;
                }
            }
        }

    }
}