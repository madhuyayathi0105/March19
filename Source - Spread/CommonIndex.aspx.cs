using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CommonIndex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string redirectval = Request.QueryString["Name"];
            if (!string.IsNullOrEmpty(redirectval))
            {
                redirectMethod(redirectval);
            }
            else
            {
                Response.Redirect("Default_LoginPage.aspx");
            }
        }
    }

    protected void redirectMethod(string redirectval)
    {
        try
        {

            string[] splname = redirectval.Split('$');
            if (splname.Length > 0)
            {
                if (Convert.ToString(splname[0]) == "SU")
                    Session["Single_User"] = "";
                else
                    Session["Single_User"] = Convert.ToString(splname[0]);

                if (Convert.ToString(splname[1]) == "GC")
                    Session["group_code"] = "";
                else
                    Session["group_code"] = Convert.ToString(splname[1]);

                if (Convert.ToString(splname[2]) == "UC")
                    Session["UserCode"] = "";
                else
                    Session["UserCode"] = Convert.ToString(splname[2]);

                if (Convert.ToString(splname[3]) == "UN")
                    Session["UserName"] = "";
                else
                    Session["UserName"] = Convert.ToString(splname[3]);

                if (Convert.ToString(splname[4]) == "SC")
                    Session["Staff_Code"] = "";
                else
                    Session["Staff_Code"] = Convert.ToString(splname[4]);

                if (Convert.ToString(splname[5]) == "IL")
                    Session["IsLogin"] = "";
                else
                    Session["IsLogin"] = Convert.ToString(splname[5]);

                if (Convert.ToString(splname[6]) == "CC")
                    Session["current_college_code"] = "";
                else
                    Session["current_college_code"] = Convert.ToString(splname[6]);

                if (Convert.ToString(splname[7]) == "IC")
                    Session["InternalCollegeCode"] = "";
                else
                    Session["InternalCollegeCode"] = Convert.ToString(splname[7]);

                string module = Convert.ToString(splname[8]);


                if (module.Trim().ToLower() == "student")
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                else if (module.Trim().ToLower() == "inventory")
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                else if (module.Trim().ToLower() == "hostel")
                    Response.Redirect("~/HostelMod/hostelindex.aspx");
                else if (module.Trim().ToLower() == "feedback")
                    Response.Redirect("~/FeedBackMOD/Feedbackhome.aspx");
                else if (module.Trim().ToLower() == "request")
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                else if (module.Trim().ToLower() == "office")
                    Response.Redirect("~/OfficeMOD/OfficeHome.aspx");

                //
                else if (module.Trim().ToLower() == "attendance")
                    Response.Redirect("~/AttendanceMOD/AttendanceHome.aspx");
                else if (module.Trim().ToLower() == "cam")
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                else if (module.Trim().ToLower() == "schedule")
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                if (module.Trim().ToLower() == "finance")
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                else if (module.Trim().ToLower() == "hr")
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                //else if (module.Trim().ToLower() == "reports")
                //    Response.Redirect("HRMenuIndex.aspx");
                else if (module.Trim().ToLower() == "blackbox")
                    Response.Redirect("~/BlackBoxMod/BlackboxHome.aspx");
                else if (module.Trim().ToLower() == "sms")
                    Response.Redirect("~/SMSMOD/sms.aspx");
                else if (module.Trim().ToLower() == "transport")
                    Response.Redirect("~/TransportMod/TransportIndex.aspx");
                else if (module.Trim().ToLower() == "chart")
                    Response.Redirect("~/ChartMOD/ChartMaster.aspx");
                else if (module.Trim().ToLower() == "allotment")
                    Response.Redirect("Allotment.aspx");
                else if (module.Trim().ToLower() == "school")
                    Response.Redirect("SchoolHome.aspx");

                else if (module.Trim().ToLower() == "coe")
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                else if (module.Trim().ToLower() == "question")
                    Response.Redirect("~/QuestionMOD/QuestionsMasterHome.aspx");
                else if (module.Trim().ToLower() == "admission")
                    Response.Redirect("~/AdmissionMod/AdmissionHome.aspx");
                else if (module.Trim().ToLower() == "library")//Added By Saranyadevi 13.3.2018
                    Response.Redirect("~/LibraryMod/LibraryHome.aspx");
            }

        }
        catch { }
    }
}