using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class TimeTableStaffCodeUpdate : System.Web.UI.Page
{
    string upqury = "";
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    string college_code = string.Empty;
    string collegeCode = string.Empty;
    string userCollegeCode = string.Empty;
    string userCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string collegecode1 = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
     {
         try
         {

             if (Session["collegecode"] == null)
             {
                 Response.Redirect("~/Default.aspx");
             }
             else
             {
                 userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                 userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                 singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                 groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
                   collegecode1 =Convert.ToString(Session["collegecode"].ToString());
             }
         }
         catch (Exception ex)
         {
             
             d2.sendErrorMail(ex, collegecode1, "TimeTableStaffCodeUpdate");
         }
     }
    protected void btn_update_click(object sender, EventArgs e)
    {
        try
        {
            string oldcode = txtoldstaffcode.Text;
            string newcode = txtnewstaffcode.Text;
            string ins = string.Empty;
            upqury = "if not exists(select distinct TT_staffcode from TT_ClassTimetableDet where TT_staffcode='" + newcode +"')";
            upqury +=  "update TT_ClassTimetableDet set TT_staffcode='" + newcode + "' where TT_staffcode='" + oldcode + "'";
            ins = "if not exists(select distinct TT_staffcode from TT_AlterTimetableDet where TT_staffcode='" + newcode + "')";
            ins += "update TT_AlterTimetableDet set TT_staffcode='" + newcode + "' where TT_staffcode='" + oldcode + "'";//delsi1903

            int cun = d2.update_method_wo_parameter(upqury, "text");
            int count = d2.update_method_wo_parameter(ins, "text");
            if (cun > 0 && count>0)
            {
                lbl_error.Text = "Update Sucessfully";
            }
            else
            {
                lbl_error.Text = "Update faild";
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "TimeTableStaffCodeUpdate");
        }
    }
}